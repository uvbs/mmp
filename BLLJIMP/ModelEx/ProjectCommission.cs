using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class ProjectCommission
    {

         //<summary>
         //贡献用户信息
         //</summary>
        public UserInfo CommissionUserInfo
        {
            get;
            set;

        }
         //<summary>
         //受益用户信息
         //</summary>
        public UserInfo UserInfo
        {
            get;
            set;

        }

    }
}
