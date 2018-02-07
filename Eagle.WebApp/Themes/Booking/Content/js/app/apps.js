function setupMenu() {
    //VERTICAL-MENU ------------------------------------------------------------------------------------------------
    $('ul.sf-menu sf-vertical').superfish({
        animation: { height: 'show' },   // slide-down effect without fade-in
        delay: 1200               // 1.2 second delay on mouseout
    });
    //MENU END =====================================================================================================

    //HORIZONTAL-MENU START ===================================================================================================
    jQuery('ul.sf-menu').superfish({
        delay: 700,                            // one second delay on mouseout
        animation: { opacity: 'show', height: 'show' },  // fade-in and slide-down animation
        speed: 'fast',                          // slow or fast  - animation speed
        dropShadows: false,                            // disable drop shadows
        dualColumn: 12, //if a submenu has at least this many items it will be divided in 2 columns
        tripleColumn: 16, //if a submenu has at least this many items it will be divided in 3 columns
        //onBeforeShow: function () {
        //    if ($(this).closest('ul').hasClass('sf-navbar') == true ||
        //        this == []) {
        //        return;
        //    }

        //    var wid = $(window).width();
        //    var ulparent = $(this).closest('ul');
        //    if (ulparent.offset() != undefined) {
        //        if (ulparent.offset().left + 2 * ulparent.width() > wid) {
        //            $(this).addClass('pullleft');
        //        }
        //    }
        //}
    });

    //$("ul.sf-menu li").addClass("ui-state-default");

    //$("ul.sf-menu li").hover(function () {
    //    $(this).addClass('ui-state-hover');
    //    //$(this).find('.ul-menu-list').addClass("ul-menu-style");
    //},
    //function () {
    //    $(this).removeClass('ui-state-hover');
    //    //$(this).find('.ul-menu-list').removeClass('ul-menu-style');
    //});
    //MENU END =====================================================================================================

}

function loadSiteMap() {
    var menuId = 0;
    var sPageUrl = window.location.href;
    var indexOfLastSlash = sPageUrl.lastIndexOf("/");

    if (indexOfLastSlash > 0 && sPageUrl.length - 1 !== indexOfLastSlash)
        menuId = sPageUrl.substring(indexOfLastSlash + 1);

    var siteMapUrl = '/Admin/Menu/PopulateSiteMapByMenuId?menuId=' + menuId;
    $.ajax({
        type: "GET",
        url: siteMapUrl,
        success: function (data) {
            $('#SiteMap').html(data);
        }, error: function (jqXhr, textStatus, errorThrown) {
            handleAjaxErrors(jqXhr, textStatus, errorThrown);
        }
    });
}


//function showDatePicker() {
//    $('.datepicker').datepicker({
//        format: "mm/dd/yyyy",
//       showButtonPanel: true,
//       closeText: 'Clear',
//        clearBtn: true
//    }).on('changeDate', function (e) {
//        var splitSignal = "/";
//        var arr = $(this).val().split(splitSignal);
//        var result = arr[0] + splitSignal + arr[1] + splitSignal + arr[2];
//        if (arr[0] != undefined && arr[1] != undefined && arr[2] != undefined)
//            $(this).next().val(result);
//        else
//            $(this).next().val('');
//    });
//}

function ShowDateTimePicker(format) {
    var formatDate = format.toLowerCase();
    $('.datepicker').datepicker({
        format: formatDate,
        autoclose: true,
        showOn: "button",
        clearBtn: true,
        minDate: "01/01/1925",
        maxDate: "31/12/2050",
        changeMonth: true,
        changeYear: true,
        yearRange: "1925:2050"
    }).on('changeDate', function (e) {
        var arr = $(this).val().split("/");
        var selectedDate = '';
        if (arr[0] !== undefined && arr[1] !== undefined && arr[2] !== undefined) {
            if (format === 'dd/MM/yyyy' || format === 'dd-MM-yyyy') {
                selectedDate = arr[1] + '/' + arr[0] + '/' + arr[2];
            }
            if (format === 'MM/dd/yyyy' || format === 'MM-dd-yyyy') {
                selectedDate = arr[0] + '/' + arr[1] + '/' + arr[2];
            }
            if (format === 'yyyy/MM/dd' || format === 'yyyy-MM-dd') {
                selectedDate = arr[2] + '/' + arr[1] + '/' + arr[0];
            }
            $(this).next().val(selectedDate);
        }
        else
            $(this).next().val('');

        $(this).datepicker('hide');
    }).on('hide', function (ev) {
        $(this).removeClass('opened');
    });
}

