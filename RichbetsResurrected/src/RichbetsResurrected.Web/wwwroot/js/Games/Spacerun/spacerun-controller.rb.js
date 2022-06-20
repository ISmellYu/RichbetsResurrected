let crashConn = new signalR.HubConnectionBuilder().withUrl("/crashHub").build();

crashConn.start().then(function () {

    var _crashStatic = {
        playersOld: null,       // 
        crashedOld: null,       // Old variables to compare if the players list changed
        startOld: null,         //   
        multiplierOld: null,    //
    
        allowPlacingBets: null,
        crashed: null,
        gameStarted: null
    }

    var userData = {
        betting: false,
        betAmount: 0,
        betMode: "manual",
        whenCashouted: false
    }

    getUserData().then(function (data) {
        let _playerData;
        if (isPlayerBetting(data, _crashStatic.playersOld)) {
            userData.betting = true;
            _playerData = isPlayerBetting(data, _crashStatic.playersOld);
            userData.betAmount = _playerData.amount;
            userData.whenCashouted = _playerData.whenCashouted;
        }
    });

    crashConn.stream("StreamCrashInfo").subscribe({
        next: function (data) {
            _crashStatic.gameStarted = data.allowRemovingBets;

            // console.log(userData.betAmount);
            // console.log(userData.betting);
            //console.log(userData.whenCashouted);

            if (JSON.stringify(data.players) !== JSON.stringify(_crashStatic.playersOld)) { // If the players list changed
                _crashStatic.playersOld = data.players;

                renderPlayersList(data.players);
            }

            if (_crashStatic.crashedOld != data.crashed) {
                if (data.crashed == true) {
                    renderPlayersLostList(data.players);
                }
                _crashStatic.crashedOld = data.crashed;
            }

            if (_crashStatic.multiplierOld != data.multiplier) {
                if (userData.betting) {
                    if (userData.betAmount != 0) {
                        if (userData.whenCashouted) {
                            $("#manual-place-bet").text(`Cashouted ${Math.trunc(userData.betAmount * _crashStatic.multiplierOld)}`);
                        }else{
                            $("#manual-place-bet").text(
                                `Cashout ${Math.trunc(userData.betAmount * data.multiplier)}`
                            );
                        }
                    }
                }
                if (data.multiplier == 1) {
                    // reset 
                }
                _crashStatic.multiplierOld = data.multiplier;
            }
            if (data.multiplier == 1) {
                if (userData.betAmount > 0) {
                    $("#manual-place-bet").text(`Place bet (${userData.betAmount})`);
                }else{
                    $("#manual-place-bet").text("Place Bet");
                }
            }
        }
    });

    // Client control section

    $("#manual-place-bet").click(function () {
        if (_crashStatic.gameStarted == true) {
            cashout();
        }
        else{
            placeBet(1000);
        }
    });

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    function renderPlayersList(data) {
        let _total = 0;

        clearPlayersList();

        data.forEach(element => {
            let cashout;
            let cashoutAmount;
            
            if (element.whenCashouted == 0) {
                cashout = `<td>Pending..</td>`;
                cashoutAmount = `<td>${element.amount}</td>`;
            }else{
                cashout = `<td style="color: #00C74D">${element.whenCashouted}x</td>`; 
                cashoutAmount = `<td style="color: #00C74D">+${Math.trunc(element.amount * element.whenCashouted)}</td>`; 
            }

            let tr = document.createElement("tr");
            tr.innerHTML = `<td style="background-image: url(${element.avatarUrl});">${element.userName}</td>${cashout}${cashoutAmount}`;
            $(".players-table").append(tr);
            _total += element.amount;
        });

        $("#player-count").text(data.length + " Players");
        $("#bets-total").text(_total + " RBC");
    }

    function renderPlayersLostList(data) {
        let _total = 0;

        clearPlayersList();

        data.forEach(element => {
            let cashout;
            let cashoutAmount;
            
            if (element.whenCashouted == 0) {
                cashout = `<td style="color: #EE5353"></td>`; 
                cashoutAmount = `<td style="color: #EE5353">-${element.whenCashouted}</td>`; 
            }else{
                cashout = `<td style="color: #00C74D">${element.whenCashouted}x</td>`; 
                cashoutAmount = `<td style="color: #00C74D">+${Math.trunc(element.amount * element.whenCashouted)}</td>`; 
            }

            let tr = document.createElement("tr");
            tr.innerHTML = `<td style="background-image: url(${element.avatarUrl});">${element.userName}</td>${cashout}${cashoutAmount}`;
            $(".players-table").append(tr);
            _total += element.amount;
        });

        $("#player-count").text(data.length + " Players");
        $("#bets-total").text(_total + " RBC");
    }

    function clearPlayersList() {
        $(".players-table").empty();
    }

    async function placeBet(amount) {
        let result = await crashConn.invoke("JoinCrash", amount).catch(function (err) {
            return console.error(err.toString());
        });
        if (result.isSuccess == true) {
            userData.betting = true;
            userData.betAmount += parseInt(amount);
        }
    }

    async function cashout() {
        let result = await crashConn.invoke("Cashout").catch(function (err) {
            return console.error(err.toString());
        });
        if (result.isSuccess == true) {
            $("#manual-place-bet").text(`Cashouted ${Math.trunc(userData.betAmount * _crashStatic.multiplierOld)}`);
            userData.betting = false;
            userData.betAmount = 0;
            userData.whenCashouted = _crashStatic.multiplierOld;
        }
    }

    async function getUserData() {
        let result = await connection.invoke("GetClientInfo").catch(function (err) {
    
            return console.error(err.toString());
    
        });
        return result;
    }

    function isPlayerBetting(playerData, gamePlayersData) {
        for (let i = 0; i < gamePlayersData.length; i++) {
            if (playerData.discordUserId == gamePlayersData[i].discordUserId) {
                return gamePlayersData[i];
            }
        }
        return false;
    }
});