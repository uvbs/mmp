yingfumodule.factory('commService', ['$http', '$rootScope', '$location', '$anchorScroll', function ($http, $rootScope, $location, $anchorScroll) {
    var commService = {};

    /**
     * [urlParams get参数处理]
     * @param  {[type]} url      [description]
     * @param  {[type]} jsonData [description]
     * @return {[type]}          [description]
     */
    commService.urlParams = function (url, jsonData) {
        var result = url + '?';
        var i = 0;
        var keys = Object.keys(jsonData);
        for (var i = 0; i < keys.length; i++) {
            if (i != 0) {
                result += '&';
            }
            result += keys[i] + '=' + jsonData[keys[i]];
        }
        return result;
    };
    commService.extend = function (reqData, option) {
        var keys = Object.keys(option);
        for (var i = 0; i < keys.length; i++) {
            if (option[keys[i]]) reqData[keys[i]] = option[keys[i]];
        }
    };

    commService.loadRemoteData = function (url, reqData, callBack, failCallBack) {
        $http.get(commService.urlParams(url, reqData)).success(function (data) {
            callBack(data);
        }).error(function (data) {
            failCallBack(data);
        });
    };

    commService.postData = function (url, reqData, callBack, failCallBack) {
        $http({
            method: 'POST',
            url: url,
            data: serializeData(reqData),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).success(function (data) {
            callBack(data)
        });
    };
    /**
     * [setArrValue 批量设置数据对象指定值]
     * @param {[type]} arr   [description]
     * @param {[type]} field [description]
     * @param {[type]} value [description]
     */
    commService.setArrValue = function (arr, field, value) {
        for (var i = 0; i < arr.length; i++) {
            arr[i][field] = value;
        };
    };

    return commService;
}]);