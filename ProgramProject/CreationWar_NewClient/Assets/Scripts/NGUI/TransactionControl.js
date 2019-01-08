#pragma strict
//var invMaker : Inventorymaker;
private var BtnGMB : BtnGameManagerBack;

private var uicl : UIControl;
function Awake(){
//	invMaker = AllResources.InvmakerStatic;
//	ts = AllManage.tsStatic;
	invcl = AllManage.InvclStatic;	
//	UIALLcl = AllManage.UIALLPCStatic;
	AllManage.btnGMBStatic.tc = gameObject;	
	uicl = AllManage.UICLStatic;
	uicl.TransactionC = this;
}

function OnDisable(){
	try{
		gameObject.active = true;	
	}catch(e){
	
	}
}

private var Inventory1 : String;
private var Inventory2 : String;
private var Inventory3 : String;
private var Inventory4 : String; 
function Start () { 
	tp = new TransactionParameters(); 
	tp.equepmentID = "";
 	var mm : boolean = false;
	while(!mm){
		if(InventoryControl.ytLoad){
			if(InventoryControl.yt.Rows.Count > 0){			
				Inventory1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				Inventory2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				Inventory3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				Inventory4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
				mm = true;
			}
		}
		yield;
	}
	SetSelectBagItem(Inventory1);
}

function Update () {

}

var ObjJiaoYi : Transform;
function thisGuiWei(){
	ObjJiaoYi.localPosition.y = 0;
}

var SpriteLeftKuang : UISprite;
var SpriteRightKuang : UISprite;
var OtherName : String;
var OtherID : String;
var transactionID : String;
var TweenJiaoyi : TweenPosition;
var SpriteTeam1 : UISprite;
var LabelRet1 : UILabel;
var SpriteTeam2 : UISprite;
var LabelRet2 : UILabel;
function ShowJiaoYi(otherName : String , otherID : String , transID : String , Pessn : String){ 
	AllManage.UIALLPCStatic.showThisPanel("jiaoyi");
	SelectBag1();
	InfoLeft = new TransactionInfo();
	InfoRight = new TransactionInfo();
	TweenJiaoyi.Play(true);
	transactionID = transID;
	OtherName = otherName;
	OtherID = otherID;

	 InputGold.text = "0";
	 InputBlood.text = "0";

	InfoLeft.isReady = false;
	InfoRight.isReady = false;
	
	InfoLeft.isTransaction = false;
	InfoRight.isTransaction = false;
	
	SpriteLeftKuang.enabled = false;
	SpriteLeftKuang.gameObject.GetComponent(BoxCollider).enabled = false;
	SpriteRightKuang.enabled = false;
	SpriteRightKuang.gameObject.GetComponent(BoxCollider).enabled = false;
	
	SpriteLeftIsReady.spriteName = "UIM_Prompt_Off";
	SpriteLeftIsTransaction.spriteName = "UIH_Main_Button_O";
	
	SpriteRightIsReady.spriteName = "UIM_Prompt_Off";
	//SpriteRightIsTransaction.spriteName = "UIH_Main_Button_O";
	setTouXiang(otherName , Pessn , SpriteTeam1 , LabelRet1);
	setTouXiang(InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText , InventoryControl.yt.Rows[0]["ProID"].YuanColumnText , SpriteTeam2 , LabelRet2);	
}

function setTouXiang(name : String , pro : String , sp1 : UISprite, text1 : UILabel){
	text1.text = name;
	if(pro == "1"){
		sp1.spriteName = "head-zhanshi";	
	}else
	if(pro == "2"){
		sp1.spriteName = "head-youxia";	
	}else
	if(pro == "3"){
		sp1.spriteName = "head-fashi";	
	}
}

var LeftInv : BagItem;
var LabelLeftBlood : UILabel;
var LabelLeftGold : UILabel;
var SpriteLeftIsReady : UISprite;
var SpriteLeftIsTransaction : UISprite;

