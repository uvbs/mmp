ubimodule.controller('indexLeftSlideDemoCtrl', function ($scope) {
	var pageData = {
		sideMenuShow:false
	};

	$('#sideMenu').slideReveal({ 
	  trigger: $("#btnShowMenu"),
	  width:150
	}); 

});