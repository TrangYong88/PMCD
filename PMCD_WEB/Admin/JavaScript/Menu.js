var menuObj,tdObj,imgObj,index1;

function img_change(){
	imgObj=document.all('img_Menu1');
	imgObj[index1].src='images/tit left.png';
}

function img_return(){
	if(index1>=0)
		imgObj[index1].src='images/tit left.png';
}

function showmenu(e,which,imgName,tdName){
	index1=imgName;
	clearhidemenu();
	tdObj=document.getElementById(tdName);
	menuObj=document.getElementById("popmenu");
	menuObj.style.visibility="hidden"
	if(which!=''){
		menuObj.filters[0].Apply();
		menuObj.innerHTML=which;
		menuObj.style.left= getX(tdObj)+tdObj.offsetWidth;
		menuObj.style.top=  getY(tdObj)-2;
		menuObj.style.visibility="visible";
		menuObj.filters[0].Play();
	}
	return false;
};

function delayhidemenu(){
	delayhide=setTimeout("hidemenu()",100);
};

function clearhidemenu(){
	if (window.delayhide)
		clearTimeout(delayhide);
};
function hidemenu(){
	//menuObj.filters[0].Apply();
	menuObj.style.visibility= "hidden";
	//menuObj.filters[0].Play();
	imgObj[index1].src='images/tit left.png';
	tdObj.className="MKstyleNoneS";
};

function dynamichide(e){
	if (!menuObj.contains(e.toElement)){
		hidemenu();
	}	
};

function highlightmenu(e,state){
	tdObj.className="MKstyleS";
	imgObj[index1].src='images/tit left.png';
	if (document.all)
		source_el=event.srcElement;
	else 
	if (document.getElementById)
		source_el=e.target;
	if (source_el.className=="menuitems"){
		source_el.id=(state=="on")? "mouseoverstyle" : "";
	}
	else{
	while(source_el.id!="popmenu"){
		source_el=document.getElementById? source_el.parentNode : source_el.parentElement;
		if (source_el.className=="menuitems"){
			source_el.id=(state=="on")? "mouseoverstyle" : "";
		}
	}
	}
};

function getX(obj) {
  if ( obj == document.body ) 
	return obj.offsetLeft;
  else 
	return obj.offsetLeft +getX( obj.offsetParent );
};

function getY(obj) {
  if ( obj == document.body ) 
	return obj.offsetTop;
  else 
	return obj.offsetTop +getY( obj.offsetParent );
};