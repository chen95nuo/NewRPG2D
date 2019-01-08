#pragma strict

private var ps : PlayerStatus;
var LeftText1 : UILabel;
var LeftText2 : UILabel;
var LeftText3 : UILabel;
var LeftText4 : UILabel;
var LeftText5 : UILabel;

var MidText1 : UILabel;
var MidText2 : UILabel;
var MidText3 : UILabel;
var MidText4 : UILabel;
var MidText5 : UILabel;

var RightText1 : UILabel;
var RightText2 : UILabel;
var RightText3 : UILabel;
var RightText4 : UILabel;
var RightText5 : UILabel;

var RightXiaoHaoText1 : UILabel;
var RightXiaoHaoText2 : UILabel;
var RightXiaoHaoText3 : UILabel;
var RightXiaoHaoText4 : UILabel;
var RightXiaoHaoText5 : UILabel;

var LabelNonPoint : UILabel;

var emits :  ParticleEmitter[];

function DoFXing(){
	for (var i : int = 0; i < emits.length; i++){
      emits[i].emit=true;
    }
     TimeOut();
}

function TimeOut() {
    yield WaitForSeconds(0.1);
	for (var i : int = 0; i < emits.length; i++){
      emits[i].emit=false;
    }
}

static var me : GameObject;
function Awake(){
	me = gameObject;
	uicl = AllManage.UICLStatic;
	invcl = AllManage.InvclStatic;
	jiaocheng = AllManage.jiaochengCLStatic;
	JiaoCheng = AllManage.jiaochengCLStatic;
//	ts = AllManage.tsStatic;
//	alluipc = AllManage.UIALLPCStatic;
}

var buttons : GameObject[];
private var jiaocheng : TaskJiaoChengControl;
function Start () {
	jiaocheng.allJiaoCheng[3].obj[1] = buttons[0];
	jiaocheng.allJiaoCheng[3].obj[2] = buttons[1];
//	jiaocheng.allJiaoCheng[3].obj[3] = buttons[2];
	var mm : boolean = false;
	while(!mm){
		if(PlayerStatus.MainCharacter != null){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			mm = true;
		}
		yield;
	}
	GetMaxPoint();
	ShowObj(false); 
	ObjSave.SetActiveRecursively(false);
	ObjSave.transform.localPosition.y = 2000;
	ObjXunLian.transform.localPosition.y = 290.8;
	yield;
	ShowXunLianButton(ps.VIPLevel);
//	CloseHelp();
}

function OnEnable(){
	ShowObj(false);
//	CloseHelp();
}


var need:int = 0;



