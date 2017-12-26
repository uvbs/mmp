wisdomlifemodule.controller('caseCtrl', ['$scope', 'commService', '$routeParams', function ($scope, commService, $routeParams) {
    var pageData = $scope.pageData = {
        title: '案例',
        cateId: $routeParams.cateId,
        page: 3
    };

    var pageFunc = $scope.pageFunc = {};

    document.title = pageData.title;

    pageFunc.init = function () {
        if (pageData.cateId > 3) {
            pageData.page = pageData.cateId;
            pageData.hasClassify = 1;
        }
    };

    pageFunc.init();

}]);