ubimodule.controller('userCaseListCtrl', ['$scope','$routeParams', function($scope,$routeParams) {
	var pageData = $scope.pageData = {
		title:'发布的案例',
		userid:$routeParams.id
	};
	document.title = pageData.title;
}]);