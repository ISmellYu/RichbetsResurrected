var conn = new signalR.HubConnectionBuilder().withUrl("/slotsHub").build();

const IMG_PATH = '/img/Slots_Classic/'

var gambleSound = new Audio('/sounds/Slots_Classic/gambleSound.mp3')
var modalOn = new Audio('/sounds/Slots_Classic/modalOn.mp3')
var slotScrollAndStop = new Audio('/sounds/Slots_Classic/slotScrollAndStop.mp3')

var rollSlots = document.querySelectorAll('.spin-slot')

conn.start().then(function () {
    $('.spin-button').click(async function() {

        let result =  await conn.invoke("spin", parseInt($('.bet-input').val()), 3)

        modalOn.play()
        gambleSound.play()
        $('.win-amount').text('RICHBETS')

        Object.values(rollSlots).forEach(item => {
            item.setAttribute('src', `${IMG_PATH}animation-${item.getAttribute('data-slot')}.gif`)
        })

        let i = 0;
        slotScrollAndStop.play()
        let showResultsInterval = setInterval(() => {
            document.querySelector(`[data-slot='${i}']`).setAttribute('src', `${IMG_PATH}${result.symbols[i]}.png`)
            i++
            if (i === 3) {
                clearInterval(showResultsInterval)
            }
        }, 1000)
    })

    conn.on('withdrawEnd', function (result) {
        showResult(result.isWin, result.winAmount, result.multiplier)
    })

    function showResult(isWin, betValue, multiplier) {
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
})