using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZentCloud.ZCBLLEngine;
using ZentCloud.BLLJIMP.Model;
using System.Web;
using ZentCloud.BLLJIMP.Model.HaiMa;

namespace ZentCloud.BLLJIMP
{
    /// <summary>
    /// 海马业务逻辑类
    /// </summary>
    public class BLLHaiMa:BLL
    {
        public BLLHaiMa()
        {

        }

       
        /// <summary>
        /// 获取所有门店信息
        /// </summary>
        /// <returns></returns>
        public List<HaiMaStore> GetStoreList()
        {
            return GetList<HaiMaStore>();

        }
        /// <summary>
        /// 根据省份获取门店信息
        /// </summary>
        /// <param name="province">省份名称</param>
        /// <returns></returns>
        public List<HaiMaStore> GetStoreList(string province)
        {
            return GetList<HaiMaStore>(string.Format(" Province='{0}'", province));

        }
        /// <summary>
        /// 获取单个门店
        /// </summary>
        /// <param name="province">省份</param>
        /// <param name="storeName">门店名称</param>
        /// <param name="storeCode">门店代码</param>
        /// <returns></returns>
        public HaiMaStore GetSingleStore(string province, string storeName, string storeCode)
        {

            return Get<HaiMaStore>(string.Format(" Province='{0}' And StoreName='{1}' And StoreCode='{2}'", province, storeName, storeCode));

        }
        /// <summary>
        /// 检查用户是否注册
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool IsReg(UserInfo userInfo)
        {
            if (userInfo==null)
            {
                return false;
            }
            if ((!string.IsNullOrEmpty(userInfo.TrueName)) && (!string.IsNullOrEmpty(userInfo.Phone)))
            {
                return true;
            }
            return false;
        }





    }
}
