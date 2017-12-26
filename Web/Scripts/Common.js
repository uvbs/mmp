//信息提示-----------
function messager(title, msg) {
        $.messager.show({
            title: title,
            msg: msg

        });
    }
    //信息提示-----------

//替换换行符-----------
function replacebrtag(str) {
        return str.replace(/<[^><]*br[^><]*>/g, '\n');

    }
    //替换换行符-----------

//移除html标签
function RemoveHtmlTag(str) {
    //return input.replace(/<[^>]+>/g, "");

    //正则调不通，通过字符串方式移除script内容
    str = str.toLowerCase();
    EGCheckIsSelect
    while (str.indexOf('</script>') > 0) {
        str = str.substring(str.indexOf('</script>') + 9);
    }
    while (str.indexOf('</style>') > 0) {
        str = str.substring(str.indexOf('</style>') + 8);
    }

    str = str.replace(/<script.*?>.*?<\/script>/ig, '');
    str = str.replace(/<\/?[^>]*>/g, ''); //去除HTML tag
    str = str.replace(/[ | ]*\n/g, '\n'); //去除行尾空白
    str = str.replace(/\n[\s| | ]*\r/g, '\n'); //去除多余空行
    str = str.replace(/&nbsp;/ig, ''); //去掉&nbsp;
    return str;
}

//监听回车事件
//$("#toolbar").live('keyup', function(event) {
//    if (event.keyCode == 13) {
//        $("#btnSearch").click();
//    }
//});

//按百分比获取宽度
function fixWidth(percent) {
    return document.body.clientWidth * percent; //这里你可以自己做调整  
}

//easyui-grid判断是否选中了多行
function EGCheckNoSelectMultiRow(r) {
    var num = r.length;
    if (num == 0) {
        //$.messager.alert('系统提示', "请选择一条记录进行操作！", "warning");
        alert("请选择一条记录进行操作", 3);
        return false;
    }
    if (num > 1) {
        //$.messager.alert("系统提示", "您选择了多条记录，只能选择一条记录进行修改。", "warning");
        alert("您选择了多条记录，只能选择一条记录进行修改", 3);
        return false;
    }
    return true;
}

//清空指定窗体内容
function ClearWinDataByTag(tag, win) {
    var arr = tag.split('|');
    for (var i = 0; i < arr.length; i++) {
        $(win).find(arr[i]).val("");
    }
}

//清除所有输入框
function ClearAll() {

    $(":input[type!='button']").val("");

}

//easyui-grid判断是否选中行
function EGCheckIsSelect(r) {
    var num = r.length;
    if (num == 0) {
        $.messager.alert('系统提示', "请至少选择一条记录进行操作！", "warning");
        //alert("请至少选择一条记录进行操作", 3);
        return false;
    }
    return true;
}

function Alert(msg) {
    $.messager.alert('系统提示', msg);
    //alert(msg);
}

function Show(showMsg) {
    $.messager.show({
        title: '系统提示',
        msg: showMsg
    });
    //alert(showMsg);
}

function FormatterTitle(value) {
    if (value == null) {
        return "";
    }
    return "<span title=' " + value + "'>" + value + "</span>";
}

function FormatterLink(link,title) {
    if (link == null) {
        return "";
    }
    return '<a href="' + link + '" style="color: #0E74BF; text-decoration: underline;" title="' + title + '">' + title + '</a>';
}

function FormatterLinkBlank(link, title) {
    if (link == null) {
        return "";
    }
    return '<a href="' + link + '" style="color: #0E74BF; text-decoration: underline;"  target="_blank" title="' + title + '">' + title + '</a>';
}

function FormatterLinkUrl(value) {
    if (value == null) {
        return "";
    }
    return '<a href="' + value + '" style="color:blue;">' + value + '</a>';
}
function FormatterLinkUrlBlank(value) {
    if (value == null) {
        return "";
    }
    return '<a href="' + value + '" style="color:blue;" target="_blank">' + value + '</a>';
}
function FormatterImage50(value) {
    if (value == '' || value == null)
        return "";
    var str = new StringBuilder();
    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="50" width="50" />', value);
    return str.ToString();        
}

function FormatterImage60_90(value) {
    if (value == '' || value == null)
        return "";
    var str = new StringBuilder();
    str.AppendFormat('<img alt="" class="imgAlign" src="{0}" title="缩略图" height="90" width="60" />', value);
    return str.ToString();
}

function padLeft(str, min) {
    if (str >= min)
        return str;
    else
        return "0" + str;
}

