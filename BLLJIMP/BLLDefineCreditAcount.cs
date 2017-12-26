using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    public class BLLDefineCreditAcount:BLL
    {
        /// <summary>
        /// 获取信用规则列表
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public List<DefineCreditAcount> GetDefineList(int pageSize, int pageIndex, string websiteOwner, out int total, int? isHide = 0)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' ", websiteOwner);
            if (isHide.HasValue) sbSql.AppendFormat(" AND IsHide={0} ", isHide);
            total = GetCount<DefineCreditAcount>(sbSql.ToString());
            return GetLit<DefineCreditAcount>(pageSize, pageIndex, sbSql.ToString(), "OrderNum,AutoID");
        }
        /// <summary>
        /// 获取规则
        /// </summary>
        /// <param name="type"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public DefineCreditAcount GetDefine(string type, string websiteOwner)
        {
            return Get<DefineCreditAcount>(string.Format(" Type='{0}' and  WebsiteOwner='{1}'", type, websiteOwner));
        }
        /// <summary>
        /// 获取规则
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public DefineCreditAcount GetDefine(int autoId)
        {
            return Get<DefineCreditAcount>(string.Format(" AutoID={0} ", autoId));
        }

    }
}
