pureCarModule.directive('homeHeader', ['ngDialog', function(ngDialog) {
	return {
		templateUrl: basePath + 'directives/home-header/home-header.html',
		replace: true,
		scope: {
			homeType: '=',
			carModel: '=carModel'
		},
		controller: function($scope, $element, $attrs) {
			var pageFunc = $scope.pageFunc = $scope.pageFunc || {};
			pageFunc.showSelectCarDialog = function () {
				ngDialog.open({
					template: publicPath + 'service/index/tpls/selectCarDialog.html',
					scope: $scope
				});
			};
			pageFunc.selectModel = function (model) {
				console.log(model);
			}
		}
	};
}]);