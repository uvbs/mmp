//
////重写Alert
//var triggers, curTimeout;
//var alert = function (msg, type, autoClose,fn) {
//    if (triggers && curTimeout) {
//        clearTimeout(curTimeout);
//        $("#alertDialog").remove();
//    }
//
//    var dialogHTML = new StringBuilder();
//    dialogHTML.Append("<span id='alertDialog'");
//    dialogHTML.Append("><span></span></span>");
//    $(document.body).append(dialogHTML.toString());
//    $("#alertDialog").removeClass();
//    if (type) {
//        if (type == 2) {
//            $("#alertDialog").addClass("errorDialog");
//        }
//        else if (type == 3) {
//            $("#alertDialog").addClass("warningDialog");
//        } else if (type == 4) {
//            $("#alertDialog").addClass("warningErrorDialog");
//        }
//    }
//    $("#alertDialog span").html(msg);
//    var clientW = document.documentElement.clientWidth;
//    var clientH = document.documentElement.clientHeight || window.innerHeight || document.body.clientHeight;
//    var scrollH = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;
//    $("#alertDialog").css({
//        top: "30%",
//        left: ($(window).width() - $("#alertDialog").innerWidth()) / 2 + "px"
//    });
//
//    triggers = $("#alertDialog");
//    if (typeof (autoClose) != "undefined") {
//        if (autoClose) {
//            curTimeout = setTimeout(function(){
//                triggers.remove();
//                clearTimeout(curTimeout);
//                if (typeof (fn) != "undefined") {
//                    fn();
//                }
//            }, 1500);
//        }
//    } else {
//        curTimeout = setTimeout(function () {
//            triggers.remove();
//            clearTimeout(curTimeout);
//            if (typeof (fn) != "undefined") {
//                fn();
//            }
//        }, 1500);
//    }
//}
////封装StringBuilder
//function StringBuilder() {
//    this._string_ = new Array();
//}
//StringBuilder.prototype.Append = function (str) {
//    this._string_.push(str);
//}
//StringBuilder.prototype.toString = function () {
//    return this._string_.join("");
//}


(function() {
  'use strict';

  angular
    .module('chunkao', ['ngAnimate', 'ngCookies', 'ngTouch', 'ngSanitize', 'ui.router','ngStorage']);

})();

(function() {
  'use strict';

  angular
    .module('chunkao')
    .config(config);

  /** @ngInject */
  function config($logProvider, toastr) {
    // Enable log
    $logProvider.debugEnabled(true);

    // Set options third-party lib
    toastr.options.timeOut = 3000;
    toastr.options.positionClass = 'toast-top-right';
    toastr.options.preventDuplicates = true;
    toastr.options.progressBar = true;
  }

})();

/* global malarkey:false, toastr:false, moment:false */
(function() {
  'use strict';

  angular
    .module('chunkao')
    .constant('malarkey', malarkey)
    .constant('toastr', toastr)
    .constant('moment', moment);


})();

(function() {
  'use strict';

  angular
    .module('chunkao')
    .config(routeConfig);



  /** @ngInject */
  function routeConfig($stateProvider, $urlRouterProvider) {
    $stateProvider
      //首页
      .state('home', {
        url: '/',
        templateUrl: 'app/home/home.html',
        controller: 'homeCtrl',
        controllerAs: 'home'
      })
      //答题页
      .state('test', {
        url: '/test',
        templateUrl: 'app/test/test.html',
        controller: 'testCtrl',
        controllerAs: 'test'
      })
      //摇奖页
      .state('result', {
        url: '/result',
        templateUrl: 'app/result/result.html',
        controller: 'resultCtrl',
        controllerAs: 'result'
      })
      ;

    $urlRouterProvider.otherwise('/');
  }

})();

