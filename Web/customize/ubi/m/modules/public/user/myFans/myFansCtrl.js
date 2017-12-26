ubimodule.controller('myFansCtrl',['$scope','$routeParams', function ($scope,$routeParams) {
	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title: '粉丝',//'我的关注 - ' + baseData.slogan,
		currTabIndex: 0,
		userAutoId:$routeParams.id
	};

	document.title = pageData.title;
}]);