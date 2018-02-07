(function ($) {
    var setRatingStar = function (dom, starId, isLoad) {
        return dom.each(function () {
            if (parseInt($('#ratingValue_' + starId).val()) >= parseInt($(this).data('rating'))) {
                if (!isLoad) {
                    $('#starRating_' + starId).css('pointer-events', 'none');
                }

                return $(this).removeClass('fa-star-o').addClass('fa-star');
            } else {
                return $(this).removeClass('fa-star').addClass('fa-star-o');
            }
        });
    };

    var setRating = function () {
        $('.star-rating').each(function () {
            var starId = $(this).attr('star-id');
            var objId = $('#starRating_' + starId + ' .fa');
            setRatingStar(objId, starId, true);
        });
    };

    var updateRating = function (url, itemId, rateValue) {
        var params = {
            customerId: null,
            id: parseInt(itemId),
            rate: rateValue
        };

        $.ajax({
            type: "POST",
            url: url,
            data: params,
            success: function (data) {
                if (data.Status === 0) {
                    $('#ratingValue_' + itemId).val(data.Data.AverageRates);
                    $('#starRating_' + itemId).attr('value', data.Data.AverageRates);

                    var objId = $('#starRating_' + itemId + ' .fa');
                    return setRatingStar(objId, itemId);
                }
                return false;
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    };
    
    var mouseOver = function (elementList, starId) {
        return elementList.each(function () {
            if (parseInt($('#ratingValue_' + starId).val()) >= parseInt($(this).data('rating'))) {
                return $(this).removeClass('fa-star-o').addClass('fa-star');
            } else {
                return $(this).removeClass('fa-star').addClass('fa-star-o');
            }
        });
    };

    $(document).ready(function () {
        setRating();
    }).ajaxComplete(function () {
        setRating();
    });

    $(document).on('mouseover', '.star-rating .fa', function () {
        var starId = $(this).parent().attr('star-id');
        var objId = $('#starRating_' + starId + ' .fa');
        $('#ratingValue_' + starId).val($(this).data('rating'));

        mouseOver(objId, starId);
    }).on('mouseout', '.star-rating .fa', function () {
        var starId = $(this).parent().attr('star-id');
        var objId = $('#starRating_' + starId + ' .fa');

        $('#ratingValue_' + starId).val($('#starRating_' + starId).attr('value'));

        mouseOver(objId, starId);
    }).on('click', '.star-rating .fa', function () {
        var id = $(this).parent().attr('star-id');
        var url = $(this).parent().attr('star-url');
        var rate = $(this).data('rating');
        updateRating(url, id, rate);
    });
})(jQuery);