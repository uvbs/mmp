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
//��ȡGet����
function GetParm(parm) {
    //��ȡ��ǰURL
    var local_url = window.location.href;

    //��ȡҪȡ�õ�get����λ��
    var get = local_url.indexOf(parm + "=");
    if (get == -1) {
        return "";
    }
    //��ȡ�ַ���
    var get_par = local_url.slice(parm.length + get + 1);
    //�жϽ�ȡ����ַ����Ƿ�������get����
    var nextPar = get_par.indexOf("&");
    if (nextPar != -1) {
        get_par = get_par.slice(0, nextPar);
    }
    return get_par;
}
//��ȡ����

