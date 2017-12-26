ubimodule.directive('followlist', function() {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'directives/followlist/tpls/index.html',
        replace: true,
        scope: {
            userid: '@',
            type: '@', //0表示关注，1表示粉丝
        },
        controller: function($scope, $element, $attrs, commService, userService) {
            var pageFunc = $scope.pageFunc = {};
            var pageData = $scope.pageData = {
                userInfo: [], //用户对象
                currUser: commService.getCurrUserInfo(),
                userList: {
                    check: false,
                    pageIndex: 1,
                    pageSize: 5,
                    totalCount: 0,
                    data: []
                }
            };
            pageFunc.init = function() {
                if ($scope.type == "0") {
                    pageFunc.loadData(true);
                } else {
                    pageFunc.loadData(true);
                }
            }

            pageFunc.loadData = function(isLoad) {
                if (isLoad) {
                    pageData.userList.data = [];
                    pageData.userList.pageIndex = 1;
                } else {
                    pageData.userList.pageIndex++;
                }
                var model = {
                    pageIndex: pageData.userList.pageIndex,
                    pageSize: pageData.userList.pageSize,
                    userAutoId: pageData.currUser ? pageData.currUser.id : ''
                }

                if ($scope.userid) {
                    model.userAutoId = $scope.userid;
                };

                if ($scope.type == "0") {
                    userService.getFollowUsers(model, function(data) {
                            if (data.list) {
                                pageData.userList.totalCount = data.totalcount;
                                for (var i = 0; i < data.list.length; i++) {
                                    pageData.userList.data.push(data.list[i]);
                                }
                            }

                        },
                        function() {
                            alert("加载数据失败！");
                        });
                } else {
                    userService.getFansUsers(model, function(data) {
                            if (data.list) {

                                pageData.userList.totalCount = data.totalcount;
                                for (var i = 0; i < data.list.length; i++) {
                                    pageData.userList.data.push(data.list[i]);
                                }

                            };
                        },
                        function() {
                            alert("加载数据失败！");
                        });
                }

            }

            pageFunc.loadMore = function() {
                if ($scope.type == "0") {
                    pageFunc.loadData(false);
                } else {
                    pageFunc.loadData(false);
                }
            }

            pageFunc.init();
        }
    };
});