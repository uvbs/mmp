ubimodule.controller("activityDetailCtrl", ['$scope', '$routeParams', 'commActivity', '$sce', 'commArticle', 'textAngularManager', '$timeout', 'commService', '$rootScope',
	function($scope, $routeParams, commActivity, $sce, commArticle, textAngularManager, $timeout, commService, $rootScope) {

		var pageFunc = $scope.pageFunc = {};

		var pageData = $scope.pageData = {
			title: '活动详情 - ' + baseData.slogan,
			aid: $routeParams.aid,
			activity: null,
			currComment: '',
			commentIncognito: 0,

			comments: [],
			commentsPageIndex: 1,
			commentsPageSize: 10,
			commentsTotalCount: 0,

			signupIsShow: false,
			signupList: [],
			signupPageIndex: 1,
			signupPageSize: 10,
			signupTotalCount: 0,

			currUser: commService.getCurrUserInfo(),
			currReportContent: '',//当前举报原因
			currCommentReport: '',//当前要举报的评论
			//isExpand: false,//是否展开
		};

		document.title = pageData.title;

		pageFunc.createSignfield = function() {
			if (pageData.activity.signfield) {
				for (var i = 0; i < pageData.activity.signfield.length; i++) {
					pageData.activity.signfield[i].input = '';
					if (pageData.activity.signfield[i].value == 'Name') {
						pageData.activity.signfield[i].input = pageData.currUser ? pageData.currUser.userName : '';
					}
					if (pageData.activity.signfield[i].value == 'Phone') {
						pageData.activity.signfield[i].input = pageData.currUser ? pageData.currUser.phone : '';
					}
				}
			};
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
				}, '您取消了登陆，继续评论必需先登录');

				return;
			}

			pageData.currComment = $.trim(pageData.currComment);
			if (pageData.currComment == '') {

				var editorScope = textAngularManager.retrieveEditor('editor1').scope;
				$timeout(function() {
					editorScope.displayElements.text.trigger('focus');
				});

				return;
			}
			var commentTime = new Date();
			commArticle.commentArticle(function(data) {
				if (!data.isSuccess) {
					alert('评论失败');
				} else {
					alert('评论成功');

					pageData.comments.push({
						id: data.returnValue,
						content: pageData.currComment,
						createDate: commentTime,
						replyCount: 0,
						praiseCount: 0,
						pubUser: pageData.currUser
					});

					pageData.currComment = '';
					pageData.activity.commentcount += 1;
					pageData.commentsTotalCount += 1;
				}

				//跳到指定锚点
				// $location.hash('more_btn');
				// $anchorScroll();
			}, function() {
				alert('评论失败');
			}, pageData.activity.activityid, pageData.currComment, pageData.commentIncognito ? 1 : 0, 0);
		};

		/**
		 * [loadCommentList 加载评论列表]
		 * @return {[type]} [description]
		 */
		pageFunc.loadCommentList = function() {
			commArticle.loadCommentList(pageData.aid, pageData.commentsPageIndex, pageData.commentsPageSize, function(data) {
				if (data.list && data.list.length > 0) {
					for (var i = 0; i < data.list.length; i++) {

						var isHave = false; //过滤跟视图重复的项

						for (var j = 0; j < pageData.comments.length; j++) {
							if (pageData.comments[j].id == data.list[i].id) {
								isHave = true;
								break;
							}
						}

						if (!isHave) {
							pageData.comments.push(data.list[i]);
						}
					}
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
            comment.isShowCommentReport = true;
            comment.isShowReport = true;
        };
        /**
         * [showReportList 隐藏回复中举报列表]
         * @param  {[type]} comment [description]
         * @return {[type]}         [description]
         */
        pageFunc.hideReportList = function (comment) {
            comment.isShowCommentReport = false;
            comment.isShowReport = false;
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
				function(data) {
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
						alert('评论失败');
					}
				}
			);
		};

	    //点赞
		pageFunc.praiseReview = function (model) {
		    commArticle.praiseReview(
                model.id,
                function (data) {
                    if (data.isSuccess) {
                        model.praiseCount += 1;
                        model.currUserIsPraise = true;
                    } else if (data.errcode == 10010) {
                        $scope.showLogin(null, '您取消了登陆，继续点赞必须先登录');
                    }
                },
                function (data) {

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

		/**
		 * [showSignUp 显示报名表]
		 * @return {[type]} [description]
		 */
		pageFunc.showSignUp = function() {

			//判断当前是否已登陆
			if (!pageData.currUser) {

				$scope.showLogin(function() {
					pageFunc.showSignUp();
				}, '您取消了登陆，继续报名必需先登录');

				return;
			}
			pageFunc.createSignfield();
			pageData.signupIsShow = true;
			setTimeout(function() {
				commService.pageScorllTo('warpSignup');
			}, 100);

		};

		pageFunc.submitSignup = function() {


			var reqData = {
				action: 'submitactivitysigndata',
				activityid: pageData.aid,
			};

			var signData = pageData.activity.signfield;
			for (var i = 0; i < signData.length; i++) {
				if (signData[i].input && signData[i].input != '') {
					reqData[signData[i].value] = $.trim(signData[i].input);
				}
			}

			commService.postData(baseData.handlerUrl, reqData, function(data) {
				if (data.errmsg == 'ok') {
					alert('报名成功');
					pageData.signupIsShow = false;

					//增加报名用户到列表
					pageData.signupList.unshift({
						signupTime: commService.timeShow(new Date()),
						name: reqData.Name,
						userId: pageData.currUser.userId,
						headimg: pageData.currUser.avatar
					});

				} else {
					if (data.errcode == -2) {

						$scope.showLogin(function() {
							alert('登录成功，正在重新提交报名信息...', 1, 1200, function() {
								pageFunc.submitSignup();
							});
						}, '您取消了登陆，继续报名必需先登录');

					} else {
						alert(data.errmsg);
					}
				}
			});

		};

		pageFunc.init = function() {
			pageFunc.loadCommentList();

			//加载活动详情
			commActivity.loadActivityDetail(pageData.aid, function(data) {
				console.log(data);
				pageData.activity = data;
				if (pageData.activity.tags)
				{
				    pageData.activity.tags= pageData.activity.tags.split(",");
				}
				pageData.activity.activitycontent = $sce.trustAsHtml(pageData.activity.activitycontent);
				pageFunc.createSignfield();
			});

			//加载报名列表
			commActivity.loadSignupList(pageData.aid, pageData.signupPageIndex, pageData.signupPageSize, function(data) {
				pageData.signupPageIndex++;
				pageData.signupTotalCount = data.totalcount;
				for (var i = 0; i < data.list.length; i++) {
					data.list[i].signupTime = commService.timeShow(data.list[i].signupTime);
					pageData.signupList.push(data.list[i]);
				}
			});

			$scope.$on('loginStatusChangeNotice', function(event, msg) {
				pageData.currUser = commService.getCurrUserInfo();
			});
		};

		pageFunc.go = function (item) {
		    window.location.href = "#/userspace/" + item.id;
		}

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
	    ////点击显示全部
        //pageFunc.showAll = function () {
        //    pageData.isExpand = true;
        //}
        //pageFunc.hidePart = function () {
        //    pageData.isExpand = false;
        //}

		pageFunc.init();

	}
]);