wisdomlifemodule.controller('advertiserDetailCtrl', ['$scope', 'commService','$routeParams', 
    function ($scope, commService,$routeParams) {
    var pageData = $scope.pageData = {
        title: '广告主详情',
        currArticleId:$routeParams.id,
        article:null,
    };

    var pageFunc = $scope.pageFunc = {};

    document.title = pageData.title;

    //返回
    pageFunc.goback = function () {
        history.go(-1);
    }
    //获取详情
    //获取新闻详情
    pageFunc.loadData = function () {
        commService.getArticleDetail(function (data) {
            console.log(data);
            pageData.article = data;
        }, function (data) {
            alert('获取广告主详情失败');
        }, pageData.currArticleId);
    }
    //初始化
    pageFunc.init = function () {
        pageFunc.loadData();
    }
    pageFunc.init();
    
}]);