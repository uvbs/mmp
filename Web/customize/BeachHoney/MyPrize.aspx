<%@ Page Title="" Language="C#" MasterPageFile="~/customize/BeachHoney/Master.Master"
    AutoEventWireup="true" CodeBehind="MyPrize.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.BeachHoney.MyPrize" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    沙滩宝贝-我的奖品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        body
        {
            background-color: White;
        }
        table
        {
            width: 95%;
        }
        .form_input
        {
            width: 100%;
        }
        .form_span
        {
            height: auto;
            padding: 7px 5px 9px 5px;
        }
        .votedetail
        {
            color: #fff;
            background-color: #e66f80;
            border-radius: 5px;
            padding: 2px 5px 2px 5px;
        }
        .vercode
        {
            color: #fff;
            background-color: #e66f80;
            border-radius: 5px;
            padding: 2px 5px 2px 5px;
        }
        
        #btnGetCardCoupon
        {
            color: #fff;
            background-color: #e66f80;
            border-radius: 5px;
            padding: 5px 5px 5px 5px;
            width: 80%;
            height: 30px;
            font-size: 14px;
            margin-top: 10px;
        }
        #btnMyCardCoupon
        {
            color: #fff;
            background-color: #e66f80;
            border-radius: 5px;
            padding: 5px 5px 5px 5px;
            width: 80%;
            height: 30px;
            font-size: 14px;
            margin-top: 10px;
            width: 206px;
        }
        #btnGetSmsVerCode
        {
            color: #fff;
            background-color: #e66f80;
            border-radius: 5px;
        }
        
        .btnRed
        {
            color: #fff;
            background-color: #e66f80;
            border-radius: 5px;
            padding: 5px 5px 5px 5px;
            width: 80%;
            height: 30px;
            font-size: 14px;
            margin-top: 10px;
            width: 206px;
        }
        .rule{font-weight:bold;margin-left:5px;margin-right:5px;margin-bottom:10px;font-size:16px;line-height:25px;}
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="image_single">
        <img src="images/match_01.jpg" alt="" title="" border="0" />
    </div>
    <div class="list2">
        <div class="form ">
            <div class="rule">
                 热带风暴门票使用规则:<br />
                 【兑票窗口】请到网购窗口把热带风暴电子门票兑换成“当日”入园卡。<br />
