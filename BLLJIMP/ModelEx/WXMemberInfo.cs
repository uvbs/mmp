using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微信会员
    /// </summary>
    public partial class WXMemberInfo : ZCBLLEngine.ModelTable
    {
        private string _wXHeadimgurlLocal;

        /// <summary>
        /// 针对会员头像QQ防盗链的情况，将头像图片下载到本地地址
        /// </summary>
        public string WXHeadimgurlLocal 
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(WXHeadimgurl))
                    return "";
                return new BLLJuActivity().DownLoadRemoteImage(WXHeadimgurl);
            }

            set { _wXHeadimgurlLocal = value; }
        }
    }
}
