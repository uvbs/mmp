using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web
{
    public partial class IPProcess : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //读取有ip的数据

            try
            {
                var dataList = bll.GetList<BLLJIMP.Model.VoteLogInfo>(" IP IS NOT NULL AND IPLocation IS NULL ");
                int i = 0;
                foreach (var item in dataList)
                {

                    var location = Common.MySpider.GetIPLocation(item.IP);

                    i += bll.Update(new BLLJIMP.Model.VoteLogInfo(), string.Format(" IPLocation = '{0}' ", location), string.Format(" AutoID = {0} ", item.AutoID));

                }
                Response.Write(i.ToString());

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                var dataList = bll.GetList<BLLJIMP.Model.VoteLogInfo_100_2959_1_proc>("");

                int i = 0;
                foreach (var item in dataList)
                {

                    string tp = "", reg = "", last = "";

                    if (!string.IsNullOrEmpty(item.tpIP))
                        tp = Common.MySpider.GetIPLocation(item.tpIP);
                    if (!string.IsNullOrEmpty(item.RegIP))
                        reg = Common.MySpider.GetIPLocation(item.RegIP);
                    if (!string.IsNullOrEmpty(item.LastLoginIP))
                        last = Common.MySpider.GetIPLocation(item.LastLoginIP);

                    i += bll.Update(new BLLJIMP.Model.VoteLogInfo_100_2959_1_proc(), string.Format(" TpIPLocation = '{0}',RegIPLocation = '{1}',LastLoginIPLocation = '{2}' ", tp, reg, last), string.Format(" AutoID = {0} ", item.AutoId));

                }
                Response.Write(i.ToString());
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            var dataList = bll.GetList<BLLJIMP.Model.VoteLogInfo_59_2918_2_proc>("");

            int i = 0;
            foreach (var item in dataList)
            {

                string tp = "", reg = "", last = "";

                if (!string.IsNullOrEmpty(item.tpIP))
                    tp = Common.MySpider.GetIPLocation(item.tpIP);
                if (!string.IsNullOrEmpty(item.RegIP))
                    reg = Common.MySpider.GetIPLocation(item.RegIP);
                if (!string.IsNullOrEmpty(item.LastLoginIP))
                    last = Common.MySpider.GetIPLocation(item.LastLoginIP);

                i += bll.Update(new BLLJIMP.Model.VoteLogInfo_59_2918_2_proc(), string.Format(" TpIPLocation = '{0}',RegIPLocation = '{1}',LastLoginIPLocation = '{2}' ", tp, reg, last), string.Format(" AutoID = {0} ", item.AutoId));

            }
            Response.Write(i.ToString());
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            var dataList = bll.GetList<BLLJIMP.Model.VoteLogInfo_9_2868_3_proc>("");

            int i = 0;
            foreach (var item in dataList)
            {

                string tp = "", reg = "", last = "";

                if (!string.IsNullOrEmpty(item.tpIP))
                    tp = Common.MySpider.GetIPLocation(item.tpIP);
                if (!string.IsNullOrEmpty(item.RegIP))
                    reg = Common.MySpider.GetIPLocation(item.RegIP);
                if (!string.IsNullOrEmpty(item.LastLoginIP))
                    last = Common.MySpider.GetIPLocation(item.LastLoginIP);

                i += bll.Update(new BLLJIMP.Model.VoteLogInfo_9_2868_3_proc(), string.Format(" TpIPLocation = '{0}',RegIPLocation = '{1}',LastLoginIPLocation = '{2}' ", tp, reg, last), string.Format(" AutoID = {0} ", item.AutoId));

            }
            Response.Write(i.ToString());
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            var dataList = bll.GetList<BLLJIMP.Model.VoteLogInfo_33_2892_4_proc>("");

            int i = 0;
            foreach (var item in dataList)
            {

                string tp = "", reg = "", last = "";

                if (!string.IsNullOrEmpty(item.tpIP))
                    tp = Common.MySpider.GetIPLocation(item.tpIP);
                if (!string.IsNullOrEmpty(item.RegIP))
                    reg = Common.MySpider.GetIPLocation(item.RegIP);
                if (!string.IsNullOrEmpty(item.LastLoginIP))
                    last = Common.MySpider.GetIPLocation(item.LastLoginIP);

                i += bll.Update(new BLLJIMP.Model.VoteLogInfo_33_2892_4_proc(), string.Format(" TpIPLocation = '{0}',RegIPLocation = '{1}',LastLoginIPLocation = '{2}' ", tp, reg, last), string.Format(" AutoID = {0} ", item.AutoId));

            }
            Response.Write(i.ToString());
        }


    }
}