$(document).ready(function () {
    function populateVideoGallery() {
        $(".video-gallery").unitegallery({
            gallery_theme: "video",
            theme_skin: "right-thumb",			//right-thumb | right-title-only | right-no-thumb
            theme_autoplay: false,				//autoplay videos at start.  true / false. Don't working on mobiles.
            theme_disable_panel_timeout: 2500,	//How much time the right panel will be disabled. in ms

            //theme options

            theme_enable_fullscreen_button: true,	//show, hide the theme fullscreen button. The position in the theme is constant
            theme_enable_play_button: true,			//show, hide the theme play button. The position in the theme is constant
            theme_enable_hidepanel_button: true,	//show, hide the hidepanel button
            theme_enable_text_panel: true,			//enable the panel text panel. 

            theme_panel_position: "right",			//top, bottom, left, right - thumbs panel position
            theme_hide_panel_under_width: 480,		//hide panel under certain browser width, if null, don't hide


            //navigation options

            theme_enable_navigation: true,
            theme_navigation_position: "bottom",	//top,bottom: the vertical position of the navigation reative to the carousel
            theme_navigation_enable_play: true,		//enable / disable the play button of the navigation
            theme_navigation_align: "center",		//the align of the navigation
            theme_navigation_offset_hor: 0,			//horizontal offset of the navigation
            theme_navigation_margin: 20,			//the space between the carousel and the navigation
            theme_space_between_arrows: 5,			//the space between arrows in the navigation

            strippanel_enable_buttons: false,
            strippanel_enable_handle: false

        });
    }

    populateVideoGallery();

    function populateVideoByPlayList(url, playListId) {
        var container = $('.video-container');
        var params = { "playListId" : playListId }
        $.ajax({
            type: "GET",
            url: url,
            data: params,
            success: function (data) {
                container.html(data);
                populateVideoGallery();
             
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function populateVideoByAlbum(url, albumId) {
        var container = $('.video-container');
        var params = { "albumId": albumId }
        $.ajax({
            type: "GET",
            url: url,
            data: params,
            success: function (data) {
                container.html(data);
                populateVideoGallery();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    $(document).on("click", ".getFilesByPlayList", function () {
        var url = $(this).data('url');
        var id = $(this).data('id');
        populateVideoByPlayList(url, id);
        return false;
    });

    $(document).on("click", ".getFilesByAlbum", function () {
        var url = $(this).data('url');
        var id = $(this).data('id');
        populateVideoByAlbum(url, id);
        return false;
    });
});