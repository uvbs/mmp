var basePath = '/customize/10000care/m/src/';
var publicPath = basePath + 'public/';
var comeonModule = angular.module("comeonModule", ['ngRoute', 'ngSanitize', 'ngStorage', 'ngDialog', 'angular-carousel', 'ngTouch']);

var baseData = {
    slogan: '阳光基地',
    handlerUrl: '/Handler/WanBang/Wap.ashx'
};

comeonModule.config(['$routeProvider', function ($routeProvider) {
    $routeProvider

        //首页
        .when('/index', {
            templateUrl: publicPath + 'index/tpls/index.html',
            controller: 'indexCtrl',
            title:'首页',
            requiredLogin:false
        })
        //登录
        .when('/login', {
            templateUrl: publicPath + 'login/tpls/index.html',
            controller: 'loginCtrl',
            title:'登陆',
            requiredLogin:false
        })
        //发布项目
        .when('/pubProject', {
            templateUrl: publicPath + 'pubProject/tpls/index.html',
            controller: 'pubProjectCtrl',
            title:'发布项目',
            requiredLogin:true
        })
        .when('/about', {
            templateUrl: publicPath + 'about/tpls/index.html',
            title:'介绍',
            requiredLogin:false
        })
        .when('/memorabilia', {
            templateUrl: publicPath + 'memorabilia/tpls/index.html',
            title:'大事记',
            requiredLogin:false
        })
        .when('/internet', {
            templateUrl: publicPath + 'internet/tpls/index.html',
            title:'互联网+ 阳光基地新型劳动项目',
            requiredLogin:false
        })
        .when('/internetDetail', {
            templateUrl: publicPath + 'internetDetail/tpls/index.html',
            title:'互联网+ 阳光基地新型劳动项目',
            requiredLogin:false
        })
        .when('/internetDetail/:id', {
            templateUrl: publicPath + 'internetDetail/tpls/index.html',
            title:'互联网+ 阳光基地新型劳动项目',
            controller:'internetDetailCtrl',
            requiredLogin:false
        })
        .when('/projectList', {
            templateUrl: publicPath + 'projectList/tpls/index.html',
            controller:'projectListCtrl',
            title:'传统项目列表',
            requiredLogin:false
        })
        .when('/projectList/:userid', {
            templateUrl: publicPath + 'projectList/tpls/index.html',
            controller:'projectListCtrl',
            title:'我的项目列表',
            requiredLogin:true
        })
        .when('/my', {
            templateUrl: publicPath + 'my/tpls/index.html',
            controller:'myCtrl',
            title:'基地中心',
            requiredLogin:true
        })
        .when('/baseEdit', {
            templateUrl: publicPath + 'baseEdit/tpls/index.html',
            controller:'baseEditCtrl',
            title:'基地信息修改',
            requiredLogin:true
        })
        .when('/companyEdit', {
            templateUrl: publicPath + 'companyEdit/tpls/index.html',
            controller:'companyEditCtrl',
            title:'企业信息修改',
            requiredLogin:true
        })
        .when('/signup', {
            templateUrl: publicPath + 'signup/tpls/index.html',
            controller: 'signupCtrl',
            title: '劳动项目对接洽谈会邀请函',
            requiredLogin: false
        })
        //Invitation1
        .when('/Invitation1', {
            templateUrl: publicPath + 'Invitation1/tpls/index.html',
            controller: 'Invitation1Ctrl',
            title: '邀请函',
            requiredLogin: false
        })
        .when('/Invitation2', {
            templateUrl: publicPath + 'Invitation2/tpls/index.html',
            controller: 'Invitation2Ctrl',
            title: '邀请函',
            requiredLogin: false
        })

        .otherwise({
            redirectTo: '/index'
        });

}]);

comeonModule.run(function ($rootScope, $location, $window) {

    // document.title = baseData.slogan;

    

    $rootScope.$on("$routeChangeStart", function (event, nextRoute, currentRoute) {
        document.title = nextRoute.title + ' - ' + baseData.slogan;
        var islogin = sessionStorage.getItem('islogin');
        if (nextRoute.requiredLogin && !islogin) {
            //alert('请先登录');
            window.location.hash = '#/login';
        }

    });

});

/**
 * [serializeData 序列化对象数据]
 * @param  {[type]} data [description]
 * @return {[type]}      [description]
 */
