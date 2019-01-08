#pragma strict
private var Fstr : String = "=";
private var Sstr : String = ";";
//var invMaker : Inventorymaker;
var inv1 : String;
var inv2 : String;
var inv3 : String;
var inv4 : String;
var inv5 : String;
var LabelIron : UILabel;
var LabelGold : UILabel;

//LabelBuildRandom.color = c;
static var oo : boolean = false;
static var st : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("Task","id"); 

static var me : GameObject;
function Awake(){
//	invMaker = AllResources.InvmakerStatic;
	invcl = AllManage.InvclStatic; 
	me = gameObject;
//	ts = AllManage.tsStatic;
//	qr = AllManage.qrStatic;
}	

var buttons : GameObject[];
var jiaocheng : TaskJiaoChengControl;
var myPanel : UIPanel;
function Start () {
	jiaocheng = AllManage.jiaochengCLStatic;
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
//		while(! oo){
//			if(InRoom.GetInRoomInstantiate().ServerConnected){ 
//				//print(Time.time);
//				if(!st.IsUpdate){
//					InRoom.GetInRoomInstantiate().GetYuanTable("select * from PlayerService","DarkSword2",st); 
//				}
		st = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytPlayerService;
//				if(st.Count > 0){
					oo = true;
//					maxTimesTieZhang = parseInt(st.Rows[0]["EquipItemUPdate"].YuanColumnText);
					maxTimesTieZhang = parseInt(st.SelectRow("VIPType" , GetBDInfoInt("Serving" , 0).ToString())["EquipItemUPdate"].YuanColumnText); 
					timesTieZhang = GetBDInfoInt("EquipUpdateNum" , 0);
					if(timesTieZhang >= maxTimesTieZhang){
						isBuildCD = true;
					}
//					for(var rows : yuan.YuanMemoryDB.YuanRow in st.Rows){
//						//print(rows["TaskName"].YuanColumnText + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"); 
//					}
//				}
//			}
//			yield;
//		}
	SetEqupmentList("");
	while(invItemArray == null || invItemArray[0] == null){
		yield;
	}
	var useEBIFirst : EquepmentBuildItem; 
	useEBIFirst = invItemArray[0];
	jiaocheng.allJiaoCheng[2].obj[3] = useEBIFirst.gameObject;
	jiaocheng.allJiaoCheng[2].obj[4] = buttons[0];
	jiaocheng.allJiaoCheng[2].obj[5] = buttons[1];
//	jiaocheng.allJiaoCheng[2].obj[6] = buttons[0];
//	jiaocheng.allJiaoCheng[2].obj[7] = buttons[0];
//	jiaocheng.allJiaoCheng[2].obj[8] = buttons[1];
	SelectBeforeInv();
	myPanel.widgetsAreStatic = false;
	yield;
	yield WaitForSeconds(1);
	yield;
	myPanel.widgetsAreStatic = true;	
}

function GetBDInfoInt(bd : String , it : int) : int{  
	var iii : int = 0;
	try{  
//		//print(InventoryControl.yt.Rows[0].ContainsKey("EquipUpdateNum"));
		iii = parseInt(InventoryControl.yt.Rows[0][bd].YuanColumnText);
		return  iii;
	}catch(e){
		return it;	
	}
}

var ebItem : EquepmentBuildItem;
var invItemArray : EquepmentBuildItem[];
var invParent : Transform; 
var GID : UIGrid;
var useInvID : String[];
var wuneirong : GameObject;
function SetEqupmentList(equStr : String){

	LabelIron.text = InventoryControl.yt.Rows[0]["MarrowIron"].YuanColumnText;
	LabelGold.text = InventoryControl.yt.Rows[0]["MarrowGold"].YuanColumnText;
	
	cantDaZao = false;
	isBuilding = false;
						TransBaoXian.localPosition.y = 0;
						TransBuildtiao.localPosition.y = 3000;
//	UIFilledBuildObj.SetActiveRecursively(false);
	invClear();
	
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
				inv5 = InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText;
	var i : int = 0;
	
	var useStr : String;
	var inv : InventoryItem;
	useStr = inv5 + inv1 + inv2+ inv3+ inv4; 
//	//print("useStr == " + useStr);
	useInvID = useStr.Split(Sstr.ToCharArray());
//	//print(useInvID.length + " == useInvID.length");
	try{
		for(i=0; i<useInvID.length; i++){
			if(useInvID[i] != "" && parseInt(useInvID[i].Substring(0,1)) < 7 && parseInt(useInvID[i].Substring(4,1)) > 1){
				wuneirong.transform.localPosition.y = 3000;
				var Obj : GameObject = Instantiate(ebItem.gameObject); 
				Obj.transform.parent = invParent;
				var useEBI : EquepmentBuildItem;
				useEBI = Obj.GetComponent(EquepmentBuildItem);
				useEBI.setInv(AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , inv));
				useEBI.ebCL = this; 
				addInvItem(useEBI);				
			}
		} 
	}catch(e){
	
	}
	yield;
	GID.repositionNow = true;
}

