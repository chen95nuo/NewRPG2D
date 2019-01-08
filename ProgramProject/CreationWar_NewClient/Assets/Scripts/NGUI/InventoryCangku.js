#pragma strict
//var LT : LootTable;
var inv : InventoryItem[];
var ProfessionLabel : UILabel;
var yuanPhoton : YuanUnityPhoton; 
var transCangku : Transform;
//var invMaker : Inventorymaker;
private var Fstr : String = ";";
var Inventory1 : String;
var Inventory2 : String;
var Inventory3 : String;
var Inventory4 : String; 
private var EquipItemStr : String;

var gm : GameObject;
var btnsd : GameObject;
var ServerAddress : String;
var ServerApplication : String;
var PlayerBankInventoryNum : int;

function Awake(){
	AllManage.invCangKuStatic = this;
	AllManage.UICLStatic.PosStores[0] = transCangku.position.x;
	newInvArray = new Array(15);
	AllManage.buttonMessCL.allTarget[4] = gameObject;
}

function Start () {
	var mm : boolean = false;
//	//print(mm);
//	//print(InventoryControl.Plys);
//	//print(InventoryControl.ytLoad);
//	//print(InventoryControl.yt.Rows.Count);
	while(!mm || InventoryControl.Plys == null){
		if(InventoryControl.ytLoad){
			if(InventoryControl.yt.Rows.Count > 0){			
				Inventory1 = InventoryControl.yt.Rows[0]["BankInventory1"].YuanColumnText;
				Inventory2 = InventoryControl.yt.Rows[0]["BankInventory2"].YuanColumnText;
				Inventory3 = InventoryControl.yt.Rows[0]["BankInventory3"].YuanColumnText;
				Inventory4 = InventoryControl.yt.Rows[0]["BankInventory4"].YuanColumnText;
//				//print(Inventory1);
//				//print(Inventory2);
//				//print(Inventory3);
//				//print(Inventory4);
				PlayerBankInventoryNum = GetBDInfoInt("BankInventoryNum" , 1);
				mm = true;
				break;
			}
		}
		yield;
	}
//	Inventory1 = "8401,20;8401,20;8401,20;8201,01;8201,01;;;;;;;;;;;";
		yield;
		yield;
			if(GetVIPBagNum() > PlayerBankInventoryNum){
				PlayerBankInventoryNum = GetVIPBagNum();
			}
		SetPlayerInventoryNum(PlayerBankInventoryNum);
		SetSelectBagItem(Inventory1);
		useInv1Array = null;
		useInv87Array = null;
		useInv6Array = null;
}

function GetVIPBagNum() : int{
	try{
		return parseInt(AllManage.InvclStatic.VIpTable.SelectRowEqual("VIPType" , GetBDInfoInt("Serving" , 0).ToString())["InventoryUpdate"].YuanColumnText.Split(Fstr.ToCharArray())[1]);
	}catch(e){
		return 1;
	}
}

function cangkuBag(){
	PlayerBankInventoryNum = GetBDInfoInt("BankInventoryNum" , 1);
			if(GetVIPBagNum() > PlayerBankInventoryNum){
				PlayerBankInventoryNum = GetVIPBagNum();
			}
//	print(PlayerBankInventoryNum + " == PlayerBankInventoryNum");
	SetPlayerInventoryNum(PlayerBankInventoryNum);
}

function GetBagStrAsLevel(level : int , sss : String) : String{
	switch(level){
		case 1 : sss = Inventory1; break;
		case 2 : sss = Inventory1 + Inventory2; break;
		case 3 : sss = Inventory1 + Inventory2 + Inventory3; break;
		case 4 : sss = Inventory1 + Inventory2 + Inventory3 + Inventory4; break;			
	}
	return sss;
}

function GetBDInfoInt(bd : String , it : int) : int{  
	var iii : int = 0;
	try{ 
		iii = parseInt(InventoryControl.yt.Rows[0][bd].YuanColumnText);
		return  iii;
	}catch(e){
		return it;	
	}
}

var ButtonsBank : UISprite[];
function SetPlayerInventoryNum(bank : int){
	var i=0;
	for(i=0; i<ButtonsBank.length; i++){
		if(i < bank){
			ButtonsBank[i].spriteName = "UIH_Backpack_A";
		}else{
			ButtonsBank[i].spriteName = "UIH_Backpack_O";
		}
	}
}

private var str : String;
private var ptime : int = -300;

var EquipIt : BagItem[];
function SetEquipItem(equStr : String){
	var i : int = 0;
	var useInv : InventoryItem;
	var useInvID : String[];
	useInvID = equStr.Split(Fstr.ToCharArray());
	if(useInvID[i].Length < 1){
		return;
	}
	for(i=0; i<EquipIt.length; i++){	 
		if(useInvID[i] != ""){ 
			useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
			EquipIt[i].SetInv(useInv);
		}
	}
}

