wisdomlifemodule.controller('putAdCtrl', ['$scope', 'commService', function ($scope, commService) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '投广告',
        provinceList: [],//投放平台      
        province: null,//当前选中投放平台 

        Name: '',//姓名
        Phone: '',//手机号码
        K1: '',//邮箱
        K2: '',//广告主名称
        K3: '',//广告预算
        K4: '',//投放平台
        K5: '',//投放起始日期
        K6: '',//投放截止日期
        K7: '',//广告内容概括
        aid: '445296',//投广告 本地 167503 线上 445296

    };

    document.title = pageData.title;

    //  城市列表
    pageFunc.init = function () {
        //获取省份
        commService.getGetKeyVauleDatas({
            type: 'Platform'
        }, function (data) {
            pageData.provinceList = data.list;
        }, function () { });

    };
    //返回
    pageFunc.goback = function () {
        history.go(-1);
    }
    //投广告 立即提交
    pageFunc.submit = function () {

        if (pageFunc.checkAll() == true) {
            if (pageData.K5 != "") {
                pageData.K5 = pageData.K5.getFullYear() + "-" + (pageData.K5.getMonth() + 1) + "-" + pageData.K5.getDate();
            }
            if (pageData.K6 != "") {
                pageData.K6 = pageData.K6.getFullYear() + "-" + (pageData.K6.getMonth() + 1) + "-" + pageData.K6.getDate();
            }
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
    //检查为空
    pageFunc.checkAll = function () {
        console.log(pageData.K5);
        console.log(pageData.K6);
        if (pageData.K2 == "") {
            alert("广告主名称为必填项！")
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
            alert("广告预算为必填项！")
            return false;
        }
        if (pageData.K4 == null) {
            alert("投放平台为必填项！")
            return false;
        }
        if (!pageFunc.isPhone(pageData.Phone)) {
            alert("您的手机号格式不正确！");
            return false;
        }
        if (pageData.K5 > pageData.K6)
        {
            alert("起始日期不能大于截止日期！");
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

    //pageFunc.format = function (date) {
    //    if (date.getMonth == 12)
    //    {

    //    }
    //    date = pdate.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
    //}
    pageFunc.init();
}]);