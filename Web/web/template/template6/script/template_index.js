//首页
var mainurl="/serv/websiteapi.ashx"
var tm=new templateModule(mainurl);

tm.weixinShare();

//网站信息
tm.addsiteinfo(function(data){
    // $("title").text(data.WebsiteTitle);
    $("footer").text(data.Copyright);
    tm.weixinsharedata.desc = data.WebsiteDescription;
    tm.weixinsharedata.title = data.WebsiteTitle;
    tm.weixinsharedata.imgUrl = data.WebsiteImage
    tm.weixinsharedata.shareUrl = document.location.href;
})

//幻灯片
tm.addslider(function(data){
    var n=1;
    for (i in data) {
        $('#slides').append("<a class='slider slider"+n+"' href='"+(data[i].ProjectorTypeValue?data[i].ProjectorTypeValue:"javascript:void(0);")+"'></a>");
        $('.slider'+n).append("<span class='sliderimg' style='background-image:url("+data[i].ProjectorImage+");' ></span>'");
        if(data[i].ProjectorDescription){$('.slider'+n).append("<span class='text'>"+data[i].ProjectorDescription+"</span>")};
        n++
    };
    if(data.length===1) return;
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
        $('#category').append("<a class='cat cat"+n+"' href='"+mainhref+"'></a>");
        $('.cat'+n).append("<img src='"+data[i].NavigateImage+"' alt='"+data[i].NavigateDescription+"' class='pic'/>");
        $('.cat'+n).append("<span class='text'>"+data[i].NavigateName+"</span>");
        n++;
    };
    $('#category').css({"width":$(".cat").width()*(n-1)});
})

//底部导航
tm.addfooternav()