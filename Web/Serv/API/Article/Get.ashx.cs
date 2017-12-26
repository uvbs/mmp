using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Article
{
    /// <summary>
    /// 文章详细信息接口
    /// </summary>
    public class Get : BaseHandlerNoAction
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        /// <summary>
        /// 用户业务逻辑
        /// </summary>
        BLLUser bllUser = new BLLUser("");
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = null;
            try
            {
                string articleId = context.Request["article_id"];
                string no_score = context.Request["no_score"];
                string no_pv = context.Request["no_pv"];
                if (string.IsNullOrEmpty(articleId))
                {
                    resp.errmsg = "article_id 为必填项,请检查";
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                JuActivityInfo model = bllJuActivity.GetJuActivity(int.Parse(articleId));
                if (model == null)
                {
                    resp.errmsg = "没有找到文章信息";
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }

                if (no_pv != "1")
                {
                    bllJuActivity.Update(model, string.Format("PV=PV+1"), string.Format(" JuActivityID={0}", model.JuActivityID));
                }
                requestModel = new RequestModel();
                requestModel.article_id = model.JuActivityID;
                requestModel.article_name = model.ActivityName;
                requestModel.articel_context = model.ActivityDescription;
                requestModel.article_img_url = model.ThumbnailsPath;
                requestModel.article_pv = model.PV;
                requestModel.article_share_total_count = model.ShareTotalCount;
                requestModel.article_status = model.IsHide;
                requestModel.article_access_level = model.AccessLevel;
                requestModel.article_summary = model.Summary;
                requestModel.article_tags = model.Tags;
                requestModel.category_name = model.CategoryName;
                requestModel.article_sort = model.Sort;
                requestModel.article_time = bllJuActivity.GetTimeStamp(model.CreateDate);
                requestModel.k3 = model.K3;
                requestModel.cate_id = model.CategoryId;
                requestModel.article_type = model.ArticleType;
                if (no_score != "1")
                {
                    UserInfo currentUserInfo = bllUser.GetCurrentUserInfo();
                    if (currentUserInfo != null)
                    {
                        string tempMsg = "";
                        bllUser.AddUserScoreDetail(currentUserInfo.UserID, "ReadType", bllUser.WebsiteOwner, out tempMsg, null, null, model.ArticleType, true, model.ArticleType);
                        bllUser.AddUserScoreDetail(currentUserInfo.UserID, "ReadCategory", bllUser.WebsiteOwner, out tempMsg, null, null, model.CategoryId, true, model.CategoryId);
                        bllUser.AddUserScoreDetail(currentUserInfo.UserID, "ReadArticle", bllUser.WebsiteOwner, out tempMsg, null, "《" + model.ActivityName + "》", model.JuActivityID.ToString(), true, model.JuActivityID.ToString(), model.ArticleType);
                    }
                }
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsSuccess;
                resp.errmsg = "查询完成";
                resp.isSuccess = true;
            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }

            //代码莫名奇妙，上面已经加有积分，先注释
            //try
            //{
            //    if (bllUser.IsLogin)
            //    {
            //        bllUser.AddUserScoreDetail(bllUser.GetCurrUserID(), CommonPlatform.Helper.EnumStringHelper.ToString(ZentCloud.BLLJIMP.Enums.ScoreDefineType.ReadArticle), this.bllUser.WebsiteOwner, null, null);
            //    }
               
               
                      
                
            //}
            //catch (Exception)
            //{
                
                
            //}
        
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
        }
        /// <summary>
        /// 返回实体
        /// </summary>
        public class RequestModel
        {
            /// <summary>
            /// 文章id
            /// </summary>
            public int article_id { get; set; }

            /// <summary>
            /// 文章名称
            /// </summary>
            public string article_name { get; set; }

            /// <summary>
            /// 文章详情
            /// </summary>
            public string articel_context { get; set; }

            /// <summary>
            /// 文章缩略图
            /// </summary>
            public string article_img_url { get; set; }

            /// <summary>
            /// 文章排序
            /// </summary>
            public int? article_sort { get; set; }

            /// <summary>
            /// 文章浏览量
            /// </summary>
            public int article_pv { get; set; }

            /// <summary>
            /// 文章描述
            /// </summary>
            public string article_summary { get; set; }

            /// <summary>
            /// 文章标签
            /// </summary>
            public string article_tags { get; set; }

            /// <summary>
            /// 文章访问等级
            /// </summary>
            public int article_access_level { get; set; }

            /// <summary>
            /// 分类名称
            /// </summary>
            public string category_name { get; set; }

            /// <summary>
            /// 分享数
            /// </summary>
            public int article_share_total_count { get; set; }

            /// <summary>
            /// 文章状态 0显示  1隐藏
            /// </summary>
            public int? article_status { get; set; }

            /// <summary>
            /// 发布时间
            /// </summary>
            public double article_time { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string k3 { get; set; }
            /// <summary>
            /// 分类id
            /// </summary>
            public string cate_id { get; set; }

            /// <summary>
            /// 文章类型
            /// </summary>
            public string article_type { get; set; }

        }
    }
}