<%@ Page Title="" Language="C#" EnableSessionState="ReadOnly" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Booking.MeetingRoom.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <%=currShowName %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="30" checkbox="true"></th>
                <th field="product_id" width="30" formatter="FormatterTitle">ID</th>
                <%if(!isAdded){ %>
                <th field="img" width="40" formatter="FormatterImage50">首图</th>
                <%} %>
                <th field="title" width="120" formatter="FormatterProduct"><%=currShowName %></th>
                <%if(!isAdded){ %>
                <th field="relation_products" width="100" formatter="FormatterAdded">增值服务</th>
                <th field="category_name" width="100" formatter="FormatterTitle">分类</th>
                <%} %>
                <th field="access_level" width="30" formatter="FormatterTitle">访问级别</th>
                <th field="status" width="30" formatter="FormatterStatus">状态</th>
                <th field="sort" width="20" formatter="FormatterTitle">排序</th>
                <th field="Action" width="20" formatter="FormatAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true" title="添加<%=currShowName %>" onclick="AddModel()" id="btnAdd" runat="server">添加<%=currShowName %></a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-start" plain="true" title="批量上架" onclick="SetOnsale(1)" id="A1" runat="server">批量上架</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-stop" plain="true" title="批量下架" onclick="SetOnsale(0)" id="A2" runat="server">批量下架</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-cancel" plain="true" title="批量删除" onclick="DelItem()" id="A3" runat="server">批量删除</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="设置访问级别" onclick="SetAccessLevel()" id="btnSetAccessLevel" runat="server">设置访问级别</a>
                <%if(!isAdded){ %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="预约二维码" onclick="ShowQRCode()" id="A4" runat="server">预约二维码</a>
                <%} %>
            <br />
                <%if(!isAdded){ %>
                    <%=currShowName %>分类:<%=sbCategory.ToString() %>
                <%} %>
            名称:<input id="txtKeyword" style="width: 200px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
     <div id="dlgInfo" class="easyui-dialog" closed="true" title="" style="width: 400px;
        padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    访问等级:
                </td>
                <td>
                    <input id="txtAccessLevel" type="text" style="width: 250px;" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                </td>
            </tr>
        </table>
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
        var handlerUrl = '/serv/api/admin/booking/';
        var handlerOldUrl = "/serv/api/admin/mall/product.ashx";
        var type = '<% =categoryType%>';
        var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Authority%>';
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url:handlerUrl + "list.ashx",
                queryParams:{type:type,cate_id:0},
                height: document.documentElement.clientHeight - 70,
                toolbar: '#divToolbar',
                pagination: true,
                striped: true,
                loadFilter:pagerFilter,
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

            //批量设置访问级别对话框
            $('#dlgInfo').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var IDs = [];
                        for (var i = 0; i < rows.length; i++) {
                            IDs.push(rows[i].product_id);
                        }
                        var access_level = $.trim($('#txtAccessLevel').val());
                        if (access_level == "") access_level = 0;
                        var dataModel = {
                            pid: IDs.join(','),
                            access_level: access_level
                        }
                        $.ajax({
                            type: 'post',
                            url: handlerUrl +"SetAccessLevel.ashx",
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#dlgInfo').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {
                                    Alert(resp.msg);
                                }
                            }
                        });
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgInfo').dialog('close');
                    }
                }]
            });
        });
        function FormatAction(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="Edit.aspx?product_id={0}&type={1}"><img alt="编辑" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑" /></a>&nbsp;', rowData['product_id'], type);
            return str.ToString();
        }
        function FormatterStatus(value){
            if(value==0){
                return '<span style="color:red;">下架</span>';
            }
            return "上架";
        }
        function FormatterProduct(value, rowData) {
            var str = new StringBuilder();
            if (rowData.price > 0) {
                str.AppendFormat('{0} <span style="color:red;">{1}</span>{2}<br />', rowData.title, rowData.price, rowData.unit);
            }
            else {
                str.AppendFormat('{0} 免费<br />', rowData.title);
            }
            return str.ToString();
        }
        function FormatterPriceUnit(value, rowData) {
            return value + " " + rowData.unit;
        }
        function FormatterAdded(value, rowData) {
            var str = new StringBuilder();
            for (var i = 0; i < value.length; i++) {
                if (i != 0) str.AppendFormat('<br />');
                if (value[i].price > 0) {
                    str.AppendFormat('{0} <span style="color:red;">{1}</span>{2}', value[i].title, value[i].price, value[i].unit);
                }
                else {
                    str.AppendFormat('{0} 免费<br />', value[i].title);
                }
            }
            return str.ToString();
        }
        

        function AddModel() {
            document.location.href = "Edit.aspx?product_id=0&type="+type;
        }
        
        function SetOnsale(value) {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var str = "上架";
            if (value == "0") {
                str = "下架";
            }
            $.messager.confirm('系统提示', '确定设定状态为' + str + '？', function (o) {
                if (o) {
                    var rows = $('#grvData').datagrid('getSelections');
                    var IDs = [];
                    for (var i = 0; i < rows.length; i++) {
                        IDs.push(rows[i].product_id);
                    }
                    var model = {
                        action: "updatefield",
                        product_ids: IDs.join(','),
                        field: "is_onsale",
                        value: value
                    }
                    $.ajax({
                        type: "Post",
                        url: handlerOldUrl,
                        data: model,
                        success: function (result) {
                            if (result.errcode ==0) {
                                $('#grvData').datagrid('reload');
                            }
                            else {
                                $.messager.alert('系统提示', result.errmsg);
                            }
                        }
                    });
                }
            });
        }
        function DelItem() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm('系统提示', '确定删除所选？', function (o) {
                if (o) {
                    var rows = $('#grvData').datagrid('getSelections');
                    var IDs = [];
                    for (var i = 0; i < rows.length; i++) {
                        IDs.push(rows[i].product_id);
                    }
                    var model = {
                        action: "updatefield",
                        product_ids: IDs.join(','),
                        field: "is_delete",
                        value: "1"
                    }
                    $.ajax({
                        type: "Post",
                        url: handlerOldUrl,
                        data: model,
                        success: function (result) {
                            if (result.errcode == 0) {
                                $('#grvData').datagrid('reload');
                            }
                            else {
                                $.messager.alert('系统提示', result.errmsg);
                            }
                        }
                    });
                }
            });
        }
        function Search() {
            var model = {
                type:type,
                cate_id:$.trim($("#ddlCate").val()),
                keyword:$.trim($("#txtKeyword").val())
            }
            $('#grvData').datagrid('load',model);
        }

        //设置访问等级
        function SetAccessLevel() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $("#dlgInfo").dialog({ title: "设置访问级别" });
            $("#dlgInfo").dialog("open");
        }
        function ShowQRCode() {
            var linkurl = "http://" + domain + "/App/Booking/MeetingRoom/m/index.aspx?type="+type;
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
    </script>
</asp:Content>
