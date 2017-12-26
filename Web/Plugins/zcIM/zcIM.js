
var zcIM = function () {
    this.initModule();
}
zcIM.fn = zcIM.prototype;
//组件初始化
zcIM.fn.initModule = function () {
    this.initBaseData();
    this.initBase();
    this.initEmoji();
}
//初始化组件数据
zcIM.fn.initBaseData = function () {
    zcIM.support = false;
    zcIM.debug = false;
    zcIM.nimDebug = false;
    zcIM.curScrollBottom = 0;
    zcIM.data = {};
}
zcIM.fn.initCurBaseData = function (avatar, userName) {
    if (!zcIM.data[zcIM.accid]) {
        zcIM.data[zcIM.accid] = {};
        zcIM.data[zcIM.accid].searchFriendKeyword = '';
        zcIM.data[zcIM.accid].curchat = {};
        zcIM.data[zcIM.accid].unread = 0;
        zcIM.data[zcIM.accid].teams = [];
        zcIM.data[zcIM.accid].teamMembers = {};
        zcIM.data[zcIM.accid].sessions = [];
        zcIM.data[zcIM.accid].friends = [];
        zcIM.data[zcIM.accid].team = false;
        zcIM.data[zcIM.accid].teamNum = 0;
        zcIM.data[zcIM.accid].session = false;
        zcIM.data[zcIM.accid].friend = false;
        zcIM.data[zcIM.accid].friendslist = [];
        zcIM.data[zcIM.accid].msgs = {};
        zcIM.data[zcIM.accid].sysMsgs = [];
        zcIM.data[zcIM.accid].persons = {};
        zcIM.data[zcIM.accid].persons[zcIM.accid] = { to: zcIM.accid, toname: userName, ico: avatar, isget: true}
        zcIM.data[zcIM.accid].sendFileProgress = 0;
    }
}
//初始化组件html
zcIM.fn.initBase = function () {
    zcIM.body = $(document.body);
    $('.IM').last().remove();
    zcIM.IM = $('<div class="IM"></div>');
    zcIM.body.append(zcIM.IM);
    //聊天提示
    zcIM.IM.IMTip = $('<div class="IM-tip IM-hidden"></div>');
    zcIM.IM.IMTip.append('<div class="IM-tip-l"><img class="IM-tip-email" src="/img/email.png" /></div>');
    zcIM.IM.IMTip.append('<div class="IM-tip-m"><span class="IM-tip-content">私信聊天</span></div>');
    zcIM.IM.IMTip.append('<div class="IM-tip-r"><div class="IM-tip-r-m"><div class="IM-tip-r-ico"></div></div></div>');
    zcIM.IM.IMTip.click(function () {
        zcIM.fn.showChat();
    });
    zcIM.IM.append(zcIM.IM.IMTip);

    //进度遮挡
    zcIM.IM.IMProgress = $('<div class="IM-progress IM-hidden"><div class="IM-progress-cell"><div class="IM-progress-progress"><div class="IM-progress-in"></div><div class="IM-progress-text"></div></div></div></div>');
    zcIM.IM.append(zcIM.IM.IMProgress);

    //好友Panel
    zcIM.IM.IMChat = $('<div class="IM-chat IM-hidden"></div>');
    zcIM.IM.append(zcIM.IM.IMChat);

    //好友列表
    zcIM.IM.Friends = $('<div class="IM-friends"></div>');

    var searchHtml = new StringBuilder();
    searchHtml.AppendFormat('<div class="IM-friends-search">');
    searchHtml.AppendFormat('<div class="div-search">');
    searchHtml.AppendFormat('<input class="txt-search" type="text" maxlength="20" placeholder="查找联系人" />');
    searchHtml.AppendFormat('<img class="ico-search" src="/img/search.png" />');
    searchHtml.AppendFormat('</div>');
    searchHtml.AppendFormat('<div class="div-jia">');
    searchHtml.AppendFormat('<img class="ico-jia" src="/img/jia.png" />');
    searchHtml.AppendFormat('</div>');
    searchHtml.AppendFormat('</div>');

    zcIM.IM.Friends.append(searchHtml.ToString());
    zcIM.IM.Friends.find('.txt-search').keypress(function () {
        var theEvent = window.event || arguments.callee.caller.arguments[0];
        if (theEvent.keyCode == 13) {
            zcIM.fn.searchFriends();
            return false;
        }
    });
    zcIM.IM.Friends.find('.ico-jia').click(function () {
        zcIM.IM.Users.removeClass('IM-hidden');
    });
    zcIM.IM.Friends.find('.ico-search').click(function () {
        zcIM.fn.searchFriends();
    });
    zcIM.IM.Friends.List = $('<div class="IM-friends-list"></div>');
    $(document).on('click', '.IM .IM-friends-list .Friend-li', function () {
        zcIM.data[zcIM.accid].curchat.scene = $(this).attr('data-scene');
        zcIM.data[zcIM.accid].curchat.to = $(this).attr('data-to');
        zcIM.data[zcIM.accid].curchat.toname = $(this).attr('data-toname');
        zcIM.data[zcIM.accid].curchat.ico = $(this).attr('data-img');
        if (!$(this).hasClass('Selected')) {
            zcIM.IM.Friends.List.find('.Selected').removeClass('Selected');
            $(this).addClass('Selected');
            zcIM.IM.Form.find('.IM-chat-list').html('');
            zcIM.IM.Form.find('.txt-sendbox').text('按回车发送私信');
            zcIM.IM.Form.find('.txt-sendbox').css('color', '#808080');
            if (zcIM.data[zcIM.accid].curchat.scene == 'sys') {
                zcIM.fn.getCurSysMsgs(false, true);
            } else {
                zcIM.fn.getCurMsgs(false);
            }
        }
        if (zcIM.data[zcIM.accid].curchat.scene == 'sys') {
            zcIM.IM.Form.find('.IM-chat-list').css('bottom', '0px');
            if (!zcIM.IM.Form.find('.IM-chat-sendbox').hasClass('IM-hidden')) {
                zcIM.IM.Form.find('.IM-chat-sendbox').addClass('IM-hidden');
            }
        } else {
            zcIM.IM.Form.find('.IM-chat-list').css('bottom', '42px');
            if (zcIM.IM.Form.find('.IM-chat-sendbox').hasClass('IM-hidden')) {
                zcIM.IM.Form.find('.IM-chat-sendbox').removeClass('IM-hidden');
            }
        }
        zcIM.IM.Form.find('.to-user').text(zcIM.data[zcIM.accid].curchat.toname);
        if (zcIM.IM.Form.find('.to-avatar-img').length > 0) {
            zcIM.IM.Form.find('.to-avatar-img').attr('src', zcIM.data[zcIM.accid].curchat.ico);
        } else {
            zcIM.IM.Form.find('.to-avatar').append('<img class="to-avatar-img" src="' + zcIM.data[zcIM.accid].curchat.ico + '" />');
        }
    });
    zcIM.IM.Friends.append(zcIM.IM.Friends.List);
    zcIM.IM.IMChat.append(zcIM.IM.Friends);
    //输入区域
    zcIM.IM.Form = $('<div class="IM-form"></div>');
    zcIM.IM.Form.append('<div class="IM-chat-head"><div class="to-avatar"></div><div class="to-user">联系人</div><div class="chat-close">X</div></div>');
    zcIM.IM.Form.append('<div class="IM-chat-list"></div>');
    zcIM.IM.Form.append('<div class="IM-chat-sendbox IM-hidden"><div class="txt-sendbox" contenteditable="true">按回车发送私信</div><div class="emojiTag"></div><img class="sendbox-face" src="/img/face.png" /><input class="sendbox-img-file" type="file" accept="image/*" /><img class="sendbox-img" src="/img/img.png" /><input class="sendbox-file-file" type="file" /><img class="sendbox-file" src="/img/file.png" /></div>');
    $(document).on('click', '.IM .IM-chat-list .u-localmsg', function () {
        if (zcIM.data[zcIM.accid].curchat.scene == 'sys') {
            zcIM.fn.getCurSysMsgs(true);
        }
        else {
            zcIM.fn.getCurMsgs(true);
        }
    });
    $(document).on('click', '.IM .IM-chat-list .j-pass', function () {
        var obj = this;
        var obj1 = $(this).closest('.item').find('.j-reject');
        var idServer = $(this).closest('.item').attr('data-idServer');
        var account = $(obj).attr('data-account');
        var nickname = $(obj).attr('data-nick');
        var avatar = $(obj).attr('data-ico');
        passFriendApply(account, nickname, avatar, idServer, function (err, data) {
            if (!err) {
                //标记已读本地系统消息
                zcIM.fn.markSysMsgRead('idServer', idServer, function (err, data) { });
                //删除本地系统消息
                //zcIM.fn.deleteLocalSysMsg(idServer, function (err, data) { });
                $(obj).remove();
                $(obj1).remove();
            }
        });
    });
    $(document).on('click', '.IM .IM-chat-list .j-reject', function () {
        var obj = this;
        var obj1 = $(this).closest('.item').find('.j-pass');
        var idServer = $(this).closest('.item').attr('data-idServer');
        var account = $(obj).attr('data-account');
        var nickname = $(obj).attr('data-nick');
        rejectFriendApply(account, nickname, idServer, function (err, data) {
            if (!err) {
                //标记已读本地系统消息
                zcIM.fn.markSysMsgRead('idServer', idServer, function (err, data) { });
                //删除本地系统消息
                //zcIM.fn.deleteLocalSysMsg(idServer, function (err, data) { });
                $(obj).remove();
                $(obj1).remove();

                //$('.IM .IM-chat-list .item chat-item item-you').each(function (k, v) {
                //    var oo = $(this);
                //    var reject = $(this).find('.j-reject').attr('data-account');
                //    var sqject = $(this).find('.j-pass');
                //    var idServ = $(this).attr('data-idserver');
                //    if (account == reject) {
                //        zcIM.fn.markSysMsgRead('idServer', idServ, function (err, data) { });
                //        $(oo).find('.j-reject').remove();
                //        $(sqject).remove();
                //    }
                //});



            }
        });
    });
    $(document).on('click', '.IM .IM-chat-list .j-read', function () {
        var obj = this;
        var idServer = $(this).closest('.item').attr('data-idServer');
        //删除本地系统消息
        //zcIM.fn.deleteLocalSysMsg(idServer, function (err, data) {
        //    if (!err) {
        //        $(obj).remove();
        //    }
        //});
        //标记已读本地系统消息
        zcIM.fn.markSysMsgRead('idServer', idServer, function (err, data) {
            if (!err) {
                $(obj).remove();
            }
        });
    });
    zcIM.IM.Form.find('.chat-close').click(function () {
        zcIM.fn.showChat();
    });
    zcIM.IM.Form.find('.sendbox-img').click(function () {
        zcIM.IM.Form.find('.sendbox-img-file').click();
    });
    zcIM.IM.Form.find('.sendbox-img-file').change(function () {
        zcIM.fn.uploadFile('.sendbox-img-file');
    });
    zcIM.IM.Form.find('.sendbox-file').click(function () {
        zcIM.IM.Form.find('.sendbox-file-file').click();
    });
    zcIM.IM.Form.find('.sendbox-file-file').change(function () {
        zcIM.fn.uploadFile('.sendbox-file-file');
    });
    zcIM.IM.Form.find('.txt-sendbox').keypress(function () {
        var theEvent = window.event || arguments.callee.caller.arguments[0];
        if (theEvent.keyCode == 13) {
            var txt = $.trim($(this).text());
            if (txt != '') {
                var scene = zcIM.data[zcIM.accid].curchat.scene,
                    to = zcIM.data[zcIM.accid].curchat.to;
                zcIM.fn.sendTextMessage(scene, to, txt, zcIM.fn.sendMsgDone.bind(this));
            }
            return false;
        }
    });
    zcIM.IM.Form.find('.txt-sendbox').blur(function () {
        var txt = $.trim($(this).text());
        if (txt == '' || txt == '按回车发送私信') {
            $(this).css('color', '#808080');
            $(this).text('按回车发送私信');
        }
    });
    zcIM.IM.Form.find('.txt-sendbox').focus(function () {
        var txt = $.trim($(this).text());
        if (txt == '' || txt == '按回车发送私信') {
            $(this).css('color', '#333');
            $(this).text('');
        }
    });
    zcIM.IM.IMChat.append(zcIM.IM.Form);

    //用户搜索弹出框
    zcIM.IM.Users = $('<div class="IM-Users IM-hidden"></div>');//IM-hidden
    searchHtml = new StringBuilder();
    searchHtml.AppendFormat('<div class="IM-User-form">');
    searchHtml.AppendFormat('<div class="IM-User-Search">');
    searchHtml.AppendFormat('<span class="search-label">手机：</span>');
    searchHtml.AppendFormat('<input type="text" class="search-text search-phone" maxlength="15" />');
    searchHtml.AppendFormat('<span class="search-label">昵称：</span>');
    searchHtml.AppendFormat('<input type="text" class="search-text search-nick" maxlength="15" />');
    searchHtml.AppendFormat('<button type="button" class="btn btn-info search-btn">查询</button>');
    searchHtml.AppendFormat('<button type="button" class="btn btn-default search-close">关闭</button>');
    searchHtml.AppendFormat('</div>');
    searchHtml.AppendFormat('<div class="IM-User-List row">');
    searchHtml.AppendFormat('</div>');
    searchHtml.AppendFormat('<div class="IM-User-Bottom">');
    searchHtml.AppendFormat('</div>');
    searchHtml.AppendFormat('</div>');
    zcIM.IM.Users.append(searchHtml.ToString());
    zcIM.IM.Users.find('.search-close').click(function () {
        zcIM.IM.Users.find('.search-phone').val('');
        zcIM.IM.Users.find('.search-nick').val('');
        zcIM.IM.Users.find('.IM-User-List').html('');
        zcIM.IM.Users.addClass('IM-hidden');
    });
    zcIM.IM.Users.find('.search-btn').click(function () {
        var phone = $.trim(zcIM.IM.Users.find('.search-phone').val());
        var nick = $.trim(zcIM.IM.Users.find('.search-nick').val());
        if (phone == "" && nick == "") return;
        zcIM.fn.findUsers(phone, nick);
    });
    $(document).on('click', '.IM .IM-Users .list-add-friend', function () {
        var id = $.trim($(this).closest('.list-user').attr('data-id'));
        var nickname = $.trim($(this).closest('.list-user').find('.list-username').text());
        applyFriend(id, nickname);
    });

    zcIM.IM.append(zcIM.IM.Users);
}

