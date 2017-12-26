<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Meifan.Activity.SignUp.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;活动&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>活动报名管理</span>
    <a href="../List.aspx" style="float: right; margin-right: 20px;" title="返回"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">

            <div>

                <input type="text" id="txtKeyWord" placeholder="姓名,手机" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>


            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/serv/api/admin/meifan/activity/signup/";
        $(function () {
            $('#grvData').datagrid(
                   {
                       method: "Post",
                       url: handlerUrl + "list.ashx",
                       queryParams: { activity_id: "<%=Request["activity_id"]%>" },
                       height: document.documentElement.clientHeight - 112,
                       pagination: true,
                       striped: true,
                       loadFilter: pagerFilter,
                       pageSize: 50,
                       rownumbers: true,
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   { field: 'Name', title: '姓名', width: 20, align: 'left' },
                                   { field: 'Phone', title: '手机', width: 20, align: 'left' },
                                   <%if (activityInfo.IsFee==1){%>
	                                
		                            {
		                                field: 'IsPay', title: '付款状态', width: 10, align: 'left', formatter: function (value, rowData) {

		                                    var str = new StringBuilder();
		                                    switch (value) {
		                                        case 1:
		                                            str.AppendFormat('<font color="green">已付款</font>');
		                                            break;
		                                        case 0:
		                                            str.AppendFormat('<font color="red">未付款</font>');
		                                            break;

		                                            break;
		                                        default:

		                                    }

		                                    return str.ToString();
		                                }


		                            },
	                                <%}%>
                                   
                                   { field: 'Sex', title: '性别', width: 10, align: 'left' },
                                   { field: 'Email', title: 'Eamil', width: 20, align: 'left' },
                                   { field: 'BirthDay', title: '生日', width: 20, align: 'left' },
                                   { field: 'DateRange', title: '时间段', width: 30, align: 'left' },
                                   { field: 'GroupType', title: '组别', width: 20, align: 'left' },
                                   { field: 'IsMember', title: '是否会员', width: 10, align: 'left' },
                                    <%if (activityInfo.IsFee==1){%>
                                   { field: 'Amount', title: '金额', width: 20, align: 'left' },<%}%>
                                   { field: 'UserRemark', title: '用户备注', width: 20, align: 'left' }

                       ]]
                   }
            );

            $("#btnSearch").click(function () {

                Search();


            })


        })

               function Search() {

                   $('#grvData').datagrid({ url: handlerUrl + "list.ashx", queryParams: { activity_id: "<%=Request["activity_id"]%>", keyword: $("#txtKeyWord").val() } });
               }

    </script>
</asp:Content>
