<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="TutorList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Tutor.TutorList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;专家管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;专家列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">批量删除专家</a>
            姓名:<input id="txtKeyword" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
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
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "getTutorList" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        { field: 'id', title: 'Id', width: 50, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'name', title: '姓名', width: 50, align: 'left', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="/#/userspace/{1}" title="{0}" target="_blank">{0}</a>', value, rowData.userAutoId);
                                return str.ToString();
                            }
                        },
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
         $.messager.confirm("系统提示", "确定删除选中专家?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "deleteTutor", ids: GetRowsIds(rows).join(',') },
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

     function Search() {
         $('#grvData').datagrid({
	        method: "Post",
	        url: handlerUrl,
	        queryParams: { Action: "getTutorList", keyword: $("#txtKeyword").val() }
	    });
     }
    </script>
</asp:Content>
