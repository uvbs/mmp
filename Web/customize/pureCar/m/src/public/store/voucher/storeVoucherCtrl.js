pureCarModule.controller('storeVoucherCtrl', ['$scope', '$location', '$routeParams', '$interval', function($scope, $location, $routeParams, $interval) {
  var pageFunc = $scope.pageFunc = {};
  var pageData = $scope.pageData = {
    title: '部落购车',
  };

  document.title = pageData.title;
  
  pageFunc.navigateTo = function (path) {
    $location.url(path);
  };

  $scope.quotation = {};
  pageFunc.init = function() {
    getReturnObj('car/quotationInfo/detail.ashx', {quotationId: $routeParams.id}).then(function (quotation) {
      $scope.quotation = quotation;
      $scope.$digest();
    });
  };
  pageFunc.apply = function () {
    window.location = ('http://purecar.comeoncloud.net/' + $scope.quotation.activityId.toString(16) + '/details.chtml')
  }

  $interval(function () {
    $scope.startTime = new Date;
  }, 1000);

  pageFunc.init();

}]);