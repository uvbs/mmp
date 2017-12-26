var mainurl="/serv/websiteapi.ashx"

var tm=new templateModule(mainurl);

tm.weixinShare();

//��վ��Ϣ
tm.addsiteinfo(function(data){
    // $("title").text(data.WebsiteTitle);
    $("footer").text(data.Copyright);
    tm.weixinsharedata.desc = data.WebsiteDescription;
    tm.weixinsharedata.title = data.WebsiteTitle;
    tm.weixinsharedata.imgUrl = data.WebsiteImage
    tm.weixinsharedata.shareUrl = document.location.href;
})

//�б�����
tm.addarticlelist(function(data){
    var n=1;
    for (i in data) {
        // var mainhref=tm.articlelisthref(data[i].ArticleUrl);
        var mainhref=data[i].ArticleUrl;

        //console.log(mainhref);
        $('#mainlist').append("<li class='list'><a class='list"+n+"' href='"+mainhref+"'></a></li>");
        $('.list'+n).append("<img src='"+data[i].ArticleThumbnails+"' alt='"+data[i].ArticleTitle+"' class='listpic'/>");
        $('.list'+n).append("<h2>"+data[i].ArticleTitle+"</h2>");
        $('.list'+n).append("<p>"+data[i].ArticleContent+"</p>");
        n++
    };
})

//�����������ض���������ײ�����
$(function(){
    if(tm.getQueryString("from")===null){
        //�ײ�����
        tm.addfooternav()
        //��������
        $("body").append("<div class='toptool' id='toptool'><a href='#' class='back'><span class='icon'></span></a><span class='more'></span></div>");
        $("#toptool").find(".back").attr({"href":"javascript: window.history.go(-1);"})
        $("#mainlist").css({"padding-top":"40px"})
    }
})
