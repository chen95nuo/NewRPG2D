private var Fstr : String = "=";
private var Sstr : String = ";";
//var invMaker : Inventorymaker;
var inv1 : String;
var inv2 : String;
var inv3 : String;
var inv4 : String;
var inv5 : String;

static var me : GameObject;
function Awake(){
	me = gameObject;
//	invMaker = AllResources.InvmakerStatic;
	invcl = AllManage.InvclStatic; 
//	ts = AllManage.tsStatic;
//	QR = AllManage.qrStatic;
	uus = AllManage.uusStatic;
}	

var myPanel : UIPanel;
function Start () {
	var mm : boolean = false; 
	var OneTime : boolean = false;
	if(!InventoryControl.ytLoad){
		while(!mm){
			if(InventoryControl.ytLoad){
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
				inv5 = InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText;
				mm = true;
				OneTime = true;
			}
			yield;
		}	
	}
	
//	//print("yun xing le zhe li");
	SetEqupmentList("");
	SelectBeforeInv();
	yield WaitForSeconds(0.1f);
	SelectOneStoneClose();
	myPanel.widgetsAreStatic = false;
	yield;
	yield WaitForSeconds(1);
	yield;
	myPanel.widgetsAreStatic = true;	
	
}
function SelectBeforeInv(){
	for(var i=0; i<invItemArray.length; i++){
		SelectOneInv(invItemArray[i] , invItemArray[i].inv);
		return;
	}
}
function SelectBeforeInv(id : String){
	for(var i=0; i<invItemArray.length; i++){
		if(id == invItemArray[i].inv.itemID){
			SelectOneInv(invItemArray[i] , invItemArray[i].inv);	
			return;
		}
	}
}

var ebItem : EquepmentPunchItem;
var invItemArray : EquepmentPunchItem[];
var invParent : Transform; 
var GID : UIGrid;
var useInvID : String[];
var wuneirong : GameObject;
function SetEqupmentList(equStr : String){
	invClear();
	
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
				inv5 = InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText;
	var i : int = 0;
	
	var useStr : String;
	var inv : InventoryItem;
	var useInv : InventoryItem;
	useStr =  inv5 + inv1 + inv2 + inv3 + inv4; 
//	//print("useStr == " + useStr);
	useInvID = useStr.Split(Sstr.ToCharArray());
	for(i=0; i<useInvID.length; i++){
		useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
		if(useInv != null){
			useInvID[i] = useInv.itemID;
		}
//		print(useInvID[i] + " ========= useInvID[i]");
		if(useInvID[i] != "" && parseInt(useInvID[i].Substring(0,1)) < 7){
			wuneirong.transform.localPosition.y = 3000;
			var Obj : GameObject = Instantiate(ebItem.gameObject); 
			Obj.transform.parent = invParent;
			var useEBI : EquepmentPunchItem;
			useEBI = Obj.GetComponent(EquepmentPunchItem);
			useEBI.setInv(AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , inv));
			useEBI.ebCL = this; 
			addInvItem(useEBI);				
		}
	} 
	yield;
	GID.repositionNow = true;
}

