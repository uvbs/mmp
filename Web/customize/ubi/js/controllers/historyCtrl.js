ubimodule.controller("historyCtrl", ['$scope', '$routeParams', 'userService'
    , function ($scope, $routeParams, userService) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            id: $routeParams.id,
            tab:0,
            historys: {
                pageIndex: 1,
                pageSize: 20,
                totalCount: 0,
                list: []
            },
            defines:[]
        };
        document.title = '积分历史 - ' + baseData.slogan;

        pageFunc.init = function () {
            pageFunc.loadDefines();
        }

        pageFunc.loadScoreHistory = function () {
            userService.getScoreHistorys({
                pageindex: pageData.historys.pageIndex,
                pagesize: pageData.historys.pageSize
            },
            function (data) {
                if (data.errcode && data.errcode == 10010) {
                    $scope.showLogin(function () {
                        pageFunc.loadScoreHistory();
                        pageFunc.loadDefines();
                    }, '您取消了登陆，查看积分必需先登录');
                    return;
                }
                pageData.historys.list = data.list;
                pageData.historys.totalCount = data.totalcount;
            },
            function () {
            });
        }
        pageFunc.selectTab = function (num) {
            pageData.tab = num;
            if (num == 0) {
                document.title = '积分历史 - ' + baseData.slogan;
            }
            else if (num == 1)
            {
                document.title = '如何积分 - ' + baseData.slogan;
            }
        }
        pageFunc.loadDefines = function () {
            userService.getScoreDefines(
            function (data) {
                if (data.errcode && data.errcode == 10010) {
                    return;
                }
                pageData.defines = data;
            },
            function () {
            });
        }
        $scope.$watch('pageData.historys.pageIndex', pageFunc.loadScoreHistory);
        pageFunc.init();
    }
]);