let crashConn = new signalR.HubConnectionBuilder().withUrl("/crashHub").build();

var _dataPlayersOld;
var _oldCrashed;
var _oldStart;
var _playerData;
var _gameStatus;
var _isUserBetting = false;
var _checkedMultiplayer = false;

crashConn.start().then(function () {
    
    getUserData().then(function (data) {
        _playerData = data;
        if (isPlayerBetting(data, _dataPlayersOld)) {
            $("#manual-place-bet").text("Cashout");
            _isUserBetting = true;
        }
    });

    crashConn.stream("StreamCrashInfo").subscribe({
        next: function (data) {
            _gameStatus = data.allowPlacingBets;

            if (JSON.stringify(data.players) !== JSON.stringify(_dataPlayersOld)) {
                _dataPlayersOld = data.players;
                renderPlayersList(data.players);
            }

            if (_oldCrashed != data.crashed) {
                if (data.crashed == true) {
                  console.log("crashed");
                  RestoreBetting();
                }
                _oldCrashed = data.crashed;
            }

            if (_oldStart != data.allowPlacingBets) {
                if (data.timeLeft == 0 && !_isUserBetting) {
                    DisableBetting();
                }
                _oldStart = data.allowPlacingBets;
            }

            checkStartMultiplayer(data.allowPlacingBets);
        }
    });

    $("#manual-place-bet").click(function () {
        if (_gameStatus == false) {
            cashout();
        }else{
            placeBet(10000);
        }
    });

    $("#auto-place-bet").click(function () {
        console.log("Auto Place Bet");
    });

    async function placeBet(amount) {
        let result = await crashConn.invoke("JoinCrash", amount).catch(function (err) {
            return console.error(err.toString());
        });

        if (result.isSuccess == true) {

            _isUserBetting = true;
            $("#manual-place-bet").text("Cashout");
        }
    }

    async function cashout() {
        let result = await crashConn.invoke("Cashout").catch(function (err) {
            return console.error(err.toString());
        });

        if (result.isSuccess == true) {

            _isUserBetting = false;
            return true;
        }
    }

    function checkStartMultiplayer(allowBetting) {
        if (!_checkedMultiplayer && !allowBetting) {
            DisableBetting();
            _checkedMultiplayer = true;
        }
    }
    
    
    function renderPlayersList(data) {
        let _total = 0;

        clearPlayersList();

        data.forEach(element => {
            let cashout;
            
            if (element.whenCashouted == 0) {
                cashout = `<td>Pending..</td>`;
            }else{
                cashout = `<td style="color: #00C74D">${element.whenCashouted}</td>`; 
            }

            let tr = document.createElement("tr");
            tr.innerHTML = `<td style="background-image: url(${element.avatarUrl});">${element.userName}</td>${cashout}<td>${element.amount}</td>`;
            $(".players-table").append(tr);
            _total += element.amount;

            
        });

        $("#player-count").text(data.length + " Players");
        $("#bets-total").text(_total + " RBC");
    }

    function clearPlayersList() {
        $(".players-table").empty();
    }

    function isPlayerBetting(playerData, gamePlayersData) {
        for (let i = 0; i < gamePlayersData.length; i++) {
            if (playerData.discordUserId == gamePlayersData[i].discordUserId) {
                console.log(gamePlayersData[i]);
                return true;
            }
        }
        return false;
    }

    function getPlayerBetAmount(playerData, gamePlayersData) {
        for (let i = 0; i < gamePlayersData.length; i++) {
            if (playerData.discordUserId == gamePlayersData[i].discordUserId) {
                return gamePlayersData[i].amount;
            }
        }
        return false;
    }

    function RestoreBetting() {
        $("#manual-place-bet").removeClass("bet-button-disabled");
        $("#auto-place-bet").removeClass("bet-button-disabled");

    }

    function DisableBetting() {
        $("#manual-place-bet").addClass("bet-button-disabled");
        $("#auto-place-bet").addClass("bet-button-disabled");
    }
});

async function getUserData() {
    let result = await connection.invoke("GetClientInfo").catch(function (err) {

        return console.error(err.toString());

    });
    return result;
}
