$(function(){
	$(".checktagbar").click(function(e){
		$(this).parent().find(".current").removeClass("current")
		$(this).addClass("current")
		if($(".tag1").hasClass("current")){
			$(".selfpickup").hide();
			$(".express").show();
		}else{
			$(".express").hide();
			$(".selfpickup").show();
		}
	})


var cartprice={
	addprice:function(){
		var totalprice=0;
		var totalnum=0;
		$(".checkbox").each(function(){
			if($(this).prop("checked")){
				var p=$(this).parent();
				var price=p.find(".price").text();
				var num=Number(p.find(".num")[0].value);
				price=Number(price.substring(1,price.length));
				totalprice+=price*num;
				totalnum+=num;
			}
		})
		$(".totalnum").text(totalnum);
		$(".totalprice").text(totalprice.toFixed(2));
		// console.log(totalprice.toFixed(2)+"---"+totalnum);
	},
	addnum:function(container){
		var numc=container.parent().find(".num")[0];
		numc.value++;
	},
	minus:function(container){
		var numc=container.parent().find(".num")[0];
		if(numc.value>1){
			numc.value--;
		}
	}
}


$(".checkbox").change(cartprice.addprice)

$(".addbtn").click(function(){
	cartprice.addnum($(this));
	cartprice.addprice();
});
$(".minus").click(function(){
	cartprice.minus($(this));
	cartprice.addprice();
});

	// $(".product").each(function(index){
	// 	var _this=$(this);
	// 	_this.find(".checkbox")
	// })


})