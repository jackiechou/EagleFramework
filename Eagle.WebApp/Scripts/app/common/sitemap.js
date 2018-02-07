/*--create site map--*/
$(document).ready(function () {
    $('#navigation *').show();
    $('.breadcrumbs').find('sitemap').remove();
    $('[isview]:first').addClass('sitemap site-finish');
    $('.breadcrumbs').append($('[isview]:first').clone());
    $('[isview]:first').parents('ul.ulparent').each(function () {
        var sitemap = $(this).prev('a').find('.pagename').parent();
        if (sitemap.length) {
            sitemap.addClass('sitemap');
            $('.breadcrumbs').find('.sitemap:first').before(sitemap.clone());
            $('.breadcrumbs').find('[isview]').removeAttr('isview');
        }
    });
    $('.breadcrumbs').find('.ui-icon').remove();
});