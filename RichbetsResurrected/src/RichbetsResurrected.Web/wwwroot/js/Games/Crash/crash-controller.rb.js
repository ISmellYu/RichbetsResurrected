let conn = new signalR.HubConnectionBuilder().withUrl("/crashHub").build(); // Create a new connection to the hub.

$(document).ready(async function() {

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

    let config = {
        type: 'line',
        data: {
            labels: [0],
            datasets: [{
                fill: false,
                borderColor: "white",
                backgroundColor: 'transparent',
                pointRadius: 0,
                color: 'red',
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
                        display: false,
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
                        display: false
                    },
                    gridLines: {
                        display: false,
                    },
                    ticks: {
                        beginAtZero: true,
                        backdropColor: 'rgba(255,255,255,1)',
                        fontColor: '#EE5353',
                        fontSize: 14,
                        fontFamily: 'Poppins',
                        fontWeight: '200',
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

        myChart.update();
    }

    function restoreChart() // Restoring chart to its original state
    {
        myChart.data.labels = [0];
        myChart.data.datasets[0].data = [0];
        myChart.options.scales.yAxes[0].ticks.max = 2;
        myChart.data.datasets[0].backgroundColor = "white";
        myChart.data.datasets[0].borderColor = 'rgb(94, 183, 110)';
        myChart.update();
    }
    
    function crashChart()   // Invoked when server sent crashmsg to client, changes line to color red and background color
    {
        myChart.data.datasets[0].backgroundColor = gradientStrokeLose;
        myChart.data.datasets[0].borderColor = '#E1675A';
        console.log("crash");
        myChart.update();
    }

    conn.start().then(function () {
        conn.stream("StreamCrashInfo").subscribe({
            next: function (data) {
                   
                    let labels = [];

                    if (data.crashed) {
                        crashChart();
                        return
                    }

                    for (let i = 1; i < data.multipliers.length; i++) {
                        labels.push(i);
                    }

                    updateChart(labels, data.multipliers);
                }
        });
    
    });

},
function(){
    ErrorConnectingToServer();
}
);