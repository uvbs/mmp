using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLPermission;
using ZentCloud.BLLPermission.Model;
using System.Text;
using System.Web.SessionState;
using ZentCloud.Common;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.JubitIMP.Web.Handler.Permission
{
    /// <summary>
    /// PmsGroupManager 的摘要说明
    /// </summary>
    public class PmsGroupManager : IHttpHandler, IReadOnlySessionState
    {

        /// <summary>
        /// 权限BLL
        /// </summary>
        BLLMenuPermission bllPer = new BLLMenuPermission("");
        BLLPermission.BLLPermission bllPms = new BLLPermission.BLLPermission();
        BLLPermission.BLLPermissionColumn bllPmsColumn = new BLLPermission.BLLPermissionColumn();
        BLLMenuInfo bllMenu = new BLLMenuInfo();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = 0;
            string action = context.Request["Action"];
            string result = "false";
            try
            {
                var userInfo=bllPer.GetCurrentUserInfo();
                if (bllPms.WebsiteOwner!=userInfo.UserID&&userInfo.UserType!=1)
                {

                    context.Response.Write("无权访问");
                    return;
                }
                switch (action)
                {
                    case "Add":
                        result = Add(context);
                        break;
                    case "Edit":
                        result = Edit(context);
                        break;
                    case "Delete":
                        result = Delete(context);
                        break;
                    case "Copy":
                        result = Copy(context);
                        break;
                    case "Query":
                        result = Query(context);
                        break;
                    case "SetPms":
                        result = SetPms(context);
                        break;
                    case "SetMenu":
                        result = SetMenu(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            context.Response.Write(result);
        }

        private string SetPms(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_PermissionGroup_AssignationPermissionGroup))
            //{
            //    return "无权设置权限组权限";
            //}
            //#endregion
            //传入：权限组ID集合，权限ID集合

            try
            {
                string groupIdsStr = context.Request["groupIds"];
                string pmsIdsStr = context.Request["pmsIds"];

                List<long> pmsIds = pmsIdsStr.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => long.Parse(p)).ToList();

                foreach (var item in groupIdsStr.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    bllPer.SetPmsGroupPms(long.Parse(item), pmsIds, true);
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "true";

        }

        private string Delete(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_PermissionGroup_DeletePermissionGroup))
            //{
            //    return "无权删除权限组";
            //}
            //#endregion
            string ids = context.Request["ids"];
            //删除前 删除相关权限组关联
            ids = Common.StringHelper.ListToStr<string>(ids.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(), "'", ",");
            //删除用户-权限组关系表
            if (bllPer.Delete(new UserPmsGroupRelationInfo(), string.Format("GroupID in ({0})", ids)) >= 0)
            {

            }

            //删除权限组对应菜单
            if (bllPer.Delete(new MenuRelationInfo(), string.Format("RelationID in({0}) and RelationType In (0)",
                ids)) >= 0)
            {

            }
            
            //删除权限组对应权限 对应权限栏目栏目
            if (bllPer.Delete(new PermissionRelationInfo(), string.Format("RelationID in({0}) and RelationType In ({1})",
                ids,
                "0,3")) >= 0)
            {

            }

            int result = bllPer.Delete(new PermissionGroupInfo(), string.Format(" GroupID in ({0})", ids));//pmsBll.DeleteUser(idsList);
            return result.ToString();

        }

        private string Add(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_PermissionGroup_AddPermissionGroup))
            //{
            //    return "无权添加权限组";
            //}
            //#endregion

            string jsonData = context.Request["JsonData"];
            PermissionGroupInfo model = ZentCloud.Common.JSONHelper.JsonToModel<PermissionGroupInfo>(jsonData);
            model.GroupID = long.Parse(bllPer.GetGUID(Common.TransacType.PermissionGroupAdd));
            model.PreID = 0;
            if (model.GroupType == 2)
            {
                model.WebsiteOwner = bllPer.WebsiteOwner;
            }
            bool result = bllPer.Add(model);
            return result.ToString().ToLower();
        }

        private string Edit(HttpContext context)
        {
            //#region 权限处理
            //if (!DataLoadTool.CheckCurrUserPms(PermissionKey.Pms_PermissionGroup_EditPermissionGroup))
            //{
            //    return "无权修改权限组";
            //}
            //#endregion
            string jsonData = context.Request["JsonData"];
            PermissionGroupInfo model = ZentCloud.Common.JSONHelper.JsonToModel<PermissionGroupInfo>(jsonData);
            bool result = bllPer.Update(model);
            return result.ToString().ToLower();
        }

        private string Query(HttpContext context)
        {
            StringBuilder sbWhere = new StringBuilder();
            int pageIndex = Convert.ToInt32(context.Request["page"]);
            int pageSize = Convert.ToInt32(context.Request["rows"]);
            string keyWord = context.Request["searchReq"];
            string group_type = context.Request["group_type"];
            string thisSite = context.Request["this_site"];

            sbWhere.AppendFormat(" 1=1 ");
            if (!string.IsNullOrWhiteSpace(keyWord))
                sbWhere.AppendFormat(" AND GroupName like '%{0}%' ", keyWord);

            if (!string.IsNullOrWhiteSpace(group_type)) {
                if (group_type == "0")
                    sbWhere.AppendFormat(" AND (GroupType ={0} OR GroupType Is Null) ", group_type);
                else
                    sbWhere.AppendFormat(" AND GroupType ={0} ", group_type);
            }

            if (thisSite == "1")
                sbWhere.AppendFormat(" AND (WebsiteOwner Is Null Or WebsiteOwner='common')");
            else if (thisSite == "2")
                sbWhere.AppendFormat(" AND WebsiteOwner ='{0}' ", bllPer.WebsiteOwner);

            List<PermissionGroupInfo> List = bllPer.GetLit<PermissionGroupInfo>(pageSize, pageIndex, sbWhere.ToString())
                .OrderByDescending(p=>p.GroupType).ToList();
            List<PermissionGroupInfo> dataList = new List<PermissionGroupInfo>();
            foreach (var item in List.Where(p=>p.GroupType == 1))
            {
                item.has_column = bllPms.ExistsRelation(item.GroupID.ToString().ToString(), 3);
                item.PmsIdsStr = bllPms.GetPmsStrByRelationID(item.GroupID.ToString(), 0);
                item.MenuIdsStr = bllMenu.GetMenuStrByRelationID(item.GroupID.ToString(), 0);
                item.PmsColumnIdsStr = bllPms.GetPmsStrByRelationID(item.GroupID.ToString(), 3);
                dataList.Add(item);
            }
            foreach (var item in List.Where(p => p.GroupType == 2))
            {
                item.has_column = bllPms.ExistsRelation(item.GroupID.ToString().ToString(), 3);
                item.PmsIdsStr = bllPms.GetPmsStrByRelationID(item.GroupID.ToString(), 0);
                item.MenuIdsStr = bllMenu.GetMenuStrByRelationID(item.GroupID.ToString(), 0);
                item.PmsColumnIdsStr = bllPms.GetPmsStrByRelationID(item.GroupID.ToString(), 3);
                dataList.Add(item);
            }
            foreach (var item in List.Where(p => p.GroupType == 0))
            {
                item.has_column = bllPms.ExistsRelation(item.GroupID.ToString().ToString(), 3);
                item.PmsIdsStr = bllPms.GetPmsStrByRelationID(item.GroupID.ToString(), 0);
                item.MenuIdsStr = bllMenu.GetMenuStrByRelationID(item.GroupID.ToString(), 0);
                item.PmsColumnIdsStr = bllPms.GetPmsStrByRelationID(item.GroupID.ToString(), 3);
                dataList.Add(item);
            }
            foreach (var item in List.Where(p => p.GroupType ==3))
            {
                item.has_column = bllPms.ExistsRelation(item.GroupID.ToString().ToString(), 3);
                item.PmsIdsStr = bllPms.GetPmsStrByRelationID(item.GroupID.ToString(), 0);
                item.MenuIdsStr = bllMenu.GetMenuStrByRelationID(item.GroupID.ToString(), 0);
                item.PmsColumnIdsStr = bllPms.GetPmsStrByRelationID(item.GroupID.ToString(), 3);
                dataList.Add(item);
            }
            foreach (var item in List.Where(p => p.GroupType ==4))
            {
                item.has_column = bllPms.ExistsRelation(item.GroupID.ToString().ToString(), 3);
                item.PmsIdsStr = bllPms.GetPmsStrByRelationID(item.GroupID.ToString(), 0);
                item.MenuIdsStr = bllMenu.GetMenuStrByRelationID(item.GroupID.ToString(), 0);
                item.PmsColumnIdsStr = bllPms.GetPmsStrByRelationID(item.GroupID.ToString(), 3);
                dataList.Add(item);
            }

            int totalCount = bllPer.GetCount<PermissionGroupInfo>(sbWhere.ToString());

            return Common.JSONHelper.ObjectToJson(
                new
                {
                    total = totalCount,
                    rows = dataList
                });
        }

        private string Copy(HttpContext context)
        {
            string idStr = context.Request["id"];
            long id = Convert.ToInt64(idStr);
            PermissionGroupInfo model = bllPms.GetPermissionGroup(null, id);
            string newIdStr = bllPer.GetGUID(TransacType.PermissionGroupAdd);
            model.GroupID = Convert.ToInt64(newIdStr);
            model.GroupName = model.GroupName+"—复制";
            //权限组权限关系
            List<PermissionRelationInfo> listPermissionRelation = bllPms.GetPermissionRelationList(idStr, 0);
            //权限组权限栏目关系
            List<PermissionRelationInfo> listPermissionRelation3 = bllPms.GetPermissionRelationList(idStr, 3);

            listPermissionRelation.AddRange(listPermissionRelation3);
            foreach (var item in listPermissionRelation)
            {
                item.RelationID = newIdStr;
            }
            List<MenuRelationInfo> listMenuRelation = bllMenu.GetMenuRelationList(idStr, 0);
            foreach (var item in listMenuRelation)
            {
                item.RelationID = newIdStr;
            }

            BLLTransaction tran = new BLLTransaction();
            bool result = bllPer.Add(model, tran);
            if (!result)
            {
                tran.Rollback();
                return "复制权限组失败";
            }
            foreach (var item in listPermissionRelation)
            {
                if (!bllPer.Add(item, tran))
                {
                    tran.Rollback();
                    return "复制权限组权限失败";
                }
            }
            foreach (var item in listMenuRelation)
            {
                if (!bllPer.Add(item, tran))
                {
                    tran.Rollback();
                    return "复制权限组菜单失败";
                }
            }
            tran.Commit();
            return result.ToString().ToLower();
        }

        private string SetMenu(HttpContext context)
        {
            try
            {
                string groupIdStr = context.Request["groupId"];
                string menuIdsStr = context.Request["menuIds"];

                List<MenuRelationInfo> oldMenuList = bllMenu.GetMenuRelationList(groupIdStr, 0);
                List<long> menuIds = menuIdsStr.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => long.Parse(p)).ToList();

                List<MenuRelationInfo> addList = new List<MenuRelationInfo>();
                List<MenuRelationInfo> delList = new List<MenuRelationInfo>();
                foreach (var item in menuIds)
                {
                    if (!oldMenuList.Exists(p => p.MenuID == item))
                    {
                        MenuRelationInfo temp = new MenuRelationInfo();
                        temp.MenuID = item;
                        temp.RelationID = groupIdStr;
                        temp.RelationType = 0;
                        addList.Add(temp);
                    }
                }
                foreach (var item in oldMenuList)
                {
                    if (!menuIds.Exists(p => p == item.MenuID))
                    {
                        delList.Add(item);
                    }
                }
                string msg="";
                if (bllMenu.UpdateMenuRelations(addList, delList, out msg))
                {
                    return "true";
                }
                else
                {
                    return msg;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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