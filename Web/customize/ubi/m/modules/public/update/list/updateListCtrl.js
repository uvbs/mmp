ubimodule.controller('updateListCtrl', ['$scope', '$routeParams', 'commArticle', 'commService', '$sce', '$location', '$anchorScroll', 'ngDialog',
    function ($scope, $routeParams, commArticle, commService, $sce, $location, $anchorScroll, ngDialog) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '易劳社区',//'我的积分 - ' + baseData.slogan,
            currTabIndex: 0,
            outParams:$routeParams.outParams,
            isShowChoose:true,

            lastestPageIndex: 1,
            hotPageIndex: 1,
            memberPageIndex: 1,
            lastestPageSize: 8,
            hotPageSize: 8,
            memberPageSize: 2,
            lastestTotalCount: 0,
            hotTotalCount: 0,
            memberTotalCount: 0,

            dataList: [],//存放所有社区，将值传到弹出框显示
            cateList: [],//存放社区
            leftHotArticle: [],//存放热门讨论
            leftMemberData: [], //存放成员列表
            articleList: [],//存放最新动态
            selected: [],//存放所选择的社区
            currComment: '',//当前评论
            talkContent:'',//当前所发的动态
            currObj: [], //所要评论动态对象
            currUser: commService.getCurrUserInfo(),
            currPath: base64encode('#/update/' + $routeParams.id),
            talkPath: base64encode('#/update/')
        };

        document.title = pageData.title;
        /**
             * [loadCateData 加载分类列表数据]
             * @return {[type]} [description]
             */
        pageFunc.loadCateData = function () {
            var outParamArray=[];
            var cateId="";
            if(pageData.outParams!=undefined)
            {
               outParamArray=pageData.outParams.split("_");
                cateId=outParamArray[0];
            }
            else
            {
                cateId=null;
            }

            if(outParamArray.length>1||pageData.outParams==undefined)
            {
                commArticle.getArticleCateList('Community', cateId, null, null, function (data) {
                    console.log(data);
                    if (data && data.list) {
                        pageData.dataList = data.list;
                        var isHave = false;//判断是否有选社区，没有默认选择第一个社区
                        for (var i = 0; i < data.list.length; i++) {
                            pageData.cateList.push(data.list[i]);
                            if ($routeParams.cateId && $routeParams.cateId == data.list[i].id) {
                                pageFunc.selectCommunity(data.list[i]);
                                isHave = true;
                            }
                        }
                        if (!isHave) {
                            pageFunc.selectCommunity(pageData.cateList[0]);
                            pageData.selected = pageData.cateList;
                        }
                    }
                }, function () { });
            }
            else
            {
                commArticle.getArticleCateList('Community', cateId, null, null, function (data) {
                    console.log(data);
                    if (data && data.list) {
                        pageData.dataList = data.list;
                        pageFunc.selectCommunity(data.list[0]);
                        //var isHave = false;//判断是否有选社区，没有默认选择第一个社区
                        //for (var i = 0; i < data.list.length; i++) {
                        //    pageData.cateList.push(data.list[i]);
                        //    if ($routeParams.cateId && $routeParams.cateId == data.list[i].id) {
                        //        pageFunc.selectCommunity(data.list[i]);
                        //        isHave = true;
                        //    }
                        //}
                        //if (!isHave) {
                        //    pageFunc.selectCommunity(pageData.cateList[0]);
                        //    pageData.selected = pageData.cateList;
                        //}
                    }
                }, function () { });
            }

        };
        //根据所选社区加载该社区下面的数据
        pageFunc.selectCommunity = function (item) {
            pageData.cateList = item;//把所选社区对象寄存在cateList里面
            pageData.selected = item;
            pageFunc.loadLatestData(true);
            pageFunc.loadHotDisData(true);
            pageFunc.getHotMembers(true, item);
        };
        //根据页签获取相应数据
        pageFunc.tabSelect = function (item) {
            pageData.currTabIndex = item;
            if (item == 0) {
                pageFunc.loadLatestData(true);
            } else if (item == 1) {
                pageFunc.loadHotDisData(true);
            } else {
                pageFunc.getHotMembers(true, pageData.cateList);
            }

        };
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
        pageFunc.loadLatestData = function (isNew) {
            if (isNew) {
                pageData.lastestPageIndex = 1;
                pageData.lastestTotalCount = 0;
                pageData.articleList = [];
            }
            else {
                pageData.lastestPageIndex++;
            }

            commArticle.getArticleList(function (data) {
                pageData.lastestTotalCount = data.totalcount;
                if (data && data.list) {
                    if (isNew) {
                        pageData.articleList = data.list;
                    } else {
                        for (var i = 0; i < data.list.length; i++) {
                            pageData.articleList.push(data.list[i]);
                        }
                    }
                }
            }, function () { }, 'Statuses', pageData.cateList.id, '', '', '', '', pageData.lastestPageIndex, pageData.lastestPageSize, 0, 1);
        };
        //初始化和加载时 --热门讨论
        pageFunc.loadHotDisData = function (isNew) {
            if (isNew) {
                pageData.hotPageIndex = 1;
                pageData.hotTotalCount = 0;
                pageData.leftHotArticle = [];
            }
            else {
                pageData.hotPageIndex++;
            }
            //热门讨论加载
            commArticle.getArticleList(function (data) {
                pageData.hotTotalCount = data.totalcount;
                //pageData.hotPageIndex++;
                if (data && data.list) {
                    if (isNew) {
                        pageData.leftHotArticle = data.list;
                    } else {
                        // commService.pushArrFilterRepeat(data.list, pageData.leftHotArticle, 'id');
                        for (var i = 0; i < data.list.length; i++) {
                            pageData.leftHotArticle.push(data.list[i]);
                        }
                    }
                }
            }, function () { }, 'Statuses', pageData.cateList.id, '', '', '', 'comment', pageData.hotPageIndex, pageData.hotPageSize, 0, 1);
        };
        //初始化和加载时 --成员列表
        pageFunc.getHotMembers = function (isNew, item) {
            if (isNew) {
                pageData.memberPageIndex = 1;
                pageData.memberTotalCount = 0;
                pageData.leftMemberData = [];
            }
            else {
                pageData.memberPageIndex++;
            }

            commArticle.getFollowArticleCategoryUser(function (data) {
                pageData.memberTotalCount = data.totalcount;
                if (data && data.list) {
                    if (isNew) {
                        pageData.leftMemberData = data.list;
                    } else {
                        for (var i = 0; i < data.list.length; i++) {
                            pageData.leftMemberData.push(data.list[i]);
                        }
                    }
                }
            }, function (data) {

            }, item.id, pageData.memberPageIndex, pageData.memberPageSize, '');
        };
        //跳转到详情
        pageFunc.go = function (id) {
            // debugger;
            var url = "#/update/" + id+"/"+pageData.selected.userIsFollow;
            window.location.href = url;
        }
        //社区选择弹框  
        pageFunc.showCommunityDialog = function () {
            ngDialog.open({
                template: basePath + 'modules/public/update/list/tpls/communityChooseDialog.html',
                plain: false,
                scope: $scope
            });
        };
        //显示评论框  item为每条动态对象
        pageFunc.showCommentDialog = function (item) {
            pageData.currObj = item;
            //判断当前是否已登陆
            if (!pageData.currUser) {
                $scope.go('#/login/' + pageData.currPath);
            } else {
                ngDialog.open({
                    template: basePath + 'modules/public/update/list/tpls/updateCommentDialog.html',
                    plain: false,
                    scope: $scope
                });
            }
        };
        var commentLock = false;
        //评论提交
        pageFunc.submitComment = function () {
            if (commentLock == true)
            { return; }
            commentLock = true;

            var commentTime = new Date();
            commArticle.commentArticle(function (data) {
                commentLock = false;
                if (!data.isSuccess) {
                    if (data.errmsg != null) {
                        alert('评论失败,' + data.errmsg);
                    }
                    else {
                        alert('评论失败');
                    }
                  //  alert('评论失败');
                } else {
                    alert('评论成功');
                    pageData.currComment = '';
                    pageData.currObj.commentCount++;
                    ngDialog.closeAll();
                }
            }, function () {
                alert('评论失败');
            }, pageData.currObj.id, pageData.currComment, pageData.commentIncognito ? 1 : 0, 0);
        }
        //社区选择
        pageFunc.communityChoose = function (item) {
            pageData.cateList = item;
            pageFunc.selectCommunity(item);
            ngDialog.closeAll();
        }
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
            //判断当前是否已登陆
            if (!pageData.currUser) {
                $scope.go('#/login/' + pageData.currPath);
            } else {
                commArticle.followArticleCategory(function (data) {
                    followArticleCategoryLock = false;
                    if (data.isSuccess) {
                        item.followUserCount += 1;
                        item.userIsFollow = true;
                    }
                }, function (data) {
                    followArticleCategoryLock = false;
                    alert('关注失败');
                }, item.id);
            }
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
            //判断当前是否已登陆
            if (!pageData.currUser) {
                $scope.go('#/login/' + pageData.currPath);
            } else {
                commArticle.disFollowArticleCategory(function (data) {
                    disFollowArticleCategory = false;
                    if (data.isSuccess) {
                        item.followUserCount -= 1;
                        item.userIsFollow = false;
                    } else {
                        alert('取消关注失败');
                    }
                }, function (data) {
                    disFollowArticleCategory = false;
                }, item.id);
            }
        };
        //发动态
        pageFunc.addTalkDialog = function () {
            ngDialog.open({
                template: basePath + 'modules/public/update/list/tpls/addTalkDialog.html',
                plain: false,
                scope: $scope
            });
        }
        //提交所发动态
        pageFunc.addTalk = function () {
            var content = pageData.talkContent;
            if (content == '') {
                alert('发布内容不能为空');
                $('#txtCurrPostUpdate').focus();
                return;
            }

            var pubDate = new Date();
            commArticle.addArticle(function (data) {
                if (data.isSuccess) {
                    ngDialog.closeAll();
                    alert('发布成功');
                    pageData.talkContent = '';
                    if (pageData.currTabIndex != 0)
                    {
                        pageData.currTabIndex == 0;
                    }
                    pageFunc.tabSelect(0);
                  //  pageFunc.selectCommunity(pageData.cateList);
                    
                } else if (data.errcode == 10010) {
                    //判断当前是否已登陆
                    if (!pageData.currUser) {
                        ngDialog.closeAll();
                        $scope.go('#/login/' + pageData.talkPath);
                    } else {
                        pageFunc.addTalk();
                    }                   
                } else if (data.errcode == 10017) {
                    ngDialog.closeAll();
                    pageData.talkContent = "";
                    alert('关注当前社区成为社区成员后，才可以发动态');
                } else {
                    if (data.errmsg != null) {
                        alert('发动态失败,' + data.errmsg);
                    }
                    else {
                        alert('发动态失败');
                    }

                  //  alert('评论失败');
                    pageData.talkContent = "";
                }
            }, function () {
                ngDialog.closeAll();
                alert('网络错误');
                pageData.talkContent = "";
            }, 'Statuses', content, pageData.selected.id, content);
        }

        pageFunc.init = function () {
            pageFunc.loadCateData();
            $scope.$on('loginStatusChangeNotice', function (event, msg) {
                pageData.currUser = commService.getCurrUserInfo();
            });
        };
        pageFunc.init();

    }]);