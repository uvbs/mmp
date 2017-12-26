
var noticeItem
$(function () {
    data.notice = { rows: 10, page: 1, auto_read: 1, list: [], total: 1 };
    noticeItem = $('.list-notice .notice-item').clone();
    $('.list-notice').html('');

    loadNotice(data.notice, true, true);
});

function GetNotice(option, showProgress, fn) {
    var layerIndex;
    if (showProgress) layerIndex = progress();
    var baseOption = {
        rows: 10,
        page: 1,
        auto_read: 1
    };
    var option = $.extend(baseOption, option);
    $.ajax({
        type: 'post',
        url: "/Serv/API/SystemNotice/List.ashx",
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
function loadNotice(obData, showProgress, bindPage) {
    GetNotice(
        obData,
        showProgress,
        function (result) {
            obData.total = result.total;
            obData["page" + obData.page] = result.list;
            if (bindPage) {
                $('.pager1').html('');
                if (obData.total > obData.rows) {
                    $.pager({
                        count: obData.total, nums: obData.rows, numsOption: [obData.rows],
                        page: obData.page,
                        pageContainer: '.pager1', onchange: function () {
                            data.notice.page = this.page;
                            $('.list-notice').html('');
                            var obData = data.notice;
                            var list = obData['page' + this.page];
                            if (!list) {
                                loadNotice(obData, true, false);
                            } else {
                                viewNotice(list);
                            }
                        }
                    });
                }
            }
            viewNotice(result.list);
        });
}
function viewNotice(list) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(noticeItem).clone();
            newItem.find('.head .time').text(dateFormat(list[i].time));
            if (!list[i].read) newItem.find('.head').append('<div class="new">新消息</div>');
            newItem.find('.content').append(list[i].content);
            newItem.show();
            $('.list-notice').append(newItem);
        }
    }
}