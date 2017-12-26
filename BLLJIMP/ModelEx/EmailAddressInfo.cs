using System;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 邮箱地址库数据
    /// </summary>
    public partial class EmailAddressInfo : ZCBLLEngine.ModelTable
    {
        public string GroupName
        {
            get
            {
                try
                {
                    if (_groupid != null)
                        if (_groupid != "0")
                            return new BLL("").Get<MemberGroupInfo>(string.Format(" GroupID = '{0}' ", _groupid)).GroupName;
                }
                catch { }
                return "无";
            }
        }
    }
}
