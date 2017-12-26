<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WeuiMaster.Master" AutoEventWireup="true"
    CodeBehind="LiveChat.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.LiveChat.LiveChat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Css/livechat.css?v=0.20170427" rel="stylesheet" />
    <style>
        .top {
            background-color: <%=string.IsNullOrEmpty(WebsiteInfo.ThemeColor)?"#1AAD19":WebsiteInfo.ThemeColor%>;
        }

        .weui-btn_primary, .weui-btn_primary:not(.weui-btn_disabled):active {
            background-color: <%=string.IsNullOrEmpty(WebsiteInfo.ThemeColor)?"#1aad19":WebsiteInfo.ThemeColor%>;
        }
        .pos-rel {
            position: relative !important;
        }
        .pos-fix {
            position:fixed !important;
        }
        .pos-abs {
            position:absolute !important;
            bottom:0;
        }
        .clear
        {
            clear:both;    
        }
        .content
        {
           min-height: 300px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
<div id="wrapLiveChat">
    <div class="top" v-on:click="sendBoxFocus">
        <img src="<%= config.DistributionQRCodeIcon%>" />
        <label class="company-name"><%= config.WeixinAccountNickName%></label>
        
    </div>
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
                if (item.UserType == "1")
                {
                    flo = "left";
                }
                else
                {
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
    <div class="clear"></div>
    <div class="sendBox" id="sendBox">

        <div class="bottom-left">
            <img src="/img/icons/jia.png" onclick="fileImg.click()" />
            <input type="file" id="fileImg" name="fileImg" @change="uploadImg" style="display: none;" />
        </div>
        <div class="bottom-center">
            <%--<input id="txtMsg" placeholder="请输入内容" />--%>

            <div id="txtMsg"  contenteditable="true"  @click="sendBoxFocus"  @blur="sendBoxFocusOut"></div>
            
        </div>
        
        <div class="bottom-right">
            <a href="javascript:;" class="weui-btn weui-btn_mini weui-btn_primary" id="btnSend" v-on:click="sendMsg">发送</a>
        </div>

        <div class="clear"></div>
    </div>
    <div class="clear"></div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottom" runat="server">
    <script src="/Scripts/jquery-1.8.3.js"></script>
    <script src="/Scripts/StringBuilder.Min.js"></script>
    <script src="/Scripts/global-m.js"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.min.js"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxfileupload2.1.js?v=2016111401"
        type="text/javascript"></script>
    <script src="http://static-files.socialcrmyun.com/Scripts/ajaxImgUpload.js?v=2016111401"
        type="text/javascript"></script>
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
                case "text":
                    var flo = "right";
                    if (message.send_user_type == "1") {
                        flo = "left";
                    }
                    str.AppendFormat(' <li>');
                    str.AppendFormat('<img src="{0}" class="img{1}">', message.send_user_head_img, flo);
                    str.AppendFormat('<span class="span{0}">{1}</span>', flo, message.message);
                    str.AppendFormat(' </li>');
                    break;
                case "system":
                    str.AppendFormat('<li> ');
                    str.AppendFormat('<div class="sys-msg">{0}</div>', message.message);
                    str.AppendFormat('</li>');
                    break;
                default:

            }



            $("#divMsgList").append(str.ToString());
            var h = $(document).height() - $(window).height();
            if (h > 100) {
                $(document).scrollTop(Number.MAX_VALUE);
            }



        }



        function connectSocketServer() {
            var support = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);

            //if (support == null) {
            //    appendMessage("* " + noSupportMessage + "<br/>");
            //    return;
            //}

            //appendMessage("* Connecting to server ..<br/>");
            // create a new websocket and connect
            ws = new window[support]('ws://<%=WebSocketHost%>/client/<%=currentUserInfo.AutoID%>');

            // when data is comming from the server, this metod is called
            ws.onmessage = function (evt) {
                appendMessage(evt.data);
            };

            // when the connection is established, this method is called
            ws.onopen = function () {
                //appendMessage('* Connection open<br/>');

                var respDefault = {
                    message_type: "system",
                    message: "您好,<%=config.WeixinAccountNickName%>很高兴为您服务!"

                }
                appendMessage(JSON.stringify(respDefault));


                 <%if (!string.IsNullOrEmpty(productInfo.PID))
                   {%>


                if (ws) {

                    setTimeout("sendProduct();", 1000);

                }
                else {

                }


                 <% }%>




            };

            // when the connection is closed, this method is called
            ws.onclose = function () {
                window.location.href = "LiveChat.aspx";

            }
        }

        function sendMessage() {
            try {
                if (ws) {

                    ws.send($("#txtMsg").text());
                    $("#txtMsg").text("");
                }
            } catch (e) {

                window.location.href = "LiveChat.aspx";
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
        
            
        
//            var oHeight = $(document).height(); //浏览器当前的高度
//    
//            $(window).resize(function(){
// 
//                if($(document).height() < oHeight){
//                    //$("#wrapSubmit img,#wrapSubmit input,.wrapFooter").css("position","static");
//                    alert("1: " + $(document).height());
//                }else{
//                    alert("2: " + $(document).height());
//                    //$("#wrapSubmit img,#wrapSubmit input,.wrapFooter").css("position","absolute");
//                }
//                
//            });
        
        

//            $("#btnSend").click(function () {

//                if ($.trim($("#txtMsg").text()) == "") {
//                    $("#txtMsg").focus();
//                    return false;
//                }
//                sendMessage();



//            });
           
            $(document).scrollTop(Number.MAX_VALUE);

            $("#txtMsg").focus(function () {

                setTimeout("$(document).scrollTop(Number.MAX_VALUE)", 500);


            });
            $("#txtMsg").keyup(function () {

                setTimeout("$(document).scrollTop(Number.MAX_VALUE)", 500);


            });
        })


        function sendProduct() {

            var str = new StringBuilder();
            str.AppendFormat('<div class="product" onclick="gotoProduct({0})" >', '<%=productInfo.PID%>');
            str.AppendFormat('<div class="product-img">');
            str.AppendFormat('<img src="{0}" />', '<%=productInfo.RecommendImg%>');
            str.AppendFormat('</div>');
            str.AppendFormat('<div class="product-name">&nbsp;{0}</div>', '<%=productInfo.PName%>');
            str.AppendFormat('</div>');

                if (ws) {
                    ws.send(str.ToString());
                } else {

                }


            }


            var __mls = [];
            function preview(obj) {
                try {


                    __mls = [];
                    __mls.push(obj.src);
                    wx.previewImage({
                        current: obj.src,
                        urls: __mls
                    });

                } catch (e) {
                    alert(e);
                }

            }

            function gotoProduct(productId) {


                window.location.href = "/customize/shop/index.aspx?v=1.0&ngroute=/productDetail/" + productId + "#/productDetail/" + productId;


            }


            var isIOS = (/iphone|ipad/gi).test(navigator.appVersion);

            var app = new Vue({
              el: '#wrapLiveChat',
              data: {
                top:0
              }, methods: {
                sendBoxFocus: function () {

                    if(!isIOS){
                        return;
                    }

                    var x = document.documentElement.clientWidth,y=document.documentElement.clientHeight;

                    y=10000000;


                    window.scrollTo(x,y);
                    
                    setTimeout(function(){
                        window.scrollTo(x,y);
                    },500);
                    
                    $('.sendBox').addClass('pos-rel');

                },
                sendBoxFocusOut: function () {
                    if(!isIOS){
                        return;
                    }
                    setTimeout(function(){
                        $('.sendBox').removeClass('pos-rel');
                    },500);
                    
                },
                sendMsg:function(){
                    if ($.trim($("#txtMsg").text()) == "") {
                        $("#txtMsg").focus();
                        return false;
                    }
                    sendMessage();
                },
                uploadImg:function(){
                    var file = $('#fileImg').get(0).files[0];
                    zcUpload(file, 800, 0, function (progress) {
                        if (progress.lengthComputable) {
                            var percentComplete = Math.round(progress.loaded * 100 / progress.total);
                        }
                    }, function (complete) {
                        var resp = JSON.parse(complete.target.responseText);
                        if (resp.errcode == 0 && resp.file_url_list && resp.file_url_list.length > 0) {
                            try {
                                if (ws) {
                                    ws.send('<img src="' + resp.file_url_list[0] + '" onclick="preview(this)"/>');                                    
                                    //window.scrollTo(100,10000000);
                                }
                            } catch (e) {
                                window.location.href = "LiveChat.aspx";
                            }
                        }
                        else {
                            alert(resp.errmsg);
                        }
                    }, function (error) {
                        alert('上传出错');
                    });
                }
              }
            })
            
        
    </script>


</asp:Content>
