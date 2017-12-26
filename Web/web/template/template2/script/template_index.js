var mainurl="/serv/websiteapi.ashx"
var tm=new templateModule(mainurl);

//幻灯片
tm.addslider(function(data){
    var n=1;
    for (i in data) {
        $('#slides').append("<a class='slider slider"+n+"' href='"+(data[i].ProjectorTypeValue?data[i].ProjectorTypeValue:"javascript:void(0);")+"'></a>");
        $('.slider'+n).append("<img src='"+data[i].ProjectorImage+"' alt='"+data[i].ProjectorName+"' class='pic'/>");
        $('.slider'+n).append("<span class='text'>"+data[i].ProjectorDescription+"</span>");
        n++
    };
    $("#slides").slidesjs({
        width: 100,
        height: 200,
        play: {
            active: true,
            auto: true,
            interval: 4000,
            swap: true
        }
    });
})

//中间模块
tm.addmodule(function(data){
    var n=1;
    for (i in data) {
        if(data[i].NavigateType==="链接"){
            var mainhref=data[i].NavigateTypeValue;
        }else if(data[i].NavigateType==="分类"){
            var mainhref=document.location.href.match(/\S*\//)+"list.aspx?cateid="+data[i].NavigateTypeValue;
        }else if(data[i].NavigateType==="图文"){
            var mainhref=data[i].NavigateTypeValue;
        }
        //console.log(mainhref)
        $('#category').append("<a class='cat cat"+n+"' href='"+mainhref+"'></a>");
        $('.cat'+n).append("<img src='"+data[i].NavigateImage+"' alt='"+data[i].NavigateDescription+"' class='pic'/>");
        $('.cat'+n).append("<span class='text'>"+data[i].NavigateName+"</span>");
        n++;
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
        $('.navbtn'+n).css({width:100/maxn+"%"})
        n++
    };
})


//网站信息
tm.addsiteinfo(function(data){
    $("title").text(data.WebsiteTitle);
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
//         }, function (res) {
//         })
//     });
//     WeixinJSBridge.on('menu:share:weibo', function (argv) {
//         WeixinJSBridge.invoke('shareWeibo', {
//             "content": title,
//             "url": shareUrl
//         }, function (res) {
//         });
//     });
// }, false)
