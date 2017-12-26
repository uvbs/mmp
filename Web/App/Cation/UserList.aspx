<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.UserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;用户管理&nbsp;&gt;&nbsp;<span>用户列表</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
             <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="Edit();" id="btnEdit">编辑信息</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="EditPassword();" id="btnEdit">修改密码</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="DisableUser(1);" id="btnEdit">批量禁用</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="DisableUser(0);" id=btnEdit">批量启用</a>
            <% if (curUser.UserType ==1 || curUser.WebsiteOwner == new ZentCloud.BLLJIMP.BLL().WebsiteOwner){ %>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true" 
                onclick="ShowEditAddScore();">批量添加<%=moduleName %></a>
            <% }%>
           <br />
             &nbsp;关键字:
            <input type="text" id="txtKeyWord" style="width: 400px;    display: inline-block;padding: 6px;" class="" placeholder="请输入关键字，可根据用户昵称进行模糊匹配" />
       
            <a href="javascript:void(0);" class="easyui-linkbutton" iconcls="icon-search" onclick="Search();">查询</a> 
        </div>
    </div>
       <table id="grvData" fitcolumns="true">
      </table>


        <div id="dlgPassword" class="easyui-dialog" closed="true" title="修改密码" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    新密码:
                </td>
                <td>
                    <input id="txtPassword" type="text" style="width: 200px;" maxlength="20" />
                </td>
            </tr>
        </table>
    </div>
     <div id="dlgPhone" class="easyui-dialog" closed="true" title="编辑" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    手机号码:
                </td>
                <td>
                    <input id="txtPhone" type="text" style="width: 250px;" />
                </td>
            </tr>
             <tr>
                <td>
                    用户昵称:
                </td>
                <td>
                    <input id="txtTrueName" type="text" style="width: 250px;" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgAddScore" class="easyui-dialog" closed="true" title="添加<%=moduleName %>" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    <%=moduleName %>:
                </td>
                <td>
                    <input id="txtAddScore" type="text" style="width: 200px;" maxlength="6" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var url = '/serv/api/admin/user/';
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: url + 'ListByCompany.ashx',
	                queryParams: { user_type: '2' },
	                height: document.documentElement.clientHeight - 120,
	                pagination: true,
	                striped: true,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'autoid', title: '编号', width: 60, align: 'left', formatter: FormatterTitle },
                                { field: 'phone', title: '手机号', width: 100, align: 'center', formatter: FormatterTitle },
                                 //{ field: 'password', title: '密码', width: 100, align: 'center', formatter: FormatterTitle },
                                 { field: 'truename', title: '昵称', width: 100, align: 'center', formatter: FormatterTitle },
                                 { field: 'score', title: '<%= moduleName%>', width: 60, align: 'center', formatter: FormatterTitle },
                                { field: 'insert_time', title: '注册时间', width: 100, align: 'center', formatter: FormatDate },
                                 {
                                     field: 'isdisable', title: '禁用状态', width: 100, align: 'center', formatter: function (value) {
                                         var str = new StringBuilder();
                                         if (value == '1') {
                                             str = "<span style='color:red;'>禁用</span>";
                                         } else if (value == '0') {
                                             str = "<span style='color:green;'>启用</span>";
                                         }
                                         return str.toString();
                                     }
                                 }
	                ]]
	            });


            //修改密码
            $('#dlgPassword').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var dataModel = {
                            Action: "UpdatePassword",
                            pwd: $.trim($('#txtPassword').val()),
                            autoid: GetRowsIds(rows).join(',')

                        }

                        if (dataModel.pwd == '') {
                            Alert("密码不能为空!");
                            return;
                        }
                        if (dataModel.pwd.length < 6) {
                            Alert("长度不能少于6位!");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: url + 'UpdatePassword.ashx',
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#dlgPassword').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                $('#txtPassword').val("");
                                Show(resp.msg);

                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgPassword').dialog('close');
                    }
                }]
            });
            //修改
            $('#dlgPhone').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var dataModel = {
                            
                            phone: $.trim($('#txtPhone').val()),
                            autoid: GetRowsIds(rows).join(','),
                            truename:$("#txtTrueName").val()

                        }

                        if (dataModel.phone == '') {
                            Alert("请输入手机号码!");
                            return;
                        }
                        if (dataModel.phone.length != 11) {
                            Alert("请输入正确的手机号码!");
                            return;
                        }
                        if (dataModel.truename == '') {
                            Alert("请输入昵称!");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: url + 'EditUserInfo.ashx',
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#dlgPhone').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                $('#txtPhone').val("");
                                $("#txtTrueName").val("");
                                Show(resp.msg);
                            }
                        });
                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgPhone').dialog('close');
                    }
                }]
            });

            ///添加<%= moduleName%>对话框
            $('#dlgAddScore').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var dataModel = {
                            Action: "AddScore",
                            AddScore: $.trim($('#txtAddScore').val()),
                            ids: GetRowsIds(rows).join(','),
                            module:'<%=moduleName%>'
                        }

                        if (dataModel.AddScore == '') {
                            Alert("<%= moduleName%>不能为空!");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: '/Handler/App/CationHandler.ashx',
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.Status > 0) {
                                    $('#dlgAddScore').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                else {

                                }
                                Alert(resp.Msg);
                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgAddScore').dialog('close');
                    }
                }]
            });
        });

        ///查询
        function Search() {
            var model = {
                keyword: $("#txtKeyWord").val(),
                user_type: '2'
            };
            $('#grvData').datagrid(
                 {
                     method: "Post",
                     url: url + "ListByCompany.ashx",
                     queryParams: model
                 });
        }
       
        //修改密码
        function EditPassword() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;
            $('#dlgPassword').dialog({ title: '修改密码' });
            $('#dlgPassword').dialog('open');

        }
        //批量禁用、启用
        function DisableUser(disableValue) {

            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;


            currSelectUsers = new Array();

            for (var i = 0; i < rows.length; i++) {
                currSelectUsers.push(rows[i]);
            }

            var ids = new Array();

            for (var i = 0; i < currSelectUsers.length; i++) {
                ids.push("'" + currSelectUsers[i].userid + "'"
                                   );
            }

            var model = {
                Action: 'DisableUser',
                userIds: ids.join(','),
                disableValue: disableValue
            }

            if ($.messager.confirm('友情提示', "确定" + (disableValue == 1 ? "禁用" : "启用") + "当前选择的用户?", function (o) {
              if (o) {
                  $.ajax({
                type: "Post",
                url: '/Handler/App/CationHandler.ashx',
                data: model,
                dataType: "json",
                success: function (resp) {
                            if (resp.Status > 0) {
                                  $('#grvData').datagrid('reload');
                                  Show('操作成功');
                            }
                            else {
                                    Alert('操作失败');
                            }
                        }
                });
              }
            }));
        }

        //编辑信息
        function Edit() {
            var rows = $('#grvData').datagrid('getSelections');

            if (!EGCheckIsSelect(rows))
                return;

            if (!EGCheckNoSelectMultiRow(rows))
                return;
            $("#txtPhone").val(rows[0].phone);
            $("#txtTrueName").val(rows[0].truename);
            $("#dlgPhone").dialog('open');
        }
        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].autoid);

            }
            return ids;
        }//显示添加积分对话框
        function ShowEditAddScore() {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            $('#dlgAddScore').dialog({ title: '添加<%=moduleName%>' });
            $('#dlgAddScore').dialog('open');
        }
    </script>
</asp:Content>
