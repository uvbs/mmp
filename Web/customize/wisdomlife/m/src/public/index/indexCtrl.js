wisdomlifemodule.controller('indexCtrl', ['$scope', 'commService','$routeParams', function ($scope, commService,$routeParams) {
    var pageIndexData = $scope.pageIndexData = {
        title: '首页',
        id:$routeParams.id,
        dataViews: [{
            id: 1,
            name: '今日头条',
            //  activeCss: 'blueBg',
          //  active: true,
            list: [],
            pageIndex: 1,
            pageSize: 5,
            totalCount: 0,
            // loadEnd: false,
            cateId: baseData.moduleCateIds.news
        }, {
            id: 2,
            name: '新认证广告主',
            // activeCss: 'yellowBg',
           // active: false,
            list: [],
            pageIndex: 1,
            pageSize: 5,
            totalCount: 0,
            //  loadEnd: false
            cateId: baseData.moduleCateIds.advertiser
        }, {
            id: 3,
            name: '新认证自媒体',
            //   activeCss: 'greenBg',
           // active: false,
            list: [],
            pageIndex: 1,
            pageSize: 5,
            totalCount: 0,
            //  loadEnd: false
            cateId: baseData.moduleCateIds.selfmedia
        }],
        currSelectItem:[],
        currSelectTabIndex: 1,
        adImgs: [],
        subCateList: {
            advertiser: [],
            selfmedia: [],
            news:[]
        }
    };

    var pageIndexFunc = $scope.pageIndexFunc = {};

    document.title = pageIndexData.title;
    //获取首页图片
    pageIndexFunc.getPageIndexImg = function () {
        commService.getAdList('1', '1', '5', function (data) {
            if (data && data.list) {
                pageIndexData.adImgs = data.list;
            }
            
        }, function () { });
    };
    pageIndexFunc.changeAdImgSize = function() {
        var h = $(window).height(), w = $(window).width();//获取页面的高度

        var ox = 420, oy = 200;//原始宽高
        var ry = (w / ox) * oy;//真实高度

        $('.wrapIndex .wrapAd img,.wrapIndex .wrapAd').css({
            'height':ry + 'px!important'
        });
        
    };
    /**
     * [tabClick tab点击]
     * @param  {[type]} item [description]
     * @return {[type]}      [description]
     */
    pageIndexFunc.tabClick = function (item) {
        // item.active = true;   
        pageIndexData.currSelectTabIndex = item.id;
        pageIndexFunc.loadData(true, item);
    };
    //跳转到对应列表
    pageIndexFunc.go = function (item) {
        window.location.href = "#/"+item;
    }
    //加载对应数据
    pageIndexFunc.loadData = function (isNew,item) {


        if (isNew) {
            item.list = [];
            item.pageIndex = 1;
        }
        else {
            item.pageIndex++;
        }
        var model = {
            cateId: item.cateId,
           // keyword: pageData.keyword,
            pageIndex: item.pageIndex,
            pageSize: item.pageSize,
        }
        commService.getArticleListByOption(model, function (data) {
            
            item.totalCount = data.totalcount;
            for (var i = 0; i < data.list.length; i++) {
                item.list.push(data.list[i]);
            }
            pageIndexData.currSelectItem = item;//将当前选中数据放入currSelectItem中
        }, function (data) {
        });
    }
    //加载更多
    pageIndexFunc.loadMore = function () {
        pageIndexFunc.loadData(false,pageIndexData.currSelectItem);
    }
    //初始化
    pageIndexFunc.init = function () {


        //加载子分类
        commService.getArticleCateList('Article', baseData.moduleCateIds.news, 1, 10000, function (data) {
            pageIndexData.subCateList.news = data.list;
        }, function (data) {

        });
        commService.getArticleCateList('Article', baseData.moduleCateIds.advertiser, 1, 10000, function (data) {
            pageIndexData.subCateList.advertiser = data.list;
        }, function (data) {

        });
        commService.getArticleCateList('Article', baseData.moduleCateIds.selfmedia, 1, 10000, function (data) {
            pageIndexData.subCateList.selfmedia = data.list;
        }, function (data) {

        });


        pageIndexFunc.getPageIndexImg();
        if (pageIndexData.id!=undefined)
        {
            if (pageIndexData.id==1)
            {
                pageIndexFunc.loadData(true, pageIndexData.dataViews[0]);
                pageIndexData.currSelectTabIndex=pageIndexData.dataViews[0].id;
            }
            else if (pageIndexData.id==2)
            {
                pageIndexFunc.loadData(true, pageIndexData.dataViews[1]);
                pageIndexData.currSelectTabIndex=pageIndexData.dataViews[1].id;
            }
            else if (pageIndexData.id==3)
            {
                pageIndexFunc.loadData(true, pageIndexData.dataViews[2]);
                pageIndexData.currSelectTabIndex=pageIndexData.dataViews[2].id;
            }
        }
        else
        {
            pageIndexFunc.loadData(true, pageIndexData.dataViews[0]);
        }
        pageIndexFunc.changeAdImgSize();
        setInterval(function () {
            pageIndexFunc.changeAdImgSize();
        },100);
        
    };
   // pageFunc.go(item.id)
   // 跳转到详情
    pageIndexFunc.goDetail = function (item) {

        for (var i = 0; i < pageIndexData.subCateList.news.length; i++) {
            var cate = pageIndexData.subCateList.news[i];
            if (item.categoryId == cate.id) {
                window.location.href = "#/newsDetail/" + item.id;
                return;
            }
        }
        for (var i = 0; i < pageIndexData.subCateList.advertiser.length; i++) {
            var cate = pageIndexData.subCateList.advertiser[i];
            if (item.categoryId == cate.id) {
                window.location.href = "#/advertiserDetail/" + item.id;
                return;
            }
        }
        for (var i = 0; i < pageIndexData.subCateList.selfmedia.length; i++) {
            var cate = pageIndexData.subCateList.selfmedia[i];

            //判断是否有k10(自媒体链接)，有则跳转到指定连接
            //if (item.k10 != '') {
            //    window.location.href = item.k10;
            //    return;
            //}

            if (item.categoryId == cate.id) {
                window.location.href = "#/selfMediaDetail/" + item.id;
                return;
            }
        }

    }

    //跳转到活动
    pageIndexFunc.goActivity = function () {
        window.location.href = "/App/Cation/Wap/ActivityLlists.aspx";
    }

    pageIndexFunc.init();

}]);