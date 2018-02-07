function BindAutoCompleteSelect2(requestUrl, selectBoxId, placeholder, hiddenID, hiddenName)
{
    var pageSize = 20;
    var selectBox = $('#' + selectBoxId);
    var hiddenIDControl = $("input[name=" + hiddenID + "]"); 

    if (hiddenIDControl.val() == '0')
        hiddenIDControl.val('');

    selectBox.select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        allowClear: true,
        multiple: false,
        ajax: {
            url: requestUrl,
            dataType: 'json',
            quietMillis: 100,  //How long the user has to pause their typing before sending the next request
            data: function (term, page) {
                var params = {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term
                };
                //params.push({ "year": "2010"});
                return params;
            },
            results: function (data, page) {
                //Used to determine whether or not there are more results available,
                //and if requests for more data should be sent in the infinite scrolling
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        },
        formatSelection: function (result) {
            if (result.id != undefined && result.id != null)
                CloseToolTip(hiddenID);
            $("input[name=" + hiddenID + "]").val(result.id);
            $("input[name=" + hiddenName + "]").val(result.name);

            return result.name;
        },
        formatResult: function (result) {
            return result.name;
        },
        initSelection: function (element, callback) {
            var selected_id = $("input[name=" + hiddenID + "]").val();
            var selected_name = $("input[name=" + hiddenName + "]").val();
            var data = { id: selected_id, name: selected_name };
            callback(data);
        }
    });
    var selectedValue = ($("input[name=" + hiddenID + "]").val() == 0) ? '' : $("input[name=" + hiddenID + "]").val();
    selectBox.select2('val', selectedValue);
    // selectBox.on('select2-removed', function (e) { $("input[name=" + hiddenID + "]").val(''); });
    selectBox.on("change", function (e) {
        if (e.added) {
           // alert('added: ' + e.added.text + ' id ' + e.added.id);
            $("input[name=" + hiddenID + "]").val(e.added.id);
        } else if (e.removed) {
            $("input[name=" + hiddenID + "]").val('');
        }
    });
}

function BindAutoCompleteSelect2WithJsonParams(requestUrl, requestParams, selectBoxId, placeholder, hiddenID, hiddenName) {
    var hiddenIDControl = $("input[name=" + hiddenID + "]");

    if (hiddenIDControl.val() == '0')
        hiddenIDControl.val('');

    var pageSize = 20;
    var selectBox = $('#' + selectBoxId);
    selectBox.select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        allowClear: true,
        multiple: false,
        ajax: {
            url: requestUrl,
            dataType: 'json',
            quietMillis: 100,  //How long the user has to pause their typing before sending the next request
            data: function (term, page) {
                var params = {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term
                };
                var objParams = $.extend({}, params, requestParams);
                //alert(JSON.stringify(objParams));
                return objParams;
            },
            results: function (data, page) {
                //Used to determine whether or not there are more results available,
                //and if requests for more data should be sent in the infinite scrolling
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        },
        formatSelection: function (result) {
            if (result.id != undefined && result.id != null)
                CloseToolTip(hiddenID);           
            $("input[name=" + hiddenID + "]").val(result.id);
            $("input[name=" + hiddenName + "]").val(result.name);
            return result.name;
        },
        formatResult: function (result) {
            return result.name;
        },
        initSelection: function (element, callback) {
            var selected_id = $("input[name=" + hiddenID + "]").val();
            var selected_name = $("input[name=" + hiddenName + "]").val();
            var data = { id: selected_id, name: selected_name };
            callback(data);
        }
    });

    selectBox.select2('val', $("input[name=" + hiddenID + "]").val());
    selectBox.on("change", function (e) {
        if (e.added) {
            // alert('added: ' + e.added.text + ' id ' + e.added.id);
            $("input[name=" + hiddenID + "]").val(e.added.id);
        } else if (e.removed) {
            $("input[name=" + hiddenID + "]").val('');
        }
    });
   // selectBox.on('select2-removed', function (e) { $("input[name=" + hiddenID + "]").val(''); });
}

