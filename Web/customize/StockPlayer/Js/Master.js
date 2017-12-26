////本地类型
var catedata = {
    zaop: 692, //早评
    wup: 693,//午评
    wanp: 694,//晚评
    buy: 687,//股权买
    sell: 688,//股权卖

    sroot: 705,//股市周线预测
    sjust: 706,//股市周线预测  正方
    sback: 707, //股市周线预测  反方

    hroot: 708,//黄金属周线预测
    hjust: 709,//黄金属周线预测 正方
    hback: 710,//黄金属周线预测 反方

    yroot: 711,//原油周线预测
    yjust: 712,//原油周线预测 正方
    yback: 713,//原油周线预测 反方

    complain:714,//投诉
    suggest: 715,//建议
}


//底部链接文章
data.bottomLinkArtcle = ['426806', '426808', '426810', '426812'];
data.register = {
    userArtcle: '426814',
    userContent:'',
    companyArtcle: '426816',
    companyContent: '',
    detailArticle: '427591',
    detailContent:''
}

//站点类型
//var catedata = {
//    zaop: 1380, //早评
//    wup: 1381,//午评
//    wanp: 1382,//晚评
//    buy: 1383,//股权买
//    sell: 1384,//股权卖

//    sroot: 1409,//股市周线预测
//    sjust: 1410,//股市周线预测  正方
//    sback: 1411, //股市周线预测  反方

//    hroot: 1412,//黄金属周线预测
//    hjust: 1413,//黄金属周线预测 正方
//    hback: 1414,//黄金属周线预测 反方

//    yroot: 1415,//原油周线预测
//    yjust: 1416,//原油周线预测 正方
//    yback: 1417,//原油周线预测 反方
//}
////底部链接文章
//data.bottomLinkArtcle = ['1380343', '1380347', '1380350', '1380352'];
//data.register = {
//    userArtcle: '1380358',
//    userContent: '',
//    companyArtcle: '1380361',
//    companyContent: '',
//    detailArticle: '1536128',
//    detailContent:''
//}







var score_name = "淘股币";
var action_name = "赠送";
var min_reward = 10;

