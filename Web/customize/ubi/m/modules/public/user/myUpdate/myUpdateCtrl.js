ubimodule.controller('myUpdateCtrl', ['$scope', '$routeParams', 'commArticle', 'commService',
    function ($scope, $routeParams, commArticle, commService) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '我的动态',

            pageIndex: 1,
            pageSize: 8,
            totalCount: 0,

            myUpdateList: [],//存放我的动态信息
            currUser: commService.getCurrUserInfo(),
            currPath: '',
            id: $routeParams.id,
        };

        document.title = pageData.title;

        //跳转到详情
        pageFunc.go = function (id) {
            var url = "#/update/" + id;
            window.location.href = url;
        }
        //点赞
        pageFunc.praiseReview = function (model) {
            //判断当前是否已登陆
            if (!pageData.currUser) {
                $scope.go('#/login/' + pageData.currPath);
            } else {
                commArticle.praiseReview(
                model.id,
                function (data) {
                    if (data.isSuccess) {
                        model.praiseCount += 1;
                    } else if (data.errcode == 10010) {
                        $scope.showLogin(null, '您取消了登陆，继续点赞必须先登录');
                    }
                },
                function (data) {

                }
            );
            }
        };
        //举报
        pageFunc.reportIllegalReview = function (model) {
            //判断当前是否已登陆
            if (!pageData.currUser) {
                $scope.go('#/login/' + pageData.currPath);
            } else {
                commArticle.reportIllegalReview(
                model.id,
                function (data) {
                    if (data.isSuccess) {
                        alert('举报成功');
                    } else if (data.errcode == 10010) {
                        $scope.showLogin(null, '您取消了登陆，继续举报必须先登录');
                    } else if (data.errcode == 10013) {
                        alert('您已经举报过了');
                    }
                },
                function (data) {

                }
            );
            }
        };
        //收藏
        pageFunc.favoriteArticle = function (model) {
            //判断当前是否已登陆
            if (!pageData.currUser) {
                $scope.go('#/login/' + pageData.currPath);
            } else {
                commArticle.favoriteArticle(
                function (data) {
                    if (data.isSuccess) {
                        alert('收藏成功');
                    } else if (data.errcode == 10010) {
                        $scope.showLogin(null, '您取消了登陆，继续收藏必须先登录');
                    } else if (data.errcode == 10013) {
                        alert('您已经收藏过了');
                    }
                },
                function (data) {

                },
                model.id
            );
            }
        };
        //初始化和加载时 --最新动态 isNew为true时表示为初始化
        pageFunc.loadData = function (isNew) {
            if (isNew) {
                pageData.pageIndex = 1;
                pageData.totalCount = 0;
                pageData.myUpdateList = [];
            }
            else {
                pageData.pageIndex++;
            }
            commArticle.getArticleListByOption({
                pageIndex: pageData.pageIndex,
                pageSize: pageData.pageSize,
                type: 'Statuses',
                author: pageData.currUser.id
            }, function (data) {
                pageData.totalCount = data.totalcount;
                if (data && data.list) {
                    if (isNew) {
                        pageData.myUpdateList = data.list;
                    } else {
                        for (var i = 0; i < data.list.length; i++) {
                            pageData.myUpdateList.push(data.list[i]);
                        }
                    }
                }
            },
            function () {
            });
        };
        pageFunc.init = function () {
            pageData.currPath = base64encode('#/update/' + pageData.currUser.id);
            pageFunc.loadData(true);
        }

        pageFunc.init();
    }]);