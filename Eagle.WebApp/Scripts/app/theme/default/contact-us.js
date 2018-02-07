$(document).ready(function () {

    //CAPTCHA ==================================================================================================
    function getRandomText(count) {
        var allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
        allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
        allowedChars += "1,2,3,4,5,6,7,8,9,0";

        var arr = allowedChars.split(",");

        var passwordString = "";
        var temp = "";

        for (var i = 0; i < count; i++) {
            var rand = Math.random();
            temp = Math.ceil(rand * (arr.length - 1));
            passwordString += arr[temp];
        }
        return passwordString;
    }

    function getCaptchaImage(numChar) {
        var randomText = getRandomText(numChar);
        $('.captcha-image').remove();
        $(".captcha-text").val(randomText);
        $('.captcha-text-confirm').val('');

        var containerForImg = $('.captcha-image-container');
        var url = containerForImg.data('url');
        var width = containerForImg.data('width');
        var height = containerForImg.data('height');

        var captchaImageUrl = url + '?captchatext=' + randomText + '&width=' + width + '&height=' + height;
        var img = $('<img />', {
            id: 'dynamicImage',
            src: captchaImageUrl,
            width: width,
            height: height
        }).addClass("captcha-image").appendTo(containerForImg);

        img.onError = function () {
            img.src = '/images/no-image.png';
        }
    }

    getCaptchaImage(6);

    $(document).on("click", ".get-captcha", function () {
        getCaptchaImage(6);
        return false;
    });
    //==========================================================================================================

    function resetControls(form) {
        var validateObj = $('#' + form);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

    function createFeedback(url, form) {
        var formData = $("#" + form).serialize();

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    resetControls(form);
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    }

    $(document).on("click", ".createFeedback", function (e) {
        e.preventDefault();

        var form = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        } else {
            var captchaText = $('.captcha-text').val();
            var captchaTextConfirm = $('.captcha-text-confirm').val();
            if (captchaText.toLowerCase() !== captchaTextConfirm.toLowerCase()) {
                showMessageWithTitle(500, 'Captcha is valid', "error", 50000);
                return false;
            } else {
                createFeedback(url, form);
                return false;
            }
        }
    });

    $('.download a').click(function () {
        if (this.classList.contains('blue')) {
            this.classList.remove('blue');
        } else {
            this.classList.add('blue');
        }
    });
});