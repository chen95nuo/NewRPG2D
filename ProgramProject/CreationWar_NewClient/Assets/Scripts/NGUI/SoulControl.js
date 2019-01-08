#pragma strict

private var Fstr : String = ";";
//var invMaker : Inventorymaker;
private var ps : PlayerStatus;
var sp : SoulPet;
var smeltNum : int = 0;

//var ts : TiShi;
//var qr : QueRen; 

var BagSoul : String;
var BagDigest : String;
var EquipItemSoul : String;
var BuildSoulLevel : int = 0;

var BagSIT : BagItem[];
var BagDIT : BagItem[];
var EquepSIT : BagItem[];

var Label1 : UILabel;
var Label2 : UILabel;
var Label3 : UILabel;
var Label4 : UILabel;
var Label5 : UILabel;
var LabelSoulName : UILabel;
var buttons : GameObject[];
var shanDigest : UISprite;
var shanSoul : UISprite;
var strsss : String = String.Empty;
var pointEffectLevelUp : Transform;
var pointEffectQualityUp : Transform;

function Awake(){
//	ts = AllManage.tsStatic;
//	qr = AllManage.qrStatic;
	invcl = AllManage.InvclStatic;
//	invMaker = AllResources.InvmakerStatic;
	SpriteQuan2 = AllManage.quanSoulStatic;
	AllManage.jiaochengCLStatic.allJiaoCheng[8].obj[1] = buttons[0];
	AllManage.jiaochengCLStatic.allJiaoCheng[8].obj[2] = buttons[1];
	AllManage.jiaochengCLStatic.allJiaoCheng[8].obj[3] = buttons[2];
	AllManage.jiaochengCLStatic.allJiaoCheng[8].obj[4] = buttons[3];
	AllManage.jiaochengCLStatic.allJiaoCheng[8].obj[5] = buttons[4];
	
	AllManage.PlyUIClStatic.LabelGoldS = Label1;
	AllManage.PlyUIClStatic.LabelSoulS = Label2;
	AllManage.PlyUIClStatic.LabelSoulPowerS = Label3;
	AllManage.PlyUIClStatic.LabelBloodS = Label4;
	AllManage.InvclStatic.soucl = this;
	AllManage.SoulCLStatic = this;
}

function Start () {
	var mm : boolean = false;
//	//print(InventoryControl.yt.Rows.Count + " == InventoryControl.yt.Rows.Count");
	if(InventoryControl.yt.Rows.Count > 0){			
		BagSoul = InventoryControl.yt.Rows[0]["BagSoul"].YuanColumnText;
		if(BagSoul == ""){
			BagSoul = ";;;;;;";
		}
		BagDigest = InventoryControl.yt.Rows[0]["BagDigest"].YuanColumnText;
		NumDigestButtons = GetBDInfoInt("BagDigestNum" , 5);
		if(BagDigest == ""){
			BagDigest = ";;;;;;;;;;;;;;;;";
		}
		EquipItemSoul = InventoryControl.yt.Rows[0]["EquipItemSoul"].YuanColumnText;
		if(EquipItemSoul == ""){
			EquipItemSoul = ";;;;;;;";
		}  
		var realSoulLevel : int;
		BuildSoulLevel =  GetBDInfoInt("BuildSoulLevel" , 0);
		mm = true;
	}
	
	PanelStatic.StaticBtnGameManager.RunOpenLoading(function() ServerRequest.requestSmeltGetNum());
	
	if(ps == null && PlayerStatus.MainCharacter){
		sp = PlayerStatus.MainCharacter.gameObject.GetComponent(SoulPet);
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	} 
	yield;
	yield;
	SetBagSoulItem(BagSoul);
	SetBagDigestItem(BagDigest);
//	//print(EquipItemSoul +  " == EquipItemSoul");
	SetEquepSoulItem(EquipItemSoul);
	//UpdateEquepSoulItem();
	SetButtonsDigest(NumDigestButtons);	
	SetButtonsSoul();
}

function ButtonLookSkill(){
	if(EquepSIT[6].inv != null){
		LookGroundSkill(EquepSIT[6].inv);
		ButtonLookJingHua();
	}else{
		AllManage.tsStatic.Show("tips046");		
	}
}

var ValueLookJingHuaArray : int[];
var TextLookJingHuaArray : String[];
var LableLookJingHua : UILabel;
function ButtonLookJingHua(){
	var i : int = 0;
	var boolj : boolean = false;
//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add("messages103");
//			AllManage.AllMge.Keys.Add("\n");
//			AllManage.AllMge.SetLabelLanguageAsID(LableLookJingHua);
//	LableLookJingHua.text = "精华加成: \n";
	for(i=0; i<ValueLookJingHuaArray.length; i++){
		ValueLookJingHuaArray[i] = 0;
	}
	for(i=0; i<TextLookJingHuaArray.length; i++){
		TextLookJingHuaArray[i] = "";
	}
	var useStr : String = "";
	for(i=0; i<EquepSIT.length; i++){
		if(EquepSIT[i].inv != null){
			if(i < 6){
				boolj = true;
				GetValueAndText(EquepSIT[i].inv , i);
				useStr += TextLookJingHuaArray[i] + ValueLookJingHuaArray[i].ToString() + "\n";
//				LableLookJingHua.text += TextLookJingHuaArray[i] + ValueLookJingHuaArray[i].ToString() + "\n";
//				LableLookJingHua.text = AllManage.AllMge.Loc.Get("info1164");
			}
		}
	}
	LabelSkill.text += "\n";
	if(!boolj){
			LabelJinghua.text = AllManage.AllMge.Loc.Get("messages125") + "\n";
//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add(LableLookJingHua.text + "");
//			AllManage.AllMge.Keys.Add("messages125");
//			AllManage.AllMge.SetLabelLanguageAsID(LableLookJingHua);
//		LableLookJingHua.text += "没有装备灵魂精华！";
	}else{
		LabelJinghua.text = AllManage.AllMge.Loc.Get("messages103") + "\n" + useStr;
	}
//	LookInitValue(2);
}

function ButtonLookInfo(){
	LookInitValue(2);
	LableLookJingHua.text = AllManage.AllMge.Loc.Get("info1164");
}

