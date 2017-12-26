<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebMainContent.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.Admin.Outlets.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="/static-modules/lib/tagsinput/jquery.tagsinput.css" />
    <link href="/static-modules/lib/chosen/chosen.min.css" rel="stylesheet" />
    <link href="/static-modules/app/admin/article/style.css" rel="stylesheet" />
    <style type="text/css">
        .Width92P{
            width:92% !important;
        }
        select {
            height:30px;
        }
        .warpOpeate {
            margin-bottom:10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderWebMap" runat="server">
    网点<%=actName %>
    <a href="List.aspx" style="float: right; margin-right: 20px; color: Black;" title="返回" class="easyui-linkbutton" iconcls="icon-back" plain="true">返回</a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderCnt" runat="server">
    <div class="ActivityBox">
        <div style="font-size: 12px; width: 100%">
            <table id="mainTable" style="width: 100%;">
                <tr>
                    <td style="width: 140px;" align="right" class="tdTitle">名称：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtTitle" class="commonTxt" placeholder="名称(必填)" value="<%=nInfo.ActivityName %>" />
                    </td>
                </tr>
                 <tr>
                    <td style="width: 100px;" align="right" class="tdTitle">所属省市区：
                    </td>
                    <td width="*" align="left">
                        <select id="selectProvince" onchange="loadCity('flush')">
                            
                        </select>
                        <select id="selectCity" onchange="loadDist('flush')">
                        </select>
                        <select id="selectArea">
                           <%-- <option value="" data-key="" data-value=""></option>
                            <option value="市中心" data-key="2704" data-value="闸北区" <% = nInfo.K1 == "市中心"?"selected='selected'":"" %> >市中心</option>
                            <option value="静安" data-key="2710" data-value="静安区" <% = nInfo.K1 == "静安"?"selected='selected'":"" %> >静安</option>
                            <option value="徐汇" data-key="2706" data-value="徐汇区" <% = nInfo.K1 == "徐汇"?"selected='selected'":"" %> >徐汇</option>
                            <option value="长宁" data-key="2703" data-value="长宁区" <% = nInfo.K1 == "长宁"?"selected='selected'":"" %> >长宁</option>
                            <option value="黄浦" data-key="2713" data-value="黄浦区" <% = nInfo.K1 == "黄浦"?"selected='selected'":"" %> >黄浦</option>
                            <option value="浦东" data-key="2707" data-value="浦东新区" <% = nInfo.K1 == "浦东"?"selected='selected'":"" %> >浦东</option>
                            <option value="杨浦" data-key="2708" data-value="杨浦区" <% = nInfo.K1 == "杨浦"?"selected='selected'":"" %> >杨浦</option>
                            <option value="虹口" data-key="2712" data-value="虹口区" <% = nInfo.K1 == "虹口"?"selected='selected'":"" %> >虹口</option>
                            <option value="普陀" data-key="2709" data-value="普陀区" <% = nInfo.K1 == "普陀"?"selected='selected'":"" %> >普陀</option>
                            <option value="闵行" data-key="2705" data-value="闵行区" <% = nInfo.K1 == "闵行"?"selected='selected'":"" %> >闵行</option>
                            <option value="松江" data-key="2715" data-value="松江区" <% = nInfo.K1 == "松江"?"selected='selected'":"" %> >松江</option>
                            <option value="奉贤" data-key="2720" data-value="奉贤区" <% = nInfo.K1 == "奉贤"?"selected='selected'":"" %> >奉贤</option>
                            <option value="宝山" data-key="2717" data-value="宝山区" <% = nInfo.K1 == "宝山"?"selected='selected'":"" %> >宝山</option>
                            <option value="嘉定" data-key="2716" data-value="嘉定区" <% = nInfo.K1 == "嘉定"?"selected='selected'":"" %> >嘉定</option>
                            <option value="青浦" data-key="2718" data-value="青浦区" <% = nInfo.K1 == "青浦"?"selected='selected'":"" %> >青浦</option>
                            <option value="金山" data-key="2719" data-value="金山区" <% = nInfo.K1 == "金山"?"selected='selected'":"" %> >金山</option>
                            <option value="崇明" data-key="2721" data-value="崇明县" <% = nInfo.K1 == "崇明"?"selected='selected'":"" %> >崇明</option>--%>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">地址(不含省市区)：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtAddress" class="commonTxt" placeholder="地址(必填)" value="<%=nInfo.ActivityAddress %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">电话：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtPhone" class="commonTxt" placeholder="电话(必填)" value="<%=nInfo.K4 %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">图片：
                    </td>
                    <td width="*" align="left">
                        <div class="ui-sortable">
                            <div style="position: relative;">
                                <img alt="缩略图" src="<% = nInfo.ThumbnailsPath %>" width="138px" height="102px" class="rounded img" />
                                <input type="text" id="txtFile" placeholder="输入图片链接" style="width:80%;position: absolute;bottom: 0px;margin-left: 10px;" value="<%= nInfo.ThumbnailsPath %>" />
                            </div>
                        </div>
                        <div class="clear">
                            <a id="auploadImg" href="javascript:;" class="easyui-linkbutton" iconcls="icon-add2" plain="true" onclick="file1.click()">上传缩略图</a>
                            <img alt="提示" src="/MainStyle/Res/easyui/themes/icons/tip.png" />请点击缩略图上传JPG、PNG和GIF格式图片，图片最佳显示效果大小为460*340。
                            <input type="file" id="file1" class="file file1" name="file1" style="display: none;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">服务时间：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtServerTime" class="commonTxt" placeholder="服务时间(选填)" value="<%=nInfo.ServerTimeMsg %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">主办业务：</td>
                    <td width="*" align="left">
                        <input type="text" id="txtServerMsg" class="commonTxt" placeholder="主办业务(选填)" value="<%=nInfo.ServicesMsg %>" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">分类：
                    </td>
                    <td width="*" align="left">
                        <%=sbCategory.ToString()%>
                    </td>
                </tr>
               
                <tr>
                    <td style="width: 100px; vertical-align: top;" align="right" class="tdTitle">标签：
                    </td>
                    <td width="*" align="left">
                        <input id="txtTags" />
                        <a href="javascript:;" class="button button-primary button-rounded button-small" id="btnSelectTags">选择标签</a>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">选择坐标：</td>
                    <td width="*" align="left" valign="bottom">
                        <div id="ifAmap" style="width:100%; height:450px;"></div>
                        <div>
                            <br />
                        <input id="txtMapKeyword" type="text" style="width:200px; height:24px;" placeholder="输入地址搜索" />&nbsp;<a href="javascript:void(0);" class="easyui-linkbutton" onclick="placeSearch()">搜索</a>
                        经度：<span id="spanLongitude" style="color:red;"><%=nInfo.UserLongitude %></span> 
                        纬度：<span id="spanLatitude"  style="color:red;"><%=nInfo.UserLatitude %></span>

                        </div>
                        <div>
                       
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="tdTitle">排序：
                    </td>
                    <td width="*" align="left">
                        <input type="text" id="txtSort" class="commonTxt" placeholder="排序" value="<%=nInfo.Sort.HasValue?nInfo.Sort.Value:0 %>" style="width: 100px;" onkeyup="value=value.replace(/[^\d]/g,'')" />  数字越大越排前
                    </td>
                </tr>
            </table>
        </div>
        <div style="text-align: center;">
            <a href="javascript:void(0);" id="btnSave" class="button button-rounded button-primary" style="width: 160px;">保存</a>
            <a href="List.aspx" id="btnPageBack" class="button glow" style="width: 160px;">返回</a>
        </div>
    </div>
    
    <div class="hidden warpTagDiv" style="border-radius: 8px;">
        <div class="warpTagSelect">
            <div class="warpContent">

                <div class="warpTagDataList hidden">
                    <div class="warpTagSelectBtn"><a href="javascript:;" class="mLeft15 btnTagSelect" data-op="all">全选</a><a href="javascript:;" class="mLeft10 btnTagSelect" data-op="reverse">反选</a></div>
                    <ul class="ulTagList">

                </ul>
                </div>
                
                <div class="warpNoData">
                    暂无数据
                </div>

                <div class="clear"></div>

            </div>
            <hr />
            <div class="warpOpeate">
                <a href="javascript:;" class="button button-primary button-rounded button-small btnSave">确定</a>
                <a href="javascript:;" class="button button-rounded button-small btnCancel">取消</a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="bottom" runat="server">
    <script src="/static-modules/lib/tagsinput/jquery.tagsinput.js" type="text/javascript"></script>
    <script src="/static-modules/lib/chosen/chosen.jquery.min.js" type="text/javascript"></script>
    <script src="/lib/layer/2.1/layer.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.3&key=224bd7222ce22c01673ff105ffb93fda"></script>
    <script type="text/javascript">
        var handlerUrl = '/serv/api/admin/outlets/';
        var nid = '<% =nInfo.JuActivityID%>';
        <%--var oProvinceCode = '<% = this.Request["provinceCode"] %>';
        var oCityCode = '<% = this.Request["cityCode"]%>';
        var oAreaCode = '<% = this.Request["areaCode"]%>';--%>

        var nCateId = '<% = nInfo.CategoryId %>';
        var nProvinceCode = '<% = nInfo.ProvinceCode %>';
        var nCityCode = '<% = nInfo.CityCode %>';
        var nAreaCode = '<% = nInfo.DistrictCode %>';
        var nk1 = '<% = nInfo.K1 %>';
        var currTagsStr = '<%=nInfo.Tags %>';
        var currTags = [];
        var nLongitude = '<%=string.IsNullOrWhiteSpace(nInfo.UserLongitude)?"121.472644":nInfo.UserLongitude %>';
        var nLatitude = '<%=string.IsNullOrWhiteSpace(nInfo.UserLatitude)?"31.231706":nInfo.UserLatitude %>';
        var mapObj;//地图对象
        var marker;//标记
        var toPlaceSearch;
        $(function () {
            //oProvinceCode = "25";
            //oCityCode = "321";
            //oAreaCode = "2717";
            loadProvince();//加载省份列表
            var opt = {zoom: 15};//地图选项
            if(nLongitude !="" && nLatitude!=""){
                opt.center = new AMap.LngLat(nLongitude, nLatitude);//设置地图中心点
            }
            else{
                opt.center = new AMap.LngLat("121.472644", "31.231706"); //设置地图中心点
            }
            mapObj = new AMap.Map("ifAmap", opt);//初始化地图
            
            if (nLongitude != "" && nLatitude != "") {//添加地图标记
                marker = new AMap.Marker({
                    position: [nLongitude, nLatitude]
                });
                marker.setMap(mapObj);
            }
            AMap.event.addListener(mapObj, 'click', getLnglat); //点击事件

            AMap.service(["AMap.PlaceSearch"], function () { //引用地图搜索插件
                toPlaceSearch = new AMap.PlaceSearch({
                    city: '上海',
                    pageSize: 1,
                    pageIndex: 1,
                    extensions: 'base'
                });
            });

            //if (nProvinceCode != "") { oProvinceCode = nProvinceCode; }
            //if (nCityCode != "") { oCityCode = nCityCode; }
            //if (nAreaCode != "") { oAreaCode = nAreaCode; }
            if (nCateId != "") { $("#ddlCate").val(nCateId); }
            if (currTagsStr != '') {
                currTags = currTagsStr.split(',');
            }
            chosenBind({ id: '#ddlCate', placeholder: '选择类型', width: '180px' });
            //chosenBind({ id: '#selectProvince', placeholder: '选择省份', width: '180px' });
            //chosenBind({ id: '#selectCity', placeholder: '选择省份', width: '180px' });
            //chosenBind({ id: '#selectArea', placeholder: '选择地区', width: '180px' });

            //areaBind();

            //标签插件初始化
            $('#txtTags').tagsInput({
                height: '60px',
                width: 'auto',
                interactive: false,
                onAddTag: function (tag) {
                    //currTags.push(tag);
                    console.log('添加了' + tag);
                },
                onRemoveTag: function (tag) {
                    currTags.RemoveItem(tag);
                    console.log('删除了' + tag);
                }
            });

            addTagList(currTags);//添加标签 编辑时显示

            //标签操作按钮
            $("#btnSelectTags").bind('click', function () {
                loadSelectTagsData();
            });

            //确定选择新标签
            $(".warpTagSelect .warpOpeate .btnSave").bind('click', function () {

                //构造标签新数组
                currTags = [];
                var chekList = $('.warpTagSelect .tagChk');
                for (var i = 0; i < chekList.length; i++) {
                    if ($(chekList[i]).attr('checked')) {
                        currTags.push($(chekList[i]).val());
                    }
                }
                //显示标签
                tagClear();
                addTagList(currTags);
                layer.closeAll();

            });

            //取消选择标签
            $(".warpTagSelect .warpOpeate .btnCancel").bind('click', function () {
                layer.closeAll();
            });

            //全选 反选 全部标签
            $(".warpTagSelect .btnTagSelect").bind('click', function () {
                var op = $(this).attr('data-op');
                if (op == 'all')
                    selectTagAll();
                if (op == 'reverse')
                    selectTagReverse();
            });

            //保存按钮
            $("#btnSave").live("click", function () {
                postUpdateData();
            });
            //图片上传按钮
            $("#txtFile").live("change", function () {
                var filep = $(this).val();
                $(".img").attr("src", filep);
            })
            //选择图片
            $("#file1").live('change', function () {
                try {
                    $.messager.progress({ text: '正在上传图片...' });
                    $.ajaxFileUpload(
                     {
                         url: '/Handler/CommHandler.ashx?action=UploadSingelFile&fd=JuActivityImg',
                         secureuri: false,
                         fileElementId: 'file1',
                         dataType: 'json',
                         success: function (resp) {
                             $.messager.progress('close');
                             if (resp.Status == 1) {
                                 $(".img").attr("src",resp.ExStr);
                                 $("#txtFile").val(resp.ExStr);
                             }
                             else {
                                 Alert(resp.Msg);
                             }
                         }
                     });

                } catch (e) {
                    alert(e);
                }
            });
        });

        //获取经纬度
        function getLnglat(e) {
            marker.setMap();
            var nlng = e.lnglat.getLng();
            var nlat = e.lnglat.getLat();
            mapObj.setCenter([nlng, nlat]);
            marker = new AMap.Marker({
                position: [nlng, nlat]
            });
            marker.setMap(mapObj);
            $('#spanLongitude').text(nlng);
            $('#spanLatitude').text(nlat);
        }

        //标签列表
        function loadSelectTagsData() {
            $.ajax({
                type: 'POST',
                url: '/Handler/App/CationHandler.ashx',
                data: { Action: "QueryMemberTag", TagType: 'Outlets', page: 1, rows: 100000000 },
                success: function (resp) {
                    var data = $.parseJSON(resp);
                    if (data.total == 0) {
                        $('.warpTagSelect').find('.warpNoData').show();
                        $('.warpTagSelect').find('.warpTagDataList').hide();
                    } else {
                        $('.warpTagSelect').find('.warpTagDataList').show();
                        $('.warpTagSelect').find('.warpNoData').hide();

                        //构造数据
                        var strHtml = new StringBuilder();
                        for (var i = 0; i < data.rows.length; i++) {
                            if (data.rows[i].TagName == "") continue;
                            strHtml.Append('<li class="overflow_ellipsis Width92P"><label>');
                            strHtml.AppendFormat('<input type="checkbox" name="tag" class="tagChk" value="{0}" {1} />{0}', data.rows[i].TagName, currTags.Contains(data.rows[i].TagName) ? 'checked' : '');
                            strHtml.Append('</label></li>');
                        }

                        $('.warpTagSelect').find('.ulTagList').html(strHtml.ToString());


                    }
                    var tagDiv = layer.open({
                        type: 1,
                        shade: [0.2, '#000'],
                        shadeClose: true,
                        area: ['300', '320'],
                        title: ['选择标签', 'background:#1B9AF7; color:#fff;'],
                        border: [0],
                        content: $('.warpTagDiv')
                    });


                }
            });
        }

        //添加标签
        function addTagList(list) {
            for (var i = 0; i < list.length; i++) {
                if (!$('#txtTags').tagExist(list[i]))
                    $('#txtTags').addTag(list[i]);
            }
        }

        //清除标签
        function tagClear() {
            $('#txtTags').importTags('');
        }

        //标签全选
        function selectTagAll() {
            $('.warpTagSelect .tagChk').attr('checked', true);
        }

        //标签反选
        function selectTagReverse() {
            $('.warpTagSelect .tagChk').each(function () {
                var v = $(this).attr('checked');
                $(this).attr('checked', !v);
            });
        }

        //保存数据
        function postUpdateData() {
            var model = {
                id: nid,
                title: $.trim($("#txtTitle").val()),
                address: $.trim($("#txtAddress").val()),
                img: $.trim($("#txtFile").val()),
                server_time: $.trim($("#txtServerTime").val()),
                server_msg: $.trim($("#txtServerMsg").val()),
                cate_id: $.trim($("#ddlCate").val()),
                province_code: $("#selectProvince").val(),
                province: $("#selectProvince").find("option:selected").text(),
                city_code: $("#selectCity").val(),
                city: $("#selectCity").find("option:selected").text(),
                k1: $("#selectArea").find("option:selected").text(),
                district_code: $("#selectArea").val(),
                district: $("#selectArea").find("option:selected").text(),
                longitude: $.trim($('#spanLongitude').text()),
                latitude: $.trim($('#spanLatitude').text()),
                tags: currTags.join(','),
                sort: $.trim($("#txtSort").val()),
                k4: $.trim($("#txtPhone").val()),
                k5:"<%=Request["supplier_id"]%>"//供应商Id
            }
            if (model.title == "") {
                // $.messager.alert("提示", "名称必填");
                //alert("名称必填");
                $("#txtTitle").focus();
                return;
            }
            if (model.address == "") {
                
                $("#txtAddress").focus();
                return;
            }
            if (model.k4 == "") {

                $("#txtPhone").focus();
                return;
            }
            //if (model.k1 == "") {
            //    $.messager.alert("提示", "请选择县区");
            //    return;
            //};
            if (model.longitude == "" || model.latitude == "") {
                //$.messager.alert("提示", "请选择坐标");
                alert("请选择坐标");
                return;
            };

            if (model.sort == "") model.sort = 0;
            $.messager.progress({ text: '正在提交...' });
            $.ajax({
                type: 'post',
                url: handlerUrl + (nid == "0" ? "Add.ashx" : "Update.ashx"),
                data: model,
                dataType: "json",
                success: function (resp) {
                    $.messager.progress('close');
                    if (resp.status) {
                        if (nid == "0") {
                            Alert("添加成功");
                            ClearForm();
                        }
                        else {
                            location.href = "List.aspx";
                        }
                    }
                    else {
                        $.messager.alert("提示", resp.msg);
                    }
                },
                error: function () {
                    $.messager.progress('close');
                }
            });
        }

        //清除输入
        function ClearForm() {
            $("#txtTitle").val("");
            $("#txtAddress").val("");
            $("#txtServerTime").val("");
            $("#txtServerMsg").val("");
            $("#txtSort").val("0");
        }
        //function areaBind() {

        //    //初始化处理省份选项
        //    setProvinceSelect(oProvinceCode);

        //    $("#selectProvince").live('change', function () {
        //        //选择省份
        //        var selectProvince = $(this).val();
        //        setCitySelect(selectProvince);
        //    });
        //    if (oProvinceCode != "") {
        //        setCitySelect(oProvinceCode, oCityCode);
        //    }

        //    $("#selectCity").live('change', function () {
        //        //选择城市
        //        var selectCode = $(this).val();
        //        setAreaSelect(selectCode);
        //    });
        //    if (oCityCode != "") {
        //        setAreaSelect(oCityCode, oAreaCode);
        //    }
        //}

        function chosenBind(data) {
            $(data.id).attr('data-placeholder', data.placeholder).chosen({
                no_results_text: '没有找到结果',
                width: data.width
            });
        }
        //function getAreaSelectOptionDom(data, selectVal,hasNull) {
        //    var strHtml = new StringBuilder();
        //    if (hasNull ==1) strHtml.Append('<option value=""></option>');
        //    for (var i = 0; i < data.length; i++) {
        //        if (selectVal == data[i].DataKey) {
        //            strHtml.AppendFormat('<option value="{0}" selected="selected">{1}</option>', data[i].DataKey, data[i].DataValue);
        //        } else {
        //            strHtml.AppendFormat('<option value="{0}">{1}</option>', data[i].DataKey, data[i].DataValue);
        //        }
        //    }
        //    return strHtml.ToString();
        //}
        //function setProvinceSelect(selectVal) {
        //    var data = zymmp.location.getProvince();
        //    if (selectVal != "") {
        //        var nstr = getAreaSelectOptionDom(data, selectVal, 0);
        //        $('#selectProvince').chosen('destroy').html('').append(nstr);
        //        $('#selectProvince').val(selectVal);
        //    }
        //    else {
        //        var nstr = getAreaSelectOptionDom(data, "", 1);
        //        $('#selectProvince').chosen('destroy').html('').append(nstr);
        //    }
        //    chosenBind({  id: '#selectProvince',placeholder: '选择省份',  width: '180px' });
        //}

        //function setCitySelect(provinceCode, selectVal) {
        //    var cityData = zymmp.location.getCity(provinceCode);
        //    var nstr = getAreaSelectOptionDom(cityData, selectVal, 1);
        //    $('#selectCity').chosen('destroy').html('').append(nstr);
        //    $('#selectArea').chosen('destroy').html('<option value=""></option>');

        //    chosenBind({ id: '#selectCity', placeholder: '选择城市', width: '180px' });
        //    chosenBind({ id: '#selectArea', placeholder: '选择地区', width: '180px' });
        //}

        //function setAreaSelect(cityCode, selectVal) {
        //    var areaData = zymmp.location.getDistrict(cityCode);
        //    var nstr = getAreaSelectOptionDom(areaData, selectVal, 1);
        //    $('#selectArea').chosen('destroy').html('').append(nstr);

        //    chosenBind({ id: '#selectArea', placeholder: '选择地区', width: '180px' });
        //}

        //搜索地址
        function placeSearch() {
            var keyword = $.trim($('#txtMapKeyword').val());
            if (keyword != '') {
                toPlaceSearch.search(keyword, function (status, result) {
                    if (status === 'complete' && result.info === 'OK') {
                        var poiArr = result.poiList.pois;
                        marker.setMap();
                        var nlng = poiArr[0].location.getLng();
                        var nlat = poiArr[0].location.getLat();
                        mapObj.setCenter([nlng, nlat]);
                        marker = new AMap.Marker({
                            position: [nlng, nlat]
                        });
                        marker.setMap(mapObj);
                        $('#spanLongitude').text(nlng);
                        $('#spanLatitude').text(nlat);
                    }
                    else if (status==='error') {
                        alert(result);
                    }
                    else {
                        alert('地址未找到');
                    }
                });
            }
        }


        //加载省份
        function loadProvince() {
              
            
               
            
            
            $.ajax({
                type: 'post',
                url:"/serv/api/mall/area.ashx",
                data: { action: "Provinces" },
                dataType: "json",
                success: function (resp) {
                    var str = new StringBuilder();
                    for (var i = 0; i < resp.list.length; i++) {
                       
                        if (resp.list[i].name == "<%=nInfo.Province%>") {//编辑
                            str.AppendFormat('<option value="{0}" selected="selected">{1}</option>', resp.list[i].code, resp.list[i].name);

                        }
                        else {
                            str.AppendFormat('<option value="{0}">{1}</option>', resp.list[i].code, resp.list[i].name);
                        }
                    }
                    $("#selectProvince").html(str.ToString());
                    $("#selectProvince").chosen({
                        no_results_text: "未找到此选项!",
                        width: "20%"
                    }); 
                    $("#selectCity").trigger("liszt:updated");
                    loadCity();
                },
                error: function () {
                    
                }
            });


        }

        //加载城市
        function loadCity() {
           
          
            $.ajax({
                type: 'post',
                url: "/serv/api/mall/area.ashx",
                data: { action: "Cities",province_code:$("#selectProvince").val() },
                dataType: "json",
                success: function (resp) {

                    var str = new StringBuilder();
                    for (var i = 0; i < resp.list.length; i++) {
                        if (resp.list[i].name == "<%=nInfo.City%>") {//编辑
                            str.AppendFormat('<option value="{0}" selected="selected">{1}</option>', resp.list[i].code, resp.list[i].name);

                        }
                        else {
                            str.AppendFormat('<option value="{0}">{1}</option>', resp.list[i].code, resp.list[i].name);
                        }
                    }
                    $("#selectCity").html(str.ToString());
                    
                    $("#selectCity").chosen({
                        no_results_text: "未找到此选项!",
                        width: "20%"
                    });
                    $("#selectCity").trigger("chosen:updated");
                    loadDist();
                },
                error: function () {

                }
            });


        }
        
        //加载区域
        function loadDist() {

            $.ajax({
                type: 'post',
                url: "/serv/api/mall/area.ashx",
                data: { action: "Districts", city_code: $("#selectCity").val() },
                dataType: "json",
                success: function (resp) {

                    var str = new StringBuilder();
                    for (var i = 0; i < resp.list.length; i++) {
                        if (resp.list[i].name == "<%=nInfo.District%>") {//编辑
                            str.AppendFormat('<option value="{0}" selected="selected">{1}</option>', resp.list[i].code, resp.list[i].name);

                        }
                        else {
                            str.AppendFormat('<option value="{0}">{1}</option>', resp.list[i].code, resp.list[i].name);
                        }
                    }
                    $("#selectArea").html(str.ToString());
                    $("#selectArea").chosen({
                        no_results_text: "未找到此选项!",
                        width: "20%"
                    });
                    $("#selectArea").trigger("chosen:updated");
                },
                error: function () {

                }
            });


        }

    </script>
</asp:Content>
