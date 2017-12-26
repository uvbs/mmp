pureCarModule.controller('serviceCouponCtrl', ['$scope', '$location', 'ngDialog', function($scope, $location, ngDialog) {
	var pageFunc = $scope.pageFunc = {
		setTabIndex: function (i) {
			pageData.tabIndex = i;
		}
	};
	var pageData = $scope.pageData = {
		title: '我的钱包',
		tabIndex: 1,
		couponStatus: 0
	};

	document.title = pageData.title;
}]);