
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexV2.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.IndexV2" %>

<!DOCTYPE html>
<html>
<head>
    <title>颂和月供宝平台</title>
    <meta name="renderer" content="webkit">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1" >
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
    <link href="http://static-files.socialcrmyun.com/lib/wdScrollTab/css/TabPanel.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/css/master/indexv1.css?v=20160902" rel="stylesheet" type="text/css" />
    <link href="http://static-files.socialcrmyun.com/lib/layer/2.1/skin/layer.css" rel="stylesheet" />
    <link href="http://static-files.socialcrmyun.com/MainStyleV2/css/bootstrap.min.css" rel="stylesheet">
    <link href="http://file.comeoncloud.net/MainStyleV2/font-awesome/css/font-awesome.css" rel="stylesheet">
    <link href="http://static-files.socialcrmyun.com/MainStyleV2/css/animate.css" rel="stylesheet">
    <link href="http://static-files.socialcrmyun.com/MainStyleV2/css/style.css" rel="stylesheet">
</head>
<body>
    <style>
        .logov1 {
            width: 180px;
            height: 48px;
            float: left;
            background-repeat: no-repeat;
            background-position: center;
            background-image: url(http://songhe-file.oss-cn-hangzhou.aliyuncs.com/www/jubit/jubit/image/20170110/logolong.png);
            background-size: 90%;
            background-color: #2F4050;
        }
        .tabpanel_mover li .title {
            font-size: 12px;
            padding-left: 15px;
            padding-right: 20px;
            width: auto !important;
            max-width: 120px;
            overflow: hidden;
            display: inline-block;
            white-space: nowrap;
            text-overflow: ellipsis;
        }
    </style>
    <div class="header">
        <div class="logov1">
        </div>
        <div class="logout"></div>
        <div class="logout">
            <span class="">欢迎您！</span>&nbsp;&nbsp;&nbsp;&nbsp;
            
            <span class="">当前用户：<%=curUserID %></span>&nbsp;&nbsp;&nbsp;&nbsp;
            
            <a href="javascript:void(0);" id="spchangpassword">[修改密码]</a>&nbsp;
            <a href="<%=LogoutHref %>">[安全退出]</a>
        </div>
    </div>
    <div class="main">
        <div class="nav wrap-nav">

             <nav class="navbar-default navbar-static-side" role="navigation">

                <div class="sidebar-collapse">
                    <ul class="nav metismenu" id="side-menu">
                           <% = menuString %>
                     </ul>

                </div>

             </nav>
           
        </div>
        <div class="concent" id="indexConcent">

        </div>
    </div>
</body>
    <script src="http://static-files.socialcrmyun.com/lib/jquery/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/MainStyleV2/js/bootstrap.min.js"></script>
    <script src="http://static-files.socialcrmyun.com/lib/layer/2.1/layer.js"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/Common.js"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/global.js?v=20160325sp1"></script>
    <script src="http://static-files.socialcrmyun.com/lib/wdScrollTab/js/Fader.js"></script>
    <script src="/lib/wdScrollTab/js/TabPanel.js"></script>
    <script src="http://static-files.socialcrmyun.com/lib/wdScrollTab/js/Math.uuid.js"></script>
    <script type="text/javascript" src="http://static-files.socialcrmyun.com/Scripts/jquery.mousewheel.min.js"></script>
    <script type="text/javascript" src="http://static-files.socialcrmyun.com/Scripts/scrollbar.js"></script>
    <!-- Mainly scripts -->
    <script src="http://static-files.socialcrmyun.com/MainStyleV2/js/plugins/metisMenu/jquery.metisMenu.js"></script>
    <script src="http://static-files.socialcrmyun.com/MainStyleV2/js/plugins/slimscroll/jquery.slimscroll.min.js"></script>
    <!-- Custom and plugin javascript -->
    <script src="http://static-files.socialcrmyun.com/MainStyleV2/js/inspinia.js"></script>
    <!-- Easyui menu -->
    <script src="/lib/jquery/BootstrapMenu/BootstrapMenu.min.js"></script>

      <%if (!string.IsNullOrWhiteSpace(ExpirationDate))
                { %>
         <script type="text/javascript">

             var expirationDate = '<%=ExpirationDate%>';
             var toDay = '<%=DateTime.Now.ToString("yyyy-MM-dd")%>';

             if (new Date(expirationDate) > new Date(toDay)) {
                 $('#spIsOverExpirationDate').html();
             }else{
                 $('#spIsOverExpirationDate').html('(已过期请续费)');
             }
             

         </script>
            <%} %>

    <script type="text/javascript">

        
        $(function () {
            //检查是否登录
            setInterval("CheckLogin()", 30000);
            var navscorll = new scrollbar(".wrap-nav");
            navscorll.init($(window).height() - 60)

            $("#side-menu li").bind("click", function () {
                var $this = $(this);
                var path = $this.attr("data-rel");
                var title = $this.find('a').text();
                //if (path) {
                //    $('#iframeMain').attr('src', path);
                //}
                if (!path) {
                    return;
                }
                addTab(title, path);
            });

            $("#spchangpassword").click(function () {

                //$('#iframeMain').attr('src', '/App/Cation/SetPwd.aspx');
                addTab('修改密码', '/App/Cation/SetPwd.aspx');
            });

            

            var tabpanel_items_storage = localStorage.getItem('tabpanelItems');


            var indexPanel;
            var indexItems = [];
            var tabpanel_items = [
                {
                    id: 'tabToolbarPlugin3',
                    title: '工作台',
                    closable:false,
                    html: '<iframe src="/customize/mmpadmin/index.aspx?hidemenu=1#/index/hometablepage" width="100%" height="' + ($(window).height() - $('.header').height()) + '" frameborder="0" class="tabMainContent" id="tabToolbarPlugin3Frame"></iframe>'
                    //html: '<div style="padding: 30px;">欢迎使用至云营销平台</div><div class="" style="font-size:18px; float:left !important; padding: 30px; "><a style="color:#518dca;" target="_blank" href="javascript:V5Notice();">移动营销平台V5版隆重发布及用户奖励政策(hot!)</a></div>', closable: false
                }
            ];

            if (tabpanel_items_storage) {
                tabpanel_items = JSON.parse(tabpanel_items_storage);
            }

            //window.onbeforeunload = function () {
            //    sessionStorage.setItem('tabpanelItems', JSO.stringfy(tabpanel.items));
            //    return true;
            //}

            setInterval(function () {
                var _arr = [];
                for (var i = 0; i < tabpanel.tabs.length; i++) {
                    var _t = tabpanel.tabs[i];

                    _arr.push({
                        id: _t.id,
                        title: $(_t.title).text(),
                        html: _t.html,
                        closable: _t.closable
                    });

                }
                localStorage.setItem('tabpanelItems', JSON.stringify(_arr));
            },5000);

            tabpanel = new TabPanel({
                renderTo: 'indexConcent',
                //width: '100%',
                //height: '100%',
                autoResizable: true,
                //border:'none',  
                active: 0,
                //maxLength : 10,  
                items: tabpanel_items
            });
            var tabMenu = new BootstrapMenu('.tabpanel_mover li', {
                fetchElementData: function ($rowElem) {
                    var item = {
                        chk_id: $rowElem.get(0).id,
                        act_id: tabpanel.getActiveTab().id,
                    }
                    return item;
                },
                actions: [{
                    name: '关闭当前',
                    isShown: function (item) {
                        return item.chk_id != 'tabToolbarPlugin3';
                    },
                    onClick: function (item) {
                        if (item.chk_id == 'tabToolbarPlugin3') {
                            tabpanel.show(item.chk_id, false);
                            return;
                        }
                        if (item.chk_id == item.act_id) {
                            tabpanel.kill(item.chk_id);
                        }
                        else {
                            tabpanel.kill(item.chk_id, item.act_id);
                        }
                    }
                },{
                    name: '关闭全部',
                    isShown: function (item) {
                        return item.chk_id != 'tabToolbarPlugin3';
                    },
                    onClick: function (item) {
                        var nLi = ['tabToolbarPlugin3'];
                        var closeIdList = [];
                        for (var i = 0; i < tabpanel.tabs.length; i++) {
                            if (nLi.indexOf(tabpanel.tabs[i].id) < 0) closeIdList.push(tabpanel.tabs[i].id);
                        }
                        if (closeIdList.length > 0) tabpanel.killList(closeIdList, 'tabToolbarPlugin3');
                    }
                },{
                    name: '关闭其他',
                    onClick: function (item) {
                        var nLi = [item.chk_id, 'tabToolbarPlugin3'];
                        var closeIdList = [];
                        for (var i = 0; i < tabpanel.tabs.length; i++) {
                            if (nLi.indexOf(tabpanel.tabs[i].id) < 0) closeIdList.push(tabpanel.tabs[i].id);
                        }
                        if (closeIdList.length>0) tabpanel.killList(closeIdList, item.chk_id);
                    }
                }]
            });

            window.addTab = function (title, url) {
                //console.log(tabpanel);
                //console.log(tabpanel.show);
            CheckLogin();
            var pos = -1;
            for (var i = 0; i < tabpanel.tabs.length; i++) {
                if (tabpanel.tabs[i].title.text() == title) {
                    pos = i;
                    break;
                }
            }
            var tabguid = CreateGUID();
            var ic_h = $(window).height() - $('.header').height();
            var tc_h = ic_h - $('.tabpanel_tab_content').height();
            var html = '<iframe src="' + url + '" width="100%" height="' + tc_h + '" frameborder="0" class="tabMainContent" id="tab' + tabguid + 'Frame"></iframe>';

                if (pos > -1) {

                    tabpanel.show(pos);
                    //tabpanel.refresh(pos);
                    tabpanel.setContent(pos, html);

                } else {
                    //var tabguid = CreateGUID();

                    tabpanel.addTab({
                        id: 'tab' + tabguid,
                        title: title,
                        html: html,
                        closable: true
                    });
                }
            }

            setTimeout(function () {
                SetSize();
            }, 200);
        });


        $(window).resize(function () {
            setTimeout(function () {
                SetSize();
            }, 3000);
        });
        function SetSize() {
            var $tabpanelContent = $('.tabpanel_content,.tabpanel_tab_content,.tabpanel');
            $tabpanelContent.width($(window).width() - 175);
            var tc_h = $(window).height() - $('.tabpanel_content').offset().top;
            $('.tabMainContent').height(tc_h);
        }

        function CheckLogin() {
            $.ajax({
                type: "GET",
                url: "/serv/api/admin/user/islogin.ashx",
                dataType: "json",
                success: function (data) {
                    if (data.is_login == false) {
                        window.location.href = "/login";
                    }
                }
            });
        }

        function SetIframeHeight(input) {
            $('#iframeMain').attr('height', input);
        }

        function BuySaas() {
            //自定页
            layer.alert('请拨打电话 021-61729583  联系客服续费')
        }

        function V5Notice() {
            layer.alert('平台用户每推荐一个新客户，平台有效使用期限直接增加2年。<br><br>具体细节请联系客服。')
        }
        function menuHandler(item) {
            console.log(item);
        }
       
    </script>
</html>