function ReInitItem(){
	ClearBagItem();
				Inventory1 = InventoryControl.yt.Rows[0]["BankInventory1"].YuanColumnText;
				Inventory2 = InventoryControl.yt.Rows[0]["BankInventory2"].YuanColumnText;
				Inventory3 = InventoryControl.yt.Rows[0]["BankInventory3"].YuanColumnText;
				Inventory4 = InventoryControl.yt.Rows[0]["BankInventory4"].YuanColumnText;

	SetSelectBagItem(Inventory1);
}

var SpriteQieHuanBag : UISlicedSprite[];
var TransQieHuanBag : Transform[];
function QieHuanBagBaoguo(){
	TransQieHuanBag[0].localPosition.y = 0;
	TransQieHuanBag[1].localPosition.y = 1000;
	SpriteQieHuanBag[0].spriteName = "UIB_Tab_A";
	SpriteQieHuanBag[1].spriteName = "UIB_Tab_N";
}
function QieHuanBagDaoju(){
	TransQieHuanBag[0].localPosition.y = 1000;
	TransQieHuanBag[1].localPosition.y = 0;
	SpriteQieHuanBag[0].spriteName = "UIB_Tab_N";
	SpriteQieHuanBag[1].spriteName = "UIB_Tab_A";
}

var TransQieHuanCangku : Transform; 
var TransQieHuanParentEquep : Transform;
var isCangku : boolean = false;
var isShangdian : boolean = false;
function QieHuanCangku(){
	if(!isCangku){
		isCangku = true;
		TransQieHuanCangku.localPosition.y = 0; 
		TransQieHuanParentEquep.localPosition.y = 1000; 
	}else{
		isCangku = false;
		TransQieHuanCangku.localPosition.y = 1000; 
		TransQieHuanParentEquep.localPosition.y = 0; 
	}
}

var SpriteQieHuanEquep : UISlicedSprite[];
var TransQieHuanEquep : Transform[];
function QieHuanEquepZhuangBei(){
	TransQieHuanEquep[0].localPosition.y = 0;
	TransQieHuanEquep[1].localPosition.y = 1000;
	SpriteQieHuanEquep[0].spriteName = "UIB_Tab_A";
	SpriteQieHuanEquep[1].spriteName = "UIB_Tab_N";
}
function QieHuanEquepShuXing(){
	TransQieHuanEquep[0].localPosition.y = 1000;
	TransQieHuanEquep[1].localPosition.y = 0;
	SpriteQieHuanEquep[0].spriteName = "UIB_Tab_N";
	SpriteQieHuanEquep[1].spriteName = "UIB_Tab_A";
	ShowEquepShuXing();
}

var LabelEquepShuXing : UILabel[];
function ShowEquepShuXing(){
	var i : int = 0;
	for(i=0; i<LabelEquepShuXing.length; i++){
		if(LabelEquepShuXing[i] != null){	
			LabelEquepShuXing[i].text = EquipStatus[i].ToString();
		}
	}
}

function UpdateEquipItem(){
}
var OpenBagMoney : int;
//var qr : QueRen;
function OpenBagNum(){
	OpenBagMoney = PlayerBankInventoryNum*10;
	if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1){
		AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.YesOpenCangKu ,  GetBDInfoInt("BankInventoryNum" , 1) , 0 , "" , gameObject , "NoOpenTips");
//		AllManage.qrStatic.ShowBuyQueRen1(gameObject , "YesOpen" , "NoOpen" , AllManage.AllMge.Loc.Get("info298")+ "" +OpenBagMoney + AllManage.AllMge.Loc.Get("info302")+ "");
	}else{
		YesOpen();
	}
}

	function NoOpenTips(objs : Object[]){
		AllManage.qrStatic.ShowBuyQueRen1(gameObject , "YesOpen" , "NoOpen" , AllManage.AllMge.Loc.Get("info298")+ "" +OpenBagMoney + AllManage.AllMge.Loc.Get("info302")+ "");
	}

function YesOpen(){
 	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesOpenCangKu , GetBDInfoInt("BankInventoryNum" , 1) , 0 , "" , gameObject , "realYesOpenCangKu");
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.OpenBankInv).ToString());
//	AllManage.AllMge.UseMoney(0 , GetBDInfoInt("BankInventoryNum" , 1)*10 , UseMoneyType.YesOpenCangKu , gameObject , "realYesOpenCangKu");
//	if(ps.UseMoney(0 , OpenBagMoney)){
}

function realYesOpenCangKu(){
	PlayerBankInventoryNum += 1;
	InventoryControl.yt.Rows[0]["BankInventoryNum"].YuanColumnText = PlayerBankInventoryNum.ToString();
			if(GetVIPBagNum() > PlayerBankInventoryNum){
				PlayerBankInventoryNum = GetVIPBagNum();
			}
	SetPlayerInventoryNum(PlayerBankInventoryNum);
}

