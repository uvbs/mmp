<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SignUpData.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.customize.kuanqiao.SignUpData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/KuanqiaoHandler.ashx";
        $(function () {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QuerySignUpData", ApplyStatus: "<%=applystatus%>" },
	                height: 570,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'K2', title: '企业名称', width: 80, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="/customize/kuanqiao/SignUpDataDetail.aspx?uid={0}"  title="点击查看详情">{1}</a>', rowData.UID,rowData.K2);
                                    return str.ToString();
                                } 
                                },
                                { field: 'K3', title: '备选企业字号1', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'K4', title: '备选企业字号2', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'K5', title: '备选企业字号3', width: 80, align: 'left', formatter: FormatterTitle },
                                { field: 'K15', title: '申请结果', width: 80, align: 'left' },
                                { field: 'InsertDate', title: '提交时间', width: 80, align: 'left', formatter: FormatDate },
                                { field: 'Opreate', title: '操作', width: 60, align: 'center', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="/customize/kuanqiao/SignUpDataDetail.aspx?uid={0}"  title="点击查看详情">点击查看详细</a>', rowData.UID);
                                    return str.ToString();
                                }
                                }

                             ]]
	            }
            );
            var applystatus = "<%=applystatus%>";
            switch (applystatus) {
                case "待处理":
                    $("#ddlapplystatus").val("待处理");
                    break;
                case "正在处理":
                    $("#ddlapplystatus").val("正在处理");
                    break;
                case "审核成功,审核失败":
                    $("#ddlapplystatus").val("");
                    break;
                default:

            }



        });


        //删除
        function Delete() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中数据 ?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteSignUpData", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            //
                            var resp = $.parseJSON(result);
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

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].UID
                 );
            }
            return ids;
        }

        function Search() {

            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: {
	                Action: "QuerySignUpData", companyName: $("#txtCompanyName").val(),
	                ApplyStatus: $("#ddlapplystatus").val(),
	                ProcessStatus: $("#txtProcessStatus").val()
                    
                     }
	            });
        }
           
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;企业核名&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=title%></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton"
                    iconcls="icon-delete" plain="true" onclick="Delete()">批量删除</a>
                    <br />
           <label style="margin-left:8px;">企业名称:</label>
           <input type="text" id="txtCompanyName" style="width:200px;" />

          <label style="margin-left:8px;">申请结果:</label>
           <select id="ddlapplystatus">
            <option value="">全部</option>
           <option value="待处理">待处理</option>
           <option value="正在处理">正在处理</option>
           <option value="审核通过">审核通过</option>
           <option value="审核失败">审核失败</option>
           </select>

            <label style="margin-left:8px;">处理状态:</label>
           <input type="text" id="txtProcessStatus" style="width:100px;" />


            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                onclick="Search();">查询</a>

        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
   
</asp:Content>
