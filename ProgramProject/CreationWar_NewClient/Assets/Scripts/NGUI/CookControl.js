#pragma strict
private var Fstr : String = "=";
private var Sstr : String = ";";
//var invMaker : Inventorymaker;
var inv1 : String;
var inv2 : String;
var inv3 : String;
var inv4 : String;

var CookLeve : UILabel;
private var ps : PlayerStatus;
static var expCooking : int;

function Awake(){
	kingID = "";
	AllManage.cookCLStatic = this;
	invcl = AllManage.InvclStatic;
//	invMaker = AllResources.InvmakerStatic;
//	uiallpc = AllManage.UIALLPCStatic;
	useitemN = AllManage.uusStatic;
//	ts = AllManage.tsStatic;
	cam = AllManage.uicamStatic;
}

var buttons : GameObject[];
var jiaocheng : TaskJiaoChengControl;
function Start () {
	jiaocheng = AllManage.jiaochengCLStatic;
	jiaocheng.allJiaoCheng[6].obj[1] = buttons[0];
	jiaocheng.allJiaoCheng[6].obj[3] = buttons[1];
 	var mm : boolean = false; 
	var OneTime : boolean = false;
	if(!InventoryControl.ytLoad){
		while(!mm){
			if(InventoryControl.ytLoad){
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
				expCooking = GetBDInfoInt("CookingExp" , 0);
				mm = true;
				OneTime = true;
			}
			yield;
		}	
	}
	SetEqupmentList("");
	SelectBeforeInv();
}

var ObjNoneFinsh : UILabel;

function YieldSelectBeforeInv(){
	yield;
	yield;
	yield;
	SelectBeforeInv();
}

function ShowNOCook()
{
	if(CookLeve)
	{
	CookLeve.text = String.Format("{0} {1} " , AllManage.AllMge.Loc.Get("info1184"),InventoryControl.yt.Rows[0]["CookingExp"].YuanColumnText);	
	}
		
	if(invParent.transform.childCount>0){
		ObjNoneFinsh.text = AllManage.AllMge.Loc.Get("info075");
	}else{
		ObjNoneFinsh.text = AllManage.AllMge.Loc.Get("info1176");
	}
}

function ReLoadFish(){
	inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
	inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
	inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
	inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
	expCooking = GetBDInfoInt("CookingExp" , 0);
	SetEqupmentList("");
	if(kingID != ""){
		SelectBeforeInv(kingID);	
	}else{
		SelectBeforeInv();	
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

var stime : float;
var joy : Joystick;
var cube : Transform;
var cam : Camera;
//function Update(){
//	TransGan.up = Vector3(joy.position.x , joy.position.y , 0);
//	TransGan.localEulerAngles.x = 0;
//	TransGan.localEulerAngles.y = 0;
//	if(stime < Time.time){
//		stime = Time.time + 0.1;
//		lookTieZhan();
//		if(cdTime > 0){
//			cdTime -= 0.1;
//		}
//		if(cdTime <0){
//			cdTime = 0;
//		}
//	}
//}

function SetZ(){
	var pos : Vector3;
	var Z : float;
	Z=cam.transform.InverseTransformPoint(cube.position).z;
	pos=cam.ScreenToWorldPoint (Vector3 (Input.mousePosition.x, Input.mousePosition.y,Z));
	cube.transform.rotation = Quaternion.LookRotation ( pos-cube.position,Vector3.forward);
	cube.localEulerAngles.y=0;
	cube.localEulerAngles.x=0; 
//	cube.localEulerAngles.z*= -1;
//	cube.localEulerAngles.z += 180;
	TransGan.rotation = cube.rotation;
}

var ebItem : CookItem;
var invItemArray : CookItem[];
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
	
	var i : int = 0;	
	var useStr : String;
	var inv : InventoryItem;
	useStr = inv1 + inv2 + inv3 + inv4; 
//	//print("useStr == " + useStr);
	useInvID = useStr.Split(Sstr.ToCharArray());
	for(var intNum=1; intNum<20; intNum++){
		for(i=0; i<useInvID.length; i++){
			if(useInvID[i] != "" && parseInt(useInvID[i].Substring(0,1)) == 8 && parseInt(useInvID[i].Substring(1,1)) == 2 && parseInt(useInvID[i].Substring(2,2)) == intNum){
				var Obj : GameObject = Instantiate(ebItem.gameObject); 
				wuneirong.transform.localPosition.y = 3000;
				Obj.transform.parent = invParent;
				var useEBI : CookItem;
				useEBI = Obj.GetComponent(CookItem);
	//			//print(useEBI);
	//			//print(invMaker);
				useEBI.setInv(AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , inv));
				useEBI.ebCL = this; 
				addInvItem(useEBI);				
			}
		} 
	}
	yield;
	JianChaKaoyu();
	GID.repositionNow = true;
}

