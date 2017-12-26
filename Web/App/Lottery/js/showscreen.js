angular.module("showscreen", []).controller("test", ["$scope", "$http", function(a, b) {
  a.awarddata = {}, a.rightspeed = 60, a.refreshtime = 2e3, a.awardleftnum = 0, a.awardrightdata = {}, a.awardtotalrightnum = 0, a.leftshownum = 1, a.awardtotalnum = 0, a.pagesize = 40, a.awardurl = "/serv/awardapi.ashx", (location.host.match("192.168.1.181") || location.host.match("192.168.0.102")) && (a.awardurl = "awardapi.php"), a.getQueryString = function(a) {
    var b = new RegExp("(^|&)" + a + "=([^&]*)(&|$)", "i"),
      c = window.location.search.substr(1).match(b);
    return null != c ? unescape(c[2]) : null
  }, a.getawardinfourl = function(b, c) {
    "undefined" == typeof b && (b = 1), "undefined" == typeof c && (c = 50);
    var d = a.awardurl + "?action=getawardrecord";
    return d += "&&id=" + a.getQueryString("id"), d += "&&pageindex=" + b, d += "&&pagesize=" + c
  }, a.getawardinfo = function() {
    b.get(a.getawardinfourl(1, a.pagesize)).success(function(b) {
      a.awarddata = b.list, a.awardtotalnum = b.totalcount
    }), setTimeout(function() {
      a.getawardinfo()
    }, a.refreshtime)
  }, a.getawardinfo()
}]).directive("leftbox", function() {
  return {
    restrict: "AE",
    templateUrl: "template/showscreen-leftbox.html",
    replace: !0,
    link: function(a, b) {

      function c() {
        function b() {
          d.children(":eq(0)")[0].removeEventListener("transitionend", b), c()
        }
        if (a.pagesize >= a.awardtotalnum) var e = a.awarddata.length - a.leftshownum;
        else {
          1 === a.leftshownum && (a.leftshownum = a.awardtotalnum - a.pagesize + 1);
          var e = a.awardtotalnum - a.leftshownum
        }
        if (0 > e||typeof a.awarddata.length==="undefined") return void setTimeout(function() {
          c()
        }, a.refreshtime);
        var g = a.awarddata[e],
          h = "<div class='awardbar' ><span class='awardbarinfo col-xs-3'><span class='whiteround'></span><img src='" + g.headimg + "' alt='' class='avatar'></span><span class='awardbarinfo col-xs-5'>" + g.nickname + "</span><span class='awardbarinfo col-xs-4'>" + g.awardname + "</span></div>";
        d.children(":eq(0)")[0] ? d.children(":eq(0)").before(h) : d.append(h), a.leftshownum++, d.children(":eq(0)")[0].addEventListener("transitionend", b), setTimeout(function() {
          d.children().each(function(b) {
            b ? b < a.awardleftnum ? $(this).css({
              transform: "translateY(" + (f / 2 + (f + 80) * b) + "px)"
            }) : b === a.awardleftnum ? $(this).css({
              transform: "translateY(" + (f + (f + 80) * (a.awardleftnum - 1)) + "px)",
              opacity: "0"
            }) : $(this).remove() : $(this).css({
              transform: "translateY(" + f / 2 + "px) scale(1)",
              opacity: "1"
            })
          })
        }, 100)
      }


      for (var d = b.find(".awardlist"), e = window.innerHeight - b[0].querySelector(".awardlist").offsetTop, f = 10, g = 10; g >= 10 || 8 > f;) g = e % (80 + f), f++;
      a.awardleftnum = parseInt(e / (f + 80)), d.css({
        height: e
      }), setTimeout(function() {
        c()
      }, 1e3)
    }
  }
}).directive("rightbox", function() {
  return {
    restrict: "AE",
    templateUrl: "template/showscreen-rightbox.html",
    replace: !0,
    link: function(a, b) {
      function c() {
        function e() {
          b.find(".awardlistbox")[0].removeEventListener("transitionend", e), b.find(".awardlistbox").hide("slow"), b.find(".awardlistbox").stop().show("slow").css({
            transform: "translateY(0px)",
            transition: "transform 0s linear 0s"
          }), setTimeout(function() {
            c()
          }, 50)
        }
        a.awardrightdata = a.awarddata;
        var f = b.find(".awardlistbox");
        if (a.awardtotalrightnum < a.awardtotalnum) {
          f.html("");
          for (var g = 0; g < a.awarddata.length; g++) {
            var h = "<div class='awardinfo'><span class='text col-xs-6'>" + a.awarddata[g].nickname + "</span><span class='text col-xs-6'>" + a.awarddata[g].awardname + "</span></div>";
            f.append(h)
          }
        }
        if (f.height() < d - 120) return void setTimeout(function() {
          c()
        }, 1e3);
        a.awardtotalrightnum < a.awardtotalnum && (a.awardtotalrightnum = a.awardtotalnum, f.append(f.children().clone()));
        var i = -b.find(".awardlistbox").height() / 2;
        b.find(".awardlistbox").css({
          transform: "translateY(" + i + "px)",
          transition: "transform " + parseInt(-i / a.rightspeed) + "s linear 0s"
        }), b.find(".awardlistbox")[0].addEventListener("transitionend", e)
      }
      var d = window.innerHeight - b[0].querySelector(".listbox").offsetTop - 20;
      b.find(".listbox").css({
        height: d
      }), setTimeout(function() {
        c()
      }, 1e3)
    }
  }
});