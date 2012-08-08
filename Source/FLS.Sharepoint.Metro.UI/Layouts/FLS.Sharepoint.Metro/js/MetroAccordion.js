jQuery(function ($) {
    $('.s4-ql li ul').hide();

    $('.s4-ql ul li').find('ul').each(function (index) {
        var $this = $(this);
        $this.parent().find('a:first .menu-item-text').append(['<span style=\'float:right;font-size:0.8em;\'>(', $this.children().length, ')</span>'].join(''));
    });

    var settings = {
        sensitivity: 4,
        interval: 200,
        over: show,
        out: hide
    }; 
    $(".s4-ql ul li").hoverIntent( settings );
});
    function show()
    {
        $(this).find('a:first').next().slideToggle();
    }

    function hide()
    {
        $(this).find('a:first').next().slideToggle();
    }