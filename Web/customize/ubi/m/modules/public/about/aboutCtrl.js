ubimodule.controller("aboutCtrl", ['$scope', 'commArticle', '$routeParams', function ($scope, commArticle, $routeParams) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '合作机构 - ' + baseData.slogan,
        currArticleId: $routeParams.id,
        article: null,
    };
    document.title = pageData.title;

    pageFunc.init = function () {
        commArticle.getArticleDetail(function (data) {
            pageData.article = data;
        }, function (data) {
            alert('获取失败');
        }, pageData.currArticleId);
    };

    pageFunc.init();

}]);