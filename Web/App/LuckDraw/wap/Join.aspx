<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="Join.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.LuckDraw.wap.Join" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="page msg_success js_show">
        <div class="weui-msg page1">
            <div class="weui-msg__icon-area">
                <%
                    if (isSuccess)
                    {
                        %>
                            <i class="weui-icon_msg weui-icon-success"></i>
                         <%
                    }
                    else
                    {
                        %>
                             <i class="weui-icon_msg weui-icon-warn"></i>
                        <%
                    }
                 %>

            </div>
            <div class="weui-msg__text-area">
                <h2 class="weui-msg__title"><%=msg %></h2>
            </div>

           <div class="msg__desc">
               <%
                   if (isSuccess && lotteryUser.IsWinning == 1)
                   {
                       %>
                            <div>
                                <img style="width: 30%;" src="<%=lotteryUser.WXHeadimgurl %>"/>
                            </div>
                            <div>
                                <p><%=lotteryUser.WXNickname %></p>
                            </div>
                            <div>
                                <p>中奖状态:已中奖</p>
                            </div>
                            <div>
                                <p>中奖时间:<%=lotteryUser.CreateDate %></p>
                            </div>
                            <div>
                                <p>中奖编号:<%=lotteryUser.Number %></p>
                            </div>
                       <%
                   }
                   else
                   {
                        %>
                            <div>
                                <img style="width: 30%;" src="<%=lotteryUser.WXHeadimgurl %>"/>
                            </div>
                            <div>
                                <p><%=lotteryUser.WXNickname %></p>
                            </div>
                            <div>
                                <p>中奖状态:未中奖</p>
                            </div>
                         
                       <%
                   }
               %>
           </div>

            <div class="weui-msg__extra-area">
                <div class="weui-footer">
                    <p class="weui-footer__links">
                    </p>
                    <p class="weui-footer__text"><%=webSite.WebsiteName %></p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="/lib/jquery/jquery-2.1.1.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        var lotterId = '<%=lottery.LotteryID%>';

        $(function () {

            //$.ajax({
            //    type: 'POST',
            //    url: '/serv/api/admin/lottery/LotteryUserInfo/QRCodeAdd.ashx',
            //    data: { lottery_id: lotterId },
            //    dataType: 'JSON',
            //    success: function (resp) {
            //        if (resp.status) {
            //            $('.weui-icon_msg').addClass('weui-icon-success');
            //        } else {
            //            if (resp.code == '10013') {
            //                $('.weui-icon_msg').addClass('weui-icon-success');
            //            } else {
            //                $('.weui-icon_msg').addClass('weui-icon-warn');
            //            }
            //        }
            //        $('.weui-msg__title').text(resp.msg);

            //    }
            //});
        });
    </script>
</asp:Content>
