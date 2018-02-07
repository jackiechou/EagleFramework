$(document).ready(function () {
    var emptyCart = $('#empty-cart-warning');

    function decrementVal(selector) {
        var currentValue = selector.val();
        if (parseInt(currentValue) - 1 <= 0)
            selector.attr("value", 1);
        else
            selector.attr("value", parseInt(currentValue) - 1);
    }

    function incrementVal(selector) {
        var currentValue = selector.val();
        var unitsinstock = selector.data('unitsinstock');
        // console.log(currentValue);
        if (unitsinstock !== null && unitsinstock !== undefined && unitsinstock > 0) {
            if (parseInt(currentValue) + 1 >= unitsinstock) {
                selector.attr("value", unitsinstock);
            } else {
                selector.attr("value", parseInt(currentValue) + 1);
            }
        } else {
            selector.attr("value", parseInt(currentValue) + 1);
        }
    }

    function loadCartInfo() {
        $.ajax({
            type: 'GET',
            url: "/Cart/LoadCartInfo",
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                $('.cart-container').html(data);
            }
        });
        return false;
    }

    function addItems(type, productId, quantity) {
        $.ajax({
            type: 'POST',
            url: "/Cart/Add",
            data: { id: productId, quantity: quantity, type: type },
            beforeSend: function () {
                $.unblockUI();
            },
            success: function () {
                loadCartInfo();
            }
        });
    }

    function removeCartItem(e) {
        var pid = $(e).attr("data-id");
        var tr = $(e).parents("tr");// find <tr> which contains <img> clicked
        $.ajax({
            type: 'POST',
            url: "/Cart/Remove",
            data: { id: pid },
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                if (data.length !== 0) {
                    emptyCart.hide();
                    loadCartInfo();
                }
                $("#" + pid).html("$" + data.SubTotal);
                $("#total").html("$" + data.Total);
                tr.hide(500);
            }
        });
        return false;
    }

    function clearCart() {
        $.ajax({
            type: 'POST',
            url: "/Cart/Clear",
            beforeSend: function () {
                $.unblockUI();
            },
            success: function () {
                loadCartInfo();
            }
        });
        return false;
    }

    //Update quantity
    function updateQuantity(e) {
        var pid = e.attr("data-id");
        var qty = e.val();
        //console.log('ProductId: '+ pid + '--- Quantity : ' + qty);
        if (pid !== null && pid > 0 && qty !== null && qty > 0) {
            $.ajax({
                type: 'POST',
                url: "/Cart/Update",
                data: { id: pid, quantity: qty },
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function (data) {
                    // Populate to new price
                    var amountElement = $("#amount-" + pid);
                    if (amountElement !== null && amountElement !== undefined) {
                        var grossPrice = amountElement.data('grossprice');
                        var amount = grossPrice * qty;
                        $("#amount-" + pid).html("$" + amount);
                    }
                    // If any items is wrong 
                    data.Items.forEach(function (product, index) {
                        if (product.Status === 0) {
                            // OK fine
                            $('.item-warning-status-' + product.Id).addClass("hidden-cart-item-status");
                            $('.item-error-status-' + pid).addClass("hidden-cart-item-status");
                            // Clear warning

                        } else if (product.Status === 4) {
                            // Warning
                            $('.item-error-status-' + pid).addClass("hidden-cart-item-status");
                            $('.item-warning-status-' + product.Id).removeClass("hidden-cart-item-status");
                        } else {
                            // Error
                            $('.item-warning-status-' + product.Id).addClass("hidden-cart-item-status");
                            $('.item-error-status-' + product.Id).removeClass("hidden-cart-item-status");
                        }
                    });

                    $("#sub-total").html("$" + data.SubTotal);
                    $("#total").html("$" + data.Total);
                    loadCartInfo();
                }
            });
        }

        return false;
    }

    //e is button
    function incrementQty(e) {
        var quantityElement = $(e).siblings('.quantity');
        incrementVal(quantityElement);
        updateQuantity(quantityElement);
    }

    //e is button
    function decrementQty(e) {
        var quantityElement = $(e).siblings('.quantity');
        decrementVal(quantityElement);
        updateQuantity(quantityElement);
    }

    // Remove this item from cart
    $(document).on("click", ".remove-item-from-cart", function () {
        removeCartItem(this);
        return false;
    });

    $(document).on("click", ".increment-quantity", function () {
        incrementQty(this);
        return false;
    });

    $(document).on("click", ".decrement-quantity", function () {
        decrementQty(this);
        return false;
    });

    $(document).on("click", ".add-qty-1", function () {
        var quantityElement = $(this).siblings('.quantity');
        incrementVal(quantityElement);
        return false;
    });

    $(document).on("click", ".subtract-qty-1", function () {
        var quantityElement = $(this).siblings('.quantity');
        decrementVal(quantityElement);
        return false;
    });

    $(document).on("change keyup", ".update-qty", function () {
        updateQuantity($(this));
        return false;
    });

    function showModal(id) {
        var modal = $('#myModal');
        var modalDialog = modal.find('.modal-dialog');
        modalDialog.css({
            'top': '15%',
            'margin-top': function () {
                return -($(this).height() / 2);
            }
        });

        $.ajax({
            url: "/Cart/GetItemDetail/" + id,
            type: "GET",
            dataType: "html",
            success: function (data) {
                var modalTitle = 'The product has been added to your cart';
                modal.find('.modal-title').text(modalTitle);
                modal.find('.modal-body').html(data);
                modal.modal('show');

                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
                modal.modal('hide');
            }
        });
    }

    //Product List Page - Add product to cart
    $(".add-one-item-to-cart").unbind('click').click(function () {
        var productId = $(this).attr("data-id");
        var type = $(this).attr("data-type");

        addItems(type, productId, 1);
        showModal(productId);
        return false;
    });
    $(".add-item-to-cart").unbind('click').click(function () {
        var productId = $(this).attr("data-id");
        var type = $(this).attr("data-type");

        var qtyContainer = $(this).siblings('.select-quantity');
        var quantity = qtyContainer.find('input').val();
       
        addItems(type, productId, quantity);
        showModal(productId);
        return false;
    });

    //Product Detail Page - Add item(s) to cart
    $(".add-items-to-cart").unbind('click').click(function () {
        var productId = $(this).attr("data-id");
        var type = $(this).attr("data-type");
        var quantity = $(this).siblings('.quantity').val();

        addItems(type, productId, quantity);
        showModal(productId);
        return false;
    });

    $(".increment-by-one").unbind('click').click(function () {
        var quantityElement = $(this).parent().siblings('input');
        var currentValue = quantityElement.val();
        var newValue = parseInt(currentValue) + 1;

        quantityElement.attr("value", newValue);
        return false;
    });

    $(".decrement-by-one").unbind('click').click(function () {
        var quantityElement = $(this).parent().siblings('input');
        var currentValue = quantityElement.val();
        if (parseInt(currentValue) - 1 <= 0)
            quantityElement.attr("value", 1);
        else
            quantityElement.attr("value", parseInt(currentValue) - 1);
        return false;
    });

    $(document).on("click", "#cart-info .close-cart-info", function () {
        $('#cart-info').css('display', 'none');
    });

    $(document).on("click", "#cart-info-wishlist .close-cart-info", function () {
        $('#cart-info-wishlist').css('display', 'none');
    });

    $(document).on("click", ".clear-cart", function () {
        clearCart();
        return false;
    });

    $(document).on("click", "#continue-cart", function () {
        var url = $(this).data('url');
        location.href = url;
        return false;
    });

    $(document).on("click", "#create-bill", function () {
        var url = $(this).data('url');
        location.href = url;
        return false;
    });
});