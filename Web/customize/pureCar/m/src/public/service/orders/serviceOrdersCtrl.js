/* global pureCarModule */
pureCarModule.controller('serviceOrdersCtrl', ['$scope', function($scope) {
	var pageFunc = $scope.pageFunc = {
    setListType: function (listTypeIndex) {
      pageData.listTypeIndex = listTypeIndex;
      pageFunc.loadList();
    },
    setStatus: function (statusIndex) {
      pageData.statusIndex = statusIndex;
      pageFunc.loadList();
    },
    loadList: function () {
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
		title: '我的订单',
    listTypeTabs: [ '预约订单' ],//[ '预约订单', '商城订单'],
    statusTabs: ['全部', '已点评', '未点评'],
    listTypeIndex: 0,
    statusIndex: 0,
    orderList: []
	};

	document.title = pageData.title;
  
  pageFunc.loadList('yuyue');

}]);
