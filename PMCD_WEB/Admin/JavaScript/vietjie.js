	var accentBase = new Array( "", "'", "`", "?", "~", ".", "^", "+", "(", "d", "D");

	var uniData = new Array			// Viet Unicode Map
	(
	new Array(  226, 97, 259, 234, 101, 105, 244, 111, 417, 117, 432, 121, 
				194, 65, 258, 202, 69, 73, 212, 79, 416, 85, 431, 89, 100, 68),
	new Array(  7845, 225, 7855, 7871, 233, 237, 7889, 243, 7899, 250, 7913, 253, 
				7844, 193, 7854, 7870, 201, 205, 7888, 211, 7898, 218, 7912, 221, 273, 272),
	new Array(  7847, 224, 7857, 7873, 232, 236, 7891, 242, 7901, 249, 7915, 7923,
				7846, 192, 7856, 7872, 200, 204, 7890, 210, 7900, 217, 7914, 7922),
	new Array(  7849, 7843, 7859, 7875, 7867, 7881, 7893, 7887, 7903, 7911, 7917, 7927,
				7848, 7842, 7858, 7874, 7866, 7880, 7892, 7886, 7902, 7910, 7916, 7926),
	new Array(  7851, 227, 7861, 7877, 7869, 297, 7895, 245, 7905, 361, 7919, 7929,
				7850, 195, 7860, 7876, 7868, 296, 7894, 213, 7904, 360, 7918, 7928),
	new Array(  7853, 7841, 7863, 7879, 7865, 7883, 7897, 7885, 7907, 7909, 7921, 7925,
				7852, 7840, 7862, 7878, 7864, 7882, 7896, 7884, 7906, 7908, 7920, 7924)
	);

ddc = String.fromCharCode(272);
DDc = String.fromCharCode(273);
uMoc = String.fromCharCode(432);
UMoc = String.fromCharCode(431);
var vowelpat = new RegExp("io\$|iu\$|ia\$|ua\$|ue\$|ui\$|uy\$","i");	
var vowelpat1 = new RegExp("^qu[^o\$]","i");	
var vowelpat2 = new RegExp("[iuy]","i");	
var xconso = new RegExp("[bcfghjklmnpqrstvwxz0-9]", "i");	//consonants and numbers only
var vconso3 = new RegExp("[gq]","i");		//applied 2 g,G,q,Q only
var vconso2 = new RegExp("[bcdfghjklmnpqrstvwxyz0-9]", "i");//consonants and numbers only
var vconso1 = new RegExp("[a-z]","i"); //	
var vconso0 = new RegExp("[~`!@#$%^&*\(\)\|\\{}.\,+=_-]");
var	dblconso = new RegExp(".h$|.g$", "i");
var dD = new RegExp("^d","i");
var telexacc = new RegExp("[sfrxjaeowd]", "i");	//Telex accents
var typeMode;

function refreshPage()
{	
	TypingMode(0);
}

function TypingMode(setMode)
{
		switch (setMode) {
		   case 0:	typeMode = "Off"; 
					tmode[0].checked = true;
					break;
		   case 1:	typeMode = "Telex"; 
					tmode[1].checked = true;
					break;
		} 
}

function CharAtCaret(txtarea)
{
    objRange = document.selection.createRange();
    objRange.moveStart("character", -1);
    txtarea.curchar = objRange.duplicate();
    return txtarea.curchar.text;
}

function WordAtCaret(txtarea) 
{
	var objRange = document.selection.createRange();
	objRange.moveStart("word", -1);   
	txtarea.curword = objRange.duplicate();
	return txtarea.curword.text;
}