function addInvItem(ebi : CookItem){
	var use : CookItem[]; 
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

function JianChaKaoyu(){
	if(invItemArray.length <1){
		UIButtonKaoStart.SetActiveRecursively(false);
	}
}

function SelectBeforeInv(){
	var i : int = 0; 
	if(AllManage.jiaochengCLStatic.JiaoChengID == 6 && AllManage.jiaochengCLStatic.step == 1){
		for(i=0; i<invItemArray.length; i++){
			if(invItemArray[i] && invItemArray[i].inv && invItemArray[i].inv.itemID.Substring(0,4) == "8201"){
				invItemArray[i].GoSelect();
//				SelectOneInv(invItemArray[i] , invItemArray[i].inv);
				return;
			}
		}
	}
	for(i=0; i<invItemArray.length; i++){
		invItemArray[i].GoSelect();
//		SelectOneInv(invItemArray[i] , invItemArray[i].inv);
		return;
	}
}

function SelectBeforeInv(id : String){
	for(var i=0; i<invItemArray.length; i++){
		if(id == invItemArray[i].inv.itemID){
			invItemArray[i].GoSelect();
//			SelectOneInv(invItemArray[i] , invItemArray[i].inv);	
			return;
		}
	}
}

private var eii :  EquepmentItemInfo; 
private var  myEBI : CookItem;
function SelectOneInv(ebi : CookItem , inv : InventoryItem){
	isUseBloodStone = true;
//	UseBloodStone();
	var i : int = 0;
	for(i=0; i<invItemArray.length; i++){
		if(invItemArray[i] == ebi){  
			myEBI = ebi;
//			LabelUseBloodStone.text = "x" + inv.itemQuality.ToString();
//			eii.showEquItemInfo(inv,null);
			invItemArray[i].SelectMe();
		}else{
			invItemArray[i].DontSelectMe();		
		}
	}
	JianChaKaoyu();
} 

var kingID : String = "";
private var invcl : InventoryControl;
//var invmaker : Inventorymaker;
var UIButtonKaoStart : GameObject; 
var TransGan : Transform; 
var aprTe : ParticleEmitter;
var ParentTiao : Transform;
function KaoYuStart(){  
	if(myEBI == null || myEBI.inv == null){
		return;
	}
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	var uyLevel = parseInt(myEBI.inv.itemID.Substring(2,2));
//	//print((expCooking / 5 - 20) + " == " + uyLevel * 6);
	if(!King && expCooking >= (uyLevel-1)*20){
		ParentTiao.localPosition.y = 0;
		aprTe.emit = true;
		yus1[0].active = true;
		yus2[0].active = true;
		UIButtonKaoStart.gameObject.SetActiveRecursively(false);
		yu1.canKao = true;
		yu2.canKao = true;
//		UseKaoYuAsID(myEBI.inv.itemID);
		invcl.useBagItem(myEBI.inv.itemID , 1);
		kingID = myEBI.inv.itemID;	 
		KaoIng(); 
		zhuandong();
		JianChaKaoyu();
		ReLoadFish();
	}else{
		if(NeedcookLevel=="0"||NeedcookLevel==""){
		AllManage.tsStatic.Show(AllManage.AllMge.Loc.Get("tips004"));
		}else{
		AllManage.tsStatic.Show(AllManage.AllMge.Loc.Get("tips004")+NeedcookLevel);
		}
	}
}

function GetOneCooking(level : int){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	var uExp : int;
	uExp = parseInt(ps.Level )* 5;
	if(expCooking < uExp && level > expCooking / 5 / 6){
		expCooking += 1;
		InventoryControl.yt.Rows[0]["CookingExp"].YuanColumnText = expCooking.ToString();
		yield WaitForSeconds(2);
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages144")+expCooking);
	}
		if(AllManage.SkillCLStatic){
			AllManage.SkillCLStatic.ShowFuYeLevel();
		}
}

function ButtonFangxiang(){
	if(fangxiang){
		 fangxiang = false;
	}else{
		fangxiang = true;
	}
}

var fangxiang : boolean = false;
function zhuandong(){
	fangxiang = false;
	while(King){
		SetZ();
//		if(fangxiang){
//			TransGan.localEulerAngles.z = Mathf.Lerp(TransGan.localEulerAngles.z , 0 , Time.deltaTime * 3);
//		}else{
//			TransGan.localEulerAngles.z = Mathf.Lerp(TransGan.localEulerAngles.z , 180 , Time.deltaTime * 3);		
//		}
		yield;
	}
}

var yu1 : YuItemYu;
var yu2 : YuItemYu;
var King : boolean = false; 
var UIfsup : UIFilledSprite;
var UIfsdown : UIFilledSprite;
//var ts : TiShi;
private var useFloat : float;
function KaoIng(){
	King = true;
	var inv : InventoryItem;
	while(King){
		useFloat = yu1.shuzhi;
		UIfsup.fillAmount = useFloat / 4.0;
		useFloat = yu2.shuzhi;
		UIfsdown.fillAmount = useFloat / 4.0;
		if(Mathf.Abs(yu1.shuzhi - yu2.shuzhi) > 2){
			King = false;
			ParentTiao.localPosition.y = 3000;
			UIButtonKaoStart.gameObject.SetActiveRecursively(true); 
			AllManage.tsStatic.Show("tips006");
			yu1.canKao = false;
			yu2.canKao = false; 
			yu1.shuzhi = 0;
			yu2.shuzhi = 0;
			yuguanbi();
			if(AllManage.jiaochengCLStatic.JiaoChengID == 6 && AllManage.jiaochengCLStatic.step == 2){
				AllManage.jiaochengCLStatic.NextStep();
			}
		}
		if(yu1.shuzhi > 4 && yu2.shuzhi > 4){
			ParentTiao.localPosition.y = 3000;
			King = false;
			yu1.canKao = false;
			yu2.canKao = false;
			UIButtonKaoStart.gameObject.SetActiveRecursively(true);
			
		InventoryControl.yt.Rows[0]["AimCooking"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimCooking"].YuanColumnText) + 1).ToString();
        InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.CookingNum , 1);
			yuguanbi();
			GetOneCooking(parseInt(kingID.Substring(2,2)));
			kingID = "83" + kingID.Substring(2,2) + ",01";
//			//print(kingID + " == ");
			var inv11 : InventoryItem;
			inv11 = AllResources.InvmakerStatic.GetItemInfo(kingID , inv);
			invcl.AddBagItem(inv11);
//			invcl.ReInitItem1();
			if(AllManage.jiaochengCLStatic.JiaoChengID == 6 && AllManage.jiaochengCLStatic.step == 2){
				AllManage.jiaochengCLStatic.NextStep();
			}
			yield WaitForSeconds(1f);
			EquipEnhance.instance.ShowMyItem("",AllManage.AllMge.Loc.Get("tips007")); 
		}
		setYuColor(yu1.shuzhi  , yu2.shuzhi);
		yield;
	}
	JianChaKaoyu();
}

function yuguanbi(){
	for(var i=0; i <yus1.length; i++){
			yus1[i].active = false;	
			yus2[i].active = false;	
	}	
//	aprTe.emit = false;	
	yu1.canKao = false;
	yu2.canKao = false;
	yu1.shuzhi = 0;
	yu2.shuzhi = 0;
}

var yus1 : GameObject[];
var yus2 : GameObject[];
function setYuColor(yu1 : float , yu2 : float){  
//	var useInt : int = Mathf.Min( yu1 , yu2);
	var useInt1 : int = yu1;
	var useInt2 : int = yu2;
	for(var i=0; i <yus1.length; i++){
		if(i == useInt1){
			yus1[i].active = true;
		}else{
			yus1[i].active = false;	
		}
		if(i == useInt2){
			yus2[i].active = true;
		}else{
			yus2[i].active = false;	
		}
	}
}

//var uiallpc : UIAllPanelControl;
function KaoYuExit(){ 
	for(var i=0; i <yus1.length; i++){
			yus1[i].active = false;	
			yus2[i].active = false;	
	}
	if(King){
//		aprTe.emit = false;
		King = false;
		yu1.canKao = false;
		yu2.canKao = false;
		UIButtonKaoStart.gameObject.SetActiveRecursively(true);
					ParentTiao.localPosition.y = 3000;
		AllManage.tsStatic.Show("tips008");
			yuguanbi();
	}
	AllManage.UIALLPCStatic.show0();
}

private var useitemN : UseItemNum;
function UseKaoYuAsID(id : String){
//	invcl.useBagItem(id , 1);
	for(var i=0; i<useInvID.length; i++){
		if(useInvID[i].Length > 3){
			if(useInvID[i].Substring(0,4) == id.Substring(0,4)){
				useInvID[i] = useitemN.useItemNum(useInvID[i] , 1); 
				BuildOneID();
				return;
			}
		}
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 
var SpriteUseBloodStone : UISprite;
var isUseBloodStone : boolean = false;
var LabelUseBloodStone : UILabel;
function UseBloodStone(){
	if(!isUseBloodStone){
		isUseBloodStone = true;
		SpriteUseBloodStone.spriteName = "UIM_Prompt_On";
	}else{
		isUseBloodStone = false;
		SpriteUseBloodStone.spriteName = "UIM_Prompt_Off";	
	}
}
function DoDaZao(){
//	useTieZhan();
//	var i : int = 0;
//	for(i=0; i<invItemArray.length; i++){
//		if(useInvID[i] == myEBI.inv.itemID){
//			BuildOneID(useInvID[i] , i);
//		}
//	}
}
 
function GetBuildJiaCheng(i : int){
//		switch(i){
//		case 1: return qua*2;
//		case 2: return qua*4+2;
//		case 3: return qua*6+12;
//		case 4: return qua*8+24;
//		case 5: return qua*9+56;
//	}
}



//var invcl : InventoryControl;
var lizi : ParticleEmitter;
function BuildOneID(){ 
//	//print("zhe li la a ");
//	if(lizi.emit){
//		return;
//	}
//	var m : int = 6;
//	var useInt : int;
//	var useStr : String;
////	//print(invID);
//	useInt = parseInt(invID.Substring(13,3)) + m;
//	useStr = useInt.ToString();
//	if(useStr.length == 1){
//		useStr = "00" + useStr;
//	}else
//	if(useStr.length == 2){
//		useStr = "0" + useStr;		
//	}
//	//print(useInvID[id] + " == " + m);
//	useInvID[id] = useInvID[id].Substring(0,13) + useStr + useInvID[id].Substring(16,7); 
//	//print(useInvID[id] + " ==");   
	InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText = "";
//	InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText = "";
	for(var i=0; i<useInvID.length; i++){	 
		if(i < 15){
			InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText += useInvID[i] + ";";
		}else
		if(i < 30){
			InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 45){
			InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 60){
			InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText += useInvID[i] + ";";		
		}
//		else{
//			InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText += useInvID[i] + ";";		
//		}
	}  
//	//print(inv1 + inv2 + inv3 + inv4 + inv5);
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
//				inv5 = InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText;
	SetEqupmentList("");
	
//	yield;
//	yield;
//	yield;
	invcl.isUpdatePhoton = false;
	invcl.ReInitItem1();
	SelectBeforeInv(); 
//	shanguang();
}

function shanguang(){
	if(!lizi.emit){
		lizi.emit = true;
		yield WaitForSeconds(lizi.particleCount);
		lizi.emit = false;
	}

}


//var nowTime : System.DateTime;
//		yuan.YuanTimeSpan uuu= new yuan.YuanTimeSpan();
//		uuu.TimeStart();
//		int shijian = uuu.TimeEndtoSeconds();
var timesTieZhang : float;
var maxTimesTieZhang : float = 10; 
var allcdTime : float;
var cdTime : float;
var jinduTieZhan : float;
var nowNeedTime : float;
var AllTime : float;
var Filledcd : UIFilledSprite;
function lookTieZhan(){
//	nowNeedTime = timesTieZhang / maxTimesTieZhang * AllTime;
//	if(cdTime < nowNeedTime){  
	var useInt : int = 0; 
	useInt = cdTime /  AllTime * maxTimesTieZhang;
	timesTieZhang = useInt; 
	if(usetimes <= timesTieZhang){
		usetimes = timesTieZhang;
	}
//	}
	Filledcd.fillAmount = cdTime / AllTime;
//	//print(timesTieZhang);
}

var usetimes : float;
function useTieZhan(){
	if(timesTieZhang < maxTimesTieZhang){
		timesTieZhang += 1; 
		usetimes = timesTieZhang;
		cdTime += 1 / maxTimesTieZhang * AllTime;
	}
}
var TextLeve : UILabel ; 
var NeedcookLevel : String ;
function NeedCookLeve(Leve : int){
	TextLeve.text = AllManage.AllMge.Loc.Get("info1216")+((Leve-1)*20).ToString();
	NeedcookLevel = ((Leve-1)*20).ToString();
}

