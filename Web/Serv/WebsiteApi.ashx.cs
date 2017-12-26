using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ZentCloud.BLLJIMP.Model;
using System.Reflection;

namespace ZentCloud.JubitIMP.Web.Serv
{
    /// <summary>
    /// 微网站接口
    /// </summary>
    public class WebsiteApi : IHttpHandler, IReadOnlySessionState
    {
        /// <summary>
        /// 默认响应模型
        /// </summary>
        AshxResponse resp = new AshxResponse();
        /// <summary>
        /// 网站BLL
        /// </summary>
        BLLJIMP.BLLWebSite bll = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// 活动文章BLL
        /// </summary>
        BLLJIMP.BLLJuActivity bllJuactivity = new BLLJIMP.BLLJuActivity();
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLJIMP.BLLActivity bllActivity = new BLLJIMP.BLLActivity("");
        /// <summary>
        /// 网站 BLL
        /// </summary>
        BLLJIMP.BLLWebSite bllWebsite = new BLLJIMP.BLLWebSite();
        /// <summary>
        /// 网站BLL
        /// </summary>
        ZentCloud.BLLJIMP.BLLCompanyWebSite bllCompanyWebSite = new ZentCloud.BLLJIMP.BLLCompanyWebSite();
        /// <summary>
        /// 
        /// </summary>
        ZentCloud.BLLJIMP.BLLSlide bllSlide = new ZentCloud.BLLJIMP.BLLSlide();
        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Expires = 0;
            //string result = "false";
            //try
            //{

            //    string action = context.Request["Action"];
            //    switch (action)
            //    {

            //        case "getprojectorlist":
            //            result = GetProjectorList(context);
            //            break;
            //        case "getnavigatelist":
            //            result = GetNavigateList(context);
            //            break;
            //        case "gettoolbarlist":
            //            result = GetToolBarList(context);
            //            break;
            //        case "getarticlelist":
            //            result = GetArticleList(context);
            //            break;
            //        case "getarticlelistv1":
            //            result = GetArticleListV1(context);
            //            break;
            //        case "getsinglearticle":
            //            result = GetSingleArticle(context);
            //            break;
            //        case "getconfig":
            //            result = GetConfig(context);
            //            break;


            //    }
            //}
            //catch (Exception ex)
            //{
            //    resp.Status = -1;
            //    resp.Msg = ex.Message;
            //    result = Common.JSONHelper.ObjectToJson(resp);

            //}

            //context.Response.Write(result);

            context.Response.ContentType = "application/json";
            context.Response.Expires = 0;
            string result = "false";
            try
            {
                string action = context.Request["action"];
                //利用反射找到未知的调用的方法
                if (!string.IsNullOrEmpty(action))
                {
                    MethodInfo method = this.GetType().GetMethod(action, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase); //找到方法BindingFlags.NonPublic指定搜索非公有方法 

                    if (method == null)
                    {
                        resp.Status = -1;
                        resp.Msg = "action not exist";
                        result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                        context.Response.Write(result);
                        return;

                    }
                    result = Convert.ToString(method.Invoke(this, new[] { context }));  //调用方法
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "action not exist";
                    result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
                }
            }
            catch (Exception ex)
            {
                resp.Status = -1;
                resp.Msg = ex.Message;
                result = ZentCloud.Common.JSONHelper.ObjectToJson(resp);
            }
            if (!string.IsNullOrEmpty(context.Request["callback"]))
            {
                //返回 jsonp数据
                result = string.Format("{0}({1})", context.Request["callback"], result);

            }
            else
            {
                //返回json数据
            }
            context.Response.Write(result);


        }
        /// <summary>
        /// 获取幻灯片 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetProjectorList(HttpContext context)
        {
            CompanyWebsite_Config companyConfig = bllWebsite.GetCompanyWebsiteConfig();
            var dataList = bllSlide.ListByType(companyConfig.ShopAdType,bll.WebsiteOwner);
            List<CompanyWebsite_Navigate> resultList = new List<CompanyWebsite_Navigate>();
            foreach (var item in dataList)
            {
                CompanyWebsite_Navigate nav = new CompanyWebsite_Navigate();
                nav.AutoID = item.AutoID;
                nav.NavigateName = item.LinkText;
                nav.NavigateImage = bll.GetImgUrl(item.ImageUrl);
                nav.NavigateType = item.Type;
                nav.NavigateTypeValue = item.Link;
                nav.PlayIndex = item.Sort;
                resultList.Add(nav);
            }
            return Common.JSONHelper.ObjectToJson(resultList);
 



        }

        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetNavigateList(HttpContext context)
        {
            CompanyWebsite_Config companyConfig = bllWebsite.GetCompanyWebsiteConfig();
            List<CompanyWebsite_ToolBar> dataList = bllCompanyWebSite.GetToolBarList(int.MaxValue, 1, bllCompanyWebSite.WebsiteOwner, "nav", companyConfig.ShopNavGroupName, false);
            List<CompanyWebsite_Navigate> resultList = new List<CompanyWebsite_Navigate>();
            foreach (var item in dataList)
            {
                CompanyWebsite_Navigate nav = new CompanyWebsite_Navigate();
                nav.AutoID = item.AutoID;
                nav.NavigateName = item.ToolBarName;
                nav.NavigateImage = bll.GetImgUrl(item.ImageUrl);
                nav.IconClass = item.ToolBarImage;
                nav.NavigateTypeValue = ReplaceArticleUrl(context, item.ToolBarTypeValue);
                //nav.NavigateType = item.KeyType;
                nav.NavigateType = item.ToolBarType;
                nav.PlayIndex = item.PlayIndex;
                resultList.Add(nav);
            }
            return Common.JSONHelper.ObjectToJson(resultList);



        }