function addInvItem(ebi : EquepmentPunchItem){
	var use : EquepmentPunchItem[]; 
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

var eii :  EquepmentItemInfo; 
var myEBI : EquepmentPunchItem;
function SelectOneInv(ebi : EquepmentPunchItem , inv : InventoryItem){
	var i : int = 0;
	for(i=0; i<invItemArray.length; i++){
		if(invItemArray[i] == ebi){  
			myEBI = ebi;
			eii.gameObject.SetActiveRecursively(true);
			eii.showHoleEquItemInfo(inv,null);
			invItemArray[i].SelectMe();
		}else{
			invItemArray[i].DontSelectMe();		
		}
	}
	xiangqianButton.SetActiveRecursively(false);
	SelectOneStoneClose();
} 

function resetList(){
	SetEqupmentList("");
	yield;
	xiangqianButton.SetActiveRecursively(false);
}

function SelectFristInv(){
	var i : int = 0;
	for(i=0; i<invItemArray.length; i++){
			myEBI = invItemArray[i];
//			LabelUseBloodStone.text = "x" + invItemArray[i].inv.itemQuality.ToString();
			yield;
			yield;
			yield;
			yield;
			eii.gameObject.SetActiveRecursively(true);
			eii.showHoleEquItemInfo(invItemArray[i].inv,null);
			invItemArray[i].SelectMe();
			xiangqianButton.SetActiveRecursively(false);
			return;
	}
}

function OnEnable(){
	StoneParent.SetActiveRecursively(false);
}

var itemHoles : UISlicedSprite[];
var oneItemHole : UISlicedSprite;
var oneInvHole : InventoryItem;
var oneHoleID : int;
var StoneParent : GameObject;
function XiangQian(uiss : UISlicedSprite , id : int){
	if(myEBI && myEBI.inv){
		SelectOneInv(myEBI , myEBI.inv);
	}
	yield;
	StoneParent.SetActiveRecursively(true);
	oneItemHole = uiss;
	oneHoleID = id;
//	//print("xiang qian");
	SetStoneEqupmentList("");
	
}

function SelectOneStone(iv : InventoryItem){
	StoneParent.SetActiveRecursively(false);
	oneInvHole = iv;
	oneItemHole.spriteName = oneInvHole.atlasStr;
	xiangqianButton.SetActiveRecursively(true);
}

function SelectOneStoneClose(){
	xiangqianButton.SetActiveRecursively(false);
	StoneParent.SetActiveRecursively(false);
}

function isDoHole(){
	var int1 : int = myEBI.inv.itemLevel;
	var int2 : int = myEBI.inv.itemQuality;
	if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
		AllManage.qrStatic.ShowBuyQueRen1(this.gameObject , "YesDoHole" , "NoDoHole" , AllManage.AllMge.Loc.Get("info1037"));
	else
		YesDoHole();
}

function NoDoHole(){

}

function YesDoHole(){
	var i : int = 0;
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	var holeString : String = "";
	
	for(var j=0; j<useInvID.length; j++){
		if(useInvID[j] == oneInvHole.itemID){ 
			holeString = useInvID[j];
//			useInvID[j] = uus.useItemNum(useInvID[j] , 1);
		}
	}
	for(i=0; i<useInvID.length; i++){
		if(useInvID[i] == myEBI.inv.itemID){
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.EquipMosaic).ToString());
			var iiv : InventoryItem;
			iiv = new InventoryItem();
			iiv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , iiv);
			var int1 : int = iiv.itemLevel;
			var int2 : int = iiv.itemQuality;
//			AllManage.tsStatic.RefreshBaffleOn();
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().EquepmentMosaic(useInvID[i] , i , oneHoleID  , holeString , (int1*10 + int2*50) * (-1) , (int1 + int2) * (-1)));
			return;
//			if(ps.UseMoney(int1*10 + int2*50 , int1 + int2)){
//				DoXiangQian(useInvID[i] , i); 
//			}
		}
	}
}
 
var uus : UseItemNum; 
var xiangqianButton : GameObject;
function DoXiangQian(invID : String , id : int){ 
	xiangqianButton.SetActiveRecursively(false);
//	//print("da kong qu" + invID + " == " + id);
	var m : int = Random.Range(-3,3) + 6;
	var useStr1 : String;
	var useStr2 : String;
	var useStr3 : String;
	var useInt : int;
	useStr1 = useInvID[id].Substring(19,2);
	useStr2 = useInvID[id].Substring(21,2);
	useStr3 = useInvID[id].Substring(23,2);
	if(oneHoleID == 1){
		useStr1 = oneInvHole.itemID.Substring(2,2);
	}else
	if(oneHoleID == 2){
		useStr2 = oneInvHole.itemID.Substring(2,2);	
	}else
	if(oneHoleID == 3){
		useStr3 = oneInvHole.itemID.Substring(2,2);	
	}
//	//print(useInvID[id] + " == " + m);
	useInvID[id] = useInvID[id].Substring(0,19) + useStr1 + useStr2 + useStr3; 
//	//print(useInvID[id] + " ==");
///////
	for(var j=0; j<useInvID.length; j++){
		if(useInvID[j] == oneInvHole.itemID){ 
			useInvID[j] = uus.useItemNum(useInvID[j] , 1);
			break;
		}
	}
	InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText = "";
//	//print(useInvID.length);
	for(var i=0; i<useInvID.length; i++){	 
		if(i < 12){
			InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText += useInvID[i] + ";";
		}else
		if(i < 27 && i >= 12){
			InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 42 && i >=27){
			InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 57 && i >= 42){
			InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 72 && i >= 57){
			InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText += useInvID[i] + ";";		
		}
	}  
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
				inv5 = InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText;
	SetEqupmentList("");
	invcl.isUpdatePhoton = false;
	invcl.ReInitItem1();
	SelectBeforeInv(useInvID[id]);
}

