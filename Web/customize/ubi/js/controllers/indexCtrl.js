ubimodule.controller("indexCtrl", ['$scope', '$routeParams', 'commService', 'commArticle', 'userService', '$interval',
    function ($scope, $routeParams, commService, commArticle, userService, $interval) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '用户空间 - ' + baseData.slogan,
            userRole: '用户',
            id: $routeParams.id,
            userInfo: [],
            currUser: commService.getCurrUserInfo(),
            dailyCase: [],
            regulations: [],
            news: [],
            cases: [],
            askarticleCateId: baseData.moduleCateIds.askarticle,
            askquestionCateId: baseData.moduleCateIds.askquestion,

            hotquestions: {
                pageIndex: 1,
                pageSize: 10,
                pageShowCount: 30,
                totalcount: 0,
                data: [],
                list: []
            },
            hotarticles: {
                pageIndex: 1,
                pageSize: 6,
               // pageShowCount: 30,
                totalcount: 0,
               // data: [],
                list: []
            },
            statuses: [],
            newquestions: [],
            newusers: [],
            opens: [],

            adImgs: []
        };

        pageFunc.init = function () {
            pageFunc.loadDailyCase();
            pageFunc.loadRegulations();
            pageFunc.loadNews();
            pageFunc.loadCases();
            pageFunc.loadHotQuestions();
            pageFunc.loadHotArticles();
            pageFunc.loadStatuses();
            pageFunc.loadNewQuestions();
            pageFunc.loadNewUsers();
            pageFunc.loadOpens();
            pageFunc.getPageIndexImg();

            //pageFunc.autoChange(1);

        };
        ///每日案例
        pageFunc.loadDailyCase = function () {
            commArticle.getDailyCase(
                function (data) {
                    pageData.dailyCase = data;
                },
                function () {
                });
        };
        ///政策法规
        pageFunc.loadRegulations = function () {
            commArticle.getArticleList(function (data) {
                pageData.regulations = data.list;
            }, function (data) {

            }, 'Article', baseData.moduleCateIds.regulations, '', '', '', '', 1, 2, 0, 0);
        }
        ///新闻资讯
        pageFunc.loadNews = function () {
            commArticle.getArticleList(function (data) {
                pageData.news = data.list;
            }, function (data) {
            }, 'Article', baseData.moduleCateIds.news, '', '', '', '', 1, 5, 0, 0);
        };
        ///案例分析
        pageFunc.loadCases = function () {
            commArticle.getArticleListByOption({
                cateId: baseData.moduleCateIds.case,
                isGetNoCommentData: 0,
                isHasCommentAndReplayCount: 0,
                pageIndex: 1,
                pageSize: 5,
                type: 'Article'
            }, function (data) {
                pageData.cases = data.list;
            }, function (data) {

            });
        };
        //热门问题
        pageFunc.loadHotArticles=function(){
            commArticle.getArticleListByOption({
                cateId: baseData.moduleCateIds.askquestion,
                isGetNoCommentData: 0,
                isHasCommentAndReplayCount: 0,
                pageIndex: pageData.hotarticles.pageIndex,
                pageSize: pageData.hotarticles.pageSize,
                orderby: "comment"
            },function(data){
                pageData.hotarticles.totalcount = data.totalcount;
                pageData.hotarticles.list = data.list;
            }, function (data) {

            });
        };
        ///评论与原创
        pageFunc.loadHotQuestions = function () {
            commArticle.getArticleListByOption({
                cateId:baseData.moduleCateIds.askarticle,
                isGetNoCommentData: 0,
                isHasCommentAndReplayCount: 0,
                pageIndex: pageData.hotquestions.pageIndex,
                pageSize: pageData.hotquestions.pageShowCount,
                orderby: "comment"
            }, function (data) {
                pageData.hotquestions.totalcount = data.totalcount;
                pageData.hotquestions.data = data.list;
                pageData.hotquestions.list = pageData.hotquestions.data.slice((pageData.hotquestions.pageIndex - 1) * pageData.hotquestions.pageSize, pageData.hotquestions.pageSize * pageData.hotquestions.pageIndex);
                if (pageData.hotquestions.totalcount != 0)
                {
                    $interval(function () {
                        var tempIndex = pageData.hotquestions.pageIndex;
                        tempIndex++;
                        if (pageData.hotquestions.totalcount < 10 && pageData.hotquestions.totalcount > 0) {
                            if (tempIndex > 1) { tempIndex = 1; }
                        }
                        else if (pageData.hotquestions.totalcount > 10 && pageData.hotquestions.totalcount < 20) {
                            if (tempIndex > 2) { tempIndex = 1; }
                        }
                        else if (pageData.hotquestions.totalcount > 20) {
                            if (tempIndex > 3) { tempIndex = 1; }
                        }
                        pageFunc.selectHotquestion(tempIndex);
                    }, 9000, 0);
                }                
            }, function (data) {

            });

        };

        pageFunc.selectHotquestion = function (num) {
            if (pageData.hotquestions.pageIndex != num) {
                pageData.hotquestions.pageIndex = num;
                pageData.hotquestions.list = pageData.hotquestions.data.slice((pageData.hotquestions.pageIndex - 1) * pageData.hotquestions.pageSize, pageData.hotquestions.pageSize * pageData.hotquestions.pageIndex);
                //pageFunc.loadHotQuestions();
            }
        };

        ///社区动态
        pageFunc.loadStatuses = function () {
            commArticle.getArticleListByOption({
                isGetNoCommentData: 0,
                isHasCommentAndReplayCount: 0,
                pageIndex: 1,
                pageSize: 5,
                type: "Statuses"
            }, function (data) {
                pageData.statuses = data.list;
            }, function (data) {

            });
        };

        ///最新问题
        pageFunc.loadNewQuestions = function () {
            commArticle.getArticleListByOption({
                isGetNoCommentData: 0,
                isHasCommentAndReplayCount: 0,
                pageIndex: 1,
                pageSize: 10,
                type: "Question"
            }, function (data) {
                pageData.newquestions = data.list;
            }, function (data) {

            });
        };

        ///最新问题
        pageFunc.loadNewUsers = function () {
            userService.getNewUsers(function (data) {
                pageData.newusers = data;
            }, function (data) {
            });
        };

        ///公开课
        pageFunc.loadOpens = function () {
            commArticle.getArticleListByOption({
                pageIndex: 1,
                pageSize: 6,
                type: "OpenClass",
                orderby: 'comment'
            },
            function (data) {
                pageData.opens = data.list;
            },
            function () { }
            );
        };
        pageFunc.go=function(){
          window.location.href="#/update";
        };

        //获取首页图片
        pageFunc.getPageIndexImg = function () {
            userService.getAdList('1', '1', '5', function (data) {
                if (data && data.list) {
                    pageData.adImgs = data.list;
                }
            }, function () { });
        }
        pageFunc.init();
    }
]);
