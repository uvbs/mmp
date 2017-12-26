using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLPermission
{
    public class PermissionKey
    {
        static BLLMenuPermission bll = new BLLMenuPermission("");

        //#region 微博平台
        ///// <summary>
        ///// 查看所有微博数据权限
        ///// </summary>
        //public static long Pms_Weibo_ViewAllCollectData = bll.GetPmsByPmsKey("Pms_Weibo_ViewAllCollectData").PermissionID;


        //#endregion


        //#region 系统设置

        //#region 菜单管理
        ///// <summary>
        ///// 系统设置-菜单管理-添加菜单权限
        ///// </summary>
        //public static long Pms_Menu_AddMenu = bll.GetPmsByPmsKey("Pms_Menu_AddMenu").PermissionID;
        ///// <summary>
        ///// 系统设置-菜单管理-编辑菜单权限
        ///// </summary>
        //public static long Pms_Menu_EditMenu = bll.GetPmsByPmsKey("Pms_Menu_EditMenu").PermissionID;
        ///// <summary>
        ///// 系统设置-菜单管理-删除菜单权限
        ///// </summary>
        //public static long Pms_Menu_DeleteMenu = bll.GetPmsByPmsKey("Pms_Menu_DeleteMenu").PermissionID;


        //#endregion

        //#region 权限管理
        ///// <summary>
        ///// 系统设置-权限管理-添加权限
        ///// </summary>
        //public static long Pms_Permission_AddPermission = bll.GetPmsByPmsKey("Pms_Permission_AddPermission").PermissionID;
        ///// <summary>
        ///// 系统设置-权限管理-编辑权限
        ///// </summary>
        //public static long Pms_Permission_EditPermission = bll.GetPmsByPmsKey("Pms_Permission_EditPermission").PermissionID;
        ///// <summary>
        ///// 系统设置-权限管理-删除权限
        ///// </summary>
        //public static long Pms_Permission_DeletePermission = bll.GetPmsByPmsKey("Pms_Permission_DeletePermission").PermissionID;


        //#endregion

        //#region 权限组管理
        ///// <summary>
        ///// 系统设置-权限组管理-添加权限组
        ///// </summary>
        //public static long Pms_PermissionGroup_AddPermissionGroup = bll.GetPmsByPmsKey("Pms_PermissionGroup_AddPermissionGroup").PermissionID;
        ///// <summary>
        ///// 系统设置-权限组管理-编辑权限组
        ///// </summary>
        //public static long Pms_PermissionGroup_EditPermissionGroup = bll.GetPmsByPmsKey("Pms_PermissionGroup_EditPermissionGroup").PermissionID;
        ///// <summary>
        ///// 系统设置-权限组管理-删除权限组
        ///// </summary>
        //public static long Pms_PermissionGroup_DeletePermissionGroup = bll.GetPmsByPmsKey("Pms_PermissionGroup_DeletePermissionGroup").PermissionID;

        ///// <summary>
        ///// 系统设置-权限组管理V2-权限分配
        ///// </summary>
        //public static long Pms_PermissionGroup_AssignationPermissionGroup = bll.GetPmsByPmsKey("Pms_PermissionGroup_AssignationPermissionGroup").PermissionID;


        //#endregion

        //#endregion

        ///// <summary>
        ///// 微博-至云大数据库搜索-高级管理
        ///// </summary>
        //public static long Pms_Weibo_CCDBSearch_Advanced = bll.GetPmsByPmsKey("Pms_Weibo_CCDBSearch_Advanced").PermissionID;

        ///// <summary>
        ///// 微博-微博转发任务-高级管理
        ///// </summary>
        //public static long Pms_Weibo_RepostPlan_Advanced = bll.GetPmsByPmsKey("Pms_Weibo_RepostPlan_Advanced").PermissionID;

        ///// <summary>
        ///// 微博-微博评论任务-高级管理
        ///// </summary>
        //public static long Pms_Weibo_CommentsPlan_Advanced = bll.GetPmsByPmsKey("Pms_Weibo_CommentsPlan_Advanced").PermissionID;

        ///// <summary>
        ///// 聚活动-高级管理
        ///// </summary>
        //public static long Pms_JuActivity_Advanced = bll.GetPmsByPmsKey("Pms_JuActivity_Advanced").PermissionID;

        //#region 聚比特手机网站
        ///// <summary>
        ///// 聚比特手机网站-会员个人中心
        ///// </summary>
        //public static long Pms_JuActivity_Wap_JuUserCenterPage = bll.GetPmsByPmsKey("Pms_JuActivity_Wap_JuUserCenterPage").PermissionID;

        ///// <summary>
        ///// 聚比特手机网站-会员专家高级功能
        ///// </summary>
        //public static long Pms_JuActivity_Wap_JuMaster = bll.GetPmsByPmsKey("Pms_JuActivity_Wap_JuMaster").PermissionID;

        //#endregion


        ///// <summary>
        ///// 查看所有微信注册会员信息
        ///// </summary>
        //public static long Pms_WX_ViewAllMember = bll.GetPmsByPmsKey("Pms_WX_ViewAllMember").PermissionID;

        ///// <summary>
        ///// 乐分享-管理员
        ///// </summary>
        //public static long Pms_FShare_Manager = bll.GetPmsByPmsKey("Pms_FShare_Manager").PermissionID;

        ///// <summary>
        ///// 鸿风-管理员
        ///// </summary>
        //public static long Pms_Hongfeng_Admin = bll.GetPmsByPmsKey("Pms_Hongfeng_Admin").PermissionID;

        ///// <summary>
        ///// 鸿风-vip用户
        ///// </summary>
        //public static long Pms_Hongfeng_VIPUser = bll.GetPmsByPmsKey("Pms_Hongfeng_VIPUser").PermissionID;

        ///// <summary>
        ///// 鸿风-教师
        ///// </summary>
        //public static long Pms_Hongfeng_Teacher = bll.GetPmsByPmsKey("Pms_Hongfeng_Teacher").PermissionID;

        ///// <summary>
        ///// 鸿风微学堂-手机端-问答回复
        ///// </summary>
        //public static long Pms_Hongfeng_Wap_QuestionDialog = bll.GetPmsByPmsKey("Pms_Hongfeng_Wap_QuestionDialog").PermissionID;

        ///// <summary>
        ///// 鸿风微学堂-手机端-问答回复-只能回复自己的问题
        ///// </summary>
        //public static long Pms_Hongfeng_Wap_QuestionDialog_OnlyOwn = bll.GetPmsByPmsKey("Pms_Hongfeng_Wap_QuestionDialog_OnlyOwn").PermissionID;

        //Pms_Hongfeng_Wap_QuestionDialog_OnlyOwn

        //Pms_Hongfeng_Wap_QuestionDialog

        //Pms_Hongfeng_Teacher

        //Pms_Hongfeng_VIPUser

        //Pms_Hongfeng_Admin

        //Pms_JuActivity_Advanced

        //Pms_Weibo_RepostPlan_Advanced
        //Pms_Weibo_CommentsPlan_Advanced

        //Pms_Weibo_CCDBSearch_Advanced
    }
}
