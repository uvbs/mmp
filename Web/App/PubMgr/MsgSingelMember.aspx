<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="MsgSingelMember.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.PubMgr.MsgSingelMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Ju-Modules/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/css/msgList.css?v=0.0.1" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .sort
        {
            width: 780px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;公众号管理&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>聊天记录 </span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <a href="MsgDetails.aspx" class="easyui-linkbutton" iconcls="icon-redo" plain="true">返回</a>
        <br />
        <hr style="border: 1px dotted #036;" />
        <div class="main">
            <div class="mainCnt">
                <div class="cntList">
                </div>
            </div>
        </div>
    </div>
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
<script type="text/javascript">
    /*
    1.一个WeixinMsgDetails实体；
    2.可能多个ZCJ_WeixinMsgDetailsImgsInfo实体；
    */
    var handlerUrl = '/Handler/App/CationHandler.ashx';
    var msgId = '<%=Request["msgId"] %>';

    $(function () {
        $.ajax({
            type: 'post',
            url: handlerUrl,
            data: { Action: 'GetMsgDetails', MsgId: msgId },
            dataType: "json",
            success: function (resp) {
                if (resp.Status != 1)
                    return;
                var html = '';
                if (resp.ExObj.length > 2) {
                    for (var i = 0; i < resp.ExObj[1].length; i++) {
                        html += CreateNewsItem(resp.ExObj[1][i], resp.ExObj[0]);
                    }
                }
                if (resp.ExObj[0].ReplyContent != '' && resp.ExObj[0].ReplyContent != null) {
                    html += CreateFirstItem(resp.ExObj[0]);
                }
                html += CreateSecondItem(resp.ExObj[0]);

                $(".cntList").html(html);

            }
        });
    });

    //管理员回复
    function CreateFirstItem(model) {
        var str = new StringBuilder();
        str.AppendFormat('<div class="listItem" data-msgid="{0}">', model.UID);
        str.AppendFormat('  <div class="listItemUserInfo">');
        str.AppendFormat('    <img alt="" src="{0}" />', '/img/offline_user.png');
        str.AppendFormat('    <div class="listItemUserInfoNikeName">{0}</div>', "系统文本回复");
        str.AppendFormat('  </div>');
        str.AppendFormat('  <div class="listItemMsg" title="{0}">{0}</div>', model.ReplyContent == "" ? "&nbsp;" : model.ReplyContent);
        str.AppendFormat('  <div class="listItemMsgTime">{0}</div>', FormatDate(model.ReplyDate));
        str.AppendFormat('</div>');
        return str.ToString();
    }

    function CreateSecondItem(model) {
        var str = new StringBuilder();
        str.AppendFormat('<div class="listItem" data-msgid="{0}">', model.UID);
        str.AppendFormat('  <div class="listItemUserInfo">');
        str.AppendFormat('    <img alt="" src="{0}" />', model.WXHeadimgurlLocal == '' ? '/img/offline_user.png' : model.WXHeadimgurlLocal);
        str.AppendFormat('    <div class="listItemUserInfoNikeName">{0}</div>', model.WXNickname == null ? "无昵称" : model.WXNickname);
        str.AppendFormat('  </div>');
        str.AppendFormat('  <div class="listItemMsg" title="{0}">{0}</div>', model.ReceiveContent == "" ? "&nbsp;" : model.ReceiveContent);
        str.AppendFormat('  <div class="listItemMsgTime">{0}</div>', FormatDate(model.ReceiveDate));
        str.AppendFormat('</div>');
        return str.ToString();
    }

    function CreateNewsItem(model, msgModel) {
        var str = new StringBuilder();
        str.AppendFormat('<div class="listItem">');
        str.AppendFormat('  <div class="listItemUserInfo">');
        str.AppendFormat('    <img alt="" src="{0}" />', '/img/offline_user.png');
        str.AppendFormat('    <div class="listItemUserInfoNikeName">{0}</div>', "系统图文回复");
        str.AppendFormat('  </div>');
        str.AppendFormat('  <div class="listItemMsg" title="{0}"><img alt="" src="{1}" width="30px" height="60px" />{0}</div>', model.Description == "" ? "&nbsp;" : model.Description, model.PicUrl);
        str.AppendFormat('  <div class="listItemMsgTime">{0}</div>', FormatDate(msgModel.ReplyDate));
        str.AppendFormat('</div>');
        return str.ToString();
    }

    </script>
</asp:Content>