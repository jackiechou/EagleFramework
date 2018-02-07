(function ($) {
    $("#discounted-product-container").carousel({
        vertical: true
    });

    $(".news-ticker").bootstrapNews({
        newsPerPage: 4,
        navigation: true,
        autoplay: true,
        pauseOnHover: true,
        direction: 'up',
        animationSpeed: 'normal',
        newsTickerInterval: 2000,//2 secs
        onStop: null,
        onPause: null,
        onReset: null,
        onPrev: null,
        onNext: null,
        onToDo: function() {
            //console.log(this);
        }
    });

    //Create topic tree for searching -Category ComboTree
    function populateCategoryComboTree() {
        var select = $("#CategoryId");
        var url = select.data('url');
        var selectedValue = select.val();

        var params = {
            "selectedId": selectedValue,
            "isRootShowed": true
        };

        $.ajax({
            async: false,
            cache: false,
            type: 'GET',
            dataType: "json",
            url: url,
            data: params,
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            },
            success: function (data) {
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
                    onLoadSuccess: function (row, data) {
                        $(this).tree("expandAll");
                        //select.combotree('setValues', [0]);
                    },
                    onClick: function (node) {
                        selectedValue = node.id;
                        $(this).val(selectedValue);
                    }
                });
            }
        });
    }
    populateCategoryComboTree();

    ////PAGINATION
    function search() {
        var filterRequest = $("#frmSearch").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: 'GET',
            url: window.SearchNewsUrl,
            data: filterRequest,
            ContentType: 'application/json;utf-8',
            datatype: 'json',
            success: function (data) {
                $("#gridcontainer").html(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $("#gridcontainer").html('<span>' + textStatus + ", " + errorThrown + '</span>');
            }
        });
    }

    $(document).on("click", ".search", function () {
        search();
    });
})(jQuery);