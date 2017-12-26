using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model.API.forbes
{
    /// <summary>
    /// 问卷模型
    /// </summary>
    public class QuestionnaireModel
    {
        /// <summary>
        ///编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 问卷名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string imgurl { get; set; }
        /// <summary>
        /// 问卷描述
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 问卷链接
        /// </summary>
        public string link { get; set; }


    }
    /// <summary>
    /// 问卷api模型
    /// </summary>
    public class QuestionnaireApi
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalcount { get; set; }
        /// <summary>
        /// 分类集合
        /// </summary>
        public List<QuestionnaireModel> list { get; set; }
    }


}
