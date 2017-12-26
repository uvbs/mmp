using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.SVCard.Wap
{
    public partial class UseRecord : System.Web.UI.Page
    {
        /// <summary>
        /// 使用记录
        /// </summary>
         public  List<StoredValueCardUseRecord> UseRecordList = new List<StoredValueCardUseRecord>();
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLStoredValueCard bllStoreValueCard = new BLLJIMP.BLLStoredValueCard();

        protected void Page_Load(object sender, EventArgs e)
        {

            UseRecordList = bllStoreValueCard.GetList<StoredValueCardUseRecord>(string.Format("WebsiteOwner='{0}' And MyCardId={1} And UseUserId='{2}'", bllStoreValueCard.WebsiteOwner, Request["id"], bllStoreValueCard.GetCurrUserID()));
        }
    }
}