pureCarModule.directive('orderButton', ['$location', function($location) {
	return {
		restrict: 'E',
		template: '<div class="orderButton" ng-click="action()" ng-disabled="disabled">{{text}}</div>',
		replace: true,
		scope: {
			order: '='
		},
		link: function (scope) {
			switch(scope.order.orderStatus) {
				case 0:
				default:
					scope.text = '点评';
					scope.action = function () {
						$location.path('service/orders/' + scope.order.id + '/rate');
					};
					break;
				case 1:
					scope.text = '取消订单';
					break;
				case 2:
					scope.disabled = true;
					scope.text = '已点评';
					break;
			}
		}
	};
}]);
