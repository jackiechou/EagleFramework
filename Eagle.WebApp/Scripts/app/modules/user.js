(function ($) {
    invokeDateTimePicker('dd/MM/yyyy');
    handleCheckBoxEvent();
    handleRadios();
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

                        populateRegionDropDownList(provinceControlId, regionControlId, false);

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

                //console.log(selectedValue + ' aaaaaaaaaaaaa');

                if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                    select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                }

                populateProvinceDropDownList(countryControlId, provinceControlId, regionControlId, false);

                select.select2({ width: '100%' });
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

    function populateDualListBoxListeners() {
        //Simple delay function that can wrap around an existing function and provides a callback.
        var delay = (function () {
            var timer = 0;
            return function (callback, ms) {
                clearTimeout(timer);
                timer = setTimeout(callback, ms);
            };
        })();

        //Checks whether or not an element is visible. The default jQuery implementation doesn't work.
        $.fn.isVisible = function () {
            return !($(this).css('visibility') === 'hidden' || $(this).css('display') === 'none');
        };

        //Sorts options in a select / list box.
        $.fn.sortOptions = function () {
            return this.each(function () {
                $(this).append($(this).find('option').remove().sort(function (a, b) {
                    var at = $(a).text(), bt = $(b).text();
                    return (at > bt) ? 1 : ((at < bt) ? -1 : 0);
                }));
            });
        };

        $.fn.filterByText = function (textBox) {
            return this.each(function () {
                var select = this;
                var options = [];
                var timeout = 10;

                $(select).find('option').each(function () {
                    options.push({ value: $(this).val(), text: $(this).text() });
                });

                $(select).data('options', options);

                $(textBox).bind('change keyup', function () {
                    delay(function () {
                        var options = $(select).scrollTop(0).data('options');
                        var search = $.trim($(textBox).val());
                        var regex = new RegExp(search, 'gi');

                        $.each(options, function (i) {
                            var option = options[i];
                            if (option.text.match(regex) === null) {
                                $(select).find($('option[value="' + option.value + '"]')).hide();
                            } else {
                                $(select).find($('option[value="' + option.value + '"]')).show();
                            }
                        });
                    }, timeout);
                });
            });
        };

        var unselected = $('select.unselected');
        var selected = $('select.selected');
        var optParentElement = $('.dual-list-box');

        optParentElement.find('button').bind('click', function () {
            switch ($(this).data('type')) {
                case 'str': /* Selected to the right. */
                    unselected.find('option:selected').appendTo(selected);
                    selected.find('option').each(function () {
                        if ($(this).isVisible()) {
                            $(this).attr('selected', true);
                        }
                    });
                    //$(this).prop('disabled', true);
                    break;
                case 'atr': /* All to the right. */
                    unselected.find('option').each(function () {
                        if ($(this).isVisible()) {
                            $(this).remove().appendTo(selected);
                            $(this).attr('selected', true);
                        }
                    });
                    break;
                case 'stl': /* Selected to the left. */
                    selected.find('option:selected').remove().appendTo(unselected);
                    //$(this).prop('disabled', true);
                    selected.find('option').each(function () {
                        if ($(this).isVisible()) {
                            $(this).prop('selected', true);
                        }
                    });
                    break;
                case 'atl': /* All to the left. */
                    selected.find('option').each(function () {
                        if ($(this).isVisible()) {
                            $(this).remove().appendTo(unselected);
                        }
                    });
                    break;
                default:
                    break;
            }

            unselected.filterByText($('input.filter-unselected')).scrollTop(0).sortOptions();
            selected.filterByText($('input.filter-selected')).scrollTop(0).sortOptions();
        });

        optParentElement.closest('form').submit(function () {
            selected.find('option').prop('selected', true);
        });

        optParentElement.find('input[type="text"]').keypress(function (e) {
            if (e.which === 13) {
                event.preventDefault();
            }
        });

        selected.find('option').each(function () {
            if ($(this).isVisible()) {
                $(this).prop('selected', true);
            }
        });
        selected.filterByText($('input.filter-selected')).scrollTop(0).sortOptions();
        unselected.filterByText($('input.filter-unselected')).scrollTop(0).sortOptions();
    }

    populateDualListBoxListeners();

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
                            "width": 150,
                            "height": 150,
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

    function populateGroupDropDownList() {
        var select = $('#GroupId');
        var url = window.PopulateGroupDropDownListUrl;
        var params = {
            'RoleId': $('#RoleId').val()
        };

        select.empty();

        $.ajax({
            type: "GET",
            url: url,
            data: params,
            success: function (data) {
                if (data.length > 0) {
                    $.each(data, function (index, itemData) {
                        select.append($('<option/>', {
                            value: itemData.Value,
                            text: itemData.Text
                        }));
                    });
                } else {
                    select.append($('<option/>', { value: 'Null', text: " ---" + window.None + " --- " }));
                }
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }
    
    function resetControls(form) {
        form.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        form.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        form.find('input[type="number"]').val(0);
        form.find('input[type=file]').val('');
        form.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        form.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        form.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

 
    function search(url) {
        var params = $('#frmSearchUser').serialize();

        $.ajax({
            type: "GET",
            url: url,
            data: params,
            success: function (data) {
                $('#search-result').html(data);
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
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

        $(".edit").val('Processing...');
        $(".edit").attr("disabled", true);

        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    $(".edit").val('Save');
                    $(".edit").prop("disabled", false);
                    showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
                    window.location.href = "/admin/user/index";
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
                $(".edit").val('Save');
                $(".edit").prop("disabled", false);
            }
        });
    }

    function create(url, formId) {
        var form = $("#" + formId);
        var formData = new FormData();
        formData.append("Contact.FileUpload", $('input[type=file]')[0].files[0]);
        var params = form.serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
        });

        $(".create").val('Processing...');
        $(".create").attr("disabled", true);

        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    $(".create").val('Save');
                    $(".create").prop("disabled", false);
                    //resetControls(form);
                    showMessageWithTitle(jqXhr.status, response.Data, "success", 20000);
                    window.location.href = "/admin/user/index";
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
                $(".create").val('Save');
                $(".create").prop("disabled", false);
            }
        });
    }

    //POST - CREATE
    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var url = $(this).data("url");
        var formId = $(this).data("form");
        var form = $("#" + formId);

        if (!form.valid()) { // Not Valid
            return false;
        } else {
            create(url, formId);
            return false;
        }
    });

    $(document).on("change", "#RoleId", function () {
        $(this).find("option[value=" + $(this).val() + "]").attr('selected', true).siblings().attr('selected', false);
        populateGroupDropDownList();
    });

    // Reset form
    $(document).on("click", ".reset", function () {
        var formId = $(this).data('form');
        var form = $("#" + formId);
        resetControls(form);
    });


    $(document).on("click", ".search", function () {
        var url = $(this).data('url');
        search(url);
        return false;
    });
    
    //PUT - EDIT
    $(document).on("click", ".edit", function (e) {
        e.preventDefault();

        var url = $(this).data("url");
        var formId = $(this).data("form");
        var form = $("#" + formId);

        if (!form.valid()) { // Not Valid
            return false;
        } else {
            edit(url, formId);
            return false;
        }
    });

    $(document).on("change", ".approve-unapprove", function () {
        var id = $(this).data('id');
        var status = $(this).is(":checked");
        var formData = { "userId": id };

        var formUrl = '';
        if (status !== null && status === true)
            formUrl = $(this).data('approve-url');
        else
            formUrl = $(this).data('unapprove-url');

        $.ajax({
            type: 'PUT',
            url: formUrl,
            data: formData,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
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
    });

    $(document).on("change", ".lock-unlock", function () {
        var id = $(this).data('id');
        var status = $(this).is(":checked");
        var formData = { "userId": id };

        var formUrl = '';
        if (status !== null && status === true)
            formUrl = $(this).data('lock-url');
        else
            formUrl = $(this).data('unlock-url');

        $.ajax({
            type: 'PUT',
            url: formUrl,
            data: formData,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
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
    });

    //Edit role group
    $(document).on("click", ".isGroupDefault", function () {
        var isChecked = $(this).is(':checked');
        if (isChecked) {
            var chkIsGroupAllowed = $(this).data('isgroupallowed');
            $("input:checkbox[name='" + chkIsGroupAllowed + "']").prop('checked', isChecked);
            $("input:checkbox[name='" + chkIsGroupAllowed + "']").val(isChecked);

            var chkIsRoleAllowed = $(this).data('isroleallowed');
            $("input:checkbox[name='" + chkIsRoleAllowed + "']").prop('checked', isChecked);
            $("input:checkbox[name='" + chkIsRoleAllowed + "']").val(isChecked);
        }

        $(this).prop('checked', isChecked);
        var chkBoxName = $(this).data('isdefaultname');
        $("input:checkbox[data-isdefaultname='" + chkBoxName + "']").not(this).prop('checked', false);
        $("input:checkbox[data-isdefaultname='" + chkBoxName + "']").not(this).val(false);
    });

    $(document).on("click", ".isGroupAllowed", function () {
        var isChecked = $(this).is(':checked');
        if (!isChecked) {
            $(this).prop('checked', false);

            var chkIsDefaultGroupId = $(this).data('isdefaultgroupid');
            $('input:checkbox[id^="' + chkIsDefaultGroupId + '"]').prop('checked', false);
            $('input:checkbox[id^="' + chkIsDefaultGroupId + '"]').val(false);
        }
    });

    $(document).on("click", ".isRoleAllowed", function () {
        var isChecked = $(this).is(':checked');

        if (!isChecked) {
            var userRoleGroupId = $(this).data('id') + '.UserRoleGroups';
            $('input:checkbox[name^="' + userRoleGroupId + '"]').prop('checked', false);
            $('input:checkbox[name^="' + userRoleGroupId + '"]').val(false);
            $('input[name^="' + userRoleGroupId + '"]').val('');
            $('input[name^="' + userRoleGroupId.replace('.','') + '"]').datetimepicker('show').datetimepicker('reset');
        }
    });
})(jQuery);