var InputBlood : UIInput;
var InputGold : UIInput;
private var invID : String;
function LeftIsReady(){
	if(InfoLeft.isReady){
		SpriteLeftIsReady.spriteName = "UIM_Prompt_Off";
		InfoLeft.isReady = false;
		SpriteLeftKuang.enabled = false; 
		SpriteLeftKuang.gameObject.GetComponent(BoxCollider).enabled = false;
		SpriteLeftIsTransaction.spriteName = "UIH_Main_Button_O";
	}else{
		InfoLeft.isTransaction = false;
		SpriteLeftIsTransaction.spriteName = "UIH_Main_Button_N";
		SpriteLeftIsReady.spriteName = "UIM_Prompt_On";
		InfoLeft.isReady = true;
		SpriteLeftKuang.enabled = true;
		SpriteLeftKuang.gameObject.GetComponent(BoxCollider).enabled = true;
	}
	InfoLeft.inv = LeftInv.inv;  
//	//print(LabelLeftBlood.text);
	InfoLeft.blood = LabelLeftBlood.text.ToString().Trim();
	InfoLeft.gold = LabelLeftGold.text.ToString().Trim();
	if(InfoLeft.inv != null){
		invID = InfoLeft.inv.itemID;
	}else{
		invID = "";
	} 
//	var int1 : int = parseInt(InfoLeft.blood);
//	var int2 : int = parseInt(InfoLeft.gold);
//	//print(InfoLeft.blood);
//	//print(InfoLeft.gold); 
	if(parseInt(InfoLeft.gold) > parseInt(InventoryControl.yt.Rows[0]["Money"].YuanColumnText)){
		InfoLeft.gold = InventoryControl.yt.Rows[0]["Money"].YuanColumnText;
		 InputGold.text = InfoLeft.gold;
	}
	
	if(parseInt(InfoLeft.blood) > parseInt(InventoryControl.yt.Rows[0]["Bloodstone"].YuanColumnText)){
		InfoLeft.blood = InventoryControl.yt.Rows[0]["Bloodstone"].YuanColumnText;
		 InputBlood.text = InfoLeft.blood;
	}
	var str : String = getKongWei();
	//InRoom.GetInRoomInstantiate().SendTransactionInfo(transactionID , invID + "%" + BagID+ "%"+  str, InputBlood.text , InputGold.text , InfoLeft.isReady , InfoLeft.isTransaction);
	
	if(parseInt(InputBlood.text) >= 0 && parseInt(InputGold.text) >= 0)
	{
	    InRoom.GetInRoomInstantiate().SendTransactionInfo(transactionID , invID, InputBlood.text , InputGold.text , InfoLeft.isReady , InfoLeft.isTransaction);
	}
	else
	{
	    AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info954"));
	}
}

//var ts : TiShi;
function LeftIsTransaction(){
	if(InfoLeft.isReady && InfoRight.isReady){
		var str : String = getKongWei(); 
		if(str == ""){
			 AllManage.tsStatic.Show("tips052");
		}else{
			InfoLeft.isTransaction = true; 
	//		//print(invID  + "%" + invcl.BagID+ "%"); 
//			//print("str == " + str);
	        if(parseInt(InfoLeft.gold) > parseInt(InventoryControl.yt.Rows[0]["Money"].YuanColumnText)){
		        InfoLeft.gold = InventoryControl.yt.Rows[0]["Money"].YuanColumnText;
		         InputGold.text = InfoLeft.gold;
	        }
	
	        if(parseInt(InfoLeft.blood) > parseInt(InventoryControl.yt.Rows[0]["Bloodstone"].YuanColumnText)){
		        InfoLeft.blood = InventoryControl.yt.Rows[0]["Bloodstone"].YuanColumnText;
		         InputBlood.text = InfoLeft.blood;
	        }
			//InRoom.GetInRoomInstantiate().SendTransactionInfo(transactionID , invID  + "%" + BagID+ "%"+  str , InputBlood.text , InputGold.text , InfoLeft.isReady , InfoLeft.isTransaction);		
	        if(parseInt(InputBlood.text) >= 0 && parseInt(InputGold.text) >= 0)
	        {
	            InRoom.GetInRoomInstantiate().SendTransactionInfo(transactionID , invID , InputBlood.text , InputGold.text , InfoLeft.isReady , InfoLeft.isTransaction);
	        }
	        else
	        {
	            AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info954"));
	        }		
	
			SpriteLeftIsTransaction.spriteName = "UIH_Main_Button_A";		
		}
	}
}

