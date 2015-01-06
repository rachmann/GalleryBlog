$(function () {
   
    //get value Onchange radio function	
    $('input[name="imgsize"]').change(function () {
        var chbox = $("input[name='imgsize']:checked");
        var value = chbox.val();
        var imgCan = jQuery("#ImageContainer");
        if (value == 1) {
            imgCan.removeClass('landscape');
            imgCan.addClass('portrait');
        } else {

            imgCan.removeClass('portrait');
            imgCan.addClass('landscape');
            var newHeight = window.innerHeight - (window.innerHeight / 10) - chbox[0].offsetTop*2 - chbox[0].offsetParent.offsetTop*2;

            imgCan.height(newHeight);
        }
    });	
});