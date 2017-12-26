<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CrowdFundInfoMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CrowdFund.Admin.CrowdFundInfoMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>众筹管理 </span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="CrowdFundInfoAddEdit.aspx" class="easyui-linkbutton" iconcls="icon-add2" plain="true">新建</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>
            <br />
                <span style="font-size: 12px; font-weight: normal">名称：</span>
                <input type="text" style="width: 200px" id="txtName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            <br />


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

       <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 320px;
        padding: 20px; text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
                <br />
         <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
       
    
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var domain = '<%=Request.Url.Host%>';
     var handlerUrl = "AdminHandler.ashx";
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryCrowdFundInfo" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'CoverImage', title: '图片', width: 10, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="封面图片" height="100" width="200" />', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'Title', title: '名称', width: 20, align: 'left' },
                                { field: 'FinancAmount', title: '筹集金额', width: 10, align: 'left' },
                                { field: 'TotalPayAmount', title: '已筹集', width: 10, align: 'left' },
                                { field: 'PayPercent', title: '进度', width: 10, align: 'left', formatter: function (value, rowData) {
                                    return value + "%";
                                } 
                                },
                                { field: 'Status', title: '状态', width: 5, align: 'left', formatter: function (value, rowData) {
                                    if (value == "1") {
                                        return "<font color='green'>进行中<font/>";
                                    }
                                    else {
                                        return "<font color='red'>已停止<font/>";
                                    }

                                }
                                },
                             { field: 'Op', title: '操作', width: 20, align: 'center', formatter: function (value, rowData) {
                                 var str = new StringBuilder();
                                 str.AppendFormat('<a href="CrowdFundInfoAddEdit.aspx?id={0}" >[修改]</a>', rowData['AutoID']);
                                 str.AppendFormat('&nbsp;&nbsp;<a href="CrowdFundRecordMgr.aspx?id={0}" >[付款记录]</a>', rowData['AutoID']);
                                 str.AppendFormat('&nbsp;&nbsp;<a href="javascript:void(0)" onclick="ShowQRcode({0})">[二维码]</a>', rowData['AutoID']);
                                 return str.ToString();
                             }
                             }

                             ]]
	            }
            );

         $("#btnSearch").click(function () {
             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryCrowdFundInfo", Title: $("#txtName").val()} });
         });

     })


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
                         Action: 'DeleteCrowdFundInfo',
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

     function ShowQRcode(aid) {
         $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v=http://' + domain + '/App/CrowdFund/Mobile/CrowdFundInfoShow.aspx?id=' + aid);
         $('#dlgSHowQRCode').dialog('open');
         var linkurl = "http://" + domain + "/App/CrowdFund/Mobile/CrowdFundInfoShow.aspx?id=" + aid;
         $("#alinkurl").html(linkurl);
         $("#alinkurl").attr("href", linkurl);
     }
 </script>
</asp:Content>
