let rollEndSound = new Audio('/sounds/rollEnd.mp3');
let rollWinSound = new Audio('/sounds/rollWin.mp3');
let rollStartSound = new Audio('/sounds/rollStart.mp3');

let FxSlider = document.getElementById("fx");
let MusicSlider = document.getElementById("music");

let FxCookieVolume = getCookie("fxVolume");
let MusicCookieVolume = getCookie("musicVolume");


if (FxCookieVolume == undefined) {
    document.cookie = `fxVolume=0.3`;
    FxCookieVolume = 0.3;
}

if (MusicCookieVolume == undefined) {
    document.cookie = `musicVolume=10`;
    MusicCookieVolume = 10;
}

FxSlider.value = (FxCookieVolume * 100).toFixed(0); // Convert the volume to a percentage.
MusicSlider.value = parseInt(MusicCookieVolume);

setGlobalVolume(FxCookieVolume);

function getCookie(name) {
    let cookieArray = document.cookie.split(";");

    for (let cookie of cookieArray) {

        cookie = cookie.replace(/\s/g, ""); // Remove whitespace.
        if (cookie.split("=")[0] == name) {

            let cookieValue = cookie.split("=")[1];
            return cookieValue;
        }
    }
}


function playSound(sound) {
    switch (sound) {
        case "rollEnd":
            rollEndSound.play();
            break;
        case "rollWin":
            rollWinSound.play();
            break;
        case "rollStart":
            rollStartSound.play();
            break;
    }
}

function setGlobalVolume(volume) {

    rollEndSound.volume = volume;
    rollWinSound.volume = volume;
    rollStartSound.volume = volume;
}


FxSlider.addEventListener("change", function () {
    let volume = FxSlider.value / 100;
    setGlobalVolume(volume);
    document.cookie = `fxVolume=${volume}`;
});


MusicSlider.addEventListener("change", function () {
    let volume = MusicSlider.value;
    document.cookie = `musicVolume=${volume}`;
    player.setVolume(volume);
});


var tag = document.createElement('script');

tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

var player;
function onYouTubeIframeAPIReady() {
  player = new YT.Player('player', {
    height: '0',
    width: '0',
    videoId: '0C3nAhSKBoU',
    playerVars: {
        'autoplay': 1,
        'controls': 0 },
    events: {
        'onReady': onPlayerReady
    }
  });
}

function onPlayerReady(event) {
    event.target.setVolume(parseInt(MusicCookieVolume));
    event.target.playVideo();
}

document.addEventListener("mousemove", function () {
    if (player.playerInfo.playerState == -1) {
        player.setVolume(parseInt(MusicCookieVolume));
        player.playVideo();
    }
    return;
});