(function () {
    'use strict';

    angular
        .module('chunkao')
        .run(runBlock);

    /** @ngInject */
    function runBlock($log, $rootScope, commService) {
        $rootScope.checkNow = checkNow;
        $rootScope.checkScore = checkScore;
        $rootScope.setConfig = setConfig;
        $rootScope.setConfig.IsHideTest = false;
        if (!$rootScope.setConfig.BgImgIndex) setConfig.BgImgIndex = 'http://open-files.comeoncloud.net/www/comeoncloud/jubit/image/20160323/13DFB3CA3254485883753A759767D697.jpg';
        if (!$rootScope.setConfig.BgImgAnswer) setConfig.BgImgAnswer = 'http://open-files.comeoncloud.net/www/comeoncloud/jubit/image/20160323/5E976A58362D40A4B0512678E4650EA8.jpg';
        if (!$rootScope.setConfig.IsLogin) {
            alert("您还没有登录！");
            return;
        }
        if ($rootScope.setConfig.StartDate) {
            var sDateStr = $rootScope.setConfig.StartDate.replace("T", " ");
            $rootScope.setConfig.StartDate = new Date(sDateStr);
        }
        if ($rootScope.setConfig.EndDate) {
            var eDateStr = $rootScope.setConfig.EndDate.replace("T", " ");
            $rootScope.setConfig.EndDate = new Date(eDateStr);
        }
        if(!$rootScope.checkNow()) return;
        if(!$rootScope.checkScore()) return;

        //检查是否已下载题库，没有下载则重新下载获取
        //if(!$rootScope.QuestionnaireSet){
        //    var questionnaireSetId = GetParm('id');
        //    if(questionnaireSetId){
        //        commService.get(
        //            commService.baseData.GetQuestionSetUrl,
        //            {
        //                id:questionnaireSetId
        //            },
        //            function(data){
        //                $log.debug(data);
        //                //if(data.status){
        //                //
        //                //}
        //
        //                $rootScope.QuestionnaireSet = data.result;
        //
        //
        //                //wx.ready(function () {
        //
        //                wxapi.wxshare({
        //                    title: data.result.title,
        //                    desc: data.result.describe,
        //                    link: 'http://' + window.location.host + '/customize/dati/index.aspx' + window.location.search,
        //                    imgUrl: data.result.img
        //                });
        //
        //                //});
        //                document.title = data.result.title;
        //
        //            },function(data){
        //
        //            }
        //        );
        //    }
        //}
        function checkNow(){
            if ($rootScope.setConfig.StartDate && $rootScope.setConfig.StartDate > new Date()) {
                alert("答题活动还未开始！");
                $rootScope.setConfig.IsHideTest = true;
                return false;
            }
            if ($rootScope.setConfig.StartDate && $rootScope.setConfig.EndDate < new Date()) {
                alert("答题活动已结束！");
                $rootScope.setConfig.IsHideTest = true;
                return false;
            }
            return true;
        }
        function checkScore(){
            if($rootScope.setConfig.UserScoreNum && $rootScope.setConfig.UserScoreNum>= $rootScope.setConfig.ScoreNum && $rootScope.setConfig.ScoreNum>0){
                alert("注意：答题活动仅能获得"+$rootScope.setConfig.ScoreNum+"次积分，您已全部获得，所有不能继续进行答题！");
                $rootScope.setConfig.IsHideTest = true;
                return false;
            }
            return true;
        }

        window.onpopstate = function (e) {
            // 获得存储在该历史记录点的json对象
            var json = window.history.state;
            console.log(e)
            // 点击一次回退到：http://www.qingdou.me/index.html
            // 获得的json为null
            // 再点击一次前进到：http://www.qingdou.me/post-1.html
            // 获得json为{time:1369647895656}
        }
        history.onpushstate = function (out, args) {
            console.log(out, args)
        }
        // if( ('onhashchange' in window) && ((typeof document.documentMode==='undefined') || document.documentMode==8)) {
        // 	// 浏览器支持onhashchange事件
        window.onhashchange = hashChangeFire;  // TODO，对应新的hash执行的操作函数
        // } else {
        // 	// 不支持则用定时器检测的办法
        // 	setInterval(function() {
        // 		var ischanged = isHashChanged();  // TODO，检测hash值或其中某一段是否更改的函数
        // 		if(ischanged) {
        // 		hashChangeFire();  // TODO，对应新的hash执行的操作函数
        // 		}
        // 	}, 150);
        // }
        function hashChangeFire(e) {
            console.log(e)
            console.log(history.length)
        }
    }

})();

