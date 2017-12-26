//文章页

var mainurl="/serv/websiteapi.ashx";
var tm=new templateModule(mainurl);

tm.addarticleconcent(function(data){
    $("#article").append("<h2 class='title'>"+data.ArticleTitle+"</h2>");
    $("#article").append(data.ArticleContent);

    //$("title").text(data.ArticleTitle);
    var desc=data.ArticleContent.replace(/[x00-xff]|\-|\"|\/|\.|\#|\s*|/ig,"").substr(0,50);
    var title = data.ArticleTitle;
    var imgUrl = data.ArticleThumbnails
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
        $("#article").css({"padding-top":"40px"})
    }
    //分享
    $$(".btnbox").tap(function() {
        console.log(0)
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
    $("footer").text("©"+data.Copyright);
})



// $.ajax({
//         url:"http://dev.comeoncloud.net/serv/websiteapi.ashx",
//         data:{action:"getsinglearticle",articleid:getQueryString("articleid")},
//         dataType:"jsonp",
//         jsonpCallback:"getarticlelist",
//         success:function(data){
//             // console.log(data);
//             $("#article").append("<h2 class='title'>"+data.ArticleTitle+"</h2>");
//             $("#article").append(data.ArticleContent);
//         }
// })

// var desc = "网站介绍未填写网站介绍未填写网站介绍未填写网站介绍未填写";
// var title = "请填写网站标题";
// var imgUrl = "http://"+document.location.host+document.location.pathname.match(/\/+\S*\/+/)+"styles/images/zhiyunlogo.jpg"
// console.log(imgUrl);
// var shareUrl = document.location.href;
// document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
//     WeixinJSBridge.on('menu:share:appmessage', function (argv) {
//         WeixinJSBridge.invoke('sendAppMessage', {
//             //"appid": appId,
//             "img_url": imgUrl,
//             "img_width": "400",
//             "img_height": "400",
//             "link": shareUrl,
//             "desc": desc,
//             "title": title
//         }, function (res) {})
//     });
//     WeixinJSBridge.on('menu:share:weibo', function (argv) {
//         WeixinJSBridge.invoke('shareWeibo', {
//             "content": title,
//             "url": shareUrl
//         }, function (res) {});
//     });
// }, false)

