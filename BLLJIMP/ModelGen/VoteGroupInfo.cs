using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 投票分组
    /// </summary>
    public class VoteGroupInfo:ModelTable
    {
        #region base
        /// <summary>
        /// 分组id
        /// </summary>
        public int VoteGroupId { get; set; }

        /// <summary>
        /// 投票id
        /// </summary>
        public int VoteId { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string VoteGroupName { get; set; }

        /// <summary>
        /// 组员：逗号分隔多个组员
        /// </summary>
        public string VoteGroupMembers { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        #endregion

        #region ex

        
        public List<VoteObjectInfo> ObjectList
        {
            get
            {
                List<VoteObjectInfo> result = new List<VoteObjectInfo>();

                if (!string.IsNullOrWhiteSpace(VoteGroupMembers))
                {
                    result = BLLStatic.bll.GetList<VoteObjectInfo>(10000,string.Format(" AutoID IN ({0}) ", VoteGroupMembers)," [RANK] DESC ");
                }

                return result;
            }
        }

        #endregion
    }
}
