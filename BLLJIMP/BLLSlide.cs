using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    public class BLLSlide : BLL
    {
        public List<Slide> ListByType(string type, string websiteOwner)
        {
            var key = string.Format("{0}:{1}:{2}", WebsiteOwner, Common.SessionKey.SliderByType, type);

            List<Slide> result = null;

            StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", websiteOwner));
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" And Type='{0}'", type);
            }
            sbWhere.Append(" order by Sort desc");
            
            try
            {
                var cacheDataStr = RedisHelper.RedisHelper.StringGet(key);

                if (string.IsNullOrWhiteSpace(cacheDataStr) || cacheDataStr == "[]")
                {
                    result = GetList<Slide>(sbWhere.ToString());
                    RedisHelper.RedisHelper.StringSet(key, JsonConvert.SerializeObject(result));
                }
                else
                {
                    result = JsonConvert.DeserializeObject<List<Slide>>(cacheDataStr);
                    
                }
            }
            catch (Exception ex)
            {
                result = GetList<Slide>(sbWhere.ToString());
            }
            
            return result;
            
        }

        /// <summary>
        /// 获取当前站点所有的幻灯片类型
        /// </summary>
        /// <returns></returns>
        public List<string> GetCurrWebsiteAllTypeList()
        {
            List<string> result = new List<string>();
            
            result = ListByType(null, WebsiteOwner).OrderBy(p => p.Type)
                    .Select(p => p.Type).Distinct().ToList();

            return result;
        }
    }
}
