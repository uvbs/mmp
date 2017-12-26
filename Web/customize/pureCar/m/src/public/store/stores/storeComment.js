pureCarModule.controller('storeCommentCtrl', ['$scope', '$location', '$routeParams', 'stores', function($scope, $location, $routeParams, stores) {
  var pageFunc = $scope.pageFunc = {
    init: function () {
      var loadingIndex = layer.open({type:2});
      stores.getPromise($routeParams.id).then(function (store) {
        layer.close(loadingIndex);
        pageData.store = store;
        //$scope.$digest();
      });
    }
  };
  var pageData = $scope.pageData = {
  };

  pageFunc.init();
}]);