
//股权交易
var pager1 = null;
var pager2 = null;
var bs_sk_buy = { pageIndex: 1, pageSize: 6, type: 'Stock', cateId: catedata.buy, order_all: 'Sort desc, JuActivityID desc', total: 1 };
var bs_sk_sell = { pageIndex: 1, pageSize: 6, type: 'Stock', cateId: catedata.sell, order_all: 'Sort desc,JuActivityID desc', list: [], total: 1 };
var bs_stock_item;
$(function () {
    //头部链接选中行情
    setHeadSelected(2);

    bs_stock_item = $('.stock-item').clone();
    $('.body-content').html('');
    data.bug_stock = $.extend({}, bs_sk_buy);
    data.sell_stock = $.extend({}, bs_sk_sell);


    loadStock(data.bug_stock, true, 1, true);

    $('.pager1').show();
    $('.pager2').hide();
    //单击买入
    $('#buy').click(function () {
        $(this).addClass('key1');
        $('#sell').removeClass('key1');
        $('#sell').addClass('key3');
        $('.pager2').hide();
        $('.pager1').show();
        $('.body-content').html('');
        checkData('bug_stock', 1);
    });

    //单击卖出
    $('#sell').click(function () {
        $(this).addClass('key1');
        $('#buy').removeClass('key1');
        $('#buy').addClass('key3');
        $('.pager1').hide();
        $('.pager2').show();
        $('.body-content').html('');
        checkData('sell_stock', 2);
    });
    $(document).on('click', '.btn-send-notice', function () {
        var id = $(this).attr('data-id');
        var title = $(this).text();
        var stockTitle = $(this).parent().parent().find('.title').text();
        showDialogSendNotice(notice_price, id, title, false, stockTitle);
    });
});

function loadStock(obData, showProgress, num, showView) {
    GetContent(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            obData["page" + obData.pageIndex] = result.list;
            if (num) bindPage(obData.total, obData.pageSize, num);
            if (showView) viewStock(result.list, obData.pageIndex);
        });
}

function bindPage(total, rows, num) {
    if (num == 1 && total > rows) {
        pager1 = $.pager({
            count: total,
            nums: rows,
            numsOption: [rows],
            pageContainer: '.pager' + num,
            datakey: 'bug_stock',
            onchange: function () {
                changePage(this);
            }
        });
    } else if (num == 2 && total > rows) {
        pager2 = $.pager({
            count: total,
            nums: rows,
            numsOption: [rows],
            pageContainer: '.pager' + num,
            datakey: 'sell_stock',
            onchange: function () {
                changePage(this);
            }
        });
    }
}
function changePage(ob) {
    data[ob.datakey].pageIndex = ob.page;
    $('.body-content').html('');
    checkData(ob.datakey, null);
}
function checkData(dataKey, num) {
    var obData = data[dataKey];
    var list = obData['page' + obData.pageIndex];
    if (!list) {
        loadStock(obData, true, num, true);
    } else {
        viewStock(list, obData.pageIndex);
    }
}
function viewStock(list, page) {
    var listHtml = new StringBuilder();
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(bs_stock_item).clone();

            newItem.find('.body-content-border img').attr('src', list[i].imgSrc);
            newItem.find('.stock-title span').text(list[i].title);
            newItem.find('.stock-title .content-bottom').text(list[i].categoryName);
            if (list[i].categoryId == catedata.sell) {
                newItem.find('.stock-buttom .btn-send-notice').text('通知卖家');
            }
            if (!!list[i].pubUser) {
                newItem.find('.username').text(list[i].pubUser.userName);
                newItem.find('.stock-buttom .btn-send-notice').attr('data-id', list[i].pubUser.id);
                newItem.find('.username').attr('data-id', list[i].pubUser.id);
                newItem.find('.username').attr('data-nickname', list[i].pubUser.userName);
                newItem.find('.username').attr('data-avatar', list[i].pubUser.avatar);
                newItem.find('.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.username').attr('data-times', list[i].pubUser.times);
                newItem.find('.username').attr('data-describe', list[i].pubUser.describe);
            } else {
                newItem.find('.stock-buttom .btn-send-notice').hide();
                newItem.find('.stock-buttom .btn-send-notice').attr('data-id', 0);
                newItem.find('.username').text('淘股玩家');
                newItem.find('.username').attr('data-id', 0);
                newItem.find('.username').attr('data-nickname', '淘股玩家');
                newItem.find('.username').attr('data-times', 0);
                newItem.find('.username').attr('data-friend', 1);
                newItem.find('.username').attr('data-avatar', 'http://file.comeoncloud.net/img/europejobsites.png');
                newItem.find('.username').attr('data-describe', '');
            }
            newItem.find('.time').text(dateFormat(list[i].createDate));
            newItem.find('.stock-content a').text(list[i].summary);
            newItem.find('.stock-title span,.stock-title .btn,.stock-content a,.body-content-img').attr('data-id', list[i].id);
            newItem.find('.stock-title span,.stock-title .btn,.stock-content a,.body-content-img').click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.stocknum').text(list[i].k3);
            newItem.show();
            $('.body-content').append(newItem);
        }
    } else if (page == 1) {
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.body-content').append(listHtml.ToString());
    }
}
