haimamodule.controller("onlineClassDetailCtrl", ['$scope', 'commService','$routeParams',
    function($scope,commService,$routeParams) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: "课堂详情",
            currPageIndex:1,
            isExpand:false,//是否显示每个课程
            isShowClassCate:true , //是否显示 课程章节和课程介绍 栏
            currSingleIndex:1, //当前选中单个课程播放

            article:[],
            currArticleId:$routeParams.id
        };
        document.title = pageData.title;

        pageFunc.loadData = function () {
            commService.getArticleDetail(function (data) {
                console.log(data);
                pageData.article = data;
            }, function (data) {
                alert('获取文章新闻失败');
            }, pageData.currArticleId);
        }
        pageFunc.init=function(){
            pageFunc.loadData();
        };
        pageFunc.init();

    }
]);