(function ($) {
    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                $('#divEdit').html(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    $(document).on("click", ".get-details", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');

        getDetails(url, id);
        return false;
    });
})(jQuery);