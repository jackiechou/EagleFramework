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
    
    $('#card-cvv').keyup(function (event) {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "");
        });
    });
    
    $(document).on("click", "#process-payment", function (e) {
        e.preventDefault();

        var formId = $(this).data("form");
        var myForm = $('#' + formId);
        //var url = $(this).data("url");
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
           myForm.submit();   
          //  console.log('okay');
           
            //processPayment(url, formId);
            return false;
        }
    });

})(jQuery);