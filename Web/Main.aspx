<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Main1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>至云微营销管理平台</title>
    <script src="/MainStyle/Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="/MainStyle/Res/easyui/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="/MainStyle/Res/easyui/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <link href="/MainStyle/Res/easyui/themes/gray/easyui.css" rel="stylesheet" type="text/css" />
    <link href="/MainStyle/Content/default.css" rel="stylesheet" type="text/css" />
    <link href="/MainStyle/Res/easyui/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/JPlayer/jquery.jplayer.min.js" type="text/javascript"></script>
    <% string workbenchUrl = "http://" + this.Request.Url.Authority + "/Home/Workbench.aspx"; %>
    <script type="text/javascript">
        // Note on the iPad you may want to use "touchstart" instead.
        //        var MsgUrl = "http://" + window.location.host + "/Media/msg.mp3"; //提示音地址
        //        $(function () {
        //            $("#jplayer").jPlayer({
        //                swfPath: "/Scripts/JPlayer/Jplayer.swf",
        //                ready: function () {
        //                    $(this).jPlayer("setMedia", {
        //                        mp3: "/Media/msg.mp3"
        //                    });
        //                },
        //                supplied: "mp3"
        //            });

        //           
        //        });
        //        function PlaySound() {
        //            $("#jplayer").jPlayer('play');
        //            setInterval("PlaySound()", 5000);
        //            return true;
        //        }
        //        setTimeout("$('#jplayer').jPlayer('play')", 5000);
    </script>
    <script type="text/javascript">
   
        $(function () {
                $("#jplayer").jPlayer({
                swfPath: "/Scripts/JPlayer/Jplayer.swf",
                ready: function () {
                    $(this).jPlayer("setMedia", {
                        mp3: "/Media/msg.mp3"
                    });
                },
                supplied: "mp3"
            });
     
         setInterval(ShowRemind, 120000); //待办事项提醒
         setInterval(ShowWeiBoEventDetailsRemind, 120000); //微博点击提醒
         setInterval(ShowEmailEventDetailsRemind, 120000); //邮件提醒
            tabCloseEven();
            addTab("工作台", "<%=workbenchUrl %>", "icon-tip", false);
            $('li a').click(function () {
                var tabTitle = $(this).text();
                var url = $(this).attr("rel");
                var icon = $(this).attr("icon"); //获取图标
                if (icon == "") {
                    icon = "icon-save";
                }
                addTab(tabTitle, url, icon, true);

            });

            $('#loginOut').click(function () {
                $.messager.confirm('系统提示', '您确定要退出本次登录吗?', function (r) {

                    if (r) {
                        <% string logoutUrl = ZentCloud.Common.ConfigHelper.GetConfigString("logoutUrl") + "?op=logout"; %>
                        location.href = '<%=logoutUrl %>';
                    }
                });
            });

        })

        function createFrame(url) {
            var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;overflow-y: auto; "></iframe>';
            return s;
        }
        function addTab(subtitle, url, icon, closable) {
            if (!$('#tabs').tabs('exists', subtitle)) {
                $('#tabs').tabs('add', {
                    title: subtitle,
                    content: createFrame(url),
                    closable: closable
                    , icon: icon
                });
            } else {
                $('#tabs').tabs('select', subtitle);

            }
            tabClose();
        }
        //是否存在
        function isExistsTab(subtitle)
        {
            if ($('#tabs').tabs('exists', subtitle)) {
                return true;
            } 
            return false;
        }
        function tabClose() {
            /*双击关闭TAB选项卡*/
            $(".tabs-inner").dblclick(function () {
                var subtitle = $(this).children(".tabs-closable").text();
                $('#tabs').tabs('close', subtitle);
            })
            /*为选项卡绑定右键*/
            $(".tabs-inner").bind('contextmenu', function (e) {
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });

                var subtitle = $(this).children(".tabs-closable").text();

                $('#mm').data("currtab", subtitle);
                $('#tabs').tabs('select', subtitle);
                return false;
            });
        }
        //绑定右键菜单事件
        function tabCloseEven() {
            //刷新
            $('#mm-tabupdate').click(function () {
                var currTab = $('#tabs').tabs('getSelected');
                var url = $(currTab.panel('options').content).attr('src');
                $('#tabs').tabs('update', {
                    tab: currTab,
                    options: {
                        content: createFrame(url)
                    }
                })
            })
            //关闭当前
            $('#mm-tabclose').click(function () {
                var currtab_title = $('#mm').data("currtab");
                $('#tabs').tabs('close', currtab_title);
            })
            //全部关闭
            $('#mm-tabcloseall').click(function () {
                $('.tabs-inner span').each(function (i, n) {
                    var t = $(n).text();
                    $('#tabs').tabs('close', t);
                });
            });
            //关闭除当前之外的TAB
            $('#mm-tabcloseother').click(function () {
                $('#mm-tabcloseright').click();
                $('#mm-tabcloseleft').click();
            });
            //关闭当前右侧的TAB
            $('#mm-tabcloseright').click(function () {
                var nextall = $('.tabs-selected').nextAll();
                if (nextall.length == 0) {
                    //msgShow('系统提示','后边没有啦~~','error');
                    //			alert('后边没有啦~~');
                    return false;
                }
                nextall.each(function (i, n) {
                    var t = $('a:eq(0) span', $(n)).text();
                    $('#tabs').tabs('close', t);
                });
                return false;
            });
            //关闭当前左侧的TAB
            $('#mm-tabcloseleft').click(function () {
                var prevall = $('.tabs-selected').prevAll();
                if (prevall.length == 0) {
                    return false;
                }
                prevall.each(function (i, n) {
                    var t = $('a:eq(0) span', $(n)).text();
                    $('#tabs').tabs('close', t);
                });
                return false;
            });

            //退出
            $("#mm-exit").click(function () {
                $('#mm').menu('hide');
            })
        }
        function tabCloseByTitle(title)
        {
            $('#tabs').tabs('close', title);
        }
         //信息提醒-----------
         function messagerremind(title, msg) {
             $.messager.show({
                 title: title,
                 msg: msg,
                 timeout:0,
                 width:300,
                 height:200
             });
            
            $('#jplayer').jPlayer('play');

         
         }
         //信息提醒-----------

