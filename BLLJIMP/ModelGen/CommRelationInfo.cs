using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 通用关系表：
    /// 针对大多数通用情况的关系可用此表，
    /// 根据type区分，如 邀请回答问题的人，提醒相关人，提醒相关的用户组，关联的标签，关联的分类 等，根据类型程序扩展区分
    /// </summary>
    public class CommRelationInfo : ZCBLLEngine.ModelTable
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 主id
        /// </summary>
        public string MainId { get; set; }
        /// <summary>
        /// 关系id
        /// </summary>
        public string RelationId { get; set; }
        /// <summary>
        /// 关系类型
        /// </summary>
        public string RelationType { get; set; }
        /// <summary>
        /// 关联时间
        /// </summary>
        public DateTime RelationTime { get; set; }
        
        /// <summary>
        /// 扩展Id 
        /// </summary>
        public string ExpandId { get; set; }
        
        public string Ex1 { get; set; }

        public string Ex2 { get; set; }

        public string Ex3 { get; set; }
        public string Ex4 { get; set; }
        public string Ex5 { get; set; }
        public string Ex6 { get; set; }
        public string Ex7 { get; set; }
        public string Ex8 { get; set; }
        public string Ex9 { get; set; }
        public string Ex10 { get; set; }

        public string WebsiteOwner { get; set; }
    }
}
