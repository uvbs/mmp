<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="BankCardAddEdit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Wap.BankCardAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="css/BankCardAddEdit.css" rel="stylesheet" />--%>
    <link href="/App/Wap/css/Common.css?v=2017030101" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="wrapComm">
        <div class="wrapContent">
            <div class="wrapCommList">
                <div class="weui-cells weui-cells_form" style="margin-top:0px;padding-top: 5px;">
                    <div class="weui-cell weui-cell_select weui-cell_select-after">
                        <div class="weui-cell__hd">
                            <label for="" class="weui-label">开户银行</label>
                        </div>
                        <div class="weui-cell__bd">
                            <select class="weui-select" name="select2" id="ddlBankName">
                                <option value="">请选择</option>
                                <option value="招商银行">招商银行</option>
                                <option value="工商银行">工商银行</option>
                                <option value="中国农业银行">中国农业银行</option>
                                <option value="北京银行">北京银行</option>
                                <option value="中国银行">中国银行</option>
                                <option value="交通银行">交通银行</option>
                                <option value="上海银行">上海银行</option>
                                <option value="中国建设银行">中国建设银行</option>
                                <option value="中国光大银行">中国光大银行</option>
                                <option value="兴业银行">兴业银行</option>
                                <option value="中信银行">中信银行</option>
                                <option value="中国民生银行">中国民生银行</option>
                                <option value="广发银行">广发银行</option>
                                <option value="华厦银行">华厦银行</option>
                                <option value="南京银行">南京银行</option>
                                <option value="平安银行">平安银行</option>
                                <option value="中国邮政储蓄银行">中国邮政储蓄银行</option>
                                <option value="浦发银行">浦发银行</option>
                                <option value="天津银行">天津银行</option>
                                <option value="其它">其它银行</option>
                            </select>
                        </div>
                    </div>
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label for="" class="weui-label">卡号</label>
                        </div>
                        <div class="weui-cell__bd">
                            <input id="txtBankAccount" class="weui-input" type="tel" pattern="[0-9]*" maxlength="19" value="<%=model.BankAccount %>" placeholder="请输入卡号">
                        </div>
                        <div class="weui-cell__ft">
                            <%--<i class="weui-icon-warn"></i>--%>
                        </div>
                    </div>
                    <div class="weui-cell">
                        <div class="weui-cell__hd">
                            <label class="weui-label">开户名</label>
                        </div>
                        <div class="weui-cell__bd">
                            <input id="txtAccountName" class="weui-input" type="text" placeholder="请输入开户名" maxlength="10" value="<%=model.AccountName %>">
                        </div>
                    </div>
                </div>
                <div class="weui-btn-area">
                    <a class="weui-btn weui-btn_blue" href="javascript:" onclick="addEdit()">确定</a>
                    <a id="btnDel" class="weui-btn weui-btn_warn" style="display: none;" href="javascript:" onclick="delEdit()">删除</a>
                </div>
            </div>
        </div>
        <div class="wrapBottom">
            <div id="bottom_text"></div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var loadBankName = '<%=model.BankName %>';
    </script>
    <script type="text/javascript" src="/App/Wap/js/Common.js?v=2017032001"></script>
    <script type="text/javascript" src="/App/Wap/js/BandCardAddEdit.js?v=2017030101"></script>
</asp:Content>
