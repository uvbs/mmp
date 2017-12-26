using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Test
{
    public partial class TestPerformance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            BLLDistribution bllDis = new BLLDistribution();
            string websiteOwner = TextBox3.Text.Trim();
            string yearMonthString = TextBox4.Text.Trim();
            int yearMonth = int.Parse(yearMonthString);
            if (string.IsNullOrWhiteSpace(websiteOwner))
            {
                ZentCloud.Common.WebMessageBox.Show(this, "请输入站点");
                return;
            }
            if (string.IsNullOrWhiteSpace(yearMonthString))
            {
                ZentCloud.Common.WebMessageBox.Show(this, "请输入月份");
                return;
            }
            try
            {
                DateTime dtime = DateTime.ParseExact(yearMonth.ToString() + "01", "yyyyMMdd", null);
            }
            catch (Exception ex)
            {
                ZentCloud.Common.WebMessageBox.Show(this, "月份格式错误");
            }
            try
            {
                bllDis.BuildMonthPerformance(yearMonth, websiteOwner);
                ZentCloud.Common.WebMessageBox.Show(this, "计算完成");
            }
            catch (Exception ex)
            {
                ZentCloud.Common.WebMessageBox.Show(this, "计算出错");
            }
        }
    }
}