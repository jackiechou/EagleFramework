(function ($) {
    var productsByCategory = {
        categories: [],
        getCategories: () => {
            var url = window.GetProductCategorySelectTree;
            var params = {
                "selectedId": null,
                "isRootShowed": true
            };

            productsByCategory.categories = $.ajax({
                type: 'GET',
                dataType: "json",
                url: url,
                data: params,
                async: false
            }).responseJSON;

            return false;
        },
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
        },
        populateCategoryTree: () => {
            //Category Tree
            var tree = $("#categoryTree");
            if (tree !== null && tree !== undefined) {
                var data = productsByCategory.categories;
                //console.log(data);
                tree.tree({
                    lines: false,
                    animate: false,
                    data: data,
                    //formatter: function (node) {
                    //    var s = node.text;
                    //    if (node.children) {
                    //        s += '&nbsp;<span style=\'color:blue\'>(' + node.children.length + ')</span>';
                    //    }
                    //    return s;
                    //},
                    onClick: function (node) {
                        var selectedId = node.id;
                        console.log(selectedId);
                        if (selectedId !== null && selectedId > 0) {
                            productsByCategory.populateProductsByCategory(selectedId);
                        }
                    }
                });
            }
        },
        populateProductsByCategory: (categoryId) => {
            var url = window.GetProductsByCategoryUrl;
            var data = { "categoryId": categoryId };
            $.ajax({
                type: "GET",
                url: url,
                data: data,
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function (data) {
                    $('#search-result').html(data);
                    return false;
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    $('#search-result').html('<span>' + textStatus + ", " + errorThrown + '</span>');
                    return false;
                }
            });
        },
        init: () => {
            productsByCategory.getCategories();
            productsByCategory.populateCategoryTree();
            productsByCategory.bindEvents();
        },
        bindEvents: () => {
            $(document).on("click", ".editItem", function (e) {
                e.preventDefault();

                var id = $(this).data('id');
                var url = $(this).data('url');

                productsByCategory.getDetails(url, id);
                return false;
            });

            $(document).on("click", ".reset", function () {
                var form = $(this).data('form');
                productsByCategory.resetControls(form);
                return false;
            });
        }
    };
    productsByCategory.init();

})(jQuery);