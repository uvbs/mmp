using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ZentCloud.BLLJIMP
{
    public class BLLShareMonitor:BLL
    {
        public BLLShareMonitor()
            : base()
        {

        }

        /// <summary>
        /// 获取分享实体
        /// </summary>
        /// <param name="shareId"></param>
        /// <returns></returns>
        public Model.ShareInfo GetShareInfo(string shareId)
        {
            return Get<Model.ShareInfo>(string.Format(" ShareId = '{0}' ", shareId));
        }

        /// <summary>
        /// 判断是否存在分享实体
        /// </summary>
        /// <param name="shareId"></param>
        /// <returns></returns>
        public bool ExistsShareInfo(string shareId)
        {
            return GetCount<Model.ShareInfo>(string.Format(" ShareId = '{0}' ", shareId)) > 0;
        }

        /// <summary>
        /// 根据url获取检测任务
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Model.ShareMonitorInfo GetMonitorByUrl(string url)
        {
            Model.ShareMonitorInfo result = new Model.ShareMonitorInfo();

            //根据url查找监测任务，移除原有参数 comeonshareid from isappinstalled，判断原始链接  增加过滤签到标识
            //移除http:// https://

            url = url.ToLower().Replace("http://", "").Replace("https://", "").Replace("amp;amp;amp;","").Replace("amp;amp;","");

            url = CommonPlatform.Helper.StringHandler.UrlRemoveParm(
                url,
                new List<string>() { "comeonshareid", "from", "isappinstalled", "redirect" }
            );

            List<Model.ShareMonitorInfo> monitorList = new List<Model.ShareMonitorInfo>();
            
            //monitorList = GetList<Model.ShareMonitorInfo>(string.Format(" MonitorUrl like'%{0}%' AND IsDel = 0 AND WebSiteOwner = '{1}' ", url,WebsiteOwner));            
            //if (monitorList != null && monitorList.Count > 0)
            //{
            //    monitorList = monitorList.OrderBy(p => p.MonitorUrl.Length).ToList();
            //    result = monitorList[0];
            //}
            
            //新算法
            monitorList = GetCurrMonitorList();
            monitorList = monitorList.Where(p => p.MonitorUrl.IndexOf(url,StringComparison.OrdinalIgnoreCase) > -1).ToList();
            if (monitorList != null && monitorList.Count > 0)
            {
                monitorList = monitorList.OrderBy(p => p.MonitorUrl.Length).ToList();
                result = monitorList[0];
            }
            else
            {
                /*
                判断，如果是chtml结尾的，则为活动或者文章任务
                针对当前url和活动id
                新建一个监测任务
                并记录下外键id和类型（文章/活动）                
                */

                if (url.IndexOf(".chtml", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    int activityId = 0;

                    try
                    {
                        string[] parameters = url.Split('/');
                        activityId = Convert.ToInt32(parameters[1], 16);
                    }
                    catch (Exception ex)
                    {
                        
                    }

                    BLLJuActivity bllJuActivity = new BLLJuActivity();

                    var juModel = bllJuActivity.GetJuActivity(activityId);

                    if (juModel != null)
                    {

                        //一个活动只建立一个微监测
                        var monitorInfoByJuActivity = GetMonitorByFK(juModel.JuActivityID.ToString(), juModel.ArticleType);

                        if (monitorInfoByJuActivity != null)
                        {
                            result = monitorInfoByJuActivity;
                        }
                        else {
                            Model.ShareMonitorInfo newMonitorInfo = new Model.ShareMonitorInfo()
                            {
                                MonitorId = Convert.ToInt32(GetGUID(TransacType.CommAdd)),
                                CreateTime = DateTime.Now,
                                CreateUser = "system",
                                ForeignkeyId = juModel.JuActivityID.ToString(),
                                IsDel = 0,
                                MonitorName = "[系统建立]" + juModel.ActivityName,
                                MonitorType = juModel.ArticleType,//类型以后有多地方用则用个枚举管理起来，防止冲突重名
                                MonitorUrl = url,
                                ReadCount = 0,
                                ShareCount = 0,
                                WebSiteOwner = WebsiteOwner

                            };

                            if (Add(newMonitorInfo))
                            {
                                UpdateCurrMonitorList();
                                result = newMonitorInfo;
                            }
                        }

                    }
                }            
            }
            
            return result;
        }

        public Model.ShareMonitorInfo GetMonitorByFK(string fkId, string type)
        {
            Model.ShareMonitorInfo result = Get<Model.ShareMonitorInfo>(
                string.Format(" [ForeignkeyId] = '{0}' AND [MonitorType] = '{1}' AND [WebSiteOwner] = '{2}' ", 
                    fkId, 
                    type, 
                    WebsiteOwner
                ));

            return result;
        }

        private string GetRedisMonitorListKey() {
            return WebsiteOwner + ":ShareMonitorList";
        }

        public List<Model.ShareMonitorInfo> GetCurrMonitorList()
        {
            var redisKey = GetRedisMonitorListKey();

            List<Model.ShareMonitorInfo> monitorList = new List<Model.ShareMonitorInfo>();

            try
            {
                monitorList = RedisHelper.RedisHelper.StringGet<List<Model.ShareMonitorInfo>>(redisKey);
            }
            catch (Exception ex)
            {
                
            }

            if (monitorList == null)
            {
                monitorList = new List<Model.ShareMonitorInfo>();
            }

            if (monitorList.Count == 0)
            {
                monitorList = UpdateCurrMonitorList();                
            }

            return monitorList;
        }

        public List<Model.ShareMonitorInfo> UpdateCurrMonitorList()
        {
            var redisKey = GetRedisMonitorListKey();

            List<Model.ShareMonitorInfo> monitorList = GetList<Model.ShareMonitorInfo>(string.Format(" IsDel = 0 AND WebSiteOwner = '{0}' ", WebsiteOwner));

            if (monitorList.Count > 0)
            {
                try
                {
                    RedisHelper.RedisHelper.StringSet(redisKey, JsonConvert.SerializeObject(monitorList));
                }
                catch (Exception ex)
                {
                    
                }
            }

            return monitorList;
        }


    }
}
