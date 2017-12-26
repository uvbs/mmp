using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Account
{
    /// <summary>
    /// List 的摘要说明  查询子账户
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            StringBuilder sbWhere = new StringBuilder(string.Format("WebsiteOwner='{0}' And IsSubAccount='1'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendLine(string.Format(" AND TrueName  LIKE '%{0}%' ", keyWord));
            }
            int totalCount = this.bll.GetCount<UserInfo>(sbWhere.ToString());
            List<UserInfo> userList = this.bll.GetLit<UserInfo>(pageSize, pageIndex, sbWhere.ToString());
            resp.isSuccess = true;
            List<dynamic> returnList = new List<dynamic>();
            foreach (var item in userList)
            {
                returnList.Add(new 
                {
                    autoid = item.AutoID,
                    user_id=item.UserID,
                    user_pwd=item.Password,
                    wx_head_img_url=item.WXHeadimgurl,
                    true_name=item.TrueName,
                    user_company=item.Company,
                    user_postion=item.Postion,
                    user_phone=item.Phone,
                    user_email=item.Email,
                    user_vote_count=item.VoteCount,
                    user_isdisable=item.IsDisable
                });
            }
            resp.returnObj=new
            {
                totalcount=totalCount,
                list=returnList
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }
    }
}