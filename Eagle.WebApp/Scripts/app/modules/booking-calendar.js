$(document).ready(function () {
    function poplulateCustomerSelect2() {
        var selectBox = $('#cbxCustomer');
        var requestUrl = selectBox.data('url');
        var hiddenSelectedId = 'CustomerId';
        var hiddenSelectedText = 'CustomerName';
        var requestJsonArrayParams = {}

        selectBox.select2({
            width: "100%",
            theme: "classic",
            minimumInputLength: 0,
            allowClear: true,
            closeOnSelect: true,
            multiple: false,
            disabled: true, //Disable select 2
            ajax: {
                url: requestUrl,
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    var query = {
                        'search': params.term || '',
                        'page': params.page || 1
                    }
                    return $.extend({}, query, requestJsonArrayParams);
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    return {
                        results: data.Results,
                        pagination: {
                            more: data.MorePage
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            templateResult: function (item) {
                if (item.placeholder) return item.placeholder;
                return item.text;
            },
            templateSelection: function (item) {
                if (item.placeholder) return item.placeholder;
                return item.text;
            },
            matcher: function (term, text) {
                return text.toUpperCase().indexOf(term.toUpperCase()) !== -1;
            }
        });


        selectBox.on("select2:select", function (e) {
            var selected = e.params.data;
            if (selected !== undefined && selected !== null && selected !== '') {
                $("[name=" + hiddenSelectedId + "]").val(selected.id);
                $("[name=" + hiddenSelectedText + "]").val(selected.text);
            }
        }).on("select2:unselecting", function (e) {
            $(this).select2('val', '');
            $(this).data('state', 'unselected');
            $("[name=" + hiddenSelectedId + "]").val('');
            $("[name=" + hiddenSelectedText + "]").val('');
        });

        //bindInitialValueToSelect2
        var selectedValue = selectBox.data('id');
        var selectedText = selectBox.data('text');

        if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined
            && selectedText !== null && selectedText !== '' && selectedText !== undefined) {
            var $option = new Option(selectedText, selectedValue, true, true);
            selectBox.append($option);
            selectBox.trigger('change');
        }
    }

    //poplulateCustomerSelect2();

    function poplulateEmployeeSelectList() {
        var select = $('#EmployeeId');
        var url = select.data('url');
        var selectedValue = select.data('id');
        var productId = $('#ProductId').val();
        var params = { "productId": productId, "selectedValue": selectedValue };

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
                            selected: itemData.Value === selectedValue
                        }));
                    });
                }

                if (selectedValue !== null && selectedValue !== '' && selectedValue !== undefined) {
                    select.find('option').removeAttr('selected').filter('[value=' + selectedValue + ']').attr('selected', true);
                }

            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });
    }


    $(document).on("change", "#ProductId", function () {
        poplulateEmployeeSelectList();
    });


    function showPopUp(content) {
        var title = "Editing....";
        var dialog = bootbox.dialog({
            className: "my-modal",
            title: title,
            message: content,
            backdrop: true,
            closeButton: true,
            onEscape: function () {
                bootbox.hideAll();
                return false;
            }
        }).find("div.modal-dialog").css({ "width": "80%" });

        dialog.css({
            'top': '5%',
            'margin-top': function () {
                return -($(this).height() / 2);
            }
        });

        return false;
    }

    function getEventDetail(id) {
        var url = window.EditServiceBookingUrl;
        var params = { "id": id };
        $.ajax({
            type: "GET",
            dataType: "html",
            url: url,
            data: params,
            success: function (data, statusCode, xhr) {
                showPopUp(data);
                invokeDateTimePicker('dd/MM/yyyy');
                setupNumber();
                handleRadios();
                poplulateCustomerSelect2();
                poplulateEmployeeSelectList();
            }, error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    }

    function createCalendar(events) {
        //var initialLangCode = 'vi'; //'en'
        var initialLangCode = 'en';
        var listDayText = 'List in a day';
        var listWeekText = 'List in a week';
        var listMonthText = 'List in a month';

        if (initialLangCode === 'vi') {
            listDayText = 'Danh mục trong ngày';
            listWeekText = 'Danh mục trong tuần';
            listMonthText = 'Danh mục trong tháng';
        }

        $('#calendar').fullCalendar({
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,agendaWeek,agendaDay,listDay,listWeek,listMonth'
            },
            views: {
                listDay: { buttonText: listDayText },
                listWeek: { buttonText: listWeekText },
                listMonth: { buttonText: listMonthText }
            },
            defaultView: 'listWeek',
            defaultDate: new Date(), //$('#calendar').fullCalendar('today'), 
            locale: initialLangCode,
            //buttonIcons: false, // show the prev/next text
            weekNumbers: true,
            weekNumbersWithinDays: true,
            weekNumberCalculation: 'ISO',
            nowIndicator: true,
            editable: true,
            allDaySlot: true,
            selectable: true,
            slotMinutes: 30,
            navLinks: true, // can click day/week names to navigate views
            businessHours: true, // display business hours
            eventLimit: true, // allow "more" link when too many events
            events: events,
            eventClick: function (calEvent, jsEvent, view) {
                //alert('You clicked on event id: ' + calEvent.id
                //    + "\nSpecial ID: " + calEvent.someKey
                //    + "\nAnd the title is: " + calEvent.title);
                var id = calEvent.id;
                getEventDetail(id);
                return false;
            },
            eventDoubleClick: function (calEvent, jsEvent, view) {
                // change the border color just for fun
                $(this).css('border-color', 'red');
            },
            eventRender: function (event, el) {
                // render the timezone offset below the event title
                if (event.start.hasZone()) {
                    el.find('.fc-title').after(
						$('<div class="tzo"/>').text(event.start.format('Z'))
					);
                }
            },
            dayClick: function (date, allDay, jsEvent, view) {
                $(".fc-state-highlight").removeClass("fc-state-highlight");
                $(jsEvent.currentTarget).addClass("fc-state-highlight");
                console.log('dayClick', date.format());
            },
            //loading: function (bool) {
            //    if (bool) $('#loading').show();
            //    else $('#loading').hide();
            //}
            //select: function (start, end) {
            //    var title = prompt('Event Title:');
            //    var eventData;
            //    if (title) {
            //        eventData = {
            //            title: title,
            //            start: start,
            //            end: end
            //        };
            //        $('#calendar').fullCalendar('renderEvent', eventData, true); // stick? = true
            //    }
            //    $('#calendar').fullCalendar('unselect');
            //},
            //viewRender: function (view, element) {
            //    if (!CalLoading) {
            //        if (view.name === 'month') {
            //            $('#calendar').fullCalendar('removeEventSource', sourceFullView);
            //            $('#calendar').fullCalendar('removeEvents');
            //            $('#calendar').fullCalendar('addEventSource', sourceSummaryView);
            //        }
            //        else {
            //            $('#calendar').fullCalendar('removeEventSource', sourceSummaryView);
            //            $('#calendar').fullCalendar('removeEvents');
            //            $('#calendar').fullCalendar('addEventSource', sourceFullView);
            //        }
            //    }
            //}
        });

        //$('#calendar').fullCalendar('option', 'contentHeight', 650);
    }

    function checkValidDate(fromDate, toDate) {
        if (fromDate !== '' && toDate !== '') {
            var startDate = Date.parse(fromDate);
            var endDate = Date.parse(toDate);
            if (startDate > endDate) {
                bootbox.alert('Start date cannot be greater than end date');
                return false;
            } else {
                return true;
            }
        } else {
            return true;
        }
    }

    function populateCalendar() {
        var isValid = checkValidDate($("#FromDate").val(), $("#ToDate").val());
        if (isValid) {
            var url = window.GetCalendarEventsUrl;
            var params = $("#frmSearch").serialize();
            
            $.ajax({
                type: "GET",
                url: url,
                data: params,
                success: function (data) {
                    $('#calendar').fullCalendar('removeEvents');
                    $('#calendar').fullCalendar('addEventSource', data);
                    $('#calendar').fullCalendar('rerenderEvents');
                    createCalendar(data);
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    handleAjaxErrors(jqXhr, textStatus, errorThrown);
                }
            });
        }
        return false;
    }

    //populateCalendar();

    function edit(url, formId) {
        var formData = $("#" + formId).serialize();
        $.ajax({
            type: "PUT",
            url: url,
            data: formData,
            dataType: "json",
            async: false,
            success: function (response, textStatus, jqXhr) {
                if (response.Status === 0) {
                    bootbox.hideAll();
                    populateCalendar();
                } else {
                    if (response.Errors !== null) {
                        var result = '';
                        $.each(response.Errors, function (i, obj) {
                            result += obj.ErrorMessage + '<br/>';
                        });
                        bootbox.alert(result);
                    }
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                handleAjaxErrors(jqXhr, textStatus, errorThrown);
            }
        });

        return false;
    }

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

    //search
    $(document).on("click", ".search", function () {
        populateCalendar();
    });
    populateCalendar();
});