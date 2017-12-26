$(function(){
    var id="508939";
    $.get("http://comeoncloud.comeoncloud.net/serv/pubapi.ashx?action=getnewsdetail&newsid="+id,function(data,status){
        $("#article-content").append($(data.newscontent).addClass("full-image"));
    });
});