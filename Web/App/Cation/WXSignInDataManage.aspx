<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WXSignInDataManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.WXSignInDataManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>签到数据管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">批量删除签到数据</a>
            
             <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-back"
                    plain="true" onclick="window.location.href='/App/Cation/ActivityManage.aspx'" style="float: right;">
                    返回</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
   
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currSelectAcvityID = '<%=Request["jid"]%>';
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXSignInData", jid: currSelectAcvityID },
	                height: document.documentElement.clientHeight - 145,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Name', title: '姓名', width: 160, align: 'left' },
                                { field: 'Phone', title: '手机', width: 160, align: 'left' },
                                { field: 'SignInTime', title: '签到时间', width: 160, align: 'left', formatter: FormatDate }

	                //                                ,
	                //                                { field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
	                //                                    if (value == '' || value == null)
	                //                                        return "";
	                //                                    var str = new StringBuilder();
	                //                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
	                //                                    return str.ToString();
	                //                                }
	                //                                },

	                //                                { field: 'WXNickname', title: '昵称', width: 160, align: 'left'}

                             ]]
	            }
            );

     });

     function Delete() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }

         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].AutoID);
         }
         if (confirm("确认删除签到数据?")) {
             $.ajax({
                 type: 'post',
                 url: handlerUrl,
                 data: { Action: 'DeleteWXSignInData', ids: ids.join(',') },
                 success: function (result) {
                     Alert("已成功删除数据" + result + "条");
                     $('#grvData').datagrid('reload');
                 }
             });
         }



     }

    </script>
</asp:Content>
