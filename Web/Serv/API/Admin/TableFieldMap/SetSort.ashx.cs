using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.TableFieldMap
{
    /// <summary>
    /// 设置排序
    /// </summary>
    public class SetSort : BaseHandlerNeedLoginAdminNoAction
    {

        BLLTableFieldMap bll = new BLLTableFieldMap();
        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request["id"];
            string sort = context.Request["sort"];
            string other_id = context.Request["other_id"];
            string other_sort = context.Request["other_sort"];
            if (sort == other_sort)
                other_sort = (int.Parse(other_sort) + 1).ToString();
            
            BLLTransaction tran = new BLLTransaction();
            if(bll.UpdateByKey<TableFieldMapping>("AutoId",id,"Sort",sort,tran)>0
                && bll.UpdateByKey<TableFieldMapping>("AutoId", other_id, "Sort", other_sort, tran) > 0)
            {
                tran.Commit();
                apiResp.status = true;
                apiResp.msg = "排序完成";
                apiResp.code = (int)APIErrCode.IsSuccess;
            }
            else
            {
                tran.Rollback();
                apiResp.status = true;
                apiResp.msg = "排序失败";
                apiResp.code = (int)APIErrCode.OperateFail;
            }
            bll.ContextResponse(context, apiResp);
        }
    }
}