<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXMallStoreStatistics.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallStoreStatistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body > div:nth-child(1) {
            border: 0;
        }

        .sort {
            display: none;
        }

        .pageSubTitle {
            padding: 10px 16px;
            background-color: #f9f8f8;
            margin: 0 16px;
        }

        .mainText {
            display: inline-block;
            margin: 0 12px 0 0;
            padding: 0 0 0 10px;
            border-left: 4px solid #f70;
            font-size: 14px;
            font-weight: bold;
            line-height: 20px;
        }

        .pageProTitle {
            padding: 10px 16px;
            background-color: #f9f8f8;
            margin: 20px 16px;
        }

        .pageTopBtnBg {
            background-color: #ffffff;
        }
        .pageTopBtnBg-sale{
            background-color: #ffffff;
            text-align:center;
        }

        .wrapGrvData {
            margin: 0 16px;
            border-right: 1px solid #ddd;
        }

        .warp-table {
            margin: 20px 16px;
            text-align: center;
        }

        .table {
            width: 50%;
            text-align: left;
            border-collapse: collapse;
            margin: auto;
        }

            .table thead tr th {
                border-bottom: 2px solid #ddd;
                padding-bottom: 10px;
                font: inherit;
                font-size: 14px;
            }

            .table tbody tr td {
                border-bottom: 1px solid #ddd;
                padding: 8px;
                font: inherit;
                font-size: 14px;
            }

        .pname {
            text-decoration: none;
            cursor: pointer;
            line-height: 20px;
        }
         .lbTip {    
            padding: 1px 5px;
            background-color: #5C5566;
            color: #fff;
            font-size: 14px;
            border-radius: 50px;
            cursor: pointer;
            margin-left: 2px;
        }

        .layui-layer-tips .layui-layer-content {
            background-color: #5C5566 !important;
            border-bottom-color: #5C5566 !important;
        }

        .layui-layer-tips i.layui-layer-TipsL, .layui-layer-tips i.layui-layer-TipsR {
            border-bottom-color: #5C5566 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <%--当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>商城统计</span>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <%-- 图表--%>
    <div style="text-align: center; font-size: 20px; padding: 10px;">
        <span class="">商城统计</span>
    </div>

    <div class="pageSubTitle">
        <span class="mainText">图表统计</span>

        <span style="color: #999;">（ 转化率=成交笔数/访客人数、月累计=每月从1号开始 成交金额累计、客单价=成交金额/成交笔数 ）</span>

    </div>

    <div style="padding-top: 20px; padding-left: 20px; font-weight: 200;">
        <input type="radio" id="zhu" value="zhu" name="rdoChart" checked="checked" class="positionTop2" /><label for="zhu">柱状图</label>
        <input type="radio" id="xian" value="xian" name="rdoChart" class="positionTop2" /><label for="xian">折线图</label>
    </div>
    <div id="main" style="width: 98%; height: 400px; margin: 0 auto; margin-top: 20px;"></div>

    <div class="pageSubTitle"><span class="mainText">详细数据</span></div>
    <div id="toolbar" class="pageTopBtnBg" style="padding: 10px 48px; height: auto;">
        <div style="margin-bottom: 5px">
            <span style="font-size: 12px; font-weight: normal">时间段：</span>
            <input class="easyui-datebox" id="sDate" />&nbsp;至
                <input class="easyui-datebox" id="eDate" />
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            <a href="javascript:;" class="easyui-linkbutton"
                iconcls="icon-edit" plain="true" onclick="Export();" id="btnEdit">导出数据</a>
        </div>
    </div>
    <div class="wrapGrvData">
        <table id="grvData">
        </table>
    </div>

    <div class="pageProTitle"><span class="mainText">商品销量排行榜</span></div>

    <div class="pageTopBtnBg-sale" style="padding: 10px 48px; height: auto;">
        <span style="font-size: 12px; font-weight: normal">时间段：</span>
        <input class="easyui-datebox" id="start" />&nbsp;至
                <input class="easyui-datebox" id="stop" />
        <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" onclick="queryProductSale()">查询</a>
    </div>

    <div class="warp-table">
        <table class="table">
            <thead>
                <tr>
                    <th width="30%">排名</th>
                    <th width="40%">商品名称</th>
                    <th width="30%">销量</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/echarts/echarts.js"></script>
    <script type="text/javascript">
        var url = "/serv/api/admin/mall/Statistics/list.ashx";
        var chart_type = "bar";//bar柱状 line折线
        var start = '';
        var stop = '';


        var option = {

            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
            },

            legend: {
                data: ['成交笔数', '成交件数', '成交金额', '当日退款件数', '当日退款金额', '商城PV', '商城UV', '在线商品数', '转化率', '客单价', '商品平均单价', '月累计', '销售总额', '开票金额', '商户结算总额'],
                selected: {
                    //'成交件数': false,
                    //'成交金额': false
                },
            },
            grid: {
                left: '3%',
                right: '4%',
                containLabel: true
            },
            xAxis: [{
                type: 'category',
                data: [],
                min: 'dataMin',
                max: 'dataMax'
            }],
            yAxis: [
                {
                    type: 'value'
                }
            ],

            dataZoom: [
               {
                   type: 'inside',
                   start: 50,
                   end: 100
               },
               {
                   show: true,
                   type: 'slider',
                   y: '90%',
                   start: 50,
                   end: 100
               }
            ],
            series: [{

                name: '成交笔数',
                type: chart_type,
                sampling: 'average',
                data: [],
                itemStyle: {
                    normal: { color: '#ff425e' }
                }
            }, {
                name: '成交件数',
                type: chart_type,
                //stack: '广告',
                sampling: 'average',
                data: [],
                itemStyle: {
                    normal: { color: '#5ab1ef' }
                }

            },
                {
                    name: '成交金额',
                    type: chart_type,
                    //stack: '广告',
                    data: [],
                    itemStyle: {
                        normal: { color: '#f8931d' }
                    }
                },
                {
                    name: '当日退款件数',
                    type: chart_type,
                    //stack: '广告',
                    data: []
                },
                {
                    name: '当日退款金额',
                    type: chart_type,
                    data: [],
                    //markLine: {
                    //    lineStyle: {
                    //        normal: {
                    //            type: 'dashed'
                    //        }
                    //    },
                    //    data: [
                    //        [{ type: 'min' }, { type: 'max' }]
                    //    ]
                    //},
                    itemStyle: {
                        normal: { color: '#5ca7ba' }
                    }
                },
                {
                    name: '商城PV',
                    type: chart_type,
                    barWidth: 5,
                    //stack: '搜索引擎',
                    data: [],
                    itemStyle: {
                        normal: { color: '#aed7ed' }
                    }
                },
                {
                    name: '商城UV',
                    type: chart_type,
                    //stack: '搜索引擎',
                    data: [],
                    itemStyle: {
                        normal: { color: '#c6eed8' }
                    }
                },
                {
                    name: '在线商品数',
                    type: chart_type,
                    //stack: '搜索引擎',
                    data: []
                },
                {
                    name: '转化率',
                    type: chart_type,
                    //stack: '搜索引擎',
                    data: []
                },
                {
                    name: '客单价',
                    type: chart_type,
                    //stack: '搜索引擎',
                    data: []
                }, {
                    name: '商品平均单价',
                    type: chart_type,
                    //stack: '搜索引擎',
                    data: [],
                    itemStyle: {
                        normal: { color: '#6cd7d9' }
                    }
                },
                {
                    name: '月累计',
                    type: chart_type,
                    //stack: '搜索引擎',
                    data: []
                },
                {
                    name: '销售总额',
                    type: chart_type,
                    //stack: '搜索引擎',
                    data: []
                },
                {
                    name: '开票金额',
                    type: chart_type,
                    //stack: '搜索引擎',
                    data: []
                },
                {
                    name: '商户结算总额',
                    type: chart_type,
                    //stack: '搜索引擎',
                    data: []
                },


            ]
        };

        $(function () {



            var myChart = echarts.init(document.getElementById('main'));

            $.ajax({
                type: "post",
                url: '/serv/api/admin/mall/Statistics/chart.ashx',
                dataType: "json",
                success: function (data) {
                    $.each(data.result, function () {
                        option.xAxis[0].data.push(this.Date);
                        option.series[0].data.push(this.OrderCount);
                        option.series[1].data.push(this.OrderProuductTotalCount);
                        option.series[2].data.push(this.OrderTotalAmount);
                        option.series[3].data.push(this.RefundProductTotalCount);
                        option.series[4].data.push(this.RefundTotalAmount);
                        option.series[5].data.push(this.PV);
                        option.series[6].data.push(this.UV);
                        option.series[7].data.push(this.ProductTotalCount);
                        option.series[8].data.push(parseFloat(this.ConvertRate));
                        option.series[9].data.push(this.PerCustomerTransaction);
                        option.series[10].data.push(this.ProcuctAveragePrice);
                        option.series[11].data.push(this.OrderTotalAmountMonth);
                        option.series[12].data.push(this.TotalSales);
                        option.series[13].data.push(this.InvoiceAmount);
                        option.series[14].data.push(this.MerchantSettlemenTotalAmount);

                    });
                    myChart.setOption(option);
                }
            });

            $("input[name=rdoChart]").click(function () {
                var val = $(this).val();
                if (val == 'xian') {
                    chart_type = 'line';
                    option.series[0].type = chart_type;
                    option.series[1].type = chart_type;
                    option.series[2].type = chart_type;
                    option.series[3].type = chart_type;
                    option.series[4].type = chart_type;
                    option.series[5].type = chart_type;
                    option.series[6].type = chart_type;
                    option.series[7].type = chart_type;
                    option.series[8].type = chart_type;
                    option.series[9].type = chart_type;
                    option.series[10].type = chart_type;
                    option.series[11].type = chart_type;
                    option.series[12].type = chart_type;
                    option.series[13].type = chart_type;
                    option.series[14].type = chart_type;

                } else {
                    chart_type = 'bar';
                    option.series[0].type = chart_type;
                    option.series[1].type = chart_type;
                    option.series[2].type = chart_type;
                    option.series[3].type = chart_type;
                    option.series[4].type = chart_type;
                    option.series[5].type = chart_type;
                    option.series[6].type = chart_type;
                    option.series[7].type = chart_type;
                    option.series[8].type = chart_type;
                    option.series[9].type = chart_type;
                    option.series[10].type = chart_type;
                    option.series[11].type = chart_type;
                    option.series[12].type = chart_type;
                    option.series[13].type = chart_type;
                    option.series[14].type = chart_type;
                }

                myChart.setOption(option);
            });





            
            $(document).on('click', '.lbTip', function () {
                var msg = $(this).attr('data-tip-msg');
                layer.tips(msg, $(this));
            });






            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: url,
                       //height: document.documentElement.clientHeight - 120,
                       pagination: true,
                       striped: true,
                       rownumbers: true,
                       singleSelect: false,
                       rowStyler: function () { return 'height:25px'; },
                       columns: [[
                                   { field: 'date', title: '日期', width: 40, align: 'left', formatter: FormatterTitle },
                                   { field: 'order_count', title: '成交笔数<span class="lbTip" data-tip-msg="<b>说明</b><br>成交笔数=当天已付款的订单数<br>">?</span>', width: 80, align: 'left' },
                                   { field: 'order_prouduct_count', title: '成交件数<span class="lbTip" data-tip-msg="<b>说明</b><br>成交件数=当天的成交件数<br>">?</span>', width: 80, align: 'left' },
                                   { field: 'order_totalamount', title: '成交金额<span class="lbTip" data-tip-msg="<b>说明</b><br>成交金额=当天的实付金额<br>">?</span>', width: 80, align: 'left' },
                                   { field: 'refund_product_totalcount', title: '当日退货件数<span class="lbTip" data-tip-msg="<b>说明</b><br>当日退货件数=当日退货总件数<br>">?</span>', width: 100, align: 'left' },
                                   { field: 'refund_totalamount', title: '当日退货金额<span class="lbTip" data-tip-msg="<b>说明</b><br>当日退货金额=当日退货总金额<br>">?</span>', width: 100, align: 'left' },
                                   { field: 'pv', title: '商城PV(浏览量)<span class="lbTip" data-tip-msg="<b>说明</b><br>商城PV=商城访问量(1个用户访问N次就有N个访问量)<br>">?</span>', width: 110, align: 'left' },
                                   { field: 'uv', title: '商城UV(访客数)<span class="lbTip" data-tip-msg="<b>说明</b><br>商城UV=商城访客量(1个用户访问N次也只算1个访客量)<br>">?</span>', width: 110, align: 'left' },
                                   { field: 'product_totalcount', title: '在线商品数<span class="lbTip" data-tip-msg="<b>说明</b><br>在线商品数=上架商品数量<br>">?</span>', width: 100, align: 'left' },
                                   { field: 'convertrate', title: '转化率<span class="lbTip" data-tip-msg="<b>说明</b><br>转化率=成交笔数/商城UV(访客数)<br>">?</span>', width: 70, align: 'left' },
                                   { field: 'per_customer_transaction', title: '客单价<span class="lbTip" data-tip-msg="<b>说明</b><br>客单价=成交金额/成交笔数<br>">?</span>', width: 70, align: 'left' },
                                   { field: 'procuct_average_price', title: '商品平均单价<span class="lbTip" data-tip-msg="<b>说明</b><br>商品平均单价=SKU价格的平均价格<br><br>">?</span>', width: 110, align: 'left' },
                                   { field: 'order_totalamount_month', title: '月累计<span class="lbTip" data-tip-msg="<b>说明</b><br>月累计=当前月的实付金额<br>">?</span>', width: 80, align: 'left' },
                                   { field: 'total_sales', title: '销售总额<span class="lbTip" data-tip-msg="<b>说明</b><br>销售总额=实付金额+积分抵扣+优惠券抵扣+余额支付+运费<br>">?</span>', width: 80, align: 'left' },
                                   { field: 'invoice_amount', title: '开票金额<span class="lbTip" data-tip-msg="<b>说明</b><br>开票金额=货品销售总额（货品销售单价*数量）-结算总额（结算基价*数量）<br>">?</span>', width: 80, align: 'left' },
                                   { field: 'merchant_settlemen_total_amount', title: '商户结算总额<span class="lbTip" data-tip-msg="<b>说明</b><br>商户结算总额=结算基价*数量+代收快递费（实际就是下单里面的那个快递费）<br>">?</span>', width: 100, align: 'left' }
                       ]]
                   }
               );

            $(".datebox :text").attr("readonly", "readonly");
            //搜索
            $("#btnSearch").click(function () {
                var start = $("#sDate").datebox('getValue');
                var end = $("#eDate").datebox('getValue');
                $('#grvData').datagrid({ url: url, queryParams: { start_date: start, stop_date: end } });
            });

            //商品销量排行
            loadSaleSort();
        });


        //导出
        function Export() {
            $.messager.confirm('系统提示', '确认导出当前数据到文件?', function (o) {
                if (o) {
                    var start = $("#sDate").datebox('getValue');
                    var end = $("#eDate").datebox('getValue');
                    var zurl = "/serv/api/admin/mall/Statistics/export.ashx?start_date=" + start + "&&end_date=" + end + "";
                    window.open(zurl);
                }
            });
        }

        function loadSaleSort() {
            $.ajax({
                type: "post",
                url: '/serv/api/admin/mall/order.ashx',
                data: { action: 'SaleCountSort', start_date: start, stop_date: stop },
                dataType: "json",
                success: function (resp) {
                    $(".table tbody tr").remove();
                    for (var i = 0; i < resp.length; i++) {
                        var tr = $("<tr></tr>");
                        var td1 = $("<td style='width:30%'>" + (i + 1) + "</td>");
                        var td2 = $("<td style='width:40%;'><a class='pname'>" + resp[i].product_name + "</a></td>");
                        //debugger;
                        var td3 = $("<td style='width:30%'>" + resp[i].sale_count + "</td>");
                        tr.append(td1).append(td2).append(td3).appendTo(".table tbody");
                    }
                }
            });
        }
        function queryProductSale() {
            start = $('#start').datebox('getValue');
            stop = $('#stop').datebox('getValue');
            loadSaleSort();
        }
    </script>
</asp:Content>
