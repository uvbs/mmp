<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexV3.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.IndexV3" %>


<!DOCTYPE html>
<html>
<head>
    <title>至云移动营销管理平台</title>
    <meta name="renderer" content="webkit">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1">
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
    <% ZentCloud.BLLJIMP.Model.UserInfo currUser = ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel(); %>
    <style>
        .logov1 {
            width: 180px;
            height: 48px;
            float: left;
            background-image: url("<%= WebsiteLogo %>");
            background-repeat: no-repeat;
            background-position: center;
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
            <span class=""><%=WebsiteName%>&nbsp;(<%=WebsiteVersion%>)</span>&nbsp;&nbsp;&nbsp;&nbsp;
            <span class="">当前用户：<%=curUserID %></span>&nbsp;&nbsp;&nbsp;&nbsp;
            
            <%if (!string.IsNullOrWhiteSpace(ExpirationDate))
              { %>
            <span class="">有效期至<%=ExpirationDate %><span id="spIsOverExpirationDate" style="color: red; cursor: pointer;" onclick="BuySaas()"></span></span>&nbsp;&nbsp;&nbsp;&nbsp;
            <%} %>
            <a href="javascript:BuySaas();">[续费]</a>&nbsp;
            <a href="javascript:void(0);" id="spchangpassword">[修改密码]</a>&nbsp;
            <a href="<%=LogoutHref %>">[安全退出]</a>&nbsp;
            <a target="_blank" href="http://comeoncloud.comeoncloud.net/c9b4f/details.chtml">[帮助中心]</a>
        </div>
    </div>
    <div class="main">
        <div class="nav wrap-nav">

            <nav class="navbar-default navbar-static-side" role="navigation">

                <div class="sidebar-collapse">

                    <ul class="nav metismenu" id="side-menu">
                        <li class="">
                            <a href="javascript:;"><i class="fa fa-weixin"></i><span class="nav-label">客服作业</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel="/Admin/Member/RegisterOffLine.aspx"><a href="javascript:;">注册实单会员</a></li>
                                <li data-rel="/Admin/Flow/List.aspx?flow_key=RegisterOffLine&module_name=注册审核情况&hide_status=0,10"><a href="javascript:;">注册审核情况</a></li>
                                <li data-rel=""><a href="javascript:;">会员撤单</a></li>
                                <li data-rel=""><a href="javascript:;">初始化密码</a></li>
                            </ul>
                        </li>
                        <li class="">
                            <a href="javascript:;"><i class="fa fa-weixin"></i><span class="nav-label">财务工作</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel="/Admin/Flow/List.aspx?flow_key=RegisterOffLine&module_name=线下注册审核"><a href="javascript:;">线下注册审核</a></li>
                                <li data-rel="/Admin/Flow/List.aspx?flow_key=OfflineRecharge&module_name=线下充值审核"><a href="javascript:;">线下充值审核</a></li>
                                <li data-rel="/Admin/Flow/List.aspx?flow_key=Withdraw&module_name=提现审核"><a href="javascript:;">提现审核</a></li>
                                <li data-rel="/Admin/Flow/List.aspx?flow_key=CancelRegister&module_name=撤单审核"><a href="javascript:;">撤单审核</a></li>
                                <li data-rel=""><a href="javascript:;">财务明细</a></li>
                            </ul>
                        </li>
                        <li class="">
                            <a href="javascript:;"><i class="fa fa-weixin"></i><span class="nav-label">管理员操作</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel=""><a href="javascript:;">添加空单会员</a></li>
                                <li data-rel=""><a href="javascript:;">更改推荐人</a></li>
                            </ul>
                        </li>
                        <li class="">
                            <a href="javascript:;"><i class="fa fa-users"></i><span class="nav-label">会员情况</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel=""><a href="javascript:;">会员管理</a></li>
                                <li data-rel=""><a href="javascript:;">会员排位图</a></li>
                                <li data-rel=""><a href="javascript:;">会员佣金明细</a></li>
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:;"><i class="fa fa-newspaper-o"></i><span class="nav-label">文章</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                            <li data-rel="/App/Cation/ArticleCompile.aspx?Action=add"><a href="javascript:;">新建文章</a></li>
                            
                            <li data-rel="/App/Cation/ArticleManage.aspx"><a href="javascript:;">所有文章</a></li>
                            
                            <li data-rel="/App/Cation/MemberTagManage.aspx?tagtype=All"><a href="javascript:;">标签管理</a></li>
                            
                            <li data-rel="/App/Cation/ArticleCategoryManage.aspx?type=article"><a href="javascript:;">文章分类</a></li>
                            
                        </ul>
                        </li>
                        <li class="">
                            <a href="javascript:;"><i class="fa fa-home"></i><span class="nav-label">移动站点</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel="/App/CompanyWebsite/CompanyWebsiteTemplateSet.aspx"><a href="javascript:;">网站模板</a></li>

                                <li data-rel="/App/CompanyWebsite/WebsiteConfig.aspx"><a href="javascript:;">全局设置</a></li>

                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/newAdList"><a href="javascript:;">幻灯片管理</a></li>

                                <li data-rel="/App/CompanyWebsite/ToolBarManage.aspx?use_type=nav"><a href="javascript:;">导航管理</a></li>

                                <li data-rel="/App/CompanyWebsite/ProjectorManage.aspx"><a href="javascript:;">旧幻灯片管理</a></li>

                                <li data-rel="/App/CompanyWebsite/NavigateManage.aspx"><a href="javascript:;">模板管理</a></li>

                                <li data-rel="/Admin/Component/EditPage.aspx?key=MallHome"><a href="javascript:;">站点首页设置</a></li>

                                <li data-rel="/Admin/Component/EditPage.aspx?key=PersonalCenter"><a href="javascript:;">个人中心设置</a></li>

                            </ul>
                        </li>
                        <li>
                            <a href="javascript:;"><i class="fa fa-shopping-cart"></i><span class="nav-label">商城</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel="/App/MallManage/WXMallStoreStatistics.aspx"><a href="javascript:;">商城统计</a></li>
                                
                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/productList////"><a href="javascript:;">商品管理</a></li>
                                
                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/orderList"><a href="javascript:;">订单管理</a></li>
                                
                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/productSort"><a href="javascript:;">商品分类</a></li>
                                
                                <li data-rel="/App/Cation/MemberTagManage.aspx?tagtype=mall"><a href="javascript:;">商品标签</a></li>
                                
                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/productProperty"><a href="javascript:;">商品规格</a></li>
                                
                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/express"><a href="javascript:;">快递设置</a></li>
                                
                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/freightTemplate"><a href="javascript:;">运费模板</a></li>
                                
                                <li data-rel="/App/MallManage/WXMallConfig.aspx"><a href="javascript:;">商城设置</a></li>
                                
                            </ul>
                        </li>
                        <li>
                            <a href="javascript:;"><i class="fa fa-home"></i><span class="nav-label">站点建设</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel="/App/CompanyWebsite/CompanyWebsiteTemplateSet.aspx"><a href="javascript:;">网站模板</a></li>
                                
                                <li data-rel="/App/CompanyWebsite/NavigateManage.aspx"><a href="javascript:;">模板管理</a></li>
                                
                                <li data-rel="/Admin/Component/EditPage.aspx?key=MallHome"><a href="javascript:;">站点首页</a></li>
                                
                                <li data-rel="/Admin/Component/EditPage.aspx?key=PersonalCenter"><a href="javascript:;">个人中心</a></li>
                                
                                <li data-rel="/Admin/Component/List.aspx"><a href="javascript:;">微页面</a></li>
                                
                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/newAdList"><a href="javascript:;">幻灯片</a></li>
                                
                                <li data-rel="/App/CompanyWebsite/ToolBarManage.aspx?use_type=nav"><a href="javascript:;">导航</a></li>
                                
                            </ul>
                        </li>
                        <li class="">
                            <a href="javascript:;"><i class="fa fa-cubes"></i><span class="nav-label">移动分销</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel="/Admin/TableFieldMap/List.aspx?table_name=ZCJ_UserLevelConfig&amp;mapping_type=0"><a href="javascript:;">分销等级字段</a></li>

                                <li data-rel="/user/userlevelconfig.aspx?type=DistributionOnLine"><a href="javascript:;">分销等级设置</a></li>

                            </ul>
                        </li>
                        <li class="">
                            <a href="javascript:;"><i class="fa fa-road"></i><span class="nav-label">营销中心</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/marketingcenter"><a href="javascript:;">营销中心</a></li>

                                <li data-rel="/App/Lottery/WXLotteryMgrV1.aspx"><a href="javascript:;">刮刮奖</a></li>

                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/shakeList"><a href="javascript:;">摇一摇_old</a></li>

                                <li data-rel="/customize/mmpadmin/index.aspx?hidemenu=1#/index/shakeList/shake"><a href="javascript:;">摇一摇</a></li>

                                <li data-rel="/Game/PlanList.aspx"><a href="javascript:;">游戏中心</a></li>

                                <li data-rel="/App/WXShow/WXShowInfoMgr.aspx"><a href="javascript:;">微秀</a></li>

                                <li data-rel="/Admin/ShareMonitor/ShareMonitorManage/ShareMonitorManage.aspx"><a href="javascript:;">微监测</a></li>

                                <li data-rel="/Admin/GreetingCard/GreetingCardsManage.aspx"><a href="javascript:;">微贺卡</a></li>

                                <li data-rel="/App/TheVote/TheVoteInfoMgr.aspx"><a href="javascript:;">选题投票</a></li>

                                <li data-rel="/App/Vote/VoteInfoMgr.aspx"><a href="javascript:;">排名投票</a></li>

                                <li data-rel="/App/Questionnaire/QuestionnaireMgr.aspx?type=1"><a href="javascript:;">问卷</a></li>

                                <li data-rel="/App/Questionnaire/QuestionnaireMgr.aspx?type=0"><a href="javascript:;">题库</a></li>

                                <li data-rel="/App/Questionnaire/QuestionnaireSetMgr.aspx"><a href="javascript:;">答题</a></li>

                                <li><a target="_blank" href="http://www.jubit.org/index.aspx">手机短信</a></li>

                                <li data-rel="/admin/review/reviewlist.aspx"><a href="javascript:;">话题</a></li>

                                <li data-rel="/admin/review/ReviewConfig.aspx"><a href="javascript:;">话题配置</a></li>

                                <li data-rel="/customize/mmpadmin/?hidemenu=1#/index/newsaleList/"><a href="javascript:;">限时特卖</a></li>

                            </ul>
                        </li>
                        <li class="">
                            <a href="javascript:;"><i class="fa fa-weixin"></i><span class="nav-label">微信公众号</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel="/App/Cation/PubConfig.aspx"><a href="javascript:;">公众号接口配置</a></li>

                                <li data-rel="/App/PubMgr/KeFuConfig.aspx"><a href="javascript:;">公众号消息转发</a></li>

                                <li data-rel="/App/PubMgr/WXMassArticleMgr.aspx"><a href="javascript:;">群发图文素材</a></li>

                                <li data-rel="/App/PubMgr/ReplyNewsRuleManage.aspx"><a href="javascript:;">自动回复</a></li>

                                <li data-rel="/App/PubMgr/NewsImgManage.aspx"><a href="javascript:;">自动回复图文素材</a></li>

                                <li data-rel="/App/Cation/PubMenuManage.aspx"><a href="javascript:;">自定义菜单设置</a></li>

                                <li data-rel="/App/PubMgr/MsgDetails.aspx"><a href="javascript:;">查看公众号消息</a></li>

                                <li data-rel="/App/PubMgr/WXKeFuManage.aspx"><a href="javascript:;">微信客服设置</a></li>

                                <li data-rel="/Admin/WXTempmsg/WXTempmsgmgr.aspx"><a href="javascript:;">微信消息模板</a></li>

                            </ul>
                        </li>
                        <li class="">
                            <a href="javascript:;"><i class="fa fa-th-large"></i><span class="nav-label">权限管理</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel="/Admin/PermissionGroup/RoleList.aspx"><a href="javascript:;">角色管理</a></li>

                                <li data-rel="/Admin/Account/List.aspx"><a href="javascript:;">账户管理</a></li>

                            </ul>
                        </li>
                        <%if (currUser.UserType.Equals(1))
                          {%>
                        <li class="">
                            <a href="javascript:;"><i class="fa fa-th-large"></i><span class="nav-label">超级管理员</span><span class="fa arrow"></span></a>
                            <ul class="nav nav-second-level">
                                <li data-rel="/App/Sys/UserManage.aspx"><a href="javascript:;">用户管理</a></li>

                                <li data-rel="/App/Cation/WebsiteManage.aspx"><a href="javascript:;">站点管理</a></li>

                                <li data-rel="/home/MenuManagerV2.aspx"><a href="javascript:;">菜单管理</a></li>

                                <li data-rel="/home/permissionmanagerv2.aspx"><a href="javascript:;">权限管理</a></li>

                                <li data-rel="/App/Cation/ArticleCategoryManage.aspx?type=Permission&amp;websiteowner=Common"><a href="javascript:;">权限分类管理</a></li>

                                <li data-rel="/Admin/PermissionColumn/List.aspx"><a href="javascript:;">权限栏目管理</a></li>

                                <li data-rel="/Home/PmsGroupManagerV2.aspx"><a href="javascript:;">权限组管理</a></li>

                            </ul>
                        </li>
                        <%} %>
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
<%--<script src="/MainStyleV2/js/plugins/metisMenu/2.5.2/metisMenu.js"></script>--%>
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
    } else {
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
        var tabToolbarPlugin3Url = '/customize/mmpadmin/index.aspx?hidemenu=1#/index/hometablepage';
            <% if (websiteOwner == "stockplayer")
               { %>
        tabToolbarPlugin3Url = '/Admin/Statistic/ScoreStatistic.aspx?moduleName=淘股币';
            <%} %>
        var tabpanel_items = [
            {
                id: 'tabToolbarPlugin3',
                title: '工作台',
                closable: false,
                html: '<iframe src="' + tabToolbarPlugin3Url + '" width="100%" height="' + ($(window).height() - $('.header').height()) + '" frameborder="0" class="tabMainContent" id="tabToolbarPlugin3Frame"></iframe>'
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
        }, 5000);

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
            }, {
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
            }, {
                name: '关闭其他',
                onClick: function (item) {
                    var nLi = [item.chk_id, 'tabToolbarPlugin3'];
                    var closeIdList = [];
                    for (var i = 0; i < tabpanel.tabs.length; i++) {
                        if (nLi.indexOf(tabpanel.tabs[i].id) < 0) closeIdList.push(tabpanel.tabs[i].id);
                    }
                    if (closeIdList.length > 0) tabpanel.killList(closeIdList, item.chk_id);
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
