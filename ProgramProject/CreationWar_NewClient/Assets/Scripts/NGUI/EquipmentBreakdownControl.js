#pragma strict
private var Fstr : String = "=";
private var Sstr : String = ";";
var inv1 : String;
var inv2 : String;
var inv3 : String;
var inv4 : String;
static var me : GameObject;
var LabelIron : UILabel;
var LabelGold : UILabel;
var LabelUSIron : UILabel;
var LabelUSGold : UILabel;
var LabelUSMoney : UILabel;
function Awake(){
	AllManage.equipBreakCL = this;
	me = gameObject;
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
				mm = true;
				OneTime = true;
			}
			yield;
		}	
	}
	SetEqupmentList("");
	while(invItemArray == null || (invItemArray.length > 0 && invItemArray[0] == null)){
		yield;
	}
//	var useEBIFirst : EquipmentBreakdownItem; 
//	useEBIFirst = invItemArray[0];
//	jiaocheng.allJiaoCheng[2].obj[3] = useEBIFirst.gameObject;
//	jiaocheng.allJiaoCheng[2].obj[4] = buttons[2];
//	jiaocheng.allJiaoCheng[2].obj[5] = buttons[0];
//	jiaocheng.allJiaoCheng[2].obj[6] = buttons[0];
//	jiaocheng.allJiaoCheng[2].obj[7] = buttons[0];
//	jiaocheng.allJiaoCheng[2].obj[8] = buttons[1];
	SelectBeforeInv();
	myPanel.widgetsAreStatic = false;
	yield;
	ShowSelectInventory();
	yield WaitForSeconds(1);
	yield;
	myPanel.widgetsAreStatic = true;	
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

var ebItem : EquipmentBreakdownItem;
var invItemArray : EquipmentBreakdownItem[];
var invParent : Transform; 
var GID : UIGrid;
var useInvID : String[];
var wuneirong : GameObject;
var breakPanel : UIPanel;
function SetEqupmentList(equStr : String){

	LabelIron.text = InventoryControl.yt.Rows[0]["MarrowIron"].YuanColumnText;
	LabelGold.text = InventoryControl.yt.Rows[0]["MarrowGold"].YuanColumnText;
	cantDaZao = false;
	isBuilding = false;
	TransBaoXian.localPosition.y = 0;
	TransBuildtiao.localPosition.y = 3000;
	invClear();
	
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
	var i : int = 0;
	
	var useStr : String;
	var inv : InventoryItem;
	useStr = inv1 + inv2+ inv3+ inv4; 
//	//print("useStr == " + useStr);
	useInvID = useStr.Split(Sstr.ToCharArray());
//	//print(useInvID.length + " == useInvID.length");
	for(i=0; i<useInvID.length; i++){
		if(useInvID[i] != "" && parseInt(useInvID[i].Substring(0,1)) < 7){
			wuneirong.transform.localPosition.y = 3000;
			var Obj : GameObject = Instantiate(ebItem.gameObject); 
			Obj.transform.parent = invParent;
			var useEBI : EquipmentBreakdownItem;
			useEBI = Obj.GetComponent(EquipmentBreakdownItem);
			useEBI.setInv(AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , inv));
//			useEBI.ebCL = this; 
			addInvItem(useEBI);				
		}
	} 
	yield;
	GID.repositionNow = true;
	breakPanel.transform.localPosition.y = 0;
	breakPanel.clipOffset.y = 0;

	ResetUIPanel();
}

    function ResetUIPanel()
    {
        yield WaitForSeconds(1.7f);
        if(breakPanel.transform.localPosition.y != 0)
        {
            breakPanel.transform.localPosition.y = 0;
            breakPanel.clipOffset.y = 0;
        }
    }

function OnEnable(){
	resetList();
}

function resetList(){
	yield SetEqupmentList("");
	SelectFristInv();
}