function NoOpen(){

}

var BagID : int = 1;
function SelectBag1(){
	BagID = 1; 
	ClearBagItem();
	SetSelectBagItem(Inventory1);
	ShowBagButton(BagID);
} 
function SelectBag2(){
	if(PlayerBankInventoryNum >= 2){
		BagID = 2;
		ClearBagItem();
		SetSelectBagItem(Inventory2); 
	ShowBagButton(BagID);
	//	//print(Inventory2 + " == ");
	}else{
		OpenBagNum();
	}
} 
function SelectBag3(){
	if(PlayerBankInventoryNum >= 3){
		BagID = 3;
		ClearBagItem();
		SetSelectBagItem(Inventory3);
	ShowBagButton(BagID);
	}else{
		OpenBagNum();
	}
}
function SelectBag4(){
	if(PlayerBankInventoryNum >= 4){
		BagID = 4;
		ClearBagItem();
		SetSelectBagItem(Inventory4);
	ShowBagButton(BagID);
	}else{
		OpenBagNum();
	}
} 

function ClearBagItem(){
	for(var i=0; i<BagIt.length; i++){
		BagIt[i].invClear();
	}
}

var SelectBagGuangs : UISprite[];
function ShowBagButton(id : int){
	for(var i=0; i<SelectBagGuangs.length; i++){
		if(i == id){
			SelectBagGuangs[i].enabled = true;
		}else{
			SelectBagGuangs[i].enabled = false;
		}
	}
}

var useShangDianInvs : InventoryItem[];
function SetShangDIanItem(){
	if(useShangDianInvs == null)
	useShangDianInvs = new Array(9);
	isCangku = false;
	ClearBagItem();
	var i : int = 0;
	var useInv : InventoryItem;
	inv = new Array(9);
	DontUpdate = true;
	for(i=0; i<15; i++){	 
		BagIt[i].myType = SlotType.Shangdian;
		BagIt[i].ColseWen();
	}
		var invStr : String; 
	if(Time.time > ptime || useShangDianInvs == null){
		ptime = Time.time + 300;
		for(i=0; i<9; i++){	 
			invStr = AllResources.staticLT.MakeItemID1(invStr, Random.Range(1,3)); 
			inv[i] = AllResources.InvmakerStatic.GetItemInfo(invStr, inv[i] );
			useShangDianInvs[i] = inv[i];
	//		inv[i] = LT.NewLootContent()[0]; 
			inv[i].costGold = ToInt.IntToStr(ToInt.StrToInt(inv[i].costGold) * 2);
			inv[i].costBlood = ToInt.IntToStr(ToInt.StrToInt(inv[i].costBlood) * 2);
			BagIt[i].SetInv(inv[i]);
		}
	}else{
		for(i=0; i<9; i++){	 
			BagIt[i].SetInv(useShangDianInvs[i]);
		}
	}
}

var useInv1Array : InventoryItem[];
var useInv87Array : InventoryItem[];
var useInv6Array : InventoryItem[];
var newInvArray : InventoryItem[];

function returnGetStoreList9(mystrs : String[]){
	var strs : String[] = new String[9];
	var i : int = 0;
	for(i=0; i<9; i++){
		strs[i] = mystrs[i];
	}
//	//print(strs.length);
	for(i=0; i<9; i++){
//		//print(strs[i]);
		newInvArray[i] = AllResources.InvmakerStatic.GetItemInfo(strs[i], newInvArray[i] );	
		if(newInvArray[i]){
			if(newInvArray[i].slotType == SlotType.Formula){
				var InvFormula : InventoryItem;
				InvFormula = new InventoryItem();
				InvFormula = AllResources.InvmakerStatic.GetItemInfo(newInvArray[i].itemID.Substring(1,25) , InvFormula); 			
				newInvArray[i].costGold = InvFormula.costGold;
				newInvArray[i].costBlood = ToInt.IntToStr(ToInt.StrToInt(InvFormula.costBlood) * 0.25); 
			}
			BagIt[i].SetInv(newInvArray[i]);			
		}else{
			BagIt[i].OtherYiChu();
		}
	}
	
	switch(nowSelectStoreType){
		case yuan.YuanPhoton.StoreType.Blacksmith : 
			copyArrayItem(ArrayBlacksmith , 9);
			break;
		case yuan.YuanPhoton.StoreType.GroceryStore : 
			copyArrayItem(ArrayGroceryStore , 9);
			break;
		case yuan.YuanPhoton.StoreType.GuildStore : 
			copyArrayItem(ArrayGuildStore , 9);
			break;
		case yuan.YuanPhoton.StoreType.HonorStore : 
			copyArrayItem(ArrayHonorStore , 9);
			break;
		case yuan.YuanPhoton.StoreType.PVPStore : 
			copyArrayItem(ArrayPVPStore , 9);
			break;
		case yuan.YuanPhoton.StoreType.RandomStore : 
			copyArrayItem(ArrayRandomStore , 9);
			break;
	}
}

