$(document).ready(function () {
    //Pick Icon
    function pickIcon() {
        var icon = $('.selectedIcon').val();

        $('.iconpicker').iconpicker({
            iconset: 'glyphicon|fontawesome',
            icon: icon,
            rows: 5,
            cols: 10,
            placement: 'top'
        }).on('change', function (e) {
            $(".selectedIcon").val(e.icon);
        });
    }

    pickIcon();

    //Set up color picker
    function pickColor() {
        //Set up color picker
        var selectColor = $('.color').val();
        $('.color-picker').colorpicker({
            color: selectColor
        }).on("change.color", function (event, color) {
            $('.color').val(color);
        });
    }

    pickColor();
    
    //function getPositions() {
    //    var selectPositions = $('#SelectedPositions option');
    //    var positionIds = $.map(selectPositions, function (option) { return option.value; });
    //    $("#PositionId").val(positionIds);
    //}

    function populateDualListBoxListeners() {
        /** Simple delay function that can wrap around an existing function and provides a callback. */
        var delay = (function () {
            var timer = 0;
            return function (callback, ms) {
                clearTimeout(timer);
                timer = setTimeout(callback, ms);
            };
        })();

        /** Checks whether or not an element is visible. The default jQuery implementation doesn't work. */
        $.fn.isVisible = function () {
            return !($(this).css('visibility') === 'hidden' || $(this).css('display') === 'none');
        };

        // Sorts options in a select / list box.
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
   

    function loadPageList() {
        var typeId = $('#TypeId').val();
        var params = {
            "pageTypeId": typeId,
            "isShowSelectText": true
        };
        $.ajax({
            async: false,
            cache: false,
            type: 'GET',
            dataType: "json",
            url: window.PopulatePageSelectListUrl,
            data: params,
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            },
            success: function (data) {
                var select = $('#PageId');
                select.empty();

                if (data.length > 0) {
                    $.each(data, function (index, item) {
                        select.append($('<option/>', {
                            value: item.Value,
                            text: item.Text
                        }));
                    });
                }
            }
        });
        return false;
    }

    $("#PageId").select2();

    function loadMenuPositions() {
        var params = { "typeId": $('#TypeId').val() };
       
        $.ajax({
            type: 'GET',
            dataType: "json",
            url: window.PopulateMenuPositionMultiSelectListUrl,
            data: params,
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            },
            success: function (data) {
                var select = $('#AvailablePositions');
                select.empty();

                if (data.length > 0) {
                    $.each(data, function (index, item) {
                        select.append($('<option/>', {
                            value: item.Value,
                            text: item.Text
                        }));
                    });
                }
            }
        });
        return false;
    }

    function loadSelectedMenuPositions() {
        var params = { "menuId": $('#MenuId').val() };
        $.ajax({
            async: false,
            cache: false,
            type: 'GET',
            dataType: "json",
            url: window.PopulateMenuPositionMultiSelectedListUrl,
            data: params,
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            },
            success: function (data) {
                var select = $('#SelectedPositions');
                select.empty();

                if (data.length > 0) {

                    var availablePositions = new Array();
                    $('#AvailablePositions > option').each(
                    function (i) {
                        availablePositions[i] = $(this).val();
                    });
                    
                    $.each(data, function (index, item) {
                        //check selected option in available list
                        if (availablePositions.indexOf(item.Value) > -1) {
                            select.append($('<option/>', {
                                value: item.Value,
                                text: item.Text,
                                selected: item.Selected
                            }));
                        }
                    });
                }
            }
        });
        return false;
    }

    function populateParentMenus() {
        var select = $(".cbxParentMenuTree");
        var selectedValue = select.val();
        var typeId = $('#TypeId').val();
        var params = {
            "typeId": typeId,
            "isRootShowed": true
        };

        $.ajax({
            async: false,
            cache: false,
            type: 'GET',
            dataType: "json",
            url: window.GetHierachicalListUrl,
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
                    children: 'Children',
                    onLoadSuccess: function (row, data) {
                        $(this).tree("collapseAll");
                    },
                    onClick: function (node) {
                        selectedValue = node.id;
                        $(this).val(selectedValue);
                    }
                });
            }
        });
        return false;
    }

    populateParentMenus();
    
    $.fn.checkFile = function (options) {
        var defaults = {
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif'],
            allowedSize: 15, //15MB	
            success: function() {}
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
        $("input[type=file][name=FileUpload]").on('change', function () {
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
                    }

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
        $(document).on("click", ".resetImage", function () {
            $('#FileUpload').val('');

            var imageHolder = $("#image-holder");
            imageHolder.hide();

            var imageContainer = $("#image-container");
            imageContainer.show();
        });
    }

    function getDetails(id) {
        $.ajax({
            type: "GET",
            url: window.EditMenuUrl,
            data: { "id": id },
            success: function (data) {
                $('#divEdit').html(data);
                handleCheckBoxEvent();
                handleRadios();
                populateParentMenus();
                populateDualListBoxListeners();
                pickColor();
                pickIcon();
                previewImage();
                resetImage();
                
                $("#PageId").select2();

                //Update selected text from MenuTypeId
                var selectedText = $('input[type=radio][name="MenuTypeId"]:checked').parent('label').text();
                $("#TypeName").text(selectedText);
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    }


    $(document).on("click", ".create-page", function () {
        window.location.href = $(this).data('action');
        $('.loading').show();
        $('.loading').fadeIn(3000).delay(2000).fadeOut("slow");
        return false;
    });

    function updateListOrder(id, parentId, listOrder) {
        var data = JSON.stringify({ "MenuId": id, "ParentId": parentId, listOrder: listOrder });
        $.ajax({
            type: "PUT",
            url: window.UpdateListOrderUrl,
            data: data,
            success: function (data) {
                var result = JSON.parse(data);
                if (result.flag === 'true') {
                    showMessageWithTitle(window.UpdateSuccess, result.message, "success", 20000);
                    getDetails(result.id);
                } else {
                    showMessageWithTitle(window.UpdateFailure, result.message, "error");
                    hideMessageWithTitle(20000);
                    $('html, body').animate({ scrollTop: 80 }, 'slow');
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function loadTreeMenu() {
        var menuSetting = {
            view: {
                dblClickExpand: false,
                showLine: true,
                showIcon: true,
                showTitle: true,
                selectedMulti: false
            },
            edit: {
                enable: true,
                editNameSelectAll: false,
                showRemoveBtn: false,
                showRenameBtn: false
            },
            data: {
                keep: {
                    leaf: true,
                    parent: true
                },
                key: {
                    name: "Name",
                    title: "Title",
                    open: "IsParent",
                    children: "Children",
                    check: "checked"
                    //url:"Url",
                },
                simpleData: {
                    enable: true,
                    idKey: "Id",
                    pIdKey: "ParentId",
                    rootPId: null
                }
            },
            callback: {
                onClick: function (event, treeId, treeNode, clickFlag) {
                    //alert("Id:" + treeNode.Id + " Name:" + treeNode.Name + " clickFlag:" + clickFlag);
                    getDetails(treeNode.Id);
                }
            }
        };

        //Get Menu Data by Type Id
        //console.log($('input:radio[name="MenuTypeId"]:checked').val());
        var params = { 'typeId': $('input:radio[name="MenuTypeId"]:checked').val() }
        $.ajax({
            async: false,
            cache: false,
            type: 'GET',
            dataType: "json",
            url: window.PopulateListBoxUrl,
            data: params,
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            },
            success: function (data) {
                var zTree = $("#tree");
                zTree = $.fn.zTree.init(zTree, menuSetting, data);
                zTree.expandAll(true);
            }
        });
    }
    loadTreeMenu();

    $(document).on('change', '#menu-type-select', function () {
        $(this).find('input:radio').attr('checked', 'checked');
        var selectedItem = $(this).find('input[type=radio]:checked');
        var selectedText = selectedItem.parent('label').text();
        var selectedValue = selectedItem.val();
        $(this).find('input:radio').filter("[value!='" + selectedValue + "']").removeAttr('checked');
        loadTreeMenu();

        //Modify for loading create or edit form
        //console.log(selectedText);
        $("#TypeId").val(selectedValue);
        $("#TypeName").text(selectedText);
        populateParentMenus();
        loadPageList();
        loadMenuPositions();
        loadSelectedMenuPositions();
        return false;
    });
    
    //$(document).on('change', '#type-select', function () {
    //    $(this).find('input:radio').attr('checked', 'checked');
    //    var result = $(this).find('input[type=radio]:checked').val();
    //    $(this).find('input:radio').filter("[value!='" + result + "']").removeAttr('checked');
    //    loadPageList();
    //    loadMenuPositions();
    //    loadSelectedMenuPositions();
    //    return false;
    //});

    function resetControls(form, mode) {
        if (mode === 'edit') {
            var id = $('.edit').data('id');
            getDetails(id);
        } else {
            form.find('.selectBox2').select2('val', '');
            form.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
            form.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
            form.find('input[type="number"]').val(0);
            form.find('input[type=file]').val('');
            form.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
            form.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
            form.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
        }
    }

    function reloadData(response, form, mode) {
        if (response.Status === 0) {
            loadTreeMenu();
            resetControls(form, mode);
            updateProgress(100);
            showMessageWithTitle(response.Status, response.Data.Message, "success", 20000);
        } else {
            if (response.Errors !== null) {
                var result = '';
                $.each(response.Errors, function (i, obj) {
                    result += obj.ErrorMessage + '<br/>';
                });
                showMessageWithTitle(response.Status, result, "error", 50000);
            }
        }
    }

    
    //POST - ADD
    $(document).on("click", ".create", function (e) {
        e.preventDefault();

        var mode = $(this).data("mode");
        var url = $(this).data("url");
        var formId = $(this).data("form");
        var form = $("#" + formId);

        var formData = new FormData();

        //add the file to the FormData collection
        var files = $("#FileUpload")[0].files;
        if (files.length > 0) {
            formData.append("FileUpload", files[0]);
        }

        var serializedFormData = form.serializeArray();
        for (var i = 0; i < serializedFormData.length; i++) {
            formData.append(serializedFormData[i].name, serializedFormData[i].value);
        }

        if (!form.valid()) {
            return false;
        } else {
            $(".create").val(window.Processing);
            $(".create").attr("disabled", true);

            updateProgress(30);

            $.ajax({
                type: 'POST',
                url: url,
                data: formData,
                dataType: "json",
                processData: false,
                contentType: false,
                success: function (data) {
                    $(".create").val(window.Save);
                    $(".create").prop("disabled", false);
                    reloadData(data, form, mode);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                    $(".create").val(window.Save);
                    $(".create").prop("disabled", false);
                }
            });
            return false;
        }
    });

    //POST - EDIT
    $(document).on("click", ".edit", function (e) {
        e.preventDefault();

        var mode = $(this).data("mode");
        var url = $(this).data("url");
        var formId = $(this).data("form");
        var form = $("#" + formId);

        var formData = new FormData();
        var files = $("#FileUpload")[0].files;

        //add the file to the FormData collection
        if (files.length > 0) {
            formData.append("FileUpload", files[0]);
        }
        //else {
        //    showNotification('warning', 'Please select file to upload.');
        //    return false;
        //}
        //var extension = $("#file").val().split('.').pop().toUpperCase();
        //if (extension !== "PNG" && extension !== "JPG" && extension !== "GIF" && extension !== "JPEG") {
        //    showNotification('warning', 'Imvalid image file format.');
        //    return false;
        //}

        var serializedFormData = form.serializeArray();
        for (var i = 0; i < serializedFormData.length; i++) {
            formData.append(serializedFormData[i].name, serializedFormData[i].value);
        }

        if (!form.valid()) {
            return false;
        } else {
            $(".edit").val(window.Processing);
            $(".edit").attr("disabled", true);
            updateProgress(30);

            $.ajax({
                type: 'POST',
                url: url,
                data: formData,
                dataType: "json",
                processData: false,
                contentType: false,
                success: function (data) {
                    $(".edit").val(window.Save);
                    $(".edit").prop("disabled", false);
                    reloadData(data, form, mode);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                    $(".edit").val(window.Save);
                    $(".edit").prop("disabled", false);
                }
            });
            return false;
        }
    });
    
    $(document).on("change", ".changePermissionStatus", function (e) {
        e.preventDefault();

        var roleId = $(this).data('roleid');
        var menuId = $(this).data('menuid');
        var permissionId = $(this).data('permissionid');
        var formUrl = $(this).data('url');
        var status = $(this).is(":checked");
        var formData = { "roleId": roleId, "menuId": menuId, "permissionId": permissionId, "status": status };

        $.ajax({
            type: 'POST',
            url: formUrl,
            data: formData,
            success: function (data) {
                var result = JSON.parse(data);
                if (result.flag === 'true') {
                    showMessageWithTitle(window.UpdateStatus, result.message, "success", 10000);
                } else {
                    showMessageWithTitle(window.UpdateStatus, result.message, "error", 10000);
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
        return false;
    });
});