$(document).ready(function () {
	$(".posts li").hide();	
    size_li = $(".posts li").size();
    x=3;
    $('.posts li:lt('+x+')').show();
});
