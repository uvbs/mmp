pureCarModule.controller('storeListCtrl', ['$scope', '$location', function($scope, $location) {
  var pageFunc = $scope.pageFunc = {
    load: function () {
      getReturnObj('user/saller/list.ashx', {
        pageIndex: 1,
        pageSize: 10000,
        keyword: pageData.keyword,
      }).then(function(result){
        pageData.totalCount = result.totalCount;
        pageData.list = result.list;
        $scope.$digest();
      });
    },
    inputKeypress: function ($event) {
      if($event.which === 13) {
        pageFunc.load();
      }
    },
    gotoDetail: function (store) {
      $location.url('/store/stores/' + store.id);
    },
    comment: function () {
      $location.url('/store/stores/' + store.id + '/comment');
    }
  };
  var pageData = $scope.pageData = {};

  pageFunc.load();
}]);