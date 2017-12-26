ubimodule.controller('updateDetailCtrl', ['$scope',function ($scope) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '动态详情',
        article: [],
        cateList:[],
    };

    document.title = pageData.title;


}]);
