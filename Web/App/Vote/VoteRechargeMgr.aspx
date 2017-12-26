<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="VoteRechargeMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Vote.VoteRechargeMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;投票&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>充值设置</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">添加</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
                <a href="javascript:history.go(-1);" class="easyui-linkbutton"
                iconcls="icon-redo" plain="true"  style="float:right;margin-right:20px;">返回</a>

                 


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgInput" class="easyui-dialog" closed="true" title="添加新分类" style="width: 370px;
        height:320px; padding: 15px;line-height:30px;">
        <table width="100%">
            <tr>
                <td>
                    票数:
                </td>
                <td>
                    <input id="txtRechargeCount" type="text" style="width: 250px;" />
                </td>
            </tr>
           <tr>
                <td>
                    金额:
                </td>
                <td>
                    <input id="txtAmount" type="text" style="width: 250px;" />
                </td>
            </tr>
              <tr>
                <td>
                    礼品名称:
                </td>
                <td>
                    <input id="txtGiftName" type="text" style="width: 250px;" />
                </td>
            </tr>
          <tr>
                <td>
                   描述:
                </td>
                <td>
                  <textarea id="txtGiftDesc" style="width: 250px;height:100px;"></textarea>
                   
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
        var VoteId = "<%=Request["vid"] %>";
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryVoteRecharge" ,VoteId:VoteId},
	                height: document.documentElement.clientHeight - 160,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'RechargeCount', title: '票数', width: 20, align: 'left' },
                                { field: 'Amount', title: '金额', width: 20, align: 'left' },
                                { field: 'GiftName', title: '礼品', width: 20, align: 'left' }
                                

                             ]]
	            }
            );



            $('#dlgInput').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        try {
                            var dataModel = {
                                Action: currAction,
                                AutoID: currSelectID,
                                VoteId:VoteId,
                                RechargeCount: $.trim($('#txtRechargeCount').val()),
                                Amount: $.trim($('#txtAmount').val()),
                                GiftName: $.trim($('#txtGiftName').val()),
                                GiftDesc: $.trim($('#txtGiftDesc').val())

                            }

                            if (dataModel.RechargeCount == '') {

                                Alert('请输入票数');
                                return;
                            }
                            if (dataModel.RechargeCount == '') {

                                Alert('请输入金额');
                                return;
                            }

                            $.ajax({
                                type: 'post',
                                url: handlerUrl,
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.Status == 1) {
                                        Show(resp.Msg);
                                        $('#dlgInput').dialog('close');
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

                        $('#dlgInput').dialog('close');
                    }
                }]
            });






        });

        function ShowAdd() {
            currAction = 'AddVoteRecharge';
            $('#dlgInput').dialog({ title: '添加' });
            $('#dlgInput').dialog('open');
            $("#dlgInput input").val("");
            $("#txtGiftDesc").val("");


        }
        function Delete() {
            try {

                var rows = $('#grvData').datagrid('getSelections');

                if (!EGCheckIsSelect(rows))
                    return;

                $.messager.confirm("系统提示", "确认删除选中数据?", function (r) {
                    if (r) {
                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].AutoID);
                        }

                        var dataModel = {
                            Action: 'DeleteVoteRecharge',
                            ids: ids.join(',')
                        }

                        $.ajax({
                            type: 'post',
                            url: handlerUrl,
                            data: dataModel,
                            success: function (result) {
                                Alert(result);
                                $('#grvData').datagrid('reload');
                            }
                        });
                    }
                });

            } catch (e) {
                Alert(e);
            }
        }

        function ShowEdit() {
            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;

            currAction = 'EditVoteRecharge';
            currSelectID = rows[0].AutoID;
            $('#txtRechargeCount').val(rows[0].RechargeCount);
            $('#txtAmount').val(rows[0].Amount);
            $('#txtGiftName').val(rows[0].GiftName);
            $('#txtGiftDesc').val(rows[0].GiftDesc);
            $('#dlgInput').dialog({ title: '编辑' });
            $('#dlgInput').dialog('open');
        }


    </script>
</asp:Content>