function resetList(){
	SetEqupmentList("");
}

function addInvItem(ebi : EquepmentBuildItem){
	var use : EquepmentBuildItem[]; 
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

var eii :  EquepmentItemInfo; 
private var myEBI : EquepmentBuildItem; 
var LabelUseGold : UILabel;
var LableUseIron : UILabel;
var LableUseGold : UILabel;
function SelectOneInv(ebi : EquepmentBuildItem , inv : InventoryItem){
//	isUseBloodStone = true;
//	UseBloodStone();
	var i : int = 0;
	for(i=0; i<invItemArray.length; i++){
		if(invItemArray[i] == ebi){
			myEBI = ebi;
			
			LabelUseBloodStone.text = "x" + GetCostblood(myEBI.inv).ToString(); 
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages056");
			AllManage.AllMge.Keys.Add((myEBI.inv.itemLevel*myEBI.inv.itemQuality*BtnGameManager.dicClientParms["Equepment"] ) + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelUseGold);
//			LabelUseGold.text =  "升级花费:      "+ (myEBI.inv.itemLevel*myEBI.inv.itemQuality*100 );
			eii.showEquItemInfo(inv,null);
			invItemArray[i].SelectMe();
			var numtBrokenMaterial : int[];
			var itemQuality : int = 0;
			itemQuality = inv.itemQuality <= 5 ? inv.itemQuality : (inv.itemQuality - 4);// 大于5的是套装
			numtBrokenMaterial = GetBrokenMaterial(inv.itemLevel , itemQuality);
			LableUseIron.text = AllManage.AllMge.Loc.Get("info882") + numtBrokenMaterial[0];
			LableUseGold.text = AllManage.AllMge.Loc.Get("info883") + numtBrokenMaterial[1];
		}else{
			invItemArray[i].DontSelectMe();		
		}
	}
//	print(PlayerPrefs.GetInt("BuildUseBloodStone" , 1) + " == cunde");
	if(PlayerPrefs.GetInt("BuildUseBloodStone" , 1) == 0){
		isUseBloodStone = true;
		SpriteUseBloodStone.spriteName = "UIM_Prompt_On";	
	}else{
		PlayerPrefs.SetInt("BuildUseBloodStone" , 1);
		SpriteUseBloodStone.spriteName = "UIM_Prompt_Off";		
	}
} 

function SelectFristInv(){
	var i : int = 0;
	for(i=0; i<invItemArray.length; i++){
			myEBI = invItemArray[i];
			LabelUseBloodStone.text = "x" + GetCostblood(myEBI.inv).ToString();
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages056");
			AllManage.AllMge.Keys.Add((invItemArray[i].inv.itemLevel*invItemArray[i].inv.itemQuality*100 ) + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelUseGold);
//			LabelUseGold.text =  "升级花费:      "+ (invItemArray[i].inv.itemLevel*invItemArray[i].inv.itemQuality*100 );
			yield;
			yield;
			yield;
			yield;
			eii.showEquItemInfo(invItemArray[i].inv,null);
			invItemArray[i].SelectMe();
			return;
	}
}

var SpriteUseBloodStone : UISprite;
var isUseBloodStone : boolean = false;
var LabelUseBloodStone : UILabel;
function UseBloodStone(){
	if(!isUseBloodStone){
		isUseBloodStone = true;
		PlayerPrefs.SetInt("BuildUseBloodStone" , 0);
		SpriteUseBloodStone.spriteName = "UIM_Prompt_On";
	}else{
		PlayerPrefs.SetInt("BuildUseBloodStone" , 1);
		isUseBloodStone = false;
		SpriteUseBloodStone.spriteName = "UIM_Prompt_Off";	
	}
//	print(PlayerPrefs.GetInt("BuildUseBloodStone" , 1) + " == cun2222de");
}

//var ts : TiShi;  
//var qr : QueRen;
private var ps : PlayerStatus;
var TransBaoXian : Transform;
var TransBuildtiao : Transform;
var UIFilledSpriteBuild : UIFilledSprite;
var UISpriteShan : UISprite;
var UIFilledlizi : ParticleEmitter;
var UIFilledBuildObj : GameObject;
var UILabelBuild : UILabel;
private var isBuilding : boolean = false;
function DoDaZao(){
	if(cantDaZao){
		return;
	}
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	UIFilledSpriteBuild.fillAmount = 0;
//	for(i=0; i<invItemArray.length; i++){ 
	if(!isBuildCD){
		SendDoDaZao();
	}else{
	if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
			AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsEquepmentBuild , NowCDTime , 0 , "" , gameObject , "YesDoCDTips");
	else
		YesDoCD();
	}
}

	function YesDoCDTips(objs : Object[]){
		 AllManage.qrStatic.ShowBuyQueRen1(gameObject , "YesDoCD" , "NoDoCD" , AllManage.AllMge.Loc.Get("info293")+ objs[2] +AllManage.AllMge.Loc.Get("info294"));
	}

