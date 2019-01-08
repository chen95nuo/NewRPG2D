#pragma strict

var PlayerFormula : String;
var useFormulas : String[];
private var Fstr : String = ";";
private var Sstr : String = ";";
//var invMaker : Inventorymaker;
static var expMake : int;
//var uiallpc : UIAllPanelControl;
var nowSelectLevelMin : int = 20;
var nowSelectLevelMax : int = 39;
var nowSelectPro : int = 1;
function Awake(){
//	invMaker = AllResources.InvmakerStatic;
	AllManage.inventoryProduceStatic = this;
	invcl = AllManage.InvclStatic; 
//	ts = AllManage.tsStatic;
//	uiallpc = AllManage.UIALLPCStatic;
}

var myPanel : UIPanel;
static var me : GameObject;
function Start () {
	me = gameObject;
	var mm : boolean = false;
	while(!mm){
		if(InventoryControl.ytLoad){
			if(InventoryControl.yt.Rows.Count > 0){			
				PlayerFormula = InventoryControl.yt.Rows[0]["Formula"].YuanColumnText;		
				expMake = GetBDInfoInt("MakeExp" , 0);
				mm = true;
			}
		}
		yield;
	}
//	nowSelectPro = parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText);
	useFormulas = PlayerFormula.Split(Fstr.ToCharArray());
//	SetEqupmentList("" , nowSelectLevelMin , nowSelectLevelMax , nowSelectPro);
//	SelectBeforeInv();
	myPanel.widgetsAreStatic = false;
	yield;
	yield WaitForSeconds(1);
	yield;
	myPanel.widgetsAreStatic = true;	
}