//var StrItemAttr : String; 
var SoulUpdateButtons : Transform;
private var EquInfo : EquepmentItemInfo; 
function LookDitemInfo(inv : InventoryItem){ 
	LabelSkill.text = "";
	LabelJinghua.text = "";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages123");
			AllManage.AllMge.Keys.Add(inv.itemName + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "名称: " + inv.itemName + "\n";
//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
//			AllManage.AllMge.Keys.Add("messages062");
//			AllManage.AllMge.Keys.Add(" " + inv.itemLevel + "\n");
//			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "等级: " + inv.itemLevel + "\n";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages075");
			AllManage.AllMge.Keys.Add(" " + inv.itemQuality + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "品质: " + inv.itemQuality + "\n";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages124");
			AllManage.AllMge.Keys.Add(inv.ItemInfo + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "说明: " + inv.ItemInfo + "\n"; 
	iconSkill.spriteName = inv.atlasStr; 
	iconGround.spriteName = "yuanY" + inv.itemQuality;
	SoulUpdateButtons.localPosition.y = 3000;
	LookInitValue(3);
//	EquInfo.showEquItemInfo(inv , null); 
//	LookInitValue(2);
}
 
function LookGroundRight(){
	LookInitValue(1);
}

var LabelSkill : UILabel;
var LabelJinghua : UILabel ; 
var LabelIns : UILabel;
var iconSkill : UISprite;
var iconGround : UISprite;
var LabelSkillName : UILabel;
var ButtonUpdateSkill : Transform;
function LookGroundSkill(inv : InventoryItem){
	LabelSkill.text = "";
	LabelJinghua.text = "";
	LabelIns.text = "";
//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
//			AllManage.AllMge.Keys.Add("messages123");
//			AllManage.AllMge.Keys.Add(inv.itemName + "\n");
			LabelSkillName.text = inv.itemName;
//			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "名称: " + inv.itemName + "\n";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages062");
			AllManage.AllMge.Keys.Add(" " + inv.itemLevel + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "等级: " + inv.itemLevel + "\n";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages075");
			AllManage.AllMge.Keys.Add(" " + inv.itemQuality + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "品质: " + inv.itemQuality + "\n";
	var Maxmana = inv.itemLevel*12+inv.itemLevel*6*inv.itemQuality;
	var damage = inv.itemLevel*6*inv.itemQuality;
	var delayAttackTime = 15 - 2*inv.itemQuality;

//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
//			AllManage.AllMge.Keys.Add("messages126");
//			AllManage.AllMge.Keys.Add(Maxmana + "\n");
//			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "魔法: " + Maxmana + "\n";  
	if(inv.itemQuality > 1){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages127");
			AllManage.AllMge.Keys.Add(damage + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//		LabelSkill.text += "伤害: " + damage + "\n";  
//			AllManage.AllMge.Keys.Clear();
//			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
//			AllManage.AllMge.Keys.Add("messages128");
//			AllManage.AllMge.Keys.Add(delayAttackTime + "\n");
//			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//		LabelSkill.text += "攻击间隔: " + delayAttackTime + "\n";  
	}
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelSkill.text + "");
			AllManage.AllMge.Keys.Add("messages129");
			AllManage.AllMge.Keys.Add(inv.SoulExp + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelSkill);
//	LabelSkill.text += "经验值: " + inv.SoulExp + "\n";   

 			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelIns.text + "");
			AllManage.AllMge.Keys.Add("info1199");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelIns);
//	LabelSkill.text += "说明: " + inv.ItemInfo + "\n";  
	
	if(inv.itemQuality > 2){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelIns.text + "");
			AllManage.AllMge.Keys.Add(DungeonControl.AllSkiss[inv.itemProAbt+46].sName + " ");
			AllManage.AllMge.Keys.Add("messages062");
			AllManage.AllMge.Keys.Add(" " + inv.SkillLevel + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelIns);
//		LabelSkill.text += DungeonControl.AllSkiss[inv.itemProAbt+46].sName + "  等级: "+ inv.SkillLevel +"\n";   
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelIns.text + "");
			AllManage.AllMge.Keys.Add("messages123");
			AllManage.AllMge.Keys.Add(DungeonControl.AllSkiss[inv.itemProAbt+46].info + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelIns);
//		LabelSkill.text += "说明: " + DungeonControl.AllSkiss[inv.itemProAbt+46].info + "\n";    
	}
	if(inv.itemQuality > 3){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelIns.text + "");
			AllManage.AllMge.Keys.Add("messages130");
			AllManage.AllMge.Keys.Add(DungeonControl.AllSkiss[inv.itemProAbt+51].sName + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelIns);
//		LabelSkill.text += "被动技能: " + DungeonControl.AllSkiss[inv.itemProAbt+51].sName + "\n";   
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelIns.text + "");
			AllManage.AllMge.Keys.Add("messages124");
			AllManage.AllMge.Keys.Add(DungeonControl.AllSkiss[inv.itemProAbt+51].info + "\n");
			AllManage.AllMge.SetLabelLanguageAsID(LabelIns);
//		LabelSkill.text += "说明: " + DungeonControl.AllSkiss[inv.itemProAbt+51].info + "\n";   	
	}
	
//	LabelSkill.text += "说明: " + inv.ItemInfo + "\n";  
	
	SoulUpdateButtons.localPosition.y = 0;
	iconSkill.spriteName = inv.atlasStr;
	LookInitValue(3); 
	if(inv.itemQuality <= 2){
		ButtonUpdateSkill.transform.localPosition.x = 5000;
	}else{
		ButtonUpdateSkill.transform.localPosition.x = 350;		
	}
}

function UpDateOneSoulItemSkill(){
	if(EquepSIT[6].inv != null){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		var useFloat1 : float = 0;
		var useFloat2 : float = 0;
		useFloat1 = EquepSIT[6].inv.SkillLevel;
		useFloat2 = EquepSIT[6].inv.itemQuality;
		
		if( ps.isSoul( Mathf.Sqrt(useFloat1) * 32) && ps.isMoney(  (useFloat1 / 2) * (useFloat1 / 2) * 20) ){
//			ps.UseMoney( EquepSIT[6].inv.SkillLevel * EquepSIT[6].inv.itemQuality * 200 , 0 );
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.UpDateOneSoulItemSkill , EquepSIT[6].inv.SkillLevel , EquepSIT[6].inv.itemQuality , "" , gameObject , "realUpDateOneSoulItemSkill");
//			AllManage.AllMge.UseMoney(EquepSIT[6].inv.SkillLevel * EquepSIT[6].inv.itemQuality * 200 , 0 , UseMoneyType.UpDateOneSoulItemSkill , gameObject , "realUpDateOneSoulItemSkill");
		}
		ButtonLookSkill();
	}else{
		AllManage.tsStatic.Show("tips046");		
	}
}

function realUpDateOneSoulItemSkill(){
		var useInv : InventoryItem;
		useInv = new InventoryItem();
		var useStr : String = "";
		var useFloat1 : float = 0;
		useFloat1 = EquepSIT[6].inv.SkillLevel;
		
			ps.UseSoul(  Mathf.Sqrt(useFloat1) * 32 );
			useStr = EquepSIT[6].inv.itemID;
			var level : String;
			var intLevel : int;
			intLevel = EquepSIT[6].inv.SkillLevel + 1;
			level = intLevel.ToString(); 
			if(intLevel < 10){
				level = "0" + level;
			}else
			if(level.Length > 2){
				level = "99";
			}
			useStr = useStr.Substring(0,8) + level;			
			EquepSIT[6].invClear();
			useInv = new InventoryItem();
			useInv = AllResources.InvmakerStatic.GetItemInfo( useStr , useInv ); 
			EquepSIT[6].SetInv(useInv);
			UpdateEquepSoulItem();
		ButtonLookSkill();

}

