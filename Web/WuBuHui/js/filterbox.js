$("#filterbtn").bind("touchend",function(){if($("#filterbox").hasClass("hidefilterbox"))$("#filterbox").removeClass("hidefilterbox"),setTimeout(function(){var e=$(window).height()-136;$("#filterbox").height(e)},100);else{$("#filterbox").attr("style","");function e(){$("#filterbox").addClass("hidefilterbox"),$("#filterbox")[0].removeEventListener("webkitTransitionEnd",e,!1)}$("#filterbox")[0].addEventListener("webkitTransitionEnd",e,!1)}});