////////////////////////////////////////////////////////////////////////////////
var esItem : EquepmentStoneItem;
var invStoneItemArray : EquepmentStoneItem[];
var invStoneParent : Transform; 
var StoneGID : UIGrid;
var useStoneInvID : String[];
function SetStoneEqupmentList(equStr : String){
	invStoneClear();
//	eii.gameObject.SetActiveRecursively(false);
	var i : int = 0;
	
	var useStr : String;
	var inv : InventoryItem;
	var useInv : InventoryItem;
	useStr =  inv5 + inv1 + inv2 + inv3 + inv4; 
//	//print("useStr == " + useStr);
	useInvID = useStr.Split(Sstr.ToCharArray());
	for(i=0; i<useInvID.length; i++){
		useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
		if(useInv != null){
			useInvID[i] = useInv.itemID;
		}
		if(useInvID[i] != "" && parseInt(useInvID[i].Substring(0,2)) == 81){
			var Obj : GameObject = Instantiate(esItem.gameObject); 
			Obj.transform.parent = invStoneParent;
			Obj.transform.localScale = Vector3.one;
			var useEBI : EquepmentStoneItem;
			useEBI = Obj.GetComponent(EquepmentStoneItem);
			useEBI.setInv(AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , inv));
			useEBI.EquepmentPC = this; 
			addStoneInvItem(useEBI);				
		}
	} 
//	yield;
	StoneGID.repositionNow = true;
}

function addStoneInvItem(ebi : EquepmentStoneItem){
	var use : EquepmentStoneItem[]; 
	use = invStoneItemArray; 
	invStoneItemArray = new Array(invStoneItemArray.length + 1);
	for(var i=0; i<(invStoneItemArray.length - 1); i++){
		 invStoneItemArray[i] = use[i];
	} 
	invStoneItemArray[invStoneItemArray.length - 1] = ebi;
}

function invStoneClear(){
	oneInvHole = null;
	for(var i=0; i<invStoneItemArray.length; i++){
		if(invStoneItemArray[i]){
			Destroy(invStoneItemArray[i].gameObject);
		}
	}
	invStoneItemArray = new Array(0);
}

function show0(){
	invcl.UpdatePhotonEquep();
	AllManage.UIALLPCStatic.show0();
}

////////////////////////////////////////////////////////////////////////////////
//var QR : QueRen;
//var ts : TiShi;
var HoleInt : int = 0;
function DaKong(){
//	var int1 : int = myEBI.inv.itemLevel;
//	var int2 : int = myEBI.inv.itemQuality;
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	switch(myEBI.inv.itemHole){
		case 0 :
			if(parseInt(ps.Level) >= 0){
				HoleInt = 5;
				if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
					AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsEquepmentHole , myEBI.inv.itemHole , 0 , "" , gameObject , "EquepmentHoleTips");
//					AllManage.qrStatic.ShowBuyQueRen1(this.gameObject , "YesDaKong" , "NoDaKong" , AllManage.AllMge.Loc.Get("info299")+ ""+ HoleInt +AllManage.AllMge.Loc.Get("info301")+"");	
				else
					YesDaKong();
			}else{
				AllManage.tsStatic.Show("tips017");
			}
			break;
		case 1 :
			if(parseInt(ps.Level) >= 0){
				HoleInt = 5;
				if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
					AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsEquepmentHole , myEBI.inv.itemHole , 0 , "" , gameObject , "EquepmentHoleTips");
//					AllManage.qrStatic.ShowBuyQueRen1(this.gameObject , "YesDaKong" , "NoDaKong" , AllManage.AllMge.Loc.Get("info299")+ ""+ HoleInt +AllManage.AllMge.Loc.Get("info301")+"");	
				else
					YesDaKong();
			}else{
				AllManage.tsStatic.Show("tips017");
			}
			break;
		case 2 :
			if(parseInt(ps.Level) >= 0){
				HoleInt = 10;
				if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
					AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsEquepmentHole , myEBI.inv.itemHole , 0 , "" , gameObject , "EquepmentHoleTips");
//					AllManage.qrStatic.ShowBuyQueRen1(this.gameObject , "YesDaKong" , "NoDaKong" , AllManage.AllMge.Loc.Get("info299")+""+ HoleInt +AllManage.AllMge.Loc.Get("info301")+"");	
				else
					YesDaKong();
			}else{
				AllManage.tsStatic.Show("tips017");
			}
			break;
		case 3 :
			if(parseInt(ps.Level) >= 0){
				HoleInt = 20;
				if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
					AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsEquepmentHole , myEBI.inv.itemHole , 0 , "" , gameObject , "EquepmentHoleTips");
