<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="ApplyList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Member.ApplyList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    申请审核管理
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <table id="grvData" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="5" checkbox="true"></th>
                <th field="UserID" width="30" formatter="FormatterTitle">用户名</th>
                
                <%
                    if (AvatarField!=null)
                  { %>
                <th field="Avatar" width="20" formatter="FormatterImg"><%=AvatarField.MappingName %></th>
                  <%} %>
                <%foreach (ZentCloud.BLLJIMP.Model.TableFieldMapping item in formField)
                  { %>
                <th field="<%=item.Field %>" width="30" formatter="FormatterTitle"><%=item.MappingName %></th>
                  <%} %>
                <th field="MemberApplyStatus" width="15" formatter="FormatterStatus">状态</th>
                <th field="MemberApplyTime" width="35" formatter="FormatterTitle">申请时间</th>
            </tr>
        </thead>
    </table>
    <div id="divToolbar" style="padding: 5px; height: auto">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" title="通过审核" onclick="SetPass()" id="btnAdd" runat="server">通过审核</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" title="不通过审核" onclick="SetNoPass()">不通过审核</a>
            <br />
            状态:
            <select id="ddlStatus" style="width: 120px;">
                <option value="">全部</option>
                <option value="1">待审核</option>
                <option value="9">已通过</option>
                <option value="2">未通过</option>
            </select>
            关键字:<input id="txtKeyword" style="width: 160px;" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/member/';
        $(function () {
            $('#grvData').datagrid({
                method: "Post",
                height: document.documentElement.clientHeight - 100,
                toolbar: '#divToolbar',
                pagination: true,
                striped: true,
                rownumbers: true,
                rowStyler: function () { return 'height:25px'; },
                onLoadSuccess: function () {
                    //加载完数据关闭等待的div   
                    $('#grvData').datagrid('loaded');
                }
            });
            $('#grvData').datagrid('getPager').pagination({
                onSelectPage: function (pPageIndex, pPageSize) {
                    //改变opts.pageNumber和opts.pageSize的参数值，用于下次查询传给数据层查询指定页码的数据   
                    loadData();
                }
            });
            loadData();
        });
        function FormatterImg(value) {
            if (value == '' || value == null)
                return "";
            var str = new StringBuilder();
            str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="40" width="40" /></a>', value);
            return str.ToString();
        }
        function FormatterStatus(value) {
            if (value == 1) {
                return "待审核";
            }
            else if(value==2){
                return '<span style="color:red;">未通过</span>';
            }
            else if (value == 9) {
                return '<span style="color:green;">已通过</span>';
            }
            return "";
        }

        function Search() {
            var gridOpts = $('#grvData').datagrid('options', { pageNumber: 1 });
            loadData();
        }
        function loadData() {
            var gridOpts = $('#grvData').datagrid('options');
            $('#grvData').datagrid('loading');//打开等待div   
            $.post(
                handlerUrl + "ApplyList.ashx",
                { page: gridOpts.pageNumber, rows: gridOpts.pageSize, status: $.trim($("#ddlStatus").val()), keyword: $.trim($("#txtKeyword").val()) },
                function (data, status) {
                    if (data.status && data.result.list) {
                        $('#grvData').datagrid('loadData', { "total": data.result.totalcount, "rows": data.result.list });
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
        //审核通过
        function SetPass() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm('系统提示', '确认审核通过？', function (o) {
                if (o) {

                    $.ajax({
                        type: "Post",
                        url: handlerUrl + "ApplyPass.ashx",
                        data: { ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            if (result.status) {
                                $.messager.show({
                                    title: '系统提示',
                                    msg: '完成审核通过'
                                });
                                loadData();
                            }
                            else {
                                $.messager.alert('系统提示', result.msg);
                            }
                        }
                    });
                }
            });
        }
        //审核不通过
        function SetNoPass() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm('系统提示', '确认审核不通过？', function (o) {
                if (o) {

                    $.ajax({
                        type: "Post",
                        url: handlerUrl + "ApplyNoPass.ashx",
                        data: { ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            if (result.status) {
                                $.messager.show({
                                    title: '系统提示',
                                    msg: '完成审核不通过'
                                });
                                loadData();
                            }
                            else {
                                $.messager.alert('系统提示', result.msg);
                            }
                        }
                    });
                }
            });
        }
    </script>
</asp:Content>
