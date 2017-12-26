wisdomlifemodule.controller('selfMediaCtrl', ['$scope', 'commService', function ($scope, commService) {
    var pageData = $scope.pageData = {
        title: '自媒体',
        // tag: [{
        //     id: '1',
        //     name: "分类一"
        // },
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
            id: '524',
            name: "全部"
        }],
        type: "Article",
        cateId: baseData.moduleCateIds.selfmedia,
        currCateId: '',
    };


    var pageFunc = $scope.pageFunc = {};

    document.title = pageData.title;
    pageFunc.toggleMenuClick = function () {
        var menuLeft = document.getElementById('cbp-spmenu-s1'),
            body = document.body;
        classie.toggle(body, 'cbp-spmenu-push-toright');
        classie.toggle(menuLeft, 'cbp-spmenu-open');
    };
    pageFunc.leftMenuClick = function (item) {
        // pageFunc.selectCateList(item);
        pageFunc.loadData(true, item.id);
        pageFunc.toggleMenuClick();
    };
    //获取分类查询
    pageFunc.loadCateList = function () {
        commService.getArticleCateList(pageData.type, pageData.cateId, 1, 100,
            function (data) {
                for (var i = 0; i < data.list.length; i++) {
                    if (data.list[i].id != 524) {
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
    pageFunc.go = function (item) {
        //if (item.k10 != '') {
        //    //window.location.href = item.k10;
        //    window.open(item.k10);
        //    return;
        //}
        window.location.href = "#/selfMediaDetail/" + item.id;
    }
    //加载广告主列表数据
    pageFunc.loadData = function (isNew, cateId) {
        pageData.currCateId = cateId;

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
        pageFunc.loadData(false, pageData.currCateId);
    }
    //搜索
    pageFunc.search = function () {
        pageFunc.loadData(true, pageData.currCateId);
    }
    //初始化
    pageFunc.init = function () {
        pageFunc.loadCateList();
        pageFunc.loadData(true, pageData.cateId);
    };
    pageFunc.init();


}]);