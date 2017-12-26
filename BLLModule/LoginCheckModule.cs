using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLModule
{
    public class LoginCheckModule : IHttpModule
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        /// <summary>
        /// Redis
        /// </summary>
        BLLRedis bllRedis = new BLLRedis();
        string currentUrl = "";//当前绝对地址
        string currentPath = "";//当前虚拟路径 不包含参数
        string pageExtraName = "";//后缀名
        string appLoginUrl = "";

        public void Dispose()
        {

        }
        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }
        void context_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                HttpContext context = ((HttpApplication)sender).Context;
                currentUrl = context.Request.Url.ToString();//当前绝对地址
                currentPath = context.Request.Path.ToLower();//当前虚拟路径 不包含参数
                pageExtraName = System.IO.Path.GetExtension(currentPath);//后缀名

                List<string> pageExtraNameFilterList = new List<string>(){
                    ".aspx",
                    ".cn",
                    ".com",
                    ".net"
                };
                //只处理aspx页面
                if (!pageExtraNameFilterList.Contains(pageExtraName))
                    return;

                ToLog("检查是否已经登录");
                ToLog("LoginCheckModule end  IsLogin: " + BLLStatic.bll.IsLogin);
                //检查是否已经登录
                if (context.Session[Common.SessionKey.UserID] != null && context.Session[Common.SessionKey.LoginStatu] != null && context.Session[Common.SessionKey.LoginStatu].ToString().Equals("1")) return;
            ToLog("checkLogin");
                appLoginUrl = Common.ConfigHelper.GetConfigString("appLoginUrl").ToLower();
                if (currentPath == appLoginUrl) return;
                //检查是否能cookie登录
                if (!CheckCookieLogin(context)) return;

                //检查是否登录
                CheckNeedLogin(context);

                ToLog("LoginCheckModule end  IsLogin: " + BLLStatic.bll.IsLogin);
            }
            catch (Exception ex)
            {
                ToLog(ex.Message);
            }
        }
        /// <summary>
        /// 检查是否需要登陆
        /// </summary>
        /// <param name="context"></param>
        private void CheckNeedLogin(HttpContext context)
        {
           List<BLLPermission.Model.ModuleFilterInfo>  pathList = bllRedis.GetModuleFilterInfoList();
           pathList = pathList.Where(p => p.FilterType== "WXOAuth").ToList();
           if (pathList.Where(p =>
                   (currentPath.Equals(p.PagePath, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("all")) ||
                   (currentPath.StartsWith(p.PagePath, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("start")) ||
                   (currentPath.EndsWith(p.PagePath, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("end")) ||
                   (currentPath.ToLower().Contains(p.PagePath.ToLower()) && p.MatchType.Equals("contains"))
               ).Count() ==0)
           {
               return;//匹配
           }
            //if (pathList.Count == 0) return;

            if (! new BLLCommRelation().ExistRelation(ZentCloud.BLLJIMP.Enums.CommRelationType.WXAuthPageMustLogin, bllUser.WebsiteOwner, "")) return;
            string ngRoute = context.Request["ngroute"];//ng路由 
            if (!string.IsNullOrWhiteSpace(ngRoute)) currentUrl = currentUrl +"#"+ngRoute;
            ToLog("跳转到登录页");
            context.Response.Redirect(appLoginUrl + string.Format("?redirect=" + HttpUtility.UrlEncode(currentUrl)), true);
        }
        /// <summary>
        /// cookie登录成功返回false
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool CheckCookieLogin(HttpContext context)
        {
            string userId = bllUser.GetUserInfoByLoginCookie();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                context.Session[ZentCloud.Common.SessionKey.UserID] = userId;
                context.Session[ZentCloud.Common.SessionKey.LoginStatu] = 1; //设置登录状态
                return false;
            }
            return true;
        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="log"></param>
        private void ToLog(string log)
        {
            try
            {

                //return;

                //if (currentUrl.ToLower().IndexOf("stockplayer.comeoncloud.net") > -1)
                //{

                //    using (StreamWriter sw = new StreamWriter(@"D:\LoginCheckModule.txt", true, Encoding.GetEncoding("gb2312")))
                //    {
                //        sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
                //    }

                //    return;
                //}


                //using (StreamWriter sw = new StreamWriter(@"D:\WXOpenOAuthDevLog.txt", true, Encoding.GetEncoding("gb2312")))
                //{
                //    sw.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString(), log));
                //}


            }
            catch { }
        }

    }
}
