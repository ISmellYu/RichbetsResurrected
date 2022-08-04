const IMG_PATH = '/img/Slots_Classic/'

var rollSlots = document.querySelectorAll('.spin-slot')

$('.spin-button').click(function() {
    $('.win-amount').text('RICHBETS')

    let serverResult = serverSpin($('.bet-input').val(), 1)

    Object.values(rollSlots).forEach(item => {
        item.setAttribute('src', `${IMG_PATH}animation-${item.getAttribute('data-slot')}.gif`)
    })

    let i = 0;
    let showResultsInterval = setInterval(() => {
        document.querySelector(`[data-slot='${i}']`).setAttribute('src', `${IMG_PATH}${serverResult.result[i]}.png`)
        i++
        if (i === 3) {
            clearInterval(showResultsInterval)
            showResult(serverResult.isWin, serverResult.betValue)
            if (serverResult.isWin) {
                serverRedeem(1, serverResult.hash)
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