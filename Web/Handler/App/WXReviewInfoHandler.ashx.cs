using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using System.Reflection;
using System.Text;
using System.Web.SessionState;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// WXReviewInfoHandler 的摘要说明
    /// </summary>
    public class WXReviewInfoHandler : IHttpHandler, IRequiresSessionState
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
                    resp.Status = -1;
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
        /// 获取评论所有信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetReviewInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.ReviewInfo> data;
            string reviewType = context.Request["ReviewType"];
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" 1=1 "));
            if (!string.IsNullOrEmpty(reviewType))
            {
                sbWhere.AppendFormat(" AND ReviewType = '{0}'", reviewType);
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.ReviewInfo>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.ReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), " InsertDate desc");

            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = data
     });

        }

        /// <summary>
        ///  删除所有评信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteReviewInfos(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "至少选一行数据";
                goto OutF;
            }
            int count = bllJuactivity.Delete(new BLLJIMP.Model.ReviewInfo(), " Autoid in (" + ids + ")");
            if (count > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功";
                goto OutF;
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "删除失败";
                goto OutF;
            }


        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveReviewConFig(HttpContext context)
        {
            string voteId = context.Request["Vote"];
            string article = context.Request["Article"];
            string activity = context.Request["Activity"];

            bool isSuccess = bllJuactivity.Update(new BLLJIMP.Model.ReviewConFig()
               {
                   AutoId = 1,
                   VoteId = voteId,
                   Article = article,
                   Activity = activity
               });
            if (isSuccess)
            {
                resp.Status = 0;
                resp.Msg = "配置成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "配置失败";
            }


            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取评论信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetReviewConFig(HttpContext context)
        {

            BLLJIMP.Model.ReviewConFig config = bllJuactivity.Get<BLLJIMP.Model.ReviewConFig>(" AutoId=1");
            if (config != null)
            {
                resp.Status = 0;
                resp.ExObj = config;
                resp.Msg = "配置成功";
            }
            else
            {
                resp.Status = -1;

            }


            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 保存回复
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReplyReviewInfo(HttpContext context)
        {

            string reviewId = context.Request["ReviewID"];
            string replyContent = context.Request["ReplyContent"];
            string parentId = context.Request["AutoId"];
            if (string.IsNullOrEmpty(replyContent))
            {
                resp.Status = -1;
                resp.Msg = "回复内容不能为空";
            }

            bool isSuccess = bllJuactivity.Add(new BLLJIMP.Model.ReplyReviewInfo()
                {
                    ReviewID = Convert.ToInt32(reviewId),
                    ReplyContent = replyContent,
                    PraentId = Convert.ToInt32(parentId),
                    UserId = currentUserInfo.UserID,
                    UserName = currentUserInfo.TrueName,
                    InsertDate = DateTime.Now,
                });
            BLLJIMP.Model.ReviewInfo review = bllJuactivity.Get<BLLJIMP.Model.ReviewInfo>(" AutoId=" + reviewId);
            if (isSuccess)
            {
                if (review != null)
                {
                    review.NumCount++;
                    bllJuactivity.Update(review);
                }
                resp.Status = 0;
                resp.Msg = "添加成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "添加失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 检查是否需要
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string IsCheck(HttpContext context)
        {

            string strWhere = context.Request["Wherestr"];
            if (string.IsNullOrEmpty(strWhere))
            {
                resp.Status = -1;
                resp.Msg = "";
            }
            BLLJIMP.Model.ReviewConFig config = bllJuactivity.Get<BLLJIMP.Model.ReviewConFig>(" " + strWhere + "='1'");
            if (config != null)
            {
                resp.Status = 0;
                resp.Msg = "成功";
            }
            else
            {
                resp.Status = -1;
                resp.Msg = "失败";
            }

            return Common.JSONHelper.ObjectToJson(resp);

        }


        #region 积分分类
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetScoreTypeInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.WXMallScoreTypeInfo> data;
            string typeName = context.Request["TypeName"];

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" websiteOwner='{0}'", bll.WebsiteOwner));
            if (!string.IsNullOrEmpty(typeName))
            {
                sbWhere.AppendFormat(" AND TypeName = '{0}'", typeName);
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.WXMallScoreTypeInfo>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.WXMallScoreTypeInfo>(pageSize, pageIndex, sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
    new
    {
        total = totalCount,
        rows = data
    });
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteScoreTypeInfos(HttpContext context)
        {

            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请选择一行数据";
                goto OutF;
            }
            int count = bllJuactivity.Delete(new BLLJIMP.Model.WXMallScoreTypeInfo(), " Autoid in (" + ids + ")");
            if (count > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "删除失败";
            }


        OutF:
            return Common.JSONHelper.ObjectToJson(resp);

        }

        /// <summary>
        /// 添加更新分类
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ADScoreTypeInfo(HttpContext context)
        {

            string autoId = context.Request["AutoId"];
            string typeName = context.Request["TName"];
            string typeImg = context.Request["TypeImg"];
            if (string.IsNullOrEmpty(autoId))
            {
                autoId = "0";
            }
            BLLJIMP.Model.WXMallScoreTypeInfo model = bllJuactivity.Get<BLLJIMP.Model.WXMallScoreTypeInfo>(string.Format(" AutoId={0}", autoId));
            if (model != null)
            {
                model.TypeName = typeName;
                model.TypeImg = typeImg;
                model.websiteOwner = bll.WebsiteOwner;
                bool isSuccess = bllJuactivity.Update(model);
                if (isSuccess)
                {
                    resp.Status = 0;
                    resp.Msg = "修改成功";
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "修改失败";
                }
            }
            else
            {
                model = new BLLJIMP.Model.WXMallScoreTypeInfo()
                {
                    TypeName = typeName,
                    TypeImg = typeImg,
                    websiteOwner = bll.WebsiteOwner

                };
                bool isSuccess = bllJuactivity.Add(model);
                if (isSuccess)
                {
                    resp.Status = 0;
                    resp.Msg = "添加成功。";
                }
                else
                {
                    resp.Status = 0;
                    resp.Msg = "添加失败。";
                }
            }

            return Common.JSONHelper.ObjectToJson(resp);
        }

        /// <summary>
        /// 获取分类详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetScoreTypeInfo(HttpContext context)
        {

            string autoId = context.Request["Autoid"];
            if (string.IsNullOrEmpty(autoId))
            {
                resp.Status = -1;
                resp.Msg = "系统错误请联系管理员";
                goto OutF;
            }
            BLLJIMP.Model.WXMallScoreTypeInfo model = bllJuactivity.Get<BLLJIMP.Model.WXMallScoreTypeInfo>(string.Format(" AutoId={0}", autoId));
            if (model != null)
            {
                resp.Msg = "";
                resp.Status = 0;
                resp.ExObj = model;
            }


        OutF:
            return Common.JSONHelper.ObjectToJson(resp);
        }
        #endregion

        private string GetReplyReviewInfos(HttpContext context)
        {
            int totalCount;
            List<BLLJIMP.Model.ReplyReviewInfo> data;
            string reviewType = context.Request["ReviewID"];

            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            //System.Text.StringBuilder sbWhere = new StringBuilder(string.Format(" Activityid='{0}'", ActivityId));
            StringBuilder sbWhere = new StringBuilder(string.Format(" 1=1 "));
            if (!string.IsNullOrEmpty(reviewType))
            {
                sbWhere.AppendFormat(" AND ReviewID = '{0}'", reviewType);
            }
            totalCount = this.bllJuactivity.GetCount<BLLJIMP.Model.ReplyReviewInfo>(sbWhere.ToString());
            data = this.bllJuactivity.GetLit<BLLJIMP.Model.ReplyReviewInfo>(pageSize, pageIndex, sbWhere.ToString(), " InsertDate desc");

            return Common.JSONHelper.ObjectToJson(
     new
     {
         total = totalCount,
         rows = data
     });

        }

        /// <summary>
        /// 删除回复信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteReplyReviewInfos(HttpContext context)
        {
            string ids = context.Request["ids"];
            if (string.IsNullOrEmpty(ids))
            {
                resp.Status = -1;
                resp.Msg = "请选择一行数据";
                goto OutF;
            }
            int count = bllJuactivity.Delete(new BLLJIMP.Model.ReplyReviewInfo(), " Autoid in (" + ids + ")");
            if (count > 0)
            {
                resp.Status = 0;
                resp.Msg = "删除成功";
            }
            else
            {
                resp.Status = 0;
                resp.Msg = "删除失败";
            }


        OutF:
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