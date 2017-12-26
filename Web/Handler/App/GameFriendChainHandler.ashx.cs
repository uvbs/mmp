using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
using ZentCloud.BLLJIMP;

namespace ZentCloud.JubitIMP.Web.Handler.App
{
    /// <summary>
    /// Summary description for GameFriendChainHandler
    /// </summary>
    public class GameFriendChainHandler : BaseHandler
    {
        /// <summary>
        /// hrlove
        /// </summary>
        BLLGameFriendChain bllFriendChain = new BLLGameFriendChain();
        /// <summary>
        /// //获取Friend 随机列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetRandomFriendList(HttpContext context)
        {

                int count = int.Parse(context.Request["count"]);
                List<GameFriendChain> data =bllFriendChain.GetRandomFriendList(count);
                if (data != null && data.Count > 0)
                {
                    resp.Status = 0;
                    resp.ExObj = data;
                }
                else
                {
                    resp.Status = -1;
                    resp.Msg = "没有数据";
                }

            return Common.JSONHelper.ObjectToJson(resp);
        }



        /// <summary>
        /// hrlove 查询
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string QueryFriendList(HttpContext context) {
            int pageIndex = int.Parse(context.Request["PageIndex"]);
            int pageSize = int.Parse(context.Request["PageSize"]);
            string title = context.Request["Title"];
            System.Text.StringBuilder sbWhere = new System.Text.StringBuilder("1=1");
            if (!string.IsNullOrEmpty(title))
            {
                sbWhere.AppendFormat("And Name like '%{0}%'",title);
            }
           
            resp.ExObj = bllFriendChain.GetLit<GameFriendChain>(pageSize,pageIndex,sbWhere.ToString(),"AutoID ASC");

            return Common.JSONHelper.ObjectToJson(resp);
        
        }




    }
}