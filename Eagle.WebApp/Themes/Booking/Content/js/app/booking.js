$(document).ready(function () {

    //Booking Single Package ========================
    var bookingSingle = {
        categories: [],
        addRule: (controlId) => {
            $('#' + controlId).apppend('<span class="help-block field-validation-valid" data-valmsg-for="' + controlId + '" data-valmsg-replace="true"></span>');
        },
        addPackageRow: () => {
            //START DYNAMIC TABLE EXTENSION
            var ul = $(".package-container");
            var ulArray = ul.data("array");

            // Get max row id and set new id
            var newid = 0;
            var rowArray = '';
            $.each(ul.find("li"), function () {
                if (parseInt($(this).data("id")) > newid) {
                    newid = parseInt($(this).data("id"));
                }
                $(this).addClass('bg-grey').removeClass('active');
                // $(this).find('input:text, input:radio, input:checkbox, select, textarea').attr('readonly', true);
                $('.readonly input:checkbox').click(function () { return false; });
                $('.readonly input:checkbox').keydown(function () { return false; });
            });
            newid++;
            rowArray = ulArray + '[' + newid + ']';

            var li = $("<li></li>", {
                id: "row" + newid,
                "data-id": newid,
                "data-array": rowArray,
                "class": "active"
            });

            // loop through each td and create new elements with name of newid
            var categoryUrl = '/Booking/GetServiceCategorySelectTree';
            var categoryElement = $('<div class="form-group" data-id="CategoryId" data-name="CategoryId">'        
                + '<div id ="category" class="col-md-12">'
                + '<select id="Packages_' + newid + '_CategoryId" name="Packages[' + newid + '].CategoryId" class="easyui-combotree form-control" data-index="' + newid + '" data-url="' + categoryUrl + '" style="width: 100%;"></select>'
                + '</div>'
                + '</div>');
	
            var packageUrl = '/Booking/PopulatePackagesByCategory';
            var packageElement = $('<div class="form-group" data-id="PackageSelect" data-name="PackageId">'                    
                    + ' <div class="dropdown col-md-12" id="Packages_' + newid + '_PackageSelect" name="Packages[' + newid + '].PackageSelect" data-index="' + newid + '" data-typeid="Single" data-url="' + packageUrl + '">'
                        + '<a class="btn btn-default btn-select dropdown-toggle" data-toggle="dropdown" role="button" href="javascript:void(0);">'
                            + '<input type="hidden" class="btn btn-select-input" id="Packages_'+newid+'_PackageId" name="Packages['+newid+'].PackageId" required="required" />'
                            + '<span class="btn btn-select-text">' + window.SelectPackage + '</span>'
                            + '<span class="btn btn-select-arrow glyphicon glyphicon-chevron-down"></span>'
                        + '</a>'
                        + '<ul id="Packages_' + newid + '" class="dropdown-menu" style="display:none"></ul>'
                    + '</div>'
                    + '<span class="help-block field-validation-valid" data-valmsg-for="Packages['+newid+'].PackageId" data-valmsg-replace="true"></span>'
               + '</div>');

            var employeeUrl = '/Booking/PopulateEmployeesByPackage';
            var employeeElement = $('<div class="form-group" data-id="EmployeeId" data-name="EmployeeId">'             
                + '<div id="employee-container" class="col-md-12">'
                    + '<select id="Packages_'+newid+'_EmployeeId" name="Packages['+newid+'].EmployeeId" class="form-control employee-select" data-index="0" data-url="'+employeeUrl+'" required="required">'
                        + '<option value="" selected>' + window.SelectEmployee + '</option>'
                    + '</select>' 
                + '</div>'
                + '<span class="help-block field-validation-valid" data-valmsg-for="Packages['+newid+'].EmployeeId" data-valmsg-replace="true"></span>'
            + '</div>');
	
            //add delete button and td
            var buttonElement = $('<div class="form-group text-right actions">'
                 + '<button type="button" data-title="Delete" data-optionid="0" data-index="0" class="btn btn-warning glyphicon glyphicon-trash row-remove disable"></button>'
             + '</div>');

            categoryElement.appendTo($(li));
            packageElement.appendTo($(li));
            employeeElement.appendTo($(li));
            buttonElement.appendTo($(li));

            //add the new row
            $(li).appendTo(ul);

            //Revalidate with new dynamic HTML
            var form = ul.closest("form");
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);

            //Invoke loading with combo tree of easyui
            bookingSingle.populateCategorySelectTree($(li).data('id'));

            $(".row-remove").on("click", function () {
                $(this).closest("li").remove();
                return false;
            });
        },
        poplulateEmployeeSelectList: (index, packageId) => {
            var controlId = '#Packages_' + index + '_EmployeeId';
            //var controlName = 'Packages[' + index + '].EmployeeId';
            var select = $(controlId);

            if (packageId === null || packageId === '' || packageId <= 0) {
                select.empty();
                select.append($('<option/>', { value: 'Null', text: " ---" + window.None + " --- " }));
                return false;
            }

            var url = select.data('url');
            var params = { 'packageId': packageId };
            //console.log('Control Id of Employee : ' + controlId + ' --- Index : ' + index + ' --- PackageId : ' + packageId);

            select.empty();
            //select.addClass('spinner-left');

            $.ajax({
                type: "GET",
                url: url,
                data: params,
                beforeSend: function () {
                    $.unblockUI();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                },
                success: function (data) {
                    select.empty();
                    if (data.length > 0) {
                        $.each(data, function (index, itemData) {
                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text
                            }));
                        });
                      
                    } else {
                        select.append($('<option/>', { value: '', text: " ---" + window.None + " --- " }));
                    }
                    
                    //parse new dynamic HTML
                    select.first().closest('form').validate();
                    //bookingSingle.validateForm();
                    //select.removeClass('spinner-left');

                    select.on("change", function (e) {
                        e.preventDefault();
                        var selectedValue = $(this).find(":selected").val();

                        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                            $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                            
                            //$("#myForm").validate({
                            //    onfocusout: false,
                            //    invalidHandler: function (form, validator) {
                            //        var errors = validator.numberOfInvalids();
                            //        if (errors) {
                            //            validator.errorList[0].element.focus();
                            //        }
                            //    }
                            //});

                            
                            //////$(this).attr('aria-hidden', true).hide();
                            ////$(this).attr('aria-invalid', false);
                            ////$(this).setAttribute("aria-describedby", "");
                            ////var formId = select.first().closest('form').attr('id');
                            //// bookingSingle.validateForm(formId);
                            ////$.validator.unobtrusive.parse(select);
                        }
                        return false;
                    });
                }
            });
           
            return false;
        },
        populateSinglePackagesByCategoryId: (index, categoryId) => {
            var select = $('#Packages_' + index + '_PackageSelect');
            var url = select.data('url');
            var typeId = select.data('typeid');
            var params = {
                "typeId": typeId,
                "categoryId": categoryId
            };


            select.hide();
            //console.log('Control Id of Package --- Index : ' + index + ' ---  CategoryId : ' + categoryId + ' ---  url : ' + url);
            //select.addClass('spinner-left');
            var ahref = $('#Packages_' + index + '_PackageSelect').children('a.dropdown-toggle');
            var ul = $('#Packages_' + index);
            ul.empty();

            //Empty dependency package like employee
            $('#Packages_' + index + '_EmployeeId').empty();

            $.ajax({
                type: 'GET',
                url: url,
                data: params,
                //async:false,
                //beforeSend: function () {
                //    $.unblockUI();
                //},
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                },
                success: function (data) {
                    if (data !== null && data !== undefined && data.length > 0)
                    {
                        $.each(data, function (index, item) {
                            var text = (item.Duration !== null & item.Duration !== '') ? (item.PackageName + " (" + item.Duration.DurationName + " - " + item.TotalFee + " " + item.CurrencyCode + ")") : item.PackageName + " (" + item.TotalFee + "  " + item.CurrencyCode + ")";
                            var li = $('<li></li>');
                            li.html('<a  data-value="' + item.PackageId + '" href="javascript:void(0);"><h4>' + text + '</h4><p>' + item.Description + '</p></a>');
                            li.appendTo(ul);
                        });

                        select.find('a').find(".btn-select-text").html(window.SelectPackage);

                        ul.removeAttr("style");
                        ul.find("a").click(function (e) {
                            e.preventDefault();

                            ahref.dropdown('toggle');
                            var selId = $(this).data('value');
                            var selText = $(this).find('h4').text();

                            $(this).parent().addClass("selected").siblings().removeClass("selected");
                            if (selId !== undefined && selId !== null && selId >= 0) {
                                ahref.find(".btn-select-input").val(selId);
                                ahref.find(".btn-select-text").html(selText);

                                bookingSingle.poplulateEmployeeSelectList(index, selId);
                            }
                            return false;
                        });

                    } else {
                        //Update label & content   
                        $(this).parent('li').parent('ul').siblings(".btn-select-input").val('');
                        select.children('a').find(".btn-select-text").html(window.None);
                        ul.attr("style", "display:none");
                    }

                    select.show();
                    //select.removeClass('spinner-left');
                    ////var formId = select.first().closest('form').attr('id');
                    //bookingSingle.validateForm();
                    return false;
                }
            });
        },
        getPackageDetail: (index, packageId) => {
            var controlId = '#Packages_' + index + '_Description';
            var label = $(controlId);

            if (packageId === null || packageId === '' || packageId <= 0) {
                label.empty();
                return false;
            }

            var url = window.getServicePackageDetailUrl;
            var params = { "packageId": packageId };
            var data = $.ajax({
                type: 'GET',
                dataType: "json",
                url: url,
                data: params,
                async: false
            }).responseJSON;

            label.html(data.Description);
            return false;
        },
        getCategories: () => {
            var typeId = $('input[type=radio][name="TypeId"]:checked').val();
            var url = window.getServiceCategorySelectTreeUrl;
            var params = {
                "typeId": typeId,
                "selectedId": 0,
                "isRootShowed": true
            };

            bookingSingle.categories = $.ajax({
                type: 'GET',
                dataType: "json",
                url: url,
                data: params,
                async: false
            }).responseJSON;

            return false;
        },
        populateCategorySelectTree: (index) => {
            var controlId = '#Packages_' + index + '_CategoryId';
            var select = $(controlId);
            var selectedValue = 0;
            var data = bookingSingle.categories;
            //// console.log('Control Id - CategoryId : ' + controlId + ' --- Index : ' + index);

            $.extend($.fn.validatebox.defaults.rules, {
                exists: {
                    validator: function (value, param) {
                        var cc = $(param[0]);
                        var val = cc.combotree('getValue');
                        if (val === null || val === '' || val === '0') {
                            return false;
                        }
                        return true;
                    },
                    message: 'Please select category'
                }
            });

            select.combotree({
                required: true,
                validType: 'exists["' + controlId + '"]',
                min: 1,
                data: data,
                valueField: 'id',
                textField: 'text',
                method: 'get',
                panelHeight: 'auto',
                children: 'children',
                onLoadSuccess: function () {
                    $(this).tree("expandAll");
                },
                onClick: function (node) {
                    selectedValue = node.id;
                    $(this).val(selectedValue);
                    select.combotree('validate');
                    ////select.combotree('isValid');
                    if (selectedValue !== null && selectedValue !== undefined && selectedValue !== '') {
                       // console.log(selectedValue);
                        bookingSingle.populateSinglePackagesByCategoryId(index, selectedValue);
                    }
                }
            });
            //select.combotree('validate');
            ////select.combotree('isValid');
            //// select.siblings('.validatebox-text').remove();

            return false;
        },
        init: () => {
            invokeDateTimePicker('dd/MM/yyyy');
            handleRadios();
            handleCheckBoxes();

            // bookingSingle.setupSmartWizardSteps();
            bookingSingle.getCategories();
            bookingSingle.populateCategorySelectTree(0);
            bookingSingle.bindEvents();
        },
        bindEvents: () => {
            //Handle Booking Time
            $(document).on("change", "#PeriodGroup", function () {
                $(this).find("option[value=" + $(this).val() + "]").attr('selected', true).siblings().attr('selected', false);
                var selectedValue = $(this).val();
                if (selectedValue === '4') {
                    if ($("#FromPeriod").hasClass('hide')) {
                        $("#FromPeriod").removeClass('hide');
                    }
                } else {
                    if (!$("#FromPeriod").hasClass('hide')) {
                        $("#FromPeriod").addClass('hide');
                    }
                }
            });
            
            //Handle Single Packages - DYNAMIC TABLE EXTENSION
            $(".row-add").on("click", () => {
               // var formId = $(".row-add").data('form');

                var isValid = bookingSingle.validateForm();
                if (isValid) {
                    bookingSingle.addPackageRow();
                }
            });
            
            $(".row-remove").on("click", () => {
                $(this).closest("tr").remove();
                return false;
            });

            $("#Capacity").on('change keyup', function () {
                var input = $(this);
                var number = parseInt(input.val());
                if (number > 0) {
                    input.removeClass("invalid").removeClass("error").addClass("valid");
                } else {
                    input.val('');
                }
            });

            $(document).on("click", "#bookingSingleService", function (e) {
                e.preventDefault();
                var form = $("#myForm");

                var isValid = bookingSingle.validateForm();
                if (!isValid) { // Not Valid
                    return false;
                } else {
                    form.submit();
                    return false;
                }
            });
        },
        validateForm: () => {
            //Remove the form's validation and re validate
            var form = $("#myForm");

            //var validator = form.validate();  //get jquery validators
            form.removeData('validator');
            //var unobtrusiveValidation = $(form).data('unobtrusiveValidation') //get the collections of unobstrusive validators
            form.removeData('unobtrusiveValidation');  
            $.validator.unobtrusive.parse(form);

            form.validate().settings.ignore = "";
            // form.data("validator").settings.ignore = "";
            $('.easyui-combotree').combotree('isValid');

            // get validator object
            var validator = form.validate({ ignore: "" });

            var invalidFields = [];
            var errorMessageObj = $('<ul class="errorMessages"></ul>');
            errorMessageObj.empty();

            // Find all invalid fields within the form.
            form.find(":invalid").each(function (index, node) {
                // Find the invalid fields
                var invalidNodeId = node.id;
                var invalidNodeName = $(node).attr('name');
                var invalidNodeMessage = node.validationMessage || 'Invalid value.';
                invalidFields.push(invalidNodeId);

                //$(node).attr("aria-invalid", "true");
                //$(node).attr("aria-hidden", "false").show();
                //$(node).attr('aria-describedby', invalidNodeName);

                console.log(invalidNodeName + ' : '  +invalidNodeMessage);
                $('<li data-nodeid = "' + invalidNodeId + '"><span>' + invalidNodeName + '</span> : ' + invalidNodeMessage + '</li>').appendTo(errorMessageObj);
            });

            if (!form.valid() || invalidFields.length > 0) { // Not Valid
                // console.log(invalidFields.length);
                showMessageWithTitle(500, errorMessageObj.html(), "error", 50000);
                return false;
            } else {
                //$('input[aria-describedby="'+invalidNodeName+'"]').attr('aria-describedby', '');
                //$('input[aria-invalid="true"]').attr("aria-invalid", "false");
                //$('input[aria-hidden="true"]').hide();
               
                validator.resetForm();
                hideMessage();
                return true;
            }
        },
        success: (response) => {
            $("body").removeClass("loading");
            if (response.Result === "success") {
                debugger;
                $('#ServicePackChoosed').append(
                    $('<span></span>').html(response.Data.ServicePackName));
            } else {
                debugger;
                $('#MessageResult').append(
                    $('<span></span>').html("add service fail"));
            }
            reload();
        },
        fail: () => {
            $("body").removeClass("loading");
            $('#MessageResult').append(
                   $('<span></span>').html("add service fail"));
        }
    };
    bookingSingle.init();

    //Booking Full Package ==========================
    var bookingPackage = {
        bindPackageEvents: function () {

            $(document).on("change", "#PeriodGroup", function () {
                $(this).find("option[value=" + $(this).val() + "]").attr('selected', true).siblings().attr('selected', false);
                var selectedValue = $(this).val();
                if (selectedValue === '4') {
                    if ($("#FromPeriod").hasClass('hide')) {
                        $("#FromPeriod").removeClass('hide');
                    }
                } else {
                    if (!$("#FromPeriod").hasClass('hide')) {
                        $("#FromPeriod").addClass('hide');
                    }
                }
            });
            
            //Create - Booking Service
            $(document).on("click", "#bookingFullPackage", function (e) {
                e.preventDefault();

                var url = $(this).data('url');
                bookingPackage.bookingService(url);
            });
        },
        bookingService: (url) => {
            //var formData = $("#" + formId).awesomeFormSerializer({
            //    Deposit: $("#Deposit").val().replace(/,/g, "")
            //});
            //console.log(url);
            var form = $("#myForm");
            
            var packages = {};
            $("#servive-packages li.selected").each(function (index, item) {
                var categoryId = $('input[type="hidden"][name="Packages[' + index + '].CategoryId"]').val();
                var packageId = $('input[type="hidden"][name="Packages[' + index + '].PackageId"]').val();
                var employeeId = $('select[name="Packages[' + index + '].EmployeeId"] option:selected').val();
                var package = {
                    'CategoryId': categoryId,
                    'PackageId': packageId,
                    'EmployeeId': employeeId
                };

                packages[index] = package;

                $("#Packages_" + index + "_EmployeeId").change(function () {
                    $(this).find("option[value=" + $(this).val() + "]").attr('selected', true).siblings().attr('selected', false);
                });

            });

            var currencyCode = $('input[type="hidden"][name="CurrencyCode"]').val();
            var categoryId = $('input[type="hidden"][name="CategoryId"]').val();
            var startDate = $('input[type="hidden"][name="StartDate"]').val();
            var periodGroup = $('select[name="PeriodGroup"] option:selected').val();
            //var fromPeriod = $('input[type="hidden"][name="FromPeriod"]').val();
            //var deposit = $('input[type="hidden"][name="Deposit"]').val();
            //var comment = $('input[type="hidden"][name="Comment"]').val();

            var output = {
                CurrencyCode: currencyCode,
                CategoryId: categoryId,
                StartDate: startDate,
                PeriodGroup: periodGroup,
                Packages: packages
                //'FromPeriod': fromPeriod,
                //'Deposit': deposit,
                //'Comment': comment
            };

            console.log(output);

            if (!form.valid()) { 
                return false;
            } else {
                $.ajax({
                    type: "POST",
                    url: url,
                    data: output,
                    dataType: "json",
                    success: function (response, textStatus, jqXhr) {
                        if (response.Status === 0) {
                            showMessageWithTitle(jqXhr.status, response.Data.Message, "success", 20000);
                            window.location.href = '/CustOrder/CreateBill';
                        } else {
                            if (response.Errors !== null) {
                                var result = '';
                                $.each(response.Errors, function (i, obj) {
                                    result += obj.ErrorMessage + '<br/>';
                                });
                                showMessageWithTitle(500, result, "error", 50000);
                            }
                        }
                        return false;
                    },
                    error: function (jqXhr, textStatus, errorThrown) {
                        handleAjaxErrors(jqXhr, textStatus, errorThrown);
                    }
                });
            }
        },
        handleCheckedListBox: function () {
            $('ul.checked-list-box li').children('.list-group-item').each(function () {
                // Settings
                var $widget = $(this),
                   $checkbox = $('<input type="checkbox" class="hidden" />'),
                   color = ($widget.data('color') ? $widget.data('color') : "primary"),
                   style = ($widget.data('style') === "button" ? "btn-" : "list-group-item-"),
                   settings = {
                       on: {
                           icon: 'glyphicon glyphicon-check'
                       },
                       off: {
                           icon: 'glyphicon glyphicon-unchecked'
                       }
                   };
                $widget.css('cursor', 'pointer');
                $widget.append($checkbox);

                // Actions
                function updateDisplay() {
                    var isChecked = $checkbox.is(':checked');

                    // Set the button's state
                    $widget.data('state', (isChecked) ? "on" : "off");

                    // Set the button's icon
                    $widget.find('.state-icon')
                        .removeClass()
                        .addClass('state-icon ' + settings[$widget.data('state')].icon);

                    // Update the button's color
                    if (isChecked) {
                        $widget.addClass(style + color + ' active');
                        $widget.parent().addClass('selected');
                    } else {
                        $widget.removeClass(style + color + ' active');
                        $widget.parent().removeClass('selected');
                    }
                }

                //Event Handle
                //$widget.find('.select-package').on('click', function () {
                //    $checkbox.prop('checked', !$checkbox.is(':checked'));
                //    $checkbox.triggerHandler('change');
                //    updateDisplay();

                //    if ($checkbox.is(':checked')) {
                //        $(this).removeClass("btn-success").addClass("btn-warning").html('Remove');
                //    } else {
                //        $(this).removeClass("btn-warning").addClass("btn-success").html('Apply');
                //    }
                //});

                $widget.on('click', function () {
                    $checkbox.prop('checked', !$checkbox.is(':checked'));
                    $checkbox.triggerHandler('change');
                    updateDisplay();

                    if ($checkbox.is(':checked')) {
                        $(this).find('button').removeClass("btn-success").addClass("btn-warning").html('Remove');
                    } else {
                        $(this).find('button').removeClass("btn-warning").addClass("btn-success").html('Apply');
                    }
                });

                $checkbox.on('change', function () {
                    updateDisplay();
                });

                // Initialization
                function init() {
                    if ($widget.data('checked') === true) {
                        $checkbox.prop('checked', !$checkbox.is(':checked'));
                    }

                    updateDisplay();

                    // Inject the icon if applicable
                    if ($widget.find('.state-icon').length === 0) {
                        $widget.find('.item-name').prepend('<span class="state-icon ' + settings[$widget.data('state')].icon + '"></span>');
                        // $widget.prepend('<span class="state-icon ' + settings[$widget.data('state')].icon + '"></span>');
                    }
                }
                init();
            });

            //$('#get-checked-data').on('click', function (event) {
            //    event.preventDefault();
            //    var checkedItems = {}, counter = 0;
            //    $("#check-list-box li.active").each(function (idx, li) {
            //        checkedItems[counter] = $(li).text();
            //        counter++;
            //    });
            //    $('#display-json').html(JSON.stringify(checkedItems, null, '\t'));
            //});
        },
        populatePackagesByCategoryId: function () {
            var container = $("#service-package-container");
            var url = container.data('url');
            var typeId = container.data('typeid');
            var categoryId = $("#CategoryId").val();

            var params = {
                "typeId": typeId,
                "categoryId": categoryId
            };

            $.ajax({
                type: 'GET',
                url: url,
                data: params,
                beforeSend: function () {
                    $.unblockUI();
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                },
                success: function (data) {
                    container.removeClass('hide');
                    if (data === null || data === '') {
                        data = '<div class="alert alert-info text-center" role="alert"><strong>Not found any service packages</strong></div>';
                    }
                    container.html(data);
                    bookingPackage.handleCheckedListBox();
                }
            });
        },
        populateCategoryComboTree: function () {
            var select = $("#CategoryId");
            var url = select.data('url');
            var selectedValue = select.val();

            var typeId = $('input[type=radio][name="TypeId"]:checked').val();
            var params = {
                "typeId": typeId,
                "selectedId": selectedValue,
                "isRootShowed": true
            };
            $.ajax({
                type: 'GET',
                dataType: "json",
                url: url,
                data: params,
                beforeSend: function () {
                    $.unblockUI();
                },
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
                            //if (selectedValue !== null && selectedValue !== '') {
                            //    //console.log(selectedValue);
                            //}
                            bookingPackage.populatePackagesByCategoryId();
                        }
                    });
                }
            });
        },
        init: function () {
            invokeDateTimePicker('dd/MM/yyyy');
            handleRadios();
            handleCheckBoxes();

            this.populateCategoryComboTree();
            this.bindPackageEvents();
        },
        success: function (response) {
            $("body").removeClass("loading");
            if (response.Result === "success") {
                debugger;
                $('#ServicePackChoosed').append(
                    $('<span></span>').html(response.Data.ServicePackName));
            } else {
                debugger;
                $('#MessageResult').append(
                    $('<span></span>').html("add service fail"));
            }
            reload();
        },
        fail: function () {
            $("body").removeClass("loading");
            $('#MessageResult').append(
                   $('<span></span>').html("add service fail"));
        }
    };

    //Load Package Form by Service Type Id (single or full)
    var loadServiceFormByTypeId = function () {
        var url = '';
        var selectedValue = $('input[type=radio][name="TypeId"]:checked').val();

        if (selectedValue === "1") {
            url = "/Booking/LoadSingleService";
        } else {
            url = "/Booking/LoadFullService";
        }

        $.ajax({
            url: url,
            type: "GET",
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (data) {
                $('#package-container').html(data);
                if (selectedValue === "1") {
                    bookingSingle.init();
                }
                else {
                    bookingPackage.init();
                }
            }
        });
    }
    var handlePackageType = function () {
        $('input[type=radio][name="TypeId"]').change(function () {

            var radioName = $(this).attr("name");
            $("input:radio[name=" + radioName + "]").attr('checked', false);
            $(this).attr("checked", true);
            $(this).parent('label').removeClass('btn-default').addClass('active btn-success');
            $(this).parent('label').siblings().removeClass('active btn-success').addClass('btn-default');
            loadServiceFormByTypeId();
        });
    }
    handlePackageType();
});