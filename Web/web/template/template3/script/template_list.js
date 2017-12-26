
var mainurl="http://dev.comeoncloud.net/serv/websiteapi.ashx"
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

tm.addarticlelist(function(data){
    var n=1;
    for (i in data) {
        var mainhref=tm.articlelisthref(data[i].ArticleUrl);
        //console.log(mainhref);
        $('#mainlist').append("<li class='list'><a class='list"+n+"' href='"+mainhref+"'></a></li>");
        $('.list'+n).append("<img src='"+data[i].ArticleThumbnails+"' alt='"+data[i].ArticleTitle+"' class='listpic'/>");
        $('.list'+n).append("<h2>"+data[i].ArticleTitle+"</h2>");
        $('.list'+n).append("<p>"+data[i].ArticleContent+"</p>");
        n++
    };
})

//底部导航
tm.addfooternav(function(data){
    var maxn=data.length;
    var n=1;
    $("body").append("<nav class='footer' id='navbar'></nav>")
    for (i in data) {
        $('#navbar').append("<a class='navbtn navbtn"+n+"' href='"+(data[i].ToolBarTypeValue?data[i].ToolBarTypeValue:"javascript:void(0);")+"'></a>");
        $('.navbtn'+n).append("<span class='icon "+data[i].ToolBarImage+"' alt='"+data[i].ToolBarDescription+"' class='pic'/>");
        $('.navbtn'+n).append("<span class='text'>"+data[i].ToolBarName+"</span>");
        $('.navbtn'+n).css({width:100/maxn+"%"});
        n++;
    };
})

$(function(){
    if(tm.getQueryString("from")===null){
        $("body").append("<div class='toptool' id='toptool'><a href='#' class='back'><span class='icon'></span></a><span class='more'></span></div>");
        $("#toptool").find(".back").attr({"href":"javascript: window.history.go(-1);"});
        $("#mainlist").css({"padding-top":"40px"});
    }
})

//网站信息
tm.addsiteinfo(function(data){
    $("footer").text(data.Copyright);
    weixinshare.desc = data.WebsiteDescription;
    weixinshare.title = data.WebsiteTitle;
    weixinshare.imgUrl = data.WebsiteImage
    weixinshare.shareUrl = document.location.href;
})