function copyArrayItem(useArray : String[] , num : int){
	var i : int = 0;
	for(i=0; i<9; i++){
		if(newInvArray[i]){
			useArray[i] = newInvArray[i].itemID;		
		}else{
			useArray[i] = "";
		}
	}
//	if(num == 12){
//		for(i=12; i<15; i++){
//			useArray[i] = strList3[i - 12];
//		}
//	}
}

private var strList3 : String[];
function returnGetStoreList3(strs : String[]){
	strList3 = strs;
	for(var i=12; i<15; i++){
		newInvArray[i] = AllResources.InvmakerStatic.GetItemInfo(strs[i - 12], newInvArray[i]);			
		newInvArray[i].costGold = ToInt.IntToStr(ToInt.StrToInt(newInvArray[i].costGold) * 2);
		newInvArray[i].costBlood = ToInt.IntToStr(ToInt.StrToInt(newInvArray[i].costBlood) * 2);
		BagIt[i].SetInv(newInvArray[i]);	
	}
}

var nowSelectStoreType : yuan.YuanPhoton.StoreType;

var ArrayBlacksmith : String[] = new String[15];
var ArrayGroceryStore : String[] = new String[15];
var ArrayGuildStore : String[] = new String[15];
var ArrayHonorStore : String[] = new String[15];
var ArrayPVPStore : String[] = new String[15];
var ArrayRandomStore : String[] = new String[15];
private var rTimeBlacksmith : int = 0;
private var rTimeGroceryStore : int = 0;
private var rTimeGuildStore : int = 0;
private var rTimeHonorStore : int = 0;
private var rTimePVPStore : int = 0;
private var rTimeRandomStore : int = 0;
function SetShangDIanItemAsType(tp : int){
	isCangku = false;
	ClearBagItem();
	if(! (AllManage.jiaochengCLStatic.JiaoChengID == 5 && AllManage.jiaochengCLStatic.step == 0)){
		var i : int = 0;
		var bool : boolean = true;
		for(i=0; i<15; i++){
			BagIt[i].myType = SlotType.Shangdian;
			BagIt[i].ColseWen();
		}
		
		var thisType : yuan.YuanPhoton.StoreType;
		if(tp < 7){
			thisType = yuan.YuanPhoton.StoreType.Blacksmith;
//			if(Time.time > rTimeBlacksmith){
//				rTimeBlacksmith = Time.time + 180;
//				bool = true;
//			}
		}else
		if(tp == 87){
			thisType = yuan.YuanPhoton.StoreType.GroceryStore;
//			if(Time.time > rTimeGroceryStore){
//				rTimeGroceryStore = Time.time + 180;
//				bool = true;
//			}
		}else
		if(tp == 81){
			thisType = yuan.YuanPhoton.StoreType.GuildStore;
//			if(Time.time > rTimeGuildStore){
//				rTimeGuildStore = Time.time + 180;
//				bool = true;
//			}
		}else
		if(tp == 9){
			thisType = yuan.YuanPhoton.StoreType.HonorStore;
//			if(Time.time > rTimeHonorStore){
//				rTimeHonorStore = Time.time + 180;
//				bool = true;
//			}
		}else
		if(tp == 7){
			thisType = yuan.YuanPhoton.StoreType.PVPStore;
//			if(Time.time > rTimePVPStore){
//				rTimePVPStore = Time.time + 180;
//				bool = true;
//			}
		}else
		if(tp == 0){
			thisType = yuan.YuanPhoton.StoreType.RandomStore;
//			if(Time.time > rTimeRandomStore){
//				rTimeRandomStore = Time.time + 180;
//				bool = true;
//			}
		}
		nowSelectStoreType = thisType;
		if(bool){
//			AllManage.tsStatic.RefreshBaffleOn();
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().GetStoreList(thisType , DungeonControl.level));
		}else{
			switch(nowSelectStoreType){
				case yuan.YuanPhoton.StoreType.Blacksmith : 
					returnGetStoreList9(ArrayBlacksmith);
					break;
				case yuan.YuanPhoton.StoreType.GroceryStore : 
					returnGetStoreList9(ArrayGroceryStore);
					break;
				case yuan.YuanPhoton.StoreType.GuildStore : 
					returnGetStoreList9(ArrayGuildStore);
					break;
				case yuan.YuanPhoton.StoreType.HonorStore : 
					returnGetStoreList9(ArrayHonorStore);
					break;
				case yuan.YuanPhoton.StoreType.PVPStore : 
					returnGetStoreList9(ArrayPVPStore);
					break;
				case yuan.YuanPhoton.StoreType.RandomStore : 
					returnGetStoreList9(ArrayRandomStore);
					returnGetStoreList3(strList3);
					break;
			}
		}
		return;
	}
	var useInv : InventoryItem;
	inv = new Array(15);
	DontUpdate = true;
	for(i=0; i<15; i++){	 
		BagIt[i].myType = SlotType.Shangdian;
		BagIt[i].ColseWen();
	}
	var invStr : String; 
	var invStrArray : String[];
	var isShua : boolean = false;
	if(tp == 87){
		if(Time.time > ptime +300 || useInv87Array == null){
			useInv87Array = new Array(9);
			isShua = true;
			ptime = Time.time;
		}		
	}else
	if(tp == 6){
		if(Time.time > ptime +300 || useInv6Array == null){
			useInv6Array = new Array(9);
			isShua = true;
			ptime = Time.time;
		}			
	}else
	if(tp == 9){
	
	}else
	if(Time.time > ptime +300 || useInv1Array == null){
			useInv1Array = new Array(9);
			isShua = true;
			ptime = Time.time;
	}

	invStrArray = GetRandomPeiFang();
	for(i=0; i<9; i++){	 
		if(tp == 9){
			if(invStrArray.length > 0){
				invStr = invStrArray[Random.Range(0,invStrArray.Length)];
				inv[i] = AllResources.InvmakerStatic.GetItemInfo(invStr, inv[i] );
				inv[i].itemName = GetPeiFangNameAsID(invStr); 
				var InvFormula : InventoryItem;
				InvFormula = new InventoryItem();
				InvFormula = AllResources.InvmakerStatic.GetItemInfo(inv[i].itemID.Substring(1,25) , InvFormula); 
				inv[i].costGold = InvFormula.costGold;
				inv[i].costBlood =  ToInt.IntToStr(ToInt.StrToInt(InvFormula.costBlood) * 0.25); 
			}
		}else
		if(tp == 0){
			invStr = AllResources.staticLT.MakeItemID2(invStr, 0 , tp); 
			inv[i] = AllResources.InvmakerStatic.GetItemInfo(invStr, inv[i] );			
		}else
		if(tp == 6){
			if(isShua){
				invStr = AllResources.staticLT.MakeItemID2(invStr, Random.Range(1,3) , tp); 
				inv[i] = AllResources.InvmakerStatic.GetItemInfo(invStr, inv[i] );
				useInv6Array[i] = inv[i];
			}else{
				inv[i] = useInv6Array[i];
			}
		}else{
			if(tp == 87){
				if(isShua){
					invStr = AllResources.staticLT.MakeItemID2(invStr, Random.Range(1,3) , tp); 
					inv[i] = AllResources.InvmakerStatic.GetItemInfo(invStr, inv[i] );
					useInv87Array[i] = inv[i];
				}else{
					inv[i] = useInv87Array[i];
				}
			}else{
				if(isShua){
					invStr = AllResources.staticLT.MakeItemID2(invStr, Random.Range(1,3) , tp); 
					inv[i] = AllResources.InvmakerStatic.GetItemInfo(invStr, inv[i] );
					useInv1Array[i] = inv[i];
				}else{
					inv[i] = AllResources.InvmakerStatic.GetItemInfo(useInv1Array[i].itemID , inv[i] ); 
				}
			}
		}
		if(inv[i] != null){
			inv[i].costGold = ToInt.IntToStr(ToInt.StrToInt(inv[i].costGold) * 2);
			inv[i].costBlood = ToInt.IntToStr(ToInt.StrToInt(inv[i].costBlood) * 2);
	//		inv[i] = LT.NewLootContent1(tp)[0]; 
			BagIt[i].SetInv(inv[i]);
		}
	}
	for(i=12; i<15; i++){
		invStr = AllResources.staticLT.MakeItemID2(invStr, 0 , 0); 
		inv[i] = AllResources.InvmakerStatic.GetItemInfo(invStr, inv[i] );			
		inv[i].costGold = ToInt.IntToStr(ToInt.StrToInt(inv[i].costGold) * 2);
		inv[i].costBlood = ToInt.IntToStr(ToInt.StrToInt(inv[i].costBlood) * 2);
//		inv[i] = LT.NewLootContent1(tp)[0]; 
		BagIt[i].SetInv(inv[i]);	
	}
}

