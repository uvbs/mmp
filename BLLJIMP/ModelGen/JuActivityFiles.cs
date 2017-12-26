using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 活动附件数据表
    /// </summary>
    [Serializable]
    public partial class JuActivityFiles : ZCBLLEngine.ModelTable
    {
        public JuActivityFiles()
        { }
        
        /// <summary>
        /// AutoId
        /// </summary>
        public int AutoId{set;get;}
        /// <summary>
        /// 活动ID
        /// </summary>
        public int JuActivityID{set;get;}
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID{set;get;}
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName{set;get;}
        
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FilePath{set;get;}
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { set; get; }
        /// <summary>
        /// 文件分类
        /// Policy 1政策原文附件 2办事指南原文附件
        /// </summary>
        public int FileClass { set; get; }
    }
}