angular
  .module('chunkao').factory("commService", ['$http', function ($http) {

    //var baseDomin='';
    var baseDomin='http://localhost:28241';
    var commService = {
      baseData: {
        GetQuestionUrl: baseDomin + "/serv/api/Question/QuestionGetHandler.ashx",
        PostQuestionUrl:baseDomin +  "/serv/api/Question/Post.ashx",
        GetClickAwardUrl: baseDomin +  "/serv/AWARDAPI.ashx",
        PostUserInfoUrl: baseDomin +  "/serv/pubapi.ashx",
        GetQuestionSetUrl:baseDomin + "/Serv/Api/Question/GetBySet.ashx"
      }
    };

    //model参数组合
    commService.extend = function (reqData, option) {
      var keys = Object.keys(option);
      for (var i = 0; i < keys.length; i++) {
        if (option[keys[i]]) {
          reqData[keys[i]] = option[keys[i]];
        }
        else if (option[keys[i]] == "") {
          reqData[keys[i]] = option[keys[i]];
        }
      }
    };

    //get参数转换url
    commService.getParams = function (url, jsonData) {
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
    //Get 异步获取数据
    commService.get = function (url, option, callBack, failCallBack) {
      $http.get(commService.urlParams(url, option)).success(function (data) {
        callBack(data);
      }).error(function (data) {
        failCallBack(data);
      });
    };

    //post因angulajs http post有点小问题需要转换post的data
    commService.postParams = function (jsonData) {
      var result = "";
      var i = 0;
      var keys = Object.keys(jsonData);
      for (var i = 0; i < keys.length; i++) {
        if (i != 0) {
          result += "&";
        }
        result += keys[i] + '=' + jsonData[keys[i]];
      }
      return result;
    };
    commService.get = function (url, option, callBack, failCallBack) {
      $http({
        method: 'GET',
        url: url,
        params: option
      }).success(function (data) {
        callBack(data)
      }).error(function (data) {
        failCallBack(data)
      });
    };
    commService.post = function (url, option, callBack, failCallBack) {
      $http({
        method: 'POST',
        url: url,
        data: commService.postParams(option),
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
        }
      }).success(function (data) {
        callBack(data)
      }).error(function (data) {
        failCallBack(data)
      });
    };

    commService.jsonp = function (url, option, callBack, failCallBack) {
      $http({
        method: 'jsonp',
        url: url,
        params: option
      }).success(function (data) {
        callBack(data)
      }).error(function (data) {
        failCallBack(data)
      });
    };
    return commService;
  }]);

/**
 * Created by 忠鸿 on 2016/1/26.
 */

window.alert = function(msg){
    layer.open({
        content: msg,
        time: 3
    });
};

function GetParm(parm) {
    //获取当前URL
    var local_url = window.location.search;

    //获取要取得的get参数位置
    var get = local_url.indexOf(parm + "=");
    if (get == -1) {
        return "";
    }
    //截取字符串
    var get_par = local_url.slice(parm.length + get + 1);
    //判断截取后的字符串是否还有其他get参数
    var nextPar = get_par.indexOf("&");
    if (nextPar != -1) {
        get_par = get_par.slice(0, nextPar);
    }
    return get_par;
}
//获取参数

