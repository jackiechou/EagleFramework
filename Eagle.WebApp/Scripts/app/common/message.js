function showAlertMessageBox(formId, title, msg, errorType, timeout) {
    var cssClass = '', strTitle = '', strMsg = '';

    if (errorType === undefined || errorType === '') {
        errorType = 'error';
        cssClass = 'alert alert-danger';
        strTitle = ((title !== '' && title !== undefined) ? title : 'Error');
        strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Error');
    } else {
        var type = errorType.toString().toLowerCase();
        switch (type) {
            case 'error':
                cssClass = 'alert alert-danger';
                strTitle = ((title !== '' && title !== undefined) ? title : 'Error');
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Error');
                break;
            case 'success':
                cssClass = 'alert alert-success';
                strTitle = ((title !== '' && title !== undefined) ? title : 'Success !');
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Success');
                break;
            case 'warning':
                cssClass = 'alert alert-warning';
                strTitle = ((title !== '' && title !== undefined) ? title : 'Warning');
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Warning');
                break;
            case 'info':
                cssClass = 'alert alert-info';
                strTitle = ((title !== '' && title !== undefined) ? title : 'Info');
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Info');
                break;
            default:
                cssClass = 'alert alert-info';
                strTitle = ((title !== '' && title !== undefined) ? title : 'Info');
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Info');
                break;
        }
        var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
            + '<strong>' + strTitle + ' :</strong><br/> ' + strMsg + '</div>';
   
        var form = $('#' + formId);
        var container = form.find('.message-box');
        var status = container.attr("aria-hidden");
        if (status === "true") {
            container.attr("aria-hidden", "false");
        }
        container.css({ 'display': 'block' });
        container.attr({
            "class": "open",
            "aria-hidden": "false"
        });
        container.html(message);

        if (timeout != undefined) {
            setTimeout(function () {
                if (container != undefined && container != null) {
                    container.attr({
                        "class": "hide",
                        "aria-hidden": "true"
                    });
                    container.css({ 'display': 'none' });
                }
            }, timeout);
        }
    }
}
function showMessageWithTitle(title, msg, errorType, timeout) {
    var cssClass = '', strTitle = '', strMsg = '';

    if (errorType === undefined || errorType === '') {
        errorType = 'error';
        cssClass = 'alert alert-danger';
        strTitle = ((title !== '' && title !== undefined) ? title : 'Error');
        strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Error');
    } else {
        var type = errorType.toString().toLowerCase();
        switch (type) {
            case 'error':
                cssClass = 'alert alert-danger';
                strTitle = ((title !== '' && title !== undefined) ? title : 'Error');
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Error');
                break;
            case 'success':
                cssClass = 'alert alert-success';
                strTitle = ((title !== '' && title !== undefined) ? title : 'Success !');
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Success');
                break;
            case 'warning':
                cssClass = 'alert alert-warning';
                strTitle = ((title !== '' && title !== undefined) ? title : 'Warning');
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Warning');
                break;
            case 'info':
                cssClass = 'alert alert-info';
                strTitle = ((title !== '' && title !== undefined) ? title : 'Info');
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Info');
                break;
            default:
                cssClass = 'alert alert-info';
                strTitle = ((title !== '' && title !== undefined) ? title : 'Info');
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Info');
                break;
        }

        var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
            + '<strong>' + strTitle + ' :</strong><br/> ' + strMsg + '</div>';
        var container = document.getElementById("alertMessageBox");
        if (container != undefined) {
            var status = container.getAttribute("aria-hidden");
            if (status === "true") {
                container.setAttribute("aria-hidden", "false");
            }
            container.style.display = "block";
            container.className = "open";
            container.setAttribute("aria-hidden", "false");
            container.innerHTML = message;
        }

        if (timeout != undefined) {
            setTimeout(function () {
                if (container != undefined && container != null) {
                    container.className = "hide";
                    container.setAttribute("aria-hidden", "true");
                    container.style.display = "none";
                }
            }, timeout);
        }
    }
}

