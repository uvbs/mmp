<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexNew.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.IndexNew" %>

<!DOCTYPE html>
<html>
<head>
    <title>至云移动营销管理平台</title>
    <link rel="shortcut icon" href="/favicon.ico" type="image/x-icon" />
    <link href="/css/master/indexv1.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
</head>
<body>
    <% ZentCloud.BLLJIMP.Model.UserInfo currUser = ZentCloud.JubitIMP.Web.DataLoadTool.GetCurrUserModel(); %>
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
                <h2>法规管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Admin/Regulations/RegulationsEdit.aspx?id=0"><a href="javascript:;">录入法规</a></li>
                    <li data-rel="/Admin/Regulations/RegulationsList.aspx"><a class="current" href="javascript:;">所有法规</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>资讯管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Admin/News/NewsEdit.aspx?id=0"><a href="javascript:;">添加资讯</a></li>
                    <li data-rel="/Admin/News/NewsList.aspx"><a href="javascript:;">所有资讯</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>案例管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Admin/Case/CaseEdit.aspx?id=0"><a href="javascript:;">添加案例</a></li>
                    <li data-rel="/Admin/Case/CaseList.aspx"><a href="javascript:;">所有案例</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>问答管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Admin/Ask/AskEdit.aspx?id=0"><a href="javascript:;">添加问题</a></li>
                    <li data-rel="/Admin/Ask/AskList.aspx"><a href="javascript:;">所有问题</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    专家库管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Admin/Tutor/TutorApplyList.aspx"><a href="javascript:;">申请列表</a></li>
                    <li data-rel="/Admin/Tutor/TutorList.aspx"><a href="javascript:;">专家列表</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>公开课管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Admin/Open/OpenEdit.aspx?id=0"><a href="javascript:;">添加课程</a></li>
                    <li data-rel="/Admin/Open/OpenList.aspx"><a href="javascript:;">所有课程</a></li>
                    <li data-rel="/Admin/Open/TypeList.aspx?cateId=86"><a href="javascript:;">分类管理</a></li>
                    <li data-rel="/Admin/Open/OpenConfig.aspx"><a href="javascript:;">基本设置</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>社区管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Admin/Statuses/StatusesEdit.aspx?id=0"><a href="javascript:;">添加社区</a></li>
                    <li data-rel="/Admin/Statuses/StatusesList.aspx"><a href="javascript:;">所有社区</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>活动管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/Cation/ActivityCompile.aspx?Action=add"><a href="javascript:;">新建活动</a></li>
                    <li data-rel="/App/Cation/ActivityManage.aspx"><a href="javascript:;">所有活动</a></li>
                    <li data-rel="/App/Cation/ArticleCategoryManage.aspx?type=activity"><a href="javascript:;">分类目录</a></li>
                    <li data-rel="/App/Cation/ActivityConfig.aspx"><a href="javascript:;">活动配置</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>
                    用户管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Admin/User/UserList.aspx"><a href="javascript:;">所有用户</a></li>
                    <li data-rel="/Admin/User/LawyerList.aspx"><a href="javascript:;">所有律师</a></li>
                    <li data-rel="/Admin/User/LawyerApplyList.aspx"><a href="javascript:;">律师申请</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>其他管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/App/Cation/MemberTagManage.aspx"><a href="javascript:;">标签管理</a></li>
                    <li data-rel="/Admin/Tag/TagManage.aspx"><a href="javascript:;">标签管理</a></li>
                    <li data-rel="/Admin/ScoreDefine/ScoreDefineList.aspx"><a href="javascript:;">积分规则管理</a></li>
                </ul>
            </div>
            <div class="tagbox">
                <h2>举报管理<span class="icon"></span></h2>
                <ul>
                    <li data-rel="/Admin/Illegal/IllegalArticleList.aspx"><a href="javascript:;">举报内容管理</a></li>
                    <li data-rel="/Admin/Illegal/IllegalReviewList.aspx"><a href="javascript:;">举报回答回复</a></li>
                </ul>
            </div>
            
        </div>
        <div class="concent">
            <iframe id="iframeMain" name="right" frameborder="0" scrolling="yes" src="/Admin/Regulations/RegulationsList.aspx"
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
