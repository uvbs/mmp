data.lineChart = {};
data.lineChart.timeData = [];
data.lineChart.interval = null;
data.lineChart.startTime = '';
data.lineChart.endTime = '';
data.lineChart.yMin = 0;
data.lineChart.yMax = 0;
data.lineChart.option = {
    title: {
        text: "",
        subtext: "",
        x: "center"
    },
    grid: { x: 50, y: 10, x2: 50, y2: 40 },
    tooltip: {
        trigger: "axis",
        backgroundColor: 'rgba(230,230,230,0.8)',
        textStyle: { color: '#525252' },
        formatter: function (param) {
            //console.log('tooltip:',param[0]);
            var time = param[0].name;
            var rowData = data.lineChart.data[time];
            if (rowData == null) return false;
            var cColor = rowData.diff_money > 0 ? '#f24957' : '#1dbf60';
            if (rowData.diff_money)
                return [
                    '时间: ' + param[0].name + '<hr size=1 style="margin: 3px 0;border-top: 1px solid #ffffff;">',
                    '当前价: <span style="color:' + cColor + ';">' + rowData.now_price + '</span><br/>',
                    '涨跌幅: <span style="color:' + cColor + ';">' + rowData.diff_rate + '</span><br/>'
                ].join('');
        }
    },
    //"legend": {
    //    x: "left",
    //    data: [""]
    //},
    xAxis: [{
        type: "category",
        axisLabel: { //坐标轴文本标签
            interval: 14,  //分14段
            textStyle: { color: '#999999' }
            ,
            formatter: function (value) {
                if (value == '09:30') return '09:30';
                if (value == '15:00') return '15:00';
                if (value == '11:30') return '11:30/13:00';
                return '';
            }
        },
        axisTick: {
            show: false
        },
        splitLine: false,
        axisLine: {
            lineStyle: {
                color: '#DCE4EA',
                width: 1,
                type: 'solid'
            }
        },
        data: []
    }
    //, {
    //    type: "value",
    //    axisLabel: {
    //        show:false
    //    },
    //    axisLine: {
    //        lineStyle: {
    //            color: '#DCE4EA',
    //            width: 1,
    //            type: 'solid'
    //        }
    //    },
    //    axisTick: {
    //        show: false
    //    },
    //    splitLine: false
    //}
    ],
    yAxis: [{
        type: "value",
        axisLabel: { //坐标轴文本标签
            textStyle: { color: '#999999' }
            //,
            //formatter: function (value) {
            //    if (value == data.lineChart.yMax) return data.lineChart.yMax;
            //    if (value == data.lineChart.yMin) return data.lineChart.yMin;
            //}
        },
        axisLine: {
            lineStyle: {
                color: '#DCE4EA',
                width: 1,
                type: 'solid'
            }
        },
        axisTick: {
            show: false
        },
        splitLine: false
        //name: " % "
    }
    //,
    //    {
    //        type: "value",
    //        axisLabel: {
    //            show: false
    //        },
    //        axisLine: {
    //            lineStyle: {
    //                color: '#DCE4EA',
    //                width: 1,
    //                type: 'solid'
    //            }
    //        },
    //        axisTick: {
    //            show: false
    //        },
    //        splitLine: false
    //    }
    ],
    series: [{
        name: "沪指指数",
        type: "line",
        symbol: 'none',
        yAxisIndex: 0,
        itemStyle: { normal: { color: '#A9D4F3' } },
        data: []
        //,
        //markLine: {
        //    data: [
        //        { name: '标线1起点', xAxis: -1, itemStyle: { normal: { color: '#dc143c' } } }
        //    ]
        //}
    }]
};
$(function () {
    data.myChart = echarts.init(document.getElementById('main'));
    FormatByDate();
    //console.log(data.lineChart.timeData);
    data.lineChart.option.xAxis[0].data = data.lineChart.timeData;
    //data.lineChart.option.series[0].markLine.data[0].xAxis = data.lineChart.timeData[data.lineChart.timeData.length-1];

    getStockList();
    data.lineChart.interval = setInterval(function () {
        getStockList();
    }, 60000);
});
function getStockList() {
    $.ajax({
        type: 'POST',
        url: '/serv/api/stockhistory/list.ashx',
        data: { code: 'sh000001' },
        dataType: 'json',
        success: function (result) {
            if (result.status) {
                if (result.result.length == 0) return;
                var lastIndex = result.result.length - 1;
                $('#yestodayClosePrice').text(Math.round(result.result[lastIndex].yestoday_close_price * 100) / 100);
                if (result.result[lastIndex].today_open_price > result.result[lastIndex].yestoday_close_price) {
                    $('#todayOpenPrice').addClass('cRed');
                    $('#todayOpenPrice').removeClass('cGreen');
                } else {
                    $('#todayOpenPrice').addClass('cGreen');
                    $('#todayOpenPrice').removeClass('cRed');
                }
                $('#todayOpenPrice').text(Math.round(result.result[lastIndex].today_open_price * 100) / 100);
                if (result.result[lastIndex].max_price > result.result[lastIndex].yestoday_close_price) {
                    $('#maxPrice').addClass('cRed');
                    $('#maxPrice').removeClass('cGreen');
                } else {
                    $('#maxPrice').addClass('cGreen');
                    $('#maxPrice').removeClass('cRed');
                }
                $('#maxPrice').text(Math.round(result.result[lastIndex].max_price * 100) / 100);
                if (result.result[lastIndex].min_price > result.result[lastIndex].yestoday_close_price) {
                    $('#minPrice').addClass('cRed');
                    $('#minPrice').removeClass('cGreen');
                } else {
                    $('#minPrice').addClass('cGreen');
                    $('#minPrice').removeClass('cRed');
                }
                $('#minPrice').text(Math.round(result.result[lastIndex].min_price * 100) / 100);
                var timeArr = result.result[lastIndex].time.split(/[- :]/);
                $('#nowTime').text(new Date(timeArr[0], timeArr[1] - 1, timeArr[2], timeArr[3], timeArr[4], timeArr[5]).format('yyyy-MM-dd hh:mm:ss') + ' 更新');
                if (result.result[lastIndex].diff_money > 0) {
                    $('#nowDiff').addClass('cRed');
                    $('#nowDiff').removeClass('cGreen');
                    $('#nowPrice').addClass('cRed');
                    $('#nowPrice').removeClass('cGreen');
                    $('#nowDiff').text('+' + Math.round(result.result[lastIndex].diff_money * 100) / 100 + ' (+' + Math.round(result.result[lastIndex].diff_rate * 100) / 100 + '%)');
                } else {
                    $('#nowDiff').addClass('cGreen');
                    $('#nowDiff').removeClass('cRed');
                    $('#nowPrice').addClass('cGreen');
                    $('#nowPrice').removeClass('cRed');
                    $('#nowDiff').text(Math.round(result.result[lastIndex].diff_money * 100) / 100 + ' (' + Math.round(result.result[lastIndex].diff_rate * 100) / 100 + '%)');
                }
                $('#nowPrice').text(Math.round(result.result[lastIndex].now_price * 100) / 100);

                if (result.result[lastIndex].trade_num > 100000000) {
                    $('#tradeNum').text(Math.round(result.result[lastIndex].trade_num / 100000000 * 100) / 100 + ' 亿手');
                } else if (result.result[lastIndex].trade_num > 10000) {
                    $('#tradeNum').text(Math.round(result.result[lastIndex].trade_num / 10000 * 100) / 100 + ' 万手');
                } else {
                    $('#tradeNum').text(Math.round(result.result[lastIndex].trade_num * 100) / 100 + ' 手');
                }

                if (result.result[lastIndex].trade_amount > 100000000) {
                    $('#tradeAmount').text(Math.round(result.result[lastIndex].trade_amount / 100000000 * 100) / 100 + ' 亿');
                } else if (result.result[lastIndex].trade_amount > 10000) {
                    $('#tradeAmount').text(Math.round(result.result[lastIndex].trade_amount / 10000 * 100) / 100 + ' 万');
                } else {
                    $('#tradeAmount').text(Math.round(result.result[lastIndex].trade_amount * 100) / 100 + ' 元');
                }

                FormatByData(result.result);
                data.lineChart.yMin = Math.floor(Math.min.apply(null, data.lineChart.numData));
                data.lineChart.yMax = Math.ceil(Math.max.apply(null, data.lineChart.numData));
                data.lineChart.option.yAxis[0].max = data.lineChart.yMax;
                data.lineChart.option.yAxis[0].min = data.lineChart.yMin;
                data.lineChart.option.series[0].data = data.lineChart.seriesData;
                data.myChart.setOption(data.lineChart.option, true);
            } else {
                //alert('数据出错,请联系管理员', 5);
            }
        }
    });
}
function initData() {
    data.lineChart.data = {};
    data.lineChart.seriesData = [];
    data.lineChart.numData = [];
    data.lineChart.inData = false;
}
function FormatByData(result) {
    initData();
    if (result.length == 0) return;
    var timeArr0 = result[0].time.split(/[- :]/);
    var timeArr1 = result[result.length - 1].time.split(/[- :]/);
    var time0 = new Date(timeArr0[0], timeArr0[1] - 1, timeArr0[2], timeArr0[3], timeArr0[4], timeArr0[5]);
    var time1 = new Date(timeArr1[0], timeArr1[1] - 1, timeArr1[2], timeArr1[3], timeArr1[4], timeArr1[5]);
    data.lineChart.startTime = time0.format('hh:mm');
    data.lineChart.endTime = time1.format('hh:mm');

    for (var i = 0; i < result.length; i++) {
        var tempTimeArr = result[i].time.split(/[- :]/);
        var rowDate = new Date(tempTimeArr[0], tempTimeArr[1] - 1, tempTimeArr[2], tempTimeArr[3], tempTimeArr[4], tempTimeArr[5]);

        if (rowDate > new Date(rowDate.getFullYear(), rowDate.getMonth(), rowDate.getDate(), 11, 30)
            && rowDate <= new Date(rowDate.getFullYear(), rowDate.getMonth(), rowDate.getDate(), 13)) continue;

        var time = rowDate.format('hh:mm');
        data.lineChart.data[time] = {
            trade_num: result[i].trade_num,
            now_price: Math.round(result[i].now_price * 100) / 100,
            diff_money: Math.round(result[i].diff_money * 100) / 100,
            diff_rate: (Math.round(result[i].diff_rate * 100) / 100) + '%'
        }
    }
    var currDate = new Date();
    var startTime = new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate(), 9, 15);
    var endTime = new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate(), 15);
    for (var i = startTime; i <= endTime; i.setTime(i.getTime() + 1000 * 60)) {
        if (i > new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate(), 11, 30)
            && i <= new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate(), 13)) continue;
        var time = i.format('hh:mm');
        //data.lineChart.timeData.push(time);
        var rowData = data.lineChart.data[time];
        if (rowData == null) {
            data.lineChart.seriesData.push('-');
        } else {
            data.lineChart.numData.push(rowData.now_price);
            data.lineChart.seriesData.push(rowData.now_price);
        }
    }
}
function FormatByDate() {
    var currDate = new Date();
    var times = [];
    var startTime = new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate(), 9, 15);
    var endTime = new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate(), 15);
    for (var i = startTime; i <= endTime; i.setTime(i.getTime() + 1000 * 60)) {
        if (i > new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate(), 11, 30)
            && i <= new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate(), 13)) continue;
        var time = i.format('hh:mm');
        data.lineChart.timeData.push(time);
    }
}