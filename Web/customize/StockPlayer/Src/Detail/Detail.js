
var bs_reward = { rows: 8, page: 1, score_type: 'Reward', relation_id: aid, list: [], total: 1, sum: 0 }

var rewardItem;

var time = new Date();
time = time.format('yyyy-MM-dd hh:mm');

var comment_item;


$(function () {

    data.bs_comment = { pagesize: 10, pageindex: 1, articleid: aid, list: [], total: 1 };
    data.reply_item = { pagesize: 10, pageindex: 1, commentid: '', total: 0, list: [], total: 1 };
    data.reply_count = { count: 0 };

    data.reward = $.extend({}, bs_reward);
    rewardItem = $('.detail-right .font4 .table tr').clone();
    $('.detail-right .font4 .table').html('');

    comment_item = $('.comment-item').clone();
    $('.comment-content').html('');


    data.shareData.title=title1;
    data.shareData.shareImgUrl=shareImgUrl1;
    data.shareData.shareUrl = shareUrl1;


    //加载详情底部内容
    GetDetail({ article_id: data.register.detailArticle }, false, function (resp) {
        if ($.trim(resp.articel_context) == '') {
            $('.warp-shengming').hide();
        } else {
            $('.warp-shengming').html(resp.articel_context);
        }
    });

    type = type.toLowerCase();
    if (type == 'comment') {
        setHeadSelected(0);
        loadReward(data.reward, false, true);
    } else if (type == 'bowen') {
        setHeadSelected(1);
        loadReward(data.reward, false, true);
    } else if (type == 'stock') {
        setHeadSelected(2);
        loadReward(data.reward, false, true);
    } else if (type == 'companypublish') {
        setHeadSelected(3);
    }

    //点赞
    $(document).on('click', '.btn-dianzan', function () {
        dianzan(false);
    });
    //取消点赞
    $(document).on('click', '.btn-deldianzan', function () {
        deldianzan(false);
    });

    $(document).on('click', '.detail-right .font3 .btn', function () {
        showRewardDialog(0, false);
    });
    //通知买/卖家
    $(document).on('click', '.btn-send', function () {
        var id = $(this).attr('data-id');
        var title = $(this).text();
        showDialogSendNotice(notice_price, id, title, false, articlename);
    });
    //评论
    $(document).on('click', '.btn-comment', function () {
        commentArticle(false);
    });

    //获取评论列表数据
    GetCommentList(data.bs_comment, true,null);

    //查看更多留言
    $('.more-comment').click(function () {
        var obData = data['bs_comment'];
        obData.pageindex++;
        GetCommentList(obData, true,null);
    });

    //头像
    loadHeadimg();

    ViewFriend();
    //点击回复
    $(document).on('click', '.hf', function () {
        var id = $(this).attr('data-id');
        var text = $(this).text();
        var tText = $(this);
        if ($.trim(text) == '收起回复') {
            if (data.reply_item.total > 0) {

                $('.content' + id + '').hide();
                $(tText).text(data.reply_item.total + '条回复');
            } else {
                $('.content' + id + '').hide();
                $(tText).text('回复');
            }
        } else {
            $('.content' + id + '').show();
            $(tText).text('收起回复');
        }
    });
    $(document).on('click', '.hfcount', function () {
        var id = $(this).attr('data-id');
        var text = $(this).text();
        var tText = $(this);
        var count = $(this).attr('replyCount');

        if ($.trim(text) == '收起回复') {
            if (data.reply_item.total > 0) {

                $('.content' + id + '').hide();
                $(tText).text(data.reply_item.total + '条回复');
            } else {

                $('.content' + id + '').hide();
                $(tText).text(count + '条回复');
            }
        } else {
            $('.content' + id + '').show();
            $(tText).text('收起回复');
            data.reply_item.commentid = id;
            loadReplyList(data.reply_item, false, function (result) {
                kk = result.result.totalcount;
                data.reply_item.total = result.result.totalcount;
                viewComment1(result.result.list, '.rList' + id + '', data.reply_item.total <= result.result.list.length);
            });
        }
    });



    $(document).on('click', '.reply', function () {
        var id = $(this).attr('data-id');
        var content = $('.text' + id).val();
        var model = {
            commentid: id,
            content: content
        };
        ReplySubmit(model, false, false);
        $('.text' + id).val('');
    });


    $(document).on('click', '.reply1', function () {
        var id = $(this).attr('data-id');
        var content = $('.text' + id).val();

        var mid = $(this).parent().parent().parent().parent().parent().parent().find('.reply').attr('data-id');

        var model = {
            commentid: mid,
            replyid: id,
            content: content
        };
        ReplySubmit(model, false, false);
        $('.text' + id).val('');
    });

});




