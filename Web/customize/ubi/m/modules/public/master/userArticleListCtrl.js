ubimodule.controller('userArticleListCtrl', ['$scope','$routeParams', function($scope,$routeParams) {
	var pageData = $scope.pageData = {
		title:'发布的文章',
		userid:$routeParams.id
	};
	document.title = pageData.title;
}]);