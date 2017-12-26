ubimodule.controller("informationCtrl", ['$scope','$routeParams', 'userService'
    , function ($scope,$routeParams, userService) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '消息中心 - ' + baseData.slogan,
            id: $routeParams.id,
            keyword: "",
            notices: {
                pageIndex: 1,
                pageSize: 10,
                totalCount: 0,
                list: []
            }
        };
        document.title = pageData.title;

        pageFunc.loadNotice = function () {
            userService.getNotices({
                pageindex: pageData.notices.pageIndex,
                pagesize: pageData.notices.pageSize,
                keyword: pageData.keyword,
            },
            function (data) {

                pageData.notices.list = data.list;
                pageData.notices.totalCount = data.totalcount;
            },
            function () {
            });
        }
        $scope.$watch('pageData.notices.pageIndex', pageFunc.loadNotice);
    }
]);