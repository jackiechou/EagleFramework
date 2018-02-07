(function ($) {
    //Facebook
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/en_GB/sdk.js#xfbml=1&version=v2.10&appId=1878014685858240";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
    
    var url = 'https://plus.google.com/share?url=' + window.location.href + '&title=' + $(document).find('title').text();
    $('#social_google_plus').attr('href', url);
    $('#fb-like').attr('data-href', window.location.href);
})(jQuery);