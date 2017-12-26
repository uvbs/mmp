using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
   
    public partial class WeiboUserCollect : ZCBLLEngine.ModelTable
    {
        public string GroupName
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(_groupid))
                            return new BLL("").Get<MemberGroupInfo>(string.Format(" GroupID = '{0}' ", _groupid)).GroupName;
                    return "无分组";

                }
                catch (Exception)
                {

                    return "无分组";
                }
            }
        }
    }
}
