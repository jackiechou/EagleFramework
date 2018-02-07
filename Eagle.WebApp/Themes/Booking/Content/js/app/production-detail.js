(function ($) {
    var productDetail = {
        getDetails: (url, id) => {
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
            return false;
        },
        resetControls: (form) => {
            var validateObj = $('#' + form);
            validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
            validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
            validateObj.find('input[type="number"]').val(0);
            validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
            validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
            validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();

            var editor = $("#CommentText").data("kendoEditor");
            if (editor)
                editor.value('');
        },
        populateProductComments: () => {
            var formUrl = window.ProductCommentSearchUrl;
            var filterRequest = { "productId": $('#ProductId').val() };
            $.ajax({
                type: "GET",
                url: formUrl,
                data: filterRequest,
                ContentType: 'application/json;utf-8',
                datatype: 'json',
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function (data) {
                    $('#viewComments').html(data);
                    return false;
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    $('#viewComments').html('<span>' + textStatus + ", " + errorThrown + '</span>');
                }
            });
            return false;
        },
        createProductComment: (url, formId) => {
            var params = $("#" + formId).serialize();

            $.ajax({
                type: 'POST',
                url: url,
                data: params,
                dataType: "json",
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function (response, textStatus, jqXhr) {
                    if (response.Status === 0) {
                        productDetail.populateProductComments();
                        productDetail.resetControls(formId);
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
        },
        getComparativeProducts: () => {
            var formUrl = window.GetComparativeProductsUrl;
            var filterProducts = { "categoryId": $('#comparativeCategoryId').val(), 'page': 1, "price": $('#comparativePrice').val() };
            $.ajax({
                type: "GET",
                url: formUrl,
                data: filterProducts,
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function (data) {
                    $("#manufacturer-slide .carousel-inner").html(data);

                    $("#manufacturer-slide").carousel({
                        // Amount of time to delay between cycling slide, If false, no cycle
                        interval: 500,

                        // Pauses slide on mouseenter and resumes on mouseleave.
                        pause: "hover",

                        // Whether carousel should cycle continuously or have hard stops.
                        wrap: true,

                        // Whether the carousel should react to keyboard events.
                        keyboard: true
                    });
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    $(".carousel-inner").html('<span>' + textStatus + ", " + errorThrown + '</span>');
                }
            });
            return false;
        },
        init: () => {
            //productDetail.populateProductComments();
            productDetail.getComparativeProducts();
            productDetail.bindEvents();
        },
        bindEvents: () => {
            $(document).on("click", ".createProductComment", function (e) {
                e.preventDefault();

                var form = $(this).data('form');
                var url = $(this).data('url');

                if (!$("#" + form).valid()) { // Not Valid
                    return false;
                } else {
                    productDetail.createProductComment(url, form);
                    return false;
                }
            });

            $(document).on("click", ".editItem", function (e) {
                e.preventDefault();

                var id = $(this).data('id');
                var url = $(this).data('url');

                productDetail.getDetails(url, id);
                return false;
            });

            $(document).on("click", ".reset", function () {
                var form = $(this).data('form');
                productDetail.resetControls(form);
                return false;
            });
        }
    };
    productDetail.init();

    var listCompare = [];
    function appendToStorage(name, data) {
        var old = sessionStorage.getItem(name);
        if (old === null) old = "";
        sessionStorage.setItem(name, old + data + ",");
    }


    function setupEditor(editorId, data) {
        // create Editor from textarea HTML element with default set of tools
        var editor = $("#" + editorId);
        // var value = editor.data("kendoEditor").value();
        editor.kendoEditor({
            tools: [
                "bold",
                "italic",
                "underline",
                //"strikethrough",
                //"justifyLeft",
                //"justifyCenter",
                //"justifyRight",
                //"justifyFull",
            ],
            value: data,
        });
    }

    setupEditor('CommentText', '');


    //Product Detail Page - Add item(s) to compare
    $(document).on("click", ".addToCompare", function () {
        var productId = $(this).attr("data-id");

        if ($(this).is(':checked')) {
            if (listCompare.length === 0) {
                sessionStorage.setItem("listCompare", "");
            }

            var smallIcon = $(this).attr("data-path");
            var pro = { productId: productId, smallIcon: smallIcon };
            listCompare.push(pro);
            appendToStorage("listCompare", productId);
        } else {
            listCompare = jQuery.grep(listCompare, function (value) {
                return value.productId !== productId;
            });
        }
        var content = "";
        for (var i = 0; i < listCompare.length; i++) {
            content += '<span class="item_pare" id="' + listCompare[i].productId + '"><img width = "40px" height="40px" src="' + listCompare[i].smallIcon + '" /> </span>';
        }
        $("#listcompare").html(content);
    });

    $("#compare-products").click(function () {
        if (listCompare.length > 0) {
            window.location.href = '/Production/CompareProducts/' + sessionStorage.getItem("listCompare");
        } else {
            sessionStorage.setItem("listCompare", "");
        }
    });

    $(document).on("click", ".toggle-accordion", function () {
        var accordionId = $(this).attr("accordion-id"),
            numPanelOpen = $(accordionId + ' .collapse.in').length;
        $(this).toggleClass("active");
        $(accordionId + ' .view.panel-collapse:not(".in")').collapse('show');
    });

    $(document).on("click", ".feedback-accordion", function () {
        var accordionId = $(this).attr("accordion-id"),
            numPanelOpen = $(accordionId + ' .collapse.in').length;
        $(this).toggleClass("active");
        $(accordionId + ' .post.panel-collapse:not(".in")').collapse('show');
    });

    $(document).on("click", ".all-accordion", function () {
        var accordionId = $(this).attr("accordion-id"),
            numPanelOpen = $(accordionId + ' .collapse.in').length;
        $(this).toggleClass("active");
        if (numPanelOpen === 0) {
            $(accordionId + ' .panel-collapse:not(".in")').collapse('show');
        } else {
            $(accordionId + ' .panel-collapse.in').collapse('hide');
        }

    });

    $(document).on("click", ".gotochat", function () {
        var $target = $('html,body');
        $target.animate({ scrollTop: $target.height() }, 'fast');
        return false;
    });

})(jQuery);