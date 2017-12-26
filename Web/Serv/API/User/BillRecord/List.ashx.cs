using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;

namespace ZentCloud.JubitIMP.Web.Serv.API.User.BillRecord
{
    /// <summary>
    /// List 的摘要说明  账单记录
    /// </summary>
    public class List : BaseHandlerNeedLoginNoAction
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 5;
            string date = context.Request["time"];
            if (string.IsNullOrEmpty(date))
            {
                apiResp.msg = "time 为必填项,请检查";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bllUser.ContextResponse(context, apiResp);
                return;
            }
            System.DateTime myTime = DateTimeHelper.UnixTimestampToDateTime(long.Parse(date));
            int totalCount=0;
            List<UserCreditAcountDetails> list = bllUser.GetUserCreditAcountDetailsByMonth(pageIndex, pageSize,bllUser.GetCurrUserID(),myTime,out totalCount);
            apiResp.status = true;
            List<dynamic> returnList = new List<dynamic>();
            foreach (UserCreditAcountDetails item in list)
            {
                switch (item.Type)
                {
                    case "ApplyCost"://报名消耗信用
                        returnList.Add(new
                        {
                            title = "你报名了一个约会",
                            credit_acount = item.CreditAcount,
                            create_time = DateTimeHelper.DateTimeToUnixTimestamp(item.AddTime)
                        });
                        break;
                    case "ApplyReturn"://报名未通过解冻（返还）信用
                        returnList.Add(new
                        {
                            title = "你报名的约会未通过",
                            credit_acount = item.CreditAcount,
                            create_time = DateTimeHelper.DateTimeToUnixTimestamp(item.AddTime)
                        });
                        break;
                    case "PublishCost"://发布消耗信用
                        returnList.Add(new 
                        {
                            title="你发布了一个约会",
                            credit_acount=item.CreditAcount,
                            create_time = DateTimeHelper.DateTimeToUnixTimestamp(item.AddTime)
                        });
                        break;
                    case "Recharge"://充值获得信用
                        returnList.Add(new
                        {
                            title = "你的银行卡"+item.CreditAcount+"充值成功",
                            credit_acount = item.CreditAcount,
                            create_time = DateTimeHelper.DateTimeToUnixTimestamp(item.AddTime)
                        });
                        break;
                    case "ReturnPublisher"://无人报名返还信用金
                        returnList.Add(new
                        {
                            title = "你发起的约会无人报名",
                            credit_acount = item.CreditAcount,
                            create_time = DateTimeHelper.DateTimeToUnixTimestamp(item.AddTime)
                        });
                        break;
                    case "SignReturn"://签到返还信用
                        returnList.Add(new
                        {
                            title = "签到返还",
                            credit_acount = item.CreditAcount,
                            create_time = DateTimeHelper.DateTimeToUnixTimestamp(item.AddTime)
                        });
                        break;
                    default:
                        break;
                }
            }
            apiResp.result = new 
            {
                totalcount=totalCount,
                list=returnList
            };
            bllUser.ContextResponse(context, apiResp);
        }


    }
}