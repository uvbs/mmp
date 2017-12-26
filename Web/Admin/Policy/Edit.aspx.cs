using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Policy
{
    public partial class Edit : System.Web.UI.Page
    {
        BLLJuActivity bllJuActivity = new BLLJuActivity();
        protected JuActivityInfo nInfo;
        protected List<JuActivityFiles> nFiles = new List<JuActivityFiles>();
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = this.Request["id"];
            if (id == "0")
            {
                nInfo = new JuActivityInfo();
                nInfo.K2 = "个人";
            }
            else
            {
                nInfo = bllJuActivity.GetColByKey<JuActivityInfo>("JuActivityID", id,
                    "JuActivityID,ActivityName,Summary,K1,K2,K3,K4,K5,K6,K7,K8,K9,K10,K11,K12,K13,K14,K15,K16,K17,K18");
                if (nInfo == null) { 
                    this.Response.Redirect("List.aspx");
                    return;
                }
                nFiles = bllJuActivity.GetColMultListByKey<JuActivityFiles>(int.MaxValue, 1, "JuActivityID", id, "AutoId,FileName,FilePath,FileClass");
            }
        }
        /// <summary>
        /// 检查政策对象
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        protected string CheckPolicyObject(string objectName)
        {
            if (string.IsNullOrWhiteSpace(nInfo.K2) && objectName == "个人") return "checked='checked'";
            if (!string.IsNullOrWhiteSpace(nInfo.K2) && objectName == nInfo.K2) return "checked='checked'";
            return "";
        }

        /// <summary>
        /// 检查政策对象
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        protected string CheckPolicyObjectShow(string objectName)
        {
            if (string.IsNullOrWhiteSpace(nInfo.K2) && objectName == "个人") return "";
            if (!string.IsNullOrWhiteSpace(nInfo.K2) && objectName == nInfo.K2) return "";
            return "style='display:none;'";
        }
        /// <summary>
        /// 检查户籍所在地
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        protected string CheckDomicilePlace(string place)
        {
            if (string.IsNullOrWhiteSpace(nInfo.K7) && place=="无要求") return "checked='checked'";
            if (!string.IsNullOrWhiteSpace(nInfo.K7) && nInfo.K7.Contains(place)) return "checked='checked'";
            return "";
        }
        /// <summary>
        /// 检查性别
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        protected string CheckSex(string sex)
        {
            if (string.IsNullOrWhiteSpace(nInfo.K8) && sex == "无要求") return "checked='checked'";
            if (!string.IsNullOrWhiteSpace(nInfo.K8) && sex == nInfo.K8) return "checked='checked'";
            return "";
        }
        /// <summary>
        /// 是否显示年龄段设置
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        protected string CheckAgeShow(string sex)
        {
            if (!string.IsNullOrWhiteSpace(nInfo.K2) && nInfo.K2 == "单位") return "style='display:none;'";
            if (string.IsNullOrWhiteSpace(nInfo.K8) || nInfo.K8 == "无要求") return "";
            if (!string.IsNullOrWhiteSpace(nInfo.K8) && sex == nInfo.K8) return "";
            return "style='display:none;'";
        }

        /// <summary>
        /// 检查字段
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        protected string CheckField(string field,string checkStr)
        {
            if (string.IsNullOrWhiteSpace(field) && checkStr == "无要求") return "checked='checked'";
            if (!string.IsNullOrWhiteSpace(field) && checkStr == field) return "checked='checked'";
            return "";
        }
        /// <summary>
        /// 检查字段
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        protected string CheckMultField(string field, string checkStr)
        {
            if (string.IsNullOrWhiteSpace(field) && checkStr == "无要求") return "checked='checked'";
            if (!string.IsNullOrWhiteSpace(field) && field.Contains(checkStr)) return "checked='checked'";
            return "";
        }
        /// <summary>
        /// 是否显示工作年限设置
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        protected string CheckJobLifeShow()
        {
            if (!string.IsNullOrWhiteSpace(nInfo.K2) && nInfo.K2 == "单位") return "style='display:none;'";
            if (string.IsNullOrWhiteSpace(nInfo.K16)) return "style='display:none;'";
            if (!string.IsNullOrWhiteSpace(nInfo.K16) && nInfo.K16 != "就业") return "style='display:none;'";
            return "";
        }
        /// <summary>
        /// 是否显示失业期限设置
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        protected string CheckUnemploymentPeriodShow()
        {
            if (!string.IsNullOrWhiteSpace(nInfo.K2) && nInfo.K2 == "单位") return "style='display:none;'";
            if (string.IsNullOrWhiteSpace(nInfo.K16)) return "style='display:none;'";
            if (!string.IsNullOrWhiteSpace(nInfo.K16) && nInfo.K16 == "就业") return "style='display:none;'";
            if (!string.IsNullOrWhiteSpace(nInfo.K16) && nInfo.K16 == "无要求") return "style='display:none;'";
            return "";
        }
    }
}