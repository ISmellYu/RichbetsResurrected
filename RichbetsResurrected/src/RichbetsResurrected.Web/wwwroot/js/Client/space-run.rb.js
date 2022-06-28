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
            document.getElementById("space-multiplier").textContent = data.multiplier + "X";

            if (oldData != data) {
              let object = constructObj(data);
              sendDataToWebGL(JSON.stringify(object));
              oldData = data;
            }

            if (oldCrashed != data.crashed) {
              if (data.crashed == true) {
                console.log("Crashed");
                unityInstance.SendMessage('Main Camera', 'CrashRocket')
                document.getElementById("space-multiplier").style.color = "#EE5353";
              }
              oldCrashed = data.crashed;
            }

            if (data.timeLeft == 10) {
              console.log("reset rocket")
              unityInstance.SendMessage('Main Camera', 'ResetRocket')
              document.getElementById("space-multiplier").style.color = "white";
            }

            if (data.timeLeft > 0) {
              document.getElementById("space-multiplier").textContent = data.timeLeft;

              if (_checkAppStart) {
                unityInstance.SendMessage('Main Camera', 'ResetRocket')
                _checkAppStart = false;
              }
            }

            if (oldStart != data.allowPlacingBets) {
              if (data.timeLeft == 0) {
                console.log("Start rocket");
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
