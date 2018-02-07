(function ($) {
    function getLatestCollection() {
        $.getJSON(window.GetGalleryFilesFromLatestCollectionUrl, function (data) {
            if (data === null || data === undefined || data.length <= 0) {
                console.log('NotFoundData');
            } else {
                var slideWidth = 158.5;
                var numberSlidesVisible = (data.length < 7) ? data.length : 7;
                var carouselOuterHeight = 210;
                var carouselOuterWidth = slideWidth * numberSlidesVisible;

                $("#multiple_slides_visible").agile_carousel({
                    carousel_data: data,
                    carousel_outer_height: carouselOuterHeight,
                    carousel_height: 200,
                    slide_height: 200,
                    carousel_outer_width: carouselOuterWidth,
                    slide_width: slideWidth,
                    number_slides_visible: numberSlidesVisible,
                    transition_time: 330,
                    continuous_scrolling: true,
                    control_set_1: "previous_button,next_button",
                    control_set_2: "group_numbered_buttons",
                    persistent_content: "<p class='persistent_content'>" + window.LatestCollection + " (" + data.length + ")</p>"
                });

                $('.photo_link').magnificPopup({
                    //delegate: 'a', // the selector for gallery item
                    type: 'image',
                    closeOnContentClick: true,
                    closeBtnInside: false,
                    gallery: {
                        enabled: true,
                        tCounter: '%curr% of %total%'
                    }
                });

                $('.caption').magnificPopup({
                    type: 'image',
                    closeOnContentClick: true,
                    closeBtnInside: false,
                    gallery: {
                        enabled: true,
                        tCounter: '%curr% of %total%'
                    }
                });
            }
            return false;
        });
    }
    
    getLatestCollection();

    //Banner
    $("#banner-top").unitegallery({
        gallery_theme: "slider",
        gallery_width: 1140,							//gallery width		
        gallery_height: 322,						//gallery height

        gallery_min_width: 1012,						//gallery minimal width when resizing
        gallery_min_height: 322,					//gallery minimal height when resizing

        gallery_skin: "default",						//default, alexis etc... - the global skin of the gallery. Will change all gallery items by default.
        gallery_images_preload_type: "minimal",		//all , minimal , visible - preload type of the images.
        //minimal - only image nabours will be loaded each time.
        //visible - visible thumbs images will be loaded each time.
        //all - load all the images first time.

        gallery_autoplay: true,						//true / false - begin slideshow autoplay on start
        gallery_play_interval: 3000,				//play interval of the slideshow
        gallery_pause_on_mouseover: true,			//true,false - pause on mouseover when playing slideshow true/false

        gallery_control_thumbs_mousewheel: false,	//true,false - enable / disable the mousewheel
        gallery_control_keyboard: true,				//true,false - enable / disble keyboard controls
        gallery_carousel: true,						//true,false - next button on last image goes to first image.

        gallery_preserve_ratio: true,				//true, false - preserver ratio when on window resize
        gallery_debug_errors: true,					//show error message when there is some error on the gallery area.
        gallery_background_color: "",				//set custom background color. If not set it will be taken from css.

        slider_enable_progress_indicator: false,		 //enable progress indicator element
        slider_control_zoom: false,                 //true, false - enable zooming control
        slider_enable_zoom_panel: false,				 //true,false - enable the zoom buttons, works together with zoom control.
        slider_transition: "slide",					//fade, slide - the transition of the slide change
        slider_transition_speed: 300,				//transition duration of slide change
        slider_transition_easing: "easeInOutQuad",	//transition easing function of slide change
    });
})(jQuery);