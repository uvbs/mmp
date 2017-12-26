using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.customize.kuanqiao
{
    public partial class SignUpData : System.Web.UI.Page
    {
        /// <summary>
        /// 申请状态
        /// </summary>
        public string applystatus;
        /// <summary>
        /// 导航
        /// </summary>
        public string title = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            applystatus = Request["status"];
            switch (applystatus)
            {
                case "new":
                    applystatus = "待处理";
                    title = "新增申请";
                    break;
                case "process":
                    applystatus = "正在处理";
                    title = "处理中申请";
                    break;
                case "complete":
                    applystatus = "审核成功,审核失败";
                    title = "已完成申请";
                    break;

                default:
                    break;
            }

        }
    }
}