function showMessageWithTitleWithoutTimeOut(title, msg, errorType) {
    var cssClass = '';

    if (title == undefined || title === '')
        title = 'Error';
    if (msg == undefined || msg === '')
        msg = 'Error';
    if (errorType == undefined || errorType === '') {
        errorType = 'error';
        cssClass = 'alert alert-danger';
    }
    if (errorType.toString().toLowerCase() === "error")
        cssClass = 'alert alert-danger';
    if (errorType.toString().toLowerCase() === "success")
        cssClass = 'alert alert-success';
    if (errorType.toString().toLowerCase() === "warning")
        cssClass = 'alert alert-warning';
    if (errorType.toString().toLowerCase() === "info")
        cssClass = 'alert alert-info';

    var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
                                + '<strong>' + title + ' :</strong><br/> ' + msg + '</div>';
    var container = document.getElementById("alertMessageBox");
    if (container != undefined) {
        var status = container.getAttribute("aria-hidden");
        if (status === "true") {
            container.setAttribute("aria-hidden", "false");
        }
        container.style.display = "block";
        container.className = "open";
        container.setAttribute("aria-hidden", "false");
        container.innerHTML = message;
    }
    //$('html, body').animate({ scrollTop: 80 }, 'slow');
}

function showMessageBox(msg, errorType) {
    var cssClass = '';
    var title = '';

    if (msg == undefined || msg === '') {
        msg = 'Error';
    }
    if (errorType == undefined || errorType === '') {
        errorType = 'error';
        cssClass = 'alert alert-danger';
        title = 'Error';
    }
    if (errorType.toString().toLowerCase() === "error") {
        cssClass = 'alert alert-danger';
        title = 'Error';
    }
    if (errorType.toString().toLowerCase() === "success") {
        cssClass = 'alert alert-success';
        title = 'Success !';
    }
    if (errorType.toString().toLowerCase() === "warning") {
        cssClass = 'alert alert-warning';
        title = 'Warning';
    }
    if (errorType.toString().toLowerCase() === "info") {
        cssClass = 'alert alert-info';
        title = 'Info';
    }

    var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
                                + '<strong>' + title + ' :</strong><br/> ' + msg + '</div>';
    var container = document.getElementById("alertMessageBox");
    if (container != undefined) {
        var status = container.getAttribute("aria-hidden");
        if (status === "true") {
            container.setAttribute("aria-hidden", "false");
        }
        container.style.display = "block";
        container.className = "open";
        container.setAttribute("aria-hidden", "false");
        container.innerHTML = message;
    }
}

function showMessagePopUpWithTitleWithoutTimeout(title, msg, errorType) {
    var cssClass = '';

    if (title == undefined || title === '')
        title = 'Error';
    if (msg == undefined || msg === '')
        msg = 'Error';
    if (errorType == undefined || errorType === '') {
        errorType = 'error';
        cssClass = 'alert alert-danger';
    }
    if (errorType.toLowerCase() === "error")
        cssClass = 'alert alert-danger';
    if (errorType.toString().toLowerCase() === "success")
        cssClass = 'alert alert-success';
    if (errorType.toString().toLowerCase() === "warning")
        cssClass = 'alert alert-warning';
    if (errorType.toString().toLowerCase() === "info")
        cssClass = 'alert alert-info';

    var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
                                + '<strong>' + title + ' :</strong><br/> ' + msg + '</div>';
    var container = document.getElementById("alertMessagePopUp");
    if (container != undefined) {
        var status = container.getAttribute("aria-hidden");
        if (status === "true") {
            container.setAttribute("aria-hidden", "false");
        }
        container.style.display = "block";
        container.className = "open";
        container.setAttribute("aria-hidden", "false");
        container.innerHTML = message;
    }
}

function showMessagePopUpWithTitle(title, msg, errorType, timeout) {
    var cssClass = '';

    if (title == undefined || title === '')
        title = 'Error';
    if (msg == undefined || msg === '')
        msg = 'Error';
    if (errorType === undefined || errorType === null || errorType === '') {
        errorType = 'error';
        cssClass = 'alert alert-danger';
    }
    if (errorType.toString().toLowerCase() === "error")
        cssClass = 'alert alert-danger';
    if (errorType.toString().toLowerCase() === "success")
        cssClass = 'alert alert-success';
    if (errorType.toString().toLowerCase() === "warning")
        cssClass = 'alert alert-warning';
    if (errorType.toString().toLowerCase() === "info")
        cssClass = 'alert alert-info';

    var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
                                + '<strong>' + title + ' :</strong><br/> ' + msg + '</div>';
    var container = document.getElementById("alertMessagePopUp");
    if (container != undefined) {
        var status = container.getAttribute("aria-hidden");
        if (status === "true") {
            container.setAttribute("aria-hidden", "false");
        }
        container.style.display = "block";
        container.className = "open";
        container.setAttribute("aria-hidden", "false");
        container.innerHTML = message;
    }


    if (timeout != undefined) {
        setTimeout(function () {
            if (container != undefined && container != null) {
                container.className = "hide";
                container.style.display = "none";
                container.setAttribute("aria-hidden", "true");
            }
        }, timeout);
    }

}