function UpDateOneSoulItemAttr(){ 
	return;
	if(EquepSIT[6].inv != null){
		var useStr : String = "";
		var useInv : InventoryItem;
		var pin : int = 0;
		useInv = new InventoryItem();
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		useStr = EquepSIT[6].inv.itemID;
		pin = Mathf.Clamp( parseInt(parseInt(useStr.Substring(4,1)) +1 ) , 1 , 5 );
		useStr = useStr.Substring(0,4) + pin + useStr.Substring(5,5) ;					
		EquepSIT[6].invClear();
		useInv = new InventoryItem();
		useInv = AllResources.InvmakerStatic.GetItemInfo( useStr , useInv ); 
		EquepSIT[6].SetInv(useInv);
		UpdateEquepSoulItem();
	}else{
		AllManage.tsStatic.Show("tips046");		
	}
}

var GroundRight : Transform;
var GroundInfo : Transform; 
var GroundSkill : Transform;
var InitValue : int = 1;
function LookInitValue(i : int){
	InitValue = i; 
	GroundRight.transform.localPosition.y = 3000;
	if(GroundInfo){
	GroundInfo.transform.localPosition.y = 3000;
	}
	GroundSkill.transform.localPosition.y = 3000;
	switch(InitValue){
		case 1:
			GroundRight.transform.localPosition.y = -22;
			break;
		case 2:
		if(GroundInfo){
			GroundInfo.transform.localPosition.y = 0;
			}
			break;
		case 3:
			GroundSkill.transform.localPosition.y = 0;
			break;
	}
}

private var useUpdateSouStr : String;
private var useUpdateSou : InventoryItem;
function SoulEat(){
	if(BagSIT[5].inv != null && EquepSIT[6].inv != null){
		if(EquepSIT[6].inv.itemProAbt == BagSIT[5].inv.itemProAbt && EquepSIT[6].inv.itemQuality >= BagSIT[5].inv.itemQuality){
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.EatSoul).ToString());
			useUpdateSouStr = EquepSIT[6].inv.itemID;
//			var level : String;
			var intLevel : int;
			var intQual : int;
			var intExp : int;
			var float1 : float;
			var float2 : float; 
			var intRan : int = 0;
			float1 = BagSIT[5].inv.itemQuality;
			float2 = BagSIT[5].inv.itemLevel;
//			intLevel = EquepSIT[6].inv.itemLevel + float1 *5/100* float2;
//			level = intLevel.ToString(); 
//			if(level.Length < 2){
//				level = "0" + level;
//			}else
//			if(level.Length > 2){
//				level = "99";
//			}
			var intJ : int = 0 ;
			intJ = EquepSIT[6].inv.itemQuality - BagSIT[5].inv.itemQuality;
			intQual = EquepSIT[6].inv.itemQuality;
			intLevel = EquepSIT[6].inv.itemLevel;
			intExp = EquepSIT[6].inv.SoulExp;
			var useInts : int[];
			if(intJ == 0){
				if(BagSIT[5].inv.itemLevel + EquepSIT[6].inv.itemLevel > 99){
//					print(BagSIT[6].inv.itemLevel);
//					print(100 - BagSIT[6].inv.itemLevel);
					useInts = AllManage.AllMge.SoulAddLevel(intQual , intLevel , 100 - EquepSIT[6].inv.itemLevel);
					intQual = useInts[0];
					intLevel = useInts[1];
				
					useInts = AllManage.AllMge.SoulAddExp(intQual , intLevel , intExp , (BagSIT[5].inv.itemLevel + EquepSIT[6].inv.itemLevel) % 100);
					intQual = useInts[0];
					intLevel = useInts[1];
					intExp = useInts[2];
				}else{
					useInts = AllManage.AllMge.SoulAddLevel(intQual , intLevel , BagSIT[5].inv.itemLevel);
					intQual = useInts[0];
					intLevel = useInts[1];
				}
			}else 
			if(intJ == 1){
				useInts = AllManage.AllMge.SoulAddExp(intQual , intLevel , intExp , BagSIT[5].inv.itemLevel);
				intQual = useInts[0];
				intLevel = useInts[1];
				intExp = useInts[2];
			}else
			if(intJ == 2){
				intRan = Random.Range(0,100);
				if(intRan < BagSIT[5].inv.itemLevel){
					useInts = AllManage.AllMge.SoulAddExp(intQual , intLevel , intExp , 1);
					intQual = useInts[0];
					intLevel = useInts[1];
					intExp = useInts[2];
				}else{
					AllManage.tsStatic.Show("meg0216");
				}
			}else
			if(intJ > 2){
				intRan = Random.Range(0,1000);
				if(intRan < BagSIT[5].inv.itemLevel){
					useInts = AllManage.AllMge.SoulAddExp(intQual , intLevel , intExp , 1);
					intQual = useInts[0];
					intLevel = useInts[1];
					intExp = useInts[2];
				}else{
					AllManage.tsStatic.Show("meg0216");
				}
			}
			
//			intQual = EquepSIT[6].inv.itemQuality;
//			intLevel = EquepSIT[6].inv.itemLevel;
//			intExp = EquepSIT[6].inv.SoulExp;
			
			var strQual : String;
			var strLevel : String;
			var strExp : String;
			
			if(intQual != EquepSIT[6].inv.itemQuality){
				strQual = intQual.ToString();
				useUpdateSouStr = useUpdateSouStr.Substring(0,4) + strQual + useUpdateSouStr.Substring(5,5) ;		
				if(intQual - EquepSIT[6].inv.itemQuality > 0)
					AllManage.AllMge.TipsUpQual(intQual - EquepSIT[6].inv.itemQuality , pointEffectQualityUp);
			}
			if(intLevel != EquepSIT[6].inv.itemLevel){
				strLevel = intLevel.ToString();
				if(strLevel.Length < 2){
					strLevel = "0" + strLevel;
				}
				print(useUpdateSouStr + " === useUpdateSouStr");
				useUpdateSouStr = useUpdateSouStr.Substring(0,2) + strLevel + useUpdateSouStr.Substring(4,6) ;			
				if(intLevel - EquepSIT[6].inv.itemLevel > 0)
					AllManage.AllMge.TipsUpLevel(intLevel - EquepSIT[6].inv.itemLevel , pointEffectLevelUp);
			}
			if(intExp != EquepSIT[6].inv.SoulExp){
				strExp = intExp.ToString();
				print(strExp  + " =1= strExp");
				if(strExp.Length < 2){
					strExp = "0" + strExp;
				}	
				print(strExp  + " =2= strExp");
				useUpdateSouStr = useUpdateSouStr.Substring(0,6) + strExp + useUpdateSouStr.Substring(8,2) ;	
				print(useUpdateSouStr + " == useUpdateSouStr");
				if(intExp - EquepSIT[6].inv.SoulExp > 0)
					AllManage.AllMge.TipsUpExp(intExp - EquepSIT[6].inv.SoulExp);					
			}
			
			
			if(BagSIT[5].inv.itemProAbt == EquepSIT[6].inv.itemProAbt){
				var SkillLevel : String; 
				var intSkill : int;
				float2 = BagSIT[5].inv.SkillLevel;
				intSkill = EquepSIT[6].inv.SkillLevel + float1 *10/100* float2;
				SkillLevel = intSkill.ToString();
				print(SkillLevel + " == " + intSkill);
				if(intSkill == 0){
					SkillLevel = "00";
				}else
				if(SkillLevel.Length < 2){
					SkillLevel = "0" + SkillLevel;
				}else
				if(SkillLevel.Length > 2){
					SkillLevel = "99";
				}
				print(SkillLevel + " == SkillLevel");
				useUpdateSouStr = useUpdateSouStr.Substring(0,8) + SkillLevel;					
			}
			print(useUpdateSouStr + " == useUpdateSouStr");
			EquepSIT[6].invClear();
			useUpdateSou = new InventoryItem();
			useUpdateSou = AllResources.InvmakerStatic.GetItemInfo( useUpdateSouStr , useUpdateSou ); 
			EquepSIT[6].SetInv(useUpdateSou);
			BagSIT[5].invClear();
			UpdateEquepSoulItem(); 
			UpdateBagDigestItem();
			UpdateBagSoulItem();
			eatemits.emit=true;
		    yield WaitForSeconds(0.1);
		    eatemits.emit=false;
		}else{
			AllManage.tsStatic.Show("tips047");
		}
	}else{
		AllManage.tsStatic.Show("tips104");		
	}
}


