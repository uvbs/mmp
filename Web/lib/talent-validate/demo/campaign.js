 function searchCampaignByName(){
	 var title=document.getElementById("campaignTitle").value;
	 self.location.href="/"+SERVERNAME+"/Campaign/searchCampaignView.do?title="+title;
	 
 }

function searchCampaignUrl(){
	 self.location.href="/"+SERVERNAME+"/Campaign/searchCampaignView.do";
	 
 }
 
 function addCampaignUrl(){
	 self.location.href="/"+SERVERNAME+"/Campaign/addCampaignView.do";
	 
 }
 
 function editCampaignById(campaignId){
		
	 if(confirm("are you sure edit ?")){
		 self.location.href="/"+SERVERNAME+"/Campaign/editCampaignView.do?campaignId="+campaignId;
	 }
 }
 
 function saveCampaignById(){
	 if(confirm("are you sure save ?")){
		 self.location.href="/"+SERVERNAME+"/Campaign/searchCampaignView.do";
	 }
 }
 
 
 function delCampaignById(campaignId,channelId){
	 if(confirm("are you sure delete ?")){
		 self.location.href="/"+SERVERNAME+"/Campaign/delCampaign.do?campaignId="+campaignId+"&channelId="+channelId;
	 }
 }
 
 function addCampaignRow(){
	var newTr = placeTable.insertRow();

	var i = placeTable.rows.length-2;
	var delId = i+1;
	var listplace = "campaignPlaceList.place";
	var listopentime = "campaignPlaceList.openTime";
	var listclosetime = "campaignPlaceList.closeTime";
	
	var newTd0 = newTr.insertCell(); 
	var newTd1 = newTr.insertCell();
	var newTd2 = newTr.insertCell();
	var newTd3 = newTr.insertCell();

	newTd0.innerHTML = "<input name='"+listplace+"' type='text' value='' style='width:300px'/>";
	newTd1.innerHTML = "<input name='"+listopentime+"' type='text' value='' style='width:150px'/>"; 
	newTd2.innerHTML = "<input name='"+listclosetime+"' type='text' value='' style='width:150px'/>"; 
	newTd3.innerHTML = "<A onclick='javascript:deleteCampaignRow("+delId+")'>Delete</A>";
	insertValidation(newTr,i,listplace,listopentime,listclosetime);
	tt.vf.resizeWindow();
	return false;
 }
 
 function insertValidation(newTr,index,place,opentime,closetime) {	 
	 tt.vf.req.rm(place,opentime,closetime);
	 tt.vf.req.add(place,opentime,closetime);
 }
 
 function deleteCampaignRow(){
	 var currRowIndex=event.srcElement.parentNode.parentNode.rowIndex;
	 if(confirm("are you sure delete ?")){
		 
		 var listplace = "campaignPlaceList.place";
		 var listopentime = "campaignPlaceList.openTime";
	     var listclosetime = "campaignPlaceList.closeTime";
	     
		 placeTable.deleteRow(currRowIndex);
		 tt.vf.req.rm(listplace,listopentime,listclosetime);
	 	 tt.vf.req.add(listplace,listopentime,listclosetime);
	 }
 }
 
 var ftitle = new tt.Field("Title: ","title");//.setMsgId("msgId");
     var ficon = new tt.Field("Icon: ","icon");//.setMsgId("msgId");
     var fdisplayDateStart = new tt.Field("Display Date(S): ","startDisplayDateString");//.setMsgId("msgId");
     var fdisplayDateEnd = new tt.Field("Display Date(E): ","endDisplayDateString");//.setMsgId("msgId");
     var fstartDate = new tt.Field("Start Date: ","startDateString");//.setMsgId("msgId");
     var fendDate = new tt.Field("End Date: ","endDateString");//.setMsgId("msgId");
     var fchannelName = new tt.Field("Channel Name: ","channelId");//.setMsgId("msgId");
     
     tt.vf.req.add(ftitle,ficon,fdisplayDateStart,fdisplayDateEnd,fstartDate,fendDate,fchannelName);
     
     new tt.DV().set("yyyy-mm-dd").add(fdisplayDateStart,fdisplayDateEnd,fstartDate,fendDate);
 