var pTime : int = 0 ;
function Update () {
	if(Time.time > pTime){ 
		pTime = Time.time + 1;
		if(ps != null){
			need = 0;
			for(var i=parseInt(ps.Level);i>0;i--)
			{
				need+=i*2+6;
			}
            /*
			RightXiaoHaoText1.text = AllManage.AllMge.Loc.Get("buttons471") + Mathf.Clamp(((need-ps.NonPoint)/6)*40+500,500,4800);
//			RightXiaoHaoText2.text = AllManage.AllMge.Loc.Get("buttons471") + 2;
			RightXiaoHaoText3.text = AllManage.AllMge.Loc.Get("buttons471") + 12;
			RightXiaoHaoText4.text = AllManage.AllMge.Loc.Get("buttons471") + 20;
			RightXiaoHaoText5.text = AllManage.AllMge.Loc.Get("buttons471") + 40;
            */

			RightXiaoHaoText1.text = AllManage.AllMge.Loc.Get("buttons471") + TrainingViplevel.instance.shiBing;
			RightXiaoHaoText3.text = AllManage.AllMge.Loc.Get("buttons471") + TrainingViplevel.instance.yongShi;
			RightXiaoHaoText4.text = AllManage.AllMge.Loc.Get("buttons471") + TrainingViplevel.instance.qiShi;
			RightXiaoHaoText5.text = AllManage.AllMge.Loc.Get("buttons471") + TrainingViplevel.instance.duJun;
		
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages105");
			AllManage.AllMge.Keys.Add("\n" + ps.Stamina);
			AllManage.AllMge.SetLabelLanguageAsID(LeftText1);
//			LeftText1.text = "耐力\n" + ps.Stamina;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages106");
			AllManage.AllMge.Keys.Add("\n" + ps.Strength);
			AllManage.AllMge.SetLabelLanguageAsID(LeftText2);
//			LeftText2.text = "力量\n" + ps.Strength;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages107");
			AllManage.AllMge.Keys.Add("\n" + ps.Agility);
			AllManage.AllMge.SetLabelLanguageAsID(LeftText3);
//			LeftText3.text = "敏捷\n" + ps.Agility;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages108");
			AllManage.AllMge.Keys.Add("\n" + ps.Intellect);
			AllManage.AllMge.SetLabelLanguageAsID(LeftText4);
//			LeftText4.text = "智力\n" + ps.Intellect;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages109");
			AllManage.AllMge.Keys.Add("\n" + ps.Focus);
			AllManage.AllMge.SetLabelLanguageAsID(LeftText5);
//			LeftText5.text = "专注\n" + ps.Focus;		
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("" + ps.NonPoint);
			AllManage.AllMge.Keys.Add("messages110");
			AllManage.AllMge.SetLabelLanguageAsID(LabelNonPoint);
//			LabelNonPoint.text = ps.NonPoint + "点";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages111");
			AllManage.AllMge.Keys.Add("\n" + maxPoint);
			AllManage.AllMge.SetLabelLanguageAsID(RightText1);
//			RightText1.text = "最大值\n" + maxPoint;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages111");
			AllManage.AllMge.Keys.Add("\n" + maxPoint);
			AllManage.AllMge.SetLabelLanguageAsID(RightText2);
//			RightText2.text = "最大值\n" + maxPoint;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages111");
			AllManage.AllMge.Keys.Add("\n" + maxPoint);
			AllManage.AllMge.SetLabelLanguageAsID(RightText3);
//			RightText3.text = "最大值\n" + maxPoint;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages111");
			AllManage.AllMge.Keys.Add("\n" + maxPoint);
			AllManage.AllMge.SetLabelLanguageAsID(RightText4);
//			RightText4.text = "最大值\n" + maxPoint;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages111");
			AllManage.AllMge.Keys.Add("\n" + (1000 + maxPoint));
			AllManage.AllMge.SetLabelLanguageAsID(RightText5);
//			RightText5.text = "最大值\n" + (1000 + maxPoint);	
		}
	}
//	if (Input.GetButtonUp ("Fire1")) { 
//		its = new Array(5);
//		its = AddRandomPoint(its , yuse);
//	}
}

//服务器返回时调用，重新给vip限制等级赋值
function ShowTrainingVip()
{
    for(var i=0; i<5; i++){
        SpriteButtons[i].spriteName = "UIH_Main_Button_O";
    }

    if(ps.VIPLevel<0){
            SpriteButtons[0].spriteName = "UIH_Main_Button_N";
            SpriteButtons[1].spriteName = "UIH_Main_Button_N";
    }
    else if(ps.VIPLevel<TrainingViplevel.instance.v1){
        SpriteButtons[0].spriteName = "UIH_Main_Button_N";
        SpriteButtons[1].spriteName = "UIH_Main_Button_N";
        SpriteButtons[2].spriteName = "UIH_Main_Button_N";
    }
    else if(ps.VIPLevel<TrainingViplevel.instance.v2){
        SpriteButtons[0].spriteName = "UIH_Main_Button_N";
        SpriteButtons[1].spriteName = "UIH_Main_Button_N";
        SpriteButtons[2].spriteName = "UIH_Main_Button_N";
        SpriteButtons[3].spriteName = "UIH_Main_Button_N";
    }else{
        SpriteButtons[0].spriteName = "UIH_Main_Button_N";
        SpriteButtons[1].spriteName = "UIH_Main_Button_N";
        SpriteButtons[2].spriteName = "UIH_Main_Button_N";
        SpriteButtons[3].spriteName = "UIH_Main_Button_N";
        SpriteButtons[4].spriteName = "UIH_Main_Button_N";
    }
}