//Multiple Choice
function BindAutoCompleteMultipleChoicesToSelect2(requestUrl, selectBoxId, placeholder, hiddenID, isMultiple) {
   
    var pageSize = 20;
    var selectBox = $('#' + selectBoxId);
    var hiddenIDControl = $("input[name=" + hiddenID + "]");
    hiddenIDControl.val('');
   
    if (isMultiple == undefined || isMultiple == null)
        isMultiple = false;

    selectBox.select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        allowClear: true,
        multiple: isMultiple,
        ajax: {
            url: requestUrl,
            dataType: 'json',
            quietMillis: 100,  //How long the user has to pause their typing before sending the next request
            data: function (term, page) {
                var params = {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term
                };                
                return params;
            },
            results: function (data, page) {
                //Used to determine whether or not there are more results available,
                //and if requests for more data should be sent in the infinite scrolling
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        },
        formatSelection: function (result) {
            hiddenIDControl.val(result.id);
            return result.text;
        },
        formatResult: function (result) {
            return result.text;
        },
        initSelection: function (element, callback) {
            var data = [];
            if (isMultiple == true) {
                //data = [{ id: 1, text: 'bug' }, { id: 2, text: 'duplicate' }];
                data = eval($(element).attr('data-init-list'));
            } else {
                //var selected_id = hiddenIDControl.val();
                // data = { id: selected_id, text: element.text };
                var elementText = $(element).attr('data-init-text');
                var elementValue = $(element).attr('data-init-value');
                data = { id: elementValue, text: elementText };
            }
            callback(data);
        }
    });
    selectBox.select2('val', hiddenIDControl.val());  
    selectBox.on('select2-removed', function (e) { hiddenIDControl.val(''); });


    $(document).on("change", '#' + selectBoxId, function (ev) {
        var choice;
        var values = ev.val;
        // This is assuming the value will be an array of strings.
        // Convert to a comma-delimited string to set the value.
        if (values !== null && values.length > 0) {
            for (var i = 0; i < values.length; i++) {
                if (typeof choice !== 'undefined') {
                    choice += ",";
                    choice += values[i];
                }
                else {
                    choice = values[i];
                }
            }
        }       
        // Set the value so that MVC will load the form values in the postback.
        hiddenIDControl.val(choice);
       
    });
}

function BindMultipleChoicesToSelect2(requestUrl, selectBoxId, placeholder, hiddenID, hiddenName, InitialDataJson) {
    var pageSize = 20;
    var selectBox = $('#' + selectBoxId);
    var hiddenIDControl = $("input[name=" + hiddenID + "]");
    var hiddenNameControl = $("input[name=" + hiddenName + "]");
    var data = JSON.stringify(InitialDataJson);
    selectBox.select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        allowClear: true,
        multiple: true,
        ajax: {
            url: requestUrl,
            dataType: 'json',
            quietMillis: 100,  //How long the user has to pause their typing before sending the next request
            data: function (term, page) {
                var params = {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term
                };
                var objParams = $.extend({}, params, requestParams);
                //alert(JSON.stringify(objParams));
                return objParams;
            },
            results: function (data, page) {
                //Used to determine whether or not there are more results available,
                //and if requests for more data should be sent in the infinite scrolling
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        },
        formatSelection: function (result) {
            $("input[name=" + hiddenID + "]").val(result.id);
            $("input[name=" + hiddenName + "]").val(result.name);

            return result.text;
        },
        formatResult: function (result) {
            return result.text;
        },
        initSelection: function (element, callback) {
            if (InitialDataJson == undefined || InitialDataJson == null) {
                var selected_id = hiddenIDControl.val();
                var selected_name = hiddenNameControl.val();
                data = { id: selected_id, text: selected_name };
            }            
            callback(data);
        }
    });

    if (InitialDataJson != undefined && InitialDataJson != null)
        selectBox.select2('val', data);
    else {
        selectBox.select2('val', hiddenIDControl.val());
    }

    selectBox.on('select2-removed', function (e) { hiddenIDControl.val(''); });


    $(document).on("change", '#' + selectBoxId, function (ev) {
        var choice;
        var values = ev.val;
        // This is assuming the value will be an array of strings.
        // Convert to a comma-delimited string to set the value.
        if (values !== null && values.length > 0) {
            for (var i = 0; i < values.length; i++) {
                if (typeof choice !== 'undefined') {
                    choice += ",";
                    choice += values[i];
                }
                else {
                    choice = values[i];
                }
            }
        }
        //alert(choice);
        // Set the value so that MVC will load the form values in the postback.
        hiddenIDControl.val(choice);

    });
}