var mianfeiTimes : int = 0;
var cantDaZao : boolean = false;
function SendDoDaZao(){
	var i : int = 0;
	if(!isBuilding){
		for(i=0; i<useInvID.length; i++){
			if(useInvID[i] == myEBI.inv.itemID){ 
				var useBlood : int = 0; 
				if(isUseBloodStone){
				
					useBlood = parseInt(myEBI.inv.itemBuild)*0.2+10;
				}else{
					useBlood = 0;
				}	
				if(parseInt(myEBI.inv.itemBuild) < GetMaxBuildPoint(myEBI.inv.itemQuality)){
					
					var numtBrokenMaterial : int[];
					var itemQuality : int = 0;
					
					var inv : InventoryItem;
					inv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , inv);
					
					itemQuality = inv.itemQuality <= 5 ? inv.itemQuality : (inv.itemQuality - 4);// 大于5的是套装
					numtBrokenMaterial = GetBrokenMaterial(inv.itemLevel , itemQuality);
					
					if(ps.isMoney(myEBI.inv.itemLevel*myEBI.inv.itemQuality*50) && ps.isBlood(useBlood) && ps.isBrokenMaterial(numtBrokenMaterial)){
						isBuilding = true;
						TransBaoXian.localPosition.y = 3000;
						TransBuildtiao.localPosition.y = 0;
						UILabelBuild.text = AllManage.AllMge.Loc.Get("messages058") + RandomBuild + "%";
//						UIFilledBuildObj.SetActiveRecursively(true);
						cantDaZao = true;
						while(UIFilledSpriteBuild.fillAmount < 1){
							UIFilledSpriteBuild.fillAmount += Time.deltaTime;
							yield;
						}
						cantDaZao = false;
						useTieZhan();
						var bool : boolean = false;
						bool = AllManage.InvclStatic.TutorialsDetectionAsID("24");
//						print(bool);
						if(isUseBloodStone){
							beforeID = i;
//								print(AllManage.jiaochengCLStatic.JiaoChengID);
//								print(AllManage.jiaochengCLStatic.step);
							if(AllManage.jiaochengCLStatic.JiaoChengID == 2 && AllManage.jiaochengCLStatic.step == 4 && bool){
								mianfeiTimes = 3;
							}
							if(mianfeiTimes > 0){
								mianfeiTimes -= 1;
							    //BuildOneID(useInvID[i] , i);
								PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().EquepmentBuild(useInvID[i] , i , 1, 0, 0 , false));
							}else{
								PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().EquepmentBuild(useInvID[i] , i , 1, 0 , 0 , true));
							}
						}else{
							beforeID = i;
							if(AllManage.jiaochengCLStatic.JiaoChengID == 2 && AllManage.jiaochengCLStatic.step == 4 && bool){
								mianfeiTimes = 3;
							}
							if(mianfeiTimes > 0){
								mianfeiTimes -= 1;
							    //BuildOneID(useInvID[i] , i);	
								PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().EquepmentBuild(useInvID[i] , i , 0, 0, 0 , false));
							}else{
								PanelStatic.StaticBtnGameManager.RunOpenLoading(function()	InRoom.GetInRoomInstantiate().EquepmentBuild(useInvID[i] , i , 0 , 0 , 0 , true));
							}
						}
					}else{
						if(! ps.isMoney(myEBI.inv.itemLevel*myEBI.inv.itemQuality*50)){
							AllManage.tsStatic.Show("tips011");							
						}else
						if(! ps.isBlood(useBlood)){
						    //AllManage.tsStatic.Show("tips060");	
						    BtnGameManagerBack.my.SwitchToStore();
						}
					}
				}else{
					AllManage.tsStatic.Show("tips010");				
				}
				return;
			}
		}
	}
}

