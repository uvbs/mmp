ubimodule.directive('ubiheader', ['commService', '$interval', function (commService, $interval) {
	return {
		restrict: 'ECMA',
		templateUrl: baseViewPath + 'tpls/global/header.html',
		replace: true,
		controller: function($scope, $element, $attrs) {
			var ubiheaderCtrl = $scope.ubiheaderCtrl = {};

			ubiheaderCtrl.pageData = {
				isLogin: false,
				user: null,
				loadComplete: false,
				loginId: "",
				loginPwd: "",
				isopen: false
			};
			ubiheaderCtrl.pageFunc = {};

			ubiheaderCtrl.pageFunc.checkLogin = function() {
				commService.checkLogin(function(user) {
					ubiheaderCtrl.pageData.isLogin = user != null;
					ubiheaderCtrl.pageData.user = user;
					ubiheaderCtrl.pageData.loadComplete = true;
				}, function() {
					ubiheaderCtrl.pageData.loadComplete = true;
				});
			};

			$scope.$on('loginStatusChangeNotice', function(event, msg) {
				ubiheaderCtrl.pageFunc.checkLogin();
			});

			$scope.$on('unreadcountChangeNotice', function (event, msg) {
			    //ubiheaderCtrl.pageFunc.checkLogin();
			    commService.getCurrUserInfo(function (data) {
			        if(data)
			        {
			            ubiheaderCtrl.pageData.user = data;
			        }
			    });
			});

			ubiheaderCtrl.pageFunc.updateNoticeCount = function () {
			    $interval(function () {
			        ubiheaderCtrl.pageFunc.checkLogin();
			    }, 3000, 0);
			}
			ubiheaderCtrl.pageFunc.updateNoticeCount();

			ubiheaderCtrl.pageFunc.checkLogin();

			ubiheaderCtrl.pageFunc.login = function () {
				var loginId = $.trim(ubiheaderCtrl.pageData.loginId),
					loginPwd = $.trim(ubiheaderCtrl.pageData.loginPwd);

					if (!loginId) {
						alert('请输入登陆账号',3);
						$('#txtLoginId').focus();
						return;
					};

					if (!loginPwd) {
						alert('请输入登陆密码',3);
						$('#txtLoginPwd').focus();
						return;
					};

				if (ubiheaderCtrl.pageData.loginId) {};
			    commService.login(loginId, loginPwd, "", false, function (data) {
			        if (data.issuccess) {
			        	$scope.$emit('loginStatusChange','loginStatusChange');
			        }else{
			        	alert(data.message,2);
			        }
			    });
			};
			ubiheaderCtrl.pageFunc.logout = function () {
			    if (confirm('确认退出登录?')) {
			        commService.logout(function (data) {
			            if (data.isSuccess) {
			                ubiheaderCtrl.pageFunc.checkLogin();
			                ubiheaderCtrl.pageData.isopen = false;
			            }
			        });
			    }
			};
			ubiheaderCtrl.pageFunc.userDropdown = function () {
			    ubiheaderCtrl.pageData.isopen = !ubiheaderCtrl.pageData.isopen
			};
			ubiheaderCtrl.pageFunc.goPassword = function () {
			    window.location.href = "#/password/" + ubiheaderCtrl.pageData.user.id;
			    ubiheaderCtrl.pageData.isopen = false;
			};
			ubiheaderCtrl.pageFunc.goInformation = function () {
			    window.location.href = "#/information/" + ubiheaderCtrl.pageData.user.id;
			    ubiheaderCtrl.pageData.isopen = false;
			    ubiheaderCtrl.pageData.user.unreadcount = 0;
			};
			ubiheaderCtrl.pageFunc.goUserspace = function () {
			    window.location.href = "#/userspace/" + ubiheaderCtrl.pageData.user.id;
			    ubiheaderCtrl.pageData.isopen = false;
			};
		}
		// compile: function(element, attrs, transclude) {
		// 	return {
		// 		pre: function preLink(scope, element, attrs, controller) {},
		// 		post: function postLink(scope, element, attrs, controller) {}
		// 	};
		// },
		// link: function(scope, element, attrs) {}
	};
}]);

