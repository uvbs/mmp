<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Activity.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Recycle.Activity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;回收站&nbsp;&gt;&nbsp;<span>活动</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="RecoverData()">批量还原活动</a>
            <br />

        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
     

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">

     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currDlgAction = '';
     var currSelectAcvityID = 0;
     var domain = '<%=Request.Url.Host%>';

     $(function () {

         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuActivityByDelete", ArticleType: "Activity" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'JuActivityID', title: '活动编号', width: 50, align: 'left', formatter: FormatterTitle },
                                {
                                    field: 'ThumbnailsPath', title: '缩略图', width: 50, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                        return str.ToString();
                                    }
                                },
                                {
                                    field: 'ActivityName', title: '主题', width: 150, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        str.AppendFormat('<a href="javascript:;" onclick="ShowQRcode(\'{1}\',\'{2}\')" title="{0}">{0}</a>', value, rowData.JuActivityIDHex, rowData.JuActivityID);
                                        return str.ToString();
                                    }
                                },
                                { field: 'CategoryName', title: '分类', width: 60, align: 'left', formatter: FormatterTitle },
                                { field: 'ActivityAddress', title: '地点', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'ActivityStartDate', title: '活动时间', width: 80, align: 'left', formatter: FormatDate },
                                 {
                                     field: 'IP', title: 'IP/PV', width: 30, align: 'center', formatter: function (value, rowData) {
                                         var str = new StringBuilder();
                                         if (rowData.PV == 0) {
                                             str.AppendFormat("{0}", rowData.PV);
                                         } else {
                                             str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}" title="点击查看统计详情">{1}/{2}</a>', rowData.JuActivityID, rowData.IP, rowData.PV);
                                         }
                                         return str.ToString();
                                     }
                                 },

                                 {
                                     field: 'ShareTotalCount', title: '分享统计', width: 40, align: 'center',
                                     formatter: function (value, rowData) {
                                         var str = new StringBuilder();
                                         if (rowData.ShareTotalCount==0) {
                                             str.AppendFormat("{0}", 0);
                                         } else {
                                             str.AppendFormat('<a class="listClickNum" href="/App/Cation/ArticleStatistics.aspx?articleId={0}"  title="点击查看统计详情" >{1}</a>', rowData.JuActivityID, value);
                                         }
                                         return str.ToString();
                                     }
                                 },
                                 {
                                     field: 'SignUpTotalCount', title: '报名人数', width: 40, align: 'center', formatter: function (value, rowData) {
                                         var str = new StringBuilder();
                                         if (value == 0) {
                                             str.AppendFormat("{0}", value);
                                         } else {
                                             str.AppendFormat('<a class="listClickNum" href="/App/Cation/ActivitySignUpDataManage.aspx?ActivityID={0}"  title="点击查看报名详情">{1}</a>', rowData.SignUpActivityID, value);
                                         }
                                         return str.ToString();
                                     }
                                 }
	                ]]
	            }
            );
     });



     

    


     //获取选中行ID集合
     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].JuActivityID
                 );
         }
         return ids;
     }

    

     //设置访问级别
     function RecoverData() {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", "确定恢复选中活动?", function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: "RecoverJuActivity", ids: GetRowsIds(rows).join(',') },
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



 </script>
</asp:Content>
