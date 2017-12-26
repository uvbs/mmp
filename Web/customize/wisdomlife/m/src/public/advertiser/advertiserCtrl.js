wisdomlifemodule.controller('advertiserCtrl', ['$scope', 'commService', '$routeParams', function ($scope, commService, $routeParams) {
    var pageData = $scope.pageData = {
        title: '广告主',
        //tag: [{
        //    id: '1',
        //    name: "分类一"
        //},
        //{
        //    id: '2',
        //    name: "分类二"
        //},
        //{
        //    id: '3',
        //    name: "分类三"
        //},
        //{
        //    id: '4',
        //    name: "分类四"
        //},
        //{
        //    id: '5',
        //    name: "分类五"
        //}],
        
        keyword: "",
        pageIndex: 1,
        pageSize: 5,
        totalCount: 0,
        dataList: [],
        tag: [{
            id: '523',
            name: "全部"
        }],
        type: "Article",
        cateId: baseData.moduleCateIds.advertiser,
        currCateId: '',
        routeCateId: $routeParams.cateId,
        moduleName:$routeParams.moduleName
    };

    var pageFunc = $scope.pageFunc = {};

   

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
        commService.getArticleCateList(pageData.type, pageData.cateId, 1, 100,
            function (data) {
                for (var i = 0; i < data.list.length; i++) {

                    if (pageData.routeCateId && data.list[i].id == pageData.routeCateId) {
                        continue;
                    }

                    if (data.list[i].id != 523)
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
        if (pageData.routeCateId) {
            window.location.href = "#/case/" + id;
        }else{
            window.location.href = "#/advertiserDetail/"+id;
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
            pageSize: pageData.pageSize,
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

        if (pageData.moduleName) {
            pageData.title = pageData.moduleName;
        }

        document.title = pageData.title;

        if (pageData.routeCateId) {
            pageData.cateId = pageData.routeCateId;
            pageData.tag[0].id = pageData.routeCateId;
        }

        pageFunc.loadCateList();
        pageFunc.loadData(true,pageData.cateId);
    };
    pageFunc.init();

}]);