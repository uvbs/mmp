using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User
{
    /// <summary>
    /// ListByCompany 的摘要说明 公司列表
    /// </summary>
    public class ListByCompany : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 用户 BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();

        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["page"]) ? int.Parse(context.Request["page"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["rows"]) ? int.Parse(context.Request["rows"]) : 20;
            string keyWord = context.Request["keyword"];//关键字
            string userType=context.Request["user_type"];
            string applyStatus=context.Request["apply_status"];

            int total = 0;

            List<UserInfo> companyList = bllUser.GetUserInfoByAllCompany(pageSize,pageIndex,keyWord,userType,applyStatus,out total);

            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in companyList)
            {
                returnList.Add(new 
                {
                    autoid=item.AutoID,
                    userid=item.UserID,
                    company=item.Company,
                    phone=item.Phone,
                    password=item.Password,
                    wxnickname=item.WXNickname,
                    insert_time=item.Regtime,
                    apply_status=item.MemberApplyStatus,
                    isdisable=item.IsDisable,
                    truename = bllUser.GetUserDispalyName(item),
                    avatar = bllUser.GetUserDispalyAvatar(item),
                    score = item.TotalScore,
                });
            }

            var data = new
            {
                rows = returnList,
                total = total
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(data));
        }

        
    }
}