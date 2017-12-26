using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Component
{
    /// <summary>
    /// Add 的摘要说明
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLLComponent bll = new BLLJIMP.BLLComponent();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = new RequestModel();
            try
            {
                requestModel = bll.ConvertRequestToModel<RequestModel>(requestModel);
            }
            catch (Exception ex)
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                //apiResp.msg = ex.Message;
                apiResp.msg = "json格式错误,请检查。";
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrEmpty(requestModel.component_name))
            {
                apiResp.msg = "请输入页面名称";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (requestModel.component_model_id==0)
            {
                apiResp.msg = "请选择模板";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (!string.IsNullOrWhiteSpace(requestModel.component_key))
            {
                if (bll.GetComponentByKey(requestModel.component_key, bll.WebsiteOwner) != null)
                {
                    apiResp.msg = "标识已被使用";
                    apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            BLLJIMP.Model.Component model = new BLLJIMP.Model.Component();
            model.WebsiteOwner = bll.WebsiteOwner;
            model.ComponentKey = requestModel.component_key;
            model.ChildComponentIds = requestModel.child_component_ids;
            model.Decription = requestModel.decription;
            model.ComponentModelId = requestModel.component_model_id;
            model.ComponentTemplateId = requestModel.component_template_id;
            ComponentModel componentModel = bll.GetByKey<ComponentModel>("AutoId", requestModel.component_model_id.ToString());
            if (componentModel == null)
            {
                apiResp.msg = "模板未找到";
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            model.ComponentType = componentModel.ComponentModelType;
            //if (model.ComponentType != "page") model.WebsiteOwner = "Common";
            model.ComponentName = requestModel.component_name;
            model.IsWXSeniorOAuth = requestModel.is_oauth;
            model.ComponentConfig = requestModel.component_config;
            model.AccessLevel = requestModel.access_level;
            model.IsInitData = requestModel.is_init_data;

            if (bll.Add(model))
            {
                //更新幻灯片
                if (!string.IsNullOrWhiteSpace(requestModel.slides)) { 
                    UpdateSlide(requestModel.slides); 
                }
                //更新导航
                if (!string.IsNullOrWhiteSpace(requestModel.toolbars)){
                    UpdateToolbar(requestModel.toolbars); 
                }

                BLLJIMP.Model.Component nModel = bll.GetNewComponent(bll.WebsiteOwner);
                apiResp.result = new
                {
                    component_id = nModel.AutoId
                };
                apiResp.status = true;
                apiResp.msg = "添加完成";
            }
            else
            {
                apiResp.code = (int)BLLJIMP.Enums.APIErrCode.OperateFail;
                apiResp.msg = "添加页面出错";
            }
            bll.ContextResponse(context,apiResp);

        }

        //更新幻灯片
        public static void UpdateSlide(string slides)
        {
            BLLSlide bllSlide = new BLLSlide();
            List<slide> listSlide = JsonConvert.DeserializeObject<List<slide>>(slides);
            List<string> listType = new List<string>();
            if (listSlide.Count > 0) listType = listSlide.Select(p => p.type).Distinct().Where(g => !string.IsNullOrWhiteSpace(g) && g!="null").ToList();
            foreach (string type in listType)
            {
                List<slide> listTypeSlide = listSlide.Where(p => p.type == type && !string.IsNullOrWhiteSpace(p.img) && p.id >= 0).ToList();
                List<int> listId = new List<int>();
                if (listTypeSlide.Count > 0) listId = listTypeSlide.Select(p => p.id).Distinct().ToList();
                List<Slide> listOldSlide = bllSlide.ListByType(type, bllSlide.WebsiteOwner);
                List<Slide> listEditSlide = listOldSlide.Where(p => listId.Contains(p.AutoID)).ToList();
                List<Slide> listDeleteSlide = listOldSlide.Where(p => !listId.Contains(p.AutoID)).ToList();

                for (int i = 0; i < listTypeSlide.Count; i++)
                {
                    Slide sli = listEditSlide.FirstOrDefault(p => p.AutoID == listTypeSlide[i].id);
                    if (sli!=null)
                    {
                        sli.LinkText = listTypeSlide[i].title;
                        sli.ImageUrl = listTypeSlide[i].img;
                        sli.Link = listTypeSlide[i].link;
                        sli.Sort = listTypeSlide.Count - i;
                        sli.Width = listTypeSlide[i].width;
                        sli.Height = listTypeSlide[i].height;
                        sli.Stype = listTypeSlide[i].s_type;
                        sli.Stext = listTypeSlide[i].s_text;
                        sli.Svalue = listTypeSlide[i].s_value;
                        bllSlide.Update(sli);
                    }
                    else
                    {
                        sli = new Slide();
                        sli.LinkText = listTypeSlide[i].title;
                        sli.ImageUrl = listTypeSlide[i].img;
                        sli.Link = listTypeSlide[i].link;
                        sli.Sort = listTypeSlide.Count - i;
                        sli.WebsiteOwner = bllSlide.WebsiteOwner;
                        sli.Type = listTypeSlide[i].type;
                        sli.Width = listTypeSlide[i].width;
                        sli.Height = listTypeSlide[i].height;
                        sli.Stype = listTypeSlide[i].s_type;
                        sli.Stext = listTypeSlide[i].s_text;
                        sli.Svalue = listTypeSlide[i].s_value;
                        bllSlide.Add(sli);
                    }
                }
                foreach (Slide item in listDeleteSlide)
                {
                    bllSlide.Delete(item);
                }
            }
        }
        //更新导航
        public static void UpdateToolbar(string toolbars)
        {
            BLLCompanyWebSite bllToolbar = new BLLCompanyWebSite();
            List<toolbar> listToolbar = JsonConvert.DeserializeObject<List<toolbar>>(toolbars);
            List<string> listType = new List<string>();
            if (listToolbar.Count > 0) listType = listToolbar.Select(p => p.key_type).Distinct().Where(g => !string.IsNullOrWhiteSpace(g) && g != "null").ToList();
            foreach (string type in listType)
            {
                List<toolbar> listTypeToolbar = listToolbar.Where(p => p.key_type == type && p.id>=0).ToList();
                List<CompanyWebsite_ToolBar> listOldToolbar = bllToolbar.GetToolBarList(int.MaxValue, 1, bllToolbar.WebsiteOwner, null, type, true, null);
                List<CompanyWebsite_ToolBar> listOldBaseToolbar = bllToolbar.GetToolBarList(int.MaxValue, 1, null, null, type, true, null, true);

                int i = 0;
                List<CompanyWebsite_ToolBar> listPostToolbar = new List<CompanyWebsite_ToolBar>();
                foreach (toolbar item in listTypeToolbar)
                {
                    i++;
                    CompanyWebsite_ToolBar sli = listOldToolbar.FirstOrDefault(p => item.id >0 &&(p.AutoID == item.id || p.BaseID == item.id));
                    if (sli != null)
                    {
                        sli.ToolBarName = item.title;
                        sli.ImageUrl = item.img;
                        sli.ToolBarImage = item.ico;
                        sli.ActBgColor = item.active_bg_color;
                        sli.BgColor = item.bg_color;
                        sli.ActColor = item.active_color;
                        sli.Color = item.color;
                        sli.IcoColor = item.ico_color;
                        sli.ActBgImage = item.active_bg_img;
                        sli.BgImage = item.bg_img;
                        sli.ToolBarType = item.type;
                        sli.ToolBarTypeValue = item.url;
                        sli.Stype = item.s_type;
                        sli.Stext = item.s_text;
                        sli.Svalue = item.s_value;
                        sli.PlayIndex = i;
                        sli.VisibleSet = item.visible_set;
                        sli.PermissionGroup = item.permission_group;
                        sli.RightText = item.right_text;
                        listPostToolbar.Add(sli);
                    }
                    else
                    {
                        CompanyWebsite_ToolBar bli = listOldBaseToolbar.FirstOrDefault(p => p.AutoID == item.id);
                        sli = new CompanyWebsite_ToolBar();
                        sli.ToolBarName = item.title;
                        sli.ImageUrl = item.img;
                        sli.ToolBarImage = item.ico;
                        sli.ActBgColor = item.active_bg_color;
                        sli.BgColor = item.bg_color;
                        sli.ActColor = item.active_color;
                        sli.Color = item.color;
                        sli.IcoColor = item.ico_color;
                        sli.ActBgImage = item.active_bg_img;
                        sli.BgImage = item.bg_img;
                        sli.ToolBarType = item.type;
                        sli.ToolBarTypeValue = item.url;
                        sli.PlayIndex = i;
                        sli.WebsiteOwner = bllToolbar.WebsiteOwner;
                        sli.KeyType = item.key_type;
                        sli.UseType = "nav";
                        sli.IsShow = "1";
                        sli.Stype = item.s_type;
                        sli.Stext = item.s_text;
                        sli.Svalue = item.s_value;
                        if (bli != null)
                        {
                            sli.BaseID = bli.AutoID;
                            sli.IsSystem = 1;
                        }
                        sli.VisibleSet = item.visible_set;
                        sli.PermissionGroup = item.permission_group;
                        sli.RightText = item.right_text;
                        listPostToolbar.Add(sli);
                    }
                }
                List<CompanyWebsite_ToolBar> listAddBaseToolbar = listOldBaseToolbar.Where(p => !listPostToolbar.Exists(pi => pi.BaseID == p.AutoID) && !listOldToolbar.Exists(po => po.BaseID == p.AutoID)).ToList();
                foreach (CompanyWebsite_ToolBar item in listAddBaseToolbar)
                {
                    i++;
                    item.IsShow = "0";
                    item.BaseID = item.AutoID;
                    item.AutoID = 0;
                    item.IsSystem = 0;
                    item.WebsiteOwner = bllToolbar.WebsiteOwner;
                    item.PlayIndex = i;
                    listPostToolbar.Add(item);
                }
                List<CompanyWebsite_ToolBar> listDeleteToolbar = listOldToolbar.Where(p => !listPostToolbar.Exists(pi => pi.AutoID == p.AutoID || (pi.AutoID != 0 && pi.BaseID != 0 && pi.BaseID == p.BaseID)) && p.IsShow == "1").ToList();
                
                foreach (CompanyWebsite_ToolBar item in listDeleteToolbar)
                {
                    CompanyWebsite_ToolBar bli = listOldBaseToolbar.FirstOrDefault(p => p.AutoID == item.BaseID);
                    if (bli != null)
                    {
                        i++;
                        bli.IsShow = "0";
                        bli.BaseID = bli.AutoID;
                        bli.AutoID = 0;
                        bli.IsSystem = 0;
                        bli.WebsiteOwner = bllToolbar.WebsiteOwner;
                        bli.PlayIndex = i;
                        listPostToolbar.Add(bli);
                    }
                    bllToolbar.Delete(item);
                }
                foreach (CompanyWebsite_ToolBar item in listPostToolbar)
                {
                    if (item.AutoID == 0)
                    {
                        bllToolbar.Add(item);
                    }
                    else
                    {
                        bllToolbar.Update(item);
                    }
                }
            }
        }
        public class RequestModel
        {
            /// <summary>
            /// 组件库ID
            /// </summary>
            public int component_model_id { get; set; }
            /// <summary>
            /// 模板ID
            /// </summary>
            public int component_template_id { get; set; }
            /// <summary>
            /// 页面标识
            /// </summary>
            public string component_key { get; set; }
            /// <summary>
            /// 页面名称
            /// </summary>
            public string component_name { get; set; }
            /// <summary>
            /// 子页面
            /// </summary>
            public string child_component_ids { get; set; }
            /// <summary>
            /// 页面配置
            /// </summary>
            public string component_config { get; set; }
            /// <summary>
            ///描述
            /// </summary>
            public string decription { get; set; }
            /// <summary>
            /// 是否微信高级授权
            /// </summary>
            public int is_oauth { get; set; }
            /// <summary>
            /// 访问等级
            /// </summary>
            public int access_level { get; set; }
            /// <summary>
            ///幻灯片
            /// </summary>
            public string slides { get; set; }
            /// <summary>
            ///导航
            /// </summary>
            public string toolbars { get; set; }
            /// <summary>
            /// 是否初始化数据
            /// </summary>
            public int is_init_data { get; set; }
        }
        public class slide { 
            public int id { get; set; }
            public string title { get; set; }
            public string img { get; set; }
            public string link { get; set; }
            public string type { get; set; }
            public string websiteOwner { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string s_type { get; set; }
            public string s_value { get; set; }
            public string s_text { get; set; }
        }
        
        public class toolbar { 
            public int id { get; set; }
            public string title { get; set; }
            public string ico { get; set; }
            public string img { get; set; }
            public string url { get; set; }
            public string type { get; set; }
            public string active_bg_color { get; set; }
            public string bg_color { get; set; }
            public string active_color { get; set; }
            public string color { get; set; }
            public string ico_color { get; set; }
            public string active_bg_img { get; set; }
            public string bg_img { get; set; }
            public string key_type { get; set; }
            public string websiteOwner { get; set; }
            public string s_type { get; set; }
            public string s_value { get; set; }
            public string s_text { get; set; }
            public int visible_set { get; set; }
            public string permission_group { get; set; }
            public string right_text { get; set; }

        }
    }
}