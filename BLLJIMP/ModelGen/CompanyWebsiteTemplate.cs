using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 企业网站模板
    /// </summary>
   public class CompanyWebsiteTemplate:ZCBLLEngine.ModelTable
    {
       /// <summary>
       /// 自动标识
       /// </summary>
       public int AutoID { get; set; }
       /// <summary>
       /// 模板名称
       /// </summary>
       public string TemplateName { get; set; }
       /// <summary>
       /// 模板文件夹名称
       /// </summary>
       public string TemplatePath { get; set; }
       /// <summary>
       ///模板缩略图
       /// </summary>
       public string TemplateThumbnail{ get; set; }


    }
}
