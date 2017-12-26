using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 字段映射表BLL
    /// </summary>
    public class BLLTableFieldMap : BLL
    {
        /// <summary>
        /// 获取当前站点所有字段映射
        /// </summary>
        /// <returns></returns>
        public List<TableFieldMapping> GetTableFieldMap()
        {
            return GetList<TableFieldMapping>(string.Format(" WebSiteOwner='{0}'", WebsiteOwner));
        }
        private string GetWhereParamString(string websiteOwner, string tableName, string foreignkeyId, string field, bool? showDelete, string mappingType = "0", string foreignkeyId1 = null, string fields = "")
        {
            StringBuilder sbSql = new StringBuilder();
            if(string.IsNullOrWhiteSpace(websiteOwner)){
                sbSql.AppendFormat(" WebSiteOwner Is Null");
            }
            else{
                sbSql.AppendFormat(" WebSiteOwner='{0}'",websiteOwner);
            }
            if (!string.IsNullOrWhiteSpace(tableName)) sbSql.AppendFormat(" AND TableName='{0}' ", tableName);
            if (!string.IsNullOrWhiteSpace(foreignkeyId)) sbSql.AppendFormat(" AND ForeignkeyId='{0}' ", foreignkeyId);
            if (!string.IsNullOrWhiteSpace(field)) sbSql.AppendFormat(" AND Field='{0}' ", field);
            if (!string.IsNullOrWhiteSpace(fields)) sbSql.AppendFormat(" AND Field In ({0}) ", fields);
            if (showDelete.HasValue && showDelete.Value == false) sbSql.AppendFormat(" AND IsDelete='0' ");
            if (string.IsNullOrWhiteSpace(mappingType)) { 
                sbSql.AppendFormat(" AND MappingType=0 ", mappingType); 
            }
            else
            {
                sbSql.AppendFormat(" AND MappingType={0} ", mappingType); 
            }
            if (!string.IsNullOrEmpty(foreignkeyId))
            {
                sbSql.AppendFormat(" AND ForeignkeyIdEx1='{0}' ", foreignkeyId1);
            }
            sbSql.AppendFormat(" Order by Sort ");
            return sbSql.ToString();
        }
        /// <summary>
        /// 获取指定站点所有字段映射
        /// </summary>
        /// <param name="websiteOwner">站点所有者</param>
        /// <returns></returns>
        public List<TableFieldMapping> GetTableFieldMap(string websiteOwner)
        {
            return GetTableFieldMap(websiteOwner, null, null, null, false);
        }
        public List<TableFieldMapping> GetTableFieldMap(string websiteOwner, string tableName)
        {
            return GetTableFieldMap(websiteOwner, tableName, null, null,false);
        }
        public List<TableFieldMapping> GetTableFieldMap(string websiteOwner, string tableName = null, string foreignkeyId = null, string field = null, bool? showDelete = false, string mappingType = "0", string colName = null, string foreignkeyId1 = null, string fields = "")
        {
            if (string.IsNullOrWhiteSpace(colName))
            {
                return GetList<TableFieldMapping>(GetWhereParamString(websiteOwner, tableName, foreignkeyId, field, showDelete, mappingType, foreignkeyId1: foreignkeyId1, fields: fields));
            }
            return GetColList<TableFieldMapping>(int.MaxValue, 1, GetWhereParamString(websiteOwner, tableName, foreignkeyId, field, showDelete, mappingType, fields), colName);
        }
        public List<TableFieldMapping> GetTableFieldMapByTableName(string websiteOwner, string tableName)
        {
            return GetTableFieldMap(websiteOwner, tableName, null, null, false);
        }
        /// <summary>
        /// 获取站点配置
        /// </summary>
        /// <param name="websiteOwner"></param>
        /// <param name="tableName"></param>
        /// <param name="foreignkeyId"></param>
        /// <param name="field"></param>
        /// <param name="mappingType"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public List<TableFieldMapping> GetTableFieldMapByWebsite(string websiteOwner, string tableName = null, string foreignkeyId = null, string field = null, string mappingType = "0", string colName = null, string fields = "")
        {
            List<TableFieldMapping> list = GetTableFieldMap(websiteOwner, tableName, foreignkeyId, field, false, mappingType, colName, fields:fields);
            if (!string.IsNullOrWhiteSpace(mappingType) && mappingType != "0" && list.Count == 0) list = GetTableFieldMap(websiteOwner, tableName, foreignkeyId, field, false, "0", colName, "", fields: fields);
            if (!string.IsNullOrWhiteSpace(websiteOwner) && list.Count == 0) list = GetTableFieldMap(null, tableName, foreignkeyId, field, false, mappingType, colName, fields: fields);
            if (list.Count == 0) list = GetTableFieldMap(null, tableName, foreignkeyId, field, false, "0", colName, fields: fields);
            return list;
        }
        /// <summary>
        /// 获取指定站点 指定字段类型映射
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public List<TableFieldMapping> GetTableFieldMap(EnumTableFieldType fieldType)
        {
            return GetList<TableFieldMapping>(string.Format(" WebSiteOwner='{0}'", WebsiteOwner, fieldType));
        }
        /// <summary>
        /// 添加字段类型映射
        /// </summary>
        /// <param name="tableFieldMap">实体</param>
        /// <returns></returns>
        public bool AddTableFieldMap(TableFieldMapping tableFieldMap)
        {

            return Add(tableFieldMap);

        }
        /// <summary>
        /// 添加字段类型映射
        /// </summary>
        /// <param name="tableFieldMap">实体</param>
        /// <param name="fieldType">字段类型枚举</param>
        /// <returns></returns>
        public bool AddTableFieldMap(TableFieldMapping tableFieldMap, EnumTableFieldType fieldType)
        {
            tableFieldMap.FieldType = ((int)fieldType).ToString();
            return Add(tableFieldMap);
        }

        public string GetTableFieldName(string defName,string websiteOwner, string tableName, string field, string mappingType = "0")
        {
            TableFieldMapping tbField = Get<TableFieldMapping>(GetWhereParamString(websiteOwner, tableName, null, field, false, mappingType));
            if (tbField == null) return defName;
            return tbField.MappingName;
        }

        public string GetTableFieldName(string defName, string field, List<TableFieldMapping> tbFieldList)
        {
            TableFieldMapping tbField = tbFieldList.FirstOrDefault(p => p.Field == field);
            if (tbField == null) return defName;
            return tbField.MappingName;
        }
        public bool GetTableFieldIsShow(bool defShow, string field, List<TableFieldMapping> tbFieldList)
        {
            TableFieldMapping tbField = tbFieldList.FirstOrDefault(p => p.Field == field);
            if (tbField == null) return defShow;
            return tbField.IsShowInList == 1;
        }
        
    }
}