var isSuccess : boolean = false;
var spriteSuccess : UISprite;
function BloodSuccess(){
	if(isSuccess){
		isSuccess = false;
		spriteSuccess.spriteName = "UIM_Prompt_Off";
	}else{
		isSuccess = true;
		spriteSuccess.spriteName = "UIM_Prompt_On";
	}
}

var BuildSDCostSoul : int[];
var BuildSDCostMoney : int[];
var LabelBuildSoulLevel : UILabel;
var emits :  ParticleEmitter[];
var Building : boolean = false;
var realSoulLevel : int = 1;
function ButtonBuildSoulAndDigest(){
	if(Building){
		return;
	}
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	
	var costBlood : int = 0;
	
	if(!isSuccess){
		costBlood = BuildSDCostMoney[realSoulLevel] ;
	}else{
			costBlood = BuildSDCostMoney[realSoulLevel] + 100;
	}
	
	if(BagHaveEmpty()){
		
		if((AllManage.jiaochengCLStatic.JiaoChengID == 8 && AllManage.jiaochengCLStatic.step == 1 && AllManage.InvclStatic.TutorialsDetectionAsID("81")) || (AllManage.jiaochengCLStatic.JiaoChengID == 8 && AllManage.jiaochengCLStatic.step == 3 && AllManage.InvclStatic.TutorialsDetectionAsID("83"))){
			realButtonBuildSoulAndDigest();
		}else{
			if(smeltNum > 0 && ps.isSoul( BuildSDCostSoul[realSoulLevel] ) ){
				if(!isSuccess){
					PanelStatic.StaticBtnGameManager.RunOpenLoading(function() ServerRequest.requestSmelt(0));						
				}else{
					if( ps.isBlood( costBlood ) ){
						if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
							AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesrequestSmelt" , "" , AllManage.AllMge.Loc.Get("info1192"));	
						else
							YesrequestSmelt();
					}else{
						AllManage.tsStatic.Show("tips060");		
					}
				}
			}else
			if( ps.isSoul( BuildSDCostSoul[realSoulLevel] ) && ps.isBlood( costBlood ) ){
				//ps.UseSoul( BuildSDCostSoul[realSoulLevel] );
	//			ps.UseMoney( , 0 ); 
				if(!isSuccess){
					AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.ButtonBuildSoulAndDigest , realSoulLevel , 0 , "0" , gameObject , "realButtonBuildSoulAndDigest");
				}else{
					if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
						AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"YesButtonBuildSoulAndDigest" , "" , AllManage.AllMge.Loc.Get("info1192"));	
					else
						YesButtonBuildSoulAndDigest();
				}
	//			AllManage.AllMge.UseMoney(BuildSDCostMoney[realSoulLevel]  , 0 , UseMoneyType.ButtonBuildSoulAndDigest , gameObject , "realButtonBuildSoulAndDigest");
			}
		}
	}else{
		AllManage.tsStatic.Show("tips049");		
	}
}

function YesrequestSmelt(){
	PanelStatic.StaticBtnGameManager.RunOpenLoading(function() ServerRequest.requestSmelt(1));	
}

function YesButtonBuildSoulAndDigest(){
	AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.ButtonBuildSoulAndDigest , realSoulLevel , 0 , "1" , gameObject , "realButtonBuildSoulAndDigest");			
}

function realButtonBuildSoulAndDigest(){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	var useItemLevel : String; 
	var ranNum : int; 
	var useItemID : String; 
	var useItemPin : String;
	var useItemAttr : String; 
	var inv : InventoryItem;  
	inv = new InventoryItem();
			var ranSuss : int;
			ranSuss = Random.Range(0,100);
			
			if(BagSoul == ";;;;;;" && EquipItemSoul == ";;;;;;;" || (AllManage.jiaochengCLStatic.JiaoChengID == 8 && AllManage.jiaochengCLStatic.step == 3) ){
				ranSuss = 101;
			}
			if(isSuccess){
				ranSuss += 30;
			}
			if(ranSuss > 100 - (realSoulLevel*10+20)){				
//			if(true){	
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.MakeSoulBtn).ToString());
				InventoryControl.yt.Rows[0]["AimMakeSoul"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimMakeSoul"].YuanColumnText) + 1).ToString();
				useItemLevel = parseInt( Mathf.Clamp( parseInt(ps.Level) , 1 + ((realSoulLevel - 1) * 20 ) , 20 + ((realSoulLevel - 1) * 20) )).ToString(); 
				
				ranNum = Random.Range(0,100);
				var useInt : int = 0;
				if(ranNum <48){
					 useInt = realSoulLevel - 1; 		
				}else
				if(ranNum <97){
					useInt = realSoulLevel; 		
				}else{
					 useInt = realSoulLevel + 1; 						
				}
				if(useInt > 5){
					 useInt = 5;
				}else
				if(useInt < 1){
					useInt = 1;
				}
				useItemAttr = useInt.ToString();
				useItemPin = useInt.ToString(); 
				if(useItemLevel.Length < 2){
					useItemLevel = "0" + useItemLevel;
				}
			
				ranNum = Random.Range(0,100);
				if(BagSoul == ";;;;;;" && EquipItemSoul == ";;;;;;;"){
					ranNum = 100;
				}
				if(AllManage.jiaochengCLStatic.JiaoChengID == 8 && AllManage.jiaochengCLStatic.step == 3 ){
					ranNum = 1;
				}
				if(isSuccess){
					ranNum += 30;
				}
				if(ranNum < 100 - (realSoulLevel*10)){
					useItemAttr = Random.Range( 1 , 9 ).ToString(); 
					useItemID = "70" + useItemLevel + useItemPin + useItemAttr;
				}else{
					useItemPin = ((realSoulLevel + 1)/2).ToString();
					useItemAttr = Random.Range(1,realSoulLevel + 1).ToString();
//					useItemID = "71" + useItemLevel + useItemPin + useItemAttr + "0101";			
					useItemID = "7120" + useItemPin + useItemAttr + "0101";			
				}

/////
//					useItemPin = ((realSoulLevel + 1)/2).ToString();
//					useItemAttr = Random.Range(1,realSoulLevel + 1).ToString();
//					useItemID = "71" + useItemLevel + useItemPin + "1" + "0101";			
/////
				emits[0].gameObject.animation.Play();
				emits[0].emit=true;
				Building = true;
				yield WaitForSeconds(1);
				emits[0].emit=false;
				emits[1].emit=true;
				yield WaitForSeconds(0.2);
				emits[1].emit=false;
				inv = AllResources.InvmakerStatic.GetItem(useItemID , inv);  
			//		//print(inv);
			//		//print(inv.slotType);
				if(inv != null){
					if(inv.slotType == SlotType.Soul){
						AddBagSoulItem(inv);
					}else
					if(inv.slotType == SlotType.Digest){
						AddBagDigestItem(inv);
					}
				}
				BuildSoulLevel += 1; 
				SetSoulBuildLabel();
				InventoryControl.yt.Rows[0]["BuildSoulLevel"].YuanColumnText = BuildSoulLevel.ToString();
				Building = false;
			}else{
				AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.SoulAndDigestGold , parseInt(ps.Level) , realSoulLevel , "" , gameObject , "");
				AllManage.tsStatic.Show("tips048");		
			}

}

