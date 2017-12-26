/**
 * jQuery EasyUI 1.3
 * 
 * Licensed under the GPL terms
 * To use it on other terms please contact us
 *
 * Copyright(c) 2009-2012 stworthy [ stworthy@gmail.com ] 
 * 
 */
(function($){
var _1=0;
function _2(a,o){
for(var i=0,_3=a.length;i<_3;i++){
if(a[i]==o){
return i;
}
}
return -1;
};
function _4(a,o,id){
if(typeof o=="string"){
for(var i=0,_5=a.length;i<_5;i++){
if(a[i][o]==id){
a.splice(i,1);
return;
}
}
}else{
var _6=_2(a,o);
if(_6!=-1){
a.splice(_6,1);
}
}
};
function _7(_8,_9){
var _a=$.data(_8,"datagrid").options;
var _b=$.data(_8,"datagrid").panel;
if(_9){
if(_9.width){
_a.width=_9.width;
}
if(_9.height){
_a.height=_9.height;
}
}
if(_a.fit==true){
var p=_b.panel("panel").parent();
_a.width=p.width();
_a.height=p.height();
}
_b.panel("resize",{width:_a.width,height:_a.height});
};
function _c(_d){
var _e=$.data(_d,"datagrid").options;
var dc=$.data(_d,"datagrid").dc;
var _f=$.data(_d,"datagrid").panel;
var _10=_f.width();
var _11=_f.height();
var _12=dc.view;
var _13=dc.view1;
var _14=dc.view2;
var _15=_13.children("div.datagrid-header");
var _16=_14.children("div.datagrid-header");
var _17=_15.find("table");
var _18=_16.find("table");
_12.width(_10);
var _19=_15.children("div.datagrid-header-inner").show();
_13.width(_19.find("table").width());
if(!_e.showHeader){
_19.hide();
}
_14.width(_10-_13.outerWidth());
_13.children("div.datagrid-header,div.datagrid-body,div.datagrid-footer").width(_13.width());
_14.children("div.datagrid-header,div.datagrid-body,div.datagrid-footer").width(_14.width());
var hh;
_15.css("height","");
_16.css("height","");
_17.css("height","");
_18.css("height","");
hh=Math.max(_17.height(),_18.height());
_17.height(hh);
_18.height(hh);
_15.add(_16)._outerHeight(hh);
if(_e.height!="auto"){
var _1a=_11-_14.children("div.datagrid-header").outerHeight(true)-_14.children("div.datagrid-footer").outerHeight(true)-_f.children("div.datagrid-toolbar").outerHeight(true);
_f.children("div.datagrid-pager").each(function(){
_1a-=$(this).outerHeight(true);
});
_13.children("div.datagrid-body").height(_1a);
_14.children("div.datagrid-body").height(_1a);
}
_12.height(_14.height());
_14.css("left",_13.outerWidth());
};
function _1b(_1c,_1d,_1e){
var _1f=$.data(_1c,"datagrid").data.rows;
var _20=$.data(_1c,"datagrid").options;
var dc=$.data(_1c,"datagrid").dc;
if(!dc.body1.is(":empty")&&(!_20.nowrap||_20.autoRowHeight||_1e)){
if(_1d!=undefined){
var tr1=_20.finder.getTr(_1c,_1d,"body",1);
var tr2=_20.finder.getTr(_1c,_1d,"body",2);
_21(tr1,tr2);
}else{
var tr1=_20.finder.getTr(_1c,0,"allbody",1);
var tr2=_20.finder.getTr(_1c,0,"allbody",2);
_21(tr1,tr2);
if(_20.showFooter){
var tr1=_20.finder.getTr(_1c,0,"allfooter",1);
var tr2=_20.finder.getTr(_1c,0,"allfooter",2);
_21(tr1,tr2);
}
}
}
_c(_1c);
if(_20.height=="auto"){
var _22=dc.body1.parent();
var _23=dc.body2;
var _24=0;
var _25=0;
_23.children().each(function(){
var c=$(this);
if(c.is(":visible")){
_24+=c.outerHeight();
if(_25<c.outerWidth()){
_25=c.outerWidth();
}
}
});
if(_25>_23.width()){
_24+=18;
}
_22.height(_24);
_23.height(_24);
dc.view.height(dc.view2.height());
}
dc.body2.triggerHandler("scroll");
function _21(_26,_27){
for(var i=0;i<_27.length;i++){
var tr1=$(_26[i]);
var tr2=$(_27[i]);
tr1.css("height","");
tr2.css("height","");
var _28=Math.max(tr1.height(),tr2.height());
tr1.css("height",_28);
tr2.css("height",_28);
}
};
};
function _29(_2a,_2b){
function _2c(){
var _2d=[];
var _2e=[];
$(_2a).children("thead").each(function(){
var opt=$.parser.parseOptions(this,[{frozen:"boolean"}]);
$(this).find("tr").each(function(){
var _2f=[];
$(this).find("th").each(function(){
var th=$(this);
var col=$.extend({},$.parser.parseOptions(this,["field","align",{sortable:"boolean",checkbox:"boolean",resizable:"boolean"},{rowspan:"number",colspan:"number",width:"number"}]),{title:(th.html()||undefined),hidden:(th.attr("hidden")?true:undefined),formatter:(th.attr("formatter")?eval(th.attr("formatter")):undefined),styler:(th.attr("styler")?eval(th.attr("styler")):undefined)});
if(!col.align){
col.align="left";
}
if(th.attr("editor")){
var s=$.trim(th.attr("editor"));
if(s.substr(0,1)=="{"){
col.editor=eval("("+s+")");
}else{
col.editor=s;
}
}
_2f.push(col);
});
opt.frozen?_2d.push(_2f):_2e.push(_2f);
});
});
return [_2d,_2e];
};
var _30=$("<div class=\"datagrid-wrap\">"+"<div class=\"datagrid-view\">"+"<div class=\"datagrid-view1\">"+"<div class=\"datagrid-header\">"+"<div class=\"datagrid-header-inner\"></div>"+"</div>"+"<div class=\"datagrid-body\">"+"<div class=\"datagrid-body-inner\"></div>"+"</div>"+"<div class=\"datagrid-footer\">"+"<div class=\"datagrid-footer-inner\"></div>"+"</div>"+"</div>"+"<div class=\"datagrid-view2\">"+"<div class=\"datagrid-header\">"+"<div class=\"datagrid-header-inner\"></div>"+"</div>"+"<div class=\"datagrid-body\"></div>"+"<div class=\"datagrid-footer\">"+"<div class=\"datagrid-footer-inner\"></div>"+"</div>"+"</div>"+"</div>"+"</div>").insertAfter(_2a);
_30.panel({doSize:false});
_30.panel("panel").addClass("datagrid").bind("_resize",function(e,_31){
var _32=$.data(_2a,"datagrid").options;
if(_32.fit==true||_31){
_7(_2a);
setTimeout(function(){
if($.data(_2a,"datagrid")){
_33(_2a);
}
},0);
}
return false;
});
$(_2a).hide().appendTo(_30.children("div.datagrid-view"));
var cc=_2c();
var _34=_30.children("div.datagrid-view");
var _35=_34.children("div.datagrid-view1");
var _36=_34.children("div.datagrid-view2");
return {panel:_30,frozenColumns:cc[0],columns:cc[1],dc:{view:_34,view1:_35,view2:_36,header1:_35.children("div.datagrid-header").children("div.datagrid-header-inner"),header2:_36.children("div.datagrid-header").children("div.datagrid-header-inner"),body1:_35.children("div.datagrid-body").children("div.datagrid-body-inner"),body2:_36.children("div.datagrid-body"),footer1:_35.children("div.datagrid-footer").children("div.datagrid-footer-inner"),footer2:_36.children("div.datagrid-footer").children("div.datagrid-footer-inner")}};
};
function _37(_38){
var _39={total:0,rows:[]};
var _3a=_3b(_38,true).concat(_3b(_38,false));
$(_38).find("tbody tr").each(function(){
_39.total++;
var col={};
for(var i=0;i<_3a.length;i++){
col[_3a[i]]=$("td:eq("+i+")",this).html();
}
_39.rows.push(col);
});
return _39;
};
function _3c(_3d){
var _3e=$.data(_3d,"datagrid");
var _3f=_3e.options;
var dc=_3e.dc;
var _40=_3e.panel;
_40.panel($.extend({},_3f,{id:null,doSize:false,onResize:function(_41,_42){
setTimeout(function(){
if($.data(_3d,"datagrid")){
_c(_3d);
_66(_3d);
_3f.onResize.call(_40,_41,_42);
}
},0);
},onExpand:function(){
_1b(_3d);
_3f.onExpand.call(_40);
}}));
_3e.rowIdPrefix="datagrid-row-r"+(++_1);
_43(dc.header1,_3f.frozenColumns,true);
_43(dc.header2,_3f.columns,false);
_44();
dc.header1.add(dc.header2).css("display",_3f.showHeader?"block":"none");
dc.footer1.add(dc.footer2).css("display",_3f.showFooter?"block":"none");
if(_3f.toolbar){
if(typeof _3f.toolbar=="string"){
$(_3f.toolbar).addClass("datagrid-toolbar").prependTo(_40);
$(_3f.toolbar).show();
}else{
$("div.datagrid-toolbar",_40).remove();
var tb=$("<div class=\"datagrid-toolbar\"></div>").prependTo(_40);
for(var i=0;i<_3f.toolbar.length;i++){
var btn=_3f.toolbar[i];
if(btn=="-"){
$("<div class=\"datagrid-btn-separator\"></div>").appendTo(tb);
}else{
var _45=$("<a href=\"javascript:void(0)\"></a>");
_45[0].onclick=eval(btn.handler||function(){
});
_45.css("float","left").appendTo(tb).linkbutton($.extend({},btn,{plain:true}));
}
}
}
}else{
$("div.datagrid-toolbar",_40).remove();
}
$("div.datagrid-pager",_40).remove();
if(_3f.pagination){
var _46=$("<div class=\"datagrid-pager\"></div>");
if(_3f.pagePosition=="bottom"){
_46.appendTo(_40);
}else{
if(_3f.pagePosition=="top"){
_46.addClass("datagrid-pager-top").prependTo(_40);
}else{
var _47=$("<div class=\"datagrid-pager datagrid-pager-top\"></div>").prependTo(_40);
_46.appendTo(_40);
_46=_46.add(_47);
}
}
_46.pagination({pageNumber:_3f.pageNumber,pageSize:_3f.pageSize,pageList:_3f.pageList,onSelectPage:function(_48,_49){
_3f.pageNumber=_48;
_3f.pageSize=_49;
_46.pagination("refresh",{pageNumber:_48,pageSize:_49});
_13b(_3d);
}});
_3f.pageSize=_46.pagination("options").pageSize;
}
function _43(_4a,_4b,_4c){
if(!_4b){
return;
}
$(_4a).show();
$(_4a).empty();
var t=$("<table class=\"datagrid-htable\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody></tbody></table>").appendTo(_4a);
for(var i=0;i<_4b.length;i++){
var tr=$("<tr class=\"datagrid-header-row\"></tr>").appendTo($("tbody",t));
var _4d=_4b[i];
for(var j=0;j<_4d.length;j++){
var col=_4d[j];
var _4e="";
if(col.rowspan){
_4e+="rowspan=\""+col.rowspan+"\" ";
}
if(col.colspan){
_4e+="colspan=\""+col.colspan+"\" ";
}
var td=$("<td "+_4e+"></td>").appendTo(tr);
if(col.checkbox){
td.attr("field",col.field);
$("<div class=\"datagrid-header-check\"></div>").html("<input type=\"checkbox\"/>").appendTo(td);
}else{
if(col.field){
td.attr("field",col.field);
td.append("<div class=\"datagrid-cell\"><span></span><span class=\"datagrid-sort-icon\"></span></div>");
$("span",td).html(col.title);
$("span.datagrid-sort-icon",td).html("&nbsp;");
var _4f=td.find("div.datagrid-cell");
if(col.resizable==false){
_4f.attr("resizable","false");
}
if(col.width){
_4f._outerWidth(col.width);
col.boxWidth=parseInt(_4f[0].style.width);
}else{
col.auto=true;
}
_4f.css("text-align",(col.align||"left"));
col.cellClass="datagrid-cell-c"+_1+"-"+col.field.replace(".","-");
col.cellSelector="div."+col.cellClass;
}else{
$("<div class=\"datagrid-cell-group\"></div>").html(col.title).appendTo(td);
}
}
if(col.hidden){
td.hide();
}
}
}
if(_4c&&_3f.rownumbers){
var td=$("<td rowspan=\""+_3f.frozenColumns.length+"\"><div class=\"datagrid-header-rownumber\"></div></td>");
if($("tr",t).length==0){
td.wrap("<tr class=\"datagrid-header-row\"></tr>").parent().appendTo($("tbody",t));
}else{
td.prependTo($("tr:first",t));
}
}
};
function _44(){
dc.view.children("style").remove();
var ss=["<style type=\"text/css\">"];
var _50=_3b(_3d,true).concat(_3b(_3d));
for(var i=0;i<_50.length;i++){
var col=_51(_3d,_50[i]);
if(col&&!col.checkbox){
ss.push(col.cellSelector+" {width:"+col.boxWidth+"px;}");
}
}
ss.push("</style>");
$(ss.join("\n")).prependTo(dc.view);
};
};
function _52(_53){
var _54=$.data(_53,"datagrid");
var _55=_54.panel;
var _56=_54.options;
var dc=_54.dc;
var _57=dc.header1.add(dc.header2);
_57.find("td:has(div.datagrid-cell)").unbind(".datagrid").bind("mouseenter.datagrid",function(){
$(this).addClass("datagrid-header-over");
}).bind("mouseleave.datagrid",function(){
$(this).removeClass("datagrid-header-over");
}).bind("contextmenu.datagrid",function(e){
var _58=$(this).attr("field");
_56.onHeaderContextMenu.call(_53,e,_58);
});
_57.find("input[type=checkbox]").unbind(".datagrid").bind("click.datagrid",function(e){
if(_56.singleSelect&&_56.selectOnCheck){
return false;
}
if($(this).is(":checked")){
_d2(_53);
}else{
_da(_53);
}
e.stopPropagation();
});
var _59=_57.find("div.datagrid-cell");
_59.unbind(".datagrid").bind("click.datagrid",function(e){
if(e.pageX<$(this).offset().left+$(this).outerWidth()-5){
var _5a=$(this).parent().attr("field");
var col=_51(_53,_5a);
if(!col.sortable||_54.resizing){
return;
}
_56.sortName=_5a;
_56.sortOrder="desc";
var c="datagrid-sort-desc";
if($(this).hasClass(c)){
c="datagrid-sort-asc";
_56.sortOrder="asc";
}
_59.removeClass("datagrid-sort-asc datagrid-sort-desc");
$(this).addClass(c);
if(_56.remoteSort){
_13b(_53);
}else{
var _5b=$.data(_53,"datagrid").data;
_9d(_53,_5b);
}
_56.onSortColumn.call(_53,_56.sortName,_56.sortOrder);
}
}).bind("dblclick.datagrid",function(e){
if(e.pageX>$(this).offset().left+$(this).outerWidth()-5){
var _5c=$(this).parent().attr("field");
var col=_51(_53,_5c);
if(col.resizable==false){
return;
}
$(_53).datagrid("autoSizeColumn",_5c);
col.auto=false;
}
});
_59.each(function(){
$(this).resizable({handles:"e",disabled:($(this).attr("resizable")?$(this).attr("resizable")=="false":false),minWidth:25,onStartResize:function(e){
_54.resizing=true;
_57.css("cursor","e-resize");
if(!_54.proxy){
_54.proxy=$("<div class=\"datagrid-resize-proxy\"></div>").appendTo(dc.view);
}
_54.proxy.css({left:e.pageX-$(_55).offset().left-1,display:"none"});
setTimeout(function(){
if(_54.proxy){
_54.proxy.show();
}
},500);
},onResize:function(e){
_54.proxy.css({left:e.pageX-$(_55).offset().left-1,display:"block"});
return false;
},onStopResize:function(e){
_57.css("cursor","");
var _5d=$(this).parent().attr("field");
var col=_51(_53,_5d);
col.width=$(this).outerWidth();
col.boxWidth=parseInt(this.style.width);
col.auto=undefined;
_33(_53,_5d);
dc.view2.children("div.datagrid-header").scrollLeft(dc.body2.scrollLeft());
_54.proxy.remove();
_54.proxy=null;
if($(this).parents("div:first.datagrid-header").parent().hasClass("datagrid-view1")){
_c(_53);
}
_66(_53);
_56.onResizeColumn.call(_53,_5d,col.width);
setTimeout(function(){
_54.resizing=false;
},0);
}});
});
dc.body1.add(dc.body2).unbind().bind("mouseover",function(e){
var tr=$(e.target).closest("tr.datagrid-row");
if(!tr.length){
return;
}
var _5e=_5f(tr);
_56.finder.getTr(_53,_5e).addClass("datagrid-row-over");
e.stopPropagation();
}).bind("mouseout",function(e){
var tr=$(e.target).closest("tr.datagrid-row");
if(!tr.length){
return;
}
var _60=_5f(tr);
_56.finder.getTr(_53,_60).removeClass("datagrid-row-over");
e.stopPropagation();
}).bind("click",function(e){
var tt=$(e.target);
var tr=tt.closest("tr.datagrid-row");
if(!tr.length){
return;
}
var _61=_5f(tr);
if(tt.parent().hasClass("datagrid-cell-check")){
if(_56.singleSelect&&_56.selectOnCheck){
if(!_56.checkOnSelect){
_da(_53,true);
}
_bf(_53,_61);
}else{
if(tt.is(":checked")){
_bf(_53,_61);
}else{
_ca(_53,_61);
}
}
}else{
var row=_56.finder.getRow(_53,_61);
var td=tt.closest("td[field]",tr);
if(td.length){
var _62=td.attr("field");
_56.onClickCell.call(_53,_61,_62,row[_62]);
}
if(_56.singleSelect==true){
_b6(_53,_61);
}else{
if(tr.hasClass("datagrid-row-selected")){
_c2(_53,_61);
}else{
_b6(_53,_61);
}
}
_56.onClickRow.call(_53,_61,row);
}
e.stopPropagation();
}).bind("dblclick",function(e){
var tt=$(e.target);
var tr=tt.closest("tr.datagrid-row");
if(!tr.length){
return;
}
var _63=_5f(tr);
var row=_56.finder.getRow(_53,_63);
var td=tt.closest("td[field]",tr);
if(td.length){
var _64=td.attr("field");
_56.onDblClickCell.call(_53,_63,_64,row[_64]);
}
_56.onDblClickRow.call(_53,_63,row);
e.stopPropagation();
}).bind("contextmenu",function(e){
var tr=$(e.target).closest("tr.datagrid-row");
if(!tr.length){
return;
}
var _65=_5f(tr);
var row=_56.finder.getRow(_53,_65);
_56.onRowContextMenu.call(_53,e,_65,row);
e.stopPropagation();
});
dc.body2.bind("scroll",function(){
dc.view1.children("div.datagrid-body").scrollTop($(this).scrollTop());
dc.view2.children("div.datagrid-header,div.datagrid-footer").scrollLeft($(this).scrollLeft());
});
function _5f(tr){
if(tr.attr("datagrid-row-index")){
return parseInt(tr.attr("datagrid-row-index"));
}else{
return tr.attr("node-id");
}
};
};
function _66(_67){
var _68=$.data(_67,"datagrid").options;
var dc=$.data(_67,"datagrid").dc;
if(!_68.fitColumns){
return;
}
var _69=dc.view2.children("div.datagrid-header");
var _6a=0;
var _6b;
var _6c=_3b(_67,false);
for(var i=0;i<_6c.length;i++){
var col=_51(_67,_6c[i]);
if(_6d(col)){
_6a+=col.width;
_6b=col;
}
}
var _6e=_69.children("div.datagrid-header-inner").show();
var _6f=_69.width()-_69.find("table").width()-_68.scrollbarSize;
var _70=_6f/_6a;
if(!_68.showHeader){
_6e.hide();
}
for(var i=0;i<_6c.length;i++){
var col=_51(_67,_6c[i]);
if(_6d(col)){
var _71=Math.floor(col.width*_70);
_72(col,_71);
_6f-=_71;
}
}
if(_6f&&_6b){
_72(_6b,_6f);
}
_33(_67);
function _72(col,_73){
col.width+=_73;
col.boxWidth+=_73;
_69.find("td[field=\""+col.field+"\"] div.datagrid-cell").width(col.boxWidth);
};
function _6d(col){
if(!col.hidden&&!col.checkbox&&!col.auto){
return true;
}
};
};
function _74(_75,_76){
var _77=$.data(_75,"datagrid").options;
var dc=$.data(_75,"datagrid").dc;
if(_76){
_7(_76);
if(_77.fitColumns){
_c(_75);
_66(_75);
}
}else{
var _78=false;
var _79=_3b(_75,true).concat(_3b(_75,false));
for(var i=0;i<_79.length;i++){
var _76=_79[i];
var col=_51(_75,_76);
if(col.auto){
_7(_76);
_78=true;
}
}
if(_78&&_77.fitColumns){
_c(_75);
_66(_75);
}
}
function _7(_7a){
var _7b=dc.view.find("div.datagrid-header td[field=\""+_7a+"\"] div.datagrid-cell");
_7b.css("width","");
var col=$(_75).datagrid("getColumnOption",_7a);
col.width=undefined;
col.boxWidth=undefined;
col.auto=true;
$(_75).datagrid("fixColumnSize",_7a);
var _7c=Math.max(_7b.outerWidth(),_7d("allbody"),_7d("allfooter"));
_7b._outerWidth(_7c);
col.width=_7c;
col.boxWidth=parseInt(_7b[0].style.width);
$(_75).datagrid("fixColumnSize",_7a);
_77.onResizeColumn.call(_75,_7a,col.width);
function _7d(_7e){
var _7f=0;
_77.finder.getTr(_75,0,_7e).find("td[field=\""+_7a+"\"] div.datagrid-cell").each(function(){
var w=$(this).outerWidth();
if(_7f<w){
_7f=w;
}
});
return _7f;
};
};
};
function _33(_80,_81){
var _82=$.data(_80,"datagrid").options;
var dc=$.data(_80,"datagrid").dc;
var _83=dc.view.find("table.datagrid-btable,table.datagrid-ftable");
_83.css("table-layout","fixed");
if(_81){
fix(_81);
}else{
var ff=_3b(_80,true).concat(_3b(_80,false));
for(var i=0;i<ff.length;i++){
fix(ff[i]);
}
}
_83.css("table-layout","auto");
_84(_80);
setTimeout(function(){
_1b(_80);
_8c(_80);
},0);
function fix(_85){
var col=_51(_80,_85);
if(col.checkbox){
return;
}
var _86=dc.view.children("style")[0];
var _87=_86.styleSheet?_86.styleSheet:(_86.sheet||document.styleSheets[document.styleSheets.length-1]);
var _88=_87.cssRules||_87.rules;
for(var i=0,len=_88.length;i<len;i++){
var _89=_88[i];
if(_89.selectorText.toLowerCase()==col.cellSelector.toLowerCase()){
_89.style["width"]=col.boxWidth?col.boxWidth+"px":"auto";
break;
}
}
};
};
function _84(_8a){
var dc=$.data(_8a,"datagrid").dc;
var _8b=dc.body1.add(dc.body2).find("td.datagrid-td-merged>div.datagrid-cell");
_8b.css("width","").each(function(){
$(this)._outerWidth($(this).parent().width());
});
};
function _8c(_8d){
var dc=$.data(_8d,"datagrid").dc;
dc.view.find("div.datagrid-editable").each(function(){
var _8e=$(this);
var _8f=_8e.parent().attr("field");
var col=$(_8d).datagrid("getColumnOption",_8f);
_8e._outerWidth(col.width);
var ed=$.data(this,"datagrid.editor");
if(ed.actions.resize){
ed.actions.resize(ed.target,_8e.width());
}
});
};
function _51(_90,_91){
function _92(_93){
if(_93){
for(var i=0;i<_93.length;i++){
var cc=_93[i];
for(var j=0;j<cc.length;j++){
var c=cc[j];
if(c.field==_91){
return c;
}
}
}
}
return null;
};
var _94=$.data(_90,"datagrid").options;
var col=_92(_94.columns);
if(!col){
col=_92(_94.frozenColumns);
}
return col;
};
function _3b(_95,_96){
var _97=$.data(_95,"datagrid").options;
var _98=(_96==true)?(_97.frozenColumns||[[]]):_97.columns;
if(_98.length==0){
return [];
}
var _99=[];
function _9a(_9b){
var c=0;
var i=0;
while(true){
if(_99[i]==undefined){
if(c==_9b){
return i;
}
c++;
}
i++;
}
};
function _9c(r){
var ff=[];
var c=0;
for(var i=0;i<_98[r].length;i++){
var col=_98[r][i];
if(col.field){
ff.push([c,col.field]);
}
c+=parseInt(col.colspan||"1");
}
for(var i=0;i<ff.length;i++){
ff[i][0]=_9a(ff[i][0]);
}
for(var i=0;i<ff.length;i++){
var f=ff[i];
_99[f[0]]=f[1];
}
};
for(var i=0;i<_98.length;i++){
_9c(i);
}
return _99;
};
function _9d(_9e,_9f){
var _a0=$.data(_9e,"datagrid");
var _a1=_a0.options;
var dc=_a0.dc;
var _a2=_a0.selectedRows;
_9f=_a1.loadFilter.call(_9e,_9f);
_a0.data=_9f;
if(_9f.footer){
_a0.footer=_9f.footer;
}
if(!_a1.remoteSort){
var opt=_51(_9e,_a1.sortName);
if(opt){
var _a3=opt.sorter||function(a,b){
return (a>b?1:-1);
};
_9f.rows.sort(function(r1,r2){
return _a3(r1[_a1.sortName],r2[_a1.sortName])*(_a1.sortOrder=="asc"?1:-1);
});
}
}
if(_a1.view.onBeforeRender){
_a1.view.onBeforeRender.call(_a1.view,_9e,_9f.rows);
}
_a1.view.render.call(_a1.view,_9e,dc.body2,false);
_a1.view.render.call(_a1.view,_9e,dc.body1,true);
if(_a1.showFooter){
_a1.view.renderFooter.call(_a1.view,_9e,dc.footer2,false);
_a1.view.renderFooter.call(_a1.view,_9e,dc.footer1,true);
}
if(_a1.view.onAfterRender){
_a1.view.onAfterRender.call(_a1.view,_9e);
}
_a1.onLoadSuccess.call(_9e,_9f);
var _a4=$(_9e).datagrid("getPager");
if(_a4.length){
if(_a4.pagination("options").total!=_9f.total){
_a4.pagination("refresh",{total:_9f.total});
}
}
_1b(_9e);
dc.body2.triggerHandler("scroll");
_a5();
$(_9e).datagrid("autoSizeColumn");
function _a5(){
if(_a1.idField){
for(var i=0;i<_9f.rows.length;i++){
var row=_9f.rows[i];
if(_a6(row)){
_b1(_9e,row[_a1.idField]);
}
}
}
function _a6(row){
for(var i=0;i<_a2.length;i++){
if(_a2[i][_a1.idField]==row[_a1.idField]){
_a2[i]=row;
return true;
}
}
return false;
};
};
};
function _a7(_a8,row){
var _a9=$.data(_a8,"datagrid").options;
var _aa=$.data(_a8,"datagrid").data.rows;
if(typeof row=="object"){
return _2(_aa,row);
}else{
for(var i=0;i<_aa.length;i++){
if(_aa[i][_a9.idField]==row){
return i;
}
}
return -1;
}
};
function _ab(_ac){
var _ad=$.data(_ac,"datagrid").options;
var _ae=$.data(_ac,"datagrid").data;
if(_ad.idField){
return $.data(_ac,"datagrid").selectedRows;
}else{
var _af=[];
_ad.finder.getTr(_ac,"","selected",2).each(function(){
var _b0=parseInt($(this).attr("datagrid-row-index"));
_af.push(_ae.rows[_b0]);
});
return _af;
}
};
function _b1(_b2,_b3){
var _b4=$.data(_b2,"datagrid").options;
if(_b4.idField){
var _b5=_a7(_b2,_b3);
if(_b5>=0){
_b6(_b2,_b5);
}
}
};
function _b6(_b7,_b8,_b9){
var _ba=$.data(_b7,"datagrid");
var dc=_ba.dc;
var _bb=_ba.options;
var _bc=_ba.data;
var _bd=$.data(_b7,"datagrid").selectedRows;
if(_bb.singleSelect){
_be(_b7);
_bd.splice(0,_bd.length);
}
if(!_b9&&_bb.checkOnSelect){
_bf(_b7,_b8,true);
}
if(_bb.idField){
var row=_bb.finder.getRow(_b7,_b8);
(function(){
for(var i=0;i<_bd.length;i++){
if(_bd[i][_bb.idField]==row[_bb.idField]){
return;
}
}
_bd.push(row);
})();
}
_bb.onSelect.call(_b7,_b8,_bc.rows[_b8]);
var tr=_bb.finder.getTr(_b7,_b8).addClass("datagrid-row-selected");
if(tr.length){
var _c0=dc.view2.children("div.datagrid-header").outerHeight();
var _c1=dc.body2;
var top=tr.position().top-_c0;
if(top<=0){
_c1.scrollTop(_c1.scrollTop()+top);
}else{
if(top+tr.outerHeight()>_c1.height()-18){
_c1.scrollTop(_c1.scrollTop()+top+tr.outerHeight()-_c1.height()+18);
}
}
}
};
function _c2(_c3,_c4,_c5){
var _c6=$.data(_c3,"datagrid");
var dc=_c6.dc;
var _c7=_c6.options;
var _c8=_c6.data;
var _c9=$.data(_c3,"datagrid").selectedRows;
if(!_c5&&_c7.checkOnSelect){
_ca(_c3,_c4,true);
}
_c7.finder.getTr(_c3,_c4).removeClass("datagrid-row-selected");
var row=_c7.finder.getRow(_c3,_c4);
if(_c7.idField){
_4(_c9,_c7.idField,row[_c7.idField]);
}
_c7.onUnselect.call(_c3,_c4,row);
};
function _cb(_cc,_cd){
var _ce=$.data(_cc,"datagrid");
var _cf=_ce.options;
var _d0=_ce.data.rows;
var _d1=$.data(_cc,"datagrid").selectedRows;
if(!_cd&&_cf.checkOnSelect){
_d2(_cc,true);
}
_cf.finder.getTr(_cc,"","allbody").addClass("datagrid-row-selected");
if(_cf.idField){
for(var _d3=0;_d3<_d0.length;_d3++){
(function(){
var row=_d0[_d3];
for(var i=0;i<_d1.length;i++){
if(_d1[i][_cf.idField]==row[_cf.idField]){
return;
}
}
_d1.push(row);
})();
}
}
_cf.onSelectAll.call(_cc,_d0);
};
function _be(_d4,_d5){
var _d6=$.data(_d4,"datagrid");
var _d7=_d6.options;
var _d8=_d6.data.rows;
var _d9=$.data(_d4,"datagrid").selectedRows;
if(!_d5&&_d7.checkOnSelect){
_da(_d4,true);
}
_d7.finder.getTr(_d4,"","selected").removeClass("datagrid-row-selected");
if(_d7.idField){
for(var _db=0;_db<_d8.length;_db++){
_4(_d9,_d7.idField,_d8[_db][_d7.idField]);
}
}
_d7.onUnselectAll.call(_d4,_d8);
};
function _bf(_dc,_dd,_de){
var _df=$.data(_dc,"datagrid");
var _e0=_df.options;
var _e1=_df.data;
if(!_de&&_e0.selectOnCheck){
_b6(_dc,_dd,true);
}
var ck=_e0.finder.getTr(_dc,_dd).find("div.datagrid-cell-check input[type=checkbox]");
$.fn.prop?ck.prop("checked",true):ck.attr("checked",true);
_e0.onCheck.call(_dc,_dd,_e1.rows[_dd]);
};
function _ca(_e2,_e3,_e4){
var _e5=$.data(_e2,"datagrid");
var _e6=_e5.options;
var _e7=_e5.data;
if(!_e4&&_e6.selectOnCheck){
_c2(_e2,_e3,true);
}
var ck=_e6.finder.getTr(_e2,_e3).find("div.datagrid-cell-check input[type=checkbox]");
$.fn.prop?ck.prop("checked",false):ck.attr("checked",false);
_e6.onUncheck.call(_e2,_e3,_e7.rows[_e3]);
};
function _d2(_e8,_e9){
var _ea=$.data(_e8,"datagrid");
var _eb=_ea.options;
var _ec=_ea.data;
if(!_e9&&_eb.selectOnCheck){
_cb(_e8,true);
}
var _ed=_eb.finder.getTr(_e8,"","allbody").find("div.datagrid-cell-check input[type=checkbox]");
$.fn.prop?_ed.prop("checked",true):_ed.attr("checked",true);
_eb.onCheckAll.call(_e8,_ec.rows);
};
function _da(_ee,_ef){
var _f0=$.data(_ee,"datagrid");
var _f1=_f0.options;
var _f2=_f0.data;
if(!_ef&&_f1.selectOnCheck){
_be(_ee,true);
}
var _f3=_f1.finder.getTr(_ee,"","allbody").find("div.datagrid-cell-check input[type=checkbox]");
$.fn.prop?_f3.prop("checked",false):_f3.attr("checked",false);
_f1.onUncheckAll.call(_ee,_f2.rows);
};
function _f4(_f5,_f6){
var _f7=$.data(_f5,"datagrid").options;
var tr=_f7.finder.getTr(_f5,_f6);
var row=_f7.finder.getRow(_f5,_f6);
if(tr.hasClass("datagrid-row-editing")){
return;
}
if(_f7.onBeforeEdit.call(_f5,_f6,row)==false){
return;
}
tr.addClass("datagrid-row-editing");
_f8(_f5,_f6);
_8c(_f5);
tr.find("div.datagrid-editable").each(function(){
var _f9=$(this).parent().attr("field");
var ed=$.data(this,"datagrid.editor");
ed.actions.setValue(ed.target,row[_f9]);
});
_fa(_f5,_f6);
};
function _fb(_fc,_fd,_fe){
var _ff=$.data(_fc,"datagrid").options;
var _100=$.data(_fc,"datagrid").updatedRows;
var _101=$.data(_fc,"datagrid").insertedRows;
var tr=_ff.finder.getTr(_fc,_fd);
var row=_ff.finder.getRow(_fc,_fd);
if(!tr.hasClass("datagrid-row-editing")){
return;
}
if(!_fe){
if(!_fa(_fc,_fd)){
return;
}
var _102=false;
var _103={};
tr.find("div.datagrid-editable").each(function(){
var _104=$(this).parent().attr("field");
var ed=$.data(this,"datagrid.editor");
var _105=ed.actions.getValue(ed.target);
if(row[_104]!=_105){
row[_104]=_105;
_102=true;
_103[_104]=_105;
}
});
if(_102){
if(_2(_101,row)==-1){
if(_2(_100,row)==-1){
_100.push(row);
}
}
}
}
tr.removeClass("datagrid-row-editing");
_106(_fc,_fd);
$(_fc).datagrid("refreshRow",_fd);
if(!_fe){
_ff.onAfterEdit.call(_fc,_fd,row,_103);
}else{
_ff.onCancelEdit.call(_fc,_fd,row);
}
};
function _107(_108,_109){
var opts=$.data(_108,"datagrid").options;
var tr=opts.finder.getTr(_108,_109);
var _10a=[];
tr.children("td").each(function(){
var cell=$(this).find("div.datagrid-editable");
if(cell.length){
var ed=$.data(cell[0],"datagrid.editor");
_10a.push(ed);
}
});
return _10a;
};
function _10b(_10c,_10d){
var _10e=_107(_10c,_10d.index);
for(var i=0;i<_10e.length;i++){
if(_10e[i].field==_10d.field){
return _10e[i];
}
}
return null;
};
function _f8(_10f,_110){
var opts=$.data(_10f,"datagrid").options;
var tr=opts.finder.getTr(_10f,_110);
tr.children("td").each(function(){
var cell=$(this).find("div.datagrid-cell");
var _111=$(this).attr("field");
var col=_51(_10f,_111);
if(col&&col.editor){
var _112,_113;
if(typeof col.editor=="string"){
_112=col.editor;
}else{
_112=col.editor.type;
_113=col.editor.options;
}
var _114=opts.editors[_112];
if(_114){
var _115=cell.html();
var _116=cell.outerWidth();
cell.addClass("datagrid-editable");
cell._outerWidth(_116);
cell.html("<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\"><tr><td></td></tr></table>");
cell.children("table").attr("align",col.align);
cell.children("table").bind("click dblclick contextmenu",function(e){
e.stopPropagation();
});
$.data(cell[0],"datagrid.editor",{actions:_114,target:_114.init(cell.find("td"),_113),field:_111,type:_112,oldHtml:_115});
}
}
});
_1b(_10f,_110,true);
};
function _106(_117,_118){
var opts=$.data(_117,"datagrid").options;
var tr=opts.finder.getTr(_117,_118);
tr.children("td").each(function(){
var cell=$(this).find("div.datagrid-editable");
if(cell.length){
var ed=$.data(cell[0],"datagrid.editor");
if(ed.actions.destroy){
ed.actions.destroy(ed.target);
}
cell.html(ed.oldHtml);
$.removeData(cell[0],"datagrid.editor");
cell.removeClass("datagrid-editable");
cell.css("width","");
}
});
};
function _fa(_119,_11a){
var tr=$.data(_119,"datagrid").options.finder.getTr(_119,_11a);
if(!tr.hasClass("datagrid-row-editing")){
return true;
}
var vbox=tr.find(".validatebox-text");
vbox.validatebox("validate");
vbox.trigger("mouseleave");
var _11b=tr.find(".validatebox-invalid");
return _11b.length==0;
};
function _11c(_11d,_11e){
var _11f=$.data(_11d,"datagrid").insertedRows;
var _120=$.data(_11d,"datagrid").deletedRows;
var _121=$.data(_11d,"datagrid").updatedRows;
if(!_11e){
var rows=[];
rows=rows.concat(_11f);
rows=rows.concat(_120);
rows=rows.concat(_121);
return rows;
}else{
if(_11e=="inserted"){
return _11f;
}else{
if(_11e=="deleted"){
return _120;
}else{
if(_11e=="updated"){
return _121;
}
}
}
}
return [];
};
function _122(_123,_124){
var opts=$.data(_123,"datagrid").options;
var data=$.data(_123,"datagrid").data;
var _125=$.data(_123,"datagrid").insertedRows;
var _126=$.data(_123,"datagrid").deletedRows;
var _127=$.data(_123,"datagrid").selectedRows;
$(_123).datagrid("cancelEdit",_124);
var row=data.rows[_124];
if(_2(_125,row)>=0){
_4(_125,row);
}else{
_126.push(row);
}
_4(_127,opts.idField,data.rows[_124][opts.idField]);
opts.view.deleteRow.call(opts.view,_123,_124);
if(opts.height=="auto"){
_1b(_123);
}
};
function _128(_129,_12a){
var view=$.data(_129,"datagrid").options.view;
var _12b=$.data(_129,"datagrid").insertedRows;
view.insertRow.call(view,_129,_12a.index,_12a.row);
_12b.push(_12a.row);
};
function _12c(_12d,row){
var view=$.data(_12d,"datagrid").options.view;
var _12e=$.data(_12d,"datagrid").insertedRows;
view.insertRow.call(view,_12d,null,row);
_12e.push(row);
};
function _12f(_130){
var data=$.data(_130,"datagrid").data;
var rows=data.rows;
var _131=[];
for(var i=0;i<rows.length;i++){
_131.push($.extend({},rows[i]));
}
$.data(_130,"datagrid").originalRows=_131;
$.data(_130,"datagrid").updatedRows=[];
$.data(_130,"datagrid").insertedRows=[];
$.data(_130,"datagrid").deletedRows=[];
};
function _132(_133){
var data=$.data(_133,"datagrid").data;
var ok=true;
for(var i=0,len=data.rows.length;i<len;i++){
if(_fa(_133,i)){
_fb(_133,i,false);
}else{
ok=false;
}
}
if(ok){
_12f(_133);
}
};
function _134(_135){
var opts=$.data(_135,"datagrid").options;
var _136=$.data(_135,"datagrid").originalRows;
var _137=$.data(_135,"datagrid").insertedRows;
var _138=$.data(_135,"datagrid").deletedRows;
var _139=$.data(_135,"datagrid").selectedRows;
var data=$.data(_135,"datagrid").data;
for(var i=0;i<data.rows.length;i++){
_fb(_135,i,true);
}
var _13a=[];
for(var i=0;i<_139.length;i++){
_13a.push(_139[i][opts.idField]);
}
_139.splice(0,_139.length);
data.total+=_138.length-_137.length;
data.rows=_136;
_9d(_135,data);
for(var i=0;i<_13a.length;i++){
_b1(_135,_13a[i]);
}
_12f(_135);
};
function _13b(_13c,_13d){
var opts=$.data(_13c,"datagrid").options;
if(_13d){
opts.queryParams=_13d;
}
var _13e=$.extend({},opts.queryParams);
if(opts.pagination){
$.extend(_13e,{page:opts.pageNumber,rows:opts.pageSize});
}
if(opts.sortName){
$.extend(_13e,{sort:opts.sortName,order:opts.sortOrder});
}
if(opts.onBeforeLoad.call(_13c,_13e)==false){
return;
}
$(_13c).datagrid("loading");
setTimeout(function(){
_13f();
},0);
function _13f(){
var _140=opts.loader.call(_13c,_13e,function(data){
setTimeout(function(){
$(_13c).datagrid("loaded");
},0);
_9d(_13c,data);
setTimeout(function(){
_12f(_13c);
},0);
},function(){
setTimeout(function(){
$(_13c).datagrid("loaded");
},0);
opts.onLoadError.apply(_13c,arguments);
});
if(_140==false){
$(_13c).datagrid("loaded");
}
};
};
function _141(_142,_143){
var opts=$.data(_142,"datagrid").options;
var rows=$.data(_142,"datagrid").data.rows;
_143.rowspan=_143.rowspan||1;
_143.colspan=_143.colspan||1;
if(_143.index<0||_143.index>=rows.length){
return;
}
if(_143.rowspan==1&&_143.colspan==1){
return;
}
var _144=rows[_143.index][_143.field];
var tr=opts.finder.getTr(_142,_143.index);
var td=tr.find("td[field=\""+_143.field+"\"]");
td.attr("rowspan",_143.rowspan).attr("colspan",_143.colspan);
td.addClass("datagrid-td-merged");
for(var i=1;i<_143.colspan;i++){
td=td.next();
td.hide();
rows[_143.index][td.attr("field")]=_144;
}
for(var i=1;i<_143.rowspan;i++){
tr=tr.next();
var td=tr.find("td[field=\""+_143.field+"\"]").hide();
rows[_143.index+i][td.attr("field")]=_144;
for(var j=1;j<_143.colspan;j++){
td=td.next();
td.hide();
rows[_143.index+i][td.attr("field")]=_144;
}
}
_84(_142);
};
$.fn.datagrid=function(_145,_146){
if(typeof _145=="string"){
return $.fn.datagrid.methods[_145](this,_146);
}
_145=_145||{};
return this.each(function(){
var _147=$.data(this,"datagrid");
var opts;
if(_147){
opts=$.extend(_147.options,_145);
_147.options=opts;
}else{
opts=$.extend({},$.extend({},$.fn.datagrid.defaults,{queryParams:{}}),$.fn.datagrid.parseOptions(this),_145);
$(this).css("width","").css("height","");
var _148=_29(this,opts.rownumbers);
if(!opts.columns){
opts.columns=_148.columns;
}
if(!opts.frozenColumns){
opts.frozenColumns=_148.frozenColumns;
}
$.data(this,"datagrid",{options:opts,panel:_148.panel,dc:_148.dc,selectedRows:[],data:{total:0,rows:[]},originalRows:[],updatedRows:[],insertedRows:[],deletedRows:[]});
}
_3c(this);
if(!_147){
var data=_37(this);
if(data.total>0){
_9d(this,data);
_12f(this);
}
}
_7(this);
_13b(this);
_52(this);
});
};
var _149={text:{init:function(_14a,_14b){
var _14c=$("<input type=\"text\" class=\"datagrid-editable-input\">").appendTo(_14a);
return _14c;
},getValue:function(_14d){
return $(_14d).val();
},setValue:function(_14e,_14f){
$(_14e).val(_14f);
},resize:function(_150,_151){
$(_150)._outerWidth(_151);
}},textarea:{init:function(_152,_153){
var _154=$("<textarea class=\"datagrid-editable-input\"></textarea>").appendTo(_152);
return _154;
},getValue:function(_155){
return $(_155).val();
},setValue:function(_156,_157){
$(_156).val(_157);
},resize:function(_158,_159){
$(_158)._outerWidth(_159);
}},checkbox:{init:function(_15a,_15b){
var _15c=$("<input type=\"checkbox\">").appendTo(_15a);
_15c.val(_15b.on);
_15c.attr("offval",_15b.off);
return _15c;
},getValue:function(_15d){
if($(_15d).is(":checked")){
return $(_15d).val();
}else{
return $(_15d).attr("offval");
}
},setValue:function(_15e,_15f){
var _160=false;
if($(_15e).val()==_15f){
_160=true;
}
$.fn.prop?$(_15e).prop("checked",_160):$(_15e).attr("checked",_160);
}},numberbox:{init:function(_161,_162){
var _163=$("<input type=\"text\" class=\"datagrid-editable-input\">").appendTo(_161);
_163.numberbox(_162);
return _163;
},destroy:function(_164){
$(_164).numberbox("destroy");
},getValue:function(_165){
return $(_165).numberbox("getValue");
},setValue:function(_166,_167){
$(_166).numberbox("setValue",_167);
},resize:function(_168,_169){
$(_168)._outerWidth(_169);
}},validatebox:{init:function(_16a,_16b){
var _16c=$("<input type=\"text\" class=\"datagrid-editable-input\">").appendTo(_16a);
_16c.validatebox(_16b);
return _16c;
},destroy:function(_16d){
$(_16d).validatebox("destroy");
},getValue:function(_16e){
return $(_16e).val();
},setValue:function(_16f,_170){
$(_16f).val(_170);
},resize:function(_171,_172){
$(_171)._outerWidth(_172);
}},datebox:{init:function(_173,_174){
var _175=$("<input type=\"text\">").appendTo(_173);
_175.datebox(_174);
return _175;
},destroy:function(_176){
$(_176).datebox("destroy");
},getValue:function(_177){
return $(_177).datebox("getValue");
},setValue:function(_178,_179){
$(_178).datebox("setValue",_179);
},resize:function(_17a,_17b){
$(_17a).datebox("resize",_17b);
}},combobox:{init:function(_17c,_17d){
var _17e=$("<input type=\"text\">").appendTo(_17c);
_17e.combobox(_17d||{});
return _17e;
},destroy:function(_17f){
$(_17f).combobox("destroy");
},getValue:function(_180){
return $(_180).combobox("getValue");
},setValue:function(_181,_182){
$(_181).combobox("setValue",_182);
},resize:function(_183,_184){
$(_183).combobox("resize",_184);
}},combotree:{init:function(_185,_186){
var _187=$("<input type=\"text\">").appendTo(_185);
_187.combotree(_186);
return _187;
},destroy:function(_188){
$(_188).combotree("destroy");
},getValue:function(_189){
return $(_189).combotree("getValue");
},setValue:function(_18a,_18b){
$(_18a).combotree("setValue",_18b);
},resize:function(_18c,_18d){
$(_18c).combotree("resize",_18d);
}}};
$.fn.datagrid.methods={options:function(jq){
var _18e=$.data(jq[0],"datagrid").options;
var _18f=$.data(jq[0],"datagrid").panel.panel("options");
var opts=$.extend(_18e,{width:_18f.width,height:_18f.height,closed:_18f.closed,collapsed:_18f.collapsed,minimized:_18f.minimized,maximized:_18f.maximized});
var _190=jq.datagrid("getPager");
if(_190.length){
var _191=_190.pagination("options");
$.extend(opts,{pageNumber:_191.pageNumber,pageSize:_191.pageSize});
}
return opts;
},getPanel:function(jq){
return $.data(jq[0],"datagrid").panel;
},getPager:function(jq){
return $.data(jq[0],"datagrid").panel.children("div.datagrid-pager");
},getColumnFields:function(jq,_192){
return _3b(jq[0],_192);
},getColumnOption:function(jq,_193){
return _51(jq[0],_193);
},resize:function(jq,_194){
return jq.each(function(){
_7(this,_194);
});
},load:function(jq,_195){
return jq.each(function(){
var opts=$(this).datagrid("options");
opts.pageNumber=1;
var _196=$(this).datagrid("getPager");
_196.pagination({pageNumber:1});
_13b(this,_195);
});
},reload:function(jq,_197){
return jq.each(function(){
_13b(this,_197);
});
},reloadFooter:function(jq,_198){
return jq.each(function(){
var opts=$.data(this,"datagrid").options;
var dc=$.data(this,"datagrid").dc;
if(_198){
$.data(this,"datagrid").footer=_198;
}
if(opts.showFooter){
opts.view.renderFooter.call(opts.view,this,dc.footer2,false);
opts.view.renderFooter.call(opts.view,this,dc.footer1,true);
if(opts.view.onAfterRender){
opts.view.onAfterRender.call(opts.view,this);
}
$(this).datagrid("fixRowHeight");
}
});
},loading:function(jq){
return jq.each(function(){
var opts=$.data(this,"datagrid").options;
$(this).datagrid("getPager").pagination("loading");
if(opts.loadMsg){
var _199=$(this).datagrid("getPanel");
$("<div class=\"datagrid-mask\" style=\"display:block\"></div>").appendTo(_199);
var msg=$("<div class=\"datagrid-mask-msg\" style=\"display:block\"></div>").html(opts.loadMsg).appendTo(_199);
msg.css("left",(_199.width()-msg.outerWidth())/2);
}
});
},loaded:function(jq){
return jq.each(function(){
$(this).datagrid("getPager").pagination("loaded");
var _19a=$(this).datagrid("getPanel");
_19a.children("div.datagrid-mask-msg").remove();
_19a.children("div.datagrid-mask").remove();
});
},fitColumns:function(jq){
return jq.each(function(){
_66(this);
});
},fixColumnSize:function(jq,_19b){
return jq.each(function(){
_33(this,_19b);
});
},fixRowHeight:function(jq,_19c){
return jq.each(function(){
_1b(this,_19c);
});
},autoSizeColumn:function(jq,_19d){
return jq.each(function(){
_74(this,_19d);
});
},loadData:function(jq,data){
return jq.each(function(){
_9d(this,data);
_12f(this);
});
},getData:function(jq){
return $.data(jq[0],"datagrid").data;
},getRows:function(jq){
return $.data(jq[0],"datagrid").data.rows;
},getFooterRows:function(jq){
return $.data(jq[0],"datagrid").footer;
},getRowIndex:function(jq,id){
return _a7(jq[0],id);
},getChecked:function(jq){
var rr=[];
var rows=jq.datagrid("getRows");
var dc=$.data(jq[0],"datagrid").dc;
dc.view.find("div.datagrid-cell-check input:checked").each(function(){
var _19e=$(this).parents("tr.datagrid-row:first").attr("datagrid-row-index");
rr.push(rows[_19e]);
});
return rr;
},getSelected:function(jq){
var rows=_ab(jq[0]);
return rows.length>0?rows[0]:null;
},getSelections:function(jq){
return _ab(jq[0]);
},clearSelections:function(jq){
return jq.each(function(){
var _19f=$.data(this,"datagrid").selectedRows;
_19f.splice(0,_19f.length);
_be(this);
});
},selectAll:function(jq){
return jq.each(function(){
_cb(this);
});
},unselectAll:function(jq){
return jq.each(function(){
_be(this);
});
},selectRow:function(jq,_1a0){
return jq.each(function(){
_b6(this,_1a0);
});
},selectRecord:function(jq,id){
return jq.each(function(){
_b1(this,id);
});
},unselectRow:function(jq,_1a1){
return jq.each(function(){
_c2(this,_1a1);
});
},checkRow:function(jq,_1a2){
return jq.each(function(){
_bf(this,_1a2);
});
},uncheckRow:function(jq,_1a3){
return jq.each(function(){
_ca(this,_1a3);
});
},checkAll:function(jq){
return jq.each(function(){
_d2(this);
});
},uncheckAll:function(jq){
return jq.each(function(){
_da(this);
});
},beginEdit:function(jq,_1a4){
return jq.each(function(){
_f4(this,_1a4);
});
},endEdit:function(jq,_1a5){
return jq.each(function(){
_fb(this,_1a5,false);
});
},cancelEdit:function(jq,_1a6){
return jq.each(function(){
_fb(this,_1a6,true);
});
},getEditors:function(jq,_1a7){
return _107(jq[0],_1a7);
},getEditor:function(jq,_1a8){
return _10b(jq[0],_1a8);
},refreshRow:function(jq,_1a9){
return jq.each(function(){
var opts=$.data(this,"datagrid").options;
opts.view.refreshRow.call(opts.view,this,_1a9);
});
},validateRow:function(jq,_1aa){
return _fa(jq[0],_1aa);
},updateRow:function(jq,_1ab){
return jq.each(function(){
var opts=$.data(this,"datagrid").options;
opts.view.updateRow.call(opts.view,this,_1ab.index,_1ab.row);
});
},appendRow:function(jq,row){
return jq.each(function(){
_12c(this,row);
});
},insertRow:function(jq,_1ac){
return jq.each(function(){
_128(this,_1ac);
});
},deleteRow:function(jq,_1ad){
return jq.each(function(){
_122(this,_1ad);
});
},getChanges:function(jq,_1ae){
return _11c(jq[0],_1ae);
},acceptChanges:function(jq){
return jq.each(function(){
_132(this);
});
},rejectChanges:function(jq){
return jq.each(function(){
_134(this);
});
},mergeCells:function(jq,_1af){
return jq.each(function(){
_141(this,_1af);
});
},showColumn:function(jq,_1b0){
return jq.each(function(){
var _1b1=$(this).datagrid("getPanel");
_1b1.find("td[field=\""+_1b0+"\"]").show();
$(this).datagrid("getColumnOption",_1b0).hidden=false;
$(this).datagrid("fitColumns");
});
},hideColumn:function(jq,_1b2){
return jq.each(function(){
var _1b3=$(this).datagrid("getPanel");
_1b3.find("td[field=\""+_1b2+"\"]").hide();
$(this).datagrid("getColumnOption",_1b2).hidden=true;
$(this).datagrid("fitColumns");
});
}};
$.fn.datagrid.parseOptions=function(_1b4){
var t=$(_1b4);
return $.extend({},$.fn.panel.parseOptions(_1b4),$.parser.parseOptions(_1b4,["url","toolbar","idField","sortName","sortOrder","pagePosition",{fitColumns:"boolean",autoRowHeight:"boolean",striped:"boolean",nowrap:"boolean"},{rownumbers:"boolean",singleSelect:"boolean",checkOnSelect:"boolean",selectOnCheck:"boolean"},{pagination:"boolean",pageSize:"number",pageNumber:"number"},{remoteSort:"boolean",showHeader:"boolean",showFooter:"boolean"},{scrollbarSize:"number"}]),{pageList:(t.attr("pageList")?eval(t.attr("pageList")):undefined),loadMsg:(t.attr("loadMsg")!=undefined?t.attr("loadMsg"):undefined),rowStyler:(t.attr("rowStyler")?eval(t.attr("rowStyler")):undefined)});
};
var _1b5={render:function(_1b6,_1b7,_1b8){
var _1b9=$.data(_1b6,"datagrid");
var opts=_1b9.options;
var rows=_1b9.data.rows;
var _1ba=$(_1b6).datagrid("getColumnFields",_1b8);
if(_1b8){
if(!(opts.rownumbers||(opts.frozenColumns&&opts.frozenColumns.length))){
return;
}
}
var _1bb=["<table class=\"datagrid-btable\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>"];
for(var i=0;i<rows.length;i++){
var cls=(i%2&&opts.striped)?"class=\"datagrid-row datagrid-row-alt\"":"class=\"datagrid-row\"";
var _1bc=opts.rowStyler?opts.rowStyler.call(_1b6,i,rows[i]):"";
var _1bd=_1bc?"style=\""+_1bc+"\"":"";
var _1be=_1b9.rowIdPrefix+"-"+(_1b8?1:2)+"-"+i;
_1bb.push("<tr id=\""+_1be+"\" datagrid-row-index=\""+i+"\" "+cls+" "+_1bd+">");
_1bb.push(this.renderRow.call(this,_1b6,_1ba,_1b8,i,rows[i]));
_1bb.push("</tr>");
}
_1bb.push("</tbody></table>");
$(_1b7).html(_1bb.join(""));
},renderFooter:function(_1bf,_1c0,_1c1){
var opts=$.data(_1bf,"datagrid").options;
var rows=$.data(_1bf,"datagrid").footer||[];
var _1c2=$(_1bf).datagrid("getColumnFields",_1c1);
var _1c3=["<table class=\"datagrid-ftable\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>"];
for(var i=0;i<rows.length;i++){
_1c3.push("<tr class=\"datagrid-row\" datagrid-row-index=\""+i+"\">");
_1c3.push(this.renderRow.call(this,_1bf,_1c2,_1c1,i,rows[i]));
_1c3.push("</tr>");
}
_1c3.push("</tbody></table>");
$(_1c0).html(_1c3.join(""));
},renderRow:function(_1c4,_1c5,_1c6,_1c7,_1c8){
var opts=$.data(_1c4,"datagrid").options;
var cc=[];
if(_1c6&&opts.rownumbers){
var _1c9=_1c7+1;
if(opts.pagination){
_1c9+=(opts.pageNumber-1)*opts.pageSize;
}
cc.push("<td class=\"datagrid-td-rownumber\"><div class=\"datagrid-cell-rownumber\">"+_1c9+"</div></td>");
}
for(var i=0;i<_1c5.length;i++){
var _1ca=_1c5[i];
var col=$(_1c4).datagrid("getColumnOption",_1ca);
if(col){
var _1cb=col.styler?(col.styler(_1c8[_1ca],_1c8,_1c7)||""):"";
var _1cc=col.hidden?"style=\"display:none;"+_1cb+"\"":(_1cb?"style=\""+_1cb+"\"":"");
cc.push("<td field=\""+_1ca+"\" "+_1cc+">");
if(col.checkbox){
var _1cc="";
}else{
var _1cc="";
_1cc+="text-align:"+(col.align||"left")+";";
if(!opts.nowrap){
_1cc+="white-space:normal;height:auto;";
}else{
if(opts.autoRowHeight){
_1cc+="height:auto;";
}
}
}
cc.push("<div style=\""+_1cc+"\" ");
if(col.checkbox){
cc.push("class=\"datagrid-cell-check ");
}else{
cc.push("class=\"datagrid-cell "+col.cellClass);
}
cc.push("\">");
if(col.checkbox){
cc.push("<input type=\"checkbox\" name=\""+_1ca+"\" value=\""+(_1c8[_1ca]!=undefined?_1c8[_1ca]:"")+"\"/>");
}else{
if(col.formatter){
cc.push(col.formatter(_1c8[_1ca],_1c8,_1c7));
}else{
cc.push(_1c8[_1ca]);
}
}
cc.push("</div>");
cc.push("</td>");
}
}
return cc.join("");
},refreshRow:function(_1cd,_1ce){
var row={};
var _1cf=$(_1cd).datagrid("getColumnFields",true).concat($(_1cd).datagrid("getColumnFields",false));
for(var i=0;i<_1cf.length;i++){
row[_1cf[i]]=undefined;
}
var rows=$(_1cd).datagrid("getRows");
$.extend(row,rows[_1ce]);
this.updateRow.call(this,_1cd,_1ce,row);
},updateRow:function(_1d0,_1d1,row){
var opts=$.data(_1d0,"datagrid").options;
var rows=$(_1d0).datagrid("getRows");
var tr=opts.finder.getTr(_1d0,_1d1);
for(var _1d2 in row){
rows[_1d1][_1d2]=row[_1d2];
var td=tr.children("td[field=\""+_1d2+"\"]");
var cell=td.find("div.datagrid-cell");
var col=$(_1d0).datagrid("getColumnOption",_1d2);
if(col){
var _1d3=col.styler?col.styler(rows[_1d1][_1d2],rows[_1d1],_1d1):"";
td.attr("style",_1d3||"");
if(col.hidden){
td.hide();
}
if(col.formatter){
cell.html(col.formatter(rows[_1d1][_1d2],rows[_1d1],_1d1));
}else{
cell.html(rows[_1d1][_1d2]);
}
}
}
var _1d3=opts.rowStyler?opts.rowStyler.call(_1d0,_1d1,rows[_1d1]):"";
tr.attr("style",_1d3||"");
$(_1d0).datagrid("fixRowHeight",_1d1);
},insertRow:function(_1d4,_1d5,row){
var opts=$.data(_1d4,"datagrid").options;
var dc=$.data(_1d4,"datagrid").dc;
var data=$.data(_1d4,"datagrid").data;
if(_1d5==undefined||_1d5==null){
_1d5=data.rows.length;
}
if(_1d5>data.rows.length){
_1d5=data.rows.length;
}
for(var i=data.rows.length-1;i>=_1d5;i--){
opts.finder.getTr(_1d4,i,"body",2).attr("datagrid-row-index",i+1);
var tr=opts.finder.getTr(_1d4,i,"body",1).attr("datagrid-row-index",i+1);
if(opts.rownumbers){
tr.find("div.datagrid-cell-rownumber").html(i+2);
}
}
var _1d6=$(_1d4).datagrid("getColumnFields",true);
var _1d7=$(_1d4).datagrid("getColumnFields",false);
var tr1="<tr class=\"datagrid-row\" datagrid-row-index=\""+_1d5+"\">"+this.renderRow.call(this,_1d4,_1d6,true,_1d5,row)+"</tr>";
var tr2="<tr class=\"datagrid-row\" datagrid-row-index=\""+_1d5+"\">"+this.renderRow.call(this,_1d4,_1d7,false,_1d5,row)+"</tr>";
if(_1d5>=data.rows.length){
if(data.rows.length){
opts.finder.getTr(_1d4,"","last",1).after(tr1);
opts.finder.getTr(_1d4,"","last",2).after(tr2);
}else{
dc.body1.html("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>"+tr1+"</tbody></table>");
dc.body2.html("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>"+tr2+"</tbody></table>");
}
}else{
opts.finder.getTr(_1d4,_1d5+1,"body",1).before(tr1);
opts.finder.getTr(_1d4,_1d5+1,"body",2).before(tr2);
}
data.total+=1;
data.rows.splice(_1d5,0,row);
this.refreshRow.call(this,_1d4,_1d5);
},deleteRow:function(_1d8,_1d9){
var opts=$.data(_1d8,"datagrid").options;
var data=$.data(_1d8,"datagrid").data;
opts.finder.getTr(_1d8,_1d9).remove();
for(var i=_1d9+1;i<data.rows.length;i++){
opts.finder.getTr(_1d8,i,"body",2).attr("datagrid-row-index",i-1);
var tr1=opts.finder.getTr(_1d8,i,"body",1).attr("datagrid-row-index",i-1);
if(opts.rownumbers){
tr1.find("div.datagrid-cell-rownumber").html(i);
}
}
data.total-=1;
data.rows.splice(_1d9,1);
},onBeforeRender:function(_1da,rows){
},onAfterRender:function(_1db){
var opts=$.data(_1db,"datagrid").options;
if(opts.showFooter){
var _1dc=$(_1db).datagrid("getPanel").find("div.datagrid-footer");
_1dc.find("div.datagrid-cell-rownumber,div.datagrid-cell-check").css("visibility","hidden");
}
}};
$.fn.datagrid.defaults=$.extend({},$.fn.panel.defaults,{frozenColumns:undefined,columns:undefined,fitColumns:false,autoRowHeight:true,toolbar:null,striped:false,method:"post",nowrap:true,idField:null,url:null,loadMsg:"Processing, please wait ...",rownumbers:false,singleSelect:false,selectOnCheck:true,checkOnSelect:true,pagination:false,pagePosition:"bottom",pageNumber:1,pageSize:10,pageList:[50,100,200,500,1000],queryParams:{},sortName:null,sortOrder:"asc",remoteSort:true,showHeader:true,showFooter:false,scrollbarSize:18,rowStyler:function(_1dd,_1de){
},loader:function(_1df,_1e0,_1e1){
var opts=$(this).datagrid("options");
if(!opts.url){
return false;
}
$.ajax({type:opts.method,url:opts.url,data:_1df,dataType:"json",success:function(data){
_1e0(data);
},error:function(){
_1e1.apply(this,arguments);
}});
},loadFilter:function(data){
if(typeof data.length=="number"&&typeof data.splice=="function"){
return {total:data.length,rows:data};
}else{
return data;
}
},editors:_149,finder:{getTr:function(_1e2,_1e3,type,_1e4){
type=type||"body";
_1e4=_1e4||0;
var _1e5=$.data(_1e2,"datagrid");
var dc=_1e5.dc;
var opts=_1e5.options;
if(_1e4==0){
var tr1=opts.finder.getTr(_1e2,_1e3,type,1);
var tr2=opts.finder.getTr(_1e2,_1e3,type,2);
return tr1.add(tr2);
}else{
if(type=="body"){
var tr=$("#"+_1e5.rowIdPrefix+"-"+_1e4+"-"+_1e3);
if(!tr.length){
tr=(_1e4==1?dc.body1:dc.body2).find(">table>tbody>tr[datagrid-row-index="+_1e3+"]");
}
return tr;
}else{
if(type=="footer"){
return (_1e4==1?dc.footer1:dc.footer2).find(">table>tbody>tr[datagrid-row-index="+_1e3+"]");
}else{
if(type=="selected"){
return (_1e4==1?dc.body1:dc.body2).find(">table>tbody>tr.datagrid-row-selected");
}else{
if(type=="last"){
return (_1e4==1?dc.body1:dc.body2).find(">table>tbody>tr:last[datagrid-row-index]");
}else{
if(type=="allbody"){
return (_1e4==1?dc.body1:dc.body2).find(">table>tbody>tr[datagrid-row-index]");
}else{
if(type=="allfooter"){
return (_1e4==1?dc.footer1:dc.footer2).find(">table>tbody>tr[datagrid-row-index]");
}
}
}
}
}
}
}
},getRow:function(_1e6,_1e7){
return $.data(_1e6,"datagrid").data.rows[_1e7];
}},view:_1b5,onBeforeLoad:function(_1e8){
},onLoadSuccess:function(){
},onLoadError:function(){
},onClickRow:function(_1e9,_1ea){
},onDblClickRow:function(_1eb,_1ec){
},onClickCell:function(_1ed,_1ee,_1ef){
},onDblClickCell:function(_1f0,_1f1,_1f2){
},onSortColumn:function(sort,_1f3){
},onResizeColumn:function(_1f4,_1f5){
},onSelect:function(_1f6,_1f7){
},onUnselect:function(_1f8,_1f9){
},onSelectAll:function(rows){
},onUnselectAll:function(rows){
},onCheck:function(_1fa,_1fb){
},onUncheck:function(_1fc,_1fd){
},onCheckAll:function(rows){
},onUncheckAll:function(rows){
},onBeforeEdit:function(_1fe,_1ff){
},onAfterEdit:function(_200,_201,_202){
},onCancelEdit:function(_203,_204){
},onHeaderContextMenu:function(e,_205){
},onRowContextMenu:function(e,_206,_207){
}});
})(jQuery);

