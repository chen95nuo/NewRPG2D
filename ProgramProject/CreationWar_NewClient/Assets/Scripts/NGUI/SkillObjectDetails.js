#pragma strict

private var skillstr : String = "ProID_";
var infoObj : GameObject;
function Awake(){
	AllManage.SkillObjDet = this;
	AllManage.UICLStatic.SkillObjREtrun();
}

function OnEnable(){
	AllManage.SkillCLStatic.SkillInit();
}

var skillID : String;
var scObj : Skillclass;

var SpriteSkill : UISprite;
var LabelSkillName : UILabel;
var LabelSkillInfo : UILabel;
var TransUpButton : Transform;
var oldSkilObj : GameObject;
var LabelSkillName1 : UILabel;
var LabelSkillName2 : UILabel;
var LabelSkillName3 : UILabel;
var LabelSkillName4 : UILabel;

var LabelSkillLeve1 : UILabel;
var LabelSkillLeve2 : UILabel;
var LabelSkillLeve3 : UILabel;
var LabelSkillLeve4 : UILabel;

var Spriteskim1 : UISprite;
var Spriteskim2 : UISprite;
var Spriteskim3 : UISprite;
var Spriteskim4 : UISprite;
var Labelskim1 : UILabel;
var Labelskim2 : UILabel;
var Labelskim3 : UILabel;
var Labelskim4 : UILabel;
var LabelUpDate : UILabel;
var LabelNextLevel : UILabel;
var parentUseButtons : Transform;
var SpriteUpDate : UISprite;
var skillLevel : int = 0;
var LabelSkillUpdate : UILabel;
function showOneSkill(level : String , sc : Skillclass , id : String , info : String , canUp : boolean , obj : GameObject){
//	print(info + " == info");
	skillLevel = parseInt(level);
	parentUseButtons.localPosition.y= 0;
	action = 0;
	TransUpButton.localPosition.y = -194;
	if(canUp){
		upCanClick = true;
		SpriteUpDate.spriteName = "UIH_Minor_Button_N";
		LabelUpDate.text = AllManage.AllMge.Loc.Get( "info681" );
	}else{
		upCanClick = false;
		SpriteUpDate.spriteName = "UIH_Minor_Button_O";
	//	TransUpButton.localPosition.y = 3000;
	}
	if(skillID.Length < 2){
		LabelNextLevel.text = "";
	}else
	if(parseInt(level.Substring(0,1)) < 3){
		LabelNextLevel.text = AllManage.SkillCLStatic.GetNextLL(parseInt(id)-1 , parseInt(level.Substring(0,1))) + AllManage.AllMge.Loc.Get( "info734" );
	}else{
		LabelNextLevel.text = AllManage.AllMge.Loc.Get( "info735" );	
	}
	oldSkilObj = obj;
//	//print(skim1.invSprite.enabled + " == skim1.invSprite.enabled");
	if(skim1.invSprite.enabled){
		Spriteskim1.enabled = true;
//		//print( skim1.invSprite.spriteName + " ==  skim1.invSprite.spriteName");
		Labelskim1.text = AllManage.AllMge.Loc.Get("");
		Spriteskim1.spriteName = skim1.invSprite.spriteName.ToString();
		
		var objs : Object[] = new Object[3] ;
		objs[0] = skim1.skillID ; 
		objs[1] = LabelSkillName1 ; 
		objs[2] = LabelSkillLeve1 ; 

		AllManage.SkillCLStatic.SetSkillLabelAsID(objs);
	}else{
		Labelskim1.text = AllManage.AllMge.Loc.Get("messages061");
		Spriteskim1.enabled = false;
		LabelSkillName1.text = "";
		LabelSkillLeve1.text = "";
	}
	
//	//print(skim2.invSprite.enabled + " == skim2.invSprite.enabled");
	if(skim2.invSprite.enabled){
		Labelskim2.text = AllManage.AllMge.Loc.Get("");
		Spriteskim2.enabled = true;
//		//print( skim2.invSprite.spriteName + " ==  skim2.invSprite.spriteName");
		Spriteskim2.spriteName = skim2.invSprite.spriteName.ToString();
		
		var objsSkill2 : Object[] = new Object[3] ;
		objsSkill2[0] = skim2.skillID ; 
		objsSkill2[1] = LabelSkillName2 ; 
		objsSkill2[2] = LabelSkillLeve2 ; 

		AllManage.SkillCLStatic.SetSkillLabelAsID(objsSkill2);
		
	}else{
		Labelskim2.text = AllManage.AllMge.Loc.Get("messages061");
		Spriteskim2.enabled = false;
		LabelSkillName2.text = "";
		LabelSkillLeve2.text = "";
	}
	
//	//print(skim3.invSprite.enabled + " == skim1.invSprite.enabled");
	if(skim3.invSprite.enabled){
		Labelskim3.text = AllManage.AllMge.Loc.Get("");
		Spriteskim3.enabled = true;
//		//print( skim3.invSprite.spriteName + " ==  skim3.invSprite.spriteName");
		Spriteskim3.spriteName = skim3.invSprite.spriteName.ToString();
		
		var objsSkill3 : Object[] = new Object[3] ;
		objsSkill3[0] = skim3.skillID ; 
		objsSkill3[1] = LabelSkillName3 ; 
		objsSkill3[2] = LabelSkillLeve3 ; 

		AllManage.SkillCLStatic.SetSkillLabelAsID(objsSkill3);
		
	}else{
		Labelskim3.text = AllManage.AllMge.Loc.Get("messages061");
		Spriteskim3.enabled = false;
		LabelSkillName3.text = "";
		LabelSkillLeve3.text = "";
	}
//	//print(skim4.invSprite.enabled + " == skim1.invSprite.enabled");
	if(skim4.invSprite.enabled){
		Labelskim4.text = AllManage.AllMge.Loc.Get("");
		Spriteskim4.enabled = true;
//		//print( skim4.invSprite.spriteName + " ==  skim4.invSprite.spriteName");
		Spriteskim4.spriteName = skim4.invSprite.spriteName.ToString();
		
		var objsSkill4 : Object[] = new Object[3] ;
		objsSkill4[0] = skim4.skillID ; 
		objsSkill4[1] = LabelSkillName4 ; 
		objsSkill4[2] = LabelSkillLeve4 ; 

		AllManage.SkillCLStatic.SetSkillLabelAsID(objsSkill4);
	}else{
		Labelskim4.text = AllManage.AllMge.Loc.Get("messages061");
		Spriteskim4.enabled = false;
		LabelSkillName4.text = "";
		LabelSkillLeve4.text = "";
	}
	yield;
	yield;
	
	infoObj.transform.localPosition.y = 0;
	infoObj.SetActiveRecursively(true);
	skillID = id;
	scObj = sc;
	LabelSkillName.text = AllManage.AllMge.Loc.Get(sc.name) + "lv." + level.Substring(0,1);
	LabelSkillInfo.text = info;
	if(level.Substring(0,1) == "0"){
		LabelSkillUpdate.text = AllManage.AllMge.Loc.Get("buttons824");
	}else
	if(level.Substring(0,1) == "3"){
		LabelSkillUpdate.text = AllManage.AllMge.Loc.Get("buttons825");
	}else
	{	
		LabelSkillUpdate.text = AllManage.AllMge.Loc.Get("info681");
	}
	try{
		if(parseInt(skillID) > 15){
			useStr = (parseInt(skillID) - 15).ToString();
		}else{
			useStr = skillID;
		}
	}catch(e){
	
	}
	////print(skillstr + InventoryControl.yt.Rows[0]["ProID"].YuanColumnText + skillID + " == spname");
	
	SpriteSkill.spriteName = skillstr + InventoryControl.yt.Rows[0]["ProID"].YuanColumnText + useStr;
}
private var useStr : String;
//function Update(){
//	if (Input.GetButtonUp ("Fire1") || Input.touchCount > 1) {  
//		if( !(AllManage.jiaochengCLStatic.JiaoChengID == 1 && AllManage.jiaochengCLStatic.step == 3) && !(AllManage.jiaochengCLStatic.JiaoChengID == 1 && AllManage.jiaochengCLStatic.step == 4))
//		closeThis();
//	}
//}