function OnDisable(){
	try{
		gameObject.active = true;	
	}catch(e){
	
	}
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

function resetList(){
	PlayerFormula = InventoryControl.yt.Rows[0]["Formula"].YuanColumnText;
	useFormulas = PlayerFormula.Split(Fstr.ToCharArray());
	SetEqupmentList("" , nowSelectLevelMin , nowSelectLevelMax , nowSelectPro);
	SelectBeforeInv();
}


function addInvItem(ebi : InventoryProduceItem){
	var use : InventoryProduceItem[]; 
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
//////////////////////////////////

function XueXi(id : String) : boolean{
	PlayerFormula = InventoryControl.yt.Rows[0]["Formula"].YuanColumnText;
	useFormulas = PlayerFormula.Split(Fstr.ToCharArray());
	var i : int = 0;
	var can : boolean = true;
	for(i=0; i<useFormulas.length; i++){
		if(useFormulas[i] == id && can){
			can  = false;
		}
	}
	if(can){
		PlayerFormula += id + ";";
		InventoryControl.yt.Rows[0]["Formula"].YuanColumnText = PlayerFormula;
		return true;
	}
	return false;
}

var eii :  EquepmentItemInfo; 
private var myEBI : InventoryProduceItem;
var useInv : InventoryItem;
function SelectOneInv(ebi : InventoryProduceItem , inv : InventoryItem){
	UseBloodStone();
	useInv = inv;
	var i : int = 0;
	for(i=0; i<invItemArray.length; i++){
		if(invItemArray[i] == ebi){  
			myEBI = ebi;
			LabelUseBloodStone.text = "x" + inv.itemQuality.ToString();			
			eii.showEquItemInfo(inv , null);
			invItemArray[i].SelectMe();
			LookCanProduce(useInv);
		}else{
			invItemArray[i].DontSelectMe();		
		}
	}
} 

var inv1 : String;
var inv2 : String;
var inv3 : String;
var inv4 : String;

var canProduce : boolean = false;
function LookCanProduce(inv : InventoryItem){
	canProduce = true;
	LookOneItem(inv.formulaItemNeed1.consumablesID , inv.formulaItemNeed1.consumablesNum , 0);
	LookOneItem(inv.formulaItemNeed2.consumablesID , inv.formulaItemNeed2.consumablesNum , 1);
	LookOneItem(inv.formulaItemNeed3.consumablesID , inv.formulaItemNeed3.consumablesNum , 2);
	LookOneItem(inv.formulaItemNeed4.consumablesID , inv.formulaItemNeed4.consumablesNum , 3);
//	//print(canProduce);
	if(! isAlreadyStudy(inv.itemID)){
		canProduce = false;
	}
	eii.SetProduceItemButton(canProduce);
}

function LookOneItem(itemID : String , itemNum : String , id){
	for(var i=0; i<useBagInvID.length; i++){
		if(useBagInvID[i] != ""){ 
			if(useBagInvID[i].Substring(0,4) == itemID.Substring(0,4)){
				eii.SetProduceItemNum(itemNum , parseInt(useBagInvID[i].Substring(5,2)) , id);
				if(parseInt(useBagInvID[i].Substring(5,2)) < parseInt(itemNum)){
					canProduce = false;
				}
				return;
			}
		}
	}
	canProduce = false;
	eii.SetProduceItemNum(itemNum , 0 , id);
	return;
}

var LabelUseBloodStone : UILabel;
function UseBloodStone(){
//	//print("shi yong xue shi");
}

private var ps : PlayerStatus;
function GetOneCooking(level : int){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	var uExp : int;
	uExp = parseInt(ps.Level) * 5;
	if(expMake < uExp && level > expMake / 5 / 6){
		expMake += 1;
		InventoryControl.yt.Rows[0]["MakeExp"].YuanColumnText = expMake.ToString();
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages147")+expMake);
	}
}

var BagIT : BagItem[];
private var invcl : InventoryControl;
//var ts : TiShi;
 var SpriteDaZao : UISprite;
 private var beforeID : String;
function DoDaZao(){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	
	var uyLevel = parseInt(useInv.itemID.Substring(3,2));
//	Mathf.Clamp((expMake / 5 - 20) , 0,9999) >= uyLevel - 20 && 
	if(SpriteDaZao.spriteName == "UIH_Main_Button_N"){
//		AllManage.tsStatic.RefreshBaffleOn();
		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().EquepmentProduce(useInv.itemID , (useInv.itemLevel * 100) * (-1) , 0));
//		if(ps.UseMoney(useInv.itemLevel * 100 , useInv.itemLevel + useInv.itemQuality)){
//			if(canProduce){
//				var Iinv : InventoryItem;
//				Iinv = AllResources.InvmakerStatic.GetItemInfo(useInv.itemID.Substring(1,15) + "0000000000" , Iinv);
//				invcl.AddBagItem(Iinv);
//				XiaoHaoItem(useInv.itemID.Substring(16,4) , useInv.formulaItemNeed1.consumablesNum.ToString());
//				XiaoHaoItem(useInv.itemID.Substring(22,4) , useInv.formulaItemNeed2.consumablesNum.ToString());
//				XiaoHaoItem(useInv.itemID.Substring(28,4) , useInv.formulaItemNeed3.consumablesNum.ToString());
//				XiaoHaoItem(useInv.itemID.Substring(34,4) , useInv.formulaItemNeed4.consumablesNum.ToString());
//			}
//			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.EquipMake).ToString());
//			InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText = "";
//			InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText = "";
//			InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText = "";
//			InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText = "";
//			for(var i=0; i<useBagInvID.length; i++){	 
//				if(i < 15){
//					InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText += useBagInvID[i] + ";";
//				}else
//				if(i < 30){
//					InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText += useBagInvID[i] + ";";		
//				}else
//				if(i < 45){
//					InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText += useBagInvID[i] + ";";		
//				}else
//				if(i < 60){
//					InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText += useBagInvID[i] + ";";		
//				}
//			}
//			
//			SetEqupmentList("");
//			invcl.isUpdatePhoton = false;
//			invcl.ReInitItem1();
//			
//			if(Iinv){
//				invcl.AddBagItem(Iinv);	
//			}
//			SelectBeforeInv(useInv.itemID);
//			InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.MakeNum , 1);
//		}
		beforeID = useInv.itemID;
	}else{
		if(SpriteDaZao.spriteName != "UIH_Main_Button_N"){		
			AllManage.tsStatic.Show("tips033");
		}else{
			AllManage.tsStatic.Show("tips035");		
		}
	}
}

function RefreshList(){	
	SetEqupmentList("" , nowSelectLevelMin , nowSelectLevelMax , nowSelectPro);
	invcl.isUpdatePhoton = false;
	invcl.ReInitItem1();
	SelectBeforeInv(beforeID);
	InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.MakeNum , 1);
}

function XiaoHaoItem(str : String , num : String){
	var useint : int;
	var usestr : String;
	for(var i=0; i<useBagInvID.length; i++){
		if(useBagInvID[i] != ""){
			if(useBagInvID[i].Substring(0,4) == str){ 
				useint = parseInt(useBagInvID[i].Substring(5,2)) - parseInt(num);
				usestr = useint.ToString();
				if(useint < 10)
				usestr = "0" + usestr;
				useBagInvID[i] =  useBagInvID[i].Substring(0,5) + usestr + ","+ useBagInvID[i].Substring(8,1);
				if(useint == 0)
				useBagInvID[i] = "";
 			}
 		}
 	}
}

