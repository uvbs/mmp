ubimodule.controller('userCtrl', ['$scope', 'commArticle', function($scope, commArticle) {
	var pageFunc = $scope.pageFunc = {};
	var pageData = $scope.pageData = {
		title: '个人中心 - ' + baseData.slogan,
		provinceList: [{
			id: '',
			name: '全部'
		}],
		currSelectProvince: null,
	};

	document.title = pageData.title;

}]);
