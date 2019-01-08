#pragma strict
enum PVPType {
	holdOn = 1,
	inTeam = 2,
	isReady = 3
}

function Awake(){
	uicl = AllManage.UICLStatic;
	AllManage.UICLStatic.pvpcl = this;
	AllManage.btnGMBStatic.PVPCL = this.gameObject;	
	yuanM = AllManage.yuanBMStatic;
//	ts = AllManage.tsStatic;
	quan = AllManage.pvpQuanStatic;
}

//function OnDisable(){
//	try{
//		gameObject.active = true;	
//	}catch(e){
//	
//	}
//}

var LabelOffLinePlayer : UILabel;
var LabelDtimes : UILabel;
var LabelBtimes : UILabel;
function Start () {
	SetDuelType(1);
	SetBattlefieldType(1);
//	yield WaitForSeconds(1);
//	//print(InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText + " == 8 times");
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages084");
			AllManage.AllMge.Keys.Add(InventoryControl.yt.Rows[0]["PVPTimes"].YuanColumnText+" /5.");
			AllManage.AllMge.SetLabelLanguageAsID(LabelDtimes);
//	LabelDtimes.text = "今日还可进入次数："+ InventoryControl.yt.Rows[0]["PVPTimes"].YuanColumnText+" /5.";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages084");
			AllManage.AllMge.Keys.Add(InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText+" /5.");
			AllManage.AllMge.SetLabelLanguageAsID(LabelBtimes);
			
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	if(ps){
		LabelOffLinePlayer.text = AllManage.AllMge.Loc.Get("messages084") + "" + (10 - ps.pvpNum);
	}
//	LabelBtimes.text = "今日还可进入次数："+ InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText+" /5.";
}

var ptime : int;
var quan : UISprite;
function Update () {
	if(Time.time > ptime && quan){
		ptime = Time.time + 1;
		if(duelType == 3 || battlefieldType == 3){
			quan.enabled = true;
		}else{
			quan.enabled = false;			
		}
	}
}

var duelType : PVPType;
var LableDuel1 : UILabel;
var LableDuel2 : UILabel;
var ButtonTuiDuel : Transform;
function SetDuelType(i : int){
	duelType = i;
	switch(duelType){
		case PVPType.holdOn :
			ButtonTuiDuel.localPosition.y = 3000;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages085");
			AllManage.AllMge.SetLabelLanguageAsID(LableDuel1);
//			LableDuel1.text = "未加入";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages086");
			AllManage.AllMge.SetLabelLanguageAsID(LableDuel2);
//			LableDuel2.text = "排队";
			ShowLblState.showLblState.SetLabelTxt("", false);
			break;
		case PVPType.inTeam :
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages087");
			AllManage.AllMge.SetLabelLanguageAsID(LableDuel1);
//			LableDuel1.text = "排队中";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages088");
			AllManage.AllMge.SetLabelLanguageAsID(LableDuel2);
//			LableDuel2.text = "退队";
			ButtonTuiDuel.localPosition.y = 3000;
			ShowLblState.showLblState.SetLabelTxt(StaticLoc.Loc.Get("messages087"), true);
			break;
		case PVPType.isReady :
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages089");
			AllManage.AllMge.SetLabelLanguageAsID(LableDuel1);
//			LableDuel1.text = "准备就绪";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages090");
			AllManage.AllMge.SetLabelLanguageAsID(LableDuel2);
//			LableDuel2.text = "进入";
			ButtonTuiDuel.localPosition.y = -140;
			AllManage.qrStatic.ShowQueRen(gameObject , "DuelButton" , "TuiDuel" , "info766");
			ShowLblState.showLblState.SetLabelTxt("", false);
			break;
	}
}

function TuiDuel(){
	InRoom.GetInRoomInstantiate().PVPTeamDissolve();
}

function DuelButton(){
	switch(duelType){
		case PVPType.holdOn :
			if(parseInt(InventoryControl.yt.Rows[0]["PVPTimes"].YuanColumnText) < 50){
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.QueuePVP1).ToString());
			    //				InRoom.GetInRoomInstantiate().PVPTeamCreate("",1);

				PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().PVPTeamCreate("4"));
			}else{
				AllManage.qrStatic.ShowBuyQueRen(gameObject ,"YesPVPTimes" , "" , "messages012");	
//				ts.Show("已超出最大排队次数。");								
			}
			break;
		case PVPType.inTeam :
//			InRoom.GetInRoomInstantiate().PVPTeamDissolve();
		    PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().PVPCancel(CommonDefine.PVP_ONE));
			// SetDuelType(1);
			break;
		case PVPType.isReady :
			InRoom.GetInRoomInstantiate().SendLegionPVPInfo(1);	
			break;
	}
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var battlefieldType : PVPType;
var LableBattlefield1 : UILabel;
var LableBattlefield2 : UILabel;
var ButtonTuiBattlefield : Transform;
function SetBattlefieldType(i : int){
	battlefieldType = i;
	switch(battlefieldType){
		case PVPType.holdOn :
			ButtonTuiBattlefield.localPosition.y = 3000;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages085");
			AllManage.AllMge.SetLabelLanguageAsID(LableBattlefield1);
//			LableBattlefield1.text = "未加入";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages086");
			AllManage.AllMge.SetLabelLanguageAsID(LableBattlefield2);
//			LableBattlefield2.text = "排队";
			break;
		case PVPType.inTeam :
			ButtonTuiBattlefield.localPosition.y = 3000;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages087");
			AllManage.AllMge.SetLabelLanguageAsID(LableBattlefield1);
//			LableBattlefield1.text = "排队中";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages088");
			AllManage.AllMge.SetLabelLanguageAsID(LableBattlefield2);
//			LableBattlefield2.text = "退队";
			break;
		case PVPType.isReady :
			ButtonTuiBattlefield.localPosition.y = -140;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages089");
			AllManage.AllMge.SetLabelLanguageAsID(LableBattlefield1);
//			LableBattlefield1.text = "准备就绪";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages090");
			AllManage.AllMge.SetLabelLanguageAsID(LableBattlefield2);
//			LableBattlefield2.text = "进入";
			break;
	}
}