ubimodule.directive('ubifooter', function() {
	return {
		restrict: 'ECMA',
		templateUrl: baseViewPath + 'tpls/global/footer.html',
		replace: true
	};
});

//热门活动
ubimodule.directive('hotactivity', ['commActivity', function(commActivity) {
	return {
		restrict: 'E',
		templateUrl: baseViewPath + 'tpls/global/hotActivity.html',
		replace: true,
		controller: function($scope, $element, $attrs) {
			var hotActivityCtrl = $scope.hotActivityCtrl = {};

			var cacheData = localStorage.getItem(baseData.localStorageKeys.hotActivity);

			hotActivityCtrl.pageData = {
				hotActivityData: null
			};

			if (cacheData && cacheData != 'undefined') {
				hotActivityCtrl.pageData.hotActivityData = localStorage.getItem(JSON.parse(cacheData));
			}

			commActivity.loadHotActivity(function(data) {
				hotActivityCtrl.pageData.hotActivityData = data.list;

				localStorage.setItem(baseData.localStorageKeys.hotActivity, JSON.stringify(data.list));

			}, function(data) {});

		}
	};
}]);

//每日案例
ubimodule.directive('smalldailycase', ['commArticle', function(commArticle) {
	return {
		restrict: 'ECMA',
		templateUrl: baseViewPath + 'tpls/global/smallDailyCase.html',
		replace: true,
		controller: function($scope, $element, $attrs) {
			var smalldailycaseData = $scope.smalldailycaseData = {
				dailyCase: null
			};

			commArticle.getDailyCase(function(data) {
				smalldailycaseData.dailyCase = data;
			}, function(data) {

			});

		}
	};
}]);

//热门标签
ubimodule.directive('smallhottags', ['commArticle', function(commArticle) {
	return {
		restrict: 'ECMA',
		templateUrl: baseViewPath + 'tpls/global/smallHotTags.html',
		replace: true,
		scope:{
		    cateid: '@',
		},
		controller: function($scope, $element, $attrs) {
			var smallhottagsData = $scope.smallhottagsData = {
			    hotTags: [],
                cateid:$scope.cateid,
			};
			var smallhottagsFunc = $scope.smallhottagsFunc = {

			};
			commArticle.getHotTags(function(data) {
				smallhottagsData.hotTags = data;
			}, function(data) {});
		    //点击标签时跳转的页面
			smallhottagsFunc.go = function (tag) {
			    var url = "";
			    switch (smallhottagsData.cateid) {
			        case "79":
			            url = "#/caseTag/" + tag;
			            break;
			        case "83":
			            url = "#/newsTag/" + tag;
			            break;
			        case "84":
			            url = "#/regulationsTag/" + tag;
			            break;
			        case "81":
			            url = "#/askTag/" + tag;
			            break;
			        //case "82":
			        //    url = "#/AskList/" + tag;
			        //    break;
			        default:
			            break;
			    }
			    window.location.href = url;
			}
		}
	};
}]); 

//热门文章
ubimodule.directive('smallhotarticle', ['commArticle', function(commArticle) {
	return {
		restrict: 'ECMA',
		templateUrl: baseViewPath + 'tpls/global/smallHotArticle.html',
		replace: true,
		scope: {
			cateid: '@',
			routename:'@'
		},
		controller: function($scope, $element, $attrs) {
			var pageFunc = $scope.pageFunc = {};
			var pageData = $scope.pageData = {
				hotTags: [],
				cateId: $scope.cateid
			};
			commArticle.getArticleList(function(data) {
				pageData.hotArticle = data.list;
			}, function(data) {}, 'Article', pageData.cateId, '', '', '', 'comment', 1, 5, 0, 1);

			pageFunc.itemClick = function (item) {
				window.location.href = '#/' + $scope.routename + '/' + item.id;
			};
		}
	};
}]);

