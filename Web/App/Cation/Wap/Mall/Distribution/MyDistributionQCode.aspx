<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Mall.Master" AutoEventWireup="true" CodeBehind="MyDistributionQCode.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution.MyDistributionQCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <%=pageTitle %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        html, body, .qrcodeBox {
            /*<%
            if(string.IsNullOrEmpty(config.QRCodeUseGuide)){

                                                           }
            %>*/
            height: 100%;
        }

        .qrcodeBox {
            /*text-align: center;
            background-size: cover;
            background-image: url(<%=website.DistributionShareQrcodeBgImg%>@553w_986h_2e);
            position: relative;*/
        }

        .wrapQrcode {
            position: absolute;
            bottom: 10%;
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


       

        .pTop8 {
            padding-top: 8px;
        }

        .userName {
            font-size: 16px;
            color: #640101;
            font-weight: bolder;
        }

        .warp-userGuide {
            position: fixed;
            width: 100%;
            bottom: 0px;
            background-color: #fff;
            height: 45px;
            padding-bottom: 5px;
        }

         .guide-title {
            padding: 10px 8px 10px 16px;
            height: 30px;
            line-height: 30px;
        }

        .guide-title .title {
            font-size: 16px;
            font-weight: bold;
            float: left;
        }

        .guide-title .desc {
            float: right;
            cursor: pointer;
            color: #bfbfbf;
            font-size: 16px;
            position: relative;
            padding-right: 32px;
        }

            .guide-title .desc img {
                position: absolute;
                top: -1px;
                right: 0px;
            }

        .content {
            padding: 10px;
            max-width: 100%;
            border-top: 1px solid #ccc;
            line-height: 20px;
            overflow-y: auto;
            height: 349px;
        }



            .content img {
                max-width: 100%;
            }

        .warp-useGuide {
            background: #fff;
        }

        .warp-userGuide1 {
            position: fixed;
            bottom: 0;
            background: #fff;
            display: none;
            max-height: 400px;
            width: 100%;
            height: 400px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="qrcodeBox">
        <img width="100%" height="100%" src="<%=qrcondeUrl %>" />
    


    <%
        if (!string.IsNullOrEmpty(config.QRCodeUseGuide))
        {
    %>
    <div class="warp-userGuide">
        <div class="guide-title">
            <div class="title">二维码使用指南</div>
            <div class="desc">
                了解详情
                            <img class="img-jian" src="images/top.png" />
            </div>

            <%--     <span class="title">二维码使用指南</span>

            <span class="desc">了解详情</span>

            <span><img class="img-jian"  src="images/top.png" /></span>--%>
        </div>

    </div>
    <%
        }     
    %>


    <div class="warp-userGuide1">
        <div class="guide-title">
            <div class="title">二维码使用指南</div>
            <div class="desc">
                了解详情
                <img class="img-jian" src="images/top.png" />
            </div>

        </div>
        <div class="clear"></div>
        <div class="content">
            <%=config.QRCodeUseGuide %>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">

    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>

    <script type="text/javascript">



        $(function () {
            $('.desc').click(function () {

                var isShow = $('.warp-userGuide1').css('display');
                if (isShow == 'none') {
                    $('.warp-userGuide1').show();

                    $('.warp-userGuide').hide();
                    $('.img-jian').attr('src', 'images/buttom.png');
                } else {

                    $('.warp-userGuide1').hide();
                    $('.warp-userGuide').show();
                    $('.img-jian').attr('src', 'images/top.png');
                }

            });
        });

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
                link: 'http://<%=Request.Url.Host%>/App/Cation/Wap/Mall/Distribution/MyDistributionQCode.aspx?sid=<%=sid%>',
                imgUrl: '<%=website.WXMallBannerImage%>'
            }, shareCallBackFunc)
        });



        function notMember() {
            //alert('你还不是代言人，必须有一个推荐人邀请');
            //setTimeout(function () {
            //    history.go(-1);
            //},2000);
            $('.wrapUserInfo').hide();
            layer.open({
                content: '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;您还不是代言人&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;',
                btn: ['确认'],
                style: '',
                shadeClose: false,
                yes: function () {
                    history.go(-1);
                }
            });

        }


        <%
        if (currUser != null)
        {
        if (string.IsNullOrWhiteSpace(currUser.DistributionOwner) && !isShare)
        { %>
        notMember();
        <%} }%>
    </script>

    <%--<script type="text/javascript">

    var handlerUrl = "/Handler/QCode.ashx";
    var code = "<%=QCode.ToString()%>";
    $(function () {

        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: { code: code },
            success: function (result) {
                $("#imgcarcode").attr("src", result);
            }
        });


    });

</script>--%>
</asp:Content>
