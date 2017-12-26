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

			currUser: commService.getCurrUserInfo()
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

			//加载活动详情
			commActivity.loadActivityDetail(pageData.aid, function(data) {
				console.log('loadActivityDetail');
				console.log(data);
				
				pageData.activity = data;
				pageData.activity.activitycontent = $sce.trustAsHtml(pageData.activity.activitycontent);
				pageFunc.createSignfield();
			});

			//加载报名列表
			commActivity.loadSignupList(pageData.aid, pageData.signupPageIndex, pageData.signupPageSize, function(data) {
				console.log('loadSignupList');
				console.log(data);

				pageData.signupPageIndex++;
				pageData.signupTotalCount = data.totalcount;
				for (var i = 0; i < data.list.length; i++) {
					data.list[i].signupTime = commService.timeShow(data.list[i].signupTime);
					pageData.signupList.push(data.list[i]);
				}
			});
			
		};

		pageFunc.init();

	}
]);