//登录时刷新页面的文件
var reloadPages = [
    '/customize/stockplayer/src/usercenter/releasecontent/releasecontent.aspx',
    '/customize/stockplayer/src/usercenter/usercenter.aspx',
    '/customize/stockplayer/src/pupildebate/mysupporthistory.aspx',
    '/customize/stockplayer/src/cognizance/cognizances.aspx',
    '/customize/stockplayer/src/Detail/Detail.aspx'
];
//退出登录时荟首页的文件
var toHomePages = [
    '/customize/stockplayer/src/usercenter/releasecontent/releasecontent.aspx',
    '/customize/stockplayer/src/usercenter/usercenter.aspx'
];
//首页
var homePage = '/customize/StockPlayer/Src/Index/Index.aspx';
var userPage = '/customize/StockPlayer/Src/UserCenter/UserCenter.aspx';
var ueditorToobars = [[
            'source', //源代码
            'undo', //撤销 
            'underline', //下划线
            'redo', //重做
            'bold', //加粗
            'indent', //首行缩进
            'fontfamily', //字体
            'fontsize', //字号
            'simpleupload', //单图上传
            'justifyleft', //居左对齐
            'justifyright', //居右对齐
            'justifycenter', //居中对齐
            'justifyjustify', //两端对齐
            'forecolor', //字体颜色
            'backcolor', //背景色
            'insertorderedlist', //有序列表
            'insertunorderedlist', //无序列表
            'fullscreen', //全屏
            'imagenone', //默认
            'imageleft', //左浮动
            'imageright', //右浮动
            'wordimage', //图片转存
            'edittip', //编辑提示
            'autotypeset', //自动排版
            'inserttable', //插入表格
            'strikethrough', //删除线
            'help', //帮助


            'emotion', //表情
            'template', //模板    
            'preview', //预览
            'spechars', //特殊字符
            //'background', //背景
            'pasteplain', //纯文本粘贴模式
            'link', //超链接
            'map', //Baidu地图
            'searchreplace', //查询替换
            'scrawl', //涂鸦

            // 'touppercase', //字母大写
            //'tolowercase', //字母小写
            //'customstyle', //自定义标题
            //'lineheight', //行间距
            //'attachment', //附件
            //'imagecenter', //居中
            //'directionalityltr', //从左向右输入
            //'directionalityrtl', //从右向左输入
            //'rowspacingtop', //段前距
            //'rowspacingbottom', //段后距
            //'insertframe', //插入Iframe
            //'insertimage', //多图上传
            //'edittable', //表格属性
            //'edittd', //单元格属性
            //'paragraph', //段落格式
            //'unlink', //取消链接
            //'insertrow', //前插入行
            //'insertcol', //前插入列
            //'mergeright', //右合并单元格
            //'mergedown', //下合并单元格
            //'deleterow', //删除行
            //'deletecol', //删除列
            //'splittorows', //拆分成行
            //'splittocols', //拆分成列
            //'splittocells', //完全拆分单元格
            //'deletecaption', //删除表格标题
            //'inserttitle', //插入标题
            //'mergecells', //合并多个单元格
            //'deletetable', //删除表格
            //'insertparagraphbeforetable', //"表格前插入行"
            //'insertcode', //代码语言
            //'horizontal', //分隔线
            //'removeformat', //清除格式
            //'time', //时间
            //'date', //日期
            //'anchor', //锚点
            //'italic', //斜体
            //'subscript', //下标
            //'fontborder', //字符边框
            //'superscript', //上标
            //'formatmatch', //格式刷
            //'blockquote', //引用
            //'selectall', //全选
            //'print', //打印
]];
var qrLayerIndex = -1;
$(function () {
    //重写windows事件
    rewriteWindowEvent();

    //ie8 9 隐藏二维码登录
    if (!data.support_im) {
        $('.noLogin .codeLoginBottom').hide();
    }
    //点击logo
    $('.logoimg').click(function () {
        window.location.href = homePage;
    });
    ////分享
    shareInit();
    $('#login_acount').keypress(function () {
        var theEvent = window.event || arguments.callee.caller.arguments[0];
        if (theEvent.keyCode == 13) {
            var txt = $.trim($(this).val());
            if (txt != '') $('#login_pwd').focus();
            return false;
        }
    });
    $('#login_pwd').keypress(function () {
        var theEvent = window.event || arguments.callee.caller.arguments[0];
        if (theEvent.keyCode == 13) {
            var txt = $.trim($(this).val());
            if (txt == '') return false;

            var useremail = $.trim($('#login_acount').val());
            if (useremail == '') {
                $('#login_acount').focus();
                return false;
            }
            login();
            return false;
        }
    });
    $(document).on('blur', '.textCheck', function () {
        if ($.trim($(this).val()) == '' && !$(this).hasClass('textError')) {
            $(this).addClass('textError');
        } else if ($.trim($(this).val()) != '' && $(this).hasClass('textError')) {
            $(this).removeClass('textError');
        }
    });
    //计算内容最小高度
    var _h = $(window).height() - $('.wrapHead').height() - $('.wrapButtom').height() - 100; //wrapContenxt中有 margin
    $('.wrapContenxt').css('min-height', _h + 'px');

    //底部链接绑定事件
    $(document).on('click', '.helpLink a', function () {
        var num = $(this).index();
        ToBottomLinkArticle(num);
    });

    //通知买家卖家类型选择
    $(document).on('click', '.dlg-notice-type .span-type', function () {
        $('.dlg-notice-type .span-type.selected').removeClass('selected');
        $(this).addClass('selected');
    });

    //点击弹出框的头像昵称跳转详情
    $(document).on('click', '.view-user .view-userimg,.view-user .view-username', function () {
        var tid = $(this).closest('.view-user').attr('data-id');
        if (u_id == tid && u_iscenter) return;
        if (!!tid && tid != 0) ToUser(tid);
    });
    $(document).on('click', '.userimg,.username,.list-friend .font-nick,.list-friend .user-avatar', function () {
        var tid = $(this).attr('data-id');
        if (u_id == tid && u_iscenter) return;
        if (!!tid && tid != 0) ToUser(tid);
    });
    $(document).on('click', '.loginInfo .useravatar,.loginInfo .userdispalyname', function () {
        ToCenter();
    });

    //登录验证
    //tt.vf.req.addId('login_acount', 'login_pwd');
    data.levels = {};
    data.user_levels = {};
    initLevelImgs();
});
//重写windows事件
function rewriteWindowEvent() {
    if (navigator.appName == "Microsoft Internet Explorer") {
        window.console = { log: function () { } };
    }

    //layer提示
    window.alert = function (msg, icon, time, fn) {
        if (!time) time = 2;
        return layer.msg(msg, {
            icon: icon,
            shadeClose: true,
            time: time * 1000 //2秒关闭（如果不配置，默认是3秒）
        }, function () {
            if (!!fn) fn();
        });
    };
    //layer 等待提示
    window.progress = function (type, time) {
        var option = {
            //shadeClose :true,
            shade: 0.1
        }
        if (!!time) option.time = time * 1000;
        return layer.load(type, option);
    };
    //layer 确认对话框
    window.confirm = function (title, msg, yesText, cancelText, yesFn, cancelFn) {
        return layer.confirm(msg, {
            title: title,
            closeBtn: 0,
            //shadeClose: true,
            btn: [yesText, cancelText] //按钮
        }, function (index, layerDom) {
            if (!!yesFn) yesFn(index, layerDom);
        }, function (index) {
            if (!!cancelFn) cancelFn(index);
        });
    };
}
//检查是否支持 IM
function checkSupportIM() {
    if (navigator.appName == "Microsoft Internet Explorer"
         && !!navigator.appVersion.match(/MSIE [56789]./i)) {
        data.support_im = false;
    }
    //if (navigator.appName == "Netscape") {
    //    data.support_im = false;
    //}
    return data.support_im;
}
//登录
function login() {
    var username = $.trim($('#login_acount').val());
    var userpassword = $.trim($('#login_pwd').val());
    if (username == '') return;
    if (userpassword == '') return;
    loginApi(username, userpassword);
}
function loginApi(username, userpassword, fn) {
    var layerIndex = progress();
    $.ajax({
        type: 'post',
        url: "/Serv/LoginApi.ashx",
        data: { action: 'Login', userid: username, pwd: userpassword },
        dataType: "json",
        success: function (resp) {
            layer.close(layerIndex);
            if (resp.issuccess) {
                is_login = true;
                if (reloadPages.indexOf(window.location.pathname.toLowerCase()) >= 0) {
                    window.location.href = window.location.href;
                    return;
                }
                if (data.support_im) {
                    ws = connectSocketServer(resp.id);
                    zcChat.login(resp.id, resp.im_token, resp.avatar, resp.userName);
                }
                $('.wrapHead .mHead .login .useravatar').attr('src', resp.avatar);
                $('.wrapHead .mHead .login .userdispalyname').text(resp.userName);
                $('.useremail').val('');
                $('.userpassword').val('');
                if (!$('.wrapHead .mHead .noLogin').hasClass('hidden')) $('.wrapHead .mHead .noLogin').addClass('hidden');
                if ($('.wrapHead .mHead .login').hasClass('hidden')) $('.wrapHead .mHead .login').removeClass('hidden');
                if (!!fn) fn(resp);
            }
            else {
                if (resp.message) {
                    alert(resp.message);
                }
                else {
                    alert('登录失败')
                }
            }
        }
    });
}
function loginApi2(username, userpassword, fn) {
    var layerIndex = progress();
    $.ajax({
        type: 'post',
        url: "/Serv/api/user/login2.ashx",
        data: { userid: username, pwd: userpassword },
        dataType: "json",
        success: function (resp) {
            layer.close(layerIndex);
            if (resp.issuccess) {
                is_login = true;
                if (reloadPages.indexOf(window.location.pathname.toLowerCase()) >= 0) {
                    window.location.href = window.location.href;
                    return;
                }
                if (data.support_im) {
                    ws = connectSocketServer(resp.id);
                    zcChat.login(resp.id, resp.im_token, resp.avatar, resp.userName);
                }
                $('.wrapHead .mHead .login .useravatar').attr('src', resp.avatar);
                $('.wrapHead .mHead .login .userdispalyname').text(resp.userName);
                $('.useremail').val('');
                $('.userpassword').val('');
                if (!$('.wrapHead .mHead .noLogin').hasClass('hidden')) $('.wrapHead .mHead .noLogin').addClass('hidden');
                if ($('.wrapHead .mHead .login').hasClass('hidden')) $('.wrapHead .mHead .login').removeClass('hidden');
                if (!!fn) fn(resp);
            }
            else {
                if (resp.message) {
                    alert(resp.message);
                }
                else {
                    alert('登录失败')
                }
            }
        }
    });
}
//登出
function loginout() {
    var layerIndex = progress();
    $.ajax({
        type: 'post',
        url: "/Serv/LoginApi.ashx",
        data: { action: 'Logout' },
        dataType: "json",
        success: function (resp) {
            layer.close(layerIndex);
            if (resp.isSuccess) {
                is_login = false;
                if (data.support_im) {
                    zcChat.loginout();
                    if (ws) ws.close();
                }
                if (toHomePages.indexOf(window.location.pathname.toLowerCase()) >= 0) {
                    window.location.href = homePage;
                    return;
                }
                reloadLoginHead();
            }
        }
    });
}
function reloadLoginHead() {
    $('.wrapHead .mHead .login .userdispalyname').text('');
    if (!$('.wrapHead .mHead .login').hasClass('hidden')) $('.wrapHead .mHead .login').addClass('hidden');
    if ($('.wrapHead .mHead .noLogin').hasClass('hidden')) $('.wrapHead .mHead .noLogin').removeClass('hidden');
}
//选中Head
function setHeadSelected(num) {
    $('.mHeadTo .spanHeadTo.selected').removeClass('selected');
    $($('.mHeadTo .spanHeadTo').get(num)).addClass('selected');
}

