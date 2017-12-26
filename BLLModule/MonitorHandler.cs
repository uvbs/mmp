using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.Common;
using System.IO;
using System.Web.SessionState;

namespace ZentCloud.BLLModule
{
    public class MonitorHandler : IHttpHandler, IReadOnlySessionState
    {

        string currentUrl = "";//当前绝对地址
        string appLoginUrl = "";
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //try
            //{
            BLLJIMP.BLLJuActivity bllJuactivity = new BLLJIMP.BLLJuActivity("");
            BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
            BLLJIMP.BLLMonitor bllMonitor = new BLLJIMP.BLLMonitor();
            BLLJIMP.BLLWebSite bllwebSite = new BLLJIMP.BLLWebSite();
            BLLJIMP.BLLShareMonitor bllShareMonitor = new BLLJIMP.BLLShareMonitor();

            ///推广用户信息
            UserInfo spreadUser = null;
                
            //分享用户
            UserInfo shareUser = null;

            CompanyWebsite_Config companyConfig=bllwebSite.GetCompanyWebsiteConfig();

            currentUrl = context.Request.Url.ToString();//当前绝对地址
            string filePath = context.Request.FilePath;//当前相对路径
            #region 微信推广(展示文章页面)
            if (filePath.Contains(".chtml"))
            {
                ToLog(context, " monitorhandler filePath:" + filePath);
                string[] parameters = filePath.Split('/');
                if (parameters.Length > 2)
                {
                    int activityId = Convert.ToInt32(parameters[1], 16);//ZCJ_JuActivityInfo 文章ID;
                    ToLog(context," monitorhandler 文章ID:" + activityId);
                    long memberID = 0;

                    if (parameters.Length > 3)
                    {
                        //memberID = parameters[2] == "XXX" ? 0 : Convert.ToInt32(parameters[2], 16);//ZCJ_WXMemberInfo 会员注册ID;
                        spreadUser = bllUser.GetUserInfoByAutoID(Convert.ToInt32(parameters[2], 16));
                    }


                    string currOpenerOpenID = parameters.Length > 5 ? parameters[3] : string.Empty;//当前打开者的OpenID
                         
                    string spreadUserID = context.Request["spreadU"] == null ? "" : context.Request["spreadU"].ToString();
                        
                    string shareTimestamp = context.Request["shareTimestamp"];

                    if (!string.IsNullOrWhiteSpace(spreadUserID))
                    {
                        spreadUserID = Common.Base64Change.DecodeBase64ByUTF8(spreadUserID);
                    }

                    string spreadUserAutoIDStr = context.Request["ua"] == null ? "" : context.Request["ua"].ToString();//推广ID，原始id*1000，16进制后进行base64，然后“=”变成“_”
                    int spreadUserAutoID = 0;
                    if (!string.IsNullOrWhiteSpace(spreadUserAutoIDStr))
                    {
                        spreadUserAutoID = bllJuactivity.TransmitIntDeCode(spreadUserAutoIDStr);//Convert.ToInt32(Common.Base64Change.DecodeBase64ByUTF8(spreadUserAutoIDStr.Replace("_", "=")), 16) / 1000;
                    }

                    string shareId = context.Request["comeonshareid"] == null ? "" : context.Request["comeonshareid"].ToString();

                    if (!string.IsNullOrWhiteSpace(shareId))
                    {
                        ToLog(context, " monitorhandler 执行shareId查找分享任务： " + shareId);
                        try
                        {
                            var shareInfo = bllShareMonitor.GetShareInfo(shareId);

                            if (!string.IsNullOrWhiteSpace(shareInfo.UserId))
                            {
                                shareUser = bllUser.GetUserInfo(shareInfo.UserId);
                            }
                        }
                        catch (Exception ex)
                        {
                            ToLog(context, " monitorhandler 获取shareUser异常： " + ex.Message);
                        }
                       
                    }


                    
                    JuActivityInfo activityInfo = bllJuactivity.Get<JuActivityInfo>(string.Format("JuActivityID={0} AND IsDelete=0 ", activityId));//文章信息

                    if (activityInfo == null)
                    {
                        context.Response.WriteFile("/Error/NotExist.html");
                        return;
                    }

                    ToLog(context, " monitorhandler 找到文章，开始构造内容： " + activityInfo.JuActivityID);

                    //if (activityInfo.ArticleType == "activity")
                    //{
                    //    if (activityInfo.ActivityStatus == 1)
                    //    {
                    //        context.Response.Redirect("/Error/CommonMsg.aspx?msg=报名已结束,有疑问请联系我们&&icon=icon iconfont icon-kulian kulian");
                    //        return;
                    //    }
                    //}

                    #region 检查是否付费活动

                    if (activityInfo.IsFee == 1 && !bllJuactivity.IsLogin )
                    {
                        context.Response.Redirect("/App/Cation/Wap/FreeActivityPage.aspx?aid="+activityInfo.JuActivityID);
                        return;
                    }
                    #endregion

                    #region 检查访问级别
                    if (activityInfo.AccessLevel > 0)
                    {
                        if (!bllUser.IsLogin)
                        {
                            appLoginUrl = Common.ConfigHelper.GetConfigString("appLoginUrl").ToLower();
                            context.Response.Redirect(appLoginUrl + string.Format("?redirect=" + HttpUtility.UrlEncode(currentUrl)), true);
                            return;
                        }
                        //else if (!bllUser.IsMember() && activityInfo.AccessLevel == 1)
                        //{

                        //}
                        else if (bllUser.GetCurrentUserInfo().AccessLevel < activityInfo.AccessLevel)
                        {
                            //if (companyConfig.NoPermissionsPage == 0)
                            //{
                            //    context.Response.WriteFile("/Error/NoPmsMobile.htm");
                            //    return;
                            //}
                            //else if (companyConfig.NoPermissionsPage == 1)
                            //{
                            //    context.Response.Redirect("/App/Cation/Wap/UserEdit.aspx",true);
                            //    context.Response.End();
                            //    return;
                            //}
                            context.Response.WriteFile("/Error/NoPmsMobile.htm");
                            return;
                        }
                    }
                    #endregion

                    #region 检查访问级别

                    //WXMemberInfo regInfo = juactivityBll.Get<WXMemberInfo>(string.Format("MemberID={0}", memberID));//会员注册信息
                    // UserInfo userInfo = juactivityBll.Get<UserInfo>(string.Format("UserID='{0}'", activityInfo.UserID));//文章发布者信息
                    SystemSet systemset = bllJuactivity.Get<SystemSet>("");//系统配置信息
                    string pageSource = ""; //待输出的html源代码

                    if (systemset != null)
                    {
                        if (string.IsNullOrWhiteSpace(currOpenerOpenID))
                        {
                            //取得Session里的当前OpenID
                            currOpenerOpenID = context.Session[systemset.WXCurrOpenerOpenIDKey] != null ? context.Session[systemset.WXCurrOpenerOpenIDKey].ToString() : "";
                        }

                        if (string.IsNullOrWhiteSpace(currOpenerOpenID))
                        {
                            //如果再为空，由链接上面get参数获取
                            currOpenerOpenID = context.Request[systemset.WXCurrOpenerOpenIDKey] != null ? context.Request[systemset.WXCurrOpenerOpenIDKey].ToString() : "";
                        }
                    }
                    #endregion





                    //if (regInfo == null)
                    //{
                    //    regInfo = new WXMemberInfo() { Name = "none", WeixinOpenID = "" };
                    //}

                    if ((activityInfo != null))//
                    {
                        //var planInfo = bllJuactivity.Get<MonitorPlan>(string.Format("MonitorPlanID={0}", activityInfo.MonitorPlanID));
                        //if (planInfo == null)
                        //{
                        //    return;
                        //}
                        //else
                        //{
                        //    if (planInfo.PlanStatus == "0")//任务已停止
                        //    {
                        //        return;
                        //    }

                        //}

                        ToLog(context, " monitorhandler 开始执行GetJuactivityHtml： " + activityInfo.JuActivityID);
                        pageSource = bllJuactivity.GetJuactivityHtml(activityInfo, currOpenerOpenID, context.Request.Url.ToString(), spreadUser, shareUser);
                        ToLog(context, " monitorhandler 执行GetJuactivityHtml完毕： " + activityInfo.JuActivityID);

                        #region 事件记录
                        //事件
                        //int OpenCount = 0;//打开人数
                        //int DistinctOpenCount = 0;//独立IP数量
                        MonitorEventDetailsInfo detailInfo = new MonitorEventDetailsInfo();
                        detailInfo.MonitorPlanID = activityInfo.MonitorPlanID;
                        detailInfo.EventType = 0;
                        detailInfo.EventBrowser = HttpContext.Current.Request.Browser == null ? "" : HttpContext.Current.Request.Browser.ToString();
                        detailInfo.EventBrowserID = HttpContext.Current.Request.Browser.Id; ;
                        if (HttpContext.Current.Request.Browser.Beta)
                        {
                            detailInfo.EventBrowserIsBata = "测试版";
                        }
                        else
                        {
                            detailInfo.EventBrowserIsBata = "正式版";
                        }

                        detailInfo.EventBrowserVersion = HttpContext.Current.Request.Browser.Version;
                        detailInfo.EventDate = DateTime.Now;
                        if (HttpContext.Current.Request.Browser.Win16)
                            detailInfo.EventSysByte = "16位系统";
                        else
                            if (HttpContext.Current.Request.Browser.Win32)
                                detailInfo.EventSysByte = "32位系统";
                            else
                                detailInfo.EventSysByte = "64位系统";

                        detailInfo.EventSysPlatform = HttpContext.Current.Request.Browser.Platform;
                        detailInfo.SourceIP = Common.MySpider.GetClientIP();
                        detailInfo.IPLocation = Common.MySpider.GetIPLocation(detailInfo.SourceIP);
                        detailInfo.SourceUrl = HttpContext.Current.Request.Url.ToString();
                        detailInfo.RequesSourcetUrl = HttpContext.Current.Request.UrlReferrer != null ? HttpContext.Current.Request.UrlReferrer.ToString() :"";
                        if (spreadUser != null)
                        {
                            detailInfo.SpreadUserID = spreadUser.UserID;
                        }
                        detailInfo.SpreadUserAutoID = spreadUserAutoID;
                        detailInfo.ShareTimestamp = shareTimestamp;
                        detailInfo.WebsiteOwner = bllJuactivity.WebsiteOwner;
                        detailInfo.ModuleType = activityInfo.ArticleType;
                        if (bllUser.IsLogin)
                        {
                            detailInfo.EventUserID = bllUser.GetCurrUserID();
                        }

                            
                        if (spreadUser != null)//带推广信息
                        {
                            //try
                            //{
                            //    if (!string.IsNullOrWhiteSpace(currOpenerOpenID))
                            //    {
                            //        //记录触发人
                            //        UserInfo eventUser = new BLLJIMP.BLLUser("").GetUserInfoByOpenId(currOpenerOpenID);
                            //        detailInfo.EventUserID = eventUser.UserID;
                            //    }
                            //}
                            //catch { }
                            string url = string.Format("http://{0}{1}", context.Request.Url.Host, filePath);
                            MonitorLinkInfo linkInfo;
                            try
                            {
                                linkInfo= bllJuactivity.Get<MonitorLinkInfo>(string.Format(" LinkName='{0}' And MonitorPlanID={1}", spreadUser.UserID, activityInfo.MonitorPlanID));
                            }
                            catch (Exception ex)
                            {
                                context.Response.Write("ex"+ex.ToString());
                                return;
                            }
                               


                            if (linkInfo != null)
                            {
                                linkInfo.ActivityName = activityInfo.ActivityName;
                                linkInfo.ThumbnailsPath = activityInfo.ThumbnailsPath;
                                //已经为该用户建立推广链接
                                detailInfo.LinkID = linkInfo.LinkID;
                                //if (linkInfo.OpenCount != null)//增加打开人数
                                //{
                                linkInfo.OpenCount++;
                                //}
                                //else
                                //{
                                //    linkInfo.OpenCount = 1;
                                //}
                                int shareCount = bllJuactivity.GetCount<MonitorEventDetailsInfo>("ShareTimestamp", string.Format(" LinkID ={0} and ShareTimestamp is not null and ShareTimestamp <> '' and ShareTimestamp <> '0' ", linkInfo.LinkID));
                                linkInfo.ShareCount = shareCount;
                                int ipCount = bllJuactivity.GetCount<MonitorEventDetailsInfo>(" SourceIP ", string.Format(" LinkID = {0} ", linkInfo.LinkID));
                                linkInfo.DistinctOpenCount = ipCount;
                                bllJuactivity.Update(linkInfo, string.Format(" OpenCount={0},DistinctOpenCount={1},ShareCount={2}", linkInfo.OpenCount, ipCount, shareCount), string.Format("LinkID={0}", linkInfo.LinkID));

                            }
                            else
                            {

                                //还没有为该用户建立推广链接
                                MonitorLinkInfo newLinkinfo = new MonitorLinkInfo();
                                newLinkinfo.LinkID = int.Parse(bllJuactivity.GetGUID(ZentCloud.BLLJIMP.TransacType.MonitorLinkID));
                                newLinkinfo.MonitorPlanID = activityInfo.MonitorPlanID;
                                newLinkinfo.WXMemberID = memberID;
                                newLinkinfo.LinkName = spreadUser.UserID;
                                newLinkinfo.RealLink = url;
                                newLinkinfo.InsertDate = DateTime.Now;
                                newLinkinfo.OpenCount = 1;
                                newLinkinfo.ActivityName = activityInfo.ActivityName;
                                newLinkinfo.ThumbnailsPath = activityInfo.ThumbnailsPath;
                                newLinkinfo.WebsiteOwner = bllJuactivity.WebsiteOwner;
                                newLinkinfo.DistinctOpenCount = 1;// ip
                                newLinkinfo.ShareCount = 0;//分享数
                                if (activityInfo.ArticleType == "article")
                                {
                                    newLinkinfo.ForwardType = "fans";
                                    newLinkinfo.ActivityId = activityInfo.JuActivityID;
                                }
                                if (!string.IsNullOrEmpty(activityInfo.SignUpActivityID)&&activityInfo.ArticleType=="activity")
                                {
                                    newLinkinfo.ActivityId = int.Parse(activityInfo.SignUpActivityID);
                                }
                                   
                                StringBuilder sqlWhere=new StringBuilder();
                                sqlWhere.AppendFormat(@"
                                if not exists(select 1 from ZCJ_MonitorLinkInfo where LinkName='{14}' and MonitorPlanID={15} ) 
                                begin
                                    insert into ZCJ_MonitorLinkInfo (LinkID,MonitorPlanID,WXMemberID,LinkName,RealLink,InsertDate,OpenCount,ActivityName,ThumbnailsPath,WebsiteOwner,DistinctOpenCount,ShareCount,ForwardType,ActivityId)
                                                values({0},{1},{2},'{3}','{4}','{5}',{6},'{7}','{8}','{9}',{10},{11},'{12}',{13})
                                end 
                                ", newLinkinfo.LinkID, newLinkinfo.MonitorPlanID, newLinkinfo.WXMemberID, newLinkinfo.LinkName, newLinkinfo.RealLink, newLinkinfo.InsertDate, newLinkinfo.OpenCount, newLinkinfo.ActivityName, newLinkinfo.ThumbnailsPath, newLinkinfo.WebsiteOwner, newLinkinfo.DistinctOpenCount, newLinkinfo.ShareCount, newLinkinfo.ForwardType, newLinkinfo.ActivityId,spreadUser.UserID,activityInfo.MonitorPlanID);

                                    
                                if (ZentCloud.ZCDALEngine.DALEngine.ExecuteSql(sqlWhere.ToString()) > 0)
                                {
                                    detailInfo.LinkID = newLinkinfo.LinkID;
                                }


                            }
                        }


                        //添加事件详细
                        //if (!filePath.Contains("?"))
                        //{
                            bllJuactivity.Add(detailInfo);
                            //DistinctOpenCount = juactivityBll.GetCount<ZentCloud.BLLJIMP.Model.MonitorEventDetailsInfo>("SourceIP", string.Format("LinkID={0} and EventType=0", detailInfo.LinkID));
                            //juactivityBll.Update(new MonitorLinkInfo(), string.Format(" OpenCount={0},DistinctOpenCount={1}", OpenCount, DistinctOpenCount), string.Format("LinkID={0}", detailInfo.LinkID));
                            #region 微转发活动加积分
                            if (bllUser.IsLogin && spreadUser != null)
                            {
                                if (activityInfo.ArticleType=="activity")
                                {
                                    if (bllJuactivity.GetCount<MonitorEventDetailsInfo>(string.Format(" MonitorPlanID='{0}' And EventUserID='{1}' And SpreadUserID='{2}' And EventUserID!='{2}'", detailInfo.MonitorPlanID, bllUser.GetCurrUserID(), spreadUser.UserID)) == 1)
                                    {
                                            
                                        string remark = string.Format("转发活动《{0}》",activityInfo.ActivityName);
                                        //微转发加积分
                                        bllUser.AddUserScoreDetail(spreadUser.UserID, CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.ForwardArticle), spreadUser.WebsiteOwner, null, remark);


                                    }
                                }

                            } 
                            #endregion


                        //}
                        context.Response.ClearContent();
                        //处理完成
                        context.Response.Write(pageSource);
                        //更新微信阅读人数
                        //bllJuactivity.UpdateUVCount(activityInfo.JuActivityID);
                        bllJuactivity.UpDateIPPVShareCount(activityInfo);
                        bllJuactivity.UpdateActivityForwardPVUV(activityInfo);
                        if (spreadUser != null)
                        {
                            bllMonitor.UpdateUV(activityInfo.MonitorPlanID, spreadUser.UserID);
                            //bllMonitor.UpdateSignUpCount(activityInfo.MonitorPlanID, spreadUser.UserID);
                        }
                        if (!string.IsNullOrWhiteSpace(activityInfo.RedirectUrl))
                        {
                            context.Response.Redirect(activityInfo.RedirectUrl);
                        }
                        return;
                        #endregion

                        ToLog(context, " monitorhandler 事件记录完毕： " + activityInfo.JuActivityID);
                    }
                    else
                    {
                        context.Response.Write("<html><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1, maximum-scale=1\" /></head><body>链接无效。</body></html>");
                        return;
                    }

                }
            }
            #endregion
            #region 注释
            //#region 微信会员注册
            //else if (filePath.StartsWith("/weixin"))
            //{

