//首页
$("body").bind("touchmove",function(e){
    e.preventDefault();
})


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

//中间模块
$.ajax({
    url:mainurl,
    data:{action:"getnavigatelist"},
    dataType:"jsonp",
    jsonpCallback:"addmodule",
    success:function(data){
       if(typeof callback === "function"){
            callback(data);
       }else{
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
            $(".cat:last").bind("webkitAnimationEnd",function(){
                $("body").unbind("touchmove")
            })
       }
    }
});

//底部导航
// tm.addfooternav()