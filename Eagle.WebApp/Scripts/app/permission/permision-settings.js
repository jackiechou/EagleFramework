function setupPermissionForMasterData(MasterDataID) {
    var checkUrl = '/Admin/FunctionPermission/setUpPermisionsForMasterData';
    var data = { "MasterDataID": MasterDataID };
    $.ajax({
        type: "GET",
        url: checkUrl,
        data:data,
        success: function (data, statusCode, xhr) {
            if (data.message == "timeout")
                window.location.reload();
            else {
                setupMasterDataPermissionByClass(data);
            }
        }, error: function (jqXHR, textStatus, errorThrown) {
            handleAjaxErrors(jqXHR, textStatus, errorThrown);
        }
    });
}

function setupMasterDataPermissionByClass(data) {
    var entity = JSON.parse(data);  
    if (entity.FView == 0 || entity.FView == null) {
        //var base_url = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');
        //window.location.href = base_url + "/Admin/login";
        window.history.back();
    }

    if (entity.FEdit == 0 || entity.FView == null) {
        $('.add_master').prop('disabled', true);
        $('.edit_master').prop('disabled', true);
        $('.cancel_master').prop('disabled', true);
        $('.delete_master').prop('disabled', true);
        $('.editItem_master').prop('disabled', true);
        $('.deleteItem_master').prop('disabled', true);
        $('.resetItem_master').prop('disabled', true);
    }

    if (entity.FDelete == 0 || entity.FView == null) {
        $('.deleteItem_master').remove();
        $('.deleteItemDisable_master').remove();
    }

    if (entity.FExport == 0 || entity.FView == null) {
        $('.export_master').remove();
        $('.exportDisable_master').remove();
    }
}


function setupPermissionForFunction(FunctionID) {
    var checkUrl = '/Admin/FunctionPermission/setupPermissionForFunction';
    $.ajax({
        type: "GET",
        url: checkUrl,
        data: data,
        success: function (data, statusCode, xhr) {
            if (data.message == "timeout")
                window.location.reload();
            else {
               setupFunctionPermissionByClass(data);
            }
        }, error: function (jqXHR, textStatus, errorThrown) {
            handleAjaxErrors(jqXHR, textStatus, errorThrown);
        }
    });
}


function setupFunctionPermissionByClass(data) {
    var entity = JSON.parse(data);

    if (entity.FView == 0) {
        var base_url = location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '');
        window.location.href = base_url + "/Admin/login";
    }

    if (entity.FEdit == 0) {
        $('.add').prop('disabled', true);
        $('.edit').prop('disabled', true);
        $('.cancel').prop('disabled', true);
        $('.delete').prop('disabled', true);
        $('.editItem').prop('disabled', true);
        $('.deleteItem').prop('disabled', true);
        $('.resetItem').prop('disabled', true);
    }

    if (entity.FDelete == 0) {
        $('.deleteItem').remove();
        $('.deleteItemDisable').remove();
    }

    if (entity.FExport == 0) {
        $('.export').remove();
        $('.exportDisable').remove();
    }
}