            //    //微信会员注册
            //    if (filePath.Contains("wx_reg.chtml") && (!filePath.Contains("/weixin/wx_reg.chtml")))
            //    {
            //        string[] parameters = filePath.Split('/');

            //        string weixinMemberId = Convert.ToInt32(parameters[2], 16).ToString();// ZCJ_WeixinMemberInfo WeixinMemberID
            //        var weixinmemberinfo = juactivityBll.Get<WeixinMemberInfo>(string.Format("WeixinMemberID={0}", weixinMemberId));
            //        string RegCode = Common.IOHelper.GetFileStr(context.Server.MapPath("/weixin/wx_reg.htm"), Encoding.UTF8);//注册代码

            //        if (RegCode.Contains("$CCWXOPENID$"))
            //        {
            //            RegCode = RegCode.Replace("$CCWXOPENID$", weixinmemberinfo.WeixinOpenID);
            //        }
            //        if (RegCode.Contains("$CCWXAID$"))//注册到哪个账户下
            //        {

            //            RegCode = RegCode.Replace("$CCWXAID$", Convert.ToString(juactivityBll.Get<UserInfo>(string.Format("UserID='{0}'", weixinmemberinfo.UserID)).AutoID, 16));

            //        }

            //        context.Response.Write(RegCode);//输出注册代码


            //        //微信会员注册

            //    }

            //}

            //#endregion
            #endregion

            //}
            //catch (Exception ex)
            //{
            //    using (StreamWriter sw = new StreamWriter(@"C:\MonitorHandlerException.txt", true, Encoding.UTF8))
            //    {
            //        sw.WriteLine(string.Format("{0} MonitorHandler拦截处理异常：{1}", DateTime.Now.ToString(), ex.ToString()));

            //    }
            //    context.Response.Write("exception");
            //}

        }
        private void ToLog(HttpContext context,string msg)
        {
            //if (context.Request.Url.Host == "geonol.comeoncloud.net")
            //{
            //    BLLJIMP.BLLStatic.bll.ToLog(msg);
            //}
          
            //using (StreamWriter sw = new StreamWriter(@"D:\log.txt", true, Encoding.UTF8))
            //{
            //    sw.WriteLine(msg);

            //}

        }


    }
}
