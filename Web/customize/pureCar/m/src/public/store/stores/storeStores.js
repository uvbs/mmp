pureCarModule.controller('storeStoresCtrl', ['$scope', '$location', function($scope, $location) {
  var pageFunc = $scope.pageFunc = {
    load: function () {
      getReturnObj('user/saller/list.ashx', {
        pageIndex: 1,
        pageSize: 10000,
      }).then(function(result){
        pageData.totalCount = result.totalCount;
        pageData.list = result.list;
        $scope.$digest();
      });
    }
  };
  var pageData = $scope.pageData = {};

  pageFunc.load();
}]);