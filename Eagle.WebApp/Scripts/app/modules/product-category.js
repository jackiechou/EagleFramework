(function ($) {
    function pickIcon() {
        var icon = 'glyphicon-picture';
        if ($("#Icon").val() !== null && $("#Icon").val() !== '') {
            icon = $("#Icon").val();
        }

        $('.iconpicker').iconpicker({
            align: 'center',
            arrowClass: 'btn-primary',
            arrowPrevIconClass: 'glyphicon glyphicon-chevron-left',
            arrowNextIconClass: 'glyphicon glyphicon-chevron-right',
            iconset: 'glyphicon|fontawesome',
            icon: icon,
            cols: 10,
            rows: 5,
            footer: true,
            header: true,
            search: true,
            searchText: 'Search',
            selectedClass: 'btn-success',
            unselectedClass: '',
            labelHeader: '{0} of {1} pages',
            labelFooter: '{0} - {1} of {2} icons',
            placement: 'top'
        }).on('change', function (e) {
            $("#Icon").val(e.icon);
        });
    }

    pickIcon();

    //TREE Start ======================================================
    function createTree(data) {
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
            //onLoadSuccess: function () {
            //    if (selectedValue !== null && selectedValue !== '') {
            //        var node = $tree.tree('find', selectedValue);
            //        if (node) {
            //            $tree.tree('select', node.target);
            //        }
            //    }
            //},
            onClick: function (node) {
                var selectedId = node.id;
                if (selectedId > 0) {
                    getDetails(selectedId);
                }
            }
        });
    }

    function populateTree() {
        $.ajax({
            type: "GET",
            url: window.GetProductCategorySelectTreeUrl,
            data: { 'selectedId': 0, 'isRootShowed': true },
            success: function (data) {
                createTree(data);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    populateTree();

    //TREE End =====================================================

    function populateParentComboTree() {
        var select = $("#ParentId");
        var url = select.data('url');
        var selectedValue = select.val();

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

    populateParentComboTree();
    
    function getDetails(id) {
        var params = { id: id };
        $.ajax({
            type: "GET",
            url: window.EditProductCategoryUrl,
            data: params,
            success: function (data) {
                $('#divEdit').html(data);
                handleCheckBoxEvent();
                handleRadios();
                populateParentComboTree();
                populateTree();
                pickIcon();
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }




    //function updateListOrders() {
    //    var positions = new Array();
    //    for (var i = 0; i < $('.Ids').length; i++) {
    //        positions[i] = $('.Ids')[i].id;
    //    }
    //    $.ajax({
    //        type: "PUT",
    //        url: window.UpdateGalleryTopicListOrderUrl,
    //        data: { 'listOrderEntry': JSON.stringify(positions) },
    //        success: function (data) {
    //            var result = JSON.parse(data);
    //            if (result.flag === 'true') {
    //                reloadData(data);
    //                showMessageWithTitle(window.UpdateSuccessResource, result.message, "success", 20000);
    //            } else {
    //                showMessageWithTitle(window.UpdateFailure, result.message, "error", 20000);
    //            }
    //        },
    //        error: function (jqXhr, textStatus, errorThrown) {
    //            handleAjaxErrors(jqXhr, textStatus, errorThrown);
    //        }
    //    });
    //    return false;
    //}

    function resetControls() {
        var validateObj = $('#myform');
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

    function addData() {
        var formData = $("#myform").serialize();
        $.ajax({
            type: "POST",
            url: window.CreateProductCategoryUrl,
            data: formData,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    populateTree();
                    resetControls();
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
        return false;
    }

    function editData() {
        var formData = $("#myform").serialize();
        $.ajax({
            type: "PUT",
            url: window.EditProductCategoryUrl,
            data: formData,
            dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    getDetails(response.Data.Id);
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
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }

    function deleteData(id) {
        var params = { id: id };
        $.ajax({
            type: "DELETE",
            url: window.DeleteProductCategoryUrl,
            data: params,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    loadTree();
                    resetControls();
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
    
    $('#Description').keypress(function () {
        if (this.value.length >= 150) {
            return false;
        }
        return true;
    });

    //ADD
    $(document).on("click", ".add", function () {
        if (!$("#myform").valid()) { // Not Valid
            return false;
        } else {
            addData();
            return true;
        }
    });

    //EDIT
    $(document).on("click", ".edit", function () {
        if (!$("#myform").valid()) { // Not Valid
            return false;
        } else {
            editData();
            return true;
        }
    });

    //DELETE
    $(document).on("click", ".deleteItem", function (e) {
        e.preventDefault();

        var id = $(this).data('id');
        var box = bootbox.confirm({
            className: "my-modal",
            size: 'small',
            title: window.WarningWarning,
            message: window.DoYouWantToContinue,
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
                    deleteData(id);
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

    //GET - EDIT
    $(document).on("click", ".editItem", function () {
        var id = $(this).data('id');
        getDetails(id);
    });

    $(document).on("click", ".reset", function () {
        var id = $(this).data('id');
        resetControls(id);
    });

    $(document).on('show', '.accordion', function (e) {
        //$('.accordion-heading i').toggleClass(' ');
        $(e.target).prev('.accordion-heading').addClass('accordion-opened');
    });

    $(document).on('hide', '.accordion', function (e) {
        $(this).find('.accordion-heading').not($(e.target)).removeClass('accordion-opened');
        //$('.accordion-heading i').toggleClass('fa-chevron-right fa-chevron-down');
    });
})(jQuery);