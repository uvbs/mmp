$(function () {

    var selectTcType = getCookie('selectTcType');
    var selectIds = getCookie('selectIds');

    var tcConfig = GetTcConfig(selectTcType);

    maxSelect = tcConfig.maxSelect; //套餐样式数量

    tcType = selectTcType; //套餐类型
    tcName = tcConfig.tcName; //套餐名称

    $('#lbTcName').html(tcConfig.tcName);
    $('#lbSelectMaxCount').html(tcConfig.maxSelect);
    $('#lbSelectCount').html(tcConfig.maxSelect);

    //初始化菜单选择
    var arrTmp = new Array();
    arrTmp = selectIds.split(',');

    $('.item').each(function () {
        var pid = $(this).attr('pid');
        if (pid.indexOf(',') > 0) {
            return;
        }
        if (arrTmp.Contains(pid)) {
            addCurrSelectItem(pid, this);
        }
    });
    //    document.write('套餐：' + selectTcType + '<br />');
    //    document.write('配菜：' + selectIds);



});