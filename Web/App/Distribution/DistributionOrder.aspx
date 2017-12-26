<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOrder.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.Distribution.DistributionOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>
        <%=userInfo.UserID%>的订单</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <link href="../Cation/Wap/Mall/Distribution/css/fenxiao.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            background-color: #f5f5f5;
        }
        #divNext
        {
            text-align: center;
            margin-top: 10px;
            margin-bottom: 50px;
        }
        body
        {
            width: 500px;
        }
        .topbar
        {
            width: 500px;
            position: relative;
        }
        .title
        {
            text-align: center;
        }
        .top50
        {
            padding-top: 20px;
        }
    </style>
</head>
<body>
    <div class="title">
        <img src="<%=userInfo.WXHeadimgurlLocal%>" width="80" height="80" />
        <h5>
            <%--用户名:<%=userInfo.UserID%>&nbsp;--%>
            姓名:<%=userInfo.TrueName%>&nbsp;微信昵称:<%=userInfo.WXNickname%></h5>
    </div>
    <div class="topbar">
        <a class="col-xs-4 categorylink current" data-orderlevel="1">会员订单</a> 
        <a class="col-xs-4 categorylink " data-orderlevel="0">Ta的订单</a> 
<%--        <a class="col-xs-4 categorylink"
            data-orderlevel="2">二级订单</a> 
        <a class="col-xs-4 categorylink" data-orderlevel="3">三级订单</a>--%>
    </div>
    <div class="orderbox bottom50 top50" id="objlist">
        <div id="divNoData" style="display: none;">
            没有数据</div>
        <div id="divNext">
            <button class="btn_main" id="btnNext" onclick="BtnNext()">
                查看更多</button>
        </div>
    </div>
</body>
<script src="../Cation/Wap/Mall/Distribution/js/jquery-1.9.1.min.js" type="text/javascript"></script>
<script src="../../Scripts/StringBuilder.js" type="text/javascript"></script>
<script src="../Cation/Wap/Mall/Distribution/js/quo.js" type="text/javascript"></script>
<script src="../Cation/Wap/Mall/Distribution/js/comm.js" type="text/javascript"></script>
<script type="text/javascript">
    var pageIndex = 1;
    var pageSize = 10;
    var orderLevel = "1";
    $(function () {
        $(".topbar>a").click(function () {
            $(".topbar>a").removeClass("current");
            $(this).addClass("current");
            orderLevel = $(this).data("orderlevel");
            ResetSearchCondtion();
            LoadData();

        })
        LoadData();

    })


    //
    //加载数据
    function LoadData() {
        var ajaxData = {
            Action: 'QueryDistributionOrder',
            Level: orderLevel,
            Uid: GetParm("uid"),
            pageIndex: pageIndex,
            pageSize: pageSize
        }
        $("#btnNext").text("加载中...");
        $.ajax({
            type: 'post',
            url: "/Handler/App/CationHandler.ashx",
            data: ajaxData,
            timeout: 60000,
            dataType: "json",
            success: function (resp) {
                $("#btnNext").text("查看更多");
                var str = new StringBuilder();
                for (var i = 0; i < resp.length; i++) {
                    str.AppendFormat('<a href="javascript:void(0)" class="order">');
                    str.AppendFormat('<div class="orderdata">订单号:{0}会员:{1}日期:{2}</div>', resp[i].OrderID, resp[i].TrueName, resp[i].InsertDate);
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
                    if (resp[i].OrderType == 0) {
                        str.AppendFormat('共计<span class="totalnum">{0}</span>件商品,<span class="totalprice">{1}</span><span class="orangetext">元</span><span class="liststatus">{2}</span>', resp[i].ProductCount, resp[i].TotalAmount, ConvertDistributionStatus(resp[i].DistributionStatus));

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

    function BtnNext() {
        pageIndex++;
        LoadData();

    }

    function ResetSearchCondtion() {
        pageIndex = 1;
        $("#objlist>a").remove();
        $("#btnNext").attr("onclick", "BtnNext()");

    }

    //转换分销订单状态
    function ConvertDistributionStatus(status) {

        switch (status) {
            case -1:
                return "退款";
                break;
            case 0:
                return "未付款";
                break;
            case 1:
                return "已付款";
                break;
            case 2:
                return "已收货";
                break;
            case 3:
                return "已审核";
                break;

            default:

        }


    }

</script>
</html>
