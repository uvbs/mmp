using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Wap
{
    public partial class BankCardAddEdit : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        /// <summary>
        /// 
        /// </summary>
        public BindBankCard model = new BindBankCard();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!MemberCenter.checkUser(this.Context))
            {
                return;
            }
            if (Request["id"]!=null)
            {
                this.Title = "修改银行卡";
                model=bll.Get<BindBankCard>(string.Format("AutoID={0} And UserId='{1}'",Request["id"],bll.GetCurrUserID()));
            }
            else
            {
                this.Title = "添加银行卡";
            }
        }
    }
}