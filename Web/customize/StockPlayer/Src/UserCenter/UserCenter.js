//个人中心
var commentItem;
var bowenItem;
var stockItem;
var friendItem;
var blogItem;
var codeInterval = null;
var codeIntervalNum = 0;
data.bs_friends = { page: 1, rows: 10, rel_id: u_id, list: [], total: 1 };
$(function () {
    commentItem = $('.usercenter-comment .comment-item').clone();
    bowenItem = $('.usercenter-bowen .bowen-item').clone();
    stockItem = $('.usercenter-stock .user-stock .col-xs-6').clone();
    friendItem = $('.list-friend .friend-info').clone();
    blogItem = $('.company-release .company-release-item').clone();

    $('.usercenter-comment').html('');
    $('.usercenter-bowen').html('');
    $('.user-stock').html('');
    $('.list-friend').html('');
    $('.company-release').html('');

    //头部链接选中行情
    setHeadSelected(6);

    if (u_type == 0) { //未登录 或未找到用户
        //登录验证
        tt.vf.req.addId('user_acount', 'user_pwd');
        $('#user_acount,#user_pwd').keypress(function () {
            var theEvent = window.event || arguments.callee.caller.arguments[0];
            if (theEvent.keyCode == 13) {
                $('.btn-login').click();
            }
        });
        $('.btn-login').click(function () {
            if (!tt.validateId('user_acount', 'user_pwd')) {   //userRegister是form的name  
                return false;
            }
            var username = $.trim($('#user_acount').val());
            var userpassword = $.trim($('#user_pwd').val());
            loginApi(username, userpassword);
        });

    }
    else if (u_type == 6) { //公司
        data.bs_blogs = { pageIndex: 1, pageSize: 10, type: 'CompanyPublish', author: u_id, list: [], total: 1 };
        data.blogs = $.extend({}, data.bs_blogs);
        loadBlogs(data.blogs, true, true);
    } else { //用户
        data.bs_sps = { pageIndex: 1, pageSize: 10, author: u_id, list: [], total: 1 };
        data.bs_bowens = { pageIndex: 1, pageSize: 10, type: 'Bowen', author: u_id, list: [], total: 1 };
        data.bs_stocks = { pageIndex: 1, pageSize: 10, type: 'Stock', is_hide: 'all',order_all: 'IsHide ASC, PraiseCount desc,JuActivityID desc', author: u_id, list: [], total: 1 };
        
        if (isme <= 0) {
            data.bs_stocks.is_hide = '0';
        }

        data.sps = $.extend({}, data.bs_sps);
        data.bowens = $.extend({}, data.bs_bowens);
        data.stocks = $.extend({}, data.bs_stocks);
        data.friends = $.extend({}, data.bs_friends);

        loadBowens(data.bowens, false, true, true);
        loadStocks(data.stocks, true, true, true);
        loadComments(data.sps, false, true, true);
        loadFriends(data.friends, false, true, true);
        loadUserLevel(u_id, u_times);

        $('.total-nav .cell').click(function () {
            var num = $(this).attr('data-index');
            var list = [];
            for (var i = 0; i < 5; i++) {
                if (num != i) list.push('.user-' + i);
            }
            $('.total-nav .tdYellow').addClass('tdGray').removeClass('tdYellow');
            $(this).addClass('tdYellow').removeClass('tdGray');
            $(list.join(',')).hide();
            $('.user-' + num).show();
            if (num == 1) {
                loadBowens(data.bowens, false, true, true);
            }

        });
        $(document).on('click', '.btn-jiahaoyou', function () {
            var id = $(this).attr('data-id');
            var nickname = $(this).attr('data-nickname');
            applyFriend(id, nickname, false);
        });
        $(document).on('click', '.btn-delhaoyou', function () {
            var id = $(this).attr('data-id');
            var nickname = $(this).attr('data-nickname');
            deleteFriend(id, nickname, function (error, obj) {
                if (!error) {
                    var oIndex = data.friends.pageIndex;
                    data.friends = $.extend({}, data.bs_friends);
                    $('.list-friend').html('');
                    loadFriends(data.friends, false, true, true);
                }
            }, false);
        });
        $(document).on('click', '.btn-cz', function () {
            ToRecharge();
        });

        $(document).on('click', '.btn-tx', function () {
            if (u_has_wx == 0) {
                if (u_id > 0) bindWXOpenId(u_id);
            }
            else {
                ToWithdrawCash();
            }
        });
    }
    //未读消息
    if (isme) {
        getUnreadMsgCount(function (num) {
            if (num > 0) {
                $('.UnReadMsg').text('（' + num + '）');
            }
        });
    }
    $(document).on('click', '.to-notice', function () {
        window.location.href = '/customize/StockPlayer/Src/UserCenter/Notice/Notice.aspx'
    });
    //修改资料

    $('#cancelBtn').click(function () {
        $('#edit').hide();
        $('#user_nick').val('');
    });
    //修改密码
    $('#updatePwd,#updateCompanyPwd').click(function () {
        showUpdatePwdDialog(function (pwd, newpwd, confirmpwd, layerIndex) {
            var layerIndex1 = progress();
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/UpdateLoginPwd.ashx',
                data: { user_pwd: pwd, new_pwd: newpwd, config_pwd: confirmpwd, pwd_length: 6 },
                dataType: 'JSON',
                success: function (resp) {
                    layer.close(layerIndex1);
                    if (resp.isSuccess) {
                        layer.close(layerIndex);
                        alert(resp.errmsg, 6);
                    } else {
                        alert(resp.errmsg, 5);
                    }
                }
            });
        });
    });


    $(document).on('change', '#logofile,#ex3file', function () {
        var layerIndex = progress();
        var imgObj = $(this).prev();
        $.ajaxFileUpload({
            url: '/serv/api/common/file.ashx?action=Add',
            secureuri: false,
            fileElement: $(this),
            dataType: 'json',
            success: function (result) {
                layer.close(layerIndex);
                if (result.errcode == 0) {
                    $(imgObj).removeAttr('src');
                    $(imgObj).attr('src', result.file_url_list[0]);
                }
                else {
                    alert(result.errmsg);
                }
            }
        });
    });
    $(document).on('change', '#dlg_phone', function () {
        var nPhone = $.trim($(this).val());
        var oPhone = $.trim($(this).attr('data-oldphone'));
        if (nPhone == oPhone) {
            $(this).closest('.row').find('.trCode').hide();
        } else {
            $(this).closest('.row').find('.trCode').show();
        }
    });
    //获取验证码
    $(document).on('click', '.sendCode', function () {
        var phone = $.trim($(this).closest('.row').find('#dlg_phone').val());
        if (phone == '') {
            $(this).closest('.row').find('#dlg_phone').focus();
            return false;
        }
        var layerIndex = progress();
        $.ajax({
            type: 'POST',
            url: '/serv/api/common/smsvercode.ashx',
            data: { phone: phone, smscontent: '{{SMSVERCODE}}',content:1 },
            dataType: 'json',
            success: function (data) {
                layer.close(layerIndex);
                if (data.errcode == 0) {
                    alert('验证码已发送');
                    codeIntervalNum = 60;
                    $('.sendCode').text('等待（' + codeIntervalNum + '）');
                    codeInterval = setInterval(function () {
                        codeIntervalNum--;
                        if (codeIntervalNum < 0) {
                            $('.sendCode').text('获取验证码');
                            $('.sendCode').removeAttr('disabled');
                            clearInterval(codeInterval);
                        } else {
                            $('.sendCode').text('等待（' + codeIntervalNum + '）');
                            $('.sendCode').attr('disabled', 'disabled');
                        }
                    }, 1000);
                } else {
                    alert(data.errmsg);
                }
            }
        });
    });
    //绑定微信
    $(document).on('click', '.company-wx', function () {
        if (u_id > 0) bindWXOpenId(u_id);
    });
    //分享加积分
    $(document).on('click', '#shareQRCode', function () {

        if (u_id > 0) bindShareWeiXin(u_id, u_has_wx);
    })
    //跳入淘股币详细页面
    $('.right-content .right-taobi').click(function () {
        window.location.href = '/customize/StockPlayer/Src/UserCenter/ScoreDetails/ScoreDetails.aspx';
    });
})
function loadUserLevel(id, times) {
    var lv = getUserLevelImgs(id, times);
    $('.color-xingji').html('');
    for (var i = 0; i < lv.imgs.length; i++) {
        if (!!lv.imgs[i]) $('.color-xingji').append('<img src="' + lv.imgs[i] + '">')
    }
}
//加载博文
function loadBowens(obData, showProgress, viewIndex, bindPage) {
    GetContent(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            obData['page' + obData.pageIndex] = result.list;
            if (viewIndex) viewIndexBowens(result.list, obData.total);
            if (bindPage) {
                $('.pager1').html('');
                if (obData.total > obData.pageSize) {
                    $.pager({
                        count: obData.total, nums: obData.pageSize, numsOption: [obData.pageSize],
                        pageContainer: '.pager1', datakey: 'bowens', onchange: function () {
                            data.bowens.pageIndex = this.page;
                            $('.list-bowen').html('');
                            var obData = data.bowens;
                            var list = obData['page' + this.page];
                            if (!list) {
                                loadBowens(obData, true, false, false);
                            } else {
                                viewBowens(list, obData.pageIndex);
                            }
                        }
                    });
                }
            }
            viewBowens(result.list, obData.pageIndex);
        });
}
//加载股权
function loadStocks(obData, showProgress, viewIndex, bindPage) {
    GetContent(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            obData['page' + obData.pageIndex] = result.list;
            if (viewIndex) viewIndexStocks(result.list, obData.total);
            if (bindPage) {
                $('.pager2').html('');
                if (obData.total > obData.pageSize) {
                    $.pager({
                        count: obData.total, nums: obData.pageSize, numsOption: [obData.pageSize],
                        pageContainer: '.pager2', onchange: function () {
                            data.stocks.pageIndex = this.page;
                            $('.list-stock .user-stock').html('');
                            var obData = data.stocks;
                            var list = obData['page' + this.page];
                            if (!list) {
                                loadStocks(obData, true, false, false);
                            } else {
                                viewStocks(list, obData.pageIndex);
                            }
                        }
                    });
                }
            }
            viewStocks(result.list, obData.pageIndex);
        });
}
//加载时评
function loadComments(obData, showProgress, viewIndex, bindPage) {
    GetContent(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            obData['page' + obData.pageIndex] = result.list;
            if (viewIndex) viewIndexComments(result.list, obData.total);
            if (bindPage) {
                $('.pager3').html('');
                if (obData.total > obData.pageSize) {
                    $.pager({
                        count: obData.total, nums: obData.pageSize, numsOption: [obData.pageSize],
                        pageContainer: '.pager3', onchange: function () {
                            data.sps.pageIndex = this.page;
                            $('.list-comment').html('');
                            var obData = data.sps;
                            var list = obData['page' + this.page];
                            if (!list) {
                                loadComments(obData, true, false, false);
                            } else {
                                viewComments(list, obData.pageIndex);
                            }
                        }
                    });
                }
            }
            viewComments(result.list, obData.pageIndex);
        });
}
//加载好友
function loadFriends(obData, showProgress, viewIndex, bindPage) {
    GetFriends(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            obData['page' + obData.page] = result.list;
            if (viewIndex) viewIndexFriends(obData.total);
            if (bindPage) {
                $('.pager4').html('');
                if (obData.total > obData.rows) {
                    $.pager({
                        count: obData.total,
                        nums: obData.rows,
                        numsOption: [obData.rows],
                        page: obData.page,
                        pageContainer: '.pager4', onchange: function () {
                            data.friends.pageIndex = this.page;
                            $('.list-friend').html('');
                            var obData = data.friends;
                            var list = obData['page' + this.page];
                            if (!list) {
                                loadFriends(obData, true, false, false);
                            } else {
                                viewFriends(list, obData.page);
                            }
                        }
                    });
                }
            }
            viewFriends(result.list, obData.page);
        });
}
//加载公司发布
function loadBlogs(obData, showProgress, bindPage) {
    GetContent(
        obData,
        showProgress,
        function (result) {
            obData.total = result.totalcount;
            obData['page' + obData.pageIndex] = result.list;
            if (bindPage) {
                $('.pager5').html('');
                if (obData.total > obData.pageSize) {
                    $.pager({
                        count: obData.total, nums: obData.pageSize, numsOption: [obData.pageSize],
                        pageContainer: '.pager5', datakey: 'blogs', onchange: function () {
                            data.blogs.pageIndex = this.page;
                            $('.company-release').html('');
                            var obData = data.blogs;
                            var list = obData['page' + this.page];
                            if (!list) {
                                loadBlogs(obData, true, false);
                            } else {
                                viewBlogs(list, obData.pageIndex);
                            }
                        }
                    });
                }
            }
            viewBlogs(result.list, obData.pageIndex);
        });
}
//呈现公司发布
function viewBlogs(list, page) {
    $('.company-release').html('');
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(blogItem).clone();
            newItem.find('.release-img').attr({ 'src': list[i].imgSrc, 'data-id': list[i].id }).click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
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
            newItem.find('.title').text(list[i].title);
            newItem.find('.context').text(list[i].summary);
            newItem.find('.context,.title').attr('data-id', list[i].id);
            newItem.find('.company-operation .icon-edit').attr("data-id",list[i].id).click(function () {
                var jid = $(this).attr('data-id');
                ToEdit(jid);
            });
            newItem.find('.company-operation .icon-del').attr({ "data-id": list[i].id, "data-title": list[i].title }).click(function () {
                var jid = $(this).attr('data-id');
                var title = $(this).attr('data-title');
                confirm('友情提示', '确定要删除' + title + '吗?', '确定', '取消', function () {
                    DelBlog(jid);
                }, function () {
                    alert('您选择了取消操作');
                });
            })
            newItem.find('.context,.title').click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.comment span').text(list[i].commentCount);
            newItem.find('.zan span').text(list[i].praiseCount);
            newItem.show();
            $('.company-release').append(newItem);
        }
    } else if (page == 1) {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.company-release').append(listHtml.ToString());
    }
}
function DelBlog(jid) {
    $.ajax({
        type: 'post',
        url: '/serv/api/article/delete.ashx',
        data: { jid: jid },
        dataType: 'json',
        success: function (resp) {
            if (resp.status) {
                data.blogs = $.extend({}, data.bs_blogs);
                loadBlogs(data.blogs, true, true);
                alert('删除完成');
            } else {
                alert('删除出错');
            }
        }

    });
}


