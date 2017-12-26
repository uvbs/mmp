ubimodule.controller("askCtrl", ['$scope', 'commArticle', 'commService', '$routeParams',
	function ($scope, commArticle, commService, $routeParams) {

		var pageFunc = $scope.pageFunc = {};
		var pageData = $scope.pageData = {
			title: '问答首页 - ' + baseData.slogan,
			questCate: baseData.moduleCateIds.askquestion, //问答-问题
			articleCate: baseData.moduleCateIds.askarticle, //问答-文章

			pageIndex: 1,
			pageSize: 5,
            totalCount:0,

			tabs: [{
				name: '最新动态',
				list: [],
				pageSize: 5,
				pageIndex: 1,
				totalCount: 0,
				checked: true,
				cate: baseData.moduleCateIds.askquestion + ',' + baseData.moduleCateIds.askarticle,
				isGetNoCommentData: 0,
				isHasCommentAndReplayCount: 0,
				orderby: ''
			}, {
				name: '热门动态',
				list: [],
				pageSize: 5,
				pageIndex: 1,
				totalCount: 0,
				checked: false,
				cate: baseData.moduleCateIds.askquestion + ',' + baseData.moduleCateIds.askarticle,
				isGetNoCommentData: 0,
				isHasCommentAndReplayCount: 1,
				orderby: 'comment'
			}, {
				name: '未回答问题',
				list: [],
				pageSize: 5,
				pageIndex: 1,
				totalCount: 0,
				checked: false,
				cate: baseData.moduleCateIds.askquestion,
				isGetNoCommentData: 1,
				isHasCommentAndReplayCount: 0,
				orderby: ''
			}],

			currViewData: {},

			currUser: commService.getCurrUserInfo(),
			currTag: $routeParams.tag,
			currSelectTag: '',
			searchKeyword: '',
			noticeList: [],//存储公告信息
		};

		document.title = pageData.title;

		pageFunc.tabClick = function(item) {
			commService.setArrValue(pageData.tabs, 'checked', false);
			item.checked = true;
			pageData.currViewData = item;
		};

		pageFunc.loadData = function (item) {
		    var tag = '';
		    if (pageData.currSelectTag != '') {
		        tag = pageData.currSelectTag;
		    }
		    if (pageData.currSelectTag == '' && pageData.currTag != '') {
		        tag = pageData.currTag;
		    }
			commArticle.getArticleListByOption({
				pageindex: item.pageIndex,
				pagesize: item.pageSize,				
				cateid: item.cate,
				keyword: pageData.searchKeyword,
				isGetNoCommentData: item.isGetNoCommentData,
				isHasCommentAndReplayCount: item.isHasCommentAndReplayCount,
				orderby: item.orderby,
				tags: tag

			}, function(data) {
				console.log(item.name);
				console.log(data);

				if (data && data.list) {
					for (var i = 0; i < data.list.length; i++) {
						if (data.list[i].categoryId == baseData.moduleCateIds.askquestion) {
							data.list[i].isQuestion = true;
						} else {
							data.list[i].isQuestion = false;
						}
					};

					commService.pushArrFilterRepeat(data.list, item.list, 'id');
					pageData.currTag = '';
				};

				item.totalCount = data.totalcount;
				item.pageIndex++;
			}, function(data) {

			});
		};
	    //获取公告信息
		pageFunc.loadNoticeData = function () {
		    commArticle.getArticleListByOption({
		        pageindex: pageData.pageIndex,
		        pagesize: pageData.pageSize,
		        cateid: baseData.moduleCateIds.asknotice,
		      //  keyword: pageData.searchKeyword,
		        //isGetNoCommentData: item.isGetNoCommentData,
		       // isHasCommentAndReplayCount: item.isHasCommentAndReplayCount,
		      //  orderby: item.orderby,
		       // tags: tag

		    }, function (data) {
		        if (data && data.list) {
		            //for (var i = 0; i < data.list.length; i++) {
		            //    if (data.list[i].categoryId == baseData.moduleCateIds.askquestion) {
		            //        data.list[i].isQuestion = true;
		            //    } else {
		            //        data.list[i].isQuestion = false;
		            //    }
		            //};
                    
		            //  commService.pushArrFilterRepeat(data.list, item.list, 'id');
		            pageData.noticeList = data.list;
		        };

		        pageData.totalCount = data.totalcount;
		        pageData.pageIndex++;
		    }, function (data) {

		    });
		}
		pageData.search=function()
		{
		    if (pageData.tabs[0].pageIndex != 1) {
		        pageData.tabs[0].pageIndex = 1;
		    } else {
		        pageData.tabs[0].list = [];
		        pageFunc.loadData(pageData.tabs[0]);
		    }
		}
		pageFunc.selectTag = function (item) {
		    pageData.tabs[0].pageIndex = 1;
		    pageData.tabs[0].list = [];
		    pageData.currSelectTag = item;
		    pageFunc.loadData(pageData.tabs[0]);
		};

		pageFunc.followArticle = function(item) {
			//判断当前是否已登陆
			if (!pageData.currUser) {

				$scope.showLogin(function() {
					pageFunc.followArticle(item.id);
				}, '您取消了登陆，关注问题必需先登录');

				return;
			}

			commArticle.followArticle(function(data) {
				if (data.isSuccess) {
					item.currUserIsFollow = true;
				};
			}, function(argument) {
				// body...
			}, item.id);
		};

		pageFunc.disFollowArticle = function(item) {
			//判断当前是否已登陆
			if (!pageData.currUser) {

				$scope.showLogin(function() {
					pageFunc.disFollowArticle(item.id);
				}, '');

				return;
			}

			commArticle.disFollowArticle(function(data) {
				if (data.isSuccess) {
					item.currUserIsFollow = false;
				};
			}, function(argument) {
				// body...
			}, item.id);
		};

		pageFunc.gotoDetail = function(item) {
			if (!item.isQuestion) {
				$scope.go('#/askArticleDetail/' + item.id);
			} else {
				$scope.go('#/ask/' + item.id);
			}

		};

		pageFunc.init = function() {

			$scope.$on('loginStatusChangeNotice', function(event, msg) {
				pageData.currUser = commService.getCurrUserInfo();
			});

			for (var i = 0; i < pageData.tabs.length; i++) {
				pageFunc.loadData(pageData.tabs[i]);
			};

			pageData.currViewData = pageData.tabs[0];
			pageFunc.loadNoticeData();
		};

		pageFunc.init();

	}
]);
ubimodule.controller("askArticleDetailCtrl", ['$scope', 'commArticle', '$routeParams','commService',
	function($scope, commArticle, $routeParams,commService) {

		var pageFunc = $scope.pageFunc = {};
		var pageData = $scope.pageData = {
			title: '文章详情 - ' + baseData.slogan,
			currCate: baseData.moduleCateIds.case, //政策法规文章列表
			currArticleId: $routeParams.id,
			isnotice: $routeParams.isnotice,
			article: null,
			currUser: commService.getCurrUserInfo()
		};

		document.title = pageData.title;

		pageFunc.init = function() {
			commArticle.getArticleDetail(function(data) {
				console.log(data);
				pageData.article = data;
			}, function(data) {
				alert('文章新闻失败');
			}, pageData.currArticleId);

			$scope.$on('loginStatusChangeNotice', function(event, msg) {
				pageData.currUser = commService.getCurrUserInfo();
			});

		};

		pageFunc.favoriteArticle = function() {
			commArticle.favoriteArticle(function(data) {
				if (data.isSuccess) {
					pageData.article.currUserIsFavorite = true;
					pageData.article.favoriteCount += 1;
					alert('收藏成功');

				} else if (data.errcode == 10010) {
					$scope.showLogin(null, '您取消了登陆，继续收藏必须先登录');

				} else if (data.errcode == 10013) {
					alert('您已经收藏过了');

				}
			}, function(data) {}, pageData.currArticleId);
		};

		pageFunc.disFavoriteArticle = function() {
			commArticle.disFavoriteArticle(function(data) {
				if (data.isSuccess) {
					pageData.article.currUserIsFavorite = false;
					pageData.article.favoriteCount -= 1;
					alert('取消收藏成功');
				} else {
					alert('取消收藏失败');
				}
			}, function(data) {}, pageData.currArticleId);
		};

		pageFunc.praiseContent = function() {
			commArticle.praiseContent(function(data) {
				if (data.isSuccess) {
					pageData.article.currUserIsPraise = true;
					pageData.article.praiseCount += 1;

				} else if (data.errcode == 10010) {
					$scope.showLogin(null, '您取消了登陆，继续收藏必须先登录');

				} else if (data.errcode == 10013) {

				}
			}, function(data) {}, pageData.currArticleId);
		};

		pageFunc.followArticle = function() {
			//判断当前是否已登陆
			if (!pageData.currUser) {

				$scope.showLogin(function() {
					
				}, '您取消了登陆，关注问题必需先登录');

				return;
			}

			commArticle.followArticle(function(data) {
				if (data.isSuccess) {
					pageData.article.currUserIsFollow = true;
					pageData.article.followCount += 1;
				};
			}, function(argument) {
				// body...
			}, pageData.currArticleId);
		};

		pageFunc.disFollowArticle = function() {
			
			commArticle.disFollowArticle(function(data) {
				if (data.isSuccess) {
					pageData.article.currUserIsFollow = false;
					pageData.article.followCount -= 1;
				};
			}, function(argument) {
				// body...
			},  pageData.currArticleId);
		};

		pageFunc.init();

	}
]);
ubimodule.controller("askAddCtrl", ['$scope', 'commArticle', '$routeParams', 'commService', '$timeout', 'userService',
	function($scope, commArticle, $routeParams,commService,$timeout,userService) {

		var pageFunc = $scope.pageFunc = {};
		var pageData = $scope.pageData = {
			title: '问答发布 - ' + baseData.slogan,
			questCate: baseData.moduleCateIds.askquestion, //问答-问题
			articleCate: baseData.moduleCateIds.askarticle, //问答-文章
			type: $routeParams.type,
			article: null,
			currUser: commService.getCurrUserInfo(),
			tagList: [],
			ueditorConfig :{
			    //这里可以选择自己需要的工具按钮名称,此处仅选择如下五个
			    toolbars: [['fullscreen', 'undo', 'redo', 'indent', 'Bold', 'italic', 'fontsize'
                    , 'underline', 'strikethrough', 'spechars', 'link', 'unlink', 'justifyleft'
                    , 'justifyright', 'justifycenter', 'justifyjustify', 'autotypeset'
                    , 'simpleupload', 'insertimage']],
			    //
			    configPath: 'ubiConfig.json',
			    //focus时自动清空初始化时的内容
			    autoClearinitialContent: true,
			    //关闭字数统计
			    wordCount:false,
			    //关闭elementPath
			    elementPathEnabled:false
			},
			tagOtherList: [],
			provinceList: [],
			cityList: [],
			users: {
			    keyword: "",
			    pageIndex: 1,
			    pageSize: 6,
			    totalCount: 0,
			    list: [],
                dataView:[]
			},
			receivers: [],
            showUsers:false,
			currSelectProvince:'0',
			currselectCity: '0',           
		};

		document.title = pageData.title;

		pageFunc.init = function() {
			
			$scope.$on('loginStatusChangeNotice', function(event, msg) {
				pageData.currUser = commService.getCurrUserInfo();
			});

			//获取省份
			commArticle.getGetKeyVauleDatas('province', null, "1", function(data) {
				if (data && data.list) {
					pageData.provinceList = data.list;
				}
			}, function() {});

			//获取标签
			commArticle.getTags(function(data) {
				if (data) {
					for (var i = 0; i < data.length; i++) {
						data[i].checked = false;
						if (i < 6) {
							pageData.tagList.push(data[i]);
						} else {
							pageData.tagOtherList.push(data[i]);
						}
					}
				}
				pageData.currSelectTag = pageData.tagList[0];
			}, function(argument) {
				// body...
			});

		};

		pageFunc.selectProvince = function () {
			//获取城市
			commService.getGetKeyVauleDatas({
				type:'city',
				prekey:pageData.currSelectProvince
			},function (data) {
				if (data && data.list) {
					pageData.cityList = data.list;
					pageData.currselectCity = '0';
				}
			},function (data) {});
		};

		pageFunc.selectTag = function(item) {

			if (!item.checked) {
				var count = commService.getArrCount(
					pageData.tagList.concat(pageData.tagOtherList),
					'checked',
					true
				);
				if (count >= 5) {
					alert('最多只能选择5个标签');
					return;
				};
			}

			item.checked = !item.checked;

		};

		pageFunc.getCanInvitUsers = function () {

		    var nPageCount = Math.ceil(pageData.users.list.length / pageData.users.pageSize);
		    if (nPageCount < pageData.users.pageIndex) {
		        userService.getCanInvitUsers({
		            pageIndex: pageData.users.pageIndex,
		            pageSize: pageData.users.pageSize,
		            keyword: pageData.users.keyword,
		        },
                function (data) {
                    pageData.users.totalCount = data.totalcount;
                    for (var i = 0; i < data.list.length; i++) {
                        var isInvit = false;
                        for (var j = 0; j < pageData.receivers.length; j++) {
                            if (data.list[i].userId == pageData.receivers[j].userId) {
                                isInvit = true;
                                break;
                            }
                        }
                        data.list[i].Invit = isInvit;
                        pageData.users.list.push(data.list[i]);
                    }
                    pageData.users.dataView = pageData.users.list.slice((pageData.users.pageIndex - 1) * pageData.users.pageSize, pageData.users.pageIndex * pageData.users.pageSize);
                },
                function () {
                });
		    }
		    else {
		        pageData.users.dataView = pageData.users.list.slice((pageData.users.pageIndex - 1) * pageData.users.pageSize, pageData.users.pageIndex * pageData.users.pageSize);
		    }
		}
		pageFunc.ShowUsers = function () {
		    commService.checkLogin(function (data) {
		        if (!data) {
		            $scope.showLogin(function () {
		                pageFunc.ShowUsers();
		            }, '您取消了登陆，邀请回答必须先登录');
		        }
		        else {
		            pageData.showUsers = !pageData.showUsers;
		            if (pageData.users.totalCount == 0) {
		                pageData.users.pageIndex = 1;
		                pageFunc.getCanInvitUsers();
		            }
		        }
		    });
		}
		pageFunc.searchData = function () {
		    pageData.users.pageIndex = 1;
		    pageData.users.list = [];
		    pageData.users.dataView = [];
		    pageFunc.getCanInvitUsers();
		}
		pageFunc.searchLeft = function () {
            if (pageData.users.pageIndex > 1) {
		        pageData.users.pageIndex -= 1;
		        pageFunc.getCanInvitUsers();
		    }
		}
		pageFunc.searchNext = function () {
		    console.info(pageData.users.pageIndex)
		    console.info(Math.ceil(pageData.users.totalCount / pageData.users.pageSize))
		    if (pageData.users.pageIndex < Math.ceil(pageData.users.totalCount / pageData.users.pageSize)) {
		        pageData.users.pageIndex += 1;
		        pageFunc.getCanInvitUsers();
		    }
		}

		pageFunc.submit = function () {

			if (!pageData.currUser) {

				$scope.showLogin(function() {
					pageFunc.submit();
				}, '您取消了登陆，发布内容必需先登录');

				return;
			}

			pageData.article.content = $.trim(pageData.article.content);
			if (pageData.article.content == '') {

				var editorScope = textAngularManager.retrieveEditor('editor1').scope;
				$timeout(function() {
					editorScope.displayElements.text.trigger('focus');
				});

				return;
			}

			var selectTag = [];

			var tagList = pageData.tagList.concat(pageData.tagOtherList);

			for (var i = 0; i < tagList.length; i++) {
				if(tagList[i].checked)
					selectTag.push(tagList[i].tag);
			};
			var selectReceivers = [];
			for (var i = 0; i < pageData.receivers.length; i++) {
			    selectReceivers.push(pageData.receivers[i].userId);
			}

			var reqData = {
				action:'addArticle',
				type: pageData.type == 'question' ? 'Question' : 'Article',
				cateId:pageData.type == 'question'? pageData.questCate:pageData.articleCate,
				province:pageData.currSelectProvince,
				//city:pageData.currselectCity,
				tag:selectTag.join(','),
				title:pageData.article.title,
				content: pageData.article.content,
				receivers: selectReceivers.join(',')
			};

			commService.postData(baseData.handlerUrl, reqData, function (data) {
				if (data.isSuccess) {
					alert('发布成功');
				    //pageFunc.reset();
					pageFunc.go();
				}
				else {
				    if (data.errcode && data.errcode == 10010) {
				        $scope.showLogin(function () {
				            pageFunc.submit();
				        }, '您取消了登陆，发布内容必需先登录');
				    }
				    else {
				        alert('发布失败，'+data.errmsg);
				    }
				}				
			}, function (data) {
				// body...
			});
		};
	    //跳转到详情
		pageFunc.go = function () {
		    var url = "#/ask";
		    window.location.href = url;
		}

		pageFunc.Invit = function (item) {
		    pageData.receivers.push(item);
		    item.Invit = true;
		}
		pageFunc.noInvit = function (item) {
		    for (var i = 0; i < pageData.receivers.length; i++) {
		        if (pageData.receivers[i].userId == item.userId) {
		             pageData.receivers.splice(i, 1);
		        }
		    }
		    item.Invit = false;
		    console.info(pageData.receivers);
		}
		pageFunc.selectType = function (type) {
		    pageData.type = type;
		    pageFunc.reset();
		}
		pageFunc.reset = function () {
			for (var i = 0; i < pageData.tagList.length; i++) {
				pageData.tagList[i].checked = false;
			};
			for (var i = 0; i < pageData.tagOtherList.length; i++) {
				pageData.tagOtherList[i].checked = false;
			};

			pageData.currselectCity = '0';
			pageData.currSelectProvince = '0';

			pageData.article = {};

			pageData.showUsers = false;
			pageData.users.list = [];
			pageData.users.dataView = [];
			pageData.users.pageIndex = 1;
			pageData.users.totalCount = 0;
			pageData.receivers = [];
		}


		pageFunc.init();

	}
]);