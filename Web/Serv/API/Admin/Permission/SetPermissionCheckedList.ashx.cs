using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Permission
{
    /// <summary>
    /// SetPermissionChecked 的摘要说明
    /// </summary>
    public class SetPermissionCheckedList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            string relation_id = context.Request["relation_id"];
            string pms_ids = context.Request["pms_ids"];
            string rel_type = context.Request["rel_type"];
            int relationType = 2;
            if (!string.IsNullOrWhiteSpace(rel_type)) relationType = int.Parse(rel_type);

            if (string.IsNullOrWhiteSpace(pms_ids)) pms_ids = "0";
            List<long> pms_id_list = pms_ids.Split(',').Select(p => Convert.ToInt64(p)).ToList();
            List<BLLPermission.Model.PermissionRelationInfo> relOList = bllPermission.GetPermissionRelationList(relation_id, relationType);
            List<long> pmso_id_list = relOList.Select(p => p.PermissionID).ToList();
            List<BLLPermission.Model.PermissionRelationInfo> relDelList = relOList.Where(p => !pms_id_list.Contains(p.PermissionID)).ToList();
            List<BLLPermission.Model.PermissionRelationInfo> relAddList = new List<BLLPermission.Model.PermissionRelationInfo>();
            foreach (long item in pms_id_list.Where(p => !pmso_id_list.Contains(p) && p != 0))
            {
                BLLPermission.Model.PermissionRelationInfo nPerRel = new BLLPermission.Model.PermissionRelationInfo();
                nPerRel.RelationType = relationType;
                nPerRel.PermissionID = item;
                nPerRel.RelationID = relation_id;
                relAddList.Add(nPerRel);
            }
            BLLTransaction tran = new BLLTransaction();
            foreach (BLLPermission.Model.PermissionRelationInfo item in relDelList)
            {
                if (bllPermission.Delete(item, tran) < 0) {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "删除原权限关系失败。";
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
                    apiResp.msg = "保存新权限关系失败。";
                    bllPermission.ContextResponse(context, apiResp);
                    return;
                }
            }
            tran.Commit();
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "保存完成。";
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