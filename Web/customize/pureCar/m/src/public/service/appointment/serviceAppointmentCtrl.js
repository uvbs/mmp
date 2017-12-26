/* global pureCarModule */
pureCarModule.controller('serviceAppointmentCtrl', ['$scope', '$location', function($scope, $location) {
  var pageFunc = $scope.pageFunc = {
    setServiceType: function (i) {
      pageData.serviceType = i;
    },
    
    selectEndpoint: function() {
      $location.url('/service/appointment/endpoints');
    },
    
    showSelectServiceDialog: function() {
      
    },
    
    showSelectDateDialog: function() {
      
    },
    
    showSelectHourDialog: function() {
      console.log('showSelectHourDialog')
      ngDialog.open({
        template: publicPath + 'service/appointment/tpls/select-hour.html',
        scope: $scope
      });
    }
  };
  var pageData = $scope.pageData = {
    title: '养护预约',
    serviceType: 0,
  };

  document.title = pageData.title;

}]);
