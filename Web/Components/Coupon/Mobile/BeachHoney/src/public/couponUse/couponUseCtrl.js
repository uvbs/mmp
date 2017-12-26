couponmodule.controller('couponUseCtrl', ['$scope', '$http', function ($scope, $http) {
    var pageFunc = $scope.pageFunc = {};
    var pageData = $scope.pageData = {
        couponDetail: '',
        currentUserInfo:''

    };

    function layermsg(msg) {
        layer.open({
            content: msg,
            btn: ['OK']
        });
    }

    function layerConfirm() {

        layer.open({
            content: '确定使用门票？',
            btn: ['OK', 'Cancel'],
            shadeClose: false,
            yes: function () {



            }, no: function () {


            }
        });


    }

    var id = localStorage.getItem('id');


    pageFunc.userCoupon = function () {

        var confirmCode = document.getElementById("txtConfirmCode").value;
        if (confirmCode == "") {
            layermsg("请输入密码");
            return false;
        }
        if (confirmCode != "7788") {
            layermsg("密码错误");
            return false;
        }
        //
        //
        $http.get("/Components/Coupon/Handler/API.ashx?action=usemycoupon&cardcoupontype=entranceticket&id=" + id)
                        .success(function (resp) {

                            if (resp.errcode == 0) {
                                window.location.href = 'index.aspx#/couponUsed'; //已经使用
                            }
                            else {
                                layermsg(resp.errmsg);
                            }



                        });
        //

    }

    pageFunc.loadData = function () {
        //
        $http.get("/Components/Coupon/Handler/API.ashx?action=getmycoupon&cardcoupontype=entranceticket&id=" + id)
                        .success(function (resp) {

                            pageData.couponDetail = resp;



                        });
        //


    }

    pageFunc.getCurrentUserInfo = function () {
        //
        $http.get("/Components/Coupon/Handler/API.ashx?action=getmyinfo")
                        .success(function (resp) {
                        pageData.currentUserInfo = resp;

                        });
        //


    }

    pageFunc.loadData();
    pageFunc.getCurrentUserInfo();

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