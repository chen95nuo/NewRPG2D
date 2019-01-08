#pragma strict
class PersonSkill{
	var level : int[]; 
	var lvzhixian : int;
	var costPoint : int = 1;
	var gold : int[];
	var blood : int[];
}

var playerSkillStr : String;
var personSkills : PersonSkill[];
//var passiveSkills : Skillpassive[];
var skItems : SkillItem[];
var zxItems : SkillItem[];
var LabelSkillPoint : UILabel;
var ProID : int;
private var Fstr : String = ";";
var ASkill : ActiveSkill;
private var PlayerLevel : int;
var SkillPoint : int;
//var skillObjDet : SkillOjectDetails;
var buttonForTask : GameObject[];

var buttons : GameObject[];
function Awake(){
	if(! this.enabled){
		return;
	
	}
	transform.parent = null;
	AllManage.SkillCLStatic = this;
	AllManage.UICLStatic.SkillRetrun();
	
	AllManage.jiaochengCLStatic.allJiaoCheng[1].obj[2] = buttons[0];
	AllManage.jiaochengCLStatic.allJiaoCheng[1].obj[3] = buttons[1];
	AllManage.jiaochengCLStatic.allJiaoCheng[1].obj[4] = buttons[2];
	AllManage.jiaochengCLStatic.allJiaoCheng[9].obj[2] = buttons[3];
	AllManage.jiaochengCLStatic.allJiaoCheng[9].obj[3] = buttons[4];
	AllManage.jiaochengCLStatic.allJiaoCheng[9].obj[4] = buttons[5];
	
	mtw = AllManage.mtwStatic;
	if(PlayerStatus.MainCharacter){
		PlayerStatus.MainCharacter.SendMessage("initSkill" , SendMessageOptions.DontRequireReceiver);
	}
	showPlayer(parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText));
	SkillCostInit();
//	InvokeRepeating("ShowSkillCanUpDate",0,0.5f);
}

var LabelJiNeng : UILabel[];
function showPlayer(id : int){
	var i : int = 0;
	switch(id){
		case 1:
			for(i=0; i<LabelJiNeng.length; i++){
				LabelJiNeng[i].text = AllManage.AllMge.Loc.Get(AllManage.UICLStatic.JiNengText1[i]) ;
			}
			break;
		case 2:
			for(i=0; i<LabelJiNeng.length; i++){
				LabelJiNeng[i].text = AllManage.AllMge.Loc.Get(AllManage.UICLStatic.JiNengText2[i]);
			}
			break;
		case 3:
			for(i=0; i<LabelJiNeng.length; i++){
				LabelJiNeng[i].text = AllManage.AllMge.Loc.Get(AllManage.UICLStatic.JiNengText3[i]);
			}
			break;
	}
	ShowFuYeLevel();
}

function SkillCostInit(){
	var i : int = 0;
	for(i=0; i<personSkills.length; i++){
		for(var rows : yuan.YuanMemoryDB.YuanRow in YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytPlayerSkill.Rows){
			if(rows["proType"].YuanColumnText == InventoryControl.yt.Rows[0]["ProID"].YuanColumnText && rows["skillId"].YuanColumnText == i.ToString()){
				rearSkillInit(rows , personSkills[i]);
			}
		}
	}
	
//		useInt1 = i;
//		if((useInt1 >= 3 && useInt1 <= 8) || (useInt1 >= 17 && useInt1 <= 19)){
//			useInt2 = 1;
//		}else
//		if((useInt1 >= 9 && useInt1 <= 14) || (useInt1 >= 20 && useInt1 <= 22)){
//			useInt2 = 2;	
//		}
//		bool = GetIsBranch(useInt2 , i);
//		
//		if(i < 3 || i == 15 || i ==16){
//			bool = true;
//		}

}

function rearSkillInit(rows : yuan.YuanMemoryDB.YuanRow , personSkill : PersonSkill){
	var skillInfos : String[];
	var skillattrs : String[];
	var i : int = 0;
	var maxLevel : int = 0;
	maxLevel =	 parseInt(rows["maxLevel"].YuanColumnText);
	
	skillInfos = rows["levelattribut"].YuanColumnText.Split(";"[0]);
	personSkill.level = new int[maxLevel];
	personSkill.gold = new int[maxLevel];
	
	for(i=0; i<maxLevel; i++){
		skillattrs = skillInfos[i].Split(","[0]);
		if(skillattrs.length > 2){
			personSkill.level[i] = parseInt(skillattrs[3]);
			personSkill.gold[i] = parseInt(skillattrs[2]);
		}
	}
}

