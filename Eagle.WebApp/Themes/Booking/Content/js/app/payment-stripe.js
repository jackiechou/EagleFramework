(function ($) {
    function populateYearSelectList() {
        var select = $("#card-exp-year");
        var d = new Date();

        //get the year
        var year = d.getFullYear();
        var selectedValue = d.getFullYear();

        //pull the last two digits of the year
        var yearText = year.toString().substr(-2);

        for (var i = 0; i <= 10; i++) {
            select.append($('<option/>', {
                value: parseInt(year + i),
                text: parseInt(yearText) + i
            }));
        }

        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
            select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
        } else {
            select.find('option:first').prop('selected', 'selected');
        }
    }

    populateYearSelectList();

    //////limits the input based on the maxlength attribute, this is by default no supported by input type number
    $("#card-exp-month").on('keydown keyup', function () {
        var $that = $(this),
            maxlength = $that.attr('maxlength');
        if ($.isNumeric(maxlength)) {
            $that.val($that.val().substr(0, maxlength));
        };
    });

    $('#card-cvv').keyup(function (event) {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "");
        });
    });

    // Create a token or display an error when the form is submitted.
    function setupStripe(formId) {
        var publicKey = $('#PublicKey').val();

        //Setting the stripe publishable key
        Stripe.setPublishableKey(publicKey);

        var cardNumber = $(".card-number").val();
        var cardCvC = $(".card-cvc").val();
        var cardExpiryYear = $(".card-expiry-year").val();
        var cardExpiryMonth = $(".card-expiry-month").val();
        var cardHolderName = $(".card-cardholder-name").val();


        var card =
        {
            number: cardNumber,
            cvc: cardCvC,
            exp_month: cardExpiryMonth, 
            exp_year: cardExpiryYear,
            name: cardHolderName
        };

        Stripe.createToken(card, function (status, response) {
            if (response.error) {
                // Re-enable the submit button
                $(":submit", form).removeAttr("disabled");

                // show the error
                $(".payment-errors").html(response.error.message);
            } else {
                $("#process-payment").attr("disabled", "disabled");

                // token contains id, last4, and card type
                var token = response["id"];
               // console.log(token);

                $("#PaymentToken").val(token);

                if (token !== null && token !== '') {
                   // processPayment(url, formId);
                    $('#' + formId).submit();
                }

                return false;
            }
        });

        return false;
    }

    //function processPayment(url, formId) {
    //    var myForm = $('#' + formId);
    //    //var params = myForm.serialize();

    //    // Show full page LoadingOverlay
    //    $.LoadingOverlay("show", {
    //        image: "",
    //        fontawesome: "fa fa-spinner fa-spin",
    //        custom: $("<div>", {
    //            id: "loading-text",
    //            css: {
    //                "font-size": "25px"
    //            },
    //            text: "PAYMENT PROCESSING..."
    //        })
    //    });
    //    myForm.submit();  
    //    //
    //    //$.ajax({
    //    //    type: 'POST',
    //    //    url: url,
    //    //    data: params,
    //    //    dataType: "json",
    //    //    //beforeSend: function () {
    //    //    //    $.unblockUI();
    //    //    //},
    //    //    success: function (response, textStatus, jqXhr) {
    //    //        if (response.Status === 0) {
    //    //            showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
    //    //           // window.location.href = 
    //    //        } else {
    //    //            if (response.Errors !== null) {
    //    //                var result = '';
    //    //                $.each(response.Errors, function (i, obj) {
    //    //                    result += obj.ErrorMessage + '<br/>';
    //    //                });
    //    //                showMessageWithTitle(500, result, "error", 50000);
    //    //            }
    //    //        }
    //    //    }, error: function (jqXhr, textStatus, errorThrown) {
    //    //        handleAjaxErrors(jqXhr, textStatus, errorThrown);
    //    //    }
    //    //});
    //}


    $(document).on("click", "#process-payment", function (e) {
        e.preventDefault();

        var formId = $(this).data("form");
        var url = $(this).data("url");
        var myForm = $('#' + formId);
        // console.log(myForm.valid());

        if (!myForm.valid()) {
            return false;
        }
        else {
            $(this).prop('disabled', true);

            // Show full page LoadingOverlay
            $.LoadingOverlay("show", {
                image: "",
                fontawesome: "fa fa-spinner fa-spin",
                custom: $("<div>", {
                    id: "loading-text",
                    css: {
                        "font-size": "25px"
                    },
                    text: "PAYMENT PROCESSING..."
                })
            });
         
            setupStripe(formId);
            return false;
        }
    });
})(jQuery);