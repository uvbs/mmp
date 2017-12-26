yingfumodule.directive('footermenu', function () {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'directives/footermenu/tpls/index.html',
        replace: true,
        scope: {
            select: '@'
        },
        controller: function ($scope, $element, $attrs) {
            var pageFunc = $scope.pageFunc = {};
            var pageData = $scope.pageData = {
                menus: [{
                    id: 0,
                    title: '首页',
                    selected: false,
                    icon: 'icon-iconfontshouye',
                    url: '#/index'
                }, {
                    id: 1,
                    title: '预约报名',
                    selected: false,
                    icon: 'icon-pinpaizixun',
                    url: '#/msg'
                }, {
                    id: 2,
                    title: '联系我们',
                    selected: false,
                    icon: 'icon-gerenzhongxin',
                    url: '#/my'
                }]
            };

            for (var i = 0; i < pageData.menus.length; i++) {
                if ($scope.select == pageData.menus[i].id) {
                    pageData.menus[i].selected = true;
                };
            };

            pageFunc.click = function (item) {
                window.location.href = item.url;
            };

        }
    };
});