var FuFishLabel : UILabel;
var FuMiningLabel : UILabel;
var FuCookingLabel : UILabel;
var FuMakeLabel : UILabel;
function ShowFuYeLevel(){
	AllManage.AllMge.Keys.Clear();
	AllManage.AllMge.Keys.Add("messages062");
	AllManage.AllMge.Keys.Add(AllManage.InvclStatic.expFishing + "");
	AllManage.AllMge.SetLabelLanguageAsID(FuFishLabel);
							
	AllManage.AllMge.Keys.Clear();
	AllManage.AllMge.Keys.Add("messages062");
	AllManage.AllMge.Keys.Add(AllManage.InvclStatic.expMining + "");
	AllManage.AllMge.SetLabelLanguageAsID(FuMiningLabel);

	AllManage.AllMge.Keys.Clear();
	AllManage.AllMge.Keys.Add("messages062");
	AllManage.AllMge.Keys.Add(AllManage.InvclStatic.expCooking + "");
	AllManage.AllMge.SetLabelLanguageAsID(FuCookingLabel);

	AllManage.AllMge.Keys.Clear();
	AllManage.AllMge.Keys.Add("messages062");
	AllManage.AllMge.Keys.Add(AllManage.InvclStatic.expMake + "");
	AllManage.AllMge.SetLabelLanguageAsID(FuMakeLabel);
}

//function OnDisable(){
//	gameObject.active = true;
//}

var boolShowButton : boolean = false;
function showSkillCanUpdateButton(){
	for(var i=0; i<skItems.length; i++){
		if(skItems[i].thisSkillCanUpDate){
			boolShowButton = true;
			return;
		}
	}
	boolShowButton = false;
	return;
}

function Start () {
	useSKstr = new Array(0);
	var mm : boolean = false;
	while(!mm){
		if(InventoryControl.ytLoad){
			if(InventoryControl.yt.Rows.Count > 0){			
				playerSkillStr = InventoryControl.yt.Rows[0]["Skill"].YuanColumnText; 
				ProID = parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText); 	
				PlayerLevel = parseInt(AllManage.psStatic.Level);
				SaveSkillButtonStr = InventoryControl.yt.Rows[0]["SkillsPostion"].YuanColumnText;
				SetSaveButtons(SaveSkillButtonStr);
				SkillPoint = GetBDInfoInt("SkillPoint" , 0);
//					//print("sdfosdjflsdjflksdjflsdjflksjdfljsdflsdjf123123123");
//					//print(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText);
//					//print(InventoryControl.yt.Rows[0]["Skill"].YuanColumnText);
//					//print(InventoryControl.yt.Rows[0]["SkillsPostion"].YuanColumnText);
//				//print(playerSkillStr);	
			}
			PSkill = PlayerStatus.MainCharacter.gameObject.GetComponent(PassiveSkill);
			ASkill = PlayerStatus.MainCharacter.gameObject.GetComponent(ActiveSkill);
//			print(PSkill);
//			print(ASkill);
			if(ASkill != null && PSkill != null){
				mm = true;
				SetSkills(playerSkillStr);
				for(var i=0; i<STitem.length; i++){
					STitem[i].ASkill = ASkill;
				}
			}
		}
		yield;
	} 
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages096");
			AllManage.AllMge.Keys.Add(SkillPoint + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkillPoint);
//	print("------------- dao le zhe li le");
//	LabelSkillPoint.text = "技能点" + SkillPoint;
}

