(function ($) {
    var productsByCategory = {
        categories: [],
        getCategories: () => {
            var url = window.GetProductCategorySelectTree;
            var params = {
                "selectedId": null,
                "isRootShowed": false
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
        populateCategoryTree: () => {
            //Category Tree
            var tree = $("#cateTree");
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
                            var url = window.ProductsByCategory + "?categoryId=" + selectedId;
                            window.location.href = url;
                        }
                    }
                });
            }
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


                return false;
            });
        }
    };
    productsByCategory.init();

})(jQuery);