var SpriteButtons : UISprite[];
function ShowXunLianButton(lv : int){
	for(var i=0; i<5; i++){
		SpriteButtons[i].spriteName = "UIH_Main_Button_O";
	}
	if(lv<0){
	}else
	if(lv<0){
		SpriteButtons[0].spriteName = "UIH_Main_Button_N";
		SpriteButtons[1].spriteName = "UIH_Main_Button_N";
	}else
	if(lv<2){
		SpriteButtons[0].spriteName = "UIH_Main_Button_N";
		SpriteButtons[1].spriteName = "UIH_Main_Button_N";
		SpriteButtons[2].spriteName = "UIH_Main_Button_N";
	}else
	if(lv<4){
		SpriteButtons[0].spriteName = "UIH_Main_Button_N";
		SpriteButtons[1].spriteName = "UIH_Main_Button_N";
		SpriteButtons[2].spriteName = "UIH_Main_Button_N";
		SpriteButtons[3].spriteName = "UIH_Main_Button_N";
	}else{
		SpriteButtons[0].spriteName = "UIH_Main_Button_N";
		SpriteButtons[1].spriteName = "UIH_Main_Button_N";
		SpriteButtons[2].spriteName = "UIH_Main_Button_N";
		SpriteButtons[3].spriteName = "UIH_Main_Button_N";
		SpriteButtons[4].spriteName = "UIH_Main_Button_N";
	}
}

function SetTextAsColor(lab : UILabel , num : int , str : String){
	var useStr : String = "";
	if(num < 0){
		useStr += "[ff0000]";
	}else
	if(num == 0){
		useStr += "[ffff00]";
	}else{
		useStr += "[00ff00]";
	}	
	lab.text = useStr + str+"\n" + num;
}

var useObj : GameObject[];
function ShowObj(bool : boolean){
	for(var i=0; i<useObj.length; i++){
		useObj[i].active = bool;
	}
}

//根据角色等级获得最大训练点数
var maxPoint : int = 0;
function GetMaxPoint(){
	maxPoint = 0;
	for(var i=0; i <=parseInt(ps.Level); i++){
		maxPoint += parseInt(i*2) + 12;
	}
}
//平民级
function Xun1(){
	if(SpriteButtons[0].spriteName == "UIH_Main_Button_N" && ps.NonPoint >= 12){
		DoXunLian(1);
	}else{
		if(JiaoCheng.JiaoChengID == 3 && JiaoCheng.JiaoChengID == 3){
			JiaoCheng.GiveUpJiaoCheng("62");
			yield;
			yield;
			yield;
			yield;
			yield;
			yield;
			uicl.show0();
		}
		if(SpriteButtons[0].spriteName != "UIH_Main_Button_N"){
			AllManage.tsStatic.Show("tips055");		
		}else{
			AllManage.tsStatic.Show("tips059");				
		}
	}
}
//士兵级
function Xun2(){
//	if(SpriteButtons[1].spriteName == "UIH_Main_Button_N" && ps.NonPoint >= 16){
		DoXunLian(2);
//		}else{
//		if(JiaoCheng.JiaoChengID == 3 && JiaoCheng.JiaoChengID == 3){
//			JiaoCheng.GiveUpJiaoCheng("62");
//			yield;
//			yield;
//			yield;
//			yield;
//			yield;
//			yield;
//			uicl.show0();
//		}
//		if(SpriteButtons[1].spriteName != "UIH_Main_Button_N"){
//			AllManage.tsStatic.Show("tips055");		
//		}else{
//			AllManage.tsStatic.Show("tips059");				
//		}
//	}
}
//勇士级
function Xun3(){
	if(SpriteButtons[2].spriteName == "UIH_Main_Button_N" && ps.NonPoint >= 12){
		DoXunLian(3);
	}else{
		if(JiaoCheng.JiaoChengID == 3 && JiaoCheng.JiaoChengID == 3){
			JiaoCheng.GiveUpJiaoCheng("62");
			yield;
			yield;
			yield;
			yield;
			yield;
			yield;
			uicl.show0();
		}
		if(SpriteButtons[2].spriteName != "UIH_Main_Button_N"){
			AllManage.tsStatic.Show("tips055");		
		}else{
			AllManage.tsStatic.Show("tips059");				
		}
	}
}
//骑士级
function Xun4(){
	if(SpriteButtons[3].spriteName == "UIH_Main_Button_N" && ps.NonPoint >= 12){
		DoXunLian(4);
	}else{
		if(JiaoCheng.JiaoChengID == 3 && JiaoCheng.JiaoChengID == 3){
			JiaoCheng.GiveUpJiaoCheng("62");
			yield;
			yield;
			yield;
			yield;
			yield;
			yield;
			uicl.show0();
		}
		if(SpriteButtons[3].spriteName != "UIH_Main_Button_N"){
			AllManage.tsStatic.Show("tips055");		
		}else{
			AllManage.tsStatic.Show("tips059");				
		}
	}
}
//督军级
function Xun5(){
	if(SpriteButtons[4].spriteName == "UIH_Main_Button_N" && ps.NonPoint >= 24){
		DoXunLian(5);
	}else{
		if(JiaoCheng.JiaoChengID == 3 && JiaoCheng.JiaoChengID == 3){
			JiaoCheng.GiveUpJiaoCheng("62");
			yield;
			yield;
			yield;
			yield;
			yield;
			yield;
			uicl.show0();
		}
		if(SpriteButtons[4].spriteName != "UIH_Main_Button_N"){
			AllManage.tsStatic.Show("tips055");		
		}else{
			AllManage.tsStatic.Show("tips059");				
		}
	}
}