var mtw : MainTaskWork;
function lookQianZhiTaskAsID(id : String) : boolean{
	for(var ids : String in mtw.MainPS.player.doneTaskID){
		if(ids == id){
			return true;
		}
	}
	return false;
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
 
private var bool : boolean;
function TSSkillButton(){
	yield WaitForSeconds(2);
	bool = true;
	PlayerLevel = parseInt(AllManage.psStatic.Level);
	ShowSkillCanUpDate();
}

var SpriteBranchs : GameObject[];
function ShowSkillCanUpDate(){
	var i : int = 0;
	var useBool : boolean = false;
	var useInt : int = 0;
	switch(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText){
		case "0":
			SpriteBranchs[0].SetActive(false);
			SpriteBranchs[1].SetActive(false);
			break;
		case "1":
			SpriteBranchs[0].SetActive(true);
			SpriteBranchs[1].SetActive(false);
			break;
		case "2":
			SpriteBranchs[0].SetActive(false);
			SpriteBranchs[1].SetActive(true);
			break;
		case "3":
			SpriteBranchs[0].SetActive(true);
			SpriteBranchs[1].SetActive(true);
			break;
	}
	
	if(useSKstr.length <= 1){
		return;
	}
	var useInt1 : int;
	var useInt2 : int = 0;
	var boolIsBranch : boolean = true;
	for(i=0; i<23; i++){  
		var o : int;
		if(i < 15){
			o = personSkills[i].level[Mathf.Clamp(parseInt(ASkill.SkillP[i].level.ToString().Substring(0,1)),0,2)];		
		}else{
			o = personSkills[i].level[Mathf.Clamp(parseInt(PSkill.SkillP[i-15].level.ToString().Substring(0,1)),0,2)];	
		}
		useInt = parseInt(useSKstr[i].Substring(0,1));
		if(i < 15){	
			if(ASkill.SkillP[i].level != 0 && ASkill.SkillP[i].level.ToString().Length > 1){	
//				if(PlayerLevel >= personSkills[i].lvzhixian && parseInt(ASkill.SkillP[i].level.ToString().Substring(1,1)) == 0 && SkillPoint > personSkills[i].costPoint){
//				print(personSkills.length + " == " + i);
//				print(ASkill.SkillP.length + " == " + i + " == " + ASkill.SkillP[i].level.ToString());
				if(parseInt(ASkill.SkillP[i].level.ToString().Substring(1,1)) == 0 && SkillPoint >= personSkills[i].costPoint){
					if(bool){
						bool = false;
//						if(Application.loadedLevelName != "Map200"&&AllManage.UICLStatic.mapType == MapType.zhucheng){
//							AllManage.tsStatic.Show("tips042");
//						}
					}
					zxItems[i].ThisCanZhiXian();
				}else{
					zxItems[i].CantZhiXian();		
				}
			}else{
				zxItems[i].CantZhiXian();					
			}
		}
		useInt1 = i;
		if((useInt1 >= 3 && useInt1 <= 8) || (useInt1 >= 17 && useInt1 <= 19)){
			useInt2 = 1;
		}else
		if((useInt1 >= 9 && useInt1 <= 14) || (useInt1 >= 20 && useInt1 <= 22)){
			useInt2 = 2;	
		}
		bool = GetIsBranch(useInt2 , i);
		
		if(i < 3 || i == 15 || i ==16){
			bool = true;
		}
		if(PlayerLevel >= o && useInt < 3 && bool){
			skItems[i].ThisCanUpDate();
			if(bool){
				bool = false;
				if(Application.loadedLevelName != "Map200"&&AllManage.UICLStatic.mapType == MapType.zhucheng){
					AllManage.tsStatic.Show("tips042");
				}
			}
		}else{
			skItems[i].CantUpDate();
		} 
		if(! bool){
			skItems[i].notBranch();
		}
//		print(bool + " == " + i);
	}
	showSkillCanUpdateButton();
}

function GetIsBranch(useInt : int , i : int) : boolean{
	var useStr : String;
	useStr = InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText;
	if(useStr == "3"){
		return true;
	}else
	if(useStr == "0"){
		return false;
	}
//	print(useStr + " == " + useInt);
	switch(useInt){
		case 0:
			return true;
			break;
		case 1:
			if(useStr == "1"){
				return true;
			}else
			if(useStr == "2"){
				return false;
			}
			break;
		case 2:
			if(useStr == "1"){
				return false;
			}else
			if(useStr == "2"){
				return true;
			}
			break;
	}
}

function GetNextLL(id : int , lv : int){
	return personSkills[id].level[lv];	
}

var SkillViewButton : UISlicedSprite[];
var ButtonXidian : Transform;
//var ParentQieHuanTianFu : Transform;
function ButtonTianFu1(){
	TForFS = 0;
	SkillViewButton[0].spriteName = "UIH_Skill_A";
	SkillViewButton[1].spriteName = "UIH_Talent_N";	
	ShowSkillView(NowGrop);
	ButtonXidian.localPosition.y = 1000;
//	ParentQieHuanTianFu.localPosition.y = 0;
}
function ButtonTianFu2(){
	TForFS = 1;
	SkillViewButton[0].spriteName = "UIH_Skill_N";
	SkillViewButton[1].spriteName = "UIH_Talent_A";	
	ShowSkillView(NowGrop);
	ButtonXidian.localPosition.y = 0;
//	ParentQieHuanTianFu.localPosition.y = 0;
}

function Buttonxidian(){
	var j : int = 0;
//	if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1){
		if(IsRight==true){
			IsRight = false;
			var times : int = parseInt(InventoryControl.yt.Rows[0]["WashAttributePoints"].YuanColumnText);
			var cBlood : int = 0;
			if(times==0){
				cBlood = 500;
			}else {
				if(times>5){
					cBlood=1000;
				}else {
					cBlood=500+times*100;
				}			
			}
			
			AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.WashAttributePoints , 0 , 0 , "" , gameObject , "YesXiDianTips");
//			AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesXiDian" , "NoMai" , AllManage.AllMge.Loc.Get("info298")+cBlood+AllManage.AllMge.Loc.Get("info297")+AllManage.AllMge.Loc.Get("meg0171"));
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.RefreshTalentPoint).ToString());
		}else{
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1025"));
		}