//呈现博文列表
function viewIndexBowens(list, total) {
    $('.total-bowen').text(total);
    var maxLength = list.length > 3 ? 3 : list.length;
    $('.usercenter-bowen').html('');
    if (maxLength > 0) {
        for (var i = 0; i < maxLength; i++) {
            var newItem = $(bowenItem).clone();
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
            newItem.find('.bowen-title').text(list[i].title);
            newItem.find('.bowen-content').text(list[i].summary);
            newItem.find('.bowen-content,.bowen-title').attr('data-id', list[i].id);
            newItem.find('.bowen-content,.bowen-title').click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.comment span').text(list[i].commentCount);
            newItem.find('.zan span').text(list[i].praiseCount);
            newItem.find('.score span').text(list[i].rewardTotal);

            newItem.find('.bowen-operation .btn-edit').attr('data-id', list[i].id).click(function () {
                var did = $(this).attr('data-id');
                ToEdit(did);
            });
            newItem.find('.bowen-operation .btn-del').attr({ 'data-id': list[i].id, 'data-title': list[i].title }).click(function () {
                var did = $(this).attr('data-id');
                var title = $(this).attr('data-title');
                confirm('友情提示', '确定要删除' + title + '吗?', '确定', '取消', function () {
                    DelBowen(did);
                }, function () {
                    alert('您选择了取消操作');
                });
            });
            newItem.show();
            $('.usercenter-bowen').append(newItem);
        }
    } else {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.usercenter-bowen').append(listHtml.ToString());
    }
}
function viewBowens(list, page) {
    $('.list-bowen').html('');
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(bowenItem).clone();
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
            newItem.find('.bowen-title').text(list[i].title);
            newItem.find('.bowen-content').text(list[i].summary);
            newItem.find('.bowen-content,.bowen-title').attr('data-id', list[i].id);
            newItem.find('.bowen-content,.bowen-title').click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.comment span').text(list[i].commentCount);
            newItem.find('.zan span').text(list[i].praiseCount);
            newItem.find('.score span').text(list[i].rewardTotal);
            newItem.find('.bowen-operation .btn-edit').attr('data-id', list[i].id).click(function () {
                var did = $(this).attr('data-id');
                ToEdit(did);
            });
            newItem.find('.bowen-operation .btn-del').attr({ 'data-id': list[i].id, 'data-title': list[i].title }).click(function () {
                var did = $(this).attr('data-id');
                var title = $(this).attr('data-title');
                confirm('友情提示', '确定要删除' + title + '吗?', '确定', '取消', function () {
                    DelStock(did);
                }, function () {
                    alert('您选择了取消操作');
                });
            });
            newItem.show();
            $('.list-bowen').append(newItem);
        }
    } else if (page == 1) {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.list-bowen').append(listHtml.ToString());
    }
}
function DelBowen(jid) {
    $.ajax({
        type: 'post',
        url: '/serv/api/article/delete.ashx',
        data: { jid: jid },
        dataType: 'json',
        success: function (resp) {
            if (resp.status) {
                data.bowens = $.extend({}, data.bs_bowens);
                loadBowens(data.bowens, true, true, true);
                alert('删除完成');
            } else {
                alert('删除出错');
            }
        }

    });
}
//呈现股权列表
function viewIndexStocks(list, total, review) {
    $('.total-stock').text(total);
    var maxLength = list.length > 4 ? 4 : list.length;
    $('.usercenter-stock .user-stock').html('');
    if (maxLength > 0) {
        for (var i = 0; i < maxLength; i++) {
            var newItem = $(stockItem).clone();
            newItem.find('.stock-img img').attr('src',list[i].imgSrc);
            if (list[i].ishide == 0) {
                newItem.find('#postion').addClass('positionText');
                newItem.find('#postion').text('上架中');
            } else {
                newItem.find('#postion').addClass('positionText1');
                newItem.find('#postion').text('下架中');
            }
            newItem.find('.stock-title .title').text(list[i].title);
            newItem.find('.stock-title .btn-type').text(list[i].categoryName);
            if (!!list[i].pubUser) {
                newItem.find('.username').text(list[i].pubUser.userName);
                newItem.find('.username').attr('data-id', list[i].pubUser.id);
                newItem.find('.username').attr('data-nickname', list[i].pubUser.userName);
                newItem.find('.username').attr('data-avatar', list[i].pubUser.avatar);
                newItem.find('.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.userimg,.username').attr('data-times', list[i].pubUser.times);
                newItem.find('.userimg,.username').attr('data-describe', list[i].pubUser.describe);
            } else {
                newItem.find('.username').text('淘股玩家');
                newItem.find('.userimg,.username').attr('data-id', 0);
                newItem.find('.userimg,.username').attr('data-nickname', '淘股玩家');
                newItem.find('.userimg,.username').attr('data-times', 0);
                newItem.find('.userimg,.username').attr('data-friend', 1);
                newItem.find('.userimg,.username').attr('data-avatar', 'http://file.comeoncloud.net/img/europejobsites.png');
                newItem.find('.userimg,.username').attr('data-describe', '');
            }
            newItem.find('.time').text(dateFormat(list[i].createDate));
            newItem.find('.stock-content').text(list[i].summary);
            newItem.find('.stock-content,.stock-title .title,.stock-img img').attr('data-id', list[i].id).click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.user-stock-buttom .btn-edit').attr('data-id', list[i].id).click(function () {
                var did = $(this).attr('data-id');
                ToEdit(did);
            });
            newItem.find('.user-stock-buttom .btn-del').attr({ 'data-jid': list[i].id, "data-title": list[i].title }).click(function () {
                var title = $(this).attr('data-title');
                var jid = $(this).attr('data-jid');
                confirm('友情提示', '确定要删除' + title + '吗?', '确定', '取消', function () {
                    DelStock(jid);
                }, function () {
                    alert('您选择了取消操作');
                });
            });
            newItem.find('.user-stock-buttom .btn-sj').attr({ 'data-jid': list[i].id, "data-title": list[i].title }).click(function () {
                var title = $(this).attr('data-title');
                var jid = $(this).attr('data-jid');
                var item = $(this).parent().parent().find('#postion');
                confirm('友情提示', '确定要上架' + title + '吗?', '确定', '取消', function () {
                    if ($(item).text() == '上架中') {
                        alert('已经上架');
                        return;
                    }
                    TopStock(jid, 0);
                }, function () {
                    alert('您选择了取消操作');
                });
            });
            newItem.find('.user-stock-buttom .btn-xj').attr({ 'data-jid': list[i].id, "data-title": list[i].title }).click(function () {
                var title = $(this).attr('data-title');
                var jid = $(this).attr('data-jid');
                var item = $(this).parent().parent().find('#postion');
                confirm('友情提示', '确定要下架' + title + '吗?', '确定', '取消', function () {
                    if ($(item).text() == '下架中') {
                        alert('已经下架');
                        return;
                    }
                    TopStock(jid, 1);
                }, function () {
                    alert('您选择了取消操作');
                });
            });
            newItem.find('.stock-num').text(list[i].k3);
            newItem.show();
            $('.usercenter-stock .user-stock').append(newItem);
        }
    } else {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.usercenter-stock .user-stock').append(listHtml.ToString());
    }
}
function viewStocks(list, page) {
    $('.list-stock .user-stock').html('');
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(stockItem).clone();
            newItem.find('.stock-img img').attr('src', list[i].imgSrc);
            newItem.find('.stock-title .title').text(list[i].title);
            if (list[i].ishide == 0) {
                newItem.find('#postion').addClass('positionText');
                newItem.find('#postion').text('上架中');
            } else {
                newItem.find('#postion').addClass('positionText1');
                newItem.find('#postion').text('下架中');
            }
            newItem.find('.stock-title .btn-type').text(list[i].categoryName);
            if (!!list[i].pubUser) {
                newItem.find('.username').text(list[i].pubUser.userName);
                newItem.find('.username').attr('data-id', list[i].pubUser.id);
                newItem.find('.username').attr('data-nickname', list[i].pubUser.userName);
                newItem.find('.username').attr('data-avatar', list[i].pubUser.avatar);
                newItem.find('.username').attr('data-friend', (list[i].pubUser.isFriend ? '1' : '0'));
                newItem.find('.userimg,.username').attr('data-times', list[i].pubUser.times);
                newItem.find('.userimg,.username').attr('data-describe', list[i].pubUser.describe);
            } else {
                newItem.find('.username').text('淘股玩家');
                newItem.find('.userimg,.username').attr('data-id', 0);
                newItem.find('.userimg,.username').attr('data-nickname', '淘股玩家');
                newItem.find('.userimg,.username').attr('data-times', 0);
                newItem.find('.userimg,.username').attr('data-friend', 1);
                newItem.find('.userimg,.username').attr('data-avatar', 'http://file.comeoncloud.net/img/europejobsites.png');
                newItem.find('.userimg,.username').attr('data-describe', '');
            }
            newItem.find('.time').text(dateFormat(list[i].createDate));
            newItem.find('.stock-content').text(list[i].summary);
            newItem.find('.stock-content,.stock-title .title,.stock-img img').attr('data-id', list[i].id).click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.stock-num').text(list[i].k3);
            newItem.find('.user-stock-buttom .btn-edit').attr('data-id', list[i].id).click(function () {
                var did = $(this).attr('data-id');
                ToEdit(did);
            });
            newItem.find('.user-stock-buttom .btn-del').attr({ 'data-jid': list[i].id, "data-title": list[i].title }).click(function () {
                var title = $(this).attr('data-title');
                var jid = $(this).attr('data-jid');
                confirm('友情提示', '确定要删除' + title + '吗?', '确定', '取消', function () {
                    DelStock(jid);
                }, function () {
                    alert('您选择了取消操作');
                });
            });
            newItem.find('.user-stock-buttom .btn-sj').attr({ 'data-jid': list[i].id, "data-title": list[i].title }).click(function () {
                var title = $(this).attr('data-title');
                var jid = $(this).attr('data-jid');
                var item = $(this).parent().parent().find('#postion');
                confirm('友情提示', '确定要上架' + title + '吗?', '确定', '取消', function () {
                    if ($(item).text() == '上架中') {
                        alert('已经上架');
                        return;
                    }
                    TopStock(jid, 0);
                }, function () {
                    alert('您选择了取消操作');
                });
            });
            newItem.find('.user-stock-buttom .btn-xj').attr({ 'data-jid': list[i].id, "data-title": list[i].title }).click(function () {
                var title = $(this).attr('data-title');
                var jid = $(this).attr('data-jid');
                var item = $(this).parent().parent().find('#postion');
                confirm('友情提示', '确定要下架' + title + '吗?', '确定', '取消', function () {
                    if ($(item).text() == '下架中') {
                        alert('已经下架');
                        return;
                    }
                    TopStock(jid, 1);
                }, function () {
                    alert('您选择了取消操作');
                });
            });
            newItem.show();
            $('.list-stock .user-stock').append(newItem);
        }
    } else if (page == 1) {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.list-stock .user-stock').append(listHtml.ToString());
    }
}