【兑换票时间】每天上午09:00-下午18:00，超过时间不予兑换热带风暴电子门票。 电子门票兑换的入园卡仅限当天使用。<br />
【获取门票步骤】点击“我的报名”页面，获取手机验证码，领取热带风暴电子门票，在“我的奖品”页面查看“我的热带风暴门票”；获奖者可在有效期内凭电子门票至热带风暴水上乐园网购窗口兑换入园卡进行使用。<br />
【现场使用步骤】现场使用手机进入沙滩宝贝微信公众号“我的热带风暴门票”页面，由工作人员输入验票密码，即完成电子票的使用！<br />
【特别提醒】热带风暴电子票仅在有效期规定时间内使用，截屏无效！
            </div>
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">我的参赛编号:&nbsp;
                                <%=model.Number%>号 &nbsp; </span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">我的排名:&nbsp; 第<%=model.Rank %>名</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">我的票数:&nbsp;
                                <%=model.VoteCount %>票</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">姓名</span>
                        </td>
                        <td>
                            <input type="text" id="txtVoteObjectName" name="user" value="<%=model.VoteObjectName %>"
                                placeholder="输入你的姓名" class="form_input" readonly="readonly" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_div radius4">
                <table style="width: 100%;">
                    <tr>
                        <td valign="top">
                            <span class="form_span">手机</span>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <input type="text" id="txtPhone" value="<%=model.Phone %>" placeholder="" class="form_input"
                                            readonly="readonly" />
                                    </td>
                                    <td valign="top">
                                        <%if (!IsGetPrize)
                                          {%>
                                        <a class="form_input" id="btnGetSmsVerCode">获取验证码</a>
                                        <%} %>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <%if (!IsGetPrize)
              {%>
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">手机验证码</span>
                        </td>
                        <td>
                            <input type="text" id="txtSmsVerCode" placeholder="输入接收到的手机验证码" class="form_input"
                                maxlength="6" />
                        </td>
                    </tr>
                </table>
            </div>
            <%} %>
            <%if (!IsGetPrize)
              {%>
            <div class="form_div radius4">
                <div class="form_div3" style="margin-top: 10px;">
                    <a id="btnGetCardCoupon">领取热带风暴门票</a>
                </div>
            </div>
            <%} %>
            <%if (IsGetPrize)
              {%>
            <div class="form_div radius4">
                <div class="form_div3" style="margin-top: 10px;">
                    <a id="btnMyCardCoupon" href="/Components/Coupon/Mobile/BeachHoney/index.aspx">我的热带风暴门票</a>
                </div>
            </div>
            <%} %>
            <%if (model.Rank <= 100)
              {%>
            <hr />
            <div class="form_div radius4">
                <table>
                    <tr>
                        <td valign="top">
                            <span class="form_span">收货地址</span>
                        </td>
                        <td>
                            <input type="text" id="txtAddress" placeholder="请输入您的收货地址" class="form_input" value="<%=model.Address %>" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form_div3" style="margin-top: 10px;">
                <a id="btnSumbit" class="btnRed">确认收货地址</a>
            </div>
            <% } %>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var IsSumbit = false;
        $(function () {
            $("#btnSumbit").click(function () {

                Sumbit();

            });
            $("#btnGetCardCoupon").click(function () {

                GetCardCoupon();

            });


            $("#btnGetSmsVerCode").click(function () {
                GetSmsVerCode();

            })







        })

        //提交我的收货地址
        function Sumbit() {
            var model = {
                Action: "UpdateMyAddress",
                Address: $.trim($("#txtAddress").val())

            }
            if (model.Address == "") {
                layermsg("请输入您的收货地址");
                return false;
            }
            $.ajax({
                type: "post",
                url: "Handler.ashx",
                data: model,
                timeout: 30000,
                dataType: "json",
                success: function (resp) {
                    if (resp.errcode == 0) {
                        layermsg("您的收货地址已经提交！");


                    } else {
                        layermsg(resp.errmsg);
                    }
                }
            })


        }


        //获取卡券
        function GetCardCoupon() {
            var model = {
                Action: "GetCardCoupon",
                SmsVerCode: $.trim($("#txtSmsVerCode").val())

            }
            if (model.SmsVerCode == "") {
                layermsg("请输入短信验证码");
                return false;
            }

            $("#btnGetCardCoupon").html("正在领取...");
            $.ajax({
                type: "post",
                url: "Handler.ashx",
                data: model,
                timeout: 30000,
                dataType: "json",
                success: function (resp) {
                    $("#btnGetCardCoupon").val("");
                    if (resp.errcode == 0) {
                        $("#btnGetCardCoupon").html("领取成功");
                        layermsg("领取门票成功，您可以在我的门票中查看");
                        window.location.href = "MyPrize.aspx";

                    } else {
                        layermsg(resp.errmsg);
                        $("#btnGetCardCoupon").html("领取热带风暴门票");
                    }
                }
            })


        }

        //获取手机验证码
        function GetSmsVerCode(phone) {

            $.ajax({
                type: 'post',
                url: "Handler.ashx ",
                data: { Action: "GetSmsVercode" },
                dataType: "json",
                success: function (resp) {
                    if (resp.errcode == 0) {
                        layermsg("验证码发送成功");

                    }
                    else {
                        layermsg(resp.errmsg);

                    }
                }
            });
        }
    </script>
    <script type="text/javascript">
        //分享
        var shareTitle = "我的奖品";
        var shareDesc = "沙滩宝贝";
        var shareImgUrl = "http://<%=Request.Url.Host %>/customize/beachhoney/images/match_01.jpg";
        var shareLink = window.location.href;
        //分享
    </script>
</asp:Content>
