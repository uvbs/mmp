
var ue;
var bs_just_item;
var bs_back_item;
var title;

$(function () {

    setHeadSelected(4);

    var toobars = ueditorToobars[0];
    var ts = [];
    var ts1 = [];
    for (var i = 0; i < toobars.length; i++) {
        if (toobars[i] == 'emotion' || toobars[i] == 'simpleupload') continue;
        ts1.push(toobars[i]);
    }
    ts.push(ts1);
    ue = UE.getEditor('just', {
        initialFrameWidth: 440,
        initialFrameHeight: 280,
        scaleEnabled: true,
        autoFloatEnabled: false,
        toolbars: ts
    });

    ue = UE.getEditor('back', {
        initialFrameWidth: 440,
        initialFrameHeight: 280,
        scaleEnabled: true,
        autoFloatEnabled: false,
        toolbars: ts
    });

    justItem = $('.duofang-item .bs_just_item').clone();
    backItem = $('.kongfang-item .bs_back_item').clone();
    $('.duofang-item').html('');
    $('.kongfang-item').html('');

    if (rootId == catedata.sroot) {
        data.ds_just = { pageIndex: 1, pageSize: 5, type: 'PupilDebate', root_id: rootId, cateId: catedata.sjust, column: 'JuActivityID,ActivityName,Summary,RewardTotal,UserID,CreateDate,CommentCount,PraiseCount', create_start: dateFormatWeek(), list: [], total: 1, order_all: 'Sort desc,JuActivityID desc' };
        data.ds_back = { pageIndex: 1, pageSize: 5, type: 'PupilDebate', root_id: rootId, cateId: catedata.sback, column: 'JuActivityID,ActivityName,Summary,RewardTotal,UserID,CreateDate,CommentCount,PraiseCount', create_start: dateFormatWeek(), list: [], total: 1, order_all: 'Sort desc,JuActivityID desc' };
        
        title = '股市周线预测（上证、深证）';
    } else if (rootId == catedata.hroot) {
        data.ds_just = { pageIndex: 1, pageSize: 5, type: 'PupilDebate', root_id: rootId, cateId: catedata.hjust, column: 'JuActivityID,ActivityName,Summary,RewardTotal,UserID,CreateDate,CommentCount,PraiseCount', create_start: dateFormatWeek(), list: [], total: 1, order_all: 'Sort desc,JuActivityID desc' };
        data.ds_back = { pageIndex: 1, pageSize: 5, type: 'PupilDebate', root_id: rootId, cateId: catedata.hback, column: 'JuActivityID,ActivityName,Summary,RewardTotal,UserID,CreateDate,CommentCount,PraiseCount', create_start: dateFormatWeek(), list: [], total: 1, order_all: 'Sort desc,JuActivityID desc' };
        title = '贵金属周线预测（黄金、白银）';
    } else {
        data.ds_just = { pageIndex: 1, pageSize: 5, type: 'PupilDebate', root_id: rootId, cateId: catedata.yjust, column: 'JuActivityID,ActivityName,Summary,RewardTotal,UserID,CreateDate,CommentCount,PraiseCount', create_start: dateFormatWeek(), list: [], total: 1, order_all: 'Sort desc,JuActivityID desc' };
        data.ds_back = { pageIndex: 1, pageSize: 5, type: 'PupilDebate', root_id: rootId, cateId: catedata.yback, column: 'JuActivityID,ActivityName,Summary,RewardTotal,UserID,CreateDate,CommentCount,PraiseCount', create_start: dateFormatWeek(), list: [], total: 1, order_all: 'Sort desc,JuActivityID desc' };
        title = '原油周线预测';
    }
    $('.head-title span').text(title);

    data.ex_just = $.extend({}, data.ds_just);
    data.ex_back = $.extend({}, data.ds_back);
    loadArticle(0,data.ex_just, true);
    loadArticle(1,data.ex_back, true);




    $('.weekButton-left').click(function () {
        UE.getEditor('just').focus(true);
    });
    $('.weekButton-right').click(function () {
        UE.getEditor('back').focus(true);
    });

    $('.submit').click(function () {
        var type = $(this).attr('data-type');
        SubmitData(false, type);
    });

    $('.duofang-more,.kongfang-more').click(function () {
        var key = $(this).attr('data-key');
        if (key == 'duofang') {
            var obData = data.ex_just;
            obData.pageIndex++;
            loadArticle(0, obData, true);
        } else {
            var obData1 = data.ex_back;
            obData1.pageIndex++;
            loadArticle(1, obData1, true);
        }
    })

    //我的支持历史
    $('.btnHistory').click(function () {
        GoHistory(false);
    });

});


