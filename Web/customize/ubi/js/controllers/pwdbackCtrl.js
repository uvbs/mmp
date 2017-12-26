ubimodule.controller("pwdbackCtrl", ['$scope', '$routeParams', 'userService', 'commService'
    , function ($scope, $routeParams, userService, commService) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '忘记密码 - ' + baseData.slogan,
            userEmail: "",
            checkCode: ""
        };
        document.title = pageData.title;

        pageFunc.findPwd = function () {
            commService.findPassword(pageData.checkCode, pageData.userEmail, function (data) {
                if(data&&data.isSucess)
                {
                    alert("请查收邮件！")
                }
                else {
                    alert("找回失败！");
                }
            })
        };
        pageFunc.init = function () {

        };
        pageFunc.init();
    }
]);