//检查是否有登录session
function checkLogin(fn) {
    $.ajax({
        type: 'post',
        url: "/Serv/API/User/IsLogin.ashx",
        data: { no_yike: 1 },
        dataType: "json",
        success: function (resp) {
            fn(resp);
        }
    });
}

function showLoginDialog(fn) {
    reloadLoginHead();
    var loginHtml = new StringBuilder();
    loginHtml.AppendFormat('<div class="row">');
    loginHtml.AppendFormat('<div class="col-xs-12"><input type="text" name="dlg_user_acount" id="dlg_user_acount" class="form-control textCheck" placeholder="请输入账号" /></div>');
  
    loginHtml.AppendFormat('<div class="col-xs-12"><input type="password" name="dlg_user_pwd" id="dlg_user_pwd" class="form-control textCheck" placeholder="请输入密码" /></div>');
    loginHtml.AppendFormat('<div class="col-xs-12"><span class="registerfont"><a href="/customize/StockPlayer/src/Register/Register.aspx">没有账号?点击注册</a></span></div>');
    loginHtml.AppendFormat('</div>');
    layer.open({
        type: 1,
        title: '登录',
        skin: 'layui-layer-login', //样式类名
        closeBtn: 0, //不显示关闭按钮
        move: false,//禁止移动
        //shift: 2,
        //shadeClose: true, //开启遮罩关闭
        content: loginHtml.ToString(),
        btn: ['登录', '取消'], //按钮
        yes: function (index, layero) {
            var user_acount = $.trim($(layero).find('#dlg_user_acount').val());
            var user_pwd = $.trim($(layero).find('#dlg_user_pwd').val());
            if (user_acount == "") {
                $(layero).find('#dlg_user_acount').focus();
                return;
            }
            if (user_pwd == "") {
                $(layero).find('#dlg_user_pwd').focus();
                return;
            }
            loginApi(user_acount, user_pwd, function (resp) {
                if (!!fn) fn(resp);
                layer.close(index);
            });
            //按钮【按钮一】的回调
        }, btn2: function (index, layero) {
            //console.log('取消', index, layero);
            //按钮【按钮二】的回调
        }
    });
}
//申请加好友
function applyFriend(id, nickname, no_check) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                applyFriend(id, nickname, true);
            } else {
                showLoginDialog(function () {
                    applyFriend(id, nickname, true);
                });
            }
        });
        return;
    }
    confirm('好友申请',
        '向' + nickname + '发送好友申请？',
        '确认',
        '取消',
        function (index, layerDom) {
            var layerIndex = progress();
            $.ajax({
                type: 'post',
                url: "/Serv/API/Relation/AddCommUserRelation.ashx",
                data: { rtype: 'FriendApply', mainId: id },
                dataType: "json",
                success: function (resp) {
                    layer.close(layerIndex);
                    if (resp.status || resp.code == 10013) {
                        alert('申请已发送');
                        if (data.support_im) zcChat.applyFriend(id, "", function (error, obj) {
                            console.log(error);
                            console.log(obj);
                        });
                    } else if (resp.code == 10010) {
                        showLoginDialog(function () {
                            applyFriend(id, nickname, true);
                        });
                    } else if (resp.code == 10032) {
                        alert('已经是好友');
                    }
                    else if (resp.code == 10013) {
                        alert('已存在申请');
                    } else if (resp.code == 10023) {
                        alert('目前暂只支持用户申请好友');
                    }
                    else {
                        alert('提交出错');
                    }
                }
            });
        })
}
//通过好友申请
function passFriendApply(id, nickname, avatar, idServer, fn, no_check) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                passFriendApply(id, nickname, avatar, idServer, fn, true);
            } else {
                showLoginDialog(function () {
                    passFriendApply(id, nickname, avatar, idServer, fn, true);
                });
            }
        });
        return;
    }
    confirm('通过好友申请',
        '通过' + nickname + '的好友申请？',
        '确认',
        '取消',
        function (index, layerDom) {
            $.ajax({
                type: 'post',
                url: "/Serv/API/Relation/AddCommUserRelation.ashx",
                data: { rtype: 'Friend', mainId: id },
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        alert('通过好友申请');
                        if (data.support_im) zcChat.passFriendApply(id, nickname, avatar, idServer, function (error, obj) {
                            //console.log(error);
                            //console.log(obj);
                            if (!!fn) fn(error, obj);
                        });
                    } else if (resp.code == 10010) {
                        showLoginDialog(function () {
                            passFriendApply(id, nickname, avatar, idServer, fn, true);
                        });
                    } else if (resp.code == 10013) {
                        alert('已是好友');
                        if (!!fn) fn(false);
                    } else {
                        alert('提交出错');
                    }
                }
            });
        });
}
//拒绝好友申请
function rejectFriendApply(id, nickname, idServer, fn, no_check) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                rejectFriendApply(id, nickname, idServer, fn, true);
            } else {
                showLoginDialog(function () {
                    rejectFriendApply(id, nickname, idServer, fn, true);
                });
            }
        });
        return;
    }
    confirm('拒绝好友申请',
        '拒绝' + nickname + '的好友申请？',
        '确认',
        '取消',
        function (index, layerDom) {
            $.ajax({
                type: 'post',
                url: "/Serv/API/Relation/DelCommUserRelation.ashx",
                data: { rtype: 'FriendApply', mainId: id },
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        alert('拒绝好友申请');
                        if (data.support_im) zcChat.rejectFriendApply(id, "", idServer, function (error, obj) {
                            console.log(error);
                            console.log(obj);
                            if (!!fn) fn(error, obj);
                        });
                    } else if (resp.code == 10010) {
                        showLoginDialog(function () {
                            rejectFriendApply(id, nickname, idServer, fn, true);
                        });
                    } else if (resp.code == 10013) {
                        alert('已经拒绝好友申请');
                        if (!!fn) fn(false);
                    } else {
                        aleer('提交出错');
                    }
                }
            });
        });
}
//删除好友
function deleteFriend(id, nickname, fn, no_check) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                deleteFriend(id, nickname, fn, true);
            } else {
                showLoginDialog(function () {
                    deleteFriend(id, nickname, fn, true);
                });
            }
        });
        return;
    }
    confirm('删除好友',
        '删除好友' + nickname + '？',
        '确认',
        '取消',
        function (index, layerDom) {
            $.ajax({
                type: 'post',
                url: "/Serv/API/Relation/DelCommUserRelation.ashx",
                data: { rtype: 'Friend', mainId: id },
                dataType: "json",
                success: function (resp) {
                    if (resp.status) {
                        alert('删除完成');
                        if (data.support_im) zcChat.deleteFriend(id, function (error, obj) {
                            console.log(error);
                            console.log(obj);
                            if (!!fn) fn(error, obj);
                        });
                    } else if (resp.code == 10010) {
                        showLoginDialog(function () {
                            deleteFriend(id, nickname, fn, true);
                        });
                    } else {
                        alert('提交出错');
                        //alert(resp.errmsg);
                    }
                }
            });
        });
}
//查找用户
function findUsers(rows, page, phone, nickname, fn) {
    var layerIndex = progress();
    $.ajax({
        type: 'post',
        url: "/Serv/API/User/FindList.ashx",
        data: { rows: rows, page: page, phone: phone, nickname: nickname },
        dataType: "json",
        success: function (resp) {
            layer.close(layerIndex);
            if (resp.status) {
                if (!!fn) fn(resp.result);
            } else {
                alert('查询出错');
            }
        }
    });
}
//socket在线
function connectSocketServer(id) {
    var support = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);
    if (support == null) {
        alert("不支持WebSocket");
        ws = false;
        return false;
    }
    // create a new websocket and connect
    // put id in path
    try {
        ws = new window[support]('ws://' + window.location.hostname + ':' + port + '/' + id);
    } catch (e) {
        ws = false;
        return ws;
    }
    // when the connection is established, this method is called
    ws.onopen = function () { };
    // when the connection is closed, this method is called
    ws.onmessage = function (evt) {
        var result = JSON.parse(evt.data);
        if (result.action === "GetOnlineFriends") {
            if (data.support_im) zcChat.initOnlineStatus(result.message, function () { });
        } else if (result.action === "FriendLogin") {
            if (data.support_im) zcChat.setOnlineStatus(result.message, true, function () { });
        } else if (result.action === "FriendLogout") {
            if (data.support_im) zcChat.setOnlineStatus(result.message, false, function () { });
        }
    };
    ws.onclose = function () { };
    ws.onerror = function (e) {
        //alert('WebSocket服务器连接失败', 5);
        console.log('WebSocket服务器连接失败');
    }
    return ws;
}
//发送好友列表到websocket
function sendFriendIds(ids) {
    if (data.support_im && !!ws && ws.readyState == 1 && ids.length > 0) {
        ws.send('GetOnlineFriends/' + ids.join(','));
    }
}

