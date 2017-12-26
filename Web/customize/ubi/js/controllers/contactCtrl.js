ubimodule.controller("contactCtrl", ['$scope', '$routeParams'
    , function ($scope, $routeParams) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '会员服务 - ' + baseData.slogan,
            id: $routeParams.id,
        };
        document.title = pageData.title;
    }
]);
