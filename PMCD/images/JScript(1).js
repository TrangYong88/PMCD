// JScript File

function openPopup(vLink, vHeight, vWidth, vTarget)
{
	winDef = 'status=no,resizable=no,scrollbars=yes,toolbar=no,location=no,fullscreen=no,titlebar=yes,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
	winDef = winDef.concat('top=').concat((screen.height - vHeight)/2).concat(',');
	winDef = winDef.concat('left=').concat((screen.width - vWidth)/2);
	window.open(vLink, vTarget, winDef);
    
}

function openImagePopup(vLink, vHeight, vWidth)
{
	var sLink = (typeof(vLink.href) == 'undefined') ? vLink : vLink.href;

	if (sLink == '')
	{
		return false;
	}

	winDef = 'status=no,resizable=no,scrollbars=no,toolbar=no,location=no,fullscreen=no,titlebar=yes,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
	winDef = winDef.concat('top=').concat((screen.height - vHeight)/2).concat(',');
	winDef = winDef.concat('left=').concat((screen.width - vWidth)/2);
	newwin = open('', '_blank', winDef);

	newwin.document.writeln('<title>VietNamNet</title><body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0">');
	newwin.document.writeln('<a href="" onClick="window.close(); return false;"><img src="', sLink, '" alt="', 'Dong lai', '" border=0></a>');
	newwin.document.writeln('</body>');

	if (typeof(vLink.href) != 'undefined')
	{
		return false;
	}
}


    function string2Array(str)
    {
        var tmp = new Array();
        
        if (str.indexOf('|') != -1)
        {
            var ar = str.split("|");
            
            var i=0;
            while (i < ar.length)
             {
              tmp[i] = ar[i];
              i += 1;
             }

        }
        
        if (str.indexOf('|') == 1 && str.length > 0)
            temp[0] = str;
        
                
        return tmp;
    }
