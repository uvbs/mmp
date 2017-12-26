using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Expand
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUserExpand bllUserEx = new BLLUserExpand();
        public void ProcessRequest(HttpContext context)
        {
            int rows = Convert.ToInt32(context.Request["rows"]);
            int page = Convert.ToInt32(context.Request["page"]);
            string type = context.Request["type"];
            string member = context.Request["member"];
            string websiteOwner = bllUser.WebsiteOwner;
            string userIds = "";
            UserExpandType nType = new UserExpandType();
            if (!Enum.TryParse(type, out nType))
            {
                apiResp.code = (int)APIErrCode.OperateFail;
                apiResp.msg = "类型格式不能识别";
                bllUserEx.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(member)) userIds = bllUser.GetSpreadUserIds(member, websiteOwner);
            int total = bllUserEx.GetExpandListCount(nType, userIds);
            List<dynamic> rlist = new List<dynamic>();
            if (total > 0) { 
                List<UserExpand> list = bllUserEx.GetExpandList(rows, page, nType, userIds);
                if (list.Count > 0)
                {
                    string uIds = ZentCloud.Common.MyStringHelper.ListToStr(list.Select(p=>p.UserId).ToList(), "'", ",");
                    List<UserInfo> uList = bllUser.GetColMultListByKey<UserInfo>(int.MaxValue, 1, "UserID", uIds, "AutoID,WXNickname,TrueName,Phone,UserID", websiteOwner: websiteOwner);
                    foreach (var item in list)
                    {
                        UserInfo u = uList.FirstOrDefault(p => p.UserID == item.UserId);
                        rlist.Add(new
                        {
                            id = u == null ? 0 : u.AutoID,
                            userid = u == null ? "" : u.UserID,
                            nickname = u == null ? "" : bllUser.GetUserDispalyName(u),
                            phone = u == null ? "" : u.Phone,
                            value = item.DataValue,
                            ex1 = item.Ex1,
                            ex2 = item.Ex2,
                            ex3 = item.Ex3,
                            ex4 = item.Ex4,
                            ex5 = item.Ex5,
                            ex6 = item.Ex6,
                            ex7 = item.Ex7,
                            ex8 = item.Ex8,
                            ex9 = item.Ex9,
                            ex10 = item.Ex10
                        });
                    }
                }
            }
            apiResp.result = new
            {
                totalcount = total,
                list = rlist
            };
            apiResp.status = true;
            apiResp.msg = "查询扩展信息";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bllUserEx.ContextResponse(context, apiResp);
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