/* global pureCarModule */
pureCarModule.controller('serviceActivityCtrl', ['$scope', function($scope) {
  var pageFunc = $scope.pageFunc = {
  };
  var pageData = $scope.pageData = {
    title: '我的活动'
  };

  document.title = pageData.title;

}]);