var action : int = 0;
var upCanClick : boolean = false;
function showOneSkill(level : String , sc : Skillclass , id : String , info : String , canUp : boolean , obj : GameObject , actionID : int , spName : String , skName : String){
	skillLevel = parseInt(level);
	TransUpButton.localPosition.y = -194;
	parentUseButtons.localPosition.y= 3000;
	action = actionID;
	TransUpButton.localPosition.y = -194;
	if(actionID != 0){
		upCanClick = true;
		LabelUpDate.text = AllManage.AllMge.Loc.Get( "info681" );
		SpriteUpDate.spriteName = "UIH_Minor_Button_N";
	}else{
		upCanClick = false;
		SpriteUpDate.spriteName = "UIH_Minor_Button_O";
	//	TransUpButton.localPosition.y = 3000;
	}
	oldSkilObj = obj;
	if(skillID.Length < 2){
		LabelNextLevel.text = "";
	}else
	if(parseInt(level.Substring(0,1)) < 3){
		LabelNextLevel.text = AllManage.SkillCLStatic.GetNextLL(parseInt(id)-1 , parseInt(level.Substring(0,1))) + AllManage.AllMge.Loc.Get( "info734" );
	}else{
		LabelNextLevel.text = AllManage.AllMge.Loc.Get( "info735" );	
	}
//	//print(skim1.invSprite.enabled + " == skim1.invSprite.enabled");
	if(skim1.invSprite.enabled){
		Spriteskim1.enabled = true;
//		//print( skim1.invSprite.spriteName + " ==  skim1.invSprite.spriteName");
		Spriteskim1.spriteName = skim1.invSprite.spriteName.ToString();
//		LabelSkillName1.text =  skim1.PassObj.name;
		Debug.Log("ran5555555555555555555555============================"+skim1.SkillObj.name);
	}else{
		Spriteskim1.enabled = false;
	}
	
//	//print(skim2.invSprite.enabled + " == skim2.invSprite.enabled");
	if(skim2.invSprite.enabled){
		Spriteskim2.enabled = true;
//		//print( skim2.invSprite.spriteName + " ==  skim2.invSprite.spriteName");
		Spriteskim2.spriteName = skim2.invSprite.spriteName.ToString();
	}else{
		Spriteskim2.enabled = false;
	}
	
//	//print(skim3.invSprite.enabled + " == skim1.invSprite.enabled");
	if(skim3.invSprite.enabled){
		Spriteskim3.enabled = true;
//		//print( skim3.invSprite.spriteName + " ==  skim3.invSprite.spriteName");
		Spriteskim3.spriteName = skim3.invSprite.spriteName.ToString();
	}else{
		Spriteskim3.enabled = false;
	}
//	//print(skim4.invSprite.enabled + " == skim1.invSprite.enabled");
	if(skim4.invSprite.enabled){
		Spriteskim4.enabled = true;
//		//print( skim4.invSprite.spriteName + " ==  skim4.invSprite.spriteName");
		Spriteskim4.spriteName = skim4.invSprite.spriteName.ToString();
	}else{
		Spriteskim4.enabled = false;
	}
	yield;
	yield;
	
	infoObj.transform.localPosition.y = 0;
	infoObj.SetActiveRecursively(true);
	skillID = id;
	scObj = sc;
	LabelSkillName.text = skName;
	LabelSkillInfo.text = "";
	SpriteSkill.spriteName = spName;
	LabelUpDate.text = AllManage.AllMge.Loc.Get( "buttons001" );
	TransUpButton.localPosition.y = -194;
}

