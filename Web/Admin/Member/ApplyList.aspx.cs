using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Member
{
    public partial class ApplyList : System.Web.UI.Page
    {
        BLLUser bllUser = new BLLUser();
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        protected List<TableFieldMapping> formField = new List<TableFieldMapping>();
        protected TableFieldMapping AvatarField;
        protected void Page_Load(object sender, EventArgs e)
        {
            List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMapByTableName(bllUser.WebsiteOwner, "ZCJ_UserInfo");
            List<string> DefFields = new List<string>() { "AutoID", "UserID", "Password", "UserType", "TrueName", "Phone", "Avatar" };
            #region 照片
            AvatarField = listFieldList.FirstOrDefault(p => p.Field.Equals("Avatar"));
            #endregion
            #region 姓名
            TableFieldMapping TrueNameField = listFieldList.FirstOrDefault(p => p.Field.Equals("TrueName"));
            if (TrueNameField == null)
            {
                formField.Add(new TableFieldMapping() { Field = "TrueName", MappingName = "姓名" });
            }
            else
            {
                formField.Add(new TableFieldMapping() { Field = "TrueName", MappingName = TrueNameField.MappingName });
            }
            #endregion
            #region 手机
            TableFieldMapping PhoneField = listFieldList.FirstOrDefault(p => p.Field.Equals("Phone"));
            if (PhoneField == null)
            {
                //curUser.IsPhoneVerify
                formField.Add(new TableFieldMapping() { Field = "Phone", MappingName = "手机"});
            }
            else
            {
                formField.Add(new TableFieldMapping() { Field = "Phone", MappingName = PhoneField.MappingName});
            }
            #endregion
            //#region 公司
            //TableFieldMapping CompanyField = listFieldList.FirstOrDefault(p => p.Field.Equals("Company"));
            //if (CompanyField == null)
            //{
            //    formField.Add(new TableFieldMapping() { Field = "Company", MappingName = "公司"});
            //}
            //else
            //{
            //    formField.Add(new TableFieldMapping() { Field = "Company", MappingName = CompanyField.MappingName });
            //}
            //#endregion
            //#region 职位
            //TableFieldMapping PostionField = listFieldList.FirstOrDefault(p => p.Field.Equals("Postion"));
            //if (PostionField == null)
            //{
            //    formField.Add(new TableFieldMapping() { Field = "Postion", MappingName = "职位" });
            //}
            //else
            //{
            //    formField.Add(new TableFieldMapping() { Field = "Postion", MappingName = PostionField.MappingName});
            //}
            //#endregion
            listFieldList = listFieldList.Where(p => !DefFields.Contains(p.Field)).OrderBy(p => p.Sort).ToList();
            JObject jtCurUser = JObject.FromObject(new UserInfo());
            List<JProperty> listPropertys = jtCurUser.Properties().ToList();
            foreach (var item in listFieldList.Where(p => !DefFields.Contains(p.Field)).OrderBy(p => p.Sort))
            {
                if (!listPropertys.Exists(p => p.Name.Equals(item.Field))) continue;
                formField.Add(new TableFieldMapping() { Field = item.Field, MappingName = item.MappingName });
            }
        }
    }
}