function vowelComposer(k,j,lacc) {
	var newpos = k;//char base
	var i = lacc;
	var acc = j;
	var composedvowel;
	var caretWord = WordAtCaret(this);

	if ((!vowelpat.test(caretWord))&&(typeMode=="Telex")&&(j==7)&&(k==0||k==12||k==1||k==13)) {
		acc = 8;
	}
	if (acc<6) {
		composedvowel = String.fromCharCode(uniData[acc][newpos); 
	}
	else {
		((acc==6)&&(k==1||k==4||k==7||k==13||k==16||k==19))? newpos=k-1:((k==2||k==8||k==14||k==20)?newpos=k-2:newpos=k);	
		if (acc==7) { (k==7||k==9||k==19||k==21)?newpos=k+1:(k==6||k==18)?newpos=k+2:newpos=k; }
		if (acc==8) { (k==1||k==13)? newpos=k+1:((k==0||k==12)?newpos=k+2:newpos=k); }
		if (lacc != -1) {
			composedvowel = String.fromCharCode(uniData[lacc][newpos);
		} else {
			composedvowel = String.fromCharCode(uniData[0][newpos);
		}
		if ((acc==9||acc==10) && (k>23)) { 
			newpos = k;
			composedvowel = String.fromCharCode(uniData[1][newpos);
		}
	}
	return composedvowel;
}

function vowelChecker(vwlBase, accTyped) {

var vowelBase;
var vowelNew;
	for (var i=0; (i<6); i++)  {
		for (var j=0; (j<26); j++)  {	//loop through all vowel arrays
			vowelBase = String.fromCharCode(uniData[i][j); 
			if (vwlBase == vowelBase) {	// found a match
				vowelNew = vowelComposer(j,accTyped,i)
				return vowelNew;
			}	
		}	
	}
	return accTyped;
}

function WordComposer(curWord,accInd) {
var wl = curWord.length;				//length of this word
var cp4 = curWord.charAt(wl-4);			//third next to last in curWord
var cp3 = curWord.charAt(wl-3);			//second next to last in curWord
var cp2 = curWord.charAt(wl-2);			//first next to last in curWord
var cp1 = curWord.charAt(wl-1);			//last consonant in curWord
var last2 = curWord.substring(wl-2,wl);	//last 2 consonants in curWord
var start1 = curWord.charAt(0);			//first consonant is a d or D
var vowelNew;
var wordNew; 
var newcp;

	if ((dD.test(start1))&&(accInd==9)) {	
		vowelNew = vowelChecker(start1, accInd);
		wordNew = vowelNew + curWord.substring(1,wl);
		return wordNew;
	}
	if ((typeMode=="Telex")&&(accInd==7)&&(vowelpat1.test(WordAtCaret(this)))) {
		accInd = 8;
	}
	if ((!vconso0.test(cp1))&&(wl>0)&&(wl<7)) {	
		if ((dblconso.test(last2))&&(wl>2)) {
			vowelNew = vowelChecker(cp3, accInd);
			wordNew = curWord.substring(0,wl-3) + vowelNew + cp2 + cp1;
			if ((accInd==7)&&(cp4=='u'||cp4=='U'||cp4==uMoc||cp4==UMoc)) {
				var vowelNew1 = vowelChecker(cp4, accInd);
				var vowelNew2 = vowelChecker(cp3, accInd);
				vowelNew = vowelNew1 + vowelNew2;
				wordNew = curWord.substring(0,wl-4) + vowelNew + cp2 + cp1;
			}
			if ((accInd==6)&&(cp4=='u'||cp4=='U'||cp4==uMoc||cp4==UMoc)) {
				if (cp4==uMoc||cp4=='u') (newcp='u');
				if (cp4==UMoc||cp4=='U') (newcp='U');
				vowelNew = vowelChecker(cp3, accInd);
				wordNew = curWord.substring(0,wl-4) + newcp + vowelNew + cp2 + cp1;
			}
				return wordNew;
		}
		if ((vconso3.test(cp3))&&(vowelpat.test(last2))&&(accInd!=7)&&(wl>1)) {
			vowelNew = vowelChecker(cp1, accInd);
			wordNew = curWord.substring(0,wl-1) + vowelNew;
				return wordNew;
		}	
		if ((accInd==6)&&(cp2=='u'||cp2=='U'||cp2==uMoc||cp2==UMoc)) {
			if (cp2==uMoc||cp2=='u') (newcp='u');
			if (cp2==UMoc||cp2=='U') (newcp='U');
			vowelNew = vowelChecker(cp1, accInd);	
			wordNew = curWord.substring(0,wl-2) + newcp + vowelNew;
				return wordNew;
		}
		if ((accInd==7)&&(cp2=='u'||cp2=='U'||cp2==uMoc||cp2==UMoc)) {
			if (!(cp1=='u'||cp1=='U'||xconso.test(cp1))) {
				var vowelNew1 = vowelChecker(cp1, accInd);
			} else {
				var vowelNew1 = cp1;
			}
			if (cp1=='o'||cp1=='O') {
				var vowelNew2 = cp2;
			} else {
				var vowelNew2 = vowelChecker(cp2, accInd);
			}
			wordNew = curWord.substring(0,wl-2) + vowelNew2 + vowelNew1;
				return wordNew;
		}
		if (!(cp2==ddc||cp2==DDc)&&(!vconso2.test(cp2))&&(vconso1.test(cp1))&&(wl>1)) { 	
			if ((accInd==7)&&(cp3=='u'||cp3=='U'||cp3==uMoc||cp3==UMoc)) {
				var vowelNew1 = vowelChecker(cp3, accInd);
				var vowelNew2 = vowelChecker(cp2, accInd);
				vowelNew = vowelNew1 + vowelNew2;
				wordNew = curWord.substring(0,wl-3) + vowelNew + cp1;
			} else if ((accInd==6)&&(cp3=='u'||cp3=='U'||cp3==uMoc||cp3==UMoc)) {
				if (cp3==uMoc||cp3=='u') (newcp='u');
				if (cp3==UMoc||cp3=='U') (newcp='U');
				vowelNew = vowelChecker(cp2, accInd);	
				wordNew = curWord.substring(0,wl-3) + newcp + vowelNew + cp1;
			} else if ((accInd==6||accInd==7)&&(vowelpat2.test(cp2))) { 
				vowelNew = vowelChecker(cp1, accInd);	
				wordNew = curWord.substring(0,wl-1) + vowelNew;
			} else if ((accInd==8)&&(!vconso2.test(cp1))) {
				vowelNew = vowelChecker(cp1, accInd);	
				wordNew = curWord.substring(0,wl-1) + vowelNew;
			} else {	 
				vowelNew = vowelChecker(cp2, accInd);
				wordNew = curWord.substring(0,wl-2) + vowelNew + cp1;
			}
				return wordNew;
		}
		if ((!xconso.test(cp1))&&(wl>0)&&(accInd>0)) {//vowel at last char 
			vowelNew = vowelChecker(cp1, accInd);
			wordNew = curWord.substring(0,wl-1) + vowelNew;
				return wordNew;
		}
	}	
	return curWord;
}

function keyhandler(e) {
}

function VietTyping(){
var	nonChar = new RegExp("\W","i");
var caretChar;
var caretWord;
var wordNew;
var accIndex;
var kc;		

    if (document.layers) {
        kc = e.which;
        alert("Sorry your Netscape browser does not support this application! Please use IE instead.");
    } else {
        kc = window.event.keyCode;
	}

	if (typeMode != "Off") {	

		caretChar = CharAtCaret(this);	//last typed in char
		if (caretChar==""||caretChar==" ") return;

		if (caretChar=="\\"||caretChar=="\/") {
			if (!(kc==47||kc==92)) {
				event.keyCode = null;
				this.curchar.text = String.fromCharCode(kc);
				this.curword.collapse(false);
				return true;
			} else {
				return;
			}
		}

		if (nonChar.test(caretChar)) return; //for testing

		switch (typeMode) {		//filter non accent key
			case "Off": accIndex = 0; break
			case "VNI": accIndex = VNIaccInd(kc); break;
			case "Telex": accIndex = TelexaccInd(caretChar,kc); break;
			case "VIQR": accIndex = VIQRaccInd(kc); break;	
		}
		if (accIndex > 0) {
			caretWord = WordAtCaret(this);	//last typed in word
			wordNew = WordComposer(caretWord,accIndex);	//add accent to vowel
			if (wordNew != caretWord) {
				event.keyCode = 0;
				this.curword.text = wordNew;
				this.curword.collapse(false);
			}
		}
	}
    return true;
}

// ~~~~~ Typing Modes ~~~~~

function VNIaccInd(keyc) {
var accInd;
	// VNI typing mode
	if ((keyc > 48) && (keyc < 57)) accInd = (keyc - 48); // key 1 to 8
	else	
	if (keyc==57||keyc==68||keyc==100) accInd = 9; // 9, d, D 
	return accInd;
}

function TelexaccInd(cp,keyc) {
var accInd;
var acckey = String.fromCharCode(keyc);
var caretWord = WordAtCaret(this);
var hasvowel = false;
	if (!telexacc.test(acckey)) { // skip if input is not an accent key
		accInd = 0;
		return;
	}
	var charArr = caretWord.split("");
	for (var i=0;i < charArr.length;i++) {
		var testChar = charArr[i];
		if (!xconso.test(testChar)) {
			hasvowel = true; 
			break;
		}
	}
	if (hasvowel) {
		// TELEX typing mode
		switch (keyc)	{
			case 115: case 83: accInd = 1; break; // s 
			case 102: case 70: accInd = 2; break; // f 
			case 114: case 82: accInd = 3; break; // r 
			case 120: case 88: accInd = 4; break; // x 
			case 106: case 74: accInd = 5; break; // j
			case 97: case 65: ((vconso2.test(cp))||(!(cp=='e'||cp=='o')))?accInd=6:accInd=0; break;  
			case 101: case 69:((vconso2.test(cp))||(!(cp=='a'||cp=='o')))?accInd=6:accInd=0; break; 	
			case 111: case 79:((vconso2.test(cp))||(!(cp=='e'||cp=='a')))?accInd=6:accInd=0; break; 	
			case 119: case 87:(!(cp=='a'||cp=='A'))? accInd=7:
			(vowelpat.test(WordAtCaret(this)))? accInd=7:accInd=8; break; // w 
			case 68: case 100: accInd = 9;break;// d, D
		} 
	} else {
		accInd = 0;
	}
	return accInd;
}

function VIQRaccInd(keyc){
var accInd;
	// VIQR typing mode
	switch (keyc)
	{
		case 39: accInd = 1;break; // ' 
		case 96: accInd = 2;break; // ` 
		case 63: accInd = 3;break; // ? 
		case 126:accInd = 4;break; // ~ 
		case 46: accInd = 5;break; // . 
		case 94: accInd = 6;break; // ^ 
		case 42: case 43: accInd = 7;break; // * or + 
		case 40: accInd = 8;break; // ( 
		case 68: case 100: accInd=9;break; // 9, d, D	
	} 
	return accInd;
}

function bsErase()
{
var kc;
var currChar;
var lastChar;
var changedChar;
var bc;
var foundChar;
var combinekey;
var kc = event.keyCode;

		combinekey = ctrl() + kc;
		switch (combinekey) {
			case 'c49': TypingMode(0);event.keyCode = 0;return;//1 =Off
			case 'c50': TypingMode(1);event.keyCode = 0;return;//2 =Telex
			case 'c51': TypingMode(2);event.keyCode = 0;return;//3 =VNI
			case 'c52': TypingMode(3);event.keyCode = 0;return;//4 =Viqr
	}

	if ((ctrl() + kc)== 'c16') cycle(myForm);

	kc = event.keyCode;
	if (kc == 8)  {
		currChar = CharAtCaret(this);
		foundChar = false;
		if ((currChar !="")&&(currChar!="d")&&(currChar!="D")) {
			for (var i=0; (i<6)&&(!foundChar); i++)  {
				for (var j=0; (j<26)&&(!foundChar); j++)  {
					combinedChar = String.fromCharCode(uniData[i][j); 
					if (currChar == combinedChar) {	// found a match
						if (i>0) { changedChar = String.fromCharCode(uniData[0][j]) }
						else {
							if (j==0||j==3||j==6||j==12||j==15||j==18) {
								bc = j+1;
								changedChar = String.fromCharCode(uniData[0][bc);
							}
							if (j==2||j==8||j==10||j==14||j==20||j==22) {
								bc = j-1;
								changedChar = String.fromCharCode(uniData[0][bc);
							}
							if (j==1||j==4||j==5||j==7||j==9||j==11||j==13||j==16||j==17||j==19||j==21||j==23) {
								changedChar = "";
							}
						}
						var objRange = document.selection.createRange();
						objRange.moveEnd("character", -1);
						this.curchar = objRange.duplicate();
						this.curchar.text = changedChar;
						this.curchar.collapse(false);
						foundChar = true;
					}	
				}
			}
		}
	}return;
}
