function Snack(givenOptions) {
    var _Options;
    var snackId = _generateId();

    function _applyUserOptions(){
        _Options = {
            message: givenOptions?.message ?? 'Default message',
            duration: givenOptions?.duration ?? 3000,
            type: givenOptions?.type ?? 'normal'
        };
    }

    function _generateId() {
        let result = '';
        let characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        for(let i = 0; i < 5; i++){
            result += characters.charAt(Math.floor(Math.random() * characters.length));
        }
        return result;
    }

    function _createSnackContainer() {
        let snackContainer = document.createElement("div");

        snackContainer.className = "snack-container";
        document.body.appendChild(snackContainer);
    }

    function _createSnack() {
        let snackBody = document.createElement("div");
        let timerBar = _createTimerBar();
        snackBody.className = "default-snack";

        document.getElementsByClassName("snack-container")[0].appendChild(snackBody);

        snackBody.appendChild(timerBar);
        snackBody.appendChild(_createBodyContainer());

        snackBody.id = snackId;

        let leftTime = _Options.duration;

        let timer = setInterval(() => {
            leftTime -= 10;
            
            timerBar.style.width = leftTime / _Options.duration * 100 + "%"; // calculate width of timer bar based on left time

            if(leftTime < 200){
                snackBody.style.animation = "slideOut 0.5s";
            }

            if (leftTime == 0) {
                clearInterval(timer);
                snackBody.remove();

                if (_ifContainerNotActive()) { // if there is no more snack in the container - remove the container
                    document.getElementsByClassName("snack-container")[0].remove();
                }
            }
        }, 10);

        function _createBodyContainer() {
            let mainContainer = document.createElement("div");

            mainContainer.classList.add("snack-body-container");
            mainContainer.innerHTML = _Options.message;

            let killButton = document.createElement("button");

            killButton.classList.add("snack-kill-button");
            killButton.textContent = "✕";
            killButton.setAttribute("onclick", `document.getElementById("${snackId}").remove();`);

            mainContainer.appendChild(killButton);

            return mainContainer;
        }
    }

    function _createTimerBar(){
        let timerBar = document.createElement("div");

        timerBar.className = "snack-timer-bar";
        switch (_Options.type) {
            case "warning":
                timerBar.style.backgroundColor = "#ffcc00";
                break;
            case "error":
                timerBar.style.backgroundColor = "#cc3300";
                break;
            default:
                timerBar.style.backgroundColor = "#3BB2E2"
                break;
        };
        return timerBar;
    }

    function _ifContainerNotActive() {
        var element = document.getElementsByClassName("snack-container")[0];

        if(typeof(element) != 'undefined' && element != null){ // double check of existence to prevent bugs from user interaction
            if (!document.getElementsByClassName("snack-container")[0].childElementCount) {
                return true;
            }
        } else{
            return false;
        }
    }

    _applyUserOptions();

    if (document.getElementsByClassName("snack-container").length > 0) {
        _createSnack();

        
    } else {
        _createSnackContainer(); 
        _createSnack();
    }
}

window.onload = () => {
    function _initStyle(){
        let style = document.createElement("style");

        style.innerHTML = `
            ${_snakContainerStyle}
            ${_defaultSnackStyle}
            `;
        style.type = "text/css";

        document.head.appendChild(style);
    }
    
    _initStyle();
}

// Stylings

const _snakContainerStyle = `
    .snack-container{
        position: fixed;
        bottom: 0;
        right: 0;
        width: fit-content;
        height: fit-content;
        z-index: 9999;
    }
`;

const _defaultSnackStyle = `
    .default-snack{
        width: fit-content;
        min-width: 258px;
        height: 51px;
        background-color: #181818;
        margin: 5px;
        position: relative;
        border-radius: 3px;
        overflow: hidden;
        color: white;
        font-family: Helvetica, sans-serif;
        font-size: 14px;
        box-shadow: 0px 0px 5px 0px rgba(0,0,0,0.75);
        animation: slideIn 0.5s;
    }

    @keyframes slideIn {
        0% {
            transform: translateX(100%);
        }
        100% {
            transform: translateX(0%);
        }
    }

    @keyframes slideOut {
        0% {
            transform: translateX(0%);
        }
        100% {
            transform: translateY(100%);
        }
    }

    .snack-timer-bar{
        width: 100%;
        height: 3px;
        background-color: #3BB2E2;
        position: absolute;
        top: 0;
        left: 0;
    }

    .snack-body-container{
        margin: auto;
        width: 90%;
        height: 100%;
        display: flex;
        justify-content: left;
        align-items: center;
    }

    .snack-kill-button{
        position: absolute;
        right: 0;
        margin: 5px;
        background-color: transparent;
        border: none;
        color: rgba(255, 255, 255, 0.5);
        font-size: 16px;
        cursor: pointer;
        outline: none;
    }
`;