private var invcl : InventoryControl; 
var ObjSave : GameObject;
var ObjXunLian : GameObject;
function xSave(){
//	AllManage.tsStatic.RefreshBaffleOn();
	PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().TrainingSave(1));
//	ShowObj(false); 
//	ps.JianNonPoint(yuse);
//	ObjSave.SetActiveRecursively(false);
//	ObjSave.transform.localPosition.y = 1000;
//	ObjXunLian.transform.localPosition.y =230;
	    //	ps.SetXunLian(its , invcl.EquipStatus);

	InRoom.GetInRoomInstantiate().ShowTrainingInfo();//点保存后，金币显示要刷新
}

function xExit(){
//	AllManage.tsStatic.RefreshBaffleOn();
	PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().TrainingSave(0));
	;
//	ObjSave.SetActiveRecursively(false);
//	ObjSave.transform.localPosition.y = 1000;
//	ObjXunLian.transform.localPosition.y = 230;
//	ShowObj(false);
}

function returnSave(){
	//print("sdljfsdjlflsdjfljk  === zhe li le");
	ps.useSetXunLian();
	ObjSave.SetActiveRecursively(false);
	ObjSave.transform.localPosition.y = 2000;
	ObjXunLian.transform.localPosition.y = 290.8;
	ShowObj(false);

}

private var uicl : UIControl;
function XunClose(){ 
	uicl.CloseXunLian();
	xExit();
}  

//点击训练按钮，id是训练等级
var its : int[];
var yuse : int;
private var JiaoCheng : TaskJiaoChengControl;
var otherYuSe : int = 0;
function DoXunLian(id : int){
	if(ps == null && PlayerStatus.MainCharacter){	
		if(PlayerStatus.MainCharacter != null){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
	}
	if(ps == null && PlayerStatus.MainCharacter){
		return;
	}
	var useBlood : int = 0;
	var useMoney : int = 0;
	var lent : int;
	switch(id){
		case 1:
				yuse = 6;
			otherYuSe = 6;
			useMoney = Mathf.Clamp(((need-ps.NonPoint)/6)*40+500,500,4800);
			lent = 0;
			break;
		case 2:
				yuse =16;
			otherYuSe = 16;
			useBlood = 2;
			lent = 1;
			break;
		case 3:
				yuse = 12;
	 
			otherYuSe = 12;
			useBlood = 12;
			lent = 1;
			break;
		case 4:
				yuse = 24;
	
			otherYuSe = 24;
			useBlood = 20;
			lent = 1;
			break;
		case 5:
			otherYuSe = 48;
			yuse = 48;			
			lent = 2;
			useBlood = 40;
			break;
	}
	
	DoFXing();
//	AllManage.tsStatic.RefreshBaffleOn();
	if(AllManage.jiaochengCLStatic.JiaoChengID == 3 && AllManage.jiaochengCLStatic.step == 1 && AllManage.InvclStatic.TutorialsDetectionAsID("31")){
		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().Training(id , 0 , 0));	
	}else{
		if(id==2)
		{
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().QuickTraining());
		}else{
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().Training(id , useMoney * (-1) , useBlood * (-1)));
		}
	}
	return;
	if(ps.NonPoint >= yuse && parseInt(ps.Bloodstone) >= useBlood && parseInt(ps.Money) >= useMoney  || (AllManage.jiaochengCLStatic.JiaoChengID == 3 && AllManage.jiaochengCLStatic.step == 1 && AllManage.InvclStatic.TutorialsDetectionAsID("31"))){
		switch(id){
			case 1:
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.Training1).ToString());
				break;
			case 2:
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.Training2).ToString());
				break;
			case 3:
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.Training3).ToString());
				break;
			case 4:
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.Training4).ToString());
				break;
			case 5:
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.Training5).ToString());
				break;
		}
		
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.Training , (id - 1).ToString());
		ShowObj(true); 
		if(! (AllManage.jiaochengCLStatic.JiaoChengID == 3 && AllManage.jiaochengCLStatic.step == 1 && AllManage.InvclStatic.TutorialsDetectionAsID("31")))
		ps.UseMoney(useMoney  , useBlood);
        
		its = new Array(5);
		its = getAddSX(its , ps.ProID , lent , otherYuSe , yuse); 
		SetTextAsColor(MidText1 , its[0] , "耐力");
		SetTextAsColor(MidText2 , its[1] , "力量");
		SetTextAsColor(MidText3 , its[2] , "敏捷");
		SetTextAsColor(MidText4 , its[3] , "智力");
		SetTextAsColor(MidText5 , its[4] , "专注"); 
		ObjSave.SetActiveRecursively(true);
		ObjSave.transform.localPosition.y = 577;
		ObjXunLian.transform.localPosition.y = 2000;
		InventoryControl.yt.Rows[0]["AimTrain"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimTrain"].YuanColumnText) + 1).ToString();
	}else{
		if(JiaoCheng.JiaoChengID == 3 && JiaoCheng.JiaoChengID == 3){
			JiaoCheng.GiveUpJiaoCheng("62");
			yield;
			yield;
			yield;
			yield;
			yield;
			yield;
			uicl.show0();
		}
		if(parseInt(ps.Bloodstone) < useBlood){
		    //AllManage.tsStatic.Show("tips060");		
		    BtnGameManagerBack.my.SwitchToStore();
		}else
		if(parseInt(ps.Money) < useMoney){
			AllManage.tsStatic.Show("tips061");				
		}else
		if(ps.NonPoint < yuse){
			AllManage.tsStatic.Show("tips059");	
		}
	}
}