//文章评论
ubimodule.directive('comment', ['commArticle', 'commService', 'textAngularManager', '$timeout', function(commArticle, commService, textAngularManager, $timeout) {
	return {
		restrict: 'ECMA',
		templateUrl: baseViewPath + 'tpls/global/comment.html',
		replace: true,
		scope: {
			articleid: '@',
			optype:'@'
		},
		controller: function($scope, $element, $attrs) {
			var pageFunc = $scope.pageFunc = {};
			var pageData = $scope.pageData = {
				hotTags: [],
				articleId: $scope.articleid,
				article: null,

				comments: [],
				commentsTotalCount: 0,
				commentsPageIndex: 1,
				commentsPageSize: 5,

				currComment: '',
				commentIncognito: false,

				currUser: commService.getCurrUserInfo(),

				opMsg: '评论',

				currReportContent: '',//当前举报原因
				currCommentReport: '',//当前要举报的评论

				//isShowReport: false,
                //isShowCommentReport:false,
			};

			if ($scope.optype == "1") {
				pageData.opMsg = '回答';
			};

			$scope.showLogin = function(okFn, cancelMsg) {
			    commService.showLoginModal(function (data, user) {
			        //$scope.$emit('loginStatusChange', 'loginStatusChange');
			        $scope.$emit('loginStatusChangeNotice', 'loginStatusChange');
					//$scope.$broadcast('loginStatusChangeNotice', 'loginStatusChange');

					if (data.issuccess) {
						if (okFn) {
							okFn();
						}
					}
				}, function() {
					alert(cancelMsg);
				});
			};

			/**
			 * [submitComment 提交评论内容]
			 * @return {[type]} [description]
			 */
			pageFunc.submitComment = function() {

				//判断当前是否已登陆
				if (!pageData.currUser) {

					$scope.showLogin(function() {
						pageFunc.submitComment();
					}, '您取消了登陆，继续' + pageData.opMsg + '必需先登录');

					return;
				}

				pageData.currComment = $.trim(pageData.currComment);
				if (pageData.currComment == '') {

					var editorScope = textAngularManager.retrieveEditor('warpCommentEditor').scope;
					$timeout(function() {
						editorScope.displayElements.text.trigger('focus');
					});

					return;
				}
				var commentTime = new Date();
				commArticle.commentArticle(function(data) {
				    if (!data.isSuccess) {
				        if (data.errcode == 10010 && pageData.currUser != null)
				        {
				            $scope.showLogin(function () {
				                pageFunc.submitComment();
				            }, '您取消了登陆，继续' + pageData.opMsg + '必需先登录');
				        } else {
				            if (data.errmsg != null) {
				                alert(pageData.opMsg + '失败,' + data.errmsg);
				            }
				            else {
				                alert(pageData.opMsg + '失败');
				            }
				        }   	
					} else {
						alert(pageData.opMsg + '成功');

						pageData.comments.push({
							id: data.returnValue,
							content: pageData.currComment,
							createDate: commentTime,
							replyCount: 0,
							praiseCount: 0,
							pubUser: pageData.currUser
						});

						$scope.$emit('unreadcountChangeNotice', 'unreadcountChange');

						pageData.currComment = '';

						pageData.commentsTotalCount += 1;
					}

				}, function() {
					alert(pageData.opMsg+ '失败');
				}, pageData.articleId, pageData.currComment, pageData.commentIncognito ? 1 : 0, 0);
			};

			/**
			 * [loadCommentList 加载评论列表]
			 * @return {[type]} [description]
			 */
			pageFunc.loadCommentList = function() {
				commArticle.loadCommentList(pageData.articleId, pageData.commentsPageIndex, pageData.commentsPageSize, function(data) {
					if (data.list && data.list.length > 0) {
						commService.pushArrFilterRepeat(data.list, pageData.comments, 'id');
					}
					pageData.commentsTotalCount = data.totalcount;
					pageData.commentsPageIndex++;
				}, function() {});
			};

			/**
			 * [loadReplyList 加载回复列表]
			 * @param  {[type]} comment [description]
			 * @return {[type]}         [description]
			 */
			pageFunc.loadReplyList = function(comment) {
				commArticle.loadReplyList(comment.id, comment.replyIndex, comment.replySize, function(data) {
					comment.replyIndex++;
					comment.replySubmitShow = true;
					comment.subListShow = true;
					if (data.list && data.list.length > 0) {
						for (var i = 0; i < data.list.length; i++) {

							var isHave = false; //过滤跟视图重复的项

							for (var j = 0; j < comment.replyList.length; j++) {
								if (comment.replyList[j].id == data.list[i].id) {
									isHave = true;
									break;
								}
							}

							if (!isHave) {
								comment.replyList.push(data.list[i]);
							}
						}

					}
				});
			};

			/**
			 * [showReplyList 显示回复列表]
			 * @param  {[type]} comment [description]
			 * @return {[type]}         [description]
			 */
			pageFunc.showReplyList = function(comment) {
				if (typeof(comment.replyList) != 'undefined' && comment.replyList.length == 0) {
					pageFunc.loadReplyList(comment);
				} else {
					comment.replySubmitShow = true;
					comment.subListShow = true;
				}
			};
		    /**
			 * [showReportList 显示回复中举报列表]
			 * @param  {[type]} comment [description]
			 * @return {[type]}         [description]
			 */
			pageFunc.showReportList = function (comment) {
			    //if (typeof (comment.replyList) != 'undefined' && comment.replyList.length == 0) {
			    //    pageFunc.loadReplyList(comment);
			    //} else {
			        comment.isShowCommentReport = true;
			        comment.isShowReport = true;
			   // }
			};
		    /**
			 * [showReportList 隐藏回复中举报列表]
			 * @param  {[type]} comment [description]
			 * @return {[type]}         [description]
			 */
			pageFunc.hideReportList = function (comment) {
			    //if (typeof (comment.replyList) != 'undefined' && comment.replyList.length == 0) {
			    //    pageFunc.loadReplyList(comment);
			    //} else {
			        comment.isShowCommentReport = false;
			        comment.isShowReport = false;
			   // }
			};
			/**
			 * [hideReplyList 隐藏回复列表]
			 * @param  {[type]} comment [description]
			 * @return {[type]}         [description]
			 */
			pageFunc.hideReplyList = function(comment) {
				comment.replySubmitShow = false;
				comment.subListShow = false;
			};

			/**
			 * [hideReplySubmit 隐藏回复提交表单]
			 * @param  {[type]} comment [description]
			 * @return {[type]}         [description]
			 */
			pageFunc.hideReplySubmit = function(comment) {
				comment.replySubmitShow = false;
				if (comment.replyCount == 0) {
					comment.subListShow = false;
				}
			};

			/**
			 * [replyComment 回复评论]
			 * @param  {[type]} comment [回复的评论]
			 * @param  {[type]} reply   [回复评论中的回复]
			 * @return {[type]}         [description]
			 */
			pageFunc.replyComment = function(comment, reply) {

				//判断当前是否已登陆
				if (!pageData.currUser) {

					$scope.showLogin(function() {
						pageFunc.replyComment();
					}, '您取消了登陆，继续回复必需先登录');

					return;
				}


				var $txtComment = $('#txtComment_' + comment.id + (reply ? '_' + reply.id : '')),
					$chkComment = $('#chkComment_' + comment.id + (reply ? '_' + reply.id : ''));

				var content = $.trim($txtComment.html());

				if (content == '' || removeHtmlTag(content) == '') {
					$txtComment.html('').focus();
					return;
				}

				var commentTime = new Date();
				commArticle.replyComment(
					comment.id,
					reply ? reply.id : 0,
					content,
					$chkComment.val() ? 1 : 0,
					function (data) {				    
						if (data.isSuccess) {

							var model = {
								id: data.returnValue,
								content: content,
								createDate: commentTime,
								replyCount: 0,
								praiseCount: 0,
								pubUser: pageData.currUser,
								replayToUser: reply ? reply.pubUser : null
							};

							if (!comment.replyList) {
								comment.replyList = [];
							}

							comment.replyList.push(model);

							comment.replyCount += 1;
							$txtComment.html('');

							if (reply) {
								reply.replySubmitShow = false;

							}

						} else {
						    if (data.errmsg != null) {
						        alert('评论失败,' + data.errmsg);
						    }
						    else {
						        alert('评论失败');
						    }
							//alert('评论失败');
						}
					}
				);
			};
            //点赞
			pageFunc.praiseReview = function(model) {
				commArticle.praiseReview(
					model.id,
					function(data) {
						if (data.isSuccess) {
						    model.praiseCount += 1;
						    model.currUserIsPraise = true;
						} else if (data.errcode == 10010) {
							$scope.showLogin(null, '您取消了登陆，继续点赞必须先登录');
						}
					},
					function(data) {

					}
				);
			};
		    //取消赞
			pageFunc.disReviewContent = function (model) {
			    commArticle.disReviewContent(
					model.id,
					function (data) {
					    if (data.isSuccess) {
					        model.praiseCount -= 1;
					        model.currUserIsPraise = false;

					    } else if (data.errcode == 10010) {
					        $scope.showLogin(null, '您取消了登陆，取消赞必须先登录');
					    }
					},
					function (data) {

					}
				);
			};
		    //跳转到个人中心
			pageFunc.goTo = function (item) {
			    if (item.pubUser.userName != "匿名用户") {
			        window.location.href = "#/userspace/" + item.pubUser.id;
			    }
			}
		    //跳转到个人中心
			pageFunc.goToRelpy = function (item) {
			    if (item.replayToUser.userName != "匿名用户") {
			        window.location.href = "#/userspace/" + item.replayToUser.id;
			    }
			}


			pageFunc.reportIllegalReview = function (comment, reply) {
			    //if (reply != "") {

			    //}
			    var $txtComment = $('#txtReport_' + comment.id + (reply ? '_' + reply.id : '')),
					$chkComment = $('#chkReport_' + comment.id + (reply ? '_' + reply.id : ''));

			    var content = $.trim($txtComment.html());
			    //var content = $.trim($('#currReportContent').html());
                var id="";
                if(reply)
                {
                    id=reply.pubUser.id;
                }
                else
                {
                    id=comment.pubUser.id;
                }

				commArticle.reportIllegalReview(
					id,
                    content,
					function(data) {
						if (data.isSuccess) {
							alert('举报成功');
						} else if (data.errcode == 10010) {
							$scope.showLogin(null, '您取消了登陆，继续举报必须先登录');
						} else if (data.errcode == 10013) {
							alert('您已经举报过了');
						}
						pageData.isShowReport = false;
						$('#currReportContent').html("");
					},
					function(data) {

					}
				);
			};

			pageFunc.init = function() {
				$scope.$on('loginStatusChangeNotice', function(event, msg) {
					pageData.currUser = commService.getCurrUserInfo();
				});

				$scope.$on('unreadcountChange', function (event, msg) {
				    pageData.currUser = commService.getCurrUserInfo();
				});
				pageFunc.loadCommentList();
			};

			pageFunc.init();
		}
	};
}]);

