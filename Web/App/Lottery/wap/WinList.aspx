<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="WinList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Lottery.wap.WinList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Lottery/wap/css/winList.css?v=20170106" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapWinList" id="wrapWinList">        
        <div class="title">
            <h1>{{lottery.name}}</h1>
            
        </div>
        <div class="subTitle">中奖记录</div>
        <div class="dataList">
            <div class="weui-panel" v-for="item in lottery.winRecord">
                <%--<div class="weui-panel__hd">文字列表附来源</div>--%>
                <div class="weui-panel__bd">
                    <div class="weui-media-box weui-media-box_text">
                        <h4 class="weui-media-box__title">{{item.org_obj.PrizeName}}</h4>
                        <p class="weui-media-box__desc">{{item.org_obj.Description}}</p>

                         <ul class="weui-media-box__info">
                            <li class="weui-media-box__info__meta">中奖时间：{{item.time}}</li>
                        </ul>

                        <div class="weui-media-box__info txtCenter">
                            <div v-if="item.type == 0 && lottery.isGetPrizeFromMobile > 0">
                                <div v-if="!item.is_reveice">
                                    <div v-if="lottery.isGetPrizeFromMobile == 2">                            
                                        <a 
                                            href="javascript:;" 
                                            class="weui-btn weui-btn_mini weui-btn_warn"
                                            v-if="item.record_user_name && item.record_user_phone"    
                                            @click="getAward(item)"
                                        >领奖(非客服勿点)</a>                            
                                        <a 
                                            href="javascript:;" 
                                            class="weui-btn weui-btn_mini weui-btn_primary"
                                            v-if="!item.record_user_name || !item.record_user_phone" 
                                            @click="openSubmitUserInfo(item)"
                                        >提交领奖信息后才可领奖</a>
                                    </div>
                            
                                    <div v-if="lottery.isGetPrizeFromMobile == 1">
                                        <a href="javascript:;" class="weui-btn weui-btn_mini weui-btn_warn" @click="getAward(item)">领奖(非客服勿点)</a>                            
                                    </div>                            
                                </div>
                                <a href="javascript:;" class="weui-btn weui-btn_mini weui-btn_default" v-if="item.is_reveice">已领奖</a>
                            </div>       
                            <div v-if="item.type > 0">
                                <a href="/customize/comeoncloud/Index.aspx?key=PersonalCenter" class="weui-btn weui-btn_mini weui-btn_default">奖品已发，点击进入个人中心查看</a>
                            </div>
                            
                           <%-- <div v-if="item.type == 2">
                                <a href="/customize/shop/?v=1.0&ngroute=/mycoupons#/mycoupons" class="weui-btn weui-btn_mini weui-btn_default">奖品已发，点击进入我的优惠券查看</a>
                            </div>--%>
                        </div>

                       
                        <%--<div class="weui-form-preview__ft">
                            <a class="weui-form-preview__btn weui-form-preview__btn_default" href="javascript:">辅助操作</a>
                            <button type="submit" class="weui-form-preview__btn weui-form-preview__btn_primary" href="javascript:">操作</button>
                        </div>--%>
                    </div>
                </div>
            </div>
            <div v-show="lottery.winRecord.length == 0" class="noRecordMsg">
                您还没有中奖
            </div>
        </div>

         <div v-show="dialog.show">
            <div class="weui-mask"></div>
            <div class="weui-dialog">
                <div class="weui-dialog__hd"><strong class="weui-dialog__title">{{dialog.title}}</strong></div>
                <div class="weui-dialog__bd">{{dialog.msg}}</div>
                <div class="weui-dialog__ft">
                    <a href="javascript:;" class="weui-dialog__btn weui-dialog__btn_primary" @click="closeDialog()">确定</a>
                </div>
            </div>
        </div>

        <div v-show="modalSubmitUserInfo.show">
            <div class="weui-mask"></div>
            <div class="weui-dialog">
                <div class="weui-dialog__hd"><strong class="weui-dialog__title">提交领奖信息</strong></div>
                <div class="weui-dialog__bd">
                    
                    <%--<div class="weui-cells__title">领奖姓名</div>--%>
                    <div class="weui-cells">
                        <div class="weui-cell">
                            <div class="weui-cell__bd">
                                <input class="weui-input" type="text" placeholder="请输入领奖姓名" v-model="modalSubmitUserInfo.name">
                            </div>
                        </div>
                    </div>

                    <%--<div class="weui-cells__title">领奖手机</div>--%>
                    <div class="weui-cells">
                        <div class="weui-cell">
                            <div class="weui-cell__bd">
                                <input class="weui-input" type="text" placeholder="请输入领奖手机" v-model="modalSubmitUserInfo.phone">
                            </div>
                        </div>
                    </div>

                </div>
                <div class="weui-dialog__ft">
                    <a href="javascript:;" class="weui-dialog__btn weui-dialog__btn_primary" @click="submitUserInfo()">确定</a>
                    <a href="javascript:;" class="weui-dialog__btn weui-dialog__btn_default" @click="cancelSubmitUserInfo()">取消</a>
                </div>
            </div>
        </div>
       
    </div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Lottery/wap/js/winList.es5.min.js?v=20170106"></script>
</asp:Content>
