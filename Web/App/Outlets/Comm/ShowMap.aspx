<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowMap.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Outlets.Comm.ShowMap" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no, width=device-width">
    <title><%=Request["address"] %></title>
    <link href="http://file.comeoncloud.net/lib/ionic/ionic.css" rel="stylesheet"/>
    <link href="http://file.comeoncloud.net/lib/layer.mobile/need/layer.css" rel="stylesheet"/>
    <link href="http://file.comeoncloud.net/css/global-m.css" rel="stylesheet"/>
    <link href="/App/Outlets/Outlets.css" rel="stylesheet"/>
    <script src="http://file.comeoncloud.net/lib/zepto/zepto.min.js"></script>
    <script src="http://file.comeoncloud.net/lib/vue/vue.min.js"></script>
    <script src="http://file.comeoncloud.net/lib/layer.mobile/layer.m.js" charset="utf-8"></script>
    <script src="http://file.comeoncloud.net/Scripts/global-m.js" charset="utf-8"></script>
    <script src="http://file.comeoncloud.net/Scripts/StringBuilder.Min.js" charset="utf-8"></script>
    <script type="text/javascript" src="http://webapi.amap.com/maps?v=1.3&key=224bd7222ce22c01673ff105ffb93fda"></script>
</head>
<body v-bind:class="{wrapQQBrowser:isQQBrowser}">
<div class="wrapGaodeMap">
    <div class="wrapHeader">
        <div class="bar bar-header item-input-inset bgColor5BD5BE">
            <label class="item-input-wrapper keyword">
                <input type="text" v-model="keyword" placeholder="输入地址查询..."/>
                <i class="icon iconfont icon-sousuo" @click="searchPlace()"></i>
            </label>
        </div>
    </div>
    <div class="mapContainer" id="container">
    </div>
    <div class="wrapFooter">
        <div>
            <span>地址：</span><span v-text="address"></span> <button class="button button-small floatR" @click="selectCurMapAddress()">确定</button>
        </div>
    </div>
</div>
</body>
</html>
<script type="text/javascript">
    var vm;
    $(function () {
        vm = new Vue({
            el: 'body',
            data: {
                longitude: GetParm('longitude'),
                latitude: GetParm('latitude'),
                address: decodeURIComponent(GetParm('address')),
                keyword: null,
                map: null,
                isQQBrowser: navigator.userAgent && navigator.userAgent.indexOf('MQQBrowser') > 0,
                info: {},
                id: GetParm('id'),
                type: GetParm('type'),
                user_longitude: 0,
                user_latitude: 0,
                toMarker: null,
                toPlaceSearch:null,
                toGeocoder:null
            },
            methods: {
                init: init,
                searchPlace: searchPlace,
                selectCurMapAddress: selectCurMapAddress
            }
        });
        vm.init();
    });
    function init() {
        if (!vm.map) {
            vm.map = new AMap.Map('container', {
                zoom: 15,
                zoomEnable:true,
                center: [vm.longitude, vm.latitude]
            });

            AMap.event.addListener(vm.map, 'click', getLnglat); //点击事件
            AMap.service(["AMap.Geocoder","AMap.PlaceSearch"], function() { //加载地理编码
                vm.toGeocoder = new AMap.Geocoder({
                    city:'上海', //城市，默认：“全国”
                    radius:100,
                    extensions:'base'
                });
                vm.toPlaceSearch = new AMap.PlaceSearch({
                    city:'上海',
                    citylimit:true,
                    pageSize:1,
                    pageIndex:1,
                    extensions:'base'
                });
            });
            setToPoint();
        }
    }
    function selectCurMapAddress(){
        var sessionCurAddress = {curAddress:vm.address,curLongitude:vm.longitude,curLatitude:vm.latitude};
        sessionStorage.setItem('curAddress', JSON.stringify(sessionCurAddress));
        window.location.href="List.aspx?type="+vm.type;
    }
    function getLnglat(e) {
        if(vm.toMarker && vm.toMarker!=null) vm.toMarker.setMap();
        vm.longitude = e.lnglat.getLng();
        vm.latitude = e.lnglat.getLat();
        console.log(vm.toGeocoder);
        vm.toGeocoder.getAddress([vm.longitude,vm.latitude],function(status, result) {
            if (status === 'complete' && result.info === 'OK') {
                vm.address = result.regeocode.formattedAddress;
            }
            else{
                vm.address = '所选地址';
            }
        })
        vm.map.setCenter([vm.longitude, vm.latitude]);
        vm.toMarker = new AMap.Marker({
            position: [vm.longitude, vm.latitude]
        });
        vm.toMarker.setMap(vm.map);
    }
    function setToPoint() {
        if(vm.toMarker && vm.toMarker!=null) vm.toMarker.setMap();

        vm.map.setCenter([vm.longitude, vm.latitude]);
        vm.toMarker = new AMap.Marker({
            position: [vm.longitude, vm.latitude]
        });
        vm.toMarker.setMap(vm.map);
    }
    function searchPlace(){
        if(vm.keyword && vm.keyword!=null){
            vm.toPlaceSearch.search(vm.keyword,function(status,result){
                if (status === 'complete' && result.info === 'OK') {
                    var poiArr = result.poiList.pois;
                    vm.longitude = poiArr[0].location.getLng();
                    vm.latitude = poiArr[0].location.getLat();
                    vm.address = poiArr[0].name;
                    setToPoint();
                }
                else{
                    alert('地址未找到');
                }
            });
        }
    }
</script>
