var $ = jQuery.noConflict();
$(function(){
    $('.form').find('input, select, textarea').on('touchstart mousedown click', function(e){
        e.stopPropagation();
    })
})

//Scroll Containers
$('.swiper-nested').each(function () {
    var swipernested = $(this).swiper({
        mode: 'vertical',
        scrollContainer: true,
        mousewheelControl: true,
        scrollbar: {
            container: $(this).find('.swiper-scrollbar')[0]
        }
    })

    $(".scrolltop").click(function () {
        swipernested.swipeTo(0);
    })
    $(".trigger").click(function () {
        function fixheighttrigger() {
            swipernested.reInit();
            setTimeout(fixheighttrigger, 1000);
        }
        setTimeout(fixheighttrigger, 1000);
    });
})