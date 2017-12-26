<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="LawyerApplyList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.User.LawyerApplyList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;用户管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;律师列表
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnSetLawyer" onclick="ActionEvent('setLawyer','确定所选用户审核通过设置为律师?')">批量审核通过</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true" id="btnSetUser" onclick="ActionEventNo('setUser')">批量审核不通过</a>
            关键字匹配:<input id="txtKeyword" style="width: 200px;"  placeholder="用户名，姓名，邮箱" />
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch" onclick="Search();">查询</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
     <div id="dlgInfo1" class="easyui-dialog" closed="true" title="系统消息发送" style="width: 400px;padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    消息:
                </td>
                <td>
                    <textarea id="txtNotice"  style="width: 300px;" rows="8"></textarea>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">

        var handlerUrl = "Handler/UserHandler.ashx";
        var currDlgAction = '';
        var domain = '<%=Request.Url.Host%>';

     $(function () {
         $('#grvData').datagrid({
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "getUserList",type:4 },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                        { title: 'ck', width: 5, checkbox: true },
                        //{ field: 'userId', title: '用户名', width: 80, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'avatar', title: '头像', width: 40, align: 'center', formatter: function (value) {
                                if (value == '' || value == null)
                                    return "";
                                var str = new StringBuilder();
                                str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
                                return str.ToString();
                            }
                        },
                        {
                            field: 'name', title: '姓名', width: 50, align: 'left', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('<a href="/#/userspace/{1}" title="{0}" target="_blank">{0}</a>', value, rowData.id);
                                return str.ToString();
                            }
                        },
                        { field: 'email', title: '邮箱', width: 80, align: 'left', formatter: FormatterTitle },
                        {
                            field: 'IDPhoto1', title: '身份证照片', width: 100, align: 'left', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                if (value != null && value != '')
                                    str.AppendFormat(' <a href="{0}" target="_blank"><img alt="" class="imgAlign" src="{0}"  height="50" width="50" /></a> ', value);

                                if (rowData.IDPhoto2 != null && rowData.IDPhoto2 != '')
                                    str.AppendFormat(' <a href="{0}" target="_blank"><img alt="" class="imgAlign" src="{0}"  height="50" width="50" /></a> ', rowData.IDPhoto2);

                                if (str.ToString() != "") { str.AppendFormat('<br />'); }
                                str.AppendFormat('{0}', rowData.idCardNo);
                                return str.ToString();
                            }
                        },
                        {
                            field: 'lawyerLicensePhoto', title: '执业证', width: 40, align: 'left', formatter: function (value) {
                            if (value == '' || value == null)
                                return "";
                            var str = new StringBuilder();
                            str.AppendFormat('<a href="{0}" target="_blank"><img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="60" /></a>', value);
                            if (str.ToString() != "") { str.AppendFormat('<br />'); }
                            str.AppendFormat('{0}', rowData.lawyerLicenseNo);
                            return str.ToString();
                        } },
                        { field: 'phone', title: '联系方式', width: 80, align: 'left', formatter: function (value, rowData) {
                            var str = new StringBuilder();
                            str.AppendFormat('手机：<span title="{0}">{0}</span><br />', value);
                            str.AppendFormat('座机：<span title="{0}">{0}</span>', rowData["tel"]);
                            return str.ToString();
                        } },
                        {
                            field: 'company', title: '公司', width: 100, align: 'left', formatter: function (value, rowData) {
                                var str = new StringBuilder();
                                str.AppendFormat('公司：<span title="{0}">{0}</span><br />', value);
                                str.AppendFormat('地址：<span title="{0}">{0}</span>', rowData["companyAddress"]);
                                return str.ToString();
                            }
                        }
	                ]]
	            }
            );
     });

     //获取选中行ID集合
     function GetRowsIds(rows) {
         var ids = [];
         for (var i = 0; i < rows.length; i++) {
             ids.push(rows[i].userId);
         }
         return ids;
     }

     //删除
     function ActionEvent(action,msg) {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

         if (!EGCheckIsSelect(rows)) {
             return;
         }
         $.messager.confirm("系统提示", msg, function (o) {
             if (o) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: action, ids: GetRowsIds(rows).join(',') },
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

     function ActionEventNo(action) {
         var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

         if (!EGCheckIsSelect(rows)) {
             return;
         }

         $.messager.prompt('审核不通过', '请填写原因', function (r) {
             if (r) {
                 $.ajax({
                     type: "Post",
                     url: handlerUrl,
                     data: { Action: action, ids: GetRowsIds(rows).join(','),ly:r },
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
         $('#grvData').datagrid({
	        method: "Post",
	        url: handlerUrl,
	        queryParams: { Action: "getUserList", type: 4, keyword: $("#txtKeyword").val() }
	    });
     }
    </script>
</asp:Content>
