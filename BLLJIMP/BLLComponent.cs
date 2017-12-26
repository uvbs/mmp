using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;
using CommonPlatform.Helper;
using ZentCloud.Common;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ZentCloud.BLLJIMP
{
    public class BLLComponent : BLL
    {

        public static List<string> limitControls = new List<string>() { "slides", "searchbox", "head_bar", "userinfo", "navs", "foottool_list", "tab_list", "activitys", "goods", "malls", "sidemenubox", "cards", "notice", "totop", "content", "block", "linetext", "linebutton", "linehead", "headsearch" };
        /// <summary>
        /// 查询页面列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="keyword"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ComponentModel> GetComponentModelList(int rows, int page, string keyword, out int total, string type = null, bool showDelete=false)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" 1=1 ");
            if(!showDelete) sbSql.AppendFormat(" AND IsDelete=0 ");
            if (!string.IsNullOrWhiteSpace(type))
            {
                sbSql.AppendFormat(" and ComponentModelType = '{0}' ", type);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sbSql.AppendFormat(" and (ComponentModel like '%{0}%' OR ComponentModelName like '%{0}%') ", keyword);
            }
            total = GetCount<ComponentModel>(sbSql.ToString());
            return GetLit<ComponentModel>(rows, page, sbSql.ToString());
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="componentModel"></param>
        /// <param name="componentModelFields"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool AddComponentModel(ComponentModel componentModel, List<ComponentModelField> componentModelFields, out string errmsg)
        {
            BLLTransaction tran = new BLLTransaction();
            errmsg = "";
            if (!Add(componentModel, tran))
            {
                errmsg = "页面添加失败";
                tran.Rollback();
                return false;
            }
            foreach (var item in componentModelFields)
            {
                if (!Add(item, tran))
                {
                    errmsg = string.Format("页面字段添加[{0}]失败", item.ComponentField);
                    tran.Rollback();
                    return false;
                }
            }
            tran.Commit();
            return true;
        }

        /// <summary>
        /// 修改页面
        /// </summary>
        /// <param name="componentModel"></param>
        /// <param name="componentModelFields"></param>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public bool UpdateComponentModel(ComponentModel UpdateComponentModel, List<ComponentModelField> AddComponentModelFields
            , List<ComponentModelField> UpdateComponentModelFields, List<ComponentModelField> DeleteComponentModelFields, out string errmsg)
        {
            BLLTransaction tran = new BLLTransaction();
            errmsg = "";
            if (!Update(UpdateComponentModel, tran))
            {
                errmsg = "页面修改失败";
                tran.Rollback();
                return false;
            }
            foreach (var item in DeleteComponentModelFields)
            {
                if (Delete(item, tran) <= 0)
                {
                    errmsg = string.Format("页面字段删除[{0}]失败", item.ComponentField);
                    tran.Rollback();
                    return false;
                }
            }
            foreach (var item in UpdateComponentModelFields)
            {
                if (!Update(item, tran))
                {
                    errmsg = string.Format("页面字段修改[{0}]失败", item.ComponentField);
                    tran.Rollback();
                    return false;
                }
            }
            foreach (var item in AddComponentModelFields)
            {
                if (!Add(item, tran))
                {
                    errmsg = string.Format("页面字段添加[{0}]失败", item.ComponentField);
                    tran.Rollback();
                    return false;
                }
            }
            tran.Commit();
            return true;
        }
        /// <summary>
        /// 根据组件代码查出页面设置
        /// </summary>
        /// <param name="componentKey"></param>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public Component GetComponentByKey(string componentKey, string websiteOwner)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" ComponentKey='{0}' ", componentKey);
            sbSql.AppendFormat(" AND WebsiteOwner='{0}' ", websiteOwner);
            return Get<Component>(sbSql.ToString());
        }
        /// <summary>
        /// 获取本站最新页面
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public Component GetNewComponent(string websiteOwner)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(" WebsiteOwner='{0}' Order by AutoId desc ", websiteOwner);
            return Get<Component>(sbSql.ToString());
        }

        #region 系统组件 添加
        public string GetComponentNameByKey(string key)
        {
            if (key == "PersonalCenter")
            {
                return "个人中心";
            }
            else if (key == "MallHome")
            {
                return "商城首页";
            }
            return "";
        }
        /// <summary>
        /// 添加站点系统页面
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <returns></returns>
        public void AddSysComponents(string websiteOwner, JObject sysJson)
        {
            foreach (JProperty item in sysJson.Properties())
            {
                if (sysJson[item.Name] == null) continue;
                AddSysComponent(item.Name, websiteOwner, sysJson[item.Name]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentKey"></param>
        /// <param name="websiteOwner"></param>
        private void AddSysComponent(string componentKey, string websiteOwner, JToken sysJson)
        {
            Component nComponent = GetComponentByKey(componentKey, websiteOwner);
            if (nComponent != null) return;
            nComponent = new Component();
            nComponent.ComponentKey = componentKey;
            if (sysJson["ComponentName"] != null) nComponent.ComponentName = sysJson["ComponentName"].ToString();
            if (sysJson["ComponentModelId"] != null) nComponent.ComponentModelId = Convert.ToInt32(sysJson["ComponentModelId"]);
            if (sysJson["ComponentType"] != null) nComponent.ComponentType = sysJson["ComponentType"].ToString();
            if (sysJson["ComponentConfig"] != null) nComponent.ComponentConfig = JsonConvert.SerializeObject(sysJson["ComponentConfig"]);
            if (sysJson["IsWXSeniorOAuth"] != null) nComponent.IsWXSeniorOAuth = Convert.ToInt32(sysJson["IsWXSeniorOAuth"]);
            nComponent.WebsiteOwner = websiteOwner;
            Add(nComponent);
        }
        #endregion

        #region 页面模板

        public List<ComponentTemplate> GetTemplateList(int rows, int page, string keyword, out int total, string cate)
        {
            StringBuilder sbWhere = new StringBuilder("1=1");
            if (!string.IsNullOrWhiteSpace(keyword)) sbWhere.AppendFormat(" And Name like '%{0}%'", keyword);
            if (!string.IsNullOrWhiteSpace(cate)) sbWhere.AppendFormat(" And CateId ='{0}'", cate);
            total = GetCount<ComponentTemplate>(sbWhere.ToString());
            return GetColList<ComponentTemplate>(rows, page, sbWhere.ToString(), "Sort desc,AutoId desc", "AutoId,Name,ThumbnailsPath,FromWebsite,Sort,CateId");
        }
        #endregion
    }
}