function addInvItem(ebi : EquipmentBreakdownItem){
	var use : EquipmentBreakdownItem[]; 
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
private var myEBI : EquipmentBreakdownItem; 
var LabelUseGold : UILabel;
var lastSelectItemID : String = "";
function SelectOneInv(ebi : EquipmentBreakdownItem , inv : InventoryItem){
	var i : int = 0;
	lastSelectItemID = inv.itemID;
	for(i=0; i<invItemArray.length; i++){
		if(invItemArray[i] == ebi){ 
			myEBI = ebi;
			LabelUseBloodStone.text = "x" + ((inv.itemLevel + inv.itemQuality) / 2).ToString(); 
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages056");
			AllManage.AllMge.Keys.Add((myEBI.inv.itemLevel*myEBI.inv.itemQuality*100 ) + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelUseGold);
			eii.showEquItemInfo(inv,null);
			invItemArray[i].SelectMe();
			var numtBrokenMaterial : int[];
			numtBrokenMaterial = GetBrokenMaterial(inv.itemLevel , inv.itemQuality , inv.itemBuild);
			if(!onSelect1 && !onSelect2 && !onSelect3){
				if(PlayerPrefs.GetInt("BuildUseBloodStone" , 1) == 0)
				{
				    LabelUSIron.text = ""+ 2 * numtBrokenMaterial[0];
				    LabelUSGold.text = ""+ 2 * numtBrokenMaterial[1];
				}
				else
				{
				    LabelUSIron.text = ""+numtBrokenMaterial[0];
				    LabelUSGold.text = ""+numtBrokenMaterial[1];
				}
			}
		}else{
			invItemArray[i].DontSelectMe();		
		}
	}
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
			LabelUseBloodStone.text = "x" + ((invItemArray[i].inv.itemLevel + invItemArray[i].inv.itemQuality)/2 ).ToString();
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages056");
			AllManage.AllMge.Keys.Add((invItemArray[i].inv.itemLevel*invItemArray[i].inv.itemQuality*100 ) + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelUseGold);
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

		SelectOneInv(myEBI, myEBI.inv);
	}else{
		PlayerPrefs.SetInt("BuildUseBloodStone" , 1);
		isUseBloodStone = false;
		SpriteUseBloodStone.spriteName = "UIM_Prompt_Off";	
		SelectOneInv(myEBI, myEBI.inv);
	}
}

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
	if(spriteButnSelect1.spriteName == "UIH_Minor_Button_N"){
		if(cantDaZao){
			return;
		}
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		UIFilledSpriteBuild.fillAmount = 0;
		if(!isBuildCD){
			SendDoDaZao();
		}else{
			AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.YesDoCD , timeSpawn.TotalMinutes , 0 , "" , gameObject , "YesDoCDTips");
//			 AllManage.qrStatic.ShowBuyQueRen1(gameObject , "YesDoCD" , "NoDoCD" , AllManage.AllMge.Loc.Get("info293")+ NowCDTime +AllManage.AllMge.Loc.Get("info294"));
		}
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
				PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().EquipmentResolve(useInvID[i] , isUseBloodStone));
				return;
				var useBlood : int = 0; 
				if(isUseBloodStone){
				
					useBlood = myEBI.inv.itemLevel + myEBI.inv.itemQuality;
				}else{
					useBlood = 0;
				}	
				if(parseInt(myEBI.inv.itemBuild) < GetMaxBuildPoint(myEBI.inv.itemQuality)){
					if(ps.isMoney(myEBI.inv.itemLevel*myEBI.inv.itemQuality*100) && ps.isBlood(useBlood)){
						isBuilding = true;
						TransBaoXian.localPosition.y = 3000;
						TransBuildtiao.localPosition.y = 0;
						UILabelBuild.text = AllManage.AllMge.Loc.Get("messages058") + RandomBuild + "%";
						cantDaZao = true;
						while(UIFilledSpriteBuild.fillAmount < 1){
							UIFilledSpriteBuild.fillAmount += Time.deltaTime;
							yield;
						}
						cantDaZao = false;
						useTieZhan();
						var bool : boolean = false;
						bool = AllManage.InvclStatic.TutorialsDetectionAsID("26");
						if(isUseBloodStone){
							beforeID = i;
							if(AllManage.jiaochengCLStatic.JiaoChengID == 2 && AllManage.jiaochengCLStatic.step == 6 && bool){
								mianfeiTimes = 3;
							}
							if(mianfeiTimes > 0){
								mianfeiTimes -= 1;
								BuildOneID(useInvID[i] , i);							
							}else{
//								PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().EquepmentBuild(useInvID[i] , i , useBlood * (-1), myEBI.inv.itemLevel*myEBI.inv.itemQuality*100 * (-1), 0));
//								PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().EquepmentBuild(useInvID[i] , i , useBlood * (-1), myEBI.inv.itemLevel*myEBI.inv.itemQuality*100 * (-1), 0));
							}
						}else{
							beforeID = i;
							if(AllManage.jiaochengCLStatic.JiaoChengID == 2 && AllManage.jiaochengCLStatic.step == 6 && bool){
								mianfeiTimes = 3;
							}
							if(mianfeiTimes > 0){
								mianfeiTimes -= 1;
								BuildOneID(useInvID[i] , i);							
							}else{
//								PanelStatic.StaticBtnGameManager.RunOpenLoading(function()	InRoom.GetInRoomInstantiate().EquepmentBuild(useInvID[i] , i , 0 , myEBI.inv.itemLevel*myEBI.inv.itemQuality*100 * (-1) , 0));
							}
						}
					}else{
						if(! ps.isMoney(myEBI.inv.itemLevel*myEBI.inv.itemQuality*100)){
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
		RefreshList();
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
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesDoCD , timeSpawn.TotalMinutes , 0 , "" , gameObject , "realYesDoCD");
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

var lizi : ParticleEmitter;
function BuildOneID(invID : String , id : int){ 
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
	useInvID[id] = useInvID[id].Substring(0,15) + useStr + useInvID[id].Substring(18,7); 
	InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText = "";
	InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText = "";
	for(var i=0; i<useInvID.length; i++){	 
		if(i < 15 && i >= 0){
			InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 30 && i >=15){
			InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 45 && i >= 30){
			InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText += useInvID[i] + ";";		
		}else
		if(i < 60 && i >= 45){
			InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText += useInvID[i] + ";";		
		}
	}
				inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
				inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
				inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
				inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
	SetEqupmentList("");
	AllManage.InvclStatic.isUpdatePhoton = false;
	AllManage.InvclStatic.ReInitItem1();
	SelectBeforeInv(useInvID[id]); 
	shanguang();
	isBuilding = false;
}
var beforeID : int;
function RefreshList(){
	SetEqupmentList("");
	AllManage.InvclStatic.isUpdatePhoton = false;
	AllManage.InvclStatic.ReInitItem1();
//	SelectBeforeInv(useInvID[beforeID]); 
	shanguang();
	
}

function shanguang(){
	if(!lizi.emit){
		lizi.emit = true;
		yield WaitForSeconds(lizi.particleCount);
		lizi.emit = false;
	}

}

var stime : float;
//function Update(){
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
var LabelBuildRandom : UILabel;
var timeSpawn : System.TimeSpan;
var NowCDTime : int; 
var RandomBuild : int;
function lookTieZhan(){
	RandomBuild = 90 - Mathf.Abs(30 - InRoom.GetInRoomInstantiate().serverTime.Minute); 
	if(isUseBloodStone){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages057");
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildRandom);
	}else{
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages058");
			AllManage.AllMge.Keys.Add(RandomBuild + "");
			AllManage.AllMge.Keys.Add("%");
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildRandom);
	}
	if(isBuildCD && InventoryControl.yt.Rows[0]["EquipUpdateCD"].YuanColumnText != ""){
		timeSpawn = InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(InventoryControl.yt.Rows[0]["EquipUpdateCD"].YuanColumnText);
		if(timeSpawn.TotalMinutes < 5){ 
			NowCDTime = 5 - timeSpawn.TotalMinutes;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(NowCDTime + "");
			AllManage.AllMge.Keys.Add("messages059");
			AllManage.AllMge.SetLabelLanguageAsID(LabelCDTime);
			Filledcd.fillAmount = 1; 
		}else{ 
			timesTieZhang = 0;
			InventoryControl.yt.Rows[0]["EquipUpdateNum"].YuanColumnText = timesTieZhang.ToString();
			isBuildCD = false;
			Filledcd.fillAmount = 0;
			LabelCDTime.text = "";
		}
	}else{
		var f1 : float;
		var f2 : float;
		f1 = timesTieZhang;
		f2 = maxTimesTieZhang;
		Filledcd.fillAmount = f1 / f2;
		LabelCDTime.text = "";		
	}
}

