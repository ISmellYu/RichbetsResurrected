let conn = new signalR.HubConnectionBuilder().withUrl("/rouletteHub").build();

conn.start().then(function () {
    console.log("connected");
    
    conn.on("UpdateTimer", function (time) {
        console.log(time);
        document.getElementById("timer").innerHTML = time;
    });
    
    conn.on("EndRoulette", function (data) {
        console.log(data);
    });

    conn.on("PlayerJoined", function (data) {
        console.log(data);
    });

    conn.on("StartAnimation", function (data) {
        console.log(data);
    });
    
    conn.stream("StreamRouletteInfo").subscribe({
        next: function (data) {
            console.log(data);
        }
    });
    
     
}).catch(function (err) {
    return console.error(err.toString());
});