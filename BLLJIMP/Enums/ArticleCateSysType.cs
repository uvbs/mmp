using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Enums
{
    /// <summary>
    /// 文章分类系统类型
    /// </summary>
    public enum ArticleCateSysType
    {
        /// <summary>
        /// 普通类型
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 系统类型，可能跟逻辑相关，id不能随便更改，不能随便删除
        /// </summary>
        System = 1
    }
}
