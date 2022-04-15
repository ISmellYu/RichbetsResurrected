let conn = new signalR.HubConnectionBuilder().withUrl("/rouletteHub").build(); // Create a new connection to the hub.

let firstImg = new Image(); // Create a new image object.

// Render the wheel image.
firstImg.onload = function () {
    firstWheel.wheelImage = firstImg; 
    firstWheel.draw(); 
}

// Set the image source, and wheel size.
firstImg.src = "/img/roulette-body.png"; 
firstImg.height = 500;
firstImg.width = 500;

let wheelPower = 3; // Used to hold current value of power. 1-3
let wheelSpinning = false; // Used to determine if the wheel is currently spinning.
let allowBetting = false; // Used to determine if betting is allowed.


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

// Spin the wheel.
function startSpin(stopAt) {

    allowBetting = false; // Set flag so that betting is not allowed.

    if (wheelSpinning == false) { 

        switch(wheelPower){
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

        firstWheel.animation.stopAngle = stopAt; // Set the stop angle to our passed in value.

        firstWheel.startAnimation(); // Start the animation.
    }
}

// Function to call when the spin animation has finished.
function restoreWheel() {

    firstWheel.stopAnimation(false);  // Stop the animation

    firstWheel.rotationAngle = firstWheel.rotationAngle % 360;
}


// Roulette controller. 


// Start the connection.
conn.start().then(function () {

    let timerText = document.querySelector('.timer-text'); // Get the timer text element.


    // Listen to the roulette data stream from the server.
    conn.stream("StreamRouletteInfo").subscribe({
        next: function (data) {
            
            // data: {players: Array(0), results: Array(10), allowBetting: true, isRolling: false, timeLeft: 12.76}

            let actualProgress = (data.timeLeft * 100) / 15; // Calculate the actual progress. 0-100

            let timeText = data.timeLeft.toFixed(1); // Split output data into one decimal place.

            if (actualProgress < 98) { // Prevent the timer from visual bug.

                setProgress(actualProgress); // Set the progress bar.
                timerText.textContent = `${timeText}s`; // Set the timer text.
            }

            const colors = ['black', 'red', 'green'];

            colors.forEach(color => {

                let res = document.querySelector(`.members-${color}`).childElementCount;

                data.players.forEach(player => {
                    
                    switch (color) { // Switch the color.
                        case "red":
                            color = 0;
                            break;
                        case "black":
                            color = 1;
                            break;
                        case "green":
                            color = 2;
                            break;
                    }

                    if (player.color == color) { // If the player is in the color.
                        console.log(player.color);
                    }
                });
            });
        }
    });

    // Listen for end of game.
    conn.on("EndRoulette", function (history, current) {

        console.log(current.colorName);
        console.log(current.winners);
        resetTimer();
    });

    // Listen for start of game.
    conn.on("StartAnimation", function (data) {

        startSpin(data);
    });

    // 0 - red 1 - black 2 - green  // Color notes

    document.getElementById("black-button").addEventListener("click", async function () { // Add click event listener to the black button.

        placeBet(1, 1); // Place a bet.
    });

    document.getElementById("red-button").addEventListener("click", async function () { // Add click event listener to the red button.

        placeBet(0, 1); // Place a bet.
    });

    document.getElementById("green-button").addEventListener("click", async function () { // Add click event listener to the green button.

        placeBet(2, 1); // Place a bet.
    });

    // Function to place a bet.
    async function placeBet(color, amount) {

        let result = await conn.invoke("JoinRoulette", amount, color).catch(function (err) { // Invoke the JoinRoulette method.

            return console.error(err.toString()); // Log the error.
        });

        if (result.isSuccess == true) { // If the result is successful.

            console.log(`Bet placed on ${color}`); // Log the bet.
        }
    }

    // conn.on("PlayerJoined", function (data) {
    //     addMemberToList(data.color, data.userName, data.amount, data.avatarUrl, data.discordUserId)
    //     console.log(data);
    // });

    function addMemberToList(color, username, amount, avatarUrl, id) {

        switch (color) { // Switch the color.
            case 0:
                color = "red";
                break;
            case 1:
                color = "black";
                break;
            case 2:
                color = "green";
                break;
        }

        let membersUl = document.querySelector(`.members-${color}`); // Get the members list.
        let coinsUl = document.querySelector(`.coins-${color}`); // Get the members list.

        let [newMemberLi, newCoinsLi] = [document.createElement("li"), document.createElement("li")]; // Create a new list items.

        newMemberLi.textContent = username; // Set the text content.
        newCoinsLi.textContent = amount; // Set the text content.

        newMemberLi.style.backgroundImage = `url(${avatarUrl})`; // Set the background image.

        newCoinsLi.dataset.id = id; // Set the data attribute.

        membersUl.appendChild(newMemberLi); // Append the new list item to the members list.
        coinsUl.appendChild(newCoinsLi); // Append the new list item to the members list.

    }
});