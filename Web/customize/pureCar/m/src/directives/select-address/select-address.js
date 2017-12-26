pureCarModule.directive('selectAddress', ['addressAreas', 'addressDistricts'
  , function(addressAreas, addressDistricts) {
  return {
    restrict: 'ECMA',
    templateUrl: basePath + 'directives/select-address/tpls/index.html',
    replace: true,
    scope: {
      confirm: '=confirm'
    },
    controller: function($scope, $element, $attrs) {
      var pageFunc = $scope.pageFunc = {
        loadDistricts: function (cityCode) {
          addressDistricts.getPromise(cityCode).then(function (districts) {
            pageData.districts = districts;
            pageData.selectedDistrict = districts[0];
          });
        },
        loadAreas: function (districtCode) {
          addressAreas.getPromise(districtCode).then(function (areas) {
            pageData.areas = areas;
            pageData.selectedArea = areas[0];
          });
        },
      };
      var pageData = $scope.pageData = {
        districts: [],
        areas: [],
      };

      pageFunc.loadDistricts(321);
      $scope.$watch('pageData.selectedDistrict', function(newValue) { if(!newValue) {return;} pageFunc.loadAreas(newValue.code); });
    }
  };
}]);