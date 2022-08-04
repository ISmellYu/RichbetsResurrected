$('.amount-button').click(function() {
    if($(this).attr('data-action') === "add"){
        $('#bet-amount').val(parseInt($('#bet-amount').val()) + 10)
    }
    if($(this).attr('data-action') === "sub"){
        if (parseInt($('#bet-amount').val()) === 0) {
            return
        }
        $('#bet-amount').val(parseInt($('#bet-amount').val()) - 10)
    }
})