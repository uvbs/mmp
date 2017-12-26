/* global pureCarModule */
pureCarModule.controller('storeOrderDetailCtrl', ['$scope', function($scope) {
  var pageFunc = $scope.pageFunc = {
    loadOrder: function () {
      pageData.order = {
        title: '常规服务',
        shop: '【闸北区】 某某某4S店',
        arrivalTime: '2012-2-2 10:00',
        orderTime: '2012-2-2 10:00',
        orderStatusText: '受理中',
        orderStatus: 0
      };
      pageData.rating = 2;
    }
  };
  var pageData = $scope.pageData = {
    title: '订单详情',
    order: null
  };

  document.title = pageData.title;
  
  pageFunc.loadOrder();

}]);