function GetBrokenMaterial(level : int , quality : int , itemBuild : String) : int[] {
	var consumableValue : int[] = new int[2]; //0精铁，1精金
	for(var row : yuan.YuanMemoryDB.YuanRow in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytEquipmentresolve.Rows){
		if(level.ToString() == row["level"].YuanColumnText){
			switch(quality){
				case 1:
					consumableValue[0] = parseInt(row["IronWhite"].YuanColumnText);
					consumableValue[1] = parseInt(row["GoldWhite"].YuanColumnText);
					break;
				case 2:
					consumableValue[0] = parseInt(row["IronGreen"].YuanColumnText);
					consumableValue[1] = parseInt(row["GoldGreen"].YuanColumnText);
					break;
				case 3:
					consumableValue[0] = parseInt(row["IronBlue"].YuanColumnText);
					consumableValue[1] = parseInt(row["GoldBlue"].YuanColumnText);
					break;
				case 4:
					consumableValue[0] = parseInt(row["IronPurple"].YuanColumnText);
					consumableValue[1] = parseInt(row["GoldPurple"].YuanColumnText);
					break;
				case 5:
					consumableValue[0] = parseInt(row["IronOrange"].YuanColumnText);
					consumableValue[1] = parseInt(row["GoldOrange"].YuanColumnText);
					break;
			}
		}
	}
	if(itemBuild.Length > 0 && parseInt(itemBuild) > 0){
 			var m : int = 0;
            var plusBuild : int = 0;
            
			if(quality > 5){
				quality -= 4;
			}
            switch (quality)
            {
                case 1: m = 1; break;
                case 2: m = 4; break;
                case 3: m = 8; break;
                case 4: m = 12; break;
                case 5: m = 12; break;
            }
            plusBuild = parseInt(itemBuild) / m;

            if (plusBuild > 1)
            {
                consumableValue[0] *= plusBuild;
                consumableValue[1] *= plusBuild;
            }
	}
	return consumableValue;
}