//	}else{
//		YesXiDian();
//	}
}

	function YesXiDianTips(objs : Object[]){
		AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesXiDian" , "NoMai" , AllManage.AllMge.Loc.Get("info298")+objs[2]+AllManage.AllMge.Loc.Get("info297")+AllManage.AllMge.Loc.Get("meg0171"));
	}

function YesXiDian(){
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.WashAttributePoints , 0 , 0 , "" , gameObject , "realYesXiDian");
//	AllManage.AllMge.UseMoney(0 , 50 , UseMoneyType.WashAttributePoints , gameObject , "realYesXiDian");
	
//	if(ps.UseMoney(0,50)){
//	}
}

function realYesXiDian(){
		var bool1 : boolean = false;
		var bool2 : boolean = false;
		var bool3 : boolean = false;
		
		var i : int = 0; 
		useSKstr = playerSkillStr.Split(Fstr.ToCharArray());	
		playerSkillStr = "";
		if(useSKstr.Length > 1){
			for(i=0; i<24; i++){ 
				bool1 = i < 3;
				bool2 = (i >= 3 && i <= 8 && GetBDInfoInt("SkillsBranch" , 0) == 1);
				bool3 = (i >= 9 && i <= 14 && GetBDInfoInt("SkillsBranch" , 0) == 2);
				if(bool1 || bool2 || bool3){
					if(useSKstr[i].length >0){
						playerSkillStr += useSKstr[i].Substring(0,1) + "0" + ";";
					}				
				}else{
					if(useSKstr[i].length >0){
						playerSkillStr += useSKstr[i].Substring(0,2)+ ";";
					}				
				
				}
			}
		} 
		useSKstr = playerSkillStr.Split(Fstr.ToCharArray()); 
		InventoryControl.yt.Rows[0]["Skill"].YuanColumnText = playerSkillStr;
		InventoryControl.yt.Rows[0]["SkillPoint"].YuanColumnText = AllManage.psStatic.Level;
		SkillPoint = parseInt(InventoryControl.yt.Rows[0]["SkillPoint"].YuanColumnText);
		
		SetSkills(playerSkillStr);
		ShowSkillCanUpDate();
}

var numBranch : int = 0;
function SaveAllBranch(){
	numBranch=0;
	for(var i=0; i<zxItems.length; i++){
		if( zxItems[i].ComputeUpdateBranch()  && i != 0 && i != 1 && i != 2){
			numBranch += 1;
		}
	}
	if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1 && AllManage.jiaochengCLStatic.JiaoChengID != 9 && AllManage.jiaochengCLStatic.step != 4 && numBranch != 0)
			AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.SKILLDeviation , numBranch , 0 , "" , gameObject , "TipsSaveBranchTips");
