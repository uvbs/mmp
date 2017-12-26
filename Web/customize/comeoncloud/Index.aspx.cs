using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model.API.Mall;

namespace ZentCloud.JubitIMP.Web.customize.comeoncloud
{
    public partial class Index : System.Web.UI.Page
    {
        BLLJIMP.BLLComponent bll = new BLLJIMP.BLLComponent();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        BLLJIMP.BLLArticleCategory bllCate = new BLLJIMP.BLLArticleCategory();
        BLLJIMP.BLLCompanyWebSite bllToolbar = new BLLJIMP.BLLCompanyWebSite();
        BLLJIMP.BLLMeifan bllMeifan = new BLLMeifan();
        BLLWebSite bllWebsite = new BLLWebSite();

        UserInfo curUser = new UserInfo();

        //加载首屏文章
        Web.Serv.pubapi p = new Serv.pubapi();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["cgid"] == null && Request["cateid"] == null && Request["ngroute"] == null && Request["key"] == null)
            {
                this.Response.Write("参数错误");
                return;
            }
            if (!bll.IsLogin)
            {
                Response.Redirect("/Error/CommonMsg.aspx?msg=请先配置微信接入");
            }

            curUser = bllUser.GetUserInfo(bllUser.GetCurrUserID(), bllUser.WebsiteOwner);

            WebsiteInfo websiteInfo = bllWebsite.GetWebsiteInfoModelFromDataBase();
            CompanyWebsite_Config nWebsiteConfig = bllWebsite.GetCompanyWebsiteConfig();

            var cgid = Request["cgid"];
            var preid = Request["cateid"] == null ? "" : Request["cateid"];
            var mallcate = Request["mallcate"] == null ? "" : Request["mallcate"];
            var keyword = Request["keyword"] == null ? "" : Request["keyword"];
            var ngroute = Request["ngroute"];
            var key = Request["key"];

            if (websiteInfo.WebsiteOwner == "jikuwifi" && key.Equals("MallHome", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("/customize/jikuwifi/?v=1.0&ngroute=/jikuhome#/jikuhome",true);
                Response.End();
                return;
            }


            if (websiteInfo.WebsiteOwner == "meifan" && key.Equals("MallHome", StringComparison.OrdinalIgnoreCase) && (curUser == null || string.IsNullOrEmpty(curUser.Phone)))
            {
                Response.Redirect("/customize/shop/index.aspx?v=1.0#/bindPhone/", true);
                Response.End();
                return;
            }

            if (!string.IsNullOrWhiteSpace(key) && key.Equals("PersonalCenter",StringComparison.OrdinalIgnoreCase))
            {
                if (websiteInfo.IsUnionHongware == 1)
                {
                    #region 宏巍个人中心处理
                    //string redirUrl = "";

                    //if (bll.IsLogin)
                    //{
                    //    //绑定了宏巍，则跳到宏巍商城地址
                    //    var appId = websiteInfo.WeixinAppId;
                    //    var orgCode = websiteInfo.OrgCode;
                    //    var openId = curUser.WXOpenId;
                    //    var hwurl = Common.ConfigHelper.GetConfigString("HongWareUserCenterUrl");

                    //    var hwReqData = new
                    //    {
                    //        openID = openId,
                    //        appID = appId,
                    //        orgCode = orgCode
                    //    };

                    //    var jsonStr = JsonConvert.SerializeObject(hwReqData);
                        
                    //    redirUrl = string.Format("{0}?action={1}", hwurl, Common.Base64Change.EncodeBase64(jsonStr));

                    //}
                    //else
                    //{
                    //    if (websiteInfo.WebsiteOwner == "hongwei")
                    //    {
                    //        redirUrl = "http://o2oswapi.hongware.com/hfive/2015/membercenter.html?action=eyJvcGVuSUQiOiJvWFY5cnVBSFNCSzlTaUFOOTBCNlM5MGRIaGxZIiwiYXBwSUQiOiJ3eDZlMGNkYmJlZjM5YWM0ZmMiLCJvcmdDb2RlIjoidGVzdCJ9";
                    //    }
                    //}
                    //if (!string.IsNullOrWhiteSpace(redirUrl))
                    //{
                    //    Response.Redirect(redirUrl);
                    //    Response.End();
                    //    return;
                    //} 
                    #endregion
                }
                else if (!string.IsNullOrWhiteSpace(nWebsiteConfig.PersonalCenterLink))
                {
                    Response.Redirect(nWebsiteConfig.PersonalCenterLink);
                    Response.End();
                    return;
                }

                if (!bll.IsLogin)
                {
                    var websiteWXUserRegType = bllWebsite.GetWebsiteWXUserRegType(websiteInfo.WebsiteOwner);

                    if (websiteWXUserRegType == BLLJIMP.Enums.WebsiteWXUserRegType.ManualRegAfterOperate)
                    {
                        Response.Redirect("/customize/shop/?v=1.0&ngroute=/bindPhone/#/bindPhone/1");
                        Response.End();
                        return;
                    }

                }
            }

            #region 读取组件 模板

            //读取配置

            if (ngroute != null)
            {
                var routes = ngroute.Split('/');
                if (string.IsNullOrWhiteSpace(cgid) && routes.Length > 2) cgid = routes[2];
                if (string.IsNullOrWhiteSpace(preid) && routes.Length > 3) cgid = routes[3];
            }

            //替换配置
            Component model = new Component();
            if (!string.IsNullOrWhiteSpace(key))
            {
                model = bll.GetComponentByKey(key, bll.WebsiteOwner);
            }
            else
            {
                model = bll.Get<BLLJIMP.Model.Component>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}", bll.WebsiteOwner, cgid));
            }
            if (model == null)
            {
                this.Response.Write("组件不存在");
                return;
            }
            #region 检查页面访问权限
            if (model.AccessLevel > 0)
            {
                string noPmsUrl = "/Error/NoPmsMobile.htm";
                if (curUser == null)
                {
                    this.Response.Redirect(noPmsUrl);
                    return;
                }
                if (curUser.AccessLevel < model.AccessLevel)
                {
                    if (bllUser.IsMember())
                    {
                        this.Response.Redirect(noPmsUrl);
                        return;
                    }
                    else
                    {
                        if (nWebsiteConfig == null || nWebsiteConfig.MemberStandard == 0)
                        {
                            this.Response.Redirect(noPmsUrl);
                            return;
                        }
                        if (nWebsiteConfig.MemberStandard == 1)
                        {
                            this.Response.Redirect("/App/Member/Wap/PhoneVerify.aspx?referrer=" + HttpUtility.UrlEncode(this.Request.Url.ToString()));
                        }
                        else
                        {
                            this.Response.Redirect("/App/Member/Wap/CompleteUserInfo.aspx?referrer=" + HttpUtility.UrlEncode(this.Request.Url.ToString()));
                        }
                        return;
                    }
                }
            }
            #endregion
            ComponentModel cmodel = bll.GetByKey<ComponentModel>("AutoId", model.ComponentModelId.ToString());
            if (cmodel == null || string.IsNullOrWhiteSpace(cmodel.ComponentModelHtmlUrl))
            {
                this.Response.Write("页面不存在");
                return;
            }
            string cmodelPath = this.Server.MapPath(cmodel.ComponentModelHtmlUrl);
            if (!File.Exists(cmodelPath))
            {
                this.Response.Write("页面不存在");
                return;
            }
            string indexStr = File.ReadAllText(cmodelPath);

            #endregion

            #region 模板内容处理

