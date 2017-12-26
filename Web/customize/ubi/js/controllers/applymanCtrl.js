ubimodule.controller("applymanCtrl", ['$scope', '$routeParams', 'userService'
    , function ($scope,$routeParams, userService) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            id: $routeParams.id,
            title: '申请成为专家 - ' + baseData.slogan
        };
        document.title = pageData.title;

        pageFunc.submitApply = function () {
            userService.applyTutor(
                function (data) {
                    if (data.isSuccess){
                        alert("提交申请完成");
                    }
                    else if (data.errcode == 10010)
                    {
                        $scope.showLogin(function () {
                            pageFunc.submitApply();
                        }, '您取消了登陆，提交申请必需先登录');
                        return;
                    }
                    else if (data.errcode = 10013) {
                        alert("您已经提交过申请");
                    }
                    else {
                        alert("提交申请失败");
                    }
                },
                function () {
                }
            );
        }
    }
]);
