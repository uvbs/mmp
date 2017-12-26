<%@ Page Title="储值卡详情" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.SVCard.Wap.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/SVCard/Wap/css/List.css?v=2016112101" rel="stylesheet" />
    <link href="/App/SVCard/Wap/css/Detail.css?v=2016112101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <svg aria-hidden="true" style="position: absolute; width: 0px; height: 0px; overflow: hidden;">
        <symbol id="icon-fanhui" viewBox="0 0 1024 1024">
            <path d="M282.543 512.035 757.77 78.479c15.667-15.57 15.667-40.769 0-56.339-15.662-15.549-40.985-15.549-56.627 0L206.505 473.395c-4.838 1.944-9.371 4.866-13.291 8.769-8.424 8.358-12.311 19.522-11.676 30.472-0.268 10.534 3.632 21.154 11.722 29.186 3.883 3.859 8.367 6.758 13.151 8.701l494.659 451.322c15.667 15.571 40.985 15.571 56.653 0 15.642-15.568 15.642-40.773 0-56.344L282.543 512.035z"></path>
        </symbol>
        <symbol id="icon-shouye" viewBox="0 0 1024 1024">
            <path d="M123.992064 582.654976l58.326016 0 0 300.071936c0 59.056128 41.385984 79.997952 76.823552 79.997952 2.286592 0 4.099072-0.098304 4.900864-0.169984l160.689152 0 0-318.48448c0-5.424128 4.219904-9.339904 4.876288-9.706496l165.49376-0.193536c1.714176 1.045504 4.729856 7.709696 4.815872 10.653696 0.279552 9.119744 0.060416 225.497088-0.012288 291.581952l-0.012288 26.14784L759.452672 962.553856c28.189696 0 52.07552-13.28128 69.088256-38.455296 11.068416-16.345088 15.336448-32.202752 16.077824-35.219456l0.560128-2.262016 1.118208-305.934336 56.382464 0c19.955712 0 36.3264-8.87808 46.09024-24.974336 7.115776-11.723776 10.569728-27.022336 10.569728-46.736384L959.33952 498.260992l-7.602176-7.61344c-39.14752-39.293952-416.6144-418.098176-418.146304-419.606528-6.106112-6.020096-15.043584-9.07264-26.550272-9.07264-10.422272 0-18.763776 3.040256-24.869888 9.097216L65.312768 496.412672l0 10.580992c0 19.494912 6.531072 38.272 18.388992 52.878336C95.291392 574.142464 110.347264 582.654976 123.992064 582.654976zM903.698432 528.4096c-0.253952 0.023552-0.595968 0.03584-1.019904 0.03584L793.662464 528.44544l-1.093632 350.560256c-2.750464 8.39168-12.174336 31.328256-33.11616 31.328256L652.722176 910.333952c0.050176-39.816192 0.29184-256.459776-0.03584-267.040768-0.256-8.950784-3.453952-22.984704-11.63776-35.511296-10.78784-16.539648-26.768384-25.63584-45.00992-25.63584L429.60896 582.146048c-30.717952 0-57.65632 28.944384-57.65632 61.9264l0 266.261504-109.246464-0.048128-3.56352 0.218112c-21.16096 0-24.018944-14.03392-24.018944-27.776l0-352.28672L128.103424 530.441216c-2.530304-2.067456-6.725632-6.77376-8.768512-14.107648L507.978752 119.735296l398.019584 399.418368C905.39008 524.299264 904.392704 527.023104 903.698432 528.4096z"></path>
        </symbol>
    </svg>
    <div class="wrapDetail">
        <div class="headerBar">
            <a href="/App/SVCard/Wap/List.aspx" class="go-back">
                <svg class="icon" aria-hidden="true">
                    <use xlink:href="#icon-fanhui"></use>
                </svg>
            </a>
            <span v-text="card.name"></span>
            <a href="/customize/comeoncloud/Index.aspx?key=MallHome" class="btnBackHomeRight">
                <svg class="icon" aria-hidden="true">
                    <use xlink:href="#icon-shouye"></use>
                </svg>
            </a>
        </div>
        <div class="wrapList">
            <div class="wrapCells">
                <div class="weui-cells">
                    <div class="weui-cell">
                        <div class="wrapCard">
                            <img class="cardImg" v-bind:src="card.bg_img" />
                            <div class="cardName">{{card.name}}(余额{{card.canuse_amount}}元)</div>
                            <div class="cardAmount">{{card.amount|amountFommat}}</div>
                            <div class="cardValid">{{card.valid_to|validFommat}}</div>
                            <div class="cardNumber" v-text="card.card_number"></div>
                            <div class="cardStatus">{{card.use_status|statusFommat}}</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapInfo" v-if="card.use_status == 11 || card.use_status == 10 || card.cur_user_id == card.touser_id">
            <table class="fromUser" v-if="card.use_status == 10 || card.cur_user_id == card.touser_id">
                <tr>
                    <td class="fromUserImg">
                        <img v-bind:src="card.user_avatar" />
                    </td>
                    <td class="fromInfo">
                        <div class="fromUserName" v-text="card.user_nickname"></div>
                        <div class="fromUserRmk">转赠您一张储值卡</div>
                    </td>
                </tr>
            </table>
            <table class="fromUser" v-if="card.use_status == 11">
                <tr>
                    <td class="fromUserImg">
                        <img v-bind:src="card.touser_avatar" />
                    </td>
                    <td>
                        <div class="fromUserName" v-text="card.touser_nickname"></div>
                        <div class="fromUserRmk" v-if="card.status==1">已接收您转赠的储值卡</div>
                        <div class="fromUserRmk" v-if="card.status==9">已使用您转赠的储值卡</div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="wrapInfo" v-if="card.use_status==0 || card.use_status==1 || card.use_status==2 ||  card.use_status==10 || card.use_status==11">
            <a  v-bind:href="'UseQrCode.aspx?id=' + card.id"  class="weui-btn weui-btn_warn" >支付二维码</a>
            <a  v-bind:href="'UseRecord.aspx?id=' + card.id"  class="weui-btn weui-btn_warn" >查看使用记录</a>
            <div class="infoRmk" v-if="card.status == 0 && card.use_status==0">转赠当好友接收才算完成</div>
            <div class="infoRmk" v-if="card.use_status==10">好友转赠的储值卡可继续转赠他人</div>
             <a href="javascript:void(0);" v-if="(card.use_status == 0) || card.use_status==10" class="weui-btn weui-btn_warn" v-on:click="showShareTip()">转赠给好友</a>
