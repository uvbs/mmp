<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="WxFansForwardUserInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Forward.WxFansForwardUserInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微营销&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>所有转发人信息</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
           <%-- <a href="javascript:;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-excel'"
                id="btnExportToFile" onclick="DownLoadData()">导出到文件</a>--%>
                 <a  href="javascript:history.go(-1);" class="easyui-linkbutton" iconcls="icon-redo" plain="true">返回</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="使用微信扫描二维码" modal="true" style="width: 320px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/WXForwardHandler.ashx";
     var domain = '<%=Request.Url.Host %>';
     var ActivityId = '<%=ActivitId %>';
     var Mid = '<%=Mid %>';
     var uid = '<%=uid %>'
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetForwarInfos", ActivityId: ActivityId, Mid: Mid },
	                height: document.documentElement.clientHeight - 150,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                singleSelect: true,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'TureName', title: '转发人姓名', width: 10, align: 'left' },
                                { field: 'Phone', title: '转发人手机号码', width: 10, align: 'left' },
                                { field: 'ActivityName', title: '文章标题', width: 18, align: 'left' },
                                {
                                    field: 'PowderCount', title: '吸粉人数', sortable: true, width: 8, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        if (rowData.PowderCount == 0) {
                                            str.AppendFormat("{0}", rowData.PowderCount);
                                        } else {
                                            str.AppendFormat('<a class="listClickNum" title="点击查看粉丝" href="WxFansUserInfo.aspx?ActivityID={0}&LinkName={1}" >{2}</a>', rowData.JuActivityID, rowData.LinkName, rowData.PowderCount);
                                        }
                                        return str.ToString();
                                    }
                                },
                                {
                                    field: 'UV', title: '微信阅读人数', sortable: true, width: 8, align: 'left', formatter: function (value, rowData) {
                                        var str = new StringBuilder();
                                        if (rowData.UV == 0) {
                                            str.AppendFormat("{0}", rowData.UV);
                                        } else {
                                            str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&uv=1&spreaduserid={1}" title="点击查看统计详情">{2}</a>', rowData.JuActivityID, rowData.LinkName, rowData.UV);
                                        }
                                        
                                        return str.ToString();
                                    }
                                },
                                 {
                                     field: 'ippv', title: 'IP/PV', width: 8, sortable: true, align: 'left', formatter: function (value, rowData) {
                                         var str = new StringBuilder();
                                         if (rowData.DistinctOpenCount == null) {
                                             rowData.DistinctOpenCount = 0;
                                         }
                                         if (rowData.OpenCount == 0) {
                                             str.AppendFormat("{0}", 0);
                                         } else {
                                             str.AppendFormat('<a class="listClickNum" href="/App/Monitor/EventDetails.aspx?aid={0}&spreaduserid={1}" title="点击查看统计详情">{2}/{3}</a>', rowData.JuActivityID, rowData.LinkName, rowData.DistinctOpenCount, rowData.OpenCount);
                                         }
                                         return str.ToString();
                                     }
                                 },
                                { field: 'RealLink', title: '链接地址', width: 20, align: 'left' },
                                { field: 'InsertDate', title: '创建时间', width: 12, align: 'left', formatter: FormatDate },
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
                     data: { Action: "DeleteForwar", ids: GetRowsIds(rows).join(',') },
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
             ids.push(rows[i].ActivityId
                 );
         }
         return ids;
     }


     function Search() {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetForwarList", ActivityName: $("#txtName").val() }
	            });
     }



     function ShowQRcode(id) {
         $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/Forward/wap/WXForwardWap.aspx?id=' + id);
         $('#dlgSHowQRCode').dialog('open');
         var linkurl = "http://" + domain + "/App/Forward/wap/WXForwardWap.aspx?id=" + id;
         $("#alinkurl").html(linkurl).attr("href", linkurl);
     }
     function DownLoadData() {
         $.messager.confirm('系统提示', '确认导出当前数据到文件？', function (o) {
             if (o) {
                 window.open(handlerUrl + '?Action=DownLoadForwardData&Mid=' + Mid + "&activityID=" + ActivityId + "&uid=" + uid);
             }
         });
     }
    </script>
</asp:Content>

