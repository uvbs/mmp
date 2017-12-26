<%@ Page Title="" Language="C#" MasterPageFile="~/customize/SuperTeam/Master.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.SuperTeam.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    为Team投票
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/teamShow.css" rel="stylesheet" type="text/css" />
    <style>
        .row
        {
            width: auto;
        }
        .colorFFF
        {
            font-weight: bold;
        }      
          .row-erweima .col-erweima
        {
            padding: 10px 90px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapTeamShow">
        <div class="top">
            <img class="imgWidth" src="image/top.png">
        </div>
        <div class="content">
            <div class="row">
                <div class="col textC colorFFF font17">
                    为Team投票
                </div>
            </div>
            <div class="row padding20">
                <div class="col">
                    <img class="imgWidth" src="<%=model.ShowImage1 %>">
                </div>
            </div>
            <div class="row padding20">
                <div class="col">
                    <img class="imgWidth" src="<%=model.ShowImage2 %>">
                </div>
            </div>
            <div class="row">
                <div class="col colorFFF">
                    NO.<%=model.Number %>
                </div>
                <div class="col textC colorFFF">
                    <%=model.VoteObjectName %>
                </div>
                <div class="col textR colorFFF">
                    <label id="lblvotecount">
                        <%=model.VoteCount %></label>赞
                </div>
            </div>
            <div class="row pRight67">
                <div class="col textR colorFFF">
                    所在区域：
                </div>
                <div class="col textL colorFFF">
                    <%=model.Area %>
                </div>
            </div>
            <div class="row pRight67">
                <div class="col textR colorFFF">
                    公司品牌：
                </div>
                <div class="col textL colorFFF">
                    <%=model.Ex1 %>
                </div>
            </div>
            <div class="row pRight67">
                <div class="col textR colorFFF">
                    团队人数：
                </div>
                <div class="col textL colorFFF">
                    <%=model.Ex3 %>人
                </div>
            </div>
             <div class="row pRight67">
                <div class="col textR colorFFF">
                    门店数量：
                </div>
                <div class="col textL colorFFF">
                    <%=model.Ex8 %>
                </div>
            </div>
            <div class="row pRight67">
                <div class="col textR colorFFF">
                    团队年限：
                </div>
                <div class="col textL colorFFF">
                    <%=model.Ex2 %>年
                </div>
            </div>
            <div class="row pRight0">
                <div class="col textR colorFFF">
                    年度业绩总金额：
                </div>
                <div class="col textL colorFFF">
                    <%=model.Ex4 %>万
                </div>
            </div>
            <div class="row pRight0">
                <div class="col textR colorFFF">
                    年度成交总单数：
                </div>
                <div class="col textL colorFFF">
                    <%=model.Ex5 %>单
                </div>
            </div>
            <div class="row pRight67">
                <div class="col textR colorFFF">
                    团队宣言：
                </div>
                <div class="col textL">
                </div>
            </div>
            <div class="row mLeft57">
                <div class="col colorFFF">
                    <%=model.Introduction %>
                </div>
            </div>
            <%--<div class="row mLeft57">
                <div class="col colorFFF">
                    优秀的团队，只为同样优秀的你!
                </div>
            </div>--%>
            <div class="row row-button-reset">
                <div class="col">
                    <img class="imgWidth" src="image/button.png" id="btnVote" />
                </div>
            </div>
            <div class="row row-button-reset mTopZero9">
                <div class="col">
                    <img class="imgWidth" src="image/shareBtn.png" id="btnShare" />
                </div>
            </div>
            <div class="row row-erweima">
                <div class="col col-erweima">
                     <img class="imgWidth" src="image/qrcode.jpg"/>
                </div>
            </div>
            <div class="row row-logo">
                <div class="col col-logo">
                    <img class="imgWidth" src="image/xinwen.png">
                </div>
            </div>
        </div>
        <div class="bottom">
            <div class="row">
                <div class="col borderLine" onclick="window.location.href='Index.aspx'">
                    <i class="iconfont icon-shixinshouye shouye"></i>
                </div>
                <div class="col col-80">
                    <div class="row">
                        <div class="col borderLine" onclick="window.location.href='Rule.aspx'">
                            参赛细则
                        </div>
                        <div class="col borderLine" onclick="window.location.href='SignUp.aspx'">
                            <%=signUpText %>
                        </div>
                        <div class="col" onclick="window.location.href='Area.aspx'">
                            为Team投票
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div style="width: 100%; height: 1500px; display: none; background: #000; opacity: 0.7;
        position: absolute; top: 0; left: 0; z-index: 999999; text-align: right;" id="sharebg">
        &nbsp;
    </div>
    <div style="position: absolute; z-index: 1000000; right: 0; width: 100%; height: 1500px;
        text-align: right; display: none;" id="sharebox">
        <img src="image/sharetip.png" style="width: 100%;" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="Js/common.js" type="text/javascript"></script>
    <script src="/Plugins/LayerM/layer.m.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {


            $("#btnVote").click(function () {

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
                        $("#lblvotecount").html(newcount);
                        layermsg("点赞成功!");


                    }
                    else {

                        layermsg(resp.errmsg);
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        layermsg("点赞超时，请重新点赞");

                    }
                }
            });


        }

    </script>
    <script type="text/javascript">
        //分享
        var shareTitle = "我是SuperTeam <%=model.Number %>号选手,快来给我投票吧";
        var shareDesc = "SuperTeam";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/SuperTeam/image/logo.jpg";
        var shareLink = window.location.href;
        //分享
    </script>
</asp:Content>