/*****************************************************************
 * emoji模块
 ****************************************************************/
zcIM.fn.initEmoji = function () {
    this.$showEmoji = $('.IM .IM-chat-sendbox .sendbox-face');
    this.$showEmoji.on('click', this.showEmoji.bind(this))
    var that = this,
        emojiConfig = {
            'emojiList': emojiList,  //普通表情
            'pinupList': pinupList,  //贴图
            'width': 370,
            'height': 230,
            'imgpath': 'http://static-files.socialcrmyun.com/lib/emoji/',
            'callback': function (result) {
                that.cbShowEmoji(result)
            }
        }
    this.$emNode = new CEmojiEngine($('.IM .IM-chat-sendbox .emojiTag')[0], emojiConfig)
}
/**
 * 选择表情回调
 * @param  {objcet} result 点击表情/贴图返回的数据
 */
zcIM.fn.cbShowEmoji = function (result) {
    if (!!result) {
        var scene = zcIM.data[zcIM.accid].curchat.scene,
            to = zcIM.data[zcIM.accid].curchat.to;
        // 贴图，发送自定义消息体
        if (result.type === "pinup") {
            var index = Number(result.emoji) + 1
            var content = {
                type: 3,
                data: {
                    catalog: result.category,
                    chartlet: result.category + '0' + (index >= 10 ? index : '0' + index)
                }
            }
            zcIM.fn.sendCustomMessage(scene, to, content, zcIM.fn.sendMsgDone.bind(this))
        } else {
            // 表情，内容直接加到输入框
            var txt = $.trim(zcIM.IM.Form.find('.txt-sendbox').text());
            if (txt == '' || txt == '按回车发送私信') {
                zcIM.IM.Form.find('.txt-sendbox').text(result.emoji);
            }
            else {
                txt = txt + result.emoji;
                zcIM.IM.Form.find('.txt-sendbox').text(txt);
            }
            zcIM.IM.Form.find('.txt-sendbox').focus();
            if (zcIM.IM.Form.find('.txt-sendbox').createTextRange) {
                var rtextRange = zcIM.IM.Form.find('.txt-sendbox').createTextRange();
                rtextRange.moveStart('character', txt.length);
                rtextRange.collapse(true);
                rtextRange.select();
            }
            else if (zcIM.IM.Form.find('.txt-sendbox').selectionStart) zcIM.IM.Form.find('.txt-sendbox').selectionStart = zcIM.IM.Form.find('.txt-sendbox').value.length;
        }
    }
}
zcIM.fn.showEmoji = function () {
    this.$emNode._$show()
}
//提示文字闪烁
zcIM.fn.tipTwinkle = function () {
    zcIM.IM.IMTip.find('.IM-tip-m').addClass('IM-tip-m-twinkle')
}
//提示文字闪烁关闭
zcIM.fn.tipTwinkleHide = function () {
    zcIM.IM.IMTip.find('.IM-tip-m-twinkle').removeClass('IM-tip-m-twinkle')
}
//查询联系人列表
zcIM.fn.searchFriends = function () {
    zcIM.fn.refreshFriendListUI()
}
//提示，聊天切换
zcIM.fn.showChat = function () {
    if (zcIM.IM.IMChat.hasClass('IM-hidden')){
        if (!!zcIM.data[zcIM.accid].curchat
        && !!zcIM.data[zcIM.accid].curchat.scene && !!zcIM.data[zcIM.accid].curchat.to) {
            zcIM.fn.resetSessionUnread(zcIM.data[zcIM.accid].curchat.scene + '-' + zcIM.data[zcIM.accid].curchat.to);
            zcIM.fn.setFriendLiUnreadZero(zcIM.data[zcIM.accid].curchat.scene, zcIM.data[zcIM.accid].curchat.to);
            if (!zcIM.IM.Friends.List.find('.Friend-li[data-to="' + zcIM.data[zcIM.accid].curchat.to + '"]').hasClass('Selected')) {
                zcIM.IM.Friends.List.find('.Friend-li[data-to="' + zcIM.data[zcIM.accid].curchat.to + '"]').addClass('Selected');
            }
            zcIM.IM.Friends.List.find('.Friend-li[data-to="' + zcIM.data[zcIM.accid].curchat.to + '"] .Friend-unread').last().remove();
            zcIM.IM.IMChat.removeClass('IM-hidden');
            zcIM.IM.IMTip.addClass('IM-hidden');
            var endScrollHeight = zcIM.IM.Form.find('.IM-chat-list').get(0).scrollHeight;
            zcIM.IM.Form.find('.IM-chat-list').scrollTop(endScrollHeight);
        }
        else{
            zcIM.IM.IMChat.removeClass('IM-hidden');
            zcIM.IM.IMTip.addClass('IM-hidden');
        }
    }
    else {
        zcIM.IM.IMTip.removeClass('IM-hidden');
        zcIM.IM.IMChat.addClass('IM-hidden');
        zcIM.data[zcIM.accid].curchat = {};
        zcIM.fn.getFriendslistUnreadTotal();
        zcIM.fn.refreshFriendListUI();
        zcIM.IM.Form.find('.IM-chat-list').html('');
        zcIM.IM.Form.find('.txt-sendbox').text('按回车发送私信');
        zcIM.IM.Form.find('.txt-sendbox').css('color', '#808080');
        zcIM.IM.Form.find('.to-avatar').html('');
        zcIM.IM.Form.find('.to-user').html('联系人');
    }
}
//用户登录初始化云信
zcIM.fn.login = function (accid, token, avatar, userName) {
    zcIM.accid = accid;
    zcIM.token = token;
    if (zcIM.accid && zcIM.token!='' && zcIM.appkey) {
        zcIM.fn.initCurBaseData(avatar, userName);
        zcIM.fn.initNIM();
        return;
    }
    //如果有登录localStorage 自动登陆 因跳过了登录判断故注释掉
    //var zcIM_loginString = localStorage.getItem("zcIM_login");
    //if ($.trim(zcIM_loginString) != "") {
    //    var zcIM_login = JSON.parse(zcIM_loginString);
    //    zcIM.appkey = zcIM_login.appkey;
    //    zcIM.accid = zcIM_login.accid;
    //    zcIM.token = zcIM_login.token;
    //    zcIM.fn.initNIM();
    //    return;
    //}
    $.ajax({
        type: 'post',
        url: "/Serv/API/User/GetCurUserIMToken.ashx",
        dataType: "json",
        success: function (resp) {
            if (resp.status && resp.result.token!='') {
                zcIM.appkey = resp.result.appkey;
                zcIM.accid = resp.result.accid;
                zcIM.token = resp.result.token;
                zcIM.fn.initCurBaseData(avatar, userName);
                zcIM.fn.initNIM();
            }
        }
    });
}
zcIM.fn.loginout = function () {
    if (zcIM.nim) zcIM.nim.disconnect();
    if (!zcIM.IM.IMTip.hasClass('IM-hidden')) zcIM.IM.IMTip.addClass('IM-hidden');
    if (!zcIM.IM.IMChat.hasClass('IM-hidden')) zcIM.IM.IMChat.addClass('IM-hidden');
    if (!zcIM.IM.IMProgress.hasClass('IM-hidden')) zcIM.IM.IMProgress.addClass('IM-hidden');

    zcIM.IM.Form.find('.to-avatar').html('');
    zcIM.IM.Form.find('.to-user').html('联系人');
    zcIM.IM.Form.find('.IM-chat-list').html('');
    zcIM.IM.Form.find('.txt-sendbox').text('按回车发送私信');
    zcIM.IM.Form.find('.txt-sendbox').css('color', '#808080');
    if (!zcIM.IM.Form.find('.IM-chat-sendbox').hasClass('IM-hidden')) zcIM.IM.Form.find('.IM-chat-sendbox').addClass('IM-hidden');
}
//云信插件初始化
zcIM.fn.initNIM = function () {
    if (zcIM.nim) {
        if (zcIM.nim.options.account == zcIM.accid) {
            zcIM.nim.connect();
            zcIM.IM.IMTip.removeClass('IM-hidden');
            getFriendslistUnreadTotal();
            refreshFriendListUI();
            return;
        }
    }
    window.nim = this.nim = zcIM.nim = new NIM({
        //控制台日志，上线时应该关掉
        //debug: true || { api: 'info', style: 'font-size:14px;color:blue;background-color:rgba(0,0,0,0.1)' },
        debug: zcIM.nimDebug,
        appKey: zcIM.appkey,
        account: zcIM.accid,
        token: zcIM.token,
        //连接
        onconnect: onConnect.bind(this),
        //ondisconnect: onDisconnect.bind(this),
        //onerror: onError.bind(this),
        //onwillreconnect: onWillReconnect.bind(this),
        //// 多端登录变化
        //onloginportschange:onLoginPortsChange.bind(this),
        // 群
        //onteams: onTeams.bind(this),
        //syncTeamMembers: true,//是否加载群成员
        //// onupdateteammember: onUpdateTeamMember.bind(this),
        //onteammembers: onTeamMembers.bind(this),
        //消息
        //syncMsgReceipts:true,
        onmsg: onMsg.bind(this),
        onroamingmsgs: onRoamingMsgs.bind(this),
        onofflinemsgs: onOfflineMsgs.bind(this),
        //会话
        onsessions: onSessions.bind(this),
        onupdatesession: onUpdateSession.bind(this),
        ////同步完成
        //// onsyncteammembersdone: onSyncTeamMembersDone.bind(this),
        onsyncdone: onSyncDone.bind(this),

        ////个人信息
        //onmyinfo:onMyInfo.bind(this),
        //onupdatemyinfo:onMyInfo.bind(this),
        //syncFriendUsers: true, //是否加载好友卡片
        //onusers: onUsers.bind(this),
        //系统通知
        onsysmsg: onSysMsg.bind(this),
        onofflinesysmsgs: onOfflineSysmsgs.bind(this),
        onsysmsgunread: onSysMsgUnread.bind(this),
        onupdatesysmsg: onUpdateSysMsg.bind(this, 0),
        //oncustomsysmsg:onCustomSysMsg.bind(this),
        //onofflinecustomsysmsgs:onOfflineCustomSysMsgs.bind(this),
        //// 静音，黑名单，好友
        //onmutelist:onMutelist.bind(this),
        //onblacklist: onBlacklist.bind(this),
        //onfriends: onFriends.bind(this)//,
        //onsynccreateteam:onSyncCreateteam.bind(this),
        //onsyncmarkinblacklist:onSyncMarkinBlacklist.bind(this),
        //onsyncmarkinmutelist:onSyncMarkinMutelist.bind(this),
        //onsyncfriendaction:onSyncFriendAction.bind(this)
    });

    //云信连接成功显示组件
    function onConnect() {
    }
    //加载群
    function onTeams(teams) {
        zcIM.data[zcIM.accid].teams = zcIM.nim.mergeTeams(zcIM.data[zcIM.accid].teams, teams);
        zcIM.data[zcIM.accid].teams = zcIM.nim.cutTeams(zcIM.data[zcIM.accid].teams, teams.invalid);
        getFriendListByTeam();
        zcIM.data[zcIM.accid].team = true;
        zcIM.data[zcIM.accid].teamNum = zcIM.data[zcIM.accid].teams.length;
    }
    //加载群成员
    function onTeamMembers(obj) {
        if (zcIM.debug) console.log('收到群成员', obj);
        var teamId = obj.teamId;
        var members = obj.members;
        //if (!zcIM.data[zcIM.accid].teamMembers[teamId]) zcIM.data[zcIM.accid].teamMembers[teamId] = [];
        zcIM.data[zcIM.accid].teamMembers[teamId] = nim.mergeTeamMembers(zcIM.data[zcIM.accid].teamMembers[teamId], members);
        zcIM.data[zcIM.accid].teamMembers[teamId] = nim.cutTeamMembers(zcIM.data[zcIM.accid].teamMembers[teamId], members.invalid);
        for (var i = 0; i < zcIM.data[zcIM.accid].teamMembers[teamId].length; i++) {
            var lis = zcIM.data[zcIM.accid].persons[zcIM.data[zcIM.accid].teamMembers[teamId][i].account];
            if (lis) continue;
            zcIM.data[zcIM.accid].persons[zcIM.data[zcIM.accid].teamMembers[teamId][i].account] = { to: zcIM.data[zcIM.accid].teamMembers[teamId][i].account, toname: $.trim(zcIM.data[zcIM.accid].teamMembers[teamId][i].alias), ico: "", isget: false };
        }
    }
    //加载私信列表
    function onSessions(sessions) {
        zcIM.data[zcIM.accid].sessions = zcIM.nim.mergeSessions(zcIM.data[zcIM.accid].sessions, sessions);
        zcIM.data[zcIM.accid].session = true;
        //getPersonBySession(sessions);
        //if (zcIM.debug) console.log("sessions", zcIM.data[zcIM.accid].sessions);
    }
    //更新私信列表
    function onUpdateSession(session) {
        if (zcIM.debug) console.log('会话更新了', session);
        zcIM.data[zcIM.accid].sessions = zcIM.nim.mergeSessions(zcIM.data[zcIM.accid].sessions, session);
        if (session.unread == 0) return;

        if (zcIM.IM.IMChat.hasClass('IM-hidden')) {
            zcIM.fn.setFriendLiUnreadNum(session.scene, session.to, session.unread);
            zcIM.fn.getFriendslistUnreadTotal();
            zcIM.fn.refreshFriendListUI();
        }
        else if (zcIM.data[zcIM.accid].curchat.to && to != zcIM.data[zcIM.accid].curchat.to) {
            zcIM.fn.setFriendLiUnreadNum(session.scene, session.to, session.unread);
            zcIM.fn.refreshFriendLiUI(scene, to);
        }
    }
    //私信列表转联系人列表
    function getPersonBySession(sessions) {
        if (!Array.isArray(sessions)) { sessions = [sessions]; }
        for (var i = 0; i < sessions.length; i++) {
            var scene = sessions[i].scene,
                to = sessions[i].to
            unread = sessions[i].unread;
            if (scene === "p2p") {
                var lis = zcIM.data[zcIM.accid].persons[to];
                if (!lis) {
                    zcIM.data[zcIM.accid].persons[to] = { to: to, toname: "", ico: "", isget: false };
                }
                var lifs = $.grep(zcIM.data[zcIM.accid].friendslist, function (cur, z) {
                    return cur['to'] == to && cur['scene'] == "p2p";
                });
                if (lifs.length == 0) {
                    zcIM.data[zcIM.accid].friendslist.push({ scene: "p2p", to: to, toname: "", ico: "", unread: unread });
                }
                else {
                    for (var j = 0; j < lifs.length; j++) {
                        lifs[j].unread = unread;
                    }
                }
            }
            else {
                var lifs = $.grep(zcIM.data[zcIM.accid].friendslist, function (cur, z) {
                    return cur['to'] == to && cur['scene'] == "team";
                });
                if (lifs.length == 0) {
                    zcIM.data[zcIM.accid].friendslist.push({ scene: "team", to: to, toname: "", ico: "", unread: unread });
                }
                else {
                    for (var j = 0; j < lifs.length; j++) {
                        lifs[j].unread = unread;
                    }
                }
            }
        }
    }
    //加载好友列表
    function onFriends(friends) {
        zcIM.data[zcIM.accid].friends = zcIM.nim.mergeFriends(zcIM.data[zcIM.accid].friends, friends);
        zcIM.data[zcIM.accid].friends = zcIM.nim.cutFriends(zcIM.data[zcIM.accid].friends, friends.invalid);
        for (var i = 0; i < zcIM.data[zcIM.accid].friends.length; i++) {
            var lis = $.grep(zcIM.data[zcIM.accid].friendslist, function (cur, z) { return cur['to'] == zcIM.data[zcIM.accid].friends[i].account && cur['scene'] == "p2p"; });
            if (lis.length > 0) return;
            zcIM.data[zcIM.accid].friendslist.push({ scene: "p2p", to: zcIM.data[zcIM.accid].friends[i].account, toname: $.trim(zcIM.data[zcIM.accid].friends[i].alias), ico: "", unread: 0 });
        }
        if (zcIM.debug) console.log("friends", zcIM.data[zcIM.accid].friends);
    }
    //加载好友卡片
    function onUsers(users) {
        zcIM.fn.onUsers(users);
    }
    //加载用户（好友，联系人，群成员）信息
    function getPersonsInfo(callback) {
        zcIM.fn.getPersonsInfo(callback);
    }
    //群列表转联系人列表
    function getFriendListByTeam() {
        var teamIds = [];
        for (var i = 0; i < zcIM.data[zcIM.accid].teams.length; i++) {
            var lis = $.grep(zcIM.data[zcIM.accid].friendslist, function (cur, z) { return cur['to'] == zcIM.data[zcIM.accid].teams[i].teamId && cur['scene'] == "team"; });
            if (lis.length > 0) return;
            var ico = $.trim(zcIM.data[zcIM.accid].teams[i].avatar) == '' ? '/img/europejobsites.png' : $.trim(zcIM.data[zcIM.accid].teams[i].avatar);
            zcIM.data[zcIM.accid].friendslist.push({ scene: "team", to: zcIM.data[zcIM.accid].teams[i].teamId, toname: $.trim(zcIM.data[zcIM.accid].teams[i].name), ico: ico, unread: 0 });
        }
    }
    //初始化时收到系统消息
    function onSysMsg(sysMsg) {
        if (sysMsg == 0) return;
        console.log('收到系统通知', sysMsg);
        var ol = zcIM.fn.findFriendLi('sys', 'sys')
        if (!ol) {
            zcIM.data[zcIM.accid].friendslist.push({ scene: "sys", to: 'sys', toname: '系统消息', ico: '/lib/emoji/emoji/emoji_84.png', unread: 1 ,online:true});
        } else {
            ol.unread = ol.unread + 1;
        }
        zcIM.fn.getFriendslistUnreadTotal();
        zcIM.fn.refreshFriendListUI();
    }
    //收到的系统消息
    function onUpdateSysMsg(sysMsg) {
        if (sysMsg == 0) return;
        console.log('收到系统通知', sysMsg);
        var ol = zcIM.fn.findFriendLi('sys', 'sys')
        if (!ol) {
            zcIM.data[zcIM.accid].friendslist.push({ scene: "sys", to: 'sys', toname: '系统消息', ico: '/lib/emoji/emoji/emoji_84.png', unread: 1, online: true });
        } else {
            ol.unread = ol.unread + 1;
        }
        zcIM.fn.getFriendslistUnreadTotal();
        zcIM.fn.refreshFriendListUI();
    }
    function onOfflineSysmsgs(sysMsgs) {
        console.log('收到离线系统通知', sysMsgs);
        //pushSysMsgs(sysMsg);
    }
    //系统消息未读数
    function onSysMsgUnread(obj) {
        if (zcIM.debug) console.log('系统消息未读数', obj);
        if (obj.total > 0) {
            var ol = zcIM.fn.findFriendLi("sys", 'sys')
            if (!ol) {
                zcIM.data[zcIM.accid].friendslist.push({ scene: "sys", to: 'sys', toname: '系统消息', ico: '/lib/emoji/emoji/emoji_84.png', unread: obj.total, online: true });
            } else {
                ol.unread = obj.total;
            }
        }
    }
    //获取漫游消息
    function onRoamingMsgs(obj) {
        if (zcIM.debug) console.log('漫游消息', obj);
        zcIM.fn.pushMsg(obj.msgs);
    }
    //获取离线消息
    function onOfflineMsgs(obj) {
        if (zcIM.debug) console.log('离线消息', obj);
        zcIM.fn.pushMsg(obj.msgs);
    }
    //获取消息
    function onMsg(msg) {
        if (zcIM.debug) console.log('收到消息', msg.scene, msg.type, msg);
        zcIM.fn.pushMsg(msg);
    }

    //所有初始同步完成
    function onSyncDone() {
        getFriendList(function (list) {
            if (zcIM.debug) console.log('accid.data', zcIM.data[zcIM.accid]);
            if (zcIM.debug) console.log('list', list);
            var scene = 'p2p';
            var fids = [];
            for (var i = 0; i < list.length; i++) {
                var to = list[i].id;
                fids.push(to);
                var sessionId = scene + '-' + to;
                var session = zcIM.fn.findSession(sessionId);
                var unread = !!session ? session.unread : 0;
                var oFriend = zcIM.fn.findFriendLi(scene, to);
                if (!oFriend) {
                    zcIM.data[zcIM.accid].friendslist.push({ scene: scene, to: to, toname: list[i].userName, ico: list[i].avatar, unread: unread, online: false });
                } else {
                    oFriend.toname = list[i].userName;
                    oFriend.ico = list[i].avatar;
                    oFriend.unread = unread;
                }
                zcIM.data[zcIM.accid].persons[to] = { to: to, toname: list[i].userName, ico: list[i].avatar, isget: true };
            }
            if (list.length > 0) {
                //发送好友列表到websocket
                sendFriendIds(fids);
            }
            zcIM.fn.getFriendslistUnreadTotal();
            zcIM.fn.refreshFriendListUI();
            zcIM.IM.IMTip.removeClass('IM-hidden');
            //console.log(list);
        });
        //getPersonsInfo();
    }

    //未读总数
    function getFriendslistUnreadTotal() {
        zcIM.fn.getFriendslistUnreadTotal();
    }
    //刷新联系人列表UI
    function refreshFriendListUI() {
        zcIM.fn.refreshFriendListUI();
    }
}
//统计未读信息总数
zcIM.fn.getFriendslistUnreadTotal = function () {
    var unread = 0;
    var lis = $.grep(zcIM.data[zcIM.accid].friendslist, function (cur, z) { return cur['unread'] > 0; });
    for (var i = 0; i < lis.length; i++) {
        unread = unread + lis[i].unread;
    }
    if (unread > 0) {
        zcIM.IM.IMTip.find('.IM-tip-content').html('您有' + unread + '条未读私信');
        zcIM.fn.tipTwinkle();
    } else {
        zcIM.IM.IMTip.find('.IM-tip-content').html('私信聊天');
        zcIM.fn.tipTwinkleHide();
    }
    zcIM.data[zcIM.accid].unread = unread;
}
//生成联系人列表行
zcIM.fn.getFriendLiHtml = function (li) {
    var liHtml = new StringBuilder();
    if (!zcIM.IM.IMChat.hasClass('IM-hidden') && !!zcIM.data[zcIM.accid].curchat && (zcIM.data[zcIM.accid].curchat.scene == li.scene && zcIM.data[zcIM.accid].curchat.to == li.to)) {
        liHtml.AppendFormat('<div class="Friend-li Selected {5}" data-scene="{0}" data-to="{1}" data-toname="{2}" data-img="{3}" data-has-unread="{4}">', li.scene, li.to, li.toname, li.ico, li.unread > 0 ? 1 : 0, li.online === false ? 'Friend-gray' : '');
        liHtml.AppendFormat('<div class="Friend-avatar"><img class="Friend-avatar-img" src="{0}"></div>', li.ico);
        liHtml.AppendFormat('<div class="Friend-name">{0}{1}</div>', li.toname, li.online === false ? '（离线）' : '');
        if (li.unread > 0) {
            liHtml.AppendFormat('<div class="Friend-unread">{0}</div>', li.unread);
        }
        liHtml.AppendFormat('</div>');
    }
    else {
        if (li.scene == 'sys') {
            liHtml.AppendFormat('<div class="Friend-li {5}" data-scene="{0}" data-to="{1}" data-toname="{2}" data-img="{3}" data-has-unread="{4}">', li.scene, li.to, li.toname, li.ico, li.unread > 0 ? 1 : 0, '');
            liHtml.AppendFormat('<div class="Friend-avatar"><img class="Friend-avatar-img" src="{0}"></div>', li.ico);
            liHtml.AppendFormat('<div class="Friend-name">{0}{1}</div>', li.toname,'');
            if (li.unread > 0) {
                liHtml.AppendFormat('<div class="Friend-unread">{0}</div>', li.unread);
            }
        } else {
            liHtml.AppendFormat('<div class="Friend-li {5}" data-scene="{0}" data-to="{1}" data-toname="{2}" data-img="{3}" data-has-unread="{4}">', li.scene, li.to, li.toname, li.ico, li.unread > 0 ? 1 : 0, li.online === false ? 'Friend-gray' : '');
            liHtml.AppendFormat('<div class="Friend-avatar"><img class="Friend-avatar-img" src="{0}"></div>', li.ico);
            liHtml.AppendFormat('<div class="Friend-name">{0}{1}</div>', li.toname, li.online === false ? '（离线）' : '');
            if (li.unread > 0) {
                liHtml.AppendFormat('<div class="Friend-unread">{0}</div>', li.unread);
            }
        }
        liHtml.AppendFormat('</div>');
    }
    return liHtml.ToString();
}
//更新行未读消息数
zcIM.fn.refreshFriendLiUI = function (scene, to) {
    var li = zcIM.fn.findFriendLi(scene, to);
    if (li.unread == 0) {
        zcIM.IM.Friends.List.find('.Friend-li[data-to="' + to + '"] .Friend-unread').last().remove();
    }
    else {
        var unReadDiv = zcIM.IM.Friends.List.find('.Friend-li[data-to="' + to + '"] .Friend-unread').last();
        if (unReadDiv.length == 0) {
            var item = zcIM.IM.Friends.List.find('.Friend-li[data-to="' + to + '"]').append('<div class="Friend-unread">' + li.unread + '</div>');
        }
        else {
            unReadDiv.text(li.unread);
        }
    }
}
//生成联系人列表
zcIM.fn.refreshFriendListUI = function () {
    var friendHtml = new StringBuilder();
    var searchFriendKeyword = $.trim(zcIM.IM.Friends.find('.txt-search').val());
    var friendslist = zcIM.data[zcIM.accid].friendslist;

    if (searchFriendKeyword != '' && zcIM.data[zcIM.accid].searchFriendKeyword != searchFriendKeyword) {
        friendslist = $.grep(friendslist, function (cur, z) {
            return cur['toname'].indexOf(searchFriendKeyword) >= 0;
        });
        zcIM.data[zcIM.accid].searchFriendKeyword = searchFriendKeyword;
    }
    //系统消息未读
    var lisus = $.grep(friendslist, function (cur, z) { return cur['unread'] > 0 && cur['scene'] == "sys"; });
    //p2p未读
    var lifus = $.grep(friendslist, function (cur, z) { return cur['unread'] > 0 && cur['scene'] == "p2p"; });
    //p2p已读 在线
    var lifs1 = $.grep(friendslist, function (cur, z) { return cur['unread'] == 0 && cur['scene'] == "p2p" && !!cur['online']; });
    //p2p已读 离线
    var lifs2 = $.grep(friendslist, function (cur, z) { return cur['unread'] == 0 && cur['scene'] == "p2p" && !cur['online']; });
    //群聊未读
    var litus = $.grep(friendslist, function (cur, z) { return cur['unread'] > 0 && cur['scene'] == "team"; });
    //群聊已读
    var lits = $.grep(friendslist, function (cur, z) { return cur['unread'] == 0 && cur['scene'] == "team"; });

    if (lisus.length > 0) {
        friendHtml.AppendFormat(zcIM.fn.getFriendLiHtml(lisus[0]));
    }
    for (var i = 0; i < lifus.length; i++) {
        friendHtml.AppendFormat(zcIM.fn.getFriendLiHtml(lifus[i]));
    }
    for (var i = 0; i < litus.length; i++) {
        friendHtml.AppendFormat(zcIM.fn.getFriendLiHtml(litus[i]));
    }
    for (var i = 0; i < lifs1.length; i++) {
        friendHtml.AppendFormat(zcIM.fn.getFriendLiHtml(lifs1[i]));
    }
    for (var i = 0; i < lifs2.length; i++) {
        friendHtml.AppendFormat(zcIM.fn.getFriendLiHtml(lifs2[i]));
    }
    for (var i = 0; i < lits.length; i++) {
        friendHtml.AppendFormat(zcIM.fn.getFriendLiHtml(lits[i]));
    }
    zcIM.IM.Friends.List.html('');
    zcIM.IM.Friends.List.append(friendHtml.ToString());
}


