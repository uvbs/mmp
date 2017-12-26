using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Weixin
{
    public partial class WeixinFollowersInfoMgr : System.Web.UI.Page
    {
        public BLLJIMP.Model.TimingTask model = new BLLJIMP.Model.TimingTask();

        BLLJIMP.BLLTimingTask bllTask = new BLLJIMP.BLLTimingTask();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLPermission.BLLMenuPermission bllMenupermission = new BLLPermission.BLLMenuPermission("");

        protected bool IsShowSendTempMessage = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            model = bllTask.GetLastTinmingTask(BLLJIMP.BLLTimingTask.TaskType.SynFans);

            IsShowSendTempMessage = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.SendTtemplatMessage);



        }
    }
}