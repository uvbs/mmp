using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using System.Data;

namespace ZentCloud.BLLJIMP
{
    public class BLLAdInfo : BLL
    {
        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="websiteOwner"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<AdInfo> GetAdInfoList(int pageSize, int pageIndex, int type, string websiteOwner, out int total)
        {

            string whereParam = string.Format("Type={0} and WebsiteOwner = '{1}'", type, websiteOwner);
            total = GetCount<AdInfo>(whereParam);
            return GetLit<AdInfo>(pageSize, pageIndex, whereParam, "Sort");
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="AutoId"></param>
        /// <returns></returns>
        public AdInfo GetAdInfo(int AutoId) {
            return Get<AdInfo>(string.Format("AutoId={0}", AutoId));
        }

        /// <summary>
        /// 提交标签修改
        /// </summary>
        /// <param name="ad"></param>
        /// <returns></returns>
        public bool PutAdInfo(AdInfo ad)
        {
            if (ad.AutoId == 0)
            {
                return Add(ad);
            }
            else
            {
                return Update(ad);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="AutoId"></param>
        /// <returns></returns>
        public bool DeleteAdInfo(int AutoId)
        { 
            return Delete(new AdInfo(),string.Format("AutoId={0}", AutoId))>0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="AutoId"></param>
        /// <returns></returns>
        public bool DeleteAdInfos(string AutoIds)
        {
            List<string> ids = AutoIds.Split(',').ToList();
            for (int i = 0; i < ids.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(ids[i])) 
                    DeleteAdInfo(int.Parse(ids[i]));
            }
            return true;
        }
    }
}