/**
* 发送普通文本消息
* @param scene：场景，分为：P2P点对点对话，team群对话
* @param to：消息的接收方
* @param text：发送的消息文本
* @param callback：回调
*/
zcIM.fn.sendTextMessage = function (scene, to, text, callback) {
    zcIM.nim.sendText({
        scene: scene || 'p2p',
        to: to,
        text: text,
        done: callback
    });
};

/**
* 发送自定义消息
* @param scene：场景，分为：P2P点对点对话，team群对话
* @param to：消息的接收方
* @param content：消息内容对象
* @param callback：回调
*/
zcIM.fn.sendCustomMessage = function (scene, to, content, callback) {
    zcIM.nim.sendCustomMsg({
        scene: scene || 'p2p',
        to: to,
        content: JSON.stringify(content),
        done: callback
    });
};

zcIM.fn.uploadFile = function (fileInputKey) {
    var scene = zcIM.data[zcIM.accid].curchat.scene,
        to = zcIM.data[zcIM.accid].curchat.to,
        fileInput = zcIM.IM.Form.find(fileInputKey).get(0);
    if (!fileInput || !fileInput.files || fileInput.files[0].size == 0) {
        alert("不能传空文件")
        return
    }
    this.sendFileMessage(scene, to, fileInput, this.sendMsgDone.bind(this))
}
/**
* 发送文件消息
* @param scene：场景，分为：P2P点对点对话，team群对话,callback回调
* @param to：消息的接收方
* @param text：发送的消息文本
* @param callback：回调
*/
zcIM.fn.sendFileMessage = function (scene, to, fileInput, callback) {
    var that = this,
		value = fileInput.value,
		ext = value.substring(value.lastIndexOf('.') + 1, value.length),
		type = /png|jpg|bmp|jpeg|gif/i.test(ext) ? 'image' : 'file';
    zcIM.data[zcIM.accid].sendFileProgress = 0;
    zcIM.nim.sendFile({
        scene: scene,
        to: to,
        type: type,
        fileInput: fileInput,
        uploadprogress: function (data) {
            if (zcIM.debug) console.log(data.percentageText);
            if (zcIM.IM.IMProgress.hasClass('IM-hidden')) zcIM.IM.IMProgress.removeClass('IM-hidden');
            var curSendFileProgress = parseFloat(data.percentageText);
            if (curSendFileProgress > zcIM.data[zcIM.accid].sendFileProgress) {
                zcIM.IM.IMProgress.find('.IM-progress-in').css('width', data.percentageText);
                zcIM.IM.IMProgress.find('.IM-progress-text').text(data.percentageText);
                zcIM.data[zcIM.accid].sendFileProgress = curSendFileProgress;
            }
        },
        uploaderror: function () {
            if (zcIM.debug) console.log('上传失败');
            if (!zcIM.IM.IMProgress.hasClass('IM-hidden')) zcIM.IM.IMProgress.addClass('IM-hidden');
        },
        uploaddone: function (error, file) {
            if (zcIM.debug) console.log(error);
            if (zcIM.debug) console.log(file);
            if (zcIM.debug) console.log('上传' + (!error ? '成功' : '失败'));
            if (!zcIM.IM.IMProgress.hasClass('IM-hidden')) zcIM.IM.IMProgress.addClass('IM-hidden');
        },
        beforesend: function (msg) {
            if (zcIM.debug) console.log('正在发送消息, id=', msg);
            $(fileInput).val('');
        },
        done: callback
    });
}
/**
 * 获取云记录消息
 * @param  {Object} param 数据对象
 * @return {void}       
 */
