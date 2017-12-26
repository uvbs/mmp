using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLPermission;

namespace ZentCloud.JubitIMP.Web.Admin.Meifan.Order
{
    public partial class List : System.Web.UI.Page
    {
        BLLPermission.BLLMenuPermission bllMenupermission = new BLLMenuPermission("");
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLMeifan bll = new BLLJIMP.BLLMeifan();
        /// <summary>
        /// 添加权限
        /// </summary>
        public bool PmsAdd = false;
        public List<MeifanCard> cardList = new List<MeifanCard>();
        protected void Page_Load(object sender, EventArgs e)
        {
            PmsAdd = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.PMS_MFCARD_ADDMYCARD);
            int totalCount;
            cardList = bll.CardList(1, int.MaxValue, "", "", out totalCount);

        }
    }
}