using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Model.API.File;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Distribution
{
    /// <summary>
    /// DistributionTreeExport 的摘要说明
    /// </summary>
    public class DistributionTreeExport : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            string member = context.Request["member"];
            int maxLevel = int.MaxValue;
            string max_level = context.Request["max_level"];
            if (!string.IsNullOrWhiteSpace(max_level)) maxLevel = Convert.ToInt32(max_level);
            string websiteOwner = bllUser.WebsiteOwner;
            string userId = "";
            UserInfo baseUser = new UserInfo();
            if (string.IsNullOrWhiteSpace(member))
            {
                userId = websiteOwner;
                baseUser = bllUser.GetSpreadUser(userId, websiteOwner);
            }
            else
            {
                baseUser = bllUser.GetSpreadUser(member, websiteOwner, 10, true);
                if (baseUser.UserID != websiteOwner && baseUser.MemberLevel < 10)
                {
                    baseUser = null;
                    userId = "-1";
                }
                else
                {
                    userId = baseUser.UserID;
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("角色", typeof(string));
            dt.Columns.Add("会员手机", typeof(string));
            dt.Columns.Add("会员姓名", typeof(string));
            dt.Columns.Add("会员身份证", typeof(string));
            dt.Columns.Add("推荐人手机", typeof(string));
            dt.Columns.Add("推荐人姓名", typeof(string));
            dt.Columns.Add("会员级别", typeof(string));
            dt.Columns.Add("状态", typeof(string));
            dt.Columns.Add("注册时间", typeof(string));
            if (baseUser != null)
            {
                List<UserLevelConfig> lvlist = bll.QueryUserLevelList(websiteOwner, "DistributionOnLine",showAll:true, colName: "AutoId,LevelNumber,LevelString");
                Dictionary<int, string> dicLevel = new Dictionary<int, string>();
                foreach (UserLevelConfig item in lvlist)
                {
                    if (!dicLevel.ContainsKey(item.LevelNumber))
                    {
                        dicLevel.Add(item.LevelNumber, item.LevelString);
                    }
                }
                DataRow dr = dt.NewRow();
                dr["角色"] = "团队管理员";
                dr["会员手机"] = baseUser.Phone;
                dr["会员姓名"] = baseUser.TrueName;
                dr["会员身份证"] = baseUser.IdentityCard;

                if (!string.IsNullOrWhiteSpace(baseUser.DistributionOwner))
                {
                    UserInfo bpUser = bllUser.GetUserInfo(baseUser.DistributionOwner, websiteOwner);
                    if (bpUser != null)
                    {
                        dr["推荐人手机"] = bpUser.Phone;
                        dr["推荐人姓名"] = bpUser.TrueName;
                    }
                }
                dr["会员级别"] = dicLevel.ContainsKey(baseUser.MemberLevel) ? dicLevel[baseUser.MemberLevel] : "";
                dr["状态"] = (baseUser.EmptyBill==1?"空单":"实单") +(baseUser.MemberApplyStatus==9?"已审":"未审");
                dr["注册时间"] = baseUser.Regtime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                dt.Rows.Add(dr);


                List<UserInfo> childs = bll.GetTeamList(userId, websiteOwner, true, maxLevel,
                    "AutoID,UserID,TrueName,WXNickname,Regtime,Phone,DistributionOwner,MemberLevel,EmptyBill,MemberApplyStatus,IdentityCard");
                foreach (var item in childs)
                {
                    UserInfo pu = childs.FirstOrDefault(p => p.UserID == item.DistributionOwner);
                    if (pu == null && item.DistributionOwner == baseUser.UserID) pu = baseUser;
                    DataRow drn = dt.NewRow();
                    drn["角色"] = "下级团员";
                    drn["会员手机"] = item.Phone;
                    drn["会员姓名"] = item.TrueName;
                    drn["会员身份证"] = item.IdentityCard;
                    drn["推荐人手机"] = pu.Phone;
                    drn["推荐人姓名"] = pu.TrueName;
                    drn["会员级别"] = dicLevel.ContainsKey(item.MemberLevel) ? dicLevel[item.MemberLevel] : "";
                    drn["状态"] = (item.EmptyBill == 1 ? "空单" : "实单") + (item.MemberApplyStatus == 9 ? "已审" : "未审");
                    drn["注册时间"] = item.Regtime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    dt.Rows.Add(drn);
                }
                dt.AcceptChanges();
            }
            MemoryStream ms = Web.DataLoadTool.NPOIHelper.Export(dt, "团队信息");
            ExportCache exCache = new ExportCache()
            {
                FileName = string.Format("团队信息{0}.xls", DateTime.Now.ToString("yyyyMMddHHmm")),
                Stream = ms
            };
            string cache = Guid.NewGuid().ToString("N").ToUpper();
            ZentCloud.Common.DataCache.SetCache(cache, exCache, slidingExpiration: TimeSpan.FromMinutes(5));

            apiResp.status = true;
            apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
            apiResp.msg = "生成完成";
            apiResp.result = new
            {
                cache = cache
            };
            bllUser.ContextResponse(context, apiResp);
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