function showMessageBoxWithTitle(title, msg, errorType, timeout) {
    var cssClass = '', strTitle = '', strMsg = '';

    if (errorType == undefined || errorType === '') {
        errorType = 'error';
        cssClass = 'alert alert-danger';
        strTitle = ((title !== '' && title !== undefined) ? title : 'Error');
        strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Error');
    }
    if (errorType.toString().toLowerCase() === "error") {
        cssClass = 'alert alert-danger';
        strTitle = ((title !== '' && title !== undefined) ? title : 'Error');
        strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Error');
    }
    if (errorType.toString().toLowerCase() === "success") {
        cssClass = 'alert alert-success';
        strTitle = ((title !== '' && title !== undefined) ? title : 'Success !');
        strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Success');
    }
    if (errorType.toString().toLowerCase() === "warning") {
        cssClass = 'alert alert-warning';
        strTitle = ((title !== '' && title !== undefined) ? title : 'Warning');
        strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Warning');
    }
    if (errorType.toString().toLowerCase() === "info") {
        cssClass = 'alert alert-info';
        strTitle = ((title !== '' && title !== undefined) ? title : 'Info');
        strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Info');
    }

    var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
                               + '<strong>' + strTitle + ' :</strong><br/> ' + strMsg + '</div>';
    var container = document.getElementById("alertMessageBox");
    if (container != undefined) {
        var status = container.getAttribute("aria-hidden");
        if (status === "true") {
            container.setAttribute("aria-hidden", "false");
        }
        container.style.display = "block";
        container.className = "open";
        container.setAttribute("aria-hidden", "false");
        container.innerHTML = message;
    }

    if (timeout != undefined) {
        setTimeout(function () {
            if (container != undefined && container != null) {
                container.attr({ "class": "hide", "aria-hidden": "true" });
                container.css("display", "none");
            }
        }, timeout);
    }
}

function showMessageWithTitleByContainerId(dvContainerId, title, msg, errorType, timeout) {
    var cssClass = '';

    if (title == undefined || title === '')
        title = 'Error';
    if (msg == undefined || msg === '')
        msg = 'Error';
    if (errorType == undefined || errorType.toString() === '') {
        errorType = 'error';
        cssClass = 'alert alert-danger';
    }
    if (errorType.toString().toLowerCase() === "error")
        cssClass = 'alert alert-danger';
    if (errorType.toString().toLowerCase() === "success")
        cssClass = 'alert alert-success';
    if (errorType.toString().toLowerCase() === "warning")
        cssClass = 'alert alert-warning';
    if (errorType.toString().toLowerCase() === "info")
        cssClass = 'alert alert-info';

    var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
                                + '<strong>' + title + ' :</strong><br/> ' + msg + '</div>';
    var container = $("#" + dvContainerId);
    if (container != undefined) {
        var status = container.attr("aria-hidden");
        if (status === "true")
            container.attr("aria-hidden", "false");
        container.css("display", "block");
        container.attr({ "class": "open", "aria-hidden": "false" });
        container.html(message);
    }

    if (timeout != undefined) {
        setTimeout(function () {
            if (container != undefined && container != null) {
                container.attr({ "class": "hide", "aria-hidden": "true" });
                container.css("display", "none");
            }
        }, timeout);
    }
}

