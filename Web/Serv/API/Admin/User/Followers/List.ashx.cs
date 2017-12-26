using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.User.Followers
{
    /// <summary>
    /// List 的摘要说明   查询全部粉丝
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = !string.IsNullOrEmpty(context.Request["pageindex"]) ? int.Parse(context.Request["pageindex"]) : 1;
            int pageSize = !string.IsNullOrEmpty(context.Request["pagesize"]) ? int.Parse(context.Request["pagesize"]) : 10;
            string keyWord = context.Request["keyword"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder(string.Format(" WebSiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND NickName like '%{0}%'",keyWord);
            }

            int totalCount = bll.GetCount<WeixinFollowersInfo>(sbWhere.ToString());

            var followerList = bll.GetLit<WeixinFollowersInfo>(pageSize,pageIndex,sbWhere.ToString()," AutoID Desc");

            resp.isSuccess = true;

            List<dynamic> returnList = new List<dynamic>();

            foreach (var item in followerList)
            {
                returnList.Add(new{
                    autoid=item.AutoID,
                    nice_name=item.NickName,
                    sex=item.Sex,
                    head_img_url=item.HeadImgUrl,
                    country=item.Country,
                    city=item.City,
                    province = item.Province
                });
            }

            resp.returnObj = new
            {
                totalcount=totalCount,
                list=returnList
            };
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }


        public class RequestModel 
        {
            /// <summary>
            /// 微信名称
            /// </summary>
            public string nice_name { get; set; }

            /// <summary>
            /// 性别
            /// </summary>
            public int sex { get; set; }

            /// <summary>
            /// 头像
            /// </summary>
            public string head_img_url { get; set; }

            /// <summary>
            /// 国家
            /// </summary>
            public string country { get; set; }

            /// <summary>
            /// 城市
            /// </summary>
            public string city { get; set; }

            /// <summary>
            /// 省份
            /// </summary>
            public string province { get; set; }
        }
    }
}