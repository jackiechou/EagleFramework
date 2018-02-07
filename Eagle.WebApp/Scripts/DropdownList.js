function BuildDataToDropdownList(data, Textname, Textvalue) {
    var source = []; var items = []; // build hierarchical source.

    for (i = 0; i < data.length; i++) {
        var item = data[i];
        var id = item[Textvalue];
        var name = item[Textname];

        items[i] = { id: id, name: name, item: item };
        ource[i] = items[i];
    } return source;
}

function LoadSerializeDDLToJson(service_url, select_obj, name, value) {
    $(select_obj).empty();
    $.ajax({
        type: "POST",
        url: service_url,
        beforeSend: function (xhr) { xhr.setRequestHeader("Content-type", "application/json; charset=utf-8"); },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var list = $.parseJSON(data.d);

            var data = [];
            $.each(list, function (i) {
                data[i] = list[i];
            });
            var source = BuildDataToDropdownList(data, name, value);

            $.each(source, function () {
                if (this.name) {
                    var li = $("<option value='" + this.id + "'>" + this.name + "</option>");
                    li.appendTo(select_obj);
                }
            });

        }, error: function (xhr, ajaxOptions, thrownError) {           
            console.log(xhr.responseText); return false;
        }

    });
}