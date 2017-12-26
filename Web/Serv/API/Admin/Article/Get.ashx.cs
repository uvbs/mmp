using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Article
{
    /// <summary>
    /// Get 的摘要说明   获取文章详情接口
    /// </summary>
    public class Get : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// BLL
        /// </summary>
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel=null;
            try
            {
                string articleId = context.Request["article_id"];
                if (string.IsNullOrEmpty(articleId))
                {
                    resp.errmsg = "article_id 为必填项,请检查";
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                BLLJIMP.Model.JuActivityInfo model = bllJuActivity.GetJuActivity(int.Parse(articleId));
                if (model == null)
                {
                    resp.errmsg = "没有找到文章信息";
                    resp.errcode = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                    return;
                }
                requestModel = new RequestModel();
                requestModel.article_id = model.JuActivityID;
                requestModel.article_name = model.ActivityName;
                requestModel.article_context = model.ActivityDescription;
                requestModel.article_img_url = model.ThumbnailsPath;
                requestModel.article_pv = model.PV;
                requestModel.article_share_total_count = model.ShareTotalCount;
                requestModel.article_access_level = model.AccessLevel;
                requestModel.article_summary = model.Summary;
                requestModel.article_tags = model.Tags;
                requestModel.category_name = model.CategoryName;
                requestModel.article_sort = model.Sort;
                requestModel.article_ishide = model.IsHide;
                requestModel.province_code = model.ProvinceCode;
                requestModel.city_code = model.CityCode;
                requestModel.district_code = model.DistrictCode;
            }
            catch (Exception ex)
            {
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                resp.errmsg = ex.Message;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(requestModel));
        }

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
            public string article_context { get; set; }

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
            /// 是否显示 0显示 1隐藏
            /// </summary>
            public int? article_ishide { get; set; }

            /// <summary>
            /// 省
            /// </summary>
            public string province_code { get; set; }

            /// <summary>
            /// 市
            /// </summary>
            public string city_code { get; set; }

            /// <summary>
            /// 区
            /// </summary>
            public string district_code { get; set; }

        }
    }
}