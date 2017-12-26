pureCarModule.controller('storeIndexCtrl', ['$scope', '$location', 'ngDialog', function($scope, $location, ngDialog) {
	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title: '部落购车',
	};

	document.title = pageData.title;
	
	pageFunc.navigateTo = function (path) {
		$location.url(path);
	};

	pageFunc.init = function() {
		console.log(123);
	};

	pageFunc.init();

}]);