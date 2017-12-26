using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;
using System.Text;

namespace ZentCloud.JubitIMP.Web.App.Cation
{
    public partial class SignUpManage : System.Web.UI.Page
    {
        /// <summary>
        /// 传入的活动ID
        /// </summary>
        public string ActivityID;
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName;
        /// <summary>
        /// 显示的列名
        /// </summary>
        public string Columns;
        private BLL bll = new BLL("");
        BLLActivity activityBll = new BLLActivity("");
        public ActivityInfo ActivityInfo;
        public StringBuilder strEditTable = new StringBuilder();
        /// <summary>
        /// 显示编辑赋值到控件
        /// </summary>
        public StringBuilder strShowEditAssignment = new StringBuilder();

        /// <summary>
        /// 保存编辑参数赋值
        /// </summary>
        public StringBuilder strSaveEditAssignment = new StringBuilder();

        protected void Page_Load(object sender, EventArgs e)
        {

            //固定活动ID
            ActivityID = Comm.DataLoadTool.GetWebsiteInfoModel().SignUpActivityID; //Common.ConfigHelper.GetConfigString("HfSignUpActivityID");//Request.QueryString["ActivityID"];

            ActivityInfo = bll.Get<ActivityInfo>(string.Format(" ActivityID={0}", ActivityID));
            if (ActivityInfo == null)
            {
                //Response.Redirect("/Activity/ActivityManage.aspx");
                Response.Write("<script>alert('活动不存在!')</script>");
                Response.End();
                return;
            }
            ActivityName = ActivityInfo.ActivityName;
            var userid = Comm.DataLoadTool.GetCurrUserID();
            if (bll.Get<ActivityInfo>(string.Format("ActivityID='{0}' and UserID='{1}'", ActivityID, userid)) == null && !activityBll.GetCurrentUserInfo().UserType.Equals(1))
          
            {
                //Response.Redirect("/Activity/ActivityManage.aspx");
                Response.Write("<script>alert('无权访问该数据，请确认当前用户是否拥有该活动权限!')</script>");
                Response.End();
                return;
            }

            List<ActivityFieldMappingInfo> list = this.activityBll.GetActivityFieldMappingList(ActivityID);//this.activityBll.GetActivityFieldMappingList(ActivityID);//bll.GetList<ActivityFieldMappingInfo>(string.Format("ActivityID='{0}'", ActivityID));
            foreach (ActivityFieldMappingInfo item in list)
            {
                Columns += string.Format(" <th field=\"{0}\" width=\"100\" formatter=\"FormatterTitle\">{1} </th>", item.FieldName, item.MappingName);

                strEditTable.Append("<tr>");
                strEditTable.AppendFormat("<td style=\"width:60px;\"><label for=\"{0}\">{1}</label></td>", item.FieldName, item.MappingName);
                if (item.IsMultiline.Equals(1))//<textarea name="K1" id="txtContent" style="height: 100px;" placeholder="请详细描述您的建议、意见、问题等。"></textarea>
                    strEditTable.AppendFormat("<td style=\"width:*;\"><textarea rows=\"5\" type=\"text\" name=\"{0}\" id=\"txt{0}\" value=\"\" placeholder=\"请输入{1}\" style=\"width:100%;height: 200px;\"></textarea> </td>", item.FieldName, item.MappingName);
                else
                    strEditTable.AppendFormat("<td style=\"width:*;\"><input style=\"width:100%\" type=\"text\" name=\"{0}\" id=\"txt{0}\" value=\"\" placeholder=\"请输入{1}\" style=\"width:100%;\"/> </td>", item.FieldName, item.MappingName);
                strEditTable.Append("</tr>");

                strShowEditAssignment.AppendFormat("$('#txt{0}').val(resp.{0});", item.FieldName);

                strSaveEditAssignment.AppendFormat("{0}:$('#txt{0}').val(),", item.FieldName);
            }

            //默认显示提交时间字段
            Columns += string.Format(" <th field=\"InsertDateStr\" width=\"100\" formatter=\"FormatterTitle\">提交时间</th>");
            //默认显示签到时间
            Columns += string.Format(" <th field=\"SignInDateStr\" width=\"100\">签到时间</th>");



        }
    }
}