function serializeData(data) {
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

comeonModule.controller("pageBaseCtrl", ['$scope', function ($scope) {
	$scope.go = function(url) {
		window.location.href = url;
	};

	$scope.goback = function() {
		history.go(-1);
	};

	var ua = navigator.userAgent;

	if (ua.indexOf('MQQBrowser') > 0) {
	    $scope.isQQBrowser = true;
	}

}]);
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

comeonModule.controller('baseEditCtrl',['$scope',function ($scope) {
	
}]);
comeonModule.controller('companyEditCtrl',['$scope','commService',function ($scope,commService) {
	var pageData = $scope.pageData = {
		user:JSON.parse(sessionStorage.getItem('user'))
	};
	var pageFunc = $scope.pageFunc = {};

	pageFunc.save = function (argument) {
		var model =
            {
                Action: "UpdateCompanyInfo",
                CompanyName: pageData.user.CompanyName,
                Thumbnails: pageData.user.Thumbnails,
                Address: pageData.user.Address,
                Area: pageData.user.Area,
                Tel: pageData.user.Tel,
                Phone: pageData.user.Phone,
                QQ: pageData.user.QQ,
                Contacts: pageData.user.Contacts,
                BusinessLicenseNumber: pageData.user.BusinessLicenseNumber,
                Introduction: pageData.user.Introduction
            };


        if (model.CompanyName == '') {
            alert('企业名称不能为空');
            return;
        }
        if (model.Contacts == '') {
            alert('负责人不能为空');
            return;
        }

        commService.postData(baseData.handlerUrl,model,function (data) {
			if (data.Status == 1) {
				alert('保存成功');
				sessionStorage.setItem('user',JSON.stringify(pageData.user));
				
				setTimeout(function() {
					window.location.href = "#/my";
				},1000);
				
			}else{
				alert(data.Msg);
			}
		},function (data) {
			// body...
			console.log(data);
		});


	}

}]);
comeonModule.controller('indexCtrl',['$scope',function ($scope) {
	
	var pageData = $scope.pageData = {
		title:'首页 - ' + baseData.slogan
	};
	var pageFunc = $scope.pageFunc = {};

	document.title = pageData.title;


}]);
comeonModule.controller('internetDetailCtrl',['$scope','$routeParams',function ($scope,$routeParams) {
	var pageData = $scope.pageData = {
		list:[],
		currObj:null
	};
	var pageFunc = $scope.pageFunc = {};


	pageFunc.init = function () {
		pageData.list.push({
			id:'zy',
			logo: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fzylogo.png',
			companyName:'上海至云信息科技有限公司',
			projectName: '微信号内容编辑推广',
			linker:'杜先生',
			phone:'18121076290',
			adimg: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fzyimg.jpg',
			content: '领先的移动电商、社群、营销一体化解决方案专家，由原IBM,SAP技术专家创立，基于移动端（微信）为主，适配全屏幕（PC网站，Android/IOS APP），为企业提供开发、运营、推广一体化解决方案，帮组企业实现线上线下(O2O），全渠道整合营销。自主研发具有完全知识产权的至云移动营销平台，基于移动互联网场景化，社群化的特征，为客户提供移动内容管理系统（MCMS），移动会员管理系统（MCRM），移动社群管理系统（MCommunity），移动电商系统（MShop），移动营销系统（MMS），移动业务系统（MBS），数据统计系统（DMS），消息推送系统（MDS）六大模块服务，为企业移动互联网转型保驾护航。现已累计为20多个行业的典型客户服务，其中包括上海联通、华为通信、三星电子、新闻晨报、华东理工大学、海马汽车、福布斯等一大批知名企业。<br>2015年10月，上海至云信息科技有限公司CEO杜鸿飞当选上海市第四届十佳创业新秀。上海至云将进一步领先企业级移动互联网服务市场。'
		}, {
		    id: 'zhuyang',
		    logo: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fzhupao.png',
		    companyName: '上海助扬信息科技有限公司',
		    projectName: '新媒体运营',
		    linker: '孙先生',
		    phone: '18121076290',
		    adimg: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fzupaoneiye.jpg',
		    content: '运营关注大学生创业的信息服务网站---助跑网，希望利用新媒体的力量帮助大学生创业者稳健起步并快速成长！助跑网主要为创业者提供业界资讯、经验分享、政策查询、服务商查询、在线路演及线上社区。定期举办各种创业讲座、经验分享会、政策宣讲、创业沙龙等各类线下活动，帮助创业者更好的拓展人脉，为创业者提供一个有全面、实用的的创业服务平台。'
		}, {
		    id: 'dianli',
		    logo: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fdianlilog.jpg',
		    companyName: '上海点立广告公司',
		    projectName: '网络段子手，微博微信营销稿',
		    linker: '靳女士',
		    phone: '18121076290',
		    adimg: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fdianlineiye.jpg',
		    content: '成立于2012年12月，隶属于掌尚酷媒集团。以专业的服务、创新的手段，为客户提供基于手机媒体的全面营销推广方案。公司整合无线移动营销平台的资源与技术，提供短信、彩信、WAP等新媒体的精准营销渠道，结合广告行业市场营销计划和策略，提供包括有针对性的无线营销方案和基于无线营销平台的线下活动策划与执行等一系列立体无线营销的方案，帮助客户迅速找到市场，抓住市场，占有市场！在为客户提供个人化的一站式专业服务的同时，也为无线媒体渠道资源供应商提供了广阔的业务空间。<br>中国最专业的无线服务媒体<br>中国最具资质的运营商代理<br>中国最具专业技术的平台研发公司<br>中国产品体系最完善的无线营销媒体。'
		}, {
		    id: 'quda',
		    logo: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fqudalog.jpg',
		    companyName: '上海趣搭网络科技有限公司',
		    projectName: '电子商务',
		    linker: '肖先生',
		    phone: '18121076290',
		    adimg: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fqudaneiye.jpg',
		    content: '是一家主做3D试衣产品研究与开发的创新型互联网企业。拥有专业的3D核心技术及管理团队，致力将3D试衣技术应用到在线购物平台。通过动感3D试衣功能，打造集购物、娱乐、体验于一体的全新B2C平台。<br>趣搭在线试衣插件：是趣搭网络科技有限公司旗下设计研发的一个可集成的3D试衣插件。趣搭试衣插件主要服务于服装品牌商城。通过搭载趣搭试衣插件，快速提升你的B2C商城的购物体验<br>趣搭魔镜试衣：趣搭魔镜是一款集成趣搭3D在线试衣功能的大屏展示设备依托趣搭3D在线试衣的优势，互动展示高效、逼真的服装搭配效果，给消费者带来强力的视觉冲击力<br>趣搭手机客户端：专门为iPhone、安卓手机用户以及iPad用户提供在线3D试衣移动应用。依托自主研发的3D试衣技术，为用户全新打造集购物、娱乐、体验于一体的全新平台'
		}, {
		    id: 'qianhao',
		    logo: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fqianhaologo.jpg',
		    companyName: '上海千浩网络科技有限公司',
		    projectName: '运营推广',
		    linker: '陈先生',
		    phone: '18121076290',
		    adimg: 'http://comeoncloud-open.oss-cn-hangzhou.aliyuncs.com/wanbancare%2Fwap%2Fqianhaoneiye.jpg',
		    content: '公司旗下品牌，“Mazone”（玛族）采用自主设计研发为一体的主导经营方式，并且拥有500人的箱包生产基地，配备了专业的技术人员，拥有完备的生产线，加上严格的管理体制，使得千浩网络科技有限公司成为了行业内的一颗闪亮的新星。经过30年的箱包经营和生产，公司规模不断壮大，产品远销世界五大洲40多个国家和地区，深受全球客商的一致信赖和好评。同时，全新打造的国外年轻箱包品牌“Mazone”（玛族）也在天猫、京东、唯品会等电子商务平台上取得了可观的成绩，并不断蓬勃发展，越来越受到年轻人的追捧，成为极具潜力的网商品牌之一。'
		});

		if ($routeParams.id) {
			for (var i = 0; i < pageData.list.length; i++) {
				if (pageData.list[i].id == $routeParams.id) {
					pageData.currObj = pageData.list[i];
				};
			};
		}

	};

	pageFunc.init();

}]);
comeonModule.controller('Invitation1Ctrl', ['$scope', 'commService', function ($scope, commService) {
	var pageData = $scope.pageData = {
	    name: '',
	    phone: '',
	    company: '',
	    opText: '动项目对接洽谈会邀请函'
	};
	var pageFunc = $scope.pageFunc = {};


	$(document).swipeUp(function () {
	     window.location.href = '#/Invitation2';
	});

	wx.ready(function () {
	    wxapi.wxshare({
	        title: '动项目对接洽谈会邀请函',
	        desc: '“2015年上海市阳光职业康复援助基地劳动项目洽谈会”即将召开，诚邀您的爱心参加，与165家“阳光基地”负责人洽商劳动项目承接事宜。',
	        imgUrl: "http://huiji.comeoncloud.net/img/yqh123.png",
	        link: window.location.href
	    })
	});

	

}]);
comeonModule.controller('Invitation2Ctrl', ['$scope', 'commService', function ($scope, commService) {
	var pageData = $scope.pageData = {
	    name: '',
	    phone: '',
	    company: '',
	    opText: '动项目对接洽谈会邀请函'
	};
	var pageFunc = $scope.pageFunc = {};

	$(document).swipeUp(function () {
	      window.location.href = '#/signup';
	});
	$(document).swipeDown(function () {
	    window.location.href = '#/Invitation1';
	});

	wx.ready(function () {
	    wxapi.wxshare({
	        title: '动项目对接洽谈会邀请函',
	        desc: '“2015年上海市阳光职业康复援助基地劳动项目洽谈会”即将召开，诚邀您的爱心参加，与165家“阳光基地”负责人洽商劳动项目承接事宜。',
	        imgUrl: "http://huiji.comeoncloud.net/img/yqh123.png"
	    })
	});
	

}]);
comeonModule.controller('loginCtrl',['$scope','commService',function ($scope,commService) {
	
	var pageData = $scope.pageData = {
		title:'登录 - ' + baseData.slogan,
		loginType:1,
		userId:'',
		pwd:''
	};
	var pageFunc = $scope.pageFunc = {};

	document.title = pageData.title;

	pageFunc.changeLoginType = function (value) {
		pageData.loginType = value;
	};


	pageFunc.login = function () {
		var jsonData = {
			Action: 'Login', 
			UserName: pageData.userId, 
			PassWord: pageData.pwd, 
			UserType:pageData.loginType
		};

		if (jsonData.UserName == '') {
			alert('请输入登录名');
			return;
		};
		if (jsonData.PassWord == '') {
			alert('请输入密码');
			return;
		};

		commService.postData(baseData.handlerUrl,jsonData,function (data) {
			if (data.Status == 1) {
				sessionStorage.setItem('islogin',1);
				sessionStorage.setItem('loginType',pageData.loginType);
				sessionStorage.setItem('userId',pageData.userId);
				sessionStorage.setItem('user',JSON.stringify(data.ExObj));
				window.location.href = "#/my";
			}else{
				alert(data.Msg);
			}
		},function (data) {
			// body...
			console.log(data);
		});

	};


}]);
comeonModule.controller('myCtrl',['$scope',function ($scope) {
	var pageData = $scope.pageData = {
		loginType:sessionStorage.getItem('loginType'),
		userId:sessionStorage.getItem('userId')
	};

	

}]);
comeonModule.controller('projectListCtrl',['$scope','commService','$routeParams',function ($scope,commService,$routeParams) {
	
	var pageData = $scope.pageData = {
		pageIndex:1,
		pageSize:6,
		area:'',
		cate:'',
		projectName:'',
		list:[],
		showList:[],
		isShowMore :true,
		userId:$routeParams.userid
	};
	var pageFunc = $scope.pageFunc = {};

	pageFunc.loadData = function () {
		var jsonData = {
			Action:'GetProjectList',
			Area:pageData.area,
			Category:pageData.cate,
			ProjectName:pageData.projectName,
			PageIndex:pageData.pageIndex,
			PageSize:pageData.pageSize
		}

		if (pageData.userId) {
			jsonData.Action = 'GetMyProjectList';
		};

		commService.loadRemoteData(baseData.handlerUrl,jsonData,function (data) {
			console.log(data);

			pageData.list = pageData.list.concat(data.ExObj);

			if (data.ExObj.length > 0) {
				pageData.isShowMore = true;
			}else{
				pageData.isShowMore = false;
			}

			for (var i = 0; i < data.ExObj.length; i++) {
				pageFunc.addShowData(data.ExObj[i]);
			};

			pageData.pageIndex++;
		},function (data) {
			
		});

	};

	pageFunc.search = function () {
		pageData.showList = [];
		pageData.pageIndex = 1;
		pageFunc.loadData();
	};

	/**
	 * [addShowData 添加展示数据]
	 * @param {[type]} item [description]
	 */
	pageFunc.addShowData = function (item) {
		
		item.ProjectCycleName = commService.getProjectCycleStr(item.ProjectCycle);

		if (pageData.showList.length > 0) {
			var lastShowData = pageData.showList[pageData.showList.length - 1];	
			if (lastShowData.length < 2) {
				lastShowData.push(item);
			}else{
				pageData.showList.push([item]);
			}
		}else{
			pageData.showList.push([item]);
		}
		
	};


	pageFunc.init = function () {
		pageFunc.loadData();
	};

	pageFunc.init();

}]);
comeonModule.controller('pubProjectCtrl',['$scope','commService',function ($scope,commService) {
	var pageData = $scope.pageData = {
		title:'发布项目 - ' + baseData.slogan,
		category:'',
		projectCycle:0,//项目周期
		logistics:0,//项目物流 0企业负责配送  1基地负责配送 
		timeRequirement:'',//工作要求
		introduction:'',//项目介绍
		projectName:''
	};
	var pageFunc = $scope.pageFunc = {};

	document.title = pageData.title;

	/**
	 * [save 保存]
	 * @return {[type]}          [description]
	 */
	pageFunc.save = function () {
		var jsonData = {
			AutoID: 0,
			Action: 'AddProjectInfo',
			ProjectName:pageData.projectName,
			Category: pageData.category,
			Logistics: pageData.logistics,
			ProjectCycle: pageData.projectCycle,
			TimeRequirement: pageData.timeRequirement,
			Introduction: pageData.introduction
		};

		if (jsonData.ProjectName == '') {
			// layer.open({content:'项目名不能为空'});
			alert('项目名不能为空');
			return;
		};
		if (jsonData.Category == '') {
			alert('请选择分类');
			return;
		};
		if (jsonData.ProjectCycle == '') {
			alert('请选择周期');
			return;
		};

		commService.postData(baseData.handlerUrl,jsonData,function (data) {
			if (data.Status == 1) {
				alert('保存成功');
				setTimeout(function () {
					window.location.href = '#/projectList';
				},1000);
			}else{
				alert(data.Msg);
			}
		},function (data) {
			// body...
		});

	};

	pageFunc.changeLogistics = function(value){
		pageData.logistics = value;
	};

}]);
comeonModule.controller('signupCtrl', ['$scope', 'commService', function ($scope, commService) {
	var pageData = $scope.pageData = {
	    name: '',
	    phone: '',
	    company: '',
	    opText:'提交报名'
	};
	var pageFunc = $scope.pageFunc = {};


	var lock = false;

	/**
	 * [save 保存]
	 * @return {[type]}          [description]
	 */
	pageFunc.save = function () {
		
	    if (lock) {
	        return;
	    }
	    lock = true;
	    pageData.opText = "正在提交......";

	    if (pageData.name == '') {
	        lock = false;
	        pageData.opText = "提交报名";
	        alert("姓名不能为空");
	        return;
	    }
	    if (pageData.phone == '') {
	        lock = false;
	        pageData.opText = "提交报名";
	        alert("联系电话不能为空");
	        return;
	    }
	    if (pageData.company == '') {
	        lock = false;
	        pageData.opText = "提交报名";
	        alert("公司不能为空");
	        return;
	    }

		var jsonData = {
		    Action: 'submitActivityDataNoLogin',
		    activityid: '506935',
		    Name: pageData.name,
		    Phone: pageData.phone,
		    K2: pageData.company
		};
		
		commService.postData("/Serv/pubapi.ashx", jsonData, function (data) {
		    if (data.errmsg == 'ok') {
		        pageData.opText = "提交完成";
				alert('报名成功');
				
		    } else {
		        pageData.opText = "提交报名";
		        lock = false;
			    alert(data.errmsg);
			}
		},function (data) {
			// body...
		});

	};

	//var i = 1;
	//$(document).swipeUp(function () {
	//    if (i % 2 == 0) {
	//        if (window.location.href.indexOf('Invitation1') > -1) {
	//            window.location.href = '#/Invitation2';
	//        } else if (window.location.href.indexOf('Invitation2') > -1) {
	//            window.location.href = '#/signup';
	//        }
	//    }
	//    i++;
	    
	//});

	//$(document).swipeDown(function () {
	//    window.location.href = '#/Invitation2';
	//});

	wx.ready(function () {
	    wxapi.wxshare({
	        title: '劳动项目对接洽谈会邀请函',
	        desc: '“2015年上海市阳光职业康复援助基地劳动项目洽谈会”即将召开，诚邀您的爱心参加，与165家“阳光基地”负责人洽商劳动项目承接事宜。',
	        imgUrl: "http://huiji.comeoncloud.net/img/yqh123.png"
	    })
	});
	

}]);