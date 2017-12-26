<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WapMainV1.Master" AutoEventWireup="true" CodeBehind="SubmitInfoStatus.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Customize.kuanqiao.SubmitInfoStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>

.box {
    width: 100%;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<section class="box">
<%--    <div class="header">
        <img src="/img/offline_user.png" alt="">
        <h2></h2>
        <p></p>
        <a href="#" class="btn">更新头像</a>
        <div class="line"></div>
    </div>--%>
    <div class="prompt">
        <p>核名信息状态</p>
    </div>
    <ul class="course">

        <%if (list.Count>0)
          { %>
 
         <%for(int i = 0; i < list.Count; i++)
           {
               
               var appstatus = list[i].K15;
               if (appstatus.Equals("待处理"))
               {
                   System.Text.StringBuilder sb=new StringBuilder();
                   sb.AppendLine("<li class=\"courseli coursecenter\">");
                   sb.AppendLine("<div class=\"coursebox\">");
                   sb.AppendLine("<h3>等待审核</h3>");
                   sb.AppendFormat("<p>申请企业名称：{0}</p>", list[i].K2);
                   sb.AppendLine("</div>");
                   sb.AppendLine("</li>");
                   Response.Write(sb.ToString());
                  
                   
                  
             
               }
               if (appstatus.Equals("正在处理"))
               {
                   System.Text.StringBuilder sb = new StringBuilder();

                   sb.AppendLine("<li class=\"courseli coursecenter\">");
                   sb.AppendLine("<div class=\"coursebox\">");
                   sb.AppendLine("<h3>正在审核</h3>");
                   sb.AppendFormat("<p>申请企业名称：{0}</p>", list[i].K2);
                   sb.AppendLine("</div>");
                   sb.AppendLine("</li>");
                   Response.Write(sb.ToString());
                  

               }
               if (appstatus.Equals("审核通过"))
               {
                   System.Text.StringBuilder sb = new StringBuilder();
                   sb.AppendLine("<li class=\"courseli coursecenter\">");
                   sb.AppendLine("<div class=\"coursebox\">");
                   sb.AppendLine("<h3>审核通过</h3>");
                   sb.AppendFormat("<p>申请企业名称：{0}</p>", list[i].K2);
                   sb.AppendLine("</div>");
                   sb.AppendLine("</li>");
                   Response.Write(sb.ToString());
                 


               }
               if (appstatus.Equals("审核失败"))
               {
                   System.Text.StringBuilder sb = new StringBuilder();
                   sb.AppendLine("<li class=\"courseli coursecenter\">");
                   sb.AppendLine("<div class=\"coursebox wrong\">");
                   sb.AppendLine("<h3>审核失败</h3>");
                   sb.AppendFormat("<p>申请企业名称：{0}</p>", list[i].K2);
                   sb.AppendFormat("<p>原因:{0}</p>", list[i].K17);
                   sb.AppendLine("</div>");
                   sb.AppendLine("</li>");
                   Response.Write(sb.ToString());


               }

               if (i<list.Count-1)
               {
                   Response.Write("<hr style=\"margin-bottom:10px;\"/>");
               }
              
               
               
           } %>

          <%} %>  
          <%else{%> 

         <li  class="courseli coursecenter">
            <div class="coursebox">
                <h3>您还没申请过企业核名</h3>
                <p></p>
            </div>
           
         </li>

          <% } %> 


    </ul>
</section>
</asp:Content>
