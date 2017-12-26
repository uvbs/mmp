<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="CouponMgrV2.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.CouponMgrV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #txtDiscount
        {
            width: 100px;
        }
        #trMallCardCoupon_Deductible, #trMallCardCoupon_Buckle, #trMallCardCoupon_FreeFreight
        {
            display: none;
        }
        #txtBuckleAmount, #txtBuckleSubAmount
        {
            width: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&gt&nbsp;<span>优惠券管理V2</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd()">添加优惠券</a>

                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowSend()">发放优惠券</a>

                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除优惠券</a>
            <br />
            <select id="ddlCouponTypeSearch">
                <option value="">全部</option>
                <option value="MallCardCoupon_Discount">折扣券：凭折扣券对指定商品（全场）打折</option>
                <option value="MallCardCoupon_Deductible">抵扣券：支付时可以抵扣现金</option>
                <option value="MallCardCoupon_FreeFreight">免邮券：满一定金额包邮</option>
                <option value="MallCardCoupon_Buckle">满扣券：消费满一定金额减去一定金额</option>
            </select>
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">
                查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

    <%--生成优惠券--%>
    <div id="dlgAddCoupon" class="easyui-dialog" closed="true" title="添加优惠券" style="width: 400px;
        padding: 15px; line-height: 25px;">
        <table width="100%">
            <tr>
                <td style="width: 80px;">
                    优惠券类型:
                </td>
                <td>
                    <select id="ddlCouponType">
                        <option value="MallCardCoupon_Discount">折扣券：凭折扣券对指定商品（全场）打折</option>
                        <option value="MallCardCoupon_Deductible">抵扣券：支付时可以抵扣现金</option>
                        <option value="MallCardCoupon_FreeFreight">免邮券：满一定金额包邮</option>
                        <option value="MallCardCoupon_Buckle">满扣券：消费满一定金额减去一定金额</option>
                    </select>
                </td>
            </tr>
             <tr >
                <td style="width: 60px;">
                    卡券名称:
                </td>
                <td>
                    <input type="text" id="txtCardCouponName" />
                </td>
            </tr>
            <tr id="trMallCardCoupon_Discount_ProductId">
                <td style="width: 60px;">
                    商品编号:
                </td>
                <td>
                    <input type="text" id="txtProductId" />
                </td>
            </tr>
            <tr id="trMallCardCoupon_Discount">
                <td style="width: 50px;">
                    折扣:
                </td>
                <td>
                    <input type="text" id="txtDiscount" />&nbsp;折
                </td>
            </tr>
            <tr id="trMallCardCoupon_Deductible">
                <td style="width: 50px;">
                    抵扣金额:
                </td>
                <td>
                    可抵扣<input type="text" id="txtDeductibleAmount" />&nbsp;元
                </td>
            </tr>
            <tr id="trMallCardCoupon_FreeFreight">
                <td style="width: 50px;">
                    免邮券
                </td>
                <td>
                    满<input type="text" id="txtFreeFreightAmount" />&nbsp;元&nbsp;包邮
                </td>
            </tr>
            <tr id="trMallCardCoupon_Buckle">
                <td style="width: 50px;">
                    满扣券
                </td>
                <td>
                    满<input type="text" id="txtBuckleAmount" />&nbsp;元&nbsp;减<input type="text" id="txtBuckleSubAmount" />&nbsp;元
                </td>
            </tr>
            <tr>
                <td style="width: 50px;">
                    生效日期:
                </td>
                <td>
                    <input type="text" id="txtValidFrom" readonly="readonly" class="easyui-datebox" />
                </td>
            </tr>
            <tr>
                <td style="width: 50px;">
                    失效日期:
                </td>
                <td>
                    <input type="text" id="txtValidTo" readonly="readonly" class="easyui-datebox" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    提示,如果商品编号留空,则表示对所有商品打折<br />
                    生效日期与失效日期选填
                </td>
            </tr>
        </table>
    </div>

    <%--发放优惠券--%>
    <div id="dlgSendCoupon" class="easyui-dialog" closed="true" title="添加优惠券" style="width: 400px;
        padding: 15px; line-height: 25px;">
        <table width="100%">
            <tr>
                <td style="width: 80px;">
                    发放类型:
                </td>
                <td>
                    <select id="ddlSendType">
                        <option value="0">个人</option>
                       
                    </select>
                </td>
            </tr>
             <tr >
                <td style="width: 60px;">
                    用户名或手机号:
                </td>
                <td>
                    <input type="text" id="txtUserId" />
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currSelectID = 0;
        var currAction = '';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryCouponV2" },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'CardCouponType', title: '卡券类型', width: 20, align: 'left'
                                , formatter: function (value, rowData) {
                                    switch (value) {
                                        case "MallCardCoupon_Discount":
                                            return "折扣券";
                                            break;
                                        case "MallCardCoupon_Deductible":
                                            return "抵扣券";
                                            break;
                                        case "MallCardCoupon_FreeFreight":
                                            return "免邮券";
                                            break;
                                        case "MallCardCoupon_Buckle":
                                            return "满扣券";
                                            break;
                                        default:

                                    }


                                }

                            },
                                { field: 'CardId', title: '卡券编号', width: 20, align: 'left' },
                                { field: 'Name', title: '卡券名称', width: 20, align: 'left' },
                                { field: 'Ex1', title: '【折扣券】折扣', width: 20, align: 'left', formatter: function (value, rowData) {
                                    if (value == "") {
                                        return "";
                                    }
                                    return value + "折"
                                }
                                },
                            { field: 'Ex3', title: '【抵扣券】抵扣金额', width: 20, align: 'left' },
                            { field: 'Ex4', title: '【免邮券】满多少金额', width: 20, align: 'left' },
                            { field: 'Ex5', title: '【满扣券】', width: 20, align: 'left', formatter: function (value, rowData) {

                                if (rowData.CardCouponType == "MallCardCoupon_Buckle") {
                                    return "满" + rowData.Ex5 + "抵" + rowData.Ex6 + "元";
                                }

                            }
                            },

                             { field: 'ValidFrom', title: '生效日期', width: 20, align: 'left', formatter: FormatDate },
                                { field: 'ValidTo', title: '失效日期', width: 20, align: 'left', formatter: FormatDate },
                                { field: 'Op', title: '操作', width: 20, align: 'left', formatter: function (value, rowData) {

                                    return "<a href=\"\">发放记录</a>";

                                }
                                },



                             ]]
	            }
            );
            //搜索
            $("#btnSearch").click(function () {

                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryCouponV2", cardCouponType: $(ddlCouponTypeSearch).val()} });
            });

            //添加优惠券弹框
            $('#dlgAddCoupon').dialog({
                buttons: [{
                    text: '添加优惠券',
                    handler: function () {
                        try {
                            var dataModel = {
                                Action: "AddCouponV2",
                                CardCouponType: $(ddlCouponType).val(),
                                CardCouponName: $(txtCardCouponName).val(),
                                ProductId: $.trim($('#txtProductId').val()),
                                Discount: $.trim($('#txtDiscount').val()),
                                DeductibleAmount: $(txtDeductibleAmount).val(),
                                FreeFreightAmount: $(txtFreeFreightAmount).val(),
                                BuckleAmount: $(txtBuckleAmount).val(),
                                BuckleSubAmount: $(txtBuckleSubAmount).val(),
                                ValidFrom: $("#txtValidFrom").datebox('getValue'),
                                ValidTo: $("#txtValidTo").datebox('getValue')
                            }

                            if (dataModel.CardCouponName == '') {

                                Alert('请输入卡券名称');
                                return;
                            }


                            //                            if (dataModel.Discount == '') {

                            //                                Alert('请输入折扣');
                            //                                return;
                            //                            }
                            //                            if (isNaN(dataModel.Discount)) {
                            //                                Alert('请输入正确的折扣');
                            //                                return;
                            //                            }
                            //                            if (parseFloat(dataModel.Discount <= 0) || (parseFloat(dataModel.Discount) >= 10)) {
                            //                                Alert('折扣在0到10折之间');
                            //                                return;

                            //                            }


                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.Status == 1) {
                                        Show("添加优惠券成功");
                                        $('#dlgAddCoupon').dialog('close');
                                        $('#grvData').datagrid('reload');

                                    }
                                    else {
                                        Alert("添加优惠券失败");
                                    }


                                }
                            });

                        } catch (e) {
                            Alert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgAddCoupon').dialog('close');
                    }
                }]
            });


            //发放优惠券弹框
            $('#dlgSendCoupon').dialog({
                buttons: [{
                    text: '发放优惠券',
                    handler: function () {
                        try {
                            var dataModel = {
                                Action: "SendCouponV2",
                                CardCouponId: $('#grvData').datagrid('getSelections')[0].CardId,
                                SendType: $(ddlSendType).val(),
                                UserId: $(txtUserId).val()
                            }
                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.Status == 1) {
                                        Show("发放优惠券成功");
                                        $('#dlgSendCoupon').dialog('close');
                                        $('#grvData').datagrid('reload');

                                    }
                                    else {
                                        Alert(resp.Msg);
                                    }


                                }
                            });

                        } catch (e) {
                            Alert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgSendCoupon').dialog('close');
                    }
                }]
            });

            $("#ddlCouponType").change(function () {

                ShowHideTr($(this).val());



            })

        });

        //添加优惠券 弹出框
        function ShowAdd() {
            $('#dlgAddCoupon').dialog({ title: '添加优惠券' });
            $('#dlgAddCoupon').dialog('open');
            $("#dlgAddCoupon input").val("");


        }

        //发放优惠券 弹出框
        function ShowSend() {

            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;

            $('#dlgSendCoupon').dialog({ title: '添加优惠券' });
            $('#dlgSendCoupon').dialog('open');
            $("#dlgSendCoupon input").val("");

        }


        //显示或隐藏对应的TR
        function ShowHideTr(couponType) {

            $(trMallCardCoupon_Buckle).hide();
            $(trMallCardCoupon_Deductible).hide();
            $(trMallCardCoupon_Discount).hide();
            $(trMallCardCoupon_Discount_ProductId).hide();
            $(trMallCardCoupon_FreeFreight).hide();

            switch (couponType) {
                case "MallCardCoupon_Discount":
                    $(trMallCardCoupon_Discount).show();
                    $(trMallCardCoupon_Discount_ProductId).show();
                    break;
                case "MallCardCoupon_Deductible":
                    $(trMallCardCoupon_Deductible).show();
                    break;
                case "MallCardCoupon_FreeFreight":
                    $(trMallCardCoupon_FreeFreight).show();
                    break;
                case "MallCardCoupon_Buckle":
                    $(trMallCardCoupon_Buckle).show();
                    break;
                default:

            }



        }

        //删除优惠券
        function Delete() {
            try {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].CardId);
                        }

                        var dataModel = {
                            Action: 'DeleteCouponV2',
                            ids: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            dataType:'json',
                            success: function (resp) {
                                if (resp.Status == 1) {
                                    Show("删除成功");
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert("删除失败");
                                }


                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }

    </script>
</asp:Content>
