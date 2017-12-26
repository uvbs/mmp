comeonModule.factory('commService', ['$http', '$rootScope', '$location', '$anchorScroll', '$sce', function ($http, $rootScope, $location, $anchorScroll, $sce) {

    var commService = {};

    commService.moduleData = {
        currUser: null
    };

    /**
     * [userModel 用户对象实体]
     * @param  {[type]} userId   [description]
     * @param  {[type]} avatar   [description]
     * @param  {[type]} userName [description]
     * @param  {[type]} phone    [description]
     * @return {[type]}          [description]
     */
    var userModel = function (id, userId, avatar, userName, phone, unreadcount) {
        this.id = id,
            this.userId = userId;
        this.avatar = avatar;
        this.userName = userName;
        this.phone = phone;
        this.unreadcount = unreadcount;
    };

    /**
     * [login 登录]
     * @param  {[type]} userid       [description]
     * @param  {[type]} pwd          [description]
     * @param  {[type]} checkcode    [description]
     * @param  {[type]} hascheckcode [description]
     * @param  {[type]} callBack     [description]
     * @return {[type]}              [description]
     */
    commService.login = function (userId, pwd, checkCode, hasCheckCode, callBack) {
        commService.postData(baseData.loginHandlerUrl, {
            action: 'login',
            userid: userId,
            pwd: pwd,
            checkcode: checkCode,
            hascheckcode: hasCheckCode
        }, function (data) {
            var user = null;
            if (data.issuccess) {
                //登录成功
                user = new userModel(
                    data.id,
                    data.userid,
                    data.avatar,
                    data.userName,
                    data.phone,
                    data.userUnReadNoticeCount
                );
            }
            commService.setCurrUserInfo(user);
            callBack(data, user);
        });
    };

    /**
     * [checkLogin 异步登录检查]
     * @return {[type]} [description]
     */
    commService.checkLogin = function (callBack) {
        var reqData = {
            action: 'islogin'
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            var user = null;
            if (data.islogin) {
                user = new userModel(
                    data.id,
                    data.userid,
                    data.avatar,
                    data.userName,
                    data.phone,
                    data.userUnReadNoticeCount
                );
            }

            commService.setCurrUserInfo(user);

            callBack(user);
        });
    };

    /**
     * [logout 登出]
     * @return {[type]} [description]
     */
    commService.logout = function (callBack) {
        var reqData = {
            action: 'logout'
        };
        commService.loadRemoteData(baseData.loginHandlerUrl, reqData, function (data) {
            callBack(data);
        });
    };

    /**
     * [getCurrUserInfo 获取当前用户信息]
     * @param  {[type]} callBack [description]
     * @return {[type]}          [description]
     */
    commService.getCurrUserInfo = function (callBack) {
        return JSON.parse(localStorage.getItem(baseData.localStorageKeys.currUserInfo));
    };

    commService.setCurrUserInfo = function (userInfo) {
        localStorage.setItem(baseData.localStorageKeys.currUserInfo, JSON.stringify(userInfo));
    };

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
        //for (item in jsonData) {
        //    if (typeof (item) == 'undefined') {
        //        continue;
        //    }
        //    if (typeof (item) == 'function') {
        //        continue;
        //    }
        //    if (i != 0) {
        //        result += '&';
        //    }
        //    result += item + '=' + jsonData[item];
        //    i++;
        //}
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

    /**
     * [pushArrFilterRepeat 追加并过滤指定字段重复的数据]
     * @param  {[type]} arr    [新数据]
     * @param  {[type]} arrOld [原数据]
     * @param  {[type]} field  [过滤字段]
     * @return {[type]}        [description]
     */
    commService.pushArrFilterRepeat = function (arr, arrOld, field) {
        if (!arrOld) {
            arrOld = [];
        }

        if (arr) {
            for (var i = 0; i < arr.length; i++) {

                var isHas = false;

                for (var j = 0; j < arrOld.length; j++) {
                    if (arrOld[j][field] == arr[i][field]) {
                        isHas = true;
                        break;
                    }
                }

                if (isHas) {
                    continue;
                } else {
                    arrOld.push(arr[i]);
                }

            }
        }

    };

    /**
     * [getArrCount 查询指定数组数量]
     * @param  {[type]} arr   [description]
     * @param  {[type]} field [description]
     * @param  {[type]} value [description]
     * @return {[type]}       [description]
     */
    commService.getArrCount = function (arr, field, value) {
        var result = 0;

        for (var i = 0; i < arr.length; i++) {
            if (arr[i][field] == value) {
                result++;
            };
        };

        return result;
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

    commService.getGetKeyVauleDatas = function (option, callBack, failCallBack) {
        var reqData = {
            action: 'getGetKeyVauleDatas'
        };

        commService.extend(reqData, option);

        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    /**
     * [pageScorllTo 页面内跳到指定锚点]
     * @param  {[type]} id [description]
     * @return {[type]}    [description]
     */
    commService.pageScorllTo = function (id) {
        $location.hash(id);
        $anchorScroll();
    };

    commService.getProjectCycleStr = function (cycle) {
        if (cycle == 0) {
            return '临时(1个月以内)';
        }
        if (cycle == 1) {
            return '短期(1-3个月)';
        }
        if (cycle == 2) {
            return '中期(3-6个月)';
        }
        if (cycle == 3) {
            return '长期(6-12个月)';
        }
    };

    return commService;

}]);
