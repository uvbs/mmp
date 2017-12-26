ubimodule.controller('openDetailCtrl', ['$scope', '$routeParams', 'commArticle', 'commService', function($scope, $routeParams, commArticle, commService) {
	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title: '公开课',
		article: null,
		articleId: $routeParams.id,
		currUser: null,
		currPath: base64encode('#/open/' + $routeParams.id),
		needBuy: false,
	};

	document.title = pageData.title;

	pageFunc.loadData = function() {
		commArticle.getOpenClassDetail(pageData.articleId, function(data) {
			console.log(data);
			pageData.article = data;

			if (data.needScore > 0 && !pageData.currUser) {
				$scope.go('#/login/' + pageData.currPath);
			}

			if (data.needScore > 0 && !data.isCanView) {
			    pageData.needBuy = true;
			};

		}, function(data) {});
	};


	pageFunc.buyClass = function() {
	    if (confirm('该课程需要' + pageData.article.needScore + '积分，确定购买该课程？')) {
			commArticle.buyClass(pageData.articleId, function(data) {
			    if (data.isSuccess) {
			        pageData.isBuy = true;
					pageData.article.isCanView = true;
					pageData.article.webUrl = data.returnObj.webUrl;
					pageData.article.files = data.returnObj.files;
				} else if (data.errcode == 10010) {
					$scope.go('#/login/' + pageData.currPath);
				} else if (data.errcode == 10024) {
					alert("积分不足");
				} else {
					alert("购买出错");
				}
			}, function(data) {});
		};

	};

	pageFunc.init = function() {
		commService.checkLogin(function(data) {
			pageData.currUser = data;
			pageFunc.loadData();
		});

	};

	pageFunc.init();

}]);