function BagHaveEmpty() : boolean {
	var i : int = 0;
	var bool : boolean = true;
	for(i=0; i<5; i ++){
		if(BagSIT[i].inv == null){
			bool = false;
		}
	}
	if(bool){
		return false;
	}
	bool = true;
	for(i=0; i<NumDigestButtons; i ++){
		if(BagDIT[i].inv == null){
			bool = false;
		}
	}
	if(bool){
		return false;
	}
	return true;
}

function ButtonDesDS(inv : InventoryItem){
	yield;
	yield;
	yield;
	var i : int = 0;
	for(i=0; i<BagDIT.length; i++){
		if(BagDIT[i].inv != null){
			if(BagDIT[i].inv.slotType == SlotType.Digest && BagDIT[i].inv.itemID == inv.itemID){
				ps.UseSoulPower( (-1) * BagDIT[i].inv.itemQuality * BagDIT[i].inv.itemLevel );
				BagDIT[i].invClear();
				UpdateBagDigestItem();
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.EatMuisek).ToString());
				return;
			}
		}
	}
}


private var useUpdateInvStr : String;
private var useUpdateInv : InventoryItem;
function ButtonUpdateDS(){ 
	if(BagDIT[15].inv != null){
		if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
//			AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsYesUpdateDS , BagDIT[15].inv.itemQuality , BagDIT[15].inv.itemLevel , "" , gameObject , "YesUpdateDSTips");
			AllManage.qrStatic.ShowBuyQueRen1(gameObject , "YesUpdateDS" , "" , AllManage.AllMge.Loc.Get("info298")+ "" + (BagDIT[15].inv.itemQuality * BagDIT[15].inv.itemLevel * 20) + AllManage.AllMge.Loc.Get("info304")+""); 
		else
			YesUpdateDS();
	}else{
		AllManage.tsStatic.Show("tips046");		
	}
}


	function YesUpdateDSTips(objs : Object[]){
//			AllManage.qrStatic.ShowBuyQueRen1(gameObject , "YesUpdateDS" , "" , AllManage.AllMge.Loc.Get("info298")+ + AllManage.AllMge.Loc.Get("info304")+""); 
	}


var upemits :  ParticleEmitter;
var eatemits :  ParticleEmitter;
function YesUpdateDS(){
	if(BagDIT[15].inv != null){
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		} 
		if(ps.isSoulPower( BagDIT[15].inv.itemQuality * BagDIT[15].inv.itemLevel * 20)){
			if(BagDIT[15].inv.itemQuality < 5){
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.LevelUpMuisek).ToString());
				ps.UseSoulPower( BagDIT[15].inv.itemQuality * BagDIT[15].inv.itemLevel * 20);
				useUpdateInvStr = BagDIT[15].inv.itemID;	
				useUpdateInvStr = useUpdateInvStr.Substring(0,4) + parseInt(parseInt(useUpdateInvStr.Substring(4,1)) +1 ) +useUpdateInvStr.Substring(5,1);	
				BagDIT[15].invClear();
				useUpdateInv = new InventoryItem();
				useUpdateInv = AllResources.InvmakerStatic.GetItemInfo( useUpdateInvStr , useUpdateInv ); 
				BagDIT[15].SetInv(useUpdateInv);
				UpdateBagDigestItem();
				upemits.emit=true;
		        yield WaitForSeconds(0.1);
		        upemits.emit=false;
			}else{
				AllManage.tsStatic.Show("tips103");
			}
		}else{
			AllManage.tsStatic.Show("tips050");				
		}
		UpdateBagDigestItem();
	}else{
		AllManage.tsStatic.Show("tips046");		
	}
}

//////////////////////

//////////////////////
function SetBagSoulItem(invID : String){
	var i : int = 0;
	var useInv : InventoryItem;
	var useInvID : String[];
	useInvID = invID.Split(Fstr.ToCharArray());
	for(i=0; i<useInvID.length; i++){	 
		if(useInvID[i] != ""){ 
			useInv = new InventoryItem();
			useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
//			//print("bagSoul == " + useInv.itemID);
			BagSIT[i].SetInv(useInv);
		}
	}
	UpdateBagSoulItem();	
}

function AddBagSoulItem(inv : InventoryItem){
	var i : int = 0; 
	var j : int = 0;
	if(inv.slotType == SlotType.Soul){	
		for(i=0; i<BagSIT.length-1; i++){ 
			if(BagSIT[i].inv == null){
				BagSIT[i].SetInv(inv);
				UpdateBagSoulItem();
				return;
			}
		}
	}else{
		for(i=0; i<BagSIT.length-1; i++){
			if(BagSIT[i].inv){
				if(BagSIT[i].inv.itemID.Length > 5 && BagSIT[i].inv.consumablesNum != 0){
					if(BagSIT[i].inv.itemID.Substring(0,4) == inv.itemID.Substring(0,4)){
						if(BagSIT[i].inv.consumablesNum + inv.consumablesNum <= 20){				
							BagSIT[i].inv.consumablesNum += inv.consumablesNum;
							UpdateBagSoulItem();
							return;
						}else{
							for(j=0; j<BagSIT.length-1; j++){
								if(BagSIT[j].inv == null){ 
									inv.consumablesNum = BagSIT[i].inv.consumablesNum + inv.consumablesNum - 20;
									BagSIT[j].SetInv(inv);
									UpdateBagSoulItem();
									return;					
								}
							}
							BagSIT[i].inv.consumablesNum =20; 
							UpdateBagSoulItem();
							return;					
						}
					}	
				}
			}
		}
		for(i=0; i<BagSIT.length-1; i++){ 
			if(BagSIT[i].inv == null){  
				 BagSIT[i].SetInv(inv);
				BagSIT[i].showConsumablesNum();
				UpdateBagSoulItem();
				return;					
			}
		}
	}
}

