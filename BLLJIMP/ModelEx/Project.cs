using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class Project
    {

        /// <summary>
        /// 
        /// </summary>
        public UserInfo UserInfo
        {
            get
            {
                return new BLLUser().GetUserInfo(UserId);
            }

        }

    }
}
