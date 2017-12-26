using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public partial class SMSDetails : ZentCloud.ZCBLLEngine.ModelTable
    {

        //public string Name
        //{
        //    get 
        //    {
        //        return BLLMember.GetName(this.Receiver);
        //    }
        //}



        public string Carrier
        {
            get
            {
                return GetCarrier(Receiver);
            }
        }

        private string GetCarrier(string phoneNumber)
        {
            if (!Common.ValidatorHelper.PhoneNumLogicJudge(phoneNumber))
            {
                return "无效号码";
            }
            
            if (Common.ValidatorHelper.YDPhoneNumLogicJudge(phoneNumber))
            {
                return "中国移动";
            }

            if (Common.ValidatorHelper.DXPhoneNumLogicJudge(phoneNumber))
            {
                return "中国电信";
            }

            if (Common.ValidatorHelper.LTPhoneNumLogicJudge(phoneNumber))
            {
                return "中国联通";
            }

            

            return "无效号码";
        }
    }
}
