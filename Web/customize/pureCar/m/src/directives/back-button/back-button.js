pureCarModule.directive('backButton', [function() {
	return {
		template: '<i ng-if="hasHistory()" ng-click="goBack()" class="backButton iconfont icon-xiangzuojiaohu"></i>',
		replace: true,
		controller: function ($scope) {
			$scope.hasHistory = function () {
				return true;
			};
			$scope.goBack = function () {
				history.back();
			};
		}
	};
}]);
