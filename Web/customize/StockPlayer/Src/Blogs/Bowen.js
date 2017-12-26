//博文

var pager1 = null;
var pager2 = null;
var bs_bowen = { pageIndex: 1, pageSize: 5, type: 'Bowen', order_all: 'Sort desc, JuActivityID desc', total: 1 };
var bs_rm_bowen = { pageIndex: 1, pageSize: 5, type: 'Bowen', order_all: 'Sort desc, PraiseCount desc,JuActivityID desc', list: [], total: 1 };
var bs_bowen_item;
$(function () {
    //头部链接选中行情
    setHeadSelected(1);
    bs_bowen_item = $('.blogs-item').clone();
    $('.index-blogs').html('');

    data.bowen = $.extend({}, bs_bowen);
    data.rm_bowen = $.extend({}, bs_rm_bowen);
    loadBowen(data.bowen, true, 1, true);
    $('.pager2').hide();
    $('.pager1').show();
    //单击热门排序

    $('#REMEN').click(function () {
        $(this).addClass('key1');
        $(XINFABU).removeClass('key1');
        $(XINFABU).addClass('key3');
        $('.pager1').hide();
        $('.pager2').show();
        $('#keyword').val($.trim(data.rm_bowen.keyword));
        $('.index-blogs').html('');
        checkData('rm_bowen', 2);
    });

    //单击新发布排序
    $('#XINFABU').click(function () {
        $(this).addClass('key1');
        $(REMEN).removeClass('key1');
        $(REMEN).addClass('key3');
        $('.pager2').hide();
        $('.pager1').show();
        $('#keyword').val($.trim(data.bowen.keyword));
        $('.index-blogs').html('');
        checkData('bowen', 1);
    });

    //搜索
    $('#Search').click(function () {
        var val = $.trim($('#keyword').val());
        val = val.replace(/\'/g, "''");
        if ($(REMEN).hasClass('key1')) {
            if ((!data.rm_bowen.keyword && val == '') ||
                (data.rm_bowen.keyword && val == data.rm_bowen.keyword)) return;
            pager2 = null;
            data.rm_bowen = $.extend({}, bs_rm_bowen);
            data.rm_bowen.keyword = val;
            if (data.rm_bowen.keyword != '') {
                data.rm_bowen.keyword_author = '1';
            }
            $('.index-blogs').html('');
            $('.pager2').html('');
            loadBowen(data.rm_bowen, true, 2, true);
        } else {
            if ((!data.bowen.keyword && val == '') ||
               (data.bowen.keyword && val == data.bowen.keyword)) return;
            pager1 = null;
            data.bowen = $.extend({}, bs_bowen);
            data.bowen.keyword = val;
            if (data.bowen.keyword != '') {
                data.bowen.keyword_author = '1';
            }
            $('.index-blogs').html('');
            $('.pager1').html('');
            loadBowen(data.bowen, true, 1, true);
        }
    });
});

function loadBowen(obData, showProgress, num, showView) {
    GetContent(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            obData["page" + obData.pageIndex] = result.list;
            if (num) bindPage(obData.total, obData.pageSize, num);
            if (showView) viewBowen(result.list, obData.pageIndex);
        });
}
function bindPage(total, rows, num) {
    if (num == 1 && total > rows) {
        pager1 = $.pager({
            count: total,
            nums: rows,
            numsOption: [rows],
            pageContainer: '.pager' + num,
            datakey: 'bowen',
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
            datakey: 'rm_bowen',
            onchange: function () {
                changePage(this);
            }
        });
    }
}
function changePage(ob) {
    data[ob.datakey].pageIndex = ob.page;
    $('.index-blogs').html('');
    checkData(ob.datakey, null);
}
function checkData(dataKey, num) {
    var obData = data[dataKey];
    var list = obData['page' + obData.pageIndex];
    if (!list) {
        loadBowen(obData, true, num, true);
    } else {
        viewBowen(list, obData.pageIndex);
    }
}
function viewBowen(list, page) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(bs_bowen_item).clone();
            newItem.find('.index-bowen-item img').attr('data-id', list[i].id);
            newItem.find('.index-bowen-item img').attr('src', list[i].imgSrc).click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });;
            newItem.find('.head').text(list[i].title);
            if (!!list[i].pubUser) {
                newItem.find('.userimg,.username').attr('data-id', list[i].pubUser.id);
                newItem.find('.userimg,.username').attr('data-nickname', list[i].pubUser.userName);
                newItem.find('.userimg,.username').attr('data-avatar', list[i].pubUser.avatar);
                newItem.find('.userimg img').attr('src', list[i].pubUser.avatar);
                newItem.find('.userimg,.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.userimg,.username').attr('data-times', list[i].pubUser.times);
                newItem.find('.userimg,.username').attr('data-describe', list[i].pubUser.describe);
                newItem.find('.username').text(list[i].pubUser.userName);
            } else {
                newItem.find('.username').text('淘股玩家');
                newItem.find('.userimg img').attr('src', 'http://file.comeoncloud.net/img/europejobsites.png');
                newItem.find('.userimg,.username').attr('data-id', 0);
                newItem.find('.userimg,.username').attr('data-nickname', '淘股玩家');
                newItem.find('.userimg,.username').attr('data-times', 0);
                newItem.find('.userimg,.username').attr('data-friend', 1);
                newItem.find('.userimg,.username').attr('data-avatar', 'http://file.comeoncloud.net/img/europejobsites.png');
                newItem.find('.userimg,.username').attr('data-describe', '');
            }

            newItem.find('.time').text(dateFormat(list[i].createDate));
            newItem.find('.comment span').text(list[i].commentCount);
            newItem.find('.zan span').text(list[i].praiseCount);
            newItem.find('.score span').text(list[i].rewardTotal);
            newItem.find('.buttom a').text(list[i].summary);
            newItem.find('.buttom a,.head').attr('data-id', list[i].id);
            newItem.find('.buttom a,.head').click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.show();
            $('.index-blogs').append(newItem);
        }
    } else if (page == 1) {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.index-blogs').append(listHtml.ToString());
    }
}