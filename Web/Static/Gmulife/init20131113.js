
var maxSelect = 5; //套餐样式数量
var currSelectItem = new Array();
var tcType = 0; //套餐类型
var tcName = ''; //套餐名称

$(function () {
    $('.item').click(function () {
        var pid = $(this).attr('pid');
        if (pid.indexOf(',') > 0) {
            return;
        }
        if (currSelectItem.Contains(pid)) {
            removeCurrSelectItem(pid, this);
        }
        else {
            addCurrSelectItem(pid, this);
        }
    });

    $('#btnConfirm').click(function () {
        if (currSelectItem.length < maxSelect) {
            $('#lbdlgOrderSelect').html('提交订单还需选择' + (maxSelect - currSelectItem.length) + '个菜！');
            $('#dlgOrderSelect').popup('open');
            return false
        }
        else {
            var selectIds = currSelectItem.join(',');
            var selectTcType = tcType;

            SetCookie('selectIds', selectIds);
            SetCookie('selectTcType', selectTcType);

            window.location.href = 'OrderSubmit.aspx';
        }
    });

    $('#btnChange').click(function () {
        if (currSelectItem.length < maxSelect) {
            //            $('#lbdlgOrderSelect').html('提交订单还需选择' + (maxSelect - currSelectItem.length) + '个菜！');
            //            $('#dlgOrderSelect').popup('open');

            MAlert('提交订单还需选择' + (maxSelect - currSelectItem.length) + '个菜！');

            return false
        }
        else {
            var selectIds = currSelectItem.join(',');
            var selectTcType = tcType;

            SetCookie('selectIds', selectIds);
            SetCookie('selectTcType', selectTcType);

//            $('#lbdlgOrderSelect').html('换菜成功！');
//            $('#dlgOrderSelect').popup('open');
            MAlert('换菜成功！');
        }
    });


});


//添加选择项
function addCurrSelectItem(pid, itemObj) {
    if (currSelectItem.length + 1 > maxSelect) {
        //alert("该套餐只能选择" + maxSelect + "个菜");
        $('#lbDlgMsg').html("该套餐只能选择" + maxSelect + "个菜");
        $('#dlgMsg').popup('open');
    }
    else {
        currSelectItem.push(pid);
        $(itemObj).find(".select-shadow").css({ display: 'block' });
        $('#lbSelectCount').html(currSelectItem.length);
    }
}

//移除选择项
function removeCurrSelectItem(pid, itemObj) {
    currSelectItem.RemoveItem(pid);
    $(itemObj).find(".select-shadow").css({ display: 'none' });
    $('#lbSelectCount').html(currSelectItem.length);
}

//获取套餐配置
function GetTcConfig(tcType) {

    var result = {
        maxSelect: 0,
        tcName: ''
    }
    switch (tcType) {
        case '1':
            result.maxSelect = 3;
            result.tcName = '"温馨套餐"';
            break;
        case '2':
            result.maxSelect = 4;
            result.tcName = '"小康套餐"';
            break;
        case '3':
            result.maxSelect = 5;
            result.tcName = '"美满套餐"';
            break;
    }

    return result;
}

//
function MAlert(msg) {
    $('#lbDlgMsg').html(msg);
    $('#dlgMsg').popup('open');
}