var usetimes : float;
function useTieZhan(){
	if(timesTieZhang < maxTimesTieZhang){
		timesTieZhang += 1;  
		InventoryControl.yt.Rows[0]["EquipUpdateNum"].YuanColumnText = timesTieZhang.ToString();
		usetimes = timesTieZhang;
		cdTime += 1 / maxTimesTieZhang * AllTime;
		if(timesTieZhang >= maxTimesTieZhang){
			isBuildCD = true;
			Filledcd.fillAmount = 1; 
			InventoryControl.yt.Rows[0]["EquipUpdateCD"].YuanColumnText = InRoom.GetInRoomInstantiate().serverTime.ToString("yyyy-MM-dd HH:mm:ss");
		}
	}
}

function show0(){
	cantDaZao = false;
	TransBaoXian.localPosition.y = 0;
	TransBuildtiao.localPosition.y = 3000;
	AllManage.InvclStatic.UpdatePhotonEquep();
	AllManage.UIALLPCStatic.show0();
}

function ButnBatchDecomposition(){
	if(spriteButnSelect3.spriteName == "UIH_Minor_Button_N"){
		if(PlayerPrefs.GetInt("BuildUseBloodStone" , 1) == 0)
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().EquipmentResolveAll(GetSelectQuality() , true));
		else
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().EquipmentResolveAll(GetSelectQuality() , false));
	}
}

function ButnBatchSell(){
	if(spriteButnSelect2.spriteName == "UIH_Main_Button_N"){
		PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().EquipmentSellAll(GetSelectQuality()));
	}
}

var onSelect1 : boolean = false;
var onSelect2 : boolean = false;
var onSelect3 : boolean = false;

var spriteSelect1 : UISprite;
var spriteSelect2 : UISprite;
var spriteSelect3 : UISprite;
var spriteButnSelect1 : UISprite;
function ButnSelect1(){
	if(!onSelect1){
		spriteSelect1.spriteName = "UIB_System_Settings_A.png";
		onSelect1 = true;
	}else{
		spriteSelect1.spriteName = "UIB_System_Settings_N";
		onSelect1 = false;
	}
	GetAllCost();
	ShowSelectInventory();
}

var spriteButnSelect2 : UISprite;
function ButnSelect2(){
		if(!onSelect2){
			spriteSelect2.spriteName = "UIB_System_Settings_A.png";
			onSelect2 = true;
		}else{
			spriteSelect2.spriteName = "UIB_System_Settings_N";
			onSelect2 = false;
		}
		GetAllCost();
		ShowSelectInventory();
}