function show0(){
	AllManage.UIALLPCStatic.show0();
}

function sorrt(ary : String[]): String[] {
    var length : int = ary.length;
    for (var i = 0; i < length; i++) {
        var _min : String = ary[i];
        var k : int = i;
        for (var j = i + 1; j < length; j++) {
            if ( _min.Length > 8 && ary[j].Length> 8 && parseInt(_min.Substring(3,2)) <  parseInt(ary[j].Substring(3,2))) {
                _min = ary[j];
                k = j;
            }
        }
        ary[k] = ary[i];
        ary[i] = _min;
    }
    return ary;
}

var ebItem : InventoryProduceItem;
var invItemArray : InventoryProduceItem[];
var invParent : Transform; 
var GID : UIGrid;
var useInvID : String[]; 
var useBagInvID : String[];
var wuneirong : GameObject;
function SetEqupmentList(equStr : String , levelMin : int , levelMax : int , selectPro : int){
	invClear();
	
	var i : int = 0;
	
	var useStr : String;
	var inv : InventoryItem; 
	var useStr2 : String;
	useStr = PlayerFormula;  
	useStr2 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText + InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText + InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText + InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText; 

	useStr2 = useStr2.Replace("-" , "");
//	//print(useStr);
	useInvID = useStr.Split(Sstr.ToCharArray()); 
//	//print(useInvID.length);
	useBagInvID =  useStr2.Split(Sstr.ToCharArray());
	useInvID = sorrt(useInvID);
	for(i=0; i<useInvID.length; i++){
		print((GetItemPro(useInvID[i]) == selectPro) + " == " + i + " == " + useInvID[i]);
		if(useInvID[i] != "" && GetItemLevel(useInvID[i]) <= levelMax && GetItemLevel(useInvID[i]) >= levelMin  && GetItemPro(useInvID[i]) == selectPro){
			wuneirong.transform.localPosition.y = 3000;
			var Obj : GameObject = Instantiate(ebItem.gameObject); 
			Obj.transform.parent = invParent;
			var useEBI : InventoryProduceItem;
			Obj.transform.localPosition = Vector3(0,0,0);
			useEBI = Obj.GetComponent(InventoryProduceItem);
			useEBI.setInv(AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , inv));
			useEBI.ebCL = this; 
			addInvItem(useEBI);				
		}
	}
	
	var bluePrintID : String = "";
	var isItem : String = "";
	for(var rows : yuan.YuanMemoryDB.YuanRow in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBlueprint.Rows){
		bluePrintID = rows["BlueprintID"].YuanColumnText ;
		isItem = rows["isItem"].YuanColumnText ;
		if(GetItemLevel( bluePrintID ) >= levelMin && GetItemLevel( bluePrintID ) <= levelMax && GetItemPro( bluePrintID ) == selectPro && !isAlreadyStudy( bluePrintID ) && isItem != "1"){
			wuneirong.transform.localPosition.y = 3000;
			var Obj2 : GameObject = Instantiate(ebItem.gameObject); 
			Obj2.transform.parent = invParent;
			var useEBI2 : InventoryProduceItem;
			Obj2.transform.localPosition = Vector3(0,0,0);
			useEBI2 = Obj2.GetComponent(InventoryProduceItem);
			useEBI2.setInv(AllResources.InvmakerStatic.GetItemInfo(bluePrintID , inv));
			useEBI2.ebCL = this; 
			useEBI2.SwitchAlreadyStudy();
			addInvItem(useEBI2);
		}
	}
	
	yield;
	GID.repositionNow = true;
}

private var getInv : InventoryItem;
function GetItemLevel(itemID : String) : int{
	getInv = AllResources.InvmakerStatic.GetItemInfo(itemID , getInv);
	if(getInv != null){
		return getInv.itemLevel;
	}
	return -1;
}

function GetItemPro(itemID : String) : int{
	getInv = AllResources.InvmakerStatic.GetItemInfo(itemID , getInv);
	if(getInv != null){
//		print(getInv.professionType);
		return getInv.professionType;
	}
	return -1;
}

function isAlreadyStudy(itemID : String) : boolean{
	PlayerFormula = InventoryControl.yt.Rows[0]["Formula"].YuanColumnText;		
	useFormulas = PlayerFormula.Split(Fstr.ToCharArray());
	for(var i=0; i<useFormulas.length; i++){
		if(useFormulas[i] == itemID){
			return true;
		}
	}
	return false;
}

