comeonModule.controller('indexCtrl',['$scope',function ($scope) {
	
	var pageData = $scope.pageData = {
		title:'首页 - ' + baseData.slogan
	};
	var pageFunc = $scope.pageFunc = {};

	document.title = pageData.title;


}]);