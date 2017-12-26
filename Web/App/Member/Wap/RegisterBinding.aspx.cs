using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Member.Wap
{
    public partial class RegisterBinding : System.Web.UI.Page
    {
        protected List<TableFieldMapping> formField = new List<TableFieldMapping>();
        protected string pageName;
        protected string memberStandardDescription;
        protected string ico_css_file;
        BLLUser bllUser = new BLLUser();
        BLLWebSite bllWebSite = new BLLWebSite();
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo curUser = bllUser.GetCurrentUserInfo();
            if (curUser != null)
            {
                //this.Response.Redirect("/Error/CommonMsg.aspx?msg=" + HttpUtility.UrlEncode("本功能仅供新用户使用"));
                this.Response.Redirect("/customize/comeoncloud/Index.aspx?key=MallHome");
                return;
            }
            curUser = new UserInfo();
            CompanyWebsite_Config nWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig();
            memberStandardDescription = nWebsiteConfig.MemberStandardDescription;
            pageName = "会员注册";

            List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo", null, null, "0", null);
            List<string> DefFields = new List<string>() { "AutoID", "UserID", "Password", "UserType", "TrueName", "Phone", "Avatar" };
            #region 照片
            TableFieldMapping AvatarField = listFieldList.FirstOrDefault(p => p.Field.Equals("Avatar"));
            if (AvatarField != null)
            {
                //curUser.IsPhoneVerify
                formField.Add(new TableFieldMapping() { Field = "Avatar", MappingName = AvatarField.MappingName, FieldType = AvatarField.FieldType, Disabled = 0, Value = curUser.Avatar, FieldIsNull = AvatarField.FieldIsNull });
            }
            #endregion
            #region 姓名
            TableFieldMapping TrueNameField = listFieldList.FirstOrDefault(p => p.Field.Equals("TrueName"));
            if (TrueNameField == null)
            {
                formField.Add(new TableFieldMapping() { Field = "TrueName", MappingName = "姓名", Disabled = 0, Value = curUser.TrueName, FieldIsNull = 1 });
            }
            else
            {
                formField.Add(new TableFieldMapping() { Field = "TrueName", MappingName = TrueNameField.MappingName, Disabled = 0, Value = curUser.TrueName, FieldIsNull = TrueNameField.FieldIsNull });
            }
            #endregion
            #region 手机
            TableFieldMapping PhoneField = listFieldList.FirstOrDefault(p => p.Field.Equals("Phone"));
            if (PhoneField == null)
            {
                //curUser.IsPhoneVerify
                formField.Add(new TableFieldMapping() { Field = "Phone", MappingName = "手机", Disabled = 0, Value = curUser.Phone, FieldIsNull = 1 });
            }
            else
            {
                formField.Add(new TableFieldMapping() { Field = "Phone", MappingName = PhoneField.MappingName, Disabled = 0, Value = curUser.Phone, FieldIsNull = PhoneField.FieldIsNull });
            }
            #endregion
            JObject jtCurUser = JObject.FromObject(curUser);
            List<JProperty> listPropertys = jtCurUser.Properties().ToList();
            foreach (var item in listFieldList.Where(p => !DefFields.Contains(p.Field)).OrderBy(p => p.Sort))
            {
                if (!listPropertys.Exists(p => p.Name.Equals(item.Field))) continue;
                formField.Add(new TableFieldMapping() { Field = item.Field, MappingName = item.MappingName, FieldType = item.FieldType, Disabled = 0, Value = jtCurUser[item.Field].ToString(), FieldIsNull = item.FieldIsNull });
            }

            //头部图标引用
            ico_css_file = bllWebSite.GetIcoFilePath();
        }
    }
}