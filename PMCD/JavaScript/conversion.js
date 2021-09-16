// ~~~~~~~~~~~~~~~~  Vietnamese JavaScript Input Editor, VietJie  ~~~~~~~~~~~~~~~~~~
// This is the Conversion Utiities of the VietJie
// Convert from VIQR to Unicode & Convert from Unicode to VIQR
// You are free to use VietJie as long as the name & url of Vovisoft are intact
// Vovisot www.vovisoft.com is a non profit organisation located in Australia 
// Please help to promote our devotion to doing something for our people

var viqrData = new Array		// Viet VIQR Map
(     
// vietchas: = 24 Vowels + d + D 

new Array(	"a^", "a", "a(", "e^", "e", "i", "o^", "o", "o+", "u", "u+", "y", 
			"A^", "A", "A(", "E^", "E", "I", "O^", "O", "O+", "U", "U+","Y", "d", "D"),
new Array(	"a^'", "a'", "a('", "e^'", "e'", "i'", "o^'", "o'", "o+'", "u'", "u+'", "y'", 
			"A^'", "A'", "A('", "E^'", "E'", "I'", "O^'", "O'", "O+'", "U'", "U+'", "Y'", "dd", "DD"),
new Array(	"a^`", "a`", "a(`", "e^`", "e`", "i`", "o^`", "o`", "o+`", "u`", "u+`", "y`", 
			"A^`", "A`", "A(`", "E^`", "E`", "I`", "O^`", "O`", "O+`", "U`", "U+`", "Y`"),
new Array(	"a^?", "a?", "a(?", "e^?", "e?", "i?", "o^?", "o?", "o+?", "u?", "u+?", "y?", 
			"A^?", "A?", "A(?", "E^?", "E?", "I?", "O^?", "O?", "O+?", "U?", "U+?", "Y?"),
new Array(	"a^~", "a~", "a(~", "e^~", "e~", "i~", "o^~", "o~", "o+~", "u~", "u+~", "y~", 
			"A^~", "A~", "A(~", "E^~", "E~", "I~", "O^~", "O~", "O+~", "U~", "U+~", "Y~"),
new Array(	"a^.","a.",  "a(.", "e^.","e.",  "i.", "o^.", "o.", "o+.", "u.", "u+.", "y.", 
			"A^.", "A.", "A(.", "E^.", "E.", "I.", "O^.", "O.", "O+.", "U.", "U+.", "Y.")	
);

function VIQR2Unicode(txtarea)
{
// These are the main accent chars for viqr used in j
var strTA = txtarea.value;	// equal to all the text in the text area
var strTemp = "";			// initialise a temp holder
var curViqrChar;
var uniChar;
var lastPos;
var lastChar;
var strBase;
var blnReplace;
var count = 1;
var lth = accentBase.length;
//txtDiv.innerHTML = VIQR2Uni;

	// loop through all the characters in the text area
	for (var i=0; i < strTA.length; i++) {
		curViqrChar = strTA.charAt(i);	
		if (curViqrChar == '*') (curViqrChar = '+');
		if (curViqrChar == "\n") {	
		   count += 1;				
		   window.Status = "Converting Line: " + count;	
	   }
       blnReplace = false;
	   // loop through all the accents in the accents array
		for (var j=1; (j<lth)&&(!blnReplace); j++) {
			if (curViqrChar == accentBase[j]){	// get the accent
			   lastPos = strTemp.length - 1;		
			   lastChar = strTemp.charAt(lastPos); // in Unicode
				for (var l=0; (l<26)&&(!blnReplace) ; l++) {	
					strBase = String.fromCharCode(uniData[0][l); 
					if (lastChar == strBase) {	
						uniChar = vowelComposer(l,j,-1);
						if (uniChar != "") {
							strTemp = strTemp.substring(0, lastPos);
							strTemp = strTemp + uniChar;
							blnReplace = true;
							break;
						}
					}
				}
			}
		}
       if (blnReplace == false) strTemp += strTA.charAt(i);	   
	}
	window.Status = "Done";	   
   txtarea.value = strTemp;
}

function Unicode2VIQR(txtarea)	
{
var strTA = txtarea.value;
var strTemp = "";
var curUniChar;
var chrReplace;
var blnReplace;
var count = 1;
var strDec;
//txtDiv.innerHTML = Uni2VIQR;

   for (var i=0; i < strTA.length; i++) {
       curUniChar = strTA.charAt(i);       
       if (curUniChar == "\n") {		
           count += 1;    
           window.Status = "Converting Line: " + count;
       }
	   blnReplace = false;
	   // loop through all the Unicode data array
		for (var j=0; (j<6)&&(!blnReplace); j++) {
			for (var k=0; (k<26)&&(!blnReplace); k++) {
				 strDec = uniData[j][k];           
				 if (curUniChar == String.fromCharCode(strDec)) {	
					 chrReplace = viqrData[j][k];	
					 blnReplace = true;
				}
			}
		}
	   (blnReplace == true)? strTemp += chrReplace : strTemp += curUniChar;
	}
	window.Status = "Done";	   
	txtarea.value = strTemp;
}
