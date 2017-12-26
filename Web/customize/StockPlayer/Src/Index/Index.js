
var tempTime = new Date();
var startTime1 = new Date(tempTime.getFullYear(), tempTime.getMonth(), tempTime.getDate());
var startTime2 = new Date(tempTime.getFullYear(), tempTime.getMonth(), tempTime.getDate(), 10);
var startTime3 = new Date(tempTime.getFullYear(), tempTime.getMonth(), tempTime.getDate(), 13);

var curDay = tempTime.format('yyyy/MM/dd');
var commentStartDay = new Date(new Date().setDate(tempTime.getDate() - 6)).format('yyyy/MM/dd');
var bowenStartDay = new Date(new Date().setDate(tempTime.getDate() - 2)).format('yyyy/MM/dd');
var bs_bowen_item;
var bs_comment_item;
$(function () {
    //头部链接选中行情
    setHeadSelected(0);
    bs_bowen_item = $('.index-bowen .index-bowen-item').clone();
    bs_comment_item = $('.index-comment .index-comment-item').clone();
    $('.index-bowen').html('');
    $('.index-comment').html('');
    data.zaop = { pageIndex: 1, cateId: catedata.zaop, create_start: commentStartDay, list: [], total: 1, order_all: 'Sort desc,JuActivityID desc' };
    data.wup = { pageIndex: 1, cateId: catedata.wup, create_start: commentStartDay, list: [], total: 1, order_all: 'Sort desc,JuActivityID desc' };
    data.wanp = { pageIndex: 1, cateId: catedata.wanp, create_start: commentStartDay, list: [], total: 1, order_all: 'Sort desc,JuActivityID desc' };

    data.ds_zaop = { pageIndex: 1, cateId: catedata.zaop, order_all: 'Sort desc, PraiseCount desc,JuActivityID desc', create_start: commentStartDay, list: [], total: 1 };
    data.ds_wup = { pageIndex: 1, cateId: catedata.wup, order_all: 'Sort desc, PraiseCount desc,JuActivityID desc', create_start: commentStartDay, list: [], total: 1 };
    data.ds_wanp = { pageIndex: 1, cateId: catedata.wanp, order_all: 'Sort desc, PraiseCount desc,JuActivityID desc', create_start: commentStartDay, list: [], total: 1 };

    data.bowen = { pageIndex: 1, type: 'Bowen', order_all: 'Sort desc, PraiseCount desc,JuActivityID desc', create_start: bowenStartDay, list: [], total: 1 };

    data.notice = { pageIndex: 1, pageSize: 8, type: 'Notice', order_all: 'Sort desc,JuActivityID desc', list: [], total: 1 };
   
    var type1 ;
    var type2 ;
    if (tempTime < startTime2) {
        $('#zao').removeClass('tdGray');
        $('#zao').addClass('tdYellow');
        $('#wan').removeClass('tdYellow');
        $('#wan').addClass('tdGray');
        $('#wu').removeClass('tdYellow');
        $('#wu').addClass('tdGray');
        type1 = data.zaop;
    } else if (tempTime < startTime3) {
        $('#zao').removeClass('tdYellow');
        $('#zao').addClass('tdGray');
        $('#wan').removeClass('tdYellow');
        $('#wan').addClass('tdGray');
        $('#wu').removeClass('tdGray');
        $('#wu').addClass('tdYellow');
        type1 = data.wup;
    } else {
        $('#zao').removeClass('tdYellow');
        $('#zao').addClass('tdGray');
        $('#wan').removeClass('tdGray');
        $('#wan').addClass('tdYellow');
        $('#wu').removeClass('tdYellow');
        $('#wu').addClass('tdGray');
        type1 = data.wanp;
    }

    if (tempTime < startTime2) {
        $('#ds_zao').removeClass('tdGray');
        $('#ds_zao').addClass('tdYellow');
        $('#ds_wan').removeClass('tdYellow');
        $('#ds_wan').addClass('tdGray');
        $('#ds_wu').removeClass('tdYellow');
        $('#ds_wu').addClass('tdGray');
        type2 = data.ds_zaop;
    } else if (tempTime < startTime3) {
        $('#ds_zao').removeClass('tdYellow');
        $('#ds_zao').addClass('tdGray');
        $('#ds_wan').removeClass('tdYellow');
        $('#ds_wan').addClass('tdGray');
        $('#ds_wu').removeClass('tdGray');
        $('#ds_wu').addClass('tdYellow');
        type2 = data.ds_wup;
    } else {
        $('#ds_zao').removeClass('tdYellow');
        $('#ds_zao').addClass('tdGray');
        $('#ds_wan').removeClass('tdGray');
        $('#ds_wan').addClass('tdYellow');
        $('#ds_wu').removeClass('tdYellow');
        $('#ds_wu').addClass('tdGray');
        type2 = data.ds_wanp;
    }
    //初始加载数据
    loadComment(0, type1, false);
    loadComment(1, type2, false);
    loadBowen(data.bowen, true);
    loadNotice(data.notice,false);
    $('.sp-comment,.ds-comment').click(function () {
        if ($(this).hasClass('tdYellow')) return;
        var dataKey = $(this).attr('data-key');
        var tpcss = '.sp-comment';
        var num = 0;
        if ($(this).hasClass('ds-comment')) {
            tpcss = '.ds-comment';
            num = 1;
        }
        //样式变更
        $(tpcss + '.tdYellow').addClass('tdGray');
        $(tpcss + '.tdYellow').removeClass('tdYellow');
        $(this).addClass('tdYellow');
        $(this).removeClass('tdGray');
        //清数据
        $($('.index-comment')[num]).html('');

        var obData = data[dataKey];
        if (obData.list.length == 0 && obData.total > 0) {
            loadComment(num, obData, true, dataKey);
        } else {
            viewComment(num, obData.list, obData.total <= obData.list.length, obData.pageIndex, dataKey);
        }
    });

    $('.sp-more .buttonGray,.ds-more .buttonGray,.bowen-more .buttonGray').click(function () {
        var dataKey = $(this).attr('data-key');
        if (dataKey == 'bowen') {
            var obData = data[dataKey];
            obData.pageIndex++;
            loadBowen(obData, true);
        }
        else {
            var tpcss = dataKey == 'ds' ? '.ds-comment' : '.sp-comment';
            var num = dataKey == 'ds' ? 1 : 0;
            dataKey = $(tpcss + '.tdYellow').attr('data-key');
            var obData = data[dataKey];
            obData.pageIndex++;
            loadComment(num, obData, true, dataKey);
        }
    });


    $(document).on('click', '.panel-body ul li', function () {
        var id = $(this).attr('data-id');
        ToDetail(id);
    });
});
function loadNotice(obData,showProgress) {
    GetContent(
       obData,
       showProgress,
       function (result) {
           obData.total = result.totalcount;
           if (result.totalcount <= 0) {
               $('.warp-notice').hide();
           } else {
               viewNitice(result.list);
           }
       });
}

