var handerurl = "/handler/app/distributionhandler.ashx";
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
    return date.getFullYear() + "-" + month + "-" + currentDate;
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

