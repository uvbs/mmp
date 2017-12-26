comeonModule.controller('myCtrl',['$scope',function ($scope) {
	var pageData = $scope.pageData = {
		loginType:sessionStorage.getItem('loginType'),
		userId:sessionStorage.getItem('userId')
	};

	

}]);