function loadReplyList(option, showProgress, fn) {
    var layerIndex;
    if (!showProgress) layerIndex = progress();
    $.ajax({
        type: 'POST',
        url: '/serv/api/article/GetCommentReplyList.ashx',
        data: option,
        dataType: 'JSON',
        success: function (resp) {
            if (!showProgress) layer.close(layerIndex);
            if (resp.status) {
                if (!!fn) fn(resp);
            }
        }
    });
}






function GetReward(option, showProgress, fn) {
    var layerIndex;
    if (showProgress) layerIndex = progress();
    var baseOption = {
        rows: 6,
        page: 1,
        relation_id: '0',
        score_type: 'Reward'
    };
    var option = $.extend(baseOption, option);
    $.ajax({
        type: 'post',
        url: "/Serv/API/Score/List.ashx",
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
function loadReward(obData, showProgress, bindPage) {
    GetReward(
        obData,
        showProgress,
        function (result) {
            obData.total = result.total;
            obData["page" + obData.page] = result.list;
            obData.sum = result.sum;
            $('.detail-right .font2').text(obData.sum);
            if (obData.total > 0) $('.detail-right .font4').show();
            if (bindPage) {
                $('.pager2').html('');
                if (obData.total > obData.rows) {
                    $.pager({
                        count: obData.total, nums: obData.rows, numsOption: [obData.rows],
                        page: obData.page,
                        numberOfButtons: 0,
                        showButtons: false,
                        showCurrentPageInfo: true,
                        pageContainer: '.pager2', onchange: function () {
                            data.reward.page = this.page;
                            $('.detail-right .font4 .table').html('');
                            var obData = data.reward;
                            var list = obData['page' + this.page];
                            if (!list) {
                                loadReward(obData, true, false);
                            } else {
                                viewReward(list);
                            }
                        }
                    });
                }
            }
            viewReward(result.list);
        });
}
function viewReward(list) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(rewardItem).clone();
            newItem.find('.from-user,.username').text(list[i].nickname);
            newItem.find('.from-user,.username').attr('data-id', list[i].uid);
            var score = Math.abs(list[i].score)
            newItem.find('.reward-num').text(score);
            newItem.show();
            $('.detail-right .font4 .table').append(newItem);
        }
    }
}

function dianzan(no_check) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                dianzan(true);
            } else {
                showLoginDialog(function () {
                    dianzan(true);
                });
            }
        });
        return;
    }
    $.ajax({
        type: 'post',
        url: '/Serv/API/Relation/AddCommUserRelation.ashx',
        data: { rtype: 'JuActivityPraise', mainId: aid },
        dataType: 'JSON',
        success: function (resp) {
            if (resp.status) {
                $('.btn-dianzan').addClass('btn-deldianzan').removeClass('btn-dianzan');
                $('.btn-deldianzan img').attr('src', '/customize/StockPlayer/Img/praise1.png');
                $('.btn-deldianzan span').text('取消点赞');
                var num = parseInt($('.dianzan-num').text());
                num = num + 1;
                $('.dianzan-num').text(num);
                alert('点赞成功');
            } else if (resp.code == 10013) {
                alert('您已经点赞');
                $('.btn-dianzan').addClass('btn-deldianzan').removeClass('btn-dianzan');
                $('.btn-deldianzan img').attr('src', '/customize/StockPlayer/Img/praise1.png');
                $('.btn-deldianzan span').text('取消点赞');
            } else {
                alert('点赞失败');
            }
        }
    });
}
function deldianzan(no_check) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                deldianzan(true);
            } else {
                showLoginDialog(function () {
                    deldianzan(true);
                });
            }
        });
        return;
    }
    $.ajax({
        type: 'post',
        url: '/Serv/API/Relation/DelCommUserRelation.ashx',
        data: { rtype: 'JuActivityPraise', mainId: aid },
        dataType: 'JSON',
        success: function (resp) {
            if (resp.status) {
                $('.btn-deldianzan').addClass('btn-dianzan').removeClass('btn-deldianzan');
                $('.btn-dianzan img').attr('src', '/customize/StockPlayer/Img/praise.png');
                $('.btn-dianzan span').text('赞一下');
                var num = parseInt($('.dianzan-num').text());
                num = num - 1;
                $('.dianzan-num').text(num);
            } else {
                alert('出错,请联系管理员');
            }
        }
    });
}
function showRewardDialog(score, no_check) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                showRewardDialog(result.score, true);
            } else {
                showLoginDialog(function (resp) {
                    showRewardDialog(resp.score, true);
                });
            }
        });
        return;
    }
    var html = new StringBuilder();
    html.AppendFormat('<div class="row">');
    html.AppendFormat('<div class="col-xs-4">你的{0}</div><div class="col-xs-8 dlg-user-score">{1}</div>', score_name, score);
    html.AppendFormat('<div class="col-xs-4">赠送{0}</div><div class="col-xs-8"><input type="number"  class="form-control textCheck dlg-reward" min="{1}" maxlength="9" data-min-reward="{1}" placeholder="请输入赠送数额" value="10" onkeyup="this.value=this.value.replace(/\D/g,\'\')" /></div>', score_name, min_reward);
    html.AppendFormat('</div>');

    layer.open({
        type: 1,
        title: '赠送',
        skin: 'layui-layer-reward', //样式类名
        closeBtn: 0, //不显示关闭按钮
        move: false,//禁止移动
        //shift: 2,
        //shadeClose: true, //开启遮罩关闭
        content: html.ToString(),
        btn: ['确认', '取消'], //按钮
        yes: function (index, layero) {
            if ($(layero).find('.layui-layer-btn0').hasClass('btn-disabled')) {
                return;
            }
            var userNumStr = $.trim($(layero).find('.dlg-user-score').text());
            var rewardNumStr = $.trim($(layero).find('.dlg-reward').val());
            var rewardMinNumStr = $.trim($(layero).find('.dlg-reward').attr('data-min-reward'));
            if (rewardNumStr == '') {
                alert('请输入' + score_name);
                return;
            }
            if (parseFloat(rewardNumStr) < parseFloat(rewardMinNumStr)) {
                alert('最少需要赠送' + rewardMinNumStr + score_name);
                $(layero).find('.dlg-reward').addClass('textError');
                return;
            }
            if (parseFloat(userNumStr) < parseFloat(rewardNumStr)) {
                alert('您的' + score_name + '不足');
                $(layero).find('.dlg-reward').addClass('textError');
                return;
            }
            rewardScore(index, parseFloat(rewardNumStr), false, rewardMinNumStr, layero);

        }, btn2: function (index, layero) {
        }
    });
}

