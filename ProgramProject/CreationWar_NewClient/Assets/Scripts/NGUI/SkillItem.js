#pragma strict
class	SkillItem	extends	Song
{
	var myButtonStr : String;
	function	Start ()
	{
		if(	myType	==	MoveType.to	)
		{
			reMoveSkill();
		}
		InvokeRepeating("UpdateSkill", 2, 0.1); 
	}

	var ASkill : ActiveSkill;
	var SpriteNotMana : UISprite;
	var invcl : InventoryControl;
	private var useSKillID : int;
	private var bool1 : boolean;
	private var bool2 : boolean;
	private var useProID : int = 0;
	private var removeSkillID : int = 0;
	function	UpdateSkill()
	{
		if(	ASkill != null && skillID.Length > 1 && invcl != null	)
		{
			if(	ps == null && PlayerStatus.MainCharacter	)
			{
				ps	=	PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}	
			useSKillID	=	parseInt(skillID);
			if(	useSKillID	>=	4	&&	useSKillID	<=	9	)
			{
	//			if(ps.ProID == 3){
	//				useProID = 2;
	//			}else{
	//				useProID = 1;
	//			}
				removeSkillID = 1;
				if(	invcl.CanSkillAsID(1)	)
				{
					bool1 = true;
				}
				else
				{
					bool1 = false;			
				}
			}
			else
			if(	useSKillID >= 10 && useSKillID <= 15	)
			{
	//			if(ps.ProID == 3){
	//				useProID = 1;
	//			}else{
	//				useProID = 2;
	//			}
				removeSkillID = 2;
				if(	invcl.CanSkillAsID(2)	)
				{
					bool1 = true;
				}
				else
				{
					bool1 = false;			
				}		
			}
			else
			if(	useSKillID < 4	)
			{
				bool1 = true;
			}
			bool2 =  ASkill.getSkillMana(parseInt(skillID.Substring(0,2)) - 1);
			if(	Application.loadedLevelName	!=	"Map200"	)
			{
				if(	bool1 && !bool2	)
				{
					SpriteNotMana.enabled = false;
				}
				else
				{
					if(! SpriteNotMana.enabled){					
						SpriteNotMana.enabled = true;
					}
					if(!bool1 && UIControl.mapType != MapType.zhucheng && invSprite.enabled && invcl.CanRemoveSkillAsID(removeSkillID) && useSKillID >= 4 && myType	==	MoveType.to	){
						reMoveSkill();
					}
				}
			}
			else
			{
				if(bool2){
					SpriteNotMana.enabled = true;
				}else{
					SpriteNotMana.enabled = false;							
				}
			}
		}
	}

	//var SkillC : SkillControl;
	var skillmove : SkillMove;
	private var skillstr : String = "ProID_";
	var skillID : String;
	var skillLevel : int;
	var cantDrag : boolean = true;
	
	function	OnDrag (delta : Vector2)
	{
		if(	myType == MoveType.from && cantDrag && !isFuZhu && skillLevel > 0)
		{
			return;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			skillmove.MoveStart(skillstr + AllManage.SkillCLStatic.ProID.ToString() + skillID);
		} 
	}

	function	particlePlay()
	{
		if(	skillpar	)
		{
			skillpar.emit = true;
			yield	WaitForSeconds(0.2);
			skillpar.emit = false;
		}
	}
	//var inv : InventoryItem = null; 
	private var SkillI : SkillItem;
	var myType : MoveType;
	var invSprite : UISprite; 
	var saveID : int;
	var cdTime : int;
	var jiaochengCL : TaskJiaoChengControl;
	var skillpar : ParticleEmitter;
	function	OnDrop(go : GameObject)
	{ 
		SkillI = go.GetComponent(SkillItem);
		if(	SkillI	)
		{	
			if(	myType == MoveType.to && SkillI.cantDrag && AllManage.SkillCLStatic.LookSameSkill(SkillI.skillID)	)
			{
				cdTime = SkillI.SkillObj.CDtime;
				skillID = SkillI.skillID;
				invSprite.enabled = true;
				
				invSprite.color.r = 1;
				invSprite.color.g = 1;
				invSprite.color.b = 1;
				invSprite.spriteName = skillstr + AllManage.SkillCLStatic.ProID.ToString() + skillID;
	//			//print("zhe li le");
				SaveSkill(cdTime , skillID , invSprite.spriteName);
				if(	jiaochengCL.JiaoChengID == 1 && jiaochengCL.step == 3	)
				{
					jiaochengCL.NextStep();
				}
			}
		}
	}

