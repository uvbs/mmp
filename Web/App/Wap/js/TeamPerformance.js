var curDate = new Date();
var commVm = new Vue({
    el: '.wrapComm',
    data: {
        my: false,
        list: [],
        years: [],
        months: [
            { value: '01', text: '1月' },
            { value: '02', text: '2月' },
            { value: '03', text: '3月' },
            { value: '04', text: '4月' },
            { value: '05', text: '5月' },
            { value: '06', text: '6月' },
            { value: '07', text: '7月' },
            { value: '08', text: '8月' },
            { value: '09', text: '9月' },
            { value: '10', text: '10月' },
            { value: '11', text: '11月' },
            { value: '12', text: '12月' }
        ],
        year: curDate.format("yyyy"),
        yearText: curDate.format("yyyy") + '年',
        month: curDate.format("MM"),
        monthText: curDate.format("M") + '月',
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            this.loadData();
            this.initYears();
        },
        initYears:function(){
            var year = Number(new Date().format("yyyy"));
            var minYear = 2016;
            for (var i = year; i >= minYear; i--) {
                this.years.push({ value: i, text:i + '年' });
            }
        },
        showYear: function () {
            $(this.$el).find('.comm-dialog.year-dialog').css('display', 'block');
        },
        selectYear: function (item) {
            if (item.value != this.year) {
                this.year = item.value;
                this.yearText = item.text;
                this.loadData();
            }
            $(this.$el).find('.comm-dialog.year-dialog').css('display', 'none');
        },
        showMonth: function () {
            $(this.$el).find('.comm-dialog.month-dialog').css('display', 'block');
        },
        selectMonth: function (item) {
            if (item.value != this.month) {
                this.month = item.value;
                this.monthText = item.text;
                this.loadData();
            }
            $(this.$el).find('.comm-dialog.month-dialog').css('display', 'none');
        },
        clearDialog: function () {
            $(this.$el).find('.comm-dialog').css('display', 'none');
        },
        loadData: function () {
            var _this = this;
            $('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/Serv/API/Distribution/GetPerformanceList.ashx',
                data: {
                    yearmonth: _this.year + '' + _this.month
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    if (resp.status) {
                        if (resp.result.my.act_status == 0) {
                            resp.result.my.act_status_name = "处理中"
                        } else if (resp.result.my.act_status == 9) {
                            resp.result.my.act_status_name = "已发放"
                        }
                        _this.my = resp.result.my;
                        _this.list = resp.result.list;
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        goApplyPerformanceReward: function () {
            window.location.href = '/app/wap/ApplyPerformanceReward.aspx?id=' + this.my.id;
        },
        buildExcel: function () {
            var _this = this;
            $('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/serv/api/Performance/ConfrimFormExport.ashx',
                data: {
                    yearmonth: _this.year + '' + _this.month
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    if (resp.status) {
                        window.location.href = '/Serv/API/Common/ExportFromRedis.ashx?cache=' + resp.result.cache;
                        //$('#exportIframe').attr('src', '/Serv/API/Common/ExportFromRedis.ashx?cache=' + resp.result.cache);
                    } else {
                        zcAlert('生成出错');
                    }
                },
                error: function () {
                    $('#loadingToast').hide();
                }
            });
        }
    }
});

$(function () {
    commVm.init();
});