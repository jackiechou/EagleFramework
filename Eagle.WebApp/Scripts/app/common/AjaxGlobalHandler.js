var AjaxGlobalHandler = {
    Initiate: function (options) {
        $.ajaxSetup({
            async: true,
            cache: false,
            //beforeSend: function (xhr) {
            //    var verificationToken = $('input[name=__RequestVerificationToken]').val();
            //    if (verificationToken) {
            //        xhr.setRequestHeader('__RequestVerificationToken', verificationToken);
            //    }
            //},
            headers: {
                //'__RequestVerificationToken': $('meta[name="csrf-token"]').attr('content'),
                '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
            }
        });

        //// Pass anti-forgery token through in the header of all ajax requests
        //$.ajaxPrefilter(function (options, originalOptions, jqXhr) {
        //    var verificationToken = $('input[name=__RequestVerificationToken]').val();
        //    if (verificationToken) {
        //        jqXhr.setRequestHeader("X-Request-Verification-Token", verificationToken);
        //        $('meta[name="csrf-token"]').attr('content', verificationToken);
        //    }
        //});

        // Ajax events fire in following order
        $(document).ajaxStart(function () {
            $.blockUI({
                message: options.AjaxWait.AjaxWaitMessage,
                css: options.AjaxWait.AjaxWaitMessageCss
            });
            //$('#ajaxProgress').show();
        }).ajaxSend(function (e, xhr, opts) {
        }).ajaxError(function (e, xhr, opts) {
            e.stopPropagation();

            if (options.SessionOut.StatusCode === xhr.status) {
                document.location.replace(options.SessionOut.RedirectUrl);
                return;
            }

            //if (xhr.status === 500) {
            //    window.location = "/Error/InternalServerError";
            //}
            //if (xhr.status === 404) {
            //    window.location = "/Error/NotFound";
            //}
            //if (xhr.status === 403) {
            //    window.location = "/Error/SessionExpired";
            //}
            //if (xhr.status === 401) {
            //    window.location = "/Error/Unauthorized";
            //}
            //if (xhr.status === 320) {
            //    window.location = "/Error/RequestTypeViolation";
            //}
            //if (xhr.status === 0) {
            //    window.location = "/Error/NetworkError";
            //}

            var message = getAjaxErrors(xhr, e);
            $.colorbox({
                html: '<div class="modal alert-danger" role="dialog">' +
                '<div class="modal-dialog">' +
                '<div class="modal-header">' +
                //'<button type="button" class="close" data-dismiss="modal">&times;</button>' +
                '<h4 class="modal-title">Error  ' + options.AjaxErrorMessage + '</h4>' +
                '</div>' +
                '<div class="modal-body">' +
                '<p>' + message + '<p>' +
                '</div>' +
                //'<div class="modal-footer">' +
                //    '<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>' +
                //'</div>' +
                '</div>' +
                '</div>',
                closeButton: false,
                opacity: 0.85
                //onOpen: function () {
                //    // make the overlay visible and re-add all it's original properties!
                //    //$('#cboxOverlay').css({
                //    //    'visibility': 'visible',
                //    //    'opacity': 0.5,
                //    //    'cursor': 'pointer',
                //    //    'background-color': '#EFF0F1'
                //    //});
                //    //$('#colorbox').css({ 'visibility': 'visible' }).fadeIn(1000);
                //    $('#cboxWrapper').css('background-color', "#0E83CD");
                //    //$('#cboxLoadedContent').css('background-color', "#0E83CD");
                //}
            });
        }).ajaxSuccess(function (e, xhr, opts) {
        }).ajaxComplete(function (e, xhr, opts) {
        }).ajaxStop(function () {
            $.unblockUI();
            //$('#ajaxProgress').hide();
        });

        //$('.submitBtn').ajaxStart(function () {
        //    $(this).prop('disabled', true);
        //}).ajaxComplete(function () {
        //    $(this).prop('disabled', false);
        //});
    }
};

function lockScreen(isShow) {
    if (isShow === true) {
        $('body div[class*="loading"]').css('display', 'block');
        $('div.loading').show();
    } else {
        $('body div[class*="loading"]').css('display', 'none');
        $('div.loading').hide();
    }
}


function getAjaxErrors(jqXhr, errorThrown) {
    var title = '', message = '';

    if (errorThrown !== null && errorThrown !== '') {
        switch (errorThrown) {
            case 'timeout':
                title = "Time-out Error \n";
                message = "Time out error.";
                break;
            case 'error':
                title = "Ajax Exception \n";
                message = "Error Ajax request";
                break;
            case 'abort':
                title = "Abort Exception \n";
                message = "Ajax request aborted";
                break;
            case 'parsererror':
                title = "Json Parse Error \n";
                message = "Requested JSON parse failed.";
                break;
            default:
                title = jqXhr.status + 'Uncaught Error.\n';
                message = jqXhr.responseText;
                break;
        }
    }
    else {
        if (jqXhr.status !== null && jqXhr.status !== '' && jqXhr.status !== undefined) {
            switch (jqXhr.status) {
                case 0:
                    title = jqXhr.status + " Network Error\n";
                    message = "Not connect.n Verify Network.";
                    break;
                case 401:
                    title = jqXhr.status + " Unauthorized !";
                    message = "Forbidden - Unauthorized Error";
                    break;
                case 403:
                    title = jqXhr.status + " Forbidden!";
                    message = "Sorry, your session has expired.";
                    break;
                case 404:
                    title = jqXhr.status + " Page Not Found !";
                    message = 'Requested page not found. [404]';
                    break;
                case 500:
                    title = jqXhr.status + " Internal Server Error !";
                    message = 'Internal Server Error';
                    break;
                case 503:
                    title = jqXhr.status + " Service Unavailable !";
                    message = 'Service Unavailable';
                    break;
                case 590:
                    title = jqXhr.status + " Unexpected time-out !";
                    message = 'Unexpected time-out';
                    break;
                default:
                    title = jqXhr.status + ' Unexpected error.';
                    message = jqXhr.responseText;
                    break;
            }
        }
    }

    return title + " : " + message;
}

//function handleAjaxLoading() {
//    $(document).on({
//        ajaxStart: function () {
//            lockScreen(true);
//        },
//        ajaxStop: function () {
//            lockScreen(false);
//        },
//        ajaxSuccess: function () {
//            lockScreen(false);
//        },
//        ajaxComplete: function () {
//            lockScreen(false);
//        },
//        ajaxError: function (xhr, textStatus, errorThrown) {
//            handleAjaxErrors(xhr, textStatus, errorThrown);
//            lockScreen(false);
//        }
//    });
//}