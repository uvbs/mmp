<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="PrintDetail.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Score.PrintDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body {
            background-color: #ffffff !important;
        }

        .centent_r_btm {
            width: auto !important;
            height: 100% !important;
            border: 0px !important;
        }

        .flowFromTable {
            width: 100%;
            border: 1px solid rgb(211, 211, 211);
            border-collapse: collapse;
            display: table;
        }

            .flowFromTable .flowTdTitle {
                text-align: center;
                font-size: 24px;
                line-height: 40px;
                border: solid #d3d3d3 1px;
                padding: 5px;
            }

            .flowFromTable .flowTdLeft {
                width: 50%;
                font-size: 16px;
                line-height: 21px;
                border: solid #d3d3d3 1px;
                padding: 5px;
            }

            .flowFromTable .flowTd {
                font-size: 16px;
                line-height: 21px;
                border: solid #d3d3d3 1px;
                padding: 5px;
            }

            .flowFromTable .flowTdm {
                font-size: 16px;
                line-height: 21px;
                border: solid #d3d3d3 1px;
                height: 52px;
                padding: 5px;
                vertical-align: top;
                word-break: break-all;
            }

            .flowFromTable .flowTh {
                font-size: 18px;
                line-height: 21px;
                background-color: #f9f9f9;
                border: solid #d3d3d3 1px;
                min-height: 52px;
                padding: 5px;
            }

            .flowFromTable .flowTrImg {
            }
            .flowFromTable .flowTrImg .flowImgDiv {
                display: inline-block; 
                padding: 5px;
            }
            .flowFromTable .flowTrImg .flowImgDiv a{
                display: inline-block;
                margin:3px;
            }
            .flowFromTable .flowTrImg .flowImgDiv a img{
                width: 80px; 
                height: 80px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="divDetail" style="padding: 10px 20px 20px;">
        <table class="flowFromTable">
            <tr class="flowTrTitle">
                <td class="flowTdTitle" colspan="2">线下注册</td>
            </tr>
            <tr class="flowTrColTwo">
                <td class="flowTdLeft">会员：曾锦坪</td>
                <td class="flowTd">金额：20000</td>
            </tr>
            <tr class="flowTrColOne">
                <td class="flowTd" colspan="2">金额：20000</td>
            </tr>
            <tr class="flowTrMult">
                <td class="flowTdm" colspan="2">金额：20000</td>
            </tr>
            <tr class="flowTrTh">
                <td class="flowTh" colspan="2">金额：20000</td>
            </tr>
            <tr class="flowTrImg">
                <td class="flowTdImg" colspan="2">
                    <div class="flowImgDiv">
                        <a href="http://open-files.comeoncloud.net/www/hf/jubit/image/20161209/3DF388A18EE9473B88445C78276BB12F.png" target="_blank">
                            <img alt="" src="http://open-files.comeoncloud.net/www/hf/jubit/image/20161209/3DF388A18EE9473B88445C78276BB12F.png" />
                        </a>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/score/';
        var id = '<%= Request["id"] %>';
        var baseTrTitle;
        var baseTrColTwo;
        var baseTrColOne;
        var baseTrMult;
        var baseTrTh;
        var baseTrImg;
        $(function () {
            if (id) GetPrintDetail(_id);
            GetEmptyTable();
        });
        function GetEmptyTable() {
            baseTrTitle = $('.flowFromTable .flowTrTitle').clone();
            baseTrColTwo = $('.flowFromTable .flowTrColTwo').clone();
            baseTrColOne = $('.flowFromTable .flowTrColOne').clone();
            baseTrMult = $('.flowFromTable .flowTrMult').clone();
            baseTrTh = $('.flowFromTable .flowTrTh').clone();
            $('.flowFromTable .flowTrImg .flowImgDiv').html('');
            baseTrImg = $('.flowFromTable .flowTrImg').clone();
            SetEmpty();
        }
        function SetEmpty() {
            $('.divDetail').html('');
        }
        function GetPrintDetail(_id) {
            $.messager.progress();
            $.ajax({
                type: 'post',
                url: handlerUrl + 'PrintDetail.ashx',
                data: { id: _id },
                dataType: 'json',
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.status) {
                        FormatHtml(resp.result);
                    } else {
                        alert(resp.msg);
                    }
                },
                error: function () {
                    $.messager.progress('close');
                }
            });
        }
        function FormatHtml(data) {
            var flowFromTable = $('<table class="flowFromTable"></table>');
            var flowTitle = $(baseTrTitle).clone();
            $(flowTitle).find('.flowTdTitle').text(data.score_event);
            $(flowFromTable).append(flowTitle);

            var flowMember = $(baseTrColTwo).clone();
            $(flowMember).find('.flowTdLeft').text('会员：' + data.member_name);
            $(flowMember).find('.flowTd').text('会员手机：' + data.member_phone);
            $(flowFromTable).append(flowMember);
            if (data.flow_action) {
                var flowCreater = $(baseTrColTwo).clone();
                $(flowCreater).find('.flowTdLeft').text('提交人：' + data.flow_action.creater_name);
                $(flowCreater).find('.flowTd').text('提交时间：' + new Date(data.flow_action.create_time).format('yyyy年MM月dd日 hh:mm'));
                $(flowFromTable).append(flowCreater);
            }
            if (data.score_event.indexOf('充值') >= 0) {
                var flowAmount = $(baseTrColOne).clone();
                $(flowAmount).find('.flowTd').text('充值金额：' + data.amount);
                $(flowFromTable).append(flowAmount);

                var flowEx1 = $(baseTrColOne).clone();
                if (data.flow_action) {
                    $(flowEx1).find('.flowTd').text('充值渠道：' + data.flow_action.ex1);
                    $(flowFromTable).append(flowEx1);
                } else {
                    var ex5_cn = '';
                    if (data.ex5 == 'alipay') ex5_cn = '支付宝';
                    if (data.ex5 == 'weixin') ex5_cn = '微信';
                    if (ex5_cn) {
                        $(flowEx1).find('.flowTd').text('充值渠道：' + ex5_cn);
                        $(flowFromTable).append(flowEx1);
                    }
                }
                if (data.ex5 != 'offline') {
                    if (data.rel_id) {
                        var flowZcPay = $(baseTrColOne).clone();
                        $(flowZcPay).find('.flowTd').text('商户单号：' + data.rel_id);
                        $(flowFromTable).append(flowZcPay);
                    }
                    if (data.serial_number) {
                        var flowPay = $(baseTrColOne).clone();
                        $(flowPay).find('.flowTd').text('支付单号：' + data.serial_number);
                        $(flowFromTable).append(flowPay);
                    }
                }
            } else if (data.score_event.indexOf('提现') >= 0) {
                var flowAmount = $(baseTrColOne).clone();
                var flowDeductAmount = $(baseTrColOne).clone();
                var flowEx1 = $(baseTrColOne).clone();
                var flowEx2 = $(baseTrColOne).clone();
                var flowEx3 = $(baseTrColOne).clone();
                $(flowAmount).find('.flowTd').text('提现金额：' + (0- data.amount));
                $(flowDeductAmount).find('.flowTd').text('扣税金额：' + data.deduct_amount);
                if (data.flow_action) {
                    $(flowEx1).find('.flowTd').text('开户银行：' + data.flow_action.ex1);
                    $(flowEx2).find('.flowTd').text('开户名：' + data.flow_action.ex2);
                    $(flowEx3).find('.flowTd').text('银行卡号：' + data.flow_action.ex3);
                } else {
                    $(flowEx1).find('.flowTd').text('开户银行：' + data.ex1);
                    $(flowEx2).find('.flowTd').text('开户名：' + data.ex2);
                    $(flowEx3).find('.flowTd').text('银行卡号：' + data.ex3);
                }
                $(flowFromTable).append(flowAmount);
                $(flowFromTable).append(flowDeductAmount);
                $(flowFromTable).append(flowEx1);
                $(flowFromTable).append(flowEx2);
                $(flowFromTable).append(flowEx3);
            }
            if (data.flow_action) {
                if (data.flow_action.content) {
                    var flowContent = $(baseTrMult).clone();
                    $(flowContent).find('.flowTdm').html(data.flow_action.content);
                    $(flowFromTable).append(flowContent);
                }
                if (data.flow_action.files.length > 0) {
                    var flowImg = $(baseTrImg).clone();
                    var count = 0;
                    for (var i = 0; i < data.flow_action.files.length; i++) {
                        var sfurl = $.trim(data.flow_action.files[i].url);
                        if (sfurl != '') {
                            count++;
                            $(flowImg).find('.flowImgDiv').append('<a href="' + sfurl + '" target="_blank"><img alt="" src="' + sfurl + '" /></a>');
                        }
                    }
                    if (count>0) $(flowFromTable).append(flowImg);
                }
            }
            if (data.flow_action && data.flow_action.details.length > 1) {
                for (var i = 1; i < data.flow_action.details.length; i++) {
                    var stepDetail = data.flow_action.details[i];

                    var flowStepName = $(baseTrTh).clone();
                    $(flowStepName).find('.flowTh').html(stepDetail.stepname);
                    $(flowFromTable).append(flowStepName);

                    if (stepDetail.content) {
                        var flowContent = $(baseTrMult).clone();
                        $(flowContent).find('.flowTdm').html(stepDetail.content);
                        $(flowFromTable).append(flowContent);
                    }
                    if (stepDetail.files.length > 0) {
                        var flowImg = $(baseTrImg).clone();
                        var count = 0;
                        for (var i = 0; i < stepDetail.files.length; i++) {
                            var sfurl = $.trim(stepDetail.files[i].url);
                            if (sfurl != '') {
                                count++;
                                $(flowImg).find('.flowImgDiv').append('<a href="' + sfurl + '" target="_blank"><img alt="" src="' + sfurl + '" /></a>');
                            }
                        }
                        if (count > 0) $(flowFromTable).append(flowImg);
                    }
                    var selectDate = new Date(stepDetail.select_date);
                    if (selectDate > new Date(2016, 1, 1)) {
                        var flowSelectDate = $(baseTrColOne).clone();
                        if (data.score_event.indexOf('充值') >= 0) {
                            $(flowSelectDate).find('.flowTd').text('到账时间：' + selectDate.format('yyyy年MM月dd日 hh:mm'));
                        } else if (data.score_event.indexOf('提现') >= 0) {
                            $(flowSelectDate).find('.flowTd').text('打款时间：' + selectDate.format('yyyy年MM月dd日 hh:mm'));
                        }
                    }
                    var flowHandler = $(baseTrColTwo).clone();
                    $(flowHandler).find('.flowTdLeft').text('审核人：' + stepDetail.handler_name);
                    $(flowHandler).find('.flowTd').text('审核时间：' + new Date(stepDetail.handle_time).format('yyyy年MM月dd日 hh:mm'));
                    $(flowFromTable).append(flowHandler);
                }
            } else {
                var flowDate = $(baseTrColOne).clone();
                $(flowDate).find('.flowTd').text('时间：' + new Date(data.time).format('yyyy年MM月dd日 hh:mm'));
                $(flowFromTable).append(flowDate);
            }
            $('.divDetail').append(flowFromTable);
        }
        function PrintDetail() {
            $('.sort').css('display', 'none');
            $('.divDetail').css('padding', '0px');
            window.print();
            $('.sort').css('display', 'block');
            $('.divDetail').css('padding', '10px 20px 20px');
        }
    </script>
</asp:Content>
