ubimodule.controller('indexCtrl', ['$scope', '$sessionStorage', 'commService', 'commArticle', 'userService', function ($scope, $sessionStorage, commService, commArticle, userService) {
    var pageIndexData = $scope.pageIndexData = $sessionStorage.pageIndexData;
    //pageIndexData = null;
    var noStorage = !pageIndexData;
    if (noStorage) {
        pageIndexData = $scope.pageIndexData = $sessionStorage.pageIndexData = {
            title: '首页 - ' + baseData.slogan,
            dailyCase: {},
            dataViews: [{
                id: 1,
                name: '新闻资讯',
                activeCss: 'blueBg',
                active: true,
                list: [],
                pageIndex: 1,
                pageSize: 5,
                totalCount: 0,
                loadEnd: false
            }, {
                id: 2,
                name: '热门问题',
                activeCss: 'yellowBg',
                active: false,
                list: [],
                pageIndex: 1,
                pageSize: 5,
                totalCount: 0,
                loadEnd: false
            }, {
                id: 3,
                name: '社区动态',
                activeCss: 'greenBg',
                active: false,
                list: [],
                pageIndex: 1,
                pageSize: 5,
                totalCount: 0,
                loadEnd: false
            }],
            currSelectTabIndex: 1,
            adImgs:[]
            //adImgs: [{
            //    img:'/img/hb/hb6.jpg',
            //    url:'#/regulations'
            //},{
            //    img:'/img/hb/hb5.jpg',
            //    url:'#/news'
            //},{
            //    img:'/img/hb/hb4.jpg',
            //    url:'#/news/1'
            //}]
        };
    }

    var pageIndexFunc = $scope.pageIndexFunc = {};

    document.title = pageIndexData.title;

    pageIndexFunc.getDailyCase = function() {
        commArticle.getDailyCase(
            function(data) {
                $scope.pageIndexData.dailyCase = data;
            },
            null);
    }
    pageIndexFunc.loadData = function() {
        var pageii = layer.open({
            type: 2
        });
        var tabIndex = pageIndexData.currSelectTabIndex - 1;
        var cateId = [baseData.moduleCateIds.news, baseData.moduleCateIds.askquestion + ',' + baseData.moduleCateIds.askarticle, null];
        var type = [null, null, "Statuses"];
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
        }
        commArticle.getArticleListByOption(model, function (data) {
           // debugger;
            pageIndexData.dataViews[tabIndex].totalCount = data.totalcount;
            for (var i = 0; i < data.list.length; i++) {
                pageIndexData.dataViews[tabIndex].list.push(data.list[i]);
            }
            layer.close(pageii);
        }, function(data) {
            layer.close(pageii);
        });
    }

    pageIndexFunc.isArticle = function(item) {
        return item.type != "Question";
    }
    pageIndexFunc.getTypeName = function(item) {
        return item.type == "Question" ? "问题" : "文章";
    }

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
        pageIndexFunc.getDailyCase();
        pageIndexFunc.getPageIndexImg();
    }
    //获取首页图片
    pageIndexFunc.getPageIndexImg = function () {
        userService.getAdList('1', '1', '5', function (data) {
            if (data && data.list) {
                pageIndexData.adImgs = data.list;
            }
        }, function () { });
    }

    pageIndexFunc.go = function (id) {
        // debugger;
        var url = "#/news/" + id;
        window.location.href = url;
    }
    pageIndexFunc.goAsk = function (id) {
        // debugger;
        var url = "#/ask/" + id;
        window.location.href = url;
    }
    pageIndexFunc.goUpdate = function (id) {
        // debugger;
        var url = "#/update/" + id;
        window.location.href = url;
    }
    pageIndexFunc.goList=function()
    {
        var url = "#/news";
        window.location.href = url;
    }

    pageIndexFunc.changeAdImgSize = function() {
        var h = $(window).height(), w = $(window).width();//获取页面的高度

        var ox = 340, oy = 256;//原始宽高
        var ry = (w / ox) * oy;//真实高度

        $('.wrapIndex .wrapAd img,.wrapIndex .wrapAd').css({
            'height':ry + 'px!important'
        });

    };

    if (noStorage) pageIndexFunc.init();

    pageIndexFunc.changeAdImgSize();

    setInterval(function(){pageIndexFunc.changeAdImgSize();},100)
}]);