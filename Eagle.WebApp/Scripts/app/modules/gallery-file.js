(function ($) {
    //Check and privew photo
    function checkPreviewPhoto() {
        $('#File').checkFile({
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'bmp'],
            allowedSize: 15 //15MB	
        });
        previewPhoto();
    }

    function populateGalleryCollectionSelectBox() {
        var topicId = $('#TopicId').val();
        var select = $('#CollectionId');
        var url = select.data('url');
        var selectedValue = select.data('value');
        var params = { "topicId": $('#TopicId').val(), "selectedValue": selectedValue, "isShowSelectText": true };

        select.empty();
        if (topicId !== null && topicId !== '' && topicId !== undefined && topicId > 0) {
            $.ajax({
                type: "GET",
                url: url,
                data: params,
                success: function (data) {
                    if (data.length > 0) {
                        $.each(data, function (index, itemData) {
                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text,
                                selected: itemData.Selected
                            }));
                        });
                    } else {
                        select.append($('<option/>', { value: 'Null', text: " ---" + window.Select + " --- " }));
                    }
                    return false;
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                }
            });
        }
        else {
            select.append($('<option/>', { value: 'Null', text: " ---" + window.None + " --- " }));
        }
        return false;
    }
    
    //Create topic tree for creating and editing
    function createTree(data) {
        var selectedValue = $('#TopicId').val();
        var $tree = $("#tree");
        $tree.tree({
            lines: true,
            animate: true,
            data: data,
            formatter: function (node) {
                var s = node.text;
                if (node.children) {
                    s += '&nbsp;<span style=\'color:blue\'>(' + node.children.length + ')</span>';
                }
                return s;
            },
            onLoadSuccess: function () {
                if (selectedValue !== null) {
                    var node = $tree.tree('find', selectedValue);
                    if (node) {
                        $tree.tree('select', node.target);
                    }
                }
            },
            onClick: function (node) {
                $('#TopicId').val(node.id);
                populateGalleryCollectionSelectBox();
            }
        });
    }

    function loadTree() {
        var selectedId = $('#TopicId').val();
        $.ajax({
            type: "GET",
            url: window.GetGalleryTopicTreeUrl,
            data: { 'selectedId': selectedId, 'isRootShowed': false },
            success: function (data) {
                createTree(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }
    
    //Collection ComboBox
    function populateGalleryCollectionComboBox() {
        var select = $('#SearchCollectionId');
        var url = select.data('url');
        var topicId = $('#SearchTopicId').val();
        var params = { "topicId": topicId, "selectedValue": "", "isShowSelectText": true };
        
        select.empty();
        if (topicId !== null && topicId !== '' && topicId !== undefined && topicId > 0) {
            $.ajax({
                type: "GET",
                url: url,
                data: params,
                success: function (data) {
                    if (data.length > 0) {
                        $.each(data, function (index, itemData) {
                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text,
                                selected: itemData.Selected
                            }));
                        });

                        //select.select2();
                    } else {
                        select.append($('<option/>', { value: 'Null', text: " ---" + window.None + " --- " }));
                    }
                    return false;
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                }
            });
        }
        else {
            select.append($('<option/>', { value: 'Null', text: " ---" + window.None + " --- " }));
        }
        return false;
    }

   // populateGalleryCollectionComboBox();

    //Topic ComboTree
    function populateTopicComboTree() {
        var select = $("#cbxTopicTree");
        var url = select.data('url');
        var topicCtrl = select.siblings('input[type="hidden"]');
        var selectedValue = topicCtrl.val();

        var params = {
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
                    onLoadSuccess: function (row, data){
                        $(this).tree("expandAll");
                        select.combotree('setValues', [0]);
                        populateGalleryCollectionComboBox();
                    },
                    onClick: function (node) {
                        selectedValue = node.id;
                        topicCtrl.val(selectedValue);
                        populateGalleryCollectionComboBox();
                    }
                });
            }
        });
    }

    populateTopicComboTree();

    function getDetails(url, collectionId, fileId) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "collectionId": collectionId, "fileId": fileId },
            success: function (data) {
                //$('#divEdit').html(data);
                var title = "Edit File";

                showPopUp(title, data);
                loadTree();
                populateGalleryCollectionSelectBox();
                checkPreviewPhoto();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
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
            url: window.GalleryFileSearchUrl,
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
        formData.append("File", $('input[type=file]')[0].files[0]);
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
        formData.append("File", $('input[type=file]')[0].files[0]);
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
                    getDetails(url, response.Data.CollectionId, response.Data.FileId);
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

    function deleteData(url, form, collectionId, fileId) {
        var params = { "collectionId": collectionId, "fileId": fileId };
        $.ajax({
            type: 'DELETE',
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    resetControls(form);
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

    function updateStatus(url, form, collectionId, fileId, status) {
        var params = { "collectionId": collectionId, "fileId": fileId, "status": status };
        $.ajax({
            type: "PUT",
            url: url,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    resetControls(form);
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

    function showPopUp(title, content) {
        var dialog = bootbox.dialog({
            className: "my-modal",
            title: title,
            message: content,
            backdrop: true,
            closeButton: true,
            onEscape: true,
        }).find("div.modal-dialog").css({ "width": "80%" });

        dialog.css({
            'top': '10%',
            'margin-top': function () {
                return -($(this).height() / 2);
            }
        });

        return false;
    }

    //$(document).on("change", '#SearchStatus', function (e) {
    //    var selectedValue = $(this).find(":selected").val();
    //    $(this).find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
    //});
    //$('#SearchStatus').find('option:first').attr('selected', true).siblings().attr('selected', false);

    $(document).on('click', '.search', function () {
        search();
    });

    $(document).on("click", ".populate-add-form", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        $.ajax({
            type: "GET",
            dataType: "html",
            url: url,
            success: function (data, statusCode, xhr) {
                var title = 'Add File';
                showPopUp(title, data);
                checkPreviewPhoto();
                loadTree();
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    });

    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var formId = $(this).data('form');

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
        }
    });

    $(document).on("change", ".changeStatus", function (e) {
        e.preventDefault();

        var collectionId = $(this).data('collectionid');
        var fileId = $(this).data('fileid');
        var form = $(this).data('form');
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
                    updateStatus(url, form, collectionId, fileId, status);
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

        var collectionId = $(this).data('collectionid');
        var fileId = $(this).data('fileid');
        var form = $(this).data('form');
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
                    deleteData(url, form, collectionId, fileId);
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
        var collectionid = $(this).data('collectionid');
        var fileid = $(this).data('fileid');
        var url = $(this).data('url');
        getDetails(url, collectionid, fileid);

        //Go to top
        $('html, body').animate({ scrollTop: 0 }, 'fast');
        return false;
    });

    $(document).on("click", ".reset", function () {
        var formId =  $(this).data('form');        
        var mode = $(this).data('mode');
        
        if (mode === 'edit') {
            var id = $(this).data('id');
            var url = $(this).data('url');
            getDetails(url, id);
        } else {
            resetControls(formId);
        }
        return false;
    });

})(jQuery);