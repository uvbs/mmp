using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Performance
{
    /// <summary>
    /// SetList 的摘要说明
    /// </summary>
    public class SetList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLDistribution bll = new BLLDistribution();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            int field_count = Convert.ToInt32(context.Request["field_count"]);
            if (field_count == 0)
            {
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                apiResp.status = false;
                apiResp.msg = "请传入字段数";
                apiResp.result = new
                {
                    totalcount = 0,
                    list = new List<string>()
                };
                bll.ContextResponse(context, apiResp);
                return;
            }
            rows = rows * field_count;
            string phone = context.Request["phone"];
            string websiteOwner = bll.WebsiteOwner;
            string searchUserId = "";
            if (!string.IsNullOrWhiteSpace(phone)){
                UserInfo searchUser = bll.GetColByKey<UserInfo>("Phone", phone, "AutoID,UserID", websiteOwner: websiteOwner);
                searchUserId = searchUser == null ? "" : searchUser.UserID;
            }
            JArray resultList = new JArray();
            int total = bll.GetSetCount(websiteOwner, searchUserId);
            List<TeamPerformanceSet> userSetList = new List<TeamPerformanceSet>();
            if(total>0) userSetList = bll.GetSetList(rows, page, websiteOwner, searchUserId);
            if(userSetList.Count >0){
                List<string> userIdList = userSetList.Select(p=>p.UserId).Distinct().ToList();
                string userIds = ZentCloud.Common.MyStringHelper.ListToStr(userIdList,"'",",");
                List<UserInfo> userList = bll.GetColMultListByKey<UserInfo>(int.MaxValue,1,"UserID",userIds,"AutoID,UserID,TrueName,Phone");
                foreach (string userId in userIdList)
	            {
                    UserInfo user = userList.FirstOrDefault(p => p.UserID == userId);

                    JObject jItem = new JObject();
                    jItem["user_id"] = userId;
                    
                    if (userId == websiteOwner)
                    {
                        jItem["user_phone"] = "";
                        jItem["user_name"] = "系统";
                        jItem["is_sys"] = 1;
                        jItem["user_auto_id"] = user.AutoID;
                    }
                    else
                    {
                        jItem["user_phone"] = user == null ? "" : user.Phone;
                        jItem["user_name"] = user == null ? "" : user.TrueName;
                        jItem["is_sys"] = 0;
                        jItem["user_auto_id"] = 0;
                    }
                    foreach (TeamPerformanceSet item in userSetList.Where(p => p.UserId == userId))
                    {
                        jItem["p"+ Convert.ToInt64(item.Performance) ] = Convert.ToInt32(item.RewardRate) + "%";
                    }
                    resultList.Add(jItem);
	            }
            }

            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "管理奖设置列表";
            apiResp.result = new
            {
                totalcount = total / field_count,
                list = resultList
            };
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