couponmodule.controller('couponListCtrl', ['$scope', '$http', function ($scope, $http) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        currTabIndex: 0,
        listunuse: [],
        listused: [],
        listoverdate: []

    };

    pageFunc.init = function () {
        pageData.currTabIndex = 0;


    };

    $scope.gouse = function (id) {
        localStorage.setItem('id', id);
        window.location.href = 'index.aspx#/couponUse'; //未使用
    }
    $scope.goused = function (id) {
        localStorage.setItem('id', id);
        window.location.href = 'index.aspx#/couponUsed'; //已经使用
    }
    $scope.gooverdate = function (id) {
        localStorage.setItem('id', id);
        window.location.href = 'index.aspx#/couponOverdate'; //已经过期
    }

    //未使用
    pageFunc.loadDataUnUse = function () {

        //
        $http.get("/Components/Coupon/Handler/API.ashx?action=getmycouponlist&cardcoupontype=entranceticket&pageindex=1&pagesize=10&status=0")
                        .success(function (resp) {
                            for (var i = 0; i < resp.list.length; i++) {
                                pageData.listunuse.push(resp.list[i]);
                                
                            }





                        });
        //


                    }
    //已经使用
    pageFunc.loadDataUsed= function () {

                        //
                        $http.get("/Components/Coupon/Handler/API.ashx?action=getmycouponlist&cardcoupontype=entranceticket&pageindex=1&pagesize=10&status=1")
                        .success(function (resp) {
                            for (var i = 0; i < resp.list.length; i++) {
                                pageData.listused.push(resp.list[i]);
                                
                            }





                        });
                        //


                    }
    //已经过期
    pageFunc.loadDataOverDate = function () {
                        //
                        $http.get("/Components/Coupon/Handler/API.ashx?action=getmycouponlist&cardcoupontype=entranceticket&pageindex=1&pagesize=10&status=2")
                        .success(function (resp) {
                            for (var i = 0; i < resp.list.length; i++) {
                                pageData.listoverdate.push(resp.list[i]);

                            }





                        });
                        //


                    }
    pageFunc.init();
    pageFunc.loadDataUnUse(); //未使用
    pageFunc.loadDataUsed(); //已经使用
    pageFunc.loadDataOverDate(); //已经过期
} ]).filter(
    'formartdate', function () {
        return function (value) {

            function padLeft(str, min) {
                if (str >= min)
                    return str;
                else
                    return "0" + str;
            }
            if (value == null || value == "") {
                return "";
            }
            var date = new Date(parseInt(value, 10));
            var month = padLeft(date.getMonth() + 1, 10);
            var currentDate = padLeft(date.getDate(), 10);
            var hour = padLeft(date.getHours(), 10);
            var minute = padLeft(date.getMinutes(), 10);
            var second = padLeft(date.getSeconds(), 10);
            return date.getFullYear() + "-" + month + "-" + currentDate;


        }
    });