function showMessageWithTitleByContainer(container, title, msg, errorType, timeout) {
    var cssClass = '';

    if (title == undefined || title === '')
        title = 'Error';
    if (msg == undefined || msg === '')
        msg = 'Error';
    if (errorType == undefined || errorType.toString() === '') {
        errorType = 'error';
        cssClass = 'alert alert-danger';
    }
    if (errorType.toString().toLowerCase() === "error")
        cssClass = 'alert alert-danger';
    if (errorType.toString().toLowerCase() === "success")
        cssClass = 'alert alert-success';
    if (errorType.toString().toLowerCase() === "warning")
        cssClass = 'alert alert-warning';
    if (errorType.toString().toLowerCase() === "info")
        cssClass = 'alert alert-info';

    var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
        + '<strong>' + title + ' :</strong><br/> ' + msg + '</div>';

    if (container != undefined) {
        var status = container.attr("aria-hidden");
        if (status === "true")
            container.attr("aria-hidden", "false");
        container.css("display", "block");
        container.attr({ "class": "open", "aria-hidden": "false" });
        container.html(message);
    }

    if (timeout != undefined) {
        setTimeout(function () {
            if (container != undefined && container != null) {
                container.attr({ "class": "hide", "aria-hidden": "true" });
                container.css("display", "none");
            }
        }, timeout);
    }
}

function showMessage(msg, errorType) {
    var cssClass = '';
    var title = '';

    if (msg == undefined || msg === '') {
        msg = 'Error';
    }
    if (errorType === undefined || errorType === '' || errorType === null) {
        cssClass = 'alert alert-danger';
        title = 'Error';
    } else {
        var strErrorType = errorType.toLowerCase();
        switch (strErrorType) {
            case "error":
                cssClass = 'alert alert-danger';
                title = 'Error';
                break;
            case "success":
                cssClass = 'alert alert-success';
                title = 'Success !';
                break;
            case "warning":
                cssClass = 'alert alert-warning';
                title = 'Warning';
                break;
            case "info":
                cssClass = 'alert alert-info';
                title = 'Info';
                break;
            default:
                cssClass = 'alert alert-info';
                title = 'Info';
        }
    }


    var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
                                + '<strong>' + title + ' :</strong><br/> ' + msg + '</div>';
    var container = document.getElementById("alertMessageBox");
    if (container != undefined) {
        var status = container.getAttribute("aria-hidden");
        if (status === "true") {
            container.setAttribute("aria-hidden", "false");
        }
        container.style.display = "block";
        container.className = "open";
        container.setAttribute("aria-hidden", "false");
        container.innerHTML = message;
    }

}

function hideMessage() {
    var container = document.getElementById("alertMessageBox");
    if (container != undefined) {
        container.className = "hide";
        container.style.display = "none";
        container.setAttribute("aria-hidden", "true");
    }
}

function hideMessageWithContainer(dvContainerId) {
    var container = $("#" + dvContainerId);
    container.attr({ "class": "hide", "aria-hidden": "true" });
    container.css("display", "none");
}

function hideMessageWithTitle(delay) {
    var container = document.getElementById("alertMessageBox");
    if (delay == undefined) {
        delay = 0;
    }
    if (container != undefined) {
        setTimeout(function () {
            container.className = "hide";
            container.style.display = "none";
            container.setAttribute("aria-hidden", "true");
        }, delay);
    }
}


function showNotification(errorType, msg) {
    var cssClass = '', strMsg = '';

    if (errorType === undefined || errorType === ''  || errorType ==null) {
        errorType = 'error';
        cssClass = 'alert alert-danger';
        strMsg = (msg !== null && msg !== '' && msg !== undefined) ? msg : 'Error';
    } else {
        var type = errorType.toString().toLowerCase();
        switch (type) {
            case 'error':
                cssClass = 'alert alert-danger';
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Error');
                break;
            case 'success':
                cssClass = 'alert alert-success';
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Success');
                break;
            case 'warning':
                cssClass = 'alert alert-warning';
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Warning');
                break;
            case 'info':
                cssClass = 'alert alert-info';
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Info');
                break;
            default:
                cssClass = 'alert alert-info';
                strMsg = ((msg !== '' && msg !== undefined) ? msg : 'Info');
                break;
        }

        var message = '<div class="' + cssClass + '"><a data-dismiss="alert" class="close" href="#">×</a>'
            + strMsg + '</div>';
        var container = document.getElementById("alertMessageBox");
        if (container != undefined) {
            var status = container.getAttribute("aria-hidden");
            if (status === "true") {
                container.setAttribute("aria-hidden", "false");
            }
            container.style.display = "block";
            container.className = "open";
            container.setAttribute("aria-hidden", "false");
            container.innerHTML = message;
        }

        setTimeout(function () {
            if (container != undefined && container != null) {
                container.className = "hide";
                container.setAttribute("aria-hidden", "true");
                container.style.display = "none";
            }
        }, 50000);
    }
}