function DelStock(jid) {
    $.ajax({
        type: 'post',
        url: '/serv/api/article/delete.ashx',
        data: { jid: jid },
        dataType: 'json',
        success: function (resp) {
            if (resp.status) {
                data.stocks = $.extend({}, data.bs_stocks);
                loadStocks(data.stocks, true, true, true);
                alert('删除完成');
            } else {
                alert('删除出错');
            }
        }

    });
}
function TopStock(jid, ishide) {
    $.ajax({
        type: 'post',
        url: '/serv/api/article/ishide.ashx',
        data: { jid: jid,is_hide:ishide },
        dataType: 'json',
        success: function (resp) {
            if (resp.status) {
                data.stocks = $.extend({}, data.bs_stocks);
                loadStocks(data.stocks, true, true, true);
                alert(ishide == 1 ? '下架完成' : '上架完成');
            } else {
                alert('操作出错');
            }
        }

    });
}
//呈现时评列表
function viewIndexComments(list, total) {
    $('.total-comment').text(total);
    var maxLength = list.length > 3 ? 3 : list.length;
    $('.usercenter-comment').html('');
    if (maxLength > 0) {
        for (var i = 0; i < maxLength; i++) {
            var newItem = $(commentItem).clone();
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
            newItem.find('.head .btn-default').text(list[i].categoryName);
            if (list[i].categoryId == catedata.wup) {
                newItem.find('.head .btn-default').addClass('wuping');
            } else if (list[i].categoryId == catedata.wanp) {
                newItem.find('.head .btn-default').addClass('neight');
            }
            newItem.find('.content').text(list[i].summary);
            newItem.find('.content').attr('data-id', list[i].id);
            newItem.find('.content').click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.comment span').text(list[i].commentCount);
            newItem.find('.zan span').text(list[i].praiseCount);
            newItem.find('.score span').text(list[i].rewardTotal);
            newItem.find('.comment-operation .btn-del').attr({ 'data-id': list[i].id, 'data-title': list[i].title }).click(function () {
                var did = $(this).attr('data-id');
                var title = $(this).attr('data-title');
                confirm('友情提示', '确定要删除' + title + '吗?', '确定', '取消', function () {
                
                    DelComment(did);
                }, function () {
                    alert('您选择了取消操作');
                });
            });
            newItem.show();
            $('.usercenter-comment').append(newItem);
        }
    } else {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.usercenter-comment').append(listHtml.ToString());
    }
}
function viewComments(list, page) {
    $('.list-comment').html('');
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(commentItem).clone();
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
            newItem.find('.head .btn-default').text(list[i].categoryName);
            if (list[i].categoryId == catedata.wup) {
                newItem.find('.head .btn-default').addClass('wuping');
            } else if (list[i].categoryId == catedata.wanp) {
                newItem.find('.head .btn-default').addClass('neight');
            }
            newItem.find('.content').text(list[i].summary);
            newItem.find('.content').attr('data-id', list[i].id);
            newItem.find('.content').click(function () {
                var did = $(this).attr('data-id');
                ToDetail(did);
            });
            newItem.find('.comment span').text(list[i].commentCount);
            newItem.find('.zan span').text(list[i].praiseCount);
            newItem.find('.score span').text(list[i].rewardTotal);
            newItem.find('.comment-operation .btn-del').attr({ 'data-id': list[i].id, 'data-title': list[i].title }).click(function () {
                var did = $(this).attr('data-id');
                var title = $(this).attr('data-title');
                confirm('友情提示', '确定要删除' + title + '吗?', '确定', '取消', function () {
                    DelComment(did);
                }, function () {
                    alert('您选择了取消操作');
                });
            });
            newItem.show();
            $('.list-comment').append(newItem);
        }
    } else if (page == 1) {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.list-comment').append(listHtml.ToString());
    }
}
function DelComment(jid) {
    $.ajax({
        type: 'post',
        url: '/serv/api/article/delete.ashx',
        data: { jid: jid },
        dataType: 'json',
        success: function (resp) {
            if (resp.status) {
                data.sps = $.extend({}, data.bs_sps);
                loadComments(data.sps, true, true, true);
                alert('删除完成');
            } else {
                alert('删除出错');
            }
        }

    });
}
//呈现好友列表
function viewIndexFriends(total) {
    $('.total-friend').text(total);
}
function viewFriends(list, page) {
    if (list.length > 0) {
        for (var i = 0; i < list.length; i++) {
            var newItem = $(friendItem).clone();
            newItem.find('.user-avatar img').attr('src', list[i].avatar);
            newItem.find('.font-nick').text(list[i].userName);
            newItem.find('.btn-jiahaoyou,.btn-delhaoyou,.font-nick,.user-avatar').attr('data-id', list[i].id);
            newItem.find('.btn-jiahaoyou,.btn-delhaoyou').attr('data-nickname', list[i].userName);

            newItem.find('.color-xingji').html('');
            var lv = getUserLevelImgs(list[i].id, list[i].times);
            for (var j = 0; j < lv.imgs.length; j++) {
                if (!!lv.imgs[j]) newItem.find('.color-xingji').append('<img src="' + lv.imgs[j] + '">')
            }
            if (list[i].userHasRelation) {
                newItem.find('.btn-jiahaoyou').remove();
            } else {
                newItem.find('.btn-delhaoyou').remove();
            }
            if ($.trim(list[i].phone) != '') {
                newItem.find('.user-phone span').text(list[i].phone);
            } else {
                newItem.find('.user-phone').text('');
            }
            newItem.find('.user-content').text($.trim(list[i].describe) == "" ? '' : $.trim(list[i].describe));
            newItem.show();
            $('.list-friend').append(newItem);
        }
    } else if (page == 1) {
        var listHtml = new StringBuilder();
        listHtml.AppendFormat('<div class="mTop20 index-comment-item row">');
        listHtml.AppendFormat('<div class="col-xs-12 empty">{0}</div>', '暂无内容');
        listHtml.AppendFormat('</div>');
        $('.list-friend').append(listHtml.ToString());
    }
}

