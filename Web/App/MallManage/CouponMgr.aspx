<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="CouponMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.CouponMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #txtDiscount
        {
            width: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&gt&nbsp;<span>优惠券管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd()">添加优惠券</a>
            <br />
            <label>
                用户:</label>
            <input type="text" style="width: 200px" id="txtUserId" />
            <label>
                优惠券号码:</label>
            <input type="text" style="width: 200px" id="txtCouponNumber" />
            <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">
                查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加优惠券" style="width: 300px;
        padding: 15px; line-height: 30px;">
        <table width="100%">
            <tr>
                <td style="width: 60px;">
                    商品编号:
                </td>
                <td>
                    <input type="text" id="txtProductId" />
                </td>
            </tr>
           
            <tr>
                <td style="width: 50px;">
                    折扣:
                </td>
                <td>
                    <input type="text" id="txtDiscount" />&nbsp;折
                </td>
            </tr>
            <tr>
                <td style="width: 50px;">
                    生效日期:
                </td>
                <td>
                    <input type="text" id="txtStartDate" readonly="readonly" class="easyui-datebox" />
                </td>
            </tr>
            <tr>
                <td style="width: 50px;">
                    失效日期:
                </td>
                <td>
                    <input type="text" id="txtStopDate" readonly="readonly" class="easyui-datebox" />
                </td>
            </tr>
             <tr><td colspan="2">提示,如果商品编号留空,则表示对所有商品打折<br />生效日期与失效日期选填</td></tr>
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
	                queryParams: { Action: "QueryCoupon" },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'CouponNumber', title: '优惠券号码', width: 20, align: 'left' },
                                { field: 'Discount', title: '折扣', width: 20, align: 'left', formatter: function (value, rowData) {
                                    return value + "折"
                                }
                            },
                                { field: 'StartDate', title: '生效日期', width: 20, align: 'left' },
                                { field: 'StopDate', title: '失效日期', width: 20, align: 'left' },
                                { field: 'CreateUserId', title: '创建用户', width: 20, align: 'left' },
                                { field: 'InsertDate', title: '时间', width: 20, align: 'left', formatter: FormatDate }
                             ]]
	            }
            );
            //搜索
            $("#btnSearch").click(function () {
                var userId = $("#txtUserId").val();
                var couponNumber = $("#txtCouponNumber").val();
                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryCoupon", UserId: userId, CouponNumber: couponNumber} });
            });

            //添加优惠券弹框
            $('#dlgInput').dialog({
                buttons: [{
                    text: '添加优惠券',
                    handler: function () {
                        try {
                            var dataModel = {
                                Action: "AddCoupon",
                                ProductId: $.trim($('#txtProductId').val()),
                                Discount: $.trim($('#txtDiscount').val()),
                                StartDate: $("#txtStartDate").datebox('getValue'),
                                StopDate: $("#txtStopDate").datebox('getValue')
                            }

                            if (dataModel.Discount == '') {

                                Alert('请输入折扣');
                                return;
                            }
                            if (isNaN(dataModel.Discount)) {
                                Alert('请输入正确的折扣');
                                return;
                            }
                            if (parseFloat(dataModel.Discount <= 0) || (parseFloat(dataModel.Discount) >= 10)) {
                                Alert('折扣在0到10折之间');
                                return;

                            }
                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.Status == 1) {
                                        Show("添加优惠券成功");
                                        $('#dlgInput').dialog('close');
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
                        $('#dlgInput').dialog('close');
                    }
                }]
            });

        });



        //        function Delete() {
        //            try {

        //                var rows = $('#grvData').datagrid('getSelections');

        //                if (!EGCheckIsSelect(rows))
        //                    return;

        //                $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
        //                    if (r) {
        //                        var ids = [];

        //                        for (var i = 0; i < rows.length; i++) {
        //                            ids.push(rows[i].AutoID);
        //                        }

        //                        var dataModel = {
        //                            Action: 'DeleteWXMallCategory',
        //                            ids: ids.join(',')
        //                        }

        //                        $.ajax({
        //                            type: 'post',
        //                            url: handlerUrl,
        //                            data: dataModel,
        //                            success: function (result) {
        //                                Alert(result);
        //                                $('#grvData').datagrid('reload');
        //                                LoadCategorySelectList();
        //                            }
        //                        });
        //                    }
        //                });

        //            } catch (e) {
        //                Alert(e);
        //            }
        //        }


        //添加优惠券 弹出框
        function ShowAdd() {
            $('#dlgInput').dialog({ title: '添加优惠券' });
            $('#dlgInput').dialog('open');
            $("#dlgInput input").val("");


        }
    


    </script>
</asp:Content>
