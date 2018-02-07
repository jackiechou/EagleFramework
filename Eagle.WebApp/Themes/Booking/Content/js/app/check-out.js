(function ($) {
    function populateRegionDropDownList(provinceControlId, regionControlId, isShowSelectText) {
        var select = $(regionControlId);
        var selectedValue = select.data('id');
        var url = select.data('url');
        var provinceId = $(provinceControlId).val();

        if (provinceId !== null && provinceId !== undefined && provinceId !== '') {
            var params = { 'provinceId': provinceId, 'isShowSelectText': isShowSelectText };

            select.empty();
            $.getJSON(url, params,
                function (classesData) {
                    if (classesData.length > 0) {
                        $.each(classesData, function (index, itemData) {
                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text
                            }));
                        });

                        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                            select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                        }

                        select.select2({ width: '100%' });
                    } else {
                        select.append($('<option/>', {
                            value: '',
                            text: 'None'
                        }));
                    }
                });
        }
    }

    function populateProvinceDropDownList(countryControlId, provinceControlId, regionControlId, isShowSelectText) {
        var select = $(provinceControlId);
        var selectedValue = select.data('id');
        var url = select.data('url');
        var countryId = $(countryControlId).val();

        if (countryId !== null && countryId !== undefined && countryId !== '') {
            var params = { 'countryId': countryId, 'isShowSelectText': isShowSelectText };

            select.empty();
            $(regionControlId).empty();
            $.getJSON(url, params,
                function (classesData) {
                    if (classesData.length > 0) {
                        $.each(classesData, function (index, itemData) {
                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text
                            }));
                        });

                        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                            select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                        }
                        populateRegionDropDownList(provinceControlId, regionControlId, true);
                        select.select2({ width: '100%' });
                    } else {
                        select.append($('<option/>', {
                            value: '',
                            text: 'None'
                        }));
                    }
                });
        }
    }

    function populateCountryDropDownList(countryControlId, provinceControlId, regionControlId, isShowSelectText) {
        var select = $(countryControlId);
        var selectedValue = select.data('id');
        var url = select.data('url');
        var params = { "selectedValue": selectedValue, "isShowSelectText": isShowSelectText };

        select.empty();
        $.getJSON(url, params,
            function (classesData) {
                if (classesData.length > 0) {
                    $.each(classesData, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                    });
                }

                if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                    select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                }

                populateProvinceDropDownList(countryControlId, provinceControlId, regionControlId, false);
                populateRegionDropDownList(provinceControlId, regionControlId, false);
                select.select2({ width: '100%' });
            });
    }

    $(document).on("change", ".ShipmentInfo-CountryId", function () {
        populateProvinceDropDownList('.ShipmentInfo-CountryId', '.ShipmentInfo-ProvinceId', '.ShipmentInfo-RegionId', true);
    });

    $(document).on("change", ".ShipmentInfo-ProvinceId", function () {
        populateRegionDropDownList('.ShipmentInfo-ProvinceId', '.ShipmentInfo-RegionId', true);
    });

    var currentCurrency = $('#current-currency');
    var formatter = new Intl.NumberFormat('en-US');
    var totalLabel = $('#total');

    $(document).on("change", ".shipping-method", function () {
        var shippingMethodId = $(this).val();
        var apiUrl = $(this).data('url');
        var zipCode = $('#PostalCode').val();

        if (zipCode !== null && zipCode !== '') {
            var data = { "shippingMethodId": shippingMethodId, "zipCode": zipCode };
            $.post(apiUrl, data, function (data) {
                if (data.Data !== null) {
                    // Popolate to UI
                    if (currentCurrency === 'USD') {
                        $('#shipment-fee-label').html(formatter.format(data.Data.ShipmentInfo.TotalShippingFee));
                        totalLabel.html(formatter.format(data.Data.Total));
                    }
                }
            });
        }
    });


    //var shippingMethodField = $('#shipping-method-field');
    //$(document).on("change", ".shipping-carrier", function () {
    //    let shippingMethodId = $(this).data("shiping-method-id");
    //    shippingMethodField.val(shippingMethodId);

    //    let shippingCarrierId = $(this).val();
    //    let zipCode = $('#PostalCode').val();
    //    let apiUrl = $('#calculate-shipping-fee-url').val();

    //    if (zipCode !== null && zipCode !== '') {
    //        var data = { shippingMethodId, shippingCarrierId, zipCode };
    //        $.post(apiUrl, data, function (data) {
    //            if (data.Data != null) {
    //                // Popolate to UI
    //                if (currentCurrency === 'USD') {
    //                    $('#shipment-fee-label').html(formatter.format(data.Data.ShipmentInfo.TotalShippingFee));
    //                    totalLabel.html(formatter.format(data.Data.Total));
    //                }
    //            }
    //        });
    //    }
    //});
  

    // END - Promotion Section
    populateCountryDropDownList('.ShipmentInfo-CountryId', '.ShipmentInfo-ProvinceId', '.ShipmentInfo-RegionId', true);
    
    $(document).on("click", ".payment-method", function () {
        var paymentMethodId = $(this).data('payment-method-id');
        $('#payment-method-id').val(paymentMethodId);
    });  

    $(document).on("click", "#check-out", function () {
        var formId = $(this).data("form");;
        var myForm = $('#' + formId);
        // console.log(myForm.valid());

        if (!myForm.valid()) {
            return false;
        }
        else {
            myForm.submit();
            return false;
        }
    });
    
})(jQuery);