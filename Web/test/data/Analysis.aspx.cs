using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace ZentCloud.JubitIMP.Web.test.data
{
    public partial class Analysis : System.Web.UI.Page
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        BLLJIMP.BLLMall bllMall = new BLLJIMP.BLLMall();

        public int objId = 0;
        public int autoId = 0;
        public int currMonth = 0;
        public int currDay = 0;
        public int number = 0;
        static List<BLLJIMP.Model.VoteLogInfo> dataList = new List<BLLJIMP.Model.VoteLogInfo>();
            
        protected void Page_Load(object sender, EventArgs e)
        {
            objId = GetSessionKey("objId");
            autoId = GetSessionKey("autoId");
            currMonth = GetSessionKey("currMonth");
            currDay = GetSessionKey("currDay");
            number = GetSessionKey("number");

        }

        private int GetSessionKey(string key)
        {
            int result = 0;

            if (Session[key] != null)
            {
                result = (int)Session[key];
            }

            return result;
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            objId = int.Parse(this.txtTpObjId.Text.Trim());
            autoId = int.Parse(this.txtId.Text.Trim());

            var classDetail = bll.Get<BLLJIMP.Model.VoteObjectInfo>(string.Format(" Number = {0} and VoteID = {1} ", autoId, objId));

            if (classDetail != null)
            {
                number = autoId;
                dataList = bll.GetList<BLLJIMP.Model.VoteLogInfo>(string.Format(" VoteObjectID = {0} and VoteID = {1} ", classDetail.AutoID, objId));
                loadMonthDataView();
            }

            
        }

        private void loadMonthDataView()
        {
            if (dataList.Count == 0)
            {
                return;
            }

            var minTime = dataList.Min(p => p.InsertDate);

            var maxTime = dataList.Max(p => p.InsertDate);

            List<dynamic> result = new List<dynamic>();

            for (int i = minTime.Month; i <= maxTime.Month; i++)
            {
                result.Add(new
                {
                    月份 = i,
                    票数 = dataList.Count(p => p.InsertDate.Month == i)
                });
            }

            this.grvMonth.DataSource = result;
            this.grvMonth.DataBind();

        }

        private void loadDayDataView(int m)
        {
            currMonth = m;
            Session["currMonth"] = m;
            var dayDataSource = dataList.Where(p => p.InsertDate.Month == m);
            var maxTime = dayDataSource.Max(p => p.InsertDate);
            List<dynamic> result = new List<dynamic>();

            for (int i = maxTime.Day; i > 0 ; i--)
            {
                result.Add(new
                {
                    日期 = i,
                    票数 = dayDataSource.Count(p => p.InsertDate.Day == i)
                });
            }

            this.grvDay.DataSource = result;
            this.grvDay.DataBind();
        }

        private void loadHourDataView(int d) 
        {
            currDay = d;
            Session["currDay"] = d;
            var dayDataSource = dataList.Where(p => p.InsertDate.Month == currMonth && p.InsertDate.Day == d).ToList();
            List<dynamic> result = new List<dynamic>();
            for (int i = 0; i < 24; i++)
            {
                result.Add(new
                {
                    时 = i,
                    票数 = dayDataSource.Count(p => p.InsertDate.Hour == i)
                });
            }
            this.grvHour.DataSource = result;
            this.grvHour.DataBind();

            List<dynamic> resultDetail = new List<dynamic>();

            foreach (var item in dayDataSource.OrderBy(p => p.InsertDate))
            {
                //var userInfo = this.bllUser.GetUserInfo(item.UserID);
                resultDetail.Add(new
                {
                    时间 = item.InsertDate,
                    IP = item.IP,
                    地址 = item.IPLocation
                    //,
                    //昵称 = userInfo.WXNickname == null ? "" : userInfo.WXNickname.Trim(),
                    //头像 = userInfo.WXHeadimgurl == null ? "" : userInfo.WXHeadimgurl.Trim(),
                    //openId = userInfo.WXOpenId == null ? "" : userInfo.WXOpenId.Trim()
                });
            }

            this.grvDayDetail.DataSource = resultDetail;
            this.grvDayDetail.DataBind();
        }

        protected void grvMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void grvMonth_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void grvMonth_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            string str = this.grvMonth.Rows[e.NewSelectedIndex].Cells[1].Text;

            loadDayDataView(int.Parse(str));

        }

        protected void grvDay_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            string str = this.grvDay.Rows[e.NewSelectedIndex].Cells[1].Text;

            loadHourDataView(int.Parse(str));
        }

        protected void grvHour_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            string str = this.grvHour.Rows[e.NewSelectedIndex].Cells[1].Text;
            int h = int.Parse(str);

            var dayDataSource = dataList.Where(p => p.InsertDate.Month == currMonth && p.InsertDate.Day == currDay && p.InsertDate.Hour == h).ToList();

            List<dynamic> resultDetail = new List<dynamic>();

            foreach (var item in dayDataSource.OrderBy(p => p.InsertDate))
            {
                //var userInfo = this.bllUser.GetUserInfo(item.UserID);
                resultDetail.Add(new
                {
                    时间 = item.InsertDate,
                    IP = item.IP,
                    地址 = item.IPLocation
                    //,
                    //昵称 = userInfo.WXNickname == null ? "" : userInfo.WXNickname.Trim(),
                    //头像 = userInfo.WXHeadimgurl == null ? "" : userInfo.WXHeadimgurl.Trim(),
                    //openId = userInfo.WXOpenId == null ? "" : userInfo.WXOpenId.Trim()
                });
            }

            this.grvDayDetail.DataSource = resultDetail;
            this.grvDayDetail.DataBind();

        }

        protected void btnCreateSubDomain_Click(object sender, EventArgs e)
        {

            List<BLLJIMP.Model.WebsiteDomainInfo> newList = new List<BLLJIMP.Model.WebsiteDomainInfo>();
            List<BLLJIMP.Model.WebsiteDomainInfo> domainList = bll.GetList<BLLJIMP.Model.WebsiteDomainInfo>();

            this.grvMonth.DataSource = domainList;
            this.grvMonth.DataBind();
            
            List<string> websiteOwnerList = domainList.DistinctBy(p => p.WebsiteOwner).Select(p => p.WebsiteOwner).ToList();

            this.grvDay.DataSource = websiteOwnerList;
            this.grvDay.DataBind();

            //gotocloud8.com  gotocloud8.net

            foreach (var item in websiteOwnerList)
            {
                BLLJIMP.Model.WebsiteDomainInfo dataCom = new BLLJIMP.Model.WebsiteDomainInfo();
                dataCom.WebsiteOwner = item;
                dataCom.WebsiteDomain = string.Format("{0}.gotocloud8.com", item);

                BLLJIMP.Model.WebsiteDomainInfo dataNet = new BLLJIMP.Model.WebsiteDomainInfo();
                dataNet.WebsiteOwner = item;
                dataNet.WebsiteDomain = string.Format("{0}.gotocloud8.net", item);

                if (domainList.Count( p => 
                    p.WebsiteDomain.Equals(dataCom.WebsiteDomain,StringComparison.OrdinalIgnoreCase)
                    &&
                    p.WebsiteOwner.Equals(dataCom.WebsiteOwner, StringComparison.OrdinalIgnoreCase)
                    ) == 0)
                {
                    newList.Add(dataCom); 
                }

                if (domainList.Count(p => 
                    p.WebsiteDomain.Equals(dataNet.WebsiteDomain, StringComparison.OrdinalIgnoreCase)
                    &&
                    p.WebsiteOwner.Equals(dataNet.WebsiteOwner, StringComparison.OrdinalIgnoreCase)
                    ) == 0)
                {
                    newList.Add(dataNet);
                }
            }

            this.grvDayDetail.DataSource = newList;
            this.grvDayDetail.DataBind();

            foreach (var item in newList)
            {
                bll.Add(item);
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Write( Common.DEncrypt.ZCEncrypt(txtId.Text));
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var startTime = DateTime.Parse(this.txtTpObjId.Text);
            var memberIds = this.txtId.Text;

            var modelOrderId = "2502463";//模板id

            var data = bllMall.GetOrderInfo(modelOrderId);
            var details = bllMall.GetOrderDetailsList(modelOrderId);

            foreach (var memberId in memberIds.Split(','))
            {
                var user = bllUser.GetUserInfoByAutoID(int.Parse(memberId),data.WebsiteOwner);

                if (bllMall.GetCount<BLLJIMP.Model.WXMallOrderInfo>(string.Format(" OrderID = '{0}' AND  OrderUserID = '{1}'  ", modelOrderId,user.UserID)) > 0)
                {
                    continue;
                }

                //复制整个数据，修改 userid order id  autoid  inserttime 等

                startTime = startTime.AddMinutes(new Random().Next(3, 20));
                startTime = startTime.AddSeconds(new Random().Next(3, 20));

                var newData = data;
                newData.OrderID = bll.GetGUID(BLLJIMP.TransacType.AddMallOrder);
                newData.InsertDate = startTime;
                newData.OrderUserID = user.UserID;
                newData.Consignee = user.WXNickname;
                newData.PayTime = startTime;

                bllMall.Add(newData);

                for (int i = 0; i < details.Count; i++)
                {
                    var newDetail = details[i];
                    newDetail.AutoID = null;
                    newDetail.OrderID = newData.OrderID;

                    bllMall.Add(newDetail);

                }

            }
            Response.Write("ok");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            var modelOrderId = this.txtTpObjId.Text.Trim();//模板id
            var listStr = this.txtId.Text.Trim();

            var list = JsonConvert.DeserializeObject<List<MemberAndPayTime>>(listStr);
            
            var data = bllMall.GetOrderInfo(modelOrderId);
            var details = bllMall.GetOrderDetailsList(modelOrderId);

            foreach (var item in list)
            {
                var user = bllUser.GetUserInfoByAutoID(item.ID, data.WebsiteOwner);


                var newData = data;
                newData.OrderID = bll.GetGUID(BLLJIMP.TransacType.AddMallOrder);
                newData.InsertDate = item.Time;
                newData.OrderUserID = user.UserID;
                newData.Consignee = user.WXNickname;
                newData.PayTime = item.Time;

                bllMall.Add(newData);

                for (int i = 0; i < details.Count; i++)
                {
                    var newDetail = details[i];
                    newDetail.AutoID = null;
                    newDetail.OrderID = newData.OrderID;

                    bllMall.Add(newDetail);

                }
            }
            Response.Write("ok");

        }


        public class MemberAndPayTime
        {
            public int ID { get; set; }
            public DateTime Time { get; set; }
        }


    }
}