            string objecStr = @"</html><script></script>";
            //加载分类数据和文章首页数据
            JObject jobject = JObject.Parse(model.ComponentConfig);
            #region 广告和导航 原
            //加载广告和导航
            List<JProperty> listSlideProperty = jobject.Properties().Where(p => p.Name.StartsWith("slide_list") || p.Name.StartsWith("foottool_list")
                || p.Name.StartsWith("tab_list") || (p.Name.StartsWith("button_list") && !p.Name.EndsWith("style"))).ToList();
            foreach (JProperty item in listSlideProperty)
            {
                if (cmodel.AutoId>10 && (item.Name.StartsWith("foottool_list") || item.Name.StartsWith("tab_list")))
                {
                    if (item.Value.Type == JTokenType.Array)
                    {
                        JToken nObject = item.Value;
                        jobject[item.Name] = new JObject();
                        jobject[item.Name]["list"] = nObject;
                    }
                    else if (item.Value.Type == JTokenType.Object)
                    {
                        JProperty nPro = new JProperty(item.Name, item.Value["key_type"]);
                        JToken nObject = new JObject();
                        GetChildConfig(nPro, ref nObject);
                        jobject[item.Name]["list"] = nObject;
                    }
                    else if (item.Value.Type == JTokenType.String || item.Value.Type == JTokenType.Integer)
                    {
                        JToken nObject = new JObject();
                        GetChildConfig(item, ref nObject);
                        jobject[item.Name] = new JObject();
                        jobject[item.Name]["list"] = nObject;
                    }
                }
                else
                {
                    if (item.Value.Type == JTokenType.String || item.Value.Type == JTokenType.Integer)
                    {
                        JToken childJObject = jobject[item.Name];
                        GetChildConfig(item, ref childJObject);
                        jobject[item.Name] = childJObject;
                    }
                }
            }
            #endregion

            #region 广告和导航
            if (model.IsInitData == 1)
            {
                List<JProperty> listNavsProperty = jobject.Properties().Where(p => p.Name.StartsWith("navs")).ToList();
                foreach (JProperty item in listNavsProperty)
                {
                    if (item.Value.Type != JTokenType.Array) break;
                    JArray childJArray = JArray.FromObject(jobject[item.Name]);
                    bool hasGetData = false;
                    foreach (JObject nJArray in childJArray)
                    {
                        List<JProperty> listNavProperty = nJArray.Properties().Where(p => p.Name.StartsWith("nav_list")).ToList();
                        foreach (JProperty cproperty in listNavProperty)
                        {
                            if (cproperty.Value.Type == JTokenType.String)
                            {
                                JToken childJObject = nJArray[cproperty.Name];
                                GetChildConfig(cproperty, ref childJObject);
                                nJArray[cproperty.Name] = childJObject;
                                hasGetData = true;
                            }
                        }
                    }
                    if (hasGetData) jobject[item.Name] = childJArray;
                }
                List<JProperty> listSlidesProperty = jobject.Properties().Where(p => p.Name.StartsWith("slides")).ToList();
                foreach (JProperty item in listSlidesProperty)
                {
                    if (item.Value.Type == JTokenType.Array)
                    {
                        JArray childJArray = JArray.FromObject(item.Value);
                        bool hasGetData = false;
                        foreach (JObject nJArray in childJArray)
                        {
                            List<JProperty> listNavProperty = nJArray.Properties().Where(p => p.Name.StartsWith("slide_list")).ToList();
                            foreach (JProperty cproperty in listNavProperty)
                            {
                                if (cproperty.Value.Type == JTokenType.String)
                                {
                                    JToken childJObject = nJArray[cproperty.Name];
                                    GetChildConfig(cproperty, ref childJObject);
                                    nJArray[cproperty.Name] = childJObject;

                                    hasGetData = true;
                                }
                            }
                        }
                        if (hasGetData) jobject[item.Name] = childJArray;
                    }
                }
            }
            #endregion
            
            #region 头部搜索栏
            //头部搜索栏
            if (model.IsInitData == 1)
            {
                List<JProperty> listHeadSearchProperty = jobject.Properties().Where(p => p.Name.StartsWith("headsearch")).ToList();
                foreach (JProperty item in listHeadSearchProperty)
                {
                    if (item.Value.Type == JTokenType.Array)
                    {
                        JArray childJArray = JArray.FromObject(item.Value);
                        foreach (JObject nJArray in childJArray)
                        {
                            if (!string.IsNullOrWhiteSpace(keyword)) nJArray["keyword"] = keyword;
                            string nav_left_type = nJArray["nav_left_type"].ToString();
                            string nav_left = nJArray["nav_left"].ToString();
                            if(nav_left_type == "1" && !string.IsNullOrWhiteSpace(nav_left)){
                                JProperty cproperty = new JProperty("nav_list",nav_left);
                                JToken childJObject = new JObject();
                                GetChildConfig(cproperty, ref childJObject);
                                nJArray["left_list"] = childJObject;
                            }
                            string nav_right = nJArray["nav_right"].ToString();
                            if(!string.IsNullOrWhiteSpace(nav_right)){
                                JProperty cproperty = new JProperty("nav_list",nav_right);
                                JToken childJObject = new JObject();
                                GetChildConfig(cproperty, ref childJObject);
                                nJArray["right_list"] = childJObject;
                            }
                        }
                        jobject[item.Name] = childJArray;
                    }
                }
            }
            #endregion
            #region 商品列表
            if (model.IsInitData == 1)
            {
                List<JProperty> listmallsProperty = jobject.Properties().Where(p => p.Name.StartsWith("malls")).ToList();
                foreach (JProperty item in listmallsProperty)
                {
                    if (item.Value.Type != JTokenType.Array) break;
                    JArray childJArray = JArray.FromObject(jobject[item.Name]);
                    foreach (JObject nJArray in childJArray)
                    {
                        string cate = nJArray["cate"] == null ? "" : nJArray["cate"].ToString();
                        string tag = nJArray["tag"] == null ? "" : nJArray["tag"].ToString();
                        string sort = nJArray["sort"] == null ? "" : nJArray["sort"].ToString();
                        string sort_tag = nJArray["sort_tag"] == null ? "" : nJArray["sort_tag"].ToString();
                        string count = nJArray["count"] == null ? "6" : nJArray["count"].ToString();
                        string is_group_buy = nJArray["is_group_buy"] == null ? "0" : nJArray["is_group_buy"].ToString();
                        
                        int total = 0;
                        if (!string.IsNullOrWhiteSpace(mallcate))
                        {
                            cate = mallcate;
                            nJArray["cate"] = mallcate;
                        }
                        if (is_group_buy == "3")
                        {
                            nJArray["mall_list"] = GetOrderMallJTokenList(count, out total, keyword);
                        }
                        else
                        {
                            string supplierId = curUser.BindId;
                            if (string.IsNullOrEmpty(supplierId))
                            {
                                supplierId = "";
                            }
                            string type = "";
                            if (is_group_buy == "4") type = "Houses";
                            nJArray["mall_list"] = GetMallJTokenList(cate, tag, is_group_buy, sort, count, out total, keyword, sort_tag, "", "", "", type,supplierId);
                        }
                        nJArray["mall_total"] = total;
                    }
                    jobject[item.Name] = childJArray;
                }

                List<JProperty> listgoodsProperty = jobject.Properties().Where(p => p.Name.StartsWith("goods")).ToList();
                foreach (JProperty item in listgoodsProperty)
                {
                    if (item.Value.Type != JTokenType.Array) break;
                    JArray childJArray = JArray.FromObject(jobject[item.Name]);
                    foreach (JObject nJArray in childJArray)
                    {
                        string productIds = nJArray["ids"] == null ? "" : nJArray["ids"].ToString();
                        string sortIds = "1";
                        string canRepeat = "1";
                        int total = 0;
                        nJArray["good_list"] = GetMallJTokenList("", "", "", "", "999", out total, "", "", productIds, sortIds, canRepeat);
                    }
                    jobject[item.Name] = childJArray;
                }
            }
            #endregion
            
