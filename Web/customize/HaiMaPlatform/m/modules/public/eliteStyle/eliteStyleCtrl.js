haimamodule.controller("eliteStyleCtrl", ['$scope', 'commService',
    function($scope,commService) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: "精英风采",
            currPageIndex:1,
            pageIndex:1,
            pageSize:5,
            totalCount:0,
            article:[]
        };
        document.title = pageData.title;

        pageFunc.loadData = function(isNew) {
            var pageii = layer.open({
                type: 2
            });
            if (isNew) {
                pageData.pageIndex=1;
            } else {
                pageData.pageIndex++;
            }
            var model = {
                cateId: baseData.moduleCateIds.knowledge,
                type: 'Article',
                pageIndex: pageData.pageIndex,
                pageSize: pageData.pageSize,
                orderby: ''
            };
            commService.getArticleListByOption(model, function(data) {
                pageData.totalCount = data.totalcount;
                for (var i = 0; i < data.list.length; i++) {
                    pageData.article.push(data.list[i]);
                }
                layer.close(pageii);
            }, function(data) {
                layer.close(pageii);
            });
        };
        pageFunc.go=function(id){
            window.location.href="/m#/news/"+id;
        };
        pageFunc.init= function () {
            pageFunc.loadData(true);
        };
        pageFunc.init();
    }
]);