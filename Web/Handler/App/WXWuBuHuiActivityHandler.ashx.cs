using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Web.SessionState;
using System.Reflection;
using System.Text;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// WXWuBuHuiActivityHandler 的摘要说明
    /// </summary>
    public class WXWuBuHuiActivityHandler : IHttpHandler, IRequiresSessionState
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
        /// 基类BLL
        /// </summary>
        BLL bll = new BLL();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        ZentCloud.BLLJIMP.Model.UserInfo currentUserInfo; //当前登陆的用户
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                string action = context.Request["Action"];
                this.currentUserInfo = bll.GetCurrentUserInfo();
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
        /// 保存用户积分
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SavaUserScore(HttpContext context)
        {

            string signUpId = context.Request["signUpId"];
            BLLJIMP.Model.JuActivityInfo juActivityInfo = bllJuactivity.Get<BLLJIMP.Model.JuActivityInfo>(string.Format(" WebsiteOwner='{0}' and SignUpActivityID='{1}'", bll.WebsiteOwner, signUpId));
            if ((this.currentUserInfo.TotalScore >= juActivityInfo.ActivityIntegral) && (juActivityInfo.ActivityIntegral > 0))
            {
                this.currentUserInfo.TotalScore -= juActivityInfo.ActivityIntegral;
                if (bllJuactivity.Update(this.currentUserInfo))
                {
                    BLLJIMP.Model.WBHScoreRecord record = new BLLJIMP.Model.WBHScoreRecord()
                    {
                        NameStr = "参加" + juActivityInfo.ActivityName,
                        Nums = "b55",
                        InsertDate = DateTime.Now,
                        ScoreNum = "-" + juActivityInfo.ActivityIntegral,
                        WebsiteOwner = bll.WebsiteOwner,
                        UserId = this.currentUserInfo.UserID,
                        RecordType = "1"

                    };
                    bllJuactivity.Add(record);
                    resp.Status = 0;
                }
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }


        /// <summary>
        /// 保存积分
        /// </summary>
        /// <param name="p"></param>
        private bool SavaUserToTalScol(string userId, int score)
        {

            BLLJIMP.Model.UserInfo userInfo = bllJuactivity.Get<BLLJIMP.Model.UserInfo>(string.Format(" UserId='{0}'", userId));
            userInfo.TotalScore = userInfo.TotalScore + score;
            return bllJuactivity.Update(userInfo, string.Format("TotalScore={0}", userInfo.TotalScore), string.Format("AutoID={0}", currentUserInfo.UserID)) > 0;


        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetActivityList(HttpContext context)
        {
            string pageIndex = context.Request["pageIndex"];
            string pageSize = context.Request["pageSize"];
            string value = context.Request["value"];
            string type = context.Request["type"];
            string currUser = context.Request["currUser"];
            string title = context.Request["Title"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" ArticleType='activity' AND IsDelete=0 AND WebsiteOwner='{0}'", bll.WebsiteOwner);
            StringBuilder sbOrderby = new StringBuilder();
            if (!string.IsNullOrEmpty(title))
            {
                sbWhere.AppendFormat(" AND ActivityName like '%{0}%'", title);
            }

            if (value.Equals("time"))
            {
                sbOrderby.Append("IsHide ASC,CreateDate  DESC");
            }
            else if (value.Equals("num"))
            {
                sbOrderby.Append("IsHide ASC,SignUpCount DESC");

            }
            else if (value.Equals("starttime"))
            {
                sbOrderby.Append("IsHide ASC,ActivityStartDate ASC");


            }
            else
            {
                sbOrderby.Append("IsHide ASC,CreateDate DESC");
            }
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" And CategoryId='{0}' ", type);
            }
            if (currUser.Equals("my"))
            {
                List<BLLJIMP.Model.ActivityDataInfo> activityData = bllUser.GetList<BLLJIMP.Model.ActivityDataInfo>(string.Format(" WeixinOpenID='{0}' And IsDelete=0 ", this.currentUserInfo.WXOpenId));
                if (activityData != null)
                {
                    string str = "";

                    for (int i = 0; i < activityData.Count; i++)
                    {
                        if (i + 1 == activityData.Count)
                        {
                            str += activityData[i].ActivityID;
                        }
                        else
                        {
                            str += activityData[i].ActivityID + ',';
                        }
                    }

                    sbWhere.AppendFormat(" AND SignUpActivityID in ({0})", str);
                }
                else
                {
                    sbWhere.AppendFormat(" AND 1=0");
                }

            }


            List<BLLJIMP.Model.JuActivityInfo> juaInfos = bllJuactivity.GetLit<BLLJIMP.Model.JuActivityInfo>(Convert.ToInt32(pageSize), Convert.ToInt32(pageIndex), sbWhere.ToString(), sbOrderby.ToString());


            if (juaInfos != null && juaInfos.Count > 0)
            {
                //if (value.Equals("num"))
                //{
                //    List<BLLJIMP.Model.JuActivityInfo> numjuainfos = new List<BLLJIMP.Model.JuActivityInfo>();
                //    int cnt = juaInfos.Count;
                //    for (int i = 0; i < cnt; ++i)
                //    {
                //        int maxindex = 0;
                //        for (int j = 0; j < juaInfos.Count; ++j)
                //        {
                //            if (juaInfos[j].SignUpTotalCount > juaInfos[maxindex].SignUpTotalCount)
                //            {
                //                maxindex = j;
                //            }
                //        }
                //        numjuainfos.Add(juaInfos[maxindex]);
                //        juaInfos.RemoveAt(maxindex);
                //    }
                //    juaInfos = numjuainfos;
                //}
                foreach (var item in juaInfos)
                {
                    if (item.ActivityEndDate != null)
                    {
                        if (DateTime.Now >= (DateTime)item.ActivityEndDate)
                        {
                            item.IsHide = 1;
                        }
                    }
                }



                var listPro = juaInfos.Where(p => p.IsHide == -1).ToList();//等待进行中的活动
                var listStart = juaInfos.Where(p => p.IsHide == 0).ToList();//正在进行中的活动
                var listStop = juaInfos.Where(p => p.IsHide == 1).ToList();//已经停止的活动
                listStop = listStop.OrderByDescending(p => p.ActivityStartDate).ToList();

                List<ZentCloud.BLLJIMP.Model.JuActivityInfo> data = new List<BLLJIMP.Model.JuActivityInfo>();
                data.AddRange(listStart);
                data.AddRange(listPro);
                data.AddRange(listStop);
                resp.Status = 0;
                resp.ExObj = data;
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "没有数据";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        private string GetScoreRecordS(HttpContext context)
        {

            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string type = context.Request["type"];
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.AppendFormat(" UserId='{0}' AND WebsiteOwner='{1}'", this.currentUserInfo.UserID, bll.WebsiteOwner);
            if (!string.IsNullOrEmpty(type))
            {
                sbWhere.AppendFormat(" AND RecordType='{0}'", type);
            }
            List<BLLJIMP.Model.WBHScoreRecord> data = bllJuactivity.GetLit<BLLJIMP.Model.WBHScoreRecord>(pageSize, pageIndex, sbWhere.ToString(), " InsertDate Desc");
            if (data != null)
            {
                resp.Status = 0;
                resp.ExObj = data;
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