(function () {
    'use strict';

    angular
        .module('chunkao')
        .controller('homeCtrl', homeCtrl);

    /** 首页 */
    function homeCtrl($rootScope, $state, $scope, commService) {
        var pageData = $scope.pageData = {};
        var pageFunc = $scope.pageFunc = {};

        pageFunc.go = function (route) { //利用路由进行跳转
            if ($rootScope.setConfig.AutoID > 0) {
                if (!$rootScope.checkNow()) return;
                if (!$rootScope.checkScore()) return;
                //加载层
                var nlay = layer.open({type: 2});

                //下载题库
                if(!$rootScope.QuestionnaireSet || $rootScope.QuestionnaireSet.IsPostAnswer){
                    commService.get(
                        commService.baseData.GetQuestionSetUrl,
                        {id: $rootScope.setConfig.AutoID},
                        function (data) {
                            layer.close(nlay);
                            if(!data.status){
                                alert(data.msg);
                            }
                            else{
                                $rootScope.QuestionnaireSet = data.result;
                                $rootScope.QuestionnaireSet.IsPostAnswer = false;
                                $state.go(route);
                            }
                        }, function (data) {
                            layer.close(nlay);
                        });
                }
                else{
                    layer.close(nlay);
                    $state.go(route);
                }
            }
        };
    }
})();

(function () {
  'use strict';

  angular
    .module('chunkao')
    .controller('resultCtrl', resultCtrl);

  /** 首页 */
  function resultCtrl($state, $scope, commService, $localStorage,$rootScope) {
    var pageData = $scope.pageData = {
      Score: 0
    };

    //更新之答题积分
    if (typeof $localStorage.Score!=="undefined") {
      pageData.Score =$localStorage.Score;
      pageData.canAward = $localStorage.canAward;
    }

    //利用路由跳转
    var pageFunc = $scope.pageFunc = {};
    pageFunc.go = function (route) {
      $state.go(route);
    };

    //再次测试
    pageFunc.testAgain = function () {
      if ($rootScope.setConfig.AutoID > 0) {
        if (!$rootScope.checkNow()) return;
        if (!$rootScope.checkScore()) return;
        //加载层
        var nlay = layer.open({type: 2});
        //下载题库
        if(!$rootScope.QuestionnaireSet || $rootScope.QuestionnaireSet.IsPostAnswer){
          commService.get(
              commService.baseData.GetQuestionSetUrl,
              {id: $rootScope.setConfig.AutoID},
              function (data) {
                layer.close(nlay);
                if(!data.status){
                  alert(data.msg);
                }
                else{
                  $rootScope.QuestionnaireSet = data.result;
                  $rootScope.QuestionnaireSet.IsPostAnswer = false;
                  $state.go("test");
                }
              }, function (data) {
                layer.close(nlay);
              });
        }
        else{
          layer.close(nlay);
          $state.go(route);
        }
      }
    };

    pageFunc.goTo = function (url) {  //利用原生JS操作地址栏进行跳转
      window.location.href = url;
    };

      pageFunc.showShare = function(){
          $("#sharebg,#sharebox").show();
          $("#sharebox").css({"top": $(window).scrollTop()})
      };

      pageFunc.hideShare = function(){
          $("#sharebg,#sharebox").hide();
      };
  }
})();


