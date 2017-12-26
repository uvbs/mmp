<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="JointProjectMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WanBang.JointProjectMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;对接项目
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="JointProjectCompile.aspx?Action=add" class="easyui-linkbutton" iconcls="icon-add2" plain="true">添加</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ShowEdit();" >编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true" onclick="Delete()">删除</a>
             <a href="/App/WanBang/Wap/JointProjectList.aspx" target="_blank" class="easyui-linkbutton" iconcls="icon-list" plain="true" >查看手机端对接项目列表</a>
            <div>
                <span style="font-size: 12px; font-weight: normal">项目名称：</span>
                <input type="text" style="width: 200px" id="txtProjectName" />
                <span style="font-size: 12px; font-weight: normal">企业名称：</span>
                <input type="text" style="width: 200px" id="txtCompanyName" />
               <span style="font-size: 12px; font-weight: normal">基地名称：</span>
                <input type="text" style="width: 200px" id="txtBaseName" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">

     var handlerUrl = "/Handler/WanBang/PC.ashx";
     $(function () {
         $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJointProject" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'Thumbnails', title: '缩略图', width: 8, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" height="50" width="50" />', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'ProjectName', title: '项目名称', width: 30, align: 'left' },
                                { field: 'CompanyName', title: '企业名称', width: 30, align: 'left' },
                                { field: 'BaseName', title: '基地名称', width: 30, align: 'left' }


                             ]]
	            }
            );






         $("#btnSearch").click(function () {

             $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "QueryJointProject", ProjectName: $("#txtProjectName").val(), CompanyName: $("#txtCompanyName").val(), BaseName: $("#txtBaseName").val()} });
         });



     });

     function Delete() {
         try {

             var rows = $('#grvData').datagrid('getSelections');

             if (!EGCheckIsSelect(rows))
                 return;

             $.messager.confirm("系统提示", "确认删除选中项目?", function (r) {
                 if (r) {
                     var ids = [];

                     for (var i = 0; i < rows.length; i++) {
                         ids.push(rows[i].AutoID);
                     }

                     var dataModel = {
                         Action: 'DeleteJointProject',
                         ids: ids.join(',')
                     }

                     $.ajax({
                         type: 'post',
                         url: handlerUrl,
                         data: dataModel,
                         success: function (result) {
                             Alert(result);
                             $('#grvData').datagrid('reload');
                         }
                     });
                 }
             });

         } catch (e) {
             Alert(e);
         }
     }

     function ShowEdit() {
         var rows = $('#grvData').datagrid('getSelections');

         if (!EGCheckIsSelect(rows))
             return;

         if (!EGCheckNoSelectMultiRow(rows))
             return;

         window.location.href = "JointProjectCompile.aspx?Action=edit&Id=" + rows[0].AutoID;

     }


    </script>
</asp:Content>