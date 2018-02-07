function CheckEdit() {
    if (FEdit == 0) {
        $('#btnAdd').prop('disabled', true);
        $('#btnSave').prop('disabled', true);
        $('#btnEdit').prop('disabled', true);
        $('#btnCancel').prop('disabled', true);
        $('#btnDelete').prop('disabled', true);
        $('#btnAddAll').prop('disabled', true);
        $('#btnFillCash').prop('disabled', true);
        $('#btnFillTOIL').prop('disabled', true);
        $('#btnSendForApproval').prop('disabled', true);
        $('#btnReset').prop('disabled', true);
        $('#btnCollectionData').prop('disabled', true);

        $('.add').prop('disabled', true);
        $('.edit').prop('disabled', true);
        $('.cancel').prop('disabled', true);
        $('.delete').prop('disabled', true);
        $('.addItem').prop('disabled', true);
        $('.editItem').prop('disabled', true);
        $('.deleteItem').prop('disabled', true);
        $('.resetItem').prop('disabled', true);
        $('.closeForm').prop('disabled', true);        
    }
}
function CheckDelete() {
    if (FDelete == 0) {
        $('.deleteItem').remove();
        $('.deleteItemDisable').remove();
        $('.delete').remove();
    }
}
function CheckExport() {
    if (FExport == 0) {
        $('#btnExport').remove();
        $('#btnExportDisable').remove();
        $('.export').remove();
    }
}