function UpdateBagSoulItem(){
	BagSoul = ""; 
	var i : int = 0;
	for(i=0; i<BagSIT.length; i++){
		if(BagSIT[i].inv != null){
			BagSoul += BagSIT[i].inv.itemID;
		}
		BagSoul += ";";
	} 
	InventoryControl.yt.Rows[0]["BagSoul"].YuanColumnText = BagSoul;
}
//////////////////////upshowupshow

//////////////////////
function SetBagDigestItem(invID : String){
	var i : int = 0;
	var useInv : InventoryItem;
	var useInvID : String[];
	useInvID = invID.Split(Fstr.ToCharArray());
	for(i=0; i<useInvID.length; i++){	 
		if(useInvID[i] != ""){ 
			useInv = new InventoryItem();
			useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
//			//print("bagDigest == " + useInv.itemID);
			BagDIT[i].SetInv(useInv);
		}
	}
	UpdateBagDigestItem();	
}

function AddBagDigestItem(inv : InventoryItem){
	var i : int = 0; 
	var j : int = 0;
	if(inv.slotType == SlotType.Digest){	
		for(i=0; i<NumDigestButtons; i++){ 
			if(BagDIT[i].inv == null){
				BagDIT[i].SetInv(inv);
				UpdateBagDigestItem();
				return;
			}
		}
	}else{
		for(i=0; i<NumDigestButtons; i++){
			if(BagDIT[i].inv){
				if(BagDIT[i].inv.itemID.Length > 5 && BagDIT[i].inv.consumablesNum != 0){
					if(BagDIT[i].inv.itemID.Substring(0,4) == inv.itemID.Substring(0,4)){
						if(BagDIT[i].inv.consumablesNum + inv.consumablesNum <= 20){				
							BagDIT[i].inv.consumablesNum += inv.consumablesNum;
							UpdateBagDigestItem();
							return;
						}else{
							for(j=0; j<NumDigestButtons; j++){
								if(BagDIT[j].inv == null){ 
									inv.consumablesNum = BagDIT[i].inv.consumablesNum + inv.consumablesNum - 20;
									BagDIT[j].SetInv(inv);
									UpdateBagDigestItem();
									return;					
								}
							}
							BagDIT[i].inv.consumablesNum =20; 
							UpdateBagDigestItem();
							return;					
						}
					}	
				}
			}
		}
		for(i=0; i<NumDigestButtons; i++){ 
			if(BagDIT[i].inv == null){  
				 BagDIT[i].SetInv(inv);
				BagDIT[i].showConsumablesNum();
				UpdateBagDigestItem();
				return;					
			}
		}
	}
}

function UpdateBagDigestItem(){
	BagDigest = ""; 
	var i : int = 0;
	for(i=0; i<BagDIT.length; i++){
		if(BagDIT[i].inv != null){
			BagDigest += (BagDIT[i].inv.itemID);
		}
		BagDigest += ";";
	} 
	InventoryControl.yt.Rows[0]["BagDigest"].YuanColumnText = BagDigest;
}
//////////////////////

////////////////////// 
private var JStr : String = "#";
function SetEquepSoulItem(invID : String){
	var i : int = 0;
	var useInv : InventoryItem;
	var useInvID : String[];
	useInvID = invID.Split(QStr.ToCharArray());
	if(useInvID[0] != "" && useInvID.length > 1){
		useInv = AllResources.InvmakerStatic.GetItemInfo(invID , useInv);
		EquepSIT[6].SetInv(useInv);
		var useInvID1 : String[]; 
		useInvID1 = useInvID[1].Split(JStr.ToCharArray());
//		//print(useInvID[1]) ;
		for(i=0; i<useInvID1.length; i++){	 
//			//print(useInvID1[i] + " == useInvID1[i]");
			if(useInvID1[i] != "" && parseInt(useInvID1[i]) > 0){ 
				useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID1[i] , useInv);
				EquepSIT[i].SetInv(useInv);
			}
		}
	}
//	UpdateEquepSoulItem();	
}
//		var useInvID1 : String[]; 
//		useInvID1 = useInvID[1].Split(JStr.ToCharArray());
//		for(i=0; i<useInvID1.length; i++){	 
//			//print(useInvID1[i] + " == useInvID1[i]");
//			if(useInvID1[i] != "" && parseInt(useInvID1[i]) > 0){ 
//				useInv = invMaker.GetItemInfo(useInvID1[i] , useInv);
//				EquepSIT[i].SetInv(useInv);
//			}
//		}
//	}
//
var  varlineOfSightMask : LayerMask ;
var SoulParent : Transform;
var SoulObj : GameObject;
var SoulArray : GameObject[];
var rennman:rendercamerapic;
function GoShowSoul(inv : InventoryItem){
	invcl.peson.SendMessage("CallObjectSoul",inv.itemProAbt);
	if(sp == null&&PlayerStatus.MainCharacter){
		sp = PlayerStatus.MainCharacter.gameObject.GetComponent(SoulPet);
	}
	LabelSoulName.text = AllManage.AllMge.GetColorAsQuality(inv.itemQuality) + inv.itemName + "  Lv." + inv.itemLevel;
	yield;
	if(sp){
	SoulObj = sp.cc;
	rennman.RenderSc(SoulObj);
	}
//	if(SoulObj){
//		Destroy(SoulObj);
//	}
//	SoulObj = GameObject.Instantiate(sp.Soulobject[inv.itemProAbt - 1].gameObject);
//	if(SoulObj){
//		SoulObj.transform.parent = SoulParent;
//		SoulObj.transform.localPosition = Vector3.zero;
//		SoulObj.transform.localScale = Vector3.one;
//		SoulObj.GetComponent(NavMeshAgent).enabled = false;
//		SoulObj.GetComponent(SoulPetAI).enabled = false;
//		ChangeLayer(SoulObj , gameObject.layer);
//	}
}

function UpShowLevel(){
	yield WaitForSeconds(1);
	if(EquepSIT[6] != null && EquepSIT[6].inv != null)
		LabelSoulName.text = AllManage.AllMge.GetColorAsQuality( EquepSIT[6].inv.itemQuality) + EquepSIT[6].inv.itemName + "  Lv." + EquepSIT[6].inv.itemLevel;
}

function ChangeLayer (src : GameObject,Layer : int)
{
	src.layer = Layer;	
	for (var child : Transform in src.transform)
	{
		var curSrc = src.transform.Find(child.name);
		if (curSrc)
			ChangeLayer(curSrc.gameObject, Layer);
	}
}

