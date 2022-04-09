let progressBar = document.querySelector('.lay-circle');
let speed = 5;


function setProgress() {
    console.log('setProgress');
    inputProgressValue = document.getElementById('progress').value;
    progressBar.style.background = `conic-gradient(
        #EE5353 ${inputProgressValue * 3.6}deg,
        #121212 ${inputProgressValue * 3.6}deg
    )`;
}

function resetTimer(){
    let progressValue = 0;
    let progress = setInterval(() => {
        progressValue += 1;
        progressBar.style.background = `conic-gradient(
            #EE5353 ${progressValue * 3.6}deg,
            #121212 ${progressValue * 3.6}deg
        )`;
        if (progressValue >= 100) {
            clearInterval(progress);
        }
    }, speed);
}
