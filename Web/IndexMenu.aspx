<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexMenu.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Index" %>

<!DOCTYPE html>
<html>
<head>
    <title>至云移动营销管理平台</title>
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
    <link href="/css/master/indexv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
</head>
<body>
    <%ZentCloud.BLLJIMP.Model.WebsiteInfo currWebSiteInfo = ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel();
      if (string.IsNullOrEmpty(currWebSiteInfo.WebsiteLogo))
      {
          currWebSiteInfo.WebsiteLogo = "/FileUpload/JuActivityImg/d5e7e7d4-5985-404a-87ff-a5bebe66b525.png";
      }   
     %>
    <style>
        .logov1
        {
            width: 180px;
            height: 60px;
            float: left;
            background-image: url("<%=currWebSiteInfo.WebsiteLogo%>");
            background-repeat: no-repeat;
            background-position: center;
            background-size: 100%;
        }
    </style>
    <div class="header">
        <div class="logov1">
        </div>
        <div class="logout">
            <%=ZentCloud.JubitIMP.Web.DataLoadTool.GetWebsiteInfoModel().WebsiteName%>&nbsp;欢迎您,&nbsp;<%=currUser.UserID %>
            &nbsp; <a href="javascript:void(0);" id="spchangpassword">[修改密码]</a>&nbsp;<a href="<%=ZentCloud.Common.ConfigHelper.GetConfigString("logoutUrl") + "?op=logout"%>">[安全退出]</a></div>
    </div>
    <div class="main">
        <div class="nav">
            <div class="tagbox currenttag tagopen">
                <h2>
                    文章管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/Cation/ArticleCompile.aspx?Action=add"><a href="javascript:;">新建文章</a></li>
                    <li data-rel="/App/Cation/ArticleManage.aspx"><a class="current" href="javascript:;">所有文章</a></li>
                    <li data-rel="/App/Cation/ArticleCategoryManage.aspx?type=article"><a href="javascript:;">分类目录</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    专家管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Admin/Tutor/TutorApplyList.aspx"><a href="javascript:;">申请列表</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    活动管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/Cation/ActivityCompile.aspx?Action=add"><a href="javascript:;">新建活动</a></li>
                    <li data-rel="/App/Cation/ActivityManage.aspx"><a href="javascript:;">所有活动</a></li>
                    <li data-rel="/App/Cation/ArticleCategoryManage.aspx?type=activity"><a href="javascript:;">分类目录</a></li>
                    <li data-rel="/App/Cation/ActivityConfig.aspx"><a href="javascript:;">活动配置</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    微网站<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/CompanyWebsite/CompanyWebsiteTemplateSet.aspx"><a href="javascript:;">网站模板</a></li>
                    <li data-rel="/App/CompanyWebsite/WebsiteConfig.aspx"><a href="javascript:;">全局设置</a></li>
                    <li data-rel="/App/CompanyWebsite/ProjectorManage.aspx"><a href="javascript:;">幻灯片管理</a></li>
                    <li data-rel="/App/CompanyWebsite/ToolBarManage.aspx"><a href="javascript:;">底部工具栏管理</a></li>
                    <li data-rel="/App/CompanyWebsite/NavigateManage.aspx"><a href="javascript:;">模块管理</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    微客服<span class="icon"></span></h2>
                <ul>
                    
                    <li data-rel="/App/PubMgr/MsgDetails.aspx"><a href="javascript:;">查看公众号消息</a></li>
                    <li data-rel="/App/PubMgr/KeFuConfig.aspx"><a href="javascript:;">公众号消息转发</a></li>
                    <li data-rel="/App/PubMgr/WXKeFuManage.aspx"><a href="javascript:;">微信客服设置</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    客户管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/Cation/UserManage.aspx"><a href="javascript:;">会员管理</a></li>
                    <li data-rel="/App/Cation/MemberTagManage.aspx?tagtype=Member"><a href="javascript:;">会员标签</a></li>
                    <li data-rel="/App/PubMgr/WeixinFollowersInfoMgr.aspx"><a href="javascript:;">粉丝管理</a></li>
                    <li data-rel="/App/MallManage/WXMallScoreProductMgr.aspx"><a href="javascript:;">积分商品</a></li>
                    <li data-rel="/App/MallManage/WXMallScoreOrderMgr.aspx"><a href="javascript:;">积分订单</a></li>
                    <li data-rel="/App/MallManage/ScoreConfig.aspx"><a href="javascript:;">积分配置</a></li>
                    <li data-rel="/App/MallManage/WXMallScoreTypeInfo.aspx"><a href="javascript:;">积分商店</a></li>
                    <li data-rel="/App/MallManage/WXMallScoreTypeInfo.aspx"><a href="javascript:;">积分分类</a></li>
                    <li data-rel="/User/UserLevelConfig.aspx"><a href="javascript:;">用户等级管理</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    数据管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/Cation/MemberTagManage.aspx"><a href="javascript:;">标签管理</a></li>
                    <li data-rel="/App/Data/ActivityManage.aspx"><a href="javascript:;">数据管理</a></li>
                    <li data-rel="/monitors/monitorplanmanage.aspx"><a href="javascript:;">监测管理</a></li>
                    <li data-rel="/user/extconfig.aspx"><a href="javascript:;">外部连接配置</a></li>
                    <%if (currUser.UserType.Equals(1))
                      {%>
                    <li data-rel="/game/GameActivityQueryLimitManage.aspx"><a href="javascript:;">允许查询的活动</a></li>
                    <%} %>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    微游戏<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Game/GameList.aspx"><a href="javascript:;">游戏列表</a></li>
                    <li data-rel="/Game/PlanList.aspx"><a href="javascript:;">游戏广告</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    微商城<span class="icon"></span>
                </h2>
                <ul>
                    <li data-rel="/App/MallManage/WXMallOrderStatusMgr.aspx"><a href="javascript:;">订单状态管理</a></li>
                    <li data-rel="/App/MallManage/WXMallCategoryMgr.aspx"><a href="javascript:;">商品分类管理</a></li>
                    <li data-rel="/App/MallManage/WXMallDeliveryStaffMgr.aspx"><a href="javascript:;">配送员管理</a></li>
                    <li data-rel="/App/MallManage/WXMallProductMgr.aspx"><a href="javascript:;">商品管理</a></li>
                    <li data-rel="/App/MallManage/WXMallOrderMgr.aspx"><a href="javascript:;">订单管理</a></li>
                    <li data-rel="/App/MallManage/WXMallConfig.aspx"><a href="javascript:;">商城配置</a></li>
                    <li data-rel="/App/MallManage/WXMallDeliveryMgr.aspx"><a href="javascript:;">配送方式管理</a></li>
                    <li data-rel="/App/MallManage/WXMallPayMentTypeMgr.aspx"><a href="javascript:;">支付方式管理</a></li>
                    <%if (currUser.WebsiteOwner.Equals("qianwei") || currUser.UserType.Equals(1))
                      {%>
                    <li data-rel="/App/MallManage/QianWeiStockStatistics.aspx"><a href="javascript:;">库存管理</a></li>
                    <%} %>
                    <%if (new ZentCloud.BLLJIMP.BLL().GetWebsiteInfoModel().IsDistributionMall == 1)
                      {%>
                    <li data-rel="/App/Distribution/DistributionTree.aspx"><a href="javascript:;">分销树</a></li>
                    <li data-rel="/App/Distribution/WithdrawCashMgr.aspx"><a href="javascript:;">分销提现</a></li>
                    <li data-rel="/App/MallManage/CouponMgr.aspx"><a href="javascript:;">优惠券</a></li>
                    <li data-rel="/App/MallManage/SlideMgr.aspx"><a href="javascript:;">Banner管理</a></li>

                    <% } %>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    微营销<span class="icon"></span>
                </h2>
                <ul>
                   <%-- <li><a href="javascript:;">微贺卡</a></li>--%>
                    <li data-rel="/App/Lottery/WXLotteryMgrV1.aspx"><a href="javascript:;">微刮奖</a></li>
                    <%--<li><a href="javascript:;">微信墙</a></li>--%>
                    <li data-rel="/App/Forward/WXForwardMange.aspx"><a href="javascript:;">微转发</a></li>
                    <li data-rel="/App/WXShow/WXShowInfoMgr.aspx"><a href="javascript:;">微秀</a></li>
                    <li data-rel="/Admin/ShareMonitor/ShareMonitorManage/ShareMonitorManage.aspx"><a href="javascript:;">分享监测</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    公众号设置<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/PubMgr/ReplyTextRuleManage.aspx"><a href="javascript:;">文本自动回复</a></li>
                    <li data-rel="/App/PubMgr/ReplyNewsRuleManage.aspx"><a href="javascript:;">图文自动回复</a></li>
                    <li data-rel="/App/PubMgr/WXMassArticleMgr.aspx"><a href="javascript:;">群发图文素材</a></li>
                    <li data-rel="/App/PubMgr/NewsImgManage.aspx"><a href="javascript:;">自动回复图文素材</a></li>
                    <li data-rel="/App/Cation/PubMenuManage.aspx"><a href="javascript:;">自定义菜单设置</a></li>
                    <li data-rel="/App/Cation/PubConfig.aspx"><a href="javascript:;">公众号接口配置</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    产品真伪查询<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/BarCode/BarCodeMgr.aspx"><a href="javascript:;">产品真伪查询</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    投票V2<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/TheVote/TheVoteInfoMgr.aspx"><a href="javascript:;">投票V2</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    短信平台<span class="icon"></span></h2>
                <ul>
                    <li><a target="_blank" href="http://www.jubit.org/index.aspx">短信平台</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    投票<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/Vote/VoteInfoMgr.aspx"><a href="javascript:;">所有投票</a></li>
                </ul>
            </div>
            <%if (currWebSiteInfo.WebsiteOwner.Equals(currUser.UserID) || (currUser.UserType.Equals(1)))
              {%>
            <div class="tagbox">
                <h2>
                    系统管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/Sys/SubAccountsMgr.aspx"><a href="javascript:;">子账户管理</a></li>
                    <li data-rel="/App/Cation/WebSiteConfigPersonal.aspx"><a href="javascript:;">系统设置</a></li>
                    <li data-rel="/App/Cation/PayConfig.aspx"><a href="#">支付配置</a></li>
                </ul>
            </div>
            <%} %>
            <div class="tagbox">
                <h2>
                    问卷调查<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/Questionnaire/QuestionnaireMgr.aspx"><a href="javascript:;">问卷调查</a></li>
                </ul>
            </div>
            <%if (currUser.WebsiteOwner.Equals("10000care") || currUser.UserType.Equals(1))
              {%>
            <div class="tagbox">
                <h2>
                    万邦关爱<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/WanBang/ProjectMgr.aspx"><a href="javascript:;">项目管理</a></li>
                    <li data-rel="/App/WanBang/BaseMgr.aspx"><a href="javascript:;">基地管理</a></li>
                    <li data-rel="/App/WanBang/CompanyMgr.aspx"><a href="javascript:;">企业管理</a></li>
                    <li data-rel="/App/WanBang/JointProjectMgr.aspx"><a href="javascript:;">对接项目管理</a></li>
                </ul>
            </div>
            <%} %>
            <%if (currUser.WebsiteOwner.Equals("forbes") || currUser.UserType.Equals(1))
              {%>
            <div class="tagbox">
                <h2>
                    福布斯<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Customize/Forbes/SlideMgr.aspx"><a href="javascript:;">首页-幻灯片管理</a></li>
                    <li data-rel="/Customize/Forbes/MasterMgr.aspx"><a href="javascript:;">理财师管理</a></li>
                    <li data-rel="/Customize/Forbes/TagsMgr.aspx"><a href="javascript:;">标签管理</a></li>
                    <li data-rel="/App/Sys/SystemNoticeManage.aspx"><a href="#" class="current">消息</a></li>
                    <li data-rel="/WuBuHui/WebAdmin/WXWordsQuestionsMgr.aspx"><a href="#" class="current">问答</a></li>
                </ul>
            </div>
               <div class="tagbox">
                <h2>
                    TOTEMA<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Customize/totema/ClassMgr.aspx"><a href="#">班级</a></li>
                </ul>
            </div>
           <div class="tagbox">
                <h2>
                    五步会<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/WuBuHui/WebAdmin/FilterWordMgr.aspx"><a href="#">关键字过滤</a></li>
                    <li data-rel="/WuBuHui/WebAdmin/ArticleReviewMgr.aspx"><a href="#">文章评论</a></li>

                </ul>
            </div>
             <div class="tagbox">
                <h2>
                    众筹<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/CrowdFund/Admin/CrowdFundInfoMgr.aspx"><a href="#">众筹</a></li>

                </ul>
            </div>
            <%} %>
            <% if (currUser.UserType == 1)
               { %>
            <div class="tagbox">
                <h2>
                    超级管理员<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/Sys/UserManage.aspx"><a href="javascript:;">用户管理</a></li>
                    <li data-rel="/App/Cation/WebsiteManage.aspx"><a href="javascript:;">站点管理</a></li>
                    <%-- <li data-rel="/Game/GameMgr.aspx"><a href="javascript:;">游戏管理</a></li>
                    <li data-rel="/App/Sys/IndustryTemplateManage.aspx"><a href="javascript:;">模板管理</a></li>--%>
                    
                    <li data-rel="/home/MenuManagerV2.aspx"><a href="javascript:;">菜单管理</a></li>
                    <li data-rel="/home/permissionmanagerv2.aspx"><a href="javascript:;">权限管理</a></li>
                    <li data-rel="/permission/modulefilterinfomanage.aspx"><a href="javascript:;">权限排除页面</a></li>
                    <li data-rel="/Home/WeixinWebOAuthManager.aspx"><a href="javascript:;">微信网页授权</a></li>
                    <li data-rel="/Home/PmsGroupManagerV2.aspx"><a href="javascript:;">权限组管理</a></li>
                    <li data-rel="/App/Sys/CompanyWebsiteTemplateManage.aspx"><a href="javascript:;">微网站模板管理</a></li>
                    <li data-rel="/App/Review/ReviewInfoMgr.aspx"><a href="javascript:;">评论</a></li>
                </ul>
            </div>
            <%} %>
        </div>
        <div class="concent">
            <iframe id="iframeMain" name="right" frameborder="0" scrolling="yes" src="/App/Cation/ArticleManage.aspx"
                style="width: 100%; height: 100%; padding-bottom: 60px; overflow-y: auto;"></iframe>
        </div>
    </div>
