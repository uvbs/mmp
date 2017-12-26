using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap.Mall
{
    public partial class CardCode : System.Web.UI.Page
    {
        /// <summary>
        /// 二维码
        /// </summary>
        public System.Text.StringBuilder QCode=new System.Text.StringBuilder();
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (bllMall.IsLogin)
            {
                UserInfo userInfo = DataLoadTool.GetCurrUserModel();
                QCode.Append(string.Format("编号:{0}", userInfo.AutoID));
                QCode.AppendFormat("姓名:{0}", userInfo.TrueName);
                QCode.AppendFormat("性别:");
                if (!string.IsNullOrEmpty(userInfo.Gender))
                {
                    if (userInfo.Gender.Equals("1"))
                    {
                        QCode.Append("男");
                    }
                    if (userInfo.Gender.Equals("0"))
                    {
                        QCode.Append("女");
                    }
                }

                QCode.AppendFormat("电话:{0}", userInfo.Phone);
                QCode.AppendFormat("Email:{0}", userInfo.Email);
                QCode.AppendFormat("地区:{0}", userInfo.AddressArea);
                


               
            }
            else
            {

                Response.Redirect(string.Format("/App/Cation/Wap/Login.aspx?redirecturl={0}", Request.FilePath));
            }


        }
    }
}