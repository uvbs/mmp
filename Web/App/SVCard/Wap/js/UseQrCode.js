

var vm = new Vue({
    el: '.vueEl',
    data: {
        handler: '/Serv/API/Common/QrCode.ashx',//处理路径
        imgSrc:"",//二维码地址
        code: "http://" + window.location.host + "/App/SvCard/Wap/Use.aspx?id=" + GetParm('id'),//二维码内容
        isShow:false
    },
    methods: {//方法集合
        createQrCode: function () {//加载二维码信息
            var _this = this;
            $.ajax({
                type: 'post',
                url: this.handler ,
                data: {
                    code: this.code
                    
                },
                dataType: 'json',
                success: function (resp) {
                    _this.closeLoading();
                    if (resp.status==true) {
                        _this.imgSrc = resp.result.qrcode_url;
                        _this.isShow = true;
                        
                    } else {
                        alert("生成二维码失败");
                    }
                    

                }
            });
        },
        loading: function () {
            $('#loadingToast').show();
        },
        closeLoading: function () {
            $('#loadingToast').hide();
        },
        setPageTitle: function () {//设置标题
            SetPageTitle(this.card.name);
        }

    }
});
vm.loading();
vm.createQrCode();//生成二维码

