using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 群发素材 图文
    /// </summary>
    public class WXMassArticle : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动编号
       /// </summary>
       public int AutoID { get; set; }
       /// <summary>
       /// 缩略图
       /// </summary>
       public string ThumbImage { get; set; }
       /// <summary>
       /// 图文消息的作者
       /// </summary>
       public string Author { get; set; }
       /// <summary>
       /// 图文消息的标题
       /// </summary>
       public string Title { get; set; }
       /// <summary>
       /// 在图文消息页面点击“阅读原文”后的页面
       /// </summary>
       public string Content_Source_Url { get; set; }
       /// <summary>
       /// 图文消息页面的内容，支持HTML标签
       /// </summary>
       public string Content { get; set; }
       /// <summary>
       /// 图文消息的描述
       /// </summary>
       public string Digest { get; set; }
       /// <summary>
       /// 是否显示封面，1为显示，0为不显示
       /// </summary>
       public int Show_Cover_Pic { get; set; }
       /// <summary>
       /// 站点所有者
       /// </summary>
       public string WebsiteOwner { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
       public int Sort { get; set; }

    }
}
