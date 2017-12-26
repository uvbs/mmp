<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="OrderPayList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.OrderPay.OrderPayList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <input id="rdoType2" name="rdoType" type="radio" checked="checked" value="2" onclick="CheckType()"  /><label for="rdoType2">充值VIP</label>
            <input id="rdoType1" name="rdoType" type="radio" value="1" onclick="CheckType()"  /><label for="rdoType1">充值积分</label>
            <span id="spanEx1"><input id="chkEx1" name="chkEx1" type="checkbox" value="1" onclick="CheckEx1()" /><label for="chkEx1">仅显示带发票</label></span>
            姓名:<input id="txtKeyword" style="width: 200px;"  placeholder="姓名" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/OrderPayHandler.ashx";
        var domain = '<%=Request.Url.Host%>',
        type = "2",
        ex1 = "";
        $(function () {
            $('#grvData').datagrid({
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "getOrderPayList", type: type, ex1: ex1 },
	                height: document.documentElement.clientHeight - 170,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        { field: 'Ex2', title: '姓名', width: 200, align: 'left', formatter: FormatterTitle },
                        { field: 'Subject', title: '说明', width: 200, align: 'left', formatter: FormatterTitle },
                        { field: 'Total_Fee', title: '金额', width: 200, align: 'left', formatter: FormatterTitle },
                        { field: 'Ex1', title: '发票', width: 200, align: 'left', formatter: FormatterTitle }
	                ]]
	            }
            );
        });

    function CheckType() {
        if ($('#rdoType1').attr("checked")) {
            type = "1";
            ex1 = "";
            $('#chkEx1').attr("checked", false);
            $("#spanEx1").hide();
        }
        else {
            type = "2";
            $("#spanEx1").show();
        }
        Search();
    }

    function CheckEx1() {
        if ($('#chkEx1').attr("checked")) {
            ex1 = "1";
        }
        else {
            ex1 = "";
        }
        Search();
    }
    function Search() {
        var Ex1 = $('#chkEx1').attr("checked");

         $('#grvData').datagrid({
             method: "Post",
             url: handlerUrl,
             queryParams: { Action: "getOrderPayList", type: type, ex1: ex1, ex2: $("#txtKeyword").val() }
         });
     }
   
    </script>
</asp:Content>
