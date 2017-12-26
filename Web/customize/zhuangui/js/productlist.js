

var listVm = new Vue({
    el: '.vue-el',
    data: {
        handler: '/Serv/API/Mall/Product.ashx',
        supplierHandler: '/Serv/API/User/Supplier/GetByUserId.ashx',
        rows: 1000,
        productData: { page: 1, total: 0, list: [] },
        supplierInfo:{},
        isLoading: true,
        isLoadAll: false,
        tagList: [{ text: '全部', value: "" }, { text: '男生必购', value: "男生必购" }, { text: '女生必购', value: "女生必购" }, { text: '选购', value: "选购" }],
        tag: ""
    },
    methods: {
        init: function () {
            this.loadSupplierInfo();
            this.checkSupplierUserId();
            this.bindScroll();
            this.loadData();
        },
        loadData: function () {
            var _this = this;
            _this.isLoading = true;
            $.ajax({
                type: 'post',
                url: _this.handler,
                data: {
                    Action: "List",
                    supplierUserId: this.getSupplierUserId(),
                    tags:this.tag,
                    pageIndex: this.productData.page,
                    pageSize: this.rows

                },
                dataType: 'json',
                success: function (resp) {
                    _this.isLoading = false;
                    if (resp.list.length == 0) {
                        _this.isLoadAll = true;

                    } else {
                        for (var i = 0; i < resp.list.length; i++) {
                            resp.list[i].img_url += "?x-oss-process=image/resize,m_pad,w_392,h_392,color_ffffff/format,png";

                        }
                    }
                    _this.productData.total = resp.totalcount;


                    _this.productData.list = _this.productData.list.concat(resp.list);

                }
            });
        },
        loadSupplierInfo: function () {
            var _this = this;
           
            $.ajax({
                type: 'post',
                url: _this.supplierHandler,
                data: {
                   
                    userId: this.getSupplierUserId()
                    

                },
                dataType: 'json',
                success: function (resp) {
                    _this.supplierInfo = resp.result;
                    if (_this.supplierInfo.desc != null && _this.supplierInfo.desc.length>30) {
                        _this.supplierInfo.desc = _this.supplierInfo.desc.substring(0, 30)+"...";
                        
                    }

                }
            });
        },
        selectTag: function (tag) {
           
            if (tag === this.tag) return;
            this.tag = tag;
            this.productData = { page: 1, total: 0, list: [] };
            this.loadData();
        },
        bindScroll: function () {
            //console.log($(window).scrollTop());
            var _this = this;
            $(window).bind('scroll', function (e) {


                var _wh = $(window).height();
                var _st = $('body').get(0).scrollTop;
                var _sh = $('body').get(0).scrollHeight;
                if ((_sh - _st - _wh < 10) && (!_this.isLoadAll)) {
                    _this.loadMore();
                } else {



                }
            });
        },
        loadMore: function () {
            this.productData.page++;
            this.loadData();
        },
        toDetail: function (item) {
            window.location.href = '/customize/shop/?v=1.0&ngroute=/productList#/productDetail/' + item.product_id;
        },
        loginOut: function (item) {
            window.localStorage.removeItem("supplierUseId");
            window.location.href = 'Login.aspx';
        },
        getSupplierUserId: function () {

            var supplierUserId = window.localStorage.getItem("supplierUseId");
            if (supplierUserId != null && supplierUserId != "") {
                return supplierUserId;
            }
            return "";


        },
        checkSupplierUserId: function () {

            if (this.getSupplierUserId() == "") {

                window.location.href = "Login.aspx";
            }


        },


    }
});

listVm.init();