//时间转换
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1,
                RegExp.$1.length == 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}

//查询内容列表
function SetOption(baseOption, option) {
    var keys = Object.keys(baseOption);
    for (var i in keys) {
        var key = keys[i];
        if (!!option[key]) {
            baseOption[key] = option[key];
        }
    }
    return baseOption;
}
function GetContent(option, showProgress, fn) {
    var layerIndex;
    if (showProgress) layerIndex = progress();
    var baseOption = {
        pageSize: 6,
        pageIndex: 1,
        type: 'Comment',
        cateId: '',
        keyword: '',
        keyword_author: '0',
        author:'',
        column: 'JuActivityID,ActivityName,Summary,UserID,ThumbnailsPath,CreateDate,CommentCount,PraiseCount,RewardTotal,K3,CategoryId,IsHide',
        hasAuthor: '1',
        hasStatistics: '0',
        order_all: 'Sort desc, JuActivityID desc',
        chk_friend: '1',
        create_start: '',
        is_hide:''
    };
    var option = SetOption(baseOption, option);
    $.ajax({
        type: 'post',
        url: "/Serv/API/Article/GetArticleList.ashx",
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
//查详情
function GetDetail(option, showProgress, fn) {
    var layerIndex;
    if (showProgress) layerIndex = progress();
    var baseOption = {
        article_id: '0',
        no_score: '1',
        no_pv: '1'
    };
    var option = $.extend(baseOption, option);
    $.ajax({
        type: 'post',
        url: "/Serv/API/Article/Get.ashx",
        data: option,
        dataType: "json",
        success: function (resp) {
            if (showProgress) layer.close(layerIndex);
            if (resp.errmsg) {
                alert('查询出错');
            } else {
                fn(resp);
            }
        }
    });
}
//去详情
function ToDetail(jid) {
    window.location.href = '/customize/StockPlayer/Src/Detail/Detail.aspx?jid=' + jid;
}
//编辑
function ToEdit(jid) {
    window.location.href = '/customize/StockPlayer/Src/UserCenter/ReleaseContent/ReleaseContent.aspx?jid=' + jid;
}
//去底部链接文章
function ToBottomLinkArticle(num) {
    var jid = $.trim(data.bottomLinkArtcle[num]);
    ToDetail(jid);
}
//去用户个人中心
function ToCenter() {
    window.location.href = '/customize/StockPlayer/Src/UserCenter/UserCenter.aspx';
}
//去用户个人中心
function ToUser(jid) {
    window.location.href = '/customize/StockPlayer/Src/UserCenter/UserCenter.aspx?id=' + jid;
}

function ToRecharge() {
    window.location.href = '/customize/StockPlayer/Src/UserCenter/ScoreRecharge/ScoreRecharge.aspx';
}
function ToWithdrawCash() {
    window.location.href = '/customize/StockPlayer/Src/UserCenter/ScoreWithdrawCash/ScoreWithdrawCash.aspx';
}
//时间检查显示
function dateFormat(value) {
    var tempTime = new Date();
    var curDay = new Date(tempTime.getFullYear(), tempTime.getMonth(), tempTime.getDate());
    var time = new Date(value);
    if (time > curDay) {
        return time.format('hh:mm');
    } else {
        return time.format('yyyy-MM-dd hh:mm');
    }
}
function dateFormatWeek() {
    var now = new Date();
    var nowTime = now.getTime();
    var day = now.getDay();
    var oneDayLong = 24 * 60 * 60 * 1000;

    var MondayTime = nowTime - (day - 1) * oneDayLong;
    var SundayTime = nowTime + (7 - day) * oneDayLong;

    var monday = new Date(MondayTime);
    var sunday = new Date(SundayTime);
    return monday.Format('yyyy/MM/dd');
}
//检查时评类型
function checkCommentDate(value) {
    var hours = new Date(value).getHours();
    if (hours < 10) {
        return 1;
    } else if (hours > 12) {
        return 3;
    } else {
        return 2;
    }
}
//计算出所有等级图片组合
function initLevelImgs() {
    var spl = ol_icos.split(',');
    var maxl = spl.length;
    var lv = {};
    for (var i = 0; i < spl.length; i++) {
        lv['li' + i] = [];
    }
    for (var i = 1; i <= Math.pow(maxl, maxl) ; i++) {
        if (lv['li' + (spl.length - 1)].length == 4) break;
        var imgs = [];
        for (var j = 0; j < spl.length; j++) {
            lv['li' + j].push(spl[j]);
            if (lv['li' + j].length < 4) break;
            if (j < spl.length - 1) lv['li' + j] = [];
        }
        for (var j = spl.length - 1; j >= 0; j--) {
            imgs = imgs.concat(lv['li' + j]);
        }
        data.levels[i] = { lv: i, imgs: imgs };
    }
    //console.log(data.levels);
}
//获取用户等级图标
function getUserLevelImgs(id, times) {
    if (!!data.user_levels[id]) return data.user_levels[id];
    data.user_levels[id] = getLevelImgs(getUserLevel(times));
    return data.user_levels[id];
}
//获取用户等级
function getUserLevel(times) {
    var level = 0;
    if (typeof (times) == 'string') times = parseInt(times);
    times = parseInt(times / 60);
    var maxl = ol_icos.split(',').length;
    try {
        for (var i = 0; i <= Math.pow(maxl, maxl) ; i++) {
            var tstr = ol_s.replace(/[L]/g, i);
            tstr = tstr.replace(/[A]/g, ol_a);
            tstr = tstr.replace(/[B]/g, ol_b);
            if (eval(tstr) < times) {
                level = i;
            } else {
                break;
            }
        }
    } catch (e) {
        alert('等级公司错误');
    }
    return level;
}
//获取对应等级图标
function getLevelImgs(level) {
    var lv = data.levels[level];
    if (!lv) return { lv: level, imgs: [] };
    return lv;
}
function getUnreadMsgCount(fn) {
    $.ajax({
        type: 'post',
        url: '/Serv/API/SystemNotice/UnReadCount.ashx',
        dataType: "json",
        success: function (resp) {
            if (resp.status) {
                if (!!fn) fn(resp.result);
            } else {
                //alert(resp.msg);
            }
        }
    });
}
//二维码登录
function qrCodeLogin() {
    var html = new StringBuilder();
    html.AppendFormat('<div class="dlg-qrcode">');
    html.AppendFormat('<div class="qrcode-tip">正在生成<br />二维码</div>');
    html.AppendFormat('<div class="qrcode-img">');
    html.AppendFormat('<img />');
    html.AppendFormat('</div>');
    html.AppendFormat('</div>');
    qrLayerIndex = layer.open({
        type: 1,
        title: '二维码登录',
        skin: 'layui-layer-qrcode', //样式类名
        closeBtn: 0, //不显示关闭按钮
        move: false,//禁止移动
        area: ['260px', '360px'],
        content: html.ToString(),
        btn: ['取消'], //按钮
        yes: function (index, layero) {
            layer.close(index);
        }
    });

    var support = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);
    if (support == null) {
        alert("不支持WebSocket", 5);
        qrws = false;
        return qrws;
    }
    // create a new websocket and connect
    // put id in path

    try {
        qrws = new window[support]('ws://' + window.location.hostname + ':' + port + '/QRCode');
    } catch (e) {
        qrws = false;
        return qrws;
    }

    // when the connection is established, this method is called
    qrws.onopen = function (e) {
        //console.log(e);
    };

    qrws.onmessage = function (evt) {
        var result = JSON.parse(evt.data);
        if (result.status === 0) {
            var redisKey = result.redisKey;
            $('.layui-layer-qrcode .qrcode-img img').attr('src', '/Handler/ImgHandler.ashx?v=' + encodeURIComponent('http://' + window.location.host + '/customize/Communal/QRCodeLogin.aspx?redis_key=' + redisKey));
            $('.layui-layer-qrcode .qrcode-tip').hide();
            $('.layui-layer-qrcode .qrcode-img').show();
        } else if (result.status === 1) {
            $.ajax({
                type: 'post',
                url: '/Serv/API/User/QRCodeLogin.ashx',
                data: { redis_key: result.redisKey },
                dataType: 'JSON',
                success: function (resp) {
                    if (resp.status) {
                        is_login = true;
                        if (reloadPages.indexOf(window.location.pathname.toLowerCase()) >= 0) {
                            window.location.href = window.location.href;
                            return;
                        }
                        if (data.support_im) zcChat.login(resp.result.id.toString(), resp.im_token);

                        qrws.close();
                        ws = connectSocketServer(resp.result.id);

                        $('.wrapHead .mHead .login .useravatar').attr('src', resp.result.avatar);
                        $('.wrapHead .mHead .login .userdispalyname').text(resp.result.nickname);
                        if (!$('.wrapHead .mHead .noLogin').hasClass('hidden')) $('.wrapHead .mHead .noLogin').addClass('hidden');
                        if ($('.wrapHead .mHead .login').hasClass('hidden')) $('.wrapHead .mHead .login').removeClass('hidden');

                        alert('登录成功');
                        layer.close(qrLayerIndex);
                    } else {
                        //alert(resp.msg);
                    }
                }
            });
        } else if (result.status === 9) {
            $('.layui-layer-qrcode .qrcode-tip').text('生成二维码出错：' + result.msg);
            qrws.close();
        }
    };
    // when the connection is closed, this method is called
    qrws.onclose = function () { };
    qrws.onerror = function (e) {
        //alert('WebSocket服务器连接失败', 5);
        $('.layui-layer-qrcode .qrcode-tip').html('WebSocket服务器<br />连接失败');
        console.log('WebSocket服务器连接失败');
    }
    return qrws;
}
//绑定微信
function bindWXOpenId(id) {
    var html = new StringBuilder();
    html.AppendFormat('<div class="dlg-qrcode">');
    html.AppendFormat('<div class="qrcode-tip">正在生成<br />二维码</div>');
    html.AppendFormat('<div class="qrcode-img">');
    html.AppendFormat('<img src="{0}" onload="showQrImg(this)" />', '/Handler/ImgHandler.ashx?v=' + encodeURIComponent('http://' + window.location.host + '/customize/Communal/BindWXOpenId.aspx?id=' + id));
    html.AppendFormat('</div>');
    html.AppendFormat('</div>');
    qrLayerIndex = layer.open({
        type: 1,
        title: '绑定微信',
        skin: 'layui-layer-qrcode', //样式类名
        closeBtn: 0, //不显示关闭按钮
        move: false,//禁止移动
        area: ['260px', '360px'],
        content: html.ToString(),
        btn: ['取消'], //按钮
        yes: function (index, layero) {
            layer.close(index);
        }
    });
}
//分享加积分
function bindShareWeiXin(id, openid) {
    var h = '';
    var html = new StringBuilder();
    html.AppendFormat('<div class="dlg-qrcode">');
    html.AppendFormat('<div class="qrcode-tip">正在生成<br />二维码</div>');
    html.AppendFormat('<div class="qrcode-img">');
    html.AppendFormat('<img src="{0}" onload="showQrImg(this)" />', '/Handler/ImgHandler.ashx?v=' + encodeURIComponent('http://' + window.location.host + '/customize/StockPlayer/Src/Index/Index.aspx'));
    html.AppendFormat('</div>');
    html.AppendFormat('</div>');
    if (openid == '0') {
        h = '380px'
        html.AppendFormat('<div class="share-score"><span>绑定微信后分享可加{0}。<span>', score_name);
        html.AppendFormat('<a href="javascript:;"  onclick="ToWXQRCode({0})">去绑定</a>', id);
        html.AppendFormat('</div>');
    } else {
        h = '360px';
    }
    qrLayerIndex = layer.open({
        type: 1,
        title: '分享到微信',
        skin: 'layui-layer-qrcode', //样式类名
        closeBtn: 0, //不显示关闭按钮
        move: false,//禁止移动
        area: ['260px', h],
        content: html.ToString(),
        btn: ['取消'], //按钮
        yes: function (index, layero) {
            layer.close(index);
        }
    });
}

function ToWXQRCode(id) {

    bindWXOpenId(id);
}

function showQrImg(obj) {
    $(obj).closest('.dlg-qrcode').find('.qrcode-tip').hide();
    $(obj).closest('.dlg-qrcode').find('.qrcode-img').show();
}

//通知买家
function showDialogSendNotice(nc_price, id, title, no_check,stockname) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                showDialogSendNotice(nc_price, id, title, true, stockname);
            } else {
                showLoginDialog(function (resp) {
                    showDialogSendNotice(nc_price, id, title, true, stockname);
                });
            }
        });
        return;
    }
    var html = new StringBuilder();
    html.AppendFormat('<div class="row dlg-notice-type">');
    html.AppendFormat('<div class="col-xs-3">通知方式：</div>');
    html.AppendFormat('<div class="col-xs-9">');
    html.AppendFormat('<span class="span-type selected" data-type="1">留言通知（免费）</span>');
    if (parseInt(nc_price) >= 0) {
        html.AppendFormat('<span class="span-type" data-type="2">短信通知（需要<span class="span-num">{0}</span>{1}）</span>', nc_price, score_name);
    }
    html.AppendFormat('</div>');
    html.AppendFormat('</div>');
    html.AppendFormat('<div class="row">');
    html.AppendFormat('<div class="col-xs-12 dlg-notice-content"><textarea class="form-control textCheck notice-content" placeholder="请输入通知内容" maxLength="50" rows="5"></textarea></div>');
    html.AppendFormat('</div>');
    layer.open({
        type: 1,
        title: title,
        skin: 'layui-layer-send-notice', //样式类名
        closeBtn: 0, //不显示关闭按钮
        move: false,//禁止移动
        //shift: 2,
        //shadeClose: true, //开启遮罩关闭
        content: html.ToString(),
        area: '560px',
        btn: ['确认', '取消'], //按钮
        yes: function (index, layero) {
            if ($(layero).find('.layui-layer-btn0').hasClass('btn-disabled')) {
                return;
            }
            var type = $.trim($(layero).find('.span-type.selected').attr('data-type'));
            var content = $.trim($(layero).find('.notice-content').val());
            if (content == "") {
                alert('请输入通知内容');
                $(layero).find('.notice-content').focus();
                return;
            }
            if (content.length > 50) {
                alert('通知内容最多能用50个汉字');
                return;
            }
            sendNotice(index, id, type, content, false, layero,stockname);
        }, btn2: function (index, layero) {
        }
    });
}
function sendNotice(layerIndex, id, type, content, no_check, layero,stockname) {
    if (!no_check) {
        checkLogin(function (result) {
            if (result.is_login) {
                sendNotice(layerIndex, id, type, content, true, layero, stockname);
            } else {
                showLoginDialog(function (resp) {
                    sendNotice(layerIndex, id, type, content, true, layero, stockname);
                });
            }
        });
        return;
    }
    $(layero).find('.layui-layer-btn0').text('正在发送...');
    $(layero).find('.layui-layer-btn0').addClass('btn-disabled');
    $.ajax({
        type: 'post',
        url: '/Serv/API/SystemNotice/Send.ashx',
        data: { id: id, type: type, content: content, module_name: score_name, article_name: stockname },
        dataType: 'JSON',
        success: function (resp) {
            $(layero).find('.layui-layer-btn0').text('确认');
            $(layero).find('.layui-layer-btn0').removeClass('btn-disabled');
            if (resp.status) {
                alert(resp.msg);
                layer.close(layerIndex);
            } else {
                alert(resp.msg);
            }
        }
    });
}

