ubimodule.controller('userCtrl', ['$scope', 'commService', 'userService', 'commArticle', function ($scope, commService, userService, commArticle) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '个人中心',//'个人中心 - ' + baseData.slogan,
        provinceList: [{
            id: '',
            name: '全部',
        }],
        currSelectProvince: null,
        userInfo: [],//用户对象
        isShowApplyMan:false,
        currUser:commService.getCurrUserInfo(),
        currPath:base64encode('#/my'),
    };

    document.title = pageData.title;

    pageFunc.init = function () {      
        commService.checkLogin(function (data) {
            if (data) {
                pageFunc.loadUserData(data.id);
            }
            else {
                $scope.go('#/login');
            }
        });
    };

    pageFunc.loadUserData = function (id) {
        userService.getUserInfo(id, function (data) {
            if (data.errcode) {
                alert('找不到该用户');
            }
            else {
                pageData.userInfo = data;
                pageData.isShowApplyMan = (pageData.userInfo.userType == 3) && (!pageData.userInfo.isTutor);
                if (data.isCurrUser) {
                    userRole = "个人";
                    pageData.title = '个人空间 - ' + baseData.slogan;
                }
                else if (data.isTutor) {
                    userRole = "专家";
                    pageData.title = '专家空间 - ' + baseData.slogan;
                }
                else {
                    userRole = "用户";
                    pageData.title = '用户空间 - ' + baseData.slogan;
                }
                document.title = pageData.title;
            }
        });
    }
    //退出登录
    pageFunc.logout = function () {
        commService.logout(function (data) {
            $scope.go('#/login');
        });
    }
    //申请为专家
    pageFunc.applyTutor = function () {
        if (confirm("确定要申请为专家吗？")) {
            if (true)
            {

            }
            userService.applyTutor(function (data) {
                if (data.isSuccess) {
                    alert("提交申请完成");
                }
                else if (data.errcode == 10010) {
                    if (!pageData.currUser) {
                        $scope.go("#/login/" + pageData.currPath);
                    }
                    else {
                        pageFunc.submitApply();
                    }
                    return;
                }
                else if (data.errcode = 10013) {
                    alert("您已经提交过申请");
                }
                else {
                    alert("提交申请失败");
                }
            }, function () { });
        }
        else
        {
            alert("您取消了申请为专家！");
        }
    };

    pageFunc.goToLink=function(){
        window.location.href="#/shareRegLink/"+pageData.userInfo.pId;
      //  window.location.href="#/shareRegLink";
    }
    pageFunc.init();
}]);
