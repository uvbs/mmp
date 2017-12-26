skymemory.controller("aboutCtrl", function ($scope) {
    
    $scope.go = function (url) {
        window.location.href = url;
    }

});

skymemory.controller("filmCtrl", function ($scope) {
    $scope.go = function (url) {
        window.location.href = url;
    }
});

skymemory.controller("updateCtrl", function ($scope) {
    $scope.go = function (url) {
        window.location.href = url;
    }

    var h =  $(window).height() - $('.warpHB img').height() - 16;

    if (h>300) {
        h = 280;
    }

    $('.bottomRow').css({
        'height': h + 'px',
    });


});

skymemory.controller("articleListCtrl", function ($scope,$routeParams,$http) {

    var pageData = $scope.pageData = {
        cateid:$routeParams.cateid,
        totalCount:0,
        pageIndex:1,
        pageSize:10,
        dataList:[],
        title:$routeParams.title,
    };

    var pageFunc = $scope.pageFunc = {};

    pageFunc.loadData = function(){
        $http.get('/serv/pubapi.ashx?action=getnewslist&cateid=' + pageData.cateid + '&pageindex=' + pageData.pageIndex + '&pagesize=' + pageData.pageSize).success(function(data){
            console.log(data);
            pageData.totalCount = data.totalcount;
            pageData.dataList = pageData.dataList.concat(data.list);
            pageData.pageIndex++;
        });
    };

    pageFunc.loadData();

    $scope.go = function (item) {
        window.location.href = '/' + (item.newsid).toString(16) + '/details.chtml';
        // window.location.href = 'app.html#/filmDetail';
    }

});

skymemory.controller("filmDetailCtrl", function ($scope) {

    var h = $(window).height(), w = $(window).width();//获取页面的高度
    $('.warpDetail').css({
        'min-height': h + 'px',
    });

});
