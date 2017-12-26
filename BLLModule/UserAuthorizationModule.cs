using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLModule
{
    public class UserAuthorizationModule : IHttpModule
    {
        /// <summary>
        /// 当前相对路径
        /// </summary>
        string CurrentPath = string.Empty;
        /// <summary>
        /// 当前用户账号
        /// </summary>
        string userID = string.Empty;
        /// <summary>
        /// 当前用户
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo CurrentUserInfo;
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 权限BLL
        /// </summary>
        BLLPermission.BLL bllPer = new BLLPermission.BLL();
        /// <summary>
        /// 权限BLL
        /// </summary>
        BLLPermission.BLLPermission bllPms = new BLLPermission.BLLPermission();
        /// <summary>
        /// Redis
        /// </summary>
        BLLJIMP.BLLRedis bllRedis = new BLLJIMP.BLLRedis();
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        void context_AcquireRequestState(object sender, EventArgs e)
        {
            // 获取应用程序
            HttpApplication application = (HttpApplication)sender;

            CurrentPath = application.Request.FilePath == null ? "" : application.Request.FilePath.ToLower();

            #region 判断文件是否检查
            string pageExtraName = System.IO.Path.GetExtension(CurrentPath);
            List<string> pageExtraNameFilterList = new List<string>()
                {
                    ".aspx",
                    ".ashx",
                    ".cn",
                    ".com",
                    ".net"
                };


            //只处理aspx、ashx页面
            if (!pageExtraNameFilterList.Contains(pageExtraName))
            {
                ToLog("后缀不检查:" + CurrentPath);
                return;
            }

            //前端页面排除掉
            if (CurrentPath.IndexOf("/wap/") > -1 || CurrentPath.IndexOf("/m/") > -1 || CurrentPath.IndexOf("/customize/") > -1
                || CurrentPath.IndexOf("/wxcallback.aspx") > -1 || CurrentPath.IndexOf("/ueditorcontroller.ashx") > -1)
            {
                ToLog("前端页面排除掉:" + CurrentPath);
                return;
            }

            #endregion

            #region 过滤页排除

            //不过滤列表
            List<string> noFilterList = new List<string>()
                {
                    //"/serv/api/admin/"
                };
            //不过滤列表中特殊的链接
            List<string> filterList = new List<string>()
                {
                    //"/serv/api/admin/user/islogin.ashx",
                    //"/serv/api/admin/mall/statistics/chart.ashx",
                    //"/serv/api/admin/mall/statistics/list.ashx",
                    //"/serv/api/admin/dashboard/get.ashx",
                    //"/serv/api/admin/log/selectactionlist.ashx",
                    //"/serv/api/admin/log/list.ashx",
                    //"/serv/api/admin/account/selectlist.ashx"
                };

            //获取过滤页面表数据
            if (!noFilterList.Exists(p => CurrentPath.StartsWith(p)) || filterList.Exists(p => CurrentPath.StartsWith(p)))
            {
                //List<BLLPermission.Model.ModuleFilterInfo> pathList = bllPer.GetList<BLLPermission.Model.ModuleFilterInfo>(string.Format("FilterType !='WXOAuth'"));
                List<BLLPermission.Model.ModuleFilterInfo> pathList = bllRedis.GetModuleFilterInfoList().Where(p => p.FilterType != "WXOAuth").ToList();
                if(pathList.Where(p =>
                        (CurrentPath.Equals(p.PagePath, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("all")) ||
                        (CurrentPath.StartsWith(p.PagePath, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("start")) ||
                        (CurrentPath.EndsWith(p.PagePath, StringComparison.OrdinalIgnoreCase) && p.MatchType.Equals("end")) ||
                        (CurrentPath.ToLower().Contains(p.PagePath.ToLower()) && p.MatchType.Equals("contains"))
                    ).Count() > 0)
                {
                    ToLog("过滤页面:" + CurrentPath);
                    return;//匹配
                }
            }
            #endregion

            #region 检查是否登录
            //检查是否登录
            if (application.Session == null || application.Session[Common.SessionKey.LoginStatu] == null || application.Session[Common.SessionKey.UserID] == null)
            {
                this.GotoLoginPage(application, pageExtraName); 
                return;
            }
            if (!application.Session[Common.SessionKey.LoginStatu].ToString().Equals("1"))
            {
                //未登录
                this.GotoLoginPage(application, pageExtraName);
                return;
            }
            userID = application.Session[Common.SessionKey.UserID].ToString();//获取登录ID
            CurrentUserInfo = bllUser.GetUserInfo(userID,bllUser.WebsiteOwner);
            //判断用户是否已被禁用
            if (CurrentUserInfo.IsDisable == 1)
            {
                GotoIsDisable(application, pageExtraName);
                return;
            }
            if (CurrentUserInfo.UserType == 1)
            {
                ToLog("超级管理员排除:" + CurrentPath);
                return;
            }
            #endregion

            #region 站点验证，判断用户是否属于当前站点，系统超级管理员除外
            if (HttpContext.Current.Session["WebsiteInfoModel"] != null && this.CurrentUserInfo.UserType != 1)
            {
                BLLJIMP.Model.WebsiteInfo webSiteModel = (BLLJIMP.Model.WebsiteInfo)HttpContext.Current.Session["WebsiteInfoModel"];

                if (webSiteModel.WebsiteExpirationDate.HasValue && webSiteModel.WebsiteExpirationDate.Value.AddDays(1).AddSeconds(-1) < DateTime.Now)
                {
                    this.GotoIsExpirePage(application, pageExtraName);
                    return;
                }

                if (this.CurrentUserInfo.WebsiteOwner == null)
                    this.CurrentUserInfo.WebsiteOwner = "";

                if (!webSiteModel.WebsiteOwner.Equals(this.CurrentUserInfo.WebsiteOwner, StringComparison.OrdinalIgnoreCase))
                {
                    //ToLog(string.Format("不属于当前用户,网站所有者{0}当前用户{1}路径:{2}", webSiteModel.WebsiteOwner,userModel.WebsiteOwner,currAbsolutePath));
                    this.GotoNoPmsPage(application, pageExtraName);
                    return;
                }

            }
            #endregion

            #region 页面权限验证

            if (string.IsNullOrWhiteSpace(userID))
            {
                this.GotoNoPmsPage(application, pageExtraName);
                return;
            }
            BLLPermission.BLLMenuPermission bllMenuPer = new BLLPermission.BLLMenuPermission(userID);
            //if (bllPms.IsActionPermissionV2(bllMenuPer.WebsiteOwner))
            //{
                string nAction = GetAction(application);//参数名不分大小写
                //检查用户是否有该页面权限V2
                if (CurrentUserInfo.PermissionGroupID.HasValue)
                {
                    ZentCloud.BLLPermission.Model.PermissionGroupInfo perGroupInfo = bllPms.Get<ZentCloud.BLLPermission.Model.PermissionGroupInfo>(string.Format(" GroupID={0}", CurrentUserInfo.PermissionGroupID));
                    if (perGroupInfo != null &&perGroupInfo.GroupType == 3)//管理员权限跟站点所有者一致
                    {
                        userID = bllMenuPer.WebsiteOwner;
                    }
                }
                if (!bllMenuPer.NewCheckUserAndPath(userID, bllMenuPer.WebsiteOwner, CurrentPath, nAction))
                {
                    this.GotoNoPmsPage(application, pageExtraName);
                    return;
                }
                ToLog("权限检查通过: userID:" + userID + ",WebsiteOwner:" + bllMenuPer.WebsiteOwner + "," + CurrentPath);
            //}
            //else
            //{
            //    //检查用户是否有该页面权限
            //    if (!bllMenuPer.CheckUserAndPath(userID, CurrentPath))
            //    {
            //        this.GotoNoPmsPage(application, pageExtraName);
            //        return;
            //    }
            //}
            #endregion


        }
        /// <summary>
        /// 获取action
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        private string GetAction(HttpApplication application)
        {
            if (string.IsNullOrWhiteSpace(application.Request["action"]))
                return application.Request["action"];
            if (string.IsNullOrWhiteSpace(application.Request["Action"]))
                return application.Request["Action"];
            return "";
        }

        /// <summary>
        /// 跳转到登录页面
        /// </summary>
        /// <param name="application"></param>
        private void GotoLoginPage(HttpApplication application, string pageExtraName)
        {
            if (pageExtraName == ".ashx")
            {
                dynamic result = new
                {
                    status = false,
                    code = (int)APIErrCode.UserIsNotLogin,
                    msg = "对不起，您没有登录！"
                };
                bllUser.ContextResponse(application.Context, result);
                application.Response.End();
            }
            string loginUrl = Common.ConfigHelper.GetConfigString("loginUrl").ToLower();
            string logoutUrl = Common.ConfigHelper.GetConfigString("logoutUrl").ToLower();
            //手机跳转登录页
            //BLLJIMP.BLL bll=new BLLJIMP.BLL();
            //if (bll.IsMobile)
            //{
            //    application.Response.Redirect(string.Format("/App/Cation/Wap/login.aspx?redirecturl={0}", application.Request.Url.PathAndQuery.ToString()));
            //}
            //手机跳转登录页
            //PC 跳转登录页
            if (CurrentPath != null && CurrentPath != loginUrl && CurrentPath != logoutUrl)
            {
                application.Response.Redirect(logoutUrl);
            }
            //PC 跳转登录页

        }

        /// <summary>
        /// 跳转到无权限提示页面
        /// </summary>
        /// <param name="application"></param>
        private void GotoNoPmsPage(HttpApplication application, string pageExtraName)
        {
            
            if (pageExtraName == ".ashx")
            {
                dynamic result = new
                {
                    status = false,
                    code = (int)APIErrCode.InadequatePermissions,
                    msg = "对不起，您没有权限访问本页面！",

                    IsSuccess = false,
                    Status = (int)APIErrCode.InadequatePermissions,
                    Msg = "对不起，您没有权限访问本页面！",

                    isSuccess=false,
                    errcode=(int)APIErrCode.InadequatePermissions,
                    errmsg="对不起，您没有权限访问本页面！",


                };
                //List<string> filePathList = new List<string>()
                //{
                //    "/handler/app/cationhandler.ashx"
                //};
                //if (filePathList.Contains(application.Request.FilePath.ToLower()))
                //{
                //    result = new
                //    {
                //        IsSuccess = false,
                //        Status = (int)APIErrCode.InadequatePermissions,
                //        Msg = "对不起，您没有权限访问本页面！"
                //    };
                //}


                bllUser.ContextResponse(application.Context, result);
                application.Response.End();
            }
            string noPmsUrl = Common.ConfigHelper.GetConfigString("noPmsUrl").ToLower();
            string loginUrl = Common.ConfigHelper.GetConfigString("loginUrl").ToLower();
            string logoutUrl = Common.ConfigHelper.GetConfigString("logoutUrl").ToLower();
            if (CurrentPath != loginUrl && CurrentPath != logoutUrl)
                application.Response.Redirect(noPmsUrl);

        }

        /// <summary>
        /// 跳转到用户禁用页面
        /// </summary>
        /// <param name="application"></param>
        private void GotoIsDisable(HttpApplication application, string pageExtraName)
        {
            if (pageExtraName == ".ashx")
            {
                dynamic result = new
                {
                    status = false,
                    code = (int)APIErrCode.InadequatePermissions,
                    msg = "对不起，该登录账户已被管理员禁用！"
                };
                bllUser.ContextResponse(application.Context, result);
                application.Response.End();
            }
            string noPmsUrl = "/Error/userisdisable.htm";
            string loginUrl = Common.ConfigHelper.GetConfigString("loginUrl").ToLower();
            string logoutUrl = Common.ConfigHelper.GetConfigString("logoutUrl").ToLower();
            if (CurrentPath != loginUrl && CurrentPath != logoutUrl)
                application.Response.Redirect(noPmsUrl);

        }
        /// <summary>
        /// 跳转到用户到期页面
        /// </summary>
        /// <param name="application"></param>
        private void GotoIsExpirePage(HttpApplication application, string pageExtraName)
        {
            if (pageExtraName == ".ashx")
            {
                dynamic result = new
                {
                    status = false,
                    code = (int)APIErrCode.InadequatePermissions,
                    msg = "对不起，版本已过期请续费！"
                };
                new ZentCloud.BLLJIMP.BLLUser("").ContextResponse(application.Context, result);
                application.Response.End();
            }
            string noPmsUrl = "/Error/expire.htm";
            string loginUrl = Common.ConfigHelper.GetConfigString("loginUrl").ToLower();
            string logoutUrl = Common.ConfigHelper.GetConfigString("logoutUrl").ToLower();
            if (CurrentPath != loginUrl && CurrentPath != logoutUrl)
                application.Response.Redirect(noPmsUrl);

        }

        //private string GetAction(HttpApplication application)
        //{
        //    string act1 = application.Request["Action"];
        //    string act2 = application.Request["action"];
        //    string act3 = application.Request["ACTION"];
        //    if (!string.IsNullOrWhiteSpace(act1))
        //        return act1;
        //    if (!string.IsNullOrWhiteSpace(act2))
        //        return act2;
        //    if (!string.IsNullOrWhiteSpace(act3))
        //        return act3;
        //    return "";
        //}
        private void ToLog(string msg)
        {
            try
            {
                //using (StreamWriter sw = new StreamWriter(@"D:\UserAuthorizationModule.txt", true, Encoding.GetEncoding("gb2312")))
                //{
                //    sw.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString(), msg));
                //}
            }
            catch { }
        }
    }
}
