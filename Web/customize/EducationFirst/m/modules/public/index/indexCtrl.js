yingfumodule.controller('indexCtrl', ['$scope', '$sessionStorage', 'commService', function ($scope, $sessionStorage, commService) {
    var pageIndexData = $scope.pageIndexData = $sessionStorage.pageIndexData;
    pageIndexData = null;
    var noStorage = !pageIndexData;
    if (noStorage) {
        pageIndexData = $scope.pageIndexData = $sessionStorage.pageIndexData = {
            title: '首页 - ' + baseData.slogan,
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
    }

    var pageIndexFunc = $scope.pageIndexFunc = {};

    document.title = pageIndexData.title;

    pageIndexFunc.init = function () {
        //  pageIndexFunc.loadData();
    };

    if (noStorage) pageIndexFunc.init();
}]);