<%--         <a href="javascript:void(0);" v-if="(card.status == 0 && card.use_status == 0) || card.use_status==10" class="weui-btn weui-btn_warn" v-on:click="showShareTip()">转赠给好友</a>--%>
            <div class="infoRmk" v-if="card.use_status==10">接收好友转赠的储值卡</div>
            <a href="javascript:void(0);" v-if="card.use_status==10" class="weui-btn weui-btn_warn" v-on:click="giveCard()">接收储值卡</a>
            <div class="infoRmk" v-if="card.use_status==0 && card.status==1">接收完成，去个人中心查看</div>
            <a href="javascript:void(0);" v-if="(card.use_status==0 && card.status==1) || card.use_status==1" class="weui-btn weui-btn_warn" v-on:click="goCenter()">去个人中心</a>
            <a href="javascript:void(0);" v-if="card.use_status==1 || card.use_status==2 || card.use_status==11" class="weui-btn weui-btn_disabled weui-btn_default">储值卡{{card.use_status|statusFommat}}</a>
        </div>
        <div class="shareTip shareTipHide" v-on:click="showShareTip()">
            <img src="http://comeoncloud-static.oss-cn-hangzhou.aliyuncs.com/img/sharetip.png" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="//static-files.socialcrmyun.com/Scripts/wxshare/wxshare1.1.0/wxshare.js"></script>
    <script type="text/javascript" src="/App/SVCard/Wap/js/Detail.js?v=2016112101"></script>
</asp:Content>