private var ps : PlayerStatus;
function GetRandomPeiFang() : String[]{
//	if(ps == null && PlayerStatus.MainCharacter){
//		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
//	}
	var Level : String = "";
	var ran1 : int;
	ran1 = Random.Range(0,100);
	////print(ran1 + " == ran1");
	if(ran1 < 35){
		Level = "20";
	}else
	if(ran1 < 55){
		Level = "40";	
	}else
	if(ran1 < 75){
		Level = "60";		
	}else{
		Level = "80";
	}
	var PFarray : Array = new Array();
	var PFstr : String[];
	var ran2 : int;
	ran2 = Random.Range(1 , 4);
//	//print(ran2 + " == ran2");
	for(var rows : yuan.YuanMemoryDB.YuanRow in InventoryControl.Blueprint.Rows){
		if(rows["BlueprintID"].YuanColumnText.Substring(3,2) == Level && (rows["BlueprintID"].YuanColumnText.Substring(1,1) == ran2.ToString() || rows["BlueprintID"].YuanColumnText.Substring(1,1) == (ran2 + 3).ToString())){
			PFarray.Add(rows["BlueprintID"].YuanColumnText);
		}
	}
	PFstr = PFarray;
	return PFstr;
}

function GetPeiFangNameAsID(id : String) : String{
	var name : String = "未知装备";
	for(var rows : yuan.YuanMemoryDB.YuanRow in InventoryControl.Blueprint.Rows){
		if(rows["BlueprintID"].YuanColumnText.Substring(0,8) == id.Substring(0,8)){
			name = rows["BlueprintName"].YuanColumnText;
		}
	}
	return name;
}

