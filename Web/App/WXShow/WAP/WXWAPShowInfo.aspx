<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WXWAPShowInfo.aspx.cs"
    Inherits="ZentCloud.JubitIMP.Web.App.WXShow.WAP.WXWAPShowInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=wxsInfo.ShowName%></title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
    <link rel="stylesheet" href="styles/css/style.css?v=0.0.4">
    <script src="/Scripts/jquery-1.8.3.js" type="text/javascript"></script>
    <script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js"></script>
    <script type="text/javascript">
        wx.ready(function () {
            wxapi.wxshare({
                title: "<%=wxsInfo.ShowName%>",
                desc: "<%=wxsInfo.ShowDescription%>",
                link:"<%=shareLink %>", 
                imgUrl: "http://<%=Request.Url.Host%><%=wxsInfo.ShowImg%>"
            }
             , {
                 message_s: function () {
                     //alert("好友分享成功") 
                     ShareToFriend();
                 },
                 message_c: function () {
                     //alert("好友分享取消")
                 },
                 timeline_s: function () {
                     //alert("朋友圈分享成功")
                     ShareToTimeLine();
                 },
                 timeline_c: function () {
                     //alert("朋友圈分享取消")
                 }
             }

            )
        })
         //发送给朋友
         function ShareToFriend() {
          ShareWXShow(0);
         }
         //分享到朋友圈
         function ShareToTimeLine() {
          ShareWXShow(1);
         }
         function ShareWXShow(type) {
             $.ajax({
                 type: 'post',
                 url: "/Handler/App/WXShowInfoHandler.ashx",
                 data: { Action: 'ShareWXShow', AutoId:<%=wxsInfo.AutoId%>,Type: type },
                 timeout: 60000,
                 dataType: "json",
                 success: function (resp) {
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                    

                 }
             });
         
         }

    </script>
    <script data-main="src/main.min1" src="src/require.min.js"></script>
</head>
<body>
     <div id="loadingscreen">
        <span class="loadtext">
            Loading...
        </span>
    </div>
    <section class="box">
    <input id="AutoPlayTimeSpan" type="hidden" value="<%=wxsInfo.AutoPlayTimeSpan %>" />
   
    <% if (!string.IsNullOrEmpty(wxsInfo.ShowMusic))
       {%>
          <div id="musicbutton" class="musicplay"></div>
          <audio id="myaudio" src="<%=wxsInfo.ShowMusic %>" ></audio>
     <%}%>
   
    <div id="imglist">
        <%=strInit.ToString() %>
         <%if (!string.IsNullOrEmpty(wxsInfo.ShowUrl))
           {%>
        <div class="listli">
            <span class="blackpage"></span>
        </div>
     <% } %>         
    </div>
</section>
    <script>
        require.config({
            baseUrl: "./src/",
            paths: {
                jquery: "commonjs/jquery.min",
                WeShow: "commonjs/weshow.min"
            }
        })
        require(["WeShow"], function (WeShow) {

            touchpic = new WeShow("#imglist", function (_this, snum) {
                var current = $(".listli:eq(" + snum + ")");
                switch (snum) {
                    <%=stranimation.ToString() %>
                }
            });



        });

         
        $(function(){
            var wxurl = '<%=wxsInfo.ShowUrl%>';
            if(wxurl){
            
               setInterval(function(){
                  if(touchpic.maini >= touchpic.msize){
                    
                    window.location.href = wxurl;
                  }
                  
               },1000);
               
            }
        });

        var startAutoPlayLock = false;
        function startAutoPlay(){
            if(startAutoPlayLock){
                return;
            } 
            startAutoPlayLock = true;
            console.log('startAutoPlay');
            var autoPlayTimeSpan = parseInt($('#AutoPlayTimeSpan').val());
            var doCount = 1;
            if (autoPlayTimeSpan>0) {
                setInterval(function(){
                    //touchpic.mchange=2;touchpic.endfun();
                    touchpic.nextpage();
                    doCount++;
                    console.log(doCount);
                },(autoPlayTimeSpan+=3)*1000);
                
            }

        };


    </script>
</body>
</html>
