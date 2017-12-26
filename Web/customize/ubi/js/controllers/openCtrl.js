ubimodule.controller("openListCtrl", ['$scope', 'commArticle', 'commService', 
	function ($scope, commArticle, commService) {
		var pageFunc = $scope.pageFunc = {};
		var pageData = $scope.pageData = {
			title: "公开课 - " + baseData.slogan,
			tag: [{
				id: '',
				name: "全部"
			}],
			type: "OpenClass",
			cateId: baseData.moduleCateIds.openclass,
			pageIndex: 1,
			pageSize: 9,
			totalCount: 0,
			selectCate: null,
			artitle: null,
			searchKeyword: '',
			notice: ''

		};
		document.title = pageData.title;
		pageFunc.loadCateList = function() {
			commArticle.getArticleCateList(pageData.type, pageData.cateId, 1, 100,
				function(data) {
					for (var i = 0; i < data.list.length; i++) {
						pageData.tag.push({
							id: data.list[i].id,
							name: data.list[i].name
						});
					}
				},
				function(data) {

				});
		}
		pageFunc.selectCateList = function(data) {
			pageData.selectCate = data;
			pageFunc.loadData();
		}
		pageFunc.loadData = function() {

			commArticle.getArticleListByOption({
					pageIndex: pageData.pageIndex,
					pageSize: pageData.pageSize,
					type: pageData.type,
					cateid: pageData.selectCate.id,
					keyword: pageData.searchKeyword
				},
				function(data) {
					pageData.artitle = data.list;
					pageData.totalCount = data.totalcount;
				},
				function() {});

		};
		pageFunc.gotoDetail = function(item) {
			window.location.href = '#/open/' + item.id;
		}
		pageFunc.search = function() {
			pageFunc.loadData();
		}
		pageFunc.getNotice = function() {
			commService.getOpenClassNotice(
				function(data) {
					pageData.notice = data.notice;
				},
				function() {});
		}

		pageFunc.init = function() {
			pageFunc.getNotice();
			pageFunc.loadCateList();
			pageData.selectCate = pageData.tag[0];
		}

		$scope.$watch('pageData.pageIndex', pageFunc.loadData);
		pageFunc.init();

	}
]);

ubimodule.controller('openDetailCtrl', ['$scope', '$routeParams', 'commArticle', 'commService', '$modal', function ($scope, $routeParams, commArticle, commService, $modal) {
	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title: "公开课详情 - " + baseData.slogan,
		articleId: $routeParams.id,
		data: [],
        isExpand:false,//是否展开
	}
	document.title = pageData.title;
	pageFunc.favoriteArticle = function() {
		commArticle.favoriteArticle(
			function(data) {
				if (data.isSuccess) {
					pageData.data.currUserIsFavorite = true;
					alert('收藏成功')
				} else if (data.errcode == 10013) {
					alert("您已收藏过了")
				} else if (data.errcode == 10010) {
					$scope.showLogin(function() {
						pageFunc.favoriteArticle();
					}, '您取消了登陆，继续收藏请登陆')
				}
			},
			function() {},
			pageData.articleId
		)
	};

	pageFunc.disFavoriteArticle = function() {
		commArticle.disFavoriteArticle(function(data) {
			if (data.isSuccess) {
				pageData.data.currUserIsFavorite = false;
				alert('取消收藏成功');
			} else if (data.errcode == 10010) {
				$scope.showLogin(function() {
					pageFunc.disFavoriteArticle();
				}, '您取消了登陆，取消收藏请登陆')
			} else {
				alert('取消收藏失败');
			}
		}, function(data) {}, pageData.articleId);
	};

	pageFunc.loadData = function() {
		commArticle.getOpenClassDetail(pageData.articleId, function(data) {
				pageData.data = data;
			},
			function(data) {});
	};


	pageFunc.getOpenClass = function() {
		commService.getOpenClassWebUrl(pageData.articleId, function(data) {
			if (data.isSuccess) {
				pageData.data.isCanView = true;
				pageData.data.webUrl = data.returnObj.webUrl;
				pageData.data.files = data.returnObj.files;
			} else if (data.errcode == 10010) {
				$scope.showLogin(function() {
					pageFunc.getOpenClass();
				}, '您取消了登陆，继续购买请登陆')
			} else if (data.errcode == 10024) {
				alert(pageFunc.tipsOpen());
			} else {
				alert("购买出错");
			}
		}, function(data) {});

	};

    //提示积分不足
	pageFunc.tipsOpen = function () {
	    var modal = $modal.open({
	        animation: true,
	        templateUrl: baseViewPath + 'tpls/open/tipsModal.html',
           // size:'sm',
	    });
	}

    //点击显示全部
	pageFunc.showAll = function () {
	    pageData.isExpand = true;
	}
	pageFunc.hidePart = function () {
	    pageData.isExpand = false;
	}

	pageFunc.init = function() {
	    pageFunc.loadData();
    };
	pageFunc.init();

}]);