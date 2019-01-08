#pragma strict


var isOn : boolean = false;
var onButton : UISprite;
var AllButtons : GameObject;
var jiaTween : TweenRotation;
var TweenGround1 : TweenPosition;
var TweenGround2 : TweenPosition;
var shouUI : UIPanel;
private var bb = true;

//function OnDisable(){
//	gameObject.active = true;
//}

function buttonsOn(){
    bb = false;
	isOn = true;
	AllButtons.SetActiveRecursively(false);
	yield;
	AllButtons.SetActiveRecursively(true);
	shouUI.widgetsAreStatic = false;
	jiaTween.Play(true);
	if(TweenGround1)
		TweenGround1.Play(true);
	if(TweenGround2)
		TweenGround2.Play(true);
	yield WaitForSeconds(0.55);
	if(shouUI)
		shouUI.widgetsAreStatic = true;	
	bb = true;
}

var uicl : UIControl;
function buttonsOff(){
   bb = false;
	uicl.startButtonTime += 1;
	isOn = false;
	shouUI.widgetsAreStatic = false;
	AllButtons.SetActiveRecursively(false);	
	jiaTween.Play(false);
	if(TweenGround1)
		TweenGround1.Play(false);
	if(TweenGround2)
		TweenGround2.Play(false);
	yield WaitForSeconds(0.55);
	if(shouUI)
		shouUI.widgetsAreStatic = true;	
	bb = true;
}

function buttons(){
if(bb){
	if(!isOn){
		buttonsOn();
	}else
		buttonsOff();
	}
}
var UIRide : GameObject;
var UIFilledSpriteRideGuang : UIFilledSprite;
var TiaoRide : GameObject;
var LabelRide : UILabel;
var RideIng : boolean = false;
var RideTime : int = 5;
var waTime : float = 0;
private var ps : PlayerStatus;
private var ag : agentLocomotion;
function ridebb(){
			if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.PetSwitch) == "0"){
				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
				return;
			}
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	if(ag == null && PlayerStatus.MainCharacter ){
		ag = PlayerStatus.MainCharacter.gameObject.GetComponent(agentLocomotion);
	}
	if(ag.enabled)
		return;
	if(RideIng)
		return;
	if(ps.ridemod){
		alljoy.rideButton =true;
		return;
	}
	
	var canRideBool : boolean = false;
	if(UIControl.mapType == MapType.zhucheng){
		canRideBool = true;
	}else{
		if(InventoryControl.yt.Rows[0]["SelectMounts"].YuanColumnText != "" && parseInt(InventoryControl.yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(4,1)) > 3)
			canRideBool = true;
	}
	
	if(!canRideBool){
		AllManage.tsStatic.Show("info1194");	
		return;
	}
	
//	LabelRide.text = nowStone.isTaskName;
	PlayerStatus.MainCharacter.SendMessage("DaDuan",SendMessageOptions.DontRequireReceiver);
	PlayerStatus.MainCharacter.SendMessage("Skill", 16);
	TiaoRide.gameObject.SetActiveRecursively(true);
	RideIng = true;
	waTime = 0;
	var timeStone : int;
	var useFloat : float;
	useFloat = RideTime;
//	//print(RideIng + " == " + waTime + " == " + RideTime);
	
	while(RideIng && waTime < RideTime){
//		//print(RideIng + " == " + waTime + " == " + RideTime);
		waTime += Time.deltaTime;
		UIFilledSpriteRideGuang.fillAmount = waTime / useFloat;
		if(waTime >= RideTime){
			alljoy.rideButton =true;
			TiaoRide.gameObject.SetActiveRecursively(false);
			RideIng = false;
			waTime = 0;
		}
		yield;
	}
}

function DaDuanRide(){
	if(RideIng){
		TiaoRide.gameObject.SetActiveRecursively(false);
		AllManage.tsStatic.Show("tips003");	
		waTime = 0;
		RideIng = false;
		UIFilledSpriteRideGuang.fillAmount = 0;	
	}
}

function DoJump(){
	alljoy.jumpButton = true;
}