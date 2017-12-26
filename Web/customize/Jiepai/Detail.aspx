<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Jiepai/Master.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Jiepai.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    全球街拍选手详情
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
     <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0">
    <link href="Styles/detail.css" rel="stylesheet" />
    <link href="Styles/article.css" rel="stylesheet" />
   
    <meta http-equiv="expires" content="0"> 

    <meta http-equiv="pragma" content="no-cache"> 

    <meta http-equiv="cache-control" content="no-cache,must-revalidate"> 
    <style>
        #sharebox img {
            width: 100%;
        }
        .wcontainer .sliders .sliderlist {
            height:auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <header class="content">
        <header class="row">
            <div class="col">
                <img src="images/header/log.png" class="full-image">
            </div>
        </header>
        <header class="row padding-add-center">
            <div class="col col-33 col-center">
                <img src="images/header/switzerland.png" class="full-image">
            </div>
            <div class="col col-33 col-center">
                <img src="images/header/newzealand.png" class="full-image">
            </div>
            <div class="col col-33 col-center">
                <img src="images/header/victoria.png" class="full-image">
            </div>
        </header>
        <header class="row padding-add-center">
            <div class="col col-33 col-center">
                <img src="images/header/news.png" class="full-image">
            </div>
            <div class="col col-33 col-center">
                <img src="images/header/lillydale.png" class="full-image">
            </div>
            <div class="col col-33 col-center">
                <img src="images/header/qee.png" class="full-image">
            </div>
        </header>
    </header>
    <div class="content wcontainer padding-add text-center">
        <div class="sliders div-list" id="sliders">
            <div class="sliderlist">
                <img src="<%=model.ShowImage1 %>" alt="">
            </div>
            <div class="sliderlist">
                <img src="<%=model.ShowImage2 %>" alt="">
            </div>
            <div class="sliderlist">
                <img src="<%=model.ShowImage3 %>" alt="">
            </div>
            <div class="sliderlist">
                <img src="<%=model.ShowImage4 %>" alt="">
            </div>
        </div>
    </div>
    <section class="content margin-top">
        <div class="row">
            <div class="col padding-add-center">
                <p class="font margin-top-2">Qee粉微信号<span class="list-id">第<%=model.Number %>号</span></p>
               
                 <div class="list-detail">
                    <p><span class="font">我的所在国家及城市：</span><span class="font-green"><%=model.Address %></span></p>
                    <p><span class="font">我的祝福语：</span><span class="font-green"><%=model.Introduction %>~</span></p>
                </div>
            </div>
        </div>
        <div class="text-center padding-add-center">
            <div class="list-info">
                <img src="images/detail/love.png" class="list-ico">
                <span id="lblvotecount"><%=model.VoteCount %>票</span>
            </div>
            <div class="list-info">
                <img src="images/detail/star.png" class="list-ico">
                <span>第<%=model.Rank %>名</span>
            </div>
        </div>
        <div class="row">
            <div class="col padding-add-center">
                <button class="full-image font" id="btn-diy">
                    <img src="images/detail/star1.png" class="list-ico-1">
                    投我一票(每人每天限投5票)
                </button>
            </div>
        </div>
        <div class="text-center padding-add-center">
            <div class="list-fight" id="btnShare">
                <img src="images/detail/call.png" class="full-image">
            </div>
            <div class="list-fight">
                <a href="List.aspx"><img src="images/detail/other.png" class="full-image"></a>
            </div>
        </div>
        <br />
    <img src="Images/qrcode.png" style="width:100%;" />
    </section>
    <div style="width: 100%; height: 1500px; display: none; background: #000; opacity: 0.7; position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;"
        id="sharebg">
        &nbsp;
    </div>
    <div style="position: absolute; z-index: 1000000; right: 0; width: 100%; height: 1500px; text-align: right; display: none;"
        id="sharebox">
        <img src="images/sharetip.png" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="Js/common.js" type="text/javascript"></script>
    <script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
    <script src="Js/touchslider.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#sliders").touchSlider({
                animatetime: 300,
                automatic: true,
                timeinterval: 4e3,
                sliderpoint: true,
                sliderpointwidth: 10,
                sliderpointcolor: "#006634",
                sliderpointbgcolor: "#9a9a9a"
            });

            $("#btn-diy").click(function () {

                Vote();

            })

            $("#btnShare").click(function () {
                $("#sharebg,#sharebox").show();
                $("#sharebox").css({ "top": $(window).scrollTop() })
            });

            $("#sharebg,#sharebox").click(function () {
                $("#sharebg,#sharebox").hide();
            });


        })
        //投票
        function Vote() {
            $.ajax({
                type: 'post',
                url: handlerPath,
                data: { Action: "UpdateVoteObjectVoteCount", id: "<%=model.AutoID%>" },
                timeout: 30000,
                dataType: "json",
                success: function (resp) {

                    if (resp.errcode == 0) {
                        //投票成功
                        var oldcount = $("#lblvotecount").text();
                        var newcount = parseInt(oldcount) + 1;
                        $("#lblvotecount").html(newcount + "票");
                        layermsg("投票成功!");


                    }
                    else {

                        layermsg(resp.errmsg);
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        layermsg("投票超时，请重新投票");

                    }
                }
            });


        }

    </script>
    <script type="text/javascript">
        //分享
        var shareTitle = "Qee全球街拍探秘项目平台";
        var shareDesc = "参加Qee全球街拍探秘，有大奖在等您！";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/Jiepai/Images/logo.jpg";
        var shareLink = "http://<%=Request.Url.Host %>/customize/Jiepai/index.aspx";
    </script>
</asp:Content>
