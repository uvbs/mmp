ubimodule.controller("edituserCtrl", ['$scope', '$routeParams', 'userService', 'commService', 'FileUploader'
    , function ($scope, $routeParams, userService, commService, FileUploader) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '编辑资料 - ' + baseData.slogan,
            id: $routeParams.id,
            userInfo: [],
            provinceList: [],
            cityList: [],
            IsSHowInfo:false,
        };
        var uploader = $scope.uploader = new FileUploader({
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

        // CALLBACKS
        uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
            //console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader.onAfterAddingFile = function (fileItem) {
            console.info('onAfterAddingFile', fileItem);
        };
        uploader.onAfterAddingAll = function (addedFileItems) {
            uploader.uploadAll();
            //console.info('onBeforeUploadItem', item);
        };
        uploader.onBeforeUploadItem = function (item) {
            //console.info('onBeforeUploadItem', item);
        };
        uploader.onProgressItem = function (fileItem, progress) {
            //console.info('onProgressItem', fileItem, progress);
        };
        uploader.onProgressAll = function (progress) {
            //console.info('onProgressAll', progress);
        };
        uploader.onSuccessItem = function (fileItem, response, status, headers) {
            //console.info('onSuccessItem', fileItem, response, status, headers);
        };
        uploader.onErrorItem = function (fileItem, response, status, headers) {
            //console.info('onErrorItem', fileItem, response, status, headers);
        };
        uploader.onCancelItem = function (fileItem, response, status, headers) {
            //console.info('onCancelItem', fileItem, response, status, headers);
        };
        uploader.onCompleteItem = function (fileItem, response, status, headers) {
            if (response.state.toLowerCase() == "success")
            {
                pageData.userInfo.avatar = response.url;
            }
        };
        uploader.onCompleteAll = function () {
            //console.info('onCompleteAll');
        };

        document.title = pageData.title;

        pageFunc.init = function () {
          //  pageFunc.loadUser();
            pageFunc.loadProvince();
            userService.getEditUserInfo(function (data) {
                if (data.errcode) {
                    alert('找不到该用户');
                }
                else {                   
                    pageData.userInfo = data;
                    if (pageData.userInfo) {
                        if (pageData.userInfo.isShowInfo == "true") {
                            pageData.userInfo.isShowInfo = true;
                        }
                        else if (pageData.userInfo.isShowInfo == "false") {
                            pageData.userInfo.isShowInfo = false;
                        }
                    }
                    if (!pageData.userInfo.userProvinceCode) pageData.userInfo.userProvinceCode = 0;
                    if (!pageData.userInfo.userCityCode) pageData.userInfo.userCityCode = 0;
                }
            });
        }
        pageFunc.clickFileDiv = function () {
            $("#fileUserPhoto").click();
        }

        pageFunc.loadProvince = function () {
            //获取省份
            commService.getGetKeyVauleDatas({
                type: 'province'
            }, function (data) {
                pageData.provinceList = data.list;
            }, function () { });
        };
        pageFunc.selectProvince = function () {
            //获取城市
            commService.getGetKeyVauleDatas({
                type: 'city',
                prekey: pageData.userInfo.userProvinceCode
            }, function (data) {
                pageData.cityList = data.list;
                pageData.userInfo.userCityCode = 0;
            }, function (data) { });
        };

        pageFunc.loadUser = function () {
            userService.getUserInfo(pageData.id, function (data) {
                if (data.errcode) {
                    alert('找不到该用户');
                }
                else {
                    pageData.userInfo = data;
                    if (!pageData.userInfo.userProvinceCode) pageData.userInfo.userProvinceCode = 0;
                    if (!pageData.userInfo.userCityCode) pageData.userInfo.userCityCode = 0;
                }
            });
        }
        
        pageFunc.submitEditUser = function () {
            //console.log(pageData.userInfo.isShowInfo);
            //if (pageData.IsSHowInfo == "") {
            //    pageData.IsSHowInfo = 0;
            //}
            //else if (pageData.IsSHowInfo == true) {
            //    pageData.IsSHowInfo = 1;
            //}
            if (pageData.userInfo.userIntroduction && pageData.userInfo.userIntroduction.length > 500) {
                alert("个人介绍不能超过500个汉字！");
                return;
            }
            
            userService.updateUserInfo({
                    avatar : pageData.userInfo.avatar,
                    name : pageData.userInfo.userName,
                    sex : pageData.userInfo.userSexInt,
                    LawyerLicenseNo:pageData.userInfo.LawyerLicenseNo,
                    phone : pageData.userInfo.userPhone,
                    tel : pageData.userInfo.userTel,
                    province : pageData.userInfo.userProvinceCode,
                    city : pageData.userInfo.userCityCode,
                    introduction : pageData.userInfo.userIntroduction,
                    company : pageData.userInfo.userCompany,
                    postion : pageData.userInfo.userPostion,
                    companyaddress : pageData.userInfo.userCompanyAddress,
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
                    else if (data.errcode == 10013) {
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
    }
]);
