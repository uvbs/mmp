ubimodule.directive('comment', function () {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'directives/comment/tpls/index.html',
        replace: true,
        scope: {
            article: '=',
            optype: '@',
            pageType: '@',
            modulename: '@' //84:政策法规，83:新闻资讯，79:案例分析.
        },
        controller: function ($scope, $element, $attrs, $routeParams, ngDialog, commArticle, commService) {
            var pageFunc = $scope.pageFunc = {};
            var pageData = $scope.pageData = {
               // article: $scope.article,
                currArticleId: $routeParams.id,
                optype: $scope.optype,
                commentsPageIndex: 1,
                commentsPageSize: 5,
                comments: [],
                commentsTotalCount: 0,


                TotalCount:0,

                replyList: [],
                replyPageIndex: 1,
                replyPageSize: 5,
                replyTotalCount: 0,

                currCommentObj: '',
                currReplyObj: '',

                currComment: '',
                reply: '',
                commentIncognito: false,

                currUser: commService.getCurrUserInfo(),

                opMsg: '评论',
                modulename: $scope.modulename,
                currPath: '',
                isClickPraise: false,

                currReportContent: '',//当前举报原因
                currCommentReport:'',//当前要举报的评论
            };

            if ($scope.optype == "1") {
                pageData.opMsg = '回答';
            };

            $scope.showLogin = function (okFn, cancelMsg) {
                commService.showLoginModal(function (data, user) {
                    $scope.$emit('loginStatusChange', 'loginStatusChange');
                    //$scope.$broadcast('loginStatusChangeNotice', 'loginStatusChange');

                    if (data.issuccess) {
                        if (okFn) {
                            okFn();
                        }
                    }
                }, function () {
                    alert(cancelMsg);
                });
            };

            //加载评论列表
            pageFunc.loadCommentList = function () {
              //  pageData.isClickPraise = false;
                //pageData.currArticleId
                commArticle.loadCommentList(pageData.currArticleId, pageData.commentsPageIndex, pageData.commentsPageSize, function (data) {
                    //  debugger;
                    if (data.list && data.list.length > 0) {
                        pageData.TotalCount = data.totalcount;
                        commService.pushArrFilterRepeat(data.list, pageData.comments, 'id');
                        for (var i = 0; i < data.list.length; i++) {
                            pageFunc.loadReplyList(data.list[i]);
                        }

                    }
                    pageData.commentsTotalCount = data.totalcount;
                    pageData.commentsPageIndex++;

                }, function () { });
            };

            var commentLock = false;
            /**
                        * [submitComment 提交评论内容]
                        * @return {[type]} [description]
                        */
            pageFunc.submitComment = function () {
                if (commentLock == true)
                { return; }
                commentLock = true;

                //判断当前是否已登陆
                if (!pageData.currUser) {

                    $scope.showLogin(function () {
                        pageFunc.submitComment();
                    }, '您取消了登陆，继续' + pageData.opMsg + '必需先登录');

                    return;
                }

                //  pageData.currComment = $.trim(pageData.currComment);
                if (pageData.currComment == '') {

                    var editorScope = textAngularManager.retrieveEditor('warpCommentEditor').scope;
                    $timeout(function () {
                        editorScope.displayElements.text.trigger('focus');
                    });

                    return;
                }
                var commentTime = new Date();
                commArticle.commentArticle(function (data) {
                    commentLock = false;
                    //  debugger;
                    if (!data.isSuccess) {
                        if (data.errmsg != null)
                        {
                            alert(pageData.opMsg + '失败,'+ data.errmsg);
                        }
                        else
                        {
                            alert(pageData.opMsg + '失败');
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

                        pageData.currComment = '';

                        pageData.commentsTotalCount += 1;
                        // pageData.article.commentCount++;
                        pageData.TotalCount++;
                        ngDialog.closeAll();
                    }                   
                }, function () {
                    alert(pageData.opMsg + '失败');
                }, pageData.currArticleId, pageData.currComment, pageData.commentIncognito ? 1 : 0, 0);
            };

            /**
			 * [loadReplyList 加载回复列表]
			 * @param  {[type]} comment [description]
			 * @return {[type]}         [description]
			 */
            pageFunc.loadReplyList = function (comment) {
                commArticle.loadReplyList(comment.id, comment.replyIndex, comment.replySize, function (data) {
                    comment.replyIndex++;
                    if (comment.replyCount == 0) {
                        pageData.replyTotalCount = 0;
                    }
                    else {
                        pageData.replyTotalCount = comment.replyCount;
                    }

                    comment.replySubmitShow = true;
                    comment.subListShow = true;
                    if (data.list && data.list.length > 0) {
                        //replyCount
                        pageData.replyTotalCount = data.totalcount;
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
            pageFunc.showReplyList = function (comment) {
                if (typeof (comment.replyList) != 'undefined' && comment.replyList.length == 0) {
                    pageFunc.loadReplyList(comment);
                } else {
                    comment.replySubmitShow = true;
                    comment.subListShow = true;
                }
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

            /**
			 * [hideReplySubmit 隐藏回复提交表单]
			 * @param  {[type]} comment [description]
			 * @return {[type]}         [description]
			 */
            pageFunc.hideReplySubmit = function (comment) {
                comment.replySubmitShow = false;
                if (comment.replyCount == 0) {
                    comment.subListShow = false;
                }
            };

            var replyLock = false;

            /**
			 * [replyComment 回复评论]
			 * @param  {[type]} comment [回复的评论]
			 * @param  {[type]} reply   [回复评论中的回复]
			 * @return {[type]}         [description]
			 */
            pageFunc.replyComment = function (comment, reply) {

                if (replyLock == true)
                {
                    return;
                }
                replyLock = true;

                //  debugger;
                //判断当前是否已登陆
                if (!pageData.currUser) {

                    $scope.showLogin(function () {
                        pageFunc.replyComment();
                    }, '您取消了登陆，继续回复必需先登录');

                    return;
                }


                //var $txtComment = $('#txtComment_' + comment.id + (reply ? '_' + reply.id : '')),
                //	$chkComment = $('#chkComment_' + comment.id + (reply ? '_' + reply.id : ''));

                //var content = $.trim($txtComment.html());

                //if (content == '' || removeHtmlTag(content) == '') {
                //    $txtComment.html('').focus();
                //    return;
                //}
                content = pageData.reply;

                var commentTime = new Date();
                commArticle.replyComment(
					comment.id,
					reply ? reply.id : 0,
					content,
                    1,
					//$chkComment.val() ? 1 : 0,
					function (data) {
					    replyLock = false;
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
					        // $txtComment.html('');

					        if (reply) {
					            reply.replySubmitShow = false;

					        }
					        pageData.reply = "";				        
					        ngDialog.closeAll();
					    } else {
					        if (data.errmsg != null) {
					            alert(pageData.opMsg + '失败,' + data.errmsg);
					        }
					        else {
					            alert(pageData.opMsg + '失败');
					        }
					       // alert('评论失败');
					    }
					}
				);
            };
            //提交回复中的回复
            pageFunc.submitReply = function () {
                pageFunc.replyComment(pageData.currCommentObj, pageData.currReplyObj);
            };

            //评论点赞功能
            pageFunc.praiseReview = function (model) {
                pageData.isClickPraise = false;
                if (!pageData.currUser) {
                    // $scope.go('#/login/' + pageData.currPath);
                    pageFunc.go(pageData.currPath);
                } else {
                    commArticle.praiseReview(
                   model.id,
                   function (data) {
                       //  debugger;
                       if (data.isSuccess) {
                           model.praiseCount += 1;
                           model.currUserIsPraise = true;
                           pageData.isClickPraise = true;
                       } else if (data.errcode == 10010) {
                           $scope.showLogin(null, '您取消了登陆，继续点赞必须先登录');
                       }
                   },
                   function (data) {

                   }
               );
                }
               
            };
            //评论点赞取消功能
            pageFunc.disReviewContent = function (model) {
                pageData.isClickPraise = false;
                if (!pageData.currUser) {
                    // $scope.go('#/login/' + pageData.currPath);
                    pageFunc.go(pageData.currPath);
                } else {
                    commArticle.disReviewContent(
                   model.id,
                   function (data) {
                       //  debugger;
                       if (data.isSuccess) {
                           model.praiseCount -= 1;
                           model.currUserIsPraise = false;
                           pageData.isClickPraise = true;
                       } else if (data.errcode == 10010) {
                           $scope.showLogin(null, '您取消了登陆，继续点赞必须先登录');
                       }
                   },
                   function (data) {

                   }
               );
                }

            };

            //举报功能
            pageFunc.reportIllegalReview = function (report) {
                if (!pageData.currUser) {
                    pageFunc.go(pageData.currPath);
                } else {
                    commArticle.reportIllegalReview(
					pageData.currCommentReport.id,
                    pageData.currReportContent,
					function (data) {
					    if (data.isSuccess) {
					        // debugger;
					        alert('举报成功');
					    } else if (data.errcode == 10010) {
					        $scope.showLogin(null, '您取消了登陆，继续举报必须先登录');
					    } else if (data.errcode == 10013) {
					        alert('您已经举报过了');
					    }
					    pageData.currReportContent = "";
					    ngDialog.closeAll();
					},
					function (data) {

					}
				);
                }
                
            };
            ////提交举报原因
            //pageFunc.submitReport = function () {

            //}

            /**
			 * [showCommentDialog 显示评论框]
			 * @param  {[type]} article [description]
			 * @param  {[type]} comment [description]
			 * @param  {[type]} reply   [description]
			 * @return {[type]}         [description]
			 */
            pageFunc.showReplyDialog = function (comment, reply) {
                pageData.currCommentObj = comment;
                pageData.currReplyObj = reply;
                if (!pageData.currUser) {
                    // $scope.go('#/login/' + pageData.currPath);
                    pageFunc.go(pageData.currPath);
                } else {
                    ngDialog.open({
                        template: basePath + 'modules/directives/comment/tpls/replyDialog.html',
                        plain: false,
                        scope: $scope
                    });
                }                
            };

            /**
			 * [showCommentDialog 举报框]
			 * @param  {[type]} article [description]
			 * @param  {[type]} comment [description]
			 * @param  {[type]} reply   [description]
			 * @return {[type]}         [description]
			 */
            pageFunc.showReportDialog = function (item) {
                pageData.currCommentReport = item;
                if (!pageData.currUser) {
                    pageFunc.go(pageData.currPath);
                } else {
                    ngDialog.open({
                        template: basePath + 'modules/directives/comment/tpls/reportDialog.html',
                        plain: false,
                        scope: $scope
                    });
                }
            };

            //最底端评论框
            pageFunc.showDialog = function () {
                if (!pageData.currUser) {
                    // $scope.go('#/login/' + pageData.currPath);
                    pageFunc.go(pageData.currPath);
                } else {
                    ngDialog.open({
                        template: basePath + 'modules/directives/comment/tpls/commentDialog.html',
                        plain: false,
                        scope: $scope
                        //,
                        //controller: ['$scope', function ($scope) {
                        //    var pageData = $scope.pageData = {};
                        //    var pageFunc = $scope.pageFunc = {};
                        //    pageFunc.submitComment = function () {
                        //        console.log(123)
                        //        $scope.$emit('submitComment', pageData.currComment, pageData.commentIncognito);
                        //    };
                        //}]
                    });
                }
                
            };

            ////提交状态更改事件
            //$scope.$on('submitComment', function (event, currComment, commentIncognito) {
            //    console.log(123);
            //    pageData.currComment = currComment;
            //    pageData.commentIncognito = commentIncognito;
            //    pageFunc.submitComment();
            //});

            pageFunc.goback = function () {
                history.go(-1);
            }

            pageFunc.init = function () {
               // pageData.article = $scope.article;
                pageFunc.loadCommentList();
                pageFunc.loadData();
            }
            //获取新闻详情
            pageFunc.loadData = function () {
                commArticle.getArticleDetail(function (data) {                   
                    pageFunc.currPath(data.categoryId);
                }, function (data) {
                    alert('获取文章新闻失败');
                }, pageData.currArticleId);
            }

            //判断URL名称
            pageFunc.currPath = function (id) {
                if (id == '84')
                {
                    pageData.currPath = base64encode('#/regulations/' + $routeParams.id);
                }
                else if (id == '83')
                {
                    pageData.currPath = base64encode('#/news/' + $routeParams.id);
                }
                else if (id == '79')
                {
                    pageData.currPath = base64encode('#/case/' + $routeParams.id);
                }
            }
            //页面跳转函数
            pageFunc.go = function (currPath) {
                // debugger;
               // var url = "#/" + pageListData.modulename + "/" + id;
                var url = "#/login/" + currPath;
              //  '#/login/' + pageData.currPath
                window.location.href = url;
            }
            //跳转到个人中心
            pageFunc.goTo=function(item)
            {
                if (item.pubUser.userName != "匿名用户")
                {
                    window.location.href = "#/master/"+item.pubUser.id;
                }
            }
            pageFunc.init();
        }
    };
});