//
//function GetNPCIDAsID(id : String) : String{
////	//print(id + " ===== id ");
//	for(var rows : yuan.YuanMemoryDB.YuanRow in NPCInfo.Rows){
//		if(rows["id"].YuanColumnText == id){
//			return rows["NPCID"].YuanColumnText;
//		}
//	}
//	return "";
//}

function SetSelectBagItem(invID : String){
	var i : int = 0;
	var useInv : InventoryItem;
	var useInvID : String[];
	useInvID = invID.Split(Fstr.ToCharArray());
	inv = new Array(9);
//	//print(invID);
	DontUpdate = true;
//	for(i=0; i<9; i++){	 
//		inv[i] = LT.NewLootContent()[0]; 
//		BagIt[i].SetInv(inv[i]);
	for(i=0; i<15; i++){	 
		BagIt[i].myType = SlotType.Cangku;
		BagIt[i].ColseWen();
	}
	for(i=0; i<useInvID.length; i++){	 
		if(useInvID[i] != ""){ 
			useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
			BagIt[i].SetInv(useInv);
//			AddBagItem(useInv); 
		}
	}
	UpdateBagItem();
}
function GoShowWeapon(){

}
var BagIt : BagItem[];
function AddBagItem(inv : InventoryItem){
	var i : int = 0; 
	var j : int = 0;
	if(inv.slotType < 12){	
		for(i=0; i<BagIt.length; i++){ 
			if(BagIt[i].inv == null){
				BagIt[i].SetInv(inv);
				UpdateBagItem();
				return;
			}
		}
	}else{
		for(i=0; i<BagIt.length; i++){
			if(BagIt[i].inv){
				if(BagIt[i].inv.itemID.Length > 5 && BagIt[i].inv.consumablesNum != 0){
					if(BagIt[i].inv.itemID.Substring(0,4) == inv.itemID.Substring(0,4)){
						if(BagIt[i].inv.consumablesNum + inv.consumablesNum <= 20){				
							BagIt[i].inv.consumablesNum += inv.consumablesNum;
							UpdateBagItem();
							return;
						}else{
							for(j=0; j<BagIt.length; j++){
								if(BagIt[j].inv == null){ 
									inv.consumablesNum = BagIt[i].inv.consumablesNum + inv.consumablesNum - 20;
									BagIt[j].SetInv(inv);
									UpdateBagItem();
									return;					
								}
							}
							BagIt[i].inv.consumablesNum =20; 
							UpdateBagItem();
							return;					
						}
					}	
				}
			}

		}
		for(i=0; i<BagIt.length; i++){ 
			if(BagIt[i].inv == null){  
//				//print("jin dao zhe li le " + inv.itemID);
				 BagIt[i].SetInv(inv);
				BagIt[i].showConsumablesNum();
				UpdateBagItem();
//				//print("jin dao zhe li le444");
				return;					
			}
		}
	}
}

private var yuseInventoryBagID : String;
private var DontUpdate : boolean = false;
var invcl : InventoryControl;
function UpdateBagItem(){
//	//print(invcl.isCangku);
	if(!AllManage.InvclStatic.isCangku){
		return;
	}
	yuseInventoryBagID = "";  
//	//print(DontUpdate);
	if(DontUpdate){	
		yuseInventoryBagID = GetSelectBagID(BagID); 
	}
	
	var i : int = 0;
	var str : String;
	if(!DontUpdate){
//		//print("gengxin le");
		for(i=0; i<BagIt.length; i++){
			if(BagIt[i].inv != null)
				str = BagIt[i].inv.itemID;
			else
				str = "";
			yuseInventoryBagID += (str + ";");  
		}
//		//print("= +" + yuseInventoryBagID );
		InventoryControl.yt.Rows[0]["BankInventory" + BagID.ToString()].YuanColumnText = yuseInventoryBagID;
//		//print(InventoryControl.yt.Rows[0]["BankInventory" + BagID.ToString()].YuanColumnText);
	} 
//	//print(yuseInventoryBagID); 
//	//print("Inventory" + BagID.ToString()); 
	switch(BagID){
		case 1:  Inventory1 = yuseInventoryBagID; break;
		case 2:  Inventory2 = yuseInventoryBagID; break;
		case 3:  Inventory3 = yuseInventoryBagID; break;
		case 4:  Inventory4 = yuseInventoryBagID; break;
	}
	DontUpdate = false;
}

