using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
 
    public partial class WeiboDetails : ZentCloud.ZCBLLEngine.ModelTable
    {

        #region ExtModel
        /// <summary>
        /// 发送状态说明
        /// </summary>
        public string WeiboSendStatusDescription
        {
            get
            {
                Model.CodeListInfo c = bll.Get<Model.CodeListInfo>(string.Format(" CodeType = 'WeiboSendStatus' and CodeValue = {0} ", _weibosendstatus.ToString()));
                return c.CodeName;
            }
        }
        /// <summary>
        /// 发送类型说明
        /// </summary>
        public string WeiboSendTypeDescription
        {
            get
            {
                Model.CodeListInfo c = bll.Get<Model.CodeListInfo>(string.Format(" CodeType = 'WeiboSendType' and CodeValue = {0} ", _weibosendtype.ToString()));
                return c.CodeName;
            }
        }
        #endregion

    }
}
