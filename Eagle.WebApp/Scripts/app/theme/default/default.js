// JavaScript Document
function openWin(url,width,height)
{
	window.open(url,'Popup','width=' + width + ',height=' + height + ',left=100,top=100, resizable=no,menubar=no,statusbar=no,toolbar=no,scrollbars=yes'); 
}

function openWindow(url,w,h,name) {
    rozion_pop_window = window.open(url, name, 'noscrollbars,width=' + w + ',height=' + h);
}

function openImage(name) {
    window.open('/image.html?img=' + name, 'Image_View', 'noscrollbars,width=400,height=400');
}

function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!==0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}



$(document).on({
    mouseenter: function() {
        $(this).find('a').find('.product-bg').removeClass('product-bgo');	
        $(this).find('.product-item-detail').css('display', 'block');	
		$(this).find('.product-item-detail').animate({bottom:"0px"}, 200);
    },
    mouseleave: function() {
        $(this).find('a').find('.product-bg').addClass('product-bgo');	
		$(this).find('.product-item-detail').css('display', 'none');	
		$(this).find('.product-item-detail').animate({bottom:"-83px"}, 100);
    }
}, '.product-item');
$(document).on({
    mouseenter: function() {
        $(this).find('a').find('.product-bgr').removeClass('product-bgo');	
    },
    mouseleave: function() {
        $(this).find('a').find('.product-bgr').addClass('product-bgo');	
    }
}, '.product-itemr');
$(document).on({
    mouseenter: function() {
        $(this).find('a').find('.article-bg').removeClass('product-bgo');	
    },
    mouseleave: function() {
        $(this).find('a').find('.article-bg').addClass('product-bgo');	
    }
}, '.article-list-img');


$(document).on({
    mouseenter: function() {
        $(this).addClass('logo-hover');		
    },
    mouseleave: function() {
        $(this).removeClass('logo-hover');	
    }
}, '.logo-img');

$(document).ready(function(){
    $(window).scroll(function () {
        if ($(document).height() - ($(window).scrollTop() + $( window ).height()) > 150) {
            $('.nav-fix').fadeIn(200);
        }
        else {
            $('.nav-fix').fadeOut(200);
        }
    });
});

    function utf8_encode(string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }
        return utftext;
    }

    function urlescape(i) {
        var o = utf8_encode(i);
        o = escape(o);

        return o;
    }

    function contact() {
    
        $('.uinput').removeClass('uinput-er');
        $('#Content').removeClass('uinput-er');
        $('#AntibotCode').removeClass('uinput-er');
        var fields = $('#contact input');
        var Error = 0;
        var i = 0;
        fields.each(function () {
            var Type = $(this).attr("type");
            var Name = $(this).attr("name");
            var value = $(this).val();
            value = $.trim(value);
            if (Type == 'text') {
                if (value == '') {
                    Error = 1;
                    if (Name == 'AntibotCode') {
                        $(this).addClass('uinput-er');
                    }
                    else
                    {
                        $(this).parent('span').parent('.uinput').addClass('uinput-er');
                    }
                }
                if (Name == 'Email') {
                    if (value.indexOf('@') == '-1' || value.indexOf('.') == '-1') {
                        Error = 1;
                        $(this).parent('span').parent('.uinput').addClass('uinput-er');
                    }
                }           
            }
            i++;
        });

        var valuse = $('#Content').val();
        if (valuse.trim() == '') {
            Error = 1;
            $('#Content').addClass('uinput-er');
        }
        if (Error == 0) {
            $('#fm').submit();
        }
    }

    function keyEnter(evt) 
    {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode == "13") {
            var $this = $('#searchid');
            var value = $this.val();
            value = $.trim(value);
            if (value != '' & value != null) {
                $('#fsm').submit();
            }
            else
            {
                $this.css('border','1px solid #e2e2e2');
            }
            return false;
        }
    }

    function addComment(id, l)
    {
        $('.product-comment-alert').css('display', 'none');          
        $('.comment-area').removeClass('uinput-er');
        $('.comment-input').removeClass('uinput-er');
        $('.comment-input-contact').removeClass('uinput-er');
        var value = $('#product-comment').val();
        var valuename = $('#product-name').val();
        var valuecontact = $('#product-contact').val();
        value = value.replace("\n", "<br />");
        value = value.replace("\r", "<br />");
        if(valuename == '' || valuename == null)
        {
            $('.comment-input').addClass('uinput-er');
            return null;
        }
        else if(valuename.length < 2)
        {        
            $('.comment-input').addClass('uinput-er');
            return null;
        }
        if(valuecontact == '' || valuecontact == null)
        {
            $('.comment-input-contact').addClass('uinput-er');
            return null;
        }
        else if(valuecontact.length < 5)
        {        
            $('.comment-input-contact').addClass('uinput-er');
            return null;
        }
        if(value == '' || value == null)
        {
            $('.comment-area').addClass('uinput-er');
            return null;
        }
        else if(value.length < 5)
        {        
            $('.comment-area').addClass('uinput-er');
            return null;
        }
        
        var r = $('#score').val();
        if(r == '' || r == null)
            r = 0;
        $('#comment-over').css('display', 'block');
        $('#button-comment').removeAttr('href');
        var url = '/xmlhttp.aspx?FID=12&AddComment=1&Rating=' + r + "&Content=" + urlescape(value) + "&Name=" + urlescape(valuename) + "&Contact=" + urlescape(valuecontact) + "&ProductID=" + id;
        if(l != '' & l != null)
            url += "&Language=" + l;
        xmlhttp2.open('GET', url + '&rd=' + Math.random(), true);
        xmlhttp2.onreadystatechange=function() 
        {
            if (xmlhttp2.readyState==4)
            {
                $('.product-comment-alert').css('display', 'block');      
                $('#product-comment').val('');
                $('#score').val('1');
                $('#comment-over').css('display', 'none');
                $('#button-comment').attr('href','javascript:addComment(' + id + ');');	
            }        
            else
            {
                $('.product-comment-alert').css('display', 'block');
                $('#product-comment').val('1');
                $('#score').val('');
                $('#comment-over').css('display', 'none');
                $('#button-comment').attr('href','javascript:addComment(' + id + ');');
            }
        }
        xmlhttp2.send(null);
        return true;
    }