function reMoveSoul(){
	LabelSoulName.text = AllManage.AllMge.Loc.Get("buttons545");
	rennman.cancelrenderCS(SoulObj);
	invcl.peson.SendMessage("reMoveSoul");
	
//	if(SoulObj){
//		Destroy(SoulObj);
//	}
}

function UpdateEquepSoulItem(){
	EquipItemSoul = ""; 
	var i : int = 0;
	if(EquepSIT[6].inv != null){
		EquipItemSoul += (EquepSIT[6].inv.itemID.Substring(0,10)) + "@";
	}
	for(i=0; i<EquepSIT.length-1; i++){
		if(EquepSIT[i].inv != null && EquepSIT[6].inv != null){
			EquipItemSoul += (EquepSIT[i].inv.itemID) + "#";
		}else{
			EquipItemSoul += "#";		
		}
	}
	SetEquepAnoTherSoulItem(EquipItemSoul);
	if(EquepSIT[6].inv != null){ 
		EquepSIT[6].inv.itemID = EquipItemSoul;
	}
	InventoryControl.yt.Rows[0]["EquipItemSoul"].YuanColumnText = EquipItemSoul;
	invcl.GetPersonEquipment();
} 

function UpdateOnEquepSoulItem(){
	EquipItemSoul = ""; 
	var i : int = 0;
	EquipItemSoul = EquepSIT[6].inv.itemID;
	SetEquepAnoTherSoulItem(EquipItemSoul);
	if(EquepSIT[6].inv != null){ 
		EquepSIT[6].inv.itemID = EquipItemSoul;
	}
	InventoryControl.yt.Rows[0]["EquipItemSoul"].YuanColumnText = EquipItemSoul;
	invcl.GetPersonEquipment();
} 

private var QStr : String = "@";
function SetEquepAnoTherSoulItem(invID : String){
	var i : int = 0;
	for(i=0; i<6; i++){
		EquepSIT[i].invClear();
	}
	var useInv : InventoryItem;
	var useInvID : String[];
	useInvID = invID.Split(QStr.ToCharArray()); 
	if(useInvID[0] != "" && useInvID.length > 1){
//		useInv = invMaker.GetItemInfo(invID , useInv);
//		EquepSIT[6].SetInv(useInv);
//		useInvID = invID.Split(Fstr.ToCharArray());
		var useInvID1 : String[]; 
		useInvID1 = useInvID[1].Split(JStr.ToCharArray());
		for(i=0; i<useInvID1.length; i++){	 
//			//print(useInvID1[i] + " == useInvID1[i]");
			if(useInvID1[i] != "" && parseInt(useInvID1[i]) > 0){ 
				useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID1[i] , useInv);
				EquepSIT[i].SetInv(useInv);
			}
		}
	}
//	UpdateEquepSoulItem();	
}

var EquipStatus : int[];
var useInvList : InventoryItem[];
function BuildEquepPes(){
	var i : int = 0;
	for(i=0; i<EquepSIT.length; i++){
		if(EquepSIT[i].inv != null){
			if(i < 6){
				RealPes(EquepSIT[i].inv);
			}else
			if(i == 6){
				invcl.SetRealSoul(EquepSIT[i].inv);
			}
		}
	}
}

private var invcl : InventoryControl;		
function RealPes(inv : InventoryItem){
	switch(inv.itemProAbt){
		case 1:
			invcl.EquipStatus[0] += inv.itemLevel * inv.itemQuality;
			break;
		case 2:
			invcl.EquipStatus[2] += inv.itemLevel * inv.itemQuality;
			break;
		case 3:
			invcl.EquipStatus[5] += inv.itemLevel * inv.itemQuality;
			break;
		case 4:
			invcl.EquipStatus[3] += inv.itemLevel * inv.itemQuality;
			break;
		case 5:
			invcl.EquipStatus[4] += inv.itemLevel * inv.itemQuality;
			break;
		case 6:
			invcl.EquipStatus[6] += inv.itemLevel * inv.itemQuality;
			break;
		case 7:
			invcl.EquipStatus[7] += inv.itemLevel * inv.itemQuality;
			break;
		case 8:
			invcl.EquipStatus[8] += inv.itemLevel * inv.itemQuality;
			break;
	}
}

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

function GetValueAndText(inv : InventoryItem , i){
	switch(inv.itemProAbt){
		case 1:
			ValueLookJingHuaArray[i] += inv.itemLevel * inv.itemQuality;
			TextLookJingHuaArray[i] = "攻击:";
			break;
		case 2:
			ValueLookJingHuaArray[i] += inv.itemLevel * inv.itemQuality;
			TextLookJingHuaArray[i] = "防御:";
			break;
		case 3:
			ValueLookJingHuaArray[i] += inv.itemLevel * inv.itemQuality;
			TextLookJingHuaArray[i] = "耐力:";
			break;
		case 4:
			ValueLookJingHuaArray[i] += inv.itemLevel * inv.itemQuality;
			TextLookJingHuaArray[i] = "专注:";
			break;
		case 5:
			ValueLookJingHuaArray[i] += inv.itemLevel * inv.itemQuality;
			TextLookJingHuaArray[i] = "致命:";
			break;
		case 6:
			ValueLookJingHuaArray[i] += inv.itemLevel * inv.itemQuality;
			TextLookJingHuaArray[i] = "力量:";
			break;
		case 7:
			ValueLookJingHuaArray[i] += inv.itemLevel * inv.itemQuality;
			TextLookJingHuaArray[i] = "敏捷:";
			break;
		case 8:
			ValueLookJingHuaArray[i] += inv.itemLevel * inv.itemQuality;
			TextLookJingHuaArray[i] = "智力:";
			break;
	}
}

//////////////////////

//////////////////////

var ptime : int;
private var updateTimes : int = 0;
function Update(){
	if(Time.time > ptime && Input.touchCount == 0 && Time.time > updateTimes && !Building ){
		ptime = Time.time + 1;
		UpdateEquepSIT6();
		LookButtonCreate(); 
		SetButtonsSoul();
	}else{
		if( Input.touchCount != 0 || Building){
			updateTimes = Time.time + 1;
		}
	}
}

var SpriteQuan1 : UISprite;
var SpriteQuan2 : UISprite;
function LookButtonCreate(){
	var realSoulLevel : int = 1;
	if(ps == null && PlayerStatus.MainCharacter){
	  var psman = PlayerStatus.MainCharacter;
	   if(psman)
		ps = psman.gameObject.GetComponent(PlayerStatus);
	}
	if(ps){
	if( ps.Soul >= BuildSDCostSoul[realSoulLevel] && parseInt( ps.Money ) >= BuildSDCostMoney[realSoulLevel] ){
//	if( ps.isSoul( BuildSDCostSoul[realSoulLevel] ) && ps.isMoney( BuildSDCostMoney[realSoulLevel] ) ){
		SpriteQuan1.enabled = true;
		SpriteQuan2.enabled = true;
	}else{
		SpriteQuan1.enabled = false;
		SpriteQuan2.enabled = false;	
	}
	}
}

