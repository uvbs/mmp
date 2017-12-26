<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="WXWordsQuestionsMgr.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.WuBuHui.WebAdmin.WXWordsQuestionsMgr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var handlerUrl = "/Handler/App/WXWuBuHuiUserHandler.ashx";
        var reviewHandlerUrl = "/Handler/App/ReviewHandler.ashx";
        $(function () {
            $('#grvData').datagrid(
	            {
	                method: "Post",
	                url: handlerUrl,
	                queryParams: { Action: "GetReviewInfos" },
	                height: document.documentElement.clientHeight - 112,
	                pagination: true,
	                striped: true,
	                pageSize: 50,
	                rownumbers: true,
	                animate: true,
	                rowStyler: function () { return 'height:25px'; },
	                columns: [[
                                { title: 'ck', width: 5, checkbox: true },
                                { field: 'UserName', title: '话题人', width: 10, align: 'left' },
                                { field: 'ReviewTitle', title: '话题名称', width: 10, align: 'left' },
                                { field: 'ForeignkeyName', title: '导师名称', width: 5, align: 'left' },
                                { field: 'ReviewPower', title: '权限', width: 5, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    if (value == 0) {
                                        return "公开";
                                    } else {
                                        return "不公开";
                                    }
                                }
                                },
                                { field: 'NumCount', title: '回复数量', width: 5, align: 'left', formatter: function (value, rowData) {
                                    var str = new StringBuilder();
                                    str.AppendFormat('<a title="点击查看回复人" href="/App/Review/ReplyReviewInfoMgr.aspx?id={0}" >{1}</a>', rowData.AutoId, value);
                                    return str.ToString();
                                }
                                },
                                { field: 'PraiseNum', title: '顶', width: 5, align: 'left' },
                                { field: 'StepNum', title: '踩', width: 5, align: 'left' },
                                { field: 'InsertDate', title: '评论时间', width: 10, align: 'left', formatter: FormatDate },
                                { field: 'AuditStatusString', title: '审核状态', width: 10, align: 'left' }
                             ]]
	            }
            );

            $("#btnSearch").click(function () {
                var VoteID = $("#ddlVote").val();
                $('#grvData').datagrid({ url: handlerUrl, queryParams: { Action: "GetReviewInfos", ReviewType: VoteID} });
            });

        })
        //删除
        function Delete() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定删除选中?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: handlerUrl,
                        data: { Action: "DelReviewInfos", ids: GetRowsIds(rows).join(',') },
                        success: function (result) {
                            var resp = $.parseJSON(result);
                            if (resp.Status == 0) {
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

        function BatchAudit() {
            var rows = $('#grvData').datagrid('getSelections'); //获取选中的行
            if (!EGCheckIsSelect(rows)) {
                return;
            }
            $.messager.confirm("系统提示", "确定修改审核状态吗?", function (o) {
                if (o) {
                    $.ajax({
                        type: "Post",
                        url: reviewHandlerUrl,
                        data: { Action: "BatchAudit", ids: GetRowsIds(rows).join(','), status: $("#ddlAudit").val() },
                        success: function (result) {
                            var resp = $.parseJSON(result);
                            if (resp.Status == 0) {
                                $('#grvData').datagrid('reload');
                                Show(resp.Msg);
                            }
                            else {
                                Alert(resp.Msg);
                            }
                        }
                        , error: function (msg) { Show(msg); }
                    });
                }
            });

        }

        //获取选中行ID集合
        function GetRowsIds(rows) {
            var ids = [];
            for (var i = 0; i < rows.length; i++) {
                ids.push(rows[i].AutoId);
            }
            return ids;
        }

        //配置是否显示评论
        function ConfigReviewInfo() {
            $('#dlgPmsInfo').window(
            {
                title: '配置是否显示评论'
            }
            );

            $.post(handlerUrl, { Action: "GetReviewConFig" }, function (data) {
                var resp2 = $.parseJSON(data);
                if (resp2.Status == 0) {
                    $('#dlgPmsInfo').dialog('open');

                    if (resp2.ExObj.VoteId == 1)
                        ckIsVote.checked = true;
                    else
                        ckIsVote.checked = false;

                    if (resp2.ExObj.Article == 1)
                        ckIsArticle.checked = true;
                    else
                        ckIsArticle.checked = false;

                    if (resp2.ExObj.Activity == 1)
                        ckIsActivity.checked = true;
                    else
                        ckIsActivity.checked = false;
                }
            });
        }
        //窗体关闭按钮---------------------
        $("#btnExit").live("click", function () {
            $("#dlgPmsInfo").window("close");
        });


        $("#btnSave").live("click", function () {
            var Vote = ckIsVote.checked ? 1 : 0;
            var Article = ckIsArticle.checked ? 1 : 0;
            var Activity = ckIsActivity.checked ? 1 : 0;
            $.post(handlerUrl, { Action: "SaveReviewConFig", Vote: Vote, Article: Article, Activity: Activity }, function (data) {
                var resp3 = $.parseJSON(data);
                if (resp3.Status = 0) {
                    Show(resp3.Msg);
                } else {
                    Alert(resp3.Msg);
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;管理 &nbsp;&gt;&nbsp;&nbsp;<span>话题信息-</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div id="toolbar" class="pageTopBtnBg" style="padding: 5px; height: auto;">
        <div style="margin-bottom: 5px">
            <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-delete" plain="true"
                onclick="Delete()">删除</a>
                &nbsp;&nbsp;&nbsp;&nbsp;批量审核:
              <select id="ddlAudit" style="width:100px">
                <option value="1">通过</option>
                <option value="2"  >拒绝</option>
              </select>
              <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-edit" plain="true"
                onclick="BatchAudit()">批量审核</a>
            <br />
        </div>
    </div>
    <table id="grvData" fitcolumns="true">
    </table>
</asp:Content>
