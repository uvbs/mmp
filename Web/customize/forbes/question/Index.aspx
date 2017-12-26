<%@ Page Title="" Language="C#" MasterPageFile="~/customize/forbes/question/Master.Master"
    AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.forbes.question.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    首页
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="css/index.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            background-image: url(images/bg1_02.png);
        }
        .attion{text-align:left;
                margin-left:5px;
                margin-right:5px;
                
                }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapForbesIndex">
        <div class="medal">
            <img class="medalImg" src="images/medal.png" />
        </div>
        <div class="appraisal">
            <img class="appraisalImg" src="images/appraisal.png" />
        </div>
        <div class="beginAnsBtn">
            <button id="beginAnswer" class="button button-block button-positive" onclick="window.location.href='Question.aspx?count=1'">
                开始答题
            </button>
        </div>
        <div class="bottomMedal">
            <img class="botMedalImg" src="images/bottomMedal.png" />
        </div>
        <div class="bottomLogo">
            <img class="botLogoImg" src="images/bottomLogo.png" />
            <div class="attion">
               <h1> 注意事项：</h1>
                一旦开始答题后每人有两次答题机会，两次答题必须按照提示连续进行，退出答题就不能再进入答题。
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {

            wxapi.wxshare({
                title: '2015理财师评选模拟答题(创富荟)',
                desc: '福布斯',
                link: 'http://<%=Request.Url.Host%>/customize/forbes/question/Index.aspx',
                imgUrl: 'http://forbes.comeoncloud.net/customize/forbes/images/login_logo.png'
            }

        )
        })
    </script>
</asp:Content>