//获取好友列表接口
function GetFriends(option, showProgress, fn) {
    var layerIndex;
    if (showProgress) layerIndex = progress();
    var baseOption = {
        rows: 10,
        page: 1,
        rel_id: '0'
    };
    var option = $.extend(baseOption, option);
    $.ajax({
        type: 'post',
        url: "/Serv/API/User/Friend/List.ashx",
        data: option,
        dataType: "json",
        success: function (resp) {
            if (showProgress) layer.close(layerIndex);
            if (!!resp.status) {
                fn(resp.result);
            } else {
                alert('查询出错');
            }
        }
    });
}
//编辑个人资料
function EditUser() {
    var isCompany = u_type == 6;
    showUpdateInfoDialog(u_type == 6, escapeHTML(u_name), u_avatar, u_describe, u_phone, u_view_type, u_ex3, function (option, layerIndex) {
        var layerIndex1 = progress();
        $.ajax({
            type: 'post',
            url: '/Serv/API/User/Update.ashx',
            data: { data: JSON.stringify(option) },
            dataType: 'JSON',
            success: function (resp) {
                layer.close(layerIndex1);
                if (resp.errcode == 0) {
                    if (!!option.truename) u_name = option.truename;
                    if (!!option.avatar) u_avatar = option.avatar;
                    if (!!option.describe) u_describe = option.describe;
                    if (!!option.phone) u_phone = option.phone;
                    if (!!option.ex3) u_ex3 = option.ex3;
                    if (option.view_type != undefined) u_view_type = option.view_type;

                    if (isCompany) {
                        if (option.avatar) $('.company-info .company-logo').attr('src', option.avatar);
                        if (option.truename) $('.company-info .company-row1').text(option.truename);
                        if (option.phone) $('.company-info .company-phone').text('手机：' + option.phone);
                        $('.company-info .company-describe').text(option.describe);
                        if (option.ex3) $('.right-img .company-img').attr('src', option.ex3);
                    } else {
                        if (option.avatar) $('.user-info .user-logo').attr('src', option.avatar);
                        if (option.truename) $('.user-info .font-nick').text(option.truename);
                        if (option.phone) $('.user-info .user-phone').text('手机：' + option.phone);
                        $('.user-info .user-content').text(option.describe);
                    }
                    layer.close(layerIndex);
                    alert('编辑完成');
                    $('.loginInfo .useravatar').attr('src', option.avatar);
                } else {
                    alert(resp.errmsg);
                }
            }
        });
    })
}

