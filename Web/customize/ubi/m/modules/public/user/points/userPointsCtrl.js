ubimodule.controller('userPointsCtrl', ['$scope', 'userService', 'commService', function ($scope, userService, commService) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        title: '我的积分',//'我的积分 - ' + baseData.slogan,
        currTabIndex: 0,
        historys: {
            pageIndex: 1,
            pageSize: 10,
            totalCount: 0,
            list: [],
            score: 0,
            fee: 100,
            totalFee: 100
        },
        userInfo: null,
        isBuy:false,
    };

    document.title = pageData.title;

    pageFunc.init = function () {
        var currUser = commService.getCurrUserInfo();
        userService.getUserInfo(currUser.id, function (data) {
            if (data.errcode) {
                alert('找不到该用户');
            }
            else {
                pageData.userInfo = data;
            }
        });

        userService.getRechargeConfig(
               function (data) {
                   if (data.isSuccess) {
                       pageData.historys.fee = data.returnValue
                   }
               },
               function () {
               });

        pageFunc.loadData(true);
    }

    pageFunc.loadData = function (isNew) {
        if (isNew) {
            pageData.historys.list = [];
            pageData.historys.pageIndex = 1;
            pageData.historys.totalCount = 0;
        }
        else {
            pageData.historys.pageIndex++;
        }
        var model = {
            pageIndex: pageData.historys.pageIndex,
            pageSize: pageData.historys.pageSize,
            balanceType:pageData.currTabIndex
        }

        userService.getScoreHistorys(model,
        function (data) {
            pageData.historys.totalCount = data.totalcount;
            for (var i = 0; i < data.list.length; i++) {
                pageData.historys.list.push(data.list[i]);
            }
        },
        function () {
        });
    }

    pageFunc.getTotalFee = function () {
        if (Number(pageData.historys.score)) {
            pageData.totalFee = pageData.historys.score * pageData.historys.fee / 100
        }
        else {
            pageData.totalFee = 0;
        }
        return pageData.totalFee;
    }

    pageFunc.submit = function () {
        if (pageData.historys.score == 0) {
            alert("积分不能为0！");
            return;
        }
        userService.getWeixinJSAPIPreOrder({
            score: pageData.historys.score
        },
        function (data) {
            if (data.isSuccess) {
                location.href = "/WxPay/WxPay.aspx?OrderId=" + data.returnValue;
            }
            else if (data.errcode == 10011) {
                alert("积分不能为0！");
                return;
            }
            else if (data.errcode == 10010) {
                alert("您还没有登录！");
                return;
            }
            else {
                alert("充值失败！");
                return;
            }
        },
        function () { })
    }

    pageFunc.buyScore = function () {
        pageData.isBuy = true;
    }

    pageFunc.loadMore = function () {
        pageFunc.loadData(false);
    }

    pageFunc.init();
}]);