comeonModule.controller('signupCtrl', ['$scope', 'commService', function ($scope, commService) {
	var pageData = $scope.pageData = {
	    name: '',
	    phone: '',
	    company: '',
	    opText:'提交报名'
	};
	var pageFunc = $scope.pageFunc = {};


	var lock = false;

	/**
	 * [save 保存]
	 * @return {[type]}          [description]
	 */
	pageFunc.save = function () {
		
	    if (lock) {
	        return;
	    }
	    lock = true;
	    pageData.opText = "正在提交......";

	    if (pageData.name == '') {
	        lock = false;
	        pageData.opText = "提交报名";
	        alert("姓名不能为空");
	        return;
	    }
	    if (pageData.phone == '') {
	        lock = false;
	        pageData.opText = "提交报名";
	        alert("联系电话不能为空");
	        return;
	    }
	    if (pageData.company == '') {
	        lock = false;
	        pageData.opText = "提交报名";
	        alert("公司不能为空");
	        return;
	    }

		var jsonData = {
		    Action: 'submitActivityDataNoLogin',
		    activityid: '506935',
		    Name: pageData.name,
		    Phone: pageData.phone,
		    K2: pageData.company
		};
		
		commService.postData("/Serv/pubapi.ashx", jsonData, function (data) {
		    if (data.errmsg == 'ok') {
		        pageData.opText = "提交完成";
				alert('报名成功');
				
		    } else {
		        pageData.opText = "提交报名";
		        lock = false;
			    alert(data.errmsg);
			}
		},function (data) {
			// body...
		});

	};

	//var i = 1;
	//$(document).swipeUp(function () {
	//    if (i % 2 == 0) {
	//        if (window.location.href.indexOf('Invitation1') > -1) {
	//            window.location.href = '#/Invitation2';
	//        } else if (window.location.href.indexOf('Invitation2') > -1) {
	//            window.location.href = '#/signup';
	//        }
	//    }
	//    i++;
	    
	//});

	//$(document).swipeDown(function () {
	//    window.location.href = '#/Invitation2';
	//});

	wx.ready(function () {
	    wxapi.wxshare({
	        title: '劳动项目对接洽谈会邀请函',
	        desc: '“2015年上海市阳光职业康复援助基地劳动项目洽谈会”即将召开，诚邀您的爱心参加，与165家“阳光基地”负责人洽商劳动项目承接事宜。',
	        imgUrl: "http://huiji.comeoncloud.net/img/yqh123.png"
	    })
	});
	

}]);