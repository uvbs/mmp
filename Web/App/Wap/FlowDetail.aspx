<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="FlowDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.FlowDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
    <style type="text/css">
        .weui-mask_transparent{
            z-index:19891020 !important;
        }
        .weui-toast{
            z-index:19891021 !important;
        }
        .weui-label{
            width:105px !important;
        }
        .popuo-form h3{
            background-color: #2d5ea8;
            color: #fff;
        }
        .popuo-form .layui-m-layercont{
            padding:10px;
        }
        .popuo-form .layui-m-layercont .weui-textarea{
                padding: 5px !important;
                border: solid #ccc 1px !important;
                width: 95% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="comm-form">
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">提现金额：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.amount">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">扣税金额：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.deduct_amount">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">实际金额：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.true_amount">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">开户银行：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.ex1">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">开户名：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.ex2">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">卡号：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.ex3">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">提现时间：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.start">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">状态：</label>
                        </div>
                        <div class="weui_cell__bd" v-bind:class="[form.status==8 || form.status==10 || form.status==11||  form.status==12?'color-red':'' ]" v-text="form.status_s">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form divNone"
                    v-bind:class="[form.status==10 || form.status==11 || form.status==12?'divBlock':'']" 
                    v-if="form.status==10 || form.status==11 || form.status==12">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">申请取消时间：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.cancel_date">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form divNone" 
                    v-bind:class="[form.status==10?'divBlock':'']" 
                    v-if="form.status==10">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">取消时间：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.end">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form divNone" 
                    v-bind:class="[form.status==12?'divBlock':'']" 
                    v-if="form.status==12">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">拒绝取消时间：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.end">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form divNone" 
                    v-bind:class="[form.status==8?'divBlock':'']" 
                    v-if="form.status==8">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">拒绝提现时间：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.end">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form divNone" 
                    v-bind:class="[form.status==9?'divBlock':'']" 
                    v-if="form.status==9">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">完成时间：</label>
                        </div>
                        <div class="weui_cell__bd" v-text="form.end">
                        </div>
                    </div>
                </div>
                <div class="weui-cells weui-cells_form divNone" 
                    v-bind:class="[ form.end_content && (form.status==8||form.status==11)?'divBlock':'']" 
                    v-if="form.end_content && (form.status==8||form.status==11)">
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">审核说明：</label>
                        </div>
                        <div class="weui_cell__bd" v-html="form.end_content">
                        </div>
                    </div>
                </div>
            </div>
            <div class="comm divNone" v-bind:class="[form.status==0?'divBlock':'']" v-if="form.status==0">
                <a href="javascript:void(0);" class="weui-btn weui-btn_warn" v-on:click="cancel()">申请取消</a>
            </div>
        </div>
        <div class="wrapBottom">
            <div v-text="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/FlowDetail.js?v=2017030101"></script>
</asp:Content>
