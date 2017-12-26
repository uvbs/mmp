ubimodule.controller('myAskCtrl', ['$scope', '$routeParams', function ($scope, $routeParams) {
    var pageData = $scope.pageData = {
        title: '我的提问',
        userid: $routeParams.id,
        type:$routeParams.type,//true表示提问，false表示文章
        tag:$routeParams.tag,
    };
    document.title = pageData.title;

}]);