function viewNitice(list) {
    var html='<ul>';
    for (var i = 0; i < list.length; i++) {
        html+='<li data-id='+list[i].id+'>'+list[i].title+'</li>';
    }
    html += '</ul>';
    $('.panel-body').append(html);
}
//加载博文数据
function loadBowen(obData, showProgress) {
    GetContent(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            for (var i = 0; i < result.list.length; i++) {
                obData.list.push(result.list[i]);
            }
            viewBowen(result.list, obData.total <= obData.list.length, obData.pageIndex);
        });
}
function viewBowen(list, hideMore, page) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(bs_bowen_item).clone();
            newItem.find('.bowen-img').attr({ 'src': list[i].imgSrc, 'data-id': list[i].id }).click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.title').attr('data-id', list[i].id).click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.title').text(list[i].title);
            if (!!list[i].pubUser) {
                newItem.find('.userimg,.username').attr('data-id', list[i].pubUser.id);
                newItem.find('.userimg,.username').attr('data-nickname', list[i].pubUser.userName);
                newItem.find('.userimg,.username').attr('data-avatar', list[i].pubUser.avatar);
                newItem.find('.userimg,.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.userimg,.username').attr('data-times', list[i].pubUser.times);
                newItem.find('.userimg,.username').attr('data-describe', list[i].pubUser.describe);
                newItem.find('.userimg img').attr('src', list[i].pubUser.avatar);
                newItem.find('.username').text(list[i].pubUser.userName);
                newItem.find('.username').click(function () {
                    var did = $(this).attr('data-id');
                    ToUser(did);
                });
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

            newItem.find('.context a').text(list[i].summary);
            newItem.find('.context a').attr('data-id', list[i].id);
            newItem.find('.context a').click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.show();
            $('.index-bowen').append(newItem);
        }
    } else if (page == 1) {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.index-bowen').append(listHtml.ToString());
    }
    if (hideMore) $('.bowen-more').hide();
    if (!hideMore) $('.bowen-more').show();
}

//加载时评数据
function loadComment(num, obData, showProgress, dataKey) {
    var pe = $('.index-comment')[num];
    GetContent(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            for (var i = 0; i < result.list.length; i++) {
                obData.list.push(result.list[i]);
            }
            viewComment(num, result.list, obData.total <= obData.list.length, obData.pageIndex, dataKey);
        });
}
//时评数据呈现
function viewComment(num, list, hideMore, page, dataKey) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(bs_comment_item).clone();
            if (!!list[i].pubUser) {
                newItem.find('.userimg,.username').attr('data-id', list[i].pubUser.id);
                newItem.find('.userimg,.username').attr('data-nickname', list[i].pubUser.userName);
                newItem.find('.userimg,.username').attr('data-avatar', list[i].pubUser.avatar);
                newItem.find('.userimg,.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.userimg,.username').attr('data-times', list[i].pubUser.times);
                newItem.find('.userimg,.username').attr('data-describe', list[i].pubUser.describe);
                newItem.find('.userimg img').attr('src', list[i].pubUser.avatar);
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

            newItem.find('.content a').text(list[i].summary);
            newItem.find('.content a').attr('data-id', list[i].id);
            newItem.find('.content a').click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.show();
            $($('.index-comment')[num]).append(newItem);
        }
    } else if (page == 1) {
        var curTime = new Date();
        var startTime = startTime1;
        var cName = '早评'
        if (dataKey == "wup" || dataKey == "ds_wup") {
            cName = '午评'
            startTime = startTime2;
        } else if (dataKey == "wanp" || dataKey == "ds_wanp") {
            cName = '晚评'
            startTime = startTime3;
        }
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="col-xs-12 index-comment-item">');
        listHtml.AppendFormat('<div class="empty">{0}</div>', (curTime < startTime ? cName + '时间还未开始' : '暂无内容'));
        listHtml.AppendFormat('</div>');
        $($('.index-comment')[num]).append(listHtml.ToString());
    }
    if (num == 0) {
        if (hideMore) $('.sp-more').hide();
        if (!hideMore) $('.sp-more').show();
    } else {
        if (hideMore) $('.ds-more').hide();
        if (!hideMore) $('.ds-more').show();
    }
}