function PopulateDataTable(numrows) {
    $('.dataTable').dataTable({
        "language": {
            "sProcessing": "Đang xử lý...",
            "sLengthMenu": "Xem _MENU_ mục",
            "sZeroRecords": "Không tìm thấy dòng nào phù hợp",
            "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
            "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
            "sInfoFiltered": "(được lọc từ _MAX_ mục)",
            "sInfoPostFix": "",
            "sSearch": "Tìm:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Đầu",
                "sPrevious": "Trước",
                "sNext": "Tiếp",
                "sLast": "Cuối"
            }
        },
        "aoColumnDefs": [{
            "bSortable": false,
            "aTargets": ["no-sort"]
        }],
        "scrollX": true,
        'iDisplayLength': numrows
    });
    ResizeDataTable();
}

function InvokeDataTable() {
    $('.dataTable').dataTable({
        "language": {
            "sProcessing": "Đang xử lý...",
            "sLengthMenu": "Xem _MENU_ mục",
            "sZeroRecords": "Không tìm thấy dòng nào phù hợp",
            "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
            "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
            "sInfoFiltered": "(được lọc từ _MAX_ mục)",
            "sInfoPostFix": "",
            "sSearch": "Tìm:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Đầu",
                "sPrevious": "Trước",
                "sNext": "Tiếp",
                "sLast": "Cuối"
            }
        },
        "aoColumnDefs": [{
            "bSortable": false,
            "aTargets": ["no-sort"]
        }],
        "bAutoWidth": true,
        "processing": true,
        "scrollX": true,
        'iDisplayLength': 10
    });
    ResizeDataTable();
}

function ResizeDataTable() {
    $(".dataTables_scrollHead").css("width", "100%").css("margin-left", "0px");
    $(".dataTables_scrollHeadInner").css("width", "100%").css("margin-left", "0px");
    $(".dataTables_scrollHeadInner .dataTable").css("width", "99%").css("margin-left", "0px");
    $(".dataTables_scrollBody .dataTable").css("width", "100%").css("margin-left", "0px");
    $(".dataTables_scrollBody").css("width", "100%").css("margin-left", "0px");
    //$(".dataTables_scrollHeadInner").css("width", $(".dataTables_scrollBody .dataTable").width() * 100 / 99).css("margin-left", "0px");
}

function ValidateFormWithQtip(formId) {
    var formName = (formId === undefined || formId === '') ? 'myform' : formId;
    $('.qtip').remove();
    // Run this function for all validation error messages
    $('#' + formName + ' .field-validation-error').each(function () {

        // Get the name of the element the error message is intended for
        // Note: ASP.NET MVC replaces the '[', ']', and '.' characters with an
        // underscore but the data-valmsg-for value will have the original characters
        var inputElem = '#' + $(this).attr('data-valmsg-for').replace('.', '_').replace('[', '_').replace(']', '_');

        var corners = ['left bottom', 'top left'];
        var flipIt = $(inputElem).parents('span.right').length > 0;

        // Hide the default validation error
        $(this).addClass('Hidden');

        // Show the validation error using qTip
        $(inputElem).filter(':not(.valid)').qtip({
            content: { text: $(this).text() }, // Set the content to be the error message
            position: {
                my: corners[flipIt ? 0 : 1],
                at: corners[flipIt ? 1 : 0],
                viewport: $(window)
            },
            show: { event: false, when: false, ready: true },
            hide: false,
            style: { classes: 'ui-tooltip-red' }
        });
    });
    $.validator.unobtrusive.parse($('#' + formName));
}

