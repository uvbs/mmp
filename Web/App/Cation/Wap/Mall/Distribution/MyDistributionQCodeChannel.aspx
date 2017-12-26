<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="MyDistributionQCodeChannel.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution.MyDistributionQCodeChannel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <%=pageTitle %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        html, body, .qrcodeBox {
            height: 100%;
        }

        .qrcodeBox {
            text-align: center;
            background-size: cover;
            background-image: url(<%=website.DistributionShareQrcodeBgImg%>);
        }

        .wrapQrcode {
            position: absolute;
            bottom: 16%;
            width: 100%;
        }

            .wrapQrcode h2 {
                margin-top: 16px;
                font-size: 16px;
                font-weight: bolder;
            }

        .codepic {
            width: 40%;
        }

        .wrapUserInfo {
            padding-top: 48px;
        }

            .wrapUserInfo img {
                width: 68px;
                height: 68px;
                border-radius: 50px;
            }

            .wrapUserInfo .userName {
                font-size: 16px;
                color: #640101;
                font-weight: bolder;
            }

        .pTop8 {
            padding-top: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="qrcodeBox">


        <div class="wrapUserInfo">
            <div>
                <img src="<%=bllUser.GetUserDispalyAvatar(channelUser) %>" />
            </div>
            <div class="pTop8">
                <span class="userName">我是&nbsp;&nbsp;<%=bllUser.GetUserDispalyName(channelUser) %> </span>

            </div>
        </div>


        <div class="wrapQrcode">


            <img id="imgcarcode" src="<%=qrcondeUrl %>" class="codepic">
            <h2 class="text">长按此图，识别图中二维码</h2>

        </div>

    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">

        var shareCallBackFunc = {
            timeline_s: function () {

            },
            timeline_c: function () {
                //朋友圈分享取消
            },
            message_s: function () {
                //分享给朋友

            },
            message_c: function () {
                //朋友分享取消
            }
        }

        wx.ready(function () {
            wxapi.wxshare({
                title: '<%=shareTitle%>',
                desc: '<%=website.ShopDescription%>',
                link: 'http://<%=Request.Url.Host%>/App/Cation/Wap/Mall/Distribution/MyDistributionQCodeChannel.aspx?sid=<%=Request["sid"]%>',
                imgUrl: '<%=website.WXMallBannerImage%>'
            }, shareCallBackFunc)
        });

    </script>


</asp:Content>
