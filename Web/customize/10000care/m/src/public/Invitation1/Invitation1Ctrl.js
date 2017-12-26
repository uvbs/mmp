comeonModule.controller('Invitation1Ctrl', ['$scope', 'commService', function ($scope, commService) {
	var pageData = $scope.pageData = {
	    name: '',
	    phone: '',
	    company: '',
	    opText: '动项目对接洽谈会邀请函'
	};
	var pageFunc = $scope.pageFunc = {};


	$(document).swipeUp(function () {
	     window.location.href = '#/Invitation2';
	});

	wx.ready(function () {
	    wxapi.wxshare({
	        title: '动项目对接洽谈会邀请函',
	        desc: '“2015年上海市阳光职业康复援助基地劳动项目洽谈会”即将召开，诚邀您的爱心参加，与165家“阳光基地”负责人洽商劳动项目承接事宜。',
	        imgUrl: "http://huiji.comeoncloud.net/img/yqh123.png",
	        link: window.location.href
	    })
	});

	

}]);