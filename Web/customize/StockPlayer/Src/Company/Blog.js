
//公司发布
var pager = null;
var bs_publish = { pageIndex: 1, pageSize: 5, type: 'CompanyPublish', order_all: 'Sort desc, JuActivityID desc', total: 1 };
var bs_publish_item;

$(function () {
    //头部链接选中行情
    setHeadSelected(3);

    bs_publish_item = $('.blogs-item').clone();
    $('.index-blogs').html('');
    data.publish = $.extend({}, bs_publish);
    loadPublish(data.publish, true, true);


    $('.blog-head-button').click(function () {
        var val = $.trim($('#keyword').val());
        val = val.replace(/\'/g, "''");
        if ((!data.publish.keyword && val == '') ||
                (data.publish.keyword && val == data.publish.keyword)) return;
        pager = null;
        data.publish = $.extend({}, bs_publish);
        data.publish.keyword = val;
        if (data.publish.keyword != '') {
            data.publish.keyword_author = '1';
        }
        $('.index-blogs').html('');
        $('.pager').html('');
        loadPublish(data.publish, true, true);
    })


});


function loadPublish(obData, showProgress, toBindPage) {
    GetContent(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            obData["page" + obData.pageIndex] = result.list;
            if (toBindPage) bindPage(obData.total, obData.pageSize);
            viewPublish(result.list, obData.pageIndex);
        });
}


function bindPage(total, rows) {
    if ( total > rows) {
        pager = $.pager({
            count: total,
            nums: rows,
            numsOption: [rows],
            pageContainer: '.pager',
            datakey: 'publish',
            onchange: function () {
                changePage(this);
            }
        });
    }
}

function changePage(ob) {
    data[ob.datakey].pageIndex = ob.page;
    $('.index-blogs').html('');
    checkData(ob.datakey);
}
function checkData(dataKey) {
    var obData = data[dataKey];
    var list = obData['page' + obData.pageIndex];
    if (!list) {
        loadPublish(obData, true, false);
    } else {
        viewPublish(list, obData.pageIndex);
    }
}



function viewPublish(list, page) {
    var listHtml = new StringBuilder();
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {

            var newItem = $(bs_publish_item).clone();

            newItem.find('.index-bowen-item img').attr({ 'src': list[i].imgSrc, 'data-id': list[i].id }).click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.head').text(list[i].title);
            if (!!list[i].pubUser) {
                newItem.find('.userimg img').attr('src', list[i].pubUser.avatar);
                newItem.find('.company-name,.username').text(list[i].pubUser.userName);
                //newItem.find('.userimg,.username').attr('data-id', list[i].pubUser.id);
                //newItem.find('.userimg,.username').attr('data-nickname', list[i].pubUser.userName);
                //newItem.find('.userimg,.username').attr('data-avatar', list[i].pubUser.avatar);
                //newItem.find('.userimg,.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.userimg,.head-img,.username').attr('data-id', list[i].pubUser.id);
                newItem.find('.userimg,.head-img,.username').attr('data-nickname', list[i].pubUser.userName);
                newItem.find('.userimg,.head-img,.username').attr('data-avatar', list[i].pubUser.avatar);
                newItem.find('.userimg,.head-img,.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.userimg,.head-img,.username').attr('data-times', list[i].pubUser.times);
                newItem.find('.userimg,.head-img,.username').attr('data-describe', list[i].pubUser.describe);
            } else {
                newItem.find('.company-name,.username').text('淘股玩家');
                newItem.find('.userimg img').attr('src', 'http://file.comeoncloud.net/img/europejobsites.png');
                newItem.find('.userimg,.head-img,.username').attr('data-id', 0);
                newItem.find('.userimg,.head-img,.username').attr('data-nickname', '淘股玩家');
                newItem.find('.userimg,.head-img,.username').attr('data-friend', 1);
                newItem.find('.userimg,.head-img,.username').attr('data-times', 0);
                newItem.find('.userimg,.head-img,.username').attr('data-avatar', 'http://file.comeoncloud.net/img/europejobsites.png');
                newItem.find('.userimg,.head-img,.username').attr('data-describe', '');
            }

            newItem.find('.time').text(dateFormat(list[i].createDate));

            newItem.find('.comment span').text(list[i].commentCount);
            newItem.find('.zan span').text(list[i].praiseCount);
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
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.index-blogs').append(listHtml.ToString());
    }
}