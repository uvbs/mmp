haimamodule.controller('userEditCtrl', ['$scope', 'commService',function ($scope, commService) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '编辑资料',//'编辑资料 - ' + baseData.slogan,
        //currSelectProvince: null,
        provinceList: [],//所有省份列表
        cityList: [{ id: "0", name: "请选择城市" }],//所有城市列表
        sexList: [{ id: "0", name: "男" }, { id: "1", name: "女" }],
        province: null,//当前选中省份
        city: null,//当前选中城市
        sex:null,//当前选中性别
        userInfo: [],
        currUser: commService.getCurrUserInfo(),
        dialog: null,
        IsSHowInfo: false//是否公开信息
    };

    document.title = pageData.title;

    //  城市列表
    pageFunc.init = function () {
        userService.getEditUserInfo(function (data) {
            if (data) {

                pageData.userInfo = data;
                if (pageData.userInfo)
                {
                    if (pageData.userInfo.isShowInfo == "true") {
                        pageData.userInfo.isShowInfo = true;
                    }
                    else if (pageData.userInfo.isShowInfo == "false") {
                        pageData.userInfo.isShowInfo = false;
                    }
                }
                if (pageData.userInfo.userSexInt == pageData.sexList[0].id) {
                    pageData.sex = pageData.sexList[0];
                }
                else if (pageData.userInfo.userSexInt == pageData.sexList[1].id) {
                    pageData.sex = pageData.sexList[1];
                }

                //获取省份
                commService.getGetKeyVauleDatas({
                    type: 'province'
                }, function (data) {
                    pageData.provinceList = data.list;
                    for (var i = 0; i < data.list.length; i++) {
                        if (data.list[i].id && data.list[i].id == pageData.userInfo.userProvinceCode) {
                            pageData.province = data.list[i];
                        }
                    }

                    pageFunc.selectProvince();
                }, function () { });
            }
        });

    };
    pageFunc.selectProvince = function () {
        var CityChoose = { id: "0", name: "请选择所属城市" };
        //获取城市
        commService.getGetKeyVauleDatas({
            type: 'city',
            prekey: pageData.province.id
        }, function (data) {
            pageData.cityList = data.list;
            pageData.city = 0;
            for (var i = 0; i < data.list.length; i++) {
                if (data.list[i].id == pageData.userInfo.userCityCode) {
                    pageData.city = data.list[i];
                }
            }
        }, function (data) { });
    };

    pageFunc.save = function () {
        userService.updateUserInfo({
                avatar: pageData.userInfo.avatar,
                name: pageData.userInfo.userName,
                sex: pageData.sex.id,
                LawyerLicenseNo:pageData.userInfo.LawyerLicenseNo,
                phone: pageData.userInfo.userPhone,
                tel: pageData.userInfo.userTel,
                province: pageData.province.id,
                city: pageData.city.id,
                introduction: pageData.userInfo.userIntroduction,
                company: pageData.userInfo.userCompany,
                postion: pageData.userInfo.userPostion,
                companyaddress: pageData.userInfo.userCompanyAddress,
                receiveaddress: pageData.userInfo.userReceiveAddress,
                IsSHowInfo: pageData.userInfo.isShowInfo
            },
            function (data) {
                if (data.isSuccess) {

                    alert("提交完成");
                }
                else if (data.errcode == 10010) {
                    $scope.showLogin(function () {
                        pageFunc.submitApply();
                    }, '您取消了登陆，提交必需先登录');
                    return;
                }
                else if (data.errcode = 10013) {
                    alert("您已经提交过");
                }
                else {
                    alert("提交失败");
                }
            },
            function () {
            }
        );
    }
    pageFunc.init();
}]);