var touchFloat=function(container){
    this.cwidth=$(container).width();
    this.box=$(container).children();
    this.starttime=0;//手势进入时间
    this.intouch={x:0,y:0};//手势进入时间点
    this.movep={x:false,y:false};//手势移动方向
    this.position={x:0,y:0};//初始位置
    this.move={x:0,y:0};//移动距离
    this.init()//初始化
    var _this=this;
    this.touchmovef = function(ev) {
        ev.preventDefault();
        // $(".mainlist").text(ev.timeStamp);
        _this.starttime=ev.timeStamp;
        _this.move.x=ev.touches[0].clientX-_this.intouch.x;
        _this.move.y=ev.touches[0].clientY-_this.intouch.y;
        //判断方向
        if(Math.abs(_this.move.x)>=10&&!_this.movep.x&&!_this.movep.y){
            _this.movep.x=true;
        }else if(Math.abs(_this.move.y)>=10&&!_this.movep.x&&!_this.movep.y){
            _this.movep.y=true;
        }
        //根据方向运动
        if(_this.movep.x){
            //左右移动
            $$(_this.box[0]).vendor("transform","translate3d("+(_this.position.x+_this.move.x)+"px, 0, 0)");
        }else if(_this.movep.y){
            //上下移动  恢复浏览器默认拖动
            _this.box[0].removeEventListener("touchmove",_this.touchmovef,false)
        }
    };
    this.touchendf = function(ev) {
        //touchend 恢复侦听
        if(_this.movep.y){
            _this.movep.y=false;
            _this.box[0].addEventListener("touchmove",_this.touchmovef,false);
        }
        _this.movep.x=false;
        _this.position.x=_this.position.x+_this.move.x//更新位置
        _this.position.y=_this.position.y+_this.move.y//更新位置
        //超出位置归位
        if(_this.position.x>0||_this.cwidth>_this.box.width()){
            _this.position.x=0;
            $$(_this.box[0]).vendor("transition","300ms");
            $$(_this.box[0]).vendor("transform","translate3d("+_this.position.x+"px, 0, 0)");
        }else if((_this.position.x+_this.box.width())<_this.cwidth){
            _this.position.x=_this.cwidth-_this.box.width();
            $$(_this.box[0]).vendor("transition","300ms");
            $$(_this.box[0]).vendor("transform","translate3d("+_this.position.x+"px, 0, 0)");
        }
    };

    //清除动画
    this.box[0].addEventListener("webkitTransitionEnd",function(ev){
        $$(_this.box[0]).vendor("transition","0ms");
    },false)

    this.box[0].addEventListener("touchstart",function(ev){
        _this.touchstartf(ev);
    },false)

    this.box[0].addEventListener("touchmove",_this.touchmovef,false)


    this.box[0].addEventListener("touchend",_this.touchendf,false)
}

touchFloat.prototype.test = function(first_argument) {
        var textconcent=movexnum+"<br/>";
        evarr=ev.targetTouches[0]
        for(dd in evarr){
            textconcent+=dd+":"+evarr[dd]+"<br>"
        }
        $(".mainlist").html(textconcent)
};

touchFloat.prototype.touchstartf = function(ev) {
    var _this=this;
    $$(_this.box[0]).vendor("transition","0ms");
    _this.starttime=ev.timeStamp;
    _this.intouch.x=ev.touches[0].clientX;
    _this.intouch.y=ev.touches[0].clientY;
};


touchFloat.prototype.init = function(first_argument) {
    var box=this.box;
    var boxwidth=0;
    box.children().each(function(){
        boxwidth+=$(this).outerWidth();
    })
    box.css({"width":boxwidth})
};

$(function () { 
var touchfloat=new touchFloat($(".kindbox"));
})


// function preventDefault(e) { 
//     e.preventDefault(); 
//     $(".kind")[0].removeEventListener('touchmove', preventDefault, false);
// }

// $(".kind")[0].addEventListener('touchmove', preventDefault2, false);
