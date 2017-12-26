<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="Use.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.SVCard.Wap.Use" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/SVCard/Wap/css/List.css?v=2016112101" rel="stylesheet" />
    <link href="css/Use.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">

    <div class="wrapDetail">
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

            <div class="weui-cells weui-cells_form" v-show="!isExpire">
                <div class="weui-cell weui-cell_warn">
                    <div class="weui-cell__hd">
                        <label for="" class="weui-label">消费金额</label></div>
                    <div class="weui-cell__bd">
                        <input class="weui-input" type="number" pattern="[0-9]*" value="weui input error" placeholder="请输入消费金额" v-model="useAmount" maxlength="6"/>
                    </div>
                    <div class="weui-cell__ft">
                        <i class="weui-icon-warn"></i>
                    </div>
                </div>
                <div class="weui-cell">
                    <div class="weui-cell__bd">
                        <input class="weui-input" type="text" placeholder="备注(必填)" v-model="remark" maxlength="50"/>
                    </div>
                </div>

                <div class="weui-btn-area">
                    <a class="weui-btn weui-btn_primary" v-on:click="useCard()">确定</a>
                </div>
            </div>
            <div class="weui-btn-area" v-if="isExpire">
                <a href="javascript:;" class="weui-btn weui-btn_disabled weui-btn_warn">该储值卡不能使用</a>
            </div>
            <div v-show="isLoading">
                <div class="weui-mask_transparent"></div>
                <div class="weui-toast">
                    <i class="weui-loading weui-icon_toast"></i>
                    <p class="weui-toast__content" v-text="loadingText">请稍候...</p>
                </div>
            </div>
            </div>
        </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript" src="/App/SVCard/Wap/js/Use.js?v=2016112101"></script>
</asp:Content>
