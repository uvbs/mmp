couponmodule.controller('couponListCtrl', ['$scope', function ($scope) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        currTabIndex: '',
    };

    pageFunc.init = function () {
        pageData.currTabIndex = 0;
    };
    pageFunc.init();
}]);