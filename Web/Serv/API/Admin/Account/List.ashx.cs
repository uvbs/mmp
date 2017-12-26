using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Account
{
    /// <summary>
    /// 账户列表
    /// </summary>
    public class List : BaseHandlerNeedLoginAdminNoAction
    {
        BLLUser bllUser = new BLLUser();
        BLLPermission.BLLPermission bllPms = new BLLPermission.BLLPermission();
        public void ProcessRequest(HttpContext context)
        {
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string userId = context.Request["user_id"];
            string trueName = context.Request["true_name"];
            string groupId = context.Request["group_id"];
            string isDisble = context.Request["is_disable"];
            List<UserInfo> dataList = new List<UserInfo>();
            List<BLLPermission.Model.PermissionGroupInfo> groupList = new List<BLLPermission.Model.PermissionGroupInfo>();
            if (!string.IsNullOrEmpty(groupId))
            {
                List<BLLPermission.Model.UserPmsGroupRelationInfo> pmsGroupRelList = bllPms.GetUserPmsGroupRelList(groupId);
                if (pmsGroupRelList.Count > 0)
                {
                    List<string> userIdList = pmsGroupRelList.Select(p => p.UserID).ToList();
                    dataList = bllUser.GetSubAccountList(bllUser.WebsiteOwner, userId, trueName);
                    dataList = dataList.Where(p => userIdList.Contains(p.UserID)).ToList();
                }
            }
            else{
                dataList = bllUser.GetSubAccountList(bllUser.WebsiteOwner, userId, trueName, isDisble);
            }
            
            if (dataList.Count > 0)
            {
                groupList = bllPms.GetGroupList(int.MaxValue, 1, null, bllUser.WebsiteOwner, null, 2);
                groupList.AddRange(bllPms.GetGroupList(pageSize, pageIndex, "", "common", null, 3));
                groupList.AddRange(bllPms.GetGroupList(pageSize, pageIndex, "", "common", null, 4));
                if(currentUserInfo.UserType != 1 && currentUserInfo.UserID != bllUser.WebsiteOwner){
                    List<long> childIdList = new List<long>();
                    if (currentUserInfo.PermissionGroupID.HasValue)
                    {
                        childIdList = bllPms.GetAllChildGroupIdList(groupList, currentUserInfo.PermissionGroupID.Value);
                    }
                    dataList = dataList.Where(p => p.UserID == currentUserInfo.UserID || (p.PermissionGroupID.HasValue && childIdList.Contains(p.PermissionGroupID.Value))).ToList();
                }
            }

            int total = dataList.Count;
            dataList = dataList.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            List<dynamic> dataResult = new List<dynamic>();
            if (dataList.Count > 0)
            {
                //先注释多角色
                //string userIds = ZentCloud.Common.MyStringHelper.ListToStr(dataList.Select(p => p.UserID).ToList(),"'",",");
                //List<BLLPermission.Model.UserPmsGroupRelationInfo> userPmsGroupRelList = bllPms.GetUserPmsGroupRelListByMultUserId(userIds);
                foreach (var item in dataList)
                {
                    //string roleName = bllPms.GetRoleName(groupList,userPmsGroupRelList,item.UserID);
                    long nGroupId = item.PermissionGroupID.HasValue ? item.PermissionGroupID.Value : 0;
                    var nRole = groupList.FirstOrDefault(p => p.GroupID == nGroupId);
                    string nRoleName = nRole == null? "":nRole.GroupName;
                    dataResult.Add(
                        new
                        {
                            id = item.AutoID,
                            username = item.UserID,
                            truename = item.TrueName,
                            avatar = item.WXHeadimgurl,
                            company = item.Company,
                            postion = item.Postion,
                            phone = item.Phone,
                            email = item.Email,
                            pre_id = item.PermissionGroupID.HasValue?item.PermissionGroupID.Value:0,
                            is_disable = item.IsDisable,
                            role_name = nRoleName
                        });
                }
            }

            var resp = new EasyUIResponse();
            resp.status = true;
            resp.code = (int)APIErrCode.IsSuccess;
            resp.result = new
            {
                totalcount = total,
                list = dataResult
            };
            resp.msg = "ok";

            resp.total = total;
            resp.rows = dataResult;

            bllUser.ContextResponse(context, resp);
        }
    }
}