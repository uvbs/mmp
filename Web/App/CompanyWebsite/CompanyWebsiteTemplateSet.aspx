<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="CompanyWebsiteTemplateSet.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.CompanyWebsite.CompanyWebsiteTemplateSet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
input 
{
    height: 35px;
    border: 1px solid #d5d5d5;
    border-radius: 5px;
    background-color: #fefefe;
   
}
textarea
{
    
    height: 35px;
    border: 1px solid #d5d5d5;
    border-radius: 5px;
    background-color: #fefefe;

}

</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微网站&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span>微网站模板</span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">

    <div class="ActivityBox" style="margin-bottom:80px;">
        

        <%foreach (var item in TemplateList)
          {
              if (item.TemplatePath.Equals(templatename))
              {
                  Response.Write("<div style=\"float:left;margin-left:20px;margin-bottom:50px;text-align:center;border:1px solid;border-radius:5px;border-color:#CCCCCC;\">");
                  Response.Write(string.Format("<img style=\"width:200px;height:280px;\" src=\"{0}\">", item.TemplateThumbnail));

                  Response.Write("<div style=\"margin-top:10px;margin-bottom:10px;font-weight:bold;font-family: 微软雅黑;\">");
                 
                  Response.Write(item.TemplateName);
                 
                  Response.Write("</div>");

                  Response.Write("<div style=\"margin-bottom:10px;\">");
                  Response.Write("<a href=\"/web/index.aspx\" target=\"_blank\" class=\"button button-action\" >正在使用</a>");
                  Response.Write("</div>");
                  Response.Write("</div>");
              }
              else
              {
                  Response.Write("<div style=\"float:left;margin-left:20px;margin-bottom:50px;text-align:center;border:1px solid;border-radius:5px;border-color:#CCCCCC;\">");
                  Response.Write(string.Format("<img style=\"width:200px;height:280px;\" src=\"{0}\">", item.TemplateThumbnail));
                  Response.Write("<div style=\"margin-top:10px;margin-bottom:10px;font-weight:bold;font-weight:bold;font-family: 微软雅黑;\">");
                  Response.Write(item.TemplateName);
                  Response.Write("</div>");
                  Response.Write("<div style=\"margin-bottom:10px;\">");
                  Response.Write(string.Format("<a href=\"javascript:void(0)\" data-template=\"{0}\" class=\"button button-rounded button-caution\" >使用该模板</a>",item.TemplatePath));
                  Response.Write("</div>");
                  Response.Write("</div>");
              }
              
              
              

        
              
              
          } %>
        
      
        


    </div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
        var handlerUrl = "/Handler/App/CationHandler.ashx";
        $(function () {

            $("[data-template]").click(function () {
                var dataModel = {
                    Action: 'UpdateCompanyWebSiteTemplate',
                    CompanyWebSiteTemplateName: $(this).attr("data-template")
                }
                $.messager.progress({ text: '正在应用模板...' });
                $.ajax({
                    type: 'post',
                    url: handlerUrl,
                    data: dataModel,
                    dataType:"json",
                    success: function (resp) {
                        $.messager.progress('close');
                        if (resp.Status == 1) {
                            window.location.href = "CompanyWebsiteTemplateSet.aspx";
                        }
                        else {
                            Alert(resp.Msg);
                        }

                    }
                });



            })


        })


    </script>
</asp:Content>