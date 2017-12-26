ubimodule.controller('msgCtrl', ['$scope', '$routeParams', 'userService', 'commService',
	function ($scope, $routeParams, userService, commService) {
		var pageFunc = $scope.pageFunc = {};
		var pageData = $scope.pageData = {
		    title: '消息 - ' + baseData.slogan,
		    id: $routeParams.id,
		    keyword: "",
		    notices: {
		        pageIndex: 1,
		        pageSize: 5,
		        totalCount: 0,
		        list: []
		    },
		    currPath: base64encode('#/msg'),
		    currUser: commService.getCurrUserInfo(),
		};

		document.title = pageData.title;

		pageFunc.loadNotice = function (isLoad) {
		    if (isLoad==true) {
		        pageData.notices.list = [];
		        pageData.notices.pageIndex = 1;
		    }
		    else
		    {
		        pageData.notices.pageIndex++;
		    }

		    userService.getNotices({
		        pageindex: pageData.notices.pageIndex,
		        pagesize: pageData.notices.pageSize,
		        keyword: pageData.keyword,
		    },
            function (data) {
                if (data && data.list) {
                    pageData.notices.totalCount = data.totalcount;
                    for (var i = 0; i < data.list.length; i++) {
                        pageData.notices.list.push(data.list[i]);
                    }
                };
            },
            function () {
            });
		}
	    //#/msg
		pageFunc.init = function () {
		    //判断当前是否已登陆
		    if (!pageData.currUser) {
		        $scope.go('#/login/' + pageData.currPath);
		    } else {
		        pageFunc.loadNotice(true);
		    }  
		}
		pageFunc.init();
	}
]);