<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXMallScoreTypeInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallScoreTypeInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置:&nbsp;客户管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>积分分类</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="ADWXMallScoreTypeInfo.aspx" class="easyui-linkbutton" iconcls="icon-add"
                plain="true">添加</a> <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit"
                    plain="true" onclick="Update()">修改</a> <a href="javascript:;" class="easyui-linkbutton"
                        iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>
            <br />
            <label style="margin-left: 8px;">
                分类名称:</label>
            <input type="text" id="txtOrderID" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     $(function () {

         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryScoreTypeInfos" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                            { title: 'ck', width: 5, checkbox: true },
                            { field: 'TypeImg', title: '缩略图', width: 50, align: 'center', formatter: function (value) {
                                if (value == '' || value == null)
                                    return "";
                                var str = new StringBuilder();
                                str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                return str.ToString();
                            }
                            },
                            { field: 'TypeName', title: '分类名称', width: 100, align: 'center', formatter: FormatterTitle },
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
         $.messager.confirm("系统提示", "确定删除选中?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "DeleteScoreTypeInfos", ids: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 0) {
                             $('#grvData').datagrid('reload');
                             Show(resp.Msg);
                         } else {
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
             ids.push(rows[i].AutoId);
         }
         return ids;
     }


     function Search() {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryScoreTypeInfos", TypeName: $("#TypeName").val() }
	            });
     }
     function Update() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (rows.length > 1) {
             Alert("请选择一行修改");
             return;
         }
         window.location.href = "ADWXMallScoreTypeInfo.aspx?AutoId=" + rows[0].AutoId;

     }
     function formartcolor(value) {

         return "<font color='red'>" + value + "</font>";
     }
    </script>
</asp:Content>