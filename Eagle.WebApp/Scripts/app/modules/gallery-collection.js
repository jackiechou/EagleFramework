(function ($) {
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

    loadTree();

    //Create topic tree for searching
    function populateSelectTree() {
        var selectBox = $('#SelectTopic');
        var hiddenBoxId = $('#SearchTopicId');
        var hiddenBoxName = $('#SearchTopicName');
        var selectedValue = hiddenBoxId.val();
        var url = window.GetGalleryTopicSelectTreeUrl;
        var params = { 'selectedId': selectedValue, 'isRootShowed': true };

        selectBox.empty();
        $.getJSON(url, params,
            function (classesData) {
                if (classesData.length > 0) {
                    $.each(classesData, function (index, itemData) {
                        selectBox.append($('<option/>', {
                            value: itemData.id,
                            text: itemData.text,
                            selected: itemData.selected
                        }));
                    });

                    if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                        selectBox.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                    }
                    selectBox.select2({
                        placeholder: 'Select an option',
                        width: "100%"
                    });

                    selectBox.on("select2:select", function (e) {
                        var selected = e.params.data;
                        if (typeof selected !== "undefined" && selected !== null) {
                            hiddenBoxId.val(selected.id);
                            hiddenBoxName.val(selected.text);
                        }
                    }).on("select2:unselecting", function (e) {
                        $("form").each(function () {
                            this.reset();
                        });
                        hiddenBoxId.val('');
                        hiddenBoxName.val('');
                    });

                    //selectBox.select2({
                    //    width: '600px',
                    //    //formatSelection: function (item) {
                    //    //    return item.text;
                    //    //},
                    //    //formatResult: function (item) {
                    //    //    return item.text;
                    //    //},
                    //    //templateResult: function (item) {
                    //    //    var $result = $('<span style="padding-left:' + (20 * item.level) + 'px;">' + item.text + '</span>');
                    //    //    return $result;
                    //    //}
                    //});
                }
            });

    }

    populateSelectTree();

    function getDetails(url, id) {
        $.ajax({
            type: "GET",
            url: url,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                loadTree();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function resetControls(formId) {
        var validateObj = $('#' + formId);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

    function search() {
        $.ajax({
            type: "GET",
            url: window.GalleryCollectionSearchUrl,
            data: $("#frmSearch").serialize(),
            success: function (data) {
                $('#search-result').html(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function create(url, formId) {
        var params = $("#" + formId).serialize();
        $.ajax({
            type: "POST",
            url: url,
            data: params,
            //headers: { '__RequestVerificationToken': $('[name=__RequestVerificationToken]').val() },
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    search();
                    loadTree();
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
        var formData = $("#" + formId).serialize();

        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            dataType: "json",
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
        var params = { "id": id };
        $.ajax({
            type: 'DELETE',
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
        var params = { "id": id, "status": status };
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
    
    $(document).on('click', '.search', function () {
        search();
    });

    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var url = $(this).data('url');
        var formId = $(this).data('form');
        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        } else {
            create(url, formId);
            return false;
        }
    });

    $(document).on("click", ".editItem", function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var url = $(this).data('url');
        getDetails(url, id);
        return false;
    });

    $(document).on("click", ".edit", function (e) {
        e.preventDefault();
        var url = $(this).data('url');
        var formId = $(this).data('form');

        if (!$("#" + formId).valid()) { // Not Valid
            return false;
        }
        else {

            edit(url, formId);
            return false;
        }
    });

    $(document).on("click", ".delete", function (e) {
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

    $(document).on("click", ".reset", function () {
        var mode = $(this).data('mode');
        var formId = $(this).data('form');
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