//推荐专家
ubimodule.directive('rcmtutor', ['commArticle', 'userService', 'commService', function (commArticle, userService, commService) {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'tpls/global/rcmtutor.html',
        replace: true,
        controller: function($scope, $element, $attrs) {
            var rcmtutorFunc = $scope.rcmtutorFunc = {};
            var rcmtutorData = $scope.rcmtutorData = {
                tutors: []
            };
            
            rcmtutorFunc.loadTutors = function () {
                commArticle.getTutors(1, 3, '', "", "", "", function (data) {
                    rcmtutorData.tutors = data.list;
                }, function () { });
            }

            rcmtutorFunc.submitFollow = function (item) {
                userService.followUser(item.userId, function (data) {
                    if (data.isSuccess) {
                        item.userIsFollow = true;
                        item.followUserCount += 1;
                    } else if (data.errcode == 10010) {
                        $scope.showLogin(function () {
                            rcmtutorFunc.submitFollow(item);
                            rcmtutorFunc.loadTutors();
                        }, '您取消了登陆，关注专家必需先登录');
                    } else {
                        alert('关注失败');
                    }
                }, function (data) {
                    alert('关注失败');
                });
            }
            rcmtutorFunc.submitDisFollow = function (item) {
                userService.disFollowUser(item.userId, function (data) {
                    if (data.isSuccess) {
                        item.userIsFollow = false;
                        item.followUserCount -= 1;
                    } else if (data.errcode == 10010) {
                        $scope.showLogin(function () {
                            rcmtutorFunc.submitDisFollow(item);
                            rcmtutorFunc.loadTutors();
                        }, '您取消了登陆，取消关注专家必需先登录');
                    } else {
                        alert('取消关注失败');
                    }
                }, function (data) {
                    alert('取消关注失败');
                });
            }

            rcmtutorFunc.loadTutors();
        }
    };
}]);

