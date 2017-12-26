using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;
using System.IO;
using System.Text;

namespace ZentCloud.JubitIMP.Web.customize.Mixblu
{
    /// <summary>
    /// mixblu 首页
    /// </summary>
    public partial class Index : System.Web.UI.Page
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        /// 首页html
        /// </summary>
        public string html = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //读取首页html
            html = Common.IOHelper.GetFileStr(HttpContext.Current.Server.MapPath("/customize/Mixblu/index.html"), Encoding.UTF8);
            string unionId = Request["unionid"];
            string nickName = Request["nickName"];
            string code = Request["code"];
            string mobileNo = Request["mobileNo"];
            string sign = Request["sign"];
            if ((!string.IsNullOrEmpty(unionId)) && (!string.IsNullOrEmpty(mobileNo)))
            {
                //ToLog(unionId+"\tmobile:\t"+mobileNo);
                var newUserInfo = bllUser.GetUserInfoByWXUnionID(unionId);
                if (newUserInfo != null)
                {
                    if (bllUser.Update(newUserInfo, string.Format(" Phone='{0}'", mobileNo), string.Format(" AutoId={0}", newUserInfo.AutoID)) > 0)
                    {


                    }
                    else
                    {
                        //更新会员手机号不成功
                    }

                }

            }
            else//检查是否需要开卡
            {
                string redirectUrl = Request["redirectUrl"];
                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    Response.Redirect(redirectUrl);
                }
            }
        }
    }
}