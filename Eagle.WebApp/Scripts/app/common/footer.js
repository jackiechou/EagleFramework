(function ($) {
    //settings from apps.js - lib.js
    setupMenu();
    loadSiteMap();
    setupValidation(); //validation.js
    setupLoading();
    setupNumber();
    handleRadios();
    //handleRadioButtionSwitch();
    handleCheckBoxes();
    handleAccordion();
    invokeDateTimePicker('dd/MM/yyyy');

    //Start Set up Ajax ====================================================
    var options = {
        AjaxWait: {
            AjaxWaitMessage: "<img style='height:150px' src='/Images/Wait.gif'/>",
            AjaxWaitMessageCss: { width: "400px", left: "40%", border: 'none', backgroundColor: 'transparent' }
        },
        AjaxErrorMessage: "<h6>Error! please contact the technical software engineer!</h6>",
        SessionOut: {
            StatusCode: 590,
            RedirectUrl: "/Admin/Account/Login"
        }
    };

    AjaxGlobalHandler.Initiate(options);
    //End Set up Ajax ====================================================

    ////change width of datatable 
    //function resizeDataTable() {
    //    $('.dataTables_scrollHead').css('width', '100%');
    //    $(".dataTables_scrollHeadInner").css("width", "100%");
    //    $(".dataTables_scrollHeadInner .dataTable").css("width", "100%");
    //    $(".dataTables_scrollBody").css("width", "100%");
    //    $(".dataTables_scrollBody .dataTable").css("width", "100%");
    //    $(".dataTables_scrollHeadInner").css("width", $(".dataTables_scrollBody .dataTable").width() * 100 / 99).css("margin-left", "0px");
    //}
    ////accordion ========================================================================================================
    //$('.accordion-group').on('show hide', function (n) {
    //    $(n.target).siblings('.accordion-heading').find('.accordion-toggle i').toggleClass('icon-chevron-up icon-chevron-down');
    //});
    ////==================================================================================================================


    ////NOTIFICATION =======================================================================   
    //$(document).click(function () {
    //    $("#notificationContainer").hide();
    //});
    ////Popup Click
    //$(document).on("click", "#notificationLink", function () {
    //    $("#notificationContainer").animate({ "width": 'toggle' }, 'slow');
    //    // $("#notification_count").fadeOut("slow");
    //    return false;
    //});
    //$(document).on("click", "#notificationContainer", function () {
    //    return false;
    //});
    //$(document).on("click", "#notificationContainer a", function () {
    //    window.location.href = $(this).attr("href");
    //});
    ////==================================================================================


    // setrightwidth();
    //function setrightwidth() {
    //    var browserwidth = $(window).outerwidth();
    //    var leftwidth = $("#leftpane").outerwidth();

    //    var leftpercentwidth = math.round(leftwidth / browserwidth * 100);

    //    var paddingwidth = 0;
    //    var rightpercentwidth = 0;

    //    rightpercentwidth = 100 - (leftpercentwidth + paddingwidth);
    //    //alert(leftpercentwidth + " - " + rightpercentwidth);
    //    $("#contentpane").width(rightpercentwidth + '%');
    //}

    //$(document).on("click", '#changeSidebarPos', function (event) {
    //    $('body').toggleClass('hide-sidebar');
    //    setRightWidth();
    //    resizeDataTable();
    //});


    //$.ajaxSetup({
    //    error: function (jqXHR, exception) {
    //        if (jqXHR.status === 0) {
    //            alert('Not connect.n Verify Network.');
    //        } else if (jqXHR.status == 404) {
    //            alert('Requested page not found. [404]');
    //        } else if (jqXHR.status == 500) {
    //            alert('Internal Server Error [500].');
    //        } else if (exception === 'parsererror') {
    //            alert('Requested JSON parse failed.');
    //        } else if (exception === 'timeout') {
    //            alert('Time out error.');
    //        } else if (exception === 'abort') {
    //            alert('Ajax request aborted.');
    //        } else {
    //            alert('Uncaught Error.n' + jqXHR.responseText);
    //        }
    //    }
    //});

    //Loading Process Spin
    //var waitimageUrl = '/Content/Admin/images/Wait.gif';
    //var sessionoutRedirect = '@Url.Action("Login", "User")';
    //$(document).ready(function () {
    //    var options = {
    //        AjaxWait: {
    //            AjaxWaitMessage: "<img style='height: 40px' src='" + waitimageUrl + "' />",
    //            AjaxWaitMessageCss: { width: "50px", left: "45%" }
    //        },
    //        AjaxErrorMessage: "<h6>Error! please contact the specialist!</h6>",
    //        SessionOut: {
    //            StatusCode: 590,
    //            RedirectUrl: sessionoutRedirect
    //        }
    //    };

    //    AjaxGlobalHandler.Initiate(options);
    //});


    //try {
    //    $.validator.addMethod("time", function (value, element) {
    //        return this.optional(element) || /^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])(:([0-5]?[0-9]))?$/i.test(value);
    //    }, "Please enter a valid time.");
    //} catch (e) {
    //}


    // Kiem tra trinh duyet dang thao tac
    //function checkBrowser(prop) {
    //    return prop in document.documentElement.style;
    //}

    //$.isOpera = !!(window.opera && window.opera.version);  // Opera 8.0+
    //$.isFirefox = checkBrowser('MozBoxSizing');                 // FF 0.8+
    //$.isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;
    //$.isChrome = !$.isSafari && CheckBrowser('WebkitTransform');  // Chrome 1+
    //$.isIE = false || CheckBrowser('msTransform');  // At least IE6
})(jQuery);