<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="TutorApplyList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Tutor.TutorApplyList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;专家管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;申请列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnApproved" onclick="Approved()">批量审核通过</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">批量删除申请</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/TutorApplyHandler.ashx";
        var currDlgAction = '';
        var currSelectAcvityID = 0;
        var domain = '<%=Request.Url.Host%>';

     $(function () {

         $('#grvData').datagrid({
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "getApplyList" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                pageSize: 50,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'userId', title: 'Id', width: 50, align: 'left', formatter: FormatterTitle },
                                { field: 'name', title: '姓名', width: 50, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="/#/userspace/{1}" title="{0}" target="_blank">{0}</a>', value, rowData.userAutoId);
                                    return str.ToString();
                                } },
                                { field: 'company', title: '公司', width: 50, align: 'left', formatter: FormatterTitle },
                                { field: 'postion', title: '职位', width: 50, align: 'left', formatter: FormatterTitle },
                                { field: 'phone', title: '手机', width: 50, align: 'left', formatter: FormatterTitle },
                                { field: 'followUserCount', title: '粉丝数', width: 50, align: 'left', formatter: FormatterTitle },
                                { field: 'userFollowCount', title: '关注数', width: 50, align: 'left', formatter: FormatterTitle },
                                { field: 'articleCount', title: '文章数', width: 50, align: 'left', formatter: FormatterTitle }
	                ]]
	            }
            );

     });
     //删除
     function Delete() {

         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", "确定删除选中申请?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "deleteApply", ids: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             $('#grvData').datagrid('reload');
                             Show(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }

                 });
             }
         });


     }

     //获取选中行ID集合
     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].userId);
         }
         return ids;
     }

    //审核通过
     function Approved() {

         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         //
         $.messager.confirm("系统提示", "确定所选用户审核通过为专家", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "approvedApply", ids: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 1) {
                             $('#grvData').datagrid('reload');
                             Alert(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }

                 });
             }
         });

         //




     }




    </script>
</asp:Content>
