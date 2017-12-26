// JavaScript Document
function pwwrong(){
    shock();
	$(".verificationbox").fadeIn("slow");
	$("#password").focus().parents(".textinput").addClass("inputfocus").addClass("pwwrong");
	$(".prompt").css("color","#cb3d3d").text("密码错误！请重新输入")
	}
function idwrong(){
	shock();
	$("#account").focus().parents(".textinput").addClass("inputfocus").addClass("pwwrong");
	$(".prompt").css("color","#cb3d3d").text("没有此账号！请重新输入")
	}
function shock(){
	var left=100;
	for (i = 1; i < 7; i++){
		$('.loginbox').animate({'left': '-=4'+'%'},3,function(){
			$(this).animate({'left': '+=8'+'%'}, 3, function() {
				$(this).animate({'left': '-=8'+'%'}, 3, function() {
					$(this).animate({'left': '+=8'+'%'}, 3, function() {
						$(this).animate({'left': '-=4'+'%'}, 3, function() {
							$(this).animate({'left': box_left+'%'}, 3, function() {
                        // shock end
						});
					});
				});
			});
		});
	});
    }
}
function mainbgshow(){
	$(".mainbg").fadeIn("slow");
	}