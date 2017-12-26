<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Scratch.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Lottery.wap.Scratch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache,must-revalidate" />
    <meta http-equiv="expires" content="Wed,26 Feb 1997 08:21:57 GMT" />
    <meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=no" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/wScratchPad.js" type="text/javascript"></script>
    <script type="text/jscript">
        var IsWin = <%=IsWin %>;  //是否中奖
        var IsGetPrize = <%=IsGetPrize %>; //是否领奖
        function SubmitFormInfo(url) {
          
            var rurl=url;
            $.post("/Serv/ActivityApiJson.ashx",
            { "LoginName": "anViaXQ=", "LoginPwd": "2!!5AED!#CA8BF9#C087E070B2C5407B", "Name": "无", "Phone": $("#Phone").val(), "ActivityID": "167435", "K1": $("#K1").val()},
            function(data) {
                var obj = $.parseJSON(data);
                if (obj.Status == 0) {
                    window.location=rurl;
                } else {
                 if (obj.Status==1) {
                     window.location=rurl;
                    }
                    else {
                        alert(obj.Msg);
                       }
                  
                   
                }
            });
        }
    </script>
    <title>
        <%=model.LotteryName%></title>
    <style type="text/css">
        body
        {
            font-size: 16px;
            font-family: '微软雅黑';
            margin: 0;
        }
        .cover
        {
            width: 100%;
            max-width: 100%;
            margin: 0 auto;
            position: relative;
        }
        .cover img
        {
            width: 100%;
        }
        #scratchpad
        {
            position: absolute;
            width: 100%;
            top: 1px;
            text-align: left;
            font-weight: bold;
            font-size: 18px;
            line-height: 40px;
            margin-left: 0%;
            margin-right: 20px;
            color: White;
        }
        #divcontent
        {
            display: none;
            color:White;
            font-size: 18px;
            line-height: 20px;
            text-align:center;
            font-weight: bold;
            line-height: 30px;
            letter-spacing: 3.5px;
            margin-left: 5px;
        }
        .uparea
        {
            text-align: left;
            margin-top: 0px;
        }
        .downarea
        {
            margin-top: 50px;
        }
    </style>
</head>
<body>
    <div class="cover">
        <div id="scratchpad">
        </div>
        <div class="uparea">
            <%=model.ScratchUpAreaContent%>
        </div>
        <div id="divcontent">
            <%=sbLottery%>
        </div>
        <div class="downarea">
            <%=model.ScratchDownAreaContent%>
        </div>
    </div>
    <script type="text/javascript">

        $(function () {



            $("#divcontent").show();
            var UserLogCount=<%=UserLogCount%>;//有抽奖记录不显示刮奖区
            
            if (parseInt(UserLogCount)>0) {
            
                return ;//有抽奖记录不显示刮奖区

                }
                  


            var obj = $("#divcontent");
            var offset = obj.offset();
            var offsetleft = offset.left;
            var offsetright = offset.right;
            //var useragent = window.navigator.userAgent.toLowerCase();
            //var statu = "enable";
            var sp;
            $("#divcontent").hide();
            sp = $("#scratchpad").wScratchPad({
                width: $(window).width(),
                height: obj.height() + 50,
                image2: "/img/ggk/overlay.jpg?v=" + Math.random(),
                color: "#a9a9a7",
                size: 50,
                scratchMove: function (e, percent) {
                    if (percent > 80) {
                        this.clear();
                        $("#scratchpad").remove();
                    }
                }
            });
            $("#scratchpad").css({ "top": offset.top });

            setInterval("setpostion()", 100);
            setTimeout("$(\"#divcontent\").css({ \"color\": \"Red\" })",3000);
        });


        function setpostion() {
            $("#divcontent").show();
            var obj = $("#divcontent");
            var offset = obj.offset();
            var offsetleft = offset.left;
            var offsetright = offset.right;
            $("#scratchpad").css({ "top": offset.top });

        }
    
    </script>
    <script type="text/javascript">
        function GetPrize() {  //领奖函数
            $.post("/Handler/CommHandler.ashx",
            { "Action": "GetPrize", "LotteryId": id, "IntentionSchool": $("#se").val() },
            function (data) {
                var obj = $.parseJSON(data);
                if (obj.Status == "1") {
                    alert(obj.Msg);
                    window.location.href = window.location.href; 
                } else {
                    alert("领奖失败")
                }
            });
        }



       
    </script>
</body>
</html>
