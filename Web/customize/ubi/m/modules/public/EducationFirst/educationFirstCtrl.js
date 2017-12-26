ubimodule.controller('educationFirstCtrl', ['$scope',function ($scope) {
    var pageData = $scope.pageData;
    pageData = null;
    pageData = $scope.pageData = {
        title: '英孚',
        adImgs: [{
            img: '/img/hb/hb6.jpg',
            url: '#/regulations'
        }, {
            img: '/img/hb/hb5.jpg',
            url: '#/news'
        }, {
            img: '/img/hb/hb4.jpg',
            url: '#/news/1'
        }]
    };

    var pageFunc = $scope.pageFunc = {};

    document.title = pageData.title;

    //1_0_3_83_1_1   6个参数分别表示ng-attr-hassearch（搜索）  ng-attr-hasimg（图片） ng-attr-page ng-attr-otherCateId（cateId）
    // ng-attr-.hasTag(是否有标签)  ng-attr-hassummary （是否显示summary）
    pageFunc.go=function(item){
        window.location.href=item;
    };
    pageFunc.init = function () {

    };

    pageFunc.init();
}]);