using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.TheVote
{
    public partial class TheVoteUserInfo : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        public string id = "0";
        /// <summary>
        /// 
        /// </summary>
        public List<DictionaryInfo> Items;
        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                id = Request["id"];
                Items = bll.GetList<DictionaryInfo>(string.Format(" ForeignKey='{0}'",id));

            }
        }
    }
}