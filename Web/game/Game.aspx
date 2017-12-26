<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Game.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.game.Game" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><%=GameInfo.GameName%></title>
<%=string.IsNullOrEmpty(GameInfo.GameViewPort)?"<meta name=\"viewport\" content=\"initial-scale=1.0, maximum-scale=1.0\"/>":GameInfo.GameViewPort%>
<style type="text/css">
li{list-style: none;}
#slides {

    width: 100%;
    background-color: #eee;
    position: relative;
    overflow: hidden;
    z-index:99999;
}
#slides .slider {
    display: block;
    width: 100%;
   
}
#slides .slider img {
    display: block;
    width: 100%;
    height: inherit
}
#slides .slider .text {
    width: 100%;
    height: 26px;
    line-height: 26px;
    text-overflow: ellipsis;
    white-space: nowrap;
    overflow: hidden;
    position: absolute;
    left: 0px;
    bottom: 0px;
    text-align: left;
    line-height: 26px;
    color: #fff;
    background-color: rgba(0,0,0,0.5);
    box-sizing: border-box;
    padding-right: 80px
}
#slides .slidesjs-navigation {
    display: none !important;
}
#slides .slidesjs-pagination {
    position: absolute;
    top: 120px;
    right: 10px;
    z-index: 99
}
#slides .slidesjs-pagination li {
    float: left;
    margin: 0 1px
}
#slides .slidesjs-pagination li a {
    display: block;
    width: 13px;
    height: 0;
    padding-top: 13px;
    background-size: 13px 39px;
    background-position: 0 0;
    float: left;
    overflow: hidden
}
#slides .slidesjs-pagination li a.active,#slides .slidesjs-pagination li a:hover.active {
    background-position: 0 -13px
}
#slides .slidesjs-pagination li a:hover {
    background-position: 0 -26px
}
#slides .slidesjs-navigation {
    display: none
}
</style>
<script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<script src="/game/js/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
<script src="/Scripts/jquery.slides.min.js" type="text/javascript"></script>
<script src="/Scripts/StringBuilder.js" type="text/javascript"></script>
<script src="/Scripts/wxshare/wxshare0.0.1/wxshare.js" type="text/javascript"></script>
</head>
<body id="body" runat="server">

</body>

<script type="text/javascript">
    $(function () {
        if ($("#slides").children().length > 1) {
            $("#slides").slidesjs({
                width: 320,
                height: 60,
                play: {
                    active: true,
                    auto: true,
                    interval: 4000,
                    swap: true
                }
            });
        }


        $("[data-pid]").click(function () {
            var pid = $(this).attr("data-pid");
            var dataurl = $(this).attr("data-url");

            $.ajax({
                type: 'post',
                url: "/Handler/OpenGuestHandler.ashx",
                data: { Action: "AddGameEventDetail", PID: pid, DataUrl: dataurl },
                success: function (result) {
                    var resp = $.parseJSON(result);
                    if (resp.Status == 1) {
                        if (dataurl != "") {
                            window.location = dataurl;
                        }
                    }
                    else {
                        alert(resp.Msg);
                    }
                }
            });
        });
    });  
</script>  
</html>