var UpdateEquepSIT6bool : boolean = false;
function UpdateEquepSIT6(){
	UpdateEquepSIT6bool = false;
	if(EquepSIT[6].inv != null && invcl.rs != null){
		if(invcl.rs.attrLevel > EquepSIT[6].inv.SoulExp || invcl.rs.level > EquepSIT[6].inv.itemLevel){
			var str1 : String;
			var str2 : String;
			EquepSIT[6].inv.itemLevel = invcl.rs.level;
			str1 = EquepSIT[6].inv.itemLevel.ToString();
			EquepSIT[6].inv.SoulExp = invcl.rs.attrLevel;
			str2 = EquepSIT[6].inv.SoulExp.ToString();
			if(str1.Length < 2){
				str1 = "0" + str1;
			}else
			if(str1.Length > 2){
				str1 = "99";
			}
			if(str2.Length < 2){
				str2 = "0" + str2;
			}else
			if(str2.Length > 2){
				str2 = "99";
			}
			EquepSIT[6].inv.itemID = EquepSIT[6].inv.itemID.Substring(0,2) + str1 + EquepSIT[6].inv.itemID.Substring(4,2) + str2 + EquepSIT[6].inv.itemID.Substring(8,2);
			UpdateEquepSIT6bool = true;
		}
	}
	if(UpdateEquepSIT6bool){
		UpdateEquepSoulItem();
	}
}

static var NumDigestButtons : int = 5;
var BagDITGround : UISprite[];
function SetButtonsDigest(num : int){
	var i : int = 0;
	for(i=0; i<BagDITGround.length; i++){
		if(i<num){
			BagDITGround[i].enabled = false;
		}else{ 
			if(BagDITGround[i].spriteName != "Lock" || BagDITGround[i].enabled != true){
				BagDITGround[i].spriteName = "Lock";
				BagDITGround[i].enabled = true;	
			}
		}
	}
}

function ButtonPlusDigestBag(){
	if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
		AllManage.qrStatic.ShowBuyQueRen(gameObject , "NumDigestButtonsPlus" , "" , "messages014"); 
	else
		NumDigestButtonsPlus();
}

function NumDigestButtonsPlus(){
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.NumDigestButtonsPlus , 0 , 0 , "" , gameObject , "realNumDigestButtonsPlus");
//	AllManage.AllMge.UseMoney(0 , 5 , UseMoneyType.NumDigestButtonsPlus , gameObject , "realNumDigestButtonsPlus");
//	if(ps.UseMoney( 0 , 5 )){
//	}
}

function realNumDigestButtonsPlus(){
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.OpenMoreMuisek).ToString());
		NumDigestButtons += 1;
		InventoryControl.yt.Rows[0]["BagDigestNum"].YuanColumnText = NumDigestButtons.ToString();
		SetButtonsDigest(NumDigestButtons);	

}

var LabelButtonsSoul : UILabel[];
var IntSoulLevel : int[]; 
var BagSoulGround : UISprite[];
function SetButtonsSoul(){
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	if(ps){
	var i : int = 0;
	for(i=0; i<BagSoulGround.length; i++){
		if(parseInt(ps.Level) < IntSoulLevel[i]){  
			if(BagSoulGround[i].spriteName != "Lock" || BagSoulGround[i].enabled != true){
				BagSoulGround[i].spriteName = "Lock";	
				BagSoulGround[i].enabled = true;		
			}
			if(LabelButtonsSoul[i].enabled != true){			
				LabelButtonsSoul[i].enabled = true;		
			}
		}else{
			if(BagSoulGround[i].spriteName != false){			
				BagSoulGround[i].enabled = false;		
			}
			if(LabelButtonsSoul[i].enabled != false){			
				LabelButtonsSoul[i].enabled = false;				
			}
		}
	}
	}
}

function ClearAnyItem(Any : BagItem[]){
	for(var i=0; i<Any.length; i++){
		Any[i].invClear();
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
 
function show0(){
	AllManage.UIALLPCStatic.show0();
}
var tween : TweenPosition;
function Play(){
	tween.Play(true);
	emits[0].emit=false;
	Building = false;
}

function OnEnable(){
	shanSoul.enabled = false;
	shanDigest.enabled = false;
	if(isSuccess){
		spriteSuccess.spriteName = "UIM_Prompt_On";
	}else{
		spriteSuccess.spriteName = "UIM_Prompt_Off";
	}
	 UpShowLevel();
}

function SetSoulBuildLabel(){
	if(smeltNum <= 0){
		if(BuildSoulLevel < 100){
			realSoulLevel = 1;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages098");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("buttons294");
			AllManage.AllMge.Keys.Add("20");
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildSoulLevel);
		}else
		if(BuildSoulLevel < 300){
			realSoulLevel = 2;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages099");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("buttons294");
			AllManage.AllMge.Keys.Add("25");
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildSoulLevel);
		} else
		if(BuildSoulLevel < 600){
			realSoulLevel = 3;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages100");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("buttons294");
			AllManage.AllMge.Keys.Add("29");
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildSoulLevel);
		}else
		if(BuildSoulLevel < 1000){
			realSoulLevel = 4;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages101");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("buttons294");
			AllManage.AllMge.Keys.Add("32");
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildSoulLevel);
		}else{
			realSoulLevel = 5;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages102");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("buttons294");
			AllManage.AllMge.Keys.Add("34");
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildSoulLevel);
		}	
	}else{
		if(BuildSoulLevel < 100){
			realSoulLevel = 1;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages098");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("meg0160");
			AllManage.AllMge.Keys.Add(smeltNum.ToString());
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildSoulLevel);
		}else
		if(BuildSoulLevel < 300){
			realSoulLevel = 2;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages099");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("meg0160");
			AllManage.AllMge.Keys.Add(smeltNum.ToString());
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildSoulLevel);
		} else
		if(BuildSoulLevel < 600){
			realSoulLevel = 3;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages100");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("meg0160");
			AllManage.AllMge.Keys.Add(smeltNum.ToString());
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildSoulLevel);
		}else
		if(BuildSoulLevel < 1000){
			realSoulLevel = 4;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages101");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("meg0160");
			AllManage.AllMge.Keys.Add(smeltNum.ToString());
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildSoulLevel);
		}else{
			realSoulLevel = 5;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages102");
			AllManage.AllMge.Keys.Add("\n");
			AllManage.AllMge.Keys.Add("meg0160");
			AllManage.AllMge.Keys.Add(smeltNum.ToString());
			AllManage.AllMge.SetLabelLanguageAsID(LabelBuildSoulLevel);
		}			
	}
	Label5.text =  "/" + BuildSDCostSoul[realSoulLevel];
}


function ReturnrequestSmelt(num : int){
	smeltNum = num;
	SetSoulBuildLabel();
	ps.UseSoul( BuildSDCostSoul[realSoulLevel] );
	realButtonBuildSoulAndDigest();
}

function RerequestSmeltGetNum(num : int){
	smeltNum = num;
	SetSoulBuildLabel();
}
