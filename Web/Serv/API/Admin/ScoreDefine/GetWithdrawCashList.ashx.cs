using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.ScoreDefine
{
    /// <summary>
    /// GetwithdrawCashList 的摘要说明
    /// </summary>
    public class GetWithdrawCashList : BaseHandlerNeedLoginAdminNoAction
    {
        public void ProcessRequest(HttpContext context)
        {
            BLLJIMP.BLLDistributionOffLine bll = new BLLJIMP.BLLDistributionOffLine();
            BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

            int page = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;//页码
            int rows = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 10;//页数

            string userId = "";
            string keyword = context.Request["keyword"];
            string websiteowner = bllUser.WebsiteOwner;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                BLLJIMP.Model.UserInfo u = bllUser.GetUserInfo(keyword, websiteowner);
                if (u != null) {
                    userId = u.UserID;
                }
                if(string.IsNullOrWhiteSpace(userId)){
                    u = bllUser.GetUserInfoByPhone(keyword, websiteowner);
                    if (u != null)
                    {
                        userId = u.UserID;
                    }
                }
                if (string.IsNullOrWhiteSpace(userId))
                {
                    List<BLLJIMP.Model.UserInfo> ul = bllUser.GetUsersByLikeName(keyword, websiteowner);
                    if (ul.Count>1)
                    {
                        userId = ZentCloud.Common.MyStringHelper.ListToStr(ul.Select(p=>p.UserID).ToList(),"'",",");
                    }
                    else if(ul.Count==1)
                    {
                        userId = ul[0].UserID;
                    }
                }

                if (string.IsNullOrWhiteSpace(userId))
                {
                    userId = "-999";
                }
            }
            int total = 0;
            var sourceList = bll.QueryWithdrawCashList(page, rows, userId, out total, context.Request["status"], context.Request["type"]);
            List<dynamic> list = new List<dynamic>();

            if (sourceList.Count > 0)
            {
                foreach (var p in sourceList)
                {
                    BLLJIMP.Model.UserInfo u = bllUser.GetUserInfo(p.UserId, websiteowner);
                    list.Add(new
                    {
                        id = p.AutoID,
                        amount = p.Amount,//金额
                        status = ConvertStatus(p.Status),//状态
                        time = bll.GetTimeStamp(p.InsertDate),//时间
                        score = p.Score,
                        username = u == null ? p.UserId : bllUser.GetUserDispalyName(u)
                    });
                }
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "ok";
            apiResp.result = new{
                totalcount = total,
                list = list
            };

            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(apiResp));

        }

        /// <summary>
        /// 转换状态
        /// </summary>
        /// <param name="statusInt"></param>
        /// <returns></returns>
        public string ConvertStatus(int statusInt)
        {
            string status = "";
            switch (statusInt)
            {
                case 0:
                    status = "待审核";
                    break;
                case 1:
                    status = "已受理";
                    break;
                case 2:
                    status = "审核通过";
                    break;
                case 3:
                    status = "审核不通过";
                    break;
                default:
                    break;
            }
            return status;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}