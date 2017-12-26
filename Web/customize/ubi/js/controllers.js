ubimodule.controller("pageBaseCtrl", ['$scope', 'commArticle', 'commService',
  function ($scope, commArticle, commService) {

      //登录状态更改事件
      $scope.$on('loginStatusChange', function (event, msg) {
          $scope.$broadcast('loginStatusChangeNotice', msg);
      });

      //消息状态更改事件
      $scope.$on('unreadcountChange', function (event, msg) {
          $scope.$broadcast('unreadcountChangeNotice', msg);
      });

      $scope.showLogin = function (okFn, cancelMsg) {
          commService.showLoginModal(function (data, user) {
             //$scope.$emit('loginStatusChange', 'loginStatusChange');
             $scope.$broadcast('loginStatusChangeNotice', 'loginStatusChange');
           //  $scope.$broadcast('unreadcountChangeNotice', 'unreadcountChange');

              if (data.issuccess) {
                  if (okFn) {
                      okFn();
                  }
              }
          }, function () {
              alert(cancelMsg);
          });
      };

      $scope.go = function (url) {
          window.location.href = url;
      }

  }
]);