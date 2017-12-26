
var maxSelect = 3; //套餐样式数量
var currSelectItem = new Array();
var tcType = 0; //套餐类型
var tcName = ''; //套餐名称
var currSelectWeek = 1;
var currWeekMenu = {
    monday: [1, 2, 3],
    wednesday: [1, 2, 3, 4],
    friday: [1, 2, 3, 4, 5]
}//当前一周菜单

$(function () {


    //先加载周一的菜
    for (var i = 0; i < currWeekMenu['monday'].length; i++) {
        try {
            $('#scrollerOrder ul').append(getSingelSelectFoodTemplate(currWeekMenu['monday'][i]));

        } catch (e) {
            alert(e);
        }
    }

    currSelectItem = currWeekMenu['monday'];
    //勾选可选菜单
    showAllCurrSelectItem(currSelectItem);



    //选择菜添加入菜单
    /*
    $('.item').click(function () {

    var pid = $(this).attr('pid');
    var type = $(this).attr('ju-type');

    alert('type:' + type);

    if (type == 'selected') {
    //选中已选菜篮子里的菜，移除一个选择菜
    //alert('减掉:' + pid);
    //removeCurrSelectItem(pid, this);
    $(this).parent().remove();

    return;
    }

    if (currSelectItem.Contains(pid)) {
    removeCurrSelectItem(pid, this);
    }
    else {
    addCurrSelectItem(pid, this);
    }
    });
    */
    $('.item').live('click', function () {
        var pid = $(this).attr('pid');
        var type = $(this).attr('ju-type');
        if (type == 'selected') {
            //选中已选菜篮子里的菜，移除一个选择菜
            removeCurrSelectItem(pid);

            return;
        }

        if (currSelectItem.Contains(pid)) {
            removeCurrSelectItem(pid);
        }
        else {
            addCurrSelectItem(pid);
        }
    });

    $('.tabWeek').click(function () {
        var week = $(this).attr('ju-week');

        //移除其他已选择的样式
        $('.tabWeek').each(function () {
            $(this).attr('class', 'tabWeek tabTitle');
        });

        $(this).attr('class', 'tabWeek tabTitleSelected');
        currSelectWeek = week;
        //alert(currSelectWeek);

        //清空旧菜篮
        $('#scrollerOrder ul li').remove();

        //重新加载当前已选择的菜
        for (var i = 0; i < currWeekMenu[currSelectWeek].length; i++) {
            try {
                var pid = currWeekMenu[currSelectWeek][i];
                //添加到菜单蓝
                $('#scrollerOrder ul').append(getSingelSelectFoodTemplate(pid));



            } catch (e) {
                alert(e);
            }
        }

        currSelectItem = currWeekMenu[currSelectWeek];
        //勾选可选菜单
        showAllCurrSelectItem(currSelectItem);

        switch (currSelectWeek) {
            case 'monday':
                maxSelect = 3;
                //scrollWith = 400;
                break;
            case 'wednesday':
                maxSelect = 4;
                //scrollWith = 500;
                break;
            case 'friday':
                maxSelect = 5;
                //scrollWith = 600;
                break;
        }
        /*
        //重载下scroller -- 性能上有点小问题，暂时不重载
        var scrollWith = 600;
        
        $('#scrollerOrder').css('width', scrollWith);
        orderScroll = new IScroll('#wrapperOrder', { eventPassthrough: true, scrollX: true, scrollY: false, preventDefault: false });
        */

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


//获取当个选择菜的模板
function getSingelSelectFoodTemplate(pid) {

    var str = new StringBuilder();
    str.AppendFormat('<li><div class="item" ju-type="selected" pid="{0}"><div class="item-title">    <table width="90px">        <tr>            <td align="left">                香菇{0}            </td>            <td align="right">                <img src="/MainStyle/Res/easyui/themes/icons/edit_remove.png" />            </td>        </tr>    </table></div><div class="item-image">    <img src="/img/Gmulife/test/{0}.jpg" alt="item image" style="width: 100px; height: 60px;        margin-top: 0px;"></div></div></li>',
        pid
    );
    return str.ToString();

    /*
    <li>
    <div class="item" pid="1">
    <div class="item-title">
    <table width="90px">
    <tr>
    <td align="left">
    香菇1
    </td>
    <td align="right">
    <img src="/MainStyle/Res/easyui/themes/icons/edit_remove.png" />
    </td>
    </tr>
    </table>
    </div>
    <div class="item-image">
    <img src="/img/Gmulife/test/1.jpg" alt="item image" style="width: 100px; height: 60px;
    margin-top: 0px;"></div>
    </div>
    </li>
    */
}

//显示当前所有选择的菜单
function showAllCurrSelectItem(arrItem) {
    $('.item').each(function () {
        var pid = $(this).attr('pid');
        if (arrItem.Contains(pid)) {
            $(this).find(".select-shadow").css({ display: 'block' });
        }
        else {
            $(this).find(".select-shadow").css({ display: 'none' });
        }
    });
}

//添加选择项
function addCurrSelectItem(pid) {
    if (currSelectItem.length + 1 > maxSelect) {
        //alert("该套餐只能选择" + maxSelect + "个菜");
        //        $('#lbDlgMsg').html("该套餐只能选择" + maxSelect + "个菜");
        //        $('#dlgMsg').popup();
        //        $('#dlgMsg').popup('open');

        //        MAlert("该套餐只能选择" + maxSelect + "个菜");

        //SMAlert();

        WXDIVAlert("该套餐只能选择" + maxSelect + "个菜", "200px");

        //MSAlert("该套餐只能选择" + maxSelect + "个菜");

    }
    else {
        currSelectItem.push(pid);
        //$('div[p="' + pid + '"]')
        //$(itemObj).find(".select-shadow").css({ display: 'block' });

        //添加到菜单蓝
        $('#scrollerOrder ul').append(getSingelSelectFoodTemplate(pid));

        //勾选
        $('div[pid="' + pid + '"]').find(".select-shadow").css({ display: 'block' });
        $('#lbSelectCount').html(currSelectItem.length);
    }
}

//移除选择项
function removeCurrSelectItem(pid) {
    currSelectItem.RemoveItem(pid);

    //从菜单蓝移除
    $('div[ju-type="selected"][pid="' + pid + '"]').parent().remove();

    //取消勾选
    $('div[pid="' + pid + '"]').find(".select-shadow").css({ display: 'none' });
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
    $('#dlgMsg').popup();
    $('#dlgMsg').popup('open');
}

function MSAlert(msg, func) {
    $(document).simpledialog2({
        mode: 'button',
        themeDialog: 'a',
        animate: false,
        headerClose: false,
        buttonPrompt: msg,
        buttons: {
            '关闭': {
                click: function () {
                    if (func) { func(); }
                },
                icon: "",
                theme: "b"
            }
        }
    });
}
