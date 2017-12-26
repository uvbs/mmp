comeonModule.controller('loginCtrl',['$scope','commService',function ($scope,commService) {
	
	var pageData = $scope.pageData = {
		title:'登录 - ' + baseData.slogan,
		loginType:1,
		userId:'',
		pwd:''
	};
	var pageFunc = $scope.pageFunc = {};

	document.title = pageData.title;

	pageFunc.changeLoginType = function (value) {
		pageData.loginType = value;
	};


	pageFunc.login = function () {
		var jsonData = {
			Action: 'Login', 
			UserName: pageData.userId, 
			PassWord: pageData.pwd, 
			UserType:pageData.loginType
		};

		if (jsonData.UserName == '') {
			alert('请输入登录名');
			return;
		};
		if (jsonData.PassWord == '') {
			alert('请输入密码');
			return;
		};

		commService.postData(baseData.handlerUrl,jsonData,function (data) {
			if (data.Status == 1) {
				sessionStorage.setItem('islogin',1);
				sessionStorage.setItem('loginType',pageData.loginType);
				sessionStorage.setItem('userId',pageData.userId);
				sessionStorage.setItem('user',JSON.stringify(data.ExObj));
				window.location.href = "#/my";
			}else{
				alert(data.Msg);
			}
		},function (data) {
			// body...
			console.log(data);
		});

	};


}]);