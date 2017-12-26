pureCarModule.factory('selectedCarModel', function() {
  var model = null,key ='selectedCarModel' ;
  return {
    set: function (value) {
      model = value;
      localStorage.setItem(key,JSON.stringify(value));
    },
    get: function () {
      model = JSON.parse(localStorage.getItem(key));
      return model;
    }
  }
}).factory('stores', ['$q', function($q) {
  var stores = {};
  return {
    set: function (value) {
      if(!value) { return; }
      stores[value.sallerId] = value;
    },
    getPromise: function (sallerId, refresh) {
      if(stores[sallerId] && !refresh) { return $q.when(stores[sallerId]); }
      else {
        return getReturnObj('user/saller/detail.ashx', {sallerId: sallerId}).then(function (detail) {
          stores[sallerId] = detail;
          return detail;
        });
      }
    }
  }
}]).factory('addressProvinces', ['$q', function ($q) {
  var provinces;
  return {
    getPromise: function() {
      if(provinces) { return $q.when(provinces); }
      else {
        var d = $q.defer();
        $.get(apiPath + 'mall/area.ashx', {action: 'provinces'}).then(function (result) {
          provinces = result.list;
          d.resolve(provinces);
        });
        return d.promise;
      }
    }
  };
}]).factory('addressCities', ['$q', function ($q) {
  var cities = {};
  return {
    getPromise: function(provinceCode) {
      if(cities[provinceCode]) { return $q.when(cities[provinceCode]); }
      else {
        var d = $q.defer();
        $.get(apiPath + 'mall/area.ashx', {action: 'cities', province_code: provinceCode}).then(function (result) {
          cities[provinceCode] = result.list;
          d.resolve(cities[provinceCode]);
        });
        return d.promise;
      }
    }
  };
}]).factory('addressDistricts', ['$q', function ($q) {
  var districts = {};
  return {
    getPromise: function(cityCode) {
      if(districts[cityCode]) { return $q.when(districts[cityCode]); }
      else {
        var d = $q.defer();
        $.get(apiPath + 'mall/area.ashx', {action: 'districts', city_code: cityCode}).then(function (result) {
          districts[cityCode] = result.list;
          d.resolve(districts[cityCode]);
        });
        return d.promise;
      }
    }
  };
}]).factory('addressAreas', ['$q', function ($q) {
  var areas = {};
  return {
    getPromise: function(districtCode) {
      if(areas[districtCode]) { return $q.when(areas[districtCode]); }
      else {
        var d = $q.defer();
        $.get(apiPath + 'mall/area.ashx', {action: 'areas', district_code: districtCode}).then(function (result) {
          areas[districtCode] = result.list;
          d.resolve(areas[districtCode]);
        });
        return d.promise;
      }
    }
  };
}]).factory('currUserInfo',['$q',function($q){
    return {
        getPromise:function(){
            if(localStorage.getItem('currUserInfo')){
                return $q.when(JSON.parse(localStorage.getItem('currUserInfo')));
            }else{
                var d = $q.defer();
                $.get(apiPath + 'user/info.ashx', {action: 'currentuserinfo'}).then(function (result) {
                    localStorage.setItem('currUserInfo',JSON.stringify(result));
                    d.resolve(result);
                });
                return d.promise;
            }
        }
    };
}]);