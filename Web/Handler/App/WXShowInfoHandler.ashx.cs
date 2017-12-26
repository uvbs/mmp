using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Reflection;
using System.Text;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using System.Text.RegularExpressions;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// 微秀
    /// </summary>
    public class WXShowInfoHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 基本响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse(); // 统一回复相应数据
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();  //活动数据
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();  //用户数据
        /// <summary>
        /// BLL基类
        /// </summary>
        BLL bll = new BLL();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo; 
         public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                this.currentUserInfo = bll.GetCurrentUserInfo();
                string action = context.Request["Action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "请联系管理员";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = Common.JSONHelper.ObjectToJson(resp);
            }
            context.Response.Write(result);
        }

        /// <summary>
        /// 保存活动配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SavaActivityConfig(HttpContext context)
        {

            string theOrganizers = context.Request["TheOrganizers"];
            string activities = context.Request["Activities"];
            string myRegistration = context.Request["MyRegistration"];
            string registerCode = context.Request["RegisterCode"];

            string organizerName = context.Request["OrganizerName"];
            string activitiesName = context.Request["ActivitiesName"];
            string myRegistrationName = context.Request["MyRegistrationName"];
            int qrCodeType = int.Parse(context.Request["QCodeType"]);
            int colorTheme = int.Parse(context.Request["ColorTheme"]);
            int isShowHideActivity = int.Parse(context.Request["IsShowHideActivity"]);
            string showName=context.Request["ShowName"];
            string ticketShowName = context.Request["TicketShowName"];
            string toolBarGroups = context.Request["ToolBarGroups"];

            BLLJIMP.Model.ActivityConfig config = bllJuactivity.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));

            if (config != null)
            {
                config.Activities = activities;
                config.MyRegistration = myRegistration;
                config.TheOrganizers = theOrganizers;
                config.WebsiteOwner = bll.WebsiteOwner;
                config.RegisterCode = registerCode;
                config.IsShowHideActivity = isShowHideActivity;
                config.OrganizerName = organizerName;
                config.ActivitiesName = activitiesName;
                config.MyRegistrationName = myRegistrationName;
                config.QCodeType = qrCodeType;
                config.ActivityStyle = 0;
                config.ColorTheme = colorTheme;
                config.ShowName = showName;
                config.TicketShowName = ticketShowName;
                config.ToolBarGroups = toolBarGroups;
                bool isSuccess = bllJuactivity.Update(config);
                if (isSuccess)
                {

                    resp.Msg = "修改成功";
                    resp.Status = 1;
                }
                else
                {
                    resp.Msg = "修改失败";
                    resp.Status = -1;

                }
            }
            else
            {
                config = new BLLJIMP.Model.ActivityConfig()
                {
                    WebsiteOwner = bll.WebsiteOwner,
                    Activities = activities,
                    MyRegistration = myRegistration,
                    TheOrganizers = theOrganizers,
                    RegisterCode = registerCode,
                    IsShowHideActivity = isShowHideActivity,
                    OrganizerName = organizerName,
                    ActivitiesName = activitiesName,
                    MyRegistrationName = myRegistrationName,
                    QCodeType = qrCodeType,
                    ActivityStyle = 0,
                    ColorTheme = colorTheme,
                    ShowName=showName,
                    TicketShowName = ticketShowName,
                    ToolBarGroups=toolBarGroups
                };
                if (bllJuactivity.Add(config))
                {
                    resp.Msg = "添加成功";
                    resp.Status = 1;
                }
                else
                {
                    resp.Msg = "添加失败";
                    resp.Status = -1;
                }
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取全局配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetActivityConfig(HttpContext context)
        {
            BLLJIMP.Model.ActivityConfig config = bllJuactivity.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
            if (config != null)
            {
                resp.Status = 1;
                resp.ExObj = config;
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWxShowInfo(HttpContext context)
        {

            string autoId = context.Request["Autoid"];
            BLLJIMP.Model.WXShowInfo wxShowInfo = bllJuactivity.Get<BLLJIMP.Model.WXShowInfo>(string.Format(" AutoId={0}", autoId));
            if (wxShowInfo != null)
            {
                wxShowInfo.WXShowImgInfos = bllJuactivity.GetList<BLLJIMP.Model.WXShowImgInfo>(string.Format(" ShowId={0}", wxShowInfo.AutoId));
                resp.Status = 0;
                resp.ExObj = wxShowInfo;
            }
            else
            {
                resp.Status = 0;
                resp.ExObj = "系统出错，请联系管理员！！！";
            }


            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string InsertUpdateWXShowInfo(HttpContext context)
        {

            string autoId = context.Request["Autoid"];
            string showName = context.Request["ShowName"];
            string showDescription = context.Request["ShowDescription"];
            string showImg = context.Request["ShowImg"];
            string showUrl = context.Request["ShowUrl"];
            string imgStr = context.Request["ImgStr"];
            string showTitle = context.Request["ShowTitle"];
            string animation = context.Request["Animation"];
            string showContext = context.Request["ShowContext"];
            string showMusic = context.Request["ShowMusic"];

            string autoPlayTimeSpan = context.Request["AutoPlayTimeSpan"];

            string showTitleColor = context.Request["ShowTitleColor"];
            string showContextColor = context.Request["ShowContextColor"];

            

            if (string.IsNullOrEmpty(showName))
            {
                resp.Msg = "请填写微秀名称";
                goto outoff;

            }
            if (!string.IsNullOrEmpty(showUrl))
            {
                string match = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                Regex reg = new Regex(match);
                if (!reg.IsMatch(showUrl))
                {
                    resp.Msg = "请输入正确的网址,格式如 http://www.baidu.com";
                    // goto outoff;
                }



            }
            if (string.IsNullOrEmpty(autoId))
            {
                autoId = "0";
            }
            BLLJIMP.Model.WXShowInfo wxShowInfo = bllJuactivity.Get<BLLJIMP.Model.WXShowInfo>(string.Format(" Autoid={0}", autoId));

            #region 修改
            if (wxShowInfo != null)
            {
                //更新微秀信息
                wxShowInfo.ShowName = showName;
                wxShowInfo.ShowDescription = showDescription;
                wxShowInfo.ShowImg = showImg;
                wxShowInfo.ShowUrl = showUrl;
                wxShowInfo.ShowMusic = showMusic;
                wxShowInfo.AutoPlayTimeSpan = Convert.ToInt32(autoPlayTimeSpan);
                if (!bllJuactivity.Update(wxShowInfo))
                {
                    resp.Msg = "更新失败";
                    goto outoff;

                }
                //删除旧的微信展示页
                var resultCount = bllJuactivity.Delete(new WXShowImgInfo(), string.Format(" ShowId={0}", wxShowInfo.AutoId));

                //添加新展示页
                List<BLLJIMP.Model.WXShowImgInfo> wxsImgInfos = new List<BLLJIMP.Model.WXShowImgInfo>();
                string[] imgStrs = imgStr.Split(',');
                string[] showTitles = showTitle.Split(',');
                string[] animations = animation.Split(',');
                string[] showContexts = showContext.Split(',');

                string[] showTitleColors = showTitleColor.Split(',');
                string[] showContextColors = showContextColor.Split(',');

                for (int i = 0; i < imgStrs.Length; i++)
                {
                    if (string.IsNullOrEmpty(imgStrs[i]) && string.IsNullOrEmpty(showTitles[i]) &&
                        string.IsNullOrEmpty(animations[i]) && string.IsNullOrEmpty(showContexts[i
                        ]))
                    {
                        break;
                    }
                    else
                    {
                        wxsImgInfos.Add(new BLLJIMP.Model.WXShowImgInfo()
                        {
                            ShowId = wxShowInfo.AutoId,
                            ImgStr = imgStrs[i],
                            ShowTitle = showTitles[i],
                            ShowAnimation = Convert.ToInt32(animations[i]),
                            ShowContext = showContexts[i],
                            ShowTitleColor = showTitleColors[i],
                            ShowContextColor = showContextColors[i]
                        });
                    }
                }

                if (bllJuactivity.AddList<BLLJIMP.Model.WXShowImgInfo>(wxsImgInfos))
                {
                    resp.Status = 1;
                    resp.Msg = "修改成功";
                    goto outoff;
                }
                else
                {
                    resp.Msg = "插入新展示页失败";
                    goto outoff;
                };

            }
            #endregion

            #region 添加
            else
            {
                BLLJIMP.Model.WXShowInfo wxsInfo = new BLLJIMP.Model.WXShowInfo()
                {
                    ShowName = showName,
                    ShowDescription = showDescription,
                    ShowImg = showImg,
                    ShowUrl = showUrl,
                    InsertDate = DateTime.Now,
                    websiteOwner = bll.WebsiteOwner,
                    UserId = currentUserInfo.UserID,
                    ShowMusic = showMusic,
                    AutoPlayTimeSpan = Convert.ToInt32(autoPlayTimeSpan)
            };
                if (bllJuactivity.Add(wxsInfo))
                {
                    BLLJIMP.Model.WXShowInfo wxsi = bllJuactivity.Get<BLLJIMP.Model.WXShowInfo>(string.Format(" ShowName='{0}' AND websiteOwner='{1}' AND UserId='{2}'", showName, bll.WebsiteOwner, currentUserInfo.UserID));

                    List<BLLJIMP.Model.WXShowImgInfo> wxsiInfos = new List<BLLJIMP.Model.WXShowImgInfo>();
                    if (wxsi != null)
                    {
                        string[] imgStrs = imgStr.Split(',');
                        string[] showTitles = showTitle.Split(',');
                        string[] animations = animation.Split(',');
                        string[] showContexts = showContext.Split(',');

                        string[] showTitleColors = showTitleColor.Split(',');
                        string[] showContextColors = showContextColor.Split(',');


                        for (int i = 0; i < imgStrs.Length; i++)
                        {
                            if (string.IsNullOrEmpty(imgStrs[i]) && string.IsNullOrEmpty(showTitles[i]) &&
                                string.IsNullOrEmpty(animations[i]) && string.IsNullOrEmpty(showContexts[i
                                ]))
                            {
                                break;
                            }
                            else
                            {
                                wxsiInfos.Add(new BLLJIMP.Model.WXShowImgInfo()
                                {
                                    ShowId = wxsi.AutoId,
                                    ImgStr = imgStrs[i],
                                    ShowTitle = showTitles[i],
                                    ShowAnimation = Convert.ToInt32(animations[i]),
                                    ShowContext = showContexts[i],
                                    ShowTitleColor = showTitleColors[i],
                                    ShowContextColor = showContextColors[i]
                                });
                            }
                        }

                    }
                    if (bllJuactivity.AddList<BLLJIMP.Model.WXShowImgInfo>(wxsiInfos))
                    {
                        resp.Status = 1;
                        resp.Msg = "添加成功";
                    }
                    else
                    {

                        resp.Msg = "添加失败";
                    }
                }
                else
                {

                    resp.Msg = "添加失败";
                }
            }
            #endregion

        outoff:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取微秀信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWxShowInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.WXShowInfo> data;
            string showName = context.Request["Name"];

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" websiteOwner='{0}'", bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(showName))
            {
                sbWhere.AppendFormat(" AND ShowName like '{0}%' ", showName);
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.WXShowInfo>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.WXShowInfo>(pageSize, pageIndex, sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = data
    });
        }

        /// <summary>
        /// 删除微秀
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeletetWxShowInfos(HttpContext context)
        {
            string ids = context.Request["ids"];
            ZentCloud.ZCBLLEngine.BLLTransaction tran = new ZCBLLEngine.BLLTransaction();
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请至少选择一条！！！";
                goto Outf;
            }
            int count = bllJuactivity.Delete(new BLLJIMP.Model.WXShowInfo(), string.Format(" AutoId in ({0})", ids), tran);
            bllJuactivity.Delete(new BLLJIMP.Model.WXShowImgInfo(), string.Format(" ShowId in ({0})", ids), tran);
            if (count > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功。";
                tran.Commit();
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "删除失败。";
                tran.Rollback();
            }

        Outf:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取个人活动列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMyActivityDataInfos(HttpContext context)
        {
            string pageIndex = context.Request["PageIndex"];
            int pageSize = int.Parse(context.Request["PageSize"]);
            string activityName = context.Request["ActivityName"];
            string ctype = context.Request["ctype"];
            StringBuilder sbWhere = new StringBuilder();
            if (string.IsNullOrEmpty(pageIndex))
            {
                pageIndex = "1";
            }
            if (!string.IsNullOrEmpty(activityName))
            {
                sbWhere.AppendFormat(" AND ActivityName LIKE '%{0}%'", activityName);
            }
            if (!string.IsNullOrEmpty(ctype))
            {
                sbWhere.AppendFormat(" AND CategoryId='{0}'", ctype);

            }
            List<BLLJIMP.Model.ActivityDataInfo> adInfos = bllUser.GetList<BLLJIMP.Model.ActivityDataInfo>(string.Format(" WeixinOpenID='{0}'", this.currentUserInfo.WXOpenId));
            if (adInfos != null)
            {
                string str = "";

                for (int i = 0; i < adInfos.Count; i++)
                {
                    if (i + 1 == adInfos.Count)
                    {
                        str += adInfos[i].ActivityID;
                    }
                    else
                    {
                        str += adInfos[i].ActivityID + ',';
                    }
                }

                sbWhere.AppendFormat(" AND SignUpActivityID in ({0})", str);
            }
            BLLJIMP.Model.ActivityConfig activityConfig = new ActivityConfig();
            activityConfig = bllUser.Get<BLLJIMP.Model.ActivityConfig>(string.Format(" WebsiteOwner='{0}'", bllUser.WebsiteOwner));
            List<BLLJIMP.Model.JuActivityInfo> data = bllUser.GetLit<BLLJIMP.Model.JuActivityInfo>(pageSize, Convert.ToInt32(pageIndex), string.Format(" WebsiteOwner='{0}' And ArticleType='activity' AND IsSys = 0   AND IsDelete=0 {1} ", bll.WebsiteOwner, sbWhere.ToString()), "Sort ASC,ActivityStartDate Desc");
            foreach (var item in data)
            {
                item.ActivityDescription = "";
            }
            if (data != null && data.Count != 0)
            {
                resp.Status = 1;
                resp.ExObj = data;
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "没有数据";
            }
            resp.ExInt = activityConfig.ActivityStyle;



            return Common.JSONHelper.ObjectToJson(resp);

        }


        /// <summary>
        /// 微秀分享
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ShareWXShow(HttpContext context)
        {
            int autoId = int.Parse(context.Request["AutoId"]);
            int type = int.Parse(context.Request["Type"]);
            WXShowInfo model = bllUser.Get<WXShowInfo>(string.Format("AutoId={0}", autoId));
            if (type.Equals(0))
            {
                model.ShareAppMessageCount++;
            }
            else if (type.Equals(1))
            {
                model.ShareTimelineCount++;
            }
            if (bllUser.Update(model))
            {
                resp.Status = 1;
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}