/**
 * Created by add on 2016/3/24.
 */

(function($) {
    //实例化插件
    $.fn.zcComment = function (options) {
        var opts = $.extend({}, $.fn.zcComment.defaults, options);

        return this.each(function() {
            var $this = $(this);
            var o = $.meta ? $.extend({}, opts, $this.data()) : opts;
            if(o.id==0){
                $(this).remove();
                return;
            }
            var str = new StringBuilder();
            str.AppendFormat('<div class="title"> <span class="commentcount"></span>');
            if (!o.checkUserId || o.currUserId != '') str.AppendFormat('<div class="writeComment">写评论 <i class="icon iconfont icon-androidcreate font20"></i></div>');
            str.AppendFormat('</div>');
            str.AppendFormat('<div class="wrapList comment"></div>');
            if (!o.checkUserId || o.currUserId != ''){
                str.AppendFormat('<div class="weui_dialog_confirm commentDialog" style="display: none">');
                str.AppendFormat('<div class="weui_mask"></div>');
                str.AppendFormat('<div class="weui_dialog weui_dialog_reset_style">');
                str.AppendFormat('<div class="weui_dialog_hd">');
                str.AppendFormat('<strong class="weui_dialog_title">添加评论</strong>');
                str.AppendFormat('</div>');
                str.AppendFormat('<div class="weui_dialog_bd">');
                str.AppendFormat('<textarea class="areastyle txtComment" placeholder="评论将由公众号筛选后显示，对所有人可见"></textarea>');
                str.AppendFormat('</div>');
                str.AppendFormat('<div class="weui_dialog_ft">');
                str.AppendFormat('<a class="weui_btn_dialog default closeCommentDialog">取消</a>');
                str.AppendFormat('<a href="javascript:;" class="weui_btn_dialog primary confirmCommentDialog">确定</a>');
                str.AppendFormat('</div>');
                str.AppendFormat('</div>');
                str.AppendFormat('</div>');
                str.AppendFormat('<div class="weui_dialog_confirm replyDialog" style="display: none">');
                str.AppendFormat('<div class="weui_mask"></div>');
                str.AppendFormat('<div class="weui_dialog weui_dialog_reset_style">');
                str.AppendFormat('<div class="weui_dialog_hd">');
                str.AppendFormat('<strong class="weui_dialog_title">添加回复</strong>');
                str.AppendFormat('</div>');
                str.AppendFormat('<div class="weui_dialog_bd">');
                str.AppendFormat('<textarea class="areastyle txtReply" placeholder="回复将由公众号筛选后显示，对所有人可见"></textarea>');
                str.AppendFormat('</div>');
                str.AppendFormat('<div class="weui_dialog_ft">');
                str.AppendFormat('<a class="weui_btn_dialog default closeReplyDialog">取消</a>');
                str.AppendFormat('<a href="javascript:;" class="weui_btn_dialog primary confirmReplyDialog">确定</a>');
                str.AppendFormat('</div>');
                str.AppendFormat('</div>');
                str.AppendFormat('</div>');
            }
            $($this).append(str.ToString());

            $($this).find(".closeReplyDialog").bind("click",function(){
                $.fn.zcComment.closeReplyDialog($this);
            });
            $($this).find(".confirmCommentDialog").bind("click",function(){
                $.fn.zcComment.submitComment($this,o);
            });
            $($this).find(".closeCommentDialog").bind("click",function(){
                $.fn.zcComment.closeCommentDialog($this);
            });
            $($this).find(".confirmReplyDialog").bind("click",function(){
                var ncomment = $($this).find(".replyDialog").attr("data-comment");
                var nreply = $($this).find(".replyDialog").attr("data-reply");
                var nreplyuser = $($this).find(".replyDialog").attr("data-reply-user");
                $.fn.zcComment.submitReply($this,o,ncomment,nreply,nreplyuser);
            });
            $($this).find(".writeComment").bind("click",function(){
                $($this).find(".txtComment").val("");
                $.fn.zcComment.openCommentDialog($this);
            });
            $.fn.zcComment.getCommentList($this,o);
        });
    };
    //post提交
    $.fn.zcComment.post=function(url, reqData, callBack, failCallBack){
        $.ajax({
            type: 'POST',
            url: url,
            data: reqData,
            success:function(result){   //function1()
                callBack(result);
            },
            failure:function (result) {
                failCallBack(result);
            }
        });
    };
    $.fn.zcComment.format = function(time,format) {
        var o = {
            "M+": time.getMonth() + 1, //month
            "d+": time.getDate(), //day
            "h+": time.getHours(), //hour
            "m+": time.getMinutes(), //minute
            "s+": time.getSeconds(), //second
            "q+": Math.floor((time.getMonth() + 3) / 3), //quarter
            "S": time.getMilliseconds() //millisecond
        };
        if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (time.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(format))
                format = format.replace(RegExp.$1,
                    RegExp.$1.length == 1 ? o[k] :
                        ("00" + o[k]).substr(("" + o[k]).length));
        return format;
    };
    //时间格式
    $.fn.zcComment.timeShow = function(timeValue) {
        //几分钟前  几小时前  几天前  几月前 超过一年的则展现原数据
        var time = new Date(timeValue);
        var now = new Date();
        console.log(time);
        console.log(now);
        var diffValue = now.getTime() - time.getTime();

        var minute = 1000 * 60;
        var hour = minute * 60;
        var day = hour * 24;
        var halfamonth = day * 15;
        var month = day * 30;
        var year = 365 * day;

        var yearC = diffValue / year;
        var monthC = diffValue / month;
        var weekC = diffValue / (7 * day);
        var dayC = diffValue / day;
        var hourC = diffValue / hour;
        var minC = diffValue / minute;

        if (yearC >= 1) {
            result = $.fn.zcComment.format(time,"yyyy-MM-dd hh:mm");
        }
        if (monthC >= 1) {
            result = "" + parseInt(monthC) + "个月前";
        } else if (weekC >= 1) {
            result = "" + parseInt(weekC) + "周前";
        } else if (dayC >= 1) {
            result = "" + parseInt(dayC) + "天前";
        } else if (hourC >= 1) {
            result = "" + parseInt(hourC) + "小时前";
        } else if (minC >= 1) {
            result = "" + parseInt(minC) + "分钟前";
        } else {
            result = "刚刚";
        }
        return result;
        //return result;
    };
    $.fn.zcComment.getCommentList = function(_this,o){
        o.page++;
        var getCommentUrl = '/Serv/pubapi.ashx';
        var model = {
            action:'getCommentList',
            articleid: o.id,
            pageindex: o.page,
            pagesize: o.rows
        };
        $.fn.zcComment.post(o.domain+getCommentUrl,model,function(data){
            o.total = data.totalcount;
            if(o.total==undefined){
                o.total=0;
            }
            //当评论为0时只显示写评论
            if(o.total==0){
                $(_this).find('.title').remove();
                var str = new StringBuilder();
                str.AppendFormat('<div class="title borderFFF"> <span class="commentcount"></span>');
                if (!o.checkUserId || o.currUserId != '') str.AppendFormat('<div class="writeComment">写评论 <i class="icon iconfont icon-androidcreate font20"></i> </div>');
                str.AppendFormat('</div>');
                $(_this).prepend(str.ToString());
                $(_this).find(".writeComment").bind("click",function(){
                    $(_this).find(".txtComment").val("");
                    $.fn.zcComment.openCommentDialog(_this);
                });
                //$(_this).append(str.ToString());
                return;
            }
            $(_this).find('.commentcount').html(o.total+'个评论');
            if (data.list && data.list.length > 0) {
                var _comment = $(_this).find('.comment');
                for (var i = 0; i < data.list.length; i++) {
                    var str = new StringBuilder();
                    str.AppendFormat('<div class="item" data-coment-id="'+data.list[i].id+'">');
                    str.AppendFormat('<img src="'+data.list[i].pubUser.avatar+'">');
                    str.AppendFormat('<div class="userstyle">'+data.list[i].pubUser.userName+'<i class="iconfont icon-xiaoxiguanli  openReplyDialog"  data-comment="'+data.list[i].id+'" data-reply="0" data-reply-user="'+data.list[i].pubUser.userName+'" ></i>');
                    //if(!data.list[i].currUserIsPraise){
                    //    str.AppendFormat('<span class="count">'+data.list[i].praiseCount+'</span>');
                    //    if(data.list[i].praiseCount==0){
                    //        str.AppendFormat('<i class="iconfont icon-dianzankong dianzan"></i>');
                    //    }
                    //    else{
                    //        str.AppendFormat('<i class="iconfont icon-dianzan dianzan"></i>');
                    //    }
                    //}
                    //else{
                    //    str.AppendFormat('<span class="count">'+data.list[i].praiseCount+'</span>');
                    //    str.AppendFormat('<i class="iconfont icon-dianzan dianzan color4996e3"></i>');
                    //}
                    str.AppendFormat(' </div>');
                    str.AppendFormat('<div class="content">'+data.list[i].content+'</div>');
                    str.AppendFormat('<div class="monthahead"><span class="time">'+$.fn.zcComment.timeShow(data.list[i].createDate)+'</span></div>');
                    str.AppendFormat('<div class="wrapSubList" data-reply-index="0" data-reply-rows="5"  data-reply-total="'+data.list[i].replyCount+'">');
                    str.AppendFormat('</div>');
                    str.AppendFormat('</div>');
                    if($(_this).find(".wrapShowMore").length > 0){
                        $(_this).find(".wrapShowMore").before(str.ToString());
                    }
                    else{
                        $(_comment).append(str.ToString());
                    }
                    $(_comment).find(".openReplyDialog").bind("click",function(){
                        var ncomment = $(this).attr("data-comment");
                        var nreply = $(this).attr("data-reply");
                        var nreplyuser = $(this).attr("data-reply-user");
                        $(_this).find(".txtReply").val("");
                        $.fn.zcComment.openReplyDialog(_this,ncomment,nreply,nreplyuser);
                    });
                    $.fn.zcComment.getReplyList(_this,data.list[i].id,o);
                }
            }
            if(o.total > o.page * o.rows){
                if($(_this).find(".wrapShowMore").length == 0){
                    $(_comment).append('<div class="wrapShowMore"><div class="button button-clear getComment">查看更多</div></div>');
                    $(_this).find(".getComment").bind("click",function(){
                        $.fn.zcComment.getCommentList(_this,o);
                    });
                }
            }
            else{
                $(_this).find(".wrapShowMore").remove();
            }
            $(_comment).append('<div class="caution">以上评论由公众号筛选后显示</div>');
        },function(data){})
    }
    $.fn.zcComment.getReplyList = function(_this,commentId,o){
        var replySub =  $(_this).find("[data-coment-id='"+commentId+"'] .wrapSubList");
        var rpage =  Number($(replySub).attr("data-reply-index"));
        var rrows =  Number($(replySub).attr("data-reply-rows"));
        var rtotal =  Number($(replySub).attr("data-reply-total"));
        if(rtotal==0){
            $('.wrapSubList').css('border-top-color','#F4F3F2'); //borderTopColor
        }
        rpage++;
        $(replySub).attr("data-reply-index",rpage);
        var getCommentUrl = '/Serv/pubapi.ashx';
        var model = {
            action:'getCommentReplyList',
            commentid: commentId,
            pageindex: rpage,
            pagesize: rrows
        }
        $.fn.zcComment.post(o.domain+getCommentUrl,model,function(data){
            rtotal = data.totalcount;
            $(replySub).attr("data-reply-total",rtotal);
            if (data.list && data.list.length > 0) {
                for(var i=0;i<data.list.length;i++){
                    var str = new StringBuilder();
                    str.AppendFormat('<div class="subItem openRReplyDialog" data-comment="'+commentId+'" data-reply="'+data.list[i].id+'" data-reply-user="'+data.list[i].pubUser.userName+'" >');
                    str.AppendFormat('<img class="replyimg" src='+data.list[i].pubUser.avatar+'>');
                    str.AppendFormat('<div class="usernames">'+data.list[i].pubUser.userName+'<span class="pLeft5">回复 ');
                    if(data.list[i].replayToUser && data.list[i].replayToUser.userName && data.list[i].replayToUser.userName != '') {
                        str.AppendFormat('<span href="javascript:" class="replyuser">'+data.list[i].replayToUser.userName+'</span>');
                    }
                    str.AppendFormat('</span></div>');
                    str.AppendFormat(' <div class="afterUpdate">'+data.list[i].content+'</div>');
                    str.AppendFormat(' <div class="monthBefore">'+$.fn.zcComment.timeShow(data.list[i].createDate)+'</div>');
                    str.AppendFormat('</div>');
                    if( $(replySub).find(".moreReply").length > 0){
                        $(replySub).find(".moreReply").before(str.ToString());
                    }
                    else{
                        $(replySub).append(str.ToString());
                    }
                    $(replySub).find(".openRReplyDialog").bind("click",function(){
                        var ncomment = $(this).attr("data-comment");
                        var nreply = $(this).attr("data-reply");
                        var nreplyuser = $(this).attr("data-reply-user");
                        $(_this).find(".txtReply").val("");
                        $.fn.zcComment.openReplyDialog(_this,ncomment,nreply,nreplyuser);
                    });
                }
                if(rtotal > rpage * rrows){
                    if($(replySub).find(".moreReply").length == 0){
                        $(replySub).append('<div class="moreReply">'+'<div class="colorBlue font13 getReply">更多</div></div>');
                        $(replySub).find(".getReply").bind("click",function(){
                            $.fn.zcComment.getReplyList(_this,commentId,o);
                        });
                    }
                }
                else{
                    $(replySub).find(".moreReply").remove();
                }
            }
        },function(){data});

    }
    window.alert = function(msg, theme, time, fn) {
        if (!time) {
            time = 3;
        }
        layer.open({
            content: msg,
            time: time
        });
        setTimeout(function() {
            if (typeof(fn) == 'function')
                fn();
        }, time * 1000);
    };


    //跳到评论页(添加评论)
    $.fn.zcComment.openCommentDialog=function(_this){
        //window.location.href=window.location.href.replace('index.html','submit.html');
        $(_this).find('.commentDialog').show();
    };
    //跳到评论页（添加回复）
    $.fn.zcComment.openReplyDialog=function(_this,commentId,relayId,replyuser){
        //window.location.href=window.location.href.replace('index.html','submit.html');
        $(_this).find('.replyDialog').attr("data-comment",commentId);
        $(_this).find('.replyDialog').attr("data-reply",relayId);
        $(_this).find('.replyDialog').attr("data-reply-user",replyuser);
        $(_this).find('.replyDialog').css('display','block');
    };
    //关闭评论弹层
    $.fn.zcComment.closeCommentDialog=function(_this){
        $(_this).find('.commentDialog').hide();
    };
    //关闭回复弹层
    $.fn.zcComment.closeReplyDialog=function(_this){
        $(_this).find('.replyDialog').css('display','none');
    };
    ////评论框的确认
    //$.fn.zcComment.confirmCommentDialog=function(_this){
    //    if($(_this).find('.areastyle').val()==''){
    //        alert('您的评论不能为空');
    //        return;
    //    }
    //    _submitComment();
    //};
    ////回复框的确认
    //$.fn.zcComment.confirmReplyDialog=function(_this){
    //    if($(_this).find('.areastyle').val()==''){
    //        alert('您的回复不能为空');
    //        return;
    //    }
    //    _submitReply();
    //};

    //提交对文章的评论
    $.fn.zcComment.submitComment=function(_this,o){
        if($(_this).find(".txtComment").val()==''){
            alert('您的评论不能为空');
            return;
        }
        var getCommentUrl = '/Serv/pubapi.ashx';
        var reqData={
            action:'commentArticle',
            articleid:o.id,
            replyid:0,
            content:$(_this).find(".txtComment").val(),
            isHideUserName:0
        };
        $.fn.zcComment.post(o.domain+getCommentUrl,reqData,function(data){
            if (!data.isSuccess) {
                if (data.errmsg != null)
                {
                    alert('提交失败,'+ data.errmsg);
                }
                else
                {
                    alert('提交失败');
                }
            } else {
                //alert('提交成功');
                if(o.isReview){
                    alert('提交成功,请等待审核通过');
                }
                else{
                    o.total ++;
                    $(_this).find('.commentcount').html(o.total+'个评论');

                    var str = new StringBuilder();
                    str.AppendFormat('<div class="item item-avatar" data-coment-id="'+data.returnValue+'">');
                    if(o.currUser && o.currUser.avatar){
                        str.AppendFormat('<img src="'+ o.currUser.avatar+'">');
                    }
                    else{
                        str.AppendFormat('<img src="/img/europejobsites.png">');
                    }
                    if(o.currUser && o.currUser.userName){
                        str.AppendFormat('<a class="colorBlue">'+o.currUser.userName+'</a>');
                    }
                    else{
                        str.AppendFormat('<a class="colorBlue">我</a>');
                    }
                    var nsc = (new Date()).getTime();
                    str.AppendFormat('<p class="time">'+$.fn.zcComment.timeShow(nsc)+'</p>');
                    str.AppendFormat('<div class="content whiteSpaceNormal">'+reqData.content+'</div>');
                    str.AppendFormat('<div class="operateBtns">');
                    //str.AppendFormat('<i class="iconfont icon-dianzan colorDDD"></i><span class="count">0</span>');
                    str.AppendFormat('<i class="iconfont icon-xiaoxiguanli openReplyDialog" data-comment="'+data.returnValue+'" data-reply="0" ></i>');
                    str.AppendFormat('</div>');
                    str.AppendFormat('<div class="wrapSubList" data-reply-index="0" data-reply-rows="5"  data-reply-total="0">');
                    str.AppendFormat('</div>');
                    str.AppendFormat('</div>');

                    if($(_this).find('.comment').children().length ==0){
                        $(_this).find('.comment').append(str.ToString());
                    }
                    else{
                        $($(_this).find('.comment').children()[0]).before(str.ToString());
                    }
                }
                $(_this).find('.txtComment').val('');
                $(_this).find('.commentDialog').hide();
            }
        },function(data){
            alert('请求失败');
        });
    };
    //提交回复
    $.fn.zcComment.submitReply=function(_this,o,commentId,relayId,replyuser){
        if($(_this).find(".txtReply").val()==''){
            alert('您的回复不能为空');
            return;
        }
        var getCommentUrl = '/Serv/pubapi.ashx';
        var reqData={
            action:'replyComment',
            commentid:commentId,
            replyid:relayId,
            content:$(_this).find(".txtReply").val(),
            isHideUserName:0
        };
        $.fn.zcComment.post(o.domain+getCommentUrl,reqData,function(data){
            if (!data.isSuccess) {
                if (data.errmsg != null)
                {
                    alert('提交失败,'+ data.errmsg);
                }
                else
                {
                    alert('提交失败');
                }
            } else {
                //alert('提交成功');
                if(o.isReview){
                    alert('提交成功,请等待审核通过');
                }else{
                    var replySub =  $(_this).find("[data-coment-id='"+commentId+"'] .wrapSubList");
                    var ntotal = $(replySub).attr("data-reply-total");
                    ntotal++;
                    var npage=$(replySub).attr("data-reply-rows");
                    $(replySub).attr("data-reply-total",ntotal);

                    var str = new StringBuilder();
                    str.AppendFormat('<div class="subItem openRReplyDialog" data-comment="'+commentId+'" data-reply="'+relayId+'" data-reply-user="'+replyuser+'" >');
                    str.AppendFormat('<span class="userName colorBlue">'+replyuser);
                    if(replyuser && replyuser != '') {
                        str.AppendFormat('<span class="mLeft_12">回复 <a href="javascript:" class="userName mLeft5">' + replyuser + '</a>');
                    }
                    str.AppendFormat('</span>：</span><span>'+$(_this).find(".txtReply").val()+'</span></div>');
                    str.AppendFormat('</div>');
                    if(npage>=ntotal){
                        $(replySub).append(str.ToString());
                    }
                    else{
                        if($(replySub).find(".moreReply").length == 0){
                            $(replySub).append('<div class="moreReply">'+'<div class="colorBlue font13 getReply">更多</div></div>');
                            $(replySub).find(".getReply").bind("click",function(){
                                $.fn.zcComment.getReplyList(_this,commentId,o);
                            });
                        }
                    }
                    $(replySub).find(".openRReplyDialog").bind("click",function(){
                        var ncomment = $(this).attr("data-comment");
                        var nreply = $(this).attr("data-reply");
                        var nreplyuser = $(this).attr("data-reply-user");
                        //$(_this).find(".txtReply").val("");
                        $.fn.zcComment.openReplyDialog(_this,ncomment,nreply,nreplyuser);
                    });
                }
                $(_this).find(".txtReply").val("");
                $(_this).find('.replyDialog').hide();
            }
        },function(data){
            alert('请求失败');
        });
    };
    //默认值
    $.fn.zcComment.defaults = {
        id:171684,
        page:0,
        rows:5,
        total: 1,
        checkUserId: false,
        currUserId: '',
        currUser:null,
        isReview:true,  //评论是否要审核  true要审核，不实时显示评论或者回复
        domain:''  // http://comeoncloud.comeoncloud.net    http://www.elao360.com
    };
})(jQuery);