using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Member
{
    /// <summary>
    /// GetCompleteField 的摘要说明
    /// </summary>
    public class GetCompleteField : BaseHandlerNoAction
    {
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        public void ProcessRequest(HttpContext context)
        {
            string mapping_type = context.Request["mapping_type"];
            List<ResultField> resultList = new List<ResultField>();

            //获取当前用户信息 未登录时实例化UserInfo
            UserInfo CurrentUserInfo = bllTableFieldMap.GetCurrentUserInfo();
            if (CurrentUserInfo == null) CurrentUserInfo = new UserInfo();

            List<TableFieldMapping> listFieldList = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo", null, null, mapping_type, null);
            mapping_type = listFieldList.Count > 0 ? listFieldList[0].MappingType.ToString() : mapping_type;

            List<string> DefFields = new List<string>() { "AutoID", "UserID", "Password", "UserType", "TrueName", "Phone", "Avatar" };
            #region 照片
            TableFieldMapping AvatarField = listFieldList.FirstOrDefault(p => p.Field.Equals("Avatar"));
            if (AvatarField != null)
            {
                resultList.Add(new ResultField { field = "Avatar", field_name = AvatarField.MappingName, type = AvatarField.FieldType, no_null = AvatarField.FieldIsNull, value = CurrentUserInfo.Avatar, read_only = AvatarField.IsReadOnly });
            }
            #endregion
            #region 姓名
            TableFieldMapping TrueNameField = listFieldList.FirstOrDefault(p => p.Field.Equals("TrueName"));
            if (TrueNameField == null)
            {
                resultList.Add(new ResultField { field = "TrueName", field_name = "姓名", type = "txt", no_null = 1, value = CurrentUserInfo.TrueName, read_only = 0 });
            }
            else
            {
                resultList.Add(new ResultField { field = "TrueName", field_name = TrueNameField.MappingName, type = "txt", no_null = TrueNameField.FieldIsNull, value = CurrentUserInfo.TrueName, read_only = TrueNameField.IsReadOnly });
            }
            #endregion
            #region 手机
            TableFieldMapping PhoneField = listFieldList.FirstOrDefault(p => p.Field.Equals("Phone"));
            if (PhoneField == null)
            {
                //curUser.IsPhoneVerify
                resultList.Add(new ResultField { field = "Phone", field_name = "手机", type = "txt", no_null = 1, value = CurrentUserInfo.Phone, read_only = 0 });
            }
            else
            {
                resultList.Add(new ResultField { field = "Phone", field_name = PhoneField.MappingName, type = "txt", no_null = PhoneField.FieldIsNull, value = CurrentUserInfo.Phone, read_only = PhoneField.IsReadOnly });
            }
            #endregion
            JObject jtCurUser = JObject.FromObject(CurrentUserInfo);
            List<JProperty> listPropertys = jtCurUser.Properties().ToList();
            foreach (var item in listFieldList.Where(p => !DefFields.Contains(p.Field)).OrderBy(p => p.Sort))
            {
                if (!listPropertys.Exists(p => p.Name.Equals(item.Field))) continue;
                string FieldType = string.IsNullOrWhiteSpace(item.FieldType) ? "txt" : item.FieldType;
                resultList.Add(new ResultField { field = item.Field, field_name = item.MappingName, type = FieldType, no_null = item.FieldIsNull, value = jtCurUser[item.Field].ToString(), read_only = item.IsReadOnly });
            }
            apiResp.result = new
            {
                can_edit = resultList.Where(p => p.read_only == 0).Count() > 0,
                field_list = resultList,
                mapping_type = mapping_type,
                id = CurrentUserInfo.AutoID
            };
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.status = true;
            apiResp.msg = "查询完成";
            bllTableFieldMap.ContextResponse(context, apiResp);
        }
        public class ResultField
        {
            public string field { get; set; }
            public string field_name { get; set; }
            public string type { get; set; }
            public int no_null { get; set; }
            public string value { get; set; }
            public int read_only { get; set; }
        }
    }
}