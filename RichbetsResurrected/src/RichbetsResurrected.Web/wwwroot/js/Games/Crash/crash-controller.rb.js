let conn = new signalR.HubConnectionBuilder().withUrl("/crashHub").build(); // Create a new connection to the hub.

$(document).ready(async function() {
    let multText = $(".multiplier");

    let ctx = $("#myChart")[0].getContext('2d');    // Get context table
    let gradientStroke;
    let gradientStrokeLose;
    
    gradientStroke = ctx.createLinearGradient(0, 0, 800, 500);  // Linear gradient
    gradientStroke.addColorStop(0, 'rgb(94, 183, 110, .4)');
    gradientStroke.addColorStop(1, 'rgb(94, 183, 110, 0)');
    //
    gradientStrokeLose = ctx.createLinearGradient(0, 0, 800, 500);  // Linear gradient
    gradientStrokeLose.addColorStop(0, 'rgb(252, 25, 28, .4)');
    gradientStrokeLose.addColorStop(1, 'rgb(234, 47, 43, 0)');

    let point = [];

    var sun = new Image();
    sun.src = 'https://i.imgur.com/yDYW1I7.png';

    let config = {
        type: 'line',
        data: {
            labels: [0],
            datasets: [{
                label: "Tokyo",
                fill: true,
                borderColor: "rgba(75,192,192,1)",
                pointBackgroundColor: "#fff",
                pointRadius: 5,
                data: [0]
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            tooltips: {
                enabled: false
            },
            legend: {
                display: false
            },
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true
                    },
                    gridLines: {
                        display: true,
                    },
                    ticks: {
                        min: 1,
                        stepSize: 1,
                        display: true,
                    }
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true
                    },
                    ticks: {
                        beginAtZero: true,
                        min: 1,
                        max: 2,

                    }
                }]
            }
        }
    };  // Config for chart


    let myChart = new Chart(ctx, config);   // Creating chart

    function updateChart(labels, data)  // Update chart( CAUTION! U MUST PROVIDE EXACTLY THE SAME NUMBERS OF TABLES AS DATA)
    {
        myChart.data.labels = labels
        myChart.data.datasets[0].data = data;
        //console.log(data.length + " " + point.length);
        myChart.options.scales.yAxes[0].ticks.max = Math.max.apply(2, data) + 1;

        //hide all points from chart and show only last one
        for (let i = 0; i < point.length; i++)
        {
            point[i].hidden = true;
        }
        point[point.length - 1].hidden = false;


        myChart.update();
    }

    function restoreChart() // Restoring chart to its original state
    {
        myChart.data.labels = [0];
        myChart.data.datasets[0].data = [0];
        myChart.options.scales.yAxes[0].ticks.max = 2;
        myChart.data.datasets[0].backgroundColor = gradientStroke;
        myChart.data.datasets[0].borderColor = 'rgb(94, 183, 110)';
        myChart.update();
    }
    
    function crashChart()   // Invoked when server sent crashmsg to client, changes line to color red and background color
    {
        myChart.data.datasets[0].backgroundColor = gradientStrokeLose;
        myChart.data.datasets[0].borderColor = '#E1675A';
        myChart.update();
        document.getElementById("placeButton").innerHTML = "Przegrałeś";
        document.getElementById("wave").style.fill = "#A84D4D";
    }

    conn.start().then(function () {
        conn.stream("StreamCrashInfo").subscribe({
            next: function (data) {
                   //console.log(data);
                    let labels = [];
                    point = [];
                    for (let i = 1; i < data.multipliers.length; i++) {
                        labels.push(i);
                        point.pop();
                        point.push(0);
                        point.push(25);
                        //console.log(point.length + " " + labels.length);
                    }

                    updateChart(labels, data.multipliers);
                    if (data.crashed) {
                        point = [];
                    }
                    //console.log(point);
                }
        });
    
    });

},
function(){
    ErrorConnectingToServer();
}
);