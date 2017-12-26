<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.LiveChat.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>微客服-PC端</title>
    <link href="Css/index.css?v=0.05" rel="stylesheet" />
</head>
    <script>
        var kefuId=<%=currentUserInfo.AutoID%>;
        var roomId="";
    </script>
<body>

    <div class="top">
        <div class="top-left">
            <img src="/img/icons/kefu.png">
            <span class="kefu-name"><%=currentUserInfo.UserID%></span>

             <span class="voice"><input  type="checkbox" id="cbPlayVoice"/><label for="cbPlayVoice">打开新消息提醒声音</label></span>

            <div class="flo-right">手机端链接:http://<%=Request.Url.Host %>/App/LiveChat/LiveChat.aspx</div>
        </div>
    </div>
    <div class="vue-el-left">


          <div class="item" >
          <div class="item-left">
                <img src="/img/icons/clock.png">
            </div>
            <div class="item-center">共有{{wait_join_count}}位用户等待</div>
            <div class="item-right">
            </div>
        </div>

        <div class="item" v-for="item in roomList">
           

          <div class="item-left">
                <img v-bind:src="item.user_info.head_img">
            </div>
            <div class="item-center"><span v-bind:class="['', { 'offline': item.is_online==0 }]">{{item.user_info.name}}</span>&nbsp;<span v-if="item.is_online==1" class="green">[在线]</span><span class="offline" v-if="item.is_online==0">[离线]</span></div>
            <div class="item-right">
                <div class="un-read-clear" v-if="item.un_read_count==0">{{item.un_read_count}}</div>
                <div class="un-read" v-if="item.un_read_count>0">{{item.un_read_count}}</div>
                <button class="btn-in" v-if="!item.is_kefu_join&&item.is_online==1" v-on:click="joinRoom(item)">接入</button>
                <button class="btn-in" v-if="item.is_kefu_join&&item.is_online==1" v-on:click="joinRoom(item)">接入中</button>
                <button class="btn-in" v-if="item.is_online==0" v-on:click="joinRoom(item)">消息</button>
            </div>
            <div class="item-bottom">
                {{item.last_update_timef}}
                订单数&nbsp;<a class="red"  v-on:click="toOrderDetail(item)">{{item.order_count}}</a>
                浏览数&nbsp;<a class="red"  v-on:click="toViewDetail(item)">{{item.view_count}}</a>
            </div>
        </div>

    </div>
    <div class="right" id="divLiveChat">
        <iframe id="ifLiveChat" ></iframe>
    </div>

    <div id="divRemind"></div>

</body>
<script src="/Scripts/jquery-1.8.2.min.js"></script>
<script src="//static-files.socialcrmyun.com/lib/vue/2.0/vue.min.js" type="text/javascript"></script>
<script src="Js/index.js"></script>
    <script>
       
        $(".right").css("width",$(window).width()-380);
        $(".right").css("height",$(window).height()-60);
        $(".vue-el-left").css("height",$(window).height()-60);



        //getRemind();
        //setInterval("getRemind()",30000);

        ////获取提示
        //function getRemind(){
        
            
        //        $.ajax({
        //            type: 'post',
        //            url: "/Serv/Api/Admin/LiveChat/Room/Remind.ashx",
        //            data: {
                       
        //            },
        //            dataType: 'json',
        //            success: function (resp) {
        //                if ( resp.status) {

        //                    $('#divRemind').html('<audio autoplay="autoplay"><source src="audio/remind.mp3" type="audio/mpeg"/></audio>'); 

        //                }
                    

        //            }
        //        })

        
        //}

        //window.onunload = function() {
           
        //        $.ajax({
        //            type: 'post',
        //            url: "/Serv/Api/Admin/LiveChat/RoomUser/UpdateOnLine.ashx",
        //            data: {
        //                online:0
        //            },
        //            dataType: 'json',
        //            success: function (resp) {
                 
                    

        //            }
        //        });


        //}  


        //window.onbeforeunload = function()
        //{
        //    setTimeout(function(){_t = setTimeout(onunloadcancel, 0)}, 0);

        //    $.ajax({
        //        type: 'post',
        //        url: "/Serv/Api/Admin/LiveChat/RoomUser/UpdateOnLine.ashx",
        //        data: {
        //            online:0
        //        },
        //        dataType: 'json',
        //        success: function (resp) {
                 
                    

        //        }
        //    });
        //    return "真的离开?";
        //}
        //window.onunloadcancel = function()
        //{
        //    clearTimeout(_t);
        //        $.ajax({
        //            type: 'post',
        //            url: "/Serv/Api/Admin/LiveChat/RoomUser/UpdateOnLine.ashx",
        //            data: {
        //             online:1
        //            },
        //         dataType: 'json',
        //         success: function (resp) {
                 
                    

        //         }
        //     });
        //}

        var isPlayVoice=<%=currentUserInfo.Ex15%>;
        var noSupportMessageIndex = "Your browser cannot support WebSocket!";
        var wsIndex;
        function connectSocketServerIndex() {
            var supportIndex = "MozWebSocket" in window ? 'MozWebSocket' : ("WebSocket" in window ? 'WebSocket' : null);
            // create a new websocket and connect
            var linIndex = "ws://<%=WebSocketHost%>/index/" + <%=currentUserInfo.AutoID%>;
            wsIndex = new window[supportIndex](linIndex);
            // when data is comming from the server, this metod is called
            wsIndex.onmessage = function (evt) {
                
                if (isPlayVoice==1) {
            
                    $('#divRemind').html('<audio autoplay="autoplay"><source src="audio/remind.mp3" type="audio/mpeg"/></audio>'); 

                }

            };

            // when the connection is established, this method is called
            wsIndex.onopen = function () {
                //appendMessage('* Connection open<br/>');
               

            };

            // when the connection is closed, this method is called
            wsIndex.onclose = function () {


            }
        }



        function disconnectWebSocketIndex() {
            if (wsIndex) {
                wsIndex.close();
            }
        }

        function connectWebSocketIndex() {
            connectSocketServerIndex();
        }

        window.onload = function () {

            connectSocketServerIndex();
        }

        $(function(){
        
            if (isPlayVoice==1) {
                $("#cbPlayVoice").attr("checked","checked");

            }
            $("#cbPlayVoice").click(function(){
            
                if ($(this).attr('checked')) {
                    isPlayVoice=1;
                }
                else {
                    isPlayVoice=0;
                }

                        $.ajax({
                            type: 'post',
                            url: "/Serv/Api/Admin/LiveChat/RoomUser/UpdateIsPlayVoice.ashx",
                            data: {
                                is_play_voice:isPlayVoice
                            },
                         dataType: 'json',
                         success: function (resp) {
                             if (resp.status) {
    
                             }else {
                                 alert("设置失败");
                             }
                    

                         }
                     });


            
            })

        })

    </script>
</html>
