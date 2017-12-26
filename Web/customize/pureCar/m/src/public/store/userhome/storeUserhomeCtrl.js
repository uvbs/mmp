pureCarModule.controller('storeUserhomeCtrl', ['$scope', function($scope) {
  var pageData = $scope.pageData = {
    title: '个人中心'
  };
  
  document.title = pageData.title;
}]);