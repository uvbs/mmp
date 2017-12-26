using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.WanBang.Wap
{
    public partial class BaseDetail : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public WBBaseInfo model = new WBBaseInfo();
        /// <summary>
        /// 是否登录标识
        /// </summary>
        public bool IsLogin = false;
        /// <summary>
        /// 是否关注标识
        /// </summary>
        public bool IsAttention = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request["id"]))
            {
                Response.Write("无参数");
                Response.End();
                return;
            }
            model = bll.Get<WBBaseInfo>(string.Format("AutoID={0}", Request["id"]));
            if (model == null)
            {
                Response.Write("基地不存在");
                Response.End();
                return;

            }
            if (model.IsDisable.Equals(1))
            {
                Response.Write("此基地已经禁用,暂时不能查看");
                Response.End();
                return;
            }
            if (DataLoadTool.CheckWanBangLogin())
            {
              IsLogin = true;
              if (bll.GetCount<WBAttentionInfo>(string.Format("UserId='{0}' And AttentionAutoID={1} And AttentionType={2}",HttpContext.Current.Session[SessionKey.WanBangUserID].ToString(),model.AutoID,0))>0)
                {
                IsAttention = true;
                }
            }




        }
    }
}