            #region 搜索栏
            //搜索栏
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                List<JProperty> listSearchProperty = jobject.Properties().Where(p => p.Name.StartsWith("searchbox")).ToList();
                foreach (JProperty item in listSearchProperty)
                {
                    jobject[item.Name]["keyword"] = keyword;
                }
            }
            #endregion
            #region 侧边栏
            //侧边栏
            if (jobject["sidemenubox"] != null)
            {
                jobject["sidemenubox"] = GetSidemenu(jobject["sidemenubox"]);
            }
            #endregion

            #region 文章列表
            bool hasActivityForward = false;
            if (model.IsInitData == 1)
            {
                List<JProperty> listcardsProperty = jobject.Properties().Where(p => p.Name.StartsWith("cards")).ToList();
                foreach (JProperty item in listcardsProperty)
                {
                    if (item.Value.Type != JTokenType.Array) break;
                    JArray childJArray = JArray.FromObject(jobject[item.Name]);
                    foreach (JObject nJArray in childJArray)
                    {
                        string cate = nJArray["cate_id"] == null ? "" : nJArray["cate_id"].ToString();
                        int rows = nJArray["rows"] == null ? 10 : int.Parse(nJArray["rows"].ToString());
                        string cateType = "Article";
                        if (!string.IsNullOrWhiteSpace(preid))
                        {
                            cate = preid;
                            nJArray["cate_id"] = preid;
                        }
                        int page = 1;
                        nJArray["rows"] = rows;
                        nJArray["page"] = page;

                        dynamic artData = new List<int>();

                        string data_type = "0";
                        if (nJArray["data_type"] != null)
                        {
                            data_type = nJArray["data_type"].ToString();
                        }
                        else
                        {
                            nJArray["data_type"] = "0";
                        }
                        //检查页面是否存在微吸粉
                        artData = p.GetArticleList(page, rows, 0, 0, cate, keyword, "", "", "", "", cateType, "", "",
                        "JuActivityID,ActivityName,Summary,CreateDate,CategoryId,ThumbnailsPath,PV,Tags,RedirectUrl",
                        false, false,data_type == "1");

                        nJArray["card_data"] = JToken.FromObject(artData);
                    }
                    jobject[item.Name] = childJArray;
                }
            }
            //设置分销上级 
            //if (hasActivityForward && curUser !=null && websiteInfo.IsNeedDistributionRecommendCode == 0)
            //{
            //    if (string.IsNullOrWhiteSpace(curUser.DistributionOwner) && curUser.UserID != bllUser.WebsiteOwner)
            //    {
            //        bllUser.Update(new BLLJIMP.Model.UserInfo(), string.Format(" DistributionOwner = '{0}' ", bllUser.WebsiteOwner), string.Format(" AutoID = {0} ", curUser.AutoID));
            //    }
            //}
            #endregion
            #region 活动列表
            if (model.IsInitData == 1)
            {
                List<JProperty> listActivitysProperty = jobject.Properties().Where(p => p.Name.StartsWith("activitys")).ToList();
                foreach (JProperty item in listActivitysProperty)
                {
                    if (item.Value.Type != JTokenType.Array) break;
                    JArray childJArray = JArray.FromObject(jobject[item.Name]);
                    foreach (JObject nJArray in childJArray)
                    {
                        string cate = nJArray["cate_id"] == null ? "" : nJArray["cate_id"].ToString();
                        string sort = nJArray["sort"] == null ? "" : nJArray["sort"].ToString();
                        if (!string.IsNullOrWhiteSpace(preid))
                        {
                            cate = preid;
                            nJArray["cate_id"] = cate;
                        }
                        int rows = 5;
                        int page = 1;
                        nJArray["rows"] = rows;
                        nJArray["page"] = page;
                        string data_type = "0";

                        //检查页面是否存在微转发
                        if (nJArray["data_type"] != null)
                        {
                            data_type = nJArray["data_type"].ToString();
                        }
                        else
                        {
                            nJArray["data_type"] = "0";
                        }

                        Serv.API.Activity.List li = new Serv.API.Activity.List();
                        int actTotal = 0;
                        var artData = li.GetActivityData(rows, page, cate, keyword, false, sort, out actTotal,
                            "JuActivityID,ActivityName,ActivityStartDate,ActivityEndDate,CreateDate,ActivityAddress,CategoryId,ThumbnailsPath,PV,SignUpCount,Tags,RedirectUrl,ActivityIntegral,IsHide,SignUpActivityID,MaxSignUpTotalCount", 
                            data_type == "1");
                        nJArray["total"] = actTotal;
                        nJArray["activity_list"] = JToken.FromObject(artData);
                    }
                    jobject[item.Name] = childJArray;
                }
            }
            #endregion
            #region 组件
            //加载组件
            List<JProperty> listComponentProperty = jobject.Properties().Where(p => p.Name.StartsWith("component_")).ToList();
            foreach (JProperty item in listComponentProperty)
            {
                GetComponentConfig(item, ref jobject, 1);
            }
            #endregion

            #region 首屏 分类列表 文章数据
            ArticleCategory nCate = new ArticleCategory();
            if (model.IsInitData == 1 && model.ComponentModelId < 10)
            {
                var data_preid = jobject["data_preid"].ToString();
                if (!string.IsNullOrWhiteSpace(preid)) data_preid = preid;
                int cateId = 0;
                int.TryParse(data_preid, out cateId);
                if (cateId != 0)
                {
                    //有分类id了，则读取分类数据和首屏文章数据了
                    int totalCount = 0;
                    string cateType = "Article";
                    var dataList = bllCate.GetCateList(out totalCount, cateType, cateId, this.bll.WebsiteOwner);
                    //读出当前分类
                    nCate = dataList.FirstOrDefault(cp => cp.AutoID == cateId);

                    List<dynamic> cateResult = new List<dynamic>();
                    foreach (var item in dataList)
                    {
                        cateResult.Add(new
                        {
                            id = item.AutoID,
                            name = item.CategoryName,
                            createTime = item.CreateTime.ToString(),
                            img = item.ImgSrc,
                            summary = item.Summary
                        });
                    }

                    objecStr = objecStr.Replace("</script>", "var pageCateList=" + JsonConvert.SerializeObject(cateResult) + ";</script>");

                    int rows = 20;
                    int page = 1;

                    var articleData = p.GetArticleList(page, rows, 0, 0, cateId.ToString(), keyword, "", "", "", "", "", "", "");
                    objecStr = objecStr.Replace("</script>", "var pageFirstArticle=" + JsonConvert.SerializeObject(articleData) + ";</script>");
                }
            }
            #endregion
            #region 页面标题
            if (jobject["pageinfo"] != null && jobject["pageinfo"]["title"] != null && !string.IsNullOrWhiteSpace(jobject["pageinfo"]["title"].ToString()))
            {
                indexStr = indexStr.Replace("</title>", jobject["pageinfo"]["title"].ToString() + "</title>");
            }
            else if (model.ComponentKey == "MallHome" && nWebsiteConfig != null)
            {
                indexStr = indexStr.Replace("</title>", nWebsiteConfig.WebsiteTitle + "</title>");
            }
            else if (jobject["data_title"] != null && !string.IsNullOrWhiteSpace(jobject["data_title"].ToString()))
            {
                indexStr = indexStr.Replace("</title>", jobject["data_title"].ToString() + "</title>");
            }
            else if (nCate != null && !string.IsNullOrWhiteSpace(nCate.CategoryName))
            {
                jobject["data_title"] = nCate.CategoryName;
                indexStr = indexStr.Replace("</title>", nCate.CategoryName + "</title>");
            }
            else
            {
                jobject["data_title"] = model.ComponentName;
                indexStr = indexStr.Replace("</title>", model.ComponentName + "</title>");
            }

            #endregion
            #region 页面分享信息
            if (jobject["shareinfo"] == null) jobject["shareinfo"] = new JObject();
            if (model.ComponentKey == "MallHome" && nWebsiteConfig != null)
            {
                JObject shareinfoObj = new JObject();
                if (jobject["shareinfo"]["title"] == null || string.IsNullOrWhiteSpace(jobject["shareinfo"]["title"].ToString())) jobject["shareinfo"]["title"] = nWebsiteConfig.WebsiteTitle;
                if (jobject["shareinfo"]["desc"] == null || string.IsNullOrWhiteSpace(jobject["shareinfo"]["desc"].ToString())) jobject["shareinfo"]["desc"] = nWebsiteConfig.WebsiteDescription;
                if (jobject["shareinfo"]["img_url"] == null || string.IsNullOrWhiteSpace(jobject["shareinfo"]["img_url"].ToString())) jobject["shareinfo"]["img_url"] = nWebsiteConfig.WebsiteImage;
            }
            else
            {
                //读取组件图片
                if ((jobject["data_img"] == null || string.IsNullOrWhiteSpace(jobject["data_img"].ToString()))
                    && nCate != null && !string.IsNullOrWhiteSpace(nCate.ImgSrc))
                {
                    jobject["data_img"] = nCate.ImgSrc;
                }
                //读取组件图片
                if ((jobject["data_summary"] == null || string.IsNullOrWhiteSpace(jobject["data_summary"].ToString()))
                    && nCate != null && !string.IsNullOrWhiteSpace(nCate.Summary))
                {
                    jobject["data_summary"] = nCate.Summary;
                }
            }
            #endregion

            #region 登录用户信息
            dynamic userObject;
            if (curUser == null)
            {
                userObject = new
                {
                    islogin = false,
                    user_id = 0,
                    user_name = "",
                    nick_name = "请先登录",
                    avatar = "/img/europejobsites.png",
                    score = 0,
                    amount = 0,
                    card = 0,
                    level = ""
                };
            }
            else
            {
                UserLevelConfig levelItem = null;

                var fxLevel = new BLLDistribution().GetUserLevel(curUser);
                if (fxLevel != null)
                {
                    levelItem = bllUser.GetUserLevelByLevelNumber(fxLevel.LevelNumber, "CommonScore");
                }
                if (levelItem == null)
                {
                    levelItem = bllUser.GetUserLevelByHistoryTotalScore(curUser.HistoryTotalScore, "CommonScore");
                }
                string area = !string.IsNullOrWhiteSpace(curUser.City) ? curUser.City : curUser.Province;
                string cardNum = bllMeifan.GetMyDefualtCardNumber(curUser.UserID);
                userObject = new
                {
                    islogin = true,
                    user_id = curUser.AutoID,
                    user_name = curUser.UserID,
                    nick_name = bllUser.GetUserDispalyName(curUser),
                    true_name=curUser.TrueName,
                    avatar = bllUser.GetUserDispalyAvatar(curUser),
                    phone = curUser.Phone,
                    area = string.IsNullOrWhiteSpace(area) ? "" :area,
                    score = curUser.TotalScore,
                    amount = curUser.AccountAmount,
                    total_amount = curUser.TotalAmount,
                    lockamount = curUser.AccountAmountEstimate,
                    accumfund = curUser.AccumulationFund,
                    stock = curUser.Stock,
                    card = 0,
                    ex1 = curUser.Ex1,
                    ex2 = curUser.Ex2,
                    ex3 = curUser.Ex3,
                    ex4 = curUser.Ex4,
                    ex5 = curUser.Ex5,
                    level = levelItem == null ? "" : levelItem.LevelString,
                    cardnum = !string.IsNullOrEmpty(cardNum) ? cardNum : "",
                    supplier_id = !string.IsNullOrEmpty(curUser.BindId) ? curUser.BindId : "",
                };
            }
            objecStr = objecStr.Replace("</script>", "var loginUserInfo=" + JsonConvert.SerializeObject(userObject) + ";</script>");
            #endregion
            objecStr = objecStr.Replace("</script>", "var pageConfig=" + JsonConvert.SerializeObject(jobject) + ";var mallConfig = " + new ZentCloud.BLLJIMP.BLLKeyValueData().GetMallConfigList() + ";</script>");


            

            //输出变量到页面
            indexStr = indexStr.Replace("</html>", objecStr);

            #region 底部图标引用
            //图标文件
            string icoScript = bll.GetIcoScript();
            if (!string.IsNullOrWhiteSpace(icoScript))
            {
                indexStr += icoScript;
            }
            #endregion

            #region 输出指令到页面
            if (indexStr.Contains("<contentdirective></contentdirective>"))
            {
                List<JProperty> listProperty = jobject.Properties().Where(p => CheckPropertyName(p.Name)).ToList();
                foreach (JProperty item in listProperty)
                {
                    ReplaceDirective(ref indexStr, item.Name, cmodel.AutoId, jobject[item.Name]);
                }
                //移除占位标签
                indexStr = indexStr.Replace("<contentdirective></contentdirective>", "");
            }
            #endregion

            #endregion

            var salerId = Request["sale_id"];

            if (!string.IsNullOrWhiteSpace(salerId))
            {
                //验证saleId是否正确
                var userSaler = bllUser.GetUserInfo(salerId);

                if (userSaler != null)
                {
                    indexStr = indexStr.Replace("</body>", "<script>localStorage.setItem('sale_id','" + salerId + "');</script></body>");
                }

            }
            indexStr = indexStr.Replace("{{STOREADDRESS}}", bllMall.GetBindStoreAddress(curUser));

            if (indexStr.Contains("\"is_onlyshow_bind_shop_product\":\"1\"") &&string.IsNullOrEmpty(curUser.BindId))
            {
                Response.Redirect("/App/Outlets/StoreMap.aspx");
            }
            //this.Response.ClearContent();
            this.Response.Write(indexStr);
        }
        /// <summary>
        /// 检查指令控件替换
        /// </summary>  
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool CheckPropertyName(string propertyName)
        {
            if (propertyName.Equals("sidemenubox")) return true;
            if (propertyName.StartsWith("slide_list")) return true;
            if (propertyName.StartsWith("slides")) return true;
            if (propertyName.StartsWith("navs")) return true;
            if (propertyName.StartsWith("tab_list")) return true;
            if (propertyName.StartsWith("userinfo")) return true;
            if (propertyName.StartsWith("cards")) return true;
            if (propertyName.StartsWith("goods")) return true;
            if (propertyName.StartsWith("malls")) return true;
            if (propertyName.StartsWith("activitys")) return true;
            if (propertyName.StartsWith("searchbox")) return true;
            if (propertyName.StartsWith("notice")) return true;
            if (propertyName.StartsWith("content")) return true;
            if (propertyName.StartsWith("block")) return true;
            if (propertyName.StartsWith("linetext")) return true;
            if (propertyName.StartsWith("linebutton")) return true;
            if (propertyName.StartsWith("linehead")) return true;
            if (propertyName.StartsWith("headsearch")) return true;
            return false;
        }
        /// <summary>
        /// 加载组件配置
        /// </summary>
        /// <param name="jProperty"></param>
        /// <param name="jObject"></param>
        /// <param name="childLevel"></param>
        /// <param name="maxChildLevel"></param>
        public void GetComponentConfig(JProperty jProperty, ref JObject jObject, int childLevel, int maxChildLevel = 3)
        {
            if (childLevel > maxChildLevel)
            {
                jObject[jProperty.Name] = JObject.FromObject(new object());
                return;
            }
            //加载组件配置
            BLLJIMP.Model.Component ComponentModel = bll.Get<BLLJIMP.Model.Component>(string.Format(" WebsiteOwner='{0}' AND AutoId={1}", bll.WebsiteOwner, jProperty.Value));
            JObject childJObject = JObject.Parse(ComponentModel.ComponentConfig);
            if (ComponentModel == null)
            {
                jObject[jProperty.Name] = JObject.FromObject(new object());
                return;
            }
            if (ComponentModel.ComponentType == "list")
            {
                string list_type = "Article";
                string list_cateid = "-9";
                int rows = 10;
                int page = 1;
                if (childJObject["list_type"] != null) list_type = childJObject["list_type"].ToString();
                if (childJObject["list_cateid"] != null) list_cateid = childJObject["list_cateid"].ToString();
                if (childJObject["rows"] != null) int.TryParse(childJObject["rows"].ToString(), out rows);
                if (childJObject["page"] != null) int.TryParse(childJObject["page"].ToString(), out page);

                //加载首屏文章
                Web.Serv.pubapi p = new Serv.pubapi();
                var articleData = p.GetArticleList(page, rows, 0, 0, list_cateid, "", "", "", "", "", list_type, "", "");
                JObject tempJObject = JObject.FromObject(articleData); //JObject.Parse(JSONHelper.ObjectToJson(articleData));
                childJObject["list_total"] = tempJObject["totalcount"];
                childJObject["list_list"] = tempJObject["list"];
            }
            else if (ComponentModel.ComponentType == "tab")
            {
                List<JProperty> childListProperty = childJObject.Properties().Where(p => p.Name.StartsWith("tab_list")).ToList();
                foreach (JProperty childitem in childListProperty)
                {
                    JArray ccJArray = ((JArray)childJObject[childitem.Name]);
                    for (int i = 0; i < ccJArray.Count; i++)
                    {
                        JObject ccJObject = (JObject)ccJArray[i];
                        List<JProperty> ccComponentProperty = ccJObject.Properties().Where(p => p.Name.StartsWith("component_")).ToList();
                        foreach (JProperty item in ccComponentProperty)
                        {
                            GetComponentConfig(item, ref ccJObject, childLevel + 1);
                        }
                    }
                }
            }
            else if (ComponentModel.ComponentType == "sidemenu")
            {
                string sidemenu_type = "Article";
                int sidemenu_cateid = -9;
                if (childJObject["sidemenu_type"] != null) sidemenu_type = childJObject["sidemenu_type"].ToString();
                if (childJObject["sidemenu_cateid"] != null) int.TryParse(childJObject["sidemenu_cateid"].ToString(), out sidemenu_cateid);
                int tempTotalCount = 0;
                var dataList = new BLLJIMP.BLLArticleCategory().GetCateList(out tempTotalCount, sidemenu_type, sidemenu_cateid, bll.WebsiteOwner);
                dynamic cateResult = from p in dataList
                                     select new
                                     {
                                         id = p.AutoID,
                                         name = p.CategoryName,
                                         createTime = p.CreateTime.ToString(),
                                         img = p.ImgSrc,
                                         summary = p.Summary
                                     };
                childJObject["sidemenu_list"] = JArray.FromObject(cateResult);
            }
            else if (ComponentModel.ComponentType == "slide")
            {
                StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
                sbWhere.AppendFormat(" And Type='{0}'", childJObject["slide_list"]);
                List<Slide> dataList = bll.GetList<Slide>(int.MaxValue, sbWhere.ToString(), " Sort DESC ");
                var slide_list = (from p in dataList
                                  select new
                                  {
                                      title = p.LinkText,
                                      img = p.ImageUrl,
                                      link = p.Link
                                  }).ToList();
                childJObject["slide_list"] = JArray.FromObject(slide_list);
            }
            else if (ComponentModel.ComponentType == "foottool")
            {
                StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
                sbWhere.AppendFormat(" And KeyType = '{0}'", childJObject["foottool_list"].ToString());
                List<CompanyWebsite_ToolBar> dataList = bll.GetList<CompanyWebsite_ToolBar>(int.MaxValue, sbWhere.ToString(), " PlayIndex ASC");
                List<dynamic> foottool_list = new List<dynamic>();
                foreach (CompanyWebsite_ToolBar p in dataList)
                {
                    if (!bllToolbar.CheckHasPms(curUser, p)) continue;

                    foottool_list.Add(new
                    {
                        title = p.ToolBarName,
                        ico = p.ToolBarImage,
                        img = p.ImageUrl,
                        url = p.ToolBarTypeValue,
                        type = p.ToolBarType,
                        active_bg_color = p.ActBgColor,
                        bg_color = p.BgColor,
                        active_color = p.ActColor,
                        color = p.Color,
                        ico_color = p.IcoColor,
                        active_bg_img = p.ActBgImage,
                        bg_img = p.BgImage,
                        ico_position = p.IcoPosition,
                        right_text = p.RightText
                    });
                }
                childJObject["foottool_list"] = JArray.FromObject(foottool_list);
            }
            jObject[jProperty.Name] = childJObject;
        }
        /// <summary>
        /// 查询幻灯片，导航数据
        /// </summary>
        /// <param name="jProperty"></param>
        /// <param name="jObject"></param>
        public void GetChildConfig(JProperty jProperty, ref JToken jObject)
        {
            if (jProperty.Value.ToString() == "-999")
            {
                List<string> ltemp = new List<string>();
                jObject = JArray.FromObject(ltemp);
                return;
            }

            if (jProperty.Name.StartsWith("slide_list"))
            {

                StringBuilder sbWhere = new StringBuilder(string.Format(" WebsiteOwner='{0}'", bll.WebsiteOwner));
                sbWhere.AppendFormat(" And Type='{0}'", jProperty.Value.ToString());
                List<Slide> dataList = bll.GetList<Slide>(int.MaxValue, sbWhere.ToString(), " Sort DESC ");
                dynamic slide_list = from p in dataList
                                     select new
                                     {
                                         title = p.LinkText,
                                         img = p.ImageUrl,
                                         link = p.Link,
                                         width = p.Width,
                                         height = p.Height
                                     };
                jObject = JArray.FromObject(slide_list);
            }
            else if (jProperty.Name.StartsWith("foottool_list") ||
                jProperty.Name.StartsWith("tab_list") ||
                jProperty.Name.StartsWith("button_list") ||
                jProperty.Name.StartsWith("nav_list") ||
                jProperty.Name.StartsWith("headtool_list"))
            {
                List<CompanyWebsite_ToolBar> dataList = bllToolbar.GetCommonToolBarList(bllToolbar.WebsiteOwner, jProperty.Value.ToString());
                List<dynamic> foottool_list = new List<dynamic>();
                foreach (CompanyWebsite_ToolBar p in dataList)
                {
                    if (!bllToolbar.CheckHasPms(curUser, p)) continue;

                    foottool_list.Add(new
                    {
                        title = p.ToolBarName,
                        ico = p.ToolBarImage,
                        img = p.ImageUrl,
                        url = p.ToolBarTypeValue,
                        type = p.ToolBarType,
                        active_bg_color = p.ActBgColor,
                        bg_color = p.BgColor,
                        active_color = p.ActColor,
                        color = p.Color,
                        ico_color = p.IcoColor,
                        active_bg_img = p.ActBgImage,
                        bg_img = p.BgImage,
                        ico_position = p.IcoPosition,
                        right_text = p.RightText
                    });
                }
                jObject = JArray.FromObject(foottool_list);
            }
        }
        /// <summary>
        /// 商品列表数据
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="tags"></param>
        /// <param name="isgroupbuy"></param>
        /// <param name="sort"></param>
        /// <param name="rows"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public JArray GetMallJTokenList(string cateId, string tags, string isgroupbuy, string sort, string rows, out int total, string keyword, string sort_tag,
            string product_ids,string sort_ids,string can_repeat,string type="",string supplierId="")
        {
            string articleCategoryType = "Mall";
            if (!string.IsNullOrEmpty(type))
            {
                articleCategoryType = type;
            }

            total = 0;
            var sourceData = bllMall.GetProductList(keyword, cateId, null, sort,
             "1", rows, null, null, null, null, null,
             null, null, null, tags, null,
             null, null, null, null, isgroupbuy,
            out total, 0, false, articleCategoryType , "", "", sort_tag, "", "", product_ids, "", "",  "",  "", "", "", "",false,  supplierId );
            dynamic list;
            if (!string.IsNullOrWhiteSpace(product_ids) && sort_ids == "1")
            {
                List<dynamic> relist = new List<dynamic>();
                List<string> productIdList = product_ids.Split(',').ToList();
                List<string> productInIdList = new List<string>();
                foreach (string productId in productIdList)
                {
                    if (can_repeat != "1" && productInIdList.Contains(productId)) continue;
                    WXMallProductInfo p = sourceData.FirstOrDefault(pi => pi.PID == productId);
                    if (p == null) continue;
                    productInIdList.Add(productId);
                    relist.Add(new
                    {
                        product_id = p.PID,
                        category_id = p.CategoryId,
                        title = p.PName,
                        summary = p.Summary,
                        quote_price = p.PreviousPrice,
                        price = bllMall.GetShowPrice(p),
                        img_url = bllMall.GetImgUrl(p.RecommendImg),
                        is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                        tags = p.Tags,
                        product_code = p.ProductCode,
                        sale_count = p.SaleCount,
                        review_count = p.ReviewCount,
                        totalcount = bllMall.GetProductTotalStock(int.Parse(p.PID)),
                        province = p.Province,
                        city = p.City,
                        district = p.District,
                        ex1 = p.Ex1,
                        group_buy_rule_list = from r in bllMall.GetProductGroupBuyRuleList(p.PID)
                                              select new
                                              {
                                                  rule_id = r.RuleId,
                                                  rule_name = r.RuleName,
                                                  head_discount = r.HeadDiscount,
                                                  head_price = Math.Round((decimal)p.Price * (decimal)(r.HeadDiscount / 10), 2),
                                                  member_discount = r.MemberDiscount,
                                                  member_price = Math.Round((decimal)p.Price * (decimal)(r.MemberDiscount / 10), 2),
                                                  people_count = r.PeopleCount,
                                                  expire_day = r.ExpireDay
                                              }
                    });

                }
                list = relist;
                total = relist.Count;
            }
            else
            {
                list = from p in sourceData
                       select new
                       {
                           product_id = p.PID,
                           category_id = p.CategoryId,
                           title = p.PName,
                           summary = p.Summary,
                           quote_price = p.PreviousPrice,
                           price = bllMall.GetShowPrice(p),
                           img_url = bllMall.GetImgUrl(p.RecommendImg),
                           is_onsale = (!string.IsNullOrEmpty(p.IsOnSale) && p.IsOnSale == "1") ? 1 : 0,
                           tags = p.Tags,
                           product_code = p.ProductCode,
                           sale_count = p.SaleCount,
                           review_count = p.ReviewCount,
                           totalcount = bllMall.GetProductTotalStock(int.Parse(p.PID)),
                           province = p.Province,
                           city = p.City,
                           district = p.District,
                           ex1 = p.Ex1,
                           group_buy_rule_list = from r in bllMall.GetProductGroupBuyRuleList(p.PID)
                                                 select new
                                                 {
                                                     rule_id = r.RuleId,
                                                     rule_name = r.RuleName,
                                                     head_discount = r.HeadDiscount,
                                                     head_price = Math.Round((decimal)p.Price * (decimal)(r.HeadDiscount / 10), 2),
                                                     member_discount = r.MemberDiscount,
                                                     member_price = Math.Round((decimal)p.Price * (decimal)(r.MemberDiscount / 10), 2),
                                                     people_count = r.PeopleCount,
                                                     expire_day = r.ExpireDay
                                                 }
                       };
            }
            return JArray.FromObject(list);
        }
        /// <summary>
        /// 商品列表数据
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="tags"></param>
        /// <param name="isgroupbuy"></param>
        /// <param name="sort"></param>
        /// <param name="rows"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public JArray GetOrderMallJTokenList(string rows, out int total, string keyword)
        {
            
            int totalCount = 0;
            var orderList = bllMall.GetOrderList(Convert.ToInt32(rows), 1, keyword, out totalCount, "", 
                "", "", "", "2","",
                "", "", "", "", "0", "", "", "", "", "", true, "", "", "1", "1");
            List<OrderListModel> list = new List<OrderListModel>();
            foreach (var orderInfo in orderList)
            {
                OrderListModel model = new OrderListModel();
                model.order_id = orderInfo.OrderID;
                model.out_order_id = orderInfo.OutOrderId;
                model.order_time = bllMall.GetTimeStamp(orderInfo.InsertDate);
                model.product_count = orderInfo.ProductCount;
                model.total_amount = orderInfo.TotalAmount;
                model.order_status = orderInfo.Status;
                model.is_pay = orderInfo.PaymentStatus;
                model.pay_type = orderInfo.PaymentType == 2 ? "WEIXIN" : "ALIPAY";
                model.express_company_code = orderInfo.ExpressCompanyCode;
                model.express_company_name = orderInfo.ExpressCompanyName;
                model.express_number = orderInfo.ExpressNumber;
                model.order_type = orderInfo.OrderType;
                //model.gift_order_type = giftOrderType;
                model.is_cansendgift = bllMall.IsCanShareGift(orderInfo);
                model.delivery_type = orderInfo.DeliveryType;
                model.review_score = orderInfo.ReviewScore;
                model.is_appointment = orderInfo.IsAppointment;
                model.is_main = orderInfo.IsMain;
                model.parent_order_id = !string.IsNullOrEmpty(orderInfo.ParentOrderId) ? orderInfo.ParentOrderId : "";
                model.supplier_name = orderInfo.SupplierName;
                model.ex9 = orderInfo.Ex9;
                model.ex10 = orderInfo.Ex10;
                model.ex11 = orderInfo.Ex11;
                model.ex12 = orderInfo.Ex12;
                model.ex13 = orderInfo.Ex13;
                model.people_count = orderInfo.PeopleCount;
                model.member_discount = orderInfo.MemberDiscount;


                model.product_list = new List<OrderProductModel>();
                var orderDetailList = bllMall.GetOrderDetailsList(orderInfo.OrderID);
                foreach (var orderDetail in orderDetailList)
                {
                    WXMallProductInfo productInfo = bllMall.GetProduct(orderDetail.PID);
                    OrderProductModel orderProductModel = new OrderProductModel();
                    orderProductModel.count = orderDetail.TotalCount;
                    orderProductModel.img_url = orderDetail.ProductImage;
                    if (string.IsNullOrEmpty(orderProductModel.img_url))
                    {
                        orderProductModel.img_url = bllMall.GetImgUrl(productInfo.RecommendImg);
                    }
                    orderProductModel.product_name = orderDetail.ProductName;
                    if (string.IsNullOrEmpty(orderProductModel.product_name))
                    {
                        orderProductModel.product_name = productInfo.PName;
                    }
                    orderProductModel.price = (decimal)orderDetail.OrderPrice;

                   
                    orderProductModel.category_name = bllMall.GetWXMallCategoryName(productInfo.CategoryId);
                    orderProductModel.quote_price = productInfo.PreviousPrice;
                    orderProductModel.show_property = orderDetail.SkuShowProp;
                    orderProductModel.parent_product_id = orderDetail.ParentProductId;
                    model.product_list.Add(orderProductModel);
                }
                Serv.API.Mall.Order orderPage = new Serv.API.Mall.Order();
                model.group_buy_info = orderPage.GetGroupBuyInfo(orderInfo);
                list.Add(model);

            }
            total = totalCount;
            return JArray.FromObject(list);
        }

        #region 侧边栏
        /// <summary>
        /// 查询侧边栏
        /// </summary>
        /// <param name="jProperty"></param>
        /// <param name="jObject"></param>
        public JToken GetSidemenu(JToken jObject)
        {
            if(jObject.Type == JTokenType.Null) return null;
            if (jObject["show"].Type == JTokenType.Null || jObject["show"].ToString() == "-1") return null;
            if (jObject["type"].Type == JTokenType.Null) return null;
            if (jObject["data_key"].Type == JTokenType.Null) return null;

            string type = jObject["type"].ToString();
            string data_key = jObject["data_key"].ToString();
            if (type == "1")
            {
                #region 文章侧边栏

                int cateId = 0;
                int.TryParse(data_key, out cateId);
                int totalCate = 0;
                var dataList = bllCate.GetCateList(out totalCate, "Article", cateId, bll.WebsiteOwner);
                if (dataList.Count == 0) return null;
                List<Sidemenu> sidemenu_list = new List<Sidemenu>();
                if (dataList.Count > 0)
                {
                    if (cateId == 0)
                    {
                        Sidemenu temp = new Sidemenu();
                        temp.text = "全部";
                        temp.value = cateId.ToString();
                        temp.is_expand = true;
                        temp.childrens = new List<Sidemenu>();
                        sidemenu_list.Add(temp);
                    }
                    else
                    {
                        var pSidemenu = dataList.FirstOrDefault(p => p.AutoID == cateId);
                        if (pSidemenu != null)
                        {
                            Sidemenu temp = new Sidemenu();
                            temp.text = "全部";
                            temp.value = pSidemenu.AutoID.ToString();
                            temp.is_expand = true;
                            temp.childrens = new List<Sidemenu>();
                            sidemenu_list.Add(temp);
                        }
                    }
                    foreach (var pitem in dataList.Where(p => p.PreID == cateId))
                    {
                        Sidemenu ptemp = new Sidemenu();
                        ptemp.text = pitem.CategoryName;
                        ptemp.value = pitem.AutoID.ToString();
                        ptemp.is_expand = true;
                        ptemp.childrens = new List<Sidemenu>();
                        List<ArticleCategory> ctList = dataList.Where(p => p.PreID == pitem.AutoID).ToList();
                        if (ctList.Count > 0)
                        {
                            Sidemenu ttemp = new Sidemenu();
                            ttemp.text = "全部";
                            ttemp.value = pitem.AutoID.ToString();
                            ttemp.is_expand = true;
                            ttemp.childrens = new List<Sidemenu>();
                            ptemp.childrens.Add(ttemp);
                            foreach (var item in ctList)
                            {
                                Sidemenu temp = new Sidemenu();
                                temp.text = item.CategoryName;
                                temp.value = item.AutoID.ToString();
                                temp.is_expand = true;
                                temp.childrens = new List<Sidemenu>();
                                ptemp.childrens.Add(temp);
                            }
                        }
                        sidemenu_list.Add(ptemp);
                    }

                }
                jObject["sidemenu_list"] = JToken.FromObject(sidemenu_list);
                #endregion 文章侧边栏
            }
            else if (type == "2")
            {
                #region 商品侧边栏
                int cateId = 0;
                int.TryParse(data_key, out cateId);
                int totalCate = 0;
                var dataList = bllMall.GetCategoryList(1, int.MaxValue, "", out totalCate);
                if (dataList.Count == 0) return null;
                List<Sidemenu> sidemenu_list = new List<Sidemenu>();
                if (dataList.Count > 0)
                {
                    if (cateId == 0)
                    {
                        Sidemenu temp = new Sidemenu();
                        temp.text = "全部";
                        temp.value = cateId.ToString();
                        temp.is_expand = true;
                        temp.childrens = new List<Sidemenu>();
                        sidemenu_list.Add(temp);
                    }
                    else
                    {
                        var pSidemenu = dataList.FirstOrDefault(p => p.AutoID == cateId);
                        if (pSidemenu != null)
                        {
                            Sidemenu temp = new Sidemenu();
                            temp.text = "全部";
                            temp.value = pSidemenu.AutoID.ToString();
                            temp.is_expand = true;
                            temp.childrens = new List<Sidemenu>();
                            sidemenu_list.Add(temp);
                        }
                    }
                    foreach (var pitem in dataList.Where(p => p.PreID == cateId))
                    {
                        Sidemenu ptemp = new Sidemenu();
                        ptemp.text = pitem.CategoryName;
                        ptemp.value = pitem.AutoID.ToString();
                        ptemp.is_expand = true;
                        ptemp.childrens = new List<Sidemenu>();
                        List<WXMallCategory> ctList = dataList.Where(p => p.PreID == pitem.AutoID).ToList();
                        if (ctList.Count > 0)
                        {
                            Sidemenu ttemp = new Sidemenu();
                            ttemp.text = "全部";
                            ttemp.value = pitem.AutoID.ToString();
                            ttemp.is_expand = true;
                            ttemp.childrens = new List<Sidemenu>();
                            ptemp.childrens.Add(ttemp);
                            foreach (var item in ctList)
                            {
                                Sidemenu temp = new Sidemenu();
                                temp.text = item.CategoryName;
                                temp.value = item.AutoID.ToString();
                                temp.is_expand = true;
                                temp.childrens = new List<Sidemenu>();
                                ptemp.childrens.Add(temp);
                            }
                        }
                        sidemenu_list.Add(ptemp);
                    }

                }
                jObject["sidemenu_list"] = JToken.FromObject(sidemenu_list);
                #endregion 商品侧边栏
            }
            else if (type == "3")
            {
                #region 活动侧边栏

                int cateId = 0;
                int.TryParse(data_key, out cateId);
                int totalCate = 0;
                var dataList = bllCate.GetCateList(out totalCate, "Activity", cateId, bll.WebsiteOwner);
                if (dataList.Count == 0) return null;
                List<Sidemenu> sidemenu_list = new List<Sidemenu>();
                if (dataList.Count > 0)
                {
                    if (cateId == 0)
                    {
                        Sidemenu temp = new Sidemenu();
                        temp.text = "全部";
                        temp.value = cateId.ToString();
                        temp.is_expand = true;
                        temp.childrens = new List<Sidemenu>();
                        sidemenu_list.Add(temp);
                    }
                    else
                    {
                        var pSidemenu = dataList.FirstOrDefault(p => p.AutoID == cateId);
                        if (pSidemenu != null)
                        {
                            Sidemenu temp = new Sidemenu();
                            temp.text = "全部";
                            temp.value = pSidemenu.AutoID.ToString();
                            temp.is_expand = true;
                            temp.childrens = new List<Sidemenu>();
                            sidemenu_list.Add(temp);
                        }
                    }
                    foreach (var pitem in dataList.Where(p => p.PreID == cateId))
                    {
                        Sidemenu ptemp = new Sidemenu();
                        ptemp.text = pitem.CategoryName;
                        ptemp.value = pitem.AutoID.ToString();
                        ptemp.is_expand = true;
                        ptemp.childrens = new List<Sidemenu>();
                        List<ArticleCategory> ctList = dataList.Where(p => p.PreID == pitem.AutoID).ToList();
                        if (ctList.Count > 0)
                        {
                            Sidemenu ttemp = new Sidemenu();
                            ttemp.text = "全部";
                            ttemp.value = pitem.AutoID.ToString();
                            ttemp.is_expand = true;
                            ttemp.childrens = new List<Sidemenu>();
                            ptemp.childrens.Add(ttemp);
                            foreach (var item in ctList)
                            {
                                Sidemenu temp = new Sidemenu();
                                temp.text = item.CategoryName;
                                temp.value = item.AutoID.ToString();
                                temp.is_expand = true;
                                temp.childrens = new List<Sidemenu>();
                                ptemp.childrens.Add(temp);
                            }
                        }
                        sidemenu_list.Add(ptemp);
                    }

                }
                jObject["sidemenu_list"] = JToken.FromObject(sidemenu_list);
                #endregion 活动侧边栏
            }
            else if (type == "4")
            {
                #region 导航侧边栏
                List<Sidemenu> sidemenu_list = new List<Sidemenu>();

                List<CompanyWebsite_ToolBar> dataList = bllToolbar.GetCommonToolBarList(bllToolbar.WebsiteOwner, data_key);
                foreach (var pitem in dataList.Where(p => p.PreID == 0))
                {
                    if (!bllToolbar.CheckHasPms(curUser, pitem)) continue;

                    Sidemenu ptemp = new Sidemenu();
                    ptemp.text = pitem.ToolBarName;
                    ptemp.value = pitem.ToolBarTypeValue;
                    ptemp.ico = pitem.ToolBarImage;
                    ptemp.ico_color = pitem.IcoColor;
                    ptemp.ico_position = pitem.IcoPosition;
                    ptemp.is_expand = true;
                    ptemp.childrens = new List<Sidemenu>();
                    foreach (var item in dataList.Where(p => p.PreID == pitem.AutoID))
                    {
                        if (!bllToolbar.CheckHasPms(curUser, item)) continue;

                        Sidemenu temp = new Sidemenu();
                        temp.text = item.ToolBarName;
                        temp.value = item.ToolBarTypeValue;
                        temp.ico_color = item.IcoColor;
                        temp.ico_position = item.IcoPosition;
                        temp.is_expand = true;
                        temp.childrens = new List<Sidemenu>();
                        ptemp.childrens.Add(temp);
                    }
                    sidemenu_list.Add(ptemp);
                }
                jObject["sidemenu_list"] = JToken.FromObject(sidemenu_list);
                #endregion 导航侧边栏
            }
            return jObject;
        }
        public class Sidemenu
        {
            public string text { get; set; }
            public string value { get; set; }
            public string ico { get; set; }
            public string ico_color { get; set; }
            public int ico_position { get; set; }
            public bool is_expand { get; set; }
            public bool is_select { get; set; }
            public List<Sidemenu> childrens { get; set; }
        }
        #endregion

        public void ReplaceDirective(ref string indexStr, string name, int modelId, JToken nObject)
        {
            if (modelId >= 10)
            {
                if (name.StartsWith("slides"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<slide key=\"{0}\"></slide><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("navs"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<navs key=\"{0}\"></navs><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("userinfo"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        "<userinfo></userinfo><contentdirective></contentdirective>");
                }
                else if (name.StartsWith("cards"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<cardlist key=\"{0}\"></cardlist><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("malls"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<malls key=\"{0}\"></malls><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("goods"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<goods key=\"{0}\"></goods><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("searchbox"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        "<search></search><contentdirective></contentdirective>");
                }
                else if (name.StartsWith("activitys"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<activitylist key=\"{0}\"></activitylist><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("notice"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<notice key=\"{0}\"></notice><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("content"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<content key=\"{0}\"></content><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("block"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<block key=\"{0}\"></block><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("linetext"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<linetext key=\"{0}\"></linetext><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("linebutton"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<linebutton key=\"{0}\"></linebutton><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("linehead"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<linehead key=\"{0}\"></linehead><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("headsearch"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<headsearch></headsearch><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("tab_list"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<tabs key=\"{0}\"></tabs><contentdirective></contentdirective>",
                        name));
                }
            }
            else
            {
                if (name.StartsWith("slide_list"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div slide ng-attr-slide-data=\"pageConfig.{0}\" ng-if=\"pageConfig.{0} && pageConfig.{0}.length > 0\"></div><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("slides"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div slide ng-attr-slide-data=\"slide.slide_list\" ng-attr-slide-config=\"slide\" ng-if=\"slide.slide_list\"  ng-repeat=\"slide in pageConfig.{0}\"></div><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("navs"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div navs class=\"wrapUserNavs\" ng-attr-config=\"nav\" ng-attr-data=\"nav.nav_list\" ng-repeat=\"nav in pageConfig.{0} track by $index\" ng-if=\"nav.nav_list\"></div><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("tab_list"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div tabs class=\"row tab\" ng-attr-tablist=\"pageConfig.{0}\" ng-if=\"pageConfig.{0}.length>0\"></div><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("userinfo"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div userinfo ng-attr-navdata=\"pageConfig.{0}\" ng-attr-logininfo=\"loginUserInfo\" ng-if=\"pageConfig.{0} && pageConfig.{0}.show==1\"></div><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("cards"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div cardlist ng-repeat=\"card in pageConfig.{0} track by $index\" ng-attr-config=\"card\" ng-attr-page-config=\"pageConfig\" ng-attr-repeat-count=\"{1}\"></div><contentdirective></contentdirective>",
                        name,
                        "{{pageConfig." + name + ".length}}"));
                }
                else if (name.StartsWith("malls"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div malls ng-attr-malldata=\"mall\" ng-attr-config=\"mallConfig\" ng-attr-page-config=\"pageConfig\" ng-attr-repeat-count=\"{1}\" ng-repeat=\"mall in pageConfig.{0} track by $index\"></div><contentdirective></contentdirective>",
                        name,
                        "{{pageConfig." + name + ".length}}"));
                }
                else if (name.StartsWith("searchbox"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div search ng-attr-page-config=\"pageConfig\" ng-attr-searchdata=\"pageConfig.{0}\" ng-if=\"pageConfig.{0} && pageConfig.{0}.show==1\"></div><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("activitys"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div activitylist ng-repeat=\"activity in pageConfig.{0} track by $index\" ng-attr-activitydata=\"activity\" ng-attr-page-config=\"pageConfig\" ng-attr-repeat-count=\"{1}\"></div><contentdirective></contentdirective>",
                        name,
                        "{{pageConfig." + name + ".length}}"));
                }
                else if (name.StartsWith("notice"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div notice ng-repeat=\"notice in pageConfig.{0} track by $index\" ng-attr-noticedata=\"notice\" ng-if=\"notice\"></div><contentdirective></contentdirective>",
                        name));
                }
                else if (name.StartsWith("content"))
                {
                    indexStr = indexStr.Replace("<contentdirective></contentdirective>",
                        string.Format("<div content ng-repeat=\"content in pageConfig.{0} track by $index\" ng-attr-config=\"content\" ng-if=\"content\"></div><contentdirective></contentdirective>",
                        name));
                }
            }
        }
    }
}