	function	reMoveSkill()
	{
		SkillI = null;
		cdTime = 0;
		skillID = "";
		invSprite.enabled = false;
		invSprite.spriteName = "";
		SaveSkill(	cdTime,	skillID,	invSprite.spriteName	);
		yield;
		if(	AllManage.SkillObjDet	)
			AllManage.SkillObjDet.SendMessage("reOpenInfo" , SendMessageOptions.DontRequireReceiver);
	}

	function	setRealSKill( sk : SkillItem	)
	{
		SkillI = sk;
		if(SkillI){	
			if(myType == MoveType.to && SkillI.cantDrag && AllManage.SkillCLStatic.LookSameSkill(SkillI.skillID)){
				cdTime = SkillI.SkillObj.CDtime;
				skillID = SkillI.skillID;
				invSprite.enabled = true;
			invSprite.color.r = 1;
			invSprite.color.g = 1;
			invSprite.color.b = 1;
				invSprite.spriteName = skillstr + AllManage.SkillCLStatic.ProID.ToString() + skillID;
	//			//print("zhe li le");
				SaveSkill(cdTime , skillID , invSprite.spriteName);
				if(jiaochengCL.JiaoChengID == 1 && jiaochengCL.step == 3){
					jiaochengCL.NextStep();
				}
			}
		}
			if(isFuZhu){
				if(myButtonStr == "Cook"){ 
					AllManage.SkillObjDet.showOneSkill(skillLevel.ToString() , SkillObj , skillID , infoStr , thisSkillCanUpDate , gameObject , 1 , invSprite.spriteName.ToString() , LabelName.text.ToString() + LabelLevel.text.ToString());
					return;
				}else
				if(myButtonStr == "Product"){
					AllManage.SkillObjDet.showOneSkill(skillLevel.ToString() , SkillObj , skillID , infoStr , thisSkillCanUpDate , gameObject , 2 , invSprite.spriteName.ToString() , LabelName.text.ToString() + LabelLevel.text.ToString());
					return; 
				}
				AllManage.SkillObjDet.showOneSkill(skillLevel.ToString() , SkillObj , skillID , infoStr , thisSkillCanUpDate , gameObject , 0 , invSprite.spriteName.ToString() , LabelName.text.ToString() + LabelLevel.text.ToString()); 
				return;
				infoStr = AllManage.SkillCLStatic.showFuZhuInfo(parseInt(skillID));				
			}else{
				infoStr = AllManage.SkillCLStatic.showOnClickSkillInfo(skillID);		
			}
		yield;
		AllManage.SkillObjDet.SendMessage("reOpenInfo" , SendMessageOptions.DontRequireReceiver);
	//		AllManage.SkillObjDet.showOneSkill(skillLevel.ToString() , SkillObj , skillID , infoStr , thisSkillCanUpDate , gameObject);
	}

	var SkillNum : int;
	private var ps : PlayerStatus;
	var mySkillSave : String;
	function SaveSkill(cd : int , id : String , name : String){
	//	if(ps == null && PlayerStatus.MainCharacter){
	//		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	//	}
	//	
	//	if(ps != null){	
	//		PlayerPrefs.SetInt(InventoryControl.PlayerID + "SkillButtonCD" + SkillNum + ps.weaponType, cd);
	//		PlayerPrefs.SetString(InventoryControl.PlayerID + "SkillButtonID" + SkillNum + ps.weaponType , id);
	//		PlayerPrefs.SetString(InventoryControl.PlayerID + "SkillButtonNAME" + SkillNum + ps.weaponType , name); 
	//	}
	//	//print(cd + " = " + id + " = " + name);
		mySkillSave = cd + "," + id + "," + name;
		if(AllManage.SkillCLStatic)
		AllManage.SkillCLStatic.SaveSkillButton();
	}

