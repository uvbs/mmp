haimamodule.factory('commService',['$http','$rootScope','$location','$anchorScroll','$sce',function($http,$rootScope,$location,$anchorScroll,$sce){
    var commService={};

    /**
     * [urlParams get参数处理]
     * @param  {[type]} url      [description]
     * @param  {[type]} jsonData [description]
     * @return {[type]}          [description]
     */
    commService.urlParams=function(url,jsonData)
    {
        var result=url+'?';
        var i=0;
        var keys=Object.keys(jsonData);
        for(var i=0;i<keys.length;i++)
        {
            if(i!=0)
            {
                result+='&';
            }
            result+=keys[i]+'='+jsonData[keys[i]];
        }
        return result;
    };

    /**
     * [timeShow 时间展示处理]
     * @param  {[type]} time [description]
     * @return {[type]}      [description]
     */
    commService.timeShow = function(timeValue) {

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
            result = time.format("yyyy-MM-dd hh:mm");
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

    commService.extend = function (reqData, option) {
        var keys = Object.keys(option);
        for (var i = 0; i < keys.length; i++) {
            if (option[keys[i]]) reqData[keys[i]] = option[keys[i]];
        }
    };

    commService.loadRemoteData = function(url, reqData, callBack, failCallBack) {
        $http.get(commService.urlParams(url, reqData)).success(function(data) {
            callBack(data);
        }).error(function(data) {
            failCallBack(data);
        });
    };

    commService.postData = function(url, reqData, callBack, failCallBack) {
        $http({
            method: 'POST',
            url: url,
            data: serializeData(reqData),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).success(function(data) {
            callBack(data)
        });
    };
    /**
     * [setArrValue 批量设置数据对象指定值]
     * @param {[type]} arr   [description]
     * @param {[type]} field [description]
     * @param {[type]} value [description]
     */
    commService.setArrValue = function(arr, field, value) {
        for (var i = 0; i < arr.length; i++) {
            arr[i][field] = value;
        };
    };

    commService.getArticleDetail = function(callBack, failCallBack, aid) {
        var reqData = {
            action: 'getArticleDetail',
            articleid: aid
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            if (data) {
                data.content = $sce.trustAsHtml(data.content);
                data.createDate = new Date(data.createDate);
                data.tags = data.tags ? data.tags.split(',') : [];
            };
            callBack(data);
        }, failCallBack);
    };

    commService.getArticleListByOption = function(option, callBack, failCallBack) {
        var reqData = {
            action: 'getArticleNewList'
        };
        commService.extend(reqData, option);

        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            if (data.list && data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {

                    //相关数据转换
                    data.list[i].summary = data.list[i].summary;
                    data.list[i].createDate = commService.timeShow(data.list[i].createDate);
                    data.list[i].tags = data.list[i].tags ? data.list[i].tags.split(',') : null;

                    //配置扩展属性
                    data.list[i].subListShow = false;
                    data.list[i].commentSubmitShow = false;
                    data.list[i].commentList = [];
                    data.list[i].commentIndex = 1;
                    data.list[i].commentSize = 5;

                }
            }

            callBack(data);

        }, failCallBack);

    };

    return commService;
}]);