(function () {
  'use strict';

  angular
    .module('chunkao')
    .controller('testCtrl', testCtrl);

  /** 答题页 */
  function testCtrl($state, $scope, commService, $localStorage,$rootScope) {

    //调用接口取到题目数据
    var pageData = $scope.pageData = {
      info: {},
      questions: [],
      nextText: "下一题",
      QuestionnaireID: "557244",
      show: false,
      backText: "返回"
    };
    var pageFunc = $scope.pageFunc = {};

    pageFunc.init = function(){
      if($rootScope.QuestionnaireSet){
          pageData.show = true;
          pageData.questions = $rootScope.QuestionnaireSet.questions;

          console.log(pageData.questions);
          for (var i = 0; i < pageData.questions.length; i++) { //从返回信息中复制题目编号与题目标题
              pageData.questions[i].Num = i + 1;
              pageData.questions[i].Answer = "";
              pageData.questions[i].Answer1 = "";
          }
          pageFunc.ToNextQuestion();
      }

    };

    pageFunc.ToNextQuestion = function () { //下一题按钮功能
      for (var i = 0; i < pageData.questions.length; i++) {
        if (pageData.questions[i].Answer1 == "") {
          if (i == pageData.questions.length - 1) pageData.nextText = "完成"; //最后一道题目时，修改下一题按钮为完成
          pageData.info = pageData.questions[i];
            console.log(pageData.info);
          break;
        }
      }
    };
    pageFunc.GetPin = function (index) { //为题目增加英文字母的编号
      if (index == 0) {
        return "A.";
      }
      else if (index == 1) {
        return "B.";
      }
      else if (index == 2) {
        return "C.";
      }
      else if (index == 3) {
        return "D.";
      }
      else if (index == 4) {
        return "E.";
      }
      else if (index == 5) {
        return "F.";
      }
    };

    pageFunc.Select = function (item) {
      pageData.info.Answer = item.option_id;
    };

    pageFunc.Submit = function () { //本地记录答案功能
      if (pageData.info.Answer == "") {
        alert("请选择答案");
        return;
      }
      pageData.info.Answer1 = pageData.info.Answer;
      if (pageData.info.Num != pageData.questions.length) {
        pageFunc.ToNextQuestion();
        pageData.backText = "上一题";
        return;
      }

      var PostDataList = [];
      for (var i = 0; i < pageData.questions.length; i++) {
        PostDataList.push({
            question_id: pageData.questions[i].question_id,
            option_ids: pageData.questions[i].Answer}
        );
      }

      var model = { //将发送给服务器的信息转化成JSON格式
        data:{
            set_id:$rootScope.QuestionnaireSet.id,
            answer_list:PostDataList
        }
      };

      commService.post(  //将本地记录的答案与题目ID返回给服务端，从而判断对错
        commService.baseData.PostQuestionUrl,
          {data:angular.toJson(model.data)},

        function (response) {
          if (response.status) {
            $rootScope.QuestionnaireSet.IsPostAnswer = true;
            $localStorage.Score = response.result.correct_count * $rootScope.setConfig.QuestionScore;//Math.round(response.result * (100 /pageData.questions.length)) ;
            if(response.result.have_score) {
              $rootScope.setConfig.UserScoreNum ++;
              if($rootScope.setConfig.UserScoreNum && $rootScope.setConfig.UserScoreNum>= $rootScope.setConfig.ScoreNum && $rootScope.setConfig.ScoreNum>0){
                $rootScope.setConfig.IsHideTest = true;
              }
            }
            //如果分数达到60分及以上，允许抽奖标志置ture，否则不允许抽奖
            if (response.result.correct_count >= $rootScope.setConfig.WinCount) {
              $localStorage.canAward = true;
            } else {
              $localStorage.canAward = false;
            }
            $state.go("result");
          }
          else{
            alert(response.msg);
          }
        }, function (response) {
          console.log(response);
        }
      );
    };

    //返回上一页
    pageFunc.goback = function () {
      //history.go(-1);
      var zIndex = 0;
      for (var i = 0; i < pageData.questions.length; i++) {
        if (pageData.questions[i].QuestionID == pageData.info.QuestionID) {
          zIndex = i - 1;
          break;
        }
      }
      if (zIndex == -1) {
        $state.go("home");
      }
      else {
        pageData.questions[zIndex].Answer1 = "";
        pageFunc.ToNextQuestion();
        console.log(pageData.info.Num);
        if (pageData.info.Num > 1) {
          pageData.backText = "上一题";
        } else if (pageData.info.Num == 1) {
          pageData.backText = "返回";
        }
      }
    };


      pageFunc.init();

  }
})();

(function() {
  'use strict';

  angular
    .module('chunkao')
    .factory('githubContributor', githubContributor);

  /** @ngInject */
  function githubContributor($log, $http) {
    var apiHost = 'https://api.github.com/repos/Swiip/generator-gulp-angular';

    var service = {
      apiHost: apiHost,
      getContributors: getContributors
    };

    return service;

    function getContributors(limit) {
      if (!limit) {
        limit = 30;
      }

      return $http.get(apiHost + '/contributors?per_page=' + limit)
        .then(getContributorsComplete)
        .catch(getContributorsFailed);

      function getContributorsComplete(response) {
        return response.data;
      }

      function getContributorsFailed(error) {
        $log.error('XHR Failed for getContributors.\n' + angular.toJson(error.data, true));
      }
    }
  }
})();

