pureCarModule.controller('storeDemandCtrl', ['$scope', '$location', 'ngDialog', 'selectedCarModel','currUserInfo', function($scope, $location, ngDialog, selectedCarModel,currUserInfo) {
  var pageFunc = $scope.pageFunc = {
    selectAddress: function () {
      ngDialog.open({
        template: 'storeDemandSelectAddressDialog',
        scope: $scope,
      });
    },
    confirmAddress: function(district, area) {
      pageData.district = district;
      pageData.area = area;
      ngDialog.closeAll();
    },
    showStores: function () {
      $location.url('/store/stores');
    },
    validate: function () {
      if (!pageData.name) {
        alert('请输入姓名');
        return false;
      }
      if (!pageData.contact) {
        alert('请输入联系方式');
        return false;
      }
      if(!pageData.district || !pageData.area) {
        alert('请选择所在区域');
        return false;
      }
      return true;
    },
    submit: function () {
      if(!pageFunc.validate()) { return; }
      getData('car/quotationInfo/add.ashx', {
        carModelId: pageData.carModel.carModelId,
        carColor: pageData.color,
        buyTime: pageData.time,
        purchaseWay: pageData.method,
        licensePlate: pageData.license,
        carOwnerName: pageData.name,
        carOwnerPhone: pageData.contact,
        city: '上海市',
        district: pageData.district.name,
        area: pageData.area.name,
        preference: pageData.preference
      }).then(function () {
        $scope.successDialog = ngDialog.open({
          template: 'storeDemandSuccessDialog',
          scope: $scope
        });
      })
    },
    closeSuccessDialog: function () {
      if($scope.successDialog) {
        $scope.successDialog.close();
      }
    }
  };
  var pageData = $scope.pageData = {
    title: '购车需求',
    carModel: null,
    time: '三个月后',
    method: '新车全款',
    license: '上沪牌',
    preference: '价格优先'
  };

  document.title = pageData.title;
  
  pageFunc.navigateTo = function (path) {
    $location.url(path);
  };

  pageFunc.init = function() {
    pageData.carModel = selectedCarModel.get();
      console.log('carModel',pageData.carModel);
      currUserInfo.getPromise().then(function(user){
          pageData.name = user.truename;
          pageData.contact = user.phone;
      });
  };

  $scope.$on('ngDialog.closing', function (e, $dialog) {
    if($scope.successDialog && $dialog.attr('id') == $scope.successDialog.id) {
      pageFunc.navigateTo('/store/orders');
    }
  });

  pageFunc.init();

}]);