<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXMallProductMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.MallManage.WXMallProductMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微商城&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>商品管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="/App/MallManage/WXMallProductCompile.aspx?Action=add" class="easyui-linkbutton"
                iconcls="icon-add2" plain="true" id="btnAdd">添加新商品</a> <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">批量删除商品</a>
            <br />
            <label style="margin-left: 8px;">
                商品分类</label>
            <%=sbCategory.ToString()%>
            上架/下架:
            <select id="ddlisonsale">
                <option value="">全部</option>
                <option value="1">上架</option>
                <option value="0">下架</option>
            </select>
            <label style="margin-left: 8px;">
                商品名称</label>
            <input type="text" id="txtPName" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 320px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
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
	                queryParams: { Action: "QueryWXMallProductInfo" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                
	                pageSize: 50,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'PID', title: '编号', width: 50, align: 'left' },
                                { field: 'RecommendImg', title: '缩略图', width: 50, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                    return str.ToString();
                                }
                                },
                            { field: 'PName', title: '商品名称', width: 160, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a target="_blank"  title="点击查看商品详情" onclick="ShowQRcode(\'{0}\')">{1}</a>', rowData.PID, rowData.PName);
                                return str.ToString();
                            }
                            },
                            { field: 'Price', title: '价格', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'Stock', title: '库存', width: 100, align: 'center', formatter: FormatterTitle },
                            { field: 'IsOnSale', title: '上架/下架', width: 100, align: 'center', formatter: function (value, rowData) {
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
                            { field: 'View', title: '访问量PV', width: 100, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('{0}', rowData.PV);
                                return str.ToString();
                            }
                            },
                             { field: 'InsertDate', title: '时间', width: 100, align: 'center', formatter: FormatDate },
                            { field: 'EditCloum', title: '操作', width: 50, align: 'center', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a title="点击修改商品信息" href="/App/MallManage/WXMallProductCompile.aspx?Action=edit&pid={0}">编辑</a>', rowData.PID);
                                return str.ToString();
                            }
                            }
                             ]]
	            }
            );


        });




            function ShowQRcode(pid) {
            var linkurl = "http://" + domain + "/App/Cation/wap/mall/Showv1.aspx?pid=" + pid;
            $("#alinkurl").html(linkurl);
            $("#alinkurl").attr("href", linkurl);
            $('#imgQrcode').attr('src', '/Handler/ImgHandler.ashx?v='+linkurl);
            $('#dlgSHowQRCode').dialog('open');
        }

        //删除
        function Delete() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中商品?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteWXMallProductInfo", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            Alert("成功删除了" + result + "件商品");
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


        function Search() {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryWXMallProductInfo", PName: $("#txtPName").val(), CategoryId: $("#ddlcategory").val(), IsOnSale: $("#ddlisonsale").val() }
	            });
        } 
    </script>
</asp:Content>
