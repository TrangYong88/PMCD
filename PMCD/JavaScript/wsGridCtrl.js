// +--------------------------------------------+
// | wsGridCtrl	for Javascript					|		
// +--------------------------------------------+
// | 2004, whitespray							|
// | http://whitespray.com, ahn@jongha.pe.kr	|
// +--------------------------------------------+
function wsGridCtrl()
{
	// Methods 
	this.initializeDocument	= initializeDocument;
	this.InsItem			= InsItem;
	this.InsTab				= InsTab;
	this.GenerateCode		= GenerateCode;
	this.ToggleTree			= ToggleTree;
	this.ResetItem			= ResetItem;
	this.ExpandAllTree		= ExpandAllTree;
	this.RecudeAllTree		= RecudeAllTree;
	
	// constant
	var nCount			= 0;
	var LastRootItem	= 0;
	var ImgDir			= "./Images/";
	var	FrmWidth		= 0;
	var OnOverColor		= "";
	
	// variable
	var Doc;
	var browserVersion;	
	var id		= "";
	var Item	= new Array();
	var ItemTab	= new Array();
	
	
	function initializeDocument(iWidth, OverColor) 
	{ 
		if (document.all) { //IE4
			Doc = document.all;
			browserVersion = 1;  
			
		}else if (document.layers) { //NS4 
			Doc = document.layers;
			browserVersion = 2; 

		}else if(document.getElementById) {	//NS6
			Doc = document;
			browserVersion = 3;		
			
		}else {	//other 
			Doc = document.all;
			browserVersion = 0; 
		}
		
		SetSettings(iWidth, OverColor);
	} 	
	
	function SetSettings(iWidth, OverColor)
	{
		FrmWidth	= iWidth;
		OnOverColor = OverColor;
	}
	
	function InsItem(parentItem, description, hreference, target)
	{
		var iDepth = 0;
		var iLength = Item.length;
		
		if(parentItem == null) {
			parentItem = iLength;
		}
		
		if(Item[parentItem] != null) {
			iDepth = Item[parentItem][4];
			iDepth++;
		}
		
		Item[iLength]		= new Array();
		Item[iLength][0]	= parentItem;
		Item[iLength][1]	= description;
		Item[iLength][2]	= hreference;
		Item[iLength][3]	= target;
		Item[iLength][4]	= iDepth;
		Item[iLength][5]	= true;
		
		nCount++;
		
		return iLength;		
	}
	
	function InsTab(description, width)
	{
		var iLength = ItemTab.length;
		
		ItemTab[iLength]	= new Array();
		ItemTab[iLength][0]	= description;
		ItemTab[iLength][1]	= width;
	}

	function GenerateTabCode()
	{
		DocWrite("<tr>");
		
		for(var i=0; i<ItemTab.length; i++) {
			DocWrite("<td width='" + ItemTab[i][1] + "'>");
			DocWrite(GenerateTitleTab(ItemTab[i][0]));
			DocWrite("</td>");
		}

		DocWrite("</tr>");	
	}
	
	function GenerateCode()
	{
		var NextItemDepth	= 0;
		var CurItemDepth	= 0;
		
		DocWrite("<table border='0' cellpadding='0' cellspacing='1' bgcolor='#CCCCCC' width='" + FrmWidth + "'>");
		GenerateTabCode();
		
		for(var i=0; i<nCount; i++) {
			
			DocWrite("<tr id=wsTree_" + i + " bgcolor='#FFFFFF' onMouseOver=this.style.backgroundColor='" + OnOverColor + "' onMouseOut=this.style.backgroundColor='#FFFFFF'><td>");
			
			DocWrite("<table border='0' cellpadding='0' cellspacing='0'><tr><td>");
			DocWrite(GetSpace(i, Item[i][4], false));
			DocWrite("</td><td>");

			DocWrite("<a href='#' onfocus='this.blur()' onclick=javascript:ToggleTree('" + i + "','" + GetChildItems(i) + "')>");
				
			TempNodeImg = "minus.gif";		
			DocWrite("<img id=wsTreeNodeImg_" + i + " src='" + ImgDir + TempNodeImg + "' border='0'>");		
			DocWrite("</a>");

			DocWrite("</td><td>");
			

			var ArrItemDesc		= Item[i][1].split(";");
			var ArrItemHref		= Item[i][2].split(";");
			var ArrItemTarget	= Item[i][3].split(";");

			if(Item[i][2] != "" && Item[i][2] != null) {
				DocWrite("<a href='" + ArrItemHref[0] + "'");

				if(Item[i][3] != "undefined" && Item[i][3] != null && Item[i][3] != "") {
					DocWrite(" target='" + ArrItemTarget[0] + "'");
				}
				DocWrite(">");
			}
			
			DocWrite(ArrItemDesc[0);
			
			if(Item[i][2] != "" && Item[i][2] != null) {
				DocWrite("</a>");
			}

			var Temp = "1";
			
			DocWrite("</td></tr></table>");	
			DocWrite("</td>");

			GenerateSubContent(Item[i][1], i);
			
			DocWrite("</tr>");
		}
		DocWrite("</table>");
		
		LastRootItem = GetRootItem(nCount-1);
	}
	
	function GenerateTitleTab(TitleName)
	{
		var strHtml = "<input type='button' style='height:20; width=100%; border-width:1px;border-style:Solid;' onfocus='this.blur();' value='" + TitleName + "'>";
		return strHtml;
	}
	
	function GenerateSubContent(ContentText, CurNode)
	{
		for(var i=0; i<ItemTab.length-1; i++) {
		
			var ArrItemDesc		= Item[CurNode][1].split(";");
			var ArrItemHref		= Item[CurNode][2].split(";");
			var ArrItemTarget	= Item[i][3].split(";");

			for(var j=1; j<ArrItemDesc.length-1; j++) {
				DocWrite("<td><a href='" + ArrItemHref[j] + "' target='" + ArrItemTarget[j] + "'>" + ArrItemDesc[j] + "</a></td>");
			}
		}
	}
	
	function GetSpace(CurItem, Depth, bBlank)
	{
		var Space = "";
		
		for(var i=0; i<Depth; i++) {
			Space += "<img src=" + ImgDir + "white.gif>";
		}
		
		return Space;
	}
	
	function bHaveSameDepthChildItem(CurItem, Depth)
	{
		if(CurItem < 0 || CurItem > Item.length) { return false; }
	
		var PItem		= Item[CurItem][0];
		var RootItem	= GetRootItemEx(PItem, Depth);
		
		if(GetItemCount(RootItem, Depth) >= 2) {
			return true;
		}else {
			return false;
		}
	}
	
	function GetChildItems(iNode)
	{
		var ChildItems	= "";
		var CurDepth	= Item[iNode][4];
		
		for(var i=iNode+1; i<Item.length; i++) {

			if(CurDepth >= Item[i][4]) { return ChildItems; }
			
			if(Item[i][4] > Item[iNode][4]) {
				ChildItems += i + ";"
			}
		}

		return ChildItems;
	}
	
	function ToggleTree(CurNode, NodeItem)
	{
		if(NodeItem == "") { return; }
		
		var NodeStatus;
		var arr		= new Array();
		
		arr = NodeItem.split(";");
		
		if(Item[CurNode][5] == true) {
			ToggleDisplayLayer(arr, CurNode, "none");
			Item[CurNode][5] = false;
		}else {
			ToggleDisplayLayer(arr, CurNode, "");
			Item[CurNode][5] = true;
			ResetItem(CurNode);
		}
	}
	
	function ToggleDisplayLayer(ItemArray, CurNode, Display)
	{
		var NodeImg;
		var bShow;
		
		if(Display == "none") { bShow = false; }
		else{ bShow = true; }

		if(!bShow) {
			NodeImg = ImgDir + "plus.gif"; 			
		}
		else {
			NodeImg = ImgDir + "minus.gif"; 
		}

		for(var i=0; i<ItemArray.length-1; i++) {
			SetTreeVisible(ItemArray[i], bShow);			
			SetImgVisible(CurNode, NodeImg, 1);
		}
	}
	
	function ResetItem(iNode)
	{
		for(var i=iNode; i<Item.length; i++) {
	
			if(!Item[i][5]) {
				var arr = new Array();
				arr = GetChildItems(i).split(";");
				
				for(var j=0; j<arr.length-1; j++) {
					SetTreeVisible(arr[j], false);
				}
			}
		}	
	}
	
	function SetTreeVisible(iItem, bShow)
	{
		if(!bShow) {
			if(browserVersion == 1) {
				Doc["wsTree_" + iItem].style.display = "none";
				
			}else if(browserVersion == 3) {
				document.getElementById("wsTree_" + iItem).style.display = "none";
				
			}else {
				Doc["wsTree_" + iItem].visibility = "hiden";
			}
	
		}else {
			if(browserVersion == 1) {
				Doc["wsTree_" + iItem].style.display = "block";
				
			}else if(browserVersion == 3) {
				Doc.getElementById("wsTree_" + iItem).style.display = "";	
							
			}else {
				Doc["wsTree_" + iItem].visibility = "show";
			}				
		}
	}	
	
	function SetImgVisible(iItem, ImgName, iType)
	{
		var ItemName;
		switch(iType) {
			case 1:		ItemName = "wsTreeNodeImg_"; break;
			case 2:		ItemName = "wsTreeItemImg_"; break;
			default:	ItemName = "wsTreeItemImg_"; break;
		}

		if(browserVersion == 3) {
			Doc.getElementById(ItemName + iItem).src = ImgName;
			
		}else {
			Doc[ItemName + iItem].src = ImgName;
		}
	}	
	
	function ExpandAllTree()
	{
		for(var i=Item.length-1; i>=0; i--) {
			Item[i][5] = false;
			ToggleTree(i, GetChildItems(i));
		}
	}
	
	function RecudeAllTree()
	{
		for(var i=Item.length-1; i>=0; i--) {
			Item[i][5] = true;
			ToggleTree(i, GetChildItems(i));
		}
	}
	
	function GetRootItem(ChildItem)
	{
		if(Item[ChildItem][4] == 0) {
			return ChildItem;
		}else {
			return GetRootItem(Item[ChildItem][0);
		}
	}
	
	function GetRootItemEx(ChildItem, Depth)
	{
		if(Item[ChildItem][4] == Depth) {
			return ChildItem;
		}else {
			return GetRootItem(Item[ChildItem][0], Depth);
		}
	}
	
	function GetItemCount(CurItem, Depth)
	{
		var nRet = 0;
		
		for(var i=CurItem; i<Item.length; i++) {
			if(Item[i][4] < Depth) break;
			
			if(Item[i][4] == Depth) {
		
				nRet++;
			}
		}
		
		return nRet;
	}

	function DocWrite(strHtml)
	{
		document.write(strHtml);
	}
}

function ToggleTree(CurNode, NodeItem)
{
	m_wsGridCtrl.ToggleTree(CurNode, NodeItem);
}
