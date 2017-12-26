using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// PC 页面
    /// </summary>
    public class PcPage : ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 页面编号
       /// </summary>
       public int PageId { get; set; }
       /// <summary>
       /// 页面名称
       /// </summary>
       public string PageName { get; set; }
       /// <summary>
       /// 最顶部内容
       /// </summary>
       public string TopContent { get; set; }
       /// <summary>
       /// Logo
       /// </summary>
       public string Logo { get; set; }
       /// <summary>
       /// 顶部菜单 存储的是导航的Id
       /// </summary>
       public string TopMenu { get; set; }
       /// <summary>
       ///中部内容
       ///存储幻灯片 编辑器内容等
       /// </summary>
       public string MiddContent { get; set; }
       /// <summary>
       /// 底部内容 存储版权等
       /// </summary>
       public string BottomContent { get; set; }
       /// <summary>
       /// 站点所有者 
       /// </summary>
       public string WebsiteOwner { get; set; }
    }
}
