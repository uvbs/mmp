ubimodule.controller("applylawyerCtrl", ['$scope', '$routeParams', 'userService', 'FileUploader'
    , function ($scope, $routeParams, userService, FileUploader) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            id: $routeParams.id,
            title: '申请成为律师 - ' + baseData.slogan,
            data: {}
        };
        document.title = pageData.title;

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
        uploader.onAfterAddingAll = function (addedFileItems) {
            uploader.uploadAll();
        };
        uploader.onCompleteItem = function (fileItem, response, status, headers) {
            if (response.state.toLowerCase() == "success") {
                pageData.data.lawyerLicensePhoto = response.url;
            }
        };

        pageFunc.clickFileDiv = function () {
            $("#fileUserPhoto").click();
        }

        pageFunc.loadData = function () {
            userService.getUserInfoToApplyLawyer(
                pageData.id,
                function (data) {
                    pageData.data = data
                },
                function () {
                }
            );
            
        }

        pageFunc.submitApply = function () {
            userService.applyLawyer(
               pageData.data.idCardNo,
               pageData.data.lawyerLicensePhoto,
                function (data) {
                    if (data.isSuccess){
                        alert('提交申请完成', 1, 2000, function () {
                            window.location.href = "#/userspace/" + pageData.id;
                        });
                    }
                    else if (data.errcode == 10010)
                    {
                        $scope.showLogin(function () {
                            pageFunc.submitApply();
                        }, '您取消了登陆，提交申请必需先登录');
                        return;
                    }
                    else {
                        alert("提交申请失败");
                    }
                },
                function () {
                }
            );
        }
        pageFunc.loadData();
    }
]);
