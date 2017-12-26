var mainurl = "/serv/websiteapi.ashx"
  , tm = new templateModule(mainurl);
tm.weixinShare(),
tm.addsiteinfo(function (e) {
    $("footer").text(e.Copyright),
    tm.weixinsharedata.desc = e.WebsiteDescription,
    tm.weixinsharedata.title = e.WebsiteTitle,
    tm.weixinsharedata.imgUrl = e.WebsiteImage,
    tm.weixinsharedata.shareUrl = document.location.href
}),
tm.addslider(function (e) {
    var t = 1;

    //for (i in e)
    //    $("#slides").append("<a class='slider slider" + t + "' href='" + (e[i].ProjectorTypeValue ? e[i].ProjectorTypeValue : "javascript:void(0);") + "'></a>"),
    //    $(".slider" + t).append("<img src='" + e[i].ProjectorImage + "' alt='" + e[i].ProjectorName + "' class='pic'/>"),
    //    e[i].ProjectorDescription && $(".slider" + t).append("<span class='text'>" + e[i].ProjectorDescription + "</span>"),
    //    t++;

    for (var i = 0; i < e.length; i++) {
        $("#slides").append("<a class='slider slider" + t + "' href='" + (e[i].NavigateTypeValue ? e[i].NavigateTypeValue : "javascript:void(0);") + "'></a>"),
        $(".slider" + t).append("<img src='" + e[i].NavigateImage + "' alt='" + e[i].NavigateName + "' class='pic'/>"),
        e[i].NavigateDescription && $(".slider" + t).append("<span class='text'>" + e[i].NavigateDescription + "</span>"),
        t++;
    }

    1 !== e.length && $("#slides").slidesjs({
        width: 100,
        height: 200,
        play: {
            active: !0,
            auto: !0,
            interval: 4e3,
            swap: !0
        }
    })
}),
tm.addmodule(),
tm.addfooternav();
