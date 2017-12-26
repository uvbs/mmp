<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="MsgDetails.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.PubMgr.MsgDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/css/msgList.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .sort
        {
            width: 780px;
        }
        .title
        {
            font-size:12px;
            
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    <div class="title">当前位置：&nbsp;微客服&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>消息对话历史</span></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="main">
        <div class="mainTop">
            <ul class="nav nav-tabs" id="myTab">
                <li class="active"><a href="#all" data-toggle="tab">全部消息</a></li>
                <li><a href="#today" data-toggle="tab">今天</a></li>
                <li><a href="#yestoday" data-toggle="tab">昨天</a></li>
                <li><a href="#beforeyestody" data-toggle="tab">前天</a></li>
            </ul>
        </div>
        <div class="mainCnt">
            <div class="cntList">
            </div>
            <div style="clear: both;">
            </div>
            <ul class="pager">
                <li><a href="javascript:void(0)" id="btnFirst">首页</a></li>
                <li><a href="javascript:void(0)" id="btnPre">上一页</a></li>
                <li><a href="javascript:void(0)" id="btnNext">下一页</a></li>
                <li><a href="javascript:void(0)" id="btnLast">尾页</a></li>
                <li>
                    <label id="lbPageIndex">
                        &nbsp;&nbsp;</label></li>
                <li>
                    <input type="text" style="width: 30px;" id="txtJumpPage" /></li>
                <li><a href="javascript:void(0)" id="btnJump">跳转</a></li>
            </ul>
        </div>
    </div>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myModalLabel">
                        系统提示</h4>
                </div>
                <div class="modal-body">
                    <label id="lbMsg">
                    </label>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        关闭</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script type="text/javascript" src="http://cdn.bootcss.com/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        String.prototype.endWith = function (oString) {
            var reg = new RegExp(oString + "$");
            return reg.test(this);
        }

        var loadType = 'all';
        var page = 1;
        var totalPage = 0;
        var rows = 5;
        var handlerUrl = '/Handler/App/CationHandler.ashx';

        $(function () {

            LoadData();

            $(".listItem").live('click', function () {
                window.location.href = 'MsgSingelMember.aspx?msgid=' + $(this).attr("data-msgid");
            });

            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var target = e.target.toString().split("#");
                loadType = target[1];
                page = 1;
                LoadData();
            });

            $("#btnFirst").click(function () {
                page = 1;
                LoadData();
            });

            $("#btnLast").click(function () {
                page = totalPage;
                LoadData();
            });

            $("#btnJump").click(function () {
                var jumpPageStr = $('#txtJumpPage').val();
                if (!isInt(jumpPageStr)) {
                    return;
                }
                var jumpPage = parseInt(jumpPageStr);
                if (jumpPage > totalPage)
                    jumpPage = totalPage;
                page = jumpPage;
                LoadData();

            });

            $("#btnPre").click(function () {
                if (page == 1) {
                    ShowMsg("已经是第一页了!");
                    return;
                }
                page--;
                LoadData();

            });

            $("#btnNext").click(function () {
                if (page == totalPage) {
                    ShowMsg("已经是最后一页了!");
                    return;
                }
                page++;
                LoadData();
            });

        });

        function LoadData() {
            var dataModel = {
                Action: 'QueryMsgDetails',
                page: page,
                rows: rows,
                loadType: loadType
            }

            $.ajax({
                type: 'post',
                url: handlerUrl,
                data: dataModel,
                dataType: "json",
                success: function (resp) {
                    totalPage = resp.ExInt;
                    var html = '';
                    for (var i = 0; i < resp.ExObj.length; i++) {
                        html += CreateItem(resp.ExObj[i]);
                    }

                    $("#lbPageIndex").html(page + '/' + totalPage);

                    if (totalPage == 0) {
                        $("#lbPageIndex").html('0/0');
                    }

                    $(".cntList").html(html);

                }
            });

        }

        function CreateItem(model) {
            var str = new StringBuilder();
            str.AppendFormat('<div class="listItem" data-msgid="{0}">', model.UID);
            str.AppendFormat('  <div class="listItemUserInfo">');
            str.AppendFormat('    <img alt="" src="{0}" />', model.WXHeadimgurlLocal == '' ? '/img/offline_user.png' : model.WXHeadimgurlLocal);
            str.AppendFormat('    <div class="listItemUserInfoNikeName">{0}</div>', model.WXNickname == null ? "无昵称" : model.WXNickname);
            str.AppendFormat('  </div>');
            str.AppendFormat('  <div class="listItemMsg" title="{0}">{0}</div>', model.ReceiveContent == "" ? "&nbsp;" : model.ReceiveContent);
            str.AppendFormat('  <div class="listItemMsgTime">{0}</div>', FormatDate(model.ReceiveDate));
            str.AppendFormat('  <div class="listItemMsgOperate">');
            str.AppendFormat('    <a href="javascript:;" class="msgReply" title="快捷回复">快捷回复</a>({0})', model.ReplyStatus == '已回复' ? '<span style="color:green;">已回复</span>' : "未回复");
            str.AppendFormat('  </div>');
            str.AppendFormat('</div>');
            return str.ToString();
        }

        function ShowMsg(msg) {
            $('#myModal').modal('show');
            $('#lbMsg').html(msg);
        }
    </script>
</asp:Content>
