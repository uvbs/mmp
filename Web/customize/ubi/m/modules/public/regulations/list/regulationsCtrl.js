ubimodule.controller('regulationsCtrl', ['$scope', '$routeParams', function ($scope, $routeParams) {

    var pageData = $scope.pageData = {
        title: '政策法规',
        tag:$routeParams.tag,
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