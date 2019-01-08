#pragma strict

var inv1 : String;
var inv2 : String;
var inv3 : String;
var inv4 : String;
var inv5 : String;
var viewSwitch : ViewActiveSwitch;
function Start () {
	AllManage.newUseItemCLStatic = this;
}

function OnEnable(){
	resetList();
}

var ebItem : NewUseItem;
var invItemArray : NewUseItem[];
var invParent : Transform; 
var GID : UIGrid;
var useInvID : String[];
var panelNewItem : UIPanel;
//var wuneirong : GameObject;
function SetEqupmentList(equStr : String){

	invClear();
	
	inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
	inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
	inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
	inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
	inv5 = InventoryControl.yt.Rows[0]["Item"].YuanColumnText;
//	inv5 = "";
	
	var i : int = 0;
	var useStr : String;
	var inv : InventoryItem;
	var rows : yuan.YuanMemoryDB.YuanRow;
	
	useStr = inv5 + inv1 + inv2+ inv3+ inv4; 
	useInvID = useStr.Split(";"[0]);
	
		for(i=0; i<useInvID.length; i++){
			if(useInvID[i] != ""){
//				wuneirong.transform.localPosition.y = 3000;
				var Obj : GameObject;
				var useEBI : NewUseItem;
				
				if(useInvID[i].Substring(0,2) == "88"){
					Obj = Instantiate(ebItem.gameObject); 
					Obj.transform.parent = invParent;
					Obj.transform.localScale = Vector3.one;
					useEBI = Obj.GetComponent(NewUseItem);
					useEBI.ebCL = this; 
					addInvItem(useEBI);								
					rows = GetItemRow(useInvID[i]);
					useEBI.AddDaoJuObj(rows["ItemID"].YuanColumnText , rows["Name"].YuanColumnText , rows["ItemInfo"].YuanColumnText , useInvID[i].Substring(5,2));
				}
				else
				if(useInvID[i].Substring(0,2) == "83"){
					Obj = Instantiate(ebItem.gameObject); 
					Obj.transform.parent = invParent;
					Obj.transform.localScale = Vector3.one;
					useEBI = Obj.GetComponent(NewUseItem);
					useEBI.ebCL = this; 
					addInvItem(useEBI);								
					useEBI.setInv(AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , inv));
				}
			}
		} 
	yield;
	GID.repositionNow = true;
//	panelNewItem.transform.localPosition.y = 0;
//	panelNewItem.clipOffset.y = 0;
}

function GetItemRow(id : String) : yuan.YuanMemoryDB.YuanRow{
	for(var rows : yuan.YuanMemoryDB.YuanRow in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytGameItem.Rows){
		if(id.Length > 4){
			if(rows["ItemID"].YuanColumnText == id.Substring(0,4)){
				return rows;
//				DaoJuPre.AddDaoJuObj(rows["ItemID"].YuanColumnText , rows["Name"].YuanColumnText , rows["ItemInfo"].YuanColumnText , id.Substring(5,2) , this);
			}
		}
	}
	return null;
}
var MyID : String ;
var MyName : String ;
var ps : PlayerStatus;
function UseItemAsID(id : String,name : String){
	MyID = id;
	MyName = name;
	if(id.Substring(0,2) == "88"){
		AllManage.qrStatic.ShowQueRen1(gameObject , "YesClick" , "" , AllManage.AllMge.Loc.Get("info1237") + AllManage.AllMge.Loc.Get("buttons833")+name+"?");
	}
	else
	if(id.Substring(0,2) == "83"){
		AllManage.qrStatic.ShowQueRen1(gameObject , "YesNowClcik" , "" , AllManage.AllMge.Loc.Get("info1237") + AllManage.AllMge.Loc.Get("buttons833")+name+"?");		
	}
	viewSwitch.ViewClose();
}


function YesClick(){
	AllManage.InvclStatic.UseDaojuAsID(MyID);
}

function YesNowClcik(){
	if(AllManage.InvclStatic.useBagItem(MyID, 1)){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(ps != null){
			ps.PlayerAction(MyID);
		}
	}
}

function resetList(){
	SetEqupmentList("");
}

function addInvItem(ebi : NewUseItem){
	var use : NewUseItem[]; 
	use = invItemArray; 
	invItemArray = new Array(invItemArray.length + 1);
	for(var i=0; i<(invItemArray.length - 1); i++){
		 invItemArray[i] = use[i];
	} 
	invItemArray[invItemArray.length - 1] = ebi;
}

function invClear(){
	for(var i=0; i<invItemArray.length; i++){
		if(invItemArray[i]){
			Destroy(invItemArray[i].gameObject);
		}
	}
	invItemArray = new Array(0);
}
