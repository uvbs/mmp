using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.WuBuHui.WordsQuestions
{
    public partial class MyWXDiscussList : UserPage
    {
        public string DisussStr;
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public string websiteOwner;
        public BLLJIMP.Model.UserInfo uinfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCType();
            }
        }

        private void GetCType()
        {
            this.websiteOwner = DataLoadTool.GetWebsiteInfoModel().WebsiteOwner;
            this.uinfo = DataLoadTool.GetCurrUserModel();
            List<BLLJIMP.Model.ArticleCategory> acategorys = bll.GetList<BLLJIMP.Model.ArticleCategory>(string.Format(" CategoryType='word'AND WebsiteOwner='{0}'", this.websiteOwner));
            if (acategorys != null)
            {
                foreach (BLLJIMP.Model.ArticleCategory item in acategorys)
                {
                    DisussStr += "<li class=\"catli\" v=\"" + item.AutoID + "\"><a >" + item.CategoryName + "</a></li>";
                }
            }

            //设置用户所有话题为已读
            BLLJIMP.BLL.ExecuteSql(string.Format("update ZCJ_ReviewInfo set IsRead = 1 where ForeignkeyId='{0}' and IsRead <> 1 and websiteowner='{1}'",
                uinfo.UserID, websiteOwner));

        }
    }
}