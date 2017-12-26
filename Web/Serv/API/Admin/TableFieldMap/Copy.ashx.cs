using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TableFieldMap
{
    /// <summary>
    /// 复制表映射
    /// </summary>
    public class Copy : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTableFieldMap bll = new BLLTableFieldMap();
        public void ProcessRequest(HttpContext context)
        {
            string ids = context.Request["ids"];
            string mapping_type = context.Request["mapping_type"];
            string t_mapping_type = context.Request["t_mapping_type"];
            int tMappingType = int.Parse(t_mapping_type);
            string table_name = context.Request["table_name"];

            if (mapping_type == t_mapping_type)
            {
                apiResp.msg = "不能复制到本身类型";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }

            List<string> sIdList = ids.Split(',').Where(p => !string.IsNullOrWhiteSpace(p)).ToList();
            if (sIdList.Count==0)
            {
                apiResp.msg = "请选择字段";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            //根据ID查出数据
            List<TableFieldMapping> sList = bll.GetTableFieldMap(bll.WebsiteOwner, table_name, null, null, true, mapping_type);
            sList = sList.Where(p => ids.Contains(p.AutoId.ToString())).ToList();
            List<string> sFieldList = sList.Select(p => p.Field).Distinct().ToList();

            if (sList.Count == 0)
            {
                apiResp.msg = "字段未找到！";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }

            //查出目标类型已有字段
            List<TableFieldMapping> mList = bll.GetTableFieldMap(bll.WebsiteOwner, table_name, null, null, true, t_mapping_type);
            //查出已有字段
            List<TableFieldMapping> hList = mList.Where(p => sFieldList.Contains(p.Field) && p.IsDelete == 0).ToList();
            List<TableFieldMapping> dList = mList.Where(p => sFieldList.Contains(p.Field) && p.IsDelete == 1).ToList();
            List<string> hFieldList = new List<string>();
            List<string> dFieldList = new List<string>();
            if (hList.Count > 0) hFieldList = hList.Select(p => p.Field).Distinct().ToList();
            if (dList.Count > 0) dFieldList = dList.Select(p => p.Field).Distinct().ToList();

            //排除已有字段
            List<TableFieldMapping> cList = sList.Where(p => !hFieldList.Contains(p.Field) && !dFieldList.Contains(p.Field)).ToList();

            if (cList.Count == 0 && dList.Count ==0)
            {
                apiResp.msg = "要复制的字段目标类型已全部存在！";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }

            foreach (TableFieldMapping item in dList)
            {
                TableFieldMapping citem = sList.FirstOrDefault(p=>p.Field == item.Field);
                item.MappingName = citem.MappingName;
                item.FieldIsNull = citem.FieldIsNull;
                item.FieldType = citem.FieldType;
                item.FormatValiFunc = citem.FormatValiFunc;
                item.IsShowInList = citem.IsShowInList;
                item.IsReadOnly = citem.IsReadOnly;
                item.IsDelete = citem.IsDelete;
                item.ForeignkeyId = citem.ForeignkeyId;
                item.Sort = citem.Sort;
                bll.Update(item);
            }
            foreach (TableFieldMapping item in cList)
            {
                item.MappingType = tMappingType;
                bll.Add(item);
            }

            if (hFieldList.Count > 0)
            {
                apiResp.msg = "[" + ZentCloud.Common.MyStringHelper.ListToStr(hFieldList,"",",") + "]已存在，其他复制完成";
            }
            else
            {
                apiResp.msg = "复制完成";
            }
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
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