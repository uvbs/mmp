using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.ZentCloudOpen.Store
{
    /// <summary>
    /// 删除门店
    /// </summary>
    public class Delete : BaseHanderOpen
    {

        /// <summary>
        /// 
        /// </summary>
        BLLJIMP.BLLUser bllUser = new BLLJIMP.BLLUser();
        public void ProcessRequest(HttpContext context)
        {

            try
            {

                string storeIds=context.Request["store_ids"];
                if (string.IsNullOrEmpty(storeIds))
                {
                    resp.code = (int)APIErrCode.PrimaryKeyIncomplete;
                    resp.msg = "store_ids 参数必传";
                    bllUser.ContextResponse(context, resp);
                    return;
                }
               
                if (bllUser.Update(new UserInfo(), string.Format(" UserType=0"), string.Format(" AutoId in({0})",storeIds))>0)
                {
                    bllUser.Update(new JuActivityInfo(), string.Format(" IsDelete=1"), string.Format(" WebsiteOwner='{0}' And ArticleType='Outlets' And K5 in({1})", bllUser.WebsiteOwner, storeIds));
                    resp.status = true;
                    resp.msg = "ok";
                }
                else
                {
                    resp.msg = "删除失败";
                }
                
            }
            catch (Exception ex)
            {

                resp.msg = ex.Message;
                resp.result = ex.ToString();

            }
            bllUser.ContextResponse(context, resp);
        }

    }
}