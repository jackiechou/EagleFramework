(function ($) {
    setupValidation();  //validation.js
    setupLoading();
    setupNumber();
    handleRadios();
    handleCheckBoxes();
    handleAccordion();
    invokeDateTimePicker('dd/MM/yyyy');

    //Start Set up Ajax ====================================================
    var options = {
        AjaxWait: {
            AjaxWaitMessage: "<img style='height:150px' src='/Images/Wait.gif' />",
            AjaxWaitMessageCss: { width: "400px", left: "40%", border: 'none', backgroundColor: 'transparent' }
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
        var menuId = 0;
        var sPageUrl = window.location.href;
        var indexOfLastSlash = sPageUrl.lastIndexOf("/");

        if (indexOfLastSlash > 0 && sPageUrl.length - 1 !== indexOfLastSlash)
            menuId = sPageUrl.substring(indexOfLastSlash + 1);

        var siteMapUrl = '/Admin/Menu/PopulateSiteMapByMenuId?menuId=' + menuId;
        $.ajax({
            type: "GET",
            url: siteMapUrl,
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
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    loadMainMenu();



    function resetControls(form) {
        var validateObj = $('#' + form);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

    $(document).on("click", ".sign-up", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var container = $(this).data('container');

        //var params = { id: id };
        $.ajax({
            type: "GET",
            url: url,
           // data: params,
            success: function (data) {
                $('.' + container).html(data);
                invokeDateTimePicker('dd/MM/yyyy');
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    });

    $(document).on("click", ".sign-in", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var container = $(this).data('container');

        //var params = { id: id };
        $.ajax({
            type: "GET",
            url: url,
            // data: params,
            success: function (data) {
                $('.' + container).html(data);
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    });

    $sidebar = $('#MainMenu');
    sidebarTop = $sidebar.position().top;
    sidebarHeight = $sidebar.height();
    $footer = $('#footer-bot');
    footerTop = $footer.position().top;
    $sidebar.addClass('fixed');
    scrollTop = $(window).scrollTop();
    topPosition = Math.max(0, sidebarTop - scrollTop);
    topPosition = Math.min(topPosition, (footerTop - scrollTop) - sidebarHeight);
    $sidebar.css('top', topPosition);

    $(window).scroll(function () {
        //alert($(document).height());
        if ($(document).height() - ($(window).scrollTop() + $(window).height()) > 150) {
            $('.nav-fix').fadeIn(200);
        }
        else {
            $('.nav-fix').fadeOut(200);
        }

        scrollTop = $(window).scrollTop();
        topPosition = Math.max(0, sidebarTop - scrollTop);
        topPosition = Math.min(topPosition, (footerTop - scrollTop) - sidebarHeight);
        $sidebar.css('top', topPosition);
    });

    $('.pop').click(function () {
        $('.popup').toggleClass("open");
    });

    $('.band').click(function () {
        $('.popup').toggleClass("open");
    });

})(jQuery);
