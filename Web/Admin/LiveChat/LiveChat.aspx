<%@ Page Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true" CodeBehind="LiveChat.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.LiveChat.LiveChat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/livechat.css?v=0.20170420" rel="stylesheet" />
    <style>
        .weui-btn_primary, .weui-btn_primary:not(.weui-btn_disabled):active {
            background-color: <%=string.IsNullOrEmpty(WebsiteInfo.ThemeColor)?"#1aad19":WebsiteInfo.ThemeColor%>;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <ul class="content" id="divMsgList">
        <%
            ZentCloud.BLLJIMP.BLLUser bllUser = new ZentCloud.BLLJIMP.BLLUser();
            System.Text.StringBuilder sb = new StringBuilder();
            foreach (var item in RecordList)
            {
                if (item.MessageType == "text")
                {
                    sb.AppendFormat("<li>");
                    sb.AppendFormat("<div class=\"sys-msg\">{0}</div>", item.InsertDate.ToString("yyyy-MM-dd HH:mm"));
                    sb.AppendFormat("</li>");


                }
                string flo = "right";
                string headImg = "/img/icons/kefu.png";
                if (item.UserType == "0")
                {
                    flo = "left";
                    headImg = item.UserHeadImg;
                }
                sb.AppendFormat("<li>");
                sb.AppendFormat("<img src=\"{0}\" class=\"img{1}\">", headImg, flo);
                sb.AppendFormat("<span class=\"span{0}\">{1}</span>", flo, item.Message);
                sb.AppendFormat("</li>");

            }
            if (RecordList.Count > 0)
            {
                sb.AppendFormat("<li>");
                sb.AppendFormat("<div class=\"sys-msg\">--以上为历史消息--</div>", "");
                sb.AppendFormat("</li>");
            }
            Response.Write(sb.ToString());
        %>
    </ul>

    <div class="sendBox">
        <div>
            <img src="/img/icons/wenjian.png" onclick="fileImg.click()" />
            <input type="file" id="fileImg" name="fileImg" style="display: none;" />

        </div>
        <div class="weui-cells weui-cells_form">
            <div class="weui-cell">
                <div class="weui-cell__bd">
                    <textarea id="txtMsg" class="weui-textarea" placeholder="请输入内容" rows="4"></textarea>
                    <div class="send"><a href="javascript:;" class="weui-btn weui-btn_mini weui-btn_primary" id="btnSend">发送</a></div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/jquery-1.8.3.js"></script>
    <script src="/Scripts/StringBuilder.Min.js"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016111401" type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016111401" type="text/javascript"></script>

    <script type="text/javascript">
        var noSupportMessage = "Your browser cannot support WebSocket!";
        var ws;
        function appendMessage(resp) {
           
            var message = $.parseJSON(resp);
            var str = new StringBuilder();

            if (message.message_type=="text") {
                str.AppendFormat('<li> ');
                str.AppendFormat('<div class="sys-msg">{0}</div>', message.send_time);
                str.AppendFormat('</li>');


            }
            switch (message.message_type) {
                case "text"://文本
                    var flo = "right";
                    if (message.send_user_type == "0") {
                        flo = "left";
                    }
                    str.AppendFormat(' <li>');
                    str.AppendFormat('<img src="{0}" class="img{1}">', message.send_user_head_img, flo);
                    str.AppendFormat('<span class="span{0}">{1}</span>', flo, message.message);
                    str.AppendFormat(' </li>');
                    break;
                case "system"://系统消息
                    str.AppendFormat('<li> ');
                    str.AppendFormat('<div class="sys-msg">{0}</div>',message.message);
                    str.AppendFormat('</li>');
                    break;
                default:

            }



            $("#divMsgList").append(str.ToString());
            var h = $(document).height() - $(window).height();
            $(document).scrollTop(h);


        }


        function connectSocketServer() {
            var support = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);
            // create a new websocket and connect
            var lin = "ws://<%=WebSocketHost%>/server/" + <%=Request["kefu_id"]%> + "/" + <%=Request["room_id"]%>;
            ws = new window[support](lin);
            // when data is comming from the server, this metod is called
            ws.onmessage = function (evt) {
                appendMessage(evt.data);
            };

            // when the connection is established, this method is called
            ws.onopen = function () {
                //appendMessage('* Connection open<br/>');

            };

            // when the connection is closed, this method is called
            ws.onclose = function () {


            }
        }

        function sendMessage() {
            if (ws) {

                ws.send($("#txtMsg").val());
                $("#txtMsg").val("");
            }
        }

        function disconnectWebSocket() {
            if (ws) {
                ws.close();
            }
        }

        function connectWebSocket() {
            connectSocketServer();
        }

        window.onload = function () {

            connectSocketServer();
        }

        $(function () {

           
            $("#btnSend").click(function () {
                if ($.trim($("#txtMsg").val())== "") {
                    $("#txtMsg").focus();
                    return false;
                }
                sendMessage();



            });
            var h = $(document).height() - $(window).height();
            $(document).scrollTop(h);

        })
        $("#fileImg").on('change', function () {
           
            uploadImg();
        });



        function uploadImg() {
            try {
                $.ajaxFileUpload({
                    url: '/Serv/Api/Common/File.ashx?action=Add',
                    secureuri: false,
                    fileElementId: 'fileImg',
                    dataType: 'json',
                    success: function (resp) {
                        if (resp.errcode == 0) {
                            try {
                                if (ws) {

                                    ws.send('<img src="' + resp.file_url_list[0] + '" onclick="preview(this)"/>');
                                    
                                }
                            } catch (e) {

                                window.location.href = "LiveChat.aspx";
                            }




                        } else {
                            alert(resp.errmsg);
                        }
  

                    }

                });

            } catch (e) {

            }


        }
        function preview(obj) {
            try {
                window.open(obj.src);

            } catch (e) {
                alert(e);
            }

        }
        //跳转商品详情
        function gotoProduct(productId) {

            var link="/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/" + productId + "#/productDetail/" + productId;
            window.open(link);


        }

    </script>


</asp:Content>
