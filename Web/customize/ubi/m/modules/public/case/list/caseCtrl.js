ubimodule.controller('caseCtrl', ['$scope', '$routeParams', function ($scope, $routeParams) {
    var pageData = $scope.pageData = {
        title: '案例分析',
        tag:$routeParams.tag,
    };

    document.title = pageData.title;
}]);