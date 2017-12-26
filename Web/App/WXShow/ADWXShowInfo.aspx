<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true"
    CodeBehind="ADWXShowInfo.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.WXShow.ADWXShowInfo_" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
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
       input[type=text],textarea
        {
          width:80%;
            
        }
        span[name=ResetCurr]{color:Blue;}
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    当前位置：&nbsp;微营销 > 微秀
        <a href="WXShowInfoMgr.aspx" style="float:right;margin-right:20px;" class="easyui-linkbutton" iconcls="icon-back" plain="true">
            返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">

        <div style="font-size: 12px; width: 100%">
            <table width="100%">
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        标题：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShowName" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        描述：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShowDescription" class="" style="width: 100%;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        缩略图：
                    </td>
                    <td width="*" align="left">
                        <img alt="缩略图" src="/img/hb/hb1.jpg" width="80px" height="80px" id="imgThumbnailsPath" />
                        <br />
                        <a href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                            id="aRandomThumbnail" onclick="GetRandomHb();">随机缩略图</a> 
                            <a id="auploadThumbnails"
                                href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true"
                                onclick="txtThumbnailsPath.click()">上传缩略图</a>
                        <img alt="" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为80*80。
                        <br />
                        <input type="file" id="txtThumbnailsPath" name="file1" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        链接：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtShowUrl" class="" style="width: 100%;" />
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                        自动播放：
                    </td>
                    <td width="*" align="left">
                        
                        <select id="selectAutoPlayTimeSpan">
                            <option value="0">不自动播放</option>
                            <option value="1">间隔1秒</option>
                            <option value="2">间隔2秒</option>
                            <option value="3">间隔3秒</option>
                            <option value="4">间隔4秒</option>
                            <option value="5">间隔5秒</option>
                            <option value="6">间隔6秒</option>
                            <option value="8">间隔8秒</option>
                            <option value="10">间隔10秒</option>
                            <option value="12">间隔12秒</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr id="da" >
                    <td style="width: 100px;" align="right" class="tdTitle">
                        上传音乐：
                    </td>
                    <td width="*" align="left">
                        <input type="file" id="FShowMusic" name="FShowMusicm" value="" />
                        <input type="hidden" id="HShowMusic" name="name" value="" />
                    </td>
                </tr>
                <%--<tr>
                    <td colspan="2">
                        
                    </td>
                </tr>--%>
                <tr id="v1">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示页一
                    </td>
                    <td width="*" align="left" colspan="2" id="tdAnswer">
                        <img  name="txtImgStr" id="img1" alt="" width="320" height="504"  src="/img/hb/hb2.jpg" />
                        <input type="file" name="f1" id="f1" value="" v='1' style="display:none;" />
                        <a id="a1" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="f1.click()">上传图片</a>
                        <br />
                        文字标题:<input type="text" name="txtShowTitle" value=" " />
                        <br />
                         <br />
                        标题颜色:<input type="color" name="txtShowTitleColor" value="" />
                        <br />
                         <br />
                        文字内容:<textarea name="txtShowContext"></textarea>
                        <br />
                         <br />
                        文字内容颜色:<input type="color" name="txtShowContextColor" value="" />
                         <br />
                         <br />
                        动画选择:<select name="SAnimation">
                            <option value="1">大→小 </option>
                            <option value="5">小→大 </option>
                            <option value="2">下→上 </option>
                            <option value="7">旋转</option>
                        </select>
                        <span name="ResetCurr">&nbsp;删除</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr id="v2">
                    <td style="width: 100px;" align="right" class="tdTitle">
                        展示页二
                    </td>
                    <td width="*" align="left" colspan="2" id="td1">
                        
                        <img src="/img/hb/hb2.jpg" name="txtImgStr" id="img2" width="320" height="504" />
                        <input type="file" name="f2" id="f2" value="" v='1' style="display:none;"/>
                         <a id="a2" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="f2.click()">上传图片</a>
                         <br />
                        文字标题:<input type="text" name="txtShowTitle" value=" " />
                         <br />
                         <br />
                        标题颜色:<input type="color" name="txtShowTitleColor" value="" />
                        <br />
                         <br />
                        文字内容:<textarea name="txtShowContext"></textarea>
                         <br />
                         <br />
                        文字内容颜色:<input type="color" name="txtShowContextColor" value="" />
                         <br />
                         <br />
                        动画选择:<select name="SAnimation">
                            <option value="1">大→小 </option>
                            <option value="5">小→大 </option>
                            <option value="2">下→上 </option>
                            <option value="7">旋转</option>
                        </select>
                        
                         <br />
                         <br />
                        <span name="ResetCurr" >&nbsp;删除</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr id="ad">
                    <td>
                    </td>
                    <td>
                        <a href="javascript:void(0)" onclick="Addanswer()" class="button button-rounded button-primary">添加展示页</a>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">
                    </td>
                    <td width="*" align="center">
                        <br />
                      
                        <a href="javascript:;" id="btnSave" style="font-weight: bold;width:200px;" class="button button-rounded button-primary">
                            保存</a> <a href="javascript:;" id="btnReset" style="font-weight: bold;width:200px;" class="button button-rounded button-flat">
                                重置</a>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
 <script type="text/javascript">
     var handlerUrl = "/Handler/App/WXShowInfoHandler.ashx";
     var currAction = '<%=currAction %>';
     var editor;
     var currID = '<%=AutoId %>';
     var i = 3;
     $(function () {

         $("span[name='ResetCurr']").live("click", function () {
             $(this).parent().parent().remove();
         });

         if ($.browser.msie) { //ie 下
             //缩略图
             $("#auploadThumbnails").hide();
             $("#txtThumbnailsPath").after($("#aRandomThumbnail"));

         }
         else {
             $("#txtThumbnailsPath").hide(); //缩略图
         }

         if (currAction == 'add') {
             GetRandomHb();

         }
         else {
             ShowEdit(currID);
         }

         $('#btnSave').click(function () {
             var ImgStr = "";
             var ShowTitle = "";
             var ShowContext = "";
             var Animation = "";
             var ShowTitleColor = "";
             var ShowContextColor = "";
             $("img[name='txtImgStr']").each(function () {
                 ImgStr += $(this).attr("src") + ",";
             })

             $("input[name='txtShowTitle']").each(function () {
                 ShowTitle += $(this).val() + ",";
             })
             $("textarea[name='txtShowContext']").each(function () {
                 ShowContext += $(this).val() + ",";
             })
             $("select[name='SAnimation']").each(function () {
                 Animation += $(this).val() + ",";
             })

             $("input[name='txtShowTitleColor']").each(function () {
                 ShowTitleColor += $(this).val() + ",";
             })
             $("input[name='txtShowContextColor']").each(function () {
                 ShowContextColor += $(this).val() + ",";
             })




             try {
                 var model =
                    {
                        Autoid: currID,
                        Action: "InsertUpdateWXShowInfo",
                        ShowName: $.trim($('#txtShowName').val()),
                        ShowDescription: $.trim($('#txtShowDescription').val()),
                        ShowImg: $("#imgThumbnailsPath").attr("src"),
                        ShowUrl: $.trim($('#txtShowUrl').val()),
                        ShowMusic: $("#HShowMusic").val(),
                        ImgStr: ImgStr,
                        ShowTitle: ShowTitle,
                        ShowContext: ShowContext,
                        Animation: Animation,
                        ShowTitleColor : ShowTitleColor,
                        ShowContextColor: ShowContextColor,
                        AutoPlayTimeSpan:$('#selectAutoPlayTimeSpan').val()
                    };


                 $.messager.progress({ text: '正在处理。。。' });
                 $.ajax({
                     type: 'post',
                     url: handlerUrl,
                     data: model,
                     dataType: "json",
                     success: function (resp) {
                         $.messager.progress('close');
                         if (resp.Status == 1) {
                             alert(resp.Msg);
                             window.location.href = "WXShowInfoMgr.aspx";
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

         $('#btnReset').click(function () {
             ResetCurr();
         });

     });

     $("#txtThumbnailsPath").live('change', function () {
         try {
             $.messager.progress({ text: '正在上传图片...' });

             $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
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

      function ShowEdit(activityID) {
          $.ajax({
              type: 'post',
              url: handlerUrl,
              data: { Action: 'GetWxShowInfo', Autoid: currID },
              dataType: "json",
              async: false,
              success: function (resp) {
                  try {
                      if (resp.Status == 0) {
                          var model = resp.ExObj;
                          $('#txtShowName').val(model.ShowName);
                          $('#txtShowDescription').val(model.ShowDescription);
                          $('#imgThumbnailsPath').attr("src",model.ShowImg);
                          $('#txtShowUrl').val(model.ShowUrl);
                          $("#HShowMusic").val(model.ShowMusic);
                          $("#selectAutoPlayTimeSpan").val(model.AutoPlayTimeSpan);
                          if (model.WXShowImgInfos.length > 0) {
                              $("#v1").remove();
                              $("#v2").remove();
                              var html;
                              for (var i = 0; i < model.WXShowImgInfos.length; i++) {
                                 
                                  html += '<tr><td style="width: 100px;" align="right" class="tdTitle">展示页</td>';
                                  html += '<td width="*" align="left" colspan="2" id="td1">';
                                  html += '<img src="' + model.WXShowImgInfos[i].ImgStr + '" name="txtImgStr" width="320" height="504" /><input type="file" v="1"  name="f' + i + '"  id="f' + i + '" value="" style="display:none;" /><a id="a1" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="f' + i + '.click()">上传图片</a><br/>文字标题:<input type="text" name="txtShowTitle" value="' + model.WXShowImgInfos[i].ShowTitle + '" /></br>';

                                  html += '标题颜色:<input type="color" name="txtShowTitleColor" value="' + model.WXShowImgInfos[i].ShowTitleColor + '" /></br>';
                                  
                                  html += '文字内容:<textarea name="txtShowContext">' + model.WXShowImgInfos[i].ShowContext + '</textarea></br>';

                                  html += '文字内容颜色:<input type="color" name="txtShowContextColor" value="' + model.WXShowImgInfos[i].ShowContextColor + '" /></br>';

                                  html += '动画选择:<select name="SAnimation" >';
                                  var sation1 = "";
                                  var sation5 = "";
                                  var sation2 = "";
                                  var sation7 = "";
                                  switch (model.WXShowImgInfos[i].ShowAnimation) {
                                      case 1:
                                          sation1 = "selected='selected'";
                                          break;
                                      case 5:
                                          sation5 = "selected='selected'";
                                          break;
                                      case 2:
                                          sation2 = "selected='selected'";
                                          break;
                                      case 7:
                                          sation7 = "selected='selected'";
                                          break;
                                  }
                                  html += '<option value="1" ' + sation1 + ' >大→小 </option><option value="5" ' + sation5 + '>小→大 </option><option value="2" ' + sation2 + '>下→上 </option><option value="7" ' + sation7 + '>旋转</option>';
                                  html += '</select>';
                                  html += '<span name="ResetCurr">&nbsp删除</span></td></tr><tr><td colspan="2"><hr /></td></tr>';

                              }
                              i = model.WXShowImgInfos.length;
                              setTimeout("i="+i+"",500);
                             
                              $("#ad").before(html);
                              if ($.browser.msie) { //ie 下
                                  $("input[type=file]").show();
                              }
                          }
                      }
                      else {
                          Alert(resp.Msg);
                      }
                  } catch (e) {
                      Alert(e);
                  }
              }

          });


     }


     function Addanswer() {
         var html = '<tr><td style="width: 100px;" align="right" class="tdTitle">追加展示页</td>';
         html += '<td width="*" align="left" colspan="2" id="td1">';
         html += '<img src="/img/hb/hb2.jpg" name="txtImgStr" width="320" height="504" /><input type="file" v="1"  name="f' + i + '"  id="f' + i + '" value="" style="display:none;"/> <a id="a1" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="f' + i + '.click()">上传图片</a> <br/>文字标题:<input type="text" name="txtShowTitle" value=" " /><br/>';
         html += '标题颜色:<input type="color" name="txtShowTitleColor" value="" /></br>';
         html += '文字内容:<textarea name="txtShowContext"></textarea></br>';
         html += '文字内容颜色:<input type="color" name="txtShowContextColor" value="" /></br>';
         html += '动画选择:<select name="SAnimation"><option value="1">大→小 </option><option value="5">小→大 </option><option value="2">下→上 </option><option value="7">旋转</option></select>'
         
         html += '<span name="ResetCurr">&nbsp删除</span></td></tr><tr><td colspan="2"><hr /></td></tr>'
         $("#ad").before(html)
         i++;
     }


     //获取随机海报
     function GetRandomHb() {
         var randInt = GetRandomNum(1, 7);
         imgThumbnailsPath.src = "/img/hb/hb" + randInt + ".jpg";
     }

     $("input[v='1']").live('change', function () {
         var v = $(this).parent().find("img");
         try {
             $.messager.progress({ text: '正在上传图片...' });
             $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=JuActivityImg&filegroup=' + $(this).attr("name"),
                         secureuri: false,
                         fileElementId: $(this).attr("id"),
                         dataType: 'text',
                         success: function (result) {
                             $.messager.progress('close');
                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {

                                 v.attr("src", resp.ExStr);
                                 
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

     $("#FShowMusic").live("change", function () {
         try {
             $.messager.progress({ text: '正在上传音乐...' });
             $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile1&fd=Music&filegroup=FShowMusicm',
                         secureuri: false,
                         fileElementId: 'FShowMusic',
                         dataType: 'text',
                         success: function (result) {
                             $.messager.progress('close');
                             var resp = $.parseJSON(result);
                             if (resp.Status == 1) {
                                 $("#HShowMusic").val(resp.ExStr);
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
    </script>
</asp:Content>
