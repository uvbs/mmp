var templateModule=function(url){
    this.url=url;
}

//在url里取值
templateModule.prototype.getQueryString = function(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
};

//加载底部导航
templateModule.prototype.addfooternav = function(callback) {
    var url=this.url;
    $.ajax({
        url:url,
        data:{action:"gettoolbarlist"},
        dataType:"jsonp",
        jsonpCallback:"addfooternav",
        success:function(data){
            callback(data)
        }
    });
};

//加载幻灯片
templateModule.prototype.addslider = function(callback) {
    var url=this.url;
    $.ajax({
        url:url,
        data:{action:"getprojectorlist"},
        dataType:"jsonp",
        jsonpCallback:"addslider",
        success:function(data){
            callback(data)
        }
    });
};

//首页中间模块
templateModule.prototype.addmodule = function(callback) {
    var url=this.url;
    $.ajax({
        url:url,
        data:{action:"getnavigatelist"},
        dataType:"jsonp",
        jsonpCallback:"addmodule",
        success:function(data){
            callback(data)
        }
    });
};

//列表页加载文章列表
templateModule.prototype.addarticlelist = function(callback) {
    var _this=this;
    var url=_this.url;
    $.ajax({
        url:url,
        data:{action:"getarticlelist",cateid:_this.getQueryString("cateid")},
        dataType:"jsonp",
        jsonpCallback:"addmodule",
        success:function(data){
            callback(data)
        }
    });
};

//列表页拼链接
templateModule.prototype.articlelisthref = function(articleurl) {
    if(articleurl){
        var articleurl=articleurl.split("/");
        var mainhref=document.location.href.match(/\S*\//)+"detail.aspx?articleid="+articleurl[articleurl.length-2];
    }else{
        var mainhref="javascript:void(0);"
    }
    return mainhref;
};


//加载文章
templateModule.prototype.addarticleconcent = function(callback) {
    var _this=this;
    var url=_this.url;
    $.ajax({
            url:url,
            data:{action:"getsinglearticle",articleid:_this.getQueryString("articleid")},
            dataType:"jsonp",
            jsonpCallback:"getarticlelist",
            success:function(data){
                callback(data)
            }
    })
};

//加载页头页尾
templateModule.prototype.addsiteinfo = function(callback) {
    var _this=this;
    var url=_this.url;
    $.ajax({
            url:url,
            data:{action:"getconfig"},
            dataType:"jsonp",
            jsonpCallback:"getsiteinfo",
            success:function(data){
                callback(data)
            }
    })
};