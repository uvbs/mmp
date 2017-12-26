<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="GameList.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.game.GameList" %>
<!DOCTYPE html>
<html>
<head>
    <title>至云移动营销管理平台</title>
    <link href="/css/game/game.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Scripts/game/inpagecommon.js" type="text/javascript"></script>
</head>
<body class="insidepage">
    <div class="breadnav">微游戏>游戏任务</div>
<%--    <div class="catbox">
            <a href="javascript:void(0);" class="catlink current">游戏分类1</a>
            <a href="javascript:void(0);" class="catlink ">游戏分类2</a>
            <a href="javascript:void(0);" class="catlink">游戏分类3</a>
            <a href="javascript:void(0);" class="catlink">游戏分类4</a>
    </div> --%>   

    <div class="mainbox">
        <div class="scorllbox">

        <%foreach (var item in new ZentCloud.BLLJIMP.BllGame().GetTopGameList(10))
	    { %>
		 
           <div class="showbox">
                <div class="gamebox">
                    <img src="<%=item.GameImage%>" alt="" class="showpic">
                    <div class="showdesc">
                        <h2 class="showtitle"><%=item.GameName %></h2>
                        <p class="showtext"><%=item.GameDesc %></p>
                    </div>
                   <%-- <a target="_blank" href="#" class="icobtn greenbtn">
                        <span class="btnico nextico"></span>
                        <span class="text">试玩游戏</span>
                    </a>--%>
                </div>
                <a href="AddAdvert.aspx?gid=<%=item.AutoID%>" class="icobtn greenbtn">
                    <span class="btnico addico"></span>
                    <span class="text">选择游戏</span>
                </a>
            </div>   
              
	   <% } %>

            

           

           
        </div>
    </div>
</body>
<script type="text/javascript">
    var tableheight = new tableHeight(".scorllbox", 60, [".breadnav"]);

</script>
</html>
