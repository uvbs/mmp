ubimodule.directive('asklist', function() {
	return {
		restrict: 'ECMA',
		templateUrl: baseViewPath + 'directives/asklist/tpls/index.html',
		replace: true,
		scope: {
			userid: '@',
			hassearch: '@',// 1表示有搜索
			hasrow: '@',//1表示有三个标签栏
			author: '@',//根据用户获取相关用户信息,用户id
            type:'@',//true表示提问，否为文章
			tag: '@',//选中标签
		},
		controller: function ($scope, $element, $attrs, commArticle, commService) {
			var pageFunc = $scope.pageFunc = {};
			var pageData = $scope.pageData = {
			    hasSearch: $scope.hassearch,
			    hasrow: $scope.hasrow,
			    author: $scope.author,
                type:$scope.type,
			    tag: $scope.tag,
			    currTabIndex: 0,
			    searchKeyword:'',//搜索关键字
			    questCate: baseData.moduleCateIds.askquestion, //问答-问题
			    articleCate: baseData.moduleCateIds.askarticle, //问答-文章

			    tabs: [{
			        name: '最新动态',
			        list: [],
			        pageSize: 5,
			        pageIndex: 1,
			        totalCount: 0,
			        checked: true,//表示最新动态为选中状态
			        cate: baseData.moduleCateIds.askquestion + ',' + baseData.moduleCateIds.askarticle,
			        isGetNoCommentData: 0,
			        isHasCommentAndReplayCount: 0,
			      //  author:'',
			        orderby: ''
			    }, {
			        name: '热门动态',
			        list: [],
			        pageSize: 5,
			        pageIndex: 1,
			        totalCount: 0,
			        checked: false,
			        cate: baseData.moduleCateIds.askquestion + ',' + baseData.moduleCateIds.askarticle,
			        isGetNoCommentData: 0,
			        isHasCommentAndReplayCount: 1,
			        //author: '',
			        orderby: 'comment'
			    }, {
			        name: '未回答问题',
			        list: [],
			        pageSize: 5,
			        pageIndex: 1,
			        totalCount: 0,
			        checked: false,
			        cate: baseData.moduleCateIds.askquestion,
			        isGetNoCommentData: 1,
			        isHasCommentAndReplayCount: 0,
			       // author: '',
			        orderby: ''
			    },
			    {
			        name: '我的问题',
			        list: [],
			        pageSize: 5,
			        pageIndex: 1,
			        totalCount: 0,
			        checked: false,
			        cate: baseData.moduleCateIds.askquestion,
			        isGetNoCommentData: 0,
			        isHasCommentAndReplayCount: 0,
			       // author: pageData.author,
			        orderby: ''
			    }, {
			        name: '我的文章',
			        list: [],
			        pageSize: 5,
			        pageIndex: 1,
			        totalCount: 0,
			        checked: false,
			        cate: baseData.moduleCateIds.askarticle,
			        isGetNoCommentData: 0,
			        isHasCommentAndReplayCount: 0,
			       // author: pageData.author,
			        orderby: ''
			    }],

			    currViewData: {},

			    currUser: commService.getCurrUserInfo()

			};
            //跳转到详情
			pageFunc.go = function (item) {			   
			    var url = "";
			    if (!item.isQuestion) {
			       // url = "#/askArticleDetail/" + item.id;
			        url = "#/ask/" + item.id;
			    } else {
			        url = "#/ask/"+ item.id;
			    }
			    window.location.href = url;
			}
            //点击标签
			pageFunc.tabClick = function (item) {
			    commService.setArrValue(pageData.tabs, 'checked', false);//默认为选中最新动态，这里是设置三个标签都不选中
			    item.checked = true;//为当前选中的页签
			    pageData.currViewData = item;//将当前页签对象寄存在currViewData中
			    pageFunc.loadData(pageData.currViewData);
			};
		    //加载数据
			pageFunc.loadData = function (item) {
			    commArticle.getArticleListByOption({
			        pageindex: item.pageIndex,
			        pagesize: item.pageSize,
			        cateid: item.cate,
			        keyword:pageData.searchKeyword,
			        author: pageData.author?pageData.author:'',
			        tags: pageData.tag ? pageData.tag : '',
			        isGetNoCommentData: item.isGetNoCommentData,
			        isHasCommentAndReplayCount: item.isHasCommentAndReplayCount,
			        orderby: item.orderby

			    }, function (data) {
			        console.log(item.name);
			        console.log(data);

			        if (data && data.list) {
			            for (var i = 0; i < data.list.length; i++) {
			                if (data.list[i].categoryId == baseData.moduleCateIds.askquestion) {
			                    data.list[i].isQuestion = true;
			                } else {
			                    data.list[i].isQuestion = false;
			                }
			            };

			            commService.pushArrFilterRepeat(data.list, item.list, 'id');
			        };

			        item.totalCount = data.totalcount;
			        item.pageIndex++;
			    }, function (data) {

			    });
			};
            //搜索
			pageFunc.search = function () {
			    if(pageData.tabs[0].pageIndex!=1)
			    {
			        pageData.tabs[0].pageIndex = 1;
			    }
			    else {
			        pageData.currViewData.list = [];
			        pageFunc.loadData(pageData.currViewData);
			    }
			}
		    //点击标签时跳转的页面
			pageFunc.goList = function (tag) {
			    var url = "#/AskList/" + tag;
			    //var url = "";
			    //switch (pageData.article.categoryId) {
			    //    case "79":
			    //        url = "#/caselist/" + tag;
			    //        break;
			    //    case "83":
			    //        url = "#/newlist/" + tag;
			    //        break;
			    //    case "84":
			    //        url = "#/regulationslist/" + tag;
			    //        break;
			    //    case "81":
			    //        url = "#/AskList/" + tag;
			    //        break;
			    //    case "82":
			    //        url = "#/AskList/" + tag;
			    //        break;
			    //    default:
			    //        break;
			    //}
			    window.location.href = url;
			}
            //初始化
			pageFunc.init = function () {
			    $scope.$on('loginStatusChangeNotice', function (event, msg) {
			        pageData.currUser = commService.getCurrUserInfo();
			    });

			    for (var i = 0; i < pageData.tabs.length; i++) {
			        pageFunc.loadData(pageData.tabs[i]);
			    };
			    if (pageData.author != undefined)
			    {
			        if (pageData.type == "true")
			        {
			            pageData.currViewData = pageData.tabs[3];
			        } else {
			            pageData.currViewData = pageData.tabs[4];
			        }
			    }
			    else
			    {
			        pageData.currViewData = pageData.tabs[0];
			    }
			};
            ////获取我的问题
			//pageFunc.loadQuestionData = function () {
			//    commArticle.getArticleListByOption({
			//        cateId: baseData.moduleCateIds.askquestion,
			//        pageIndex: pageData.pageIndexQ,
			//        pageSize: pageData.pageSizeQ,
			//        type: 'Question',
			//        author: pageData.author
			//    }, function (data) {
			//        pageData.totalCountQ = data.totalcount;
			//        pageData.questionList.data = data.list;
			//    },
            //    function () {
            //    });
			//}
            ////获取我的文章
			//pageFunc.loadArticleData = function () {
			//    commArticle.getArticleListByOption({
			//        cateId: baseData.moduleCateIds.askarticle,
			//        pageIndex: pageData.pageIndexA,
			//        pageSize: pageData.pageSizeA,
			//        type: 'Article',
			//        author: pageData.author
			//    }, function (data) {
			//        pageData.totalCountA = data.totalcount;
			//        pageData.articleList.data = data.list;		        
			//    },
            //    function () {
            //    });
			//}
			pageFunc.init();
		}
	};
});