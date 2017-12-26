

$(function () {

    currSelectItem = new Array();
    tcType = GetParm('tcType');

    setTcType(tcType);

//    $('#btnConfirm').click(function () {
//        if (currSelectItem.length < maxSelect) {
//            $('#lbdlgOrderSelect').html('提交订单还需选择' + (maxSelect - currSelectItem.length) + '个菜！');
//            $('#dlgOrderSelect').popup('open');
//            return false
//        }
//        else {
//            var selectIds = currSelectItem.join(',');
//            var selectTcType = tcType;

//            SetCookie('selectIds', selectIds);
//            SetCookie('selectTcType', selectTcType);

//            window.location.href = 'OrderSubmit.aspx';
//        }
//    });
   

});

//设置套餐类型
function setTcType(tcType) {
    switch (tcType) {
        case '1':
            maxSelect = 3;
            tcName = '"温馨套餐"';
            break;
        case '2':
            maxSelect = 4;
            tcName = '"小康套餐"';
            break;
        case '3':
            maxSelect = 5;
            tcName = '"美满套餐"';
            break;
    }
    $('#lbTcName').html(tcName);
    $('#lbSelectMaxCount').html(maxSelect);
}

//提交订单
function submitOrder() {

}