(function() {
  'use strict';

  angular
    .module('chunkao')
    .directive('acmeMalarkey', acmeMalarkey);

  /** @ngInject */
  function acmeMalarkey(malarkey) {
    var directive = {
      restrict: 'E',
      scope: {
        extraValues: '=',
      },
      template: '&nbsp;',
      link: linkFunc,
      controller: MalarkeyController,
      controllerAs: 'vm'
    };

    return directive;

    function linkFunc(scope, el, attr, vm) {
      var watcher;
      var typist = malarkey(el[0], {
        typeSpeed: 40,
        deleteSpeed: 40,
        pauseDelay: 800,
        loop: true,
        postfix: ' '
      });

      el.addClass('acme-malarkey');

      angular.forEach(scope.extraValues, function(value) {
        typist.type(value).pause().delete();
      });

      watcher = scope.$watch('vm.contributors', function() {
        angular.forEach(vm.contributors, function(contributor) {
          typist.type(contributor.login).pause().delete();
        });
      });

      scope.$on('$destroy', function () {
        watcher();
      });
    }

    /** @ngInject */
    function MalarkeyController($log, githubContributor) {
      var vm = this;

      vm.contributors = [];

      activate();

      function activate() {
        return getContributors().then(function() {
          $log.info('Activated Contributors View');
        });
      }

      function getContributors() {
        return githubContributor.getContributors(10).then(function(data) {
          vm.contributors = data;

          return vm.contributors;
        });
      }
    }

  }

})();

(function() {
  'use strict';

  angular
    .module('chunkao')
    .directive('acmeNavbar', acmeNavbar);

  /** @ngInject */
  function acmeNavbar() {
    var directive = {
      restrict: 'E',
      templateUrl: 'app/components/navbar/navbar.html',
      scope: {
          creationDate: '='
      },
      controller: NavbarController,
      controllerAs: 'vm',
      bindToController: true
    };

    return directive;

    /** @ngInject */
    function NavbarController(moment) {
      var vm = this;

      // "vm.creation" is avaible by directive option "bindToController: true"
      vm.relativeDate = moment(vm.creationDate).fromNow();
    }
  }

})();

(function() {
  'use strict';

  angular
      .module('chunkao')
      .service('webDevTec', webDevTec);

  /** @ngInject */
  function webDevTec() {
    var data = [
      {
        'title': 'AngularJS',
        'url': 'https://angularjs.org/',
        'description': 'HTML enhanced for web apps!',
        'logo': 'angular.png'
      },
      {
        'title': 'BrowserSync',
        'url': 'http://browsersync.io/',
        'description': 'Time-saving synchronised browser testing.',
        'logo': 'browsersync.png'
      },
      {
        'title': 'GulpJS',
        'url': 'http://gulpjs.com/',
        'description': 'The streaming build system.',
        'logo': 'gulp.png'
      },
      {
        'title': 'Jasmine',
        'url': 'http://jasmine.github.io/',
        'description': 'Behavior-Driven JavaScript.',
        'logo': 'jasmine.png'
      },
      {
        'title': 'Karma',
        'url': 'http://karma-runner.github.io/',
        'description': 'Spectacular Test Runner for JavaScript.',
        'logo': 'karma.png'
      },
      {
        'title': 'Protractor',
        'url': 'https://github.com/angular/protractor',
        'description': 'End to end test framework for AngularJS applications built on top of WebDriverJS.',
        'logo': 'protractor.png'
      },
      {
        'title': 'Bootstrap',
        'url': 'http://getbootstrap.com/',
        'description': 'Bootstrap is the most popular HTML, CSS, and JS framework for developing responsive, mobile first projects on the web.',
        'logo': 'bootstrap.png'
      },
      {
        'title': 'Sass (Node)',
        'url': 'https://github.com/sass/node-sass',
        'description': 'Node.js binding to libsass, the C version of the popular stylesheet preprocessor, Sass.',
        'logo': 'node-sass.png'
      }
    ];

    this.getTec = getTec;

    function getTec() {
      return data;
    }
  }

})();
