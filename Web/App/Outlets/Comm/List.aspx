<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Outlets.Comm.List" %>
 <% 
     StringBuilder strHtml = new StringBuilder();
     List<string> searchOthers = new List<string>() { "CategoryId", "Tags" };
    %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no, width=device-width"/>
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <meta name="apple-mobile-app-status-bar-style" content="default"/>
    <meta name="format-detection" content="telephone=no"/>
    <title><%=typeConfig.CategoryTypeDispalyName %></title>
    <link href="http://file.comeoncloud.net/lib/ionic/ionic.css" rel="stylesheet"/>
    <link href="http://file.comeoncloud.net/lib/layer.mobile/need/layer.css" rel="stylesheet"/>
    <link href="http://file.comeoncloud.net/css/global-m.css" rel="stylesheet"/>
    <link href="/App/Outlets/Outlets.css" rel="stylesheet"/>
    <script src="http://file.comeoncloud.net/lib/zepto/zepto.min.js"></script>
    <script src="http://file.comeoncloud.net/lib/vue/vue.min.js"></script>
    <script src="http://file.comeoncloud.net/lib/layer.mobile/layer.m.js" charset="utf-8"></script>
    
    <%if (!string.IsNullOrWhiteSpace(typeConfig.ShareTitle))
        {%>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <%}%>
    <%--<script src="http://file.comeoncloud.net/Scripts/global-m.js" charset="utf-8"></script>--%>
    <script src="/Scripts/global-m.js" charset="utf-8"></script>
    <script src="http://file.comeoncloud.net/Scripts/StringBuilder.Min.js" charset="utf-8"></script>
    <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.3&key=224bd7222ce22c01673ff105ffb93fda"></script>
    <%}%>
