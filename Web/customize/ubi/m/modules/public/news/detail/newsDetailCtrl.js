ubimodule.controller('newsDetailCtrl', ['$scope', function ($scope) {

	var pageData = $scope.pageData = {
	    title: '新闻详情',
		//article:{
		//	title:'test',
		//	tags:['aaa','bbb','ccc'],
		//	content:'<p>asdasdassd</p>',
		//	createDate:new Date(),
		//	pubUser:{
		//		userName:'你妹'
		//	}
		//}
	};

	document.title = pageData.title;



}]);