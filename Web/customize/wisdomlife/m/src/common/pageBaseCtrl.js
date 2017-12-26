wisdomlifemodule.controller("pageBaseCtrl", ['$scope', function($scope) {
    $scope.go = function(url) {
        window.location.href = url;
    };

    $scope.goback = function() {
        history.go(-1);
    };

    var ua = navigator.userAgent;

    if (ua.indexOf('MQQBrowser') > 0) {
        $scope.isQQBrowser = true;
        // alert('I am qq shit. ');
    }

}]);