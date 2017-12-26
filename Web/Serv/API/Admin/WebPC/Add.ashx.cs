using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Admin.WebPC
{
    /// <summary>
    /// 添加
    /// </summary>
    public class Add : BaseHandlerNeedLoginAdminNoAction
    {
        BLLJIMP.BLL bll = new BLLJIMP.BLL();
        public void ProcessRequest(HttpContext context)
        {

            var requestModel = ZentCloud.Common.JSONHelper.JsonToModel<ZentCloud.BLLJIMP.ModelGen.PcPage.PcPage>(context.Request["jsonData"]);
            PcPage model = new PcPage();
            model.PageName = requestModel.PageName;//页面名称
            model.TopContent = requestModel.TopContent;//顶部内容
            model.Logo = requestModel.Logo;//Logo
            model.BottomContent = requestModel.BottomContent;//底部内容
            model.TopMenu = requestModel.TopMenu;//顶部菜单
            model.MiddContent = ZentCloud.Common.JSONHelper.ObjectToJson(requestModel.MiddList);//中部列表
            model.WebsiteOwner = bll.WebsiteOwner;//所有者
            model.PageId = int.Parse(bll.GetGUID(BLLJIMP.TransacType.CommAdd));
            if (bll.Add(model))
            {
                apiResp.status = true;
            }
            else
            {
                apiResp.msg = "添加失败";
            }
            bll.ContextResponse(context, apiResp);


        }


    }
}