function shareInit() {

    var shareCallBackFunc = {
        timeline_s: function () {
            //分享到朋友圈
            shareComeplete();
        },
        timeline_c: function () {
            //朋友圈分享取消
        },
        message_s: function () {
            //分享给朋友
            shareComeplete();
        },
        message_c: function () {
            //朋友分享取消
        }
    }
    //分享
    wx.ready(function () {
        wxapi.wxshare({
            title: data.shareData.title,
            desc: data.shareData.summary,
            link: data.shareData.shareUrl,
            imgUrl: data.shareData.shareImgUrl
        }, shareCallBackFunc)
    });
}
function shareComeplete() {
    //分享到朋友圈
    $.ajax({
        type: 'post',
        url: '/Serv/API/Score/Add.ashx',
        data: { type: "ShareWebsite" },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
            }
        }
    });
}

function getFriendList(fn) {
    $.ajax({
        type: 'post',
        url: '/Serv/API/User/Friend/List.ashx',
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                if (!!fn) fn(data.result.list);
            }
        }
    });
}

function getUsersByIds(ids, fn) {
    $.ajax({
        type: 'post',
        url: '/Serv/API/User/FindList.ashx',
        data: { ids: ids.join(','), page: 1, rows: ids.length },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                if (!!fn) fn(data.result);
            }
        }
    });
}
function isPositiveInteger(s) {//是否为正整数
    var re = /^[0-9]+$/;
    return re.test(s)
}