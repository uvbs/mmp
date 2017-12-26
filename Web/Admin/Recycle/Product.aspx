<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Recycle.Product" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;回收站&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>商品</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
           <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-edit" plain="true" onclick="SetRestoreProduct()">批量还原商品</a>
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
        var domain = '<%=Request.Url.Host %>';
        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallProductInfoByDelete" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'PID', title: '编号', width: 50, align: 'left' },
                                {
                                    field: 'RecommendImg', title: '缩略图', width: 50, align: 'center', formatter: function (value) {
                                        if (value == '' || value == null)
                                            return "";
                                        var str = new StringBuilder();
                                        str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                        return str.ToString();
                                    }
                                },
                            {
                                field: 'PName', title: '商品名称', width: 160, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a target="_blank"  title="点击查看商品详情" onclick="ShowQRcode(\'{0}\')">{1}</a>', rowData.PID, rowData.PName);
                                    return str.ToString();
                                }
                            },
                            { field: 'Price', title: '价格', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Stock', title: '库存', width: 100, align: 'center', formatter: FormatterTitle },
                            {
                                field: 'IsOnSale', title: '上架/下架', width: 100, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    if (value == "1") {
                                        str.AppendFormat("<font color='green'>上架</font>");
                                    }
                                    else {
                                        str.AppendFormat("<font color='red'>下架</font>");
                                    }
                                    return str.ToString();
                                }
                            },
                            {
                                field: 'View', title: '访问量PV', width: 100, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    if (rowData.PV == 0||rowData==null) {
                                        str.AppendFormat('{0}', 0);
                                    } else {
                                        str.AppendFormat('<a class="listClickNum" href="javascript:;">{0}</a>',rowData.PV);
                                    }
                                    return str.ToString();
                                }
                            },
                            { field: 'InsertDate', title: '时间', width: 100, align: 'center', formatter: FormatDate }
                            
	                ]]
	            }
            );


        });

        //批量还原商品
        function SetRestoreProduct() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定还原选中商品?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "RestoreWXMallProductInfo", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            $('#grvData').datagrid('reload');
                        }

                    });
                }
            });
        }

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].PID
                 );
            }
            return ids;
        }
    </script>
</asp:Content>
