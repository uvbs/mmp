using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;
namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.WebPC
{
    /// <summary>
    /// 删除
    /// </summary>
    public class Delete : BaseHandlerNeedLoginAdminNoAction
    {

        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {

           
            if (bll.Delete(new PcPage(),string.Format(" PageId in({0})",context.Request["ids"]))>0)
            {
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "删除失败";
            }
            bll.ContextResponse(context, apiResp);


        }
    }
}