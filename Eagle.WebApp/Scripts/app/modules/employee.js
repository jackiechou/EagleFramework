(function ($) {

    $.fn.checkFile = function (options) {
        var defaults = {
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif'],
            allowedSize: 15, //15MB	
            success: function () { }
        };

        options = $.extend(defaults, options);

        if ($(this).value === "") {
            return;
        }

        // get the file name, possibly with path (depends on browser)
        var fileName = $(this).val();
        var fileNameLower = fileName.toLowerCase();
        var extension = fileNameLower.substr((fileNameLower.lastIndexOf('.') + 1));

        var fileSize = $(this)[0].files[0].size; //size in kb
        fileSize = fileSize / 1048576; //size in mb  

        if ($.inArray(extension, options.allowedExtensions) === -1) {
            if (fileSize > options.allowedSize) {
                showNotification('error', 'Wrong extension type! You can upload only ' + options.allowedExtensions + ' extension file, and file size is less than ' + options.allowedSize + ' MB');
            } else {
                showNotification('error', 'Wrong extension type! You can upload only ' + options.allowedExtensions + ' extension file');
            }
            $(this).focus();
        } else {
            if (fileSize > options.allowedSize) {
                showNotification('error', 'You can only upload file up to ' + options.allowedSize + ' MB');
                $(this).focus();
            } else {
                hideMessage();
                options.success();
            }
        }
    };

    function previewImage() {
        $('input[type=file]').on('change', function () {
            if (typeof (FileReader) !== "undefined") {
                var imageHolder = $("#image-holder");
                imageHolder.empty();

                var file = $(this)[0].files[0];
                if (file !== null) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $("<img />", {
                            "width": 50,
                            "height": 50,
                            "src": e.target.result,
                            "class": "thumb-image"
                        }).appendTo(imageHolder);
                    };

                    imageHolder.show();
                    reader.readAsDataURL(file);
                }

                var imageContainer = $("#image-container");
                imageContainer.hide();

                //Check file
                if ($(this).val() !== '' && $(this).val() !== null) {

                    $(this).checkFile();
                }
            } else {
                console.log("This browser does not support FileReader.");
            }
        });
    }

    previewImage();

    function resetImage() {
        $(document).on("click", ".resetPhoto", function () {
            $('input[type=file]').val('');

            var imageHolder = $("#image-holder");
            imageHolder.hide();

            var imageContainer = $("#image-container");
            imageContainer.show();
        });
    }

    resetImage();

    //Company ComboTree
    function populateCompanyComboTree() {
        var select = $("#CompanyId");
        var url = select.data('url');
        var selectedValue = select.val();

        var params = {
            "selectedId": "0",
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

    populateCompanyComboTree();
    
    function populateRegionDropDownList(provinceControlId, regionControlId, isShowSelectText) {
        var select = $(regionControlId);
        var selectedValue = select.data('id');
        var url = select.data('url');
        var provinceId = $(provinceControlId).val();

        if (provinceId !== null && provinceId !== undefined && provinceId !== '') {
            var params = { 'provinceId': provinceId, 'isShowSelectText': isShowSelectText };

            select.empty();
            $.ajax({
                type: "GET",
                url: url,
                data: params,
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function (data) {
                    if (data.length > 0) {
                        $.each(data, function (index, itemData) {
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
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
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

            $.ajax({
                type: "GET",
                url: url,
                data: params,
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function (data) {
                    if (data.length > 0) {
                        $.each(data, function (index, itemData) {
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
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
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
        $.ajax({
            type: "GET",
            url: url,
            data: params,
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                    });
                }

                if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                    select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                }

                populateProvinceDropDownList(countryControlId, provinceControlId, regionControlId, true);

                select.select2({ width: '100%' });
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    populateCountryDropDownList('.EmergencyAddress-CountryId', '.EmergencyAddress-ProvinceId', '.EmergencyAddress-RegionId', true);
    populateCountryDropDownList('.PermanentAddress-CountryId', '.PermanentAddress-ProvinceId', '.PermanentAddress-RegionId', true);

    $(document).on("change", ".EmergencyAddress-CountryId", function () {
        populateProvinceDropDownList('.EmergencyAddress-CountryId', '.EmergencyAddress-ProvinceId', '.EmergencyAddress-RegionId', true);
    });

    $(document).on("change", ".EmergencyAddress-ProvinceId", function () {
        populateRegionDropDownList('.EmergencyAddress-ProvinceId', '.EmergencyAddress-RegionId', true);
    });

    $(document).on("change", ".PermanentAddress-CountryId", function () {
        populateProvinceDropDownList('.PermanentAddress-CountryId', '.PermanentAddress-ProvinceId', '.PermanentAddress-RegionId', true);
    });

    $(document).on("change", ".PermanentAddress-ProvinceId", function () {
        populateRegionDropDownList('.PermanentAddress-ProvinceId', '.PermanentAddress-RegionId', true);
    });

    function search() {
        var formUrl = window.EmployeeSearchUrl;
        var filterRequest = $("#frmSearch").serialize().replace(/[^&]+=&/g, '').replace(/&[^&]+=$/g, '');
        $.ajax({
            type: "GET",
            url: formUrl,
            data: filterRequest,
            ContentType: 'application/json;utf-8',
            datatype: 'json',
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

    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                populateCompanyComboTree();
                handleCheckBoxEvent();
                handleRadios();
                populateCountryDropDownList('.EmergencyAddress-CountryId', '.EmergencyAddress-ProvinceId', '.EmergencyAddress-RegionId', true);
                populateCountryDropDownList('.PermanentAddress-CountryId', '.PermanentAddress-ProvinceId', '.PermanentAddress-RegionId', true);
                previewImage();
                resetImage();
                invokeDateTimePicker('dd/MM/yyyy');
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }
    
    function resetControls(formId, mode) {
        if (mode === 'edit') {
            var id = $('.edit').data('id');
            var container = $('.edit').data('container');
            getDetails(id, container);
        } else {
            var form = $("#" + formId);
            form.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
            form.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
            form.find('input[type="number"]').val(0);
            form.find('input[type=file]').val('');
            form.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
            form.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
            form.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
        }
    }

    function generateCode(url) {
        $.ajax({
            type: "GET",
            url: url,
            success: function (data) {
                $('#EmployeeNo').html(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function create(url, formId) {
        var form = $("#" + formId);
        var formData = new FormData();
        formData.append("Contact.FileUpload", $('input[type=file]')[0].files[0]);
        var params = form.serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    resetControls(formId,'create');
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
    }

    function edit(url, formId) {
        var form = $("#" + formId);
        var formData = new FormData();
        formData.append("Contact.FileUpload", $('input[type=file]')[0].files[0]);
        var params = form.serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
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
    
    function deleteData(url, id) {
        var params = { id: id };
        $.ajax({
            type: "DELETE",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
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
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function updateStatus(url, id, status) {
        var params = { id: id, status: status };
        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
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
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }
    
    //search
    $(document).on("click", ".search", function () {
        search();
    });

    $(document).on("click", ".generate-code", function () {
        var url = $(this).data('url');
        generateCode(url);
        return false;
    });

    $(document).on("click", ".load-create-form", function () {
        var formUrl = $(this).data('url');
        $.ajax({
            type: "GET",
            url: formUrl,
            data: null,
            success: function (data) {
                $('#MainBody').html(data);
                populateCountryDropDownList('.EmergencyAddress-CountryId', '.EmergencyAddress-ProvinceId', '.EmergencyAddress-RegionId', true);
                populateCountryDropDownList('.PermanentAddress-CountryId', '.PermanentAddress-ProvinceId', '.PermanentAddress-RegionId', true);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    });
    
    $(document).on("click", ".load-back-form", function () {
        var formUrl = $(this).data('url');
        $.ajax({
            type: "GET",
            url: formUrl,
            data: null,
            success: function (data) {
                $('#MainBody').html(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    });

    // Reset form
    $(document).on("click", ".reset", function () {
        var formId = $(this).data('form');
        var mode = $(this).data('mode');
        resetControls(formId, mode);
    });
    
    
    //GET - EDIT
    $(document).on("click", ".editItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');

        getDetails(url, id);
        return false;
    });
    
    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var form = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        } else {
            create(url, form);
            return false;
        }
    });

    //POST - EDIT
    $(document).on("click", ".edit", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var form = $(this).data('form');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        }
        else {
            edit(url, form);
            return false;
        }
    });


    $(document).on("change", ".changeStatus", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        var warning = $(this).data('warning');
        var status = $(this).is(":checked");

        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.Warning,
            message: warning,
            buttons: {
                confirm: {
                    label: window.Ok,
                    className: 'confirm-button-class'
                },
                cancel: {
                    label: window.Cancel,
                    className: 'cancel-button-class'
                }
            },
            callback: function (result) {
                if (result) {
                    updateStatus(url, id, status);
                    box.modal('hide');
                }
            },
            onEscape: function () { return false; }
        });

        box.css({
            'top': '50%',
            'margin-top': function () {
                return -(box.height() / 2);
            }
        });
        return false;
    });

    $(document).on("click", ".deleteItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        var warning = $(this).data('warning');

        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.Warning,
            message: warning,
            buttons: {
                confirm: {
                    label: window.Ok,
                    className: 'confirm-button-class'
                },
                cancel: {
                    label: window.Cancel,
                    className: 'cancel-button-class'
                }
            },
            callback: function (result) {
                if (result) {
                    deleteData(url, id);
                    box.modal('hide');
                }
            },
            onEscape: function () { return false; }
        });

        box.css({
            'top': '50%',
            'margin-top': function () {
                return -(box.height() / 2);
            }
        });
        return false;
    });

})(jQuery);