using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.App.Cation.Wap
{
    public partial class GreetingCardStatistics : System.Web.UI.Page
    {
        BLLJIMP.BLLJuActivity juBll;
        public BLLJIMP.Model.JuActivityInfo juActivityModel = new BLLJIMP.Model.JuActivityInfo();
        public BLLJIMP.Model.UserInfo userModel=new BLLJIMP.Model.UserInfo();
        public List<BLLJIMP.Model.ActivityDataInfo> signUpDataList=new List<BLLJIMP.Model.ActivityDataInfo>();
        public string pubjid;
        protected void Page_Load(object sender, EventArgs e)
        {
            userModel = DataLoadTool.GetCurrUserModel();
            juBll = new BLLJIMP.BLLJuActivity();
            //获取活动ID
            int jid = Convert.ToInt32(Request["jid"], 16);//由16进制转换
            pubjid = Request["jid"];
            juActivityModel = juBll.GetJuActivity(jid);

            //判断权限
            if (this.userModel.UserType != 1)
            {
                if (juActivityModel.UserID != this.userModel.UserID)
                {
                    Response.Write("<script>alert('无权访问该贺卡!');</script>");
                    Response.End();
                    return;
                }
            }

            try
            {
                //获取活动报名数据
                signUpDataList = this.juBll.QueryJuActivitySignUpData(jid);
            }
            catch { }

            if (signUpDataList == null)
                signUpDataList = new List<BLLJIMP.Model.ActivityDataInfo>();
        }
    }
}