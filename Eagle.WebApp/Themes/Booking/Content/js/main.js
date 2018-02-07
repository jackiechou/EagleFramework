jQuery(document).ready(function () {
    App.init();
    //App.initScrollBar();
    App.initParallaxBg();
    //OwlCarousel.initOwlCarousel();
    //StepWizard.initStepWizard();
    RevolutionSlider.initRSfullWidth();
});


//Content Details
var App = function () {
    //Start Set up Ajax ====================================================
    var options = {
        AjaxWait: {
            AjaxWaitMessage: "<img style='height:150px' src='/Images/Wait.gif' />",
            AjaxWaitMessageCss: {
                width: "400px", left: "40%", border: 'none', backgroundColor: 'transparent'
            }
        },
        AjaxErrorMessage: "<h6>Error! please contact the technical software engineer!</h6>",
        SessionOut: {
            StatusCode: 590,
            RedirectUrl: "/Login"
        }
    };

    AjaxGlobalHandler.Initiate(options);
    //End Set up Ajax ====================================================

    function loadSiteMap() {
        //var menuCode = '';
        //var sPageUrl = window.location.href;
        //var indexOfLastSlash = sPageUrl.lastIndexOf("/");

        //if (indexOfLastSlash > 0 && sPageUrl.length - 1 !== indexOfLastSlash)
        //    menuCode = sPageUrl.substring(indexOfLastSlash + 1);

        var siteMapUrl = '/MenuDesktop/PopulateSiteMap';

        var path = window.location.pathname;
        var arrayPath = path.split("/");
        var controllerName = arrayPath[1] || "home";
        var actionName = arrayPath[2] || "index";

        var params = { "controllerName": controllerName, "actionName": actionName }

        $.ajax({
            type: "GET",
            url: siteMapUrl,
            data: params,
            success: function (data) {
                $('#SiteMap').html(data);
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function loadMainMenu() {
        var container = $('#MainMenu');
        var url = container.data('url');
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                container.html(data);
                container.superfish({
                    speed: 'fast'
                });
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function populateQuickTopMenu() {
        var container = $('#QuickTopMenu');
        var url = container.data('url');
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                container.html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

  
    function handleBootstrap() {
        /*Bootstrap Carousel*/
        jQuery('.carousel').carousel({
            interval: 15000,
            pause: 'hover'
        });

        /*Tooltips*/
        //jQuery('.tooltips').tooltip();
        //jQuery('.tooltips-show').tooltip('show');      
        //jQuery('.tooltips-hide').tooltip('hide');       
        //jQuery('.tooltips-toggle').tooltip('toggle');       
        //jQuery('.tooltips-destroy').tooltip('destroy');       

        /*Popovers*/
        //jQuery('.popovers').popover();
        //jQuery('.popovers-show').popover('show');
        //jQuery('.popovers-hide').popover('hide');
        //jQuery('.popovers-toggle').popover('toggle');
        //jQuery('.popovers-destroy').popover('destroy');
    }

    function handleSearchV1() {
        jQuery('.search-button').click(function () {
            jQuery('.search-open').slideDown();
        });

        jQuery('.search-close').click(function () {
            jQuery('.search-open').slideUp();
        });

        jQuery(window).scroll(function () {
            if (jQuery(this).scrollTop() > 1) jQuery('.search-open').fadeOut('fast');
        });
    }

    function handleToggle() {
        jQuery('.list-toggle').on('click', function () {
            jQuery(this).toggleClass('active');
        });

        /*
        jQuery('#serviceList').on('shown.bs.collapse'), function() {
            jQuery(".servicedrop").addClass('glyphicon-chevron-up').removeClass('glyphicon-chevron-down');
        }

        jQuery('#serviceList').on('hidden.bs.collapse'), function() {
            jQuery(".servicedrop").addClass('glyphicon-chevron-down').removeClass('glyphicon-chevron-up');
        }
        */
    }

    function handleBoxed() {
        jQuery('.boxed-layout-btn').click(function () {
            jQuery(this).addClass("active-switcher-btn");
            jQuery(".wide-layout-btn").removeClass("active-switcher-btn");
            jQuery("body").addClass("boxed-layout container");
        });
        jQuery('.wide-layout-btn').click(function () {
            jQuery(this).addClass("active-switcher-btn");
            jQuery(".boxed-layout-btn").removeClass("active-switcher-btn");
            jQuery("body").removeClass("boxed-layout container");
        });
    }

    function handleHeader() {
        jQuery(window).scroll(function () {
            if (jQuery(window).scrollTop() > 100) {
                jQuery(".header-fixed .header-static").addClass("header-fixed-shrink");
            }
            else {
                jQuery(".header-fixed .header-static").removeClass("header-fixed-shrink");
            }
        });
    }

    function handleMegaMenu() {
        $(document).on('click', '.mega-menu .dropdown-menu', function (e) {
            e.stopPropagation();
        });
    }

    
    //$(document).on("click", ".sign-up", function (e) {
    //    e.preventDefault();

    //    var url = $(this).data('url');
    //    var container = $(this).data('main-content-page');

    //    //var params = { id: id };
    //    $.ajax({
    //        type: "GET",
    //        url: url,
    //        // data: params,
    //        success: function (data) {
    //            $('.' + container).html(data);
    //            invokeDateTimePicker('dd/MM/yyyy');
    //        }, error: function (jqXhr, textStatus, errorThrown) {
    //            handleAjaxErrors(jqXhr, textStatus, errorThrown);
    //        }
    //    });
    //    return false;
    //});

    //$sidebar = $('#MainMenu');
    //sidebarTop = $sidebar.position().top;
    //sidebarHeight = $sidebar.height();
    //$footer = $('#footer-bot');
    //footerTop = $footer.position().top;
    //$sidebar.addClass('fixed');
    //scrollTop = $(window).scrollTop();
    //topPosition = Math.max(0, sidebarTop - scrollTop);
    //topPosition = Math.min(topPosition, (footerTop - scrollTop) - sidebarHeight);
    //$sidebar.css('top', topPosition);

    //$(window).scroll(function () {
    //    //alert($(document).height());
    //    if ($(document).height() - ($(window).scrollTop() + $(window).height()) > 150) {
    //        $('.nav-fix').fadeIn(200);
    //    }
    //    else {
    //        $('.nav-fix').fadeOut(200);
    //    }

    //    scrollTop = $(window).scrollTop();
    //    topPosition = Math.max(0, sidebarTop - scrollTop);
    //    topPosition = Math.min(topPosition, (footerTop - scrollTop) - sidebarHeight);
    //    $sidebar.css('top', topPosition);
    //});

    function bindEvents() {
        $('#QuickTopMenu').click(function () {
            $('#cart-info').css('display', 'block');
        });
        
        $('.show_popup').click(function () {
            //debugger;
            if ($('.popup_customer').css('display') === 'none') {
                $('.popup_customer').css('display', 'block');
            } else {
                $('.popup_customer').css('display', 'none');
            }
        });

        $('.pop').click(function () {
            $('.popup').toggleClass("open");
        });

        $('.band').click(function () {
            $('.popup').toggleClass("open");
        });

        $('#thumbcarousel li').on('click', function () {
            var indexi = $(this).index();
           
            $('#carousel .carousel-inner .item').removeClass('active');
            $($('#carousel .carousel-inner .item')[indexi]).addClass('active');
        });

    

        // Select and loop the container element of the elements you want to equalise
        $('.container-service').each(function () {

            // Cache the highest
            var highestBox = 0;

            // Select and loop the elements you want to equalise
            $('.item-service', this).each(function () {

                // If this box is higher than the cached highest then store it
                if ($(this).height() > highestBox) {
                    highestBox = $(this).height();
                }

            });

            // Set the height of all those children to whichever was highest 
            $('.item-service', this).height(highestBox);

        });
    }

    function handleRandomBackground() {
        var images = ['/Themes/Booking/Content/img/breadcrumb/banner1.jpg', '/Themes/Booking/Content/img/breadcrumb/banner2.jpg',
            '/Themes/Booking/Content/img/breadcrumb/banner3.jpg', '/Themes/Booking/Content/img/breadcrumb/banner4.jpg',
            '/Themes/Booking/Content/img/breadcrumb/banner5.jpg', '/Themes/Booking/Content/img/breadcrumb/banner6.jpg'];
        var randomImage = Math.floor(Math.random() * 3);
        $(".breadcrumbs-dark").css("background-image", "url('" + images[randomImage] + "')");
    }
    
    return {
        init: function () {
            loadMainMenu();
            populateQuickTopMenu();
            loadSiteMap();
            bindEvents();

            //Handle Events
            handleBootstrap();
            handleSearchV1();
            handleToggle();
            handleBoxed();
            handleHeader();
            handleMegaMenu();

            //Defaut
            setupValidation();  //validation.js
           // setupLoading();
            setupNumber();
            handleRadios();
            handleCheckBoxes();
            handleAccordion();
            handleRandomBackground();
            invokeDateTimePicker('dd/MM/yyyy');
            $(".dotdot").dotdotdot();
        },

        //initScrollBar: function () {
        //    jQuery('.mCustomScrollbar').mCustomScrollbar({
        //        theme:"minimal",
        //        scrollInertia: 300,
        //        scrollEasing: "linear"
        //    });
        //},

        initCounter: function () {
            jQuery('.counter').counterUp({
                delay: 10,
                time: 1000
            });
        },

        initParallaxBg: function () {
            jQuery('.parallaxBg').parallax("50%", 0.2);
            jQuery('.parallaxBg1').parallax("50%", 0.4);
        }
    };
}();