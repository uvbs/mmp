﻿
/**
 * @homepage https://github.com/LACERBA/LADatePicker.Core
 * (c) 2014, Charles Huang. 
 *
 * 调用方法：
 * var arr = LADatePicker.createDayArr(2014,12);
 * 返回基于二维数组(7列6行)的日历数据
 *
 */

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

var LADatePicker = {};

LADatePicker.config = {
    week:['日','一','二','三','四','五','六']
};

/**
 * [DayModel 日期对象]
 * @param {[type]}  year         [description]
 * @param {[type]}  month        [description]
 * @param {[type]}  week         [description]
 * @param {[type]}  day          [description]
 * @param {Boolean} isTody       [description]
 * @param {Boolean} isCheck      [description]
 * @param {Boolean} isCurrSelect [description]
 */
var DayModel = function (year, month, week, day, isTody, isCheck, isCurrSelect) {
    this.year = year;
    this.week = week;
    this.day = day;
    this.month = month;
    this.isTody = isTody;
    this.isCheck = isCheck;
    this.isCurrSelect = isCurrSelect;
};

/**
 * [getDate 获取时间对象]
 * @return {[type]} [description]
 */
DayModel.prototype.getDate = function () {
    return new Date(this.year + '-' + this.month + '-' + this.day + ' 00:00:00');
};

/**
 * [isBeforeToday 是否是今天之前的日期]
 * @return {Boolean} [description]
 */
DayModel.prototype.isBeforeToday = function () {
    var toDay = new Date();
    var a = this.getDate();
    var b =  new Date(toDay.format('yyyy-MM-dd') + ' 00:00:00');
    return a < b;
};

/**
 * [hasBeforeToday 是否包含今天之前的数据]
 * @param  {[type]}  dayList [description]
 * @return {Boolean}         [description]
 */
LADatePicker.hasBeforeToday = function(dayList){
    var result = false;
    for (var i = 0; i < dayList.length; i++) {
        for (var j = 0; j < dayList[i].length; j++) {
            if(dayList[i][j] && dayList[i][j].isBeforeToday()){
                return true;
            }
        };
    };
    return result;
};

/**
 * [getLastDay 获取最后一天]
 * @param  {[type]} year  [description]
 * @param  {[type]} month [description]
 * @return {[type]}       [description]
 */
LADatePicker.getLastDay = function (year, month) {
    var new_year = year; //取当前的年份
    var new_month = month++; //取下一个月的第一天，方便计算（最后一天不固定）
    if (month > 12) //如果当前大于12月，则年份转到下一年
    {
        new_month -= 12; //月份减
        new_year++; //年份增
    }
    var new_date = new Date(new_year, new_month, 1); //取当年当月中的第一天
    return (new Date(new_date.getTime() - 1000 * 60 * 60 * 24)).getDate(); //获取当月最后一天日期
};

/* 
  *   功能:DateAdd 
  *   参数:interval,字符串表达式，表示要添加的时间间隔. 
  *   参数:number,数值表达式，表示要添加的时间间隔的个数. 
  *   参数:date,时间对象. 
  *   返回:新的时间对象. 
  *   var   now   =   new   Date(); 
  *   var   newDate   =   DateAdd( "d ",5,now); 
  *---------------   DateAdd(interval,number,date)   ----------------- 
  */  
LADatePicker.dateAdd = function  (interval,number,date)  
{  

// var   now   =   new   Date();  
// //加五天.  
// var   newDate   =   DateAdd( "d ",5,now);  
// alert(newDate.toLocaleDateString())  
// //加两个月.  
// newDate   =   DateAdd( "m ",2,now);  
// alert(newDate.toLocaleDateString())  
// //加一年  
// newDate   =   DateAdd( "y ",1,now);  
// alert(newDate.toLocaleDateString())   

    switch(interval)  
    {  
        case   "y"   :   {  
                date.setFullYear(date.getFullYear()+number);  
                return   date;  
                break;  
        }  
        case   "q"   :   {  
                date.setMonth(date.getMonth()+number*3);  
                return   date;  
                break;  
        }  
        case   "m"   :   {  
                date.setMonth(date.getMonth()+number);  
                return   date;  
                break;  
        }  
        case   "w"   :   {  
                date.setDate(date.getDate()+number*7);  
                return   date;  
                break;  
        }  
        case   "d"   :   {  
                date.setDate(date.getDate()+number);  
                return   date;  
                break;  
        }  
        case   "h"   :   {  
                date.setHours(date.getHours()+number);  
                return   date;  
                break;  
        }  
        case   "m"   :   {  
                date.setMinutes(date.getMinutes()+number);  
                return   date;  
                break;  
        }  
        case   "s"   :   {  
                date.setSeconds(date.getSeconds()+number);  
                return   date;  
                break;  
        }  
        default   :   {  
                date.setDate(date.getDate()+number);  
                return   date;  
                break;  
        }  
    }  
};  

/**
 * [GetDayDiff 获取天数时间差，比如剩余天数]
 * @param {[type]} start [description]
 * @param {[type]} end   [description]
 */
LADatePicker.GetDayDiff = function(start,end){
    start = new Date(start);  //开始时间  
    end = new Date(end);    //结束时间  
    var diff = start.getTime() - end.getTime();  //时间差的毫秒数  
      
    //计算出相差天数  
    var days=Math.floor(diff/(24*3600*1000));
    return days;
};

/**
 * [clearSelectDate 清除选中d时间状态]
 * @param  {[type]} item [description]
 * @return {[type]}      [description]
 */
LADatePicker.clearCheck = function(dayList){
    for (var i = 0; i < dayList.length; i++) {
        for (var j = 0; j < dayList[i].length; j++) {
            dayList[i][j].isCheck = false;
        };
    };
};

/**
 * [init 初始化入口]
 * @return {[type]} [description]
 */
LADatePicker.init = function () {

};

/**
 * [createDayArr 创建月份时间数组]
 * @param  {[type]} year  [description]
 * @param  {[type]} month [description]
 * @return {[type]}       [description]
 */
LADatePicker.createDayArr = function (year, month) {
    var resultDayArr = [];
    //7*6时间组合
    var day = 1, //时间增量
        startWeek = 0, //0（周日） 到 6（周六）
        lastDay = LADatePicker.getLastDay(year, month);

    //计算第一天开始位置
    startWeek = new Date(year + '-' + month + '-' + 1).getDay();

    for (var i = 0; i < 6; i++) {
        var tmpDays = []; //每行数据
        for (var j = 0; j < 7; j++) {

            if (day > lastDay) {
                tmpDays.push(0);
                continue;
            } //时间增加超过最后一天，跳出循环

            if (i == 0) { //如果是第一行，必须等开始位置才进行时间构造
                if (j < startWeek) {
                    tmpDays.push(0);
                    continue;
                } //还未到达
            }

            var
                addDay = day++, //添加时间
                toDay = new Date();

            //构造单个日期对象
            var obj = new DayModel(
                year,
                month,
                j,
                addDay,
                (toDay.getDate() == addDay && (toDay.getMonth() + 1) == month && toDay.getFullYear() == year) ? true : false,
                0,
                0
            );

            tmpDays.push(obj);

        };
        resultDayArr.push(tmpDays.slice(0));
    };

    return resultDayArr;
};
