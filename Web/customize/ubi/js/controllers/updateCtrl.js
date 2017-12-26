ubimodule.controller("updateCtrl", ['$scope', '$routeParams', 'commArticle', 'commService', '$sce', '$location', '$anchorScroll',
	function ($scope, $routeParams, commArticle, commService, $sce, $location, $anchorScroll) {

	    var pageFunc = $scope.pageFunc = {};
	    var pageData = $scope.pageData = {
	        title: '社区 - ' + baseData.slogan,
	        pageIndex: 1,
	        pageSize: 20,
	        totalCount: 0,
	        cateList: [],
	        currSelectCate: {
	            id: ''
	        },
	        articleList: [],
	        currPostUpdate: '',
	        currUser: commService.getCurrUserInfo(),
	        currTab: {
	            id: 0
	        },
	        tabs: [{
	            id: 0,
	            title: '最新动态'
	        }, {
	            id: 1,
	            title: '热门动态'
	        }, {
	            id: 2,
	            title: '成员列表'
	        }],

	        leftHotArticle: [],

	        leftMemberData: [],

	        currReportContent: '',//当前举报原因
	        currCommentReport: '',//当前要举报的评论
	    };

	    document.title = pageData.title;

	    pageFunc.init = function () {
	        pageFunc.loadCateData();
	        $scope.$on('loginStatusChangeNotice', function (event, msg) {
	            pageData.currUser = commService.getCurrUserInfo();
	        });

	    };

	    /**
		 * [loadCateData 加载分类列表数据]
		 * @return {[type]} [description]
		 */
	    pageFunc.loadCateData = function () {

	        commArticle.getArticleCateList('Community', null, null, null, function (data) {
	            //console.log(data);
	            if (data && data.list) {
	                var isHave = false;
	                for (var i = 0; i < data.list.length; i++) {
	                    pageData.cateList.push(data.list[i]);
	                    if ($routeParams.cateId && $routeParams.cateId == data.list[i].id) {
	                        pageFunc.selectCate(data.list[i]);
	                        isHave = true;
	                    }
	                }
	                if (!isHave) pageFunc.selectCate(pageData.cateList[0]);
	            }

	        }, function () { });

	    };

	    pageFunc.loadArticleList = function (isNew) {
	        if (isNew) pageFunc.initPageCountData();

	        commArticle.getArticleList(function (data) {
	            pageFunc.setArticleDaraToCurr(isNew, data);
	        }, function () { }, 'Statuses', pageData.currSelectCate.id, '', '', '', '', pageData.pageIndex, pageData.pageSize, 0);
	    };

	    pageFunc.loadHotArticleList = function (isNew) {
	        if (isNew) pageFunc.initPageCountData();

	        commArticle.getArticleList(function (data) {
	            pageFunc.setArticleDaraToCurr(isNew, data);
	        }, function () { }, 'Statuses', pageData.currSelectCate.id, '', '', '', 'comment', pageData.pageIndex, pageData.pageSize, 0, 1);
	    };

	    pageFunc.initPageCountData = function () {
	        pageData.pageIndex = 1;
	        pageData.totalCount = 0;
	        pageData.articleList = [];
	    };

	    pageFunc.loadLeftData = function () {
	        pageData.leftHotArticle = null;
	        //左侧热门讨论加载
	        commArticle.getArticleList(function (data) {
	            pageData.leftHotArticle = data.list;
	        }, function () { }, 'Statuses', pageData.currSelectCate.id, '', '', '', 'comment', 1, 5, 0, 1);
	    };

	    /**
		 * [setArticleDaraToCurr 数据设置到当前视图]
		 * @param {Boolean} isNew [description]
		 * @param {[type]}  data  [description]
		 */
	    pageFunc.setArticleDaraToCurr = function (isNew, data) {
	        pageData.totalCount = data.totalcount;
	        pageData.pageIndex++;
	        if (data && data.list) {
	            //console.log(data);
	            if (isNew) {
	                pageData.articleList = data.list;
	            } else {
	                commService.pushArrFilterRepeat(data.list, pageData.articleList, 'id');
	            }
	        }
	    };

	    pageFunc.addArticle = function () {

	        var content = $.trim(pageData.currPostUpdate);
	        if (content == '') {
	            alert('发布内容不能为空');
	            $('#txtCurrPostUpdate').focus();
	            return;
	        }

	        var pubDate = new Date();
	        commArticle.addArticle(function (data) {
	            //console.log('addArticle');
	            //console.log(data);
	            if (data.isSuccess) {
	                alert('发布成功');
	                pageData.currPostUpdate = '';

	                if (pageData.currTab.id != 0) {
	                    pageFunc.tabSelect(pageData.tabs[0]);

	                } else {

	                    var model = {
	                        id: data.returnValue,
	                        commentCount: 0,
	                        commentIndex: 1,
	                        commentList: [],
	                        commentSize: 5,
	                        commentSubmitShow: false,
	                        content: $sce.trustAsHtml(content),
	                        pubUser: pageData.currUser,
	                        title: '',
	                        praiseCount: 0
	                    };

	                    pageData.articleList.unshift(model);
	                }
	            } else if (data.errcode == 10010) {
	                $scope.showLogin(function () {
	                    pageFunc.addArticle();
	                }, '您取消了登陆，继续发动态必需先登录');
	            } else if (data.errcode == 10017) {
	                alert('关注当前社区成为社区成员后，才可以发动态');
	            } else {

	                if (data.errmsg != null) {
	                    alert('评论失败,' + data.errmsg);
	                }
	                else {
	                    alert('评论失败');
	                }
	                //alert('评论失败');
	            }
	        }, function () {
	            alert('网络错误');
	        }, 'Statuses', content, pageData.currSelectCate.id, content);

	    };

	    /**
		 * [selectCate 选择列表]
		 * @param  {[type]} item [description]
		 * @return {[type]}      [description]
		 */
	    pageFunc.selectCate = function (item) {
	        pageData.currTab = pageData.tabs[0];
	        pageData.currSelectCate = item;
	        pageFunc.loadLeftData();
	        pageFunc.loadArticleList(true);
	        pageFunc.getHotMembers(item);
	    };

	    pageFunc.selectShowMembers = function () {
	        alert('selectShowMembers');
	    };

	    /**
		 * [showReplyList 显示回复列表]
		 * @param  {[type]} article [description]
		 * @return {[type]}         [description]
		 */
	    pageFunc.showCommentList = function (article) {
	        if (typeof (article.commentList) != 'undefined' && article.commentList.length == 0) {
	            pageFunc.loadCommentList(article);
	        } else {
	            article.commentSubmitShow = true;
	            article.subListShow = true;
	        }
	    };

	    /**
		 * [showReportList 显示举报列表]
		 * @param  {[type]} article [description]
		 * @return {[type]}         [description]
		 */
	    pageFunc.showReportList = function (article) {
	        //if (typeof (article.commentList) != 'undefined' && article.commentList.length == 0) {
	        //    pageFunc.loadCommentList(article);
	        //} else {
	        //article.commentSubmitShow = true;
	        //article.subListShow = true;
	        article.isShowCommentReport = true;
	        article.isShowReport = true;

	        // }
	    };

	    /**
            * [hideReportList 隐藏回复中举报列表]
            * @param  {[type]} comment [description]
            * @return {[type]}         [description]
            */
	    pageFunc.hideReportList = function (article) {
	        article.isShowCommentReport = false;
	        article.isShowReport = false;
	    };


	    /**
		 * [hideReplyList 隐藏回复列表]
		 * @param  {[type]} comment [description]
		 * @return {[type]}         [description]
		 */
	    pageFunc.hideCommentList = function (article, reply) {
	        article.commentSubmitShow = false;
	        article.subListShow = false;
	    };

	    pageFunc.loadCommentList = function (article) {
	        commArticle.loadCommentList(article.id, article.commentIndex, article.commentSize, function (data) {
	            //console.log(data);

	            commService.pushArrFilterRepeat(data.list, article.commentList, 'id');
	            article.commentIndex += 1;
	            article.commentSubmitShow = true;
	            article.subListShow = true;
	        }, function () { });
	    };

	    /**
		 * [submitComment 提交评论内容]
		 * @return {[type]} [description]
		 */
	    pageFunc.submitComment = function (article, reply) {

	        //判断当前是否已登陆
	        if (!pageData.currUser) {

	            $scope.showLogin(function () {
	                pageFunc.submitComment(article, reply);
	            }, '您取消了登陆，继续评论必需先登录');

	            return;
	        }


	        var $txtComment = $('#txtComment_' + article.id + (reply ? '_' + reply.id : '')),
				$chkComment = $('#chkComment_' + article.id + (reply ? '_' + reply.id : ''));

	        var content = $.trim($txtComment.html());

	        var replyId = reply ? reply.id : 0;

	        var opMsg = replyId > 0 ? '回复' : '评论';

	        if (content == '' || removeHtmlTag(content) == '') {
	            $txtComment.html('').focus();
	            return;
	        }


	        var commentTime = new Date();

            if(pageData.currSelectCate.userIsFollow==true)
            {
                commArticle.commentArticle(function (data) {
                    console.log(data);
                    if (!data.isSuccess) {
                        if (data.errmsg != null) {
                            alert(opMsg + '失败,' + data.errmsg);
                        }
                        else {
                            alert(opMsg + '失败');
                        }
                        //alert(opMsg + '失败');
                    } else {
                        alert(opMsg + '成功');

                        var model = {
                            id: data.returnValue,
                            content: content,
                            createDate: commentTime,
                            replyCount: 0,
                            praiseCount: 0,
                            pubUser: pageData.currUser,
                            replayToUser: reply ? reply.pubUser : null
                        };

                        if (!article.commentList) {
                            article.commentList = [];
                        }

                        article.commentList.push(model);

                        article.commentCount += 1;
                        $txtComment.html('');

                        if (reply) {
                            reply.replySubmitShow = false;

                        }

                    }

                    //跳到指定锚点
                    // $location.hash('more_btn');
                    // $anchorScroll();
                }, function () {
                    alert(opMsg + '失败');
                }, article.id, content, $chkComment.val() ? 1 : 0, replyId);
            }
            else
            {
                alert("关注当前社区成为社区成员后，才可以"+opMsg+"！");
            }

	    };

	    /**
		 * [showReplyList 显示回复列表]
		 * @param  {[type]} comment [description]
		 * @return {[type]}         [description]
		 */
	    pageFunc.showReplyList = function (comment) {

	        comment.replySubmitShow = true;
	        comment.subListShow = true;
	    };

	    /**
		 * [hideReplyList 隐藏回复列表]
		 * @param  {[type]} comment [description]
		 * @return {[type]}         [description]
		 */
	    pageFunc.hideReplyList = function (comment) {
	        comment.replySubmitShow = false;
	        comment.subListShow = false;
	    };

	    pageFunc.praiseReview = function (model) {
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
	    };

	    pageFunc.reportIllegalReview = function (comment, reply) {

	        var $txtComment = $('#txtReport_' + comment.id + (reply ? '_' + reply.id : '')),
					$chkComment = $('#chkReport_' + comment.id + (reply ? '_' + reply.id : ''));

	        var content = $.trim($txtComment.html());
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
				function (data) {
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
				function (data) {

				}
			);
	    };
        //点赞
	    pageFunc.praiseContent = function (model) {
	        commArticle.praiseContent(
				function (data) {
				    if (data.isSuccess) {
				        model.praiseCount += 1;
				        model.currUserIsPraise = 1;
				    } else if (data.errcode == 10010) {
				        $scope.showLogin(null, '您取消了登陆，继续点赞必须先登录');
				    }
				},
				function (data) {

				},
				model.id
			);
	    };
	    //取消点赞
	    pageFunc.disPraiseContent = function (model) {
	        commArticle.disPraiseContent(
				function (data) {
				    if (data.isSuccess) {
				        model.praiseCount -= 1;
				        model.currUserIsPraise = 0;
				    } else if (data.errcode == 10010) {
				        $scope.showLogin(null, '您取消了登陆，取消赞必须先登录');
				    }
				},
				function (data) {

				},
				model.id
			);
	    };

	    pageFunc.reportIllegalContent = function (model) {
	        commArticle.reportIllegalContent(
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

				},
				model.id
			);
	    };

	    pageFunc.favoriteArticle = function (model) {
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
	    };

	    pageFunc.tabSelect = function (item) {
	        pageData.currTab = item;
	        if (item.id == 0) {
	            pageFunc.loadArticleList(true);
	        } else if (item.id == 1) {
	            pageFunc.loadHotArticleList(true);
	        } else {
	            pageFunc.loadMembers(pageData.currSelectCate);
	        }

	    };

	    pageFunc.showMoreHot = function () {
	        pageData.currTab = pageData.tabs[1];

	        commService.pageScorllTo('warpUpdateDataPannel');
	        pageFunc.loadHotArticleList(true);
	    };

	    var followArticleCategoryLock = false;
	    /**
		 * [followArticleCategory 关注社区]
		 * @param  {[type]} item [description]
		 * @return {[type]}      [description]
		 */
	    pageFunc.followArticleCategory = function (item) {
	        if (followArticleCategoryLock) {
	            return;
	        };
	        followArticleCategoryLock = true;
	        commArticle.followArticleCategory(function (data) {
	            followArticleCategoryLock = false;
	            if (data.isSuccess) {
	                item.followUserCount += 1;
	                item.userIsFollow = true;
                    pageData.currSelectCate.userIsFollow=true;
	            } else if (data.errcode == 10010) {
	                $scope.showLogin(function () {
	                    pageFunc.followArticleCategory(item);
	                }, '您取消了登陆，关注社区必须先登录');
	            } else {
	                alert('关注失败');
	            }

	        }, function (data) {
	            followArticleCategoryLock = false;
	            alert('关注失败');
	        }, item.id);
	    };

	    var disFollowArticleCategory = false;
	    /**
		 * [disFollowArticleCategory 取消关注社区]
		 * @param  {[type]} item [description]
		 * @return {[type]}      [description]
		 */
	    pageFunc.disFollowArticleCategory = function (item) {
	        if (disFollowArticleCategory) {
	            return;
	        };
	        disFollowArticleCategory = true;
	        commArticle.disFollowArticleCategory(function (data) {
	            disFollowArticleCategory = false;
	            if (data.isSuccess) {
	                item.followUserCount -= 1;
	                item.userIsFollow = false;
                    pageData.currSelectCate.userIsFollow=false;
	            } else {
	                alert('取消关注失败');
	            }
	        }, function (data) {
	            disFollowArticleCategory = false;
	        }, item.id);
	    };

	    pageFunc.getFollowArticleCategoryUser = function () {

	    };

	    pageFunc.getHotMembers = function (item) {

	        if (!item.hotMembers) {
	            item.hotMembers = [];
	        };

	        if (item.hotMembers.length < 20) {
	            commArticle.getFollowArticleCategoryUser(function (data) {
	                console.log(data);
	                item.hotMembers = data.list;
	            }, function (data) {

	            }, item.id, 1, 20, '');
	        }
	    };

	    pageFunc.showMoreMember = function () {
	        pageData.currTab = pageData.tabs[2];

	        commService.pageScorllTo('warpUpdateDataPannel');
	        pageFunc.loadMembers(pageData.currSelectCate);
	    };

	    pageFunc.loadMembers = function (item) {
	        if (!item.members) {
	            item.members = [];
	        };

	        if (!item.memberIndex) {
	            item.memberIndex = 1;
	        };

	        if (!item.memberTotalCount) {
	            item.memberTotalCount = 0;
	        };

	        commArticle.getFollowArticleCategoryUser(function (data) {
	            console.log(data);
	            commService.pushArrFilterRepeat(data.list, item.members, 'userId');
	            item.memberTotalCount = data.totalcount;
	            item.memberIndex++;
	        }, function (data) {

	        }, item.id, item.memberIndex, 48, '');
	    };

	    pageFunc.init();

	}
]);