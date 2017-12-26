﻿ubimodule.controller('regCtrl', ['$scope', 'commService', 'userService', 'FileUploader', '$interval','$routeParams',
    function ($scope, commService, userService, FileUploader, $interval,$routeParams) {
    // var userService = {};
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '注册',
        //isTutor: false,
        Name: '',//姓名
        Mail: '',//邮箱
        pwd: '',//密码
        pwd2: '',
        comName: '',//公司名称
        comPosition: '',//公司职位
        telePhone: '',//手机号码
        phoneNum: '',//座机号码
        provinceList: [],//所有省份列表
        cityList: [{ id: "0", name: "请选择城市" }],//所有城市列表
        comAddress: '',//公司地址
        currTabIndex: 0,//当前页签
        province: null,//当前选中省份
        city: null,//当前选中城市
        checkCode: '',//验证码
        idCardNo: '',//身份证
        LawyerLicenseNo:'',//律师执业证编号
        avatar: '',//律师免冠照
        licensePhoto: '',//执业证照片
        IDPhoto1: '',//身份证正面
        IDPhoto2: '',//身份证反面
        IsSHowInfo: false,//是否公开信息 0隐藏 1公开，默认隐藏
        isAccept: false,//是否接受易劳协议 
        checkCodeTime: 0,
        pId:$routeParams.id//获取分享注册链接
    };
    document.title = pageData.title;
    //commService.postData

    var uploader = $scope.uploader = new FileUploader({
        url: '/Handler/UeditorController.ashx?action=uploadimage&configPath=ubiConfig.json&noCache=' + Math.random(),
    });

    var uploader1 = $scope.uploader1 = new FileUploader({
        url: '/Handler/UeditorController.ashx?action=uploadimage&configPath=ubiConfig.json&noCache=' + Math.random(),
    });
    var uploader2 = $scope.uploader2 = new FileUploader({
        url: '/Handler/UeditorController.ashx?action=uploadimage&configPath=ubiConfig.json&noCache=' + Math.random(),
    });
    var uploader3 = $scope.uploader3 = new FileUploader({
        url: '/Handler/UeditorController.ashx?action=uploadimage&configPath=ubiConfig.json&noCache=' + Math.random(),
    });

    // FILTERS

    uploader.filters.push({
        name: 'imageFilter',
        fn: function (item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });

    uploader1.filters.push({
        name: 'imageFilter',
        fn: function (item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });
    uploader2.filters.push({
        name: 'imageFilter',
        fn: function (item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });
    uploader3.filters.push({
        name: 'imageFilter',
        fn: function (item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });

    // CALLBACKS
    uploader.onAfterAddingAll = function (addedFileItems) {
        uploader.uploadAll();
    };
    uploader.onCompleteItem = function (fileItem, response, status, headers) {
        if (response.state.toLowerCase() == "success") {
            pageData.licensePhoto = response.url;
        }
    };

    uploader1.onAfterAddingAll = function (addedFileItems) {
        uploader1.uploadAll();
    };
    uploader1.onCompleteItem = function (fileItem, response, status, headers) {
        if (response.state.toLowerCase() == "success") {
            pageData.IDPhoto1 = response.url;
        }
    };

    uploader2.onAfterAddingAll = function (addedFileItems) {
        uploader2.uploadAll();
    };
    uploader2.onCompleteItem = function (fileItem, response, status, headers) {
        if (response.state.toLowerCase() == "success") {
            pageData.IDPhoto2 = response.url;
        }
    };

    uploader3.onAfterAddingAll = function (addedFileItems) {
        uploader3.uploadAll();
    };
    uploader3.onCompleteItem = function (fileItem, response, status, headers) {
        if (response.state.toLowerCase() == "success") {
            pageData.avatar = response.url;
        }
    };

    pageFunc.clickFileDiv = function () {
        // $("#fileUserPhoto").click();
    }

    pageFunc.init = function () {
        var ProChoose = { id: "0", name: "请选择省份" };
        pageData.city = pageData.cityList[0];
        //获取省份
        commService.getGetKeyVauleDatas({
            type: 'province'
        }, function (data) {
            pageData.provinceList = data.list;
            pageData.provinceList.unshift(ProChoose);
            pageData.province = pageData.provinceList[0];
            console.log(data);
        }, function () { });

    };
    pageFunc.selectProvince = function () {
        var CityChoose = { id: "0", name: "请选择所属城市" };
        //console.log(pageData.province);
        //获取城市
        commService.getGetKeyVauleDatas({
            type: 'city',
            prekey: pageData.province.id
        }, function (data) {
            pageData.cityList = data.list;
            pageData.cityList.unshift(CityChoose);
            pageData.city = 0;
            pageData.city = pageData.cityList[0];
            // console.log(data);
        }, function (data) { });
    };

    pageFunc.register = function () {
       // pageFunc.checkAll();
        if (pageFunc.checkAll() == true)
        {
            var model = {};
            model.pId = pageData.pId;
            model.name = pageData.Name;
            model.email = pageData.Mail;
            model.pwd = pageData.pwd;
            model.IsSHowInfo = pageData.IsSHowInfo;
            model.vercode = pageData.checkCode;
            if (pageData.currTabIndex == 1) {
                model.company = pageData.comName;
                model.idCardNo = pageData.idCardNo;
                model.LawyerLicenseNo=pageData.LawyerLicenseNo;
                model.postion = pageData.comPosition;
                model.phone = pageData.telePhone;
                model.tel = pageData.phoneNum;
                model.province = pageData.province.id;
                model.city = pageData.city.id;
                model.avatar = pageData.avatar;
                model.licensePhoto = pageData.licensePhoto;
                model.IDPhoto1 = pageData.IDPhoto1;
                model.IDPhoto2 = pageData.IDPhoto2;
                model.companyaddress = pageData.comAddress;
                model.action = "LawyerRegister";
            }
            else {
                model.action = "UserRegister";
            }
            if (pageData.isAccept == true) {
                userService.register(model, function (data) {
                    if (data.isSuccess) {
                        $scope.$emit('loginStatusChange', 'loginStatusChange');
                        alert('注册成功', 1, 2, function () {
                            $scope.go('#/index');
                        });
                        // $scope.go('#/my');
                    }
                    else if (data.errcode == 10016) {
                        alert('验证码错误', 2);
                    }
                    else if (data.errcode == 10020) {
                        alert('邮箱已被注册', 2);
                    }
                    else if (data.errcode == 10021) {
                        alert('手机已被注册', 2);
                    }
                    else if (data.errcode == 10022) {
                        alert('手机格式错误', 2);
                    }
                    else {
                        alert('注册失败');
                    }
                }, function (data) {
                    alert('注册失败');
                });
            }
            else {
                alert("您需要同意易劳协议才能注册！");
            }
        }        
    }
    //验证邮箱
    pageFunc.getEmailCheckCode = function () {
       // pageFunc.checkAll();
        if (pageFunc.checkAll() == true)
        {
            userService.getEmailCheckCode(
            pageData.Mail,
            function (data) {
                if (data.isSuccess) {
                    alert("验证码已发送至您填写的邮箱中！");
                    pageData.checkCodeTime = 60;
                    pageFunc.CheckCodeInterval = $interval(function () {
                        if (pageData.checkCodeTime <= 0) {
                            $interval.cancel(pageFunc.CheckCodeInterval);
                            pageData.checkCodeTime = 0;
                        }
                        else {
                            pageData.checkCodeTime--;
                        }
                    }, 1000, 0);
                }
                else {
                    alert("发送失败！");
                }
            },
            function () {
            });
        }        
    }

    pageFunc.checkAll = function () {
        if (pageData.Name == "") {
            alert("您的姓名为空!");
            return false;
        }
        if (pageData.Name == "匿名用户") {
            alert("该名称被系统占用，请填写其他用户名！");
            return false;
        }
        if (pageData.Mail == "") {
            alert("您的邮箱为空！");
            return false;
        }
        if (pageData.pwd == "") {
            alert("您的密码为空！");
            return false;
        }
        if (pageData.pwd2 == "") {
            alert("您的确认密码为空！");
            return false;
        }
        if (pageData.pwd != pageData.pwd2) {
            alert("两次密码不一致！");
            return false;
        }

        if (pageData.currTabIndex == 1) {
            if (pageData.comName == "") {
                alert("您的律所/公司名称为空！");
                return false;
            }
            if (pageData.idCardNo == "") {
                alert("您的身份证为空！");
                return false;
            }
            if(pageData.LawyerLicenseNo=="")
            {
                alert("您的律师执业证号为空！");
                return false;
            }
            if (pageData.avatar == "") {
                alert("您的免冠照没有上传！");
                return false;
            }
            if (pageData.licensePhoto == "") {
                alert("您的执业证没有上传！");
                return false;
            }
            if (pageData.IDPhoto1 == "") {
                alert("您的身份证正面照没有上传");
                return false;
            }
            if (pageData.IDPhoto2 == "") {
                alert("您的身份证反面照没有上传！");
                return false;
            }
            if (pageData.comPosition == "") {
                alert("您的职位为空！");
                return false;
            }
            if (pageData.telePhone == "") {
                alert("您的手机号为空！");
                return false;
            }
            if (pageData.phoneNum == "") {
                alert("您的座机为空！");
                return false;
            }
            if (pageData.province == 0) {
                alert("请选择省份");
                return false;
            }
            if (pageData.city == 0) {
                alert("请选择城市");
                return false;
            }
            if (pageData.comAddress == "") {
                alert("您的律所/公司地址为空！");
                return false;
            }
            if (!pageFunc.isCardNo(pageData.idCardNo)) {
                alert("您输入的身份证格式不正确！");
                return false;
            }
            if (!pageFunc.isPhone(pageData.telePhone)) {
                alert("您的手机号格式不正确！");
                return false;
            }
            if (!pageFunc.isTel(pageData.phoneNum)) {
                alert("您的座机格式不正确！您可填写类似格式：021-60825088 或 0427-7531992 或 76423865");
                return false;
            }
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
    pageFunc.isTel = function (tel) {
        var regPhone = /^(([0\+]\d{2,3}-)?(0\d{2,3})-)?(\d{7,8})(-(\d{3,}))?$/;
        //  var phone = "021-60825088";//"0427-7531992"; //"76423865"; 
        if (regPhone.test(tel)) {
            return true;
        }
        else {
            return false;
        }
    }
    pageFunc.isCardNo = function (card) {
        // 身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是校验位，可能为数字或字符X  
        var reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;

      //  var reg = /(^d{15}$)|(^d{18}$)|(^d{17}(d|X|x)$)/;
        if (reg.test(card) === false) {
            // alert("身份证输入不合法");
            return false;
        }
        return true;
    };

    pageFunc.init();
}]);