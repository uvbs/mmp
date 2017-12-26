
var mainurl="/serv/websiteapi.ashx"
var tm=new templateModule(mainurl);


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

//分享链接隐藏顶部返回与底部导航
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
        $("#mainlist").css({"padding-top":"40px"})
    }

})



//网站信息
tm.addsiteinfo(function(data){
    //$("title").text(data.WebsiteTitle);
    $("footer").text("©"+data.Copyright);

    var desc = data.WebsiteDescription;
    var title = data.WebsiteTitle;
    var imgUrl = data.WebsiteImage
    var shareUrl = document.location.href;
    document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
        WeixinJSBridge.on('menu:share:appmessage', function (argv) {
            WeixinJSBridge.invoke('sendAppMessage', {
                //"appid": appId,
                "img_url": imgUrl,
                "img_width": "400",
                "img_height": "400",
                "link": shareUrl,
                "desc": desc,
                "title": title
            }, function (res) {
            })
        });
        WeixinJSBridge.on('menu:share:weibo', function (argv) {
            WeixinJSBridge.invoke('shareWeibo', {
                "content": title,
                "url": shareUrl
            }, function (res) {
            });
        });
    }, false)
})



//列表
// function getQueryString(name) {
//     var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
//     var r = window.location.search.substr(1).match(reg);
//     if (r != null) return unescape(r[2]); return null;
// }

// $.ajax({
//         url:'http://dev.comeoncloud.net/serv/websiteapi.ashx',
//         data:{action:"getarticlelist",cateid:getQueryString("cateid")},
//         dataType:"jsonp",
//         jsonpCallback:"getarticlelist",
//         success:function(data){
//             console.log(data);
//             var n=1;
//             for (i in data) {
//                 if(data[i].ArticleUrl){
//                     var articleurl=data[i].ArticleUrl.split("/");
//                     var mainhref=document.location.href.match(/\S*\//)+"detail.aspx?articleid="+articleurl[articleurl.length-2];
//                     //var articleurl=data[i].ArticleUrl.split("/");
//                     //var href=document.location.origin+document.location.pathname.match(/\/+\S*\/+/)+"article.html?articleid="+articleurl[articleurl.length-2];                   
//                 }else{
//                     var mainhref="javascript:void(0);"
//                 }
//                 console.log(mainhref);
//                 $('#mainlist').append("<li class='list'><a class='list"+n+"' href='"+mainhref+"'></a></li>");
//                 $('.list'+n).append("<img src='"+data[i].ArticleThumbnails+"' alt='"+data[i].ArticleTitle+"' class='listpic'/>");
//                 $('.list'+n).append("<h2>"+data[i].ArticleTitle+"</h2>");
//                 $('.list'+n).append("<p>"+data[i].ArticleContent+"</p>");
//                 n++
//             };
//         }
// })
