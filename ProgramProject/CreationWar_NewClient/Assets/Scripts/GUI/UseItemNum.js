#pragma strict

function Awake(){
	AllManage.uusStatic = this;
}

function useItemNum(id : String , num : int){
	var idNum : int = parseInt(id.Substring(5,2)) - num;
	var useidNum : String;
	useidNum = idNum.ToString();
	if(useidNum.Length < 2){
		useidNum = "0" + useidNum;
	}
	if(idNum <= 0){
		return "";
	}else{
		if(id.Length > 8){
			id = id.Substring(0,5) + useidNum + ","+id.Substring(8,1);	
		}else{
			id = id.Substring(0,5) + useidNum;	
		}
	}
	return id;
}

function CardMoveOn(){
	AllManage.UIALLPCStatic.show30();
	yield;
	yield;
	yield;
	var invs : InventoryItem[];
	invs = new Array(5);
	var str : String;
	for(var i=0; i<5; i++){
		str = AllResources.staticLT.MakeItemID1(str, i+1);
		var inv : InventoryItem;
		inv = AllResources.InvmakerStatic.GetItemInfo(str, inv);
		invs[i] = inv;
	}
	AllManage.CardCLStatic.GoShowCards(invs , 1);
}
