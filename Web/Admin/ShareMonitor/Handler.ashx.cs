using System;
using System.Reflection;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using System.Text;
using System.Web.SessionState;
using System.Linq;
using System.Collections.Generic;

namespace ZentCloud.JubitIMP.Web.Admin.ShareMonitor
{
    /// <summary>
    /// 监测
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 默认响应
        /// </summary>
        DefaultResponse resp = new DefaultResponse();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLJuActivity bll = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        UserInfo currentUserInfo;
        ///// <summary>
        ///// 网站所有者
        ///// </summary>
        //private string webSiteOwner;
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                if (bll.IsLogin)
                {
                    currentUserInfo = bll.GetCurrentUserInfo();
                }
                //webSiteOwner = bll.WebsiteOwner;
                string Action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(Action))
                {
                    MethodInfo method = this.GetType().GetMethod(Action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase); //找到方法BindingFlags.NonPublic指定搜索非公有方法
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {

                    resp.errmsg = "action not exist";
                    result = Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {

                resp.errcode = -1;
                resp.errmsg = ex.ToString();
                result = Common.JSONHelper.ObjectToJson(resp);
            }

            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                context.Response.Write(string.Format("{0}({1})", context.Request["callback"], result));
            }
            else
            {
                //返回json数据
                context.Response.Write(result);
            }
        }

        /// <summary>
        /// 监测列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryShareMonitor(HttpContext context)
        {
            string result = string.Empty;
            int page = Convert.ToInt32(context.Request["page"]), rows = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["keyword"];
            StringBuilder strWhere = new StringBuilder(" IsDel = 0 ");
            strWhere.AppendFormat(" AND  WebSiteOwner = '{0}' ", bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(keyWord))
            {
                strWhere.AppendFormat(" AND  MonitorName like '%{0}%' ", keyWord);
            }
            var list = this.bll.GetLit<ShareMonitorInfo>(rows, page, strWhere.ToString(), " MonitorId DESC ");
            var totalCount = this.bll.GetCount<ShareMonitorInfo>(strWhere.ToString());
            if (list.Count > 0)
            {
                //统计分享数和阅读数

                for (int i = 0; i < list.Count; i++)
                {
                    //取出所有share
                    var shareList = this.bll.GetList<ShareInfo>(string.Format(" MonitorId = '{0}' ", list[i].MonitorId));

                    //添加那些没有shareid的阅读记录
                    var readCount = this.bll.GetCount<ShareReaderInfo>(string.Format("  MonitorId = '{0}'  ", list[i].MonitorId));

                    if (shareList != null && shareList.Count > 0)
                    {
                        list[i].ShareCount = shareList.Count;

                        //根据share取出阅读数
                        readCount += this.bll.GetCount<ShareReaderInfo>(string.Format(" ShareId in({0}) ", Common.StringHelper.ListToStr<string>(
                            shareList.Select(p => p.ShareId).ToList(),
                            "'",
                            ","
                            )));
                    }

                    list[i].ReadCount = readCount;
                }
            }

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = list
    });
        }
        /// <summary>
        /// 添加分享监测
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddShareMonitor(HttpContext context)
        {
            string result = string.Empty,
                   name = context.Request["name"],
                   url = context.Request["url"];

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url))
            {
                resp.isSuccess = false;
                resp.errcode = 1;
                resp.errmsg = "必要参数为空";
                goto returnResult;
            }

            ShareMonitorInfo model = new ShareMonitorInfo()
            {
                MonitorId = int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd)),
                CreateTime = DateTime.Now,
                MonitorName = name,
                MonitorUrl = url,
                ReadCount = 0,
                ShareCount = 0,
                IsDel = 0,
                CreateUser = currentUserInfo != null ? currentUserInfo.UserID : "",
                WebSiteOwner = bll.WebsiteOwner
            };

            if (bll.GetCount<ShareMonitorInfo>(string.Format(" MonitorUrl='{0}' and IsDel = 0 ", url)) > 0)
            {
                resp.isSuccess = false;
                resp.errcode = 2;
                resp.errmsg = "该链接已存在";
                goto returnResult;
            }

            if (bll.Add(model))
            {
                resp.isSuccess = true;
            }
            else
            {
                resp.errcode = 3;
                resp.isSuccess = false;
            }

        returnResult:
            return Common.JSONHelper.ObjectToJson(resp);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteShareMonitor(HttpContext context)
        {
            string ids = context.Request["ids"];
            int count = bll.Update(new ShareMonitorInfo(), " IsDel = 1 ", string.Format(" MonitorId in ({0}) ", ids));
            return count.ToString();
        }

        /// <summary>
        /// 分享统计
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetShareCountStatistics(HttpContext context)
        {
            string preId = context.Request["preId"], //shareId，每次查询是取出传入shareId的子集
                   mid = context.Request["mid"];

            bool isRoot = false;
            StringBuilder strShareInfo = new StringBuilder(" 1=1 ");
            if (string.IsNullOrWhiteSpace(preId))
            {
                //一级目录，返回一级分享和当前检测实体信息， 分享总数  阅读总数  成果总数
                strShareInfo.Append(" AND ( PreId = '' OR PreId IS NULL)");
                isRoot = true;
            }
            else
            {
                if (preId.IndexOf(',') > 0)
                {
                    var tmpList = preId.Split(',').ToList();
                    strShareInfo.AppendFormat(" AND PreId in ({0}) ", Common.StringHelper.ListToStr<string>(tmpList, "'", ","));
                }
                else
                {
                    strShareInfo.AppendFormat(" AND PreId = '{0}' ", preId);
                }
            }

            strShareInfo.AppendFormat(" AND MonitorId = {0} ", mid);

            var shareList = this.bll.GetList<ShareInfo>(strShareInfo.ToString());

            //share信息处理，按个人分享数归类到一块
            List<object> shareResultList = new List<object>();

            #region share信息处理

            if (shareList != null && shareList.Count > 0)
            {
                //取出分享人列表
                var sharerList = shareList.GroupBy(p => p.UserId).ToList();
                BLLJIMP.BLLUser userBll = new BLLJIMP.BLLUser();
                BLLJIMP.BLLWeixin wxBll = new BLLJIMP.BLLWeixin();

                foreach (var item in sharerList)
                {
                    //获取user信息
                    var userInfo = userBll.GetUserInfo(item.Key);

                    if (userInfo == null) continue;

                    //TODO:如果没有头像和姓名  需要去接口取相关头像和姓名 并保存到数据库
                    try
                    {
                        if (string.IsNullOrWhiteSpace(userInfo.WXHeadimgurl))
                        {
                            string accesstoken = wxBll.GetAccessToken(userInfo.WebsiteOwner);
                            var newflowerInfo = wxBll.GetWeixinUserInfo(accesstoken, userInfo.WXOpenId);

                            userInfo.WXHeadimgurl = newflowerInfo.headimgurl;
                            userInfo.WXNickname = newflowerInfo.nickname;

                            this.bll.Update(
                                    new UserInfo(),
                                    string.Format(" WXHeadimgurl='{0}',WXNickname='{1}' ", userInfo.WXHeadimgurl, userInfo.WXNickname),
                                    string.Format(" UserID='{0}' ", userInfo.UserID)
                                );

                        }
                    }
                    catch { }//TODO:临时方案取用户信息，如果出现异常暂时不处理

                    //获取ShareIds
                    var userShareList = shareList.Where(p => (p.UserId != null && p.UserId.ToLower() == userInfo.UserID.ToLower())).Select(p => p.ShareId).ToList();

                    //获取子分享数（所有shareIds的子分享数）
                    var childShareCount = this.bll.GetCount<ShareInfo>(string.Format(" PreId in ({0}) ", Common.StringHelper.ListToStr<string>(userShareList, "'", ",")));

                    //获取阅读数（所有shareIds的阅读总数）
                    var readCount = this.bll.GetCount<ShareReaderInfo>(string.Format(" ShareId in ({0}) ", Common.StringHelper.ListToStr<string>(userShareList, "'", ",")));

                    //获取分享数（等于shareIds总数）
                    var shareCount = userShareList.Count;

                    //获取成果数（所有shareIds带来的报名数，目前统计的是校服对应的那个报名表来统计）
                    //var achievement = this.bll.GetCount<VoteObjectInfo>(string.Format(" ComeonShareId in ({0}) ", Common.StringHelper.ListToStr<string>(userShareList, "'", ",")));

                    //成果数改成活动报名数，取出分享的活动id，查询活动报名是来自哪个用户或者哪个分享id

                    //var achievement = 
                    //    this.bll.GetCount<VoteObjectInfo>(string.Format(" ComeonShareId in ({0}) ", Common.StringHelper.ListToStr<string>(userShareList, "'", ",")));
                    
                    var achievement =
                        this.bll.GetCount<ActivityDataInfo>(string.Format(" ShareID in ({0}) ", Common.StringHelper.ListToStr<string>(userShareList, "'", ",")));
                    
                    shareResultList.Add(new
                    {
                        tmpDataKey = Guid.NewGuid().ToString(),
                        userInfo = new
                        {
                            avatar = userInfo.WXHeadimgurl,
                            userName = string.IsNullOrWhiteSpace(userInfo.WXNickname) ? "" : userInfo.WXNickname,
                            userId = userInfo.UserID,
                            wxOpenId = userInfo.WXOpenId
                        },
                        shareIds = userShareList,
                        preId = preId,
                        childShareCount = childShareCount,
                        readCount = readCount,
                        shareCount = shareCount,
                        achievement = achievement//成果
                    });

                }

            }
            #endregion

            object rootInfo = new object();

            #region rootInfo处理
            if (isRoot)
            {
                //当前检测实体信息， 分享总数  阅读总数  成果总数
                var monitor = this.bll.Get<ShareMonitorInfo>(string.Format(" MonitorId = {0} ", mid));
                var shareTotalCount = this.bll.GetCount<ShareInfo>(string.Format(" MonitorId = {0} ", mid));

                //阅读总数= 分享的阅读总数 + 直接阅读总数（未分享）
                var readTotalCount = this.bll.GetCount<ShareReaderInfo>(string.Format(" MonitorId = {0} ", mid));

                //成果总数= 分享的成功总数
                var achievementTotalCount = 0;

                //取出所有分享id
                var allShareList = this.bll.GetList<ShareInfo>(string.Format(" MonitorId = {0} ", mid));
                if (allShareList != null && allShareList.Count > 0)
                {
                    var allShareIds = allShareList.Select(p => p.ShareId).ToList();
                    readTotalCount += this.bll.GetCount<ShareReaderInfo>(string.Format(" ShareId IN ({0}) ", Common.StringHelper.ListToStr<string>(allShareIds, "'", ",")));

                    achievementTotalCount = this.bll.GetCount<VoteObjectInfo>(string.Format(" ComeonShareId IN ({0}) ", Common.StringHelper.ListToStr<string>(allShareIds, "'", ",")));
                }
                rootInfo = new
                {
                    title = monitor.MonitorName,
                    shareTotalCount = shareTotalCount,
                    readTotalCount = readTotalCount,
                    achievementTotalCount = achievementTotalCount
                };
            }
            #endregion

            return Common.JSONHelper.ObjectToJson(new
            {
                rootInfo = isRoot ? rootInfo : null,
                shareResultList = shareResultList
            });
        }
        /// <summary>
        /// 分享模型
        /// </summary>
        public class ShareResult
        {
            /// <summary>
            /// DataKey
            /// </summary>
            public string tmpDataKey { get; set; }
            /// <summary>
            /// 用户信息s
            /// </summary>
            public object userInfo { get; set; }
            /// <summary>
            /// 分享id
            /// </summary>
            public List<string> shareIds { get; set; }
            /// <summary>
            /// 上级Id
            /// </summary>
            public string preId { get; set; }
            /// <summary>
            /// 子分享数量
            /// </summary>
            public int childShareCount { get; set; }
            /// <summary>
            /// 阅读量
            /// </summary>
            public int readCount { get; set; }
            /// <summary>
            /// 分享数量 
            /// </summary>
            public int shareCount { get; set; }
            /// <summary>
            /// 附件
            /// </summary>
            public int achievement { get; set; }
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