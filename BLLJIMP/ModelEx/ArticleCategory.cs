﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class ArticleCategory
    {
        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
