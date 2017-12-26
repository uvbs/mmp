pureCarModule.controller('serviceRatingsCtrl', ['$scope', function($scope) {
  var pageData = $scope.pageData = {
    title: '我的点评'
  };
  
  document.title = pageData.title;
}]);