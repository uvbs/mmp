using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Article
{
    /// <summary>
    /// Add 的摘要说明  添加文章接口
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLJuActivity bllJuActivity = new BLLJIMP.BLLJuActivity();
        public void ProcessRequest(HttpContext context)
        {
            string data = context.Request["data"];
            RequestModel requestModel;
            try
            {
                requestModel = ZentCloud.Common.JSONHelper.JsonToModel<RequestModel>(context.Request["data"],false);
            }
            catch (Exception)
            {
                resp.errcode =(int)BLLJIMP.Enums.APIErrCode.PrimaryKeyIncomplete;
                resp.errmsg = "json格式错误,请检查";
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.article_name))
            {
                resp.errmsg = "article_name 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.article_context))
            {
                resp.errmsg = "article_context 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.article_img_url))
            {
                resp.errmsg = "article_img_url 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            if (string.IsNullOrEmpty(requestModel.article_tags))
            {
                resp.errmsg = "article_tags 为必填项,请检查";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.ContentNotFound;
                context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
                return;
            }
            BLLJIMP.Model.JuActivityInfo model = new BLLJIMP.Model.JuActivityInfo();
            model.JuActivityID = int.Parse(bllJuActivity.GetGUID(BLLJIMP.TransacType.ActivityAdd));
            model.ActivityName = requestModel.article_name;
            model.UserID = bllJuActivity.GetCurrUserID();
            model.ActivityDescription = requestModel.article_context;
            model.ThumbnailsPath = requestModel.article_img_url;
            model.IsSignUpJubit = 0;
            model.CreateDate = DateTime.Now;
            model.Sort = requestModel.article_sort;
            model.IsToJubitActivity = "未收录";
            model.IsDelete = 0;
            model.ArticleType = "article";
            model.IsSpread = 1;
            model.IsHide = requestModel.article_ishide;
            model.WebsiteOwner = bllJuActivity.WebsiteOwner;
            model.ArticleTypeEx1 = bllJuActivity.GetCurrUserID() + "_" + "article";
            model.CategoryId = requestModel.category_id;
            model.LastUpdateDate = DateTime.Now;
            model.Summary = requestModel.article_summary;
            model.PV = requestModel.article_pv;
            model.ShareTotalCount = requestModel.article_share_total_count;
            model.AccessLevel = requestModel.article_access_level;
            model.Tags = requestModel.article_tags;
            model.ProvinceCode = requestModel.province_code;
            model.CityCode = requestModel.city_code;
            model.DistrictCode = requestModel.district_code;
            model.RootCateId = requestModel.root_cate_id;
            model.K1 = requestModel.article_ex1;
            model.K2 = requestModel.article_ex2;
            model.K3 = requestModel.article_ex3;
            model.K4 = requestModel.article_ex4;
            model.K5 = requestModel.article_ex5;
            model.K6 = requestModel.article_ex6;
            model.K7 = requestModel.article_ex7;
            model.K8 = requestModel.article_ex8;
            model.K9 = requestModel.article_ex9;
            model.K10 = requestModel.article_ex10;
            model.K11 = requestModel.article_ex11;
            model.K12 = requestModel.article_ex12;
            model.K13 = requestModel.article_ex13;
            model.K14 = requestModel.article_ex14;
            model.K15 = requestModel.article_ex15;

            if (bllJuActivity.Add(model))
            {
                resp.errmsg = "ok";
                resp.isSuccess = true;
            }
            else
            {
                resp.errmsg = "添加文章出错";
                resp.errcode = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
            }
            context.Response.Write(ZentCloud.Common.JSONHelper.ObjectToJson(resp));
        }

        public class RequestModel
        {
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
            /// 0显示  1隐藏
            /// </summary>
            public int article_ishide { get; set; }

            /// <summary>
            /// 文章访问等级
            /// </summary>
            public int article_access_level { get; set; }

            /// <summary>
            /// 分类名称
            /// </summary>
            public string category_id { get; set; }

            /// <summary>
            /// 分享数
            /// </summary>
            public int article_share_total_count { get; set; }


            public string root_cate_id { get; set; }


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

            /// <summary>
            /// 扩展字段1
            /// </summary>
            public string article_ex1 { get; set; }
            /// <summary>
            /// 扩展字段2
            /// </summary>
            public string article_ex2 { get; set; }
            /// <summary>
            /// 扩展字段3
            /// </summary>
            public string article_ex3 { get; set; }
            /// <summary>
            /// 扩展字段4
            /// </summary>
            public string article_ex4 { get; set; }
            /// <summary>
            /// 扩展字段5
            /// </summary>
            public string article_ex5 { get; set; }
            /// <summary>
            /// 扩展字段6
            /// </summary>
            public string article_ex6 { get; set; }
            /// <summary>
            /// 扩展字段7
            /// </summary>
            public string article_ex7 { get; set; }
            /// <summary>
            /// 扩展字段8
            /// </summary>
            public string article_ex8 { get; set; }
            /// <summary>
            /// 扩展字段9
            /// </summary>
            public string article_ex9 { get; set; }
            /// <summary>
            /// 扩展字段10
            /// </summary>
            public string article_ex10 { get; set; }

            /// <summary>
            /// 扩展字段11
            /// </summary>
            public string article_ex11 { get; set; }

            /// <summary>
            /// 扩展字段12
            /// </summary>
            /// 
            public string article_ex12 { get; set; }
            /// <summary>
            /// 扩展字段13
            /// </summary>
            /// 
            public string article_ex13 { get; set; }
            /// <summary>
            /// 扩展字段14
            /// </summary>
            public string article_ex14 { get; set; }

            /// <summary>
            /// 扩展字段15
            /// </summary>
            public string article_ex15 { get; set; }

        }
    }
}