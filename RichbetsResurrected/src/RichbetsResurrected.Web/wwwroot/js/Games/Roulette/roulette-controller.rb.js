let conn = new signalR.HubConnectionBuilder().withUrl("/rouletteHub").build();
let timerText = document.querySelector('.timer-text');

conn.start().then(function () {

    conn.stream("StreamRouletteInfo").subscribe({
        next: function (data) {
            //console.log(data.timeLeft);
            time = (data.timeLeft * 100) / 15;

            timeText = data.timeLeft.toFixed(1);
            //timeText = timeText.toFixed(2);

            //console.log(time);
            if (time < 98) {
                setProgress(time);
                timerText.textContent = `${timeText}s`;
            }
        }
    });

    conn.on("EndRoulette", function (history, current) {
        console.log(current.colorName);
        resetTimer();
    });

    conn.on("StartAnimation", function (data) {
        startSpin(data.stopAt);
    });
});





let firstImg = new Image();

// Create callback to execute once the image has finished loading.
firstImg.onload = function () {
    firstWheel.wheelImage = firstImg; // Make wheelImage equal the loaded image object.
    firstWheel.draw(); // Also call draw function to render the wheel.
}

// Set the image source, once complete this will trigger the onLoad callback (above).
firstImg.src = "/img/roulette-body.png"; //"https://i.imgur.com/nJfYAzd.png";
firstImg.height = 500;
firstImg.width = 500;

// Vars used by the code in this page to do power controls.
let wheelPower = 3;
let wheelSpinning = false;
let allowBetting = false;

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
        'duration': 2,
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
        if (wheelPower == 1) {
            firstWheel.animation.spins = 3;
        } else if (wheelPower == 2) {
            firstWheel.animation.spins = 8;
        } else if (wheelPower == 3) {
            firstWheel.animation.spins = 15;
        }
        firstWheel.animation.stopAngle = stopAt;
        // Begin the spin animation by calling startAnimation on the wheel object.
        firstWheel.startAnimation();
    }
}

async function restoreWheel(indicatedSegment) {
    // Do basic alert of the segment text. You would probably want to do something more interesting with this information.
    //alert("You have won " + indicatedSegment.text);
    firstWheel.stopAnimation(false); // Stop the animation, false as param so does not call callback function.

    firstWheel.rotationAngle = firstWheel.rotationAngle % 360;
}