using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.PermissionColumn
{
    /// <summary>
    /// SetPermissionColumnCheckedList 的摘要说明
    /// </summary>
    public class SetPermissionColumnCheckedList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            string group_id = context.Request["group_id"];
            string col_ids = context.Request["col_ids"];
            if (string.IsNullOrWhiteSpace(col_ids)) col_ids = "0";
            List<long> col_id_list = col_ids.Split(',').Select(p => Convert.ToInt64(p)).ToList();
            List<BLLPermission.Model.PermissionRelationInfo> relOList = bllPermission.GetPermissionRelationList(group_id, 3);
            List<long> colo_id_list = relOList.Select(p => p.PermissionID).ToList();
            List<BLLPermission.Model.PermissionRelationInfo> relDelList = relOList.Where(p => !col_id_list.Contains(p.PermissionID)).ToList();
            List<BLLPermission.Model.PermissionRelationInfo> relAddList = new List<BLLPermission.Model.PermissionRelationInfo>();
            foreach (long item in col_id_list.Where(p => !colo_id_list.Contains(p) && p!=0))
            {
                BLLPermission.Model.PermissionRelationInfo nPerRel = new BLLPermission.Model.PermissionRelationInfo();
                nPerRel.RelationType = 3;
                nPerRel.PermissionID = item;
                nPerRel.RelationID = group_id;
                relAddList.Add(nPerRel);
            }
            BLLTransaction tran = new BLLTransaction();
            foreach (BLLPermission.Model.PermissionRelationInfo item in relDelList)
            {
                if (bllPermission.Delete(item, tran) < 0)
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "删除原栏目失败。";
                    bllPermission.ContextResponse(context, apiResp);
                    return;
                }
            }
            foreach (BLLPermission.Model.PermissionRelationInfo item in relAddList)
            {
                if (!bllPermission.Add(item, tran))
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "保存新栏目失败。";
                    bllPermission.ContextResponse(context, apiResp);
                    return;
                }
            }
            tran.Commit();
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "提交完成。";
            bllPermission.ContextResponse(context, apiResp);
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