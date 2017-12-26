<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ReplyList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Review.ReplyList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .pass
        {
            color: Green;
            font-weight: bold;
        }
        .unpass
        {
            color: Red;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;&nbsp;话题&nbsp;&nbsp;&gt;&nbsp;&nbsp;<%=Review.ReviewTitle%>&nbsp;&nbsp;&gt;&nbsp;&nbsp;回复列表
    <a href="ReviewList.aspx" style="float: right; margin-right: 20px; color: Black;"
        title="返回上级列表" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                id="btnDelItem" onclick="DelItem()">删除</a>
            <br />

            审核状态:<select id="ddlStatus">
            <option value="">全部</option>
            <option value="0">待审核</option>
            <option value="1">审核通过</option>
            <option value="2">审核不通过</option>
            </select>
            关键字:<input id="txtKeyword" style="width: 200px;" placeholder="" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/ReviewHandler.ashx";
       
        $(function () {
          
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "ReplyList", ReviewId: <%=Review.AutoId%> },
                height: document.documentElement.clientHeight - 150,
                pagination: true,
                striped: true,
                pageSize: 50,
                rownumbers: true,
                singleSelect: true,
                rowStyler: function () { return 'height:25px'; },
                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        { field: 'Status', title: '审核状态', width: 50, align: 'left', formatter:                              function (value, rowData) {
                         var str = new StringBuilder();
                         if (value=="0") {
                          str.AppendFormat('<a>待审核</a>');
                         }
                        else if (value=="1") {
                         str.AppendFormat('<a>审核通过</a>');
                        }
                        else {
                        str.AppendFormat('<a>不通过</a>',rowData.AutoId);
                        }
                        return str.ToString();
                        } },
                        {field: 'ReplyContent', title: '内容', width: 240, align: 'left', formatter: FormatterTitle },
                        { field: 'Operate', title: '操作', width: 50, align: 'left',formatter: 
                         function (value, rowData) {
                         var str = new StringBuilder();
                         str.AppendFormat('<a  class="pass"  onclick="Update({0},1)" >通过</a>&nbsp;&nbsp;',rowData.AutoId);
                         str.AppendFormat('<a  class="unpass"  onclick="Update({0},2)" >不通过</a>',rowData.AutoId);
                         return str.ToString();
                        } }
                        
	                ]]
            }
            );

            $(ddlStatus).change(function(){
            Search();
            });
           

        });

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoId);
            }
            return ids;
        }
        function DelItem() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确认删除选中的？", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteReply", ids: GetRowsIds(rows).join(',') },
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


        function Update(id,status) {
            
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "UpdateReplyStatus", ids: id,Status:status },
                        dataType: "json",
                        success: function (resp) {
                            if (resp.Status == 1) {
                                $('#grvData').datagrid('reload');
                            }
                            else {
                                Alert(resp.Msg);
                            }
                        }

                    });
                
           
        }

        //搜索
        function Search() {
            $('#grvData').datagrid({
                method: "Post",
                url: handlerUrl,
                queryParams: { Action: "ReplyList", ReviewId: <%=Review.AutoId%>, keyword: $("#txtKeyword").val(),status:$(ddlStatus).val()}
            });
        }
       
    </script>
</asp:Content>
