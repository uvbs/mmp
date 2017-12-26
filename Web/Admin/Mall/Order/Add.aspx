<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Mall.Order.Add" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body {
            font-family: 微软雅黑;
            background-color: white !important;
        }

        .tdTitle {
            font-weight: bold;
        }

        .title {
            font-size: 12px;
        }

        input, select {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }


        .items {
            border: 1px solid;
            border-radius: 5px;
            border-color: #CCCCCC;
            margin-top: 10px;
            width: 98%;
            position: relative;
        }

            .items input {
                height: 30px;
                border: 1px solid #d5d5d5;
                border-radius: 5px;
                background-color: #fefefe;
            }

        .product-title {
            width: 80px;
        }

        .delete-item {
            float: right;
            right: 5px;
            margin-top: -5px;
            cursor: pointer;
        }

        .items input[type=text] {
            width: 90%;
        }


        .product-price, .product-count {
            color: red;
        }

        .div-save {
            border-top: 1px solid #DDDDDD;
            position: fixed;
            bottom: 0px;
            height: 52px;
            line-height: 55px;
            text-align: center;
            width: 100%;
            background-color: rgb(245, 245, 245);
            padding-top: 10px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">
        当前位置：&nbsp;&nbsp;&nbsp;订单>&nbsp;&gt;&nbsp;&nbsp;<span>手动添加</span> 
        
        <a title="返回管理" style="display:none;float: right; margin-right: 20px;" href="" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%;">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">订单来源：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlOrderSource">
                           <option value="Eleme">饿了么</option>
                           <option value="Meituan">美团外卖</option>
                            <option value="Baidu">百度外卖</option>
                            <option value="DazhongDianPing">大众点评</option>
                            <option value="Comeoncloud">至云商城</option>
                            <%foreach (var item in SupplierList){%>
                             <option value="<%=item.UserID %>"><%=item.Company %></option>
                            <% } %>
                       </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">订单编号：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtOutOrderId" maxlength="100" value="" style="width: 100%;" placeholder="外部订单号(必填)" />
                    </td>
                </tr>
                
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">订单状态：
                    </td>
                    <td width="*" align="left">
                       <select id="ddlOrderStatus">
                           <option value="待付款">待付款</option>
                           <option value="待发货">待发货</option>
                           <option value="已发货">已发货</option>
                           <option value="交易成功">交易成功</option>
                           <option value="已取消">已取消</option>
                       </select>
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">下单时间：
                    </td>
                    <td width="*" align="left">
                        <input class="easyui-datetimebox" style="width: 150px;" editable="false" id="txtInsertDate" />
                    </td>
                </tr>

                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">付款方式：
                    </td>
                    <td width="*" align="left">
                       <select id="ddlPaymentType">
                           <option value="2">微信</option>
                           <option value="1">支付宝</option>
                       </select>
                    </td>
                </tr>
                
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">付款状态：
                    </td>
                    <td width="*" align="left">
                         <select id="ddlPaymentStatus">
                           <option value="0">未支付</option>
                           <option value="1">已支付</option>
                       </select>
                    </td>
                </tr>

                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">付款时间：
                    </td>
                    <td width="*" align="left">
                         <input class="easyui-datetimebox" style="width: 150px;" editable="false" id="txtPayTime" />
                    </td>
                </tr>

                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">实付：
                    </td>
                    <td width="*" align="left">
                        <input type="number" id="txtTotalAmount" maxlength="100" value=""  placeholder="实付(必填)" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">使用积分：
                    </td>
                    <td width="*" align="left">
                        <input type="number" id="txtUseScore" maxlength="100" value="0"  placeholder="使用积分" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">使用余额：
                    </td>
                    <td width="*" align="left">
                         <input type="number" id="txtUseAccountAmount" maxlength="100" value="0"  placeholder="使用余额" />
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">使用优惠券：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlCardcoupon">
                            <option value="">无</option>
                            <%foreach (var item in CardCouponList){%>
                             <option value="<%=item.Name %>"><%=item.Name %></option>
                             <% } %>
                           
                       </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">运费：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtFreight" maxlength="100" value="0"  placeholder="运费" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">配送方式：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlDeveliveType">
                           <option>快递</option>
                           <option>无需物流</option>
                       </select>
                    </td>
                </tr>
                                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">收货信息：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtAddress" maxlength="100" value="" style="width: 100%;" placeholder="收货信息" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">收货人姓名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtConsignee" maxlength="100" value=""  placeholder="收货人姓名" />
                    </td>
                </tr>
                   <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">收货人手机：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtPhone" maxlength="100" value=""  placeholder="收货人手机" />
                    </td>
                </tr>

                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">下单人姓名：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtBuyName" maxlength="100" value=""  placeholder="下单人姓名" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">下单人手机：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtBuyPhone" maxlength="100" value=""  placeholder="下单人手机" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">买家留言：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtRemark" maxlength="100" value="" style="width: 100%;" placeholder="买家留言" />
                    </td>
                </tr>
            </table>

            <div>
                <strong style="font-size: 20px;">商品列表:</strong>
                <div class="items" data-item-index="0" data-item-id="">
                    <img src="/img/delete.png" class="delete-item" />
                    <table style="width: 100%; margin-left: 10px;">
                        <tr>
                            <td class="product-title">商品名称:</td>
                            <td>
                                <input type="text" class="product-name" placeholder="请输入商品名称" />
                            </td>
                        </tr>
                        <tr>
                            <td class="product-title">商品单价:
                            </td>
                            <td>
                                <input type="number" class="product-price" placeholder="请输入商品单价" />
                            </td>

                        </tr>
                        <tr>
                            <td class="product-title">数量:
                            </td>
                            <td>
                                <input type="number" class="product-count" placeholder="请输入商品数量" />
                            </td>
                        </tr>
                    </table>
                </div>
                <a class="button button-rounded button-primary" style="width: 90%; margin-top: 10px; margin-bottom: 10px;" id="btnAddItem">添加商品</a>
            </div>


            <div class="div-save">
                <a href="javascript:;" id="btnSave" class="button button-rounded button-primary" style="width: 200px;">添加</a>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/json2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var handlerUrl = "/serv/api/admin/mall/order.ashx";

        $(function () {

            //删除
            $('.delete-item').live("click", function () {

                if (confirm("确定要删除?")) {

                    $(this).parent().remove();
                }


            });
            //删除

            //添加商品
            $("#btnAddItem").click(function () {

                var appendhtml = new StringBuilder();
                appendhtml.AppendFormat('<div class="items" data-item-index="0" data-item-id="">');
                appendhtml.AppendFormat('<img src="/img/delete.png" class="delete-item" />');
                appendhtml.AppendFormat('<table style="width: 100%; margin-left: 10px;">');
                appendhtml.AppendFormat('<tr>');
                appendhtml.AppendFormat(' <td class="product-title">商品名称:</td>');
                appendhtml.AppendFormat('<td>');
                appendhtml.AppendFormat('<input type="text" class="product-name" placeholder="请输入商品名称" />');
                appendhtml.AppendFormat(' </td>');
                appendhtml.AppendFormat('</tr>');
                appendhtml.AppendFormat('<tr>');
                appendhtml.AppendFormat('<td class="product-title">商品单价:');
                appendhtml.AppendFormat('</td>');
                appendhtml.AppendFormat('<td><input type="number" class="product-price" placeholder="请输入商品单价" />');
                appendhtml.AppendFormat('</td>');

                appendhtml.AppendFormat('</tr>');
                appendhtml.AppendFormat('<tr>');
                appendhtml.AppendFormat('<td class="product-title">数量:');
                appendhtml.AppendFormat('</td>');
                appendhtml.AppendFormat('<td><input type="number" class="product-count" placeholder="请输入商品数量"/>');
                appendhtml.AppendFormat('</td>');
                appendhtml.AppendFormat('</tr>');
                appendhtml.AppendFormat('</table>');
                appendhtml.AppendFormat('</div>');
                $(this).before(appendhtml.ToString());


            });
            //添加商品


            //确定添加
            $('#btnSave').click(function () {

                if (confirm("确定添加?")) {

                    try {
                        var model = getModel();
                        
                        //order_source: $("#ddlOrderSource").val(),
                        //out_order_id: $("#txtOutOrderId").val(),
                        //insert_date: $("#txtInsertDate").datetimebox('getValue'),
                        //order_status: $("#ddlOrderStatus").val(),
                        //pay_type: $("#ddlPaymentType").val(),
                        //pay_status: $("#ddlPaymentStatus").val(),
                        //pay_time: $("#txtPayTime").datetimebox('getValue'),
                        //total_amount: $("#txtTotalAmount").val(),
                        //use_score: $("#txtUseScore").val(),
                        //use_amount: $("#txtUseAccountAmount").val(),
                        //card_coupon_name: $("#ddlCardcoupon").val(),
                        //develive_type: $("#ddlDeveliveType").val(),
                        //receiver_address: $("#txtAddress").val(),
                        //receiver_name: $("#txtConsignee").val(),
                        //receiver_phone: $("#txtPhone").val(),
                        //buy_name: $("#txtBuyName").val(),
                        //buy_phone: $("#txtBuyPhone").val(),
                        //buyer_memo: $("#txtRemark").val(),
                        //freight: $("#txtFreight").val(),



                        if (model.out_order_id == "") {
                            $("#txtOutOrderId").focus();
                            return false;
                        }
                        if (model.insert_date == "") {
                            Alert("请输入下单时间");
                            return false;
                        }
                        if (model.total_amount == "") {
                            $("#txtTotalAmount").focus();
                            return false;
                        }
                        
                        var jsonData = JSON.stringify(model);
                        $.messager.progress({ text: '正在添加...' });
                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: { action:"add",data: jsonData },
                            dataType: "json",
                            success: function (resp) {
                                $.messager.progress('close');
                                if (resp.status == true) {
                                    alert("添加成功");
                                    window.location.href = window.location.href;
                                }
                                else {
                                    Alert(resp.msg);
                                }
                            }
                        });

                    } catch (e) {
                        Alert(e);
                    }

                }



            });


        });

        //获取模型
        function getModel() {
            //模型
            var model = {
               
                order_source: $("#ddlOrderSource").val(),
                out_order_id: $("#txtOutOrderId").val(),
                insert_date: $("#txtInsertDate").datetimebox('getValue'),
                order_status: $("#ddlOrderStatus").val(),
                pay_type: $("#ddlPaymentType").val(),
                pay_status: $("#ddlPaymentStatus").val(),
                pay_time: $("#txtPayTime").datetimebox('getValue'),
                total_amount: $("#txtTotalAmount").val(),
                use_score: $("#txtUseScore").val(),
                use_amount: $("#txtUseAccountAmount").val(),
                card_coupon_name: $("#ddlCardcoupon").val(),
                develive_type: $("#ddlDeveliveType").val(),
                receiver_address: $("#txtAddress").val(),
                receiver_name: $("#txtConsignee").val(),
                receiver_phone: $("#txtPhone").val(),
                buy_name: $("#txtBuyName").val(),
                buy_phone: $("#txtBuyPhone").val(),
                buyer_memo: $("#txtRemark").val(),
                freight: $("#txtFreight").val(),
                product_list: []//商品列表
            }
            //中间内容
            $(".items").each(function () {

                var product = {
                    product_name: $(this).find(".product-name").first().val(),
                    product_price: $(this).find(".product-price").first().val(),
                    product_count: $(this).find(".product-count").first().val()

                };
                model.product_list.push(product);
            });
            return model;
        }


    </script>
</asp:Content>
