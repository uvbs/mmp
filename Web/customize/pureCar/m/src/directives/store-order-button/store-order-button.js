pureCarModule.directive('storeOrderButton', ['$location', function($location) {
	return {
		restrict: 'E',
		template: '<div class="storeOrderButton" ng-show="show" ng-click="action()">{{text}}</div>',
		replace: true,
		scope: {
			order: '=',
			onCanceled: '=',
		},
		link: function (scope) {
			scope.show = true;
			switch(scope.order.status) {
				case 0:
				default:
					scope.show = false;
					break;
				case 1:
					scope.text = '取消订单';
					scope.action = function () {
						getData('car/quotationInfo/cancel.ashx', {quotationId: scope.order.quotationId}).then(function() {
							scope.onCanceled(scope.order);
						});
					};
					break;
				case 2:
				case 3:
					scope.text = '重新下单';
					scope.action = function () {
						// body...
					};
					break;
			}
		}
	};
}]);
