using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZentCloud.BLLJIMP;
using ZentCloud.BLLJIMP.Enums;
using ZentCloud.BLLJIMP.Model;

namespace ZentCloud.JubitIMP.Web.Serv.API.Common
{
    /// <summary>
    /// List 的摘要说明
    /// </summary>
    public class ToolBarList : BaseHandlerNoAction
    {
        BLLCompanyWebSite bll = new BLLCompanyWebSite();
        public void ProcessRequest(HttpContext context)
        {
            UserInfo curUser = bll.GetCurrentUserInfo();
            List<CompanyWebsite_ToolBar> dataList = bll.GetCommonToolBarList(bll.WebsiteOwner, context.Request["key_type"]);

            List<dynamic> foottool_list = new List<dynamic>();
            foreach (CompanyWebsite_ToolBar p in dataList)
            {
                if (context.Request["show_all"] != "1" && !bll.CheckHasPms(curUser,p)) continue;

                foottool_list.Add(new
                {
                    title = p.ToolBarName,
                    ico = p.ToolBarImage,
                    img = p.ImageUrl,
                    url = p.ToolBarTypeValue,
                    type = p.ToolBarType,
                    active_bg_color = p.ActBgColor,
                    bg_color = p.BgColor,
                    active_color = p.ActColor,
                    color = p.Color,
                    ico_color = p.IcoColor,
                    active_bg_img = p.ActBgImage,
                    bg_img = p.BgImage,
                    ico_position = p.IcoPosition,
                    visible_set = p.VisibleSet,
                    permission_group = p.PermissionGroup,
                    right_text = p.RightText
                });
            }

            apiResp.result = foottool_list;
            apiResp.status = true;
            apiResp.msg = "查询完成";
            apiResp.code = (int)APIErrCode.IsSuccess;
            bll.ContextResponse(context, apiResp);
        }
    }
}