wisdomlifemodule.directive('articlelist', function() {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'directives/articlelist/tpls/index.html',
        replace: true,
        scope: {
            hasLabel: '@',//列表中是否有标签 1表示有
            hasClassify: '@',//是否有侧边栏，传id过来，523 表示广告主  524 表示自媒体
            page: '@' //1:自媒体，2:广告主，
        },
        controller: function ($scope, $element, $attrs, commService) {
            var pageFunc = $scope.pageFunc = {};
            var pageData = $scope.pageData ;
            pageData = null;
           
            pageData = $scope.pageData = {
                hasLabel: $scope.hasLabel,
                hasClassify: $scope.hasClassify,
                page: $scope.page,
                keyword: "",
                pageIndex: 1,
                pageSize: 5,
                totalCount: 0,
                dataList: [],

                tag: [{
                    id: '',
                    name: "全部"
                }],
                type: "Article",
                cateId: '',
                currCateId: ''

            };
            
            pageFunc.toggleMenuClick = function () {
                var menuLeft = document.getElementById('cbp-spmenu-s1'),
                    body = document.body;
                classie.toggle(body, 'cbp-spmenu-push-toright');
                classie.toggle(menuLeft, 'cbp-spmenu-open');
            };
            pageFunc.leftMenuClick = function (item) {
                // pageFunc.selectCateList(item);
                pageFunc.loadData(true,item.id);
                pageFunc.toggleMenuClick();
            };
            //获取分类查询
            pageFunc.loadCateList = function () {
                if(pageData.page==1)
                {
                    pageData.cateId=baseData.moduleCateIds.selfmedia;
                }
                else if(pageData.page==2)
                {
                    pageData.cateId=baseData.moduleCateIds.advertiser;
                } else if (pageData.page == 3) {
                    pageData.cateId = baseData.moduleCateIds.case;
                } else {
                    pageData.cateId = pageData.page;
                }
                commService.getArticleCateList(pageData.type, pageData.cateId, 1, 100,
                    function (data) {
                        for (var i = 0; i < data.list.length; i++) {
                            if (data.list[i].id != pageData.cateId)
                            {
                                pageData.tag.push({
                                    id: data.list[i].id,
                                    name: data.list[i].name
                                });
                            }

                        }
                    },
                    function (data) {

                    });
            };
            //跳转到详情
            pageFunc.go = function (id) {
                if(pageData.page==1)
                {
                    window.location.href = "#/selfMedia/"+id;
                }
                else if(pageData.page==2)
                {
                    window.location.href = "#/advertiserDetail/"+id;
                } else if (pageData.page == 3) {
                    window.location.href = "#/case/" + id;
                } else {
                    window.location.href = "#/case/" + id;
                }

            }
            //加载广告主列表数据
            pageFunc.loadData = function (isNew,cateId) {
                pageData.currCateId=cateId;

                if (isNew) {
                    pageData.dataList = [];
                    pageData.pageIndex = 1;
                }
                else {
                    pageData.pageIndex++;
                }
                var model = {
                    cateId: cateId,
                    keyword: pageData.keyword,
                    pageIndex: pageData.pageIndex,
                    pageSize: pageData.pageSize
                }
                commService.getArticleListByOption(model, function (data) {
                    pageData.totalCount = data.totalcount;
                    for (var i = 0; i < data.list.length; i++) {
                        pageData.dataList.push(data.list[i]);
                    }
                }, function (data) {
                });
            }
            pageFunc.loadMore = function () {
                pageFunc.loadData(false,pageData.currCateId);
            }
            //搜索
            pageFunc.search = function () {
                pageFunc.loadData(true,pageData.currCateId);
            }
            //初始化
            pageFunc.init = function () {
                if(pageData.hasClassify!=undefined)
                {
                    pageData.tag.id=pageData.hasClassify;
                    pageFunc.loadCateList();
                }
                if(pageData.page==1)
                {
                    pageData.cateId=baseData.moduleCateIds.selfmedia;
                }
                else if(pageData.page==2)
                {
                    pageData.cateId=baseData.moduleCateIds.advertiser;
                } else if (pageData.page == 3) {
                    pageData.cateId = baseData.moduleCateIds.case;
                } else {
                    pageData.cateId = pageData.page;
                }

                pageFunc.loadData(true,pageData.cateId);

            };
            pageFunc.init();
        }
    };
});