	private var infoStr : String;
	var isFuZhu : boolean = false;
	
	var mapTalk : MapTalkControl;
	//使用技能//
	function	OnClick()
	{
		var	bool	:	boolean	=	false;
		if(	myType == MoveType.to	)
		{
			if(	UIControl.mapType != MapType.zhucheng	)
			{
				if(	invSprite.enabled && !SpriteNotMana.enabled	)
				{
					Faguan.enabled	=	true;
					CDFaguanOpen	=	false;
					bool	=	AllManage.SkillCLStatic.UseSkills(skillID);
					cdTime	=	AllManage.SkillCLStatic.GetSkillCDAsID(parseInt(skillID) - 1);
					if(	bool	)
					{			
						SkillCoolDown(cdTime);
						AllManage.SkillCLStatic.GongCD();
						//SkillGongCoolDown(1);
					}
					yield;
					yield;
					Faguan.enabled = false;
				}
			}
		}
		else
		if(	myType == MoveType.from	)
		{
			AllManage.SkillCLStatic.useSkillItem = this;
			if(	isFuZhu	)
			{
				if(	myButtonStr == "Cook"	)
				{ 
					AllManage.SkillObjDet.showOneSkill(skillLevel.ToString() , SkillObj , skillID , infoStr , thisSkillCanUpDate , gameObject , 1 , invSprite.spriteName.ToString() , LabelName.text.ToString() + LabelLevel.text.ToString());
					return;
				}
				else
				if(	myButtonStr == "Product"	)
				{
					AllManage.SkillObjDet.showOneSkill(skillLevel.ToString() , SkillObj , skillID , infoStr , thisSkillCanUpDate , gameObject , 2 , invSprite.spriteName.ToString() , LabelName.text.ToString() + LabelLevel.text.ToString());
					return; 
				}
				AllManage.SkillObjDet.showOneSkill(skillLevel.ToString() , SkillObj , skillID , infoStr , thisSkillCanUpDate , gameObject , 0 , invSprite.spriteName.ToString() , LabelName.text.ToString() + LabelLevel.text.ToString()); 
				return;
				infoStr = AllManage.SkillCLStatic.showFuZhuInfo(parseInt(skillID));				
			}
			else
			{
				infoStr = AllManage.SkillCLStatic.showOnClickSkillInfo(skillID);		
			}
			AllManage.SkillObjDet.showOneSkill(skillLevel.ToString() , SkillObj , skillID , infoStr , thisSkillCanUpDate , gameObject);
		}
		
			if(Application.loadedLevelName == "Map200"){
				mapTalk	=	FindObjectOfType(	MapTalkControl	);
			}
			if(mapTalk){
				mapTalk.TalkObj1.SetActive(false);
			}
		
	}

	var Faguan : UISprite;
	var iconCD : UIFilledSprite;
	private var CDFaguanOpen : boolean = false;
	function	SkillCoolDown(o : int)
	{
	//	//print("o == " + o);
		var cd : int = Time.time + o;
		iconCD.fillAmount = 1;
		while(iconCD.fillAmount > 0){
			iconCD.fillAmount -= 1.0 / o * Time.deltaTime;
			yield;
		}
		Faguan.enabled = true;
		CDFaguanOpen = true;
		yield;
		yield;
		if(CDFaguanOpen){
			CDFaguanOpen = false;
			Faguan.enabled = false;
		}
	}

	function OnEnable(){
		if(iconCD)
			iconCD.fillAmount = 0;
	}

	var iconGongCD : UIFilledSprite;
	function SkillGongCoolDown(o : int){
	//	//print("o == " + o);
		var cd : int = Time.time + o;
		iconGongCD.fillAmount = 1;
		while(iconGongCD.fillAmount > 0){
			iconGongCD.fillAmount -= 1.0 / o * Time.deltaTime;
			yield;
		}
	}

