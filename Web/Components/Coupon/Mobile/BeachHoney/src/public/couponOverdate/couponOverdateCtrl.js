couponmodule.controller('couponOverdateCtrl', ['$scope', '$http', function ($scope, $http) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        couponDetail: ''

    };
    var id = localStorage.getItem('id');
    pageFunc.loadData = function () {
        //
        $http.get("/Components/Coupon/Handler/API.ashx?action=getmycoupon&cardcoupontype=entranceticket&id=" + id)
                        .success(function (resp) {

                            pageData.couponDetail = resp;



                        });
        //


    }

    pageFunc.loadData();

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
            return date.getFullYear() + "年" + month + "月" + currentDate + "日";


        }
    });