ubimodule.directive('activitylist', function () {
	return {
		restrict: 'ECMA',
		templateUrl: baseViewPath + 'directives/activitylist/tpls/index.html',
		replace: true,
		scope: {
			userid: '@',
			hasimg: '@',
			hassearch: '@'
		},
		controller: function($scope, $element, $attrs) {
			var pageFunc = $scope.pageFunc = {};
			var pageData = $scope.pageData = {
				hasImg: $scope.hasimg,
				hasSearch: $scope.hassearch,
				sideMenuShow:false
			};

			pageFunc.leftMenuClick = function(){
				var menuLeft = document.getElementById( 'cbp-spmenu-s1' ),
					body = document.body;
				classie.toggle( body, 'cbp-spmenu-push-toright' );
				classie.toggle( menuLeft, 'cbp-spmenu-open' );
			};

		}
	};
});