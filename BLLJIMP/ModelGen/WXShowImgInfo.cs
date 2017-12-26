using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 微秀图片
    /// </summary>
    public class WXShowImgInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 微秀ID
        /// </summary>
        public int ShowId { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImgStr { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string ShowTitle { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string ShowContext { get; set; }
        /// <summary>
        /// 动画方向
        /// </summary>
        public int ShowAnimation { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string ShowTitleColor { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string ShowContextColor { get; set; }


    }
}
