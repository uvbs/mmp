<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreMap.aspx.cs" Inherits="ZentCloud.JubitIMP.Web.App.Outlets.StoreMap" %>
<!DOCTYPE html>
<html lang="en" ng-app="storeMap">
<head>
	<meta charset="UTF-8">
	<meta name="description" content="">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1, user-scalable=no, width=device-width">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-app-status-bar-style" content="white">
    <meta name="format-detection" content="telephone=no">
    <meta name="full-screen" content="yes">
    <meta name="x5-fullscreen" content="true">
    <meta name="renderer" content="webkit"/>
	<title>EICHTOO爱居兔</title>
	<link rel="stylesheet" type="text/css" href="/plugins/layerm/need/layer.css">
	<link rel="stylesheet" type="text/css" href="all.css">
	<link rel="stylesheet" href="http://cache.amap.com/lbs/static/main1119.css"/>
	<style type="text/css">
		*{ padding: 0; margin: 0; }
	    #tip{ z-index: 2; right: 0; }
	    .theme {

            background:<%=currWebSiteInfo.ThemeColor%> !important;
           
	    }
	    .theme-boder {
             border: 1px solid <%=currWebSiteInfo.ThemeColor%> !important;
	    }
	</style>
</head>
<body ng-controller="mapController">
<div class="map">
	
	<div class="header theme">
		
        <div>
		<em ng-class="{ 'active' : vm.currentTab== 1 }"  ng-click="vf.setCurrentTab(1)">门店地图</em>
		<em ng-class="{ 'active' : vm.currentTab == 2 }" ng-click="vf.setCurrentTab(2)">门店列表</em>
        </div>

		<div class="headerCity">

			<div ng-click="vf.selectCity()" class="address">
                <span><img src="images/address.png" class="
                icon-address"></span>
                <span ng-bind="vm.currentCity"></span>
                
                </div>

		</div>
	</div>
    			<div class="select-city" ng-show="vm.isShowSelectCity">
                <div>
				<select  ng-model="vm.province" ng-options="province.name for province in vm.provinceList" ng-change="vf.getCity();">
                </select>
                </div>
                <div>
				<select ng-model="vm.city" ng-options="city.name for city in vm.cityList"></select>
               </div>
                <div>
                    <button ng-click="vf.setPostion()" class="select-city-button theme"> 确定 </button>
                </div>
                
			</div>
	<div class="container" id="container" tabindex="0" ng-show="vm.currentTab== 1"></div>
     <div class="store-info" id="divStoreInfo" style="display:none;" >

     <div class="store-info-top">
     <div class="store-info-left">
         <div class="sotre-info-left-name" id="txtStoreName"></div>
         <div class="sotre-info-left-address" id="txtStoreAddress"></div>
     </div>
     <div class="store-info-right">
         <div class="store-info-right-title"> 距离目的地
         </div>
        <div class="store-info-right-range"><label class="range-number" id="lblDistance"></label></div>

     </div>
     </div>
     <div class="store-info-bottom">

         <div class="store-info-bottom-button" ng-click="vf.goProductList(vm.storeInfo,false)">确定</div>

     </div>
    </div>
   
	<div class="storeMapList" ng-show="vm.currentTab == 2">
		<ul>
			<li ng-repeat="item in vm.storeMapList" ng-click="vf.goProductList(item,true);">
                <span class="storeName">{{item.title}} </span>
                
				<span class="storeAddress"> {{item.address}}</span>
			</li>
		</ul>
	</div>
</div>
</body>
    <script src="/Scripts/jquery2.1.1.js"></script>
	<script type="text/javascript" src="js/angular.js"></script>
	<script type="text/javascript" src="storeMap.js"></script>
	<script type="text/javascript" src="http://webapi.amap.com/maps?v=1.3&key=<%=AMapKey %>"></script>
	<script type="text/javascript" src="http://comeoncloud.comeoncloud.net/plugins/layerm/layer.m.js"></script>


</html>