zcIM.fn.getHistoryMsgs = function (param) {
    zcIM.nim.getHistoryMsgs(param);
}
/**
 * 获取本地历史记录消息  
 */
zcIM.fn.getLocalMsgs = function (scene, to, lastMsgId, done) {
    if (lastMsgId) {
        zcIM.nim.getLocalMsgs({
            scene: scene,
            to: to,
            lastMsgIdClient: lastMsgId,
            limit: 20,
            done: done
        });
    } else {
        zcIM.nim.getLocalMsgs({
            scene: scene,
            to: to,
            limit: 20,
            done: done
        });
    }
}
//消息规整
zcIM.fn.pushMsg = function (msgs) {
    if (!Array.isArray(msgs)) { msgs = [msgs]; }
    var sessionId = msgs[0].sessionId;
    //if (!zcIM.data[zcIM.accid].msgs[sessionId]) zcIM.data[zcIM.accid].msgs[sessionId] = [];
    zcIM.data[zcIM.accid].msgs[sessionId] = nim.mergeMsgs(zcIM.data[zcIM.accid].msgs[sessionId], msgs);
    var id = zcIM.data[zcIM.accid].curchat.scene + '-' + zcIM.data[zcIM.accid].curchat.to;
    if (zcIM.data[zcIM.accid].curchat.to && sessionId == id) {
        zcIM.fn.addChatLis(msgs, false);
        //重置某个会话的未读数
        //zcIM.fn.resetSessionUnread(id);
    }
}
zcIM.fn.getUserByPerson = function (to) {
    if (zcIM.data[zcIM.accid].persons[to]) return zcIM.data[zcIM.accid].persons[to];

    return false;
}
//聊天框加载消息 
zcIM.fn.addChatLis = function (msgs, before) {
    if (!msgs || msgs.length == 0) return;

    var scrollHeight = zcIM.IM.Form.find('.IM-chat-list').get(0).scrollHeight;
    var curScrollTop = zcIM.IM.Form.find('.IM-chat-list').scrollTop();
    zcIM.curScrollBottom = scrollHeight - curScrollTop;
    var msgHtml = "";
    for (var i = 0; i < msgs.length; i++) {
        var msg = msgs[i];
        var lastTime = null;
        if (i == 0) {
            var lastItem = zcIM.IM.Form.find('.IM-chat-list .chat-item').last();
            if (lastItem.length > 0) lastTime = parseInt(lastItem.attr('data-time'));
        }
        else {
            lastTime = msgs[i - 1].time;
        }
        var user = zcIM.fn.getUserByPerson(msg.from);
        //msgHtml += this.makeTimeTag(transTime(msg.time));
        if (!lastTime) {
            msgHtml += this.makeTimeTag(transTime(msg.time));
        }
        else {
            if (msg.time - lastTime > 5 * 60 * 1000) {
                msgHtml += this.makeTimeTag(transTime(msg.time));
            }
        }
        msgHtml += this.makeChatContent(msg, user);
    }
    if (before) zcIM.IM.Form.find('.IM-chat-list').html('');
    zcIM.IM.Form.find('.IM-chat-list').append(msgHtml);
    var endScrollHeight = zcIM.IM.Form.find('.IM-chat-list').get(0).scrollHeight;
    var scrollTop = before ? endScrollHeight - zcIM.curScrollBottom : endScrollHeight;
    zcIM.IM.Form.find('.IM-chat-list').scrollTop(scrollTop);
}
//聊天框加载系统消息 
zcIM.fn.addSysMsgChatLis = function (msgs, before) {
    if (!msgs || msgs.length == 0) return;

    var scrollHeight = zcIM.IM.Form.find('.IM-chat-list').get(0).scrollHeight;
    var curScrollTop = zcIM.IM.Form.find('.IM-chat-list').scrollTop();
    zcIM.curScrollBottom = scrollHeight - curScrollTop;
    var msgHtml = "";
    for (var i = 0; i < msgs.length; i++) {
        var msg = msgs[i];
        var lastTime = null;
        if (i == 0) {
            var lastItem = zcIM.IM.Form.find('.IM-chat-list .chat-item').last();
            if (lastItem.length > 0) lastTime = parseInt(lastItem.attr('data-time'));
        }
        else {
            lastTime = msgs[i - 1].time;
        }
        var user = zcIM.fn.getUserByPerson(msg.from);
        //msgHtml += this.makeTimeTag(transTime(msg.time));
        if (!lastTime) {
            msgHtml += this.makeTimeTag(transTime(msg.time));
        }
        else {
            if (msg.time - lastTime > 5 * 60 * 1000) {
                msgHtml += this.makeTimeTag(transTime(msg.time));
            }
        }
        msgHtml += this.makeSysMsgChatContent(msg, user);
    }
    if (before) zcIM.IM.Form.find('.IM-chat-list').html('');
    zcIM.IM.Form.find('.IM-chat-list').append(msgHtml);
    var endScrollHeight = zcIM.IM.Form.find('.IM-chat-list').get(0).scrollHeight;
    var scrollTop = before ? endScrollHeight - zcIM.curScrollBottom : endScrollHeight;
    zcIM.IM.Form.find('.IM-chat-list').scrollTop(scrollTop);
}
//查看更多消息
zcIM.fn.makeMoreTag = function () {
    return '<p class="u-localmsg">查看更多消息</p>';
}
//聊天消息中的时间显示
zcIM.fn.makeTimeTag = function (time) {
    return '<p class="u-msgTime">- - - - -&nbsp;' + time + '&nbsp;- -- - -</p>';
}

