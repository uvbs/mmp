
var vm = new Vue({
    el: '.weui-cells_form',
    data: {
        handler: '/Serv/API/User/Supplier/GetByCode.ashx',//处理路径
        componyCode: ''//专柜代码

    },
    watch: {//监视输入
        componyCode: function (val) {
            
        } 
    },
    methods: {//方法集合
        login: function () {
            var _this = this;
            if (_this.componyCode=="") {
                alert("请输入专柜码");
                return false;
            }
            _this.loading();
            $.ajax({
                type: 'post',
                url: _this.handler,
                data: {
                    code: _this.componyCode

                },
                dataType: 'json',
                success: function (resp) {
                    _this.closeLoading();
                    if (resp.status==true) {

                        window.localStorage.setItem("supplierUseId", resp.result.user_id);
                        window.location.href = "ProductList.aspx";

                    }
                    else {
                        alert(resp.msg);
                    }
                }
            });
        },
        loading: function () {
            $('#loadingToast').show();
        },
        closeLoading: function () {
            $('#loadingToast').hide();
        }


    }
});


