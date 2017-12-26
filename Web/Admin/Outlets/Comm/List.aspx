<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Outlets.Comm.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <%=typeConfig.CategoryTypeDispalyName %>管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="JuActivityID" width="40" formatter="FormatterTitle">编号</th>
                <% 
                    StringBuilder strHtml = new StringBuilder();
                    foreach (var item in formField.Where(p => !string.IsNullOrWhiteSpace(typeConfig.Ex3) && typeConfig.Ex3.Split(',').Contains(p.Field)))
                    {
                        if (item.FieldType == "img")
                        {
                            strHtml.AppendLine(string.Format("<th field='{0}' width='50' formatter='FormatterImage50'>{1}</th>", item.Field, item.MappingName));
                        }
                        else
                        {
                            strHtml.AppendLine(string.Format("<th field='{0}' width='100' formatter='FormatterTitle'>{1}</th>", item.Field, item.MappingName));
                        }
                    }

                    if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
                    {
                        strHtml.AppendLine(string.Format("<th field='LngLat' width='60' formatter='FormatterHasLngLat'>有无坐标</th>"));
                    }
                    this.Response.Write(strHtml.ToString());
                    %>
                <th field="action" width="30" formatter="FormatAction">操作</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add" plain="true" title="添加<%=typeConfig.CategoryTypeDispalyName %>" onclick="AddItem()" id="btnAdd" runat="server">添加<%=typeConfig.CategoryTypeDispalyName %></a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-cancel" plain="true" title="批量删除" onclick="DelItem()" id="A3" runat="server">批量删除</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="二维码" onclick="ShowQRCode()" id="A4" runat="server">二维码</a>
            <br />
                <% strHtml = new StringBuilder();
                   foreach (var item in formField.Where(p => typeConfig.ListFields.Split(',').Contains(p.Field) || typeConfig.EditFields.Split(',').Contains(p.Field)))
                   {
                       if (!string.IsNullOrWhiteSpace(item.Options))
                       {
                           strHtml.AppendLine(string.Format("{1}:<select id='ddlSearch{0}'>", item.Field, item.MappingName));
                           strHtml.AppendLine("<option value=''></option>");
                           foreach (var opt in item.Options.Split(','))
                           {
                               strHtml.AppendLine(string.Format("<option value='{0}'>{0}</option>", opt));
                           }
                           strHtml.AppendLine("</select>");
                       }
                       else
                       {
                           strHtml.AppendLine(string.Format("{1}:<input id='txtSearch{0}' style='width: 120px;' />", item.Field, item.MappingName));
                       }
                    }
                   if (!string.IsNullOrWhiteSpace(typeConfig.NeedFields))
                   {
                       strHtml.AppendLine("关键字:<input id='txtSearchKeyword' style='width: 120px;' />");
                   }
                   if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
                   {
                       strHtml.AppendLine(string.Format("<input id='chkLngLat' class='positionTop2' type='checkbox' onclick='Search();' /><label for='chkLngLat'>显示无坐标<label>&nbsp;"));
                   }
                   this.Response.Write(strHtml.ToString());
                    %>
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
        var handlerUrl = '/serv/api/admin/outlets/comm/';
        var handlerOldUrl = "/Handler/App/CationHandler.ashx";
        var domain = '<%=!string.IsNullOrEmpty(strDomain)?strDomain:Request.Url.Host%>';
        var type = '<%=Request["type"]%>';
        var app_page_path = '/App/Outlets/Comm/List.aspx';
        
        <% StringBuilder strHtml = new StringBuilder();%>
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl + "list.ashx",
                queryParams: { type: type },
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
                } ,
            });
        });
        function FormatterHasLngLat(value, rowData) {
            if (rowData.UserLongitude && $.trim(rowData.UserLongitude) != "") return '<span style="color:green;">有坐标</span>';
            return '<span style="color:red;">无坐标</span>';
        }
        function FormatAction(value, rowData) {
            var str = new StringBuilder();
            str.AppendFormat('<a href="Edit.aspx?id={0}&type={1}"><img alt="编辑" class="imgAlign" src="/MainStyle/Res/easyui/themes/icons/database_Edit.gif" title="编辑" /></a>&nbsp;', rowData['JuActivityID'], type);
            return str.ToString();
        }
        function AddItem() {
            document.location.href = "Edit.aspx?id=0&type=" + type;
        }

        function DelItem() {
            var rows = $("#grvData").datagrid('getSelections');//获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].JuActivityID);
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
            var model = { type: type };
                <% 
                strHtml = new StringBuilder();
                   foreach (var item in formField.Where(p => typeConfig.ListFields.Split(',').Contains(p.Field) || typeConfig.EditFields.Split(',').Contains(p.Field)))
                   {
                       if (!string.IsNullOrWhiteSpace(item.Options))
                       {
                           strHtml.AppendLine(string.Format("var temp{0} = $.trim($('#ddlSearch{0}').val());", item.Field));
                           strHtml.AppendLine(string.Format("if(temp{0}!='') model.{0}= temp{0};", item.Field));
                       }
                       else
                       {
                           strHtml.AppendLine(string.Format("var temp{0} = $.trim($('#txtSearch{0}').val());", item.Field));
                           strHtml.AppendLine(string.Format("if(temp{0}!='') model.{0}= temp{0};", item.Field));
                       }
                   }
                   if (!string.IsNullOrWhiteSpace(typeConfig.NeedFields))
                   {
                       strHtml.AppendLine("var tempKeyword = $.trim($('#txtSearchKeyword').val());");
                       strHtml.AppendLine("if(tempKeyword!='') model.Keyword= tempKeyword;");
                   }
                   
                   if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
                   {
                       strHtml.AppendLine(string.Format("model.OnlyLngLatIsNull = chkLngLat.checked?1:0;"));
                   }
                   
                   this.Response.Write(strHtml.ToString());
                    %>
            $('#grvData').datagrid('load',model);
        }
        function ShowQRCode() {
            var linkurl;
            if (app_page_path.indexOf('http://') == 0) {
                linkurl = app_page_path;
            }
            else {
                linkurl = "http://" + domain + app_page_path+"?type="+type;
            }
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
