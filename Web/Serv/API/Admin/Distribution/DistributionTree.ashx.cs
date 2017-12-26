using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Distribution
{
    /// <summary>
    /// DistributionTree 的摘要说明
    /// </summary>
    public class DistributionTree : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            string member = context.Request["member"];
            int maxLevel = 3;
            string max_level = context.Request["max_level"];
            if (!string.IsNullOrWhiteSpace(max_level)) maxLevel = Convert.ToInt32(max_level);
            string websiteOwner = bllUser.WebsiteOwner;
            string userId = "";
            UserInfo baseUser  = new UserInfo();
            if(string.IsNullOrWhiteSpace(member)){
                userId = websiteOwner;
                baseUser = bllUser.GetSpreadUser(userId, websiteOwner);
            }else{
                baseUser = bllUser.GetSpreadUser(member, websiteOwner,10,true);
                if (baseUser.UserID != websiteOwner && baseUser.MemberLevel < 10) {
                    baseUser = null;
                    userId = "-1";
                }
                else
                {
                    userId = baseUser.UserID;
                }
            }
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "获取团队";
            if (baseUser == null)
            {
                apiResp.result = new
                {
                    base_user = false,
                    list = new List<int>()
                };
            }
            else
            {
                List<UserInfo> childs = bll.GetTeamList(userId, websiteOwner, true, maxLevel, 
                    "AutoID,UserID,TrueName,WXNickname,Regtime,Phone,DistributionOwner,MemberLevel,EmptyBill,MemberApplyStatus");
                string bpPhone = "";
                if (!string.IsNullOrWhiteSpace(baseUser.DistributionOwner)){
                    UserInfo bpUser = bllUser.GetUserInfo(baseUser.DistributionOwner, websiteOwner);
                    if (bpUser != null) bpPhone = bpUser.Phone;
                }

                List<UserLevelConfig> lvlist = bll.QueryUserLevelList(websiteOwner, "DistributionOnLine", colName: "AutoId,LevelNumber,LevelString");
                Dictionary<int, string> dicLevel = new Dictionary<int, string>();
                foreach (UserLevelConfig item in lvlist)
                {
                    if (!dicLevel.ContainsKey(item.LevelNumber))
                    {
                        dicLevel.Add(item.LevelNumber, item.LevelString);
                    }
                }

                apiResp.result = new
                {
                    base_user = new
                    {
                        id = baseUser.UserID,
                        aid = baseUser.AutoID,
                        pid = baseUser.DistributionOwner,
                        pphone = bpPhone,
                        issys = baseUser.UserID == websiteOwner ? true : false,
                        status = (baseUser.EmptyBill==1?"空单":"实单") +(baseUser.MemberApplyStatus==9?"已审":"未审"),
                        name = baseUser.TrueName,
                        phone = baseUser.Phone,
                        regtime = !baseUser.Regtime.HasValue ? "" : baseUser.Regtime.Value.ToString("yyyy/MM/dd HH:mm"),
                        member_lv = baseUser.MemberLevel,
                        member_lvname = dicLevel.ContainsKey(baseUser.MemberLevel) ? dicLevel[baseUser.MemberLevel] : ""
                    },
                    list = from p in childs
                           select new
                           {
                               id = p.UserID,
                               aid = p.AutoID,
                               pid = p.DistributionOwner,
                               name = p.TrueName,
                               phone = p.Phone,
                               status = (p.EmptyBill == 1 ? "空单" : "实单") + (p.MemberApplyStatus == 9 ? "已审" : "未审"),
                               regtime = !p.Regtime.HasValue ? "" : p.Regtime.Value.ToString("yyyy/MM/dd HH:mm"),
                               empty_bill = p.EmptyBill,
                               apply_status = p.MemberApplyStatus,
                               member_lv = p.MemberLevel,
                               member_lvname = dicLevel.ContainsKey(p.MemberLevel) ? dicLevel[p.MemberLevel] : ""
                           }
                };
            }
            apiResp.status = true;
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