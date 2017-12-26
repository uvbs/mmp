using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud;
using ZentCloud.ZCBLLEngine;
using System.Data;
using System.Data.SqlClient;
using ZentCloud.BLLJIMP.Model;
using System.Web;
using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json;
using ZentCloud.ZCDALEngine;

namespace ZentCloud.BLLJIMP
{

    public static class MyEnumerableExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element))) { yield return element; }
            }
        }
    }

    /// <summary>
    /// 逻辑基类
    /// </summary>
    [Serializable]
    public class BLL : BLLBase
    {
        public BLL()
        {
            _userid = "";
        }
        public BLL(string userID)
        {
            _userid = userID;
        }
        private string _userid;

        public string UserID
        {
            get { return _userid; }
        }
        /// <summary>
        /// 重写数据库表名
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        protected override string GetRealTableName(string modelName)
        {
            string tableName = modelName.EndsWith("Ex", true, null) ? modelName.Substring(0, modelName.Length - 2) : modelName;

            return "ZCJ_" + tableName;
        }
        /// <summary>
        /// 创建GUID
        /// </summary>
        /// <param name="transacType"></param>
        /// <returns></returns>
        public string GetGUID(TransacType transacType)
        {
            string strSql = string.Format(
                                @"insert into ZCJ_GUID (UserID, TransacDescription, TransacDate) 
                                    values ('{0}', {1}, GETDATE()) select @@IDENTITY",
                                                           this.UserID, (int)transacType);
            return GetSingle(strSql).ToString();

        }

        public string GetNewGUID(string websiteowner,string type, int length)
        {
            string newNum = "1";
            NewGUID model = Get<NewGUID>(string.Format(" WebsiteOwner='{0}' AND Type='{1}' ",websiteowner,type));
            if (model != null) {
                newNum = (model.Num + 1).ToString(); 
                if(newNum.Length > length) return newNum;
                return newNum.PadLeft(length,'0');
            }
            else
            {
                Add(new NewGUID() { WebsiteOwner = websiteowner, Type = type, Num = 0 });
                if (newNum.Length > length) return newNum;
                return newNum.PadLeft(length, '0');
            }
        }
        public void UpdateNewGUID(string websiteowner, string type)
        {
            NewGUID model = Get<NewGUID>(string.Format(" WebsiteOwner='{0}' AND Type='{1}' ", websiteowner, type));
            model.Num++;
            Update(model);
        }

        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <param name="total">总数</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns></returns>
        public int GetTotalPage(decimal total, decimal pageSize)
        {
            try
            {
                return (int)Math.Ceiling(total / pageSize);
            }
            catch { }
            return 0;
        }

        ///// <summary>
        ///// 获取缓存列表分页数据
        ///// </summary>
        ///// <typeparam name="T">泛型类型</typeparam>
        ///// <param name="pageSize">页面大小</param>
        ///// <param name="pageIndex">从1开始</param>
        ///// <param name="list"></param>
        ///// <returns>分页数据</returns>
        //public List<T> GetPageList<T>(int pageSize, int pageIndex, List<T> list)
        //{
        //    if (pageIndex > 0)
        //        --pageIndex;
        //    int starIndex = pageIndex * pageSize;

        //    List<T> result = new List<T>();

        //    for (int i = 0; i < pageSize; i++)
        //    {
        //        try
        //        {
        //            int j = starIndex + i;
        //            if (j > list.Count - 1)
        //                break;
        //            result.Add(list[j]);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// 获取系统配置信息
        /// </summary>
        /// <returns></returns>
        public Model.SystemSet GetSysSet()
        {
            return Get<Model.SystemSet>("");//系统配置信息
        }

        /// <summary>
        /// 传输过程int编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string TransmitIntEnCode(int input)
        {
            string result = "";
            //原始id*1000，16进制后进行base64，然后“=”变成“_”
            result = Common.Base64Change.EncodeBase64ByUTF8(Convert.ToString(input * 1000, 16)).Replace("=", "_");

            return result;
        }

        /// <summary>
        /// 传输过程int解码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int TransmitIntDeCode(string input)
        {
            int result = 0;
            result = Convert.ToInt32(Common.Base64Change.DecodeBase64ByUTF8(input.Replace("_", "=")), 16) / 1000;
            return result;
        }

        /// <summary>
        /// 传输过程string编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string TransmitStringEnCode(string input)
        {
            string result = "";
            //原始进行base64，然后“=”变成“_”
            result = Common.Base64Change.EncodeBase64ByUTF8(input).Replace("=", "_");

            return result;
        }

        /// <summary>
        /// 传输过程string解码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string TransmitStringDeCode(string input)
        {
            string result = "";

            result = Common.Base64Change.DecodeBase64ByUTF8(input.Replace("_", "="));

            return result;
        }

        ///// <summary>
        ///// 发送系统短信
        ///// </summary>
        ///// <param name="mobile"></param>
        ///// <param name="cnt"></param>
        ///// <returns></returns>
        //public bool SendSysSms(string mobile, string cnt)
        //{
        //    Model.SystemSet sysSet = GetSysSet();

        //    //构造发送url
        //    string url = string.Format("{0}?userName={1}&userPwd={2}&mobile={3}&content={4}&pipeID=membertrigger",
        //            sysSet.SysSmsApi,
        //            sysSet.SysTriggerSmsUserID,
        //            sysSet.SysTriggerSmsUserPwd,
        //            mobile,
        //            cnt
        //        );

        //    string result = Common.MySpider.GetPageSourceForUTF8(url);

        //    if (result == "0")
        //        return true;

        //    return false;
        //}

        /// <summary>
        /// 获取当前站点信息
        /// </summary>
        /// <returns></returns>
        public BLLJIMP.Model.WebsiteInfo GetWebsiteInfoModel()
        {
            
            var websiteInfo = GetWebsiteInfoModelFromDataBase(WebsiteOwner);
            FilterCompanyWebsiteInfo(ref websiteInfo);
            return websiteInfo;
            //if (System.Web.HttpContext.Current.Session["WebsiteInfoModel"] != null)
            //{
            //    var websiteInfo = (BLLJIMP.Model.WebsiteInfo)System.Web.HttpContext.Current.Session["WebsiteInfoModel"];
            //    FilterCompanyWebsiteInfo(ref websiteInfo);
            //    return websiteInfo;
            //}

            //return null;
        }

        /// <summary>
        /// 获取当前站点信息 Redis
        /// </summary>
        /// <returns></returns>
        public WebsiteInfo GetWebsiteInfoModelFromDataBase()
        {
            return GetWebsiteInfoModelFromDataBase(WebsiteOwner);

        }
        /// <summary>
        /// 获取指定站点信息 Redis
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public WebsiteInfo GetWebsiteInfoModelFromDataBase(string websiteOwner)
        {

            return new BLLRedis().GetWebsiteInfo(websiteOwner);
            //return Get<WebsiteInfo>(string.Format("WebsiteOwner='{0}'", websiteOwner));
        }

        /// <summary>
        /// 站点信息过滤处理
        /// </summary>
        /// <param name="websiteInfo"></param>
        public void FilterCompanyWebsiteInfo(ref WebsiteInfo websiteInfo)
        {
            if (string.IsNullOrWhiteSpace(websiteInfo.SmsSignature))
            {
                websiteInfo.SmsSignature = websiteInfo.WebsiteName;
            }
            if (websiteInfo.SmsSignature == "((0))")
            {
                websiteInfo.SmsSignature = websiteInfo.WebsiteName;
            }
        }

        /// <summary>
        /// 获取当前站点所有者信息
        /// </summary>
        /// <returns></returns>
        public Model.UserInfo GetCurrWebSiteUserInfo()
        {
            return Get<UserInfo>(string.Format(" UserID='{0}' and WebsiteOwner='{0}' ", WebsiteOwner));//
        }
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public DateTime GetTime(long timeStamp)
        {
            //return  new DateTime(1970, 1, 1).AddMilliseconds(timeStamp).AddHours(8);
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(timeStamp).ToLocalTime();

        }
        //public Model.AnalyticsViewModel GetAnalyticsViewModel()
        //{


        //    Model.AnalyticsViewModel result = new Model.AnalyticsViewModel();

        //    #region 新用户统计
        //    /*
        //     * 用户数统计
        //     * 判断标准：只有有微信昵称的，进入手机主页必会经过userinfo授权，取得授权必得微信昵称
        //     * 
        //     */

        //    result.NewUserToday = GetCount<Model.UserInfo>(string.Format("DATEDIFF(DAY,Regtime,GETDATE()) = 0 and WXNickname is not null and WXNickname <> '' And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.NewUserYesterday = GetCount<Model.UserInfo>(string.Format("DATEDIFF(DAY,Regtime,DATEADD(day,-1, GETDATE())) = 0 and WXNickname is not null and WXNickname <> ''And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.NewUserThisWeek = GetCount<Model.UserInfo>(string.Format("DATEDIFF(WEEK,Regtime,GETDATE()) = 0 and WXNickname is not null and WXNickname <> ''And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.NewUserThisMonth = GetCount<Model.UserInfo>(string.Format("DATEDIFF(MONTH,Regtime,GETDATE()) = 0 and WXNickname is not null and WXNickname <> ''And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.NewUserAll = GetCount<Model.UserInfo>(string.Format("WXNickname is not null and WXNickname <> ''And WebsiteOwner='{0}'", WebsiteOwner));

        //    #endregion

        //    #region PC主页访问统计

        //    result.PCIndexViewIPAll = GetCount<Model.WebAccessLogsInfo>("IP", string.Format("pageurl like '%Workbench.aspx%' And WebsiteOwner='{0}'", WebsiteOwner));



        //    result.PCIndexViewIPThisMonth = GetCount<Model.WebAccessLogsInfo>("IP", string.Format("DATEDIFF(MONTH,AccessDate,GETDATE()) = 0 and pageurl like '%Workbench.aspx%' And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.PCIndexViewIPThisWeek = GetCount<Model.WebAccessLogsInfo>("IP", string.Format("DATEDIFF(WEEK,AccessDate,GETDATE()) = 0 and pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.PCIndexViewIPToday = GetCount<Model.WebAccessLogsInfo>("IP", string.Format("DATEDIFF(DAY,AccessDate,GETDATE()) = 0 and pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.PCIndexViewIPYesterday = GetCount<Model.WebAccessLogsInfo>("IP", string.Format("DATEDIFF(DAY,AccessDate,DATEADD(day,-1, GETDATE())) = 0 and pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.PCIndexViewUVAll = GetCount<Model.WebAccessLogsInfo>("userid", string.Format("pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.PCIndexViewUVThisMonth = GetCount<Model.WebAccessLogsInfo>("userid", string.Format("DATEDIFF(MONTH,AccessDate,GETDATE()) = 0 and pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.PCIndexViewUVThisWeek = GetCount<Model.WebAccessLogsInfo>("userid", string.Format("DATEDIFF(WEEK,AccessDate,GETDATE()) = 0 and pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.PCIndexViewUVToday = GetCount<Model.WebAccessLogsInfo>("userid", string.Format("DATEDIFF(DAY,AccessDate,GETDATE()) = 0 and pageurl like '%Workbench.aspx%' And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.PCIndexViewUVYesterday = GetCount<Model.WebAccessLogsInfo>("userid", string.Format("DATEDIFF(DAY,AccessDate,DATEADD(day,-1, GETDATE())) = 0 and pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.PCIndexViewPVAll = GetCount<Model.WebAccessLogsInfo>(string.Format("pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.PCIndexViewPVThisMonth = GetCount<Model.WebAccessLogsInfo>(string.Format("DATEDIFF(MONTH,AccessDate,GETDATE()) = 0 and pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.PCIndexViewPVThisWeek = GetCount<Model.WebAccessLogsInfo>(string.Format("DATEDIFF(WEEK,AccessDate,GETDATE()) = 0 and pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.PCIndexViewPVToday = GetCount<Model.WebAccessLogsInfo>(string.Format("DATEDIFF(DAY,AccessDate,GETDATE()) = 0 and pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.PCIndexViewPVYesterday = GetCount<Model.WebAccessLogsInfo>(string.Format("DATEDIFF(DAY,AccessDate,DATEADD(day,-1, GETDATE())) = 0 and pageurl like '%Workbench.aspx%'  And WebsiteOwner='{0}'", WebsiteOwner));


        //    #endregion

        //    #region 手机主页访问统计
        //    result.MobileIndexViewIPAll = GetCount<Model.WebAccessLogsInfo>("IP", string.Format("pageurl like '%UserHub.aspx%' And WebsiteOwner='{0}'", WebsiteOwner));


        //    result.MobileIndexViewIPThisMonth = GetCount<Model.WebAccessLogsInfo>("IP", string.Format("DATEDIFF(MONTH,AccessDate,GETDATE()) = 0 and pageurl like '%UserHub.aspx%' And WebsiteOwner='{0}'", WebsiteOwner));


        //    result.MobileIndexViewIPThisWeek = GetCount<Model.WebAccessLogsInfo>("IP", string.Format("DATEDIFF(WEEK,AccessDate,GETDATE()) = 0 and pageurl like '%UserHub.aspx%'And WebsiteOwner='{0}'", WebsiteOwner));



        //    result.MobileIndexViewIPToday = GetCount<Model.WebAccessLogsInfo>("IP", string.Format("DATEDIFF(DAY,AccessDate,GETDATE()) = 0 and pageurl like '%UserHub.aspx%'And WebsiteOwner='{0}'", WebsiteOwner));


        //    result.MobileIndexViewIPYesterday = GetCount<Model.WebAccessLogsInfo>("IP", string.Format("DATEDIFF(DAY,AccessDate,DATEADD(day,-1, GETDATE())) = 0 and pageurl like '%UserHub.aspx%'And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.MobileIndexViewUVAll = GetCount<Model.WebAccessLogsInfo>("userid", string.Format("pageurl like '%UserHub.aspx%'And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.MobileIndexViewUVThisMonth = GetCount<Model.WebAccessLogsInfo>("userid", string.Format("DATEDIFF(MONTH,AccessDate,GETDATE()) = 0 and pageurl like '%UserHub.aspx%' And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.MobileIndexViewUVThisWeek = GetCount<Model.WebAccessLogsInfo>("userid", string.Format("DATEDIFF(WEEK,AccessDate,GETDATE()) = 0 and pageurl like '%UserHub.aspx%' And WebsiteOwner='{0}'", WebsiteOwner));


        //    result.MobileIndexViewUVToday = GetCount<Model.WebAccessLogsInfo>("userid", string.Format("DATEDIFF(DAY,AccessDate,GETDATE()) = 0 and pageurl like '%UserHub.aspx%' And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.MobileIndexViewUVYesterday = GetCount<Model.WebAccessLogsInfo>("userid", string.Format("DATEDIFF(DAY,AccessDate,DATEADD(day,-1, GETDATE())) = 0 and pageurl like '%UserHub.aspx%'And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.MobileIndexViewPVAll = GetCount<Model.WebAccessLogsInfo>(string.Format("pageurl like '%UserHub.aspx%' And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.MobileIndexViewPVThisMonth = GetCount<Model.WebAccessLogsInfo>(string.Format("DATEDIFF(MONTH,AccessDate,GETDATE()) = 0 and pageurl like '%UserHub.aspx%'And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.MobileIndexViewPVThisWeek = GetCount<Model.WebAccessLogsInfo>(string.Format("DATEDIFF(WEEK,AccessDate,GETDATE()) = 0 and pageurl like '%UserHub.aspx%'And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.MobileIndexViewPVToday = GetCount<Model.WebAccessLogsInfo>(string.Format("DATEDIFF(DAY,AccessDate,GETDATE()) = 0 and pageurl like '%UserHub.aspx%'And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.MobileIndexViewPVYesterday = GetCount<Model.WebAccessLogsInfo>(string.Format("DATEDIFF(DAY,AccessDate,DATEADD(day,-1, GETDATE())) = 0 and pageurl like '%UserHub.aspx%'And WebsiteOwner='{0}'", WebsiteOwner));
        //    #endregion


        //    #region 文章访问统计
        //    //根据网页访问日志表统计
        //    //result.ArticleViewIPAll = GetCount<Model.WebAccessLogsInfo>("IP", "pageurl like '%.chtml%'");
        //    //result.ArticleViewIPThisMonth = GetCount<Model.WebAccessLogsInfo>("IP", "DATEDIFF(MONTH,AccessDate,GETDATE()) = 0 and pageurl like '%.chtml%'");
        //    //result.ArticleViewIPThisWeek = GetCount<Model.WebAccessLogsInfo>("IP", "DATEDIFF(WEEK,AccessDate,GETDATE()) = 0 and pageurl like '%.chtml%'");
        //    //result.ArticleViewIPToday = GetCount<Model.WebAccessLogsInfo>("IP", "DATEDIFF(DAY,AccessDate,GETDATE()) = 0 and pageurl like '%.chtml%'");
        //    //result.ArticleViewIPYesterday = GetCount<Model.WebAccessLogsInfo>("IP", "DATEDIFF(DAY,AccessDate,DATEADD(day,-1, GETDATE())) = 0 and pageurl like '%.chtml%'");

        //    //result.ArticleViewUVAll = GetCount<Model.WebAccessLogsInfo>("userid", "pageurl like '%.chtml%'");
        //    //result.ArticleViewUVThisMonth = GetCount<Model.WebAccessLogsInfo>("userid", "DATEDIFF(MONTH,AccessDate,GETDATE()) = 0 and pageurl like '%.chtml%'");
        //    //result.ArticleViewUVThisWeek = GetCount<Model.WebAccessLogsInfo>("userid", "DATEDIFF(WEEK,AccessDate,GETDATE()) = 0 and pageurl like '%.chtml%'");
        //    //result.ArticleViewUVToday = GetCount<Model.WebAccessLogsInfo>("userid", "DATEDIFF(DAY,AccessDate,GETDATE()) = 0 and pageurl like '%.chtml%'");
        //    //result.ArticleViewUVYesterday = GetCount<Model.WebAccessLogsInfo>("userid", "DATEDIFF(DAY,AccessDate,DATEADD(day,-1, GETDATE())) = 0 and pageurl like '%.chtml%'");

        //    //result.ArticleViewPVAll = GetCount<Model.WebAccessLogsInfo>("pageurl like '%.chtml%'");
        //    //result.ArticleViewPVThisMonth = GetCount<Model.WebAccessLogsInfo>("DATEDIFF(MONTH,AccessDate,GETDATE()) = 0 and pageurl like '%.chtml%'");
        //    //result.ArticleViewPVThisWeek = GetCount<Model.WebAccessLogsInfo>("DATEDIFF(WEEK,AccessDate,GETDATE()) = 0 and pageurl like '%.chtml%'");
        //    //result.ArticleViewPVToday = GetCount<Model.WebAccessLogsInfo>("DATEDIFF(DAY,AccessDate,GETDATE()) = 0 and pageurl like '%.chtml%'");
        //    //result.ArticleViewPVYesterday = GetCount<Model.WebAccessLogsInfo>("DATEDIFF(DAY,AccessDate,DATEADD(day,-1, GETDATE())) = 0 and pageurl like '%.chtml%'");


        //    //根据监测表统计（两个模块发布时间不一样，监测表可能数据会比日志表多，多的部分出自于日志模块未发布时间）
        //    result.ArticleViewIPAll = GetCount<Model.MonitorEventDetailsInfo>("SourceIP", string.Format("WebsiteOwner='{0}'", WebsiteOwner));
        //    result.ArticleViewIPThisMonth = GetCount<Model.MonitorEventDetailsInfo>("SourceIP", string.Format("DATEDIFF(MONTH,EventDate,GETDATE()) = 0 And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.ArticleViewIPThisWeek = GetCount<Model.MonitorEventDetailsInfo>("SourceIP", string.Format("DATEDIFF(WEEK,EventDate,GETDATE()) = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewIPToday = GetCount<Model.MonitorEventDetailsInfo>("SourceIP", string.Format("DATEDIFF(DAY,EventDate,GETDATE()) = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewIPYesterday = GetCount<Model.MonitorEventDetailsInfo>("SourceIP", string.Format("DATEDIFF(DAY,EventDate,DATEADD(day,-1, GETDATE())) = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewUVAll = GetCount<Model.MonitorEventDetailsInfo>("EventUserID", string.Format("WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewUVThisMonth = GetCount<Model.MonitorEventDetailsInfo>("EventUserID", string.Format("DATEDIFF(MONTH,EventDate,GETDATE()) = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewUVThisWeek = GetCount<Model.MonitorEventDetailsInfo>("EventUserID", string.Format("DATEDIFF(WEEK,EventDate,GETDATE()) = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewUVToday = GetCount<Model.MonitorEventDetailsInfo>("EventUserID", string.Format("DATEDIFF(DAY,EventDate,GETDATE()) = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewUVYesterday = GetCount<Model.MonitorEventDetailsInfo>("EventUserID", string.Format("DATEDIFF(DAY,EventDate,DATEADD(day,-1, GETDATE())) = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewPVAll = GetCount<Model.MonitorEventDetailsInfo>(string.Format("WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewPVThisMonth = GetCount<Model.MonitorEventDetailsInfo>(string.Format("DATEDIFF(MONTH,EventDate,GETDATE()) = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewPVThisWeek = GetCount<Model.MonitorEventDetailsInfo>(string.Format("DATEDIFF(WEEK,EventDate,GETDATE()) = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewPVToday = GetCount<Model.MonitorEventDetailsInfo>(string.Format("DATEDIFF(DAY,EventDate,GETDATE()) = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticleViewPVYesterday = GetCount<Model.MonitorEventDetailsInfo>(string.Format("DATEDIFF(DAY,EventDate,DATEADD(day,-1, GETDATE())) = 0 And WebsiteOwner='{0}'", WebsiteOwner));



        //    #endregion

        //    #region 文章发布统计
        //    /*
        //     * 文章发布统计:
        //     * 文章类型 article
        //     * 
        //     */
        //    result.ArticlePubAll = GetCount<Model.JuActivityInfo>(string.Format("IsDelete = 0 and ArticleType = 'article'And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.ArticlePubThisMonth = GetCount<Model.JuActivityInfo>(string.Format("DATEDIFF(MONTH,CreateDate,GETDATE()) = 0 and ArticleType = 'article' and  IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ArticlePubThisWeek = GetCount<Model.JuActivityInfo>(string.Format("DATEDIFF(WEEK,CreateDate,GETDATE()) = 0 and ArticleType = 'article' and  IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.ArticlePubToday = GetCount<Model.JuActivityInfo>(string.Format("DATEDIFF(DAY,CreateDate,GETDATE()) = 0 and ArticleType = 'article' and  IsDelete = 0  And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.ArticlePubYesterday = GetCount<Model.JuActivityInfo>(string.Format("DATEDIFF(DAY,CreateDate,DATEADD(day,-1, GETDATE())) = 0 and ArticleType = 'article' and  IsDelete = 0  And WebsiteOwner='{0}'", WebsiteOwner));
        //    #endregion

        //    #region 活动发布统计
        //    result.ActivityPubAll = GetCount<Model.JuActivityInfo>(string.Format(" IsDelete = 0 and ArticleType = 'activity'And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.ActivityPubThisMonth = GetCount<Model.JuActivityInfo>(string.Format("DATEDIFF(MONTH,CreateDate,GETDATE()) = 0 and ArticleType = 'activity' and  IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner));

        //    result.ActivityPubThisWeek = GetCount<Model.JuActivityInfo>(string.Format("DATEDIFF(WEEK,CreateDate,GETDATE()) = 0 and ArticleType = 'activity' and  IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.ActivityPubToday = GetCount<Model.JuActivityInfo>(string.Format("DATEDIFF(DAY,CreateDate,GETDATE()) = 0 and ArticleType = 'activity' and  IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.ActivityPubYesterday = GetCount<Model.JuActivityInfo>(string.Format("DATEDIFF(DAY,CreateDate,DATEADD(day,-1, GETDATE())) = 0 and ArticleType = 'activity' and  IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner));
        //    #endregion

        //    #region 报名统计
        //    result.SignUpAll = GetCount<Model.ActivityDataInfo>(string.Format("IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.SignUpThisMonth = GetCount<Model.ActivityDataInfo>(string.Format("DATEDIFF(MONTH,InsertDate,GETDATE()) = 0  and IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.SignUpThisWeek = GetCount<Model.ActivityDataInfo>(string.Format("DATEDIFF(WEEK,InsertDate,GETDATE()) = 0 and IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.SignUpToday = GetCount<Model.ActivityDataInfo>(string.Format("DATEDIFF(DAY,InsertDate,GETDATE()) = 0 and  IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner));
        //    result.SignUpYesterday = GetCount<Model.ActivityDataInfo>(string.Format("DATEDIFF(DAY,InsertDate,DATEADD(day,-1, GETDATE())) = 0 and  IsDelete = 0 And WebsiteOwner='{0}'", WebsiteOwner)); ;
        //    #endregion

        //    #region 分享统计
        //    /*
        //     * 分享统计:
        //     * 
        //     * 
        //     */
        //    result.ShareAll = (int)ZCDALEngine.DbHelperSQL.GetSingle(GetShareAllSql());//GetCount<Model.MonitorEventDetailsInfo>("ShareTimestamp", "ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ");
        //    result.ShareThisMonth = (int)ZCDALEngine.DbHelperSQL.GetSingle(GetShareThisMonthSql());//GetCount<Model.MonitorEventDetailsInfo>("ShareTimestamp", "DATEDIFF(MONTH,EventDate,GETDATE()) = 0 and ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ");
        //    result.ShareThisWeek = (int)ZCDALEngine.DbHelperSQL.GetSingle(GetShareThisWeekSql()); //GetCount<Model.MonitorEventDetailsInfo>("ShareTimestamp", "DATEDIFF(WEEK,EventDate,GETDATE()) = 0 and ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ");
        //    result.ShareToday = (int)ZCDALEngine.DbHelperSQL.GetSingle(GetShareTodaySql());// GetCount<Model.MonitorEventDetailsInfo>("ShareTimestamp", "DATEDIFF(DAY,EventDate,GETDATE()) = 0 and ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ");
        //    result.ShareYesterday = (int)ZCDALEngine.DbHelperSQL.GetSingle(GetShareYesterdaySql());// GetCount<Model.MonitorEventDetailsInfo>("ShareTimestamp", "DATEDIFF(DAY,EventDate,DATEADD(day,-1, GETDATE())) = 0 and ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ");

        //    #endregion

        //    return result;
        //}

        /// <summary>
        /// 当前站点所有者
        /// </summary>
        public string WebsiteOwner
        {
            get
            {

                try
                {
                    if (System.Web.HttpContext.Current.Session["WebsiteOwner"] != null)
                        return (string)System.Web.HttpContext.Current.Session["WebsiteOwner"];
                }
                catch { }

                return null;
            }
        }

        public void SetWebSiteOwner(string websiteOwner)
        {
            HttpContext.Current.Session["WebsiteOwner"] = websiteOwner;
        }

        //public string GetShareAllSql()
        //{
        //    StringBuilder str = new StringBuilder();

        //    //select 
        //    //ShareTimestamp,MonitorPlanID,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00') as ShareTime,
        //    //COUNT(*) as c 
        //    //into #tmp 
        //    //from ZCJ_MonitorEventDetailsInfo
        //    //where ShareTimestamp is not null
        //    //group by ShareTimestamp,MonitorPlanID;
        //    //select COUNT(*) as count from #tmp;


        //    str.AppendLine(" select ");
        //    str.AppendLine(" ShareTimestamp,MonitorPlanID,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00') as ShareTime,");
        //    str.AppendLine(" COUNT(*) as c ");
        //    str.AppendLine(" into #tmp ");
        //    str.AppendLine(" from ZCJ_MonitorEventDetailsInfo");
        //    str.AppendLine(string.Format(" where ShareTimestamp is not null And WebsiteOwner='{0}'", WebsiteOwner));
        //    str.AppendLine(" group by ShareTimestamp,MonitorPlanID;");
        //    str.AppendLine(" select COUNT(*) as count from #tmp;");

        //    return str.ToString();
        //}

        //public string GetShareThisMonthSql()
        //{
        //    StringBuilder str = new StringBuilder();

        //    str.AppendLine(" select ");
        //    str.AppendLine(" ShareTimestamp,MonitorPlanID,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00') as ShareTime,");
        //    str.AppendLine(" COUNT(*) as c ");
        //    str.AppendLine(" into #tmp ");
        //    str.AppendLine(" from ZCJ_MonitorEventDetailsInfo");
        //    str.AppendLine(" where ShareTimestamp is not null");

        //    str.AppendLine(string.Format(" AND DATEDIFF(MONTH,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00'),GETDATE()) = 0  And WebsiteOwner='{0}'", WebsiteOwner));

        //    str.AppendLine(" group by ShareTimestamp,MonitorPlanID;");
        //    str.AppendLine(" select COUNT(*) as count from #tmp;");

        //    return str.ToString();
        //}

        //public string GetShareThisWeekSql()
        //{
        //    StringBuilder str = new StringBuilder();

        //    str.AppendLine(" select ");
        //    str.AppendLine(" ShareTimestamp,MonitorPlanID,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00') as ShareTime,");
        //    str.AppendLine(" COUNT(*) as c ");
        //    str.AppendLine(" into #tmp ");
        //    str.AppendLine(" from ZCJ_MonitorEventDetailsInfo");
        //    str.AppendLine(" where ShareTimestamp is not null");

        //    str.AppendLine(string.Format(" AND DATEDIFF(WEEK,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00'),GETDATE()) = 0  And WebsiteOwner='{0}'", WebsiteOwner));

        //    str.AppendLine(" group by ShareTimestamp,MonitorPlanID;");
        //    str.AppendLine(" select COUNT(*) as count from #tmp;");

        //    return str.ToString();
        //}

        //public string GetShareTodaySql()
        //{
        //    StringBuilder str = new StringBuilder();

        //    str.AppendLine(" select ");
        //    str.AppendLine(" ShareTimestamp,MonitorPlanID,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00') as ShareTime,");
        //    str.AppendLine(" COUNT(*) as c ");
        //    str.AppendLine(" into #tmp ");
        //    str.AppendLine(" from ZCJ_MonitorEventDetailsInfo");
        //    str.AppendLine(" where ShareTimestamp is not null");

        //    str.AppendLine(string.Format(" AND DATEDIFF(DAY,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00'),GETDATE()) = 0  And WebsiteOwner='{0}'", WebsiteOwner));

        //    str.AppendLine(" group by ShareTimestamp,MonitorPlanID;");
        //    str.AppendLine(" select COUNT(*) as count from #tmp;");

        //    return str.ToString();
        //}

        //public string GetShareSqlByDate(DateTime dt)
        //{
        //    StringBuilder str = new StringBuilder();

        //    str.AppendLine(" select ");
        //    str.AppendLine(" ShareTimestamp,MonitorPlanID,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00') as ShareTime,");
        //    str.AppendLine(" COUNT(*) as c ");
        //    str.AppendLine(" into #tmp ");
        //    str.AppendLine(" from ZCJ_MonitorEventDetailsInfo");
        //    str.AppendLine(" where ShareTimestamp is not null");

        //    str.AppendLine(string.Format(" AND DATEDIFF(DAY,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00'),GETDATE()) = 0   And WebsiteOwner='{0}'", WebsiteOwner));

        //    str.AppendLine(" group by ShareTimestamp,MonitorPlanID;");
        //    str.AppendLine(" select COUNT(*) as count from #tmp;");

        //    return str.ToString();
        //}

        //public string GetShareYesterdaySql()
        //{
        //    StringBuilder str = new StringBuilder();

        //    str.AppendLine(" select ");
        //    str.AppendLine(" ShareTimestamp,MonitorPlanID,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00') as ShareTime,");
        //    str.AppendLine(" COUNT(*) as c ");
        //    str.AppendLine(" into #tmp ");
        //    str.AppendLine(" from ZCJ_MonitorEventDetailsInfo");
        //    str.AppendLine(" where ShareTimestamp is not null");

        //    str.AppendLine(string.Format(" AND DATEDIFF(DAY,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00'),DATEADD(day,-1, GETDATE())) = 0  And WebsiteOwner='{0}'", WebsiteOwner));

        //    str.AppendLine(" group by ShareTimestamp,MonitorPlanID;");
        //    str.AppendLine(" select COUNT(*) as count from #tmp;");

        //    return str.ToString();
        //}

        ///// <summary>
        ///// 根据时间获取新增用户数量
        ///// </summary>
        ///// <param name="inputDate"></param>
        ///// <returns></returns>
        //public int GetNewUserCountByDate(DateTime? startDate, DateTime? endDate = null, bool isOneDay = true)
        //{

        //    int result = 0;
        //    StringBuilder strWhere = new StringBuilder(" WXNickname is not null and WXNickname <> '' ");

        //    if (isOneDay)
        //    {
        //        strWhere.AppendFormat(" AND DATEDIFF(DAY,Regtime,'{0}') = 0", startDate);
        //    }
        //    else
        //    {
        //        if (startDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,Regtime,'{0}') > 0", startDate);

        //        if (endDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,Regtime,'{0}') < 0", endDate);
        //    }

        //    strWhere.AppendFormat(" And WebsiteOwner='{0}'", WebsiteOwner);

        //    result = GetCount<Model.UserInfo>(strWhere.ToString());

        //    return result;
        //}

        ///// <summary>
        ///// 根据时间获取新发文章数量
        ///// </summary>
        ///// <param name="inputDate"></param>
        ///// <returns></returns>
        //public int GetNewArticleCountByDate(DateTime? startDate, DateTime? endDate = null, bool isOneDay = true)
        //{

        //    int result = 0;
        //    StringBuilder strWhere = new StringBuilder(" IsDelete = 0 and ArticleType = 'article' ");

        //    if (isOneDay)
        //    {
        //        strWhere.AppendFormat(" AND DATEDIFF(DAY,CreateDate,'{0}') = 0", startDate);
        //    }
        //    else
        //    {
        //        if (startDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,CreateDate,'{0}') > 0", startDate);

        //        if (endDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,CreateDate,'{0}') < 0", endDate);
        //    }

        //    strWhere.AppendFormat(" And WebsiteOwner='{0}'", WebsiteOwner);
        //    result = GetCount<Model.JuActivityInfo>(strWhere.ToString());

        //    return result;
        //}

        ///// <summary>
        ///// 根据时间获取新发活动数量
        ///// </summary>
        ///// <param name="inputDate"></param>
        ///// <returns></returns>
        //public int GetNewActivityCountByDate(DateTime? startDate, DateTime? endDate = null, bool isOneDay = true)
        //{

        //    int result = 0;
        //    StringBuilder strWhere = new StringBuilder(" IsDelete = 0 and ArticleType = 'activity' ");

        //    if (isOneDay)
        //    {
        //        strWhere.AppendFormat(" AND DATEDIFF(DAY,CreateDate,'{0}') = 0", startDate);
        //    }
        //    else
        //    {
        //        if (startDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,CreateDate,'{0}') > 0", startDate);

        //        if (endDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,CreateDate,'{0}') < 0", endDate);
        //    }
        //    strWhere.AppendFormat(" And WebsiteOwner='{0}'", WebsiteOwner);
        //    result = GetCount<Model.JuActivityInfo>(strWhere.ToString());

        //    return result;
        //}

        ///// <summary>
        ///// 根据时间获取分享数量
        ///// </summary>
        ///// <param name="startDate"></param>
        ///// <param name="endDate"></param>
        ///// <param name="isOneDay"></param>
        ///// <returns></returns>
        //public int GetShareCountByDate(DateTime? startDate, DateTime? endDate = null, bool isOneDay = true)
        //{
        //    int result = 0;

        //    StringBuilder strWhere = new StringBuilder();

        //    strWhere.AppendLine(" select ");
        //    strWhere.AppendLine(" ShareTimestamp,MonitorPlanID,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00') as ShareTime,");
        //    strWhere.AppendLine(" COUNT(*) as c ");
        //    strWhere.AppendLine(" into #tmp ");
        //    strWhere.AppendLine(" from ZCJ_MonitorEventDetailsInfo");
        //    strWhere.AppendLine(" where ShareTimestamp is not null");

        //    if (isOneDay)
        //    {
        //        strWhere.AppendFormat(" AND DATEDIFF(DAY,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00'),'{0}') = 0", startDate);
        //    }
        //    else
        //    {
        //        if (startDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00'),'{0}') > 0", startDate);
        //        if (endDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,DATEADD(s, CAST( ShareTimestamp as int), '1970-01-01 00:00:00'),'{0}') < 0", endDate);
        //    }
        //    strWhere.AppendFormat(" And WebsiteOwner='{0}'", WebsiteOwner);

        //    strWhere.AppendLine(" group by ShareTimestamp,MonitorPlanID;");
        //    strWhere.AppendLine(" select COUNT(*) as count from #tmp;");

        //    result = (int)ZCDALEngine.DbHelperSQL.GetSingle(strWhere.ToString());

        //    return result;
        //}


        ///// <summary>
        ///// 根据时间获取报名数量
        ///// </summary>
        ///// <param name="inputDate"></param>
        ///// <returns></returns>
        //public int GetSignUpCountByDate(DateTime? startDate, DateTime? endDate = null, bool isOneDay = true)
        //{
        //    int result = 0;
        //    StringBuilder strWhere = new StringBuilder(" IsDelete = 0 ");

        //    if (isOneDay)
        //    {
        //        strWhere.AppendFormat(" AND DATEDIFF(DAY,InsertDate,'{0}') = 0", startDate);
        //    }
        //    else
        //    {
        //        if (startDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,InsertDate,'{0}') > 0", startDate);

        //        if (endDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,InsertDate,'{0}') < 0", endDate);
        //    }
        //    strWhere.AppendFormat(" And WebsiteOwner='{0}'", WebsiteOwner);
        //    result = GetCount<Model.ActivityDataInfo>(strWhere.ToString());

        //    return result;
        //}


        ///// <summary>
        ///// 根据时间获取文章访问统计
        ///// </summary>
        ///// <param name="inputDate"></param>
        ///// <returns></returns>
        //public int GetArticleViewCountByDate(StatisticsType sType, DateTime? startDate, DateTime? endDate = null, bool isOneDay = true)
        //{
        //    int result = 0;
        //    StringBuilder strWhere = new StringBuilder(" 1=1 ");

        //    if (isOneDay)
        //    {
        //        strWhere.AppendFormat(" AND DATEDIFF(DAY,EventDate,'{0}') = 0", startDate);
        //    }
        //    else
        //    {
        //        if (startDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,EventDate,'{0}') > 0", startDate);

        //        if (endDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,EventDate,'{0}') < 0", endDate);
        //    }
        //    strWhere.AppendFormat(" And WebsiteOwner='{0}'", WebsiteOwner);

        //    switch (sType)
        //    {
        //        case StatisticsType.IP:
        //            result = GetCount<Model.MonitorEventDetailsInfo>("SourceIP", strWhere.ToString());
        //            break;
        //        case StatisticsType.PV:
        //            result = GetCount<Model.MonitorEventDetailsInfo>(strWhere.ToString());
        //            break;
        //        case StatisticsType.UV:
        //            result = GetCount<Model.MonitorEventDetailsInfo>("EventUserID", strWhere.ToString());
        //            break;
        //        default:
        //            break;
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// 根据时间获取PC主页访问统计
        ///// </summary>
        ///// <param name="inputDate"></param>
        ///// <returns></returns>
        //public int GetPCIndexViewCountByDate(StatisticsType sType, DateTime? startDate, DateTime? endDate = null, bool isOneDay = true)
        //{

        //    return GetPageViewCountByDate(sType, "%ActivityManage.aspx% ", startDate, endDate, isOneDay);
        //}

        ///// <summary>
        ///// 根据时间获取手机主页访问统计
        ///// </summary>
        ///// <param name="inputDate"></param>
        ///// <returns></returns>
        //public int GetMobileIndexViewCountByDate(StatisticsType sType, DateTime? startDate, DateTime? endDate = null, bool isOneDay = true)
        //{
        //    return GetPageViewCountByDate(sType, "%UserHub.aspx%", startDate, endDate, isOneDay);
        //}

        ///// <summary>
        ///// 根据时间获取指定页面访问统计
        ///// </summary>
        ///// <param name="inputDate"></param>
        ///// <returns></returns>
        //public int GetPageViewCountByDate(StatisticsType sType, string pageExpression, DateTime? startDate, DateTime? endDate = null, bool isOneDay = true)
        //{
        //    int result = 0;
        //    StringBuilder strWhere = new StringBuilder(" 1=1 ");

        //    if (!string.IsNullOrWhiteSpace(pageExpression))
        //        strWhere.AppendFormat(" and pageurl like '{0}' ", pageExpression);

        //    if (isOneDay)
        //    {
        //        strWhere.AppendFormat(" AND DATEDIFF(DAY,AccessDate,'{0}') = 0", startDate);
        //    }
        //    else
        //    {
        //        if (startDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,AccessDate,'{0}') > 0", startDate);

        //        if (endDate != null)
        //            strWhere.AppendFormat(" AND DATEDIFF(DAY,AccessDate,'{0}') < 0", endDate);
        //    }
        //    strWhere.AppendFormat(" And WebsiteOwner='{0}'", WebsiteOwner);
        //    switch (sType)
        //    {
        //        case StatisticsType.IP:
        //            result = GetCount<Model.WebAccessLogsInfo>("IP", strWhere.ToString());
        //            break;
        //        case StatisticsType.PV:
        //            result = GetCount<Model.WebAccessLogsInfo>(strWhere.ToString());
        //            break;
        //        case StatisticsType.UV:
        //            result = GetCount<Model.WebAccessLogsInfo>("userid", strWhere.ToString());
        //            break;
        //        default:
        //            break;
        //    }

        //    return result;
        //}



        ///// <summary>
        ///// 获取鸿风对应权限组ID
        ///// </summary>
        ///// <returns></returns>
        //public Dictionary<string, int> GetHFPmsGroupMatch()
        //{
        //    Dictionary<string, int> pmsGroupKeyValue = new Dictionary<string, int>();
        //    pmsGroupKeyValue.Add("管理员", 130273);
        //    pmsGroupKeyValue.Add("游客", 130334);
        //    pmsGroupKeyValue.Add("正式学员", 130335);
        //    pmsGroupKeyValue.Add("教师", 130388);

        //    return pmsGroupKeyValue;
        //}

        ///// <summary>
        ///// 获取鸿风对应积分
        ///// </summary>
        ///// <returns></returns>
        //public Dictionary<string, int> GetHFScoreMatch()
        //{
        //    Dictionary<string, int> scoreKeyValue = new Dictionary<string, int>();
        //    scoreKeyValue.Add("月度成果分享", 3);
        //    scoreKeyValue.Add("老板点评分享", 3);
        //    scoreKeyValue.Add("实用资源分享", 1);
        //    scoreKeyValue.Add("成功案例分享", 2);
        //    scoreKeyValue.Add("每周感悟分享", 2);
        //    scoreKeyValue.Add("问与答", 2);

        //    return scoreKeyValue;
        //}


        //public bool IsUserExist()
        //{

        //}
        /// <summary>
        /// 检查是否登录 已登录返回true 未登录返回false
        /// </summary>
        /// <returns></returns>
        public bool IsLogin
        {
            get
            {
                //if (HttpContext.Current.Session["userID"] != null)
                //{
                //    if (!string.IsNullOrEmpty(HttpContext.Current.Session["userID"].ToString()))
                //    {
                //        //if (HttpContext.Current.Session["login"] != null)
                //        //{
                //        // if (HttpContext.Current.Session["login"].ToString() == "1")
                //        // {
                //        return true;
                //        //}
                //        //}
                //    }


                //}
                
                return !string.IsNullOrWhiteSpace(GetCurrUserID());
            }
        }



        /// <summary>
        /// 获取当前登录用户名
        /// </summary>
        /// <returns></returns>
        public string GetCurrUserID()
        {
            try
            {
                return HttpContext.Current.Session["userID"].ToString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取当前登录用户的信息
        /// </summary>
        /// <returns></returns>
        public UserInfo GetCurrentUserInfo()
        {
            try
            {
                //string userId = GetCurrUserID();

                //if (string.IsNullOrWhiteSpace(userId))
                //{
                //    return null;
                //}

                //StringBuilder strWhere = new StringBuilder();
                //strWhere.AppendFormat(" UserID = '{0}' ", userId);

                //if (!string.IsNullOrWhiteSpace(WebsiteOwner) && userId != "jubit")
                //{
                //    strWhere.AppendFormat(" AND WebsiteOwner = '{0}' ", WebsiteOwner);
                //}

                //return Get<ZentCloud.BLLJIMP.Model.UserInfo>(strWhere.ToString());


                BLLUser bllUser = new BLLUser();

                return bllUser.GetUserInfoByCache(GetCurrUserID(), WebsiteOwner);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 检查是否是会员
        /// </summary>
        /// <returns></returns>
        public bool IsMember()
        {
            UserInfo nUser = GetCurrentUserInfo();
            if (nUser == null) return false;
            CompanyWebsite_Config nWebsiteConfig = Get<CompanyWebsite_Config>(string.Format("WebsiteOwner='{0}'", WebsiteOwner));
            if (nWebsiteConfig == null || nWebsiteConfig.MemberStandard == 0)
            {
                return nUser.AccessLevel > 0;
            }
            if (nWebsiteConfig.MemberStandard == 1)
            {
                return (!string.IsNullOrWhiteSpace(nUser.Phone) && nUser.IsPhoneVerify == 1 && nUser.AccessLevel > 0);
            }
            else if (nWebsiteConfig.MemberStandard == 2 || nWebsiteConfig.MemberStandard == 3)
            {
                List<TableFieldMapping> listFieldList = GetList<TableFieldMapping>(string.Format(" WebSiteOwner='{0}' AND TableName='{1}' AND IsDelete=0 ", WebsiteOwner, "ZCJ_UserInfo"));
                List<string> defFields = new List<string>() { "AutoID", "UserID", "Password", "UserType", "TrueName", "Phone" };
                #region 检查姓名
                TableFieldMapping trueNameField = listFieldList.FirstOrDefault(p => p.Field.Equals("TrueName"));
                if (trueNameField == null || trueNameField.FieldIsNull == 1)
                {
                    if (string.IsNullOrWhiteSpace(nUser.TrueName)) return false;
                }
                #endregion
                #region 检查手机
                TableFieldMapping phoneField = listFieldList.FirstOrDefault(p => p.Field.Equals("Phone"));
                if (phoneField == null || phoneField.FieldIsNull == 1)
                {
                    if (nUser.IsPhoneVerify != 1) return false;
                    if (string.IsNullOrWhiteSpace(nUser.Phone)) return false;
                }
                #endregion
                //#region 检查公司
                //TableFieldMapping companyField = listFieldList.FirstOrDefault(p => p.Field.Equals("Company"));
                //if (companyField == null || companyField.FieldIsNull == 1)
                //{
                //    if (string.IsNullOrWhiteSpace(nUser.Company)) return false;
                //}
                //#endregion
                //#region 检查职位
                //TableFieldMapping postionField = listFieldList.FirstOrDefault(p => p.Field.Equals("Postion"));
                //if (postionField == null || postionField.FieldIsNull == 1)
                //{
                //    if (string.IsNullOrWhiteSpace(nUser.Postion)) return false;
                //}
                //#endregion
                Newtonsoft.Json.Linq.JObject jtCurUser = Newtonsoft.Json.Linq.JObject.FromObject(nUser);
                List<Newtonsoft.Json.Linq.JProperty> listPropertys = jtCurUser.Properties().ToList();
                foreach (var item in listFieldList.Where(p => p.FieldIsNull == 1 && !defFields.Contains(p.Field)).OrderBy(p => p.Sort))
                {
                    if (!listPropertys.Exists(p => p.Name.Equals(item.Field))) continue;
                    if (string.IsNullOrWhiteSpace(jtCurUser[item.Field].ToString()))
                    {
                        return false;
                    }
                }
                if (nWebsiteConfig.MemberStandard == 3 && nUser.MemberApplyStatus != 9)
                {
                    return false;
                }
                return nUser.AccessLevel > 0;
            }
            return false;
        }


        /// <summary>
        /// 把请求参数转换成给定实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">实体对象</param>
        /// <returns></returns>
        public T ConvertRequestToModel<T>(object obj)
        {
            String[] allKeys = System.Web.HttpContext.Current.Request.HttpMethod.ToUpper().Equals("POST") ? System.Web.HttpContext.Current.Request.Form.AllKeys : System.Web.HttpContext.Current.Request.QueryString.AllKeys;
            Type type = obj.GetType();
            System.Reflection.PropertyInfo[] propertyInfoCollection = type.GetProperties();
            foreach (var key in allKeys)
            {
                string value = System.Web.HttpContext.Current.Request.HttpMethod.ToUpper().Equals("POST") ? System.Web.HttpContext.Current.Request.Form[key] : System.Web.HttpContext.Current.Request.QueryString[key];
                if (value == null) continue;
                var propInfo = type.GetProperty(key);
                if (propInfo == null) continue;
                string nFullName = propInfo.PropertyType.FullName;
                if (string.IsNullOrWhiteSpace(value) && nFullName.Contains("System.Nullable"))
                {
                    type.GetProperty(key).SetValue(obj, null, null);
                }
                else if (nFullName.Contains("System.Single"))
                {
                    type.GetProperty(key).SetValue(obj, Single.Parse(value), null);
                }
                else if (nFullName.Contains("System.UInt16"))
                {
                    type.GetProperty(key).SetValue(obj, UInt16.Parse(value), null);
                }
                else if (nFullName.Contains("System.UInt32"))
                {
                    type.GetProperty(key).SetValue(obj, UInt32.Parse(value), null);
                }
                else if (nFullName.Contains("System.UInt64"))
                {
                    type.GetProperty(key).SetValue(obj, UInt64.Parse(value), null);
                }
                else if (nFullName.Contains("System.Byte"))
                {
                    type.GetProperty(key).SetValue(obj, Byte.Parse(value), null);
                }
                else if (nFullName.Contains("System.SByte"))
                {
                    type.GetProperty(key).SetValue(obj, SByte.Parse(value), null);
                }
                else if (nFullName.Contains("System.Boolean"))
                {
                    type.GetProperty(key).SetValue(obj, Boolean.Parse(value), null);
                }
                else if (nFullName.Contains("System.Int16"))
                {
                    type.GetProperty(key).SetValue(obj, Int16.Parse(value), null);
                }
                else if (nFullName.Contains("System.Int32"))
                {
                    type.GetProperty(key).SetValue(obj, Int32.Parse(value), null);
                }
                else if (nFullName.Contains("System.Int64"))
                {
                    type.GetProperty(key).SetValue(obj, Int64.Parse(value), null);
                }
                else if (nFullName.Contains("System.Decimal"))
                {
                    type.GetProperty(key).SetValue(obj, Decimal.Parse(value), null);
                }
                else if (nFullName.Contains("System.Double"))
                {
                    type.GetProperty(key).SetValue(obj, Double.Parse(value), null);
                }
                else if (nFullName.Contains("System.Char"))
                {
                    type.GetProperty(key).SetValue(obj, Char.Parse(value), null);
                }
                else if (nFullName.Contains("System.DateTime"))
                {
                    type.GetProperty(key).SetValue(obj, DateTime.Parse(value), null);
                }
                else if (nFullName.Contains("System.String"))
                {
                    type.GetProperty(key).SetValue(obj, value, null);
                }
            }
            return (T)obj;

        }
        /// <summary>
        /// 把请求参数转换成给定实体
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="obj">实体对象</param>
        /// <returns></returns>
        public T ConvertToModel<T>(object obj, string key, string value)
        {
            Type type = obj.GetType();
            System.Reflection.PropertyInfo[] propertyInfoCollection = type.GetProperties();
            var propInfo = type.GetProperty(key);
            if (propInfo == null) return (T)obj;
            string nFullName = propInfo.PropertyType.FullName;
            if (nFullName.Contains("System.Single"))
            {
                type.GetProperty(key).SetValue(obj, Single.Parse(value), null);
            }
            else if (nFullName.Contains("System.UInt16"))
            {
                type.GetProperty(key).SetValue(obj, UInt16.Parse(value), null);
            }
            else if (nFullName.Contains("System.UInt32"))
            {
                type.GetProperty(key).SetValue(obj, UInt32.Parse(value), null);
            }
            else if (nFullName.Contains("System.UInt64"))
            {
                type.GetProperty(key).SetValue(obj, UInt64.Parse(value), null);
            }
            else if (nFullName.Contains("System.Byte"))
            {
                type.GetProperty(key).SetValue(obj, Byte.Parse(value), null);
            }
            else if (nFullName.Contains("System.SByte"))
            {
                type.GetProperty(key).SetValue(obj, SByte.Parse(value), null);
            }
            else if (nFullName.Contains("System.Boolean"))
            {
                type.GetProperty(key).SetValue(obj, Boolean.Parse(value), null);
            }
            else if (nFullName.Contains("System.Int16"))
            {
                type.GetProperty(key).SetValue(obj, Int16.Parse(value), null);
            }
            else if (nFullName.Contains("System.Int32"))
            {
                type.GetProperty(key).SetValue(obj, Int32.Parse(value), null);
            }
            else if (nFullName.Contains("System.Int64"))
            {
                type.GetProperty(key).SetValue(obj, Int64.Parse(value), null);
            }
            else if (nFullName.Contains("System.Decimal"))
            {
                type.GetProperty(key).SetValue(obj, Decimal.Parse(value), null);
            }
            else if (nFullName.Contains("System.Double"))
            {
                type.GetProperty(key).SetValue(obj, Double.Parse(value), null);
            }
            else if (nFullName.Contains("System.Char"))
            {
                type.GetProperty(key).SetValue(obj, Char.Parse(value), null);
            }
            else if (nFullName.Contains("System.DateTime"))
            {
                type.GetProperty(key).SetValue(obj, DateTime.Parse(value), null);
            }
            else if (nFullName.Contains("System.String"))
            {
                type.GetProperty(key).SetValue(obj, value, null);
            }
            return (T)obj;
        }
        /// <summary>
        /// 隐藏部分字符
        /// </summary>
        /// <param name="fristlen">显示前几个字符</param>
        /// <param name="endlen">显示后几个字符</param>
        /// <param name="word">原始字符</param>
        /// <returns></returns>
        public string HidePartialString(int fristlen, int endlen, string word)
        {
            try
            {
                string result = "";
                if (word.Length <= (fristlen + endlen))
                {
                    return word;
                }
                for (int i = 0; i < word.Length; i++)
                {
                    if ((i <= fristlen) || (i >= word.Length - endlen))
                    {
                        result += word[i];
                    }
                    else
                    {
                        result += "*";
                    }

                }

                return result;
            }
            catch (Exception)
            {
                return word;

            }


        }




        /// <summary>
        /// 检测是否移动设备访问
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool IsMobile
        {
            get
            {
                string[] mobileAgents = { "iphone", "android", "phone", "mobile", "wap", "netfront", "java", "opera mobi", "opera mini", "ucweb", "windows ce", "symbian", "series", "webos", "sony", "blackberry", "dopod", "nokia", "samsung", "palmsource", "xda", "pieplus", "meizu", "midp", "cldc", "motorola", "foma", "docomo", "up.browser", "up.link", "blazer", "helio", "hosin", "huawei", "novarra", "coolpad", "webos", "techfaith", "palmsource", "alcatel", "amoi", "ktouch", "nexian", "ericsson", "philips", "sagem", "wellcom", "bunjalloo", "maui", "smartphone", "iemobile", "spice", "bird", "zte-", "longcos", "pantech", "gionee", "portalmmm", "jig browser", "hiptop", "benq", "haier", "^lct", "320x320", "240x320", "176x220", "w3c ", "acs-", "alav", "alca", "amoi", "audi", "avan", "benq", "bird", "blac", "blaz", "brew", "cell", "cldc", "cmd-", "dang", "doco", "eric", "hipt", "inno", "ipaq", "java", "jigs", "kddi", "keji", "leno", "lg-c", "lg-d", "lg-g", "lge-", "maui", "maxo", "midp", "mits", "mmef", "mobi", "mot-", "moto", "mwbp", "nec-", "newt", "noki", "oper", "palm", "pana", "pant", "phil", "play", "port", "prox", "qwap", "sage", "sams", "sany", "sch-", "sec-", "send", "seri", "sgh-", "shar", "sie-", "siem", "smal", "smar", "sony", "sph-", "symb", "t-mo", "teli", "tim-", "tosh", "tsm-", "upg1", "upsi", "vk-v", "voda", "wap-", "wapa", "wapi", "wapp", "wapr", "webc", "winw", "winw", "xda", "xda-", "Googlebot-Mobile" };
                if (HttpContext.Current.Request.UserAgent.ToString().ToLower() != null)
                {
                    string userAgent = HttpContext.Current.Request.UserAgent.ToString().ToLower();
                    for (int i = 0; i < mobileAgents.Length; i++)
                    {
                        if (userAgent.IndexOf(mobileAgents[i]) >= 0)
                        {
                            return true;
                        }
                    }
                }
                return false;

            }
        }

        /// <summary>
        ///是否微信访问
        /// </summary>
        public bool IsWeiXinBrowser { get { return HttpContext.Current.Request.UserAgent.ToLower().Contains("micromessenger"); } }

        /// <summary>
        /// 获取请求参数键值对
        /// </summary>
        /// <returns>请求参数数组</returns>
        public Dictionary<string, string> GetRequestParameter()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            String[] allKeys = HttpContext.Current.Request.HttpMethod.ToUpper().Equals("POST") ? HttpContext.Current.Request.Form.AllKeys : HttpContext.Current.Request.QueryString.AllKeys;
            foreach (var key in allKeys)
            {
                dic.Add(key, HttpContext.Current.Request.HttpMethod.ToUpper().Equals("POST") ? HttpContext.Current.Request.Form[key] : HttpContext.Current.Request.QueryString[key]);
            }

            return dic;

        }
        /// <summary>
        /// 集合串联
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string Join(List<string> list)
        {
            string str = "";
            for (int i = 0; i < list.Count; i++)
            {
                str += string.Format("\"{0}\",", list[i]);
            }
            return str.TrimEnd(',');

        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public double GetTimeStamp(DateTime? dt)
        {
            if (dt == null)
            {
                return 0;
            }
            TimeSpan ts = ((DateTime)dt).ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return System.Math.Round(ts.TotalMilliseconds,0);
        }

        /// <summary>
        /// 基姆拉尔森计算公式计算日期
        /// </summary>
        /// <param name="dateTime">日期</param>
        public string CaculateWeekDay(DateTime dateTime)
        {

            int year = dateTime.Year;
            int month = dateTime.Month;
            int day = dateTime.Day;
            if (month == 1 || month == 2)
            {
                month += 12;
                year--;         //把一月和二月看成是上一年的十三月和十四月，例：如果是2004-1-10则换算成：2003-13-10来代入公式计算。
            }
            int week = (day + 2 * month + 3 * (month + 1) / 5 + year + year / 4 - year / 100 + year / 400) % 7;
            string weekstr = "";
            switch (week)
            {
                case 0: weekstr = "星期一"; break;
                case 1: weekstr = "星期二"; break;
                case 2: weekstr = "星期三"; break;
                case 3: weekstr = "星期四"; break;
                case 4: weekstr = "星期五"; break;
                case 5: weekstr = "星期六"; break;
                case 6: weekstr = "星期日"; break;
            }
            return weekstr;
        }

        /// <summary>
        /// 获取图片 url
        /// </summary>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public string GetImgUrl(string imgUrl)
        {

            if (string.IsNullOrEmpty(imgUrl))
            {
                return "";
            }
            if (imgUrl.StartsWith("http://"))
            {
                return imgUrl;
            }
            else
            {
                return string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Authority, imgUrl);
            }

        }

        /// <summary>
        /// 获取图标文件链接
        /// </summary>
        /// <returns></returns>
        public string GetIcoScript()
        {
            string iconfontPath = Common.ConfigHelper.GetConfigString("iconfont_comeoncloud");
            if (!string.IsNullOrWhiteSpace(iconfontPath))
            {
                string iconjson = File.ReadAllText(HttpContext.Current.Server.MapPath(iconfontPath));
                Newtonsoft.Json.Linq.JToken jo = Newtonsoft.Json.Linq.JToken.Parse(iconjson);
                if (jo["js_file"] != null) {
                    return string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", jo["js_file"].ToString());
                }
            }
            return "";
        }

        /// <summary>
        /// 获取图标文件链接
        /// </summary>
        /// <returns></returns>
        public string GetIcoFilePath()
        {
            string iconfontPath = Common.ConfigHelper.GetConfigString("iconfont_comeoncloud");
            if (!string.IsNullOrWhiteSpace(iconfontPath))
            {
                string iconjson = File.ReadAllText(HttpContext.Current.Server.MapPath(iconfontPath));
                Newtonsoft.Json.Linq.JToken jo = Newtonsoft.Json.Linq.JToken.Parse(iconjson);
                if (jo["css_file"] != null) return jo["css_file"].ToString();
            }
            return "";
        }

        /// <summary>
        /// 获取图标Svg文件链接
        /// </summary>
        /// <returns></returns>
        public string GetIcoSvgFilePath()
        {
            string iconfontPath = Common.ConfigHelper.GetConfigString("iconfont_comeoncloud");
            if (!string.IsNullOrWhiteSpace(iconfontPath))
            {
                string iconjson = File.ReadAllText(HttpContext.Current.Server.MapPath(iconfontPath));
                Newtonsoft.Json.Linq.JToken jo = Newtonsoft.Json.Linq.JToken.Parse(iconjson);
                if (jo["svg_file"] != null) return jo["svg_file"].ToString();
            }
            return "";
        }
        /// <summary>
        /// 获取图标样式数组字符串
        /// </summary>
        /// <returns></returns>
        public string GetIcoClassArray()
        {
            string iconfontPath = Common.ConfigHelper.GetConfigString("iconfont_comeoncloud");
            if (!string.IsNullOrWhiteSpace(iconfontPath))
            {
                string iconjson = File.ReadAllText(HttpContext.Current.Server.MapPath(iconfontPath));
                Newtonsoft.Json.Linq.JToken jo = Newtonsoft.Json.Linq.JToken.Parse(iconjson);
                if (jo["icon_class"] != null) return JsonConvert.SerializeObject(jo["icon_class"]);
            }
            return "[]";
        }


        /// <summary>
        /// 响应内容返回
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        public void ContextResponse(HttpContext context, dynamic result)
        {
            string respStr = JsonConvert.SerializeObject(result);

            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            context.Response.Clear();
            context.Response.ClearContent();

            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], respStr));
            }
            else
                context.Response.Write(respStr);
        }
        /// <summary>
        /// 检查表字段是否存在
        /// </summary>
        /// <param name="table"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool ExistsTableField(string table, string field)
        {
            MetaTable metaTable = DALEngine.GetMetas().Tables[table];
            return metaTable.Columns.Keys.FirstOrDefault(p => p.Equals(field)) != null;
            //StringBuilder sbSql = new StringBuilder();
            //sbSql.AppendFormat(" select T0.name from syscolumns T0 ");
            //sbSql.AppendFormat(" inner join sysobjects T1 on T0.id = T1.id and T1.xtype = 'U' and T1.name ='{0}' ",table.Replace(" ",""));
            //sbSql.AppendFormat(" where T0.name='{0}' ",field.Replace(" ",""));
            //object oName = GetSingle(sbSql.ToString(), null);
            //return oName != null;
        }

        public T GetCol<T>(string strWhere,string colName) where T : ModelTable, new()
        {
            List<T> list = GetColList<T>(1, 1, strWhere, colName);
            if (list.Count == 0) return null;
            return list[0];
        }

        public T GetByKey<T>(string keyName, string value, bool thisWebsiteOwner = false, string websiteOwner = "", BLLTransaction tran = null) where T : ModelTable, new()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("{0}='{1}'", keyName, value);
            if (thisWebsiteOwner)
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", WebsiteOwner);
            }
            else if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            }
            return Get<T>(sbWhere.ToString(), tran);
        }
        public T GetColByKey<T>(string keyName, string value, string colName, bool thisWebsiteOwner = false, string websiteOwner = "") where T : ModelTable, new()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("{0}='{1}'", keyName, value);
            if (thisWebsiteOwner)
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", WebsiteOwner);
            }
            else if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            }
            List<T> list = GetColList<T>(1, 1, sbWhere.ToString(), colName);
            if (list.Count == 0) return null;
            return list[0];
        }
        public List<T> GetListByKey<T>(string keyName, string value, string websiteOwner = "") where T : ModelTable, new()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("{0}='{1}'", keyName, value);
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            }
            return GetList<T>(sbWhere.ToString());
        }
        public List<T> GetListByKey<T>(int rows, int page, string keyName, string value, string websiteOwner = "") where T : ModelTable, new()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("{0}='{1}'", keyName, value);
            if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            }
            return GetLit<T>(rows, page, sbWhere.ToString());
        }

        public List<T> GetMultListByKey<T>(string keyName, string value, bool thisWebsiteOwner = false, string websiteOwner = "") where T : ModelTable, new()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("{0} In ({1})", keyName, value);
            if (thisWebsiteOwner)
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", WebsiteOwner);
            }
            else if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            }
            return GetList<T>(sbWhere.ToString());
        }
        public List<T> GetMultListByKey<T>(int rows, int page, string keyName, string value, bool thisWebsiteOwner = false, string websiteOwner = "") where T : ModelTable, new()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("{0} In ({1})", keyName, value);
            if (thisWebsiteOwner)
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", WebsiteOwner);
            }
            else if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            }
            return GetLit<T>(rows, page, sbWhere.ToString());
        }

        public List<T> GetColMultListByKey<T>(int rows, int page, string keyName, string value, string colName, 
            bool thisWebsiteOwner = false, string websiteOwner = "") where T : ModelTable, new()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("{0} In ({1})", keyName, value);
            if (thisWebsiteOwner)
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", WebsiteOwner);
            }
            else if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            }
            return GetColList<T>(rows, page, sbWhere.ToString(), colName);
        }

        public int GetCountByKey<T>(string keyName, string value, bool thisWebsiteOwner = false, string websiteOwner = "") where T : ModelTable, new()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("{0}='{1}'", keyName, value);
            if (thisWebsiteOwner) {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", WebsiteOwner);
            }else if(!string.IsNullOrWhiteSpace(websiteOwner)){
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            }
            return GetCount<T>(sbWhere.ToString());
        }

        public int DeleteByKey<T>(string keyName, string value, BLLTransaction tran = null) where T : ModelTable, new()
        {
            return Delete(new T(), string.Format("{0}='{1}'", keyName, value), tran);
        }

        public int DeleteMultByKey<T>(string keyName, string value, BLLTransaction tran = null) where T : ModelTable, new()
        {
            return Delete(new T(), string.Format("{0} In ({1})", keyName, value), tran);
        }

        public int UpdateByKey<T>(string keyName, string value, string toKeyName, string toValue, BLLTransaction tran = null, bool thisWebsiteOwner = false, string websiteOwner = "") where T : ModelTable, new()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("{0}='{1}'", keyName, value);
            if (thisWebsiteOwner)
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", WebsiteOwner);
            }
            else if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            }
            if (toValue != null) { 
                toValue = string.Format("'{0}'", toValue);
            }
            else
            {
                toValue = "Null";
            }
            return Update(new T(), string.Format("{0}={1}", toKeyName, toValue), sbWhere.ToString(), tran);
        }
        public int UpdateMultByKey<T>(string keyName, string value, string toKeyName, string toValue, BLLTransaction tran = null, bool thisWebsiteOwner = false, string websiteOwner = "") where T : ModelTable, new()
        {
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat("{0} In ({1})", keyName, value);
            if (thisWebsiteOwner)
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", WebsiteOwner);
            }
            else if (!string.IsNullOrWhiteSpace(websiteOwner))
            {
                sbWhere.AppendFormat(" AND WebsiteOwner='{0}'", websiteOwner);
            }
            if (toValue != null) { 
                toValue = string.Format("'{0}'", toValue); 
            }
            else{
                toValue = "Null";
            }
            return Update(new T(), string.Format("{0}={1}", toKeyName, toValue), sbWhere.ToString(), tran);
        }

        /// <summary>
        /// 合成图片 图片加logo（注：生成的都是本地文件，如果需要存到oss则在调用的地方去处理，有些地方是直接读取出图片流）
        /// </summary>
        /// <param name="mainImgPath">主图路径</param>
        /// <param name="icoPath">图标路径</param>
        /// <returns></returns>
        public string CompoundImageLogo(string mainImgPath, string icoPath = "", string websiteOwner = "")
        {
            try
            {
                BLLWebSite bllWebsite = new BLLWebSite();
                BLLJuActivity bllJuac = new BLLJuActivity();

                #region 用默认logo
                if (string.IsNullOrEmpty(icoPath))
                {
                    CompanyWebsite_Config comConfig = null;

                    if (string.IsNullOrWhiteSpace(websiteOwner))
                    {
                        comConfig = bllWebsite.GetCompanyWebsiteConfig();
                    }
                    else
                    {
                        comConfig = bllWebsite.GetCompanyWebsiteConfig(websiteOwner);
                    }

                    if (comConfig != null)
                    {
                        if (!string.IsNullOrEmpty(comConfig.DistributionQRCodeIcon))
                        {
                            if (comConfig.DistributionQRCodeIcon.Contains("http"))
                            {
                                icoPath = bllJuac.DownLoadRemoteImage(comConfig.DistributionQRCodeIcon);
                            }
                            else
                            {
                                icoPath = comConfig.DistributionQRCodeIcon;
                            }

                        }

                    }
                } 
                #endregion
                if (string.IsNullOrEmpty(icoPath))
                {
                    return mainImgPath;
                }
                if (mainImgPath.StartsWith("http"))
                {
                    mainImgPath = bllJuac.DownLoadRemoteImage(mainImgPath);
                }
                string imgOrgPath = System.Web.HttpContext.Current.Server.MapPath(mainImgPath);
                string imgOrgWater = System.Web.HttpContext.Current.Server.MapPath(icoPath);
                string imgVstr = "/FileUpload/WXMerge/" + Guid.NewGuid().ToString() + ".jpg";
                if (!Directory.Exists(imgVstr)) Directory.CreateDirectory(imgVstr);
                string imgVstrLocal = HttpContext.Current.Server.MapPath(imgVstr);
                ZentCloud.Common.ImgWatermarkHelper imgHelper = new ZentCloud.Common.ImgWatermarkHelper();
                imgHelper.SaveWatermark(imgOrgPath, imgOrgWater, 1f, ZentCloud.Common.ImgWatermarkHelper.WatermarkPosition.Center, 0, imgVstrLocal, 0.3f);//, 0.25f
                return imgVstr;

            }
            catch (Exception)
            {
                return mainImgPath;

            }




        }

        public string CompoundImageLogoToOss(string mainImgPath, string websiteOwner, string icoPath = "")
        {
            BLLJuActivity bllJuActivity = new BLLJuActivity();
            string result = string.Empty;

            //先处理成本地图片
            result = CompoundImageLogo(mainImgPath, icoPath, websiteOwner);

            result = HttpContext.Current.Server.MapPath(result);

            //存到oss
            result = bllJuActivity.DownLoadImageToOss(result, websiteOwner, true);

            return result;
        }
        
        public bool PlusNumericalCol<T>(string keyName,  string toKeyName, string toKeyValue, int value = 1) where T : ModelTable, new()
        {
            var result = Update(new T(), string.Format(" {0} = {0} + ({1}) ", keyName, value), string.Format(" {0} in ({1}) ", toKeyName, toKeyValue)) > 0;
            return result;
        }


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message"></param>
        public void ToLog(string message)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("D:\\log\\devlog.txt", true, Encoding.UTF8))
                {
                    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), message));
                }

            }
            catch { }
        }
        public void ToLog(string message,string path)
        {

            try
            {
                if (path == "D:\\getjsapiconfig.txt")
                {
                    return;
                }

                using (StreamWriter sw = new StreamWriter(path, true, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), message));
                }
            }
            catch { }

        }
        /// <summary>
        /// 枚举转成String
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string EnumToString(object o)
        {
            Type t = o.GetType();
            string s = o.ToString();
            EnumDescriptionAttribute[] os = (EnumDescriptionAttribute[])t.GetField(s).GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (os != null && os.Length == 1)
            {
                return os[0].Text;
            }
            return s;
        }
        private class EnumDescriptionAttribute : Attribute
        {
            private string _text = "";
            public string Text
            {
                get { return this._text; }
            }
            public EnumDescriptionAttribute(string text)
            {
                _text = text;
            }

        }
    }

    public class BLLStatic
    {
        public static BLL bll = new BLL();
    }

    //public enum StatisticsType
    //{
    //    IP,
    //    PV,
    //    UV
    //}

    //public enum StatisticsItem
    //{
    //    /// <summary>
    //    /// 时间
    //    /// </summary>
    //    Date = 0,
    //    /// <summary>
    //    /// 新增用户
    //    /// </summary>
    //    NewUser,
    //    /// <summary>
    //    /// 新发文章
    //    /// </summary>
    //    ArticlePub,
    //    /// <summary>
    //    /// 新发活动
    //    /// </summary>
    //    ActivityPub,
    //    /// <summary>
    //    /// 分享数
    //    /// </summary>
    //    Share,
    //    /// <summary>
    //    /// 报名人数
    //    /// </summary>
    //    SignUp,
    //    PCIndexViewIP,
    //    PCIndexViewUV,
    //    PCIndexViewPV,
    //    MobileIndexViewIP,
    //    MobileIndexViewUV,
    //    MobileIndexViewPV,
    //    ArticleViewIP,
    //    ArticleViewUV,
    //    ArticleViewPV
    //}

    public struct ReturnValue
    {
        public int Code;
        public string Msg;
    }



}
