using System;
using System.Collections.Generic;
namespace ZentCloud.BLLJIMP.Model
{
	/// <summary>
	/// 接收人列表
	/// </summary>
    public partial class EmailDetails : ZCBLLEngine.ModelTable
	{
        public string SendStatusDescription
        {
            get
            {
                return Common.DictionaryClump.EmailSendStatusDictionary[_sendstatus];
            }
        }


        
	}
}

