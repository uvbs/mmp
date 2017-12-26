ubimodule.controller('loginCtrl', ['$scope','commService', '$routeParams',function($scope,commService,$routeParams) {

	var pageData = $scope.pageData = {
		title: '用户登录',
		userName:'',
		pwd:'',
		ref:$routeParams.ref ? base64decode($routeParams.ref):'#/my'
	},
	pageFunc = $scope.pageFunc = {};
	
	document.title = pageData.title;

	pageFunc.login = function () {

		if (pageData.userName == '') {
			alert('请输入登陆账号');
			return;
		};
		if (pageData.pwd == '') {
			alert('请输入登陆密码');
			return;
		};

		commService.login(pageData.userName,pageData.pwd,0,false,function (data) {
			console.log(data);
			if (data.issuccess) {
				$scope.go(pageData.ref);
			}else{
				alert(data.message);
			}
		});
	};

}]);