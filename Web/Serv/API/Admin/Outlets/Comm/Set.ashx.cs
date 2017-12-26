using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Outlets.Comm
{
    /// <summary>
    /// Set 的摘要说明
    /// </summary>
    public class Set : BaseHandlerNeedLoginAdminNoAction
    {
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();
        bool isAdd = false;
        public void ProcessRequest(HttpContext context)
        {
            PostModel postModel = bllArticleCategory.ConvertRequestToModel<PostModel>(new PostModel());
            ArticleCategoryTypeConfig nCategoryTypeConfig = bllArticleCategory.GetByKey<ArticleCategoryTypeConfig>("CategoryType", postModel.field, true);
            if (nCategoryTypeConfig == null)
            {
                isAdd = true;
                nCategoryTypeConfig = new ArticleCategoryTypeConfig();
                nCategoryTypeConfig.CategoryType = postModel.field;
                nCategoryTypeConfig.WebsiteOwner = bllArticleCategory.WebsiteOwner;
            }
            nCategoryTypeConfig.CategoryTypeDispalyName = postModel.name;
            nCategoryTypeConfig.TimeSetMethod = postModel.map_show;
            //nCategoryTypeConfig.AppPagePath = postModel.app_page_path;
            nCategoryTypeConfig.ShareTitle = postModel.share_title;
            nCategoryTypeConfig.ShareImg = postModel.share_img;
            nCategoryTypeConfig.ShareDesc = postModel.share_desc;
            nCategoryTypeConfig.ShareLink = postModel.share_link;
            nCategoryTypeConfig.ShareTitle = postModel.share_title;
            nCategoryTypeConfig.ListFields = postModel.search_options;
            nCategoryTypeConfig.EditFields = postModel.search_fields;
            nCategoryTypeConfig.NeedFields = postModel.searc_keyword;
            nCategoryTypeConfig.Ex1 = postModel.search_app_options;
            nCategoryTypeConfig.Ex2 = postModel.searc_app_keyword;
            nCategoryTypeConfig.Ex3 = postModel.list_fields;
            nCategoryTypeConfig.Ex4 = postModel.list_app_fields;
            nCategoryTypeConfig.Ex5 = postModel.list_detail_fields;
            bool result = false;
            if (isAdd)
            {
                result = bllArticleCategory.Add(nCategoryTypeConfig);
            }
            else
            {
                result = bllArticleCategory.Update(nCategoryTypeConfig);
            }


            if (result)
            {
                apiResp.status = true;
                apiResp.msg = "提交成功";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }

            bllArticleCategory.ContextResponse(context, apiResp);
        }
        /// <summary>
        /// 模型
        /// </summary>
        public class PostModel
        {
            /// <summary>
            /// 字段
            /// </summary>
            public string field { get; set; }
            /// <summary>
            /// 类型名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 客户端地图显示 0无地图 1详情跳转 2列表直达
            /// </summary>
            public int map_show { get; set; }
            /// <summary>
            /// 客户端页面地址 生成二维码用
            /// </summary>
            public string app_page_path { get; set; }
            /// <summary>
            /// 分享标题
            /// </summary>
            public string share_title { get; set; }
            /// <summary>
            /// 分享图片
            /// </summary>
            public string share_img { get; set; }
            /// <summary>
            /// 分享描述
            /// </summary>
            public string share_desc { get; set; }
            /// <summary>
            /// 分享链接
            /// </summary>
            public string share_link { get; set; }
            /// <summary>
            /// 后台选项查询条件
            /// </summary>
            public string search_options { get; set; }
            /// <summary>
            /// 后台字段查询条件
            /// </summary>
            public string search_fields { get; set; }
            /// <summary>
            /// 后台关键字查询条件
            /// </summary>
            public string searc_keyword { get; set; }
            /// <summary>
            /// 前端选项查询条件
            /// </summary>
            public string search_app_options { get; set; }
            /// <summary>
            /// 前端关键字查询条件
            /// </summary>
            public string searc_app_keyword { get; set; }
            /// <summary>
            /// 后台列表字段
            /// </summary>
            public string list_fields { get; set; }
            /// <summary>
            /// 前端列表字段
            /// </summary>
            public string list_app_fields { get; set; }
            /// <summary>
            /// 前端详情字段
            /// </summary>
            public string list_detail_fields { get; set; }
        }
    }
}