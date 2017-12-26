haimamodule.directive('footermenu', function() {
    return {
        restrict: 'ECMA',
        templateUrl: baseViewPath + 'directives/footermenu/tpls/index.html',
        replace: true,
        scope: {
            select: '@'
        },
        controller: function($scope, $element, $attrs) {
            var pageFunc = $scope.pageFunc = {};
            var pageData = $scope.pageData = {
                menus: [{
                    id: 0,
                    title: '首页',
                    selected: false,
                    icon: 'icon-iconfontshouye',
                    url:'#/index'
                }, {
                    id: 1,
                    title: '资讯',
                    selected: false,
                    icon: 'icon-pinpaizixun',
                    url:'#/msg'
                }, {
                    id: 2,
                    title: '个人中心',
                    selected: false,
                    icon: 'icon-gerenzhongxin',
                    url:'#/center'
                }]
            };

            for (var i = 0; i < pageData.menus.length; i++) {
                if ($scope.select == pageData.menus[i].id) {
                    pageData.menus[i].selected = true;
                };
            };

            pageFunc.click = function(item) {
                window.location.href = item.url;
            };

        }
    };
});
