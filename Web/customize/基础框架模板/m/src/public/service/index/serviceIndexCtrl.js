pureCarModule.controller('serviceIndexCtrl', ['$scope', 'ngDialog', function($scope, ngDialog) {
	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title: '我的首页',
		currTabIndex: 0,
		adImgs: [{
			img: '/customize/pureCar/m/static/img/demo/che1.png',
			url: '#/regulations',
			title: '活动标题活动标题1'
		}, {
			img: '/customize/pureCar/m/static/img/demo/che2.png',
			url: '#/news',
			title: '活动标题活动标题2'
		}, {
			img: '/customize/pureCar/m/static/img/demo/che3.png',
			url: '#/news/1',
			title: '活动标题活动标题3'
		}]

	};

	document.title = pageData.title;

	pageFunc.showInsuranceDialog = function() {
		console.log('showInsuranceDialog');
		ngDialog.open({
			template: publicPath + 'service/index/tpls/insuranceDialog.html',
			scope: $scope
		});
	};

	pageFunc.showSelectCarDialog = function () {
		ngDialog.open({
			template: publicPath + 'service/index/tpls/selectCarDialog.html',
			scope: $scope
		});
	};

	pageFunc.click = function() {
		console.log(pageData.test);
		ngDialog.closeAll();
	};

	pageFunc.init = function() {
		console.log(123);
	};

	pageFunc.init();

}]);