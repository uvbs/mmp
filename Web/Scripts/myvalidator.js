﻿
/*-----------------验证方法--------------*/


//"^//d+$"　　//非负整数（正整数 + 0） 
//"^[0-9]*[1-9][0-9]*$"　　//正整数 
//"^((-//d+)|(0+))$"　　//非正整数（负整数 + 0） 
//"^-[0-9]*[1-9][0-9]*$"　　//负整数 
//"^-?//d+$"　　　　//整数 
//"^//d+(//.//d+)?$"　　//非负浮点数（正浮点数 + 0） 
//"^(([0-9]+//.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*//.[0-9]+)|([0-9]*[1-9][0-9]*))$"　　//正浮点数 
//"^((-//d+(//.//d+)?)|(0+(//.0+)?))$"　　//非正浮点数（负浮点数 + 0） 
//"^(-(([0-9]+//.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*//.[0-9]+)|([0-9]*[1-9][0-9]*)))$"　　//负浮点数 
//"^(-?//d+)(//.//d+)?$"　　//浮点数


/*
　 *　方法:isInt(str)
　 *　功能:验证输入字符串是否是int数字.
　 *　参数:str输入字符串.
　 *　返回:bool.
　 */
function isInt(value) {
    var reg = new RegExp("^[0-9]*$");
    if (!reg.test(value)) {
        return false;
    }
    return true;
}

function isFloat(value) {
    return /^(-?\d+)(\.\d+)?$/.test(value);
}

/*
　 *　方法:isEmail(str)
　 *　功能:验证输入字符串是否是Email.
　 *　参数:str输入字符串.
　 *　返回:bool.
　 */
function isEmail(value) {
    //return /^[\w\-\.]+@[\w\-\.]+(\.\w+)+$/.test(str);
    return /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/.test(value); //对应myspider里面的验证表达式
}

function isTel(value) {
    return /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/i.test(value);
}

function isPhone(value) {
    return /^(13|15|18)\d{9}$/i.test(value);
}

/*
　 *　方法:regExpValidate(v, e)
　 *　功能:正则表达式验证.
　 *　参数:v要验证的输入字符串，e验证表达式
　 *　返回:bool.
　 */
function regExpValidate(v, e) {
    var reg = new RegExp(e);
    if (!reg.test(v)) {
        return false;
    }
    return true;
}

