ubimodule.directive('backbtn', function() {
	return {
		restrict: 'ECMA',
		templateUrl: baseViewPath + 'directives/backbtn/tpls/index.html',
		replace: true,
		scope:{
			url:'@'
		},
		controller:function ($scope) {
			var func = $scope.func = {};

			func.back = function() {
				if ($scope.url) {
					window.location.href = $scope.url;
				}else{
					history.go(-1);
				}
			};

		}
	};
});