function close(){
	closeThis();
}

function closeThis(){
	yield;
	if(infoObj.active){
		infoObj.transform.localPosition.y = 3000;
		infoObj.SetActiveRecursively(false);		
	}
}

function reOpenInfo(){
	yield closeThis();
	yield;
	if(oldSkilObj){
		oldSkilObj.SendMessage("OnClick" , SendMessageOptions.DontRequireReceiver);
	}
}

function upSkill(){
	if(!upCanClick){
		return;
	}
	switch(action){
		case 0:
			//print(oldSkilObj + " == oldSkilObj");
			if(oldSkilObj){
				oldSkilObj.SendMessage("updateSkill" , SendMessageOptions.DontRequireReceiver);
			}
			break;
		case 1:
			if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.CookSwitch) == "0"){
				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
				return;
			}
			AllManage.UICLStatic.closeNewPlayerInfo();
			AllManage.UICLStatic.cookReslist();
			break;
		case 2:
			if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.EqupmentBuildSwitch) == "0"){
				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
				return;
			}
			AllManage.UICLStatic.closeNewPlayerInfo();
			AllManage.UICLStatic.produceReslist();
			break;
	}
}

var skim1 : SkillItem;
function select1(){
//	//print("11111");
	if(skillLevel <= 0){
		return;
	}
//	if(skim1.invSprite.enabled){
//		if(oldSkilObj)
//		skim1.reMoveSkill();		
//	}else{
		if(oldSkilObj)
//		skim1.reMoveSkill();	
		skim1.setRealSKill(oldSkilObj.GetComponent(SkillItem));
//	}
}

var skim2 : SkillItem;
function select2(){
	if(skillLevel <= 0){
		return;
	}
//	//print("11111");
//	if(skim2.invSprite.enabled){
//		if(oldSkilObj)
//		skim2.reMoveSkill();		
//	}else{
		if(oldSkilObj)
//		skim2.reMoveSkill();	
		skim2.setRealSKill(oldSkilObj.GetComponent(SkillItem));
//	}
}

var skim3 : SkillItem;
function select3(){
	if(skillLevel <= 0){
		return;
	}
//	//print("11111");
//	if(skim3.invSprite.enabled){
//		if(oldSkilObj)
//		skim3.reMoveSkill();		
//	}else{
		if(oldSkilObj)
//		skim3.reMoveSkill();		
		skim3.setRealSKill(oldSkilObj.GetComponent(SkillItem));
//	}

}
var skim4 : SkillItem;
function select4(){
	if(skillLevel <= 0){
		return;
	}
//	//print("11111");
//	if(skim4.invSprite.enabled){
//		if(oldSkilObj)
//		skim4.reMoveSkill();		
//	}else{
		if(oldSkilObj)
//		skim4.reMoveSkill();
		skim4.setRealSKill(oldSkilObj.GetComponent(SkillItem));
//	}
}