//		AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"TipsSaveBranch" , "" , AllManage.AllMge.Loc.Get("meg0169")+(numBranch*100)+AllManage.AllMge.Loc.Get("info297") + AllManage.AllMge.Loc.Get("meg0172"));	
	else
		TipsSaveBranch();
		
}

	function TipsSaveBranchTips(objs : Object[]){
		AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"TipsSaveBranch" , "" , AllManage.AllMge.Loc.Get("meg0169")+objs[2]+AllManage.AllMge.Loc.Get("info297") + AllManage.AllMge.Loc.Get("meg0172"));	
	}

function TipsSaveBranch(){
	AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.SKILLDeviation , numBranch, 0 , "" , gameObject , "RealSaveAllBranch");
}

var IsRight : boolean = true;
function RealSaveAllBranch(){
	for(var i=0; i<zxItems.length; i++){
		zxItems[i].UpdateBranchSkill();
		IsRight = true;
	}
		SetSkills(playerSkillStr);
		ShowSkillCanUpDate();
}

function NoMai(){
		IsRight = true;
}

function SkillInit(){
//	print("sd;fksld;fkladsjgfldkjfglfdkjsglkdfjgldkfjglfdkjg");
	yield;
	yield;
	yield;
	yield;
	yield;
	SkillPoint = parseInt(InventoryControl.yt.Rows[0]["SkillPoint"].YuanColumnText);		
	SetSkills(playerSkillStr);
	ShowSkillCanUpDate();
}

private var NowGrop : int = 0;
private var TForFS : int = 0;
function ButtonSkillPage1(){
	ShowSkillView(0);
}
private var ps : PlayerStatus;
//var ts : TiShi;
var bchC : BranchControl;
function ButtonSkillPage2(){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	
	if(ps != null){
//		if(ps.weaponType == PlayerWeaponType.weapon1 || parseInt(ps.Level) >= 60){
//			if(lookQianZhiTaskAsID("23")){	
//				if(GetBDInfoInt("SkillsBranch" , 0) == 1 || GetBDInfoInt("SkillsBranch" , 0) == 3){
					ShowSkillView(1);
//				}else{
//					AllManage.tsStatic.Show("当前不可用。");
//				}
//			}else{
//					AllManage.tsStatic.Show("需要先完成职业之路");		
//			}
//		}
	}
}
function ButtonSkillPage3(){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	
	if(ps != null){
//		if(ps.weaponType == PlayerWeaponType.weapon2 || parseInt(ps.Level) >= 60){
//			if(lookQianZhiTaskAsID("23")){	
//				if(GetBDInfoInt("SkillsBranch" , 0) == 2 || GetBDInfoInt("SkillsBranch" , 0) == 3){
					ShowSkillView(2);
//				}else{
//					AllManage.tsStatic.Show("当前不可用。");
//				}
//			}else{
//					AllManage.tsStatic.Show("需要先完成职业之路");		
//			}
//		}
	}
}

var SkillPageButtons : UISlicedSprite[];
var SKillGroup1 : Transform[];
var SKillGroup2 : Transform[];
function ShowSkillView(id : int){
	NowGrop = id;
	for(var i=0; i<SkillPageButtons.length; i++){
		if(i == NowGrop){
			if(TForFS == 0){
				SKillGroup1[i].transform.localPosition.y = 0;
				SKillGroup2[i].transform.localPosition.y = 1000;
			}else{
				SKillGroup1[i].transform.localPosition.y = 1000;
				SKillGroup2[i].transform.localPosition.y = 0;			
			}
//			SkillPageButtons[i].depth = 2;
			SkillPageButtons[i].spriteName = "UIH_Select_Unions_A";
		}else{	
			SKillGroup1[i].transform.localPosition.y = 1000;
			SKillGroup2[i].transform.localPosition.y = 1000;			
//			SkillPageButtons[i].depth = -2;
			SkillPageButtons[i].spriteName = "UIH_Select_Unions_N";
		}
	}
}