/**
* 通用消息内容UI
*/
zcIM.fn.makeChatContent = function (message, user) {
    var msgHtml;
    //通知类消息
    if (message.attach && message.attach.type) {
        //var notificationText = transNotification(message);
        //msgHtml = '<p class="u-notice tc item chat-item" data-time="' + message.time + '" data-id="' + message.idClient + '" data-idServer="' + message.idServer + '"><span class="radius5px">' + notificationText + '</span></p>';
    } else {
        //聊天消息
        var type = message.type,
            from = message.from,
            sender = from != zcIM.accid ? 'you' : 'me';
        showNick = message.scene === 'team' && from != zcIM.accid,
        msgHtml;
        if (type === "tip") {
            //msgHtml = ['<div data-time="' + message.time + '" data-id="' + message.idClient + '" id="' + message.idClient + '" data-idServer="' + message.idServer + '">',
            //                '<p class="u-notice tc item ' + (from == zcIM.accid && message.idServer ? "j-msgTip" : "") + '" data-time="' + message.time + '" data-id="' + message.idClient + '" data-idServer="' + message.idServer + '"><span class="radius5px">' + getMessage(message, zcIM.accid) + '</span></p>',
            //            '</div>'].join('');
        } else {
            if (!user) {
                console.log(message, user);
                return;
            }
            msgHtml = ['<div data-time="' + message.time + '" data-id="' + message.idClient + '" id="' + message.idClient + '" data-idServer="' + message.idServer + '" class="item chat-item item-' + sender + '">',
                        '<img class="img j-img" src="' + user.ico + '" data-account="' + user.to + '" title="' + user.toname + '" />',
                        showNick ? '<p class="nick" title="' + user.toname + '">' + user.toname + '</p>' : '',
                        '<div class="msg msg-text j-msg">',
                            '<div class="box">',
                                '<div class="cnt">',
                                    getMessage(message, zcIM.accid),
                                '</div>',
                            '</div>',
                        '</div>',
                        message.status === "fail" ? '<span class="error j-resend" data-session="' + message.sessionId + '" data-id="' + message.idClient + '" title="发送失败,点击重发"><i class="iconfont icon-changjianwenti"></i></span>' : '',
                       '<span class="readMsg"><i></i>已读</span>',
                    '</div>'].join('');
        }
    }
    return msgHtml;
};

