<%@ Page Title="" Language="C#" MasterPageFile="~/customize/Iluxday/Master.Master"
    AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.Iluxday.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    为TA点赞
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Styles/userDetail.css" rel="stylesheet" type="text/css" />
    <style>
        img
        {
            width: 90%;
        }
        
        .wrapUserDetail
        {
            background-size: 100% 100%;
           
        }
        .wrapUserDetail .wrapInput .row .col .inputControl label .textArea
        {
            height: 80px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="wrapUserDetail">
        <div class="slideArea">
            <img src="<%=model.VoteObjectHeadImage %>" style="margin-top: 30px;">
        </div>
        <div class="wrapInput">
            <div class="row color000000 font15">
                <div class="col">
                    <%=model.VoteObjectName %>
                </div>
                <div class="col">
                    第<%=model.Number %>号
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <div class="inputControl">
                        <label>
                            <span class="share">分享理由</span><br />
                            <textarea class="textArea" readonly="readonly"><%=model.Introduction %></textarea>
                        </label>
                    </div>
                </div>
            </div>
            <div class="row color000000 font15">
                <div class="col">
                    <i class="iconfont icon-aixin2 colorDC0000"></i>
                    <label id="lblvotecount">
                        <%=model.VoteCount %>票</label>
                </div>
                <div class="col">
                    <i class="iconfont icon-xingbiao colorFFD700 xing"></i>&nbsp;第<%=model.Rank %>名
                </div>
            </div>
        </div>
        <div class="wrapBottomBtn">
            <div class="row">
                <div class="col">
                    <a id="btnVote" href="javascript:;" class="font18">为我点赞</a>
                </div>
            </div>
            <%--        <div class="row">
            <div class="col">
                <a href="javascript:;" class="font18">关注微信再点赞</a>
            </div>
        </div>--%>
            <div class="row">
                <div class="col col-50">
                    <a href="javascript:;" id="btnShare">喊好友为TA点赞</a>
                </div>
                <div class="col col-50">
                    <a href="List.aspx">看看其他选手</a>
                </div>
            </div>
        </div>
        <div class="contact">
            扫描二维码<br />
            获取活动最新进程<br />
            了解奖品领取方法<br />
            咨询活动相关事项
        </div>
        <div class="erweima">
            <img src="images/erweima.png" class="erweimaImg0" />
        </div>
        <div class="intro">
            “爱奢汇”-专业跨境进口电商平台，精致生活，<br />
            我们只做最赞的！<br />
            <span class="">长按二维码，关注爱奢汇马上明白</span><br />
            <span class="zhubanfang">活动主办方：跨境电商爱奢汇</span>
        </div>
        <div class="bottom">
            <div class="row">
                <div class="col borderLine pLeft0" onclick="window.location.href='Index.aspx'">
                    <i class="iconfont icon-shouye shouye"></i>
                </div>
                <div class="col col-80 pLeft0 pRight0">
                    <div class="row pLeft0 pRight0">
                        <div class="col borderLine" onclick="window.location.href='Rule.aspx'">
                            活动规则
                        </div>
                        <div class="col borderLine" onclick="window.location.href='SignUp.aspx'">
                            <%=signUpText %>
                        </div>
                        <div class="col pRight0" onclick="window.location.href='List.aspx'">
                            为TA点赞
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
        <img src="images/sharetip.png" style="width: 100%;" />
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
                        $("#lblvotecount").html(newcount + "票");
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
        var shareTitle = "这是我的年度最赞微信，继续为我点赞，赢最赞时尚大奖！";
        var shareDesc = "【爱奢汇】专业跨境进口电商平台，精致生活，我们只做最赞的！";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/Iluxday/images/logo.jpg";
        var shareLink = window.location.href;
        //分享
    </script>
</asp:Content>
