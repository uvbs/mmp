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
    /// 表映射
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLTableFieldMap bll = new BLLTableFieldMap();
        public void ProcessRequest(HttpContext context)
        {
            TableFieldMapping model = new TableFieldMapping();
            model =bll.ConvertRequestToModel<TableFieldMapping>(model);
            if(string.IsNullOrWhiteSpace(model.WebSiteOwner)) model.WebSiteOwner = bll.WebsiteOwner;

            if(string.IsNullOrWhiteSpace(model.TableName)){
                apiResp.msg = "表名不能为空";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (string.IsNullOrWhiteSpace(model.Field))
            {
                apiResp.msg = "字段不能为空";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            if (!bll.ExistsTableField(model.TableName, model.Field))
            {
                apiResp.msg = "字段不存在";
                apiResp.code = (int)APIErrCode.OperateFail;
                bll.ContextResponse(context, apiResp);
                return;
            }
            //所增字段，是否存在删除的记录
            List<TableFieldMapping> oldList = bll.GetTableFieldMap(bll.WebsiteOwner, model.TableName, model.ForeignkeyId, model.Field, true, model.MappingType.ToString(), colName:"AutoId,Sort,IsDelete");
            if (oldList.Count > 0)
            {
                if(oldList.Where(p=>p.IsDelete==0).Count()>0){
                    apiResp.msg = "字段重复";
                    apiResp.code = (int)APIErrCode.OperateFail;
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                else
                {
                    model.AutoId = oldList[0].AutoId;
                    model.Sort = oldList[0].Sort;
                }
            }
            else
            {
                //检查最大排序
                oldList = bll.GetTableFieldMap(bll.WebsiteOwner, model.TableName, null, null, true, model.MappingType.ToString(), colName: "AutoId,Sort");
                if (oldList.Count == 0)
                {
                    model.Sort = 1;
                }
                else
                {
                    model.Sort = oldList.Max(p => p.Sort) + 1;
                }
            }

            if (model.AutoId > 0)
            {
                if (bll.Update(model))
                {
                    apiResp.status = true;
                    apiResp.msg = "提交完成，原字段存在禁用记录，继承原记录排序";
                    apiResp.code = (int)APIErrCode.IsSuccess;
                }
                else
                {
                    apiResp.msg = "提交失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                }
            }
            else
            {
                if (bll.Add(model))
                {
                    apiResp.status = true;
                    apiResp.msg = "新增完成";
                    apiResp.code = (int)APIErrCode.IsSuccess;
                }
                else
                {
                    apiResp.msg = "新增失败";
                    apiResp.code = (int)APIErrCode.OperateFail;
                }
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}