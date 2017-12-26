using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class TeamPerformance
    {
        public TeamPerformance()
        {
            DetailIDList = new List<int>();
        }
        public TeamPerformance(string websiteOwner, int yearMonth, string userId, string distributionOwner, 
            string userName, string userPhone, decimal performance, int detailID)
        {

            WebsiteOwner = websiteOwner;
            YearMonth = yearMonth;
            UserId = userId;
            DistributionOwner = distributionOwner;
            UserName = userName;
            UserPhone = userPhone;
            Performance = performance;

            DetailIDList = new List<int>();
            if (detailID>0) DetailIDList.Add(detailID);
        }
        public List<int> DetailIDList { get; set; }
    }
}
