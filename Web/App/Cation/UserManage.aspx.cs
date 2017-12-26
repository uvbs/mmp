using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class UserManage : System.Web.UI.Page
    {
        BLLPermission.BLLMenuPermission bllMenupermission = new BLLPermission.BLLMenuPermission("");
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        /// <summary>
        /// 是否显示用户类型
        /// </summary>
        protected bool IsShowUserType;
        protected bool IsShowScore=false;
        protected bool IsShowAccountAmount=false;
        protected bool IsShowSendTempMessage = false;

        protected List<TableFieldMapping> formField = new List<TableFieldMapping>();

        protected string mapping_type = "0";
        protected string idFieldName = "会员ID";
        protected string wxHeadimgurlFieldName = "微信头像";
        protected TableFieldMapping wxHeadimgurlField;
        protected string totalScoreFieldName = "积分";
        protected string userTypeFieldName = "是否经销商";
        protected string accessLevelFieldName = "会员等级";
        protected string accountAmountLevelFieldName = "账户余额(元)";
        protected List<string> limitForeach = new List<string>() { "AutoID", "WXHeadimgurl", "TotalScore", "UserType", "AccessLevel", "AccountAmount" };
        protected string page_type = string.Empty;
        protected string websiteOwner = string.Empty;


        protected int isHideAddBtn = 0;//新增按钮
        protected int isHideEditBtn = 0;//编辑按钮
        protected int isHideTagBtn = 0;//标签按钮
        protected int isHideScoreClearBtn=0;//积分清零按钮
        protected int isHideMemberLevelBtn = 0;//会员等级按钮
        protected int isHideOrderInfoBtn = 0;//从订单、活动同步信息按钮
        protected int isHideChannelBtn = 0;//设置为渠道按钮
        protected int isHideWxNewsBtn = 0;//发送微信消息按钮
        protected int isHideDataClrar = 0;//数据清洗按钮
        protected int isHideDisableBtn = 0;//批量禁用启用按钮
        protected int isHideEditPwdBtn = 0;//修改密码按钮
        protected int isHidefilterBtn = 0;//高级筛选
        protected int isHideRecommended = 0;//推荐人
        protected int isHideAccountAmount = 0;//账户余额
        protected string userType = string.Empty;
        protected string moduleName="积分";

        protected WebsiteInfo websiteInfo = new WebsiteInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            websiteInfo = bllUser.GetWebsiteInfoModelFromDataBase(bllUser.WebsiteOwner);
            if (websiteInfo.MemberMgrBtn==null)
            {
                websiteInfo.MemberMgrBtn = "updateinfo,updatetag,sendweixinmsg,sendweixinmsgbytag";
            }
            websiteOwner = bllUser.WebsiteOwner;
            
            isHideAddBtn = Convert.ToInt32(Request["isHideAddBtn"]);
            isHideEditBtn = Convert.ToInt32(Request["isHideEditBtn"]);
            isHideTagBtn = Convert.ToInt32(Request["isHideTagBtn"]);
            isHideScoreClearBtn = Convert.ToInt32(Request["isHideScoreClearBtn"]);
            isHideMemberLevelBtn = Convert.ToInt32(Request["isHideMemberLevelBtn"]);
            isHideOrderInfoBtn = Convert.ToInt32(Request["isHideOrderInfoBtn"]);
            isHideChannelBtn = Convert.ToInt32(Request["isHideChannelBtn"]);
            isHideWxNewsBtn = Convert.ToInt32(Request["isHideWxNewsBtn"]);
            isHideDataClrar = Convert.ToInt32(Request["isHideDataClrar"]);
            isHideDisableBtn = Convert.ToInt32(Request["isHideDisableBtn"]);
            isHideEditPwdBtn = Convert.ToInt32(Request["isHideEditPwdBtn"]);
            isHidefilterBtn = Convert.ToInt32(Request["isHidefilterBtn"]);
            isHideRecommended = Convert.ToInt32(Request["isHideRecommended"]);
            isHideAccountAmount = Convert.ToInt32(Request["isHideAccountAmount"]);
            if (!string.IsNullOrEmpty(Request["moduleName"]))
            {
                moduleName = Request["moduleName"];
            }
            if (!string.IsNullOrEmpty(Request["user_type"]))
            {
                userType = Request["user_type"];
            }
            List<string> domainList = new List<string>();
            domainList.Add("www.aussieorigin.cn");
            domainList.Add("www.aussieorigin.com.cn");
            if (domainList.Contains(Request.Url.Host))
            {
                IsShowUserType = true;
            }
            page_type=Request["page"];
            IsShowScore=bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.UpdateMemberScore);
            IsShowAccountAmount = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(),BLLPermission.Enums.PermissionSysKey.UpdateMemberBalance);
            IsShowSendTempMessage = bllMenupermission.CheckUserAndPmsKey(bllUser.GetCurrUserID(), BLLPermission.Enums.PermissionSysKey.SendTtemplatMessage);

            formField = bllTableFieldMap.GetTableFieldMapByWebsite(bllTableFieldMap.WebsiteOwner, "ZCJ_UserInfo", null, null, this.Request["mapping_type"]);
            if (formField.Count > 0) mapping_type = formField[0].MappingType.ToString();

            if (formField.FirstOrDefault(p => p.Field == "AutoID") != null) idFieldName = formField.FirstOrDefault(p => p.Field == "AutoID").MappingName;
            if (formField.FirstOrDefault(p => p.Field == "WXHeadimgurl") != null) wxHeadimgurlFieldName = formField.FirstOrDefault(p => p.Field == "WXHeadimgurl").MappingName;
            if (formField.FirstOrDefault(p => p.Field == "TotalScore") != null) totalScoreFieldName = formField.FirstOrDefault(p => p.Field == "TotalScore").MappingName;
            if (formField.FirstOrDefault(p => p.Field == "UserType") != null) userTypeFieldName = formField.FirstOrDefault(p => p.Field == "UserType").MappingName;
            if (formField.FirstOrDefault(p => p.Field == "AccessLevel") != null) accessLevelFieldName = formField.FirstOrDefault(p => p.Field == "AccessLevel").MappingName;
            if (formField.FirstOrDefault(p => p.Field == "AccountAmount") != null) accountAmountLevelFieldName = formField.FirstOrDefault(p => p.Field == "AccountAmount").MappingName;


        }
        protected void GetDatagridField()
        {
            StringBuilder sb = new StringBuilder();
            foreach (TableFieldMapping item in formField.Where(p => p.IsShowInList == 1 && !limitForeach.Contains(p.Field)))
            {
                if (item.Field == "Avatar" || item.FieldType == "img")
                {
                    sb.AppendLine(string.Format("{0}field:'{1}', title: '{2}', width: 50, align: 'center', formatter: function (value) {0}", "{", item.Field, item.MappingName));
                    sb.AppendLine(string.Format("if (value == '' || value == null) value = defImg;"));
                    sb.AppendLine(string.Format("var str = new StringBuilder();"));
                    sb.AppendLine(string.Format("str.AppendFormat('<a href=\"javascript:;\"><img alt=\"\" class=\"imgAlign\" src=\"{0}0{1}\" title=\"缩略图\" height=\"25\" width=\"25\" /></a>', value);", "{", "}"));
                    sb.AppendLine(string.Format("return str.ToString();{0}{0},", "}"));
                }
                else if (item.Field == "Birthday" || item.FieldType == "date")
                {
                    sb.AppendLine(string.Format("{0} field: '{1}', title: '{2}', width: 70, align: 'left', formatter: function (value) {0}", "{", item.Field, item.MappingName));
                    sb.AppendLine(string.Format("if (value == '' || value == null) return '';"));
                    sb.AppendLine(string.Format("if (new Date(value) < new Date(1901,1,1)) return '';"));
                    sb.AppendLine(string.Format("return new Date(value).format('yyyy/MM/dd');{0}{0},", "}"));
                    if (item.Field == "Birthday")
                    {
                        sb.AppendLine(string.Format("{0} field: 'Age', title: '年龄', width: 70, align: 'left', formatter:FormatterTitle {1},", "{", "}"));
                    }
                }
                else if (item.FieldType == "sex")
                {
                    sb.AppendLine(string.Format("{0} field: '{1}', title: '{2}', width: 50, align: 'left', formatter: function (value) {0}", "{", item.Field, item.MappingName));
                    sb.AppendLine(string.Format("if (value == '1') return '男';"));
                    sb.AppendLine(string.Format("if (value == '0') return '女';"));
                    sb.AppendLine(string.Format("return '';{0}{0},", "}"));
                }
                else
                {
                    sb.AppendLine(string.Format("{0} field: '{2}', title: '{3}', width: 120, align: 'left', formatter: FormatterTitle {1},", "{", "}", item.Field, item.MappingName));
                }
            }
            this.Response.Write(sb.ToString());
        }
    }
}