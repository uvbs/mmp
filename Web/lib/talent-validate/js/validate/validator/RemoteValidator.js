tt.RemoteV = tt.BV.ext({
	init:function(){
		this._super();
		this.vType = 'r';
		this.rmId = this.rm;
	},
	v:function()
	{
		var thisObj = this;
		
		var ajaxConf = this.ajaxConf;
		
		var postData = [];
		//[{name:conf.keyConfig.pageIndex, value: pageIndex},{name:conf.keyConfig.pageSize, value:pageSize}];
		if (ajaxConf.form) {
			postData.push($(ajaxConf.form).serialize());
		}
		if (ajaxConf.formId) {
			postData.push($("#" + ajaxConf.formId).serialize());
		}
		if (ajaxConf.postData) {
			var data;
			for (var i = 0; i < ajaxConf.postData.length; i++) {
				data = ajaxConf.postData[i];
				postData.push(data.name + "=" + encodeURIComponent(data.value));
			}
		}
		var postDataStr = postData.join("&");
		
		ajaxConf.data = postDataStr;
		
		$.ajax(ajaxConf);
		
		return true;
	},
	rm:function() {
		tt.vf.rm([this]);
		tt.vf.rm(this.vs);
	},
	add:function() {
		tt.vf.add(this);
		return this;
	},
	/**
	 * 
	 * @param {} ajaxConf 形如：
	 * {
  		url:"xx.do",       //请求的url
  		form: formElement, //要提交的form
  		type:"post",    //post/get。默认为post
  		async:false,    //true/false。默认为true
  		postData:[{name:"e", value:"ee"},{name:"w", value:"ww"}]
	  }
	 */
	set:function(ajaxConf)
	{
		ajaxConf.async = false;
		ajaxConf.type = ajaxConf.type ? ajaxConf.type : "POST";
	    ajaxConf.dataType = "json";
		ajaxConf.thisObj = this;
		ajaxConf.complete =  function(_data) {
	        //tt_complete();       //关闭进度条
			tt.vf.rm(ajaxConf.thisObj.vs);
			var responseObj;
			if (!_data.responseText){
				ajaxConf.thisObj.setI18(tt.ajaxError1); //"从服务器端获取数据失败!";
				return;
			}
			try {
				responseObj = eval("(" + _data.responseText + ")");
			} catch(e){
				ajaxConf.thisObj.setI18(tt.ajaxError2); //"服务器异常!";
				return;
			}
			
			/**
			 * {
			 * "name1":{'result':true, 'msg':'验证成功'},
			 * "name2":{'result':false, 'msg':'验证失败'}
			 * }
			 */
			ajaxConf.thisObj.vs = [];
			for (var item in responseObj) {
				var onlyShow = new tt.OnlyShow().setVType("ext");
				ajaxConf.thisObj.vs.push(onlyShow);
				onlyShow.result = responseObj[item].result;
				onlyShow.msg = responseObj[item].msg;
				var msgId = responseObj[item].msgId;
				
				var f1;
				if (item.indexOf("id:") != -1) {
					f1 = new tt.Field("", "", item.substring(3)).setMsgId(msgId);
				}
				else {
					f1 = new tt.Field("", item).setMsgId(msgId);
				}
				onlyShow.add(f1);
			}
	    };
		ajaxConf.beforeSend =  function(){};
		
		
		this.ajaxConf = ajaxConf;
		return this;
	}
});