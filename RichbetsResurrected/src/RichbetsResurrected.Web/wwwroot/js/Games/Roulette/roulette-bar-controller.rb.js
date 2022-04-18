$('.control-bar-button').click(function () {

    let inputField = document.querySelector('#coins');
    let action = $(this).attr("action");


    if (inputField.value == "") {
        inputField.value = 0;
    }


    switch (action) {
        case "clear":
            inputField.value = "";
            break;

        case "1":
            inputField.value = parseInt(inputField.value) + 1;
            break;

        case "10":
            inputField.value = parseInt(inputField.value) + 10;
            break;

        case "100":
            inputField.value = parseInt(inputField.value) + 100;
            break;

        case "1000":
            inputField.value = parseInt(inputField.value) + 1000;
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

async function getAll() {

    let inputField = document.querySelector('#coins');
    let result = await connection.invoke("GetPoints").catch(function (err) {

        return console.error(err.toString());

    });
    inputField.value = result;
}