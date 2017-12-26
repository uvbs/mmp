var scrollbar=function(container,moreheight){
    this.moreheight=moreheight?moreheight:0;
    this.container=$(container);
    this.cheight=$(container).height();//外框高度
    this.offsettop=0;
    this.dheight=0;//文档高度
    this.scrollbar=false;//滚动条是否显示
    this.barclicky=-1;
    this.mouseclicky=0;
    this.container.append("<div class='scrollbarbox'><div class='scrollbar'></div></div>");
    $(".scrollbarbox").css({"position":"absolute","top":"0px","right":"0px","width":"10px","height":"100%","background-color":"rgba(0,0,0,0.3)","display":"none"});
    $(".scrollbar").css({"position":"absolute","top":"0px","right":"1px","width":"6px","height":"30%","background-color":"rgba(255,255,255,0.3)","border-radius":"8px","border-left":"1px solid #888","border-right":"1px solid #555"});
    this.init();
    this.control();
}

scrollbar.prototype.init = function(cheight) {
    var _this=this;
    if(cheight){
        _this.container.css({"height":cheight})
        _this.cheight=_this.container.height();
    }
    var cc=_this.container.children();
        cc=cc[cc.length-2];
    _this.dheight=cc.offsetTop+cc.clientHeight+_this.moreheight;//更新文档高度
    // console.log("dheight:"+_this.dheight+"--cheight:"+_this.cheight)
    _this.scrollbar=_this.dheight>_this.cheight?true:false;//是否出现滚动条
    if(_this.scrollbar){
        $(".scrollbarbox").css({"display":"block"});
        $(".scrollbar").css({"height":_this.cheight/_this.dheight*100+"%"});
        _this.move();
    }else{
        $(".scrollbarbox").css({"display":"none"});
        this.offsettop=0;
        _this.move();
    }
};

scrollbar.prototype.control = function() {
    var _this=this;
    this.container.mousewheel(function(e){
        if(!_this.scrollbar) return;
        if(e.deltaY<0){
            _this.offsettop+=20;
        }else if(e.deltaY>0){
            _this.offsettop-=20;
        }
        _this.move();
    })
    $(".scrollbar").mousedown(function(e){
        console.log(e)
        _this.checkbar=true;
        _this.mouseclicky=e.clientY;
        _this.barclicky=_this.offsettop;
    })
    $(window).mouseup(function(){
        _this.barclicky=-1;
    })
    $(window).mousemove(function(e){
        if(_this.barclicky===-1) return;
        e.clientY-_this.mouseclicky

        _this.offsettop=_this.barclicky+(e.clientY-_this.mouseclicky)*_this.dheight/_this.cheight;
        _this.move();

    })
};

scrollbar.prototype.move=function(){
    var _this=this;
    var d=_this.dheight-_this.cheight;
    if(_this.offsettop>=d)_this.offsettop=d;//滚动条不能超出最下方
    if(_this.offsettop<=0)_this.offsettop=0;//滚动条不能超出最上方
    _this.container[0].scrollTop=_this.offsettop;
    $(".scrollbarbox").css({"top":_this.container[0].scrollTop});
    $(".scrollbar").css({"top":_this.container[0].scrollTop*_this.cheight/_this.dheight});
    //console.log(_this.container[0].scrollTop);
}
