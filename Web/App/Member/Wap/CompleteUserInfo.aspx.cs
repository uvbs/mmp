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
    public partial class CompleteUserInfo : System.Web.UI.Page
    {
        protected List<TableFieldMapping> formField = new List<TableFieldMapping>();
        protected string pageName;
        protected string memberStandardDescription;
        protected string ico_css_file;
        protected string referrer;
        BLLUser bllUser = new BLLUser();
        BLLWebSite bllWebSite = new BLLWebSite();
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        protected void Page_Load(object sender, EventArgs e)
        {
            referrer = this.Request["referrer"];
            UserInfo curUser = bllUser.GetCurrentUserInfo();
            if (curUser == null) curUser = new UserInfo();
            CompanyWebsite_Config nWebsiteConfig = bllWebSite.GetCompanyWebsiteConfig();
            memberStandardDescription = nWebsiteConfig.MemberStandardDescription;
            if (nWebsiteConfig.MemberStandard == 1)
            {
                this.Response.Redirect("PhoneVerify.aspx?referrer=" + HttpUtility.UrlEncode(referrer));
                return;
            }
            pageName = "完善资料";
            if (nWebsiteConfig.MemberStandard == 3)
            {
                pageName = "会员注册";
                referrer = "/Error/IsInApply.htm";
                if (curUser.MemberApplyStatus == 1)
                {
                    this.Response.Redirect("/Error/IsInApply.htm");
                    return;
                }

            }

            List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMap(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo");
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
            //#region 公司
            //TableFieldMapping CompanyField = listFieldList.FirstOrDefault(p => p.Field.Equals("Company"));
            //if (CompanyField != null)
            //{
            //    formField.Add(new TableFieldMapping() { Field = "Company", MappingName = CompanyField.MappingName, Disabled = 0, Value = curUser.Company, FieldIsNull = CompanyField.FieldIsNull });
            //}
            //#endregion
            //#region 职位
            //TableFieldMapping PostionField = listFieldList.FirstOrDefault(p => p.Field.Equals("Postion"));
            //if (PostionField != null)
            //{
            //    formField.Add(new TableFieldMapping() { Field = "Postion", MappingName = PostionField.MappingName, Disabled = 0, Value = curUser.Postion, FieldIsNull = PostionField.FieldIsNull });
            //}
            //#endregion
            JObject jtCurUser = JObject.FromObject(curUser);
            List<JProperty> listPropertys = jtCurUser.Properties().ToList();
            foreach (var item in listFieldList.Where(p => !DefFields.Contains(p.Field)).OrderBy(p=>p.Sort))
            {
                if (!listPropertys.Exists(p => p.Name.Equals(item.Field))) continue;
                formField.Add(new TableFieldMapping() { Field = item.Field, MappingName = item.MappingName, FieldType = item.FieldType, Disabled = 0, Value = jtCurUser[item.Field].ToString(), FieldIsNull = item.FieldIsNull });
            }

            //头部图标引用
            ico_css_file = bllWebSite.GetIcoFilePath();
        }
    }
}