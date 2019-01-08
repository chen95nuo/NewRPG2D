#pragma strict
//enum TipsType{
//	upSkillPoint = 1,
//	upgrade = 2,
//	newSkill = 3,
//	combat = 4
//}

function Awake(){
	AllManage.tipclStatic = this;
}

function Update () {
	if(Time.time > ptimeTextButton && boolTextButton){
//		objTextButton.SetActiveRecursively(false);
//		spriteBezelTextButton.enabled = true;
		boolTextButton = false;
	}
	if(parseInt(AllManage.psStatic.Level)<=5||!isHaveNewSkill||parseInt(AllManage.psStatic.Level)==60||SpriteNewSkills.spriteName == "dunpai"){
	if(Time.time > ptimeUpgrade && boolUpgrade){
		closeUpgrade();
		
	}
	}
	if(Time.time > ptimeNewSkills && boolNewSkills && !isHaveNewSkill){
		
//		spriteBezelNewSkills.enabled = true;
//		boolNewSkills = false;
	}
	if(Time.time > ptimeCombat && boolCombat){
		objCombat.SetActive(false);
		boolCombat = false;
	}
}

function closeUpgrade(){
	objUpgrade.SetActive(false);
	objNewSkills.SetActive(false);
	boolUpgrade = false;
	isHaveNewSkill = false;
}

//var labelTextButtonFirst : UILabel;
//var labelTextButtonMiddle : UILabel;
//var labelTextButtonLast : UILabel;
var labelTextButton : UILabel;
var objTextButton : GameObject;
var ptimeTextButton : int = 0;
var boolTextButton : boolean = false;
var objReturnButton : GameObject;
var objFunctionName : String;
var isHaveNewSkill : boolean = false;
public var isClickTips : boolean = false;
//var spriteBezelTextButton : UISprite;
function ShowTextButton(strFirst : String , strMiddle : String , strLast : String , strButton , obj : GameObject , funcName : String){
//	labelTextButtonFirst.text = strFirst;
//	labelTextButtonMiddle.text = strMiddle;
//	labelTextButtonLast.text = strLast;
	labelTextButton.text = strButton;
	ptimeTextButton = Time.time + 8;
//	objTextButton.SetActiveRecursively(true);
	//spriteBezelTextButton.enabled = false;
	boolTextButton = true;
	objReturnButton = obj;
	objFunctionName = funcName;
}

function ClickTextButton(){
	if(objReturnButton){
		objReturnButton.SendMessage(objFunctionName , SendMessageOptions.DontRequireReceiver);
	}
	closeUpgrade();
	isClickTips = true;
	isHaveNewSkill = false;
}

function ClickUpgradeButton(){
	if(isHaveNewSkill&&parseInt(AllManage.psStatic.Level)>5){
	AllManage.UICLStatic.SkillMoveOn();
	closeUpgrade();
	}else{
	closeUpgrade();
	}
}

var labelUpgradeText : UILabel;
var objUpgrade : GameObject;
var ptimeUpgrade : int = 0;
var boolUpgrade : boolean = false;
function ShowUpgrade(str : String){
//	SpriteNewSkills.spriteName = AllManage.InvclStatic.GetProSpriteName();
	labelUpgradeText.text = str;

	objUpgrade.SetActive(true)	;
	ptimeUpgrade = Time.time + 4;
	boolUpgrade = true;
	
	if(!boolNewSkills)
	{
//	SpriteNewSkills.spriteName=="";					//新技能图标
//	if(LblNewSkill){
//	LblNewSkill.enabled = true ;					//有新技能可学
//	}
//	labelNewSkillsText.text = str;					//技能名称
	isHaveNewSkill = true;							//不关闭升级UI
	boolNewSkills = true;
	}
	
	
}

var labelNewSkillsText : UILabel;
var SpriteNewSkills : UISprite	;
var LblNewSkill : UILabel 		; 
var SpriteNewTipS : UISprite	;
var objNewSkills : GameObject	;
var ptimeNewSkills : int = 0	;
var boolNewSkills : boolean = true;
//var spriteBezelNewSkills : UISprite;
function ShowNewSkills(str : String , spriteName : String){
	labelNewSkillsText.text = str;
	SpriteNewSkills.spriteName = spriteName;
	if(LblNewSkill){
	LblNewSkill.enabled = true ;
	}
	isHaveNewSkill = true;
	SpriteNewSkills.enabled = true;
	SpriteNewTipS.enabled = true; 
//	spriteBezelNewSkills.enabled = false;
	objNewSkills.SetActive(true);
//	ptimeNewSkills = Time.time + 4;
	boolNewSkills = false;
//	Debug.Log("ran==================================000000000000000000000000000000");
}

var labelCombatText : UILabel;
var labelCombatIsUp : UILabel;
var objCombat : GameObject;
var ptimeCombat : int = 0;
var boolCombat : boolean = false;
var SpriteisUp : UISprite;
function ShowCombat (str : String , isUp : boolean){
	if(isUp){
		labelCombatText.text = "[ffc600]" + str;
		SpriteisUp.spriteName = "up_h";
		labelCombatIsUp.text =  AllManage.AllMge.Loc.Get("info892") ;
	}else{
		labelCombatText.text = "[00ffff]" + str;
		SpriteisUp.spriteName = "down_lv";
		labelCombatIsUp.text =  AllManage.AllMge.Loc.Get("info893") ;
	}
	objCombat.SetActive(true);
	ptimeCombat = Time.time + 3;
	boolCombat = true;
}
	