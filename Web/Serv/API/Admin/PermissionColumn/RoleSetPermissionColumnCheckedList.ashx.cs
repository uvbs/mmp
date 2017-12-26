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
    public class RoleSetPermissionColumnCheckedList : BaseHandlerNeedLoginAdminNoAction
    {
        BLLPermission.BLLPermission bllPermission = new BLLPermission.BLLPermission();
        ZentCloud.BLLJIMP.BLL bll = new ZentCloud.BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {
            RequestModel requestModel = bll.ConvertRequestToModel<RequestModel>(new RequestModel());
            //bool isSuccess = false;
            BLLPermission.Model.PermissionGroupInfo nGroup = new BLLPermission.Model.PermissionGroupInfo();
            if (requestModel.group_id > 0)
            {
                nGroup = bll.GetByKey<BLLPermission.Model.PermissionGroupInfo>("GroupID", requestModel.group_id.ToString());
                if (nGroup == null)
                {
                    apiResp.code = (int)APIErrCode.IsNotFound;
                    apiResp.msg = "角色未找到";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                if (nGroup.GroupID == requestModel.pre_id)
                {
                    apiResp.code = (int)APIErrCode.IsNotFound;
                    apiResp.msg = "上级角色不能选择自己";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
                if (requestModel.pre_id == 0)
                {
                    nGroup.PreID = currentUserInfo.PermissionGroupID.HasValue?currentUserInfo.PermissionGroupID.Value : 0;
                }
                else
                {
                    nGroup.PreID = requestModel.pre_id;
                }
                nGroup.GroupName = requestModel.group_name;
                nGroup.GroupDescription = requestModel.group_describe;
                if (!bllPermission.Update(nGroup))
                {
                    apiResp.code = (int)APIErrCode.IsNotFound;
                    apiResp.msg = "编辑角色失败";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }
            else
            {
                nGroup.GroupID = Convert.ToInt64(bllPermission.GetGUID(ZentCloud.Common.TransacType.PermissionGroupAdd));
                nGroup.GroupName = requestModel.group_name;
                nGroup.GroupDescription = requestModel.group_describe;
                nGroup.WebsiteOwner = bllPermission.WebsiteOwner;
                nGroup.GroupType = 2;

                if (currentUserInfo.UserType != 1 && currentUserInfo.UserID != bll.WebsiteOwner && requestModel.pre_id == 0)
                {
                    if (!currentUserInfo.PermissionGroupID.HasValue)
                    {
                        apiResp.code = (int)APIErrCode.IsNotFound;
                        apiResp.msg = "新增角色失败，请联系管理员设置您的角色";
                        bll.ContextResponse(context, apiResp);
                        return;
                    }
                    nGroup.PreID = currentUserInfo.PermissionGroupID.Value;
                }
                else
                {
                    nGroup.PreID = requestModel.pre_id;
                }
                if (!bllPermission.Add(nGroup))
                {
                    apiResp.code = (int)APIErrCode.IsNotFound;
                    apiResp.msg = "新增角色失败";
                    bll.ContextResponse(context, apiResp);
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(requestModel.col_ids)) requestModel.col_ids = "0";
            List<long> col_id_list = requestModel.col_ids.Split(',').Select(p => Convert.ToInt64(p)).ToList();
            List<BLLPermission.Model.PermissionRelationInfo> relOList = bllPermission.GetPermissionRelationList(nGroup.GroupID.ToString(), 3);
            List<long> colo_id_list = relOList.Select(p => p.PermissionID).ToList();
            List<BLLPermission.Model.PermissionRelationInfo> relDelList = relOList.Where(p => !col_id_list.Contains(p.PermissionID)).ToList();
            List<BLLPermission.Model.PermissionRelationInfo> relAddList = new List<BLLPermission.Model.PermissionRelationInfo>();
            foreach (long item in col_id_list.Where(p => !colo_id_list.Contains(p) && p != 0))
            {
                BLLPermission.Model.PermissionRelationInfo nPerRel = new BLLPermission.Model.PermissionRelationInfo();
                nPerRel.RelationType = 3;
                nPerRel.PermissionID = item;
                nPerRel.RelationID = nGroup.GroupID.ToString();
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
        public class RequestModel
        {
            /// <summary>
            /// 权限组id
            /// </summary>
            public long group_id { get; set; }
            /// <summary>
            /// 权限组名称
            /// </summary>
            public string group_name { get; set; }
            /// <summary>
            /// 权限组描述
            /// </summary>
            public string group_describe { get; set; }
            /// <summary>
            /// 权限组上级id
            /// </summary>
            public long pre_id { get; set; }
            /// <summary>
            /// 栏目id
            /// </summary>
            public string col_ids { get; set; }
        }
    }
}