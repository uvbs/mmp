distributionmodule.factory('commService', ['$http', '$rootScope', '$location', '$anchorScroll', '$sce',
        function ($http, $rootScope, $modal, $location, $anchorScroll, $sce) {
            var commService = {};
            //新接口处理方法
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
            };
            /**
             * [urlParams get参数处理]
             * @param  {[type]} action      [description]
             * @param  {[type]} jsonData [description]
             * @return {[type]}          [description]
             */
            commService.urlParams = function (action, jsonData) {
                var url = baseData.handlerUrl + action;
                if (typeof jsonData === 'object') {
                    url += '?';
                    for (var key in jsonData) {
                        url += key + '=' + jsonData[key] + '&';
                    }
                    url = url.slice(0, url.length - 1);
                }
                return url;

                //var result = newurl + '?';
                ////var result = url + '?';
                //var i = 0;
                //var keys = Object.keys(jsonData);
                //for (var i = 0; i < keys.length; i++) {
                //    if (i != 0) {
                //        result += '&';
                //    }
                //    result += keys[i] + '=' + jsonData[keys[i]];
                //}
                //return result;
            };
            commService.loadRemoteData = function (action, reqData, callBack, failCallBack) {
                if(reqData.isnoloading){
                    
                }else{
                    layer.open({type: 2});
                }
                $http.get(commService.urlParams(action, reqData)).success(function (data) {
                    layer.closeAll();
                    callBack(data);
                }).error(function (data) {
                    layer.closeAll();
                    failCallBack(data);
                });
            };

            commService.get =commService.loadRemoteData;

            commService.postData = function (url, reqData, callBack, failCallBack) {
                layer.open({type: 2});
                $http({
                    method: 'POST',
                    url: url,
                    data: commService.serializeData(reqData),
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }).success(function (data) {
                    layer.closeAll();
                    callBack(data)
                });
            };

            //新接口处理方法 end

            //commService.extend = function (reqData, option) {
            //    var keys = Object.keys(option);
            //    for (var i = 0; i < keys.length; i++) {
            //        if (typeof(option[keys[i]]) != 'undefined')
            //            reqData[keys[i]] = option[keys[i]];
            //    }
            //};

            //添加银行卡接口(post)
            commService.addBankCard = function(option, callBack, failCallBack) {
                var action='bankcard/add.ashx';
                commService.postData(baseData.handlerUrl+action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取银行卡列表接口(get)
            commService.getBankCardList = function(option, callBack, failCallBack) {
                var action='bankcard/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取银行卡详情接口(get)
            commService.getBankCardDetail = function(option, callBack, failCallBack) {
                var action='bankcard/get.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //修改银行卡接口(post)
            commService.updateBankCard = function(option, callBack, failCallBack) {
                var action='bankcard/update.ashx';
                commService.postData(baseData.handlerUrl+action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //删除银行卡接口(get)
            commService.deleteBankCard = function(option, callBack, failCallBack) {
                var action='bankcard/delete.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };

            //添加项目接口(get)
            commService.addProject = function(option, callBack, failCallBack) {
                var action='project/add.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取项目接口(get)
            commService.getProjectList = function(option, callBack, failCallBack) {
                var action='project/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //申请提现接口(get)
            commService.applyForCash = function(option, callBack, failCallBack) {
                var action='WithdrawCash/add.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //申请提现记录接口(get)
            commService.applyForCashRecord = function(option, callBack, failCallBack) {
                var action='withdrawcash/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取当前用户信息接口(get)
            commService.getCurrUserInfo = function(option, callBack, failCallBack) {
                var action='user/currentuserinfo.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取用户下级分销接口(get)
            commService.getUserNextLevel = function(option, callBack, failCallBack) {
                var action='user/down/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //分销配置接口(get)
            commService.getDistributeConfig = function(option, callBack, failCallBack) {
                var action='config/get.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //生成二维码接口(get)  http://dev1.comeoncloud.net/serv/api/common/qrcode.ashx?code=http://www.qq.com
            //commService.getQrCode = function(option, callBack, failCallBack) {
            //    var url=baseDomain+'serv/api/common/qrcode.ashx?code='+option;
            //    $http.get(url).success(function (data) {
            //        layer.closeAll();
            //        callBack(data);
            //    }).error(function (data) {
            //        layer.closeAll();
            //        failCallBack(data);
            //    });
            //};
            //活动报名接口(post)
            commService.getQrCode = function(option, callBack, failCallBack) {
                commService.postData(baseDomain+'serv/api/common/qrcode.ashx', option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取线下分销申请活动ID接口(get)
            commService.getDisActivity = function(option, callBack, failCallBack) {
                var action='User/ApplyActivity.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取项目日志列表接口(get)
            commService.getProjectLog = function(option, callBack, failCallBack) {
                var action='projectlog/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //活动报名接口(post)
            commService.activityRegist = function(option, callBack, failCallBack) {
                commService.postData(baseDomain+'serv/ActivityApiJson.ashx', option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取用户当前信息详细接口(get)  http://fenxiao.comeoncloud.net/serv/api/user/info.ashx?action=currentuserinfo
            commService.getUserDetail = function(callBack, failCallBack) {
                var url=baseDomain+'serv/api/user/info.ashx?action=currentuserinfo';
                $http.get(url).success(function (data) {
                    layer.closeAll();
                    callBack(data);
                }).error(function (data) {
                    layer.closeAll();
                    failCallBack(data);
                });
            };
            //获取项目分佣记录(积分记录)接口(get)
            commService.getScore = function(option, callBack, failCallBack) {
                var action='projectcommission/list.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };
            //获取项目提交字段接口(get)
            commService.getProjectFields = function(option, callBack, failCallBack) {
                var action='config/get.ashx';
                commService.loadRemoteData(action, option, function(data) {
                    callBack(data);
                }, failCallBack);
            };

            return commService;
        }]);
