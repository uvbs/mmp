/* global pureCarModule */
pureCarModule.controller('serviceMessageCtrl', ['$scope', function($scope) {
  var pageFunc = $scope.pageFunc = {
  };
  var pageData = $scope.pageData = {
    title: '我的消息',
  };

  document.title = pageData.title;

}]);
