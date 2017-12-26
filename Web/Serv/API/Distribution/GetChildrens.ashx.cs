using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Distribution
{
    /// <summary>
    /// GetChildrens 的摘要说明
    /// </summary>
    public class GetChildrens : BaseHandlerNeedLoginNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string parentId = context.Request["parent_id"];
            string is_website = context.Request["is_website"];
            string has_base = context.Request["has_base"];
            string websiteOwner = bll.WebsiteOwner;
            if (string.IsNullOrWhiteSpace(parentId) && is_website == "1")
            {
                parentId = websiteOwner;
            }
            else if (string.IsNullOrWhiteSpace(parentId))
            {
                parentId = CurrentUserInfo.UserID;
            }
            bool hideName = false;
            if (context.Request["hide_name"] == "1") hideName = true;
            bool hidePhone = false;
            if (context.Request["hide_phone"] == "1") hidePhone = true;
            string limit_level = context.Request["limit_level"];

            string type = context.Request["type"];
            if(string.IsNullOrWhiteSpace(type)) type = "DistributionOnLine";
            WebsiteInfo website = bll.GetWebsiteInfoModelFromDataBase();
            int limiLevel = 0;
            if (string.IsNullOrWhiteSpace(limit_level))
            {
                limiLevel = website.DistributionLimitLevel;
            }
            else
            {
                limiLevel = Convert.ToInt32(limit_level);
            }
            List<UserLevelConfig> lvlist = bll.QueryUserLevelList(websiteOwner, type,colName:"AutoId,LevelNumber,LevelString");
            Dictionary<int, string> dicLevel = new Dictionary<int, string>();
            foreach (UserLevelConfig item in lvlist) {
                if (!dicLevel.ContainsKey(item.LevelNumber)){
                    dicLevel.Add(item.LevelNumber, item.LevelString);
                }
            }
            List<user> uList = new List<user>();
            string parentIds = "";
            for (int i = 0; i < website.DistributionLimitLevel; i++)
			{
                List<UserInfo> nextList = bll.GetChildrenIdList(parentId, websiteOwner, true, "AutoID,UserID");
                if(nextList.Count ==0) break;
                parentIds = ZentCloud.Common.MyStringHelper.ListToStr(nextList.Select(p=>p.UserID).ToList(),"",",");
                uList.AddRange(from p in nextList
                              select new user
                              {
                                  id = p.AutoID,
                                  childlevel = i+1
                              });
			}

            List<UserInfo> list = new List<UserInfo>();
            if (uList.Count > 0)
            {
                string Ids = ZentCloud.Common.MyStringHelper.ListToStr(uList.Select(p => p.id).ToList(), "", ",");
                list = bll.GetChildrenList(rows, page, Ids, websiteOwner, true, "AutoID,UserID,TrueName,WXNickname,Regtime,Phone,DistributionOwner,MemberLevel,EmptyBill,MemberApplyStatus");
            }
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "获取团队";
            if (has_base =="1")
            {
                UserInfo baseUser = bllUser.GetUserInfo(parentId,websiteOwner);
                string bpPhone = "";
                if (!string.IsNullOrWhiteSpace(baseUser.DistributionOwner))
                {
                    UserInfo bpUser = bllUser.GetUserInfo(baseUser.DistributionOwner, websiteOwner);
                    if (bpUser!=null) bpPhone = bpUser.Phone;
                }
                apiResp.result = new
                {
                    totalcount = list.Count,
                    limitlevel = limiLevel,
                    base_user = new
                    {
                        id = baseUser.UserID,
                        aid = baseUser.AutoID,
                        pid = baseUser.DistributionOwner,
                        pphone = bpPhone,
                        name = bllUser.GetUserDispalyName(baseUser, hideName),
                        phone = bllUser.GetUserDispalyPhone(baseUser.Phone, hidePhone),
                        regtime = !baseUser.Regtime.HasValue ? "" : baseUser.Regtime.Value.ToString("yyyy/MM/dd HH:mm"),
                        empty_bill = baseUser.EmptyBill,
                        has_child = list.Count>0,
                        apply_status = baseUser.MemberApplyStatus,
                        member_lvname = dicLevel.ContainsKey(baseUser.MemberLevel) ? dicLevel[baseUser.MemberLevel] : "",
                        childlevel = 0
                    },
                    list = from p in list
                           select new
                           {
                               id = p.UserID,
                               aid = p.AutoID,
                               pid = p.DistributionOwner,
                               name = bllUser.GetUserDispalyName(p, hideName),
                               phone = bllUser.GetUserDispalyPhone(p.Phone, hidePhone),
                               regtime = !p.Regtime.HasValue ? "" : p.Regtime.Value.ToString("yyyy/MM/dd HH:mm"),
                               empty_bill = p.EmptyBill,
                               has_child = bll.HaveChildrens(websiteOwner, p.UserID, true),
                               apply_status = p.MemberApplyStatus,
                               member_lvname = dicLevel.ContainsKey(p.MemberLevel) ? dicLevel[p.MemberLevel] : "",
                               childlevel = uList.FirstOrDefault().childlevel
                           }
                };
            }
            else
            {
                apiResp.result = new
                {
                    totalcount = list.Count,
                    limitlevel = limiLevel,
                    base_user = false,
                    list = from p in list
                           select new
                           {
                               id = p.UserID,
                               aid = p.AutoID,
                               pid = p.DistributionOwner,
                               name = bllUser.GetUserDispalyName(p, hideName),
                               phone = bllUser.GetUserDispalyPhone(p.Phone, hidePhone),
                               regtime = !p.Regtime.HasValue ? "" : p.Regtime.Value.ToString("yyyy/MM/dd HH:mm"),
                               empty_bill = p.EmptyBill,
                               has_child = bll.HaveChildrens(websiteOwner, p.UserID, true),
                               apply_status = p.MemberApplyStatus,
                               member_lvname = dicLevel.ContainsKey(p.MemberLevel) ? dicLevel[p.MemberLevel] : "",
                               childlevel = uList.FirstOrDefault().childlevel
                           }
                };
            }
            bll.ContextResponse(context, apiResp);
        }
        public class user
        {
            public int id { get; set; }
            public int childlevel { get; set; }
        }

    }
}