//文章页

var mainurl="http://dev.comeoncloud.net/serv/websiteapi.ashx";
var tm=new templateModule(mainurl);


var weixinshare={
    desc:"",
    title:"",
    imgUrl:"",
    shareUrl:""
}
document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
    WeixinJSBridge.on('menu:share:appmessage', function (argv) {
        WeixinJSBridge.invoke('sendAppMessage', {
            //"appid": appId,
            "img_url": weixinshare.imgUrl,
            "img_width": "400",
            "img_height": "400",
            "link": weixinshare.shareUrl,
            "desc": weixinshare.desc,
            "title": weixinshare.title
        }, function (res) {
        })
    });
    WeixinJSBridge.on('menu:share:weibo', function (argv) {
        WeixinJSBridge.invoke('shareWeibo', {
            "content": weixinshare.title,
            "url": weixinshare.shareUrl
        }, function (res) {
        });
    });
}, false)

tm.addarticleconcent(function(data){
    $("#article").append("<h2 class='title'>"+data.ArticleTitle+"</h2>");
    $("#article").append(data.ArticleContent);

    $("title").text(data.ArticleTitle);
    weixinshare.desc=data.ArticleContent.replace(/[x00-xff]|\-|\"|\/|\.|\#|\s*|/ig,"").substr(0,50);
    weixinshare.title = data.ArticleTitle;
    weixinshare.imgUrl = data.ArticleThumbnails
    weixinshare.shareUrl = document.location.href;
})

$(function(){
    if(tm.getQueryString("from")===null){
        //底部导航
        tm.addfooternav(function(data){
            var maxn=data.length;
            var n=1;
            $("body").append("<nav class='footer' id='navbar'></nav>")
            for (i in data) {
                $('#navbar').append("<a class='navbtn navbtn"+n+"' href='"+(data[i].ToolBarTypeValue?data[i].ToolBarTypeValue:"javascript:void(0);")+"'></a>");
                $('.navbtn'+n).append("<span class='icon "+data[i].ToolBarImage+"' alt='"+data[i].ToolBarDescription+"' class='pic'/>");
                $('.navbtn'+n).append("<span class='text'>"+data[i].ToolBarName+"</span>");
                $('.navbtn'+n).css({width:100/maxn+"%"})
                n++
            };
        })
        //顶部返回
        $("body").append("<div class='toptool' id='toptool'><a href='#' class='back'><span class='icon'></span></a><span class='more'></span></div>");
        $("#toptool").find(".back").attr({"href":"javascript: window.history.go(-1);"})
        $("#article").css({"padding-top":"40px"})
    }
    //分享
    $$(".btnbox").tap(function() {
        if(!$(".shade")[0]){
            $("body").append("<div class='shade'><span class='icon'></span></div>");
        }else{
            $(".shade").css({"display":"block"})
        }

        if(!$(".shade").attr("style")){
            $$(".shade").tap(function() {
                $(".shade").css({"display":"none"})
            })
        }
    })
})

//网站信息
tm.addsiteinfo(function(data){
    $("footer").text(data.Copyright);
})


