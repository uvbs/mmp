ubimodule.controller('favoriteArticleCtrl', ['$scope', 'userService', '$routeParams', function ($scope, userService, $routeParams) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '我的收藏 - ' + baseData.slogan,
        pageIndex: 1,
        pageSize: 5,
        totalCount:0,
        id: $routeParams.id,
        dataList:[],
    };

    document.title = pageData.title;
    pageFunc.loadFavoriteData = function (isLoad) {
        if (isLoad == true)
        {
            pageData.pageIndex++;
        }
        else {
            pageData.pageIndex = 1;
            pageData.pageSize = 5;
        }
        userService.getUserFavoriteListByOption({
            pageIndex: pageData.pageIndex,
            pageSize: pageData.pageSize,
            userAutoId: pageData.id
        }, function (data) {
            if (data)
            {
                for(var i=0;i<data.list.length;i++)
                {
                    pageData.dataList.push(data.list[i]);
                }
            }
            pageData.totalCount = data.totalcount;
           // pageData.dataList.push(data.list);
        },
        function () {
        })
    };
    //跳转到详情
    pageFunc.go = function (item) {
        var url = "";
        url = "#/ask/" + item.id;
        window.location.href = url;
    }

    pageFunc.init = function () {
        pageFunc.loadFavoriteData(false);
    };
    pageFunc.init();
}]);
