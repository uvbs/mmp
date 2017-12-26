using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 网站域名
    /// </summary>
    [Serializable]
    public partial class WebsiteDomainInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 网站域名
        /// </summary>
        public WebsiteDomainInfo()
        { }
        #region Model
        private string _websitedomain;
        private string _websiteowner;
        /// <summary>
        /// 域名
        /// </summary>
        public string WebsiteDomain
        {
            set { _websitedomain = value; }
            get { return _websitedomain; }
        }
        /// <summary>
        /// 站点所有者
        /// </summary>
        public string WebsiteOwner
        {
            set { _websiteowner = value; }
            get { return _websiteowner; }
        }
        #endregion Model

    }
}