private var invcl : InventoryControl;
private var Fstr : String = ";";
var useinvID : String[];
function getKongWei() : String{ 
	var str : String = invcl.yt.Rows[0]["Inventory1"].YuanColumnText + invcl.yt.Rows[0]["Inventory2"].YuanColumnText + invcl.yt.Rows[0]["Inventory3"].YuanColumnText + invcl.yt.Rows[0]["Inventory4"].YuanColumnText; 
	var returnStr : String;
	useinvID = str.Split(Fstr.ToCharArray()); 
	for(var i=0; i<useinvID.length; i++){	 
		if(i < 15){   
			if(useinvID[i] == ""){ 
			//	//print(" == " + tp.equepmentID + " == ");
				returnStr = "1%" +   getRealBagID(tp.equepmentID.Split(bStr.ToCharArray())[0] , i , 0) ; 
				return returnStr;			
			}
		}else
		if(i < 30){
			if(useinvID[i] == ""){ 
			//	//print(" == " + tp.equepmentID + " == ");
				returnStr = "2%" +   getRealBagID(tp.equepmentID.Split(bStr.ToCharArray())[0]  , i , 15) ; 
				return returnStr; 
			}
		}else
		if(i < 45){ 
			if(useinvID[i] == ""){			
				returnStr = "3%" +   getRealBagID(tp.equepmentID.Split(bStr.ToCharArray())[0]  , i , 30) ; 
				return returnStr; 
			}
		}else
		if(i < 60){
			if(useinvID[i] == ""){			
				returnStr = "4%" +   getRealBagID(tp.equepmentID.Split(bStr.ToCharArray())[0]  , i , 45) ; 
				return returnStr; 
			}
		}
	}
	return "";
}

function getRealBagID(inv : String , id : int , ids : int){ 
	var str : String;
	for(var i=ids; i<ids + 15; i++){
		if(i != id){
			str += useinvID[i] + ";";
		}else{
			str +=  inv + ";";
		}
	}
	return str;
}

var tp : TransactionParameters;
var InfoLeft : TransactionInfo;
var InfoRight : TransactionInfo;
var RightInv : BagItem;
var LabelRightBlood : UILabel;
var LabelRightGold : UILabel;
var SpriteRightIsReady : UISprite;
var SpriteRightIsTransaction : UISprite; 
private var bStr : String = "%";
function SetItemRight(t : TransactionParameters){
	tp = t; 
	var str : String = tp.equepmentID.Split(bStr.ToCharArray())[0];
	if(str != ""){
		InfoRight.inv = AllResources.InvmakerStatic.GetItemInfo( str , InfoRight.inv );
		RightInv.invClear();
		RightInv.SetInv(InfoRight.inv);
	}else{
		RightInv.invClear();		
	}
	
	InfoRight.blood = tp.blood;
	LabelRightBlood.text = InfoRight.blood.ToString();
	
	InfoRight.gold =  tp.gold;
	LabelRightGold.text = InfoRight.gold.ToString();
	
	InfoRight.isReady = tp.isReady; 
	InfoRight.isTransaction = tp.isTransaction;
	if(InfoRight.isReady){ 
		SpriteRightKuang.enabled = true;
		SpriteRightKuang.gameObject.GetComponent(BoxCollider).enabled = true;
		SpriteRightIsReady.spriteName = "UIM_Prompt_On";
		if(InfoRight.isTransaction){
//			SpriteRightIsTransaction.spriteName = "UIH_Main_Button_A";
		}else{
//			SpriteRightIsTransaction.spriteName = "UIH_Main_Button_N";
		}
	}else{
		SpriteRightKuang.enabled = false;
		SpriteRightKuang.gameObject.GetComponent(BoxCollider).enabled = false;
		SpriteRightIsReady.spriteName = "UIM_Prompt_Off";
//		SpriteRightIsTransaction.spriteName = "UIH_Main_Button_O";
	}
}
  
function TransactionZhongZhi(){
//	UIALLcl.showThisPanel(0);
	LeftInv.invClear();
	RightInv.invClear();
	TweenJiaoyi.Play(false);  
	InRoom.GetInRoomInstantiate().TransactionClose(transactionID);
	//AllManage.tsStatic.Show("tips053");
}

function TweenClose(){ 
	LeftInv.invClear();
	RightInv.invClear();
	AllManage.UIALLPCStatic.show0();
 	 TweenJiaoyi.Play(false);  
} 

//var UIALLcl : UIAllPanelControl;
 
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

 var BagID : int = 1;
var inv : InventoryItem[];
function SelectBag1(){
    if(InfoLeft.isReady)
    {
        return;
    }	
	Inventory1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
	Inventory2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
	Inventory3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
	Inventory4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
	BagID = 1; 
	ClearBagItem();
	SetSelectBagItem(Inventory1);

	LeftInv.invClear();
} 
function SelectBag2(){
    if(InfoLeft.isReady)
    {
        return;
    }
	Inventory1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
	Inventory2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
	Inventory3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
	Inventory4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
	BagID = 2;
	ClearBagItem();
	SetSelectBagItem(Inventory2); 

	LeftInv.invClear();
//	//print(Inventory2 + " == ");
} 
function SelectBag3(){
    if(InfoLeft.isReady)
    {
        return;
    }
	Inventory1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
	Inventory2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
	Inventory3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
	Inventory4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
	BagID = 3;
	ClearBagItem();
	SetSelectBagItem(Inventory3);

	LeftInv.invClear();
} 
function SelectBag4(){
    if(InfoLeft.isReady)
    {
        return;
    }
	Inventory1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
	Inventory2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
	Inventory3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
	Inventory4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
	BagID = 4;
	ClearBagItem();
	SetSelectBagItem(Inventory4);

	LeftInv.invClear();
} 

