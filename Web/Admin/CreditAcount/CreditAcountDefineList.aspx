<%@ Page Title="" Language="C#" EnableSessionState="ReadOnly" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CreditAcountDefineList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.CreditAcount.CreditAcountDefineList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;其他管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;信用金规则列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnAddItem" onclick="AddItem()">新增规则</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnEditItem" onclick="EditItem()">修改规则</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: 400px;padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    类型:
                </td>
                <td>
                    <%=new ZentCloud.Common.MyCategoriesV2().GetSelectOptionHtml(new ZentCloud.BLLJIMP.BLLKeyValueData().GetKeyVauleDataInfoList("CreditAcountType", "0", "Common"), "DataKey", "PreKey", "DataValue", "0", "ddlType", "width:200px", "") %>
                </td>
            </tr>
            <tr>
                <td>
                    信用金:
                </td>
                <td>
                    <input id="txtCreditAcount" type="text" class="easyui-numberbox" style="width: 80px;" data-options="min:-1000,max:5000,precision:0" />最小-1000，最大5000
                </td>
            </tr>
            <tr>
                <td>
                    每日上限:
                </td>
                <td>
                    <input id="txtDayLimit" type="text" class="easyui-numberbox" style="width: 80px;" data-options="min:-1,max:500,precision:0" />小于0时不限制，最小-1，最大500
                </td>
            </tr>
            <tr>
                <td>
                    是否隐藏:
                </td>
                <td>
                    <select id="selectHide" style="width: 250px;">
                        <option value="0" selected="selected">显示</option>
                        <option value="1">隐藏</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    描述:
                </td>
                <td>
                    <input id="txtDescription" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    排序:
                </td>
                <td>
                    <input id="txtOrderNum" type="text" class="easyui-numberbox" style="width: 100px;" data-options="precision:0"/>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/Serv/Api/Admin/CreditAcount/";
        var ActionType = "";
        var curId=0;
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                height: document.documentElement.clientHeight - 112,
                pagination: true,
                striped: true,
                pageSize: 20,
                rownumbers: true,
                singleSelect: false,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                    { title: 'ck', width: 5, checkbox: true },
                    { field: 'type', title: '类型编码', width: 50, align: 'left', formatter: FormatterTitle },
                    { field: 'name', title: '名称', width: 50, align: 'left', formatter: FormatterTitle },
                    { field: 'credit_acount', title: '信用金', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'day_limit', title: '每日上限', width: 30, align: 'left', formatter: FormatterTitle },
                    { field: 'describe', title: '描述', width: 120, align: 'left', formatter: FormatterTitle },
                    {
                        field: 'ishide', title: '隐藏', width: 30, align: 'left', formatter: function (value, rowData) {
                            var str = value == 1 ? "是" : "否";
                            return str;
                        }
                    },
                    { field: 'order_num', title: '排序', width: 30, align: 'left', formatter: FormatterTitle }
                ]],
                onLoadSuccess: function(){   
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loaded');
                }   
            });
            $('#grvData').datagrid('getPager').pagination({      
                onSelectPage : function(pPageIndex, pPageSize) {   
                    //改变opts.pageNumber和opts.pageSize的参数值，用于下次查询传给数据层查询指定页码的数据   
                    loadData();
                }   
            });
            //初始加载
            loadData();

            $('#dlgInfo').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        var dataModel = {
                            id:curId,
                            type: $('#ddlType').val(),
                            credit_acount: $.trim($('#txtCreditAcount').val()),
                            day_limit: $.trim($('#txtDayLimit').val()),
                            ishide: $.trim($('#selectHide').val()),
                            describe: $.trim($('#txtDescription').val()),
                            order_num: $.trim($('#txtOrderNum').val())
                        }

                        if (dataModel.type == '' || dataModel.type==0) {
                            Alert("类型不能为空!");
                            return;
                        }
                        
                        if (dataModel.credit_acount == '') {
                            Alert("信用金不能为空!");
                            return;
                        }
                        if (dataModel.describe == '') {
                            Alert("描述不能为空!");
                            return;
                        }
                        var actionAshx = ActionType == "Edit" ? "Update.ashx" : "Add.ashx";
                        $.ajax({
                            type: 'post',
                            url: handlerUrl + actionAshx,
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#dlgInfo').dialog('close');
                                    loadData();
                                }
                                else {
                                    Alert(resp.msg);
                                }
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgInfo').dialog('close');
                    }
                }]
            });
        });

        function EditItem() {

            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            ActionType = "Edit";
            curId = rows[0].id;
            $('#txtCreditAcount').numberbox("setValue", rows[0].credit_acount);
            $('#txtDayLimit').numberbox("setValue", rows[0].day_limit);
            $('#ddlType').val(rows[0].type);
            $('#selectHide').val(rows[0].ishide);
            $('#txtDescription').val(rows[0].describe);
            $('#txtOrderNum').numberbox("setValue", rows[0].order_num);
            ddlType.readOnly = true;
            $('#dlgInfo').dialog({ title: '规则修改' });
            $('#dlgInfo').dialog('open');
        }

        function AddItem() {
            ActionType = "Add";
            curId = 0;
            $('#txtCreditAcount').numberbox("setValue", 0);
            $('#txtDayLimit').numberbox("setValue", -1);
            $('#ddlType').val("");
            $('#txtName').val("");
            $('#selectHide').val(0);
            $('#txtDescription').val("");
            $('#txtOrderNum').numberbox("setValue", 0);
            ddlType.readOnly = false;
            $('#dlgInfo').dialog({ title: '规则新增' });
            $('#dlgInfo').dialog('open');
        }
        function loadData() {
            var gridOpts = $('#grvData').datagrid('options');
            $('#grvData').datagrid('loading');//打开等待div   
            $.post(
                handlerUrl + "List.ashx",
                { page: gridOpts.pageNumber, rows: gridOpts.pageSize },
                function (data, status) {
                    if (data.status && data.result.list) {
                        $('#grvData').datagrid('loadData', { "total": data.result.totalcount, "rows": data.result.list });
                    }
                });
        }
    </script>
</asp:Content>
