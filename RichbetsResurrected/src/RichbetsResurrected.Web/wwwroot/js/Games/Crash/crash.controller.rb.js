let crashConn = new signalR.HubConnectionBuilder().withUrl('/crashHub').build();

const CONSOLE_DEBUG_MODE = true;
const VISUAL_DEBUG_MODE = true;

let gradientStroke;
let gradientStrokeLose;
const config = {
    type: 'line',
    data: {
        labels: [0],
        datasets: [
            {
                borderDash: [10],
                borderWidth: [4],
                pointBackgroundColor: 'rgba(0,	0,	0,	0)',
                pointColor: 'rgba(0,	0,	0,	0)',
                pointRadius: 0,
                pointBorderWidth: 0,
                borderColor: 'rgb(94,	183,	110)',
                backgroundColor: gradientStroke,
                data: [0],
            },
        ],
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        tooltips: {
            enabled: false,
        },
        legend: {
            display: false,
        },
        scales: {
            xAxes: [
                {
                    display: false,
                    scaleLabel: {
                        display: true,
                    },
                    gridLines: {
                        display: false,
                    },
                    ticks: {
                        min: 1,
                        stepSize: 1,
                        display: false,
                    },
                },
            ],
            yAxes: [
                {
                    display: true,
                    scaleLabel: {
                        display: true,
                    },
                    ticks: {
                        beginAtZero: true,
                        min: 1,
                        max: 2,
                    },
                },
            ],
        },
    },
}; //	Config	for	chart

var streamDataTemp = {
    crashed: false,
    players: {},
    onlinePlayers: {},
    running: false,
    allowPlacingBets: false,
};

var playerData = {
    isBetting: false,
    amount: 0,
    cashouted: false,
    whenCashouted: 0,
    autoCashout: false,
    whenAutoCashout: 0,
    autoBetting: false,
    autoBetsTotal: 0,
};

var sessionData = {
    betting: false,
    amount: 0,
};

var playersStyling = new Map();

