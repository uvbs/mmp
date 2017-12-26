angular.module('comm-services',[]).factory('commService', ['$http', '$rootScope', '$location', '$anchorScroll', '$sce',
    function ($http, $rootScope, $modal, $location, $anchorScroll, $sce) {
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
        commService.loadRemoteData = function (url, reqData, callBack, failCallBack) {
            //layer.open({type: 2});
            $http.get(commService.urlParams(url, reqData)).success(function (data) {
                //layer.closeAll();
                callBack(data);
            }).error(function (data) {
                //layer.closeAll();
                failCallBack(data);
            });
        };
        commService.postData = function (url, reqData, callBack, failCallBack) {
            //layer.open({type: 2});
            $http({
                method: 'POST',
                url: url,
                data: commService.serializeData(reqData),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            }).success(function (data) {
                //layer.closeAll();
                callBack(data)
            }).error(function (data) {
                //layer.closeAll();
                failCallBack(data);
            });
        };

        commService.extend = function (reqData, option) {
            var keys = Object.keys(option);
            for (var i = 0; i < keys.length; i++) {
                if (typeof(option[keys[i]]) != 'undefined')
                    reqData[keys[i]] = option[keys[i]];
            }
        };

        /**
         * [timeShow 时间展示处理]
         * @param  {[type]} time [description]
         * @return {[type]}      [description]
         */
        commService.timeShow = function (timeValue) {

            //几分钟前  几小时前  几天前  几月前 超过一年的则展现原数据
            var time = new Date(timeValue);
            var now = new Date();
            var diffValue = now.getTime() - time.getTime();

            var minute = 1000 * 60;
            var hour = minute * 60;
            var day = hour * 24;
            var halfamonth = day * 15;
            var month = day * 30;
            var year = 365 * day;

            var yearC = diffValue / year;
            var monthC = diffValue / month;
            var weekC = diffValue / (7 * day);
            var dayC = diffValue / day;
            var hourC = diffValue / hour;
            var minC = diffValue / minute;

            if (yearC >= 1) {
                // result = time.format("yyyy-MM-dd hh:mm");
            }
            if (monthC >= 1) {
                result = "" + parseInt(monthC) + "个月前";
            } else if (weekC >= 1) {
                result = "" + parseInt(weekC) + "周前";
            } else if (dayC >= 1) {
                result = "" + parseInt(dayC) + "天前";
            } else if (hourC >= 1) {
                result = "" + parseInt(hourC) + "小时前";
            } else if (minC >= 1) {
                result = "" + parseInt(minC) + "分钟前";
            } else {
                result = "刚刚";
            }
            return result;
            //return result;
        };
        /**
         * [serializeData 序列化对象数据]
         * @param  {[type]} data [description]
         * @return {[type]}      [description]
         */
        commService.serializeData = function (data) {
            // If this is not an object, defer to native stringification.
            if (!angular.isObject(data)) {
                return ((data == null) ? "" : data.toString());
            }

            var buffer = [];

            // Serialize each key in the object.
            for (var name in data) {
                if (!data.hasOwnProperty(name)) {
                    continue;
                }

                var value = data[name];

                buffer.push(
                    encodeURIComponent(name) + "=" + encodeURIComponent((value == null) ? "" : value)
                );
            }

            // Serialize the buffer and clean it up for transportation.
            var source = buffer.join("&").replace(/%20/g, "+");
            return (source);
        }
        return commService;
    }]);