function returnSetTextAsColor(it : int[]){
//	its = it;
	ps.NonPoint = ps.GetBDInfoInt("NonPoint" , 0);
		ObjSave.SetActiveRecursively(true);
		ObjSave.transform.localPosition.y = 577;
		ObjXunLian.transform.localPosition.y = 2000;
	ShowObj(true);
	SetTextAsColor(MidText1 , it[0] , "耐力");
	SetTextAsColor(MidText2 , it[1] , "力量");
	SetTextAsColor(MidText3 , it[2] , "敏捷");
	SetTextAsColor(MidText4 , it[3] , "智力");
	SetTextAsColor(MidText5 , it[4] , "专注"); 	
}

//var ts : TiShi;
//返回随机分配5个属性点的数组
function getAddSX(it : int[] , ProID : int , lent : int , point : int , useP : int) : int[]{
	if(ps == null && PlayerStatus.MainCharacter){	
		if(PlayerStatus.MainCharacter != null){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
	}
//	//print(point + " == " + useP);
	switch(ProID){
		case 1 : 
			if(lent == 0){
				it[0] = Random.Range(Random.Range(0 - point/2 , point/2),point/2);
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) > maxPoint) it[0] = Mathf.Clamp(it[0] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) < parseInt(ps.Level)) it[0] = Mathf.Clamp(it[0] , 0 , 999); 			
				it[1] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[0]);
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) > maxPoint) it[1] = Mathf.Clamp(it[1] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) < parseInt(ps.Level)) it[1] = Mathf.Clamp(it[1] , 0 , 999); 			
				it[2] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[1]);
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) > maxPoint) it[2] = Mathf.Clamp(it[2] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) < parseInt(ps.Level)) it[2] = Mathf.Clamp(it[2] , 0 , 999); 			
				it[3] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[2]);
					if(parseInt(InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText) > maxPoint) it[3] = Mathf.Clamp(it[3] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText) < parseInt(ps.Level)) it[3] = Mathf.Clamp(it[3] , 0 , 999); 			
				it[4] = useP - it[0] - it[1] - it[2] - it[3];		
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) > maxPoint + 1000) it[4] = Mathf.Clamp(it[4] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) < parseInt(ps.Level) + 1000) it[4] = Mathf.Clamp(it[4] , 0 , 999); 			
			}else
			if(lent == 1){
				it[0] = Random.Range(Random.Range(0 - point/2 , point/2),point/2);
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) > maxPoint) it[0] = Mathf.Clamp(it[0] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) < parseInt(ps.Level)) it[0] = Mathf.Clamp(it[0] , 0 , 999); 			
				it[1] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[0]);
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) > maxPoint) it[1] = Mathf.Clamp(it[1] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) < parseInt(ps.Level)) it[1] = Mathf.Clamp(it[1] , 0 , 999); 			
				it[2] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[1]);
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) > maxPoint) it[2] = Mathf.Clamp(it[2] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) < parseInt(ps.Level)) it[2] = Mathf.Clamp(it[2] , 0 , 999); 			
				it[4] = useP - it[0] - it[1] - it[2];		
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) > maxPoint + 1000) it[4] = Mathf.Clamp(it[4] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) < parseInt(ps.Level) + 1000) it[4] = Mathf.Clamp(it[4] , 0 , 999); 			
			
			}else
			if(lent == 2){
				it[0] = Random.Range(Random.Range(0 - point/2 , point/2),point/2);
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) > maxPoint) it[0] = Mathf.Clamp(it[0] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) < parseInt(ps.Level)) it[0] = Mathf.Clamp(it[0] , 0 , 999); 			
				it[1] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[0]);
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) > maxPoint) it[1] = Mathf.Clamp(it[1] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) < parseInt(ps.Level)) it[1] = Mathf.Clamp(it[1] , 0 , 999); 			
				it[4] = useP - it[0] - it[1];					
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) > maxPoint + 1000) it[4] = Mathf.Clamp(it[4] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) < parseInt(ps.Level) + 1000) it[4] = Mathf.Clamp(it[4] , 0 , 999); 			
			}
			break; 
			
		case 2 : 
			if(lent == 0){
				it[0] = Random.Range(Random.Range(0 - point/2 , point/2),point/2);
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) > maxPoint) it[0] = Mathf.Clamp(it[0] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) < parseInt(ps.Level)) it[0] = Mathf.Clamp(it[0] , 0 , 999); 			
				it[1] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[0]);
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) > maxPoint) it[1] = Mathf.Clamp(it[1] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) < parseInt(ps.Level)) it[1] = Mathf.Clamp(it[1] , 0 , 999); 			
				it[2] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[1]);
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) > maxPoint) it[2] = Mathf.Clamp(it[2] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) < parseInt(ps.Level)) it[2] = Mathf.Clamp(it[2] , 0 , 999); 			
				it[3] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[2]);
					if(parseInt(InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText) > maxPoint) it[3] = Mathf.Clamp(it[3] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText) < parseInt(ps.Level)) it[3] = Mathf.Clamp(it[3] , 0 , 999); 			
				it[4] = useP - it[0] - it[1] - it[2] - it[3];		
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) > maxPoint + 1000) it[4] = Mathf.Clamp(it[4] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) < parseInt(ps.Level) + 1000) it[4] = Mathf.Clamp(it[4] , 0 , 999); 			
			}else
			if(lent == 1){
				it[0] = Random.Range(Random.Range(0 - point/2 , point/2),point/2);
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) > maxPoint) it[0] = Mathf.Clamp(it[0] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) < parseInt(ps.Level)) it[0] = Mathf.Clamp(it[0] , 0 , 999); 			
				it[1] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[0]);
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) > maxPoint) it[1] = Mathf.Clamp(it[1] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) < parseInt(ps.Level)) it[1] = Mathf.Clamp(it[1] , 0 , 999); 			
				it[2] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[1]);
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) > maxPoint) it[2] = Mathf.Clamp(it[2] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) < parseInt(ps.Level)) it[2] = Mathf.Clamp(it[2] , 0 , 999); 			
				it[4] = useP - it[0] - it[1] - it[2];		
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) > maxPoint + 1000) it[4] = Mathf.Clamp(it[4] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) < parseInt(ps.Level) + 1000) it[4] = Mathf.Clamp(it[4] , 0 , 999); 						
			}else
			if(lent == 2){
				it[0] = Random.Range(Random.Range(0 - point/2 , point/2),point/2);
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) > maxPoint) it[0] = Mathf.Clamp(it[0] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) < parseInt(ps.Level)) it[0] = Mathf.Clamp(it[0] , 0 , 999); 			
				it[2] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[1]);
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) > maxPoint) it[2] = Mathf.Clamp(it[2] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) < parseInt(ps.Level)) it[2] = Mathf.Clamp(it[2] , 0 , 999); 			
				it[4] = useP - it[0] - it[2];					
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) > maxPoint + 1000) it[4] = Mathf.Clamp(it[4] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) < parseInt(ps.Level) + 1000) it[4] = Mathf.Clamp(it[4] , 0 , 999); 			
			}
			break; 
			
		case 3 : 
			if(lent == 0){
				it[0] = Random.Range(Random.Range(0 - point/2 , point/2),point/2);
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) > maxPoint) it[0] = Mathf.Clamp(it[0] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) < parseInt(ps.Level)) it[0] = Mathf.Clamp(it[0] , 0 , 999); 			
				it[1] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[0]);
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) > maxPoint) it[1] = Mathf.Clamp(it[1] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Strength"].YuanColumnText) < parseInt(ps.Level)) it[1] = Mathf.Clamp(it[1] , 0 , 999); 			
				it[2] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[1]);
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) > maxPoint) it[2] = Mathf.Clamp(it[2] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) < parseInt(ps.Level)) it[2] = Mathf.Clamp(it[2] , 0 , 999); 			
				it[3] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[2]);
					if(parseInt(InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText) > maxPoint) it[3] = Mathf.Clamp(it[3] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText) < parseInt(ps.Level)) it[3] = Mathf.Clamp(it[3] , 0 , 999); 			
				it[4] = useP - it[0] - it[1] - it[2] - it[3];		
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) > maxPoint + 1000) it[4] = Mathf.Clamp(it[4] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) < parseInt(ps.Level) + 1000) it[4] = Mathf.Clamp(it[4] , 0 , 999); 			
			}else
			if(lent == 1){
				it[0] = Random.Range(Random.Range(0 - point/2 , point/2),point/2);
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) > maxPoint) it[0] = Mathf.Clamp(it[0] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) < parseInt(ps.Level)) it[0] = Mathf.Clamp(it[0] , 0 , 999); 			
				it[2] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[1]);
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) > maxPoint) it[2] = Mathf.Clamp(it[2] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Agility"].YuanColumnText) < parseInt(ps.Level)) it[2] = Mathf.Clamp(it[2] , 0 , 999); 			
				it[3] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[2]);
					if(parseInt(InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText) > maxPoint) it[3] = Mathf.Clamp(it[3] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText) < parseInt(ps.Level)) it[3] = Mathf.Clamp(it[3] , 0 , 999); 			
				it[4] = useP - it[0] - it[3] - it[2];		
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) > maxPoint + 1000) it[4] = Mathf.Clamp(it[4] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) < parseInt(ps.Level) + 1000) it[4] = Mathf.Clamp(it[4] , 0 , 999); 			
			
			}else
			if(lent == 2){
				it[0] = Random.Range(Random.Range(0 - point/2 , point/2),point/2);
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) > maxPoint) it[0] = Mathf.Clamp(it[0] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Stamina"].YuanColumnText) < parseInt(ps.Level)) it[0] = Mathf.Clamp(it[0] , 0 , 999); 			
				it[3] = Random.Range(Random.Range(0 - point/2 , point/2),point/2-it[2]);
					if(parseInt(InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText) > maxPoint) it[3] = Mathf.Clamp(it[3] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Intellect"].YuanColumnText) < parseInt(ps.Level)) it[3] = Mathf.Clamp(it[3] , 0 , 999); 			
				it[4] = useP - it[0] - it[3];					
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) > maxPoint + 1000) it[4] = Mathf.Clamp(it[4] , -999 , 0); 
					if(parseInt(InventoryControl.yt.Rows[0]["Focus"].YuanColumnText) < parseInt(ps.Level) + 1000) it[4] = Mathf.Clamp(it[4] , 0 , 999); 			
			}
			break;
	}
	return its;
}

//var alluipc : UIAllPanelControl;
function CloseXunLian(){
//	if( !alluipc){
//		alluipc = FindObjectOfType(UIAllPanelControl);
//	}
	AllManage.UIALLPCStatic.show0();
}

var ObjHelp : UIPanel;
function OpenHelp(){
		ObjHelp.enabled=true;
}
function CloseHelp(){
		ObjHelp.enabled=false;
}