</head>
<body v-bind:class="{wrapQQBrowser:isQQBrowser}">
<div class="offer-view wrapOffer">
    <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
    <div class="amb" v-if="filterAddressShowNum != 0" v-on:click="hideFilterAddressPanel()">
    </div>
    <div class="ambAddress" v-if="filterAddressShowNum != 0" style="top: 44px;">
        <div class="ambCurAddressPanel">
            <div style="height: 7px;">
                <%--经度:<span class="colorRed" v-text="curLongitude"></span>，纬度:<span class="colorRed" v-text="curLatitude"></span>--%>&nbsp;
            </div>
            <div class="ambCurAddressConfrim">
                <button class="button button-small" @click="addLocalAddress()">记录地址</button>
                <%--<button class="button button-small" @click="toSelectMapAddress()">地图选址</button>--%>
            </div>
        </div>
        <div class="ambLogAddressPanel">
            <div class="ambLogAddressList">
                <div class="ambLogAddress" :class="{'selectedLogAddress':$index==curSelectLogIndex}"
                     v-for="localAddress in localAddressList"
                        @click="selectLocalAddress($index)">
                    <div class="ambLogAddressContent">
                        <div class="ambLogAddressContentSpan">
                            <i class="icon iconfont icon-place"></i><span v-text="localAddress.address"></span>
                        </div>
                    </div>
                    <div class="ambDelAddressConfrim" @click="deleteLocalAddress($index)">
                        <i class="icon iconfont icon-guanbi font12 colorRed"></i>
                    </div>
                </div>
            </div>
        </div>
        <div class="ambAddressConfrim" v-if="filterAddressShowNum != 0">
            <button class="button button-small width70" style="float:left;" v-if="curSelectLogIndex>=0" @click="showReplaceAddressName()">重命名</button>
            <button class="button button-small width70" @click="confrimSelectLogAddress()">确定</button>
            <button class="button button-small width70" @click="closeSelectLogAddress()">关闭</button>
        </div>
    </div>
    <%}%>
    <%
        int topNum = 0;
        if ((typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2) || !string.IsNullOrWhiteSpace(typeConfig.Ex2))
        {%>
    <div class="wrapHeader">
    <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {
          %>
        <div class="bar bar-header item-input-inset bgColor5BD5BE ZIndex999" >
            <label class="item-input-wrapper cur-address lineHeight28" v-if="filterAddressShowNum == 0" @click="showFilterAddressPanel()">
                <i class="icon iconfont icon-place"></i>
                <div class="divSpanCurAddress">
                    <span v-text="curAddress"></span>
                </div>
            </label>
            <label class="item-input-wrapper cur-address lineHeight28" v-if="filterAddressShowNum != 0" @click="toSelectMapAddress()">
                <i class="icon iconfont icon-place"></i>
                <div class="divSpanCurAddress">
                    <span v-text="curAddress"></span>
                </div>
                <%--<input class="txtCurAddress" type="text" v-model="curAddress" placeholder="当前地址别名"/>--%>
            </label>
        </div>
    <%
        topNum += 44;
        }%>
    <%if (!string.IsNullOrWhiteSpace(typeConfig.Ex2))
      {
          %>
        <div class="bar bar-header item-input-inset bgColor5BD5BE">
            <label class="item-input-wrapper keyword">
                <input type="text" v-model="keyword" placeholder="输入关键字查询..."/>
                <i class="icon iconfont icon-sousuo" @click="getDataList(true)"></i>
            </label>
        </div>
    <%
        topNum += 44;
      }%>
    </div>
    <%}%>
    <%if (searchField.Count>0)
        {%>
    <div class="wrapFilter" 
            :style="{'top':'<%=topNum %>px'}">
        <div class="row">
            <%
                strHtml = new StringBuilder();
                for (var i = 0; i<searchField.Count;i++)
                {
                    strHtml.AppendLine(string.Format("<div class=\"col\" @click=\"openFilterPanel({0})\">", i + 1));
                    if (searchField[i].Field == "CategoryId")
                    {
                        strHtml.AppendLine(string.Format("<span v-text=\"curCateText\">所有{0}</span>", searchField[i].MappingName));
                    }
                    else if (searchField[i].Field == "Tags")
                    {
                        strHtml.AppendLine(string.Format("<span v-text=\"curTagText\">所有{0}</span>", searchField[i].MappingName));
                    }
                    else
                    {
                        strHtml.AppendLine(string.Format("<span v-text=\"cur{0}Text\">所有{1}</span>", searchField[i].Field, searchField[i].MappingName));
                    }
                    strHtml.AppendLine("<i class=\"iconfont icon-arrowdown arrowdown\"></i>");
                    strHtml.AppendLine("</div>");
                }
                this.Response.Write(strHtml.ToString());
                 %>
        </div>
            <%
                strHtml = new StringBuilder();
                for (var i = 0; i<searchField.Count;i++)
                {
                    strHtml.AppendLine(string.Format("<div class=\"wrapFilterList\" v-if=\"filterListShowNum == {0}\">", i + 1));
                    strHtml.AppendLine("<div class=\"list\">");
                    strHtml.AppendLine("<div class=\"item font12\"");
                    if (searchField[i].Field == "CategoryId")
                    {
                        strHtml.AppendLine(" v-for=\"cate in cateList\"");
                        strHtml.AppendLine(string.Format(" @click=\"setFilter({0},cate,'{1}')\"", i + 1, searchField[i].Field));
                        strHtml.AppendLine(" :class=\"{'selected':cate.value == curCate}\"");
                        strHtml.AppendLine(" v-text=\"cate.text\">");
                    }
                    else if (searchField[i].Field == "Tags")
                    {
                        strHtml.AppendLine(" v-for=\"tag in tagList\"");
                        strHtml.AppendLine(string.Format(" @click=\"setFilter({0},tag,'{1}')\"", i + 1, searchField[i].Field));
                        strHtml.AppendLine(" :class=\"{'selected':tag.value == curTag}\"");
                        strHtml.AppendLine(" v-text=\"tag.text\">");
                    }
                    else
                    {
                        strHtml.AppendLine(string.Format(" v-for=\"item in {0}List\"", searchField[i].Field));
                        strHtml.AppendLine(string.Format(" @click=\"setFilter({0},item,'{1}')\"", i + 1, searchField[i].Field));
                        strHtml.AppendLine(string.Format(" :class=\"{1}'selected':item.value == cur{0}{2}\"", searchField[i].Field,"{","}"));
                        strHtml.AppendLine(" v-text=\"item.text\">");
                    }
                    strHtml.AppendLine("</div>");
                    strHtml.AppendLine("</div>");
                    strHtml.AppendLine("</div>");
                }
                this.Response.Write(strHtml.ToString());
                 %>
    </div>
    <%}%>
    <div class="list wrapDataList"
            :style="{'padding-top':'<%=topNum+44%>px'}">
        <div class="mb" v-if="filterListShowNum != 0" @click="hideFilterPanel()">
        </div>
        <!--<div class="item item-divider item-divider-reset">-->
            <!--<i class="iconfont icon-place"></i>服务网点-->
        <!--</div>-->

        <div class="item item-reset "
             v-for="row in list"
            @click="goInfo(row)"
            >
            <img class="leyeImg" v-if="row.ThumbnailsPath" :src="row.ThumbnailsPath"/>
            <div class="wrapItem"
                :style="{'padding-left':row.ThumbnailsPath?'85px':'5px'}">
            <p>
                <span class="titleArea" v-text="row.ActivityName"></span>
            <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2){%>
                <span class="metre" v-if="row.Distance" v-text="row.Distance|distance row.UserLongitude row.UserLatitude"></span>
            <%}%>
            </p>
            <p>
                    <span class="font12 whitespace address" v-text="row.Summary"></span>
            </p>
            <%
                List<string> ors = new List<string>() { "ThumbnailsPath", "ActivityName", "Distance", "Summary" };
                    strHtml = new StringBuilder();
                    foreach (var item in listField.Where(p => !ors.Contains(p.Field)))
                    {
                        strHtml.AppendLine(string.Format("<p v-if=\"row.{0}\">", item.Field));
                        strHtml.AppendLine(string.Format("<span class=\"font12\">{0}:</span>", item.MappingName));
                        strHtml.AppendLine(string.Format("<span class=\"font12 whitespace address\" v-text=\"row.{0}\"></span>", item.Field));
                        strHtml.AppendLine("</p>");
                    }
                    this.Response.Write(strHtml.ToString());
                 %>
            </div>
        </div>
    </div>
