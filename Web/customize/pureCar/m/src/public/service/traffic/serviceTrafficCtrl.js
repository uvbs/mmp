pureCarModule.controller('serviceTrafficCtrl', ['$scope', function($scope) {
  var pageData = $scope.pageData = {
    title: '车务服务'
  };
  
  document.title = pageData.title;
}]);