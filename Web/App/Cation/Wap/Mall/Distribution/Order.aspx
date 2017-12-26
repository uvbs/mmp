<%@ Page Title="" Language="C#" MasterPageFile="~/App/Cation/Wap/Mall/Distribution/Distribution.Master"
    AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall.Distribution.Order" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    订单
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #divNext {
            text-align: center;
            margin-top: 10px;
            margin-bottom: 50px;
        }

        .col-xs-4 {
            width: 50%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <%ZentCloud.BLLJIMP.BLLDistribution bllDis = new ZentCloud.BLLJIMP.BLLDistribution();
      int level = bllDis.GetDistributionRateLevel();
    %>
    <div class="topbar">
        <a class="col-xs-4 categorylink current" data-orderlevel="1">会员订单</a>
        <a class="col-xs-4 categorylink " data-orderlevel="0">我的订单</a>
        <%if (level >= 2)
          {%>
        <a class="col-xs-4 categorylink" data-orderlevel="2">二级订单</a>
        <%}%>
        <%if (level >= 3)
          {%>
        <a class="col-xs-4 categorylink" data-orderlevel="3">三级订单</a>
        <%}%>
    </div>
    <div class="orderbox bottom50 top50" id="objlist">
        <div id="divNoData" style="display: none;">
            没有数据
        </div>
        <div id="divNext">
            <button class="btn_main" id="btnNext" onclick="BtnNext()" style="display: none;">
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
        var pageIndex = 1;//页码
        var pageSize = 10;//页数
        var orderLevel = "1";
        $(function () {
            $(".topbar>a").click(function () {
                $(".topbar>a").removeClass("current");
                $(this).addClass("current");
                orderLevel = $(this).data("orderlevel"); //显示几级订单
                ResetSearchCondtion();
                LoadData();

            })
            LoadData();

        })


        //
        //加载数据
        function LoadData() {
            var ajaxData = {
                Action: 'QueryMyOrder',
                Level: orderLevel,
                Status: GetParm("status"),
                pageIndex: pageIndex,
                pageSize: pageSize
            }
            $("#btnNext").text("加载中...");
            $.ajax({
                type: 'post',
                url: handerurl,
                data: ajaxData,
                timeout: 60000,
                dataType: "json",
                success: function (resp) {
                    $("#btnNext").text("查看更多");
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.length; i++) {
                        str.AppendFormat('<a href="javascript:void(0)" class="order">');

                        str.AppendFormat('<div class="orderdata">订单号:{0}<br/>会员:{1}, 时间:{2}</div>', resp[i].OrderID, resp[i].TrueName, resp[i].InsertDate);

                        for (var k = 0; k < resp[i].ProductList.length; k++) {
                            str.AppendFormat('<div class="product">');
                            str.AppendFormat('<img src="{0}" >', resp[i].ProductList[k].RecommendImg);
                            str.AppendFormat('<div class="info">');
                            str.AppendFormat('<span class="text">{0}</span>', resp[i].ProductList[k].PName);
                            str.AppendFormat('<span class="price">￥{0}<span class="num">x{1}</span></span>', resp[i].ProductList[k].Price, resp[i].ProductList[k].Stock);
                            str.AppendFormat('</div>');
                            str.AppendFormat('</div>');

                        }
                        str.AppendFormat('<div class="total">');

                        if (resp[i].OrderType!=4) {
                            str.AppendFormat('共计<span class="totalnum">{0}</span>件商品,<span class="totalprice">{1}</span><span class="orangetext">元(不含运费)</span><span class="liststatus">{2}</span>', resp[i].ProductCount, resp[i].TotalAmount, ConvertDistributionStatus(resp[i]));

                        }
                        else if (resp[i].OrderType == 4) {
                            if (resp[i].Remark != null) {
                                str.AppendFormat(resp[i].Remark);
                            }


                        }

                        str.AppendFormat('<p class="tichengbox">');

                        if (resp[i].DistributionStatus == 3) {//已分佣
                            str.AppendFormat('提成:<span class="ticheng">{0}%</span>佣金金额:<span class="brokerage">{1}</span>元', resp[i].DistributionRate, resp[i].DistributionAmount);
                        }
                        else {//未分佣 显示预估的
                            str.AppendFormat('预计提成:<span class="ticheng">{0}%</span>预计佣金金额:<span class="brokerage">{1}</span>元', resp[i].DistributionRate, resp[i].DistributionAmount);
                        }
                        str.AppendFormat('</p>');
                        str.AppendFormat('</div>');
                        str.AppendFormat('</a>');

                    };

                    if (pageIndex == 1) {
                        if (resp.length == 0) {
                            $("#divNoData").show();
                            $("#divNext").hide();
                            return;

                        }
                        else {
                            $("#divNext").show();
                            $("#btnNext").show();
                            $("#divNoData").hide();
                        }


                    }
                    else {

                        if (resp.length == 0) {
                            $("#btnNext").text("没有更多");
                            $("#btnNext").removeAttr("onclick");

                        }

                    }
                    $("#divNext").before(str.ToString());

                },
                complete: function () { },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (textStatus == "timeout") {
                        alert("加载超时");

                    }

                }
            });
        }

        //下一页
        function BtnNext() {
            pageIndex++;
            LoadData();

        }
        //重置搜索条件
        function ResetSearchCondtion() {
            pageIndex = 1;
            $("#objlist>a").remove();
            $("#btnNext").attr("onclick", "BtnNext()");

        }

        //获取订单状态
        function ConvertDistributionStatus(order) {
            var status = "";
            switch (order.DistributionStatus) {
                case -1:
                    status= "退款";
                    break;
                case 0:
                    status = "未付款";
                    break;
                case 1:
                    status = "已付款";
                    break;
                case 2:
                    status = "已收货";
                    break;
                case 3:
                    status = "已审核";
                    break;
                default:

            }
            if (status=="") {
                status = order.Status;
            }
            return status;
        }

    </script>
    <% = new ZentCloud.BLLJIMP.BLL().GetIcoScript() %>
</asp:Content>
