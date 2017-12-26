using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    /// <summary>
    /// 众筹信息扩展
    /// </summary>
    public partial class CrowdFundInfo : ZCBLLEngine.ModelTable
    {

        /// <summary>
        ///付款人数
        /// </summary>
        public int PayPersionCount
        {
            get
            {
                try
                {
                    if (CrowdFundID>0)
                    {
                         return new BLL().GetCount<CrowdFundRecord>(string.Format("CrowdFundID={0} And Status=1", CrowdFundID));
                    }
                    else
                    {
                        return new BLL().GetCount<CrowdFundRecord>(string.Format("CrowdFundID={0} And Status=1", AutoID));
                    }
                   

                }
                catch (Exception)
                {

                    return 0;
                }
            }
        }
        /// <summary>
        ///已付款总金额
        /// </summary>
        public decimal TotalPayAmount
        {
            get
            {
                try
                {
                    if (CrowdFundID > 0)
                    {
                        return Convert.ToDecimal(ZentCloud.ZCBLLEngine.BLLBase.GetSingle(string.Format("select sum(Amount) from ZCJ_CrowdFundRecord where CrowdFundID={0} And Status=1", CrowdFundID)));
                    
                    }

                    else
                    {
                        return Convert.ToDecimal(ZentCloud.ZCBLLEngine.BLLBase.GetSingle(string.Format("select sum(Amount) from ZCJ_CrowdFundRecord where CrowdFundID={0} And Status=1", AutoID)));
                    }
                   

                }
                catch (Exception)
                {

                    return 0;
                }
            }
        }
        /// <summary>
        /// 进度 百分比
        /// </summary>
        public double PayPercent
        {
            get
            {
                try
                {
                    double Percent = 0;

                    Percent = Math.Round((Convert.ToDouble(TotalPayAmount) / Convert.ToDouble(FinancAmount)) * 100, 0);
                    return Percent;

                }
                catch (Exception)
                {

                    return 0;
                }
            }



        }
        /// <summary>
        /// 投标剩余天数
        /// </summary>
        public double RemainingDays
        {
            get
            {
                try
                {
                    double day = 0;
                    day = Math.Ceiling((StopTime - DateTime.Now).TotalDays);
                    return day > 0 ? day : 0;

                }
                catch (Exception)
                {

                    return 0;
                }
            }



        }

    }
}
