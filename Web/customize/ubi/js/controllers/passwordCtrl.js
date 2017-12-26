ubimodule.controller("passwordCtrl", ['$scope', '$routeParams', 'userService'
    , function ($scope,$routeParams, userService) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '修改密码 - ' + baseData.slogan,
            id:$routeParams.id,
            password1: "",
            password2: ""
        };
        document.title = pageData.title;

        pageFunc.updatePassword = function () {
            if (pageData.password1 != pageData.password2) {
                alert("两次密码不一致"); return;
            }
            userService.updatePassword(
                pageData.password1,
                function (data) {
                    if (data.isSuccess) {
                        alert("密码修改成功");
                        pageData.password1 = "";
                        pageData.password2 = "";
                    }
                    else if (data.errcode == 10010) {
                        $scope.showLogin(function () {
                            pageFunc.updatePassword();
                        }, '您取消了登陆，修改密码必需先登录');
                    }
                    else {
                        alert("密码修改失败");
                    }
                },
                function () {
                }
            )
        }
    }
]);
