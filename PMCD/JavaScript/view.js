function viewPic1(image1){
tipDiv.innerHTML = "<img src='" + image1 + "'>";
tipDiv.style.visibility="hidden";
var height1=tipDiv.scrollHeight;
var width1=tipDiv.scrollWidth;
newwin = open(' ', '_blank','Status=0,height=' + height1 + ', width=' + width1 + ',left='+((screen.width-width1)/2)+', top='+((screen.height-height1)/2-50));
newwin.document.writeln('<html><title>view Image</title><body bottommargin=0 topmargin=0 leftmargin=0>');
newwin.document.writeln('<a href=# onClick="window.close(); return false;"><img src="' + image1 + '" border=0></a>');
newwin.document.writeln('</body></html>');
};