function rewardScore(layerIndex, score, no_check, rewardMinNumStr, layero) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                rewardScore(layerIndex, score, true, rewardMinNumStr, layero);
            } else {
                showLoginDialog(function (resp) {
                    rewardScore(layerIndex, score, true, rewardMinNumStr, layero);
                });
            }
        });
        return;
    }


    $(layero).find('.layui-layer-btn0').text('赠送中...');
    $(layero).find('.layui-layer-btn0').addClass('btn-disabled');

    $.ajax({
        type: 'post',
        url: '/Serv/API/Score/Reward.ashx',
        data: { score: score, id: aid, score_name: score_name, action_name: action_name, min: rewardMinNumStr },
        dataType: 'JSON',
        success: function (resp) {
            layer.close(layerIndex);
            if (resp.status) {
                $(layero).find('.layui-layer-btn0').text('确认');
                $(layero).find('.layui-layer-btn0').removeClass('btn-disabled');
                $('.detail-right .font4 .table').html('');
                data.reward = $.extend({}, bs_reward);
                loadReward(data.reward, true, true);
                alert(resp.msg);
            } else {
                alert(resp.msg);
            }
        }
    });
}


function commentArticle(showProgress) {
    if (!showProgress) {
        checkLogin(function (result) {
            if (result.is_login) {
                commentArticle(true);
            } else {
                showLoginDialog(function () {
                    commentArticle(true);
                });
            }
        });
        return;
    }

    var content = $('#commentcontent').val();
    if ($.trim(content) == '') {
        alert('留言不能为空');
        return;
    }
    var model = {
        articleid: aid,
        content: content
    };
    $.ajax({
        type: 'post',
        url: '/Serv/API/article/Comment.ashx',
        data: model,
        dataType: 'JSON',
        success: function (resp) {
            if (resp.status) {
                $('#commentcontent').val('');
                alert(resp.msg);
                data.bs_comment.pageindex = 1;
                GetCommentList(data.bs_comment, true,'111');
            } else {
                alert(resp.msg);
            }
        }
    });
}
function loadHeadimg() {
    $('.nick').click(function () {
        var did = $('.username').attr('data-id');
        ToUser(did);
    });
}


function ViewFriend() {
    $('.userimg img').attr('src', avatar);
    $('.company-name,.username').text(username);
    $('.userimg,.head-img,.username').attr('data-id', id);
    $('.userimg,.head-img,.username').attr('data-nickname', username);
    $('.userimg,.head-img,.username').attr('data-avatar', avatar);
    $('.userimg,.head-img,.username').attr('data-friend', (isfriend == 'True' ? '1' : '0'));
    $('.userimg,.head-img,.username').attr('data-times', times);
    $('.userimg,.head-img,.username').attr('data-describe', describe);
}









