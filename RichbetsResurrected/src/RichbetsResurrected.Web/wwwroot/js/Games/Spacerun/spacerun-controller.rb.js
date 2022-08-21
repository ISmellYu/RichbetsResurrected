let crashConn = new signalR.HubConnectionBuilder().withUrl("/crashHub").build();

crashConn.start().then(function () {
    var streamData;
    var playerInit = true;
    var oldCrashed;
    var oldAllowPlacingBets;
    var _total;
    var playersOld;
    var onlinePlayersOld;

    var playerData = {
        isBetting: false,
        amount: 0,
        cashouted: false,
        whenCashouted: 0,
        autoCashout: false,
        whenAutoCashout: 0,
        autoBetting: false,
        autoBetsTotal: 0
    };

    var playersStyling = new Map();

    var manualButton = document.getElementById("manual-place-bet");
    var autoButton = document.getElementById("auto-place-bet");

    $("#manual-place-bet").click(function () {
        placeBet(1000);
        if (playerData.isBetting) {
            cashout();
        }
    });

    $("#auto-place-bet").click(function () {
        if (playerData.autoBetting) {
            playerData.autoBetting = false;
            autoButton.textContent = "Start Autobet"
        }else{
            playerData.autoBetting = true;
            autoButton.textContent = "Autobet active"
            placeBet(100);
        }
    });

    crashConn.stream("StreamCrashInfo").subscribe({
        next: function (data) {
            streamData = data;

            // Render dev data
            document.getElementById("dev-info").innerHTML = "";
            (Object.entries(playerData)).forEach(element => {
                let para = document.createElement("p");
                para.innerHTML = element[0] + ": " + element[1];
                document.getElementById("dev-info").appendChild(para);
            });

            if (JSON.stringify(data.players) !== JSON.stringify(playersOld)) { // If the players list changed
                playersOld = data.players;

                renderPlayersList(data.players);
            }

            if (oldCrashed != data.crashed) {
                if (data.crashed == true) {
                    clearPlayerData();
                    renderPlayersLostList(data.players);
                }
                oldCrashed = data.crashed;
            }

            if (JSON.stringify(data.onlinePlayers) !== JSON.stringify(onlinePlayersOld)) {

                onlinePlayersOld = data.onlinePlayers;

                renderOnlinePlayers(data.onlinePlayers);
            }

            if (oldAllowPlacingBets != data.allowPlacingBets) {
                if (data.allowPlacingBets == true) {
                    if (playerData.autoBetting && playerData.autoBetsTotal > 0) {
                        placeBet(100);
                        document.getElementById("auto_total-bets").value -= 1;
                    }
                    if (playerData.autoBetsTotal == 0) {
                        autoButton.textContent = "Start Autobet";
                        playerData.autoBetting = false;
                    }
                }
                oldAllowPlacingBets = data.allowPlacingBets;
            }
            
            if (playerInit == true) {
                updatePlayerData();
                playerInit = false;
            }

            let manual_cashout = document.getElementById("manual_cashout").value;
            let auto_cashout = document.getElementById("auto_cashout").value;

            if (manual_cashout) {
                playerData.autoCashout = true;
                playerData.whenAutoCashout = manual_cashout
            }

            if (auto_cashout) {
                playerData.autoCashout = true;
                playerData.whenAutoCashout = auto_cashout
            }

            if (!manual_cashout && !auto_cashout) {
                playerData.autoCashout = false;
                playerData.whenAutoCashout = 0;
            }

            if (playerData.isBetting && playerData.autoCashout) {
                if (data.multiplier >= playerData.whenAutoCashout) {
                    cashout(parseFloat(playerData.whenAutoCashout));
                }
            }

            if (playerData.autoBetting) {
                playerData.autoBetsTotal = document.getElementById("auto_total-bets").value;
            }

            if (playerData.isBetting) {
                if (streamData.gameStarted) {
                    manualButton.textContent = "Cashout +" + (playerData.amount * streamData.multiplier).toFixed(0);
                }else{
                    manualButton.textContent = "Place Bet (" + playerData.amount  +")";
                }
            }else{
                if (playerData.whenCashouted) {
                    manualButton.textContent = "Cashouted";
                }else{
                    manualButton.textContent = "Place Bet";
                }
                if (streamData.gameStarted) {
                    lockBettingButtons(true);
                }else{
                    lockBettingButtons(false);
                }
            }
        }
    });

    async function placeBet(amount) {
        let result = await crashConn.invoke("JoinCrash", amount).catch(function (err) {
            new Snack(
                {
                    message: `Error: ${err.toString()}`,
                    duration: 3000,
                    type: `error`
                }
            );
            return console.error(err.toString());
        });
        let verb = '';
        if (amount > 1) {
            verb = "'s"
        }
        if (result.isSuccess == true) {
            playerData.isBetting = true;
            playerData.amount += amount;
            new Snack(
                {
                    message: `${amount} RBC${verb} successfully placed.`,
                    duration: 3000,
                }
            );
        }
    }

    async function cashout(multiplier) {
        if (!multiplier) {
            multiplier = null;
        }
        let result = await crashConn.invoke("Cashout", multiplier).catch(function (err) {
            new Snack(
                {
                    message: `Error: ${err.toString()}`,
                    duration: 3000,
                    type: `error`
                }
            );
            return console.error(err.toString());
        });
        let verb = '';
        if (playerData.amount > 1) {
            verb = "'s"
        }
        if (result.isSuccess == true) {
            updatePlayerData();
            new Snack(
                {
                    message: `Successfully paid out.`,
                    duration: 3000,
                }
            );
        }
    }

    async function getUserData() {

        let result = await connection.invoke("GetClientInfo").catch(function (err) {
            return console.error(err.toString());
    
        });
        return result;
    }

    function lockBettingButtons(status){
        if (status == true) {
            document.getElementById("manual-place-bet").classList.add("bet-button-disabled"); 
            document.getElementById("auto-place-bet").classList.add("bet-button-disabled");
        }else{
            document.getElementById("manual-place-bet").classList.remove("bet-button-disabled"); 
            document.getElementById("auto-place-bet").classList.remove("bet-button-disabled");
        }
    }

    function updatePlayerData() {
        getUserData().then(function (data) {
            (streamData.players).forEach(element => {
                if (element.discordUserId == data.discordUserId) {
                    if (element.cashouted) {
                        playerData.isBetting = false;
                        playerData.amount = 0
                    }else{
                        playerData.isBetting = true;
                        playerData.amount = element.amount;
                    }
                    playerData.cashouted = element.cashouted;
                    playerData.whenCashouted = element.whenCashouted;
                }
            });
        });
    }

    function clearPlayerData() {
        playerData.cashouted = false;
        playerData.whenCashouted = 0;
        playerData.amount = 0;
        playerData.isBetting = false;
    }

    function renderPlayersList(data) {
        let _total = 0;

        clearPlayersList();

        data.forEach(element => {
            let cashout;
            let cashoutAmount;
            let tr = document.createElement("tr");
            
            if (element.whenCashouted == 0) {
                cashout = `<td>Pending..</td>`;
                cashoutAmount = `<td>${element.amount}</td>`;
            }else{
                cashout = `<td style="color: #00C74D">${element.whenCashouted}x</td>`; 
                cashoutAmount = `<td style="color: #00C74D">+${Math.trunc(element.amount * element.whenCashouted)}</td>`; 
            }

            console.log(playersStyling.has(element.discordUserId));

            if (playersStyling.has(element.discordUserId)) {
                tr.innerHTML = `<td style="background-image: url(${element.avatarUrl});"><p class="${playersStyling.get(element.discordUserId)}">${element.userName}</p></td>${cashout}${cashoutAmount}`;
            }else{
                tr.innerHTML = `<td style="background-image: url(${element.avatarUrl});">${element.userName}</td>${cashout}${cashoutAmount}`;
            }

            $(".players-table").append(tr);
            _total += element.amount;
        });

        $("#player-count").text(data.length + " Players");
        $("#bets-total").text(_total + " RBC");
    }

    function renderPlayersLostList(data) {
        let _total = 0;

        clearPlayersList();

        data.forEach(element => {
            let cashout;
            let cashoutAmount;
            
            if (element.whenCashouted == 0) {
                cashout = `<td style="color: #EE5353"></td>`; 
                cashoutAmount = `<td style="color: #EE5353">-${element.whenCashouted}</td>`; 
            }else{
                cashout = `<td style="color: #00C74D">${element.whenCashouted}x</td>`; 
                cashoutAmount = `<td style="color: #00C74D">+${Math.trunc(element.amount * element.whenCashouted)}</td>`; 
            }

            let tr = document.createElement("tr");
            tr.innerHTML = `<td style="background-image: url(${element.avatarUrl});">${element.userName}</td>${cashout}${cashoutAmount}`;
            $(".players-table").append(tr);
            _total += element.amount;
        });

        $("#player-count").text(data.length + " Players");
        $("#bets-total").text(_total + " RBC");
    }

    function clearPlayersList() {
        $(".players-table").empty();
    }

    function renderOnlinePlayers(data) {
        let list = document.querySelector('.onlinePlayersList');

        while (list.firstChild) {
            list.removeChild(list.firstChild);
        }

        data.forEach(player => {
            console.log(playersStyling);
            console.log(player);

            let memberElement = document.createElement("li");
            let paragraph = document.createElement("p");

            (player.equippedItems).forEach(element => {
                if (element.itemType.isNicknameAnimation || element.itemType.isNicknameBanner || element.itemType.isNicknameEffect || element.itemType.isNicknamePattern) {
                    paragraph.classList.add(element.description);
                    if (!playersStyling.has(player.discordUserId)) {
                        playersStyling.set(player.discordUserId, element.description);
                    }
                }
            });

            paragraph.textContent = player.userName;
            paragraph.style.fontSize = "15px";

            memberElement.appendChild(paragraph);
            memberElement.style.backgroundImage = `url(${player.avatarUrl})`;
            memberElement.classList.add('onlinePlayersElement');

            list.appendChild(memberElement);
        });
    }
});