var useSKstr : String[];
function SetSkills(SkillStr : String){
	var i : int = 0;
	useSKstr = SkillStr.Split(Fstr.ToCharArray());	
	if(useSKstr.Length <= 1){
		for(i=0; i<24; i++){
			playerSkillStr += "00" + ";";
		}
	}
	useSKstr = playerSkillStr.Split(Fstr.ToCharArray());
	var useSkillObj : Skillclass; 
	var usePassSkillobj : Skillpassive;
	for(i=0; i<23; i++){
//		//print("useSKstr[i] == "+ useSKstr[i] + "=="+parseInt(skItems[i].skillID));
		if(useSKstr[i] != ""){
			if(i < 15){
				useSkillObj = ASkill.GetSkillInfoAsID(ProID*100 + parseInt(skItems[i].skillID));
//				useSkillObj = ASkill.GetSkillInfoAsID(ProID*100 + parseInt(useSKstr[i]));
//				//print(useSkillObj);
//				//print(useSkillObj + " == name");
				skItems[i].SetSkill(useSKstr[i],useSkillObj , 0);
				zxItems[i].SetSkill(useSKstr[i],useSkillObj , personSkills[i].costPoint);			
			}else{ 
//				//print("parseInt(skItems[i].skillID) == " + parseInt(skItems[i].skillID));
				usePassSkillobj = PSkill.GetSkillInfoAsID(parseInt(skItems[i].skillID)); 
				skItems[i].SetPassSkill(useSKstr[i],usePassSkillobj);
			}
		}
	} 
//	//print(playerSkillStr + " === 123");
	InventoryControl.yt.Rows[0]["Skill"].YuanColumnText = playerSkillStr;
	ASkill.SetSkillLevel(useSKstr);
	PSkill.SetSkillLevel(useSKstr);
//	SetButtonSkills();
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages096");
			AllManage.AllMge.Keys.Add(SkillPoint + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkillPoint);
	LabelSkillPoint.text = AllManage.AllMge.Loc.Get( "messages096" ) + "[64ffff]" + SkillPoint;
	ShowSkillCanUpDate();
}

var SkillBottons : SkillItem[];
function SetButtonSkills(){
//	var str : String;
//	for(var i=0; i<4; i++){
//		str = PlayerPrefs.GetString("saveSkill" + i,"");
//		if(str != ""){
//			SkillBottons[i].StartSetSkill(str);
//		}
//	}
}

function GetThisSkillInfo(str : String){ 
	if(parseInt(str) < 16){
		if(ASkill){
			ASkill.GetSkillInfoAsID(ProID*100 + parseInt(str));
		}		
	}else{ 
		if(PSkill){
			PSkill.GetSkillInfoAsID(ProID*10 + parseInt(str));		
		}
	}
}

var LabelSkillInfo : UILabel;
function showOnClickSkillInfo(str : String) : String{
	var info : String;
//	if(parseInt(str) < 16){	
//		LabelSkillInfo.text = ASkill.GetSkillInfoAsID(ProID*100 + parseInt(str)).explain;
//	}else{
//		LabelSkillInfo.text = PSkill.GetSkillInfoAsID(parseInt(str)).explain;	
//	}
	try{
		if(parseInt(str) < 16){	
			info = ASkill.GetSkillAsID(ProID*100 + parseInt(str)).explain;
		}else{
			info = PSkill.GetSkillexplainAsID(parseInt(str)).explain;	
		}
	}catch(e){
		return "";
	}
	return info;
}

function showFenZhiInfo(Skill : String , id : int) : boolean{
	var useInt1 : int = 0;
	var useInt2 : String = "";
	useInt1 = parseInt(Skill) - 1;
	if((useInt1 >= 3 && useInt1 <= 8)){
		useInt2 = "1";
	}else
	if((useInt1 >= 9 && useInt1 <= 14)){
		useInt2 = "2";	
	}
	if(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText == "1" || InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText == "2"){
		if(useInt2 != InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText && useInt1 >= 3){
			return false;
		}
	}

	if(id == 1){	
		LabelSkillInfo.text = AllManage.AllMge.Loc.Get( ASkill.GetSkillInfoAsID(ProID*100 + parseInt(Skill)).Branch1 );
	}else{
		LabelSkillInfo.text = AllManage.AllMge.Loc.Get( ASkill.GetSkillInfoAsID(ProID*100 + parseInt(Skill)).Branch2 );		
	}
	return true;
}

