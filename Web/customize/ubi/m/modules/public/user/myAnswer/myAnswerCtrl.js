ubimodule.controller('myAnswerCtrl', ['$scope', 'commArticle', '$routeParams', function ($scope, commArticle, $routeParams) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '我的回答 - ' + baseData.slogan,
        pageIndex: 1,
        pageSize: 5,
        totalCount: 0,
        id: $routeParams.id,
        dataList: [],
    };

    document.title = pageData.title;
    pageFunc.loadAnswerData = function (isLoad) {
        if (isLoad == true) {
            pageData.pageIndex++;
        }
        else {
            pageData.pageIndex = 1;
            pageData.pageSize = 5;
        }
        commArticle.loadCommentListByOption({
            pageIndex: pageData.pageIndex,
            pageSize: pageData.pageSize,
            reviewType: 'Answer',
            userAutoId: pageData.id
        }, function (data) {
            if (data) {
                for (var i = 0; i < data.list.length; i++) {
                    pageData.dataList.push(data.list[i]);
                }
            }
            pageData.totalCount = data.totalcount;
        },
        function () {
        });  
    }

    //跳转到详情
    pageFunc.go = function (item) {
        var url = "";
        url = "#/ask/" + item.articleId;
        window.location.href = url;
    }

    pageFunc.init = function () {
        pageFunc.loadAnswerData(false);
    };
    pageFunc.init();
}]);