function PopulateDropDownListAutoComplete(strSelectBox, strHiddenId, strHiddenName, strPlaceholder, requestUrl) {
    var pageSize = 20;
    var selectBox = $("#" + strSelectBox);
    var hiddenId = $("#" + strHiddenId);
    var hiddenName = $("#" + strHiddenName);

    selectBox.select2({
        placeholder: strPlaceholder,
        minimumInputLength: 0,
        allowClear: true,
        ajax: {
            url: requestUrl,
            dataType: 'json',
            quietMillis: 100,  //How long the user has to pause their typing before sending the next request
            data: function (term, page) {
                return {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term
                };
            },
            results: function (data, page) {
                //Used to determine whether or not there are more results available,
                //and if requests for more data should be sent in the infinite scrolling
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        },
        formatSelection: function (result) {
            hiddenId.val(result.id);
            hiddenName.val(result.name);
            hiddenId.trigger("change");
            return result.name;
        },
        formatResult: function (result) {
            return result.name;
        },
        initSelection: function (element, callback) {

            var selectedId = hiddenId.val();
            var selectedName = hiddenName.val();
            var data = { id: selectedId, name: selectedName, text: selectedName };
            callback(data);
        }
    });
    selectBox.select2('val', hiddenId.val());
    selectBox.on("select2-removed", function (e) {
        hiddenId.val('');
        hiddenName.val('');
    });
}

function PopulateCountryProvinceDistrictToDropDownList(selectBox, strInputCountry) {
    var requestUrl = '/LS_tblCountry/DropdownList';
    var pageSize = 20;
    var countrySelectBox = $("#Select" + selectBox + 'CountryID');
    var countryHiddenId = $("#" + selectBox + 'CountryID');
    var countryHiddenName = $("#" + selectBox + 'CountryName');
    countrySelectBox.select2({
        placeholder: strInputCountry,
        minimumInputLength: 0,
        allowClear: true,
        multiple: false,
        ajax: {
            url: requestUrl,
            dataType: 'json',
            params: {
                contentType: 'application/json; charset=utf-8'
            },
            quietMillis: 100,  //How long the user has to pause their typing before sending the next request
            data: function (term, page) {
                return {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term
                };
            },
            results: function (data, page) {
                //Used to determine whether or not there are more results available,
                //and if requests for more data should be sent in the infinite scrolling
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        },
        //Chọn xong => làm gì đó
        formatSelection: function (result) {
            countryHiddenId.val(result.id);
            PopulateProvincesToDropDownList(selectBox);
            countryHiddenName.val(result.name);
            return result.name;
        },
        //Chọn xong => return kết quả hiển thị
        formatResult: function (result) {
            return result.name;
        },
        //Đầu tiên gán vào từ đầu
        initSelection: function (element, callback) {
            var selectedId = countryHiddenId.val();
            var selectedName = countryHiddenName.val();
            var data = { id: selectedId, name: selectedName, text: selectedName };
            callback(data);
        }
    });

    countrySelectBox.select2('val', countryHiddenId.val());
    PopulateProvincesToDropDownList(selectBox);
}

function PopulateProvincesToDropDownList(selectBox, strInputProvince) {

    var requestUrl = '/LS_tblProvince/DropdownList';
    var pageSize = 20;
    var provinceSelectBox = $("#Select" + selectBox + 'ProvinceID');
    var provinceHiddenId = $("#" + selectBox + 'ProvinceID');
    var provinceHiddenName = $("#" + selectBox + 'ProvinceName');
    var countryid = $("#" + selectBox + "CountryID").val();

    provinceSelectBox.select2({
        placeholder: strInputProvince,
        minimumInputLength: 0,
        allowClear: true,
        ajax: {
            url: requestUrl,
            dataType: 'json',
            quietMillis: 100,  //How long the user has to pause their typing before sending the next request
            data: function (term, page) {
                return {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term,
                    CountryID: countryid
                };
            },
            results: function (data, page) {
                //Used to determine whether or not there are more results available,
                //and if requests for more data should be sent in the infinite scrolling
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        },
        formatSelection: function (result) {
            provinceHiddenId.val(result.id);
            PopulateDistrictsToDropDownList(selectBox);
            provinceHiddenName.val(result.name);
            return result.name;
        },
        formatResult: function (result) {
            return result.name;
        },
        initSelection: function (element, callback) {
            var selectedId = provinceHiddenId.val();
            var selectedName = provinceHiddenName.val();
            var data = { id: selectedId, name: selectedName, text: selectedName };
            callback(data);
        }
    });
    provinceSelectBox.select2('val', provinceHiddenId.val());
    PopulateDistrictsToDropDownList(selectBox);
}

function PopulateDistrictsToDropDownList(selectBox, strInputDistrict) {
    var requestUrl = '/LS_tblDistrict/DropdownList';
    var pageSize = 20;
    var districtSelectBox = $("#Select" + selectBox + 'DistrictID');
    var districtHiddenId = $("#" + selectBox + 'DistrictID');
    var districtHiddenName = $("#" + selectBox + 'DistrictName');

    var provinceId = $("#" + selectBox + "ProvinceID").val();
    districtSelectBox.select2({
        placeholder: strInputDistrict,
        minimumInputLength: 0,
        allowClear: true,
        ajax: {
            url: requestUrl,
            dataType: 'json',
            quietMillis: 100,  //How long the user has to pause their typing before sending the next request
            data: function (term, page) {
                return {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term,
                    ProvinceID: provinceId
                };
            },
            results: function (data, page) {
                //Used to determine whether or not there are more results available,
                //and if requests for more data should be sent in the infinite scrolling
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        },
        formatSelection: function (result) {
            districtHiddenId.val(result.id);
            districtHiddenName.val(result.name);
            return result.name;
        },
        formatResult: function (result) {
            return result.name;
        },
        initSelection: function (element, callback) {
            var selectedId = districtHiddenId.val();
            var selectedName = districtHiddenName.val();
            var data = { id: selectedId, name: selectedName, text: selectedName };
            callback(data);
        }
    });
    districtSelectBox.select2('val', districtHiddenId.val());
}

function LoadComboTreeWithAction(controlId, action) {
    $('#' + controlId).combotree({
        url: '/CommonCompany/' + action,
        textField: 'title',
        valueField: 'id',
        onLoadSuccess: function (row, data) {
            $(this).tree("collapseAll");
        },
        onSelect: function (node) {
            $("input[name='" + controlId + "']").val(node.id);
            $("input[name='" + controlId + "']").trigger("change");
        }
    });
}

function LoadComboTreeWithActionWithRequired(controlId, action, requiredMessage, selectedvalue) {

    $('#' + controlId).combotree({
        url: '/CommonCompany/' + action,
        textField: 'title',
        valueField: 'id',
        onLoadSuccess: function (row, data) {
            if (selectedvalue === null) {
                $(this).tree("collapseAll");
            }
        },
        onSelect: function (node) {
            $("input[name='" + controlId + "']").val(node.id);
            $("input[name='" + controlId + "']").trigger("change");

            // xóa qtip
            if (!isNaN(node.id)) {
                var $input = $('#' + controlId).siblings().children("input");
                if ($input.attr('aria-describedby') !== undefined) {
                    $('#' + $input.attr('aria-describedby')).remove();
                }
            }
        }
    });

    if (selectedvalue !== null) {
        $('#' + controlId).combotree('setValue', selectedvalue);
    }
    //$input = $('#' + controlId + '.easyui-combotree').siblings().children("input");
    //$input.addClass('input-small input-validation-error');
    //$input.attr('data-val', 'true');
    //$input.attr('data-val-required', requiredMessage);
}

function LoadComboTreeWithActionWithValue(controlId, action, selectedvalue) {

    $('#' + controlId).combotree({
        url: '/CommonCompany/' + action,
        textField: 'title',
        valueField: 'id',
        onLoadSuccess: function (row, data) {
            if (selectedvalue === null) {
                $(this).tree("collapseAll");
            }
        },
        onSelect: function (node) {
            $("input[name='" + controlId + "']").val(node.id);
            $("input[name='" + controlId + "']").trigger("change");

            // xóa qtip
            if (!isNaN(node.id)) {
               var $input = $('#' + controlId).siblings().children("input");
                if ($input.attr('aria-describedby') !== undefined) {
                    $('#' + $input.attr('aria-describedby')).remove();
                }
            }
        }
    });

    if (selectedvalue !== null) {
        $('#' + controlId).combotree('setValue', selectedvalue);
    }
}

function loadComboTree(selectId, actionUrl, isRequired) {
    var selectBox = $('#' + selectId);

    var hiddenBox = $('input[type=hidden][name="' + selectId + '"]');
    var selectedValue = selectBox.val();

    if (isRequired === undefined)
        isRequired = false;

    selectBox.combotree({
        url: actionUrl,
        textField: 'text',
        valueField: 'id',
        required: isRequired,
        editable: false,
        onLoadSuccess: function (row, data) {
            $(this).tree("collapseAll");
        },
        onSelect: function (node) {
            var tree = $(this).tree;
            //Selected node is a leaf node, if it is not a leaf node, clear the check
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                showMessageWithTitle("Please Select", result.message, "error");
            } else {
                hideMessageWithTitle(20000);
            }
        },
        onClick: function (node) {
            selectedValue = node.id;
            $(this).val(selectedValue);
            hiddenBox.val(selectedValue);
        }
    });
    selectBox.combotree('setValue', selectedValue);
}

function uploadFile(folderKey, fileKey, hiddenFileId, message) {
    if ($('input[type="file"]').val() !== '') {
        var hiddenBox = $("input[name=" + hiddenFileId + "]:hidden");
        var fileId = hiddenBox.val();

        var formData = new FormData();
        formData.append('fileKey', fileKey); //fileKey = 'FileUpload'
        formData.append('FileUpload', $('input[type=file]')[0].files[0]);
        formData.append('folderKey', folderKey);
        formData.append('fileId', fileId);

        var baseUrl = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');
        $.ajax({
            type: "POST",
            url: baseUrl + "/Handlers/UploadFile.ashx",
            contentType: false,
            processData: false,
            data: formData,
            success: function (result) {
                if (result >= 1) {
                    hiddenBox.val(result);
                }
            },
            error: function () {
                showMessageWithTitle('error', message, "error", 3000);
            }
        });
    }
}



function showUITooltip() {
    $("#myform :input").tooltip({
        // place tooltip on the right edge
        position: "center right",

        // a little tweaking of the position
        offset: [-2, 10],

        // use the built-in fadeIn/fadeOut effect
        effect: "fade",

        // custom opacity setting
        opacity: 0.7
        //content: function () {
        //    return $(this).prop('title');
        //}
    });
}

//function ShowToolTip(elementId, message) {
//    if (elementId != undefined) {
//        var qtipId = 'qtip-' + elementId;
//        var element = $('input[name="' + elementId + '"]');

//        if (element.hasClass('input-validation-error'))
//            element.addClass('input-validation-error');
//        element.attr({ 'data-hasqtip': elementId, 'aria-describedby': qtipId });


//        var valSpan = $('span[data-valmsg-for="' + elementId + '"]');
//        if (valSpan == undefined && valSpan === null) {
//            valSpan = $('<span/>', { attr: { 'data-valmsg-replace': true, 'data-valmsg-for': elementId } }).insertAfter(element);
//            valSpan.addClass('field-validation-error').removeClass('field-validation-valid');
//        }

//        var divContent = $('<div/>', {
//            id: qtipId + '-content',
//            // 'class': 'qtip qtip-default ui-tooltip-red qtip-pos-rc qtip-focus',
//            'class': 'ui-tooltip-red qtip-pos-rc qtip-focus',
//            attr: { 'aria-atomic': "true" },
//            html: '<span id="' + qtipId + '-content-span" for="' + elementId + '"></span>'
//        });

//        var divWrapper = $('<div/>', {
//            id: qtipId,
//            'class': 'qtip-default ui-tooltip-red qtip-pos-rc qtip-focus',
//            attr: {
//                style: "z-index: 15003; display: block; top: 375px; left: 737.467px;",
//                tracking: "false", role: "alert", "aria-live": "polite", "aria-atomic": "false",
//                "aria-describedby": "qtip-1-content", "aria-hidden": "false", "data-qtip-id": "1"
//            }
//        });

//        if (valSpan !=undefined && valSpan.siblings('#' + qtipId).length === 0) {
//            divWrapper.append(divContent);
//            divWrapper.insertAfter(valSpan);
//        }


//        showMessageWithTitle('warning', message, "warning", 3000);
//        $('#' + qtipId + '-content-span').html(message);
//        element.show();
//    }
//}

//function closeToolTip(elementId) {
//    $('#qtip-' + elementId).remove();
//}