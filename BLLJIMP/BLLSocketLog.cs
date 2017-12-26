using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.BLLJIMP
{
    public class BLLSocketLog:BLL
    {
        /// <summary>
        /// 更新用户在线时长
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateUserOnlineTimes(int id)
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.AppendFormat(" SELECT SUM([Minutes]) FROM [ZCJ_SocketLog] WHERE [UserAutoID]={0}",id);
            DataSet ds = BLLUser.Query(sbHtml.ToString());
            int onlineTimes = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            return Update(new UserInfo(), string.Format("OnlineTimes={0}", onlineTimes), string.Format("AutoID={0}", id)) > 0;
        }
    }
}
