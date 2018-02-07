function BindChart(chartPlaceholder, chartType, actionUrl, chartLabel, xAxisLabel, yAxisLabel) {
    $.ajax({
        url: actionUrl,
        type: "GET",
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function () { console.log("ajax error"); },
        success: function (response) {
            switch (chartType) {
                case '1':
                    DrawPieChart(chartPlaceholder, response, chartLabel, xAxisLabel, yAxisLabel);
                    break;
                case '2':
                    DrawBarChart(chartPlaceholder, response, chartLabel, xAxisLabel, yAxisLabel);
                    break;
                case '3':
                    DrawLineChart(chartPlaceholder, response, chartLabel, xAxisLabel, yAxisLabel);
                    break;
                default:
                    DrawPieChart(chartPlaceholder, response, chartLabel, xAxisLabel, yAxisLabel);
                    break;
            }
        }
    });
}


function DrawBarChart(chartPlaceholder, data, chartLabel, xAxisLabel, yAxisLabel) {
    var pieChart = $("#" + chartPlaceholder);
    var myData = [];
    var ticks = [];
    $.each(data, function (index, item) {
        myData[index] = [index + 1, item.data];
        ticks[index] = [index + 1, item.label];
    });

    var dataset = [
      { label: chartLabel, data: myData, color: "#5482FF" }
    ];

    var options = {
        series: {
            bars: {
                show: true
            }
        },
        bars: {
            align: "center",
            barWidth: 0.5,
            order: 1
        },
        xaxis: {
            axisLabel: xAxisLabel,
            axisLabelUseCanvas: true,
            axisLabelFontSizePixels: 12,
            axisLabelFontFamily: 'Verdana, Arial',
            axisLabelPadding: 10,
            ticks: ticks

        },
        yaxis: {
            axisLabel: yAxisLabel,
            axisLabelUseCanvas: true,
            axisLabelFontSizePixels: 12,
            axisLabelFontFamily: 'Verdana, Arial',
            axisLabelPadding: 3,
            tickFormatter: function (v, axis) {
                return v;
            }
        },
        legend: {
            noColumns: 0,
            labelBoxBorderColor: "#000000",
            position: "nw"
        },
        grid: {
            hoverable: true,
            clickable: true,
            autoHighlight: true,
            borderWidth: 2,
            backgroundColor: { colors: ["#ffffff", "#EDF5FF"] }
        },
        valueLabels: {
            show: false
        },
        tooltip: true,
        tooltipOpts: {
            content: "<strong>%s</strong><br>%x : <strong>%y</strong>",
            shifts: {
                x: -60,
                y: 25
            }
        }
    };




    $.plot(pieChart, dataset, options);
    pieChart.bind("plotclick", function (event, pos, item) {
        if (item) {
            pieChart.highlight(item.series, item.datapoint);
        }
    });
}

function DrawLineChart(chartPlaceholder, data, chartLabel, xAxisLabel, yAxisLabel) {
    var chartHolder = $("#" + chartPlaceholder);
    var myData = [];
    $.each(data, function (index, item) {
        myData[index] = {
            label: item.label,
            data: JSON.parse(item.data),
            lines: { show: item.lines.show },
            points: { show: item.points.show, symbol: item.points.symbol, fillColor: item.points.symbol.color },
            color: item.color
        }
    });

    $.plot(chartHolder, myData, {
        xaxis: {
            mode: "time",
            timeformat: "%b",
            minTickSize: [1, "month"],
            ticks: [
                [1, "Jan"], [2, "Feb"], [3, "Mar"], [4, "Apr"], [5, "May"], [6, "Jun"],
                [7, "Jul"], [8, "Aug"], [9, "Sep"], [10, "Oct"], [11, "Nov"], [12, "Dec"]
            ],
            tickFormatter: function (v, ticks) {
                return v;
            },
            axisLabel: xAxisLabel,
            axisLabelUseCanvas: true,
            axisLabelFontSizePixels: 12,
            axisLabelFontFamily: 'Verdana, Arial, Helvetica, Tahoma, sans-serif',
            axisLabelPadding: 5
        },
        yaxis: {
            axisLabel: yAxisLabel,
            axisLabelUseCanvas: true,
            axisLabelFontSizePixels: 12,
            axisLabelFontFamily: 'Verdana, Arial, Helvetica, Tahoma, sans-serif',
            axisLabelPadding: 5,
            axisLabelMargin: 20,
            reserveSpace: true,
            tickFormatter: function (v, axis) {
                return v;
            },
            margin: {
                top: 18
            },
        },
        series: {
            lines: { show: true },
            points: {
                radius: 3,
                show: true,
                fill: true
            },
        },
        grid: {
            hoverable: true,
            clickable: false,
            mouseActiveRadius: 30,
            borderWidth: 1,
            minBorderMargin: 20,
            labelMargin: 10,
            backgroundColor: {
                colors: ["#fff", "#e4f4f4"]
            },
        },
        tooltip: true,
        tooltipOpts: {
            content: "<strong>%s</strong>%b<br>%x : <strong>%y</strong>",
            shifts: {
                x: -60,
                y: 25
            }
        },
        xaxisTransform: function (month) { return Date.parse(month + ' 1, 1970'); }
    });

    var legendContainer = $(".legend");
    legendContainer.css({
        margin: '18px 0 0 600px'
    });
    legendContainer.children('div').css({
        'background-color': '#e6f5f5',
        'opacity': '0.85',
        'position': 'absolute',
        'left': '600px',
        'top': '18px',
        'width': '249px',
        'z-index': '10',
    })
    legendContainer.children('table').removeAttr("style").css({ 'position': 'relative', 'z-index': '11', 'max-width': '138px' });
}

function DrawPieChart(chartPlaceholder, data, chartLabel, xAxisLabel, yAxisLabel) {
    var pieChart = $("#" + chartPlaceholder);
    var options = {
        series: {
            pie: {
                show: true,
                label: {
                    show: true,
                    radius: 0.8,
                    formatter: function (label, series) {
                        //return '<div style="border:1px solid grey;font-size:8pt;text-align:center;padding:5px;color:white;">' +
                        //label + ' : ' +Math.round(series.percent) +'%</div>';

                        return '<div style="border:1px solid grey;font-size:8pt;text-align:center;padding:5px;color:white;">' +
                      series.percent +
                      '%</div>';
                    },
                    background: {
                        opacity: 0.5,
                        color: '#000'
                    }
                },
                radius: 0.8,
            }

        },
        legend: {
            show: true
        }
    };
    $.plot(pieChart, data, options);
}