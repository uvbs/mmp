using System;
namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 检测统计明细表
    /// </summary>
    public partial class EmailEventDetailsInfo : ZCBLLEngine.ModelTable
    {
        public string EventTypeDescription
        {
            get
            {
                if (_eventtype.Equals(0))
                    return "阅读";
                if (_eventtype.Equals(1))
                    return "点击";
                return "";
            }
        }

        /// <summary>
        /// 点击链接地址
        /// </summary>
        public string ClickUrl
        {
            get
            {
                try
                {
                    BLL bll = new BLL("");

                    return bll.Get<EmailLinkInfo>(string.Format(" LinkID = '{0}' ", _linkid)).RealLink;
                }
                catch { }

                return "";
            }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }
    }
}

