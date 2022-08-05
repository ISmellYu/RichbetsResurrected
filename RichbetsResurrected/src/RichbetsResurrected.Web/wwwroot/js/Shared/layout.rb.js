let connection = new signalR.HubConnectionBuilder().withUrl("/clientHub").build();

let url= window.location.href.split('/');
url = url[3].toLocaleLowerCase();

if (url == "itemshop") {
    document.getElementById("itemshop-link").classList.add("-link-active");
}
else if (url == "dashboard") {
    document.getElementById("dashboard-link").classList.add("-link-active");
}
else if (url == "home" || url == "games" || url == "") {
    document.getElementById("home-link").classList.add("-link-active");
}

$(".hamburger").click(function() {
    if ($("#menu").hasClass("menu-open")) { // menu hidden
        $("#menu").removeClass("menu-open");
        $("#menu").addClass("menu-hidden");
        hamburgerOpen();
        $("body").css("overflow", "auto");
        return;
    }
    $("#menu").removeClass("menu-hidden"); // menu shown
    $("#menu").addClass("menu-open");
    hamburgerClose();
    $("body").css("overflow", "hidden");
});

function hamburgerClose() {
    $("#bar1").css("transform", "matrix(0.7071, 0.7071, -0.7071, 0.7071, 0, 4)");
    $("#bar2").css("width", "0%");
    $("#bar3").css("transform", "matrix(0.7071, -0.7071, 0.7071, 0.7071, 0, -4)");
}

function hamburgerOpen() {
    $("#bar1").css("transform", "none");
    $("#bar2").css("width", "16px");
    $("#bar3").css("transform", "none");
}

connection.start().then(async function () {

    setInterval(async function () {

        let result = await connection.invoke("GetPoints").catch(function (err) {

            return console.error(err.toString());

        });

        document.querySelectorAll('.walletmeter').forEach(element => {
            element.textContent = result;
        })

    }, 1000);

    
    let clientInfo = await connection.invoke("GetClientInfo").catch(function (err) {

        return console.error(err.toString());
    
    });

    globalThis.clientInfo = clientInfo;
 
}).catch(function (err) {
    return console.error(err.toString());
});