function GetSelectBagID(id : int) : String{	
	switch(id){
		case 1: return Inventory1;
		case 2: return Inventory2;
		case 3: return Inventory3;
		case 4: return Inventory4;
	}
}

var TPWeapon : ThirdPersonWeapon;
function GoShowWeapon(inv : InventoryItem , type : SlotType){
	if(TPWeapon == null){
		return;
	}
	if(type == SlotType.Bag){
		if(inv.itemmodle1){
			inv.itemmodle1.transform.parent = null;
			inv.itemmodle1.SetActiveRecursively(false);
		}
		if(inv.itemmodle2){
			inv.itemmodle2.transform.parent = null;
			inv.itemmodle2.SetActiveRecursively(false);
		}
	}else{
//		TPWeapon.ShowWeapon(inv);
//		SetPersonEquipment(inv);
	}
	GetPersonEquipment();
}

function SetPersonEquipment(inv : InventoryItem){
	var i : int = 0;
	for(i=0; i<PES.length; i++){
		if(PES[i].invType == inv.slotType){
			PES[i].inv = inv;
		}
	}
}

var PES : PersonEquipment[];
function GetPersonEquipment(){
	var i : int = 0;
	for(i=0; i<EquipStatus.length; i++){
		EquipStatus[i] = 0;
	}
	
	for(i=0; i<PES.length; i++){
		if(PES[i] != null){		
			if(PES[i].inv.itemID != ""){
				GetEquipStatus(PES[i]);			
			}
		}
	}
}

//enum SlotType 	{Helmet=1,Breastplate=2,Spaulders=3,Gauntlets=4,Leggings=5,Rear=6,Ring=7,Collar=8,Belt=9,Weapon1=10,Weapon2=11,Hand, Chest, Wrist,Expendable,Empty,Bag}
//pes shuxing		attr xiu zheng			item xiu zheng
//0 : gongji    	0:nai li				0:toukui
//1 : hujia			1:li liang				1:xiongjia
//2 : fangyu		2:min jie				2:jianjia
//3 : zhunque		3:zhi li				3:hushou
//4 : baoji			4:bao ji				4:tuijia
//5 : naili			5:zhun que				5:houbei
//6 : liliang		6:fang yu zhi			6:jiezhi
//7 : minjie		7:mo fa kang xing		7:bozi
//8 : zhili			8:zui da mo fa			8:yaodai
//9 : huifusudu		9:hui fu neng li		9:wuqi1
//10 : Matk		10:e wai shanghai		
//11 : Mdef 		
//12 : Mana			

var EquipStatus : int[];
var attrXiuZheng : float[];
var itemXiuZheng : float[];
var usePes : PersonEquipment;
function GetEquipStatus(pes : PersonEquipment){
	usePes = pes;
	var itemType : SlotType = pes.invType;
	var pfType : ProfessionType = pes.inv.professionType;
	var level : int = pes.inv.itemLevel;
	var quality : int = pes.inv.itemQuality;
	var endurance : int = pes.inv.itemEndurance;
	var proAbt : int = pes.inv.itemProAbt;
	var abt1 : String = pes.inv.itemAbt1;
	var abt2 : String = pes.inv.itemAbt2;
	var abt3 : String = pes.inv.itemAbt3;
	var hAttr1 : HoleAttr = pes.inv.holeAttr1;
	var hAttr2 : HoleAttr = pes.inv.holeAttr2;
	var hAttr3 : HoleAttr = pes.inv.holeAttr3;
	
	var pinzhi : int = 0;
	pinzhi = getQuality(quality , level);
	if(itemType == SlotType.Weapon1 || itemType == SlotType.Weapon2){
		if(itemType == SlotType.Weapon1 && pfType == ProfessionType.Soldier ){
			EquipStatus[0] += 0.56 * pinzhi + 40;
			EquipStatus[1] += (0.6*pinzhi + 40)*1.2;		
		}else{
			EquipStatus[0] += 0.68 * pinzhi + 56;			
		}
	}else{
		if(pfType == ProfessionType.Master){
			EquipStatus[1] += (0.4 * pinzhi + 12) *itemXiuZheng[itemType];	
		}else
		if(pfType == ProfessionType.Robber){
			EquipStatus[1] += (0.6 * pinzhi + 26) *itemXiuZheng[itemType];	
		}else
		if(pfType == ProfessionType.Soldier){
			EquipStatus[1] += (0.8 * pinzhi + 40) *itemXiuZheng[itemType];	
		}
	}
	
	EquipStatus[5] = pinzhi * endurance / 10 / 0.66 * itemXiuZheng[itemType];
	switch(pfType){
		case ProfessionType.Soldier : 		
			EquipStatus[6] = pinzhi * endurance / 10 / 0.66 * itemXiuZheng[itemType];  break;
		case ProfessionType.Robber : 
			EquipStatus[7] = pinzhi * endurance / 10 / 0.66 * itemXiuZheng[itemType];  break;
		case ProfessionType.Master : 
			EquipStatus[8] = pinzhi * endurance / 10 / 0.66 * itemXiuZheng[itemType];  break;
	}
	getAtb(abt1 , pinzhi , itemType , pfType);
	getAtb(abt2 , pinzhi , itemType , pfType);
	getAtb(abt3 , pinzhi , itemType , pfType); 
	getHole(hAttr1 , pes.inv.itemHole1);
	getHole(hAttr2 , pes.inv.itemHole2);
	getHole(hAttr3 , pes.inv.itemHole3);
}

