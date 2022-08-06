var conn = new signalR.HubConnectionBuilder().withUrl("/slotsHub").build();

const IMG_PATH = '/img/Slots_Classic/'

var gambleSound = new Audio('/sounds/Slots_Classic/gambleSound.mp3')
var modalOn = new Audio('/sounds/Slots_Classic/modalOn.mp3')
var slotScrollAndStop = new Audio('/sounds/Slots_Classic/slotScrollAndStop.mp3')

var rollSlots = document.querySelectorAll('.spin-slot')

$('.spin-button').click(function() {
    spin(10, 3)
    modalOn.play()
    gambleSound.play()
    $('.win-amount').text('RICHBETS')

    let serverResult = //serverSpin($('.bet-input').val(), 1)

    Object.values(rollSlots).forEach(item => {
        item.setAttribute('src', `${IMG_PATH}animation-${item.getAttribute('data-slot')}.gif`)
    })

    let i = 0;
    slotScrollAndStop.play()
    let showResultsInterval = setInterval(() => {
        document.querySelector(`[data-slot='${i}']`).setAttribute('src', `${IMG_PATH}${serverResult.result[i]}.png`)
        i++
        if (i === 3) {
            clearInterval(showResultsInterval)
            showResult(serverResult.isWin, serverResult.betValue)
            if (serverResult.isWin) {
                //serverRedeem(1, serverResult.hash)
            }
        }
    }, 1000)
})

function showResult(isWin, betValue) {
    if (isWin) {
        $('.win-amount').text(betValue + ' RBC')
    }else{
        $('.win-amount').text('LOSER')
    }
}

$('.amount-button').click(function() {
    if($(this).attr('data-action') === "add"){
        $('.bet-input').val(parseInt($('.bet-input').val()) + 10)
    }
    if($(this).attr('data-action') === "sub"){
        if (parseInt($('.bet-input').val()) === 0) {
            return
        }
        $('.bet-input').val(parseInt($('.bet-input').val()) - 10)
    }
})

conn.start().then(async function () {
    console.log(await conn.invoke("spin", 1000, 1))

    conn.on('withdrawEnd', function (result) {
        console.log(result)
    })
})

// create a connection to hub with url 'slotsHub'
// invoke the 'spin' method with arguments 'betAmount'(int) and 'delayAmountToWithdraw'(float)
// the server will return an object with the following properties: 'isSuccess', 'symbols'(it's an array with 3 values) and 'errorMessage' (null if isSuccess is true)
// server will wait 'delayAmountToWithdraw' seconds before withdrawing the bet
// after 'delayAmountToWithdraw' passed the server will send an object to the specific CLIENT(not user) with the following properties: 
// 'isWin', 'winAmount'(null if isWin is false) and 'multiplier' (null if isWin is false)
// u must listen to the event called 'withdrawEnd' to get that information