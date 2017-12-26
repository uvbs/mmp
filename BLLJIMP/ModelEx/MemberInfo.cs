using System;
namespace ZentCloud.BLLJIMP.Model
{
    public partial class MemberInfo : ZCBLLEngine.ModelTable
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

