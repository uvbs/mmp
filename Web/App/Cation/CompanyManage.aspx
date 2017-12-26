<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CompanyManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.CompanyManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
     当前位置：&nbsp;公司管理&nbsp;&gt;&nbsp;<span>公司列表</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
      <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="Edit();" id="btnEdit">编辑</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="StatusPassed(9);" id="btnAdd">标记为审核通过</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="StatusPassed(2);" id="btnAdd">标记为审核未通过</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="EditPassword();" id="btnEdit">修改密码</a>
             <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="DisableUser(1);" id="btnEdit">批量禁用</a>
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="DisableUser(0);" id="btnEdit">批量启用</a>
           <br />
             &nbsp;关键字:
            <input type="text" id="txtKeyWord" style="width: 400px;    display: inline-block;padding: 6px;" class="" placeholder="请输入关键字，可根据公司名称进行模糊匹配" />
            <select id="sStatus">
                <option value="">全部</option>
                <option value="1">待审核</option>
                <option value="9">审核通过</option>
                <option value="2">审核失败</option>
            </select>
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
                    <input id="txtPassword" type="text" style="width: 200px;" />
                </td>
            </tr>
        </table>
    </div>
     <div id="dlgCompany" class="easyui-dialog" closed="true" title="编辑" style="width: 400px; padding: 15px;">
        <table width="100%">
            <tr>
                <td>
                    公司名称:
                </td>
                <td>
                    <input id="txtCompany" type="text" style="width: 250px;" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript">
        var url = '/serv/api/admin/user/';
        var currSelectUsers = [];
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: url + 'listbycompany.ashx',
	                queryParams: { user_type: '6' },
	                height: document.documentElement.clientHeight - 120,
	                pagination: true,
	                striped: true,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'autoid', title: '编号', width: 60, align: 'left', formatter: FormatterTitle },
                                { field: 'userid', title: '用户名', width: 100, align: 'center' },
                                { field: 'company', title: '公司名称', width: 100, align: 'center' },
                                { field: 'insert_time', title: '注册时间', width: 100, align: 'center', formatter: FormatDate },
                                {
                                    field: 'apply_status', title: '审核状态', width: 100, align: 'center', formatter: function (value) {
                                        var str = new StringBuilder();
                                        if (value == '1') {
                                            str = "<span style='color:red;'>待审核</span>";
                                        } else if (value == '2') {
                                            str = "<span style='color:red;'>审核失败</span>";
                                        } else {
                                            str = "<span style='color:green;'>审核通过</span>";
                                        }
                                        return str.toString();
                                    }
                                },
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
                            pwd: $.trim($('#txtPassword').val()),
                            autoid: GetRowsIds(rows).join(',')
                        }

                        if (dataModel.pwd == '') {
                            Alert("密码不能为空!");
                            return;
                        }
                        if (dataModel.pwd.length < 5 || dataModel.pwd.length>8) {
                            Alert("长度不能少于5位并且不能大于8位!");
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
            $('#dlgCompany').dialog({
                buttons: [{
                    text: '确定',
                    handler: function () {
                        var rows = $('#grvData').datagrid('getSelections');
                        var dataModel = {
                            company: $.trim($('#txtCompany').val()),
                            autoid: GetRowsIds(rows).join(',')
                        }

                        if (dataModel.company == '') {
                            Alert("公司名称不能为空!");
                            return;
                        }
                        if (dataModel.company.length > 50) {
                            Alert("字数长度不能大于50!");
                            return;
                        }
                        $.ajax({
                            type: 'post',
                            url: url + 'EditUserInfo.ashx',
                            data: dataModel,
                            dataType: "json",
                            success: function (resp) {
                                if (resp.status) {
                                    $('#dlgCompany').dialog('close');
                                    $('#grvData').datagrid('reload');
                                }
                                $('#dlgCompany').val("");
                                Show(resp.msg);

                            }
                        });

                    }
                }, {
                    text: '取消',
                    handler: function () {
                        $('#dlgCompany').dialog('close');
                    }
                }]
            });
        });

        ///查询
        function Search() {
            var model = {
                keyword: $("#txtKeyWord").val(),
                user_type: '6',
                apply_status:$("#sStatus").val()
            };
            $('#grvData').datagrid(
                 {
                     method: "Post",
                     url: url + "ListByCompany.ashx",
                     queryParams: model
                 });
        }
        //审核
        function StatusPassed(status) {
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            var str='';
            if(status==2){
                str='确定标记为审核未通过吗?';
            }else{
                str='确定标记为审核通过吗?';
            }
            if ($.messager.confirm('友情提示', str, function (o) {
                if (o) {
                    $.ajax({
                    type: "Post",
                    url: url + 'UpdateMemberStatus.ashx',
                    data: { ids: GetRowsIds(rows).join(','), status: status },
                    dataType: "json",
                    success: function (resp) {
                             if (resp.status) {
                                $('#grvData').datagrid('reload');
                                Show(resp.msg);
                            }
                            else {
                                Alert(resp.msg);
                            }
                    }
                    });
                }
            }));
            

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
                    data:model,
                    dataType: "json",
                    success: function (resp) {
                           if (resp.Status>0) {
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
        function Edit(){
            var rows = $('#grvData').datagrid('getSelections');
            if (!EGCheckIsSelect(rows))
                return;
            if (!EGCheckNoSelectMultiRow(rows))
                return;
            $("#txtCompany").val(rows[0].company);
            $("#dlgCompany").dialog('open');
        }

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].autoid);

            }
            return ids;
        }
    </script>
</asp:Content>