function FormatDate(value) {

    if (value == null || value == "") {
        return "";
    }
    var date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
    var month = padLeft(date.getMonth() + 1, 10);
    var currentDate = padLeft(date.getDate(), 10);
    var hour = padLeft(date.getHours(), 10);
    var minute = padLeft(date.getMinutes(), 10);
    var second = padLeft(date.getSeconds(), 10);
    return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + minute + ":" + second;
}

//验证长时间
function CheckDateTime(str) {
    var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
    var r = str.match(reg);
    if (r == null) return false;
    var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7]);
}

Date.prototype.format = function(format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1,
                RegExp.$1.length == 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}

//随机数
function GetRandomNum(Min, Max) {
    var Range = Max - Min;
    var Rand = Math.random();
    return (Min + Math.round(Rand * Range));
}

//多选框分组赋值
function SetCheckGroupVal(groupName, values, attr) {
        var value = values.split(',');
        for (var i = 0; i < value.length; i++) {
            $('input[name="' + groupName + '"]').each(function() {
                if (value[i] == $(this).attr(attr)) {
                    this.checked = true;
                }
            });
        }
    }
//多选框分组取值
function GetCheckGroupVal(groupName, attr) {
    var values = [];
    $('input[name="' + groupName + '"]:checked').each(function() {
        var id = $(this).attr(attr);
        values.push(id);
    });
    if (values.length > 0)
        return values.join(',');
    else
        return '';
}

//对象转到rel
function GotoRel(object) {

        window.location = "" + $(object).attr("rel") + "";

    }
//获取Get参数
function GetParm(parm) {
        //获取当前URL
        var local_url = window.location.href;

        //获取要取得的get参数位置
        var get = local_url.indexOf(parm + "=");
        if (get == -1) {
            return "";
        }
        //截取字符串
        var get_par = local_url.slice(parm.length + get + 1);
        //判断截取后的字符串是否还有其他get参数
        var nextPar = get_par.indexOf("&");
        if (nextPar != -1) {
            get_par = get_par.slice(0, nextPar);
        }
        return get_par;
    }
    //获取参数

function DelUrlParam(url, name) {
    var str = "";
    if (url.indexOf('?') != -1) {
        str = url.substr(url.indexOf('?') + 1);
    }
    else {
        return url;
    }
    var arr = "";
    var returnurl = "";
    var setparam = "";
    if (str.indexOf('&') != -1) {
        arr = str.split('&');
        for (i in arr) {
            if (isNaN(i)) {
                continue;
            }
            if (arr[i].split('=')[0] != name) {
                returnurl = returnurl + arr[i].split('=')[0] + "=" + arr[i].split('=')[1] + "&";
            }
        }
        return url.substr(0, url.indexOf('?')) + "?" + returnurl.substr(0, returnurl.length - 1);
    }
    else {
        arr = str.split('=');
        if (arr[0] == name) {
            return url.substr(0, url.indexOf('?'));
        }
        else {
            return url;
        }
    }
}

function IsURL(str_url) {
    //var strRegex = "^((https|http|ftp|rtsp|mms)?://)"
    //+ "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //ftp的user@  
    //      + "(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP形式的URL- 199.194.52.184  
    //      + "|" // 允许IP和DOMAIN（域名） 
    //      + "([0-9a-z_!~*'()-]+\.)*" // 域名- www.  
    //      + "([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // 二级域名  
    //      + "[a-z]{2,6})" // first level domain- .com or .museum  
    //      + "(:[0-9]{1,4})?" // 端口- :80  
    //      + "((/?)|" // a slash isn't required if there is no file name  
    //      + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";

    var strRegex = "[a-zA-z]+://[^\s]*";//"[a-zA-z]+:\/\/[^\s]";

    var re = new RegExp(strRegex);
    //re.test() 
    if (re.test(str_url)) {
        return (true);
    } else {
        return (false);
    }
}

//数组元素移位排序
function ArrayItemMoveIndex(oldIndex, newIndex, arr) {
    var tmp = arr[newIndex];
    arr[newIndex] = arr[oldIndex];
    arr[oldIndex] = tmp;
    return arr;
}

/*
 *　方法:Array.Contains(e)
 *　功能:程序是否存在某元素.
 *　参数:元素.
 *　返回:true/false.
 */
Array.prototype.Contains = function (e) {
    var i = 0;
    for (i = 0; i < this.length && this[i] != e; i++);
    return !(i == this.length);
}