var spriteButnSelect3 : UISprite;
function ButnSelect3(){
		if(!onSelect3){
			spriteSelect3.spriteName = "UIB_System_Settings_A.png";
			onSelect3 = true;
		}else{
			spriteSelect3.spriteName = "UIB_System_Settings_N";
			onSelect3 = false;
		}
		GetAllCost();
		ShowSelectInventory();
}

function ShowSelectInventory(){
	if(!onSelect1 && !onSelect2 && !onSelect3){
		SetLabelText(LabelUSIron , "0");
		if(lastSelectItemID != ""){
			SelectBeforeInv(lastSelectItemID);		
		}else{
			SelectBeforeInv();
		}
		spriteButnSelect2.spriteName = "UIH_Minor_Button_O";
		spriteButnSelect3.spriteName = "UIH_Minor_Button_O";
		spriteButnSelect1.spriteName = "UIH_Minor_Button_N";		
	}else{
		spriteButnSelect2.spriteName = "UIH_Minor_Button_N";
		spriteButnSelect3.spriteName = "UIH_Minor_Button_N";		
		spriteButnSelect1.spriteName = "UIH_Minor_Button_O";
	}
}

var allCostMoney : int = 0;
var allCostIron : int = 0;
var allCostGold : int = 0;
function GetAllCost(){
	var invs1 : String;
	var invs2 : String;
	var invs3 : String;
	var invs4 : String;

	invs1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
	invs2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
	invs3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
	invs4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
	
	var i : int = 0;
	var useStr : String;
	var inv : InventoryItem;
	var numtBrokenMaterial : int[];
	var doublePlus : int = 0;
	if(PlayerPrefs.GetInt("BuildUseBloodStone" , 1) == 0)
	{
	   doublePlus = 2;
	}
	else
	{
	   doublePlus = 1;
	}			
	
	useStr = String.Format("{0}{1}{2}{3}" , invs1 , invs2 , invs3 , invs4);
	GetUseStr = useStr.Split(Sstr.ToCharArray());
	
	var inventoryItemIDs : String = "";
	if(onSelect1){
		inventoryItemIDs += FindItemsAsQuality(1);
	}
	if(onSelect2){
		inventoryItemIDs += FindItemsAsQuality(2);
	}
	if(onSelect3){
		inventoryItemIDs += FindItemsAsQuality(3);
	}
	
	allCostMoney  = 0;
	allCostIron  = 0;
	allCostGold  = 0;
	var UseInvs : String[];
	UseInvs = inventoryItemIDs.Split(";"[0]);
	
	for(i=0; i<UseInvs.length; i++){
		inv = AllResources.InvmakerStatic.GetItemInfo(UseInvs[i] , inv);
		if(inv){
			numtBrokenMaterial = GetBrokenMaterial(inv.itemLevel , inv.itemQuality , inv.itemBuild);
			allCostIron += numtBrokenMaterial[0] * doublePlus;
			allCostGold += numtBrokenMaterial[1] * doublePlus;
			allCostMoney += (ToInt.StrToInt(inv.costGold) + ToInt.StrToInt(inv.costBlood) * 500);
		}
	}
	
	SetLabelText(LabelUSMoney , allCostMoney.ToString());
	SetLabelText(LabelUSGold , allCostGold.ToString());
	SetLabelText(LabelUSIron , allCostIron.ToString());
}

private var GetUseStr : String[];
function FindItemsAsQuality(itemQuality : int) : String{
	var returnStr : String = "";
	for(var i=0; i<GetUseStr.length; i++){
		try{
			if(parseInt(GetUseStr[i].Substring(0,1)) < 7 && parseInt(GetUseStr[i].Substring(4,1)) == itemQuality){
				returnStr += GetUseStr[i] + ";";
			}
		}catch(e){
			
		}
	}
	return returnStr;
}

function SetLabelText(Label : UILabel , text : String){
	if(Label != null)
		Label.text = text;
	else
		print(String.Format("{0} text = {1}" , "SetLabelText null !!!!!!!!!!" , text));
}

function GetSelectQuality() : int[]{
		var useInt : int = 0;
		if(onSelect1)
			useInt += 1;
		if(onSelect2)
			useInt += 1;
		if(onSelect3)
			useInt += 1;
		var intS : int[] = new int[3];
		if(onSelect1)
			intS[0] = 1;
		if(onSelect2)
			intS[1] = 2;
		if(onSelect3)
			intS[2] = 3;
		return intS;
}