var StrFuZhus : String[];
function showFuZhuInfo(id : int){
	var str : String;
//	LabelSkillInfo.text = AllManage.AllMge.Loc.Get( StrFuZhus[id] );
	str = AllManage.AllMge.Loc.Get( StrFuZhus[id] );
	return str;
}

function getSkill(o : ActiveSkill){
	ASkill = o;
}
var PSkill : PassiveSkill;
function getSkill1(o : PassiveSkill){
	PSkill = o;
}

function UseSkills(id : String) : boolean{
//	//print(id);
	var useInt1 : int = 0;
	var useInt2 : int = 0;
	useInt1 = parseInt(id);
	if((useInt1 >= 4 && useInt1 <= 9) || (useInt1 >= 18 && useInt1 <= 20)){
		useInt2 = 1;
	}else
	if((useInt1 >= 10 && useInt1 <= 15) || (useInt1 >= 21 && useInt1 <= 23)){
		useInt2 = 2;	
	}
	
	alljoy.skillButton = true;
	alljoy.ptime = Time.time + 2;
	StartCoroutine(huifu());
	var bool : boolean = false;
	bool = ASkill.AttackSkill(parseInt(id) - 1);
	return bool;
}

function huifu(){
		yield ;  
		alljoy.skillButton = false;  
}

private var useUpDateID : String = "";
function UpDateOneSkill(id : String){
//	//print(id);
	useUpDateID = id;
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	var useInt1 : int;
	var useInt2 : int;
	useInt1 = parseInt(id);
	if((useInt1 >= 4 && useInt1 <= 9) || (useInt1 >= 18 && useInt1 <= 20)){
		useInt2 = 1;
	}else
	if((useInt1 >= 10 && useInt1 <= 15) || (useInt1 >= 21 && useInt1 <= 23)){
		useInt2 = 2;	
	}
//	if(useSkillPoint(1)){


		if(lookQianZhiTaskAsID("23") || useInt1 < 4 || useInt1 == 16 || useInt1 == 17){	
			AllManage.UIALLPCStatic.showbranch15();
			yield;
			if(BranchControl.BranchID == useInt2 || BranchControl.BranchID == 3 || useInt1 < 4 || useInt1 == 16 || useInt1 == 17){
//				playerSkillStr = "";
				var i : int = 0;
				for(i=0; i<23; i++){
					if(i == (parseInt(id)) - 1){
						if((AllManage.jiaochengCLStatic.JiaoChengID == 1 && AllManage.jiaochengCLStatic.step == 3 && AllManage.InvclStatic.TutorialsDetectionAsID("13") )){
//							useSKstr[i] = (parseInt(useSKstr[i].Substring(0,1)) + 1) + useSKstr[i].Substring(1,1);
							realUpDateOneSkill();
						}else{
							
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.UpDateOneSkill , personSkills[i].level[parseInt(useSKstr[i].Substring(0,1))] , 0 , "" , gameObject , "realUpDateOneSkill");
//AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.UpDateOneSkill , i , parseInt(useSKstr[i].Substring(0,1)) , "" , gameObject , "realUpDateOneSkill");
//							AllManage.AllMge.UseMoney(personSkills[i].gold[parseInt(useSKstr[i].Substring(0,1))] , personSkills[i].blood[parseInt(useSKstr[i].Substring(0,1))] , UseMoneyType.UpDateOneSkill , gameObject , "realUpDateOneSkill");
//							ps.UseMoney(personSkills[i].gold[parseInt(useSKstr[i].Substring(0,1))] , personSkills[i].blood[parseInt(useSKstr[i].Substring(0,1))] )
						}
					}
				}
			}else{
				AllManage.tsStatic.Show("info748");
			}
		}else{
				AllManage.tsStatic.Show("tips044");		
		}
		
		
//	}else{
//		ts.Show("当前剩余技能点不足");	
//	}
}

var useSkillItem : SkillItem;
function realUpDateOneSkill(){
				playerSkillStr = "";
				var i : int = 0;
				for(i=0; i<23; i++){
					if(i == (parseInt(useUpDateID)) - 1){
						var useLevel : int = 0;
						useLevel = parseInt(useSKstr[i].Substring(0,1)) + 1;
						if(useLevel > 3){
							useLevel = 3;
						}
						useSKstr[i] = useLevel + useSKstr[i].Substring(1,1);
					}
				}
				for(i=0; i<23; i++){
					playerSkillStr += useSKstr[i] + ";";
				} 
				SetSkills(playerSkillStr);
				ShowSkillCanUpDate();
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.StudySkill).ToString());
	if(useSkillItem){
		useSkillItem.OnClick();
	}
}