function ClearOneBagItem(itemID : String){
	for(var i=0; i<BagIt.length; i++){
		if(BagIt[i].inv != null){
			if(BagIt[i].inv.itemID == itemID){
				BagIt[i].OtherYiChu();
			}	
		}
	}
}

function ClearBagItem(){
	for(var i=0; i<BagIt.length; i++){
		BagIt[i].invClear();
	}
}
 
function SetSelectBagItem(invID : String){
	var i : int = 0;
	var useInv : InventoryItem;
	var useInvID : String[];
	useInvID = invID.Split(Fstr.ToCharArray());
	inv = new Array(9);
//	//print(invID);
//	DontUpdate = true;
//	for(i=0; i<9; i++){	 
//		inv[i] = LT.NewLootContent()[0]; 
//		if(i==0){
//			inv[i] = invMaker.GetItemInfo("94420342325200201330145015",inv[i]);
//		}
//		BagIt[i].SetInv(inv[i]);
	for(i=0; i<useInvID.length; i++){	 
		if(useInvID[i] != ""){ 
			useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
			BagIt[i].SetInv(useInv);
//			AddBagItem(useInv); 
		}
	}
	
}

var BagIt : BagItem[];
function AddBagItem(inv : InventoryItem){
	var i : int = 0; 
	var j : int = 0;
	if(inv.slotType < 12){	
		for(i=0; i<BagIt.length; i++){ 
			if(BagIt[i].inv == null){
				BagIt[i].SetInv(inv);
//				UpdateBagItem();
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
//							UpdateBagItem();
							return;
						}else{
							for(j=0; j<BagIt.length; j++){
								if(BagIt[j].inv == null){ 
									inv.consumablesNum = BagIt[i].inv.consumablesNum + inv.consumablesNum - 20;
									BagIt[j].SetInv(inv);
//									UpdateBagItem();
									return;					
								}
							}
							BagIt[i].inv.consumablesNum =20; 
//							UpdateBagItem();
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
//				UpdateBagItem();
//				//print("jin dao zhe li le444");
				return;					
			}
		}
	}
//	var i : int = 0; 
//	var j : int = 0;
//	if(inv.slotType < 12){	
//		for(i=0; i<BagIt.length; i++){ 
//			if(BagIt[i].inv == null){
//				BagIt[i].SetInv(inv);
//				return;
//			}
//		}
//	}else{
////		//print(inv.itemID);
//		for(i=0; i<BagIt.length; i++){
//			if(BagIt[i].inv != null && BagIt[i].active){
//				if(BagIt[i].inv.itemID.Substring(0,5) == inv.itemID.Substring(0,5)){
//					if(BagIt[i].inv.consumablesNum + inv.consumablesNum <= 20){				
//						BagIt[i].inv.consumablesNum += inv.consumablesNum; 
//						BagIt[i].inv.itemID =  BagIt[i].inv.itemID.Substring(0,5) + BagIt[i].inv.consumablesNum.ToString(); 
//						BagIt[i].showConsumablesNum();
//						
//						return;
//					}else{
//						for(j=0; j<BagIt.length; j++){
//							if(BagIt[j].inv == null){ 
//								inv.consumablesNum = BagIt[i].inv.consumablesNum + inv.consumablesNum - 20;
//								BagIt[j].SetInv(inv);
//							}
//						}
//						BagIt[i].inv.consumablesNum =20; 
//						BagIt[i].inv.itemID =  BagIt[i].inv.itemID.Substring(0,5) + BagIt[i].inv.consumablesNum.ToString();
//						BagIt[i].showConsumablesNum();
//						return;					
//					}
//				}
//			}
//		} 
//		for(i=0; i<BagIt.length; i++){ 
//			if(BagIt[i].inv == null){  
//				 BagIt[i].SetInv(inv);
//				BagIt[i].showConsumablesNum();
//				
//				return;					
//			}
//		}
//	}
}

function show0(){
	AllManage.UIALLPCStatic.show0();
}