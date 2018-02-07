(function ($) {
    function search(url, formId) {
        var filterRequest = $("#" + formId).serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: "GET",
            url: url,
            data: filterRequest,
            ContentType: 'application/json;utf-8',
            datatype: 'json',
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                $('#search-result').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $('#search-result').html('<span>' + textStatus + ", " + errorThrown + '</span>');
            }
        });
        return false;
    }

    //search
    $(document).on("click", ".search", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var formId = $(this).data('form');
        search(url, formId);
    });

    function populateRegionDropDownList() {
        var select = $('.Address-RegionId');
        var selectedValue = select.data('id');
        var url = select.data('url');
        var provinceId = $('.Address-ProvinceId').val();

        if (provinceId !== null && provinceId !== undefined && provinceId !== '') {
            var params = { 'provinceId': provinceId, 'isShowSelectText': true };

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

    function populateProvinceDropDownList() {
        var select = $('.Address-ProvinceId');
        var selectedValue = select.data('id');
        var url = select.data('url');
        var countryId = $('.Address-CountryId').val();

        if (countryId !== null && countryId !== undefined && countryId !== '') {
            var params = { 'countryId': countryId, 'isShowSelectText': true };

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

                        populateRegionDropDownList();

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

    function populateCountryDropDownList() {
        var select = $('.Address-CountryId');
        var selectedValue = select.data('id');
        var url = select.data('url');
        var params = { "selectedValue": selectedValue, "isShowSelectText": true };

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

                populateProvinceDropDownList();

                select.select2({ width: '100%' });
            });
    }

    $(document).on("change", ".Address-CountryId", function () {
        populateProvinceDropDownList();
    });

    $(document).on("change", ".Address-ProvinceId", function () {
        populateRegionDropDownList();
    });
    
    ////Check and privew photo
    //function checkPreviewPhoto() {
    //    $('#File').checkFile({
    //        allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'bmp'],
    //        allowedSize: 15 //15MB	
    //    });
    //    previewPhoto();
    //}
    
    function getCustomerDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                $('.customer-profile-container').html(data);
                //checkPreviewPhoto();
                handleRadios();
                invokeDateTimePicker('dd/MM/yyyy');
                populateCountryDropDownList();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function editProfile(url, formId) {
        var formData = $("#" + formId).serialize();

        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                     getCustomerDetails(url, response.Data.Id);
                     showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        showMessageWithTitle(500, result, "error", 50000);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    $(document).on("click", ".editProfileDetails", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');

        getCustomerDetails(url, id);
        return false;
    });

    $(document).on("click", ".editProfile", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var formId = $(this).data('form');
        $.validator.methods["date"] = function (value, element) { return true; };

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        }
        else {
            editProfile(url, formId);
            return false;
        }
    });
})(jQuery);