using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Member
{
    /// <summary>
    /// ApplyList 的摘要说明
    /// </summary>
    public class ApplyList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        public void ProcessRequest(HttpContext context)
        {
            int page = Convert.ToInt32(context.Request["page"]);
            int rows = Convert.ToInt32(context.Request["rows"]);
            string keyword = context.Request["keyword"];
            string status = context.Request["status"];
            List<UserInfo> list = bllUser.GetMemberApplyList(rows, page, bllUser.WebsiteOwner, keyword, status);
            int total = bllUser.GetMemberApplyCount( bllUser.WebsiteOwner, keyword, status);
            List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMapByTableName(bllUser.WebsiteOwner, "ZCJ_UserInfo");
            List<string> DefFields = new List<string>() { "AutoID", "UserID", "Password", "UserType", "TrueName", "Phone", "Company", "Postion" };
            listFieldList = listFieldList.Where(p => !DefFields.Contains(p.Field)).OrderBy(p => p.Sort).ToList();

            Type t = currentUserInfo.GetType();
            PropertyInfo[] properties = t.GetProperties();

            List<JObject> result = new List<JObject>();
            foreach (var item in list)
            {
                JObject njo = new JObject();
                njo["AutoID"] = item.AutoID;
                njo["UserID"] = item.UserID;
                njo["TrueName"] = item.TrueName;
                njo["Phone"] = item.Phone;
                njo["Company"] = item.Company;
                njo["Postion"] = item.Postion;
                if (listFieldList.Count > 0)
                {
                    foreach (var field in listFieldList)
                    {
                        PropertyInfo p = properties.FirstOrDefault(pi => pi.Name == field.Field);
                        object val = p.GetValue(item, null);
                        if (val == null)
                        {
                            njo[field.Field] = "";
                        }
                        else
                        {
                            njo[field.Field] = val.ToString();
                        }
                    }
                }
                njo["MemberApplyStatus"] = item.MemberApplyStatus;
                njo["MemberApplyTime"] = item.MemberApplyTime.ToString("yyyy-MM-dd HH:mm:ss");
                result.Add(njo);
            }
            apiResp.result = new
            {
                totalcount = total,
                list = result
            };
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            bllUser.ContextResponse(context, apiResp);
        }
    }
}