using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Performance
{
    /// <summary>
    /// Set 的摘要说明
    /// </summary>
    public class Set : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            string user_auto_id = context.Request["user_auto_id"];
            string keys = context.Request["keys"];
            string keynames = context.Request["keynames"];
            string websiteOwner = bll.WebsiteOwner;
            UserInfo user = bll.GetColByKey<UserInfo>("AutoID", user_auto_id, "AutoID,UserID", websiteOwner: websiteOwner);
            if (user == null)
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.status = false;
                apiResp.msg = "会员未找到";
                bll.ContextResponse(context, apiResp);
                return;
            }
            List<string> keyList = keys.Split(',').ToList();
            List<string> keyNameList = keynames.Split(',').ToList();
            Dictionary<decimal, decimal> dicPerformance = new Dictionary<decimal, decimal>();
            Dictionary<decimal, string> dicName = new Dictionary<decimal, string>();
            for (int i = 0; i < keyList.Count; i++)
			{
                string keyString = keyList[i];
                string keyName = keyNameList[i];
                string valueString = context.Request[keyString];
                decimal key = Convert.ToDecimal(keyString.Substring(1));
                decimal value = Convert.ToDecimal(valueString);
                dicPerformance.Add(key, value);
                dicName.Add(key, keyName);
			}
            List<TeamPerformanceSet> userSetList = bll.GetSetList(int.MaxValue, 1, websiteOwner, user.UserID);
            BLLTransaction tran = new BLLTransaction();
            foreach (TeamPerformanceSet item in userSetList.Where(p=> dicPerformance.ContainsKey( p.Performance)))
            {
                item.RewardRate = dicPerformance[item.Performance];
                if (!bll.Update(item, tran))
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.status = false;
                    apiResp.msg = item.Name + "更新错误";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            foreach (var item in dicPerformance.Where(p=> !userSetList.Exists(pi=>pi.Performance == p.Key)))
            {
                TeamPerformanceSet set = new TeamPerformanceSet();
                set.UserId = user.UserID;
                set.WebsiteOwner = websiteOwner;
                set.Name = dicName[item.Key];
                set.Performance = item.Key;
                set.RewardRate = item.Value;
                if (!bll.Add(set, tran))
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.status = false;
                    apiResp.msg = set.Name + "新增错误";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            tran.Commit();
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "设置成功";
            bll.ContextResponse(context, apiResp);
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