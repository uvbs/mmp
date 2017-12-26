
$(function () {
    var h = document.documentElement.clientHeight;
    $('#divIndexPage').css({ height: h });
    //$('#footerIndex').css({ height: parseInt(h * 0.45) });
    $(window).resize(function () {
        //var w = document.body.clientWidth;
        h = document.documentElement.clientHeight;
        $('#divIndexPage').css({ height: h });
        //$('#footerIndex').css({ height: parseInt(h * 0.45) });
    });
    
});
