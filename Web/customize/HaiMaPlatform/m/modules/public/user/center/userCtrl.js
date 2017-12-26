haimamodule.controller('userCtrl', ['$scope', 'commService',function ($scope, commService) {
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
       // currUser:commService.getCurrUserInfo(),
        //currPath:base64encode('#/my')
    };

    document.title = pageData.title;

    pageFunc.init = function () {
      //  pageFunc.loadUserData(data.id);
    };

    pageFunc.loadUserData = function (id) {
        commService.getUserInfo(id, function (data) {
            if (data.errcode) {
                alert('找不到该用户');
            }
            else {
                pageData.userInfo = data;
                document.title = pageData.title;
            }
        });
    };
    //退出登录
    pageFunc.logout = function () {
        commService.logout(function (data) {
            $scope.go('#/login');
        });
    }
    pageFunc.goToLink=function(){
        window.location.href="#/shareRegLink/"+pageData.userInfo.pId;
    }
    pageFunc.init();
}]);