//					AllManage.qrStatic.ShowBuyQueRen1(this.gameObject , "YesDaKong" , "NoDaKong" ,AllManage.AllMge.Loc.Get("info299")+ ""+ HoleInt +AllManage.AllMge.Loc.Get("info301")+"");	
				else
					YesDaKong();
			}else{
				AllManage.tsStatic.Show("tips017");
			}
			break;
	}
}

	function EquepmentHoleTips(objs : Object[]){
		AllManage.qrStatic.ShowBuyQueRen1(this.gameObject , "YesDaKong" , "NoDaKong" ,AllManage.AllMge.Loc.Get("info299")+ ""+ objs[2] +AllManage.AllMge.Loc.Get("info301")+"");	
	}

function NoDaKong(){

}

private var ps : PlayerStatus; 

function YesDaKong(){
	var i : int = 0;
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	for(i=0; i<useInvID.length; i++){
		if(useInvID[i] == myEBI.inv.itemID){  
//			var int1 : int =  myEBI.inv.itemLevel;
//			var int2 : int =  myEBI.inv.itemQuality;
//			if(ps.UseMoney( 0 , HoleInt )){
//				DoDaKong(useInvID[i] , i);		
//			}
//			AllManage.tsStatic.RefreshBaffleOn();
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().EquepmentHole(useInvID[i] , i , myEBI.inv.itemHole ));
//			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().EquepmentHole(useInvID[i] , i , 0 , HoleInt * (-1)));
			beforeID = i;
			break;
		}
	}
}


private var beforeID : int = 0;
function DoDaKong(invID : String , id : int){
	
//	//print("da kong qu" + invID + " == " + id);
	var m : int = Random.Range(-3,3) + 6;
	var useInt : int;
	var useStr : String;
	useInt = parseInt(invID.Substring(18,1)) + 1;
	if(useInt < 2)
	useInt = 2;
	useStr = useInt.ToString();

//	//print(useInvID[id] + " == " + m);
	useInvID[id] = useInvID[id].Substring(0,18) + useStr + useInvID[id].Substring(19,6); 
//	//print(useInvID[id] + " ==");
///////
	InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText = "";
//	//print(useInvID.length);
	for(var i=0; i<useInvID.length; i++){	 
		if(i < 12){
			InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText += useInvID[i] + ";";
		}else
		if(i < 27 && i >= 12){
			InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 42 && i >=27){
			InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 57 && i >= 42){
			InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 72 && i >= 57){
			InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText += useInvID[i] + ";";		
		}
	}  
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
				inv5 = InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText;
}

function RefreshList(){
	//print("sdlfjklsdjflsdkfljk zhi xing le");
	SetEqupmentList("");
	invcl.isUpdatePhoton = false;
	invcl.ReInitItem1();
	SelectBeforeInv(useInvID[beforeID]);
}

function DoDaZao(){
	var i : int = 0;
	for(i=0; i<invItemArray.length; i++){
		if(useInvID[i] == myEBI.inv.itemID){
			BuildOneID(useInvID[i] , i);
		}
	}
}

var invcl : InventoryControl;
function BuildOneID(invID : String , id : int){
//	//print("zhe li la a ");
	var m : int = Random.Range(-3,3) + 6;
	var useInt : int;
	var useStr : String;
	useInt = parseInt(invID.Substring(15,3)) + m;
	useStr = useInt.ToString();
	if(useStr.length == 1){
		useStr = "00" + useStr;
	}else
	if(useStr.length == 2){
		useStr = "0" + useStr;		
	}
//	//print(useInvID[id] + " == " + m);
	useInvID[id] = useInvID[id].Substring(0,15) + useStr + useInvID[id].Substring(18,7); 
//	//print(useInvID[id] + " ==");   
	
//	var useInvID : String[];
//	useInvID = Inventorys.Split(Fstr.ToCharArray());
//	inv1 = "";
//	inv2 = "";
//	inv3 = "";
//	inv4 = "";
//	inv5 = "";
	InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText = "";
//	//print(useInvID.length);
	for(var i=0; i<useInvID.length; i++){	 
		if(i < 12){
			InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText += useInvID[i] + ";";
		}else
		if(i < 27 && i >= 12){
			InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 42 && i >=27){
			InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 57 && i >= 42){
			InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 72 && i >= 57){
			InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText += useInvID[i] + ";";		
		}
	}  
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
				inv5 = InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText;
	SetEqupmentList("");
	invcl.isUpdatePhoton = false;
	invcl.ReInitItem1();
	SelectBeforeInv(useInvID[id]);
}
