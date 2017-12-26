using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.customize.totema.mobile
{
    
    public partial class ClassList : TotemaBase
    {
        BLLJIMP.BLLVote bllVote = new BLLJIMP.BLLVote();
        public List<VoteObjectInfo> data=new List<VoteObjectInfo>();
        protected void Page_Load(object sender, EventArgs e)
        {
            int totalcount;
            data = bllVote.GetVoteObjectInfoList(bllVote.TotemaVoteID, 1, 10000, out totalcount, Request["keyword"], "", "1");
        }

            //ClassModel model = new ClassModel();
                //model.classid = item.AutoID;
                //model.classimage = item.VoteObjectHeadImage;
                //model.classname = item.VoteObjectName;
                //model.classnumber = item.Number;
                //model.rank = item.Rank;
                //model.votecount = item.VoteCount;
                //model.watchword = item.Introduction;
                //apiresult.list.Add(model);
            

    }
}