wisdomlifemodule.controller('caseDetailCtrl', ['$scope', 'commService','$routeParams', function ($scope, commService,$routeParams) {
    var pageData = $scope.pageData = {
        title: '',
        currArticleId:$routeParams.id,
        article:null,
        selfDataMedia:null
    };

    var pageFunc = $scope.pageFunc = {};

   
    //返回
    pageFunc.goback = function () {
        history.go(-1);
    }

    pageFunc.hideShareBg = function () {
        $scope.showweixinshareshade = false;
    };

    /**
     * [loadSelfDataMedia 加载自媒体主信息]
     * @param  {[type]} id [description]
     * @return {[type]}    [description]
     */
    pageFunc.loadSelfDataMedia = function (id) {
        commService.getArticleDetail(function (data) {
            console.log('selfDataMedia',data);
            pageData.selfDataMedia = data;
        }, function (data) {
        }, id);
    }


    //获取详情
    //获取新闻详情
    pageFunc.loadData = function () {
        commService.getArticleDetail(function (data) {
            console.log(data);
            pageData.article = data;
            //if (pageData.article.k1) {
            //    pageFunc.loadSelfDataMedia(pageData.article.k1);
            //};
            document.title = pageData.article.title;
            wx.ready(function () {
                wxapi.wxshare({
                    title: pageData.article.title,
                    desc: pageData.article.summary,
                    imgUrl: pageData.article.imgSrc
                })
            });

        }, function (data) {
            alert('获取资讯详情失败');
        }, pageData.currArticleId);
    }
    //初始化
    pageFunc.init = function () {
        pageFunc.loadData();
    }
    pageFunc.init();

}]);