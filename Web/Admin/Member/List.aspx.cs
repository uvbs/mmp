using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.Member
{
    public partial class List : System.Web.UI.Page
    {
        protected bool canLockMember = false;
        protected bool canResetMemberPwd = false;
        protected bool canCancelMemberRegister = false;
        protected bool canUpdateDistributionOwner = false;
        protected bool canUpdateMemberInfo = false;
        protected bool canUpdateLoginPhone = false;
        protected bool canMemberExport = false;
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser("");
        BLLPermission.BLLMenuPermission bll = new BLLPermission.BLLMenuPermission("");
        protected void Page_Load(object sender, EventArgs e)
        {
            string curUserID = bllUser.GetCurrUserID();
            canLockMember = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.LockMember);
            canResetMemberPwd = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.ResetMemberPwd);
            canCancelMemberRegister = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.CancelMemberRegister);
            canUpdateDistributionOwner = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.UpdateDistributionOwner);
            canUpdateMemberInfo = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.UpdateMemberInfo);
            canUpdateLoginPhone = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.UpdateLoginPhone);
            canMemberExport = bll.CheckUserAndPmsKey(curUserID, BLLPermission.Enums.PermissionSysKey.MemberExport);
        }
    }
}