/**
* 系统消息内容UI
*/
zcIM.fn.makeSysMsgChatContent = function (message, user) {
    var msgHtml;
    //暂时不支持群
    if (message.category == 'team') {
    } else {
        //聊天消息
        var type = message.type,
            from = message.from,
            sender = from != zcIM.accid ? 'you' : 'me',
            msgHtml;
        if (!user) {
            console.log(message, user);
            return;
        }
        msgHtml = ['<div data-time="' + message.time + '" data-idServer="' + message.idServer + '" class="item chat-item item-' + sender + '">',
                    '<img class="img j-img" src="/lib/emoji/emoji/emoji_84.png"/>',
                    '<div class="msg msg-text j-msg">',
                        '<div class="box">',
                            '<div class="cnt">',
                                getSysFriendMessage(message, user, zcIM.accid),
                            '</div>',
                        '</div>',
                    '</div>',
                '</div>'].join('');
    }
    return msgHtml;
};
/**
* 发送消息完毕后的回调
* @param error：消息发送失败的原因
* @param msg：消息主体，类型分为文本、文件、图片、地理位置、语音、视频、自定义消息，通知等
*/
zcIM.fn.sendMsgDone = function (error, msg) {
    if (!error) {
        console.log('发送消息完成', msg);
        if (msg.type == "text") {
            zcIM.IM.Form.find('.txt-sendbox').text('');
        }
        zcIM.IM.Form.find('.IM-chat-list .no-msg').remove();
        zcIM.fn.pushMsg(msg);
        //zcIM.IM.Form('.uploadForm').get(0).reset();
    }
    else {
        console.log('发送消息出错', error, msg);
    }
}
//查找session
zcIM.fn.findSession = function (sessionId) {
    for (var i = zcIM.data[zcIM.accid].sessions.length - 1; i >= 0; i--) {
        if (zcIM.data[zcIM.accid].sessions[i].id === sessionId) {
            return zcIM.data[zcIM.accid].sessions[i];
        }
    }
    return false;
    //var sp = sessionId.split('-');
    //return {id:sessionId,unread:0,scene:sp[0],to:sp[1]};
}
//查找friendslist Li
zcIM.fn.findFriendLi = function (scene, to) {
    for (var i = zcIM.data[zcIM.accid].friendslist.length - 1; i >= 0; i--) {
        if (zcIM.data[zcIM.accid].friendslist[i].scene == scene && zcIM.data[zcIM.accid].friendslist[i].to == to) {
            return zcIM.data[zcIM.accid].friendslist[i];
        }
    }
    return false;
}
//重置某个会话的未读数
zcIM.fn.resetSessionUnread = function (sessionId) {
    zcIM.nim.resetSessionUnread(sessionId);
}