//热门问题
ubimodule.directive('hotquestion', ['commArticle', function (commArticle) {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'tpls/global/hotquestion.html',
        replace: true,
        controller: function ($scope, $element, $attrs) {
            var hotquestionFunc = $scope.hotquestionFunc = {};
            var hotquestionData = $scope.hotquestionData = {
                questions: []
            };

            hotquestionFunc.loadHotQuestions = function () {
                commArticle.getArticleListByOption({
                    cateId: baseData.moduleCateIds.askquestion + ',' + baseData.moduleCateIds.askarticle,
                    isGetNoCommentData: 0,
                    isHasCommentAndReplayCount: 0,
                    pageIndex: 1,
                    pageSize: 10,
                    orderby: "comment"
                }, function (data) {
                    for (var i = 0; i < data.list.length; i++) {
                        data.list[i].hrefType = data.list[i].categoryId == baseData.moduleCateIds.askquestion ? "ask" : "askArticleDetail";
                        hotquestionData.questions.push(data.list[i]);
                    }
                    console.log(hotquestionData.questions);
                }, function (data) {
                });
            };

            hotquestionFunc.loadHotQuestions();
        }
    };
}]);
//感兴趣的人
ubimodule.directive('interesteduser', ['userService', 'commService', function ( userService, commService) {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'tpls/global/interesteduser.html',
        replace: true,
        controller: function ($scope, $element, $attrs) {
            var interesteduserFunc = $scope.interesteduserFunc = {};
            var interesteduserData = $scope.interesteduserData = {
                users: []
            };

            interesteduserFunc.loadInterestedUser = function () {
                userService.getInterestedUser(3, function (data) {
                    interesteduserData.users = data;
                }, function () { });
            }

            interesteduserFunc.submitFollow = function (item) {
                userService.followUser(item.userId, function (data) {
                    if (data.isSuccess) {
                        item.userIsFollow = true;
                    } else if (data.errcode == 10010) {
                        $scope.showLogin(function () {
                            interesteduserFunc.submitFollow(item);
                        }, '您取消了登陆，关注用户必需先登录');
                    } else {
                        alert('关注失败');
                    }
                }, function (data) {
                    alert('关注失败');
                });
            }
            interesteduserFunc.submitDisFollow = function (item) {
                userService.disFollowUser(item.userId, function (data) {
                    if (data.isSuccess) {
                        item.userIsFollow = false;
                    } else if (data.errcode == 10010) {
                        $scope.showLogin(function () {
                            interesteduserFunc.submitDisFollow(item);
                        }, '您取消了登陆，取消关注用户必需先登录');
                    } else {
                        alert('取消关注失败');
                    }
                }, function (data) {
                    alert('取消关注失败');
                });
            }
            interesteduserFunc.loadInterestedUser();
        }
    };
}]);


