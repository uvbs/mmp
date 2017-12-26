using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.BLLJIMP
{
    public enum ReturnCode
    {
        SUCCEED = 0,    //返回成功
        //1-999 为系统保留返回码

        //短信业务错误码 1001-1999

        /// <summary>
        /// 用户鉴权失败
        /// </summary>
        SMS_LoginError = 1001,
        /// <summary>
        /// 用户余额不足
        /// </summary>
        SMS_PointNotEnough = 1002,
        /// <summary>
        /// 手机号码为空
        /// </summary>
        SMS_MobileEmpty = 1003,
        /// <summary>
        /// 手机号码格式无效
        /// </summary>
        SMS_MobileInvalid = 1004,
        /// <summary>
        /// 手机号码过多
        /// </summary>
        SMS_MobileToMuch = 1005,
        /// <summary>
        /// 短信内容为空
        /// </summary>
        SMS_ContentEmpty = 1006,
        /// <summary>
        /// 短信内容太长
        /// </summary>
        SMS_ContentToMuch = 1007,
        /// <summary>
        /// 通道错误
        /// </summary>
        SMS_PipeError = 1008,
        /// <summary>
        /// 通道不存在
        /// </summary>
        SMS_PipeNotExist = 1009,
        /// <summary>
        /// 用户没有购买这个通道
        /// </summary>
        SMS_PipeNotForUser = 1010,
        /// <summary>
        /// 添加trigger短信失败
        /// </summary>
        SMS_AddTriggerError = 1011,
        /// <summary>
        /// 添加Mission短信失败
        /// </summary>
        SMS_AddMissionError = 1012,
        /// <summary>
        /// 去重去无效后无可用号码
        /// </summary>
        SMS_NoUseMobile = 10013,
        /// <summary>
        /// 添加接收号码失败
        /// </summary>
        SMS_AddSMSDetailError = 10014,
        /// <summary>
        /// 添加号码到发送队列失败
        /// </summary>
        SMS_AddSMSSendQueueError = 10015,
        /// <summary>
        /// 定时时间格式失败
        /// </summary>
        SMS_AddSMSPlanTimeError = 10016,

        /// <summary>
        /// 未设置发送通道
        /// </summary>
        SMS_NotSetSendPipe=10017,
        /// <summary>
        /// 未知错误
        /// </summary>
        SMS_Exception = 10050,



        /// <summary>
        /// 会议业务错误码
        /// </summary>
        MEETING_AddFailed = 2001,  //会议业务错误码 2000-2999
        /// <summary>
        /// 用户不存在
        /// </summary>
        USER_NotExist = 3001,


        SYSTEM_RESERVED = 999999
    }
}