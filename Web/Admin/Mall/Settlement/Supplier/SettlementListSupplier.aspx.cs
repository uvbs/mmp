﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZentCloud.JubitIMP.Web.Admin.Mall.Settlement.Supplier
{
    public partial class SettlementListSupplier : System.Web.UI.Page
    {
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        ///// <summary>
        /////供应商列表
        ///// </summary>
        //public List<UserInfo> supplierList = new List<UserInfo>();
        public List<int> yearList = new List<int>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (bllUser.GetCurrentUserInfo().UserType!=7)
            {
                Response.Write("无权访问,只有商户可访问本页面");
                Response.End();
            }
            //int totalCount = 0;
            //supplierList = bllUser.GetSupplierList(bllUser.WebsiteOwner, 1, int.MaxValue, "", "", out totalCount);
            //supplierList = supplierList.OrderBy(p => p.Company).ToList();

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