using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.test.efast
{
    public partial class GetPaymentList : System.Web.UI.Page
    {
        Open.EfastSDK.Client client = new Open.EfastSDK.Client();
        Open.EZRproSDK.Client zrClient = new Open.EZRproSDK.Client();
        BLLJIMP.BLLEfast bllEfast = new BLLJIMP.BLLEfast();

        protected void Page_Load(object sender, EventArgs e)
        {

            //GridView1.DataSource = client.GetPaymentList();

            //this.DataBind();
        }

        protected void btnGetSku_Click(object sender, EventArgs e)
        {
            GridView1.DataSource = client.GetSkuList().list;

            this.DataBind();
        }

        protected void btnGetColorList_Click(object sender, EventArgs e)
        {
            int newCount = 0, updateCount = 0;
            bllEfast.ColorSync(out newCount, out updateCount);

            Response.Write("新增数据" + newCount.ToString() + "条，更新数据" + updateCount.ToString() + "条");

        }

        protected void btnGetSizeList_Click(object sender, EventArgs e)
        {
            int newCount = 0, updateCount = 0;
            bllEfast.SizeSync(out newCount, out updateCount);

            Response.Write("新增数据" + newCount.ToString() + "条，更新数据" + updateCount.ToString() + "条");
        }

        protected void btnGetGoodsList_Click(object sender, EventArgs e)
        {
            int newCount = 0, updateCount = 0;
            bllEfast.GoodsSync(out newCount, out updateCount);

            Response.Write("新增数据" + newCount.ToString() + "条，更新数据" + updateCount.ToString() + "条");
        }

        protected void btnGetSkuAndStock_Click(object sender, EventArgs e)
        {
            int newCount = 0, updateCount = 0;
            bllEfast.SkuSync(out newCount, out updateCount);

            Response.Write("新增数据" + newCount.ToString() + "条，更新数据" + updateCount.ToString() + "条");
        }

        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {
            Response.Write(client.CreateOrder());
        }

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            var outOrderId = this.txtInput.Text.Trim();
            var result = bllEfast.CancelOrderByOutId(outOrderId);
        }

        protected void btnGetShipping_Click(object sender, EventArgs e)
        {
            var outOrderId = this.txtInput.Text.Trim();

            string code = "", number = "";

            var result = bllEfast.GetOrderExpressInfoByOutId(outOrderId, out code, out number);

            Response.Write(string.Format(" 发货状态：{0},快递代码：{1}，快递号：{2} ", result, code, number));

        }

        protected void btnChangeBrouns_Click(object sender, EventArgs e)
        {
            var resp = zrClient.BonusUpdate(this.txtInput.Text.Trim(), Convert.ToInt32(this.txtInput2.Text.Trim()), "至云测试");

            Response.Write("目前可以积分为：" + resp.Bonus);

        }
    }
}