</div>
</body>
</html>
<script type="text/javascript">
    var vm;
    $(function () {
        
    <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
        Vue.filter('distance', function (value, rLng, rLat) {
            if(!rLng){
                if (!value || value == "") return "";
                if (value < 0) return "";
                var m = value * 1000;
                if (m < 10) return "<10m";
                if (m < 1000) return Math.round(m) + "m";
                return Math.round(value * 100) / 100 + "km";
            }
            else{
                var curValue = vm.curLngLat.distance([rLng, rLat]);
                if (curValue < 0) return "";
                if (curValue < 10) return "<10m";
                if (curValue < 1000) return Math.round(curValue) + "m";
                return Math.round(curValue / 10) / 100 + "km";
            }
        })
    <%}%>
        vm = new Vue({
            el: 'body',
            data: {
                first: 0,
                isQQBrowser: navigator.userAgent && navigator.userAgent.indexOf('MQQBrowser') > 0,
                list: [],
                type:'<%=Request["type"]%>',
                page: 1,
                rows: 10,
                total: 0,
                layerIndex: -1,
                keyword: '',
                filterAddressShowNum:0,
                filterListShowNum: 0,
    <%
    var categoryIdField = searchField.FirstOrDefault(p => p.Field.Equals("CategoryId"));
    if (categoryIdField!=null)
        {%>
                curCate: '',
                curCateText: '所有<%=categoryIdField.MappingName%>',
                cateList: [{ value: '', text: '所有<%=categoryIdField.MappingName%>' }],
                <%}%>
    <%
    var tagsField = searchField.FirstOrDefault(p => p.Field.Equals("Tags"));
    if (tagsField!=null)
        {%>
                curTag: '',
                curTagText: '所有<%=tagsField.MappingName%>',
                tagList: [{ value: '', text: '所有<%=tagsField.MappingName%>' }],
                <%}%>
    <%if (searchField.Count>0)
        {%>
        <%
            strHtml = new StringBuilder();
            foreach (var item in searchField.Where(p=>!searchOthers.Contains(p.Field)))
            {
                strHtml.AppendLine(string.Format(" cur{0}: '',", item.Field));
                strHtml.AppendLine(string.Format(" cur{0}Text: '所有{1}',", item.Field, item.MappingName));
                strHtml.AppendLine(string.Format(" {0}List:[ ", item.Field, "{", "}"));
                strHtml.AppendLine(string.Format(" {1}value: '', text: '所有{0}'{2},", item.MappingName, "{", "}"));

                foreach (var option in item.Options.Split(','))
                {
                    strHtml.AppendLine(string.Format(" {1}value: '{0}', text: '{0}'{2},", option, "{", "}"));
                }
                strHtml.AppendLine(" ], ");
            }
        this.Response.Write(strHtml.ToString());
        %>
    <%}%>
                <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
                curLngLat:null,
                curLongitude: 0,
                curLatitude: 0,
                curAddress: '',
                curSelectLogIndex:-1,
                curSelectAddressIndex:-1,
                localAddressList: [],
                geolocation: null,
                geocoder: null,
                mapObj: null,
                <%}%>
    <%if (!string.IsNullOrWhiteSpace(typeConfig.ShareTitle))
        {%>
                shareInfo: {
                    title: '<%= typeConfig.ShareTitle %>', // 分享标题
                    desc: '<%= typeConfig.ShareDesc %>', // 分享描述
                    link: '<%= typeConfig.ShareLink %>', // 分享链接
                    imgUrl: '<%= typeConfig.ShareImg %>', // 分享图标
                    type: '', // 分享类型,music、video或link，不填默认为link
                    dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                },
    <%}%>
                isShowShade:false  //是否显示阴影
            },
            methods: {
                init: init,
    <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
                showFilterAddressPanel:showFilterAddressPanel,
                hideFilterAddressPanel:hideFilterAddressPanel,
                getLocation: getLocation,
                confrimSelectLogAddress:confrimSelectLogAddress,
                closeSelectLogAddress:closeSelectLogAddress,
                addLocalAddress:addLocalAddress,
                deleteLocalAddress:deleteLocalAddress,
                selectLocalAddress:selectLocalAddress,
                toSelectMapAddress: toSelectMapAddress,
    <%}%>
                getDataList: getDataList,
                setFilter: setFilter,
                openFilterPanel: openFilterPanel,
                hideFilterPanel: hideFilterPanel,
                goInfo: goInfo,
                toTel:toTel
            }
        });
        vm.init();
    });
    function init() {
        vm.layerIndex = layer.open({ type: 2, shadeClose: false });
    <%if (!string.IsNullOrWhiteSpace(typeConfig.ShareTitle))
        {%>
        if (RegExp("MicroMessenger").test(navigator.userAgent)) {
            WXInit();
        }
    <%}%>
    <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
        AddressLogInit();
        getLocation();
        <%} else{ %>
        getDataList(true);
        <%}%>
        
    <%if (searchField.Exists(p => p.Field.Equals("CategoryId")))
        {%>
        getCateList();
        <%}%>
    <%if (searchField.Exists(p => p.Field.Equals("Tags")))
        {%>
        getTagList();
    <%}%>
    }
    <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
    function AddressLogInit(){
        var localAddressList = localStorage.getItem('addressList');
        if(localAddressList != null) vm.localAddressList = JSON.parse(localAddressList);
    }
    function toSelectMapAddress(){
        window.location.href="ShowMap.aspx?type="+vm.type+"&address="+encodeURIComponent(vm.curAddress) +"&longitude="+vm.curLongitude+"&latitude="+vm.curLatitude;
    }
    function addLocalAddress(){
        vm.localAddressList.unshift({longitude: vm.curLongitude, latitude: vm.curLatitude,address:vm.curAddress});
        if(vm.curSelectAddressIndex>=0) vm.curSelectAddressIndex = vm.curSelectAddressIndex + 1;
        localStorage.setItem('addressList',JSON.stringify(vm.localAddressList));
    }
    function deleteLocalAddress(index){
        if(vm.curSelectAddressIndex>=0 ){
            if(vm.curSelectAddressIndex == index) {
                vm.curSelectAddressIndex = -1;
            }
            else if(vm.curSelectAddressIndex>index){
                vm.curSelectAddressIndex = vm.curSelectAddressIndex-1;
            }
        }
        vm.localAddressList.splice(index,1);
        localStorage.setItem('addressList',JSON.stringify(vm.localAddressList));
    }
    function selectLocalAddress(index){
        vm.curSelectLogIndex = index;
    }
    <%}%>
    <%if (!string.IsNullOrWhiteSpace(typeConfig.ShareTitle))
        {%>
    function WXInit() {
        $.ajax({
            url: "http://" + location.host + "/serv/wxapi.ashx",
            data: {
                action: "getjsapiconfig",
                url: location.href
            },
            dataType: "json",
            success: function (wxapidata) {
                if (wxapidata.appId && wxapidata.timestamp &&
                        wxapidata.nonceStr && wxapidata.signature) {
                    wx.config({
                        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                        appId: wxapidata.appId, // 必填，公众号的唯一标识
                        timestamp: wxapidata.timestamp, // 必填，生成签名的时间戳
                        nonceStr: wxapidata.nonceStr, // 必填，生成签名的随机串
                        signature: wxapidata.signature,// 必填，签名，见附录1
                        jsApiList: [
                            "onMenuShareTimeline",
                            "onMenuShareAppMessage",
                            "onMenuShareQQ",
                            "onMenuShareWeibo",
                            "startRecord",
                            "stopRecord",
                            "onVoiceRecordEnd",
                            "playVoice",
                            "pauseVoice",
                            "stopVoice",
                            "onVoicePlayEnd",
                            "uploadVoice",
                            "downloadVoice",
                            "chooseImage",
                            "previewImage",
                            "uploadImage",
                            "downloadImage",
                            "translateVoice",
                            "getNetworkType",
                            "openLocation",
                            "getLocation",
                            "hideOptionMenu",
                            "showOptionMenu",
                            "hideMenuItems",
                            "showMenuItems",
                            "hideAllNonBaseMenuItem",
                            "showAllNonBaseMenuItem",
                            "closeWindow",
                            "scanQRCode",
                            "chooseWXPay",
                            "openProductSpecificView",
                            "addCard",
                            "chooseCard",
                            "openCard"
                        ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
                    });
                    WXReadly();
                }
            },
            error: function (errmes) {
            }
        })
    }
    function WXReadly() {
        //重新配置
        wx.ready(function () {
            //重写朋友分享
            wx.onMenuShareTimeline({
                title: vm.shareInfo.title, // 分享标题
                link: vm.shareInfo.link, // 分享链接
                imgUrl: vm.shareInfo.imgUrl, // 分享图标
                success: function () {
                },
                cancel: function () {
                }
            });
            //重写朋友圈分享
            wx.onMenuShareAppMessage({
                title: vm.shareInfo.title, // 分享标题
                desc: vm.shareInfo.desc, // 分享描述
                link: vm.shareInfo.link, // 分享链接
                imgUrl: vm.shareInfo.imgUrl, // 分享图标
            
                
                type: vm.shareInfo.type, // 分享类型,music、video或link，不填默认为link
                dataUrl: vm.shareInfo.dataUrl, // 如果type是music或video，则要提供数据链接，默认为空
                success: function () {
                },
                cancel: function () {
                }
            });
            //重写QQ分享
            wx.onMenuShareQQ({
                title: vm.shareInfo.title, // 分享标题
                desc: vm.shareInfo.desc, // 分享描述
                link: vm.shareInfo.link, // 分享链接
                imgUrl: vm.shareInfo.imgUrl, // 分享图标
                success: function () {
                },
                cancel: function () {
                }
            });
            //重写微博分享
            wx.onMenuShareWeibo({
                title: vm.shareInfo.title, // 分享标题
                desc: vm.shareInfo.desc, // 分享描述
                link: vm.shareInfo.link, // 分享链接
                imgUrl: vm.shareInfo.imgUrl, // 分享图标
                success: function () {
                },
                cancel: function () {
                }
            });
        });
    }
    <%}%>
    <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
    function getLocation() {
        var sessionCurAddress = sessionStorage.getItem('curAddress');
        if(sessionCurAddress == null){
            AMap.plugin(['AMap.Geolocation'], function () {
                vm.geolocation = new AMap.Geolocation({
                    enableHighAccuracy: true,//是否使用高精度定位，默认:true
                    convert: true,           //自动偏移坐标，偏移后的坐标为高德坐标，默认：true
                    maximumAge: 0,           //定位结果缓存0毫秒，默认：0
                    timeout: 3000          //超过10秒后停止定位，默认：无穷大
                });
                AMap.event.addListener(vm.geolocation, 'complete', onComplete);//返回定位信息
                AMap.event.addListener(vm.geolocation, 'error', onError);      //返回定位出错信息
                vm.geolocation.getCurrentPosition();
            });
            AMap.service('AMap.Geocoder',function(){//回调函数
                //实例化Geocoder
                vm.geocoder = new AMap.Geocoder();
            })
        }
        else{
            var curAddressJson = JSON.parse(sessionCurAddress);
            vm.curAddress = curAddressJson.curAddress;
            vm.curLongitude = curAddressJson.curLongitude;
            vm.curLatitude = curAddressJson.curLatitude;
            vm.curLngLat = new AMap.LngLat(vm.curLongitude,vm.curLatitude);
            getDataList(true);
        }
    }
    function onComplete(data) {
        //解析定位结果
        vm.curLongitude = data.position.getLng();
        vm.curLatitude = data.position.getLat();
        vm.curLngLat = new AMap.LngLat(vm.curLongitude,vm.curLatitude);
        getCurAddress();
        getDataList(true);
    }
    function onError(data) {    //解析定位错误信息
        switch (data.info) {
            case 'PERMISSION_DENIED':
                alert('浏览器拒绝定位');
                break;
            case 'POSITION_UNAVAILBLE':
                alert('浏览器无法获取当前位置');
                break;
            case 'NOT_SUPPORTED':
                alert('当前浏览器不支持定位功能');
                break;
            case 'TIMEOUT':
                alert('定位超时');
                break;
            case 'UNKNOWN_ERROR':
            default:
                alert('未知错误');
                break;
        }
        getDataList(false);
    }

    function getCurAddress(){
        vm.geocoder.getAddress([vm.curLongitude, vm.curLatitude], function(status, result) {
            if (status === 'complete' && result.info === 'OK') {
                //获得了有效的地址信息:
                 vm.curAddress = result.regeocode.formattedAddress
            }else{
                //获取地址失败
                vm.curAddress = "当前地址";
            }
            var curAddressJson = {
                curAddress:vm.curAddress,
                curLongitude:vm.curLongitude,
                curLatitude:vm.curLatitude,
            }
            sessionStorage.setItem('curAddress',JSON.stringify(curAddressJson));
        });
    }
    <%}%>
    function scrollLoadData() {
        $(window).scroll(function () {
            //当内容滚动到底部时加载新的内容
            if (document.body.scrollTop + $(window).height() >= $('.wrapOffer').height()) {
                //判断当没有数据的时候不加载
                if (vm.total > vm.list.length) {
                    getDataList(false);
                }
            }
        });
    }
    function getDataList(isNew) {
        if (vm.layerIndex == -1) {
            vm.layerIndex = layer.open({type: 2, shadeClose: false});
        }
        if (isNew) {
            vm.page = 1;
            vm.list = [];
            if (vm.first == 0) {
                vm.first = 1;
                scrollLoadData();
            }
        }
        else {
            vm.page++;
        }
        var dataobj = {
            type: 'Post',
            url: '/serv/api/outlets/comm/list.ashx',
            data: {
                type: vm.type,
                page: vm.page,
                rows: vm.rows,
                
    <%if (searchField.Count > 0)
        {%>
                
    <%if (searchField.Exists(p => p.Field.Equals("CategoryId")))
        {%>
                CategoryId: vm.curCate,
        <%}%>
        <%if (searchField.Exists(p => p.Field.Equals("Tags")))
        {%>
                Tags: vm.curTag,
    <%}%>
                <%
            foreach (var item in searchField.Where(p=>!searchOthers.Contains(p.Field)))
            { %>
                <%=item.Field%>:vm.cur<%=item.Field%>,
        <%}%>

    <%}%>
    <%if (!string.IsNullOrWhiteSpace(typeConfig.Ex2))
        {%>
                keyword: vm.keyword,
    <%}%>
    <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
                longitude: vm.curLongitude,
                latitude: vm.curLatitude,
    <%}%>
                rd:0
            }
        };
        ajaxReq(dataobj, function (data) {
            layer.close(vm.layerIndex);
            vm.layerIndex = -1;
            if (data.status) {
                vm.total = data.result.totalcount;
                for (var i = 0; i < data.result.list.length; i++) {
                    //row.ThumbnailsPath @140h_140w_1e_1c
                    vm.list.push(data.result.list[i]);
                }
            } else {
                alert('加载网点失败');
            }
        }, function (data) {
            layer.close(vm.layerIndex);
            vm.layerIndex = -1;
            alert('加载网点失败');
        }, false)

    }
    //类型
    function getCateList() {
        var dataobj = {
            type: 'Post',
            url: '/serv/api/article/category/selectlist.ashx',
            data: {
                type: vm.type
            }
        };
        ajaxReq(dataobj, function (data) {
            if (data.status) {
                vm.cateList = vm.cateList.concat(data.result);
            } else {
                alert('加载类型失败');
            }
        }, function (data) {
            alert('加载类型失败');
        }, false)
    }
    //获取标签
    function getTagList() {
        var dataobj = {
            type: 'Post',
            url: '/serv/api/mall/tag/list.ashx',
            data: {
                tag_type: vm.type
            }
        };
        ajaxReq(dataobj, function (data) {
            if (data.status) {
                for (var i = 0; i < data.result.list.length; i++) {
                    vm.tagList.push({value: data.result.list[i].tag_name, text: data.result.list[i].tag_name});
                }
            } else {
                alert('加载业务类型失败');
            }
        }, function (data) {
            alert('加载业务类型失败');
        }, false)
    }
    <%if (typeConfig.TimeSetMethod == 1 || typeConfig.TimeSetMethod == 2)
        {%>
    function showFilterAddressPanel(){
        if(vm.filterAddressShowNum == 1){
            hideFilterAddressPanel();
            return;
        }
        $("html").addClass("sidebar-move");
        vm.filterAddressShowNum = 1;
    }
    function hideFilterAddressPanel(){
        $("html").removeClass("sidebar-move");
        if(vm.curSelectLogIndex != vm.curSelectAddressIndex) vm.curSelectLogIndex = vm.curSelectAddressIndex;
        vm.filterAddressShowNum = 0;
    }
    function confrimSelectLogAddress(){
        if(vm.curSelectLogIndex!=-1 && vm.curSelectLogIndex < vm.localAddressList.length){
           var nLocalAddress =  vm.localAddressList[vm.curSelectLogIndex];
            vm.curLongitude = nLocalAddress.longitude;
            vm.curLatitude = nLocalAddress.latitude;
            vm.curAddress = nLocalAddress.address;
            vm.curSelectAddressIndex = vm.curSelectLogIndex;
            getDataList(true);
        }
        hideFilterAddressPanel();
    }
    function closeSelectLogAddress(){
        hideFilterAddressPanel();
    }
    <%}%>
    function openFilterPanel(num) {
        if(vm.filterListShowNum == num){
            hideFilterPanel();
            return;
        }
        $("html").addClass("sidebar-move");
        vm.filterListShowNum = num;
    }
    function hideFilterPanel(){
        $("html").removeClass("sidebar-move");
        vm.filterListShowNum = 0;
    }
    function setFilter(num, item, field) {
        if (field == "CategoryId") {
            vm.curCate = item.value;
            vm.curCateText = item.text;
        }
        else if (field == "Tags") {
            vm.curArea = item.value;
            vm.curAreaText = item.text;
        }
        else{
            vm['cur' + field] = item.value;
            vm['cur' + field + 'Text'] = item.text;
        }
        getDataList(true);
        $("html").removeClass("sidebar-move");
        vm.filterListShowNum = 0;
    }
    function goInfo(row) {
        <%if (typeConfig.TimeSetMethod == 2) {%>
            if(row.UserLongitude =="") return;
            var strHtml = new StringBuilder();
            strHtml.AppendFormat('http://m.amap.com/navi/?dest={0},{1}', row.UserLongitude, row.UserLatitude)
            strHtml.AppendFormat('&destName={0}', encodeURIComponent(row.ActivityName));
            if(row.ActivityPhone && row.ActivityPhone!="null")strHtml.AppendFormat('||电话：{0}', encodeURIComponent(row.ActivityPhone));
            if(row.ActivityAddress && row.ActivityAddress!="null")strHtml.AppendFormat('||地址：{0}', encodeURIComponent(row.ActivityAddress));
            strHtml.AppendFormat('&key={0}',zymmp.gaodeCompentKey);
            window.location.href = strHtml.ToString();
        <%} else {%>
        window.location.href = "Info.aspx?type="+vm.type+"&id=" + row.JuActivityID+"&title="+ encodeURIComponent(row.ActivityName);
        <%}%>
    }
    function toTel(row){
        if(row.ActivityPhone && row.ActivityPhone!="null") {
            window.location.href = "tel:" + row.ActivityPhone;
        }
        else{
            goInfo(row);
        }
    }
</script>