//获取评论列表
function GetCommentList(obData, showProgress, isclear) {
    var layerIndex;
    if (showProgress) layerIndex = progress();
    var model = {
        pageindex: 1,
        pagesize: 10,
        articleid:''
    };
    if (!!isclear) obData.list = [];
    var option = SetOption(model, obData);

    $.ajax({
        type: 'post',
        url: '/Serv/API/article/GetCommentList.ashx',
        data: option,
        dataType: 'JSON',
        success: function (resp) {
            if (showProgress) layer.close(layerIndex);
            if (resp.status) {
                obData.total = resp.result.totalcount;
                for (var i = 0; i < resp.result.list.length; i++) {
                    obData.list.push(resp.result.list[i]);
                }
                $('.comment-font span').text(resp.result.totalcount);
               
                viewComment(resp.result.list, '.comment-content', obData.total <= obData.list.length, obData.pageindex, isclear);
            }
        }
    });
}

function viewComment(list, selector1, hidebtn, page,isload) {
    if (!!isload) {
        $(selector1).html('');
    } 
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(comment_item).clone();
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

            $(newItem).find('.rowButtom').text(list[i].content);
            if (list[i].replyCount <= 0) {
                $(newItem).find('.huiu-font').text('回复').addClass('hf').attr('data-id', list[i].id);
            } else {
                $(newItem).find('.huiu-font').text(list[i].replyCount + '条回复').addClass('hfcount').attr({ 'data-id': list[i].id, 'replyCount': list[i].replyCount });
            }
            $(newItem).find('.rList').addClass('rList' + list[i].id + '');
            $(newItem).find('.rowContent .time').text(dateFormat(list[i].createDate));
            $(newItem).find('.add-content').addClass('content' + list[i].id + '');
            $(newItem).find('.reply').attr('data-id', list[i].id);
            $(newItem).find('.text-content').addClass('text' + list[i].id + '');
            $(newItem).show();
            $(selector1).append(newItem);

        }
    } else if (page == 1) {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $(selector1).append(listHtml.ToString());
    }

    if (!hidebtn) $('.detail-more-comment').show();
    if (hidebtn) $('.detail-more-comment').hide();
}

function viewComment1(list, selector1, hidebtn) {
    $(selector1).html('');
    for (var i = 0; i < list.length; i++) {
        var newItem = $(comment_item).clone();
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

        $(newItem).find('.rowButtom').text(list[i].content);


        $(newItem).find('.huiu-font').text('回复').addClass('hf').attr('data-id', list[i].id);

        $(newItem).find('.rList').addClass('rList' + list[i].id + '');

        $(newItem).find('.rowContent .time').text(dateFormat(list[i].createDate));

        if (!!list[i].replayToUser.userName) {
            $(newItem).find('.rowContent .k1').text('回复');
            $(newItem).find('.rowContent .k2').text(list[i].replayToUser.userName);
        }
        $(newItem).find('.add-content').addClass('content' + list[i].id + '');
        $(newItem).find('.reply').removeClass('reply').addClass('reply1').attr('data-id', list[i].id);
        $(newItem).find('.text-content').addClass('text' + list[i].id + '');
        $(newItem).show();
        $(selector1).append(newItem);

    }
    if (!hidebtn) $('.detail-more-reply').show();
    if (hidebtn) $('.detail-more-reply').hide();
}


function ReplySubmit(option, ischeck, showProgress) {
    if (!ischeck) {
        checkLogin(function (result) {
            if (result.is_login) {
                ReplySubmit(option, true, false);
            } else {
                showLoginDialog(function (result) {


                    ReplySubmit(option, true, false);
                });
            }
        });
        return;
    }
    if ($.trim(option.content) == '') {
        alert('请输入回复内容');
        return;
    }
    var layerIndex;
    if (!showProgress) layerIndex = progress();
    $.ajax({
        type: 'POST',
        url: '/serv/api/article/CommentReply.ashx',
        data: option,
        dataType: 'JSON',
        success: function (resp) {
            if (!showProgress) layer.close(layerIndex);
            if (resp.status) {

                data.reply_item.commentid = option.commentid;
                loadReplyList(data.reply_item, true, function (result) {
                    data.reply_item.total = result.result.totalcount
                    viewComment1(result.result.list, '.rList' + data.reply_item.commentid + '');

                });
            }

        }
    });
}









