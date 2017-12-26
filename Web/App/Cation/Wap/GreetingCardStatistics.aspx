<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapUser.Master" AutoEventWireup="true" CodeBehind="GreetingCardStatistics.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Cation.Wap.GreetingCardStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div data-role="page" id="page-title" data-theme="b">

        <div data-role="header" data-theme="b" data-position="fixed" style="" id="divTop">
            <a href="/App/Cation/Wap/MyGreetingCard.aspx" data-role="button"  data-icon="arrow-l" data-ajax="false">返回</a>
            <h1>
                <%Response.Write(juActivityModel.ActivityName); %>
                
            </h1>
            
            <div data-role="navbar">
        <ul>
            <li><a href="#" id="atitle"  >(<label class="lblsigncount"><%=signUpDataList.Count.ToString() %></label>人)回复<img style="vertical-align:middle;" src="/img/reload.png" width="18" height="18" /></a></li>
            
        </ul>
    </div>
        </div>
        <div data-role="content">

         
            
            <%
                StringBuilder sb = new StringBuilder();
                foreach (var item in signUpDataList)
                {
                    sb.AppendFormat("<div style=\"border: 1px solid #CCC;margin-top:10px;\" onmouseover=\"currentcolor=this.style.backgroundColor;this.style.backgroundColor='#FFF4C1';this.style.cursor= 'hand ';\" onmouseout=\"this.style.backgroundColor=currentcolor\">");
                    sb.AppendLine("  <div style=\"font-family: Helvetica,Arial,sans-serif;text-align: left;font-weight: bold;background-color: #E7E7E7;padding: 5px;font-size: 16px;color: #930;\">");

                    sb.AppendFormat("<table><tr><td><img src=\"{0}\" width=\"50\" height=\"50\"/></td><td valign=\"center\">{1} {2}</td></tr></table>", item.K2, item.Name, string.Format("{0:f}", item.InsertDate));

                    sb.AppendLine("</div>");

                    sb.AppendLine("<div style=\"font-family: Helvetica,Arial,sans-serif;margin-left:5px;margin-top:10px;font-size: 16px;color: #666666;line-height: 18px;\">");

                    sb.Append(item.K4);

                    sb.AppendLine("</div>");
                }
                Response.Write(sb.ToString());
                
             %>



        



        <script type="text/javascript">
            $(function () {
                $("#atitle").bind("click", function (event, ui) {
                    $.mobile.loading('show', { textVisible: true, text: '正在刷新...' });
                    window.location = "/App/Cation/Wap/GreetingCardStatistics.aspx?jid=" + "<%=pubjid%>";

                });


            })
        </script>
        

        </div>
    </div>
</asp:Content>
