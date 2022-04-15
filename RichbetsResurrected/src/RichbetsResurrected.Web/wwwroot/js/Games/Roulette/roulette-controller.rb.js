let conn = new signalR.HubConnectionBuilder().withUrl("/rouletteHub").build(); // Create a new connection to the hub.

let firstImg = new Image();

firstImg.onload = function () {
    firstWheel.wheelImage = firstImg;
    firstWheel.draw();
}


firstImg.src = "/img/roulette-body.png";
firstImg.height = 500;
firstImg.width = 500;


let wheelPower = 3; // scope 1-3
let wheelSpinning = false;
let allowBetting = false;


// Make wheel spinning without focus.
TweenMax.ticker.useRAF(false);
TweenMax.lagSmoothing(0);
jQuery.fx.off = true;


let firstWheel = new Winwheel({
    'responsive': true,
    'drawText': false,
    'drawMode': 'image', // drawMode must be set to image.
    'numSegments': 37, // The number of segments must be specified.
    'imageOverlay': false, // Set imageOverlay to true to display the overlay.
    'lineWidth': 2, // Overlay uses wheel line width and stroke style so can set these
    'strokeStyle': 'red', // as desired to change appearance of the overlay.
    'animation': {
        'type': 'spinToStop',
        'duration': 3,
        'spins': 8,
        // Remember to do something after the animation has finished specify callback function.
        'callbackFinished': restoreWheel,
    },
    'segments': [{
        'text': '32'
    },
        {
            'text': '15'
        },
        {
            'text': '19'
        },
        {
            'text': '4'
        },
        {
            'text': '21'
        },
        {
            'text': '2'
        },
        {
            'text': '25'
        },
        {
            'text': '17'
        },
        {
            'text': '34'
        },
        {
            'text': '6'
        },
        {
            'text': '27'
        },
        {
            'text': '13'
        },
        {
            'text': '36'
        },
        {
            'text': '11'
        },
        {
            'text': '30'
        },
        {
            'text': '8'
        },
        {
            'text': '23'
        },
        {
            'text': '10'
        },
        {
            'text': '5'
        },
        {
            'text': '24'
        },
        {
            'text': '16'
        },
        {
            'text': '33'
        },
        {
            'text': '1'
        },
        {
            'text': '20'
        },
        {
            'text': '14'
        },
        {
            'text': '31'
        },
        {
            'text': '9'
        },
        {
            'text': '22'
        },
        {
            'text': '18'
        },
        {
            'text': '29'
        },
        {
            'text': '7'
        },
        {
            'text': '28'
        },
        {
            'text': '12'
        },
        {
            'text': '35'
        },
        {
            'text': '3'
        },
        {
            'text': '26'
        },
        {
            'text': '0'
        },
    ]
});


function startSpin(stopAt) {

    allowBetting = false;

    if (wheelSpinning == false) {

        switch (wheelPower) {
            case 1:
                firstWheel.animation.spins = 3;
                break;
            case 2:
                firstWheel.animation.spins = 8;
                break;
            case 3:
                firstWheel.animation.spins = 15;
                break;
        }

        firstWheel.animation.stopAngle = stopAt;

        firstWheel.startAnimation();
    }
}


function restoreWheel() {

    firstWheel.stopAnimation(false);

    firstWheel.rotationAngle = firstWheel.rotationAngle % 360;
}


// Roulette controller. 


conn.start().then(function () {

    let timerText = document.querySelector('.timer-text');

    const colors = ['Red', 'Black', 'Green'];

    let PlayerBetHistory = [];

    let dataPlayersOld;


    // Listen to the roulette data stream from the server.
    conn.stream("StreamRouletteInfo").subscribe({
        next: function (data) {

            let actualProgress = (data.timeLeft * 100) / 15; // Actual progress. 0-100.

            let timeText = data.timeLeft.toFixed(1); // Split output data into one decimal place.

            if (actualProgress < 98) { // Prevent the timer from visual bug.

                if (actualProgress != 0) {
                    setProgress(actualProgress);
                    timerText.textContent = `${timeText}s`;
                }

            }

            if (JSON.stringify(data.players) !== JSON.stringify(dataPlayersOld)) {

                dataPlayersOld = data.players;

                renderPlayersList(data.players);
            }
        }
    });


    conn.on("EndRoulette", function (history, current) {

        resetTimer();

        if (PlayerBetHistory.includes(current.color)) { // Play correct sound if the player won.
            playSound("rollWin");
        } 
        else {
            playSound("rollEnd");
        }

        timerText.textContent = `${current.colorName}`;

        PlayerBetHistory = [];
    });

    conn.on("StartAnimation", function (data) {

        timerText.textContent = `Rolling...`;

        startSpin(data);
        playSound("rollStart");
    });


    // 0 - red 1 - black 2 - green  // Color notes

    document.getElementById("black-button").addEventListener("click", async function () {
        placeBet(1, 1);
        PlayerBetHistory.push(1);
    });

    document.getElementById("red-button").addEventListener("click", async function () {
        placeBet(0, 1);
        PlayerBetHistory.push(0);
    });

    document.getElementById("green-button").addEventListener("click", async function () {
        placeBet(2, 1);
        PlayerBetHistory.push(2);
    });


    async function placeBet(color, amount) {


        let result = await conn.invoke("JoinRoulette", amount, color).catch(function (err) {

            return console.error(err.toString());

        });


        if (result.isSuccess == true) {

            console.log(`Bet placed on ${color}`);

        }
    }


    function renderPlayersList(data) {

        clearPlayersList();

        data.forEach(player => { // Prepare list for each player.

            let memberList = document.querySelector(`.members-${player.colorName}`);
            let coinsList = document.querySelector(`.coins-${player.colorName}`);

            let memberElement = document.createElement("li");
            let coinsElement = document.createElement("li");

            memberElement.textContent = player.userName;
            coinsElement.textContent = player.amount;

            memberElement.style.backgroundImage = `url(${player.avatarUrl})`;

            memberList.appendChild(memberElement);
            coinsList.appendChild(coinsElement);

        });
    }


    function clearPlayersList() {

        colors.forEach(color => {

            let playersList = document.querySelector(`.members-${color}`);
            let coinsList = document.querySelector(`.coins-${color}`);

            while (playersList.firstChild) {

                playersList.removeChild(playersList.firstChild);
            }

            while (coinsList.firstChild) {

                coinsList.removeChild(coinsList.firstChild);
            }

        });
    }

});