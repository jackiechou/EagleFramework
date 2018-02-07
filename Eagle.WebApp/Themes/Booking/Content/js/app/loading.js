//Usage 1
//var overlay = new Overlay("box");
//overlay.show();
//setTimeout(function () { overlay.hide(); }, 2000);

//Usage 2
//var overlay = new Overlay();
//overlay.show("body");
//setTimeout ( function(){ overlay.hide("body");}, 2000 );

//Usage 3
//var overlay = new Overlay();
//overlay.show("body");
//overlay.hide("body");

function Overlay(id) {
    this.id = id;

    /**
	 * Show the overlay
	 */
    this.show = function (id) {
        if (id) {
            this.id = id;
        }

        // Gets the object of the body tag
        var bgObj = document.getElementById(this.id);

        // Adds a overlay
        var oDiv = document.createElement('div');
        oDiv.setAttribute('id', 'box-loading-overlay');
        oDiv.setAttribute("class", "box-loading-overlay");
        oDiv.style.display = 'block';
        bgObj.appendChild(oDiv);

        // Adds loading
        var lDiv = document.createElement('div');
        lDiv.setAttribute('id', 'box-loading');
        lDiv.setAttribute("class", "box-loading");
        lDiv.style.display = 'block';
        bgObj.appendChild(lDiv);
    }

    /**
	 * Hide the overlay
	 */
    this.hide = function (id) {
        if (id) {
            this.id = id;
        }

        var bgObj = document.getElementById(this.id);

        // Removes loading 
        var elementLoading = document.getElementById('box-loading');
        bgObj.removeChild(elementLoading);

        // Removes a overlay box
        var elementOverlay = document.getElementById('box-loading-overlay');
        bgObj.removeChild(elementOverlay);
    }
}

///////////////////////////////////////////////////////////////////
/////////////////////// Start Processing Ajax /////////////////////
///////////////////////////////////////////////////////////////////
function setOverlayScreen(isShow) {
    if (isShow === true) {
        $('body div[class*="loading"]').css('display', 'block');
        $('div.loading').show();
    } else {
        $('body div[class*="loading"]').css('display', 'none');
        $('div.loading').hide();
    }
}

function handleAjaxLoading() {
    $(document).on({
        ajaxStart: function () {
            setOverlayScreen(true);
        },
        ajaxStop: function () {
            setOverlayScreen(false);
        },
        ajaxSuccess: function () {
            setOverlayScreen(false);
        },
        ajaxComplete: function () {
            setOverlayScreen(false);
        },
        ajaxError: function (xhr, textStatus, errorThrown) {
            handleAjaxErrors(xhr, textStatus, errorThrown);
            setOverlayScreen(false);
        }
    });
}

//Loading Progress
function setupLoading() {
    //setUpLoadingForMenu
    $('a[class="menu-item"]').on('click', function () {
        $('.loading').show();
        $('.loading').fadeIn(3000).delay(2000).fadeOut("slow");
    });

    //Setup link action
    $('a[class="action-link"]').on('click', function () {
        $('.loading').show();
        $('.loading').fadeIn(3000).delay(2000).fadeOut("slow");
    });
}

///////////////////////////////////////////////////////////////////
/////////////////////// End Processing Ajax ///////////////////////
///////////////////////////////////////////////////////////////////