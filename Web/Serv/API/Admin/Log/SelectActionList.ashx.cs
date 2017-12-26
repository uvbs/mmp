using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.Log
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public class SelectActionList : BaseHandlerNeedLoginAdminNoAction
    {
        /// <summary>
        /// 用户BLL
        /// </summary>
        BLLUser bllUser = new BLLUser();
        public void ProcessRequest(HttpContext context)
        {
            string showAll = context.Request["show_all"];
            List<dynamic> resultList = new List<dynamic>();
            if (showAll == "1")
            {
                resultList.Add(new { value = "", text = "所有行为" });
            }
            resultList.Add(new { value = "SignIn", text = "登录" });
            resultList.Add(new { value = "Add", text = "添加" });
            resultList.Add(new { value = "Update", text = "修改" });
            resultList.Add(new { value = "Delete", text = "删除" });
            resultList.Add(new { value = "Export", text = "导出" });

            apiResp.result = resultList;
            apiResp.status = true;
            apiResp.code = (int)APIErrCode.IsSuccess;
            apiResp.msg = "查询完成";
            bllUser.ContextResponse(context, apiResp);
        }
    }
}