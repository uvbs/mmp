/* global pureCarModule */
pureCarModule.controller('serviceRecordCtrl', ['$scope', function($scope) {
  var pageFunc = $scope.pageFunc = {
    setTabIndex: function (i) {
      pageData.tabIndex = i;
      if(i === 1) {
        pageFunc.loadOrderList();
      }
    },
    loadOrderList: function () {
      pageData.orderList = [
        {
          id: 123,
          title: '常规服务',
          arrivalTime: '2012-2-2 10:00',
          orderTime: '2012-2-2 10:00',
          orderStatusText: '受理中',
          orderStatus: 0
        }, {
          id: 134,
          title: '常规服务',
          arrivalTime: '2012-2-2 10:00',
          orderTime: '2012-2-2 10:00',
          orderStatusText: '已确认',
          orderStatus: 1
        }, {
          id: 888,
          title: '常规服务',
          arrivalTime: '2012-2-2 10:00',
          orderTime: '2012-2-2 10:00',
          orderStatusText: '已完成',
          orderStatus: 2
        }
      ];
    }
  };
  var pageData = $scope.pageData = {
    title: '养护预约',
    tabIndex: 0,
    orderList: []
  };

  document.title = pageData.title;

}]);
