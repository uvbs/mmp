wisdomlifemodule.controller('takeAdCtrl', ['$scope', 'commService', function ($scope, commService) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '接广告',

        provinceList: [],//受影响区域
        province: null,//当前受影响区域

        cityList: [],//受众行业     
        city: null,//当前受众行业

        districtList: [],//结算方式
        district: null,//当前结算方式

        Name: '',//名称
        Phone: '',//联系手机
        K1: '',//邮箱
        K2: '',//受众行业
        K3: '',//账号名称
        K4: '',//微信ID号
        K5: '',//粉丝数量
        K6: '',//影响区域
        K7: '',//刊例
        K8: '',//对外折扣
        K9: '',//结算方式
        K10: '',//其他账号平台
        K11: '',//最低折扣
        aid: '445299',//本地 167506  线上 445299
    };

    document.title = pageData.title;

    //  
    pageFunc.init = function () {
        //获取省份(受影响区域）
        commService.getGetKeyVauleDatas({
            type: 'province'
        }, function (data) {
            pageData.provinceList = data.list;
            //for (var i = 0; i < data.list.length; i++) {
            //    if (data.list[i].id && data.list[i].id == pageData.userInfo.userProvinceCode) {
            //        pageData.province = data.list[i];
            //    }
            //}

            // pageFunc.selectProvince();
        }, function () { });
        //获取（受众行业）
        commService.getGetKeyVauleDatas({
            type: 'Influence'
        }, function (data) {
            pageData.cityList = data.list;
            //for (var i = 0; i < data.list.length; i++) {
            //    if (data.list[i].id && data.list[i].id == pageData.userInfo.userProvinceCode) {
            //        pageData.province = data.list[i];
            //    }
            //}

            // pageFunc.selectProvince();
        }, function () { });
        //获取（结算方式）
        commService.getGetKeyVauleDatas({
            type: 'Buyway'
        }, function (data) {
            pageData.districtList = data.list;
            //for (var i = 0; i < data.list.length; i++) {
            //    if (data.list[i].id && data.list[i].id == pageData.userInfo.userProvinceCode) {
            //        pageData.province = data.list[i];
            //    }
            //}

            // pageFunc.selectProvince();
        }, function () { });
    };
    //接广告 立即提交
    pageFunc.submit = function () {
        if (pageFunc.checkAll() == true) {
            var reqData = {
                action: 'submitactivitysigndata',
                activityid: pageData.aid,
                Name: pageData.Name,
                Phone: pageData.Phone,
                K1: pageData.K1,
                K2: pageData.K2,
                K3: pageData.K3,
                K4: pageData.K4,
                K5: pageData.K5,
                K6: pageData.K6,
                K7: pageData.K7,
                K8: pageData.K8,
                K9: pageData.K9,
                K10: pageData.K10,
                K11: pageData.K11,
            };
            commService.postData(baseData.handlerUrl, reqData, function (data) {
                if (data.errmsg == 'ok') {
                    alert('提交成功，等待管理员审核',1,2,function() {
                        window.location.href = '#/index';
                    });
                } else {

                    alert(data.errmsg);
                    // if (data.errcode == -2) {

                    // } else {
                    //     if (data.errmsg == '已经报过名了！')
                    //     {
                    //         alert('已经投过了！');
                    //     }
                        
                    // }else{

                    // }
                }
            });
        }
    };
    //返回
    pageFunc.goback = function () {
        history.go(-1);
    }
    pageFunc.checkAll = function () {
        if (pageData.city != null)
        {
            pageData.K2 = pageData.city.id;
        }
        else
        {
            alert("受众行业为必填项！")
            return false;
        }    
        
        if (pageData.Name == "") {
            alert("联系姓名为必填项！")
            return false;
        }
        if (pageData.Phone == "") {
            alert("联系电话为必填项！")
            return false;
        }
        if (pageData.K1 == "") {
            alert("联系邮箱为必填项！")
            return false;
        }
        if (pageData.K3 == "") {
            alert("账号名称为必填项！")
            return false;
        }
        if (pageData.K4 == "") {
            alert("微信ID号为必填项！")
            return false;
        }
        if (pageData.K5 == "") {
            alert("粉丝数量为必填项！")
            return false;
        }
        if (pageData.province != null) {
            pageData.K6 = pageData.province.id;
        }
        else {
            alert("影响区域为必填项！")
            return false;
        }
        if (pageData.K7 == "") {
            alert("刊例为必填项！")
            return false;
        }
        if (pageData.K8 == "") {
            alert("对外折扣为必填项！")
            return false;
        }
        if (pageData.K11 == "") {
            alert("最低折扣为必填项！")
            return false;
        }
        if (pageData.district != null) {
            pageData.K9 = pageData.district.id;
        }
        else {
            alert("结算方式为必填项！")
            return false;
        }
        if (!pageFunc.isPhone(pageData.Phone)) {
            alert("您的手机号格式不正确！");
            return false;
        }

        return true;

    }
    pageFunc.isPhone = function (phone) {
        var pattern = /(^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$)|(^0{0,1}1[3|4|5|6|7|8|9][0-9]{9}$)/;

        if (pattern.test(phone)) {
            return true;
        } else {
            return false;
        }
    };
    pageFunc.init();
}]);