function ReturnDoDaZao(bool : boolean){
	if(bool){
		if(!UIFilledlizi.emit){
			UILabelBuild.text = AllManage.AllMge.Loc.Get("info694");
			UIFilledlizi.emit = true;
			yield WaitForSeconds(lizi.particleCount);
			UIFilledlizi.emit = false;
		}
	}else{
		UILabelBuild.text = AllManage.AllMge.Loc.Get("tips009");
		UISpriteShan.enabled = true;
		yield WaitForSeconds(0.5);
		UISpriteShan.enabled = false;
	}
	yield WaitForSeconds(0.5);
	isBuilding = false;
	TransBaoXian.localPosition.y = 0;
	TransBuildtiao.localPosition.y = 3000;
//	UIFilledBuildObj.SetActiveRecursively(false);
}

function GetMaxBuildPoint(pin : int) : int{	
	var max : int = 0;
	var plevel : int = 0;
	plevel = parseInt(AllManage.psStatic.Level)* 6;
	switch(pin){
		case 1:
			max = plevel;
			break;
		case 2:
			max = plevel + 12;
			break;
		case 3:
			max = plevel + 24;
			break;
		case 4:
			max = plevel + 48;
			break;
		case 5:
			max = plevel + 96;
			break;
			
		case 6:
			max = plevel + 12;
			break;
		case 7:
			max = plevel + 24;
			break;
		case 8:
			max = plevel + 48;
			break;
		case 9:
			max = plevel + 96;
			break;
	}
	return max;
}

function YesDoCD(){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesDoCD , NowCDTime , 0 , "" , gameObject , "realYesDoCD");
//	AllManage.AllMge.UseMoney(0 , 12 - timeSpawn.TotalMinutes , UseMoneyType.YesDoCD , gameObject , "realYesDoCD");
//	if(ps.UseMoney(0,NowCDTime)){
//	}
}

function realYesDoCD(){
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.UpdaetEquipRefreshCool).ToString());
		isBuildCD = false;
		timesTieZhang = 0; 
		InventoryControl.yt.Rows[0]["EquipUpdateNum"].YuanColumnText = timesTieZhang.ToString();
		InventoryControl.yt.Rows[0]["EquipUpdateCD"].YuanColumnText = "1900-01-01 00:00:00";

}

function NoDoCD(){
	
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



private var invcl : InventoryControl;
var lizi : ParticleEmitter;
var SuccessfulPar : ParticleEmitter;
function BuildOneID(invID : String , id : int){ 
//	//print("zhe li la a ");
	if(lizi.emit){
		return;
	}
	InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.UpdateEquip).ToString());
	InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.Strengthen , invID);			
	var m : int = 6;
	var useInt : int;
	var useStr : String;
//	//print(invID);
	var invitemQuality : int;
	invitemQuality =  parseInt(invID.Substring(4,1));
	if(invitemQuality >= 6){
		invitemQuality -= 4;
	}
	switch(invitemQuality){
		case 1: m = 0; break;
		case 2: m = 4; break;
		case 3: m = 8; break;
		case 4: m = 12; break;
		case 5: m = 12; break;
	}
		InventoryControl.yt.Rows[0]["AimUpdateEquip"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimUpdateEquip"].YuanColumnText) + 1).ToString();
	
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
//	//print(inv1 + inv2 + inv3 + inv4 + inv5);
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
				inv5 = InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText;
	
//	yield;
//	//print(InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText + " == zhuangbei 1");
	SetEqupmentList("");
//	yield;
//	yield;
	invcl.isUpdatePhoton = false;
	invcl.ReInitItem1();
	SelectBeforeInv(useInvID[id]); 
	shanguang();
	isBuilding = false;
//	//print(InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText + " == zhuangbei 2");
//	InventoryControl.yt.Rows[0]["EquipItem"].YuanColumnText = inv5;
}
var beforeID : int;
function RefreshList(){
	SetEqupmentList("");
	invcl.isUpdatePhoton = false;
	invcl.ReInitItem1();
	SelectBeforeInv(useInvID[beforeID]); 
	shanguang();
	
}

