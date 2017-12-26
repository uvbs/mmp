var commVm = new Vue({
    el: '.wrapComm',
    data: {
        id: GetParm('id'),
        withdraw_back_url: "",
        form: {},
        bottom_text: baseData.wrapBottomHtml
    },
    methods: {
        init: function () {
            this.withdraw_back_url = GetParm('withdraw_back_url');
            this.loadData();
        },
        loadData: function () {
            var _this = this;
            $('#loadingToast').show();
            $.ajax({
                type: 'post',
                url: '/Serv/API/Flow/Get.ashx',
                data: {
                    id: _this.id
                },
                dataType: 'json',
                success: function (resp) {
                    $('#loadingToast').hide();
                    if (resp.status) {
                        if (resp.result.status == 9) {
                            if (resp.result.flow_key == 'Withdraw') {
                                resp.result.status_s = '提现成功';
                            } else if (resp.result.flow_key == 'OfflineRecharge') {
                                resp.result.status_s = '充值成功';
                            } else {
                                resp.result.status_s = '审核通过';
                            }
                        }
                        else if (resp.result.status == 8) {
                            resp.result.status_s = '审核未过';
                        }
                        else if (resp.result.status == 11) {
                            resp.result.status_s = '申请取消';
                        }
                        else if (resp.result.status == 10) {
                            resp.result.status_s = '已取消';
                        }
                        else if (resp.result.status == 12) {
                            resp.result.status_s = '拒绝取消';
                        } else if (resp.result.status == 0) {
                            resp.result.status_s = '处理中';
                        }
                        _this.form = resp.result;
                    }
                    else {
                        zcAlert(resp.msg);
                    }
                }
            });
        },
        cancel: function () {
            var _this = this;
            layer.open({
                title:'取消原因',
                content: '<textarea class="weui-textarea popuo-content" placeholder="请填写取消原因" rows="5" maxlength="300"></textarea>',
                className: 'popuo-form',
                btn: ['提交', '关闭'],
                shadeClose: false,
                yes: function (index) {
                    var content = $.trim($('.popuo-form .popuo-content').val());
                    content = $("<div>").text(content).html().split("\n").join("<br />");
                    if (!content) return;
                    $('#loadingToast').show();
                    $.ajax({
                        type: 'post',
                        url: '/Serv/API/Flow/Cancel.ashx',
                        data: {
                            id: _this.id,
                            content: content
                        },
                        dataType: 'json',
                        success: function (resp) {
                            $('#loadingToast').hide();
                            if (resp.status) {
                                sessionStorage.removeItem('withrawData');
                                zcAlert(resp.msg, '', 2, function () {
                                    if (_this.withdraw_back_url) {
                                        window.location.href = '/app/wap/Withdraw.aspx?back_url=' + _this.withdraw_back_url;
                                    } else {
                                        window.location.href = '/app/wap/Withdraw.aspx';
                                    }
                                    
                                });
                            }
                            else {
                                zcAlert(resp.msg);
                            }
                        }
                    });
                    console.log(content);
                    //layer.close(index);
                }
            });
        }
    }
});
$(function () {
    commVm.init();
});