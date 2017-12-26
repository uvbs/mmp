ubimodule.controller("rechargeCtrl", ['$scope', '$routeParams', 'commService', 'userService',
    function ($scope, $routeParams, commService, userService) {
        var pageFunc = $scope.pageFunc = {};
        var pageData = $scope.pageData = {
            title: '在线充值积分 - ' + baseData.slogan,
            id: $routeParams.id,
            type: 1,
            score: 100,
            fee: 100,
            totalFee: 100,
            currTabIndex:1,
            vipData:[]
        };
        document.title = pageData.title;
        pageFunc.init = function () {
            userService.getRechargeConfig(
                function (data) {
                    if (data.isSuccess) {
                        pageData.vipData=data;
                        pageData.fee = data.returnValue
                    }
                },
                function () {
                });
        }

        pageFunc.getTotalFee = function () {
            if (Number(pageData.score)) {
                pageData.totalFee = pageData.score * pageData.fee / 100
            }
            else {
                pageData.totalFee = 0;
            }
            return pageData.totalFee;
        }

        pageFunc.submit = function () {
            if (pageData.score == 0) {
                alert("积分不能为0！");
                return;
            }
            if (pageData.type == 1) {
                userService.getWeixinPreOrder({
                    score: pageData.score
                    },
                    function (data) {
                        if (data.isSuccess && data.returnObj) {
                            commService.showQRCodeModal(data.returnObj.orderId, "扫描二维码支付", "/Handler/ImgHandler.ashx?v=" + encodeURIComponent(data.returnObj.code_url));
                        }
                        else if (data.errcode == 10011) {
                            alert("充值金额不能为0！");
                            return;
                        }
                        else if (data.errcode == 10010) {
                            alert("您还没有登录！");
                            return;
                        }
                        else {
                            alert("生成预支付二维码失败！");
                            return;
                        }
                    },
                    function () { })
            }
            else {
                alert("平台暂时仅支持微信支付！");
            }
        }

        pageFunc.init();
    }
]);