function shanguang(){
	if(!lizi.emit){
		lizi.emit = true;
		yield WaitForSeconds(lizi.particleCount);
		lizi.emit = false;
	}

}

function SuccessfulNow(){
	if(!SuccessfulPar.emit){
		SuccessfulPar.emit = true;
		yield WaitForSeconds(SuccessfulPar.particleCount);
		SuccessfulPar.emit = false;
	}

}

var stime : float;
function Update(){
	if(stime < Time.time){
		stime = Time.time + 0.1;
		lookTieZhan();
		if(cdTime > 0){
			cdTime -= 0.1;
		}
		if(cdTime <0){
			cdTime = 0;
		}
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
var isBuildCD : boolean = false;
var LabelCDTime : UILabel; 
var LabCDtim : UILabel ;
var LabelBuildRandom : UILabel;
var timeSpawn : System.TimeSpan;
var NowCDTime : int; 
var RandomBuild : int;
function lookTieZhan(){
//	nowNeedTime = timesTieZhang / maxTimesTieZhang * AllTime;
//	if(cdTime < nowNeedTime){    
	if(myEBI != null && myEBI.inv != null){
		RandomBuild = GetSuccess(myEBI.inv);
	}
//	RandomBuild = 90 - Mathf.Abs(30 - InRoom.GetInRoomInstantiate().serverTime.Minute); 
	if(isUseBloodStone){
			AllManage.AllMge.Keys.Clear();
	    //AllManage.AllMge.Keys.Add("messages057");
			AllManage.AllMge.Keys.Add("messages058");
			AllManage.AllMge.Keys.Add("info1195");// 高
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildRandom);
			LabelBuildRandom.color = Color.green;// 设置颜色
//		 LabelBuildRandom.text  = "强化成功率 ： 100%";	
	}else{
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages058");
			if(RandomBuild >= 80 && RandomBuild < 100)
			{
			    AllManage.AllMge.Keys.Add("info1195");// 高
			    AllManage.AllMge.SetLabelLanguageAsID(LabelBuildRandom);
			    LabelBuildRandom.color = Color.green;// 设置颜色
			}
			else if(RandomBuild >= 40 && RandomBuild < 80)
			{
			    AllManage.AllMge.Keys.Add("info1196");// 中
			    AllManage.AllMge.SetLabelLanguageAsID(LabelBuildRandom);
			    LabelBuildRandom.color = Color.yellow;// 设置颜色
			}
			else
			{
			    AllManage.AllMge.Keys.Add("info1197");// 低
			    AllManage.AllMge.SetLabelLanguageAsID(LabelBuildRandom);
			    LabelBuildRandom.color = Color.red;// 设置颜色
			}
//			Debug.Log("强化成功率-----------------" + RandomBuild);
			//AllManage.AllMge.Keys.Add(RandomBuild + "");
			//AllManage.AllMge.Keys.Add("%");
			//AllManage.AllMge.SetLabelLanguageAsID(LabelBuildRandom);
//		LabelBuildRandom.text  = "强化成功率 ： "+ RandomBuild +"%";	
	}
	if(isBuildCD && InventoryControl.yt.Rows[0]["EquipUpdateCD"].YuanColumnText != ""){
		timeSpawn = InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(InventoryControl.yt.Rows[0]["EquipUpdateCD"].YuanColumnText);
		if(timeSpawn.TotalMinutes < 5){ 
			NowCDTime = 5 - timeSpawn.TotalMinutes;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(NowCDTime + "");
			AllManage.AllMge.Keys.Add("messages059");
			AllManage.AllMge.SetLabelLanguageAsID(LabelCDTime);
			LabCDtim.enabled = true;
//			LabelCDTime.text = NowCDTime+ "分";	
			Filledcd.fillAmount = 1; 
		}else{ 
			timesTieZhang = 0;
			InventoryControl.yt.Rows[0]["EquipUpdateNum"].YuanColumnText = timesTieZhang.ToString();
			isBuildCD = false;
			Filledcd.fillAmount = 0;
			LabelCDTime.text = "";
			
			LabCDtim.enabled = false;
			
		}
	}else{
		var f1 : float;
		var f2 : float;
		f1 = timesTieZhang;
		f2 = maxTimesTieZhang;
		Filledcd.fillAmount = f1 / f2;
		LabelCDTime.text = "";	
	
			LabCDtim.enabled = false;

	}
//	var useInt : int = 0; 
//	useInt = cdTime /  AllTime * maxTimesTieZhang;
//	timesTieZhang = useInt; 
//	if(usetimes <= timesTieZhang){
//		usetimes = timesTieZhang;
//	}
//	}
//	Filledcd.fillAmount = cdTime / AllTime;
//	//print(timesTieZhang);
}

var usetimes : float;
function useTieZhan(){
	//print(timesTieZhang + " == " + maxTimesTieZhang);
	if(timesTieZhang < maxTimesTieZhang){
		timesTieZhang += 1;  
//		//print(InventoryControl.yt.Rows[0].ContainsKey("EquipUpdateNum"));
		InventoryControl.yt.Rows[0]["EquipUpdateNum"].YuanColumnText = timesTieZhang.ToString();
		usetimes = timesTieZhang;
		cdTime += 1 / maxTimesTieZhang * AllTime;
		if(timesTieZhang >= maxTimesTieZhang){
			isBuildCD = true;
			Filledcd.fillAmount = 1; 
//			//print( InRoom.GetInRoomInstantiate().serverTime.ToString("yyyy-MM-dd HH:mm:ss") + " = servertime");
			InventoryControl.yt.Rows[0]["EquipUpdateCD"].YuanColumnText = InRoom.GetInRoomInstantiate().serverTime.ToString("yyyy-MM-dd HH:mm:ss");
		}
	}
}

function GetBrokenMaterial(level : int , quality : int ) : int[] {
	var consumableValue : int[] = new int[2]; //0精铁，1精金
	for(var row : yuan.YuanMemoryDB.YuanRow in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytEquipmentenhance.Rows){
		if(level.ToString() == row["level"].YuanColumnText){
			switch(quality){
				case 1:
					consumableValue[0] = parseInt(row["WhiteIron"].YuanColumnText);
					consumableValue[1] = parseInt(row["WhiteGold"].YuanColumnText);
					break;
				case 2:
					consumableValue[0] = parseInt(row["GreenIron"].YuanColumnText);
					consumableValue[1] = parseInt(row["GreenGold"].YuanColumnText);
					break;
				case 3:
					consumableValue[0] = parseInt(row["BlueIron"].YuanColumnText);
					consumableValue[1] = parseInt(row["BlueGold"].YuanColumnText);
					break;
				case 4:
					consumableValue[0] = parseInt(row["PurpleIron"].YuanColumnText);
					consumableValue[1] = parseInt(row["PurpleGold"].YuanColumnText);
					break;
				case 5:
					consumableValue[0] = parseInt(row["OrangeIron"].YuanColumnText);
					consumableValue[1] = parseInt(row["OrangeGold"].YuanColumnText);
					break;
			}
		}
	}
	return consumableValue;
}

function show0(){
	cantDaZao = false;
	TransBaoXian.localPosition.y = 0;
	TransBuildtiao.localPosition.y = 3000;
	invcl.UpdatePhotonEquep();
	AllManage.UIALLPCStatic.show0();
}
 
	function GetCostblood(inv : InventoryItem) : int{
//		var float1 : float = 0;
//		var float2 : float = 0;
//		var float3 : float = 0;
//		float1 = parseInt(inv.itemBuild);
//		float2 = GetUpPoint(inv.itemQuality);
//		float3 = inv.itemQuality;
//		return float1/float2 * 5 / 4 * float3;
		return	parseInt(inv.itemBuild)*0.2+10;
	}
 
 function GetUpPoint(invitemQuality : int ) : int{
 	var m : int = 0;
	switch (invitemQuality)
	{
		case 1: m = 0; break;
		case 2: m = 4; break;
		case 3: m = 8; break;
		case 4: m = 12; break;
		case 5: m = 12; break;
	}
                  return m;
 }
 
 function GetSuccess(inv : InventoryItem) : int{
	 	return Mathf.Clamp((1000-(inv.ATzongfen*0.5+parseInt(inv.itemBuild))/1000),0.01,0.8) * 100;
 }
 
 function GetPointAsQuality(qua : int) : int{
 	switch(qua){
 		case 1:
 			return 100;
 			break;
 		case 2:
  			return 12;
			break;
 		case 3:
  			return 18;
			break;
 		case 4:
  			return 54;
			break;
 		case 5:
  			return 102;
			break;
 	}
 }
 
 