/**
 * Created by add on 2016/3/21.
 */
(function($){
    //用来存储获取的数据
    var pageData={
        articleId:'171684',
        commentPageIndex:1,
        commentPageSize:5,
        commentTotalCount:0,
        commentLists:[],

        replyPageIndex:1,
        replyPageSize:5,
        replyTotalCount:0,
        replyLists:[],
        isForOver:false,  //判断循环是否结束
        currComment:{},  //获取当前评论
        commentCount:0  //评论数

    };
    $.fn.getOuterParams=function(options){
        pageData.articleId=options;
    };
    //$.fn.getOuterParams({
    //    options:function(){
    //        pageData.articleId='171684'; //188143
    //    }
    //});
    var html='';
    /**
     * [timeShow 时间展示处理]
     * @param  {[type]} time [description]
     * @return {[type]}      [description]
     */
    var _timeShow = function(timeValue) {

        //几分钟前  几小时前  几天前  几月前 超过一年的则展现原数据
        var time = new Date(timeValue);
        var now = new Date();
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
            result = time.format("yyyy-MM-dd hh:mm");
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
    var _serializeData=function(data) {
            // If this is not an object, defer to native stringification.
        if(typeof (data)!='object'){
            return ((data == null) ? "" : data.toString());
        }
            //if (!angular.isObject(data)) {
            //    return ((data == null) ? "" : data.toString());
            //}
            var buffer = [];
            // Serialize each key in the object.
            for (var name in data) {
                if (!data.hasOwnProperty(name)) {
                    continue;
                }
                var value = data[name];
                buffer.push(
                    encodeURIComponent(name) + "=" + encodeURIComponent((value == null) ? "" : value)
                );
            }
            // Serialize the buffer and clean it up for transportation.
            var source = buffer.join("&").replace(/%20/g, "+");
            return (source);
        };
    var _post=function(url, reqData, callBack, failCallBack){
        $.ajax({
            type: 'POST',
            url: url,
            data: _serializeData(reqData),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            success:function(result){   //function1()
                callBack(result);
            },
            failure:function (result) {
                failCallBack(result);
            }
        });
    };
    var _get=function(url,callBack, failCallBack){
        $.ajax({
            type:"get",
            url:url,
            dataType:"json",
            success:function(result){
                callBack(result);
            },
            failure:function (result) {
                failCallBack(result);
            }
        });
    };
    var _pushArrFilterRepeat=function(arr, arrOld, field){
        if (!arrOld) {
            arrOld = [];
        }
        if (arr) {
            for (var i = 0; i < arr.length; i++) {
                var isHas = false;
                for (var j = 0; j < arrOld.length; j++) {
                    if (arrOld[j][field] == arr[i][field]) {
                        isHas = true;
                        break;
                    }
                }
                if (isHas) {
                    continue;
                } else {
                    arrOld.push(arr[i]);
                }
            }
        }
    };

    //获取评论列表
    window._getCommentList=function(isNew){
        if(isNew){
            pageData.commentLists=[];
            pageData.commentPageIndex=1;
        }
        else{
            pageData.commentPageIndex++;
        }
        var url="http://www.elao360.com/Serv/pubapi.ashx?action=getCommentList&articleid="+pageData.articleId+"&pageindex="+pageData.commentPageIndex+"&pagesize="+pageData.commentPageSize;
        _get(url,function(data){
            if (data.list && data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {
                    //相关数据转换
                    //data.list[i].content = $sce.trustAsHtml(data.list[i].content);
                    data.list[i].createDate = _timeShow(data.list[i].createDate);

                    //配置扩展属性
                    data.list[i].subListShow = false;
                    data.list[i].replySubmitShow = false;
                    data.list[i].replyCount = 99;
                    data.list[i].replyIndex = 0;
                    data.list[i].replySize = 5;

                }
            }

            pageData.commentsTotalCount = data.totalcount;
            if (data.list && data.list.length > 0) {
                pageData.commentCount+=data.list.length;

                pageData.commentLists=[];
                _pushArrFilterRepeat(data.list, pageData.commentLists, 'id');
                _getHtml();
                pageData.commentTotalCount = data.totalcount;
                $('#commentcount').html(pageData.commentTotalCount+'个评论');
                for (var i = 0; i < data.list.length; i++) {
                    pageData.currComment=data.list[i];
                    _getReplyList(data.list[i].id);
                }
            }
            //pageData.commentPageIndex++;
        },function(data){

        });
    };
    //获取回复列表
    window._getReplyList=function(id){
        var comment={};
        for(var i=0;i<pageData.commentLists.length;i++){
            if(id==pageData.commentLists[i].id){
                comment=pageData.commentLists[i];
            }
        }

        comment.replyIndex++;
        //http://www1.elao360.com/Serv/pubapi.ashx?action=getCommentReplyList&commentid=171694&pageindex=1&pagesize=5
        var url="http://www.elao360.com/Serv/pubapi.ashx?action=getCommentReplyList&commentid="+comment.id+"&pageindex="+comment.replyIndex+"&pagesize="+comment.replySize;
        _get(url,function(data){
            comment.replyCount = data.totalcount;
            if(data.list && data.list.length > 0){
                _getRepHtml(comment,data.list);
            }
            //if (data.list && data.list.length > 0) {
            //    for (var i = 0; i < data.list.length; i++) {
            //        //相关数据转换
            //        //data.list[i].content = $sce.trustAsHtml(data.list[i].content);
            //        data.list[i].createDate = _timeShow(data.list[i].createDate);
            //        //配置扩展属性
            //        data.list[i].subListShow = false;
            //        data.list[i].replySubmitShow = false;
            //
            //    }
            //}
            //comment.replyIndex++;
            //if (comment.replyCount == 0) {
            //    pageData.replyTotalCount = 0;
            //}
            //else {
            //    pageData.replyTotalCount = comment.replyCount;
            //}
            //comment.replySubmitShow = true;
            //comment.subListShow = true;
            //if (data.list && data.list.length > 0) {
            //    //replyCount
            //    pageData.replyTotalCount = data.totalcount;
            //    for (var i = 0; i < data.list.length; i++) {
            //        var isHave = false; //过滤跟视图重复的项
            //        for (var j = 0; j < comment.replyList.length; j++) {
            //            if (comment.replyList[j].id == data.list[i].id) {
            //                isHave = true;
            //                break;
            //            }
            //        }
            //        if (!isHave) {
            //            comment.replyList.push(data.list[i]);
            //        }
            //    }
            //}
            //console.log('回复列表',data);
            //if(index==(pageData.commentLists.length-1)){
            //    _getHtml();
            //}
        },function(data){

        });
    };

    //提交对文章的评论
    var _submitComment=function(){
        var url='http://www.elao360.com/Serv/pubapi.ashx';
        var reqData={
            action:'commentArticle',
            articleid:pageData.articleId,
            replyid:0,
            content:$('.areastyle').val(),
            isHideUserName:0
        };
        _post(url,reqData,function(data){
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
                alert('提交成功');
                pageData.commentLists.push({
                    id: data.returnValue,
                    content: $('.textarea').val(),
                    createDate: new Date(),
                    replyCount: 0,
                    praiseCount: 0,
                    pubUser: pageData.currUser
                });
                $('.textarea').val('');
                pageData.commentTotalCount += 1;
                $('#commentDialog').css('display','none');
            }
        },function(data){
            alert('请求失败');
        });
    };
    //提交回复
    var _submitReply=function(){
        var url='http://www.elao360.com/Serv/pubapi.ashx';
        var reqData={
            action:'replyComment',
            commentid:pageData.currComment.id==undefined?'':pageData.currComment.id,
            replyid:0,
            content:$('.areastyle').val(),
            isHideUserName:0
        };
        _post(url,reqData,function(data){
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
                alert('提交成功');
                pageData.commentLists.push({
                    id: data.returnValue,
                    content: pageData.currComment,
                    createDate: commentTime,
                    replyCount: 0,
                    praiseCount: 0,
                    pubUser: pageData.currUser
                });
                pageData.currComment = '';
                pageData.commentsTotalCount += 1;
                pageData.TotalCount++;
                $('#replyDialog').css('display','none');
            }
        },function(data){
            alert('请求失败');
        });
    };
    var _getHtml=function(isNew){
        if(pageData.commentLists && pageData.commentLists.length > 0){
            if(!isNew){
                html='';
            }
            for (var i = 0; i < pageData.commentLists.length; i ++) {
                html += '<div class="item item-avatar" data-coment-id="'+pageData.commentLists[i].id+'">'+'<img src="'+pageData.commentLists[i].pubUser.avatar+'">'
                +'<a class="colorBlue">'+pageData.commentLists[i].pubUser.userName+'</a>'+'<p class="time">'+pageData.commentLists[i].createDate+'</p>'+
                '<div class="content whiteSpaceNormal">'+pageData.commentLists[i].content+'</div>'+' <div class="operateBtns">';
                //console.log('内容',pageData.commentLists[i].content);
                if(!pageData.commentLists[i].currUserIsPraise){
                    html+=' <i class="iconfont icon-dianzan colorDDD"></i><span class="count">'+pageData.commentLists[i].praiseCount+'</span> ';
                }
                else{
                    html+=' <i class="iconfont icon-dianzan colorLightBlue"></i><span class="count">'+pageData.commentLists[i].praiseCount+'</span> ';
                }
                html+='<i onclick="openReplyDialog()" class="iconfont icon-xiaoxiguanli"></i> </div>';
                html+=' <div class="wrapSubList" >';
                html+='</div>';
                html+='</div>';
            }
            if(pageData.commentCount < pageData.commentsTotalCount){
                $('.wrapShowMore').remove();
                html+='<div class="wrapShowMore"> <button onclick="_getCommentList(false)"  class="button button-clear">查看更多</button> </div>';
            }
            else{
                $('.wrapShowMore').remove();
            }
        }
        $('#comment').append(html);
    };
    var _getRepHtml =function(comment,data){
        var nhtml  ="";
        for(var j=0;j<data.length;j++){
            nhtml+=' <div class="subItem"> <span class="userName colorBlue">'+data[j].pubUser.userName;
            if(data[j].replayToUser && data[j].replayToUser.userName && data[j].replayToUser.userName != ''){
                nhtml+='<span class="mLeft_12">回复 <a href="javascript:" class="userName mLeft5">'+data[j].replayToUser.userName+
                '</a>';
            }
            nhtml+='</span>：</span><span>'+data[j].content+'</span></div>';
        }
        //data-coment-id="'+pageData.commentLists[i].id+'"
        if(comment.replyIndex * comment.replySize <comment.replyCount){
            $('#moreReply').remove();
            nhtml +='<div id="moreReply">'+'<div onclick="_getReplyList('+comment.id+')" class="colorBlue font13">更多</div></div>';
        }
        else{
            $('#moreReply').remove();
        }
        $("[data-coment-id='"+comment.id+"']").find(".wrapSubList").append(nhtml);
    };
    //跳到评论页(添加评论)
    window.openCommentDialog=function(){
        //window.location.href=window.location.href.replace('index.html','submit.html');
        $('#commentDialog').css('display','block');
    };
    //跳到评论页（添加回复）
    window.openReplyDialog=function(){
        //window.location.href=window.location.href.replace('index.html','submit.html');
        $('#replyDialog').css('display','block');
    };
    //关闭评论弹层
    window.closeCommentDialog=function(){
        $('#commentDialog').css('display','none');
    };
    //关闭回复弹层
    window.closeReplyDialog=function(){
        $('#replyDialog').css('display','none');
    };
    //评论框的确认
    window.confirmCommentDialog=function(){
        if($('.areastyle').val()==''){
            alert('您的评论不能为空');
            return;
        }
        _submitComment();
    };
    //回复框的确认
    window.confirmReplyDialog=function(){
        if($('.areastyle').val()==''){
            alert('您的回复不能为空');
            return;
        }
        _submitReply();
    };

    var _init=function(){
        _getCommentList(true);
    };
    //启动插件
    _init();

})(jQuery);