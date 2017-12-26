<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="SystemNoticeManageAdd.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Sys.SystemNoticeManageAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 30px;
        }
        input[type=text], select
        {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp; >系统通知
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtTitle" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        内容类型：
                    </td>
                    <td width="*" align="left">
                        <select id="txtType">
                            <option value="1">系统消息</option>
                            <option value="21">问卷调查</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        发送类型：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlSendType">
                            <option value="0">全部</option>
                            <option value="1">分组</option>
                            <option value="2">个人</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        接收人：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtUserID" onclick="ShowUserID();" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        跳转链接：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="RedirectUrl" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        内容：
                    </td>
                    <td width="*" align="left">
                        <div id="divEditor">
                            <div id="txtNcontent" style="width: 100%; height: 400px;">
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                        <input type="hidden" id="Aid" value="0" />
                        <a href="javascript:;" id="btnSave" style="font-weight: bold; width: 200px;" class="button button-rounded button-primary">
                            确定发送</a> <a href="SystemNoticeManage.aspx" style="font-weight: bold; width: 200px;"
                                class="button button-rounded button-flat">返回</a>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </div>
    <div id="dlgUserID" class="easyui-dialog" closed="true" title="" style="width: 550px;
        height: 400px; padding: 15px;">
        <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
            <div style="margin-bottom: 5px">
                <label style="margin-left: 8px;">
                    查找:</label>
                <input type="text" id="txtTitles" style="width: 200px;height:25px;border-radius:0px;" />
                <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-search" id="btnSearch"
                    onclick="Search();">查询</a>
            </div>
        </div>
        <table id="grvUserID" fitcolumns="true">
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    var handlerUrl = "/Handler/App/CationHandler.ashx";
    var editor;
    var methodName = "QueryWebsiteUser";
    var tagFieldName = "UserID";
    $(function () {
        $("#txtUserID").hide();
        $("#ddlSendType").change(function () {
            if ($("#ddlSendType").val() == "0") {
                $("#txtUserID").val("");
                $("#txtUserID").hide();
            } else {
                $("#txtUserID").show();

            }
            if ($("#ddlSendType").val() == "1") {
                methodName = "QueryMemberTag";
                $("#txtUserID").click();
            }
            if ($("#ddlSendType").val() == "2") {
                methodName = "QueryWebsiteUser";
                $("#txtUserID").click();
            }


        });

        $('#btnSave').click(function () {

            try {

                var model =
                    {

                        Action: "AddSystemNotice",
                        Title: $.trim($('#txtTitle').val()),
                        Ncontent: editor.html(),
                        MessageType: $.trim($('#txtType').val()),
                        SendType: $.trim($('#ddlSendType').val()),
                        Receive: $.trim($('#txtUserID').val()),
                        RedirectUrl: $.trim($("#RedirectUrl").val())

                    };
                if (model.Title == "") {
                    $('#txtTitle').focus();
                    return false;
                }
                $.messager.progress({ text: '正在处理...' });
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: model,
                    dataType: 'json',
                    success: function (resp) {
                        $.messager.progress('close');
                        if (resp.Status == 1) {
                            alert(resp.Msg);
                            window.location.href = "SystemNoticeManage.aspx";
                        }
                        else {
                            Alert(resp.Msg);
                        }
                    }
                });

            } catch (e) {
                Alert(e);
            }
        });


        $('#dlgUserID').dialog({
            buttons: [{
                text: '确定',
                handler: function () {
                    var rowsUserID = $('#grvUserID').datagrid('getSelections');
                    var UserID = [];
                    for (var i = 0; i < rowsUserID.length; i++) {
                        if ($("#ddlSendType").val() == "1") {
                            UserID.push(rowsUserID[i].TagName);
                        }
                        else {
                            UserID.push(rowsUserID[i].UserID);
                        }

                    }

                    $("#txtUserID").val(UserID.join(','));
                    $("#dlgUserID").dialog('close');


                }
            }, {
                text: '取消',
                handler: function () {
                    $('#dlgUserID').dialog('close');
                }
            }]
        });

    });


    KindEditor.ready(function (K) {
        editor = K.create('#txtNcontent', {
            uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
            items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
            filterMode: false
        });
    });

    function ShowUserID() {

        if ($("#ddlSendType").val() == "1") {
            // methodName = "QueryMemberTag";
            tagFieldName = "TagName";
        }
        else {
            tagFieldName = "UserID";
        }
        $('#grvUserID').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: methodName },
	                height: 400,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                 { field: tagFieldName, title: '名称', width: 50, align: 'left', formatter: FormatterTitle }
                              ]]
	            });

        // var rows = $('#grvUserID').datagrid('getSelections');
        $('#dlgUserID').dialog({ title: '选择接收人' });
        $('#dlgUserID').dialog('open');

    }

    function Search() {
        $('#grvUserID').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: methodName, UserId: $("#txtTitles").val() },
	                height: 400,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                singleSelect: false,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: tagFieldName, title: '名称', width: 50, align: 'left', formatter: FormatterTitle }
                              ]]
	            });


    }
    </script>
</asp:Content>
