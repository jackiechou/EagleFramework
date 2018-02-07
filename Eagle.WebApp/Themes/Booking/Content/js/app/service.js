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

    $("img.lazy").lazyload();

    //Hanle Category Tab
    function handleCategoryTab() {
        $.ajax({
            type: 'GET',
            url: window.GetServicePacksByCategoryId,
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                $('#list-container').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $('#list-container').html('<span>' + textStatus + ", " + errorThrown + '</span>');
                return false;
            }
        });
    }

    $(document).on("click", "a.categorytab", function (e) {
        // e.preventDefault();
        handleCategoryTab();
        return false;
    });
})(jQuery);