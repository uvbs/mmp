
var cashItem;
$(function () {
    setHeadSelected(4);
    data.cash = { pagesize:10,pageindex:1,list: [], total: 1 };

    cashItem = $('.list-notice .notice-item').clone();
    $('.list-notice').html('');

    loadCashs(data.cash, true, true);

    $('.lbTip').click(function () {
        var msg = $(this).attr('data-tip-msg');
        layer.tips(msg, '.lbTip');
    });

    $(document).on('click', '.warp-tj span', function () {
        var num = $.trim($(this).text());
        var r_num = parseFloat(num) * rc_num / 100;
        var z_num = $.trim($('.warp-score').text());
        $('.warp-money').text(r_num);
        $('.recharge-num').val(num);
        $('.warp-sscore').text((parseFloat(z_num) - parseFloat(num)));
        $('.warp-tj .selected').removeClass('selected');
        $(this).addClass('selected');
    });
    $(document).on('change', '.recharge-num', function () {
        var num = $.trim($(this).val());
        var r_num = parseFloat(num) * rc_num / 100;
        var z_num = $.trim($('.warp-score').text());
        $('.warp-money').text(r_num);
        $('.warp-sscore').text((parseFloat(z_num) - parseFloat(num)));
        $('.warp-tj .selected').removeClass('selected');
    });
    $(document).on('click', '.btn-apply', function () {
        var num = $.trim($('.recharge-num').val());
        if (parseFloat(num) < min_cashscore) {
            alert('最少需提现' + min_cashscore+'淘股币');
            return;
        }
        var r_num = parseFloat(num) * rc_num / 100;
        var z_num = $.trim($('.warp-score').text());
        var s_num = (parseFloat(z_num) - parseFloat(num));
        if (s_num < min_score) {
            alert('剩余淘股币不能少于' + min_score + '淘股币');
            return;
        }
        window.confirm('淘股币提现', '确认使用' + num + '淘股币，提现' + r_num + '元？', '确认申请', '取消', function (layerIndex, layero) {
            if ($(layero).find('.layui-layer-btn0').hasClass('btn-disabled')) {
                return;
            }
            $(layero).find('.layui-layer-btn0').text('正在提交...');
            $(layero).find('.layui-layer-btn0').addClass('btn-disabled');
            $.ajax({
                type: 'post',
                url: '/Serv/API/Score/ApplyWithdrawCash.ashx',
                data: { score: num, module_name: '淘股币' },
                dataType: 'JSON',
                success: function (resp) {
                    $(layero).find('.layui-layer-btn0').text('确认申请');
                    $(layero).find('.layui-layer-btn0').removeClass('btn-disabled');
                    if (resp.status) {
                        var n_z_num = $('.warp-sscore').text();
                        $('.warp-score').text(n_z_num);
                        var n_num = $.trim($('.recharge-num').val());
                        $('.warp-sscore').text((parseFloat(n_z_num) - parseFloat(n_num)));
                        //构造第一页数据
                        if (!data.cash.page1) data.cash.page1 = [];
                        data.cash.page1.unshift({ status: '待审核', amount: r_num, score: num, time: Date.parse(new Date()) });

                        if (!!data.pager) {
                            data.pager.page = 1;
                            data.pager.onchange();
                        }

                        layer.close(layerIndex);
                        alert('你的提现申请已提交，等待审核通过（一般不超过24小时）', 6);
                    } else {
                        alert(resp.msg, 5);
                    }
                }
            });
        }, function () { });
    });
});
function GetCashs(option, showProgress, fn) {
    var layerIndex;
    if (showProgress) layerIndex = progress();
    var baseOption = {
        pagesize: 10,
        pageindex: 1,
        type: 'ScoreOnLine'
    };
    var option = $.extend(baseOption, option);
    $.ajax({
        type: 'post',
        url: "/Serv/API/Score/GetWithdrawCashList.ashx",
        data: option,
        dataType: "json",
        success: function (resp) {
            if (showProgress) layer.close(layerIndex);
            if (resp.status) {
                fn(resp.result);
            } else {
                alert('查询出错');
            }
        }
    });
}
function loadCashs(obData, showProgress, bindPage) {
    GetCashs(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            obData["page" + obData.pageindex] = result.list;
            if (bindPage) {
                $('.pager1').html('');
                if (obData.total > obData.pagesize) {
                    data.pager = $.pager({
                        count: obData.total, nums: obData.pagesize, numsOption: [obData.pagesize],
                        page: obData.pageindex,
                        pageContainer: '.pager1', onchange: function () {
                            data.cash.pageindex = this.page;
                            $('.list-notice').html('');
                            var obData = data.cash;
                            var list = obData['page' + this.page];
                            if (!list) {
                                loadCashs(obData, true, false);
                            } else {
                                viewCashs(list);
                            }
                        }
                    });
                }
            }
            viewCashs(result.list);
        });
}
function viewCashs(list) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(cashItem).clone();
            newItem.find('.head .autoid').text(list[i].id);
            newItem.find('.head .time').text(dateFormat(list[i].time));
            if (list[i].status == "待审核" || list[i].status == "已受理") newItem.find('.head').append('<div class="new">' + list[i].status + '</div>');
            if (list[i].status == "审核不通过") newItem.find('.head').append('<div class="ref">' + list[i].status + '</div>');
            if (list[i].status == "审核通过") newItem.find('.head').append('<div class="pass">' + list[i].status + '</div>');
            var str = '消耗' + list[i].score + '，申请提现' + list[i].amount + '元';
            newItem.find('.content').append(str);
            newItem.show();
            $('.list-notice').append(newItem);
        }
    }
}