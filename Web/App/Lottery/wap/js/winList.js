
var winListVm = new Vue({
        el: '#wrapWinList',
        data: {
            handler: '/Serv/AwardApi.ashx',
            lottoryId: GetParm('lottoryId'),
            lottery: {
                winRecord:[]
            },
            dialog: {
                show: false,
                title: '系统提示',
                msg:''
            },
            modalSubmitUserInfo: {
                show: false,
                name:'',
                phone:''
            },
            currOpearteItem:{}
        },
        methods: {
            init: function () {
                var _this = this;
                $.getJSON(this.handler + '?'+ $.param({
                    action: 'GetLottery',
                    id: this.lottoryId
                }), function (resp) {                   
                    //console.log('init_name', resp.winRecord[0].org_obj.record_user_name);
                    //$('#loadingToast').hide();
                    _this.closeLoading();
                    $('#wrapWinList').show();
                    if (resp.IsSuccess) {                        
                        //winListVm.$data.lottery = resp.Result;
                        _this.lottery = resp.Result;
                    } else {
                        _this.dialog = {
                            show: true,
                            title: '系统提示',
                            msg:resp.Msg
                        };
                    }
                });

            },
            loading: function () {
                $('#loadingToast').show();
            },
            closeLoading: function () {
                $('#loadingToast').hide();
            },
            closeDialog: function () {
                this.dialog.show = false;
            },
            openSubmitUserInfo: function (item) {
                this.currOpearteItem = item;
                this.modalSubmitUserInfo = {
                    show: true,
                    name: this.lottery.currUserName,
                    phone: this.lottery.currUserPhone
                };
            },
            submitUserInfo: function () {

                this.modalSubmitUserInfo.name = _.trim(this.modalSubmitUserInfo.name);
                this.modalSubmitUserInfo.phone = _.trim(this.modalSubmitUserInfo.phone);

                //TODO:提交领奖信息
                if (!this.modalSubmitUserInfo.name) {
                    alert("请输入领奖姓名");
                }
                if (!this.modalSubmitUserInfo.phone) {
                    alert("请输入领奖手机");
                }

                this.loading();

                var _this = this;

                $.post(this.handler, {
                    action: 'SubmitInfo',
                    id: this.lottoryId,
                    aid: this.currOpearteItem.id,
                    rid: this.currOpearteItem.rid,
                    name: this.modalSubmitUserInfo.name,
                    phone: this.modalSubmitUserInfo.phone
                }, function (resp) {
                    _this.closeLoading();
                    resp = JSON.parse(resp);

                    if (resp.IsSuccess) {

                        _this.currOpearteItem.record_user_name = _this.modalSubmitUserInfo.name;
                        _this.currOpearteItem.record_user_phone = _this.modalSubmitUserInfo.phone;
                        _this.modalSubmitUserInfo.show = false;

                    } else {
                        alert(resp.Msg);
                    }
                });



            },
            cancelSubmitUserInfo: function () {
                this.modalSubmitUserInfo.show = false;
            },
            //领奖
            getAward: function (item) {
                var _this = this;
                layer.open({
                    content: '确认领奖？',
                    btn: ['确认', '取消'],
                    yes: function (index) {

                        _this.loading();
                        layer.close(index);

                        $.post(_this.handler, {
                            action: 'GetAward',
                            id: _this.lottoryId,
                            aid: item.id,
                            rid: item.rid
                        }, function (resp) {
                            resp = JSON.parse(resp);
                            _this.closeLoading();
                            if (resp.IsSuccess) {
                                item.is_reveice = true;
                            } else {
                                alert(resp.Msg);
                            }

                        });

                    }
                });
            }
        }
    });

$(function () {
    //$('#loadingToast').show();
    winListVm.loading();
    winListVm.init();
    document.title = "中奖记录";
});
