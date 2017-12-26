ubimodule.directive('askdetail', function () {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'directives/askdetail/tpls/index.html',
        replace: true,
        controller: function ($scope, $element, $attrs, $routeParams, commArticle, commService) {
            var pageFunc = $scope.pageFunc = {};
            var pageData = $scope.pageData = {}
        }
    }
});