function useSkillPoint(num : int) : boolean{
	if(SkillPoint >= num){
		SkillPoint -= num;
		InventoryControl.yt.Rows[0]["SkillPoint"].YuanColumnText = SkillPoint.ToString();
		return true;
	}
	return false;
}

function UpSkillZhiXian(id : String,fenzhi : int , cost : int){ 
//	if(useSkillPoint(cost)){
		playerSkillStr = "";
		var i : int = 0;
		for(i=0; i<23; i++){
			if(i == (parseInt(id)) - 1){
				useSKstr[i] = useSKstr[i].Substring(0,1) + fenzhi.ToString();
			}
		}
		for(i=0; i<23; i++){
			playerSkillStr += useSKstr[i] + ";";
		}
//		SetSkills(playerSkillStr);
//		ShowSkillCanUpDate();
//	}else{
//		AllManage.tsStatic.Show("tips045");		
//	}
}

function AttackSimple(){
	alljoy.attackButton = true;
	alljoy.ptime = Time.time + 2;
	yield;
	alljoy.attackButton = false;
}

function jump(){
	alljoy.rollButton = true;
//	//print(alljoy.jumpButton);
}

var STitem : SkillItem[];
function LookSameSkill(id : String){
	for(var i=0; i<STitem.length; i++){
		if(STitem[i].skillID == id){
			return false;
		}
	}
	return true;
}

var SaveSkillButtonStr : String;
function SaveSkillButton(){
	SaveSkillButtonStr = "";
	for(var i=0; i<STitem.length; i++){
		SaveSkillButtonStr += STitem[i].mySkillSave + ";";
	}
	InventoryControl.yt.Rows[0]["SkillsPostion"].YuanColumnText = SaveSkillButtonStr;
}

var FStr : String = ";";
var DStr : String = ",";
var useSKStr : String[];
var uStr : String[];
function SetSaveButtons(str : String){
	useSKStr = str.Split(Fstr.ToCharArray());  
//	print(SaveSkillButtonStr + " == SaveSkillButtonStr" );
	if(useSKStr.length > 2){
		for(var i =0; i<4; i++){
			uStr = useSKStr[i].Split(DStr.ToCharArray()); 
			if(uStr.length > 2){
				STitem[i].SetButton(uStr[0] , uStr[1] , uStr[2]);
			}
		}
	}
}

function GetSkillCDAsID(id : int) : int{
	var cd : int = 0;
//	//print(id + " == id " + ASkill + " == " + ASkill.GetSkillInfoAsID(id));
	cd = ASkill.GetSkillInfoWithI(id).CDtime;
	return cd;
}

function GongCD(){
	for(var i=0; i<STitem.length; i++){
		STitem[i].SkillGongCoolDown(BtnGameManager.numPubSkillCD);
	}
}

private var aSkill : Skillclass;
private var pSkill : Skillpassive;
function SetSkillLabelAsID(objs : Object[]){
//	var sMame : String = "";
//	var sLevel : String = "";
	var labelName : UILabel;
	var labelLevel : UILabel;
	var skillID : String;
	skillID = objs[0];
	labelName = objs[1];
	labelLevel = objs[2];
	if(parseInt(skillID) < 16){
		aSkill = ASkill.GetSkillInfoAsID(ProID*100 + parseInt(skillID));
		if(aSkill.name != ""){
			labelName.text = AllManage.AllMge.Loc.Get(aSkill.name);
		}else{
			labelName.text = "";
		}
		
		labelLevel.text =  "lv." + parseInt(aSkill.level.ToString().Substring(0,1)).ToString();
	}else{
		pSkill = PSkill.GetSkillInfoAsID(parseInt(skillID));	
		if(pSkill.name != ""){
			labelName.text = AllManage.AllMge.Loc.Get(pSkill.name);	
		}else{
			labelName.text = "";	
		}
		labelLevel.text = "lv." + parseInt(pSkill.level.ToString().Substring(0,1)).ToString();
	}
}