//获取当前会话消息
zcIM.fn.getCurMsgs = function (before) {
    if (!zcIM.data[zcIM.accid].curchat.to) return false;
    var scene = zcIM.data[zcIM.accid].curchat.scene;
    var to = zcIM.data[zcIM.accid].curchat.to;
    var id = zcIM.data[zcIM.accid].curchat.scene + "-" + zcIM.data[zcIM.accid].curchat.to;
    //var session = zcIM.fn.findSession(id);
    var li = zcIM.fn.findFriendLi(scene, to);
    if (li.hasLocalMsg === undefined) li.hasLocalMsg = true;
    var msgs = zcIM.data[zcIM.accid].msgs[id];

    //重置某个会话的未读数
    if (li.unread > 0) zcIM.fn.resetSessionUnread(id);
    //if (!!session && li.hasLocalMsg) {
    if (li.hasLocalMsg) {
        var msgid = (!!msgs && msgs.length > 0) ? msgs[0].idClient : false
        zcIM.fn.getLocalMsgs(zcIM.data[zcIM.accid].curchat.scene, zcIM.data[zcIM.accid].curchat.to, msgid, function (err, data) {
            if (!err) {
                zcIM.IM.Form.find('.IM-chat-list .u-localmsg').last().remove();
                var id = data.scene + "-" + data.to;
                if (data.msgs.length > 0) {
                    msgs = nim.mergeMsgs(msgs, data.msgs);
                    zcIM.data[zcIM.accid].msgs[id] = msgs;
                    zcIM.fn.addChatLis(msgs, before);
                }
                if (!before) zcIM.fn.setFriendLiUnreadZero(data.scene, data.to);
                if (data.msgs.length < 20) li.hasLocalMsg = false;
                if (li.hasLocalMsg === true) zcIM.IM.Form.find('.IM-chat-list').prepend(zcIM.fn.makeMoreTag());
            } else {
                alert("获取历史消息失败");
            }
        });
        return false;
    }
    else {
        zcIM.IM.Form.find('.IM-chat-list .u-localmsg').last().remove();
    }
    zcIM.fn.setFriendLiUnreadZero(zcIM.data[zcIM.accid].curchat.scene, zcIM.data[zcIM.accid].curchat.to);
    zcIM.fn.addChatLis(msgs, false);
};
//获取当前会话消息
zcIM.fn.getCurSysMsgs = function (before, reload) {
    if (zcIM.data[zcIM.accid].curchat.scene != 'sys') return false;
    var msgs = !!reload?[]:zcIM.data[zcIM.accid].sysMsgs;
    var li = zcIM.fn.findFriendLi('sys', 'sys');
    if (!!reload || li.hasLocalMsg === undefined) li.hasLocalMsg = true;
    //重置某个会话的未读数
    if (li.hasLocalMsg) {
        var msgid = (!!msgs && msgs.length > 0) ? msgs[0].idServer : false
        zcIM.fn.getLocalSysMsgs(null, null, null, null, function (err, data) {
            if (!err) {
                zcIM.IM.Form.find('.IM-chat-list .u-localmsg').last().remove();
                if (data.sysMsgs.length > 0) {
                    var npCount = 0;
                    for (var i = 0; i < data.sysMsgs.length; i++) {
                        var mto = data.sysMsgs[i].from == zcIM.accid ? data.sysMsgs[i].to : data.sysMsgs[i].from;
                        var lis = zcIM.data[zcIM.accid].persons[mto];
                        if (lis) continue;
                        zcIM.data[zcIM.accid].persons[mto] = { to: mto, toname: "", ico: "", isget: false };
                        npCount++;
                    }
                    msgs = nim.mergeMsgs(msgs, data.sysMsgs);
                    //只取未读系统消息
                    msgs = $.grep(msgs, function (cur, z) {
                        return cur['read'] == false;
                    });
                    zcIM.data[zcIM.accid].sysMsgs = msgs;
                    var lifs = $.grep(data.sysMsgs, function (cur, z) {
                        return cur['category'] == 'friend' && (cur['type'] == 'addFriend' || cur['type'] == 'passFriendApply' || cur['type'] == 'rejectFriendApply');
                    });
                    if (lifs.length > 0) {
                        zcIM.nim.markSysMsgRead({
                            sysMsgs: lifs,
                            done: function () {
                            }
                        });
                    }
                    if (!before) zcIM.fn.setFriendLiUnreadZero('sys', 'sys');
                    if (data.sysMsgs.length < 20) li.hasLocalMsg = false;
                    if (npCount > 0) {
                        zcIM.fn.getPersonsInfo(function () {
                            zcIM.fn.addSysMsgChatLis(msgs, before);
                            if (li.hasLocalMsg === true) zcIM.IM.Form.find('.IM-chat-list').prepend(zcIM.fn.makeMoreTag());
                        });
                    } else {
                        zcIM.fn.addSysMsgChatLis(msgs, before);
                        if (li.hasLocalMsg === true) zcIM.IM.Form.find('.IM-chat-list').prepend(zcIM.fn.makeMoreTag());
                    }
                } else {
                    if (!before) zcIM.fn.setFriendLiUnreadZero('sys', 'sys');
                    li.hasLocalMsg = false;
                }
            } else {
                alert("获取历史系统消息失败");
            }
        });
        return false;
    }
    else {
        zcIM.IM.Form.find('.IM-chat-list .u-localmsg').last().remove();
    }
    zcIM.fn.setFriendLiUnreadZero('sys', 'sys');
    zcIM.fn.addSysMsgChatLis(msgs, false);
};

zcIM.fn.addMsgs = function (msgs) {
    var item,
        user;
    for (var i = 0; i < msgs.length; i++) {
        if (msgs[i].scene === "team") {
            user = msgs[i].to;
            if (!zcIM.data[zcIM.accid].msgs["team-" + user]) {
                zcIM.data[zcIM.accid].msgs["team-" + user] = [];
            }
            zcIM.data[zcIM.accid].msgs["team-" + user].push(msgs[i]);
        } else {
            user = (msgs[i].from == zcIM.accid ? msgs[i].to : msgs[i].from);
            if (!zcIM.data[zcIM.accid].msgs["p2p-" + user]) {
                zcIM.data[zcIM.accid].msgs["p2p-" + user] = [];
            }
            zcIM.data[zcIM.accid].msgs["p2p-" + user].push(msgs[i]);
        }
    };
};
/**
 * 获取云记录消息 
 */
zcIM.fn.getHistoryMsgs = function (scene, to, lastMsgId, done) {
    if (lastMsgId) {
        zcIM.nim.getLocalMsgs({
            scene: scene,
            to: to,
            lastMsgId: lastMsgId,
            limit: 20,
            reverse: false,
            done: done
        });
    } else {
        zcIM.nim.getLocalMsgs({
            scene: scene,
            to: to,
            lastMsgId: 0,
            limit: 20,
            reverse: false,
            done: done
        });
    }
}
/**
 * 获取本地历史记录消息  
 */
zcIM.fn.getLocalMsgs = function (scene, to, lastMsgId, done) {
    if (lastMsgId) {
        zcIM.nim.getLocalMsgs({
            scene: scene,
            to: to,
            lastMsgIdClient: lastMsgId,
            limit: 20,
            done: done
        });
    } else {
        zcIM.nim.getLocalMsgs({
            scene: scene,
            to: to,
            limit: 20,
            done: done
        });
    }
}

/**
 * 获取本地历史记录消息  
 */
zcIM.fn.getLocalSysMsgs = function (category, type, read, lastIdServer, done) {
    var option = { limit: 20, done: done };
    if (category !== null) option.category = category;
    if (type !== null) option.type = type;
    if (read !== null) option.read = read;
    if (lastIdServer !== null) option.lastIdServer = lastIdServer;
    zcIM.nim.getLocalSysMsgs(option);
}


