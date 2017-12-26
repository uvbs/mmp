<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SystemNoticeManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Sys.SystemNoticeManage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;系统管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>系统通知</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="SystemNoticeManageAdd.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true">
                发送系统通知</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                        iconcls="icon-delete" onclick="Delete()" plain="true">删除</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var domain = '<%=Request.Url.Host %>';
     $(function () {



         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetSystemNotice" },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                singleSelect: false,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'SerialNum', title: '发送批次', width: 10, align: 'left' },
                                { field: 'NoticeTypeString', title: '消息类型', width: 10, align: 'left' },
                                { field: 'SendTypeString', title: '发送类型', width: 10, align: 'left' },
                                { field: 'Title', title: '标题', width: 20, align: 'left' },
                                { field: 'Ncontent', title: '内容', width: 20, align: 'left' },
                                { field: 'RedirectUrl', title: '跳转Url', width: 20, align: 'left' },
                                { field: 'UserId', title: '接收人', width: 20, align: 'left' },
                                { field: 'ReadTimeString', title: '阅读时间', width: 10, align: 'left' },
                                { field: 'InsertTimeString', title: '发送时间', width: 10, align: 'left' }


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
                     data: { Action: "DelSystemNotice", ids: GetRowsIds(rows).join(',') },
                     dataType: "json",
                     success: function (resp) {
                         if (resp.Status == 0) {
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
             ids.push(rows[i].AutoID);
         }
         return ids;
     }

     $.post(handlerUrl, { Action: "GetConfigureConfigInfo" }, function (data) {
         var resp2 = $.parseJSON(data);
         if (resp2.Status == 0) {
             $("#txtQueryNum").val(resp2.ExObj.QueryNum);
             $("#txtPopupInfoon").val(resp2.ExObj.PopupInfo);
         }
         $('#dlgPmsInfo').dialog('open');
     });




     //窗体关闭按钮---------------------
     $("#btnExit").live("click", function () {
         $("#dlgPmsInfo").window("close");
     });

     $("#btnSave").live("click", function () {
         var QueryNum = $("#txtQueryNum").val();
         var PopupInfoon = $("#txtPopupInfoon").val();
         $.post(handlerUrl, { Action: "ConfigureConfigInfo", QueryNum: QueryNum, PopupInfo: PopupInfoon }, function (data) {
             var resp3 = $.parseJSON(data);
             if (resp3.Status = 0) {
                 Show(resp3.Msg);
             } else {
                 Alert(resp3.Msg);
             }
         });
     });






     function ShowQRcode(id) {
         //dlgSHowQRCode
         $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/WXShow/wap/WXWAPShowInfo.aspx?autoid=' + id);
         $('#dlgSHowQRCode').dialog('open');
         var linkurl = "http://" + domain + "/App/WXShow/wap/WXWAPShowInfo.aspx?autoid=" + id;
         $("#alinkurl").html(linkurl).attr("href", linkurl);
     }
    </script>
</asp:Content>
