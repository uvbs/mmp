using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    public class BLLWebsiteDomainInfo : BLL
    {
        public List<WebsiteDomainInfo> GetDomainListByOwner(string websiteOwner)
        {
            return GetList<WebsiteDomainInfo>(string.Format(" WebsiteOwner='{0}' ", websiteOwner));
        }
        /// <summary>
        /// 获取站点域名 第一个
        /// </summary>
        /// <returns></returns>
        public string GetWebsiteDoMain(string websiteOwner) {

            var doMainInfo = Get<WebsiteDomainInfo>(string.Format(" WebsiteOwner='{0}' AND WebsiteDomain != 'localhost' ", websiteOwner));
            string doMain = "";
            if (doMainInfo != null)
            {
                doMain = doMainInfo.WebsiteDomain;
            }
            if (websiteOwner=="comeoncloud")
            {
                doMain ="comeoncloud.comeoncloud.net";
            }
            return doMain;
        
        }

    }
}
