ubimodule.directive('articlelist', function () {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'directives/articlelist/tpls/index.html',
        replace: true,
        scope: {
            hasimg: '@',
            hassearch: '@',//1表示有搜索
            page: '@', //1:政策法规，2:新闻资讯，3:案例分析.
            otherCateId: '@',//从其他项目传过来的cateId
            hasTag: '@', //是否要有标签
            hassummary: '@', //是否显示summary
            hasleftslide:'@',//是否显示左边侧边栏
            modulename: '@', //regulation:政策法规，new s:新闻资讯，case:案例分析.
            haschoose: '@',//是否显示按地区或者标签筛选，案例分析中有
            author: '@',//根据用户获取相关用户信息
            tag:'@',//选中标签
        },
        controller: function ($scope, $element, $attrs, $sessionStorage, commArticle, ngDialog, commService) {
            var pageListFunc = $scope.pageListFunc = {};
            var pageListData = $scope.pageListData = $sessionStorage.pageListData;
            pageListData = null;
            var noStorage = !pageListData || pageListData.page != $scope.page;
            if (noStorage) {
                pageListData = $scope.pageListData = $sessionStorage.pageListData = {
                    page: $scope.page,
                    hasImg: $scope.hasimg,
                    hasSearch: $scope.hassearch,
                    hasTag: $scope.hasTag,
                    otherCateId: $scope.otherCateId,
                    hassummary: $scope.hassummary,
                    hasleftslide:$scope.hasleftslide,
                    author: $scope.author,
                    tag:$scope.tag,
                    keyword: "",
                    pageIndex: 1,
                    pageSize: 5,
                    totalCount: 0,
                    dataList: [],
                    routeType: "",
                    modulename: $scope.modulename,
                    haschoose: $scope.haschoose,

                    province: null,//当前选中省份
                    provinceList: [],//所有省份列表
                    tagList: [],//标签列表
                    currTag: [],//当前选中标签
                    cateId: [baseData.moduleCateIds.regulations, baseData.moduleCateIds.news, baseData.moduleCateIds.case, $scope.otherCateId],
                    isHideTagChoose:false,

                    toggleTag: [{
                        id: '',
                        name: "全部"
                    }],
                    toggleObj:{
                        type: "Article",
                        cateId: $scope.otherCateId,
                        pageIndex: 1,
                        pageSize: 3,
                        totalCount: 0,
                        selectCate: null,
                    }
                };
            }
            pageListFunc.loadData = function (isNew) {
                var pageii = layer.open({
                    type: 2
                });
                var tabIndex = $scope.page;
               // var cateId = [baseData.moduleCateIds.regulations, baseData.moduleCateIds.news, baseData.moduleCateIds.case];
                var variCateId="";
                if(pageListData.toggleObj.selectCate!=null)
                {
                    if(pageListData.toggleObj.selectCate.id)
                    {
                        variCateId=pageListData.toggleObj.selectCate.id;
                    }
                }
                else {
                    variCateId=pageListData.cateId[pageListData.page];
                }
                var type = ["Article", "Article", "Article"];
                var orderby = [null, null, null];
                if (isNew) {
                    pageListData.dataList = [];
                    pageListData.pageIndex = 1;
                }
                else {
                    pageListData.pageIndex++;
                }
                var model = {
                    cateId: variCateId,
                    type: type[tabIndex],
                    keyword: pageListData.keyword,
                    author: pageListData.author?pageListData.author:'',
                    tags:pageListData.tag?pageListData.tag:'',
                    pageIndex: pageListData.pageIndex,
                    pageSize: pageListData.pageSize,
                    orderby: orderby[tabIndex]
                };
                commArticle.getArticleListByOption(model, function (data) {
                    pageListData.totalCount = data.totalcount;
                    //当为政策法规时，筛选弹出框隐藏标签
                    if (pageListData.modulename == "regulations")
                    {
                        pageListData.isHideTagChoose = true;
                    }

                    for (var i = 0; i < data.list.length; i++) {
                        pageListData.dataList.push(data.list[i]);
                    }
                    console.log(pageListData.dataList);
                    layer.close(pageii);
                }, function (data) {
                    layer.close(pageii);
                });
            };
            pageListFunc.loadMore = function () {
                pageListFunc.loadData(false);
            }
            pageListFunc.search = function () {
                pageListFunc.loadData(true);
            }
            pageListFunc.init = function () {
                pageListFunc.loadCateList();
                pageListFunc.loadData(true);
            }
            pageListFunc.go = function (id) {
                var url="";
                if(pageListData.hassummary!='1')
                {
                    url="/haima#/onlineClassDetail/"+id;
                }
                else if(pageListData.hassummary==1)
                {
                    url = "#/" + pageListData.modulename + "/" + id;
                }
                window.location.href = url;
            }

            //点击筛选弹出标签和地区选择
            pageListFunc.showChooseDialog = function () {
                ngDialog.open({
                    template: basePath + 'modules/directives/articlelist/tpls/chooseDialog.html',
                    plain: false,
                    scope: $scope
                });
                pageListFunc.selectProvince();
                pageListFunc.getTags();
            }
            //获取省份
            pageListFunc.selectProvince = function () {
                var ProChoose = { id: "0", name: "请选择省份" };
                //获取省份
                commService.getGetKeyVauleDatas({
                    type: 'province'
                }, function (data) {
                    pageListData.provinceList = data.list;
                    pageListData.provinceList.unshift(ProChoose);
                    pageListData.province = pageListData.provinceList[0];
                    console.log(data);
                }, function () { });
            };
            //获取标签
            pageListFunc.getTags = function () {
                commArticle.getTags(function (data) {
                    if (data) {
                        //for (var i = 0; i < data.length; i++) {
                        //    data[i].checked = false;
                        //}
                        pageListData.tagList = data;
                    }

                }, function (argument) {

                });
                console.log(pageListData.tagList);
            };
            ////单个标签点击
            //pageListFunc.tagClick = function (item) {
            //    pageListData.currTag = item;
            //}
            //筛选框点击确定
            pageListFunc.confirm = function () {
                var currTag = '';
                if (pageListData.currTag&& pageListData.currTag.length!=0) {
                    currTag = pageListData.currTag.tag;
                }

                commArticle.getArticleListByOption({
                    cateId: pageListData.cateId[pageListData.page],
                    keyword: '',
                    tags: currTag,
                    province: pageListData.province && (pageListData.province.id != 0) ? pageListData.province.id : '',
                    orderby: '',
                    isGetNoCommentData: 0,
                    isHasCommentAndReplayCount: 0,
                    pageIndex: pageListData.pageIndex,
                    pageSize: pageListData.pageSize,
                    type: 'Article'
                }, function (data) {
                    console.log(data);
                    pageListData.dataList = data.list;
                    pageListData.totalCount = data.totalcount;
                    pageListData.currTag = [];
                    pageListData.province = null;
                }, function (data) {

                });
                ngDialog.closeAll();
            }
            //点击标签时跳转的页面
            pageListFunc.goList = function (tag) {
                var url = "";
                switch (pageListData.cateId[pageListData.page]) {
                    case 79:
                        url = "#/caselist/" + tag;
                        break;
                    case 83:
                        url = "#/newlist/" + tag;
                        break;
                    case 84:
                        url = "#/regulationslist/" + tag;
                        break;
                    default:
                        break;
                }
                window.location.href = url;
            }

            //侧边栏
            pageListFunc.toggleMenuClick = function () {
                var menuLeft = document.getElementById('cbp-spmenu-s1'),
                    body = document.body;
                classie.toggle(body, 'cbp-spmenu-push-toright');
                classie.toggle(menuLeft, 'cbp-spmenu-open');
            };

            pageListFunc.leftMenuClick = function(item) {
                pageListFunc.selectCateList(item);
                pageListFunc.toggleMenuClick();
            };

            pageListFunc.loadCateList = function() {
                commArticle.getArticleCateList(pageListData.toggleObj.type, pageListData.toggleObj.cateId, 1, 100,
                    function(data) {
                        for (var i = 0; i < data.list.length; i++) {
                            pageListData.toggleTag.push({
                                id: data.list[i].id,
                                name: data.list[i].name
                            });
                        }
                    },
                    function(data) {

                    });
            };

            pageListFunc.selectCateList = function(data) {
                pageListData.toggleObj.pageIndex = 1;
                //pageListData.toggleObj.artitle = [];
                pageListData.toggleObj.selectCate = data;
                pageListFunc.loadData(true);
            };

            if (noStorage) pageListFunc.init();
        }
    };
});