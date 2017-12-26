<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Outlets.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    网点管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="30" checkbox="true"></th>
                <th field="img" width="50" formatter="FormatterImage50">图片</th>
                <th field="title" width="100" formatter="FormatterTitle">名称</th>
                <th field="k1" width="50" formatter="formatterArea">省市区</th>
                <th field="address" width="100" formatter="FormatterTitle">地址</th>
                <th field="k4" width="50" formatter="FormatterTitle">电话</th>
                <th field="cate_name" width="50" formatter="FormatterTitle">类型</th>
                
                <%--<th field="tags" width="100" formatter="FormatterTitle">标签</th>
                <th field="server_time" width="100" formatter="FormatterTitle">服务时间</th>
                <th field="server_msg" width="100" formatter="FormatterTitle">主办业务</th>--%>
                <th field="action" width="30" formatter="FormatAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" title="添加网点" onclick="AddItem()" id="btnAdd" runat="server">添加网点</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" title="批量删除" onclick="DelItem()" id="A3" runat="server">批量删除</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="二维码" onclick="ShowQRCode()" id="A4" runat="server">二维码</a>
            <br />
            类型:<%=sbCategory.ToString() %>

          <%--  县区:<select id="selectArea">
                <option value=""></option>
                <option value="市中心">市中心</option>
                <option value="静安">静安</option>
                <option value="徐汇">徐汇</option>
                <option value="长宁">长宁</option>
                <option value="黄浦">黄浦</option>
                <option value="浦东">浦东</option>
                <option value="杨浦">杨浦</option>
                <option value="虹口">虹口</option>
                <option value="普陀">普陀</option>
                <option value="闵行">闵行</option>
                <option value="松江">松江</option>
                <option value="奉贤">奉贤</option>
                <option value="宝山">宝山</option>
                <option value="嘉定">嘉定</option>
                <option value="青浦">青浦</option>
                <option value="金山">金山</option>
                <option value="崇明">崇明</option>
            </select>--%>

            名称:<input id="txtKeyword" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <div id="dlgSHowQRCode" class="easyui-dialog" closed="true" data-options="iconCls:'icon-tip'"
        title="用微信扫描二维码" modal="true" style="width: 380px; height: 360px; padding: 20px;
        text-align: center; vertical-align: middle;">
        <img alt="正在加载" id="imgQrcode" width="220" height="220" />
        <br />
        <a id="alinkurl" href="" target="_blank" title="点击查看"></a>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/outlets/';
        var handlerOldUrl = "/Handler/App/CationHandler.ashx";
        var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl + "list.ashx",
                queryParams: { cate_id: 0 },
                height: document.documentElement.clientHeight - 70,
                toolbar: '#divToolbar',
                pagination: true,
                striped: true,
                loadFilter: pagerFilter,
                rownumbers: true,
                rowStyler: function () { return 'height:25px'; },
                onLoadSuccess: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loaded');
                },
                onBeforeLoad: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loading');
                },
            });
        });

        function FormatAction(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="Edit.aspx?id={0}"><img alt="编辑" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑" /></a>&nbsp;', rowData['id']);
            return str.ToString();
        }
        function AddItem() {
            document.location.href = "Edit.aspx?id=0";
        }

        function DelItem() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].id);
            }
            if (ids.length == 0) return;
            $.messager.confirm('系统提示', '确定删除所选？', function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerOldUrl,
                        data: { Action: "DeleteJuActivity", ids: ids.join(','), type: "Outlets" },
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
        function Search() {
            var model = {
                cate_id:$.trim($("#ddlCate").val()),
                keyword: $.trim($("#txtKeyword").val()),
                k1: $.trim($("#selectArea").val()),
            }
            $('#grvData').datagrid('load',model);
        }
        function ShowQRCode() {
            var linkurl = "http://" + domain + "/App/Outlets/List.aspx";
            $.ajax({
                type: 'post',
                url: "/Handler/QCode.ashx",
                data: { code: linkurl },
                success: function (result) {
                    $("#imgQrcode").attr("src", result);
                }
            });
            $('#dlgSHowQRCode').dialog('open');
            $("#alinkurl").html(linkurl);
            $("#alinkurl").attr("href", linkurl);
        }

        ///格式化地址
        function formatterArea(value, rowData) {
            var address = "";
            if (rowData.province!=null) {
                address += rowData.province;
            }
            if (rowData.city != null) {
                address += rowData.city;
            } if (rowData.district != null) {
                address += rowData.district;
            }
            return address;
           
        }
    </script>
</asp:Content>
