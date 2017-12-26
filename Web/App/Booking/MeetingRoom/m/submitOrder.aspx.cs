using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.App.Booking.MeetingRoom.m
{
    public partial class submitOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string indexStr = File.ReadAllText(this.Server.MapPath("submitOrder.html"));
            indexStr = indexStr.Replace("index.html", "index.aspx");
            indexStr = indexStr.Replace("orderlist.html", "orderlist.aspx");
            indexStr = indexStr.Replace("orderDetail.html", "orderDetail.aspx");
            indexStr = indexStr.Replace("submitOrder.html", "submitOrder.aspx");
            indexStr = indexStr.Replace("/OpenWebApp/", "/");

            #region 根据类型输出配置
            string categoryType = Request["type"] == null ? "MeetingRoom" : Request["type"];
            BLLJIMP.BLLArticleCategory bllArticleCategory = new BLLJIMP.BLLArticleCategory();
            ArticeCategoryTypeResponse bookingConfig = bllArticleCategory.GetTypeConfig(bllArticleCategory.WebsiteOwner, categoryType);
            indexStr = indexStr.Replace("<title>我的预订</title>", string.Format("<title>{0}</title>", bookingConfig.title));
            indexStr = indexStr.Replace("var BookingConfig = {};", string.Format("var BookingConfig = {0};", JsonConvert.SerializeObject(bookingConfig)));
            #endregion

            this.Response.Write(indexStr);
        }
    }
}