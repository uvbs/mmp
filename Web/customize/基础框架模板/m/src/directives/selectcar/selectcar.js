comonModule.directive('selectcar', function() {
	return {
		restrict: 'ECMA',
		templateUrl: basePath + 'directives/selectcar/tpls/index.html',
		replace: true,
		scope: {

		},
		controller: function($scope, $element, $attrs) {
			var pageFunc = $scope.pageFunc = {};
			var pageData = $scope.pageData = {
				topTabSelectIndex: 0,
				topTab: [{
					index: 0,
					title: '选择车辆型号'
				}, {
					index: 1,
					title: '没有我的车型'
				}],
				modelSelectStage:1
			};


		}
	};
});