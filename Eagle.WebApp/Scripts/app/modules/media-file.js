(function ($) {
    //Check and privew photo
    function checkPreviewPhoto() {
        $('#Photo').checkFile({
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'bmp'],
            allowedSize: 15 //15MB	
        });
        previewPhoto();
    }

    checkPreviewPhoto();
    
    function populateAlbumMultipleSelect() {
        var select = $('#Albums');
        var url = select.data('url');
        var selectedValue = select.data('value');
        var typeId = $('#TypeId').val();
        var topicId = $('#TopicId').val();
        var params = { "typeId": typeId, "topicId": topicId, "status": 1, "selectedId": selectedValue, "isShowSelectText": false };

        if (typeId !== null && typeId !== '' && typeId !== undefined && topicId !== null && topicId !== '' && topicId !== undefined)
        {
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
                                text: itemData.Text,
                                selected: itemData.Selected
                            }));
                        });

                        select.multipleSelect({
                            selectAll: false,
                            filter: true,
                            isOpen: true,
                            keepOpen: true

                        });
                        //select.find('option:first').attr('selected', true).siblings().attr('selected', false);
                    } else {
                        select.append($('<option/>', { value: 'Null', text: " ---" + window.NotFound + " --- " }));
                    }
                    return false;
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                }
            });
        }
        return false;
    }

    function populatePlayListMultipleSelect() {
        var select = $('#PlayLists');
        var url = select.data('url');
        var selectedValue = select.data('value');
        var typeId = $('#TypeId').val();
        var topicId = $('#TopicId').val();
        var params = { "typeId": typeId, "topicId": topicId, "status": 1, "selectedId": selectedValue, "isShowSelectText": false };
        if (typeId !== null && typeId !== '' && typeId !== undefined && topicId !== null && topicId !== '' && topicId !== undefined) {
            select.empty();
            $.ajax({
                type: "GET",
                url: url,
                data: params,
                beforeSend: function () {
                    $.unblockUI();
                },
                success: function(data) {
                    if (data.length > 0) {
                        $.each(data, function(index, itemData) {
                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text,
                                selected: itemData.Selected
                            }));
                        });

                        select.multipleSelect({
                            selectAll: false,
                            filter: true,
                            isOpen: true,
                            keepOpen: true

                        });
                        //select.find('option:first').attr('selected', true).siblings().attr('selected', false);
                    } else {
                        select.append($('<option/>', { value: 'Null', text: " ---" + window.NotFound + " --- " }));
                    }
                    return false;
                },
                error: function(jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                }
            });
        }
        return false;
    }
    
    //Create topic tree for searching -Topic ComboTree
    function populateSearchTopicComboTree() {
        var select = $("#SearchTopicId");
        var url = select.data('url');
        var selectedValue = select.val();
        var typeId = $("#SearchTypeId").val();

        var params = {
            "typeId": typeId,
            "selectedId": selectedValue,
            "isRootShowed": true
        };

        $.ajax({
            async: false,
            cache: false,
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
                    }
                });
            }
        });
    }
    populateSearchTopicComboTree();
    
    function populateTopicComboTree() {
        var select = $("#cbxTopicId");
        var url = select.data('url');
        var selectedValue = $("#TopicId").val();
        var typeId = $('#TypeId').find(":selected").val();

        var params = {
            "typeId": typeId,
            "selectedId": selectedValue,
            "isRootShowed": false
        };

        $.ajax({
            async: false,
            cache: false,
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
                        select.combotree('setValue', selectedValue);

                        populateAlbumMultipleSelect();
                        populatePlayListMultipleSelect();
                    },
                    onClick: function (node) {
                        selectedValue = node.id;
                        $(this).val(selectedValue);
                        $("#TopicId").val(selectedValue);
                        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                            populateAlbumMultipleSelect();
                            populatePlayListMultipleSelect();
                        }
                    }
                });
            }
        });
    }

    populateTopicComboTree();

    function populateTypeSelectBox() {
        var select = $('#TypeId');
        var url = select.data('url');
        var selectedValue = select.data('value');
        var params = { "selectedId": selectedValue, "isShowSelectText": false };

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
                            text: itemData.Text,
                            selected: itemData.Selected
                        }));
                    });
                  
                    $(document).on("change", "#TypeId", function (e) {
                        e.preventDefault();
                        var selectedValue = $(this).find(":selected").val();
                        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                            $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                            populateTopicComboTree();
                        }

                        return false;
                    });
                    //select.find('option:first').attr('selected', true).siblings().attr('selected', false);
                } else {
                    select.append($('<option/>', { value: 'Null', text: " ---" + window.Select + " --- " }));
                }
                return false;
            },
            complete: function () {
                populateTopicComboTree();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    }
    populateTypeSelectBox();

    function populateSearchTypeSelectBox() {
        var select = $('#SearchTypeId');
        var url = select.data('url');
        var selectedValue = select.data('value');
        var params = { "selectedId": selectedValue, "isShowSelectText": true };

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
                            text: itemData.Text,
                            selected: itemData.Selected
                        }));
                    });

                    $(document).on("change", "#SearchTypeId", function (e) {
                        e.preventDefault();
                        var selectedValue = $(this).find(":selected").val();
                        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                            $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                            populateSearchTopicComboTree();
                        }
                        return false;
                    });
                    //select.find('option:first').attr('selected', true).siblings().attr('selected', false);
                } else {
                    select.append($('<option/>', { value: 'Null', text: " ---" + window.Select + " --- " }));
                }
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    }

    populateSearchTypeSelectBox();
    handleCheckBoxEvent();


    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            //beforeSend: function () {
            //    $.unblockUI();
            //},
            success: function (data) {
                $('#divEdit').html(data);
                populateTypeSelectBox();
                handleCheckBoxEvent();
                return false;
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
                return false;
            }
        });
    }

    function resetControls(formId) {
        var validateObj = $('#' + formId);
        validateObj.find('input:text,input:hidden, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

    function search() {
        var formData = $('#frmSearch').serialize();
        $.ajax({
            type: "GET",
            url: window.MediaFileSearchUrl,
            data: formData,
            success: function (data) {
                $('#search-result').html(data);
            }
        });
    }

    function create(url, formId) {
        var form = $("#" + formId);
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("Media", $('input[type=file]')[0].files[0]);
        formData.append("Photo", $('input[type=file]')[1].files[1]);
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
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    resetControls(formId);
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
        var params = form.serializeArray();
        var formData = new FormData();
        formData.append("Media", $('input[type=file]')[0].files[0]);
        formData.append("Photo", $('input[type=file]')[1].files[1]);
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
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    getDetails(url, response.Data.Id);
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
    }

    function deleteData(url, id) {
        var params = { id: id };
        $.ajax({
            type: 'DELETE',
            url: url,
            data: params,
            cache: false,
            dataType: "json",
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                   // resetControls();
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
            beforeSend: function () {
                $.unblockUI();
            },
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    //resetControls();
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

    //$(document).on("change", '#SearchStatus', function (e) {
    //    var selectedValue = $(this).find(":selected").val();
    //    $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
    //});
    //$('#SearchStatus').find('option:first').attr('selected', true).siblings().attr('selected', false);

    $(document).on('click', '.search', function () {
        search();
    });

    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var formId = $(this).data('form');
        //$.validator.addMethod('accept', function () { return true; });

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            if ($('input[type=file]').get(0).files.length === 0) {
                showMessageWithTitle(500, "Please select photo", "error", 50000);
            } else {
                create(url, formId);
            }
            return false;
        }
    });

    $(document).on("click", ".edit", function (e) {
        e.preventDefault();

        var formId = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        }
        else {
            var flag = false;
            if ($('input[type=file]')[0].files[0] !== null) {
                flag = true;
            } else {
                if ($('input[name=File]').val() !== null && $('input[name=File]').val() !== '') {
                    flag = true;
                }
            }

            if (flag) {
                edit(url, formId);
            } else {
                showMessageWithTitle(500, "Please select photo", "error", 50000);
            }
            return false;
        }
    });

    $(document).on("change", ".changeStatus", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        var warning = $(this).data('warning');
        var status = $(this).find('input[type="radio"]:checked').val();

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

    $(document).on("click", ".editItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var url = $(this).data('url');
        getDetails(url, id);

        //Go to top
        $('html, body').animate({ scrollTop: 0 }, 'fast');
        return false;
    });

    $(document).on("click", ".reset", function (e) {
        e.preventDefault();
        var mode = $(this).data('mode');
        if (mode === 'edit') {
            var id = $(this).data('id');
            var url = $(this).data('url');
            //console.log(id);
            getDetails(url, id);
        } else {
            var formId = $(this).data('form');
            resetControls(formId);
        }
        return false;
    });

    $(document).on("change", "#StorageType", function () {
        var selectedValue = $(this).val();
        console.log(selectedValue);
        var divContainer1 = $("#video-source");
        var divContainer2 = $("#video-container");

        if (selectedValue === "1") {
            divContainer1.css("display", "none");
            divContainer2.css("display", "block");
        } else {
            divContainer1.css("display", "block");
            divContainer2.css("display", "none");
        }
        return false;
    });


})(jQuery);