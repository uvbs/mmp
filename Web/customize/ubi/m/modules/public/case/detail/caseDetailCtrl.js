ubimodule.controller('caseDetailCtrl', ['$scope', function ($scope) {

    var pageData = $scope.pageData = {
        title: '案例详情',
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