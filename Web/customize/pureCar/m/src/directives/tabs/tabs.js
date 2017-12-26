pureCarModule.directive('tabs', function() {
	return {
		restrict: 'E',
		template: '<ul class="pcarTabs"><li ng-repeat="tab in tabs" ng-class="{pcarTab: true, current: $index == current}" ng-click="onSelect($index)">{{tab}}</li></ul>',
		replace: true,
		scope: {
			tabs: '=',
			current: '=',
			onSelect: '='
		}
	};
});