function loadArticle(num,obData, showProgress) {
    GetContent(
            obData,
            showProgress,
            function (result) {
                obData.total = result.totalcount;
               

                for (var i = 0; i < result.list.length; i++) {
                    obData.list.push(result.list[i]);
                }
                if (num == 0) {
                    $('.duoTotal').text(result.totalcount);
                    viewArticle(result.list, obData.total <= obData.list.length, obData.pageIndex);
                } else {
                    $('.kongTotal').text(result.totalcount);
                    viewArticle1(result.list, obData.total <= obData.list.length, obData.pageIndex);
                }
               
            });
}

//正方视图
function viewArticle(list, hideMore, page) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(justItem).clone();
            if (!!list[i].pubUser) {
                newItem.find('.userimg img').attr('src', list[i].pubUser.avatar);
                newItem.find('.username').text(list[i].pubUser.userName);
                newItem.find('.userimg,.username').attr('data-id', list[i].pubUser.id);
                newItem.find('.userimg,.username').attr('data-nickname', list[i].pubUser.userName);
                newItem.find('.userimg,.username').attr('data-avatar', list[i].pubUser.avatar);
                newItem.find('.userimg,.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.userimg,.username').attr('data-times', list[i].pubUser.times);
                newItem.find('.userimg,.username').attr('data-describe', list[i].pubUser.describe);
            }
            else {
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
            $('.duofang-item').append(newItem);
        }
    } else if (page == 1) {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.duofang-item').append(listHtml.ToString());
    }
    if (hideMore) $('.duofang-more').hide();
    if (!hideMore) $('.duofang-more').show();
}
//反方视图
function viewArticle1(list, hideMore, page) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(backItem).clone();
            if (!!list[i].pubUser) {
                newItem.find('.userimg img').attr('src', list[i].pubUser.avatar);
                newItem.find('.username').text(list[i].pubUser.userName);
                newItem.find('.userimg,.username').attr('data-id', list[i].pubUser.id);
                newItem.find('.userimg,.username').attr('data-nickname', list[i].pubUser.userName);
                newItem.find('.userimg,.username').attr('data-avatar', list[i].pubUser.avatar);
                newItem.find('.userimg,.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.userimg,.username').attr('data-times', list[i].pubUser.times);
                newItem.find('.userimg,.username').attr('data-describe', list[i].pubUser.describe);
            }
            else {
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
            $('.kongfang-item').append(newItem);
        }
    } else if (page == 1) {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.kongfang-item').append(listHtml.ToString());
    }
    if (hideMore) $('.kongfang-more').hide();
    if (!hideMore) $('.kongfang-more').show();
}


function SubmitData(no_check, type) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                SubmitData(true, type);
            } else {
                showLoginDialog(function () {
                    SubmitData(true, type);
                });
            }
        });
        return;
    }
    var cateid = 0;
    var content = '';
    if (rootId == catedata.sroot && type == 'just') {
        cateid = catedata.sjust;
    } else if (rootId == catedata.sroot && type == 'back') {
        cateid = catedata.sback;
    } else if (rootId == catedata.hroot && type == 'just') {
        cateid = catedata.hjust;
    } else if (rootId == catedata.hroot && type == 'back') {
        cateid = catedata.hback;
    } else if (rootId == catedata.yroot && type == 'just') {
        cateid = catedata.yjust;
    } else if (rootId == catedata.yroot && type == 'back') {
        cateid = catedata.yback;
    }
    if (type == 'just') {
        content = UE.getEditor('just').getContent();
    } else {
        content = UE.getEditor('back').getContent();
    }
    if ($.trim(content) == '') {
        alert('请输入内容');
        return;
    }
    var model = {
        type: 'PupilDebate',
        title: title,
        content: content,
        summary: '',
        rootid: rootId,
        cateId: cateid,
        action: 'AddArticle',
        k4: dateFormatWeek()
    };
    var layerIndex = progress();
    $.ajax({
        type: 'POST',
        url: '/Serv/PubApi.ashx',
        data: model,
        dataType: 'json',
        success: function (result) {
            layer.close(layerIndex);
            UE.getEditor('just').setContent('');
            UE.getEditor('back').setContent('');
            if (result.isSuccess) {
                alert('提交成功');
                if (type == 'just') {
                    $('.duofang-item').html('');
                    loadArticle(0, data.ex_just, true);
                } else {
                    $('.kongfang-item').html('');
                    loadArticle(1, data.ex_back, true);
                }
            } else if (result.errcode == 10013) {
                alert('你已经支持过了', 5);
            } else {
                alert('提交出错');
            }
        }
    });
}


//我的支持历史
function GoHistory(no_check) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                window.location.href = '/customize/StockPlayer/Src/PupilDebate/MySupportHistory.aspx';
            } else {
                showLoginDialog(function () {
                    GoHistory(false);
                });
            }
        });
        return;
    }
}