using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 投票日志扩展
    /// </summary>
    public partial class VoteLogInfo : ZCBLLEngine.ModelTable
    {
        BLLVote bllVote = new BLLVote();
        /// <summary>
        /// 投票信息
        /// </summary>
       public VoteInfo VoteInfo  {

           get {
               return bllVote.GetVoteInfo(VoteID);

           }
       
       }
        /// <summary>
        /// 投票对象信息
        /// </summary>
       public VoteObjectInfo VoteObjectInfo
       {
           get
           {
               return bllVote.GetVoteObjectInfo(VoteObjectID);
           }

       }
        /// <summary>
        /// 投票名称
        /// </summary>
       public string VoteName { get { return VoteInfo == null ? "" : VoteInfo.VoteName; } }

       /// <summary>
       /// 投票对象名称
       /// </summary>
       public string VoteObjectName { get { return VoteObjectInfo == null ? "" : VoteObjectInfo.VoteObjectName; } }

       


    }
}
