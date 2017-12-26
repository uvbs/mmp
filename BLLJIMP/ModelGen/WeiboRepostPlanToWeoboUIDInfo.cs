using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微博转发@微博ID表
    /// </summary>
    [Serializable]
    public partial class WeiboRepostPlanToWeoboUIDInfo : ZCBLLEngine.ModelTable
    {
        public WeiboRepostPlanToWeoboUIDInfo()
        { }
        #region Model
        private int _planid;
        private string _weibouid;
        /// <summary>
        /// 
        /// </summary>
        public int PlanID
        {
            set { _planid = value; }
            get { return _planid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string WeiboUID
        {
            set { _weibouid = value; }
            get { return _weibouid; }
        }
        #endregion Model

    }
}