//公开课排行
ubimodule.directive('smallhotopen', ['commArticle', function (commArticle) {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'tpls/global/smallHotOpen.html',
        replace: true,
        scope: {
            type: '@',
            cateid: '@',
            routename: '@'
        },
        controller: function ($scope, $element, $attrs) {
            var pageHotOpenFunc = $scope.pageHotOpenFunc = {};
            var pageHotOpenData = $scope.pageHotOpenData = {
                type: $scope.type,
                cateId: $scope.cateid,
                openList: []
            };
            commArticle.getArticleListByOption(
                {
                    pageIndex: 1,
                    pageSize: 5,
                    type: pageHotOpenData.type,
                    cateid: pageHotOpenData.cateId,
                    orderby: 'comment'
                },
                function (data) {
                    pageHotOpenData.openList = data.list;
                },
                function () { }
                );

            pageHotOpenFunc.itemClick = function (item) {
                window.location.href = '#/' + $scope.routename + '/' + item.id;
            };
        }
    };
}]);

//感兴趣的人
//ubimodule.directive('uploadfile', ['userService', 'commService', function (userService, commService) {
//    return {
//        restrict: 'ECMA',
//        templateUrl: baseViewPath + 'tpls/global/uploadfile.html',
//        replace: true,
//        scope: {
//            flielist: '='
//        },
//        controller: function ($scope, $element, $attrs, $upload) {
//            var uploadfileFunc = $scope.uploadfileFunc = {};
//            var uploadfileData = $scope.uploadfileData = {
//                file: []
//            };
//            uploadfileFunc.onFileSelect = function ($files) {    //$files: an array of files selected, each file has name, size, and type.
//                uploadfileData.file = $files;
//            };
//            uploadfileFunc.uploadFile = function () {
//                if (uploadfileData.file) {
//                    commService.uploadFile(
//                    uploadfileData.file,
//                    function (evt) {
//                        console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
//                    },
//                    function (data) {
//                        console.log(data);
//                    });
//                }
//            }
//        }
//    };
//}]);

//缩略图
ubimodule.directive('ngThumb', ['$window', function ($window) {
    var helper = {
        support: !!($window.FileReader && $window.CanvasRenderingContext2D),
        isFile: function(item) {
            return angular.isObject(item) && item instanceof $window.File;
        },
        isImage: function(file) {
            var type =  '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    };

    return {
        restrict: 'A',
        template: '<canvas/>',
        link: function(scope, element, attributes) {
            if (!helper.support) return;

            var params = scope.$eval(attributes.ngThumb);

            if (!helper.isFile(params.file)) return;
            if (!helper.isImage(params.file)) return;

            var canvas = element.find('canvas');
            var reader = new FileReader();

            reader.onload = onLoadFile;
            reader.readAsDataURL(params.file);

            function onLoadFile(event) {
                var img = new Image();
                img.onload = onLoadImage;
                img.src = event.target.result;
            }

            function onLoadImage() {
                var width = params.width || this.width / this.height * params.height;
                var height = params.height || this.height / this.width * params.width;
                canvas.attr({ width: width, height: height });
                canvas[0].getContext('2d').drawImage(this, 0, 0, width, height);
            }
        }
    };
}]);







