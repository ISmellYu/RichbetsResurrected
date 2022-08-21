let conn = new signalR.HubConnectionBuilder().withUrl("/crashHub").build();

let oldData;
let oldCrashed;
let oldStart;
let oldTime;
let _checkAppStart = true;

conn.start().then(function () {
    conn.stream("StreamCrashInfo").subscribe({
        next: function (data) {

          if (wakeup == true) {
            setMultiplier(data.multiplier);

            if (oldData != data) {
              let object = constructObj(data);
              sendDataToWebGL(JSON.stringify(object));
              oldData = data;
            }

            if (oldCrashed != data.crashed) {
              if (data.crashed == true) {
                unityInstance.SendMessage('Main Camera', 'CrashRocket')
                document.getElementById("multiplier").style.color = "#EE5353";
              }
              oldCrashed = data.crashed;
            }

            if (data.timeLeft == 10) {
              unityInstance.SendMessage('Main Camera', 'ResetRocket')
              document.getElementById("multiplier").style.color = "#fff";
            }

            if (data.timeLeft > 0) {
              unityInstance.SendMessage('Main Camera', 'ResetRocket')
              document.getElementById("multiplier").style.color = "#fff";

              if (_checkAppStart) {
                unityInstance.SendMessage('Main Camera', 'ResetRocket')
                document.getElementById("multiplier").style.color = "#fff";
                _checkAppStart = false;
              }
            }

            if (oldStart != data.allowPlacingBets) {
              if (data.timeLeft == 0) {
                document.getElementById("multiplier").style.color = "#fff";
                unityInstance.SendMessage('Main Camera', 'StartRocket')
              }
              oldStart = data.allowPlacingBets;
            }

          }
        }
    });

  function sendDataToWebGL(obj) {
    unityInstance.SendMessage('Main Camera', 'UpdateData', obj)
  }

  function setMultiplier(data) {
  	if (data == 1) {
  		data = "1.00";
  	}

  	let multiplierArray = (data + "").split("."); // convert ex. 1.2 -> 1.20

  	if (multiplierArray[1].length == 1) {
  		multiplierArray[1] = multiplierArray[1] + 0;
  	}
  	document.getElementById("n0").textContent = multiplierArray[0];
  	document.getElementById("n1").textContent = multiplierArray[1][0];
  	document.getElementById("n2").textContent = multiplierArray[1][1];
  }

  function constructObj(data) {
    return {
      "Multiplier": data.multiplier,
      "TimeLeft": data.timeLeft,
      "AllowPlacingBets": data.allowPlacingBets,
      "AllowRemovingBets": data.allowRemovingBets,
      "Crashed": data.crashed,
      "GameStarted": data.gameStarted,
      "Running": data.running
    }
  }
});
