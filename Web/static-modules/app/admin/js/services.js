comeonModule.factory('commService', ['$http', '$rootScope', '$modal', '$location', '$anchorScroll', function ($http, $rootScope, $modal, $location, $anchorScroll) {

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
    commService.login = function(userId, pwd, checkCode, hasCheckCode, callBack) {
        commService.postData(baseData.loginHandlerUrl, {
            action: 'login',
            userid: userId,
            pwd: pwd,
            checkcode: checkCode,
            hascheckcode: hasCheckCode
        }, function(data) {
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
    commService.checkLogin = function(callBack) {
        var reqData = {
            action: 'islogin'
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
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
    commService.findPassword = function (checkCode, email, callBack) {
        var reqData = {
            action: 'findPassword',
            checkcode: checkCode,
            email: email
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        });
    }

    /**
     * [getCurrUserInfo 获取当前用户信息]
     * @param  {[type]} callBack [description]
     * @return {[type]}          [description]
     */
    commService.getCurrUserInfo = function(callBack) {
        return JSON.parse(localStorage.getItem(baseData.localStorageKeys.currUserInfo));
    };

    commService.setCurrUserInfo = function(userInfo) {
        localStorage.setItem(baseData.localStorageKeys.currUserInfo, JSON.stringify(userInfo));
    };

    commService.showLoginModal = function(okFn, cancelFn) {
        var modal = $modal.open({
            animation: true,
            templateUrl: baseViewPath + 'tpls/global/loginModal.html?v=1.0',
            controller: function($scope, $modalInstance, commService) {
                var loginModalFn = $scope.loginModalFn = {};
                var loginModalData = $scope.loginModalData = {
                    userId: '',
                    pwd: '',
                    checkCode: ''
                };

                loginModalFn.ok = function() {
                    commService.login(loginModalData.userId, loginModalData.pwd, loginModalData.checkCode, 1, function(data, user) {
                        if (data.issuccess) {
                            okFn(data, user);
                            $modalInstance.close();
                        } else {
                            alert(data.message);
                        }
                    });
                };

                loginModalFn.cancel = function() {
                    cancelFn();
                    $modalInstance.close();
                };

            }
        });
    };

    commService.checkIsRecharge = function (id,callBack, failCallBack) {
        var reqData = {
            action: 'checkIsRecharge',
            id: id
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    }

    //显示二维码
    commService.showQRCodeModal = function (id, title, url) {
        var modal = $modal.open({
            animation: true,
            templateUrl: baseViewPath + 'tpls/global/qRCodeModal.html?v=1.0',
            size:"sm",
            controller: function ($scope, $modalInstance, commService) {
                var ModalFn = $scope.ModalFn = {};
                var ModalData = $scope.ModalData = {
                    id:id,
                    title: title,
                    isOk :false,
                    url: url
                };
                ModalFn.checkIsOk = function () {
                    commService.checkIsRecharge(ModalData.id,
                        function (data) {
                            if (data.isSuccess && data.returnValue == "ok") {
                                ModalData.isOk = true;
                                clearInterval(ModalFn.TInteral);
                            }
                        },
                        function () { });
                }
                ModalFn.TInteral = setInterval(function () {
                    ModalFn.checkIsOk();
                }, 3000);
                ModalFn.cancel = function () {
                    clearInterval(ModalFn.TInteral);
                    $modalInstance.close();
                };
            }
        });
    };
    /**
     * [urlParams get参数处理]
     * @param  {[type]} url      [description]
     * @param  {[type]} jsonData [description]
     * @return {[type]}          [description]
     */
    commService.urlParams = function(url, jsonData) {
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

    commService.loadRemoteData = function (url, reqData, callBack, failCallBack) {
        var layerIndex = layer.load();
        $http.get(commService.urlParams(url, reqData)).success(function (data) {
            layer.close(layerIndex);
            callBack(data);
        }).error(function (data) {
            layer.close(layerIndex);
            failCallBack(data);
        });
    };

    commService.get = function (url, reqData, callBack, failCallBack) {
        commService.loadRemoteData(url,reqData,callBack,failCallBack);
    };


    commService.postData = function (url, reqData, callBack, failCallBack) {
        var layerIndex = layer.load();
        $http({ 
            method: 'POST',
            url: url,
            data: $.param(reqData),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            }
        }).success(function (data) {
            layer.close(layerIndex);
            callBack(data)
        });
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
        }
        else if (weekC >= 1) {
            result = "" + parseInt(weekC) + "周前";
        }
        else if (dayC >= 1) {
            result = "" + parseInt(dayC) + "天前";
        }
        else if (hourC >= 1) {
            result = "" + parseInt(hourC) + "小时前";
        }
        else if (minC >= 1) {
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
    commService.pushArrFilterRepeat = function(arr, arrOld, field) {
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
    commService.getArrCount = function(arr, field, value) {
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
    commService.setArrValue = function(arr, field, value) {
        for (var i = 0; i < arr.length; i++) {
            arr[i][field] = value;
        };
    };

    commService.getGetKeyVauleDatas = function(option, callBack, failCallBack) {
        var reqData = {
            action: 'getGetKeyVauleDatas'
        };

        reqData = $.extend(reqData, option);

        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    /**
     * [pageScorllTo 页面内跳到指定锚点]
     * @param  {[type]} id [description]
     * @return {[type]}    [description]
     */
    commService.pageScorllTo = function (id) {
        var offsetTop = $("#" + id).offset().top;
        document.body.scrollTop = offsetTop;
        //$location.hash(id);
        //$anchorScroll();
    };

    return commService;

}]);

/**
 * [description]
 * @param  {Object} ) {               var DatePicker [description]
 * @return {[type]}   [description]
 */
comeonModule.factory('datePickerCore', function() {
    var DatePicker = {};

    /**
     * [DayModel 日期对象]
     * @param {[type]}  year         [description]
     * @param {[type]}  month        [description]
     * @param {[type]}  week         [description]
     * @param {[type]}  day          [description]
     * @param {Boolean} isTody       [description]
     * @param {Boolean} isCheck      [description]
     * @param {Boolean} isCurrSelect [description]
     */
    var DayModel = function(year, month, week, day, isTody, isCheck, isCurrSelect) {
        this.year = year;
        this.week = week;
        this.day = day;
        this.month = month;
        this.isTody = isTody;
        this.isCheck = isCheck;
        this.isCurrSelect = isCurrSelect;
    };

    /**
     * [getDate 获取时间对象]
     * @return {[type]} [description]
     */
    DayModel.prototype.getDate = function() {
        return DatePicker.newDate(this.year, this.month, this.day);
    };

    /**
     * [newDate 创建一个时间]
     * @param  {[type]} year  [description]
     * @param  {[type]} month [description]
     * @param  {[type]} day   [description]
     * @return {[type]}       [description]
     */
    DatePicker.newDate = function(year, month, day) {
        return new Date(Date.parse((year + '-' + month + '-' + day).replace(/-/g, "/")));
    };

    /**
     * [getLastDay 获取最后一天]
     * @param  {[type]} year  [description]
     * @param  {[type]} month [description]
     * @return {[type]}       [description]
     */
    DatePicker.getLastDay = function(year, month) {
        var new_year = year; //取当前的年份
        var new_month = month++; //取下一个月的第一天，方便计算（最后一天不固定）
        if (month > 12) //如果当前大于12月，则年份转到下一年
        {
            new_month -= 12; //月份减
            new_year++; //年份增
        }
        var new_date = new Date(new_year, new_month, 1); //取当年当月中的第一天
        return (new Date(new_date.getTime() - 1000 * 60 * 60 * 24)).getDate(); //获取当月最后一天日期
    };

    /**
     * [init 初始化入口]
     * @return {[type]} [description]
     */
    DatePicker.init = function() {

    };

    /**
     * [createDayArr 创建月份时间数组]
     * @param  {[type]} year  [description]
     * @param  {[type]} month [description]
     * @return {[type]}       [description]
     */
    DatePicker.createDayArr = function(year, month) {
        var resultDayArr = [];
        //7*6时间组合
        var day = 1, //时间增量
            startWeek = 0, //0（周日） 到 6（周六）
            lastDay = DatePicker.getLastDay(year, month);

        //计算第一天开始位置
        startWeek = DatePicker.newDate(year, month, 1).getDay();

        var isDone = false;
        for (var i = 0; i < 6; i++) {
            var tmpDays = []; //每行数据
            if (isDone) {
                break;
            }
            for (var j = 0; j < 7; j++) {

                if (day > lastDay && j == 0) {
                    break;
                }

                if (day > lastDay) {
                    tmpDays.push(new DayModel());
                    isDone = true;
                    continue;
                } //时间增加超过最后一天，跳出循环

                if (i == 0) { //如果是第一行，必须等开始位置才进行时间构造
                    if (j < startWeek) {
                        tmpDays.push(new DayModel());
                        continue;
                    } //还未到达
                }

                var
                    addDay = day++, //添加时间
                    toDay = new Date();


                //构造单个日期对象
                tmpDays.push(new DayModel(
                    year,
                    month,
                    j,
                    addDay, (toDay.getDate() == addDay && (toDay.getMonth() + 1) == month && toDay.getFullYear() == year) ? true : false,
                    0,
                    0
                ));


            };
            resultDayArr.push(tmpDays.slice(0));
        };

        return resultDayArr;
    };

    return DatePicker;
});

comeonModule.factory('commActivity', ['$http', 'commService', function($http, commService) {

    var commActivity = {};

    commActivity.moduleData = {

    };

    commActivity.loadHotActivity = function(callBack, failCallBack) {

        var reqData = {
            action: 'getactivitylist',
            pageindex: 1,
            pagesize: 6
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    commActivity.loadActivityDetail = function(aid, callBack, failCallBack) {
        var reqData = {
            action: 'getactivitydetail',
            activityid: aid
        }

        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    /**
     * [loadSignupList 加载报名列表]
     * @param  {[type]} aid          [description]
     * @param  {[type]} pageIndex    [description]
     * @param  {[type]} pageSize     [description]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}              [description]
     */
    commActivity.loadSignupList = function(aid, pageIndex, pageSize, callBack, failCallBack) {
        var reqData = {
            action: 'getsignpersonlist',
            activityid: aid,
            pageindex: pageIndex,
            pagesize: pageSize
        }

        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    return commActivity;

}]);

comeonModule.factory('commArticle', ['$http', 'commService', '$sce', function($http, commService, $sce) {

    var commArticle = {};

    commArticle.moduleData = {

    };

    /**
     * [loadCommentList 加载评论列表]
     * @param  {[type]} articleid    [description]
     * @param  {[type]} pageindex    [description]
     * @param  {[type]} pagesize     [description]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}              [description]
     */
    commArticle.loadCommentList = function(articleid, pageindex, pagesize, callBack, failCallBack) {

        var reqData = {
            action: 'getCommentList',
            articleid: articleid,
            pageindex: pageindex,
            pagesize: pagesize
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {

            if (data.list && data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {
                    //相关数据转换
                    data.list[i].content = $sce.trustAsHtml(data.list[i].content);
                    data.list[i].createDate = commService.timeShow(data.list[i].createDate);

                    //配置扩展属性
                    data.list[i].subListShow = false;
                    data.list[i].replySubmitShow = false;
                    data.list[i].replyList = [];
                    data.list[i].replyIndex = 1;
                    data.list[i].replySize = 5;

                }
            }

            callBack(data);

        }, failCallBack);

    };

    /**
     * [loadCommentList 加载评论列表]
     * @param  {[type]} option     [description]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}              [description]
     */
    commArticle.loadCommentListByOption = function(option, callBack, failCallBack) {

        var reqData = {
            action: 'getCommentList'
        };
        reqData = $.extend(reqData, option);
        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            if (data.list && data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {
                    //相关数据转换
                    data.list[i].content = $sce.trustAsHtml(data.list[i].content);
                    data.list[i].createDate = commService.timeShow(data.list[i].createDate);

                    //配置扩展属性
                    data.list[i].subListShow = false;
                    data.list[i].replySubmitShow = false;
                    data.list[i].replyList = [];
                    data.list[i].replyIndex = 1;
                    data.list[i].replySize = 5;
                }
            }
            callBack(data);
        }, failCallBack);
    };

    /**
     * [loadReplyList 加载回复列表]
     * @param  {[type]} commentid    [description]
     * @param  {[type]} pageindex    [description]
     * @param  {[type]} pagesize     [description]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}              [description]
     */
    commArticle.loadReplyList = function(commentid, pageindex, pagesize, callBack, failCallBack) {
        var reqData = {
            action: 'getCommentReplyList',
            commentid: commentid,
            pageindex: pageindex,
            pagesize: pagesize
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {

            if (data.list && data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {
                    //相关数据转换
                    data.list[i].content = $sce.trustAsHtml(data.list[i].content);
                    data.list[i].createDate = commService.timeShow(data.list[i].createDate);
                    //配置扩展属性
                    data.list[i].subListShow = false;
                    data.list[i].replySubmitShow = false;

                }
            }

            callBack(data);

        }, failCallBack);

    };

    /**
     * [commentArticle 提交评论]
     * @param  {[type]} articleid [文章id]
     * @param  {[type]} content   [评论内容]
     * @param  {[type]} incognito [是否匿名 1是 0否]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}           [description]
     */
    commArticle.commentArticle = function(callBack, failCallBack, articleid, content, incognito, replyId) {
        var reqData = {
            action: 'commentArticle',
            articleid: articleid,
            content: content,
            isHideUserName: incognito,
            replyId: replyId
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    }

    /**
     * [replyComment 提交评论的回复]
     * @param  {[type]} commentid    [description]
     * @param  {[type]} replyid      [description]
     * @param  {[type]} content      [description]
     * @param  {[type]} incognito    [description]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}              [description]
     */
    commArticle.replyComment = function(commentid, replyid, content, incognito, callBack, failCallBack) {
        var reqData = {
            action: 'replyComment',
            commentid: commentid,
            replyid: replyid,
            content: content,
            isHideUserName: incognito
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    }

    commArticle.praiseReview = function(commentid, callBack, failCallBack) {
        var reqData = {
            action: 'praiseReview',
            id: commentid
        }

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };
    //取消赞
    commArticle.disReviewContent = function (commentid, callBack, failCallBack) {
        var reqData = {
            action: 'disReviewContent',
            id: commentid
        }

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    /**
     * [praiseContent 内容点赞]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @param  {[type]} id           [description]
     * @return {[type]}              [description]
     */
    commArticle.praiseContent = function(callBack, failCallBack, id) {
        var reqData = {
            action: 'praiseContent',
            id: id
        }

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };
    /**
     * [praiseContent 取消内容点赞]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @param  {[type]} id           [description]
     * @return {[type]}              [description]
     */
    commArticle.disPraiseContent = function (callBack, failCallBack, id) {
        var reqData = {
            action: 'disPraiseContent',
            id: id
        }

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };


    commArticle.reportIllegalReview = function(commentid,reason, callBack, failCallBack) {
        var reqData = {
            action: 'reportIllegalReview',
            id: commentid,
            reason: reason
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    commArticle.reportIllegalContent = function(callBack, failCallBack, id) {
        var reqData = {
            action: 'reportIllegalContent',
            id: id
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    commArticle.favoriteArticle = function(callBack, failCallBack, id) {
        var reqData = {
            action: 'favoriteArticle',
            id: id
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    commArticle.disFavoriteArticle = function(callBack, failCallBack, id) {
        var reqData = {
            action: 'disFavoriteArticle',
            id: id
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    commArticle.addArticle = function(callBack, failCallBack, type, content, cateId, summary) {
        var reqData = {
            action: 'addArticle',
            type: type,
            content: content,
            cateId: cateId,
            summary: summary
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    commArticle.getArticleCateList = function(type, preId, pageIndex, pageSize, callBack, failCallBack) {
        var reqData = {
            action: 'getArticleCateList',
            type: type,
            preId: preId ? preId : 0,
            pageIndex: pageIndex ? pageIndex : 0,
            pageSize: pageSize ? pageSize : 0
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            if (data && data.list) {
                for (var i = 0; i < data.list.length; i++) {
                    data.list[i].createTime = commService.timeShow(data.list[i].createTime);
                };
            };
            callBack(data);
        }, failCallBack);
    };

    //查询省市区
    commArticle.getGetKeyVauleDatas = function(type, prekey, canall, callBack, failCallBack) {
        var reqData = {
            action: 'getGetKeyVauleDatas',
            type: type,
            prekey: prekey,
            canall: canall
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };


    //查询专家
    commArticle.getTutors = function(pageindex, pagesize, province, city, keyword, sort, callBack, failCallBack) {
        var reqData = {
            action: 'GetTutors',
            pageIndex: pageindex,
            pageSize: pagesize,
            province: province,
            city: city,
            keyword: keyword,
            sort: sort
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    commArticle.getArticleList = function(callBack, failCallBack, type, cateId, keyword, tags, city, orderby, pageIndex, pageSize, isGetNoCommentData, isHasCommentAndReplayCount) {
        var reqData = {
            action: 'getArticleNewList',
            cateId: cateId,
            keyword: keyword,
            tags: tags,
            city: city,
            orderby: orderby,
            isGetNoCommentData: isGetNoCommentData ? isGetNoCommentData : 0,
            isHasCommentAndReplayCount: isHasCommentAndReplayCount ? isHasCommentAndReplayCount : 0,
            pageIndex: pageIndex ? pageIndex : 0,
            pageSize: pageSize ? pageSize : 0,
            type: type
        };


        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            if (data.list && data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {

                    //相关数据转换
                    data.list[i].content = $sce.trustAsHtml(data.list[i].content);
                    data.list[i].summary = $sce.trustAsHtml(data.list[i].summary);
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

    commArticle.getArticleListByOption = function(option, callBack, failCallBack) {
        var reqData = {
            action: 'getArticleNewList'
        };

        reqData = $.extend(reqData, option);

        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            if (data.list && data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {

                    //相关数据转换
                    data.list[i].content = $sce.trustAsHtml(data.list[i].content);
                    var temp = data.list[i].summary;
                    data.list[i].summarystr = temp;
                    data.list[i].summary = $sce.trustAsHtml(data.list[i].summary);
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
 

    commArticle.getArticleDetail = function(callBack, failCallBack, aid) {
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

    /**
     * [getHotTags 获取热门标签]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @param  {[type]} cateId       [description]
     * @return {[type]}              [description]
     */
    commArticle.getHotTags = function(callBack, failCallBack) {
        var reqData = {
            action: 'getHotTags',
            num: 30
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    /**
     * [getTags 获取标签]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @param  {[type]} cateId       [description]
     * @param  {[type]} num          [description]
     * @return {[type]}              [description]
     */
    commArticle.getTags = function(callBack, failCallBack) {
        var reqData = {
            action: 'getTags'
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    /**
     * [followArticleCategory 关注文章分类]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @param  {[type]} id           [分类id]
     * @return {[type]}              [description]
     */
    commArticle.followArticleCategory = function(callBack, failCallBack, id) {
        var reqData = {
            action: 'followArticleCategory',
            id: id
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    /**
     * [disFollowArticleCategory 取消关注文章分类]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @param  {[type]} id           [分类id]
     * @return {[type]}              [description]
     */
    commArticle.disFollowArticleCategory = function(callBack, failCallBack, id) {
        var reqData = {
            action: 'disFollowArticleCategory',
            id: id
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    commArticle.followArticle = function(callBack, failCallBack, id) {
        var reqData = {
            action: 'followArticle',
            id: id
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    commArticle.disFollowArticle = function(callBack, failCallBack, id) {
        var reqData = {
            action: 'disFollowArticle',
            id: id
        };

        commService.postData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };


    /**
     * [getFollowArticleCategoryUser 获取成员列表]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @param  {[type]} cateId       [description]
     * @param  {[type]} pageIndex    [description]
     * @param  {[type]} pageSize     [description]
     * @param  {[type]} keyword      [description]
     * @return {[type]}              [description]
     */
    commArticle.getFollowArticleCategoryUser = function(callBack, failCallBack, cateId, pageIndex, pageSize, keyword) {
        var reqData = {
            action: 'getFollowArticleCategoryUser',
            cateId: cateId,
            pageIndex: pageIndex,
            pageSize: pageSize,
            keyword: keyword
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, callBack, failCallBack);
    };

    /**
     * [getDailyCase 获取每日案例]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}              [description]
     */
    commArticle.getDailyCase = function(callBack, failCallBack) {
        var reqData = {
            action: 'getDailyCase',
            cateId: baseData.moduleCateIds.case //读取配置的案例分类id
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            if (data.summary)
                var summary = data.summary;
            data.summary = summary.substr(0, 20);
            if (summary.length > 200)
            {
                data.summaryLong = summary.substr(0, 200) + "...";
            }
            else {
                data.summaryLong = summary;
            }           
            callBack(data);
        }, failCallBack);
    };


    /**
     * [loadReplyList 加载回复列表]
     * @param  {[type]} commentid    [description]
     * @param  {[type]} pageindex    [description]
     * @param  {[type]} pagesize     [description]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}              [description]
     */
    commService.getOpenClassWebUrl = function (articleId, callBack, failCallBack) {
            var reqData = {
                action:'getOpenClassWebUrl',
                articleId: articleId
            };
            commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
                if (data.returnObj) data.returnObj.webUrl = $sce.trustAsResourceUrl(data.returnObj.webUrl);
                callBack(data);
            }, failCallBack);
        };
    commArticle.getOpenClassDetail = function(articleId,callBack, failCallBack) {
        var reqData = {
            action: 'getOpenClassDetail',
            articleId: articleId
        };

        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {

            data.content = $sce.trustAsHtml(data.content);
            data.createDate = commService.timeShow(data.createDate);
            data.webUrl = $sce.trustAsResourceUrl(data.webUrl);
            callBack(data);

        }, failCallBack);

    };
    ///公开课公告
    commService.getOpenClassNotice = function (callBack, failCallBack) {
        var reqData = {
            action: 'getOpenClassNotice'
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            if (data.notice) {
                data.notice = data.notice.replace(new RegExp("\n", "gm"), "</p><p>")
                data.notice = $sce.trustAsHtml(data.notice);
            }
            callBack(data);
        }, failCallBack);
    };
    return commArticle;

}]);

comeonModule.factory('userService', ['$http', 'commService', '$sce', function($http, commService, $sce) {
    var userService = {};

    /**
     * [getUserInfo 获取用户信息]
     * @param  {[type]} id     [description]
     * @param  {[type]} callBack     [description]
     * @return {[type]}              [description]
     */
    userService.getUserInfo = function(id, callBack) {
        var reqData = {
            action: 'getUserInfo',
            id: id
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            callBack(data);
        });
    };
    /**
    * [getUserInfo 获取含有是否显示个人信息的用户信息]
    * @param  {[type]} id     [description]
    * @param  {[type]} callBack     [description]
    * @return {[type]}              [description]
    */
    userService.getEditUserInfo = function (callBack) {
        var reqData = {
            action: 'getEditUserInfo',
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        });
    }
    /**
     * [followUser 关注]
     * @param  {[type]} userId     [description]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}              [description]
     */
    userService.followUser = function(userId, callBack, failCallBack) {
        var reqData = {
            action: 'followUser',
            userId: userId
        };
        commService.postData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    /**
     * [disFollowUser 取消关注]
     * @param  {[type]} userId     [description]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}              [description]
     */
    userService.disFollowUser = function(userId, callBack, failCallBack) {
        var reqData = {
            action: 'disFollowUser',
            userId: userId
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            callBack(data);
        }, failCallBack);
    }

    /**
     * [getArticleCount 获取文章数]
     * @param  {[type]} type     [description]
     * @param  {[type]} cateId     [description]
     * @param  {[type]} userId     [description]
     * @param  {[type]} callBack     [description]
     * @param  {[type]} failCallBack [description]
     * @return {[type]}              [description]
     */
    userService.getArticleCount = function(type, cateId, userId, callBack, failCallBack) {
        var reqData = {
            action: 'getArticleCount',
            type: type,
            cateId: cateId,
            userId: userId
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            callBack(data);
        }, failCallBack);
    }

    userService.getUserFavoriteListByOption = function(option, callBack, failCallBack) {
        var reqData = {
            action: 'getUserFavoriteList'
        };

        reqData = $.extend(reqData, option);

        commService.loadRemoteData(baseData.handlerUrl, reqData, function(data) {
            if (data.list && data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {
                    //相关数据转换
                    data.list[i].summary = $sce.trustAsHtml(data.list[i].summary);
                    data.list[i].time = commService.timeShow(data.list[i].time);
                    data.list[i].tags = data.list[i].tags ? data.list[i].tags.split(',') : null;
                }
            }
            callBack(data);
        }, failCallBack);

    };
    userService.getNewUsers = function (callBack, failCallBack) {
        var reqData = {
            action: 'getNewUsers',
            topNum:5
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            if (data && data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    //相关数据转换
                    data[i].createDate = commService.timeShow(data[i].createDate);
                }
            }
            callBack(data);
        }, failCallBack);
    }
    userService.register = function (option, callBack, failCallBack) {
        var reqData = {
        };
        reqData = $.extend(reqData, option);
        commService.postData(baseData.loginHandlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    userService.updatePassword = function (password, callBack, failCallBack){
        var reqData = {
            action: 'UpdatePassword',
            pwd: password
        };
        commService.postData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    userService.getNotices = function (option, callBack, failCallBack) {
        var reqData = {
            action: 'getnoticelistt'
        };
        reqData = $.extend(reqData, option);
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            if (data.list && data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {
                    //相关数据转换
                    data.list[i].content = $sce.trustAsHtml(data.list[i].content);
                    data.list[i].date = commService.timeShow(data.list[i].date);
                }
            }
            callBack(data);
        }, failCallBack);
    }

    userService.getScoreHistorys = function (option, callBack, failCallBack) {
        var reqData = {
            action: 'getScoreDetailsList'
        };
        reqData = $.extend(reqData, option);
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            if (data.list && data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {
                    data.list[i].date = (new Date(data.list[i].date)).format("yyyy.MM.dd");
                }
            }
            callBack(data);
        }, failCallBack);
    }
    userService.getScoreDefines = function (callBack, failCallBack) {
        var reqData = {
            action: 'getScoreDefineList'
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    userService.applyTutor = function (callBack, failCallBack) {
        var reqData = {
            action: 'applyToTutor'
        };
        commService.postData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }
    userService.updateUserInfo = function (option, callBack, failCallBack) {
        var reqData = {
            action: 'UpdateUserInfo'
        };
        reqData = $.extend(reqData, option);
        commService.postData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    userService.getInterestedUser = function (topNum, callBack, failCallBack) {
        var reqData = {
            action: 'GetInterestedUser',
            topNum: topNum
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    userService.getCanInvitUsers = function (option, callBack, failCallBack) {
        var reqData = {
            action: 'getCanInvitUsers'
        };
        reqData = $.extend(reqData, option);
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }


    userService.getFansUsers = function (option, callBack, failCallBack) {
        var reqData = {
            action: 'getFansUsers'
        };
        reqData = $.extend(reqData, option);
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    userService.getFollowUsers = function (option, callBack, failCallBack) {
        var reqData = {
            action: 'getFollowUsers'
        };
        reqData = $.extend(reqData, option);
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }
    userService.getUserInfoToApplyLawyer = function (id, callBack, failCallBack) {
        var reqData = {
            action: 'getUserInfoToApplyLawyer',
            id: id
        };
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    userService.applyLawyer = function (idCardNo, lawyerLicensePhoto, callBack, failCallBack) {
        var reqData = {
            action: 'applyLawyer',
            idCardNo: idCardNo,
            lawyerLicensePhoto: lawyerLicensePhoto
        };
        commService.postData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    userService.getRechargeConfig = function (callBack, failCallBack) {
        var reqData = {
            action: 'getRechargeConfig'
        };
        commService.postData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }
    userService.getWeixinPreOrder = function (option, callBack, failCallBack) {
        var reqData = {
            action: 'getWeixinPreOrder'
        };
        reqData = $.extend(reqData, option);
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    userService.getEmailCheckCode = function (email, callBack, failCallBack) {
        var reqData = {
            action: 'getEmailCheckCode',
            email:email
        };
        commService.loadRemoteData(baseData.loginHandlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    //获取首页图片信息
    userService.getAdList = function (type, pageIndex, pageSize, callBack, failCallBack) {
        var reqData = {
            action: 'getAdList',
            Type: type,
            pageIndex: pageIndex,
            pageSize: pageSize
        }
        // commService.extend(reqData, option);
        commService.loadRemoteData(baseData.handlerUrl, reqData, function (data) {
            callBack(data);
        }, failCallBack);
    }

    return userService;
}]);


/**
 * [分类服务模块]
 *
 * @return {[obj]} [模块对象]
 */
 comeonModule.factory('cateService', ['commService',
     function(commService) {

         var cateService = {};

         cateService.cateList = function (rootId,callback) {

             //加载服务分类列表
             commService.loadRemoteData("/Handler/App/CationHandler.ashx", {
                 Action: 'QueryArticleCategory',
                 CategoryType: 'article',
                 cateRootId: rootId,
                 page: 1,
                 rows: 50000
             }, function (data) {

                 callback(data.rows);

             }, function (data) {

             });

         };


         return cateService;

     }
 ]);


 comeonModule.factory('carService', ['commService',
     function (commService) {
         var carService = {};

         //获取品牌列表


         //获取车系类别


         //获取车系


         //获取车型


         //获取配件列表
         carService.loadParts = function (option,callback) {

             var jsonData = {
                 action: 'GetPartsList',
                 pageIndex: 1,
                 pageSize: 1000
             };
             
             ObjExtend(jsonData, option);

             commService.loadRemoteData(cc.handler.car,jsonData, function (data) {
                 callback(data);
             }, function (data) {

             });
             
         };

         //删除配件
         carService.deleteParts = function (ids, callback) {
             var jsonData = {
                 action: 'DeleteParts',
                 ids:ids
             };

             commService.loadRemoteData(cc.handler.car, jsonData, function (data) {
                 callback(data);
             }, function (data) {

             });
         };

         //加载商户
         carService.loadSallerAutocomplete = function ($query,filterUsers, callback) {
             var jsonData = {
                 action: 'GetUserListAutocomplete',
                 keyword: $query,
                 userType: 5,
                 filterUsers:filterUsers
             };

             commService.loadRemoteData(cc.handler.car, jsonData, function (data) {
                 callback(data);
             }, function (data) {

             });
         };

         //添加服务
         carService.addServer = function (option, callback) {

             var jsonData = {
                 action: 'AddServer'
             };

             ObjExtend(jsonData, option);

             commService.postData(cc.handler.car, jsonData, function (data) {
                 callback(data);
             }, function (data) {

             });

         };

         //获取服务列表
         carService.loadServer = function (option, callback) {
             var jsonData = {
                 action: 'GetServerList',
                 pageIndex: 1,
                 pageSize: 1000
             };

             ObjExtend(jsonData, option);

             commService.loadRemoteData(cc.handler.car, jsonData, function (data) {
                 callback(data);
             }, function (data) {

             });
         };

         return carService;
     }
 ]);

