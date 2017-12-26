//控制表格高度
function tableHeight(mainctn,num,arr){
    var _this=this;
    this.mainheight=num;
    for(var i=0;i<arr.length;i++){
        this.mainheight+=$(arr[i]).outerHeight()
    }

    this.changeheight = function(first_argument) {
        $(mainctn).css({"height":$(window).height()-_this.mainheight});
    }
    thtimeout=setTimeout(function(){_this.changeheight()},100);
    $(window).resize(function(){
        clearTimeout(thtimeout);
        thtimeout=setTimeout(function(){_this.changeheight()},100);
    })
};