let connection = new signalR.HubConnectionBuilder().withUrl("/clientHub").build();

connection.start().then(async function () {

    setInterval(async function () {

        let result = await connection.invoke("GetPoints").catch(function (err) {

            return console.error(err.toString());

        });

        document.querySelector('.odometer').textContent = result;

    }, 1000);

    
    let clientInfo = await connection.invoke("GetClientInfo").catch(function (err) {

        return console.error(err.toString());
    
    });

    globalThis.clientInfo = clientInfo;
 
}).catch(function (err) {
    return console.error(err.toString());
});