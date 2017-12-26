using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.WanBang.Wap
{
    public partial class ProjectDetail : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public WBProjectInfo model = new WBProjectInfo();
        public WBCompanyInfo companymodel = new WBCompanyInfo();
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
            try
            {
                if (string.IsNullOrEmpty(Request["id"]))
                {
                    Response.Write("无参数");
                    Response.End();
                    return;
                }
                model = bll.Get<WBProjectInfo>(string.Format("AutoId={0}", Request["id"]));
                if (model == null)
                {
                    Response.Write("项目不存在");
                    Response.End();
                    return;

                }
                if (model.Status.Equals(0))
                {
                    Response.Write("项目正在审核中,暂时不能查看");
                    Response.End();
                    return;
                }

                companymodel = bll.Get<WBCompanyInfo>(string.Format("UserId='{0}'", model.UserId));
                if (DataLoadTool.CheckWanBangLogin())
                {
                    IsLogin = true;
                    if (bll.GetCount<WBAttentionInfo>(string.Format("UserId='{0}' And AttentionAutoID={1} And AttentionType={2}", HttpContext.Current.Session[SessionKey.WanBangUserID].ToString(), model.AutoID, 2)) > 0)
                    {
                        IsAttention = true;
                    }
                }
            }
            catch (Exception)
            {

                Response.End();
            }

        }
    }
}