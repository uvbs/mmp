using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Component
{
    /// <summary>
    /// GetKeyConfig 的摘要说明
    /// </summary>
    public class GetKeyConfig : BaseHandlerNoAction
    {
        BLLComponent bll = new BLLComponent();
        public void ProcessRequest(HttpContext context)
        {
            var cgid = context.Request["cgid"];
            var key = context.Request["key"];
            var property = context.Request["property"];
            var key_type = context.Request["key_type"];
            //参数检查
            if ((string.IsNullOrWhiteSpace(cgid) && string.IsNullOrWhiteSpace(key)) || string.IsNullOrWhiteSpace(property))
            {
                apiResp.msg = "参数错误";
                apiResp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                bll.ContextResponse(context, apiResp);
                return;
            }
            //查询组件
            BLLJIMP.Model.Component model = new BLLJIMP.Model.Component();
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
                apiResp.msg = "组件不存在";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            //加载具体配置
            JObject jobject = JObject.Parse(model.ComponentConfig);
            if (jobject[property] == null)
            {
                apiResp.msg = "属性不存在";
                apiResp.code = (int)APIErrCode.IsNotFound;
                bll.ContextResponse(context, apiResp);
                return;
            }
            List<string> PropertyList = new List<string>(){"slide_list","foottool_list","tab_list","button_list","nav_list","headtool_list"};
            for (int i = 0; i < PropertyList.Count; i++)
            {
                if (property.StartsWith(PropertyList[i]))
                {
                    if (model.ComponentModelId > 10 && (property.StartsWith("foottool_list") || property.StartsWith("tab_list")))
                    {
                        customize.comeoncloud.Index ComponentIndex = new customize.comeoncloud.Index();
                        JToken childJObject = jobject[property];
                        if (childJObject.Type == JTokenType.String)
                        {
                            JProperty nPro = new JProperty(property, childJObject);
                            JToken nObject = new JObject();
                            ComponentIndex.GetChildConfig(nPro, ref nObject);
                            jobject[property] = new JObject();
                            jobject[property]["list"] = nObject;
                        }
                        else
                        {
                            JProperty nPro = new JProperty(property, childJObject["key_type"]);
                            JToken nObject = new JObject();
                            ComponentIndex.GetChildConfig(nPro, ref nObject);
                            jobject[property]["list"] = nObject;
                        }
                        break;
                    }
                    else
                    {
                        customize.comeoncloud.Index ComponentIndex = new customize.comeoncloud.Index();
                        JToken childJObject = jobject[property];
                        JProperty propertyKey = jobject.Properties().FirstOrDefault(p => p.Name == property);
                        if (!string.IsNullOrWhiteSpace(key_type)) propertyKey.Value = key_type;
                        ComponentIndex.GetChildConfig(propertyKey, ref childJObject);
                        jobject[property] = childJObject;
                        break;
                    }
                }
            }
            if (property.StartsWith("navs") && jobject[property].Type == JTokenType.Array)
            {
                JArray childJArray = JArray.FromObject(jobject[property]);
                bool hasGetData = false;
                foreach (JObject nJArray in childJArray)
                {
                    List<JProperty> listNavProperty = nJArray.Properties().Where(p => p.Name.StartsWith("nav_list")).ToList();
                    foreach (JProperty cproperty in listNavProperty)
                    {
                        if (cproperty.Value.Type == JTokenType.String)
                        {
                            JToken childJObject = nJArray[cproperty.Name];
                            customize.comeoncloud.Index ComponentIndex = new customize.comeoncloud.Index();
                            ComponentIndex.GetChildConfig(cproperty, ref childJObject);
                            nJArray[cproperty.Name] = childJObject;
                            hasGetData = true;
                        }
                    }
                }
                if (hasGetData) jobject[property] = childJArray;
            }
            if (property.StartsWith("slides") && jobject[property].Type == JTokenType.Array)
            {
                JArray childJArray = JArray.FromObject(jobject[property]);
                bool hasGetData = false;
                foreach (JObject nJArray in childJArray)
                {
                    List<JProperty> listNavProperty = nJArray.Properties().Where(p => p.Name.StartsWith("slide_list")).ToList();
                    foreach (JProperty cproperty in listNavProperty)
                    {
                        if (cproperty.Value.Type == JTokenType.String)
                        {
                            JToken childJObject = nJArray[cproperty.Name];
                            customize.comeoncloud.Index ComponentIndex = new customize.comeoncloud.Index();
                            ComponentIndex.GetChildConfig(cproperty, ref childJObject);
                            nJArray[cproperty.Name] = childJObject;
                            hasGetData = true;
                        }
                    }
                }
                if (hasGetData) jobject[property] = childJArray;
            }
            apiResp.result = jobject[property];
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
        }
    }
}