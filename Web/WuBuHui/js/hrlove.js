var hrloveheight=($(window).height()-$(".hrlovelist").height())/3;$(".hrlovelist").css({"margin-top":parseInt(hrloveheight)}),$(".jiesao").bind("tap",function(){$(this).addClass("jiesaohide")}),$(".jiesao")[0].addEventListener("webkitTransitionEnd",function(){$(".jiesao").remove()},!1);