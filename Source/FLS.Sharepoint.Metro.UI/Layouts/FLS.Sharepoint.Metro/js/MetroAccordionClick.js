jQuery(function($) {    
//Hide all    
    $('.s4-ql li ul').hide();     
    //Set the Images up    
    var Collapse = "/_layouts/images/collapse.gif";    
    var Expand = "/_layouts/images/expand.gif";          
    //Find each top level UI and add reletive buttons & children numbers     
    $('.s4-ql ul li').find('ul').each(function(index) {
                var $this = $(this);        
        $this.parent().find('a:first .menu-item-text').append(['<a class=\'min\' style=\'float:right; margin: 0px 0px 0px 5px; padding:0px !important; border:0px !important; font-size:1em !important;\'><img src=\'/_layouts/images/expand.gif\'/></a><span style=\'float:right;font-size:0.8em;\'>(', $this.children().length, ')</span>'].join(''));
    });     
    //Setup Click Hanlder    
    $('.min').click(function() {        
    //Get Reference to img        
        var img = $(this).children();        
        //Traverse the DOM to find the child UL node        
        var subList = $(this).parent().parent().parent().next();        
        //Check the visibility of the item and set the image       
        var Visibility = subList.is(":visible")? img.attr('src',Expand) : img.attr('src',Collapse);
        //Toggle the UL        
        subList.slideToggle();
    });
});