<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SubAccountsMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Sys.SubAccountsMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
   当前位置：&nbsp;系统管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>子账户管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowAdd();" id="btnAdd">新建子账户</a><a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="ShowEdit();" id="btnEdit">编辑子账户</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="DisableUser(1)">批量禁用账户</a> <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="DisableUser(0)">批量启用账户</a>
                    <br />
                    姓名:
                    <input type="text" id="txtTrueNames" style="width:200px" /> 
            &nbsp; <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

    <div id="dlgInput" class="easyui-dialog" closed="true" title="" style="width: 400px;
         padding: 15px;">
        <table width="100%">
                    <tr>
                <td>
                    头像
                </td>
                <td>
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imghead" /><br />
                             <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="fileheadimg.click()">上传</a>

                        <input type="file" id="fileheadimg" name="file1" style="display:none;"/>

                </td>
            </tr>
            <tr>
                <td>
                    账户名
                </td>
                <td>
                    <input id="txtUserID" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    登录密码
                </td>
                <td>
                    <input id="txtPwd" type="password" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    登录密码确认
                </td>
                <td>
                    <input id="txtPwd2" type="password" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    公司名称
                </td>
                <td>
                    <input id="txtCompany" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    真实姓名
                </td>
                <td>
                    <input id="txtTrueName" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    手机
                </td>
                <td>
                    <input id="txtPhone" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    职位
                </td>
                <td>
                    <input id="txtPostion" type="text" style="width: 250px;" />
                </td>
            </tr>
            <tr>
                <td>
                    可建的投票数
                </td>
                <td>
                    <input id="txtVoteCount" type="text" style="width: 250px;" />
                </td>
            </tr>
             <tr>
                <td>
                    不能删除数据
                </td>
                <td>
                    <input type="checkbox" style="width: 250px;" id="cb1" />
                </td>
            </tr>
              <tr>
                <td>
                    不能导出数据
                </td>
                <td>
                    <input type="checkbox" style="width: 250px;"  id="cb2" />
                </td>
            </tr>
        </table>
    </div>
   
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currSelectUsers = [];
     var currUserAction = '';

     $(function () {

         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QuerySubAccount" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'WXHeadimgurl', title: '头像', width: 50, align: 'left', formatter: function (value, row) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                
                                 } },
                                { field: 'UserID', title: '账户名', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'TrueName', title: '姓名', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'VoteCount', title: '可建投票数', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'Company', title: '公司', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'Postion', title: '职位', width: 50, align: 'left', formatter: FormatterTitle },
                                { field: 'Phone', title: '手机', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'Email', title: '邮箱', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'IsDisable', title: '启用/禁用', width: 100, align: 'left', formatter: FormartIsDisable }

                             ]]
	            }
            );

         $('#dlgInput').dialog({
             buttons: [{
                 text: '保存',
                 handler: function () {
                     var dataModel = {
                         Action: currUserAction,
                         UserID: $.trim($('#txtUserID').val()),
                         Pwd: $.trim($('#txtPwd').val()),
                         Pwd2: $.trim($('#txtPwd2').val()),
                         TrueName: $.trim($('#txtTrueName').val()),
                         Company: $.trim($('#txtCompany').val()),
                         Phone: $.trim($('#txtPhone').val()),
                         Postion: $.trim($('#txtPostion').val()),
                         VoteCount: $.trim($('#txtVoteCount').val()),
                         HeadImg: $("#imghead").attr("src"),
                         isDelete: $("#cb1").attr("checked") == "checked" ? "true" : "false",
                         isExport:$("#cb2").attr("checked")=="checked"?"true":"false"
                     }
                     if (dataModel.Pwd == '') {
                         Alert("密码不能为空！");
                         return;
                     }

                     if (dataModel.UserID == '') {
                         Alert("UserID不能为空!");
                         return;
                     }
                     if (dataModel.Pwd != dataModel.Pwd2) {
                         Alert("确认密码输入不一致!");
                         return;
                     }
                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         dataType: "json",
                         success: function (resp) {
                             if (resp.Status > 0) {
                                 $('#dlgInput').dialog('close');
                                 $('#grvData').datagrid('reload');
                             }
                             else {

                             }
                             Alert(resp.Msg);

                         }
                     });

                 }
             }, {
                 text: '取消',
                 handler: function () {
                     $('#dlgInput').dialog('close');
                 }
             }]
         });

         $("#fileheadimg").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                         secureuri: false,
                         fileElementId: 'fileheadimg',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');

                             if (resp.Status == 1) {
                                 $("#imghead").attr("src", resp.ExStr);

                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });

     });



     function Search() {
         //pmsGroup
         $('#grvData').datagrid({
             queryParams: {
                 Action: "QuerySubAccount",
                 TrueName: $("txtTrueNames").val()
             }
         });
     }


     function ShowAdd() {
         currUserAction = "AddSubAccount";
         txtUserID.readOnly = false;
         $('#dlgInput').dialog({ title: '添加子账户' });
         $('#dlgInput').dialog('open');
     }

     function ShowEdit() {
         var rows = $('#grvData').datagrid('getSelections');

         if (!EGCheckIsSelect(rows))
             return;

         if (!EGCheckNoSelectMultiRow(rows))
             return;

         $('#txtCompany').val(rows[0].Company);
         $('#txtPhone').val(rows[0].Phone);
         $('#txtPwd').val(rows[0].Password);
         $('#txtPwd2').val(rows[0].Password);
         $('#txtTrueName').val(rows[0].TrueName);
         $('#txtUserID').val(rows[0].UserID);
         $('#txtPostion').val(rows[0].Postion);
         $('#txtVoteCount').val(rows[0].VoteCount);
         $("#imghead").attr("src", rows[0].WXHeadimgurl);
         currUserAction = "EditSubAccount";
         txtUserID.readOnly = true;
         $('#dlgInput').dialog({ title: '编辑子账户' });
         $('#dlgInput').dialog('open');

     }

     //批量禁用
     function DisableUser(disableValue) {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

         if (!EGCheckIsSelect(rows)) {
             return;
         }

         currSelectUsers = new Array();

         for (var i = 0; i < rows.length; i++) {
             currSelectUsers.push(rows[i]);
         }

         var ids = new Array();

         for (var i = 0; i < currSelectUsers.length; i++) {
             ids.push("'" + currSelectUsers[i].UserID + "'"
                                );
         }

         var modelData = {
             Action: 'DisableUser',
             userIds: ids.join(','),
             disableValue: disableValue
         }

         $.messager.confirm("系统提示", "确定" + (disableValue == 1 ? "禁用" : "启用") + "当前选择的用户？", function (r) {
             if (r) {
                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: modelData,
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             $('#grvData').datagrid('reload');
                         }
                         else {
                             Alert("操作失败");
                         }

                     }
                 });
             }
         });


     }

     function FormartIsDisable(value) {
         if (value == "0") {
             return "<font color='green'>启用</font>";
         }
         else {
             return "<font color='red'>禁用</font>"
         }


     }
    </script>
</asp:Content>