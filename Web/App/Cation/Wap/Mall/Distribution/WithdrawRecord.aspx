<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Distribution/Distribution.Master"
    AutoEventWireup="true" CodeBehind="WithdrawRecord.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution.WithdrawRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    提现记录
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        #divNextUnPay {
            text-align: center;
            margin-top: 10px;
            margin-bottom: 50px;
        }

        #divNextHavePay {
            text-align: center;
            margin-top: 10px;
            margin-bottom: 50px;
        }

        .lblhavepay {
            color: Green;
        }

        .cashlistbox .cashlist, .viplistbox .cashlist, .cashlistbox .viplista, .viplistbox .viplista {
            height: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <div class="checkbarbox">
        <span class="col-xs-2 "></span>
        <span class="col-xs-4 checkbar current payingbar ">
            <svg class="icon" aria-hidden="true">
                <use xlink:href="#icon-dengdaidaishen1"></use>
            </svg>
            未打款 
        </span>
        <span class="col-xs-4 checkbar paidbar">
            <svg class="icon" aria-hidden="true">
                <use xlink:href="#icon-shenhechenggong"></use>
            </svg>
            已打款
        </span>
        <span class="col-xs-2 "></span>
    </div>
    <div class="cashlistbox bottom50 payinglist">
        <div id="divNextUnPay">
            <button class="btn_main" id="btnNextUnPay" onclick="BtnNextUnPay()" style="display: none;">
                查看更多</button>
        </div>
    </div>
    <div class="cashlistbox bottom50 paiedlist">
        <div id="divNextHavePay">
            <button class="btn_main" id="btnNextHavePay" onclick="BtnNextHavePay()" style="display: none;">
                查看更多</button>
        </div>
    </div>
    <div class="backbar">
        <a class="col-xs-2" href="javascript:history.go(-1);">
            <svg class="icon colorDDD" aria-hidden="true">
                <use xlink:href="#icon-fanhui"></use>
            </svg>

        </a>
        <div class="col-xs-8">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        $(".paidbar").bind("tap", function () {
            $(this).addClass("current")
            $(".payingbar").removeClass("current")
            $(".payinglist").hide()
            $(".paiedlist").show()
        })
        $(".payingbar").bind("tap", function () {
            $(this).addClass("current")
            $(".paidbar").removeClass("current")
            $(".paiedlist").hide()
            $(".payinglist").show()
        })

        var PageIndexUnPay = 1;
        var PageSizeUnPay = 10;

        var PageIndexHavePay = 1;
        var PageSizeHavePay = 10;
        $(function () {
            LoadDataUnPay();
            LoadDataHavePay();
        })



        //加载数据
        function LoadDataUnPay() {
            var ajaxData = {
                Action: 'QueryMyWithdrawRecord',
                Status: 0,
                PageIndex: PageIndexUnPay,
                PageSize: PageSizeUnPay,
                IsChannel: "<%=Request["ischannel"]%>"
            }
            $("#btnNextUnPay").text("加载中...");
            $.ajax({
                type: 'post',
                url: handerurl,
                data: ajaxData,
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    $("#btnNextUnPay").text("查看更多");
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        str.AppendFormat('<div  class="cashlist">');
                        str.AppendFormat('<span class="avatar">');
                        str.AppendFormat('<img src="images/money.png" />');
                        str.AppendFormat('</span>');
                        str.AppendFormat('<span class="cashnum">金额:{0}元</span>', resp.ExObj[i].RealAmount);
                        str.AppendFormat('<span class="cashnum">到账方式:{0}</span>', ConvertTransfersType(resp.ExObj[i].TransfersType));

                        if (resp.ExObj[i].TransfersType == 0) {
                            str.AppendFormat('<span class="cashtime">银行卡号:{0}<br/></span>', resp.ExObj[i].BankAccount);
                        }
                        str.AppendFormat('<span class="cashtime">时间:{0}</span>', FormatDate(resp.ExObj[i].LastUpdateDate));

                        str.AppendFormat('<span class="cashstatus">{0}</span>', ConvertStatus(resp.ExObj[i].Status));
                        str.AppendFormat('</div>');

                    };
                    if (PageIndexUnPay == 1) {
                        if (resp.ExObj.length == 0) {
                            $("#divNextUnPay").before("没有数据");
                            $("#divNextUnPay").remove();
                            return;

                        }
                        else {
                            $("#btnNextUnPay").show();
                        }


                    }
                    else {

                        if (resp.ExObj.length == 0) {
                            $("#btnNextUnPay").text("没有更多");
                            $("#btnNextUnPay").removeAttr("onclick");

                        }

                    }
                    $("#divNextUnPay").before(str.ToString());


                },
                complete: function () { },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("加载超时");

                    }

                }
            });
        }

        function BtnNextUnPay() {

            PageIndexUnPay++;
            LoadDataUnPay();


        }

        //
        //加载数据
        function LoadDataHavePay() {
            var ajaxData = {
                Action: 'QueryMyWithdrawRecord',
                Status: 1,
                PageIndex: PageIndexHavePay,
                PageSize: PageSizeHavePay,
                IsChannel: "<%=Request["ischannel"]%>"
            }
            $("#btnNextHavePay").text("加载中...");
            $.ajax({
                type: 'post',
                url: handerurl,
                data: ajaxData,
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    $("#btnNextHavePay").text("查看更多");
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        str.AppendFormat('<div  class="cashlist">');
                        str.AppendFormat('<span class="avatar">');
                        str.AppendFormat('<img src="images/money.png" />');
                        str.AppendFormat('</span>');
                        str.AppendFormat('<span class="cashnum">金额:{0}元</span>', resp.ExObj[i].RealAmount);

                        str.AppendFormat('<span class="cashnum">到账方式:{0}</span>', ConvertTransfersType(resp.ExObj[i].TransfersType));

                        if (resp.ExObj[i].TransfersType == 0) {
                            str.AppendFormat('<span class="cashtime">银行卡号:{0}<br/></span>', resp.ExObj[i].BankAccount);
                        }
                        str.AppendFormat('<span class="cashtime">时间:{0}</span>', FormatDate(resp.ExObj[i].LastUpdateDate));

                        str.AppendFormat('<span class="cashstatus"><label class="lblhavepay">已打款</label></span>');
                        str.AppendFormat('</div>');

                    };
                    if (PageIndexHavePay == 1) {
                        if (resp.ExObj.length == 0) {
                            $("#divNextHavePay").before("没有数据");
                            $("#divNextHavePay").remove();
                            return;

                        }
                        else {
                            $("#btnNextHavePay").show();
                        }


                    }
                    else {

                        if (resp.ExObj.length == 0) {
                            $("#btnNextHavePay").text("没有更多");
                            $("#btnNextHavePay").removeAttr("onclick");

                        }

                    }
                    $("#divNextHavePay").before(str.ToString());


                },
                complete: function () { },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("加载超时");

                    }

                }
            });
        }

        function BtnNextHavePay() {

            PageIndexHavePay++;
            LoadDataHavePay();


        }

        function ConvertStatus(status) {
            var statustext = "";
            switch (status) {
                case 0:
                    statustext = "待审核";
                    break;
                case 1:
                    statustext = "已受理";
                    break;
                case 2:
                    statustext = "已打款";
                    break;
                case 3:
                    statustext = "提现失败";
                    break;
                default:

            }
            return statustext;

        }
        //到账方式
        function ConvertTransfersType(type) {
            var typeText = "";
            switch (type) {
                case 0:
                    typeText = "银行卡";
                    break;
                case 1:
                    typeText = "微信";
                    break;
                case 2:
                    typeText = "账户余额";
                    break;
                default:

            }
            return typeText;

        }

    </script>
    <% = new ZentCloud.BLLJIMP.BLL().GetIcoScript() %>
</asp:Content>
