using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// BLL 转发
    /// </summary>
    public class BLLActivityForwardInfo : BLL
    {
        /// <summary>
        /// 获取转发列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyWord"></param>
        /// <param name="forwardType"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ActivityForwardInfo> GetActivityForwardList(int pageSize,int pageIndex,string keyWord,string forwardType,string order, string sort,out int totalCount)
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" WebsiteOwner='{0}' ",WebsiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND ActivityName like '%{0}%' ",keyWord);
            }
            if (!string.IsNullOrEmpty(forwardType))
            {
                sbWhere.AppendFormat(" AND ForwardType='{0}' ",forwardType);
            }
            totalCount = GetCount<ActivityForwardInfo>(sbWhere.ToString());

            string orderBy = "";
            switch (sort)
            {
                case "ippv":
                    orderBy = " PV " + order;
                    break;
                case "uv":
                    orderBy = " UV " + order;
                    break;
                default:
                    orderBy = " InsertDate DESC ";
                    break;
            }
            return GetLit<ActivityForwardInfo>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
        }

        /// <summary>
        /// 获取转发信息详情
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public ActivityForwardInfo GetActivityForwardInfo(string activityId)
        {
            return Get<ActivityForwardInfo>(string.Format(" WebsiteOwner='{0}' AND ActivityId='{1}' ",WebsiteOwner,activityId));
        }
    }
}
