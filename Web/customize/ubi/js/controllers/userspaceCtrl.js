ubimodule.controller("userspaceCtrl", ['$scope', '$routeParams', 'commService', 'userService', 'commArticle','$modal',
	function ($scope, $routeParams, commService, userService, commArticle,$modal) {

	    var pageFunc = $scope.pageFunc = {};
	    var pageData = $scope.pageData = {
	        title: '用户空间 - ' + baseData.slogan,
            userRole:'用户',
	        id: $routeParams.id,
	        userInfo: [],
	        currUser: commService.getCurrUserInfo(),
	        defList: {
	            check: true,
	            caseDef: [],
	            questionDef: [],
	            articleDef: [],
	            answerDef: [],
	            favoriteDef: [],
	            statusesDef: []
	        },
	        caseList: {
	            check: false,
	            pageIndex: 1,
	            pageSize: 5,
	            totalCount: 0,
	            data: []
	        },
	        questionList: {
	            check: false,
	            pageIndex: 1,
	            pageSize: 5,
	            totalCount: 0,
                data:[]
	        },
	        articleList: {
	            check: false,
	            pageIndex: 1,
	            pageSize: 5,
	            totalCount: 0,
	            data: []
	        },
	        answerList: {
	            check: false,
	            pageIndex: 1,
	            pageSize: 5,
	            totalCount: 0,
	            data: []
	        },
	        favoriteList: {
	            check: false,
	            pageIndex: 1,
	            pageSize: 5,
	            totalCount: 0,
	            data: []
	        },
	        statusesList: {
	            check: false,
	            pageIndex: 1,
	            pageSize: 5,
	            totalCount: 0,
	            data: []
	        },
	        fansList: {
	            check: false,
	            pageIndex: 1,
	            pageSize: 8,
	            totalCount: 0,
	            data: []
	        },
	        followUserList: {
	            check: false,
	            pageIndex: 1,
	            pageSize: 8,
	            totalCount: 0,
	            data: []
	        },
            shareRegLink:'',
            isNull:''
	    };

	    pageFunc.init = function () {
	        //pageFunc.shareReg();
	        pageFunc.loadUserData();
	    };
        
	    pageFunc.loadUserData = function () {
	        userService.getUserInfo(pageData.id, function (data) {
	            if(data.errcode){
	                alert('找不到该用户');
	            }
	            else{
	                pageData.userInfo = data;
                    pageData.shareRegLink = "http://www.elao360.com/#/register/" + pageData.userInfo.pId;
	                if (data.isCurrUser) {
	                    userRole = "个人";
	                    pageData.title = '个人空间 - ' + baseData.slogan;
	                }
	                else if (data.isTutor) {
	                    userRole = "专家";
	                    pageData.title = '专家空间 - ' + baseData.slogan;
	                }
	                else {
	                    userRole = "用户";
	                    pageData.title = '用户空间 - ' + baseData.slogan;
	                }
	                document.title = pageData.title;
	            }
	        });
	    }

	    pageFunc.loadCaseData = function () {
	        commArticle.getArticleListByOption({
	            cateId: baseData.moduleCateIds.case,
	            pageIndex: pageData.caseList.pageIndex,
	            pageSize: pageData.caseList.pageSize,
	            type: 'Article',
	            author: pageData.id
	        }, function (data) {
	            pageData.caseList.totalCount = data.totalcount;
	            pageData.caseList.data = data.list;
	            if (pageData.defList.caseDef.length == 0) pageData.defList.caseDef = data.list.slice(0, 2);
	        },
            function () {
            });
	    }

	    pageFunc.loadQuestionData = function () {
	        commArticle.getArticleListByOption({
	            cateId: baseData.moduleCateIds.askquestion,
	            pageIndex: pageData.questionList.pageIndex,
	            pageSize: pageData.questionList.pageSize,
	            type: 'Question',
	            author: pageData.id
	        }, function (data) {
	            pageData.questionList.totalCount = data.totalcount;
	            pageData.questionList.data = data.list;
	            if (pageData.defList.questionDef.length==0) pageData.defList.questionDef = data.list.slice(0, 2);
	        },
            function () {
            });
	    }
	    pageFunc.loadArticleData = function () {
	        commArticle.getArticleListByOption({
	            cateId: baseData.moduleCateIds.askarticle,
	            pageIndex: pageData.articleList.pageIndex,
	            pageSize: pageData.articleList.pageSize,
	            type: 'Article',
	            author: pageData.id
	        }, function (data) {
	            pageData.articleList.totalCount = data.totalcount;
	            pageData.articleList.data = data.list;
	            if (pageData.defList.articleDef.length == 0) pageData.defList.articleDef = data.list.slice(0, 2);
	        },
            function () {
            });
	    }
	    pageFunc.loadAnswerData = function () {
	        commArticle.loadCommentListByOption({
	            pageIndex: pageData.answerList.pageIndex,
	            pageSize: pageData.answerList.pageSize,
	            reviewType: 'Answer',
	            userAutoId: pageData.id
	        }, function (data) {
	            pageData.answerList.totalCount = data.totalcount;
	            pageData.answerList.data = data.list;
	            if (pageData.defList.answerDef.length == 0) pageData.defList.answerDef = data.list.slice(0, 2);
	        },
            function () {
            });
	    }
	    pageFunc.loadFavoriteData = function () {
	        userService.getUserFavoriteListByOption({
	            pageIndex: pageData.favoriteList.pageIndex,
	            pageSize: pageData.favoriteList.pageSize,
	            userAutoId: pageData.id
	        }, function (data) {
	            pageData.favoriteList.totalCount = data.totalcount;
	            pageData.favoriteList.data = data.list;
	            if (pageData.defList.favoriteDef.length == 0) pageData.defList.favoriteDef = data.list.slice(0, 2);
	        },
            function () {
            })
	    }
	    pageFunc.loadStatusesData = function () {
	        commArticle.getArticleListByOption({
	            pageIndex: pageData.statusesList.pageIndex,
	            pageSize: pageData.statusesList.pageSize,
	            type: 'Statuses',
	            author: pageData.id
	        }, function (data) {
	            pageData.statusesList.totalCount = data.totalcount;
	            pageData.statusesList.data = data.list;
	            if (pageData.defList.statusesDef.length == 0) pageData.defList.statusesDef = data.list.slice(0, 2);
	        },
            function () {
            });
	    }
	    pageFunc.loadFansData = function () {
	        userService.getFansUsers({
	            pageIndex: pageData.fansList.pageIndex,
	            pageSize: pageData.fansList.pageSize,
	            userAutoId: pageData.id
	        }, function (data) {
	            pageData.fansList.totalCount = data.totalcount;
	            pageData.fansList.data = data.list;
	        },
            function () {
            });
	    }
	    pageFunc.loadFollowUserData = function () {
	        userService.getFollowUsers({
	            pageIndex: pageData.followUserList.pageIndex,
	            pageSize: pageData.followUserList.pageSize,
	            userAutoId: pageData.id
	        }, function (data) {
	            pageData.followUserList.totalCount = data.totalcount;
	            pageData.followUserList.data = data.list;
	        },
            function () {
            });
	    }

	    pageFunc.submitFollow = function () {
	        userService.followUser(pageData.userInfo.userId, function (data) {
	            if (data.isSuccess) {
	                pageData.userInfo.followUserCount += 1;
	                pageData.userInfo.userIsFollow = true;
	            } else if (data.errcode == 10010) {
	                $scope.showLogin(function () {
	                    pageFunc.submitFollow();
	                }, '您取消了登陆，关注'+pageData.userRole+'必需先登录');
	            } else {
	                alert('关注失败');
	            }
	        }, function (data) {
	            alert('关注失败');
	        });
	    }
	    pageFunc.submitDisFollow = function () {
	        userService.disFollowUser(pageData.userInfo.userId, function (data) {
	            if (data.isSuccess) {
	                pageData.userInfo.followUserCount -= 1;
	                pageData.userInfo.userIsFollow = false;
	            } else if (data.errcode == 10010) {
	                $scope.showLogin(function () {
	                    pageFunc.submitDisFollow();
	                }, '您取消了登陆，取消关注' + pageData.userRole + '必需先登录');
	            } else {
	                alert('取消关注失败');
	            }
	        }, function (data) {
	            alert('取消关注失败');
	        });
	    }
	    pageFunc.submitFollowUser = function (item) {
	        userService.followUser(item.userId, function (data) {
	            if (data.isSuccess) {
	                item.userIsFollow = true;
	            } else if (data.errcode == 10010) {
	                $scope.showLogin(function () {
	                    pageFunc.submitFollowUser(item);
	                }, '您取消了登陆，关注必需先登录');
	            } else {
	                alert('关注失败');
	            }
	        }, function (data) {
	            alert('关注失败');
	        });
	    }
	    pageFunc.submitDisFollowUser = function (item) {
	        userService.disFollowUser(item.userId, function (data) {
	            if (data.isSuccess) {
	                item.userIsFollow = false;
	            } else if (data.errcode == 10010) {
	                $scope.showLogin(function () {
	                    pageFunc.submitDisFollowUser(item);
	                }, '您取消了登陆，取消关注必需先登录');
	            } else {
	                alert('取消关注失败');
	            }
	        }, function (data) {
	            alert('取消关注失败');
	        });
	    }
	    pageFunc.selectTab = function (indexNow) {
	        pageData.defList.check = false;
	        pageData.caseList.check = false;
	        pageData.questionList.check = false;
	        pageData.articleList.check = false;
	        pageData.answerList.check = false;
	        pageData.favoriteList.check = false;
	        pageData.statusesList.check = false;
	        pageData.fansList.check = false;
	        pageData.followUserList.check = false;

	        switch (indexNow) {
	            case 0:
	                pageData.defList.check = true;
	                break;
                case 1:
                    pageData.caseList.check = true;
                    break;
                case 2:
                    pageData.questionList.check = true;
                    break;
                case 3:
                    pageData.articleList.check = true;
                    break;
                case 4:
                    pageData.answerList.check = true;
                    break;
                case 5:
                    pageData.favoriteList.check = true;
                    break;
                case 6:
                    pageData.statusesList.check = true;
                    break;
                case 7:
                    pageData.fansList.check = true;
                    break;
                case 8:
                    pageData.followUserList.check = true;
                    break;
	            default:
	                break;
	        }

	    }
        //弹出邀请框
        pageFunc.shareLinkOpen = function (shareRegLink) {
            var modal = $modal.open({
                animation: true,
                templateUrl: baseViewPath + 'tpls/master/shareRegLinkModal.html',
                // size:'sm',
                controller: function($scope) {
                    var shareLinkFn = $scope.shareLinkFn = {};
                    var shareLinkData = $scope.shareLinkData = {
                        textContent:'易劳(eLao360)系上海易劳商务咨询有限公司旗下的服务平台，是一家致力于服务企业内部HR，为其搭建及时的劳动法资讯、高质量的在线答疑和务实的培训交流等一站式服务平台。',
                        allContent:'',
                        shareRegLink:''
                    };
                    shareLinkData.shareRegLink=shareRegLink;

                    shareLinkFn.showMessage = function () {
                        alert("复制成功");
                    };
                    shareLinkFn.getTextToCopy=function(){
                        shareLinkData.allContent=shareLinkData.textContent+"请在浏览器中打开下面的链接进行注册："+shareLinkData.shareRegLink;
                        return shareLinkData.allContent;
                    }
                }
            });
        }

	    $scope.$watch('pageData.caseList.pageIndex', pageFunc.loadCaseData);
	    $scope.$watch('pageData.questionList.pageIndex', pageFunc.loadQuestionData);
	    $scope.$watch('pageData.articleList.pageIndex', pageFunc.loadArticleData);
	    $scope.$watch('pageData.answerList.pageIndex', pageFunc.loadAnswerData);
	    $scope.$watch('pageData.favoriteList.pageIndex', pageFunc.loadFavoriteData);
	    $scope.$watch('pageData.statusesList.pageIndex', pageFunc.loadStatusesData);
	    $scope.$watch('pageData.fansList.pageIndex', pageFunc.loadFansData);
	    $scope.$watch('pageData.followUserList.pageIndex', pageFunc.loadFollowUserData);

	    pageFunc.init();

	}
]);