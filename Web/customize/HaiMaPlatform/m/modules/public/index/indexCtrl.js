haimamodule.controller('indexCtrl', ['$scope','$sessionStorage','commService',function($scope, $sessionStorage,commService) {
    var pageIndexData = $scope.pageIndexData = $sessionStorage.pageIndexData;
    pageIndexData = null;
    var noStorage = !pageIndexData;
    if (noStorage) {
        pageIndexData = $scope.pageIndexData = $sessionStorage.pageIndexData = {
            title: '首页 - ' + baseData.slogan,
            dataViews: [{
                id: 1,
                name: '海马资讯',
                activeCss: 'blueBg',
                active: true,
                list: [],
                pageIndex: 1,
                pageSize: 5,
                totalCount: 0,
                loadEnd: false
            }, {
                id: 2,
                name: '热门课程',
                activeCss: 'yellowBg',
                active: false,
                list: [],
                pageIndex: 1,
                pageSize: 5,
                totalCount: 0,
                loadEnd: false
            }, {
                id: 3,
                name: '知识库',
                activeCss: 'greenBg',
                active: false,
                list: [],
                pageIndex: 1,
                pageSize: 5,
                totalCount: 0,
                loadEnd: false
            }],
            currSelectTabIndex: 1,
            adImgs: [{
                img:'/img/hb/hb6.jpg',
                url:'#/regulations'
            },{
                img:'/img/hb/hb5.jpg',
                url:'#/news'
            },{
                img:'/img/hb/hb4.jpg',
                url:'#/news/1'
            }]
        };
    }

    var pageIndexFunc = $scope.pageIndexFunc = {};

    document.title = pageIndexData.title;

    pageIndexFunc.loadData = function() {
        var pageii = layer.open({
            type: 2
        });
        var tabIndex = pageIndexData.currSelectTabIndex - 1;
        var cateId = [baseData.moduleCateIds.news, baseData.moduleCateIds.hotclass, baseData.moduleCateIds.knowledge];
        var type = ["Article", "Article", "Article"];
        var orderby = [null, "comment", null];
        if (pageIndexData.dataViews[tabIndex].loadEnd) {
            pageIndexData.dataViews[tabIndex].pageIndex++;
        } else {
            pageIndexData.dataViews[tabIndex].loadEnd = true;
        }
        var model = {
            cateId: cateId[tabIndex],
            type: type[tabIndex],
            pageIndex: pageIndexData.dataViews[tabIndex].pageIndex,
            pageSize: pageIndexData.dataViews[tabIndex].pageSize,
            orderby: orderby[tabIndex]
        };
        commService.getArticleListByOption(model, function(data) {
            pageIndexData.dataViews[tabIndex].totalCount = data.totalcount;
            for (var i = 0; i < data.list.length; i++) {
                pageIndexData.dataViews[tabIndex].list.push(data.list[i]);
            }
            layer.close(pageii);
        }, function(data) {
            layer.close(pageii);
        });
    };

    pageIndexFunc.isArticle = function(item) {
        return item.type != "Question";
    };
    pageIndexFunc.getTypeName = function(item) {
        return item.type == "Question" ? "问题" : "文章";
    };

    /**
     * [tabClick tab点击]
     * @param  {[type]} item [description]
     * @return {[type]}      [description]
     */
    pageIndexFunc.tabClick = function(item) {
        commService.setArrValue(pageIndexData.dataViews, 'active', false);
        item.active = true;
        pageIndexData.currSelectTabIndex = item.id;
        if (!pageIndexData.dataViews[pageIndexData.currSelectTabIndex - 1].loadEnd) {
            pageIndexFunc.loadData();
        }
    };
    pageIndexFunc.init = function() {
        pageIndexFunc.loadData();
    };
    //1_0_3_83_1_1   6个参数分别表示ng-attr-hassearch（搜索）  ng-attr-hasimg（图片） ng-attr-page ng-attr-otherCateId（cateId）
    // ng-attr-.hasTag(是否有标签)  ng-attr-hassummary （是否显示summary）
    pageIndexFunc.go=function(item){
        window.location.href=item;
    };
    pageIndexData.goDetail=function(id){
      window.location.href="/m#/news/"+id;
    };

    if (noStorage) pageIndexFunc.init();
}]);