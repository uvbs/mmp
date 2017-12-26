var voteControl=function(){
    if(!$(".addvotenum")[0]){
        $("body").append("<div class='addvotenum'>+1</div>")
    }
    if(!$(".addvotetext")[0]){
        $("body").append("<div class='addvotetext'></div>")
    }
}
//按钮上冒数字
voteControl.prototype.note = function(obj,text) {
    $(".addvotenum").text(text)
    $(".addvotenumanimate").removeClass("addvotenumanimate");
    var top=obj.offsetTop;
    var left = obj.offsetLeft;
    left=left+(obj.offsetWidth-$(".addvotenum")[0].offsetWidth)/2;
    $(".addvotenum").css({"top":top,"left":left});
    //动画
    $(".addvotenum").addClass("addvotenumanimate");
    //动画结束归位
    setTimeout(function(){$(".addvotenum").css({"top":0,"left":0})},500);
};
//提示
voteControl.prototype.notetext = function(text,color) {
    var text=text?text:"投票成功";
    var color=color?color:"green";
    $(".addvotetext").text(text)
    $(".addvotetext").css({"left":($(window).width()-$(".addvotetext").outerWidth())/2,"top":60,"background-color": color});
    //提示动画
    $(".addvotetext").addClass("addvotetextanimate");
    //动画结束归位
    setTimeout(function(){$(".addvotetextanimate").css({"top":0,"left":0}).removeClass("addvotetextanimate")},1000);
};
//投票成功
voteControl.prototype.votesuccess = function (obj, text) {
    $(".votenumturngreen").removeClass("votenumturngreen")
    text=text?text:1;
    $(".addvotenum").css({"color":"#ff6000","font-size":"18px"})
    this.note(obj,"+"+text)
    //修改票数
    var numbox=$(obj).parent().find(".votenum")
    numbox.addClass("votenumturngreen")
    var votenum = numbox.parent().find(".votenum")
  
    setTimeout(function(){votenum.text((parseInt(votenum.text())+parseInt(text))+"票")},250);
    //提示
    this.notetext("投票成功","#63a11a")
};
//投票失败
voteControl.prototype.votefailure = function(obj,text) {
    $(".cantvote").removeClass("cantvote");
    text=text?text:"没票了";
    $(".addvotenum").css({"color":"#FF0000","font-size":"14px"})
    this.note(obj,text)
    //按钮晃动
    $(obj).addClass("cantvote")
    //提示
    this.notetext("余票不足，投票失败","#d00e0e")
};
