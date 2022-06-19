let conn = new signalR.HubConnectionBuilder().withUrl("/crashHub").build();

let oldData;
let oldCrashed;
let oldMulti;
let oldStart;
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
                unityInstance.SendMessage('Main Camera', 'CrashRocket')
                document.getElementById("space-multiplier").style.color = "#EE5353";
              }
              oldCrashed = data.crashed;
            }

            if (oldMulti != data.multiplier) {
              if (data.multiplier == 1) {
                unityInstance.SendMessage('Main Camera', 'ResetRocket')
                document.getElementById("space-multiplier").style.color = "white";
              }
              oldMulti = data.multiplier;
            }

            if (oldStart != data.allowPlacingBets) {
              if (data.timeLeft == 0) {
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
