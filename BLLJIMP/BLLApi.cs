using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// api业务逻辑层
    /// </summary>
    public class BLLApi:BLL
    {
        /// <summary>
        /// 获取api模块详情
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public ApiModule GetApiModule(int moduleId)
        {
            return Get<ApiModule>(string.Format(" AutoID={0} ",moduleId));
        }
        /// <summary>
        /// 获取api模块列表列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="keyWord"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public List<ApiModule> GetApiModuleList(int pageSize,int pageIndex,string keyWord,string sort,out int totalCount)
        {
            StringBuilder sbWhere=new StringBuilder();
            if (!string.IsNullOrEmpty(keyWord))
            {
                sbWhere.AppendFormat(" AND ModuleName like '%{0}%' ",keyWord);
            }
            string orderBy=" AutoID DESC ";
            if(!string.IsNullOrEmpty(sort)){
                orderBy=" SORT DESC ";
            }
            totalCount = GetCount<ApiModule>(sbWhere.ToString());
            return GetLit<ApiModule>(pageSize, pageIndex, sbWhere.ToString(), orderBy);
        }


    }
}
