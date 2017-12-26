<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="QuestionManage.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.QuestionManage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<span>问答管理</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="DeleteQuestion()">批量删除问题</a>
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
    <div id="dlgReplay" class="easyui-dialog" closed="true" modal="true" title="问题回复"
        style="width: 550px; height: 350px;">
        <table id="grvReplayData" fitcolumns="true">
        </table>
    </div>
    <%--<script type="text/javascript">
        $(function () {
            var myMenu;
            myMenu = new SDMenu("my_menu");
            myMenu.init();
            var firstSubmenu = myMenu.submenus[3];
            myMenu.expandMenu(firstSubmenu);
        });
    </script>--%>
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuMasterFeedBackForPCGrid" },
	                height: document.documentElement.clientHeight - 145,
	                pagination: true,
	                striped: true,
	                pageSize: 20,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'HFUserPmsGroup', title: '用户组', width: 60, align: 'left', formatter: FormatterTitle },
                                { field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'WXNickname', title: '微信昵称', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'SortContent', title: '问题内容', width: 200, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat("<span title=\"{0}\">{1}<span>", rowData.FeedBackContent, rowData.SortContent);
                                    return str.ToString();
                                }
                                }, { field: 'ProcessStatus', title: '问题状态', width: 100, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    switch (value) {
                                        case '未处理':
                                            str.AppendFormat("<span title=\"{0}\" style=\"color:red;\">{0}<span>", value);
                                            break;
                                        case '已回复':
                                            str.AppendFormat("<a href=\"javascript:;\" onclick=\"ShowReplay({1},'{2}')\" title=\"点击查看回复内容\" style=\"color:green;\">{0}<a>", value, rowData.FeedBackID, rowData.SortContent);
                                            break;
                                        default:
                                            return value;
                                    }
                                    return str.ToString();
                                }
                                },
                                { field: 'SubmitDate', title: '提交时间', width: 100, align: 'left', formatter: function (value) {
                                    var timeStr = FormatDate(value);
                                    var str = new StringBuilder();
                                    str.AppendFormat("<span title=\"{0}\">{0}<span>", timeStr);
                                    return str.ToString();

                                }
                                }
                             ]]
	            }
            );

            $('#grvReplayData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "QueryJuMasterFeedBackDialogueForPCGrid" },
	                height: 280,
	                pagination: true,
	                striped: true,
	                pageSize: 10,
	                rownumbers: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'HFUserPmsGroup', title: '用户组', width: 60, align: 'left', formatter: FormatterTitle },
                                { field: 'WXHeadimgurlLocal', title: '头像', width: 50, align: 'center', formatter: function (value) {
                                    if (value == '' || value == null)
                                        return "";
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a href="javascript:;" "><img alt="" class="imgAlign" src="{0}" title="头像缩略图" height="25" width="25" /></a>', value);
                                    return str.ToString();
                                }
                                },
                                { field: 'WXNickname', title: '微信昵称', width: 100, align: 'left', formatter: FormatterTitle },
                                { field: 'SortContent', title: '回复内容', width: 200, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat("<span title=\"{0}\">{1}<span>", rowData.DialogueContent, rowData.SortContent);
                                    return str.ToString();
                                }
                                },
                                { field: 'SubmitDate', title: '回复时间', width: 100, align: 'left', formatter: function (value) {
                                    var timeStr = FormatDate(value);
                                    var str = new StringBuilder();
                                    str.AppendFormat("<span title=\"{0}\">{0}<span>", timeStr);
                                    return str.ToString();
                                }
                                }
                             ]]
	            }
            );

            $('#dlgReplay').dialog({
                toolbar: [{
                    text: '批量删除回复',
                    iconCls: 'icon-delete',
                    handler: function () {

                        var rows = $('#grvReplayData').datagrid('getSelections'); //获取选中的行

                        if (!EGCheckIsSelect(rows)) {
                            return;
                        }

                        var ids = [];

                        for (var i = 0; i < rows.length; i++) {
                            ids.push(rows[i].DialogueID
                            );
                        }

                        $.messager.confirm("系统提示", "确定删除选中回复?", function (o) {
                            if (o) {
                                $.ajax({
                                    type: "Post",
                                    url: handlerUrl,
                                    data: { Action: "DeleteJuMasterFeedBackDialogue", ids: ids.join(',') },
                                    success: function (result) {
                                        var resp = $.parseJSON(result);
                                        if (resp.Status == 1) {
                                            $('#grvReplayData').datagrid('reload');
                                            Show(resp.Msg);
                                        }
                                        else {
                                            Alert(resp.Msg);
                                        }
                                    }

                                });
                            }
                        });


                        //alert('edit')

                    }
                }]
            });

        });

        function DeleteQuestion(FeedBackID) {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行

            if (!EGCheckIsSelect(rows)) {
                return;
            }

            var ids = [];

            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].FeedBackID
                            );
            }

            $.messager.confirm("系统提示", "确定删除选中回复?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DeleteJuMasterFeedBack", ids: ids.join(',') },
                        success: function (result) {
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

        function ShowReplay(FeedBackID, Title) {
            $('#grvReplayData').datagrid(
	            {
	                queryParams: { Action: "QueryJuMasterFeedBackDialogueForPCGrid", FeedBackID: FeedBackID }
	            });

            $('#dlgReplay').dialog({
                title: '问题回复：' + Title
            });

            $('#dlgReplay').dialog('open');

            //alert('ShowReplay');
        }


    </script>
</asp:Content>
