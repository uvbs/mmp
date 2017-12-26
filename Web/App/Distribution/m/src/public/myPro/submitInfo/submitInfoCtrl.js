/**
 * Created by add on 2016/1/26.
 */
distributionmodule.controller('submitInfoCtrl', ['$sessionStorage','$timeout', 'commService', function ($sessionStorage,$timeout, commService) {
    /* jshint validthis : true*/
    var vm = this;
    vm.title = '财富商机';
    vm.currUseInfo = $sessionStorage.currUseInfo;  //当前用户信息，可以判断是否是分销员
    vm.fields = []; //后台传回来的字段
    vm.dataInfo = {
        contact: '', //是	联系人
        phone: '',   //是	联系方式
        weixin: '',  //否	微信
        company: '',  //否	单位
        project_intro: '备注'  //否	项目推荐  推荐理由
    };//提交信息
    vm.submitInfo = submitInfo;
    //vm.checkIsNull = checkIsNull;
    //vm.clearAll = clearAll;
    vm.backToList = backToList;
    vm.getProjectFields = getProjectFields; //获取项目提交字段

    init();
    function init() {
        getProjectFields();
    }

    //function checkIsNull() {
    //    if (vm.dataInfo.porject_name == '') {
    //        alert('项目名称不能为空');
    //        return false;
    //    }
    //    if (vm.dataInfo.contact == '') {
    //        alert('姓名不能为空');
    //        return false;
    //    }
    //    if (vm.dataInfo.phone == '') {
    //        alert('手机号码不能为空');
    //        return false;
    //    }
    //    if (vm.dataInfo.weixin == '') {
    //        alert('微信不能为空');
    //        return false;
    //    }
    //    if (vm.dataInfo.company == '') {
    //        alert('单位不能为空');
    //        return false;
    //    }
    //    if (vm.dataInfo.project_intro == '') {
    //        alert('推荐理由不能为空');
    //        return false;
    //    }
    //    return true;
    //}
    //
    //function clearAll() {
    //    vm.dataInfo.porject_name = '';
    //    vm.dataInfo.contact = '';
    //    vm.dataInfo.phone = '';
    //    vm.dataInfo.weixin = '';
    //    vm.dataInfo.company = '';
    //    vm.dataInfo.project_intro = '推荐理由';
    //}

    function submitInfo() {
        var reqData = {
            //ActivityID: vm.signData.activity_id,
        };
        var signData = vm.fields;
        for (var i = 0; i < signData.length; i++) {
            if (signData[i].input && signData[i].input != '') {
                reqData[signData[i].field] = $.trim(signData[i].input);
            }
        }
        commService.addProject(reqData, function (data) {
            if (data.status) {
                alert('添加成功');
                $timeout(function () {
                    getProjectFields();
                }, 2000);
            } else {
                alert(data.msg);
            }
        }, function (data) {
            alert(data.msg);
        });
    }

    function backToList() {
        window.location.href = '#/mystatic';
    }

    function getProjectFields() {
        commService.getProjectFields({}, function (data) {
            if (data.status) {
                vm.fields = data.result.project_field_list;
                if (data.result.project_field_list) {
                    for (var i = 0; i < data.result.project_field_list.length; i++) {
                        vm.fields[i].input = '';
                    }
                }
                console.log('项目提交信息', vm.fields);
            } else {
                alert(data.msg);
            }
        }, function (data) {
            alert(data.msg);
        });
    }
}]);