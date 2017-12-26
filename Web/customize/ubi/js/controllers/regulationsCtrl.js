ubimodule.controller("regulationsCtrl", ['$scope', 'commArticle', '$routeParams', function ($scope, commArticle, $routeParams) {

    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '政策法规 - ' + baseData.slogan,
        articles: [],
        pageIndex: 1,
        pageSize: 8,
        totalCount: 0,
        keyword: '',
        currCate: baseData.moduleCateIds.regulations, //政策法规文章列表
        hotArticle: [],
        hotTags: [],
        currSelectTag: '',
        currTag: $routeParams.tag,

        //pageIndex: 1,
        //pageSize: 8,
        //totalCount: 0,
        provinceList: [{
            id: '',
            name: '全部'
        }],
        provinceOtherList: [],
        currSelectProvince: null,
        tagList: [{
            tag: '全部'
        }],
        tagOtherList: [],
        searchType: [
        {
            name: '全部',
            type: 1,
            selected:true
        }, {
            name: '查标题',
            type: 2,
            selected: false
        }, {
            name: '查内容',
            type: 3,
            selected: false
        }],
        currType:1
    };

    document.title = pageData.title;

    pageFunc.selectProvince = function (item) {
        pageData.pageIndex = 1;
        pageData.currSelectProvince = item;
        pageFunc.loadData();
    };

    //pageFunc.loadData = function() {
    //	commArticle.getArticleList(function(data) {
    //		console.log(data);
    //		pageData.articles = data.list;
    //		pageData.totalCount = data.totalcount;
    //	}, function(data) {

    //	}, 'Article', pageData.currCate, pageData.keyword, '', '', '', pageData.pageIndex, pageData.pageSize, 0, 0);
    //};
    pageFunc.loadData = function () {
       
        console.log(pageData.currType);
        var tagFilter = pageData.currSelectTag ? pageData.currSelectTag.tag : '';
        if (tagFilter == '全部') {
            tagFilter = '';
        };
        if (pageData.currTag != '') {
            tagFilter = pageData.currTag;
        }

        //var tag = '';
        //if (pageData.currSelectTag != '') {
        //    tag = pageData.currSelectTag;
        //}
        //if (pageData.currSelectTag==''&&pageData.currTag != '') {
        //    tag = pageData.currTag;
        //}
        commArticle.getArticleListByOption({
            cateId: pageData.currCate,
            keyword: pageData.keyword,
            tags: tagFilter,
            province: pageData.currSelectProvince ? pageData.currSelectProvince.id : '',
            orderby: 'CreateDate desc',
            isGetNoCommentData: 0,
            isHasCommentAndReplayCount: 0,
            pageIndex: pageData.pageIndex,
            pageSize: pageData.pageSize,
            type: 'Article',
            keyType: pageData.currType
        }, function (data) {
            console.log(data);
            pageData.articles = data.list;
            pageData.totalCount = data.totalcount;
            pageData.currTag = '';
        }, function (data) {

        });
    };
    pageFunc.selectTag = function (item) {
        pageData.pageIndex = 1;
        pageData.currSelectTag = item;
        pageFunc.loadData();
    };

    pageFunc.search = function () {
        if (pageData.pageIndex != 1) {
            pageData.pageIndex = 1;
        } else {
            pageFunc.loadData();
        }
    };

    pageFunc.init = function () {
        //获取省份
        commArticle.getGetKeyVauleDatas('province', null, "1", function (data) {
            if (data && data.list) {
                for (var i = 0; i < data.list.length; i++) {
                    if (i < 6) {
                        pageData.provinceList.push(data.list[i]);
                    } else {
                        pageData.provinceOtherList.push(data.list[i]);
                    }

                }
            }
            console.log(pageData.provinceList);
            pageData.currSelectProvince = pageData.provinceList[0];

        }, function () { });

        ////获取标签
        //commArticle.getTags(function (data) {
        //    if (data) {
        //        for (var i = 0; i < data.length; i++) {
        //            if (i < 6) {
        //                pageData.tagList.push(data[i]);
        //            } else {
        //                pageData.tagOtherList.push(data[i]);
        //            }
        //        }
        //    }
        //    pageData.currSelectTag = pageData.tagList[0];
        //}, function (argument) {
        //});

        $scope.$watch('pageData.pageIndex', pageFunc.loadData);
    }
    pageFunc.init();

}]);
ubimodule.controller("regulationsDetailCtrl", ['$scope', 'commArticle', '$routeParams',
	function ($scope, commArticle, $routeParams) {

	    var pageFunc = $scope.pageFunc = {};
	    var pageData = $scope.pageData = {
	        title: '政策法规详情 - ' + baseData.slogan,
	        currCate: baseData.moduleCateIds.regulations, //政策法规文章列表
	        currArticleId: $routeParams.id,
	        article: null
	    };

	    document.title = pageData.title;

	    pageFunc.init = function () {
	        commArticle.getArticleDetail(function (data) {
	            console.log(data);
	            pageData.article = data;
	        }, function (data) {
	            alert('文章获取失败');
	        }, pageData.currArticleId);
	    };

	    pageFunc.favoriteArticle = function () {
	        commArticle.favoriteArticle(function (data) {
	            if (data.isSuccess) {
	                pageData.article.currUserIsFavorite = true;
	                pageData.article.favoriteCount += 1;
	                alert('收藏成功');

	            } else if (data.errcode == 10010) {
	                $scope.showLogin(null, '您取消了登陆，继续收藏必须先登录');

	            } else if (data.errcode == 10013) {
	                alert('您已经收藏过了');

	            }
	        }, function (data) { }, pageData.currArticleId);
	    };

	    pageFunc.disFavoriteArticle = function () {
	        commArticle.disFavoriteArticle(function (data) {
	            if (data.isSuccess) {
	                pageData.article.currUserIsFavorite = false;
	                pageData.article.favoriteCount -= 1;
	                alert('取消收藏成功');
	            } else {
	                alert('取消收藏失败');
	            }
	        }, function (data) { }, pageData.currArticleId);
	    };


	    pageFunc.init();

	}
]);