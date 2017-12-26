pureCarModule.controller('storeDetailCtrl', ['$scope', '$location', '$routeParams', 'stores', function($scope, $location, $routeParams, stores) {
  var pageFunc = $scope.pageFunc = {
    init: function () {
      var loadingIndex = layer.open({type:2});
      stores.getPromise($routeParams.id).then(function (store) {
        layer.close(loadingIndex);
        pageData.store = store;
        //$scope.$digest();
      });
    },
    gotoComment: function () {
      $location.url('/store/stores/' + $routeParams.id + '/comment');
    }
  };
  var pageData = $scope.pageData = {
  };

  pageFunc.init();
}]);