$(document).ready(async function () {
    crashConn.start().then(function () {
        showUserData();
        let ctx = $('#myChart')[0].getContext('2d'); //	Get	context	table

        gradientStroke = ctx.createLinearGradient(0, 0, 800, 500); //	Linear	gradient
        gradientStroke.addColorStop(0, 'rgb(94,	183,	110,	.4)');
        gradientStroke.addColorStop(1, 'rgb(94,	183,	110,	0)');

        gradientStrokeLose = ctx.createLinearGradient(0, 0, 800, 500); //	Linear	gradient
        gradientStrokeLose.addColorStop(0, 'rgb(252,	25,	28,	.4)');
        gradientStrokeLose.addColorStop(1, 'rgb(234,	47,	43,	0)');

        let myChart = new Chart(ctx, config);

        function updateChart(labels, data) {
            //	Update	chart(	CAUTION!	U	MUST	PROVIDE	EXACTLY	THE	SAME	NUMBERS	OF	TABLES	AS	DATA)
            myChart.data.labels = labels;
            myChart.data.datasets[0].data = data;
            myChart.options.scales.yAxes[0].ticks.max = Math.max.apply(2, data) + 1;
            myChart.update();
            sendVisualPing('updatechart');
        }

        function restoreChart() {
            //	Restoring	chart	to	its	original	state
            myChart.data.labels = [0];
            myChart.data.datasets[0].data = [0];
            myChart.options.scales.yAxes[0].ticks.max = 2;
            myChart.data.datasets[0].backgroundColor = gradientStroke;
            myChart.data.datasets[0].borderColor = 'rgb(94,	183,	110)';
            myChart.update();
            sendVisualPing('restore');
        }

        function crashChart() {
            //	Invoked	when	server	sent	crashmsg	to	client,	changes	line	to	color	red	and	background	color
            myChart.data.datasets[0].backgroundColor = gradientStrokeLose;
            myChart.data.datasets[0].borderColor = '#E1675A';
            myChart.update();
            sendVisualPing('crash');
        }

        $('#manual-place-bet').click(function () {
            let x = parseInt($('.amount-input').val());
            if (isNaN(x)){
                return;
            }
            placeBet(parseInt($('.amount-input').val()));
            if (sessionData.betting && streamDataTemp.running) {
                cashout();
            }
        });

        crashConn.stream('StreamCrashInfo').subscribe({
            next: function (data) {
                updatePlayerData(data);
                let labels = [];

                for (let i = 1; i < data.multipliers.length; i++) {
                    labels.push(i);
                }

                if (streamDataTemp.allowPlacingBets != data.allowPlacingBets) {
                    log('Allow changing bets changed');
                    $('#manual-place-bet').text('Place Bet');
                    streamDataTemp.allowPlacingBets = data.allowPlacingBets;
                }

                //Player behaviours controller
                if (!_.isEqual(data.players, streamDataTemp.players)) {
                    // Player list update
                    renderPlayersList(data.players);
                    log('Player list update');
                    sendVisualPing('players');
                    streamDataTemp.players = data.players;
                }

                if (!_.isEqual(data.onlinePlayers, streamDataTemp.onlinePlayers)) {
                    // Online players list changed
                    renderOnlinePlayers(data.onlinePlayers);
                    log('Online players list changed');
                    sendVisualPing('onlinePlayers');
                    streamDataTemp.onlinePlayers = data.onlinePlayers;
                }

                // Crash behaviours guards
                if (!_.isEqual(data.crashed, streamDataTemp.crashed)) {
                    streamDataTemp.crashed = data.crashed;
                    log('Crashed');
                    renderPlayersLostList(data.players);
                    $('#multiplierid').css('color', '#a74c5dd7');
                    $('#multiplierid').text(`Crashed at ${data.multiplier.toFixed(2)}x`);
                    sessionData.amount = 0;
                    sessionData.betting = false;
                    crashChart();
                }

                if (!_.isEqual(data.running, streamDataTemp.running)) {
                    log('Start Chart');
                    sendVisualPing('start');
                    streamDataTemp.running = data.running;
                }

                // Others
                if (data.timeLeft !== 0) {
                    restoreChart();
                    $('#multiplierid').text(`${data.timeLeft.toFixed(2)}s`);
                    $('#multiplierid').css('color', 'rgba(189, 189, 189, 0.4)');
                }

                if (data.gameStarted && !data.crashed) {
                    updateChart(labels, data.multipliers);
                    $('#multiplierid').text(`${data.multiplier.toFixed(2)}x`);
                    $('#multiplierid').css('color', 'rgba(189, 189, 189, 0.4)');
                }
            },
        });

        async function placeBet(amount) {
            let verb = '';
            log('Place bet attempt');
            let result = await crashConn.invoke('JoinCrash', amount).catch(function (err) {
                new Snack({
                    message: `Error: ${err.toString()}`,
                    duration: 3000,
                    type: `error`,
                });
                return console.error(err.toString());
            });

            if (result.isSuccess == true) {
                sessionData.betting = true;
                sessionData.amount += amount;
                log(`Placed ${amount}`);
                new Snack({
                    message: `${amount} RBC${verb} successfully placed.`,
                    duration: 3000,
                });
            }
        }

        async function cashout(multiplier) {
            let verb = '';
            if (!multiplier) multiplier = null;
            log('Cashout attempt');
            let result = await crashConn.invoke('Cashout', multiplier).catch(function (err) {
                new Snack({
                    message: `Error: ${err.toString()}`,
                    duration: 3000,
                    type: `error`,
                });
                return console.error(err.toString());
            });

            if (result.isSuccess == true) {
                sessionData.amount = 0;
                sessionData.betting = false;
                log(`Cashouted at ${multiplier}`);
                new Snack({
                    message: `Successfully paid out.`,
                    duration: 3000,
                });
            }
        }

        function renderPlayersList(data) {
            let _total = 0;
            clearPlayersList();

            data.forEach((element) => {
                let cashout;
                let cashoutAmount;
                let tr = document.createElement('tr');

                if (element.whenCashouted == 0) {
                    cashout = `<td>Pending..</td>`;
                    cashoutAmount = `<td>${element.amount}</td>`;
                } else {
                    cashout = `<td style="color: #00C74D">${element.whenCashouted}x</td>`;
                    cashoutAmount = `<td style="color: #00C74D">+${Math.trunc(
                        element.amount * element.whenCashouted
                    )}</td>`;
                }

                console.log(playersStyling.has(element.discordUserId));

                if (playersStyling.has(element.discordUserId)) {
                    tr.innerHTML = `<td style="background-image: url(${
                        element.avatarUrl
                    });"><p class="${playersStyling.get(element.discordUserId)}">${
                        element.userName
                    }</p></td>${cashout}${cashoutAmount}`;
                } else {
                    tr.innerHTML = `<td style="background-image: url(${element.avatarUrl});">${element.userName}</td>${cashout}${cashoutAmount}`;
                }

                $('.players-table').append(tr);
                _total += element.amount;
            });
            $('#player-count').text(data.length + ' Players');
            $('#bets-total').text(_total + ' RBC');
        }

        function renderPlayersLostList(data) {
            let _total = 0;
            clearPlayersList();

            data.forEach((element) => {
                let cashout;
                let cashoutAmount;

                if (element.whenCashouted == 0) {
                    cashout = `<td style="color: #EE5353"></td>`;
                    cashoutAmount = `<td style="color: #EE5353">-${element.whenCashouted}</td>`;
                } else {
                    cashout = `<td style="color: #00C74D">${element.whenCashouted}x</td>`;
                    cashoutAmount = `<td style="color: #00C74D">+${Math.trunc(
                        element.amount * element.whenCashouted
                    )}</td>`;
                }
                let tr = document.createElement('tr');
                tr.innerHTML = `<td style="background-image: url(${element.avatarUrl});">${element.userName}</td>${cashout}${cashoutAmount}`;
                $('.players-table').append(tr);
                _total += element.amount;
            });

            $('#player-count').text(data.length + ' Players');
            $('#bets-total').text(_total + ' RBC');
        }

        function clearPlayersList() {
            $('.players-table').empty();
        }

        function renderOnlinePlayers(data) {
            let list = document.querySelector('.onlinePlayersList');

            while (list.firstChild) {
                list.removeChild(list.firstChild);
            }

            data.forEach((player) => {
                console.log(playersStyling);
                console.log(player);

                let memberElement = document.createElement('li');
                let paragraph = document.createElement('p');

                player.equippedItems.forEach((element) => {
                    if (
                        element.itemType.isNicknameAnimation ||
                        element.itemType.isNicknameBanner ||
                        element.itemType.isNicknameEffect ||
                        element.itemType.isNicknamePattern
                    ) {
                        paragraph.classList.add(element.description);
                        if (!playersStyling.has(player.discordUserId)) {
                            playersStyling.set(player.discordUserId, element.description);
                        }
                    }
                });

                paragraph.textContent = player.userName;
                paragraph.style.fontSize = '15px';

                memberElement.appendChild(paragraph);
                memberElement.style.backgroundImage = `url(${player.avatarUrl})`;
                memberElement.classList.add('onlinePlayersElement');

                list.appendChild(memberElement);
            });
        }

        async function getUserData() {
            let result = await connection.invoke('GetClientInfo').catch(function (err) {
                return console.error(err.toString());
            });
            return result;
        }

        function updatePlayerData(data) {
            getUserData().then(function (userData) {
                data.players.forEach((element) => {
                    if (element.discordUserId == userData.discordUserId) {
                        if (element.cashouted) {
                            sessionData.betting = false;
                            sessionData.amount = 0;
                        } else {
                            sessionData.betting = true;
                            sessionData.amount = element.amount;
                        }
                    }
                });
            });
        }
    });
});

function log(message) {
    if (!CONSOLE_DEBUG_MODE) return;
    console.log(
        `%c[RB-DEBUG]%c ${message}`,
        'color: black; background:#F39C12; border-radius: 5px; padding: 1px 2px;',
        'color:white'
    );
}

function sendVisualPing(name) {
    if (!VISUAL_DEBUG_MODE) return;
    $(`.${name}`).children('.dev-node-ping').remove();
    $(`.${name}`).append('<span class="dev-node-ping"></span>');
}

function showUserData() {
    if (!VISUAL_DEBUG_MODE) return;
    setInterval(() => {
        console.log(sessionData.betting, sessionData.amount);
        $('.betting').text(sessionData.betting);
        $('.amount').text(sessionData.amount);
        sendVisualPing('userResult');
    }, 1000);
}
