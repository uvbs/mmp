using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Permission
{
    /// <summary>
    /// SetMenuChecked 的摘要说明
    /// </summary>
    public class SetMenuCheckedList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLMenuInfo bllMenu = new BLLPermission.BLLMenuInfo();
        public void ProcessRequest(HttpContext context)
        {
            string relation_id = context.Request["relation_id"];
            string menu_ids = context.Request["menu_ids"];
            if (string.IsNullOrWhiteSpace(menu_ids)) menu_ids = "0";
            List<long> menu_id_list = menu_ids.Split(',').Select(p => Convert.ToInt64(p)).ToList();
            List<BLLPermission.Model.MenuRelationInfo> relOList = bllMenu.GetMenuRelationList(relation_id, 5);
            List<long> menuo_id_list = relOList.Select(p => p.MenuID).ToList();
            List<BLLPermission.Model.MenuRelationInfo> relDelList = relOList.Where(p => !menu_id_list.Contains(p.MenuID)).ToList();
            List<BLLPermission.Model.MenuRelationInfo> relAddList = new List<BLLPermission.Model.MenuRelationInfo>();
            foreach (long item in menu_id_list.Where(p => !menuo_id_list.Contains(p) && p != 0))
            {
                BLLPermission.Model.MenuRelationInfo nPerRel = new BLLPermission.Model.MenuRelationInfo();
                nPerRel.RelationType = 5;
                nPerRel.MenuID = item;
                nPerRel.RelationID = relation_id;
                relAddList.Add(nPerRel);
            }
            BLLTransaction tran = new BLLTransaction();
            foreach (BLLPermission.Model.MenuRelationInfo item in relDelList)
            {
                if (bllMenu.Delete(item, tran) < 0)
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "删除原菜单失败。";
                    bllMenu.ContextResponse(context, apiResp);
                    return;
                }
            }
            foreach (BLLPermission.Model.MenuRelationInfo item in relAddList)
            {
                if (!bllMenu.Add(item, tran))
                {
                    tran.Rollback();
                    apiResp.code = (int)APIErrCode.OperateFail;
                    apiResp.msg = "保存新菜单失败。";
                    bllMenu.ContextResponse(context, apiResp);
                    return;
                }
            }
            tran.Commit();
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "保存完成。";
            bllMenu.ContextResponse(context, apiResp);
        }

    }
}