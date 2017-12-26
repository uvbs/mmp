
var navbar = $('#navbar');
$(window).scroll(function () {
    var scrollTop = $(document).scrollTop();
    if (scrollTop > 100) {
       navbar.addClass('fixed');
    } else {
        navbar.removeClass('fixed');
    }
});
