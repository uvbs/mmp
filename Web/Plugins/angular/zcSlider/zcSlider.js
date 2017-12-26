/**
 * Created by Samma on 2016/8/19.
 * 需要引用
 * jquery 2.0 +
 * jquery-ui-slider
 * 需要时添加less中的样式
 */

angular.module('zcSlider', []);
angular.module('zcSlider').directive('zcSlider', ['$timeout',function ($timeout) {
    return {
        restrict: 'E',
        //templateUrl:'/OpenWebAppComponent/Plugins/angular/zcSlider/index.html',
        templateUrl:'/Plugins/angular/zcSlider/index.html',
        replace: false,
        scope: {
            config:'@',
            value: '='
        },
        controller: function ($scope) {
            if (typeof ($scope.config) == 'string') $scope.config = angular.fromJson($scope.config);
            if($.trim($scope.value)===''){
                if($.trim($scope.config.value)!=''){
                    $scope.value = $scope.config.value;
                }
                else if($.trim($scope.config.min)!=''){
                    $scope.value = $scope.config.min;
                }
            } else {
                var sps = $.trim($scope.value).split('$');
                if (sps.length > 1) {
                    $scope.value = sps[sps.length - 1];
                }
                $scope.config.value = $scope.value;
            }
        },
        link:function (scope, element) {
            $(element).find('.sliderHandle').slider(scope.config);
            $(element).find('.sliderHandle').on("slide", function(event,ui) {
                $timeout(function(){
                    scope.$apply(function(){
                        scope.value = ui.value;
                    });
                })
            });
        }
    }
}]);