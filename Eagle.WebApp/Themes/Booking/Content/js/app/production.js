(function ($) {
    var product = {
        categories: [],
        getDetails: (url, id) => {
            $.ajax({
                type: "GET",
                url: url,
                data: { "id": id },
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function(data) {
                    $('#divEdit').html(data);
                },
                error: function(jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                }
            });
            return false;
        },
        resetControls: (form) => {
            var validateObj = $('#' + form);
            validateObj.find('input:text,input:hidden, input:password, input:file, select, textarea').not('.ignored').val('');
            validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
            validateObj.find('input[type="number"]').val(0);
            validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
            validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
            validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();

            //reset easy ui
            var easyuiSlider = $('#filter-price-slider');
            easyuiSlider.slider('clear');
            var options = easyuiSlider.slider('options');
            $('#filter-price-min').val(options.min);
            $('#filter-price-max').val(options.max);

            product.search();
        },
        search: () => {
            var formUrl = window.ProductSearchUrl;
            //var filterRequest = $("#frmSearch").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
            var filterRequest = $("#frmSearch").serialize();
            $.ajax({
                type: "GET",
                url: formUrl,
                data: filterRequest,
                ContentType: 'application/json;utf-8',
                datatype: 'json',
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function(data) {
                    $('#search-result').html(data);
                    return false;
                },
                error: function(jqXhr, textStatus, errorThrown) {
                    $('#search-result').html('<span>' + textStatus + ", " + errorThrown + '</span>');
                }
            });
            return false;
        },
        getCategories: () => {
            var url = window.GetProductCategorySelectTree;
            var params = {
                "selectedId": null,
                "isRootShowed": false
            };

            product.categories = $.ajax({
                type: 'GET',
                dataType: "json",
                url: url,
                data: params,
                async: false
            }).responseJSON;

            return false;
        },
        populateCategoryComboTree: () => {
            var select = $("#CategoryId");
            var data = product.categories;

            select.combotree({
                data: data,
                valueField: 'id',
                textField: 'text',
                required: false,
                editable: false,
                method: 'get',
                panelHeight: 'auto',
                checkbox: true,
                children: 'children',
                onLoadSuccess: function(row, data) {
                    $(this).tree("expandAll");
                    select.combotree('setValues', [0]);
                },
                onClick: function(node) {
                    $(this).val(node.id);
                    product.search();
                }
            });
        },
        populateCategoryTree: () => {
            //Category Tree
            var tree = $("#categoryTree");
            if (tree !== null && tree !== undefined) {
                var data = product.categories;
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
                        $('input[type="hidden"][name="CategoryId"]').val(selectedId);
                        $("#CategoryId").combotree('setValue', selectedId);
                        product.search();
                    }
                });
            }
        },
        populateDiscountedProducts:() => {
            $('#discounted-products-slider').nivoSlider({
                effect: 'random',
                slices: 15,
                boxCols: 8,
                boxRows: 4,
                animSpeed: 500,
                pauseTime: 3000,
                startSlide: 0,
                directionNav: true,
                controlNav: true,
                controlNavThumbs: false,
                pauseOnHover: true,
                manualAdvance: true,
                manualCaption: true, // Manual caption placement
                //prevText: 'Prev',
                //nextText: 'Next',
                //randomStart: false,
                //beforeChange: function () { },
                //afterChange: function () { },
                //slideshowEnd: function () { },
                //lastSlide: function () { },
                //afterLoad: function () { }
            });
        },
        getManufacturers:() => {
            var formUrl = window.GetManufacturerUrl;

            $.ajax({
                type: "GET",
                url: formUrl,
                data: {'page':1},
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function (data) {
                    $(".carousel-inner").html(data);

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


                    //$('.photo_link').magnificPopup({
                    //    //delegate: 'a', // the selector for gallery item
                    //    type: 'image',
                    //    closeOnContentClick: true,
                    //    closeBtnInside: false,
                    //    gallery: {
                    //        enabled: true,
                    //        tCounter: '%curr% of %total%'
                    //    }
                    //});

                    //$('.caption').magnificPopup({
                    //    type: 'image',
                    //    closeOnContentClick: true,
                    //    closeBtnInside: false,
                    //    gallery: {
                    //        enabled: true,
                    //        tCounter: '%curr% of %total%'
                    //    }
                    //});
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    $(".carousel-inner").html('<span>' + textStatus + ", " + errorThrown + '</span>');
                }
            });
            return false;
        },
        init: () => {
            product.populateDiscountedProducts();
            product.getCategories();
            product.populateCategoryComboTree();
            product.populateCategoryTree();
            product.getManufacturers();
           // product.search(); //Refactor search
            product.bindEvents();
        },
        bindEvents: () => {
            //search
            $(document).on("click", ".search", function () {
                product.search();
                return false;
            });

            $(document).on("click", ".editItem", function (e) {
                e.preventDefault();

                var id = $(this).data('id');
                var url = $(this).data('url');

                product.getDetails(url, id);
                return false;
            });

            $(document).on("click", ".reset", function () {
                var form = $(this).data('form');
                product.resetControls(form);
                return false;
            });

            $('#filter-price-slider').slider({
                onComplete: (value) => {
                    // Populate into 2 hidden fiels 
                    $('#filter-price-min').val(value[0]);
                    $('#filter-price-max').val(value[1]);
                    product.search();
                }
            });
        }
    };
    product.init();
   
})(jQuery);