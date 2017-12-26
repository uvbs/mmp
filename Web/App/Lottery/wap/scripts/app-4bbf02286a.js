!function(t){"use strict";function a(a,e){this.$el=t(a),this.options=e,this.init=!1,this.enabled=!0,this._generate()}a.prototype={_generate:function(){return t.support.canvas?(this.canvas=document.createElement("canvas"),this.ctx=this.canvas.getContext("2d"),"static"===this.$el.css("position")&&this.$el.css("position","relative"),this.$img=t('<img src=""/>').attr("crossOrigin","").css({position:"absolute",width:"100%",height:"100%"}),this.$scratchpad=t(this.canvas).css({position:"absolute",width:"100%",height:"100%"}),this.$scratchpad.bindMobileEvents(),this.$scratchpad.mousedown(t.proxy(function(a){return this.enabled?(this.canvasOffset=t(this.canvas).offset(),this.scratch=!0,void this._scratchFunc(a,"Down")):!0},this)).mousemove(t.proxy(function(t){this.scratch&&this._scratchFunc(t,"Move")},this)).mouseup(t.proxy(function(t){this.scratch&&(this.scratch=!1,this._scratchFunc(t,"Up"))},this)),this._setOptions(),this.$el.append(this.$img).append(this.$scratchpad),this.init=!0,void this.reset()):(this.$el.append("Canvas is not supported in this browser."),!0)},reset:function(){var a=this,e=Math.ceil(this.$el.innerWidth()),s=Math.ceil(this.$el.innerHeight()),n=window.devicePixelRatio||1;this.pixels=e*s,this.$scratchpad.attr("width",e).attr("height",s),this.canvas.setAttribute("width",e*n),this.canvas.setAttribute("height",s*n),this.ctx.scale(n,n),this.pixels=e*n*s*n,this.$img.hide(),this.options.bg&&("#"===this.options.bg.charAt(0)?this.$el.css("backgroundColor",this.options.bg):(this.$el.css("backgroundColor",""),this.$img.attr("src",this.options.bg))),this.options.fg&&("#"===this.options.fg.charAt(0)?(this.ctx.fillStyle=this.options.fg,this.ctx.beginPath(),this.ctx.rect(0,0,e,s),this.ctx.fill(),this.$img.show()):t(new Image).attr("src",this.options.fg).load(function(){a.ctx.drawImage(this,0,0,e,s),a.$img.show()}))},clear:function(){this.ctx.clearRect(0,0,Math.ceil(this.$el.innerWidth()),Math.ceil(this.$el.innerHeight()))},enable:function(t){this.enabled=t===!0?!0:!1},destroy:function(){this.$el.children().remove(),t.removeData(this.$el,"wScratchPad")},_setOptions:function(){var t,a;for(t in this.options)this.options[t]=this.$el.attr("data-"+t)||this.options[t],a="set"+t.charAt(0).toUpperCase()+t.substring(1),this[a]&&this[a](this.options[t])},setBg:function(){this.init&&this.reset()},setFg:function(){this.setBg()},setCursor:function(t){this.$el.css("cursor",t)},_scratchFunc:function(t,a){t.pageX=Math.floor(t.pageX-this.canvasOffset.left),t.pageY=Math.floor(t.pageY-this.canvasOffset.top),this["_scratch"+a](t),(this.options.realtime||"Up"===a)&&this.options["scratch"+a]&&this.options["scratch"+a].apply(this,[t,this._scratchPercent()])},_scratchPercent:function(){for(var t=0,a=this.ctx.getImageData(0,0,this.canvas.width,this.canvas.height),e=0,s=a.data.length;s>e;e+=4)0===a.data[e]&&0===a.data[e+1]&&0===a.data[e+2]&&0===a.data[e+3]&&t++;return t/this.pixels*100},_scratchDown:function(t){this.ctx.globalCompositeOperation="destination-out",this.ctx.lineJoin="round",this.ctx.lineCap="round",this.ctx.strokeStyle=this.options.color,this.ctx.lineWidth=this.options.size,this.ctx.beginPath(),this.ctx.arc(t.pageX,t.pageY,this.options.size/2,0,2*Math.PI,!0),this.ctx.closePath(),this.ctx.fill(),this.ctx.beginPath(),this.ctx.moveTo(t.pageX,t.pageY)},_scratchMove:function(t){this.ctx.lineTo(t.pageX,t.pageY),this.ctx.stroke()},_scratchUp:function(){this.ctx.closePath()}},t.support.canvas=document.createElement("canvas").getContext,t.fn.wScratchPad=function(e,s){function n(){var s=t.data(this,"wScratchPad");return s||(s=new a(this,t.extend(!0,{},e)),t.data(this,"wScratchPad",s)),s}if("string"==typeof e){var i,c=[],r=(void 0!==s?"set":"get")+e.charAt(0).toUpperCase()+e.substring(1),o=function(){i.options[e]&&(i.options[e]=s),i[r]&&i[r].apply(i,[s])},h=function(){return i[r]?i[r].apply(i,[s]):i.options[e]?i.options[e]:void 0},u=function(){i=t.data(this,"wScratchPad"),i&&(i[e]?i[e].apply(i,[s]):void 0!==s?o():c.push(h()))};return this.each(u),c.length?1===c.length?c[0]:c:this}return e=t.extend({},t.fn.wScratchPad.defaults,e),this.each(n)},t.fn.wScratchPad.defaults={size:5,bg:"#cacaca",fg:"#6699ff",realtime:!0,scratchDown:null,scratchUp:null,scratchMove:null,cursor:"crosshair"},t.fn.bindMobileEvents=function(){t(this).on("touchstart touchmove touchend touchcancel",function(t){var a=t.changedTouches||t.originalEvent.targetTouches,e=a[0],s="";switch(t.type){case"touchstart":s="mousedown";break;case"touchmove":s="mousemove",t.preventDefault();break;case"touchend":s="mouseup";break;default:return}var n=document.createEvent("MouseEvent");n.initMouseEvent(s,!0,!0,window,1,e.screenX,e.screenY,e.clientX,e.clientY,!1,!1,!1,!1,0,null),e.target.dispatchEvent(n)})}}(jQuery),function(){"use strict";angular.module("scratchCard",["ngAnimate","ngCookies","ngTouch","ngSanitize","ngMessages","ngAria","ui.router","clientapi"])}(),function(){"use strict";function t(t,a,e,s,n,i,c,r){function o(){f.myIsAward?(f.canvasbg="assets/images/bg.png",angular.element(".maincanvas h2").css({"padding-top":"16%"})):(f.canvasbg="assets/images/bg2.png",f.myAwardName="")}function h(t){f.scratchInfo=t,angular.element("body").css({"background-color":t.background_color}),i.share({title:t.name,desc:t.share_desc,imgUrl:t.share_img_url,link:window.location.href})}function u(){}function l(){t.userInfo=f.userInfo;var a=n.show({template:'<input type="text" ng-model="userInfo.user_truename" placeholder="姓名"><input type="tel" ng-model="userInfo.user_phone" placeholder="手机">',title:"请输入姓名跟手机",subTitle:"录入信息即可领奖",scope:t,buttons:[{text:"取消"},{text:"<b>提交</b>",type:"button-assertive",onTap:function(a){return t.userInfo.user_truename&&t.userInfo.user_phone?t.userInfo:(n.alert("姓名或手机没有填写"),void a.preventDefault())}}]});a.then(function(t){return c.userUpdate(t)}).then(function(t){f.myPageCash=2})["catch"](function(t){n.alert(JSON.stringify(t))})}function d(){f.dtime<=0||(f.nowTime=(new Date).getTime(),f.dtime=f.myStartTime-f.nowTime,e(function(){d()},1e3))}function m(t){var a=new RegExp("(^|&)"+t+"=([^&]*)(&|$)","i"),e=window.location.search.substr(1).match(a);return null!=e?unescape(e[2]):null}function g(){angular.element(".maincanvas").css({height:angular.element(".maincanvas").width()/2.8}).wScratchPad({size:20,bg:f.canvasbg,fg:f.canvasfg,realtime:!0,scratchUp:null,cursor:"crosshair",scratchDown:function(){f.isstart||(f.isstart=!0,p())},scratchMove:function(t,a){if(a>f.percent){if(this.clear(),"boolean"===f.myIsAward)return;e(function(){f.myIsAward=f.getdata.isAward})}}})}function p(){location.host.match("localhost")||location.host.match("192.168.1")?f.awardurl="assets/main.json":f.awardurl="/serv/awardapi.ashx?action=scratch&id="+m("id"),a({method:"get",url:f.awardurl}).success(function(t,a,e,s){f.getdata=t,f.myAwardName=t.awardName}).error(function(t,a,e,s){f.myAwardName="抽奖失败，请刷新后重新抽奖"})}var f=this;f.myAwardGameOver=s.myAwardGameOver,f.myStartTime=new Date(s.myStartTime).getTime(),f.myIsAward=s.myIsAward,f.myAwardName=s.myAwardName,f.myCashed=s.myCashed,f.myPageCash=s.myPageCash,f.nowTime=(new Date).getTime(),f.isstart=!1,f.canvasfg="assets/images/fg.png",f.canvasbg="#fff",f.percent=90,f.getAward=u,f.updateMyInfo=l,f.userInfo={user_truename:"",user_phone:""},f.myAwardGameOver?(f.isstart=!0,f.canvasfg="",f.myAwardName="对不起,活动已结束！"):f.nowTime<f.myStartTime?d():"boolean"==typeof f.myIsAward&&(o(),f.canvasfg="assets/images/fg2.png",f.isstart=!0,f.percent=100),f.myCashed&&angular.element(".maincanvas h2").css({"background-image":"url(assets/images/cashed.png)"});c.scratchInfo(m("id")).then(function(t){h(t),g()})["catch"](function(t){n.alert(JSON.stringify(t))});if(3===f.myPageCash){c.userInfo().then(function(t){f.userInfo.user_truename=t.truename,f.userInfo.user_phone=t.phone,t.truename&&t.phone&&(f.myPageCash=2)})["catch"](function(t){n.alert(JSON.stringify(t))})}}angular.module("scratchCard").controller("MainController",t),t.$inject=["$scope","$http","$timeout","scratchCardStatus","wPopup","wxAPI","nAPI","$q"]}(),function(){"use strict";function t(t){t.debug("runBlock end")}angular.module("scratchCard").run(t),t.$inject=["$log"]}(),function(){"use strict";function t(t,a){t.state("home",{url:"/",templateUrl:"app/main/main.html",controller:"MainController",controllerAs:"main"}),a.otherwise("/")}angular.module("scratchCard").config(t),t.$inject=["$stateProvider","$urlRouterProvider"]}(),function(){"use strict";angular.module("scratchCard").constant("scratchCardStatus",{myAwardGameOver:!1,myStartTime:"2015-11-26 10:00:00",myIsAward:!0,myAwardName:"二等奖",myCashed:!1,myPageCash:3})}(),function(){"use strict";function t(t){t.debugEnabled(!0)}angular.module("scratchCard").config(t),t.$inject=["$logProvider"]}(),angular.module("scratchCard").run(["$templateCache",function(t){t.put("app/main/main.html",'<div class="mainbox" style=""><img class="mainlogo" ng-src="{{main.scratchInfo.img_url}}" alt=""><div class="maincanvas"><h2 ng-bind="main.myAwardName"></h2><div class="gamenotstart" ng-if="main.dtime>0" ng-bind="main.dtime | date:\'HH:mm:ss\'"></div></div><span ng-if="main.myPageCash==2&&main.myIsAward" class="cashbtn" ng-click="main.getAward()">领奖(非客服勿点)</span> <span ng-if="main.myPageCash==3&&main.myIsAward" class="cashbtn" ng-click="main.updateMyInfo()">完善资料后才可领奖</span><div class="article container" ng-bind-html="main.scratchInfo.content"></div></div>')}]);
//# sourceMappingURL=../maps/scripts/app-4bbf02286a.js.map
