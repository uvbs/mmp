ubimodule.controller("caseCtrl", ['$scope', 'commArticle', '$routeParams', function ($scope, commArticle, $routeParams) {

	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title: '案例分析 - ' + baseData.slogan,
		articles: [],
		pageIndex: 1,
		pageSize: 8,
		totalCount: 0,
		keyword: '',
		currCate: baseData.moduleCateIds.case,
		provinceList: [{
			id: '',
			name: '全部'
		}],
		provinceOtherList: [],
		currSelectProvince: null,
		tagList: [{
			tag: '全部'
		}],
		tagOtherList: [],
		currSelectTag: null,
		currTag: $routeParams.tag,
	};

	document.title = pageData.title;

	pageFunc.loadData = function() {
		// commArticle.getArticleList(function(data) {
		// 	console.log(data);
		// 	pageData.articles = data.list;
		// 	pageData.totalCount = data.totalcount;
		// }, function(data) {

		// }, 'Article', pageData.currCate, pageData.keyword, '', '', '', pageData.pageIndex, pageData.pageSize, 0, 0);
		var tagFilter = pageData.currSelectTag ? pageData.currSelectTag.tag : '';
		if (tagFilter == '全部') {
			tagFilter = '';
		};
		if (pageData.currTag != '')
		{
		    tagFilter = pageData.currTag;
		}
		commArticle.getArticleListByOption({
			cateId: pageData.currCate,
			keyword: pageData.keyword,
			tags: tagFilter,
			province: pageData.currSelectProvince ? pageData.currSelectProvince.id : '',
			orderby: 'CreateDate desc',
			isGetNoCommentData: 0,
			isHasCommentAndReplayCount: 0,
			pageIndex: pageData.pageIndex,
			pageSize: pageData.pageSize,
			type: 'Article'
		}, function(data) {
			console.log(data);
			pageData.articles = data.list;
			pageData.totalCount = data.totalcount;
			pageData.currTag = '';
		}, function(data) {

		});
	};
	pageFunc.search = function() {
		if (pageData.pageIndex != 1) {
			pageData.pageIndex = 1;
		} else {
			pageFunc.loadData();
		}
	};

	pageFunc.selectProvince = function(item) {
		pageData.pageIndex = 1;
		pageData.currSelectProvince = item;
		pageFunc.loadData();
	};
    //0为点击文章中的标签
	pageFunc.selectTag = function(item,type) {
	    pageData.pageIndex = 1;
	    if (type == 0)
	    {
	        pageData.currSelectTag.tag = item;
	    }
	    else if (type == 1)
	    {
	        pageData.currSelectTag = item;
	    }
		pageFunc.loadData();
	};

	pageFunc.init = function() {
		//获取省份
		commArticle.getGetKeyVauleDatas('province', null, "1", function(data) {
			if (data && data.list) {
				for (var i = 0; i < data.list.length; i++) {
					if (i < 6) {
						pageData.provinceList.push(data.list[i]);
					} else {
						pageData.provinceOtherList.push(data.list[i]);
					}

				}
			}
			console.log(pageData.provinceList);
			pageData.currSelectProvince = pageData.provinceList[0];

		}, function() {});

		//获取标签
		commArticle.getTags(function(data) {
			if (data) {
				for (var i = 0; i < data.length; i++) {
					if (i < 6) {
						pageData.tagList.push(data[i]);
					} else {
						pageData.tagOtherList.push(data[i]);
					}
				}
			}
			pageData.currSelectTag = pageData.tagList[0];
		}, function(argument) {
		});

		$scope.$watch('pageData.pageIndex', pageFunc.loadData);

	};
	pageFunc.alert = function () {
	    alert("该功能正在完善中。。。");
	}
	pageFunc.init();

}]);
ubimodule.controller("caseDetailCtrl", ['$scope', 'commArticle', '$routeParams',
	function ($scope, commArticle, $routeParams) {

		var pageFunc = $scope.pageFunc = {};
		var pageData = $scope.pageData = {
			title: '案例分析详情 - ' + baseData.slogan,
			currCate: baseData.moduleCateIds.case, //政策法规文章列表
			currArticleId: $routeParams.id,
			article: null
		};
		document.title = pageData.title;
		pageFunc.init = function() {
			commArticle.getArticleDetail(function(data) {
				pageData.article = data;
			}, function(data) {
				alert('文章新闻失败');
			}, pageData.currArticleId);
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
		pageFunc.alert = function () {
		    alert("该功能正在完善中。。。");
		}
		pageFunc.init();

	}
]);
ubimodule.controller("addCaseCtrl", ['$scope', 'commArticle', 'commService', 'textAngularManager', '$timeout',
	function($scope, commArticle, commService, textAngularManager, $timeout) {

		var pageFunc = $scope.pageFunc = {};
		var pageData = $scope.pageData = {
			title: '案例发布 - ' + baseData.slogan,
			currCate: baseData.moduleCateIds.case, //政策法规文章列表
			article: {
				title:'',
				content: ''
			},
			currUser: commService.getCurrUserInfo(),

			ueditorConfig: {
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
			    wordCount: false,
			    //关闭elementPath
			    elementPathEnabled: false
			},
			tagList: [],
			tagOtherList: [],

			provinceList: [],
			cityList: [],

			currSelectProvince:'0',
			currselectCity:'0'

		};

		document.title = pageData.title;

		pageFunc.init = function() {
			$scope.$on('loginStatusChangeNotice', function(event, msg) {
				pageData.currUser = commService.getCurrUserInfo();
			});

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

			//获取省份
			commArticle.getGetKeyVauleDatas('province', null, "1", function(data) {
				if (data && data.list) {
					pageData.provinceList = data.list;
				}

			}, function() {});


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


		pageFunc.addCase = function() {


			//判断当前是否已登陆
			if (!pageData.currUser) {

				$scope.showLogin(function() {
					pageFunc.addCase();
				}, '您取消了登陆，发布案例必需先登录');

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

			var reqData = {
				action: 'addArticle',
				type: 'Article',
				content: pageData.article.content,
				cateId: pageData.currCate,
				province:pageData.currSelectProvince,
				//city:pageData.currselectCity,
				tag:selectTag.join(','),
				title:pageData.article.title
			};

			commService.postData(baseData.handlerUrl, reqData, function (data) {
				console.log(data);
				if (data.isSuccess) {
					alert('添加成功');
					pageFunc.reset();
				}else{
					alert(data.errmsg);
				}
			}, function (data) {
				// body...
			});

		};

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
		}

		pageFunc.init();

	}
]);