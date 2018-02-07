CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.language = 'en';
    // config.uiColor = '#AADC6E';

    // The toolbar groups arrangement, optimized for two toolbar rows.
    config.toolbar = [
            ['Source'],
            ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], ['Find', 'Replace', '-', 'SelectAll', 'Scayt'],
            ['-', 'NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
            ['-', 'Link', 'Unlink', 'Image'], ['Flash', 'HorizontalRule', 'SpecialChar', 'PageBreak', 'Smiley', 'CreateDiv', 'Table'],
            '/',
            ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'TextColor', 'BGColor', 'Styles', 'Format', 'Font', 'FontSize', 'RemoveFormat'],
            ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'], ['Blockquote', 'syntaxhighlight', 'bbcodeselector'], ['Maximize']
    ];
};

//$("a[class*='.create'],a[class*='edit'],a[class*='save'],a[class*='submit']").click(function () {
//    CKEDITOR.instances.updateElement();
//});

function getCkEditorInstance() {
    var myinstances = [];

    //this is the foreach loop
    for (var i in CKEDITOR.instances) {
        if (CKEDITOR.instances.hasOwnProperty(i)) {
            ///* this  returns each instance as object try it with alert(CKEDITOR.instances[i]) */
            //CKEDITOR.instances[i];

            ///* this returns the names of the textareas/id of the instances. */
            //CKEDITOR.instances[i].name;

            ///* returns the initial value of the textarea */
            //CKEDITOR.instances[i].value;

            /* this updates the value of the textarea from the CK instances.. */
            CKEDITOR.instances[i].updateElement();

            /* this retrieve the data of each instances and store it into an associative array with
            the names of the textareas as keys... */
            myinstances[CKEDITOR.instances[i].name] = CKEDITOR.instances[i].getData();
        }
    }
}

getCkEditorInstance();