// 查找某个元素所在的键值
Array.prototype.IndexOf = function(val) {
    for (var i = 0, l = this.length; i < l; i++) {
        if (this[i] == val) return i;
    }
    return null;
}

/*
 *　方法:Array.RemoveIndexOf(dx)
 *　功能:删除数组元素.
 *　参数:dx删除元素的下标.
 *　返回:在原数组上修改数组.
 */
Array.prototype.RemoveIndexOf = function (dx) {
    if (isNaN(dx) || dx > this.length) {
        return false;
    }
    this.splice(dx, 1);
}

Array.prototype.RemoveItem = function (it) {
    this.RemoveIndexOf(this.IndexOf(it));
}

Array.prototype.ToStr = function (sp) {
    var str = '';

    for (var i = 0; i < this.length; i++) {
        str += this[i] + sp;
    }

    return str;
}
/*
在数组指定位置插入元素
*/
Array.prototype.insert = function (index, item) {
    this.splice(index, 0, item);
};

String.prototype.endWith = function(s) {
    if (s == null || s == "" || this.length == 0 || s.length > this.length)
        return false;
    if (this.substring(this.length - s.length) == s)
        return true;
    else
        return false;
    return true;
}
String.prototype.startWith = function(s) {
    if (s == null || s == "" || this.length == 0 || s.length > this.length)
        return false;
    if (this.substr(0, s.length) == s)
        return true;
    else
        return false;
    return true;
}

function CreateGUID() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}

function Clear(str) {
    var idarry = str.split('|');
    for (var i = 0; i < idarry.length; i++) {
        $("#" + idarry[i]).val("");
    }
}

if (typeof jQuery != 'undefined') {

    $(window).resize(function () {
        var width = document.body.clientWidth - 17;
        var height = document.documentElement.clientHeight - 112;

        var doms = ['#grvData', '#list_data', '#dataGrid'];
        setTimeout(function () {
            for (var i = 0; i < doms.length; i++) {
                if ($(doms[i]).length > 0) {
                    $(doms[i]).datagrid('resize', {
                        width: width,
                        height: height
                    });
                }
            }
        }, 1000);

    });
}


function layermsg(msg) {
    layer.open({
        content: msg,
        btn: ['OK']
    });
}
//分享完成加积分
function shareComeplete(id) {
    //分享到朋友圈
    $.ajax({
        type: 'post',
        url: '/Serv/API/Score/Add.ashx',
        data: { type: "ShareArticle",id:id },
        dataType: 'jsonp',
        success: function (data) {
            if (data.isSuccess) {
               
            }

        }
    });
}

function checkNum(obj) {
    //检查是否是非数字值  
    if (isNaN(obj.value)) {
        obj.value = "";
    }
    if (obj != null) {
        //检查小数点后是否对于两位
        if (obj.value.toString().split(".").length > 1 && obj.value.toString().split(".")[1].length > 2) {
            alert("小数点后多于两位！");
            obj.value = "";
        }
    }
}

function pagerLocalFilter(result) {
    var data = result.result;
    if (data == null) {
        return {
            total: 0,
            rows: []
        };
    }
    data = {
        total: data.totalcount,
        rows: data.list
    };
    var dg = $(this);
    var opts = dg.datagrid('options');
    var pager = dg.datagrid('getPager');
    pager.pagination({
        onSelectPage: function (pageNum, pageSize) {
            opts.pageNumber = pageNum;
            opts.pageSize = pageSize;
            pager.pagination('refresh', {
                pageNumber: pageNum,
                pageSize: pageSize
            });
            dg.datagrid('loadData', result);
        }
    });
    if (!data.originalRows) {
        data.originalRows = (data.rows);
    }
    var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
    var end = start + parseInt(opts.pageSize);
    data.originalRows.sort(function (r1, r2) {
        var r = 0;
        var sn = opts.sortName;
        if (sn) {
            var so = opts.sortOrder;

            var col = dg.datagrid('getColumnOption', sn);
            var _f6 = col.sorter || function (a, b) {
                return a == b ? 0 : (a > b ? 1 : -1);
            };
            r = _f6(r1[sn], r2[sn]) * (so == "asc" ? 1 : -1);
            if (r != 0) {
                return r;
            }
        }
        return r;
    });
    data.rows = (data.originalRows.slice(start, end));
    return data;
}

function pagerFilter(result) {
    var data = result.result;
    if (data == null) {
        return {
            total: 0,
            rows: []
        };
    }
    return {
        total: data.totalcount,
        rows: data.list
    };
}

function layerAlert(msg) {
    layer.msg(msg);
}