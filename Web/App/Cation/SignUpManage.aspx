<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="SignUpManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.SignUpManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script type="text/javascript">
        $(function () {
            var myMenu;
            myMenu = new SDMenu("my_menu");
            myMenu.init();
            var firstSubmenu = myMenu.submenus[5];
            myMenu.expandMenu(firstSubmenu);
        });
    </script>--%>
    <script type="text/javascript">
        var grid;

        //传入的活动ID
        var ActivityID = "<%=ActivityID%>";

        var currSelectUid = 0;

        var currMember;

        var handlerUrl = "/Handler/App/CationHandler.ashx";
        var currSelectUsers = [];

        //加载文档
        jQuery().ready(function () {

            grid = jQuery("#grvData").datagrid({
                method: "Post",
                url: handlerUrl,
                height: document.documentElement.clientHeight - 150,
                pageSize: 20,
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                queryParams: { Action: "QueryActivityData", ActivityID: ActivityID, SearchTitle: "", pmsGroup: '游客' }

            });


            //取消---------------------
            $("#win").find("#btnWinClose").bind("click", function () {
                $("#win").window("close");
            });
            //取消---------------------

            $('#dlgUserPmsGroupSet').dialog({
                buttons: [{
                    text: '保存',
                    handler: function () {

                        var ids = new Array();

                        for (var i = 0; i < currSelectUsers.length; i++) {
                            ids.push(currSelectUsers[i].SignUpUserID
                                );
                        }

                        var modelDate = {
                            Action: 'SetHFUserPmsGroup',
                            userIds: ids.join(','),
                            pmsGroupId: GetCheckGroupVal('UserPmsGroup', 'value')
                        }

                        //alert(modelDate.userIds);
                        //alert(modelDate.pmsGroupId);

                        if (modelDate.userIds == '') {
                            Alert("请选择用户!");
                            return;
                        }

                        if (modelDate.pmsGroupId == '') {
                            Alert("请选择权限组!");
                            return;
                        }

                        //return;

                        $.messager.confirm("系统提示", "确定将当前选择的用户分配到选定权限组？", function (r) {
                            if (r) {
                                $.ajax({
                                    type: 'post',
                                    url: handlerUrl,
                                    data: modelDate,
                                    success: function (result) {
                                        var resp = $.parseJSON(result);

                                        if (resp.Status == 1) {
                                            Alert(resp.Msg);
                                            $('#dlgUserPmsGroupSet').dialog('close');
                                            grid.datagrid('reload');
                                            //$('#grvData').datagrid({ queryParams: { Action: "QueryActivityData", ActivityID: ActivityID, SearchTitle: ""} });
                                        }
                                        else {
                                            Alert(resp.Msg);
                                        }

                                    }
                                });
                            }
                        });
                    }
                },
                    {
                        text: '取消',
                        handler: function () {
                            $('#dlgUserPmsGroupSet').dialog('close');
                        }
                    }
                ]
            });



        });

        // 删除---------------------
        function Delete() {
            var rows = grid.datagrid('getSelections');
            var num = rows.length;
            if (num == 0) {
                messager("系统提示", "请选择您要删除的记录");
                return;
            }
            var ids = [];

            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].UID);
            }

            $.messager.confirm("系统提示", "是否确定删除选中信息?", function (r) {
                if (r) {
                    jQuery.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteActivityData", ActivityID: ActivityID, id: ids.join(',') },
                        success: function (result) {
                            if (result == "true") {
                                messager('系统提示', "删除成功！");
                                grid.datagrid('reload');


                            }
                            else {
                                $.messager.alert("删除失败。");
                            }

                        }
                    });
                }
            });
        };
        // 删除---------------------

        function FormateImg(value) {
            if (value == '' || value == null)
                return "";
            var str = new StringBuilder();
            str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
            return str.ToString();
        }


        function ShowUserGroupSet() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }

            $('#dlgUserPmsGroupSet').dialog('open');

            //1.真实姓名：   ，微信昵称：<br />

            currSelectUsers = new Array();
            var strSelectUserHtml = new StringBuilder();
            for (var i = 0; i < rows.length; i++) {
                currSelectUsers.push(rows[i]);
                strSelectUserHtml.AppendFormat('{0}.真实姓名：{1} ，微信昵称：{2}<br />', i + 1, rows[i].Name == null ? "" : rows[i].Name, rows[i].WXNickname == null ? "" : rows[i].WXNickname);
                //ids.push(rows[i].JuActivityID
                //);
            }

            if (currSelectUsers.length == 1) {
                switch (currSelectUsers[0].HFUserPmsGroup) {
                    case "访客":
                        chkPmsPT.checked = true;
                        break;
                    case "正式学员":
                        chkPmsFF.checked = true;
                        break;
                    case "管理员":
                        chkPmsAdmin.checked = true;
                        break;
                    case "教师":
                        chkPmsTeacher.checked = true;
                        break;
                }
            }

            $('#divSelectUser').html(strSelectUserHtml.ToString());

        }

        function Search() {
            //pmsGroup
            $('#grvData').datagrid({
                queryParams: { Action: "QueryActivityData", ActivityID: ActivityID, SearchTitle: "", pmsGroup: $('#selectPmsGroup').combobox('getText') }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>报名管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-list" plain="true"
                onclick="ShowUserGroupSet()" id="btnUserGroupSet">批量转为正式学院</a> <a href="javascript:;"
                    class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="Delete()">
                    删除</a>
            <br />
            <div style="display: none;">
                &nbsp;&nbsp;&nbsp;&nbsp;所属用户组：
                <select class="easyui-combobox" name="state" id="selectPmsGroup" style="width: 200px;"
                    data-options="multiple:true,panelHeight:'auto'">
                    <option value="游客">游客</option>
                    <option value="正式学员">正式学员</option>
                    <option value="教师">教师</option>
                    <option value="管理员">管理员</option>
                </select>
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                    onclick="Search();">查询</a>
            </div>
        </div>
    </div>
    <table id="grvData" cellspacing="0" cellpadding="0" fitcolumns="true">
        <thead>
            <tr>
                <th field="ck" width="10" checkbox="true">
                </th>
                <th field="HFUserPmsGroup" width="70" formatter="FormatterTitle">
                    用户组
                </th>
                <th field="WXHeadimgurlLocal" width="60" formatter="FormateImg" align="center">
                    头像
                </th>
                <th field="WXNickname" width="80" formatter="FormatterTitle">
                    微信昵称
                </th>
                <%=Columns %>
            </tr>
        </thead>
    </table>
    <div id="dlgUserPmsGroupSet" class="easyui-dialog" closed="true" modal="true" title="用户权限组设置"
        style="width: 350px; height: 280px; padding: 5px;">
        <fieldset>
            <legend>勾选分配权限组：</legend>
            <input type="radio" id="chkPmsFF" name="UserPmsGroup" value="1fd1f" checked /><label
                for="chkPmsFF">正式学员</label>
        </fieldset>
        <br />
        <fieldset>
            <legend>当前已选择用户：</legend>
            <div id="divSelectUser">
            </div>
        </fieldset>
    </div>
</asp:Content>
