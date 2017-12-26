using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// api 分类模型
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 分类id编号
        /// </summary>
        public int categoryid { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string categoryname { get; set; }


    }
    /// <summary>
    /// 分类api模型
    /// </summary>
    public class CategoryApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 分类集合
        /// </summary>
        public List<Category> list { get; set; }
    }


}