function TuiBattlefield(){
	InRoom.GetInRoomInstantiate().LegionOneRemove();
}

private var uicl : UIControl;
var yuanM : BtnGameManager;
private var ps : PlayerStatus;
//var ts : TiShi;
static var PVPTaskNanDu : int = 1;
function BattlefieldButton(){
	if(parseInt(AllManage.psStatic.Level) < 30){
		AllManage.tsStatic.Show("tips038");
		return;
	}
 	switch(battlefieldType){
		case PVPType.holdOn :
			if(parseInt(InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText) < 5){
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.QueuePVP8).ToString());
				yuanM.BtnRandomAddLegionOne();			
			}else{
				AllManage.qrStatic.ShowBuyQueRen(gameObject ,"YesPVP8Times" , "" , "messages012");	
			}
//			InRoom.GetInRoomInstantiate().LegionAddQueue();
			break;
		case PVPType.inTeam :
			yuanM.BtnRemoveLegionOne();
//			InRoom.GetInRoomInstantiate().LegionOneRemove();
			break;
		case PVPType.isReady :
			uicl.PVPBattlefieldGo();
			break;
	}
}

function YesPVP8Times(){
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesPVP8Times , 5 , 0 , "" , gameObject , "realYesPVP8Times");
//	AllManage.AllMge.UseMoney(0 , 5 , UseMoneyType.YesPVP8Times , gameObject , "realYesPVP8Times");
//	if(ps.UseMoney(0,5)){
//	}
}

function realYesPVP8Times(){
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.RefreshPVP8Num).ToString());
		InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText = "0";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages084");
			AllManage.AllMge.Keys.Add(InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText+" /5.");
			AllManage.AllMge.SetLabelLanguageAsID(LabelBtimes);
//		LabelBtimes.text = "今日还可进入次数："+ InventoryControl.yt.Rows[0]["PVP8Times"].YuanColumnText+" /5.";
}

function YesPVPTimes(){
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesPVPTimes , 5 , 0 , "" , gameObject , "realYesPVPTimes");
//	AllManage.AllMge.UseMoney(0 , 5 , UseMoneyType.YesPVPTimes , gameObject , "realYesPVPTimes");
//	if(ps.UseMoney(0,5)){
//	}
}

function realYesPVPTimes(){
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.RefreshPVP1Num).ToString());
		InventoryControl.yt.Rows[0]["PVPTimes"].YuanColumnText = "0";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages084");
			AllManage.AllMge.Keys.Add(InventoryControl.yt.Rows[0]["PVPTimes"].YuanColumnText+" /5.");
			AllManage.AllMge.SetLabelLanguageAsID(LabelDtimes);
//		LabelDtimes.text = "今日还可进入次数："+ InventoryControl.yt.Rows[0]["PVPTimes"].YuanColumnText+" /5.";
}

function TuiChuZhanChang(){
	if(ArenaControl.areType == ArenaType.zhanchang){
		yuanM.BtnRemoveLegionOne();	
	}
}

function show0(){
	AllManage.UIALLPCStatic.show0();
}

var parDuel : Transform;
var parBattlefield : Transform;
var parArena : Transform;
function SetParent(par1 : Transform , par2 : Transform , par3 : Transform){
	parDuel.parent = par1;
	parDuel.localScale = Vector3.one;
	parDuel.localPosition.y = 0;
	
	parBattlefield.parent = par2;
	parBattlefield.localScale = Vector3.one;
	parBattlefield.localPosition.y = 0;
	
	parArena.parent = par3;
	parArena.localScale = Vector3.one;
	parArena.localPosition.y = 0;
}

function OnDestroy () {
	if(parDuel)
	Destroy(parDuel.gameObject);
	if(parBattlefield)
	Destroy(parBattlefield.gameObject);
	if(parArena)
	Destroy(parArena.gameObject);
}
function ArenaButton2v2(){
	if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.PVP2Switch) == "0"){
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
		return;
	}
	AllManage.UICLStatic.PaiDui2v2();
}

function ArenaButton4v4(){
	if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.PVP4Switch) == "0"){
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
		return;
	}
	AllManage.UICLStatic.PaiDui4v4();
}

function OffLinePlayerButton(){
	if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.OfflinePlayerSwitch) == "0"){
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
		return;
	}
	
	if(! AllManage.UICLStatic.canOpenViewAsTaskID("527")){
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info817"));
		return;
	}
	
	AllManage.UICLStatic.OtherShowOffLinePlayer();	
}

