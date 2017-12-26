using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TypeConfig
{
    /// <summary>
    /// Set 的摘要说明
    /// </summary>
    public class Set : BaseHandlerNeedLoginAdminNoAction
    {
        BLLArticleCategory bllArticleCategory = new BLLArticleCategory();

        public void ProcessRequest(HttpContext context)
        {
            ConfigModel nConfig = bllArticleCategory.ConvertRequestToModel<ConfigModel>(new ConfigModel());
            ArticleCategoryTypeConfig oConfig = bllArticleCategory.GetArticleCategoryTypeConfig(bllArticleCategory.WebsiteOwner, nConfig.type);
            string action = (oConfig == null || oConfig.WebsiteOwner=="Common")?"add":"edit";
            if (oConfig == null) oConfig = new ArticleCategoryTypeConfig();
            oConfig.WebsiteOwner = bllArticleCategory.WebsiteOwner;
            oConfig.CategoryType = nConfig.type;
            oConfig.TimeSetMethod = nConfig.time_set_method;
            oConfig.TimeSetStyle = nConfig.time_set_style;
            oConfig.CategoryTypeTitle = nConfig.title;
            oConfig.CategoryTypeHomeTitle = nConfig.home_title;
            oConfig.CategoryTypeOrderListTitle = nConfig.order_list_title;
            oConfig.CategoryTypeOrderDetailTitle = nConfig.order_detail_title;
            oConfig.CategoryTypeDispalyName = nConfig.name;
            oConfig.CategoryTypeExDispalyName = nConfig.name_ex;
            oConfig.ShareTitle = nConfig.share_title;
            oConfig.ShareImg = nConfig.share_img;
            oConfig.ShareDesc = nConfig.share_desc;
            oConfig.ShareLink = nConfig.share_link;
            oConfig.SpendMethod = nConfig.spend_method;
            oConfig.CategoryTypeStockName = nConfig.stock_name;
            bool result = false;
            if (action == "add")
            {
                result = bllArticleCategory.Add(oConfig);
            }
            else
            {
                result = bllArticleCategory.Update(new ArticleCategoryTypeConfig(),
                    string.Format("CategoryTypeDispalyName='{0}',CategoryTypeExDispalyName='{1}',ShareTitle='{2}',ShareImg='{3}',ShareDesc='{4}',"+
                    "ShareLink='{5}',CategoryTypeTitle='{6}',CategoryTypeHomeTitle='{7}',CategoryTypeOrderListTitle='{8}',"+
                    "CategoryTypeOrderDetailTitle='{9}',TimeSetMethod='{10}',TimeSetStyle='{11}',SpendMethod='{12}',CategoryTypeStockName='{13}'",
                    oConfig.CategoryTypeDispalyName, oConfig.CategoryTypeExDispalyName, oConfig.ShareTitle, oConfig.ShareImg, oConfig.ShareDesc, 
                    oConfig.ShareLink, oConfig.CategoryTypeTitle, oConfig.CategoryTypeHomeTitle, oConfig.CategoryTypeOrderListTitle,
                    oConfig.CategoryTypeOrderDetailTitle, oConfig.TimeSetMethod, oConfig.TimeSetStyle, oConfig.SpendMethod, oConfig.CategoryTypeStockName),
                    string.Format("WebsiteOwner='{0}' AND CategoryType='{1}'", oConfig.WebsiteOwner, oConfig.CategoryType))>0;
            }
            if (result)
            {
                apiResp.msg = "提交完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "提交失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bllArticleCategory.ContextResponse(context, apiResp);
        }
        private class ConfigModel
        {
            /// <summary>
            /// 类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 时间方式
            /// </summary>
            public int time_set_method { get; set; }
            /// <summary>
            /// 时间展示
            /// </summary>
            public int time_set_style { get; set; }
            /// <summary>
            /// 消费方式
            /// </summary>
            public int spend_method { get; set; }
            /// <summary>
            /// 微信标题
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 首页标题
            /// </summary>
            public string home_title { get; set; }
            /// <summary>
            /// 订单列表标题
            /// </summary>
            public string order_list_title { get; set; }
            /// <summary>
            /// 订单详情标题
            /// </summary>
            public string order_detail_title { get; set; }
            /// <summary>
            /// 商品别名
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 分类别名
            /// </summary>
            public string name_ex { get; set; }
            /// <summary>
            /// 库存别名
            /// </summary>
            public string stock_name { get; set; }
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
        }
    }
}