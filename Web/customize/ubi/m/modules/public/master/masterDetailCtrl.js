ubimodule.controller('masterDetailCtrl', ['$scope', '$routeParams', 'userService','$location', function($scope, $routeParams, userService, $location) {

	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title: baseData.slogan,
		userId: $routeParams.id,
		userInfo: null,
		pathCode: base64encode('#' + $location.path()),
        userIntro:false,
	};

	pageFunc.loadData = function() {
		userService.getUserInfo(pageData.userId, function(data) {
		    console.log(data);
		    if (data)
		    {
		        if (data.userIntroduction!="")
		        {
		            pageData.userIntro = true;
		        }
		    }
			if (data.errcode) {
				alert('找不到该用户');
			} else {
				pageData.userInfo = data;
				if (data.isCurrUser) {
					userRole = "个人";
					pageData.title = '个人空间 - ' + baseData.slogan;
				} else if (data.isTutor) {
					userRole = "专家";
					pageData.title = '专家空间 - ' + baseData.slogan;
				} else {
					userRole = "用户";
					pageData.title = '用户空间 - ' + baseData.slogan;
				}
				document.title = pageData.title;
			}
		});
	};


	pageFunc.submitFollowUser = function(item) {
		userService.followUser(item.userId, function(data) {
			if (data.isSuccess) {
				item.userIsFollow = true;
				alert('关注成功');
			} else if (data.errcode == 10010) {
				$scope.go('#login/'+ pageData.pathCode);
			} else {
				alert('关注失败');
			}
		}, function(data) {
			alert('关注失败');
		});
	};

	pageFunc.submitDisFollowUser = function(item) {
		userService.disFollowUser(item.userId, function(data) {
			if (data.isSuccess) {
				item.userIsFollow = false;
			} else if (data.errcode == 10010) {
				$scope.go('#login/'+ pageData.pathCode);
			} else {
				alert('取消关注失败');
			}
		}, function(data) {
			alert('取消关注失败');
		});
	};
	pageFunc.goMyFollow = function () {
	    var url = '';
	    url = "#/myFollow/" + pageData.userInfo.id;
	        window.location.href = url;
	}
	pageFunc.goMyFans = function () {
	    var url = '';
	    url = "#/myFans/" + pageData.userInfo.id;
	    window.location.href = url;
	}

	pageFunc.loadData();

}]);