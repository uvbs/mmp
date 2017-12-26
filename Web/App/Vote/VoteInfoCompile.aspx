<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="VoteInfoCompile.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Vote.VoteInfoCompile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link  href="/MainStyleV2/css/bootstrap.min.css" type="text/css"/>
    <style type="text/css">
        .tdTitle
        {
            font-weight: bold;
        }
        table td
        {
            height: 30px;
        }
       input[type=text],select,textarea
        {
            height: 30px;
            border: 1px solid #d5d5d5;
            border-radius: 5px;
            background-color: #fefefe;
             
        }
         .datebox-calendar-inner
         {
             
             height:195px;
          }
         .sp-replacer{
             width:20%;
         }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;<a href="/App/Vote/VoteInfoMgr.aspx" >投票活动管理</a>&nbsp;&nbsp;&gt;&nbsp;&nbsp;<span><%=actionStr %>投票活动<%if (model != null && webAction == "edit") { Response.Write("：" + model.VoteName); } %></span>
    <a href="VoteInfoMgr.aspx" style="float:right;margin-right:20px;color:Black;" title="返回列表" class="easyui-linkbutton" iconcls="icon-back" plain="true" >
            返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <fieldset>
                <legend>基本配置</legend>
            <table width="100%">
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">投票活动名称：</td>
                    <td width="*" align="left"><input type="text" id="txtVoteName" class="" style="width: 100%;" value="<%=model.VoteName%>" /></td>
                </tr>
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">描述：</td>
                    <td width="*" align="left"><input id="txtSummary" type="text" style="width:100%;" value="<%=model.Summary %>"/></td>
                </tr>
                
                 <tr style="display:none;">
                    <td style="width: 120px;" align="right" class="tdTitle" >
                        投票活动类型：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlVoteType">
                        <option value="0">图片投票活动</option>
                        <option value="1">视频投票活动</option>
                        <option value="2">投票后抽奖</option>
                        </select>
                    </td>
                </tr>

                   <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        投票活动状态：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdstatus" id="rdstatus0" value="0" /><label for="rdstatus0">停止投票</label>
                        <input type="radio" name="rdstatus" id="rdstatus1" value="1" checked="checked"/><label for="rdstatus1">列表模式</label>
                        <input type="radio" name="rdstatus" id="rdstatus2" value="2"/><label for="rdstatus2">展示模式</label>
                        <input type="radio" name="rdstatus" id="rdstatus3" value="3"/><label for="rdstatus3">PK模式</label>
                    </td>
                </tr>
                    <tr style="display:none;">
                    <td style="width: 120px;" align="right" class="tdTitle" >
                        是否收费：
                    </td>
                    <td width="*" align="left">
                        <input type="radio" name="rdisfree" id="rdisfree1" value="1" checked="checked" /><label for="rdisfree1">免费</label>
                        <input type="radio" name="rdisfree" id="rdisfree0" value="0" /><label for="rdisfree0">收费</label>
                    </td>
                </tr>
                 <tr >
                    <td style="width: 120px;" align="right" class="tdTitle">
                        投票限制：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlLimitType">
                        <option value="0">每人最多投多少票</option>
                        <option value="1">每人每天可以投多少票</option>
                        </select>
                    </td>
                </tr>

                  <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                         可投总票数：
                    </td>
                    <td width="*" align="left">
                    <input id="txtFreeVoteCount" type="text" style="width:100px;" value="<%=model.FreeVoteCount%>"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                         每个选手限制票数：
                    </td>
                    <td width="*" align="left">
                    <input id="txtVoteObjectLimitVoteCount" type="text" style="width:100px;" value="<%=model.VoteObjectLimitVoteCount%>"/>
                    </td>
                </tr>

                   <tr style="display:none;">
                    <td style="width: 120px;" align="right" class="tdTitle">
                        票数更新：
                    </td>
                    <td width="*" align="left">
                        <select id="ddlVoteCountAutoUpdate">
                        <option value="0">不更新</option>
                        <option value="1">每天更新</option>
                        </select>
                    </td>
                </tr>
                 <tr id="trOfflinePayUrl" style="display:none;">
                    <td style="width: 120px;" align="right" class="tdTitle">
                         线下支付链接：
                    </td>
                    <td width="*" align="left">
                    <input id="txtOfflinePayUrl" type="text" style="width:100%;" value="<%=model.OfflinePayUrl%>"/>
                    </td>
                </tr>
               <tr id="trIntroduction" style="display:none;">
                    <td style="width: 120px;" align="right" class="tdTitle">
                         支付页介绍：
                    </td>
                    <td width="*" align="left">
                    <textarea id="txtIntroduction" style="width:100%;height:200px;"><%=model==null?"":model.Introduction%></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        Logo图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="" width="300px" height="150px" id="imglogo" />
                        <a id="a1"href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="txtThumbnailsPathLogo.click()">上传Logo</a>
                        <input type="file" id="txtThumbnailsPathLogo" name="filelogo"  style="display:none;"/>
                        提示:logo 最佳图片大小为 300*150
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        底部内容：
                    </td>
                    <td width="*" align="left">
                       <textarea id="txtBottomContent" style="width:100%;height:200px;"><%=model.BottomContent%></textarea>
                    </td>
                </tr>
                <tr >
                    <td style="width: 120px;" align="right" class="tdTitle">
                        投票活动截止日期：
                    </td>
                    <td width="*" align="left">
                    <input id="txtStopDate" type="text" style="width:150px;height:30px;" value="" class="easyui-datetimebox"/>
                    </td>
                </tr>

                 <tr >
                    <td style="width: 120px;" align="right" class="tdTitle">
                        奖品名称：
                    </td>
                    <td width="*" align="left">
                    <input id="txtPrize" type="text"  value="<%=model.Prize%>" />
                    </td>
                </tr>
                <tr >
                    <td style="width: 120px;" align="right" class="tdTitle">
                        奖品类型：
                    </td>
                    <td width="*" align="left">
                   <select id="ddlPrizeType">
                   <option value="0">积分</option>
                   <option value="1">优惠券</option>
                   </select>
                    </td>
                </tr>
               <tr >
                    <td style="width: 120px;" align="right" class="tdTitle">
                        第一名获得的积分或优惠券编号：
                    </td>
                    <td width="*" align="left">
                    <input id="txtEx2" type="text"  value="<%=model.Ex2%>" />
                    </td>
                </tr>

               <tr >
                    <td style="width: 120px;" align="right" class="tdTitle">
                        使用积分：
                    </td>
                    <td width="*" align="left">
                    <input id="txtUseScore" type="text"  value="<%=model.UseScore%>" />
                    </td>
                </tr>
            </table>
            </fieldset>

            <fieldset>
                <legend>展示配置</legend>
            <table width="100%">
                
                 <tr>
                     <td style="width: 120px;" align="right" class="tdTitle">主题色：</td>
                     <td width="*" align="left"><input type="text" id="txtThemeColor"
                          class="color"  width="100%" ></td>
                </tr>
                 <tr>
                     <td style="width: 120px;" align="right" class="tdTitle">主题字色：</td>
                     <td width="*" align="left"><input type="text" id="txtThemeFontColor"
                          class="color"  width="100%" ></td>
                </tr>
                
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">Banner高度：</td>
                    <td width="*" align="left"><input id="txtBannerHeight" type="text" value="<%=model.BannerHeight %>" style="width:100%;"/></td>
                </tr>
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">手持字样：</td>
                    <td width="*" align="left"><input id="txtHandheldWords" value="<%=model.HandheldWords %>" type="text" style="width:100%;"/></td>
                </tr>
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">分享标题：</td>
                    <td width="*" align="left"> <input id="txtShareTitle" value="<%=model.ShareTitle %>" type="text" style="width:100%;"/></td>
                </tr>

                 <tr>
                     <td style="width: 120px;" align="right" class="tdTitle">其他资料链接展示文本：</td>
                     <td width="*" align="left">
                         <input type="text" style="width:100%" value="<%=model.OtherInfoLinkText %>" id="txtVoteObjectVideoLinkText"/>
                     </td>
                </tr>
                <%-- <tr>
                     <td style="width: 120px;" align="right" class="tdTitle">链接：</td>
                     <td width="*" align="left">
                         <input type="text" style="width:100%" id="txtMyVideoLink"/>
                     </td>
                </tr>--%>
                <tr>
                     <td style="width: 120px;" align="right" class="tdTitle">投票页面的背景色：</td>
                     <td width="*" align="left"><input type="text" id="txtVotePageBgColor"
                          class="color"  width="100%" ></td>
                </tr>
                <tr>
                     <td style="width: 120px;" align="right" class="tdTitle">首页底部菜单：</td>
                     <td width="*" align="left">
                          <input type="radio" id="menu0" name="menu" value="0" checked="checked"/><label for="menu0">显示</label>
                         <input type="radio" id="menu1" name="menu" value="1" /><label for="menu1">隐藏</label>
                        
                     </td>
                </tr>
                
                <tr>
                     <td style="width: 120px;" align="right" class="tdTitle">底部导航按钮组：</td>
                     <td width="*" align="left">
                         <select style="width:162px;" id="FooterMenuGroup">
                         </select>
                     </td>
                </tr>
                 <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        背景音乐：
                    </td>
                     <td width="*" align="left">
                        <input type="file" id="bgMusicThumbnailsPath" name="bgMusic" value="" />
                        <input type="text" width="100%" id="bgMusic" name="name" value="<%=model.BgMusic %>" />
                    </td>
                </tr>
                
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                         参赛宣言别名：
                    </td>
                    <td width="*" align="left">
                       <textarea id='txtSignUpDeclarationRename' rows="5" cols="65" style="width:50%;height:150px;resize:none;"><%=model.SignUpDeclarationRename %></textarea>
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                         参赛宣言说明：
                    </td>
                    <td width="*" align="left">
                       <textarea id='txtSignUpDeclarationDescription' rows="5" cols="65" style="width:50%;height:150px;resize:none;"><%=model.SignUpDeclarationDescription %></textarea>
                    </td>
                </tr>
                

               <tr>
                    <td style="width: 120px;" align="right" class="tdTitle"> 缩略图：</td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" width="100px" height="130px" id="imgThumbnailsPath" /><br />
                         <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传图片</a><br />
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                     </td>
                     
                </tr>
               <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        首页背景图：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg"  width="160px" height="252px" id="indexBgThumbnailsPath" /><br />
                         <a id="indexBgThumbnailsPaths"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtIndexBgPath.click()">上传图片[图片最佳显示效果大小为640*1008]</a><br />
                        <br />
                        <input type="file" id="txtIndexBgPath" name="file1" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        背景Banner图：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" width="320px" height="160px" id="bannerBgThumbnailsPath" /><br />
                         <a id="bannerBgThumbnailsPaths"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtBannerBgPath.click()">上传图片[图片最佳显示效果大小为640*320]</a><br />
                        
                        <br />
                        <input type="file" id="txtBannerBgPath" name="file1" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        手持图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" width="100px" height="130px" id="handheldImgThumbnailsPath" /><br />
                         <a id="handheldImgThumbnailsPaths"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtHandheldImgPath.click()">上传图片</a><br />
                        <br />
                        <input type="file" id="txtHandheldImgPath" name="file1" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        合伙人图片：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" width="160px" height="252px" id="partnerImgThumbnailsPath" /><br />
                         <a id="partnerImgThumbnailsPaths"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtPartnerImgPath.click()">上传图片[图片最佳显示效果大小为640*1008]</a><br />
                        
                        <br />
                        <input type="file" id="txtPartnerImgPath" name="file1" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        投票对象详情Banner图：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" width="160px" height="252px" id="voteObjDetailBannerImgThumbnailsPath" /><br />
                         <a id="voteObjDetailBannerImgThumbnailsPaths"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtVoteObjDetailBannerImgPath.click()">上传图片[图片最佳显示效果大小为640*1008]</a><br />
                        
                        <br />
                        <input type="file" id="txtVoteObjDetailBannerImgPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        投票列表详情Banner图：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" width="160px" height="252px" id="voteObjListBannerImgThumbnailsPath" /><br />
                         <a id="voteObjListBannerImgThumbnailsPaths"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtVoteObjListBannerImgPath.click()">上传图片[图片最佳显示效果大小为640*1008]</a><br />
                        
                        <br />
                        <input type="file" id="txtVoteObjListBannerImgPath" name="file1" />
                    </td>
                </tr>
                  <tr>
                    <td style="width: 120px;" align="right" class="tdTitle">
                        活动未开始海报：
                    </td>
                    <td width="*" align="left">
                        <img alt="图片" src="/img/hb/hb1.jpg" width="160px" height="252px" id="notStartPosterImgThumbnailsPath" /><br />
                         <a id="notStartPosterImgThumbnailsPaths"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtNotStartPosterImgPath.click()">上传图片[图片最佳显示效果大小为640*1008]</a>
                        <br />
                        <br />
                        <input type="file" id="txtNotStartPosterImgPath" name="file1" />
                    </td>
                </tr>
                 <tr>
                     <td style="width: 120px;" align="right" class="tdTitle">首页页面配置：</td>
                     <td width="*" align="left">
                         <textarea id="txtIndexPageHtml"   style="width:100%;height:400px;"><%=model.IndexPageHtml %></textarea>
                     </td>
                </tr>
                
                  <tr id="RulePageHtml">
                    <td style="width: 120px;" align="right" class="tdTitle">
                         规则页：
                    </td>
                    <td width="*" align="left">
                    <textarea id="txtRulePageHtml"  style="width:100%;height:400px;"><%=model==null?"":model.RulePageHtml%></textarea>
                    </td>
                </tr>
               
            </table>
            </fieldset>





            <div style="border-top: 1px solid #DDDDDD; position: fixed; bottom: 0px; height: 45px; line-height: 55px; text-align: center; width: 100%; background-color: rgb(245, 245, 245); padding-top:10px;">
                             <a href="javascript:;" id="btnSave" style="font-weight: bold;width:200px;" class="button button-rounded button-primary">
                            保存</a>
                            </div>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/CationHandler.ashx";
     var currAction = '<%=webAction %>';
     var groups =<% = groups%>;
     var currId = '<%=model.AutoID %>';
     var editor;
     var rulepage;
     var editorbottom;
     var indexpagehtml;
     $(function () {
         if ($.browser.msie) { //ie 下
             //缩略图
             $("#auploadThumbnails").hide();
             $("#txtThumbnailsPath").after($("#aRandomThumbnail"));
         }
         else {
             $("#txtThumbnailsPath").hide(); //缩略图
             $("#txtIndexBgPath").hide();
             $("#txtBannerBgPath").hide();
             $("#txtHandheldImgPath").hide();
             $("#txtPartnerImgPath").hide();
             $("#txtVoteObjDetailBannerImgPath").hide();
             $("#txtVoteObjListBannerImgPath").hide();
             $("#txtNotStartPosterImgPath").hide();
             $("#txtBgMusicPath").hide();
         }
         if (currAction == 'edit') {
             $('#imgThumbnailsPath').attr("src", "<%=model.VoteImage%>");
             $('#indexBgThumbnailsPath').attr("src", "<%=model.IndexBg%>");
             $("#bannerBgThumbnailsPath").attr("src", "<%=model.BannerBg%>");
             $("#handheldImgThumbnailsPath").attr("src", "<%=model.HandheldImg%>");
             $("#partnerImgThumbnailsPath").attr("src", "<%=model.PartnerImg%>");
             $("#voteObjDetailBannerImgThumbnailsPath").attr("src", "<%=model.VoteObjDetailBannerImg%>");
             $("#voteObjListBannerImgThumbnailsPath").attr("src", "<%=model.VoteObjListBannerImg%>");
             $("#notStartPosterImgThumbnailsPath").attr("src", "<%=model.NotStartPoster%>");
             $('#imglogo').attr("src", "<%=model.Logo%>");
             $(ddlVoteType).val("<%=model.VoteType%>");
             $(ddlVoteCountAutoUpdate).val("<%=model.VoteCountAutoUpdate%>");
             $(ddlPrizeType).val("<%=model.Ex1%>");
             $(ddlLimitType).val("<%=model.LimitType%>");
             var status = "<%=model.VoteStatus%>";
             var color="<%=model.VotePageBgColor%>";
             if(color!=null&&color!=''){
                 color.substr(0,1);
             }
             var themeColor="<%=model.ThemeColor%>";
             //if(themeColor!=null&&themeColor!=''){
             //    themeColor.substr(0,1);
             //}
             var themeFontColor="<%=model.ThemeFontColor%>";
             //if(themeFontColor!=null&&themeFontColor!=''){
             //    themeFontColor.substr(0,1);
             //}

             $("#txtVotePageBgColor").val(color);

             $("#txtThemeColor").val(themeColor);
             $("#txtThemeFontColor").val(themeFontColor);

             if (status == "0") {
                 rdstatus0.checked = true;
             }else if(status=="1"){
                 rdstatus1.checked = true;
             }else if(status=="2"){
                 rdstatus2.checked = true;
             }
             else if(status=="3"){
                 rdstatus3.checked = true;
             }

             var isfree = "<%=model.IsFree%>";
             if (isfree == "0") {
                 rdisfree0.checked = true;
             }
             else {
                 rdisfree1.checked = true;
             }
             ShowVoteIsFree(isfree);

             var isHideIndexFooterMenu="<%=model.IsHideIndexFooterMenu%>";
             if(isHideIndexFooterMenu=="0"){
                 menu0.checked=true;
             }else{
                 menu1.checked=true;
             }
             $('#txtStopDate').datetimebox('setValue', '<%=model.StopDate%>');
         }
         $('.color').spectrum();
         $('#btnSave').click(function () {
             try {
                 var model =
                    {
                        Action: currAction == 'add' ? 'AddVoteInfo' : 'EditVoteInfo',
                        AutoID: currId,
                        VoteName: $.trim($('#txtVoteName').val()),
                        Summary: $.trim($('#txtSummary').val()),
                        VoteStatus: $("input[name=rdstatus]:checked").val(),
                        IsFree: $("input[name=rdisfree]:checked").val(),
                        FreeVoteCount: $.trim($('#txtFreeVoteCount').val()),
                        OfflinePayUrl: $.trim($('#txtOfflinePayUrl').val()),
                        Introduction: editor.html(),
                        VoteType: $("#ddlVoteType").val(),
                        VoteCountAutoUpdate: $("#ddlVoteCountAutoUpdate").val(),
                        BottomContent: editorbottom.html(),
                        StopDate: $('#txtStopDate').datetimebox('getValue'),
                        Prize: $(txtPrize).val(),
                        UseScore: $(txtUseScore).val(),
                        Ex1: $(ddlPrizeType).val(),
                        Ex2: $(txtEx2).val(),
                        //缩略图
                        VoteImage: $('#imgThumbnailsPath').attr('src'),
                        //logo
                        Logo: $("#imglogo").attr("src"),
                        //首页背景图
                        IndexBg: $('#indexBgThumbnailsPath').attr('src'),
                        // 背景Banner图
                        BannerBg:$("#bannerBgThumbnailsPath").attr('src'),
                        //banner 高度
                        BannerHeight: $.trim($("#txtBannerHeight").val()),
                        //分享标题
                        ShareTitle: $.trim($("#txtShareTitle").val()),
                        //别名
                        SignUpDeclarationRename: $.trim($("#txtSignUpDeclarationRename").val()),
                        //描述
                        SignUpDeclarationDescription: $.trim($("#txtSignUpDeclarationDescription").val()),
                        //手持字样
                        HandheldWords: $.trim($("#txtHandheldWords").val()),
                        //手持图片
                        HandheldImg: $('#handheldImgThumbnailsPath').attr('src'),
                        //合伙人图片
                        PartnerImg: $('#partnerImgThumbnailsPath').attr('src'),
                        //投票列表详情banner图
                        VoteObjDetailBannerImg: $('#voteObjDetailBannerImgThumbnailsPath').attr('src'),
                        //投票列表详情banner图
                        VoteObjListBannerImg: $('#voteObjListBannerImgThumbnailsPath').attr('src'),
                        // 活动未开始海报
                        NotStartPoster: $('#notStartPosterImgThumbnailsPath').attr('src'),
                        //规则也
                        RulePageHtml: rulepage.html(),
                        //背景音乐
                        BgMusic: $('#bgMusic').val(),


                        //投票页面背景色
                        VotePageBgColor:$.trim('#'+$("#txtVotePageBgColor").val()),

                        ThemeColor:$.trim('#'+$("#txtThemeColor").val()),
                        ThemeFontColor:$.trim('#'+$("#txtThemeFontColor").val()),

                        //首页底部菜单是否可以隐藏：1是，0否
                        IsHideIndexFooterMenu:$("input[name=menu]:checked").val(),
                        //首页
                        IndexPageHtml:indexpagehtml.html(),
                        //底部导航按钮组，默认为空，可以选择导航里面其中一组
                        FooterMenuGroup:$("#FooterMenuGroup").val(),
                        //投票参与者其他资料链接展示文本
                        VoteObjectVideoLinkText:$.trim($("#txtVoteObjectVideoLinkText").val()),
                        LimitType:$("#ddlLimitType").val(),
                        VoteObjectLimitVoteCount:$("#txtVoteObjectLimitVoteCount").val()

                    }


             



                 if (model.VoteName == '') {
                     $('#txtVoteName').focus();

                     return

                 }
                 if (model.StopDate == "") {
                     Alert("截止日期必填");
                     return false;
                 }


                 $.messager.progress({ text: '正在处理...' });
                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: model,
                     dataType: "json",
                     success: function (resp) {
                         $.messager.progress('close');
                         if (resp.Status == 1) {

                             if (currAction == 'add')
                                 ResetCurr();
                             Alert(resp.Msg);
                         }
                         else {
                             Alert(resp.Msg);
                         }
                     }
                 });

             } catch (e) {
                 Alert(e);
             }


         });

         loadGroups();

         if(currAction == 'edit'){
             var groups="<%=model.FooterMenuGroup%>";
             $("#FooterMenuGroup").val(groups);
         }

         //缩略图
         $("#txtThumbnailsPath").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Vote',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {

                                 $('#imgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });


         

         //logo图
         $("#txtThumbnailsPathLogo").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });
                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Vote&filegroup=filelogo',
                         secureuri: false,
                         fileElementId: 'txtThumbnailsPathLogo',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#imglogo').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });

         //首页背景图
         $("#txtIndexBgPath").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Vote',
                         secureuri: false,
                         fileElementId: 'txtIndexBgPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#indexBgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });
         //背景Banner图
         $("#txtBannerBgPath").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Vote',
                         secureuri: false,
                         fileElementId: 'txtBannerBgPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#bannerBgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });

         // //手持图片
         $("#txtHandheldImgPath").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Vote',
                         secureuri: false,
                         fileElementId: 'txtHandheldImgPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#handheldImgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });

         //合伙人图片
         $("#txtPartnerImgPath").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Vote',
                         secureuri: false,
                         fileElementId: 'txtPartnerImgPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#partnerImgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });

         //投票对象详情banner图
         $("#txtVoteObjDetailBannerImgPath").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Vote',
                         secureuri: false,
                         fileElementId: 'txtVoteObjDetailBannerImgPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#voteObjDetailBannerImgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });
         //投票列表详情banner图
         $("#txtVoteObjListBannerImgPath").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Vote',
                         secureuri: false,
                         fileElementId: 'txtVoteObjListBannerImgPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#voteObjListBannerImgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });

         //活动未开始海报
         $("#txtNotStartPosterImgPath").live('change', function () {
             try {
                 $.messager.progress({ text: '正在上传图片...' });

                 $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=Vote',
                         secureuri: false,
                         fileElementId: 'txtNotStartPosterImgPath',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $('#notStartPosterImgThumbnailsPath').attr('src', resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     }
                    );

             } catch (e) {
                 alert(e);
             }
         });
         //上传音乐
        

         $("#bgMusicThumbnailsPath").live("change", function () {
             try {
                 $.messager.progress({ text: '正在上传音乐...' });
                 $.ajaxFileUpload(
                         {
                             url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Music&filegroup=bgMusic',
                             secureuri: false,
                             fileElementId: 'bgMusicThumbnailsPath',
                             dataType: 'json',
                             success: function (resp) {
                                 $.messager.progress('close');
                                 if (resp.Status == 1) {
                                     $("#bgMusic").val(resp.ExStr);
                                     Alert("上传背景音乐成功");
                                 }
                                 else {
                                     Alert(resp.Msg);
                                 }
                             }
                         }
                        );

             } catch (e) {
                 alert(e);
             }

         });
        //免费收费切换
         $("input[name=rdisfree]").click(function () {
             ShowVoteIsFree($(this).val());
         });


     });

     function loadGroups(){
         var appendhtml = new StringBuilder();
         appendhtml.AppendFormat('<option value="">默认</option>');
         for (var i = 0; i < groups.length; i++) {
             appendhtml.AppendFormat('<option value="{0}">{0}</option>',groups[i]);
         }
         if(groups.length == 0) appendhtml.AppendFormat('<option value="商品导航">商品导航</option>');
         $("#FooterMenuGroup").append(appendhtml.ToString());
     }



     function ResetCurr() {

         $(":input[type=text]").val("");
         editor.html('');
     }

     KindEditor.ready(function (K) {
         editor = K.create('#txtIntroduction', {
             uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
             items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
             filterMode: false
         });
     });

     //规则页
     KindEditor.ready(function (K) {
         rulepage = K.create('#txtRulePageHtml', {
             uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
             items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
             filterMode: false
         });
     });


     //        //显示图片投票活动或视频投票活动 对应内容
     //        function ShowVoteType(type) {
     //            switch (type) {

     //                case "0":
     //                    $("#trOfflinePayUrl").show();
     //                    $("#trIntroduction").show();
     //                    break;
     //                case "1":
     //                    $("#trOfflinePayUrl").hide();
     //                    $("#trIntroduction").hide();
     //                    break;
     //                default:
     //                    $("#trOfflinePayUrl").show();
     //                    $("#trIntroduction").show();
     //                    break;


     //            }



     //        }

     //显示投票活动免费或收费 对应内容
     function ShowVoteIsFree(isfree) {
         switch (isfree) {

             case "0":
                 $("#trOfflinePayUrl").show();
                 $("#trIntroduction").show();
                 $("#trVoteCountAutoUpdate").hide();
                 break;
             case "1":
                 $("#trOfflinePayUrl").hide();
                 $("#trIntroduction").hide();
                 $("#trVoteCountAutoUpdate").show();
                 break;
             default:
                 $("#trOfflinePayUrl").show();
                 $("#trIntroduction").show();
                 $("#trVoteCountAutoUpdate").hide();
                 break;
         }
     }
     KindEditor.ready(function (K) {
         editorbottom = K.create('#txtBottomContent', {
             uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
             items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
             filterMode: false
         });
     });
     KindEditor.ready(function (K) {
         indexpagehtml = K.create('#txtIndexPageHtml', {
             uploadJson: '/kindeditor-4.1.10/asp.net/upload_json_ChangeName.ashx',
             items: [
                    'source', '|', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', '|', 'undo', 'redo', '|', 'textcolor', 'bgcolor', 'bold', 'italic', 'underline',
                    'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                    'insertunorderedlist', '|', 'image', 'multiimage', 'link', 'unlink', '|', 'baidumap', '|', 'template', '|', 'table'],
             filterMode: false
         });
     });
 </script>
</asp:Content>

