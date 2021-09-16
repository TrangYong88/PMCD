//  ~~~~~~~~~~ Interface for the Vietnamese JavaScript Input Editor, VietJie ~~~~~~~~~~~
//This is an interface for the radio buttons and Vietnamese brief typing instruction
// Important note: Saving this file to normal text format will make the text unreadable
// If you edit the file, save it to UTF-8 format to preserve the Vietnamese characters
// You are free to use VietJie as long as the name & url of Vovisoft are intact
// Vovisot www.vovisoft.com is a non profit organisation located in Australia 
// Please help to promote our devotion to doing something for our people

var AppName = " Vietnamese JavaScript Input Editor, VietJie 1.0 ";

var RadioBtn = ""; 
RadioBtn += "<table border='1' width='468' cellpadding='1' cellspacing='0' style='border-collapse: collapse' bordercolor='#D0D0D0' class='tblb'>";
  RadioBtn += "<tr class='xtr'>";
    RadioBtn += "<td><font color='red'>Ctrl + </font></td>";
    RadioBtn += "<td><font color='red'>1</font> = Off</td>";
    RadioBtn += "<td><font color='red'>2</font> = Telex</td>";
    RadioBtn += "<td><font color='red'>3</font> = VNI</td>";
    RadioBtn += "<td><font color='red'>4</font> = VIQR</td>";
    RadioBtn += "<td><font color='red'>Shift</font> = <--></td>";
  RadioBtn += "</tr>";
  RadioBtn += "<tr>";
    RadioBtn += "<td class='txt' colspan='6'><font size='1'>";
    RadioBtn += AppName + " by <a href='http://www.vovisoft.com'>";
    RadioBtn += "<font color='#0066FF'>Vovisoft</font></a></font></td>";
  RadioBtn += "</tr>";
RadioBtn += "</table>";

var Englishrule = "";
Englishrule = RadioBtn; 

var AccentType = "";
AccentType += "<table border='1' width='468' cellpadding='1' cellspacing='0' style='border-collapse: collapse' bordercolor='#D0D0D0' class='tblb'>";
  AccentType += "<tr class='txt'>";
    AccentType += "<td width='8%'>á</td>";
    AccentType += "<td width='8%'>à</td>";
    AccentType += "<td width='8%'>ả</td>";
    AccentType += "<td width='8%'>ã</td>";
    AccentType += "<td width='8%'>ạ</td>";
    AccentType += "<td>â ê ô</td>";
    AccentType += "<td>ơ ư</td>";
    AccentType += "<td width='8%'>ă</td>";
    AccentType += "<td>đ</td>";
    AccentType += "<td>\\ /</td>";
  AccentType += "</tr>";

var VNIrule = "";
  VNIrule += "<tr class='xtr'>";
    VNIrule += "<td>1</td>";
    VNIrule += "<td>2</td>";
    VNIrule += "<td>3</td>";
    VNIrule += "<td>4</td>";
    VNIrule += "<td>5</td>";
    VNIrule += "<td>6</td>";
    VNIrule += "<td>7</td>";
    VNIrule += "<td>8</td>";
    VNIrule += "<td>9, d</td>";
    VNIrule += "<td>Thoát</td>";
  VNIrule += "</tr>";
VNIrule += "</table>";
VNIrule = AccentType + VNIrule;

var Telexrule = "";
  Telexrule += "<tr class='xtr'>";
    Telexrule += "<td>s</td>";
    Telexrule += "<td>f</td>";
    Telexrule += "<td>r</td>";
    Telexrule += "<td>x</td>";
    Telexrule += "<td>j</td>";
    Telexrule += "<td>a e o</td>";
    Telexrule += "<td>w</td>";
    Telexrule += "<td>w</td>";
    Telexrule += "<td>9, d</td>";
    Telexrule += "<td>Thoát</td>";
  Telexrule += "</tr>";
Telexrule += "</table>";
Telexrule = AccentType + Telexrule; 

var VIQRrule = "";
  VIQRrule += "<tr class='xtr'>";
    VIQRrule += "<td>'</td>";
    VIQRrule += "<td>`</td>";
    VIQRrule += "<td>?</td>";
    VIQRrule += "<td>~</td>";
    VIQRrule += "<td>.</td>";
    VIQRrule += "<td>^</td>";
    VIQRrule += "<td>* +</td>";
    VIQRrule += "<td>(</td>";
    VIQRrule += "<td>9, d</td>";
    VIQRrule += "<td>Thoát</td>";
  VIQRrule += "</tr>";
VIQRrule += "</table>";
VIQRrule = AccentType + VIQRrule; 

var VIQR2Uni = ""; 
VIQR2Uni += "<table border='1' width='468' cellpadding='1' cellspacing='0' style='border-collapse: collapse' bordercolor='#D0D0D0' class='tblb'>";
  VIQR2Uni += "<tr>";
    VIQR2Uni += "<td class='txt'>" + AppName + "</td>";
  VIQR2Uni += "</tr>";
  VIQR2Uni += "<tr class='xtr'>";
    VIQR2Uni += "<td>Chuyển dạng chữ VIQR thành Unicode...</td>";
  VIQR2Uni += "</tr>";
VIQR2Uni += "</table>";

var Uni2VIQR = ""; 
Uni2VIQR += "<table border='1' width='468' cellpadding='1' cellspacing='0' style='border-collapse: collapse' bordercolor='#D0D0D0' class='tblb'>";
  Uni2VIQR += "<tr>";
    Uni2VIQR += "<td class='txt'>" + AppName + "</td>";
  Uni2VIQR += "</tr>";
  Uni2VIQR += "<tr class='xtr'>";
    Uni2VIQR += "<td>Chuyển từ Unicode thành dạng chữ VIQR...</td>";
  Uni2VIQR += "</tr>";
Uni2VIQR += "</table>";

function ctrl() {
var prefix = '';
	if (window.event.shiftKey) prefix = 's'; 
	if (window.event.ctrlKey) prefix = 'c'; 
    	if (window.event.altKey) prefix = 'a';
	return prefix;
}

function selectedBtn(btnGroup){
	for (var i = 0; i < btnGroup.length; i++) {
		if (btnGroup[i].checked) {
			return i
		}
	}
	return 0
}

function cycle(PostTopic) {
	var i = selectedBtn(PostTopic.tmode)
	if (i+1 == PostTopic.tmode.length) {
		PostTopic.tmode[0].checked = true;
		TypingMode(0);
	} else {
		PostTopic.tmode[i+1].checked = true;
		TypingMode(i+1);
	}
}