function showUpdateInfoDialog(isCompany, truename, headimg, desc, phone, view_type, ex3, fn) {
    var html = new StringBuilder();
    if (isCompany) {
        if ($.trim(headimg) == '') headimg = "/customize/StockPlayer/Img/clogo.png";
        if ($.trim(ex3) == '') ex3 = "/customize/StockPlayer/Img/zhizhao.png";
        html.AppendFormat('<div class="row dlg-compony">');
        html.AppendFormat(' <div class="col-xs-7">');
        html.AppendFormat('  <div class="row">');
        html.AppendFormat('     <div class="col-xs-3">公司Logo</div><div class="col-xs-9 userhead"><img src="{0}" id="user-head-edit" class="user-edit-logo img-circle" /><input type="file" id="logofile" name="file1" /></div>', headimg);
        html.AppendFormat('     <div class="col-xs-3">公司名称</div><div class="col-xs-9"><input type="text" name="dlg_truename" id="dlg_truename" class="form-control textCheck" maxlength="20" placeholder="请填写公司名称" value="{0}" /></div>', truename);
        html.AppendFormat('     <div class="col-xs-3">公司简介</div><div class="col-xs-9"><textarea class="form-control" id="dlg_description" placeholder="请填写公司简介" rows="5">{0}</textarea></div>', desc);
        html.AppendFormat('  </div>');
        html.AppendFormat(' </div>');
        html.AppendFormat(' <div class="col-xs-5">');
        html.AppendFormat('  <div class="row">');
        html.AppendFormat('     <div class="col-xs-12">营业执照</div>');
        html.AppendFormat('     <div class="col-xs-12"><img src="{0}" id="user-head-ex3" class="user-edit-ex3" /><input type="file" id="ex3file" name="file1" /></div>', ex3);
        html.AppendFormat('  </div>');
        html.AppendFormat(' </div>');
        html.AppendFormat('</div>');
    } else {
        if ($.trim(headimg) == '') headimg = "/img/europejobsites.png";
        html.AppendFormat('<div class="row">');
        html.AppendFormat('<div class="col-xs-3">头像</div><div class="col-xs-9 userhead"><img src="{0}" id="user-head-edit" class="user-edit-logo img-circle" /><input type="file" id="logofile" name="file1" /></div>', headimg);
        html.AppendFormat('<div class="col-xs-3">昵称</div><div class="col-xs-9"><input type="text" name="dlg_truename" id="dlg_truename" class="form-control textCheck" maxlength="20" placeholder="请填写昵称" value="{0}" /></div>', truename);
        html.AppendFormat('<div class="col-xs-3">个人介绍</div><div class="col-xs-9"><textarea class="form-control" id="dlg_description" placeholder="请填写个人简介" rows="5">{0}</textarea></div>', desc);
        html.AppendFormat('<div class="col-xs-3">手机号码</div><div class="col-xs-9"><input type="text" name="dlg_phone" id="dlg_phone" class="form-control textCheck" maxlength="11" placeholder="请填写手机号码（唯一）" data-oldphone="{0}" value="{0}" /></div>', phone);
        html.AppendFormat('<div class="col-xs-3 trCode">验证码</div><div class="col-xs-5 trCode"><input type="text" name="dlg_code" id="dlg_code" class="form-control textCheck" maxlength="6" placeholder="验证码" /></div><div class="col-xs-4 trCode">');
        if (codeIntervalNum > 0) {
            html.AppendFormat('<button class="btn btn-default sendCode" disabled="disabled">等待（{0}）</button></div>', codeIntervalNum);
        } else {
            html.AppendFormat('<button class="btn btn-default sendCode">获取验证码</button></div>');
        }
        html.AppendFormat('<div class="col-xs-3">隐藏手机</div><div class="col-xs-9"><input type="checkbox" class="position-top3" name="dlg_view" id="dlg_view" {0} /><label class="for-view" for="dlg_view">是</label></div>', (view_type == 1 ? 'checked="checked"' : ''));
        html.AppendFormat('</div>');
    }
    layer.open({
        type: 1,
        title: '修改资料',
        skin: 'layui-layer-edit', //样式类名
        closeBtn: 0, //不显示关闭按钮
        move: false,//禁止移动
        area: isCompany ? '590px' : '590px',
        //shift: 2,
        //shadeClose: true, //开启遮罩关闭
        content: html.ToString(),
        btn: ['确认', '取消'], //按钮
        yes: function (index, layero) {
            var option = {};
            option.avatar = $.trim($(layero).find('#user-head-edit').attr('src'));
            option.truename = escapeHTML($.trim($(layero).find('#dlg_truename').val()));

            if (isCompany) {
                if (option.avatar == '/customize/StockPlayer/Img/clogo.png') option.avatar = '';
                option.ex3 = $.trim($(layero).find('#user-head-ex3').attr('src'));
                if (option.ex3 == '/customize/StockPlayer/Img/zhizhao.png') option.ex3 = '';
                option.company = option.truename;
                option.company_is_repeat = '1';
            } else {
                if (option.avatar == '/img/europejobsites.png') option.avatar = '';
                var ophone = $.trim($(layero).find('#dlg_phone').attr('data-oldphone'));
                var nphone = $.trim($(layero).find('#dlg_phone').val());
                if (ophone != nphone) {
                    option.phone = nphone;
                    option.code = $.trim($(layero).find('#dlg_code').val());
                    if (option.code == "") {
                        $(layero).find('#dlg_code').focus();
                        return;
                    }
                    option.check_code = 1;
                }
                option.nickname = option.truename;
                option.view_type = $(layero).find('#dlg_view').get(0).checked ? 1 : 0;
                option.user_is_repeat = '1';
                //option.phone_is_repeat='1';
            }
            option.describe = $.trim($(layero).find('#dlg_description').val());
            if (!!fn) fn(option, index);
            //按钮【按钮一】的回调
        }, btn2: function (index, layero) {
            //console.log('取消', index, layero);
            //按钮【按钮二】的回调
        }
    });
}
function escapeHTML(a) {  
    return a.replace(/"/g, "”").replace(/'/g, "‘");
} 
function showUpdatePwdDialog(fn) {
    var html = new StringBuilder();
    html.AppendFormat('<div class="row">');
    html.AppendFormat('<div class="col-xs-3">密码</div><div class="col-xs-9"><input type="password" name="dlg_password" id="dlg_password" class="form-control textCheck" placeholder="请输入原始密码" /></div>');
    html.AppendFormat('<div class="col-xs-3">新密码</div><div class="col-xs-9"><input type="password" name="dlg_new_password" id="dlg_new_password" class="form-control textCheck" maxlength="20" placeholder="请输入新密码" /></div>');
    html.AppendFormat('<div class="col-xs-3">确认密码</div><div class="col-xs-9"><input type="password" name="dlg_confirm_password" id="dlg_confirm_password" class="form-control textCheck" maxlength="20" placeholder="请输入确认密码" /></div>');
    html.AppendFormat('</div>');
    layer.open({
        type: 1,
        title: '修改密码',
        skin: 'layui-layer-edit', //样式类名
        closeBtn: 0, //不显示关闭按钮
        move: false,//禁止移动
        //shift: 2,
        //shadeClose: true, //开启遮罩关闭
        content: html.ToString(),
        btn: ['确认', '取消'], //按钮
        yes: function (index, layero) {
            var password = $.trim($(layero).find('#dlg_password').val());
            var new_password = $.trim($(layero).find('#dlg_new_password').val());
            var confirm_password = $.trim($(layero).find('#dlg_confirm_password').val());
            if (password == "") {
                $(layero).find('#dlg_password').focus();
                return;
            }
            if (new_password == "") {
                $(layero).find('#dlg_new_password').focus();
                return;
            }
            if (new_password.length < 6) {
                alert('新密码长度限制6-20位');
                $(layero).find('#dlg_new_password').focus();
                return;
            }
            if (confirm_password == "") {
                $(layero).find('#dlg_confirm_password').focus();
                return;
            }
            if (new_password != confirm_password) {
                alert('新密码和确认密码不一致');
                if (!$(layero).find('#dlg_confirm_password').hasClass('textError')) $(layero).find('#dlg_confirm_password').addClass('textError');
                $(layero).find('#dlg_confirm_password').focus();
                return;
            }
            if (!!fn) fn(password, new_password, confirm_password, index);
            //按钮【按钮一】的回调
        }, btn2: function (index, layero) {
            //console.log('取消', index, layero);
            //按钮【按钮二】的回调
        }
    });
}