//     ///获取待办事项提醒
//     function ShowRemind() {

//         $.post("/Handler/User/UserRemindManage.ashx", { Action: "GetRemindByTime" }, function (result) {
//             if (result!="") {

//             var msg ="您有以下待办事项:</br>"+result;
//             messagerremind("系统提示：", msg);

//             }


//         })
//     ///获取待办事项提醒

          ///获取待办事项提醒
     function ShowRemind() {

               jQuery.ajax({
                   type: "Post",
                   url: "/Handler/User/UserRemindManage.ashx",
                   data: { Action: "GetRemindByTime"},
                   dataType:"html",
                   success: function (result) {
                    if (result!="") {
                     var msg ="您有以下待办事项:</br>"+result;
                     messagerremind("系统提示：", msg);

             }
                   }
               }) 


 }
     ///获取待办事项提醒

//     //微博事件点击提醒
//      function ShowWeiBoEventDetailsRemind() {

//     $.post("/Handler/User/UserRemindManage.ashx", { Action: "GetWeiBoEventDetailsInfoRemind" }, function (result) {
//         if (result != "") {            
//             messagerremind("系统提示：", result);

//         }


//     })

// }
//     //微博事件点击提醒


          //微博事件点击提醒
      function ShowWeiBoEventDetailsRemind() {

               jQuery.ajax({
                   type: "Post",
                   url: "/Handler/User/UserRemindManage.ashx",
                   data: { Action: "GetWeiBoEventDetailsInfoRemind"},
                   dataType:"html",
                   success: function (result) {
                    if (result!="") {
                     
                     messagerremind("系统提示：",result);

             }
                   }
               }) 

 }
     //微博事件点击提醒


//     //邮件事件点击提醒
//     function ShowEmailEventDetailsRemind()
// {

//     $.post("/Handler/User/UserRemindManage.ashx", { Action: "GetEmailEventDetailsInfoRemind" }, function (result) {
//         if (result != "") {
//             messagerremind("系统提示：", result);

//         }


//     })

// }
//     //邮件事件点击提醒


          //邮件事件点击提醒
     function ShowEmailEventDetailsRemind()
 {
              jQuery.ajax({
                   type: "Post",
                   url: "/Handler/User/UserRemindManage.ashx",
                   data: { Action: "GetEmailEventDetailsInfoRemind"},
                   dataType:"html",
                   success: function (result) {
                    if (result!="") {
                     
                     messagerremind("系统提示：",result);

             }
                   }
               }) 


 }
     //邮件事件点击提醒


    </script>
    <style type="text/css">
        body
        {
            font-family: 微软雅黑,新宋体;
        }
        a
        {
            color: Black;
            text-decoration: none;
        }
        .easyui-tree li
        {
            margin: 5px 0px 0px 0px;
            padding: 1px;
        }
    </style>