        /// <summary>
        /// 获取工具栏列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetToolBarList(HttpContext context)
        {
            CompanyWebsite_Config companyConfig = bllWebsite.GetCompanyWebsiteConfig();
            List<CompanyWebsite_ToolBar> dataList = bllCompanyWebSite.GetToolBarList(int.MaxValue, 1, bllCompanyWebSite.WebsiteOwner, "nav", companyConfig.BottomToolbars, false);
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].ToolBarType.Equals("电话"))
                {
                    dataList[i].ToolBarTypeValue = string.Format("tel:{0}", dataList[i].ToolBarTypeValue);
                }
                if (dataList[i].ToolBarType.Equals("短信"))
                {
                    dataList[i].ToolBarTypeValue = string.Format("sms:{0}", dataList[i].ToolBarTypeValue);
                }
                dataList[i].ToolBarTypeValue = ReplaceArticleUrl(context, dataList[i].ToolBarTypeValue);

            }
            return Common.JSONHelper.ObjectToJson(dataList);



        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetArticleList(HttpContext context)
        {
            var dataList = bll.GetArticleList(context.Request["cateid"], context.Request["name"]);
            dataList = dataList.Where(p => p.IsHide.Equals(0) && p.IsDelete.Equals(0)).ToList();
            List<ArticleModel> list = new List<ArticleModel>();
            string host = string.Format("http://{0}", context.Request.Url.Authority);
            foreach (var source in dataList)
            {
                ArticleModel model = new ArticleModel();
                model.ArticleTitle = source.ActivityName;
                model.ArticleContent = source.Summary;
                model.ArticleThumbnails = bll.GetImgUrl(source.ThumbnailsPath);
                model.ArticleUrl = string.Format("{0}/{1}/details.chtml", host, source.JuActivityIDHex);
                model.Pv = source.PV;
                model.Time = source.CreateDate.ToString("yyyy-MM-dd");
                list.Add(model);


            }
            return Common.JSONHelper.ObjectToJson(list);



        }

        /// <summary>
        /// 获取文章列表 分类或最终的详情链接
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetArticleListV1(HttpContext context)
        {
            string host = string.Format("http://{0}", context.Request.Url.Authority);
            string cateId = context.Request["cateid"];
            string name = context.Request["name"];
            List<ArticleModel> list = new List<ArticleModel>();
            List<ArticleCategory> subCategoryList = new List<ArticleCategory>();// 下级分类列表
            subCategoryList = bll.GetList<ArticleCategory>(string.Format(" WebsiteOwner='{0}' And PreID={1}", bll.WebsiteOwner, cateId));
            //先检查有没有下级分类
            if (subCategoryList.Count > 0)//有下级分类输出下级分类列表
            {
                foreach (var category in subCategoryList)
                {
                    ArticleModel model = new ArticleModel();
                    model.ArticleTitle = category.CategoryName;
                    model.ArticleThumbnails = "/web/defaultcategory.jpg";
                    model.ArticleContent = "";
                    model.ArticleUrl = string.Format("/web/list.aspx?cateid={0}", category.AutoID);
                    list.Add(model);
                }
            }
            else
            {
                var sourceDataList = bll.GetArticleListV1(context.Request["cateid"], context.Request["name"]);
                sourceDataList = sourceDataList.Where(p => p.IsHide.Equals(0) && p.IsDelete.Equals(0)).ToList();
                foreach (var source in sourceDataList)
                {
                    ArticleModel model = new ArticleModel();
                    model.ArticleTitle = source.ActivityName;
                    model.ArticleContent = source.Summary;
                    model.ArticleThumbnails = bll.GetImgUrl(source.ThumbnailsPath);
                    model.ArticleUrl = string.Format("{0}/{1}/details.chtml", host, source.JuActivityIDHex);
                    list.Add(model);
                }

            }


            return Common.JSONHelper.ObjectToJson(list);



        }


        /// <summary>
        /// 获取篇文章内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSingleArticle(HttpContext context)
        {
            var source = bllJuactivity.GetJuActivity(Convert.ToInt32(context.Request["articleid"], 16));
            string host = string.Format("http://{0}", context.Request.Url.Authority);
            ArticleModel model = new ArticleModel();
            model.ArticleTitle = source.ActivityName;
            model.ArticleContent = source.ActivityDescription;

            #region 报名表单
            try
            {
                //加载报名表单
                if ((!string.IsNullOrEmpty(source.SignUpActivityID)) && (int.Parse(source.SignUpActivityID) > 0))
                {
                    //当前登录信息
                    ZentCloud.BLLJIMP.Model.UserInfo currUserInfo = new BLLJIMP.Model.UserInfo();
                    if (bll.IsLogin)
                    {
                        currUserInfo = DataLoadTool.GetCurrUserModel();
                    }

                    System.Text.StringBuilder sbAppend = new System.Text.StringBuilder();
                    sbAppend.AppendLine("<link type=\"text/css\" rel=\"stylesheet\" href=\"/css/buttons.css\"/>");
                    sbAppend.AppendLine(" <style type=\"text/css\">input[type='text'],textarea{height:30px;width:100%;border-radius: 2px;margin-top:5px;}</style>");
                    sbAppend.AppendLine("<script src=\"/Scripts/jquery.form.js\" type=\"text/javascript\"></script>");
                    sbAppend.AppendLine("<form id=\"formsignin\">");

                    var mapList = bllActivity.GetActivityFieldMappingList(source.SignUpActivityID);
                    foreach (var item in mapList)
                    {
                        if (item.FieldName.Equals("Name"))
                        {
                            sbAppend.AppendLine(string.Format("<input  placeholder=\"姓名\" name=\"Name\"  id=\"txtName\" type=\"text\" value=\"{0}\">", currUserInfo.TrueName));

                        }
                        else if (item.FieldName.Equals("Phone"))
                        {
                            sbAppend.AppendLine(string.Format("<input  placeholder=\"手机\" name=\"Phone\"  id=\"txtPhone\" type=\"text\" value=\"{0}\">", currUserInfo.Phone));
                        }
                        else
                        {
                            if (item.IsMultiline.Equals(1))
                            {
                                sbAppend.AppendLine(string.Format("<textarea  placeholder=\"{0}\" name=\"{1}\" style=\"height:50px;\" ></textarea>", item.MappingName, "K" + item.ExFieldIndex.ToString()));

                            }
                            else
                            {

                                if (item.MappingName.Contains("公司"))
                                {
                                    sbAppend.AppendLine(string.Format("<input  placeholder=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\">", item.MappingName, "K" + item.ExFieldIndex.ToString(), currUserInfo.Company));

                                }
                                else if (item.MappingName.Contains("职位") || item.MappingName.Contains("职务"))
                                {
                                    sbAppend.AppendLine(string.Format("<input  placeholder=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\">", item.MappingName, "K" + item.ExFieldIndex.ToString(), currUserInfo.Postion));
                                }
                                else if (item.MappingName.Contains("邮箱") || item.MappingName.Contains("邮件") || item.MappingName.ToLower().Contains("email"))
                                {
                                    sbAppend.AppendLine(string.Format("<input  placeholder=\"{0}\" name=\"{1}\" type=\"text\" value=\"{2}\">", item.MappingName, "K" + item.ExFieldIndex.ToString(), currUserInfo.Email));
                                }
                                else
                                {
                                    sbAppend.AppendLine(string.Format("<input  placeholder=\"{0}\" name=\"{1}\" type=\"text\">", item.MappingName, "K" + item.ExFieldIndex.ToString()));

                                }


                            }

                        }

                    }

                    sbAppend.AppendLine("<span class=\"button button-rounded button-flat-action\" style=\"width:86%;margin-top:10px;\"  onclick=\"SumitData()\" >提交</span>");
                    sbAppend.AppendLine(string.Format("<input  type=\"hidden\" value=\"{0}\" name=\"ActivityID\">", source.SignUpActivityID));
                    BLLJIMP.Model.UserInfo userInfo = bll.Get<BLLJIMP.Model.UserInfo>(string.Format(" UserId='{0}'", source.UserID));
                    sbAppend.AppendLine(string.Format("<input id=\"loginName\" type=\"hidden\" value=\"{0}\" name=\"LoginName\" />", ZentCloud.Common.Base64Change.EncodeBase64ByUTF8(userInfo.UserID)));//外部登录名
                    sbAppend.AppendLine(string.Format("<input id=\"loginPwd\" type=\"hidden\" value=\"{0}\" name=\"LoginPwd\" />", ZentCloud.Common.DEncrypt.ZCEncrypt(userInfo.Password)));//外部登录密码
                    sbAppend.AppendLine("</form>");
                    //
                    sbAppend.AppendLine("<script type=\"text/javascript\">");
                    sbAppend.AppendLine("function SumitData() {");
                    sbAppend.AppendLine("var Name = $(\"#txtName\").val();");
                    sbAppend.AppendLine("var Phone = $(\"#txtPhone\").val();");
                    sbAppend.AppendLine("if (Name == \"\" || (Phone == \"\")) {alert(\"请输入姓名、手机号码\");return false; }");

                    sbAppend.AppendLine("$(\"#formsignin\").ajaxSubmit({");
                    sbAppend.AppendLine("url: \"/serv/ActivityApiJson.ashx\",");
                    sbAppend.AppendLine("type: \"post\",");
                    sbAppend.AppendLine("dataType: \"json\",");
                    sbAppend.AppendLine("success: function (resp) {");
                    sbAppend.AppendLine("if (resp.Status == 0) {//清空");
                    sbAppend.AppendLine(" $('input:text').val(\"\");");
                    sbAppend.AppendLine("$('textarea').val(\"\");");
                    sbAppend.AppendLine("alert(\"提交成功!\");");
                    sbAppend.AppendLine("return;");
                    sbAppend.AppendLine("}");
                    sbAppend.AppendLine("else if (resp.Status == 1) {alert(\"重复提交!\");}");
                    sbAppend.AppendLine(" else {alert(resp.Msg);}");
                    sbAppend.AppendLine("}});return false;  };");
                    sbAppend.AppendLine("</script>");


                    model.ArticleContent += sbAppend.ToString();

                }
            }
            catch (Exception)
            {


            }
            //加载报名表单 
            #endregion

            model.ArticleThumbnails = bll.GetImgUrl(source.ThumbnailsPath);
            //model.ArticleUrl = string.Format("{0}/{1}/details.chtml", host, source.JuActivityIDHex);
            return Common.JSONHelper.ObjectToJson(model);

        }


        /// <summary>
        /// 获取网站配置
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetConfig(HttpContext context)
        {
            //var host = string.Format("http://{0}", context.Request.Url.Authority);
            var config = bll.GetConfig();
            WebsiteConfigModel apiConfig = new WebsiteConfigModel();
            if (config != null)
            {
                apiConfig.WebsiteTitle = config.WebsiteTitle;
                apiConfig.WebsiteDescription = config.WebsiteDescription;
                apiConfig.WebsiteImage = bll.GetImgUrl(config.WebsiteImage);
                apiConfig.Copyright = config.Copyright;
                apiConfig.ShopNavGroupName = config.ShopNavGroupName;
                apiConfig.ShopAdType = config.ShopAdType;
                apiConfig.BottomToolbars = config.BottomToolbars;

            }
            return Common.JSONHelper.ObjectToJson(apiConfig);

        }


        /// <summary>
        /// 文章接口api
        /// </summary>
        public class ArticleModel
        {

            /// <summary>
            /// 链接
            /// </summary>
            public string ArticleUrl { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string ArticleTitle { get; set; }
            /// <summary>
            /// 内容
            /// </summary>
            public string ArticleContent { get; set; }
            /// <summary>
            /// 缩略图
            /// </summary>
            public string ArticleThumbnails { get; set; }
            /// <summary>
            /// pv
            /// </summary>
            public int Pv { get; set; }
            /// <summary>
            /// time
            /// </summary>
            public string Time { get; set; }

        }

        /// <summary>
        /// 网站配置api
        /// </summary>
        public class WebsiteConfigModel
        {

            /// <summary>
            /// 网站标题
            /// </summary>
            public string WebsiteTitle { get; set; }
            /// <summary>
            /// 版权
            /// </summary>
            public string Copyright { get; set; }

            /// <summary>
            /// 网站图片
            /// </summary>
            public string WebsiteImage { get; set; }

            /// <summary>
            /// 网站描述
            /// </summary>
            public string WebsiteDescription { get; set; }
            /// <summary>
            /// 导航分组名称
            /// </summary>
            public string ShopNavGroupName { get; set; }

            /// <summary>
            /// 首页广告设置
            /// </summary>
            public string ShopAdType { get; set; }

            /// <summary>
            /// 底部工具栏
            /// </summary>
            public string BottomToolbars { get; set; }


        }


        /// <summary>
        /// 清除所有Html标记，返回纯文本
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        private static string ClearHtml(string strHtml, int subLength = 0)
        {
            if (!string.IsNullOrEmpty(strHtml))
            {
                System.Text.RegularExpressions.Regex r = null;
                System.Text.RegularExpressions.Match m = null;

                r = new System.Text.RegularExpressions.Regex(@"<\/?[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                for (m = r.Match(strHtml); m.Success; m = m.NextMatch())
                {
                    strHtml = strHtml.Replace(m.Groups[0].ToString(), null);
                }
            }
            if (!string.IsNullOrEmpty(strHtml))
            {
                strHtml = strHtml.Replace("\n", null).Replace("\t", null).Replace("\r", null);
                if (strHtml.Contains("&nbsp;"))
                {
                    strHtml = strHtml.Replace("&nbsp;", null);
                }
                if (subLength != 0)
                {
                    if (strHtml.Length > subLength)
                    {
                        return string.Format("{0}...", strHtml.Substring(0, subLength));
                    }
                    else
                    {
                        return strHtml.Trim();
                    }
                }
                else
                {
                    return strHtml.Trim();
                }


            }
            return "";


        }

        /// <summary>
        /// 替换特定链接为模板链接
        /// </summary>
        /// <param name="sourceUrl">源链接</param>
        /// <returns></returns>
        private string ReplaceArticleUrl(HttpContext context, string sourceUrl)
        {

            try
            {
                if (sourceUrl.StartsWith("http://") && (sourceUrl.EndsWith(".chtml")))
                {
                    string articleId = sourceUrl.Split('/')[sourceUrl.Split('/').Length - 2];

                    return string.Format("http://{0}/web/detail.aspx?articleid={1}", context.Request.Url.Authority, articleId);

                }
                else
                {
                    return sourceUrl;
                }

            }
            catch
            {
                return sourceUrl;

            }


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