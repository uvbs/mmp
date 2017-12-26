using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{

    /// <summary>
    ///条形码配置 无用
    /// </summary>
    [Serializable]
    public class ConfigBarCodeInfo : ZCBLLEngine.ModelTable
    {
        public ConfigBarCodeInfo()
        { }
        #region Model

        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public string PopupInfo { get; set; }

        /// <summary>
        /// 查询次数
        /// </summary>
        public int QueryNum { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        public string websiteOwner { get; set; }


        #endregion Model
    }
}