function getHole(hAttr : HoleAttr , hole : String){ 
	if(hole == "00"){
		return;
	}
	var holeValue : int = 0;
	for(var rows : yuan.YuanMemoryDB.YuanRow in InventoryControl.GameItem.Rows){
		if(rows["ItemID"].YuanColumnText == "81" + hAttr.hType + "" +  hAttr.hValue){
			holeValue = parseInt(rows["ItemValue"].YuanColumnText);
		}
	}
	switch(hAttr.hType){
		case holeType.atk :
			 EquipStatus[0] += holeValue; break;
		case holeType.zhuanzhu :
			 EquipStatus[3] += holeValue;  break;
		case holeType.baoji :
			 EquipStatus[4] += holeValue;  break;
		case holeType.def :
			 EquipStatus[2] += holeValue; break;
		case holeType.mokang :
			 EquipStatus[11] += holeValue; break;
	}
}

function getAtb(str : String , pin : int , type : SlotType , pfType : ProfessionType){
	var a1 : int = parseInt(str.Substring(0,1)); 
	var a2 : int = parseInt(str.Substring(1,1)); 
	switch(a1){
		case 1: EquipStatus[4] += pin * a2 / 10 / 1 * itemXiuZheng[type]; break;
		case 2: EquipStatus[3] += pin * a2 / 10 / 1 * itemXiuZheng[type]; break;
		case 3: EquipStatus[2] += pin * a2 / 10 / 1 * itemXiuZheng[type]; break;
		case 4: 
			if(pfType == ProfessionType.Soldier) 
				EquipStatus[0] += pin * a2 / 10 / 1 * itemXiuZheng[type]; else
			if(pfType == ProfessionType.Robber) 
				EquipStatus[0] += pin * a2 / 10 / 1 * itemXiuZheng[type]; else
			if(pfType == ProfessionType.Master) 
				EquipStatus[10] += pin * a2 / 10 / 5 * itemXiuZheng[type]; 
			break;
		case 5: EquipStatus[11] += pin * a2 / 10 / 1 * itemXiuZheng[type]; break;
		case 6: EquipStatus[12] += pin * a2 / 10 / 0.5 * itemXiuZheng[type]; break;
		case 7: EquipStatus[9] += pin * a2 / 10 / 1 * itemXiuZheng[type]; break;
		case 8: 
			if(pfType == ProfessionType.Soldier) 
				EquipStatus[6] += pin * a2 / 10 / 1 * itemXiuZheng[type]; else
			if(pfType == ProfessionType.Robber) 
				EquipStatus[7] += pin * a2 / 10 / 1 * itemXiuZheng[type]; else
			if(pfType == ProfessionType.Master) 
				EquipStatus[8] += pin * a2 / 10 / 1 * itemXiuZheng[type];
			break;
		case 9: 
			if(pfType == ProfessionType.Soldier) 
				EquipStatus[7] += pin * a2 / 10 / 1 * itemXiuZheng[type]; else
			if(pfType == ProfessionType.Robber) 
				EquipStatus[6] += pin * a2 / 10 / 1 * itemXiuZheng[type]; else
			if(pfType == ProfessionType.Master) 
				EquipStatus[7] += pin * a2 / 10 / 1 * itemXiuZheng[type];
			break;
	}
}


function getQuality(qua : int , lv : int){
	switch(qua){
		case 1: return qua*2;
		case 2: return qua*4+2;
		case 3: return qua*6+12;
		case 4: return qua*8+24;
		case 5: return qua*9+56;
	}
}

var bagParent : Transform;
function ItemInfoOn(){
	bagParent.localPosition.y = 1000;
}
