﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Admin.Mall.Settlement.SupplierChannel
{
    public partial class SettlementList : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        /// <summary>
        ///供应商渠道列表
        /// </summary>
        public List<UserInfo> supplierChannelList = new List<UserInfo>();
        public List<int> yearList = new List<int>();
        protected void Page_Load(object sender, EventArgs e)
        {
            int totalCount = 0;
            supplierChannelList = bllUser.GetSupplierChannelList(bllUser.WebsiteOwner, 1, int.MaxValue, "", "", out totalCount);
            supplierChannelList = supplierChannelList.OrderBy(p => p.Company).ToList();

            DateTime dtFrom = new DateTime(2017, 1, 1);
            yearList.Add(2017);
            if (DateTime.Now.Year > dtFrom.Year)
            {
                for (int i = 0; i < DateTime.Now.Year - dtFrom.Year; i++)
                {
                    DateTime dt = dtFrom.AddYears(i + 1);
                    yearList.Add(dt.Year);

                }
            }
        }
    }
}