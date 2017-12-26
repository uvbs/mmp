$(function () {
    var pathName = window.location.pathname.toLowerCase();
    if (pathName.indexOf("index.aspx") > 0) {
        $("#liindex").addClass("active");
        $("#liindex>a>img").attr("src", "images/home1_1.png");
    }
    if (pathName.indexOf("category.aspx") > 0) {
        $("#licategory").addClass("active");
        $("#licategory>a>img").attr("src", "images/home2_1.png");
    }
    if (pathName.indexOf("subcategory.aspx") > 0) {
        $("#licategory").addClass("active");
        $("#licategory>a>img").attr("src", "images/home2_1.png");
    }
    if (pathName.indexOf("productlist.aspx") > 0) {
        $("#licategory").addClass("active");
        $("#licategory>a>img").attr("src", "images/home2_1.png");
    }
    if (pathName.indexOf("productdetail.aspx") > 0) {
        $("#licategory").addClass("active");
        $("#licategory>a>img").attr("src", "images/home2_1.png");
    }
    if (pathName.indexOf("shopcar.aspx") > 0) {
        $("#lishopcar").addClass("active");
        $("#lishopcar>a>img").attr("src", "images/home3_1.png");
    }
    if (pathName.indexOf("mycenter.aspx") > 0) {
        $("#limycenter").addClass("active");
        $("#limycenter>a>img").attr("src", "images/home4_1.png");
    }
    //
    if (pathName.indexOf("addcoupon.aspx") > 0) {
        $("#limycenter").addClass("active");
        $("#limycenter>a>img").attr("src", "images/home4_1.png");
    }
    //
    if (pathName.indexOf("addressinfocompile.aspx") > 0) {
        $("#limycenter").addClass("active");
        $("#limycenter>a>img").attr("src", "images/home4_1.png");
    }
    //
    if (pathName.indexOf("couponmgr.aspx") > 0) {
        $("#limycenter").addClass("active");
        $("#limycenter>a>img").attr("src", "images/home4_1.png");
    }
    //
    if (pathName.indexOf("myaddresslist.aspx") > 0) {
        $("#limycenter").addClass("active");
        $("#limycenter>a>img").attr("src", "images/home4_1.png");
    }
    //
    if (pathName.indexOf("myorderlist.aspx") > 0) {
        $("#limycenter").addClass("active");
        $("#limycenter>a>img").attr("src", "images/home4_1.png");
    }
    //
    if (pathName.indexOf("myproductcollect.aspx") > 0) {
        $("#limycenter").addClass("active");
        $("#limycenter>a>img").attr("src", "images/home4_1.png");
    }
    //
    if (pathName.indexOf("personinfo.aspx") > 0) {
        $("#limycenter").addClass("active");
        $("#limycenter>a>img").attr("src", "images/home4_1.png");
    }
    //
    if (pathName.indexOf("scorerecord.aspx") > 0) {
        $("#limycenter").addClass("active");
        $("#limycenter>a>img").attr("src", "images/home4_1.png");
    }
    //
    if (pathName.indexOf("orderconfirm.aspx") > 0) {
        $("#lishopcar").addClass("active");
        $("#lishopcar>a>img").attr("src", "images/home3_1.png");
    }



    
})