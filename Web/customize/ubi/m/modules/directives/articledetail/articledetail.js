ubimodule.directive('articledetail', function () {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'directives/articledetail/tpls/index.html',
        replace: true,
        scope: {
            hasimg: '@',
            hassearch: '@',
            article: '=',
            hasattention: '@',//是否显示关注
            hasview: '@',//是否显示浏览数
            optype: '@',//是否回答数
        },
        controller: function ($scope, $element, $attrs, $routeParams, commArticle, commService, userService) {
            var pageFunc = $scope.pageFunc = {};
            var pageData = $scope.pageData = {
                // article: $scope.article,
                currArticleId: $routeParams.id,
                commentsPageIndex: 1,
                commentsPageSize: 5,
                comments: [],
                hasattention: $scope.hasattention,
                optype: $scope.optype,
                hasview: $scope.hasview,
                article: {
                    title: '',
                    tags: [],
                    content: '',
                    createDate: new Date(),
                    pubUser: {
                        userName: ''
                    }
                },


                currUser: commService.getCurrUserInfo(),
                currPath: base64encode('#/ask/' + $routeParams.id),
                articleType: ''
            };
            //获取新闻详情
            pageFunc.loadData = function () {
                commArticle.getArticleDetail(function (data) {
                    console.log(data);
                    pageData.article = data;
                    if (pageData.article.categoryId == "79") {
                        pageData.articleType = "Case";
                    }
                    else if (pageData.article.categoryId == "81") {
                        pageData.articleType = "Question";
                    }

                    pageFunc.shareToFriend();
                }, function (data) {
                    alert('获取文章新闻失败');
                }, pageData.currArticleId);
            }
            //收藏文章
            pageFunc.favoriteArticle = function () {
                commArticle.favoriteArticle(function (data) {
                    if (data.isSuccess) {
                        pageData.article.currUserIsFavorite = true;
                        pageData.article.favoriteCount += 1;
                        alert('收藏成功');

                    } else if (data.errcode == 10010) {
                        $scope.showLogin(null, '您取消了登陆，继续收藏必须先登录');

                    } else if (data.errcode == 10013) {
                        alert('您已经收藏过了');

                    }
                }, function (data) { }, pageData.currArticleId);
            };
            //取消收藏
            pageFunc.disFavoriteArticle = function () {
                commArticle.disFavoriteArticle(function (data) {
                    if (data.isSuccess) {
                        pageData.article.currUserIsFavorite = false;
                        pageData.article.favoriteCount -= 1;
                        alert('取消收藏成功');
                    } else {
                        alert('取消收藏失败');
                    }
                }, function (data) { }, pageData.currArticleId);
            };
            //关注文章
            pageFunc.followArticle = function (item) {
                //判断当前用户是否登录
                if (!pageData.currUser) {
                    $scope.go('#/login/' + pageData.currPath);
                } else {
                    commArticle.followArticle(function (data) {
                        if (data.isSuccess) {
                            item.currUserIsFollow = true;
                        };
                    }, function (argument) {
                        // body...
                    }, item.id);
                }
            };
            //取消关注文章
            pageFunc.disFollowArticle = function (item) {
                //判断当前用户是否登录
                if (!pageData.currUser) {
                    $scope.go('#/login/' + pageData.currPath);
                } else {
                    commArticle.disFollowArticle(function (data) {
                        if (data.isSuccess) {
                            item.currUserIsFollow = false;
                        };
                    }, function (argument) {
                        // body...
                    }, item.id);
                }
            };
            //点击标签时跳转的页面
            pageFunc.go = function (tag) {
                var url = "";
                switch (pageData.article.categoryId) {
                    case "79":
                        url = "#/caselist/" + tag;
                        break;
                    case "83":
                        url = "#/newlist/" + tag;
                        break;
                    case "84":
                        url = "#/regulationslist/" + tag;
                        break;
                    case "81":
                        url = "#/AskList/" + tag;
                        break;
                    case "82":
                        url = "#/AskList/" + tag;
                        break;
                    default:
                        break;
                }
                window.location.href = url;
            }

            pageFunc.init = function () {
                pageFunc.loadData();
                // pageFunc.shareToFriend();
            };

            pageFunc.shareToFriend = function () {
                var shareCallBackFunc = {
                    timeline_s: function () {
                        submitShare('timeline_s');
                    },
                    timeline_c: function () {
                        //朋友圈分享取消
                    },
                    message_s: function () {
                        //分享给朋友
                        submitShare('message_s');
                    },
                    message_c: function () {
                        //朋友分享取消
                    }
                }
                var submitShare = function (WxMsgType) {
                    var reqData = {
                        Action: 'ShareSubmit',
                        url: window.location.href,
                        userId: pageData.currUser.id,
                        //userWxOpenId: pageData.currUserOpenId,
                        wxMsgType: WxMsgType
                    }

                    //分享到朋友圈
                    $.ajax({
                        type: 'post',
                        url: '/serv/pubapi.ashx',
                        data: reqData,
                        dataType: 'jsonp',
                        success: function (data) {
                            pageFunc.ShareAddScore();
                        }
                    });
                }

                wx.ready(function () {
                    wxapi.wxshare({
                        title: pageData.article.title,
                        desc: pageData.article.summary,
                        link: window.location.href
                        //  imgUrl: pageData.shareImgUrl
                    }, shareCallBackFunc)
                });
            };

            pageFunc.ShareAddScore = function () {
                userService.ShareAddScore(function (data) {
                    alert(data);
                    if (data.isSuccess) {
                        alert("分享获得积分成功！");
                    }
                    else {
                        alert("获得积分失败！");
                    }
                }, pageData.articleType);
            };
            pageFunc.init();
        }
    };
});