</head>
<body class="easyui-layout">
    <%-- <div region="north" split="true" border="false" style="text-align: center; background: #993400;
        direction: ltr; height: 76px;">--%>
    <div region="north" split="true" border="false" style="text-align: center; background: #EFEFEF;
        direction: ltr; height: 76px;">
        <div id="header-inner">
            <table cellpadding="0" cellspacing="0" style="width: 100%; height: 60px">
                <tbody>
                    <tr>
                        <td rowspan="2" style="width: 50px; height: 50px;">
                            <img alt="正在加载" src="img/logo_zeny1.png" width="50px" height="50px" />
                        </td>
                        <td style="height: 52px;" align="left">
                            <div style="color: #3f3f3f; font-size: 22px; font-weight: bold;">
                                至云微营销管理平台
                            </div>
                            <%--<div style="color: #3f3f3f">
                                一站式在线营销云平台（短信、邮件、会议、活动、微博、微信、会员、社区）
                            </div>--%>
                        </td>
                        <td style="padding-right: 5px; text-align: right; vertical-align: bottom;">
                            <div id="topmenu" style="color: #3f3f3f;">
                                <a href="/Help/Index.aspx" target="_blank" style="color: #3f3f3f;">帮助中心</a> <a href="/FeedBack/FeedBack.aspx"
                                    target="_blank" style="color: #3f3f3f;">问题反馈</a> <a href="javascript:;" id="loginOut"
                                        style="color: #3f3f3f;">安全退出</a>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div style="height: 10px; width: 100%; background-color: #85bafc;">
            </div>
        </div>
    </div>
    <div region="west" hide="true" split="true" title="功能导航" style="width: 200px;" id="west">
        <div class="easyui-accordion" fit="true" border="false">
            <%= menuHtml %>
            <%--<div data-options="iconCls:'tu0810'" title="短信平台">
                <div class="easyui-panel" fit="true" border="false">
                    <ul class="easyui-tree">
                        <li data-options="iconCls:'tu0910'"><a href="javascript:;" icon="tu0910" rel="/SMS/Send.aspx">
                            发短信</a></li>
                        <li data-options="iconCls:'tu0404'"><a href="javascript:;" icon="tu0404" rel="/Member/MemberList.aspx">
                            号码库</a></li>
                        <li data-options="iconCls:'tu0414'"><a href="javascript:;" icon="tu0414" rel="/SMS/PlanList.aspx">
                            短信发送历史</a></li>
                    </ul>
                </div>
            </div>
            <div data-options="iconCls:'tu0504'" title="邮件平台">
                <div class="easyui-panel" fit="true" border="false">
                    <ul class="easyui-tree">
                        <li data-options="iconCls:'tu2010'"><a href="javascript:;" icon="tu2010" rel="http://www.comeoncloud.com/">
                            发邮件</a></li>
                        <li data-options="iconCls:'tu0414'"><a href="javascript:;" icon="tu0414" rel="http://www.comeoncloud.com/">
                            邮件发送历史</a></li>
                    </ul>
                </div>
            </div>
            <div data-options="iconCls:'tu0711'" title="微博平台">
                <div class="easyui-panel" fit="true" border="false">
                    <ul class="easyui-tree">
                        <li data-options="iconCls:'tu0522'"><a href="javascript:;" icon="tu0522" rel="/Weibo/Account.aspx">
                            授权信息</a></li>
                        <li data-options="iconCls:'tu0221'"><a href="javascript:;" icon="tu0221" rel="/Weibo/Friendships.aspx">
                            粉丝管理</a></li>
                        <li data-options="iconCls:'tu0611'"><a href="javascript:;" icon="tu0611" rel="/Weibo/SendSingel.aspx">
                            单条发送微博 </a></li>
                        <li data-options="iconCls:'tu0611'"><a href="javascript:;" icon="tu0611" rel="/Weibo/SendBatch.aspx">
                            批量发送微博</a></li>
                        <li data-options="iconCls:'tu0414'"><a href="javascript:;" icon="tu0414" rel="/Weibo/History.aspx">
                            微博发送历史</a></li>
                    </ul>
                </div>
            </div>
            <div data-options="iconCls:'tu0625'" title="微信平台">
                <div class="easyui-panel" fit="true" border="false">
                    <ul class="easyui-tree">
                        <li data-options="iconCls:'tu2010'"><a href="javascript:;" icon="tu2010" rel="http://www.comeoncloud.com/">
                            发微信</a></li>
                        <li data-options="iconCls:'tu0414'"><a href="javascript:;" icon="tu0414" rel="http://www.comeoncloud.com/">
                            微信发送历史</a></li>
                    </ul>
                </div>
            </div>
            <div data-options="iconCls:'tu0625'" title="会议活动">
                <div class="easyui-panel" fit="true" border="false">
                    <ul class="easyui-tree">
                        <li data-options="iconCls:'tu0601'"><a href="javascript:;" icon="tu0601" rel="/Meeting/AddMeeting.aspx">
                            新建会议</a></li>
                        <li data-options="iconCls:'tu0701'"><a href="javascript:;" icon="tu0701" rel="/Meeting/MeetingList.aspx">
                            所有会议</a></li>
                        <li data-options="iconCls:'tu1911'"><a href="javascript:;" icon="tu1911" rel="/Meeting/MeetingEnroll.aspx">
                            批量报名</a></li>
                        <li data-options="iconCls:'tu1001'"><a href="javascript:;" icon="tu1001" rel="/Meeting/MeetingSign.aspx">
                            会议签到</a></li>
                        <li data-options="iconCls:'tu0414'"><a href="javascript:;" icon="tu0414" rel="/Member/MemberList.aspx">
                            客户信息库</a></li>
                    </ul>
                </div>
            </div>
            <div data-options="iconCls:'tu0625'" title="客户管理">
                <div class="easyui-panel" fit="true" border="false">
                    <ul class="easyui-tree">
                        <li data-options="iconCls:'tu1911'"><a href="javascript:;" icon="tu1911" rel="/Member/MemberList.aspx">
                            客户管理</a></li>
                    </ul>
                </div>
            </div>
            <div data-options="iconCls:'tu0306'" title="账户中心">
                <div class="easyui-panel" fit="true" border="false">
                    <ul class="easyui-tree">
                        <li data-options="iconCls:'tu0306'"><a href="javascript:;" icon="tu0306" rel="/Trac/TracHistory.aspx">
                            账户历史记录</a></li>
                    </ul>
                </div>
            </div>
           
            <div data-options="iconCls:'tu2011'" title="系统设置">
                <div class="easyui-panel" fit="true" border="false">
                    <ul class="easyui-tree">
                        <li data-options="iconCls:'tu0504'"><span>用户管理</span>
                            <ul class="easyui-tree">
                                <li data-options="iconCls:'tu1001'"><a href="javascript:;" icon="tu1001" rel="/User/Add.aspx">
                                    添加新用户</a></li>
                                <li data-options="iconCls:'tu1911'"><a href="javascript:;" icon="tu1911" rel="/User/List.aspx">
                                    用户列表</a></li>
                                <li data-options="iconCls:'tu0704'"><a href="javascript:;" icon="tu0704" rel="http://www.comeoncloud.com/">
                                    查找用户</a></li>
                            </ul>
                        </li>
                        <li data-options="iconCls:'tu0525'"><a href="javascript:;" icon="tu0525" rel="http://www.comeoncloud.com/?q=1">
                            角色管理</a></li>
                        <li data-options="iconCls:'tu0701'"><a href="javascript:;" icon="tu0701" rel="/Home/Workbench.aspx">
                            菜单管理</a></li>
                    </ul>
                </div>
            </div>--%>
        </div>
    </div>
    <div id="mainPanle" region="center" style="background: #eee; overflow-y: hidden;">
        <div id="tabs" class="easyui-tabs" fit="true" border="false">
        </div>
    </div>
    <div region="south" split="true" style="height: 29px;">
        <div style="padding: 0px; margin-left: 50%;">
            <a target="_blank" href="http://www.comeoncloud.com/">上海至云信息科技有限公司</a>
        </div>
    </div>
    <div id="mm" class="easyui-menu" style="width: 150px;">
        <div id="mm-tabupdate">
            刷新</div>
        <div class="menu-sep">
        </div>
        <div id="mm-tabclose">
            关闭</div>
        <div id="mm-tabcloseall">
            全部关闭</div>
        <div id="mm-tabcloseother">
            除此之外全部关闭</div>
        <div class="menu-sep">
        </div>
        <div id="mm-tabcloseright">
            当前页右侧全部关闭</div>
        <div id="mm-tabcloseleft">
            当前页左侧全部关闭</div>
    </div>
    <div id="jplayer">
    </div>
</body>
</html>
