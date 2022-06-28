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
            // document.getElementById("space-multiplier").textContent = data.multiplier + "X";
            setMultiplier(data.multiplier);

            if (oldData != data) {
              let object = constructObj(data);
              sendDataToWebGL(JSON.stringify(object));
              oldData = data;
            }

            if (oldCrashed != data.crashed) {
              if (data.crashed == true) {
                console.log("Crashed");
                unityInstance.SendMessage('Main Camera', 'CrashRocket')
                document.getElementById("multiplier").style.color = "#EE5353";
              }
              oldCrashed = data.crashed;
            }

            if (data.timeLeft == 10) {
              console.log("reset rocket")
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
                console.log("Start rocket");
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
    let firstPart = (data+"").split(".")[0];
    let secondArray = Array.from(String(data), Number);
    console.log(secondArray[2] + " . " + secondArray[3])

    if (isNaN(secondArray[2])) {
      secondArray[2] = 0;
    }

    if (isNaN(secondArray[3])) {
      secondArray[3] = 0;
    }

    document.getElementById("n0").textContent = firstPart;
    document.getElementById("n1").textContent = secondArray[2];
    document.getElementById("n2").textContent = secondArray[3];
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
