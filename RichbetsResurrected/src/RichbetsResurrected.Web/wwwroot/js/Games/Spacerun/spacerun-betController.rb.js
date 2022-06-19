var _actualStatus = "manual";

async function getAll() {
    let inputField = document.querySelector(`#${_actualStatus}_coins`);

    let result = await connection.invoke("GetPoints").catch(function (err) {

        return console.error(err.toString());

    });
    inputField.value = result;
}

$('.input-button').click(function () {

    let inputField = document.querySelector(`#${_actualStatus}_coins`);
    let action = $(this).attr("action");


    if (inputField.value == "") {
        inputField.value = 0;
    }


    switch (action) {
        case "clear":
            inputField.value = "";
            break;

        case "half":
            inputField.value = (inputField.value / 2).toFixed(0);
            break;

        case "double":
            inputField.value = parseInt(inputField.value * 2);
            break;

        case "max":
            getAll();
            break;
    }
});

$('.option-container').click(function () {
    let action = $(this).attr("action");
    switch (action) {
        case "auto":
            _actualStatus = "auto";
            $('#manual').hide();
            $('#manual').find('input[type="number"]').val(''); // Clear the input field

            $('#auto').show();
            break;

        case "manual":
            _actualStatus = "manual";
            $('#auto').hide();
            $('#auto').find('input[type="number"]').val(''); // Clear the input field

            $('#manual').show();
            break;
    }
    // Change the option container class manual/auto
    let optionContainer = $('.option-container[action="' + _actualStatus + '"]');

    optionContainer.addClass('option-container-active');
    optionContainer.siblings().removeClass('option-container-active');


});

$('.del-aco').click(function () {
    $(`#${_actualStatus}_cashout`).val('');
})