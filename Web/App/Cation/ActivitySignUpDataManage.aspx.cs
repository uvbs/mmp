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
    public partial class ActivitySignUpDataManage : System.Web.UI.Page
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
        /// 用户名称
        /// </summary>
        public string LinkName;
        /// <summary>
        /// 显示的列名
        /// </summary>
        public string Columns;
        /// <summary>
        /// 活动类型 DistributionOffLine分销
        /// </summary>
        public string activityType = string.Empty;
        /// <summary>
        /// 是否隐藏 返回按钮
        /// </summary>
        public int isHideBackBtn = 0;
        /// <summary>
        /// 
        /// </summary>
        public ActivityInfo ActivityInfo;
        /// <summary>
        /// 
        /// </summary>
        public StringBuilder strEditTable = new StringBuilder();
        /// <summary>
        /// 显示编辑赋值到控件
        /// </summary>
        public StringBuilder strShowEditAssignment = new StringBuilder();

        /// <summary>
        /// 保存编辑参数赋值
        /// </summary>
        public StringBuilder strSaveEditAssignment = new StringBuilder();
        /// <summary>
        /// 
        /// </summary>
        public bool isHide = false;
        /// <summary>
        /// 
        /// </summary>
        public bool isDelete = false;
        /// <summary>
        /// 是否显示导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        BLLPermission.BLLMenuPermission bllMenupermission = new ZentCloud.BLLPermission.BLLMenuPermission("");
        /// <summary>
        /// 
        /// </summary>
        BLLUser bllUser = new BLLUser("");
        /// <summary>
        /// 
        /// </summary>
        BLLDistributionOffLine bllDistributionOffLine = new BLLDistributionOffLine();
        /// <summary>
        /// 基类BLL
        /// </summary>
        private BLL bll = new BLL("");
        /// <summary>
        /// 活动BLL
        /// </summary>
        BLLActivity bllActivity = new BLLActivity("");
        /// <summary>
        /// 
        /// </summary>
        BLLJuActivity bllJuactivity = new BLLJuActivity();
        /// <summary>
        /// 
        /// </summary>
        BLLTableFieldMap bllTableFieldMap = new BLLTableFieldMap();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.UrlReferrer != null)
            {
                isHideBackBtn = 1;
            }

            //获取传入的活动ID
            ActivityID = Request.QueryString["ActivityID"];

            //用户名称
            LinkName = Request.QueryString["LinkName"];

            activityType = Request.QueryString["activityType"];

            if (!string.IsNullOrWhiteSpace(activityType) && activityType == "DistributionOffLine")
            {
                ActivityID = bllDistributionOffLine.GetDistributionOffLineApplyActivityID();
            }

            //TODO：判断如果是分销审核类型，则走分销处理逻辑

            //获取当前的分销活动id
            

            ActivityInfo = bll.Get<ActivityInfo>(string.Format(" ActivityID={0} And WebsiteOwner='{1}'", ActivityID,bllUser.WebsiteOwner));
            if (ActivityInfo == null)
            {
                //Response.Redirect("/Activity/ActivityManage.aspx");
                Response.Write("<script>alert('活动不存在!')</script>");
                Response.End();
                return;
            }
            ActivityName = ActivityInfo.ActivityName;
            //var userid = DataLoadTool.GetCurrUserID();
            //if (bll.Get<ActivityInfo>(string.Format("ActivityID='{0}' and WebsiteOwner='{1}'", ActivityID, bllUser.WebsiteOwner)) == null && (DataLoadTool.GetCurrUserModel().UserType != 1))
            //{
            //    //Response.Redirect("/Activity/ActivityManage.aspx");
            //    Response.Write("<script>alert('无权访问该数据，请确认当前用户是否拥有该活动权限!')</script>");
            //    Response.End();
            //    return;
            //}

           
            //用户名
            //strEditTable.Append("<tr>");
            //strEditTable.AppendFormat("<td style=\"width:80px;\"><label >用户名:</label></td>");
            //strEditTable.AppendFormat("<td style=\"width:*;\"><input style=\"width:100%\" type=\"text\"  id=\"txtUserId\" value=\"\" placeholder=\"请输入用户名\" style=\"width:100%;\"/> </td>");
            //strEditTable.Append("</tr>");
            List<ActivityFieldMappingInfo> fieldMap = this.bllActivity.GetActivityFieldMappingList(ActivityID);
            //默认列
            foreach (ActivityFieldMappingInfo item in fieldMap)
            {
                Columns += string.Format(" <th field=\"{0}\" width=\"100\">{1} </th>", item.FieldName, item.MappingName);

                strEditTable.Append("<tr>");
                strEditTable.AppendFormat("<td style=\"width:80px;\"><label for=\"{0}\">{1}</label></td>", item.FieldName, item.MappingName);
                if (item.IsMultiline.Equals(1))//<textarea name="K1" id="txtContent" style="height: 100px;" placeholder="请详细描述您的建议、意见、问题等。"></textarea>
                { strEditTable.AppendFormat("<td style=\"width:*;\"><textarea rows=\"5\" type=\"text\" name=\"{0}\" id=\"txt{0}\" value=\"\" placeholder=\"请输入{1}\" style=\"width:100%;height: 200px;\"></textarea> </td>", item.FieldName, item.MappingName); }
                else
                {
                    strEditTable.AppendFormat("<td style=\"width:*;\"><input style=\"width:100%\" type=\"text\" name=\"{0}\" id=\"txt{0}\" value=\"\" placeholder=\"请输入{1}\" style=\"width:100%;\"/> </td>", item.FieldName, item.MappingName);
                }
                strEditTable.Append("</tr>");
                strShowEditAssignment.AppendFormat("$('#txt{0}').val(resp.{0});", item.FieldName);
                strSaveEditAssignment.AppendFormat("{0}:$('#txt{0}').val(),", item.FieldName);



            }
            //备注
            strEditTable.Append("<tr>");
            strEditTable.AppendFormat("<td style=\"width:80px;\"><label for=\"{0}\">{1}</label></td>", "txtRemarks", "备注");
            strEditTable.AppendFormat("<td style=\"width:*;\"><textarea rows=\"5\" type=\"text\" name=\"{0}\" id=\"txt{0}\" value=\"\" placeholder=\"请输入{1}\" style=\"width:100%;height: 200px;\"/></textarea> </td>", "Remarks", "备注");
            strEditTable.Append("</tr>");
            strShowEditAssignment.AppendFormat("$('#txt{0}').val(resp.{0});", "Remarks");
            strSaveEditAssignment.AppendFormat("{0}:$('#{0}').val(),", "Remarks");



            List<TableFieldMapping> tableFieldMap = bllTableFieldMap.GetTableFieldMap(bllTableFieldMap.WebsiteOwner, "ActivityDataInfo", ActivityID);
            foreach (TableFieldMapping item in tableFieldMap)
            {
                Columns += string.Format(" <th field=\"{0}\" width=\"100\">{1} </th>", item.Field, item.MappingName);

                strEditTable.AppendFormat("<td style=\"width:80px;\"><label for=\"{0}\">{1}</label></td>", item.Field, item.MappingName);

                strEditTable.AppendFormat("<td style=\"width:*;\"><input style=\"width:100%\" type=\"{2}\" name=\"{0}\" id=\"txt{0}\" value=\"\" placeholder=\"请输入{1}\" style=\"width:100%;\"/> </td>", item.Field, item.MappingName, item.FieldType=="6"?"date":"text");
                strEditTable.Append("</tr>");
                strShowEditAssignment.AppendFormat("$('#txt{0}').val(resp.{0});", item.Field);
                strSaveEditAssignment.AppendFormat("{0}:$('#txt{0}').val(),", item.Field);
            }



            strShowEditAssignment.AppendFormat("$('#txtUserId').val(resp.UserId);");
            strSaveEditAssignment.AppendFormat("UserId:$('#txtUserId').val(),");
            strSaveEditAssignment.AppendFormat("Remarks:$('#txtRemarks').val(),");

            JuActivityInfo juActivityInfo = bllJuactivity.GetJuActivityByActivityID(ActivityID);
            if (juActivityInfo!=null&&juActivityInfo.IsFee==1)//显示收费活动相关列
            {
                activityType = "PayActivity";
                //Columns += string.Format(" <th field=\"OrderId\" width=\"100\">订单号</th>");
                Columns += string.Format(" <th field=\"ItemName\" width=\"100\">购买选项名称</th>");
                Columns += string.Format(" <th field=\"ItemAmount\" width=\"100\">选项金额(元)</th>");
                Columns += string.Format(" <th field=\"CouponName\" width=\"100\">优惠券</th>");
                Columns += string.Format(" <th field=\"UseScore\" width=\"100\">使用积分</th>");
                Columns += string.Format(" <th field=\"UseAmount\" width=\"100\">使用余额</th>");
                Columns += string.Format(" <th field=\"Amount\" width=\"100\">支付金额</th>");
                Columns += string.Format(" <th field=\"PaymentStatus\" formatter=\"FormartPaymentStatus\" width=\"100\">支付状态</th>");
                Columns += string.Format(" <th field=\"Status\" formatter=\"FormartDealStatus\" width=\"100\">交易状态</th>");
            }

            //默认显示提交时间字段
            Columns += string.Format(" <th field=\"InsertDateStr\" width=\"100\">提交时间</th>");
            Columns += string.Format(" <th field=\"Remarks\" width=\"100\">备注</th>");


            if (bll.WebsiteOwner != "songhe")
            {



                if (activityType != "DistributionOffLine")
                {
                    //默认显示签到时间
                    Columns += string.Format(" <th field=\"SignInDateStr\" width=\"100\">签到时间</th>");

                    if (string.IsNullOrWhiteSpace(LinkName))
                    {
                        Columns += string.Format(" <th field=\"ShareUserName\" width=\"100\">分享推荐人</th>");
                    }
                    else
                    {
                        Columns += string.Format(" <th field=\"SpreadUserTrueName\" width=\"100\">渠道来源</th>");
                    }

                }
                if (activityType == "DistributionOffLine")
                {
                    //默认显示状态
                    Columns += string.Format(" <th field=\"Status\" width=\"100\" formatter=\"FormartStatus\">状态</th>");
                    //默认邀请人
                    Columns += string.Format(" <th field=\"DistributionOffLineRecommendName\" width=\"100\">邀请人</th>");

                    Columns += string.Format(" <th field=\"SpreadUserID\" width=\"100\" formatter=\"FormartSource\">来源</th>");

                }
            }

            //查出数据就隐藏  没有查到数据就显示  true=隐藏  false=显示
            isHide = bllMenupermission.CheckPerRelationByaccount(bllUser.GetCurrUserID(), -2);
            isDelete = bllMenupermission.CheckPerRelationByaccount(bllUser.GetCurrUserID(), -1);

        }
    }
}