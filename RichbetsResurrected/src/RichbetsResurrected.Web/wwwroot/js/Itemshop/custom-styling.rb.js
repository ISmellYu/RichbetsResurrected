$(".buy-button").click(function() {
    console.log(this.getAttribute("data-item-id"));
    $.ajax({
        type: "POST",
        url: `/Itemshop/BuyItem?itemId=${this.getAttribute("data-item-id")}`,
        dataType: "html",
        success: function(data) {
            data = JSON.parse(data);
            console.log(data);
            if (data.isSuccess) {
                new Snack(
                    {
                        message: `Item <p style="margin:5px;" class="${data.item.description}">${data.item.name}</p> successfully purchased.`,
                        duration: 3000
                    }
                );
            }else{
                new Snack(
                    {
                        message: `${data.error.message}`,
                        duration: 3000,
                        type: `error`
                    }
                );
            }
        },
        error: function() {
            new Snack(
                {
                    message: `Unexpected error`,
                    duration: 3000,
                    type: `error`
                }
            );
        }
    });
});