</body>
<script type="text/javascript" src="/scripts/jquery.mousewheel.min.js"></script>
<script type="text/javascript" src="/scripts/scrollbar.js"></script>
<script type="text/javascript">

    $(function () {

        var navscorll = new scrollbar(".nav");
        navscorll.init($(window).height() - 60)

        $(window).resize(function () {
            navscorll.init($(window).height() - 60)
        })

        $("li").bind("click", function () {
            var path = $(this).attr("data-rel");
            if (path) {
                $('#iframeMain').attr('src', path);

            }
        });

        $("#spchangpassword").click(function () {

            $('#iframeMain').attr('src', '/App/Cation/SetPwd.aspx');


        });

        //                $(".tagbox>h2").click(function () {
        //                    var _this = $(this).parent(".tagbox");
        //                    _this.find("ul").slideToggle("slow", function () {
        //                        if (_this.hasClass("tagopen")) {
        //                            _this.removeClass("tagopen");
        //                        } else {
        //                            _this.addClass("tagopen");
        //                        }
        //                    });
        //                })

        $(".tagbox>h2").click(function () {
            var _this = $(this).parent(".tagbox");

            $(".tagopen").removeClass("tagopen");
            _this.addClass("tagopen");
            $(".tagbox").each(function (index) {
                if ($(this).hasClass("tagopen")) {
                    $(this).find("ul").slideDown("slow", function () { navscorll.init(); }
)
                } else {
                    $(this).find("ul").slideUp("slow", function () { navscorll.init(); });


                }
            })
        })





        //        $(".tagbox>h2").click(function () {
        //            $("ul").slideUp();
        //            var _this = $(this).parent(".tagbox");
        //            _this.find("ul").slideToggle("slow", function () {

        //                if (_this.hasClass("tagopen")) {
        //                    _this.removeClass("tagopen");
        //                } else {
        //                    _this.addClass("tagopen");
        //                }

        //            });

        //            
        //            $(".tagbox").find(".current").removeClass("current");
        //            $(".tagbox").removeClass("currenttag");
        //            $(this).parents().first().addClass("currenttag")



        //        })


        //        $(".tagbox").find("a").click(function () {
        //            $(".tagbox").find(".current").removeClass("current");
        //            $(this).addClass("current");
        //            $(".tagbox").removeClass("currenttag");
        //            $(this).parents(".tagbox").addClass("currenttag")
        //        })

        $(".tagbox").find("li").click(function () {
            $(".tagbox").find(".current").removeClass("current");
            $(this).children("a").addClass("current");
            $(".tagbox").removeClass("currenttag");
            $(this).parents(".tagbox").addClass("currenttag")
        })




        //$(".tagbox:eq(-1)").css({ "padding-bottom": "60px" })


    });
    function SetIframeHeight(input) {
        $('#iframeMain').attr('height', input);
    }
</script>
</html>
