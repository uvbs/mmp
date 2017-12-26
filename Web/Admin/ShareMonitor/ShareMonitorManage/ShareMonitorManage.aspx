<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ShareMonitorManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.ShareMonitor.ShareMonitorManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="http://cdn.bootcss.com/bootstrap/3.3.4/css/bootstrap.css" rel="stylesheet" />--%>
    <link href="/admin/ShareMonitor/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;分享监测
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
        <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            
            <a href="javascript:;" class="easyui-linkbutton" id="btnAdd" iconcls="icon-add2"
                plain="true">新建监测</a>
            
            <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" id="btnDeleteBatch">批量删除</a>

            <br />
            关键字:<input id="txtKeyWord" style="width: 200px;" />
             <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                >查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
      <div class="warpAddDiv warpLayer hidden" style="border-radius: 8px; width:500px;">
        <div class="warpContent">
            <div class="form-group mTop15">
                <label>监测名称</label>
                <input type="text" class="form-control txtName"  placeholder="监测名称" />
            </div>
                <div class="form-group">
                <label>监测链接</label>
                <input type="text" class="form-control txtUrl"  placeholder="监测链接:http://example.example.com" />
            </div>
               
        </div>
        <div class="warpOpeate">
            <a href="javascript:;" class="button button-primary button-rounded button-small btnSave">
                确定</a> <a href="javascript:;" class="button button-rounded button-small btnCancel">取消</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/admin/ShareMonitor/ShareMonitorManage/shareMonitorManage.js" type="text/javascript"></script>
    <script type="text/javascript">
        shareMonitorManage.init();
    </script>
</asp:Content>
