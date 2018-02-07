﻿(function ($) {
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
                       // if ($(this).isVisible()) {
                            $(this).attr('selected', true);
                        //}
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
                       // if ($(this).isVisible()) {
                            $(this).remove().appendTo(unselected);
                       // }
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

    function resetControls(form) {
        var validateObj = $('#' + form);
        validateObj.find('input:text, input:password, input:file, select, textarea').not('.ignored').val('');
        validateObj.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
        validateObj.find('input[type="number"]').val(0);
        validateObj.find('select').find('option:first').attr('selected', true).siblings().attr('selected', false);
        validateObj.find("[data-valmsg-summary=true]").removeClass("validation-summary-errors").addClass("validation-summary-valid").find("ul").empty();
        validateObj.find("[data-valmsg-replace]").removeClass("field-validation-error").addClass("field-validation-valid").empty();
    }

    function search() {
        var formUrl = window.LanguageSearchUrl;
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

    //search
    $(document).on("click", ".search", function () {
        search();
    });

    function edit(url, form) {
        $('select.selected').find('option').each(function () {
            $(this).attr('selected', true);
        });

        var formData = $("#" + form).serialize();

        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            //dataType: "json",
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    resetControls(form);
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
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }

    function updateStatus(url, code, status) {
        var params = { "languageCode": code, "status": status };

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

    function updateSelectedItem(url, code) {
        var params = { "languageCode": code };

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

    $(document).on("click", ".edit", function (e) {
        e.preventDefault();

        var form = $(this).data('form');
        var url = $(this).data('url');

        if (!$("#" + form).valid()) { // Not Valid
            return false;
        } else {
            edit(url, form);
            return false;
        }
    });

    $(document).on("click", ".changeSelectedItem", function (e) {
        e.preventDefault();

        var code = $(this).data('code');
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
                    updateSelectedItem(url, code);
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

        var code = $(this).data('code');
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
                    updateStatus(url, code, status);
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
        var form = $(this).data('form');
        resetControls(form);
        return false;
    });

})(jQuery);