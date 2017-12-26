ubimodule.controller('newsCtrl', ['$scope', '$routeParams', function ($scope, $routeParams) {
	var pageData = $scope.pageData = {
	    title: '新闻资讯',
	    tag: $routeParams.tag,
	    id: $routeParams.id,//从海马或其他传来的参数
	    routeParam: {
	        hassearch: '1',
	        hasimg: '0',
	        page: '1',
	        otherCateId: "",
	        hasTag: "0",
	        hassummary: '1',
            hasleftslide:'0'
	    } //解析传过来的参数
	};
	var pageFunc = $scope.pageFunc = {};
	document.title = pageData.title;
	pageFunc.init = function () {
	    if (pageData.id != undefined) {
	        var paramArray = pageData.id.split("_");
	        pageData.routeParam.hassearch = paramArray[0];
	        pageData.routeParam.hasimg = paramArray[1];
	        pageData.routeParam.page = paramArray[2];
	        pageData.routeParam.otherCateId = paramArray[3];
	        pageData.routeParam.hasTag = paramArray[4];
	        pageData.routeParam.hassummary = paramArray[5];
            pageData.routeParam.hasleftslide=paramArray[6];
	        if (paramArray[5] == "1") {
	            document.title = "知识库列表";
	        }
	        else {
	            document.title = "在线课堂列表";
	        }
	    }

	};
	pageFunc.init();


}]);