	function	StartSetSkill()
	{
	//	if(ps == null && PlayerStatus.MainCharacter){
	//		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	//	}
	//	
	//	if(ps != null){	
	//	
	//		skillID = PlayerPrefs.GetString(InventoryControl.PlayerID + "SkillButtonID" + SkillNum + ps.weaponType);
	//		if(skillID != ""){
	//			cdTime = PlayerPrefs.GetInt(InventoryControl.PlayerID + "SkillButtonCD" + SkillNum + ps.weaponType);
	//			invSprite.enabled = true;
	//			invSprite.spriteName = PlayerPrefs.GetString(InventoryControl.PlayerID + "SkillButtonNAME" + SkillNum + ps.weaponType); 
	//		}else{
	//			cdTime = 0;
	//			invSprite.enabled = false;
	//		}
	//	}
	}

	function	SetButton(useCD : String , useID : String , useName : String)
	{
		if(	ps == null && PlayerStatus.MainCharacter	)
		{
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		mySkillSave = useCD + "," + useID + "," + useName;
	//	print(useID + " == useID");
		if(	useID != ""	)
		{
			invSprite.color.r = 1;
			invSprite.color.g = 1;
			invSprite.color.b = 1;
			skillID = useID;
			cdTime = parseInt(useCD); 
			invSprite.enabled = true;
			particlePlay();
			invSprite.spriteName = useName;
		}
		else
		{
			cdTime = 0;
			invSprite.enabled = false;	
		}
	}

	var LabelName : UILabel;
	var Labelfen1 : UILabel;
	var Labelfen2 : UILabel;
	var LabelLevel : UILabel;
	var SkillObj : Skillclass;
	var ZhiXianButton : UISprite[];
	var myTypeStr : String; 
	var zhixianCost : int;
	var LabelCost : UILabel;
	function	SetSkill(level : String , sc : Skillclass , cost : int)
	{
	//	//print(SkillObj.name + " == " + LabelName + " == " + gameObject.name);
		SkillObj = sc; 
		zhixianCost = cost;
		skillLevel = parseInt(level); 
		if(	myTypeStr == "fenzhi"	)
		{
			LabelCost.text = AllManage.AllMge.Loc.Get( "info940" ) + zhixianCost;
			if(skillLevel >= 10)
			{
				invSprite.color.r = 1;
				invSprite.color.g = 1;
				invSprite.color.b = 1;
				invSprite.spriteName = skillstr + AllManage.SkillCLStatic.ProID.ToString() + skillID;
			}
			else
			{
				invSprite.color.r = 0.3;
				invSprite.color.g = 0.3;
				invSprite.color.b = 0.3;
				invSprite.spriteName = skillstr + AllManage.SkillCLStatic.ProID.ToString() + skillID+"";	 
			}
			invSprite.enabled = true; 
			Labelfen1.text = "[ffffff]" +AllManage.AllMge.Loc.Get( SkillObj.Branch1name );
			Labelfen2.text = "[ffffff]" +AllManage.AllMge.Loc.Get( SkillObj.Branch2name );
			LabelName.text = AllManage.AllMge.Loc.Get( SkillObj.name );
		}
		else
		{ 
			if(	skillLevel >= 10	)
			{	
				LabelName.text = AllManage.AllMge.Loc.Get( SkillObj.name );
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages060");
				AllManage.AllMge.Keys.Add(level.Substring(0,1) + "");
				AllManage.AllMge.SetLabelLanguageAsID(LabelLevel);
	//			LabelLevel.text = "等级" + level.Substring(0,1);  
				invSprite.color.r = 1;
				invSprite.color.g = 1;
				invSprite.color.b = 1;
				invSprite.spriteName = skillstr + AllManage.SkillCLStatic.ProID.ToString() + skillID;
				invSprite.enabled = true;
			}
			else
			{
	//			//print(LabelName);
	//			//print(SkillObj);
				if(	LabelName	)
				{
					LabelName.text = AllManage.AllMge.Loc.Get( SkillObj.name );
					AllManage.AllMge.Keys.Clear();
					AllManage.AllMge.Keys.Add("messages060");
					AllManage.AllMge.Keys.Add("0");
					AllManage.AllMge.SetLabelLanguageAsID(LabelLevel);
		//			LabelLevel.text = "等级0"; 	
					invSprite.color.r = 0.3;
					invSprite.color.g = 0.3;
					invSprite.color.b = 0.3;
					invSprite.spriteName = skillstr + AllManage.SkillCLStatic.ProID.ToString() + skillID+"";	 
					invSprite.enabled = true;
					
				}
			}
		}
		if(ZhiXianButton.length > 0 && level != "00"){ 
	//		//print("sdfsdf == "+SkillObj.level.ToString());
			switch(level.ToString().Substring(1,1)){
				case "0": 
					ZhiXianButton[0].spriteName = "UITalent_Deviation_N";
					ZhiXianButton[1].spriteName = "UITalent_Deviation_N";
					Labelfen1.text ="[ffffff]" + AllManage.AllMge.Loc.Get( SkillObj.Branch1name );
			        Labelfen2.text ="[ffffff]" + AllManage.AllMge.Loc.Get( SkillObj.Branch2name );
					break;
				case "1":
					ZhiXianButton[0].spriteName = "UITalent_Deviation_A";
					ZhiXianButton[1].spriteName = "UITalent_Deviation_N";
					Labelfen1.text ="[ffa200]" + AllManage.AllMge.Loc.Get( SkillObj.Branch1name );
			        Labelfen2.text ="[ffffff]" + AllManage.AllMge.Loc.Get( SkillObj.Branch2name );
					break;
				case "2":
					ZhiXianButton[0].spriteName = "UITalent_Deviation_N";
					ZhiXianButton[1].spriteName = "UITalent_Deviation_A";
					Labelfen1.text ="[ffffff]" + AllManage.AllMge.Loc.Get( SkillObj.Branch1name );
			        Labelfen2.text ="[ffa200]" + AllManage.AllMge.Loc.Get( SkillObj.Branch2name );
					break;
			}
		}else{ 
			if(ZhiXianButton.length > 0){
				ZhiXianButton[0].spriteName = "UITalent_Deviation_N";
				ZhiXianButton[1].spriteName = "UITalent_Deviation_N";
			}
		}
	}

	var PassObj : Skillpassive;
	function SetPassSkill(level : String , sp : Skillpassive){
		PassObj = sp;
		skillLevel = parseInt(level.Substring(0,1));
	//	//print(skillLevel);
		if(skillLevel >= 1){	
			LabelName.text = AllManage.AllMge.Loc.Get( PassObj.name );
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages060");
				AllManage.AllMge.Keys.Add(level.Substring(0,1) + "");
				AllManage.AllMge.SetLabelLanguageAsID(LabelLevel);
	//		LabelLevel.text = "等级" + level.Substring(0,1);  
	//		//print(skillstr + AllManage.SkillCLStatic.ProID.ToString() + (parseInt(skillID) - 15) + " ====i");
			invSprite.color.r = 1;
			invSprite.color.g = 1;
			invSprite.color.b = 1;
			invSprite.spriteName = skillstr + AllManage.SkillCLStatic.ProID.ToString() + (parseInt(skillID) - 15);
			invSprite.enabled = true;
		
		}else{
	//		//print(" == 0" + level);
			LabelName.text = AllManage.AllMge.Loc.Get( PassObj.name );
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages060");
				AllManage.AllMge.Keys.Add("0");
				AllManage.AllMge.SetLabelLanguageAsID(LabelLevel);
	//		LabelLevel.text = "等级0"; 	
			invSprite.color.r = 0.3;
			invSprite.color.g = 0.3;
			invSprite.color.b = 0.3;
			invSprite.spriteName = skillstr + AllManage.SkillCLStatic.ProID.ToString() + (parseInt(skillID) - 15)+"";	 
			invSprite.enabled = true;
		
		}
	}

	var UpDateButton : UISprite; 
	var UpDateButtonParent : GameObject;
	var thisSkillCanUpDate : boolean = false;
	function ThisCanUpDate(){
		thisSkillCanUpDate = true;
		UpDateButton.enabled = true;
	}
	function CantUpDate(){
		thisSkillCanUpDate = false;
	//	UpDateButtonParent.active = false;
		UpDateButton.enabled = false;
	}
	function updateSkill(){
		yield AllManage.SkillCLStatic.UpDateOneSkill(skillID);
		OnClick();
	}

	var ZhiXianButtonParent : GameObject[];
	function ThisCanZhiXian(){
		ZhiXianButtonParent[0].active = true;	
		ZhiXianButtonParent[1].active = true;	
	}
	function CantZhiXian(){
		ZhiXianButtonParent[0].active = false;	
		ZhiXianButtonParent[1].active = false;	
	} 

	function SkillZhiXian1(){ 
		var bool : boolean = false;
		bool = AllManage.SkillCLStatic.showFenZhiInfo(skillID , 1);
		if(!bool)
			return;
		if(ZhiXianButton[0].spriteName == "UITalent_Deviation_A"){
//			AllManage.SkillCLStatic.UpSkillZhiXian(skillID,1 , zhixianCost);
//			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.StudySkillBiased).ToString());
		}else{
			if(AllManage.jiaochengCLStatic.JiaoChengID == 9 && AllManage.jiaochengCLStatic.step == 3 ){
				AllManage.jiaochengCLStatic.NextStep();
			}
			SelectOneAsID(0);
		}
	}
	function SkillZhiXian2(){
		var bool : boolean = false;
		bool = AllManage.SkillCLStatic.showFenZhiInfo(skillID , 2);
		if(!bool)
			return;
		if(ZhiXianButton[1].spriteName == "UITalent_Deviation_A"){
//			AllManage.SkillCLStatic.UpSkillZhiXian(skillID,2 , zhixianCost);
//			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.StudySkillBiased).ToString());
		}else{
			if(AllManage.jiaochengCLStatic.JiaoChengID == 9 && AllManage.jiaochengCLStatic.step == 3 ){
				AllManage.jiaochengCLStatic.NextStep();
			}
			SelectOneAsID(1);
		}
	}
	
	function SelectOneAsID(id : int){
		ZhiXianButton[0].spriteName = "UITalent_Deviation_N";
		ZhiXianButton[1].spriteName = "UITalent_Deviation_N";

		ZhiXianButton[id].spriteName = "UITalent_Deviation_A";
	}

	function NonSelect(){
		ZhiXianButton[0].spriteName = "UITalent_Deviation_N";
		ZhiXianButton[1].spriteName = "UITalent_Deviation_N";	
	}

	function UpdateBranchSkill(){
		try{
			if(skillLevel.ToString().Substring(1,1) != "0"){
				return;
			}
		}catch(e){
		
		}
		if(ZhiXianButton[0].spriteName == "UITalent_Deviation_A"){
//			print(skillID + " =1= skillID");
			AllManage.SkillCLStatic.UpSkillZhiXian(skillID,1 , zhixianCost);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.StudySkillBiased).ToString());
		}else
		if(ZhiXianButton[1].spriteName == "UITalent_Deviation_A"){
//			print(skillID + " =2= skillID");
			AllManage.SkillCLStatic.UpSkillZhiXian(skillID,2 , zhixianCost);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.StudySkillBiased).ToString());
		}
	}

	function ComputeUpdateBranch() : boolean{
		try{
			if(skillLevel.ToString().Substring(1,1) != "0"){
				return false;
			}
		}catch(e){
			return false;
		}
		if(ZhiXianButton[0].spriteName == "UITalent_Deviation_A"){
			return true;
		}else
		if(ZhiXianButton[1].spriteName == "UITalent_Deviation_A"){
			return true;
		}
		return false;
	}

	function notBranch(){
		var Str : String;
		if(parseInt(skillID) > 3 && parseInt(skillID) <= 15){
			invSprite.color.r = 0.3;
			invSprite.color.g = 0.3;
			invSprite.color.b = 0.3;
			invSprite.spriteName = skillstr + AllManage.SkillCLStatic.ProID.ToString() + skillID+"";	 
		}else
		if(parseInt(skillID) > 17){
			invSprite.color.r = 0.3;
			invSprite.color.g = 0.3;
			invSprite.color.b = 0.3;
			invSprite.spriteName = skillstr + AllManage.SkillCLStatic.ProID.ToString() + (parseInt(skillID) - 15)+"";
	 	}
	}
}