/* global pureCarModule */
pureCarModule.controller('storeOrdersCtrl', ['$scope', '$location', function($scope, $location) {
	var pageFunc = $scope.pageFunc = {
    loadList: function (pageIndex) {
      getReturnObj('car/quotationInfo/list.ashx', {
        pageSize: 10000,
        pageIndex: pageIndex
      }).then(function (result) {
        pageData.orderList = result.list;
        $scope.$digest();
      });
    },
    gotoVoucher: function (order) {
      if (order.status != 0) {
        $location.url('/store/voucher/' + order.quotationId);
      }
    },
    onCanceled: function (order) {
      alert('取消成功');
      pageData.orderList.forEach(function (o) {
        if(o.quotationId === order.quotationId) {
          o.status = 3;
        }
      })
    }
  };
	var pageData = $scope.pageData = {
		title: '我的报价单',
    orderList: []
	};

	document.title = pageData.title;
  
  pageFunc.loadList(1);



}]);
