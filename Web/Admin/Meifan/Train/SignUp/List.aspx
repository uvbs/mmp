<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Meifan.Train.SignUp.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;培训&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>培训报名管理</span>
    <a href="../List.aspx" style="float: right; margin-right: 20px;" title="返回"
        class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                onclick="ShowDlg();">填写导师评语</a>
            <div>

                <input type="text" id="txtKeyWord" placeholder="姓名,手机" />
                <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>


            </div>


        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
     <div id="dlgInput" class="easyui-dialog" closed="true" title="添加" style="width: 600px; padding: 15px; line-height: 30px;height:400px;">
        <table width="100%">


            <tr>
                <td style="width:50px;">导师评语:
                </td>
                <td>
                    <textarea id="txtRemark" ></textarea>

                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "/serv/api/admin/meifan/train/signup/";
        var currSelectID = "";
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
                       singleSelect: true,
                       columns: [[
                                   { title: 'ck', width: 5, checkbox: true },
                                   { field: 'Name', title: '姓名', width: 20, align: 'left' },
                                   { field: 'Phone', title: '手机', width: 20, align: 'left' },
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
                                   { field: 'Sex', title: '性别', width: 10, align: 'left' },
                                   { field: 'Email', title: 'Eamil', width: 20, align: 'left' },
                                   { field: 'BirthDay', title: '生日', width: 20, align: 'left' },
                                   { field: 'DateRange', title: '时间段', width: 30, align: 'left' },
                                   { field: 'GroupType', title: '组别', width: 20, align: 'left' },
                                   { field: 'IsMember', title: '是否会员', width: 10, align: 'left' },
                                   { field: 'Amount', title: '金额', width: 20, align: 'left' },
                                   { field: 'UserRemark', title: '用户备注', width: 20, align: 'left' },
                                   { field: 'Remark', title: '导师评语', width: 20, align: 'left' }

                       ]]
                   }
            );

            $('#dlgInput').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {
                        try {
                            var dataModel = {

                                id: currSelectID,
                                remark: editor.html()

                            }


                            if (dataModel.remark == '') {


                                return;
                            }
                            $.ajax({
                                type: 'post',
                                url: handlerUrl + "update.ashx",
                                data: dataModel,
                                dataType: "json",
                                success: function (resp) {
                                    if (resp.status == true) {
                                        Show("操作成功");
                                        $('#dlgInput').dialog('close');
                                        $('#grvData').datagrid('reload');

                                    }
                                    else {
                                        Alert(resp.msg);
                                    }


                                }
                            });

                        } catch (e) {
                            Alert(e);
                        }
                    }
                }, {
                    text: '取消',
                    handler: function () {

                        $('#dlgInput').dialog('close');
                    }
                }]
            });



            $("#btnSearch").click(function () {

                Search();


            })


        })

               function Search() {

                   $('#grvData').datagrid({ url: handlerUrl + "list.ashx", queryParams: { activity_id: "<%=Request["activity_id"]%>", keyword: $("#txtKeyWord").val() } });
               }


               function ShowDlg() {

                   var rows = $('#grvData').datagrid('getSelections');

                   if (!EGCheckIsSelect(rows))
                       return;

                   if (!EGCheckNoSelectMultiRow(rows))
                       return;
                   currSelectID = rows[0].AutoId;
                   $('#dlgInput').dialog({ title: '填写成绩单' });
                   $('#dlgInput').dialog('open');
                   $("#dlgInput input").val("");



               }


               KindEditor.ready(function (K) {
                   editor = K.create('#txtRemark', {
                       uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
                       items: [
           'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'bold', 'italic', 'underline',
           'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
           'insertunorderedlist', '|', 'importword', 'video', 'image', 'multiimage', 'link', 'unlink', 'lineheight', '|', 'baidumap', '|', 'template', '|', 'table', 'cleardoc'],
                       filterMode: false,
                       width: "100%",
                       height: "300px",
                   });
               });

    </script>
</asp:Content>