function select2Dropdown(hiddenID, valueID, placeholder, listActionURL, getActionURL, isMultiple) {
    var selectId = '#' + hiddenID;
    $(selectId).select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        allowClear: true,
        multiple: isMultiple,
        ajax: {
            url: listActionURL,
            dataType: 'json',
            data: function (term, page) {
                return {
                    id: term // search term
                };
            },
            results: function (data) {
                return { results: data };
            }
        },
        initSelection: function (element, callback) {
            // the input tag has a value attribute preloaded that points to a preselected make's id
            // this function resolves that id attribute to an object that select2 can render
            // using its formatResult renderer - that way the make text is shown preselected
            var id = $('#' + valueID).val();
            if (id !== null && id.length > 0) {
                $.ajax(getActionURL + "/" + id, {
                    dataType: "json"
                }).done(function (data) { callback(data); });
            }
        },
        formatResult: function (result) {
            return result.text;
        },
        formatSelection: function (result) {
            return result.text;
        }
    });
 
    $(document.body).on("change", selectId, function (ev) {
        var choice;
        var values = ev.val;
        // This is assuming the value will be an array of strings.
        // Convert to a comma-delimited string to set the value.
        if (values !== null && values.length > 0) {
            for (var i = 0; i < values.length; i++) {
                if (typeof choice !== 'undefined') {
                    choice += ",";
                    choice += values[i];
                }
                else {
                    choice = values[i];
                }
            }
        }
 
        // Set the value so that MVC will load the form values in the postback.
        $('#' + valueID).val(choice);
    });
}

//Show and get with id ===================================================================================================
function BindAutoCompleteSelect2Single(requestUrl, requestParams, selectBoxId, placeholder, hiddenID) {   
    var pageSize = 20;
    var selectBox = $('#' + selectBoxId);
    var hiddenIDControl = $("input[name=" + hiddenID + "]");

    if (hiddenIDControl.val() == '0')
        hiddenIDControl.val('');

    selectBox.select2({
        placeholder: placeholder,
        minimumInputLength: 0,
        allowClear: true,
        multiple: false,
        ajax: {
            url: requestUrl,
            dataType: 'json',
            quietMillis: 100,  //How long the user has to pause their typing before sending the next request
            data: function (term, page) {              
                var params = {
                    pageSize: pageSize,
                    pageNum: page,
                    searchTerm: term
                };
                var objParams = $.extend({}, params, requestParams);
                //params.push({ "year": "2010"});
                //alert(JSON.stringify(objParams));
                return objParams;
            },
            results: function (data, page) {
                //Used to determine whether or not there are more results available,
                //and if requests for more data should be sent in the infinite scrolling
                var more = (page * pageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        },
        formatSelection: function (result) {
            if (result.id != undefined && result.id != null)
                CloseToolTip(hiddenID);            
            $("input[name=" + hiddenID + "]").val(result.id);
            return result.name;
        },
        formatResult: function (result) {
            return result.name;
        },
        initSelection: function (element, callback) {
            var selected_id = $("input[name=" + hiddenID + "]").val();
            var data = { id: selected_id, name: selected_id };
            callback(data);
        }
    });
    var selectedValue = ($("input[name=" + hiddenID + "]").val() == 0) ? '' : $("input[name=" + hiddenID + "]").val();
    selectBox.select2('val', selectedValue);
    selectBox.on("change", function (e) {
        if (e.added) {
            $("input[name=" + hiddenID + "]").val(e.added.id);
        } else if (e.removed) {
            $("input[name=" + hiddenID + "]").val('');
        }
    });
}