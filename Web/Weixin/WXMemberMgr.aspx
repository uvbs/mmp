<%@ Page Title="" Language="C#" MasterPageFile="~/EasyUI.Master" AutoEventWireup="true"
    CodeBehind="WXMemberMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Weixin.WXMemberMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var grid;
        //处理文件路径
        var url = "/Handler/WeiXin/WeixinHandler.ashx";
        //加载文档

        jQuery().ready(function () {


            $(window).resize(function () {
                $('#grvData').datagrid('resize', {
                    height: function () { return document.documentElement.clientHeight - 70; }
                });
            });
            //-----------------加载gridview
            grid = jQuery("#grvData").datagrid({
                method: "Post",
                url: url,
                height: document.documentElement.clientHeight - 70,
                pageSize: 20,
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                fitColumns: true,
                queryParams: { Action: "QueryWXMember" }
            });
            //------------加载gridview



            //搜索开始------------------------
            $("#btnSearch").click(function () {
                var UserID = $("#txtUserID").val();
                var name = $("#txtSearchName").val();
                var openID = $("#txtOpenID").val();
                var phone = $("#txtSearchPhone").val();
                grid.datagrid({ url: url, queryParams: { Action: "QueryWXMember", UserID: UserID, Name: name, OpenID: openID, Phone: phone} });
            });
            //搜索结束---------------------

            //批量删除
            $("#btnDelete").click(function () {

                var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

                if (!EGCheckIsSelect(rows)) {
                    return;
                }
                $.messager.confirm("系统提示", "确定删除选中会员?", function (o) {
                    if (o) {
                        $.ajax({
                            type: "Post",
                            url: url,
                            data: { Action: "DeleteWXMember", ids: GetRowsIds(rows).join(',') },
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


            });


            //弹出编辑框
            $("#btnEdit").click(function () {

                var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

                if (!EGCheckNoSelectMultiRow(rows)) {
                    return;
                }


                var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

                $("#txtName").val(rows[0].Name);
                Phone: $("#txtPhone").val(rows[0].Phone);
                Email: $("#txtEmail").val(rows[0].Email);
                Company: $("#txtCompany").val(rows[0].Company);
                Postion: $("#txtPostion").val(rows[0].Postion);


                $("#dlgMemberInfo").dialog("open");








            });


            $("#dlgMemberInfo").dialog({
                buttons: [
                           {
                               text: '确定',
                               handler: function () {
                                   var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
                                   $.messager.progress({ text: '正在处理。。。' });
                                   $.ajax({
                                       type: 'post',
                                       url: url,
                                       data: { Action: "EditWXMember", id: rows[0].MemberID, Name: $("#txtName").val(), Phone: $("#txtPhone").val(), Email: $("#txtEmail").val(), Company: $("#txtCompany").val(), Postion: $("#txtPostion").val() },
                                       success: function (result) {
                                           $.messager.progress('close');
                                           var resp = $.parseJSON(result);
                                           if (resp.Status == 1) {
                                               try {
                                                   $("#dlgMemberInfo").dialog('close');
                                                   Show(resp.Msg);
                                                   $('#grvData').datagrid('reload');

                                               } catch (e) {
                                                   alert(e);
                                               }

                                           }
                                           else {
                                               Alert(resp.Msg);
                                           }
                                       }
                                   });

                               }
                           },
                           {
                               text: '取消',
                               handler: function () {

                                   $("#dlgMemberInfo").dialog('close');
                               }
                           }
                           ]
            });

        });

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].MemberID
                 );
            }
            return ids;
        }

        function FormatImg(value) {
            if (value == '' || value == null)
                return '';
            else
                return '<img src="' + value + '" height="40px" width="40px" />';
        }
 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div>
            <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto">
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                    id="btnEdit">编辑</a> <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete"
                        plain="true" id="btnDelete">批量删除</a>
                <div style="margin-bottom: 5px">
                    <% if (Pms_WX_ViewAllMember)
                       { %>
                    <span style="font-size: 12px; font-weight: normal">所属用户名：</span>
                    <input type="text" style="width: 200px" id="txtUserID" />
                    <%}%>
                    <span style="font-size: 12px; font-weight: normal">姓名：</span>
                    <input type="text" style="width: 120px" id="txtSearchName" />
                    <span style="font-size: 12px; font-weight: normal">手机：</span>
                    <input type="text" style="width: 120px" id="txtSearchPhone" />
                    <span style="font-size: 12px; font-weight: normal">OpenID：</span>
                    <input type="text" style="width: 200px" id="txtOpenID" />
                    <a href="#" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch">查询</a>
                </div>
            </div>
            <table id="grvData" cellspacing="0" cellpadding="0" fitcolumns="true">
                <thead>
                    <tr>
                        <th field="ck" width="10" checkbox="true">
                        </th>
                        <%--<th field="WXHeadimgurl" width="10" formatter="FormatImg">
                            微信头像
                        </th>--%>
                        <th field="WXNickname" width="20">
                            微信昵称
                        </th>
                        <% if (Pms_WX_ViewAllMember)
                           { %>
                        <th field="UserID" width="20">
                            用户名
                        </th>
                        <% 
                            }%>
                        <th field="Name" width="20">
                            姓名
                        </th>
                        <th field="Company" width="30">
                            公司
                        </th>
                        <th field="Postion" width="20">
                            职位
                        </th>
                        <th field="Phone" width="30">
                            手机号码
                        </th>
                        <th field="Email" width="20">
                            Email
                        </th>
                        <th field="WeixinOpenID" width="30">
                            OpenID
                        </th>
                        <th field="InsertDate" width="20" formatter="FormatDate">
                            注册时间
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <div id="dlgMemberInfo" class="easyui-dialog" closed="true" title="编辑" modal="true"
        style="width: 280px; height: 230px; padding: 10px;">
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
            <tr>
                <td height="25" width="20%" align="left">
                    姓名:
                </td>
                <td height="25" width="*" align="left">
                    <input id="txtName" type="text" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td height="25" width="20%" align="left">
                    手机号码:
                </td>
                <td height="25" width="*" align="left">
                    <input id="txtPhone" type="text" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td height="25" width="20%" align="left">
                    Email
                </td>
                <td height="25" width="*" align="left">
                    <input id="txtEmail" type="text" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td height="25" width="20%" align="left">
                    公司:
                </td>
                <td height="25" width="*" align="left">
                    <input id="txtCompany" type="text" style="width: 200px;" />
                </td>
            </tr>
            <tr>
                <td height="25" width="20%" align="left">
                    职位:
                </td>
                <td height="25" width="*" align="left">
                    <input id="txtPostion" type="text" style="width: 200px;" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
