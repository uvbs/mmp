using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP.Model
{
  public partial class SMSTriggerDetails : ZCBLLEngine.ModelTable
    {

      public string MessageSubmitStatus
      {

          get
          {
              return new BLL("").Get<CodeListInfo>(string.Format("CodeType = 'SubmitStatus' and CodeValue={0}", _submitstatus)).CodeName;
          }



      }
    }
}
