(function ($) {

    function search() {
        var formUrl = window.ServiceBookingOrderSearchUrl;
        var filterRequest = $("#frmSearch").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: "GET",
            url: formUrl,
            data: filterRequest,
            success: function (data) {
                $('#search-result').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $('#search-result').html('<span>' + textStatus + ", " + errorThrown + '</span>');
            }
        });
        return false;
    }

    function resetControls(form, mode) {
        var validateObj = $('#' + form);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();

        if (mode === 'search') {
            search();
        }
    }

    //search
    $(document).on("click", ".search", function () {
        search();
    });


    $(document).on("click", ".reset", function () {
        var form = $(this).data('form');
        var mode = $(this).data('mode');

        if (mode === 'edit') {
            var id = $(this).data('id');
            var url = $(this).data('url');
            getDetails(url, id);
        } else {
            resetControls(form, mode);
        }
        return false;
    });

})(jQuery);