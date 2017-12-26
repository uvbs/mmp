pureCarModule.controller('storeIndexCtrl', ['$scope', '$location', 'ngDialog', 'selectedCarModel', function($scope, $location, ngDialog, selectedCarModel) {
	var pageFunc = $scope.pageFunc = {
		navigateTo: function (path) {
			$location.url(path);
		},
		selectCarModel: function ($event) {
			$event.target.blur();
			ngDialog.open({
				template: 'storeSelectCarDialog',
				scope: $scope
			});
		},
		selectModel: function (model) {
			selectedCarModel.set(model);
			pageFunc.navigateTo('/store/demand');
			ngDialog.closeAll();
		}
	};
	var pageData = $scope.pageData = {
		title: '部落购车',
	};

	document.title = pageData.title;

}]);