//设置未读数为零
zcIM.fn.setFriendLiUnreadZero = function (scene, to) {
    var li = zcIM.fn.findFriendLi(scene, to);
    if (!li) return;
    if (li.unread > 0) {
        li.unread = 0;
        zcIM.fn.getFriendslistUnreadTotal();
        zcIM.fn.refreshFriendLiUI(scene, to);
    }
}
//收到消息后设置未读数
zcIM.fn.setFriendLiUnread = function (scene, to, num) {
    var li = zcIM.fn.findFriendLi(scene, to);
    if (!li) return;
    li.unread = li.unread + num;
}
//收到消息后设置未读数
zcIM.fn.setFriendLiUnreadNum = function (scene, to, num) {
    var li = zcIM.fn.findFriendLi(scene, to);
    if (!li) return;
    li.unread = num;
}

//移除消息用于重发状态变化 session-id idClient 消息 
zcIM.fn.removeMsg = function (sid, cid) {
    var list = zcIM.data[zcIM.accid].msgs[sid];
    for (var i = list.length - 1; i >= 0; i--) {
        if (list[i].idClient === cid) {
            list.splice(i, 1);
            return;
        }
    };
}
//查找消息
zcIM.fn.findMsg = function (sid, cid) {
    var list = zcIM.data[zcIM.accid].msgs[sid];
    for (var i = list.length - 1; i >= 0; i--) {
        if (list[i].idClient === cid) {
            return list[i];
        }
    };
    return false;
}
/**
 * 消息重发
 */
zcIM.fn.resendMsg = function (msg, done) {
    zcIM.nim.resendMsg({
        msg: msg,
        done: done
    });
}
/**
 * 申请加好友
 */
zcIM.fn.applyFriend = function (account, ps, done) {
    zcIM.nim.applyFriend({
        account: account,
        ps: ps,
        done: done
    });
}
/**
 * 通过好友申请
 */
zcIM.fn.passFriendApply = function (account, nickname, avatar, idServer, done) {
    zcIM.nim.passFriendApply({
        account: account,
        ps: { nickname: nickname, avatar: avatar },
        idServer: idServer,
        done: function (error, obj) {
            if (!error) {
                var to = obj.account;
                var nickname = obj.ps.nickname;
                var avatar = obj.ps.avatar;
                var lifs = $.grep(zcIM.data[zcIM.accid].friendslist, function (cur, z) {
                    return cur['to'] == to && cur['scene'] == "p2p";
                });
                if (lifs.length == 0) {
                    var li = { scene: "p2p", to: to, toname: nickname, ico: avatar, unread: 0 };
                    zcIM.data[zcIM.accid].friendslist.push(li);
                    var liHtml = zcIM.fn.getFriendLiHtml(li);
                    var preLi = zcIM.IM.Friends.List.find('.Friend-li[data-to="sys"]');
                    if (preLi.length > 0) {
                        preLi.after(liHtml);
                    } else {
                        zcIM.IM.Friends.List.prepend(liHtml);
                    }
                }
            }
            done(error, obj);
        }
    });
}
/**
 * 拒绝好友申请
 */
zcIM.fn.rejectFriendApply = function (account, ps, idServer, done) {
    zcIM.nim.rejectFriendApply({
        account: account,
        ps: ps,
        idServer: idServer,
        done: done
    });
}
/**
 * 删除好友
 */
zcIM.fn.deleteFriend = function (account, done) {
    zcIM.nim.deleteFriend({
        account: account,
        done: done
    });
}
/**
 * 已读
 */
zcIM.fn.markSysMsgRead = function (keyPath, value, done) {
    var option = {
        keyPath: keyPath,
        value: value
    };
    var msg = NIM.util.findObjInArray(zcIM.data[zcIM.accid].sysMsgs, option);
    if (msg) {
        zcIM.nim.markSysMsgRead({
            sysMsgs: msg,
            done: done
        });
    }
    else {
        alert('消息未找到');
    }
}

/**
 * 查找用户
 */
zcIM.fn.findUsers = function (phone, nickname) {
    findUsers(10, 1, phone, nickname, function (data) {
        zcIM.IM.Users.find('.IM-User-List').html('');
        if (data.list.length > 0) {
            var listHtml = new StringBuilder();
            for (var i = 0; i < data.list.length; i++) {
                listHtml.AppendFormat('<div class="list-user col-xs-6" data-id="{0}">', data.list[i].id);
                listHtml.AppendFormat('<div class="list-userimg"><img src="{0}"></div>', data.list[i].avatar);
                listHtml.AppendFormat('<div class="list-username">{0}</div>', data.list[i].nickname);
                listHtml.AppendFormat('<div class="list-bottom">');
                if (data.list[i].isFriend == '0') {
                    listHtml.AppendFormat('<span class="list-add-friend">好友申请</span>');
                }
                listHtml.AppendFormat('</div>');
                listHtml.AppendFormat('</div>');
            }
            zcIM.IM.Users.find('.IM-User-List').append(listHtml.ToString());
        }
    });
}
//加载好友卡片
zcIM.fn.onUsers = function (users, refreshUI) {
    for (var i = 0; i < users.length; i++) {
        var lis = zcIM.data[zcIM.accid].persons[users[i].account];
        var ico = $.trim(users[i].avatar) == '' ? '/img/europejobsites.png' : $.trim(users[i].avatar);
        if (!lis) {
            zcIM.data[zcIM.accid].persons[users[i].account] = { to: users[i].account, toname: $.trim(users[i].nick), ico: ico, isget: true };
        }
        else {
            lis.isget = true;
            lis.toname = $.trim(users[i].nick);
            lis.ico = ico;
        }
        var lifs = $.grep(zcIM.data[zcIM.accid].friendslist, function (cur, z) { return cur['to'] == users[i].account && cur['scene'] == "p2p"; });
        if (lifs.length > 0) {
            for (var j = 0; j < lifs.length; j++) {
                lifs[j].toname = $.trim(users[i].nick);
                lifs[j].ico = ico;
            }
        }
    }
    if (zcIM.debug) console.log("persons", zcIM.data[zcIM.accid].persons);
    if (zcIM.debug) console.log("friendslist", zcIM.data[zcIM.accid].friendslist);
    if (refreshUI) {
        zcIM.fn.getFriendslistUnreadTotal();
        zcIM.fn.refreshFriendListUI();
    }
}
//加载用户（好友，联系人，群成员）信息
zcIM.fn.getPersonsInfo = function (callback) {
    var accounts = [];
    var lis = Object.keys(zcIM.data[zcIM.accid].persons);
    for (var i = 0; i < lis.length; i++) {
        if (accounts.length > 140) break;
        if (zcIM.data[zcIM.accid].persons[lis[i]].isget) continue;
        accounts.push(zcIM.data[zcIM.accid].persons[lis[i]].to);
    }
    if (accounts.length > 0) {
        var max = accounts.length - 1;
        getUsersByIds(
            accounts,
            function (data) {
                zcIM.fn.onNewUsers(data);
                if (callback) {
                    callback();
                } else {
                    zcIM.fn.getPersonsInfo();
                }
            });
    } else {
        if (zcIM.IM.IMChat.hasClass('IM-hidden') || !zcIM.data[zcIM.accid].curchat.to) {
            zcIM.fn.getFriendslistUnreadTotal();
            zcIM.fn.refreshFriendListUI();
            return;
        }
        var to = zcIM.IM.Friends.List.find('.Friend-li.Selected').attr('data-to');
        if (to != zcIM.data[zcIM.accid].curchat.to) {
            zcIM.fn.getFriendslistUnreadTotal();
            zcIM.fn.refreshFriendLiUI(scene, to);
        }
    }
}
zcIM.fn.onNewUsers = function (data) {
    for (var i = 0; i < data.list.length; i++) {
        var lis = zcIM.data[zcIM.accid].persons[data.list[i].id];
        var ico = $.trim(data.list[i].avatar) == '' ? '/img/europejobsites.png' : $.trim(data.list[i].avatar);
        if (!lis) {
            zcIM.data[zcIM.accid].persons[data.list[i].id] = { to: data.list[i].id, toname: $.trim(data.list[i].nickname), ico: ico, isget: true };
        }
        else {
            lis.isget = true;
            lis.toname = $.trim(data.list[i].nickname);
            lis.ico = ico;
        }
        var lifs = $.grep(zcIM.data[zcIM.accid].friendslist, function (cur, z) { return cur['to'] == data.list[i].id && cur['scene'] == "p2p"; });
        if (lifs.length > 0) {
            for (var j = 0; j < lifs.length; j++) {
                lifs[j].toname = $.trim(data.list[i].nickname);
                lifs[j].ico = ico;
            }
        }
    }
}
//刷新在线状态
zcIM.fn.initOnlineStatus = function (idsString,callback) {
    var ids = idsString.split(',');
    for (var i = 0; i < zcIM.data[zcIM.accid].friendslist.length; i++) {
        zcIM.data[zcIM.accid].friendslist[i].online = (ids.indexOf(zcIM.data[zcIM.accid].friendslist[i].to.toString()) >= 0);
    }
    if (zcIM.IM.IMChat.hasClass('IM-hidden')) {
        zcIM.fn.refreshFriendListUI();
    }
}
//刷新在线状态
zcIM.fn.setOnlineStatus = function (id,status, callback) {
    var friend = zcIM.fn.findFriendLi('p2p', id);
    if (friend) friend.online = status;

    if (zcIM.IM.IMChat.hasClass('IM-hidden')) {
        zcIM.fn.refreshFriendListUI();
    }
}

//删除本地系统消息
zcIM.fn.deleteLocalSysMsg = function (idServer, done) {
    zcIM.nim.deleteLocalSysMsg({
        idServer: idServer,
        done: done
    });
}