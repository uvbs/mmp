/**
 * author:   谭耀武
 * email:    tywo45@163.com
 */
var tt = {};
tt.Util = {};
tt.f={};

tt.i18Req = "请填写{0}!";
tt.i18Req1 = "请选择{0}!";
tt.i18Email = "{0}看起来不像个邮箱!";
tt.i18Int = "请输入整数!";
tt.i18Datetime = "正确的格式为{1}，譬如：{2}";

//  num range start
tt.i18NumRange = "{0}必须在{1}和{2}之间!";
tt.i18NumRangeMin = "{0}必须小于等于{1}!";
tt.i18NumRangeMax = "{0}必须大于等于{1}!";
tt.i18NumRangeExp = "{0}合法范围：{1}!";
//  num range end

tt.i18LenMin = "{0}最多只能输入{1}个字符，您已超出{2}个字符(一个汉字占两个字符)!";
tt.i18Len = "{0}最少要输入{1}个字符，你还需要输入{2}个字符(一个汉字占两个字符)!";
tt.i18LenExp = "{0}长度合法范围：{1}(一个汉字占两个字符)!";
tt.i18LenTip = "您已输入{0}个字符，还可以输入{1}个字符(一个汉字占两个字符)!";

tt.i18Num = "请输入数字!";
tt.i18Regex = "{0}{1}";
tt.i18Ip = "{0}看起来不像个IP地址!";
tt.i18Postcode = "{0}看起来不像个邮政编码!";
tt.i18Tel = "{0}看起来不像个电话号码!";
tt.i18Idcard = "{0}看起来不像身份证号码!";

tt.i18SelectCountMin = "{0}最多只能选中{1}项，您已多选{2}项!";
tt.i18SelectCount = "{0}最少要选中{1}项，你还需要选择{2}项!";
tt.i18SelectCount_1 = "{0}最少要选中{1}项!";
tt.i18SelectCountExp = "{0}选中个数的合法范围：{1}!";

tt.ajaxError1="从服务器端获取数据失败!";
tt.ajaxError2="服务器异常!";

tt.i18DftOk = null;//'OK!';

//-----  compare validator
tt.i18Compare = "{0}不正确!";
tt.i18StrCompare = "{0}应该{1}{2}{3}!";
tt.i18NumCompare = "{0}应该{1}{2}{3}!";
tt.i18StrValueCompare = "{0}必须{1}{2}!";
tt.i18NumValueCompare = "{0}必须{1}{2}!";
tt.operMap = [];
tt.operMap["<"] = "小于";
tt.operMap["<="] = "小于或等于";
tt.operMap["=="] = "等于";
tt.operMap["!="] = "不等于";
tt.operMap[">"] = "大于";
tt.operMap[">="] = "大于或等于";

tt.close = "关闭";