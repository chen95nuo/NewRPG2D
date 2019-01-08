#pragma strict
import System.Collections.Generic;
var playerInventory : PersonEquipment[];
private var photonView : PhotonView;

private var uicl : UIControl;
//var ts : TiShi;
function Awake(){
//	uiallPC = AllManage.UIALLPCStatic;
//	UIALLcl = AllManage.UIALLPCStatic;
	btns = AllManage.yuanBMStatic.gameObject;
	uicl = AllManage.UICLStatic;
	uicl.tween = tween;
//	QR = AllManage.qrStatic;
//	ts = AllManage.tsStatic;
//	BtnGameManaB = AllManage.btnGMBStatic;
//	BFI = AllManage.btnInfoStatic;
	AllManage.btnGMBStatic.opc = gameObject;
//	photonView = GetComponent(PhotonView);
}
var TransBlood1 : Transform;
var TransBlood2 : Transform;
function Start(){
	OtherBottons.transform.parent = null;
	if(InRoom.GetInRoomInstantiate ().GetBloodTran()!=1){
		TransBlood1.localPosition.y = 3000;
		TransBlood2.localPosition.y = 3000;
	}
}
function OnDisable(){
	try{
		gameObject.active = true;	
	}catch(e){
	
	}
//	OpenMe();
}

function OpenMe(){
	yield;
}

var tween : TweenPosition;
var BagIT :  BagItem[]; 
//var uiallPC : UIAllPanelControl;
var TagetName : String;
var TagetID : String;
var TagetInstanceID : int = -1;
var TagetDuelState : int = -1;
var TagetPro : String;
var TargetLevel : String;
var TargetGuild : String;
//var UIALLcl : UIAllPanelControl;
function SetNewPlayer(pes : PersonEquipment[]){
    //	//print("dian cha kan wan jia");

    for(var j=0;j<BagIT.length;j++)
    {
        BagIT[j].invClear();
    }

	AllManage.UIALLPCStatic.show25();
	tween.Play(true);
	AllManage.UIALLPCStatic.show25();
	var i : int = 0;
	for(i=0; i<pes.length; i++){
		playerInventory[i] = pes[i];
		if(playerInventory[i].inv != null){
			if(playerInventory[i].inv.itemID != ""){
				BagIT[i].invClear();
				BagIT[i].SetInv(playerInventory[i].inv);		
			}
		}else{
			BagIT[i].invClear();
		}
	}
}

var LabelName : UILabel;
var LabelName2 : UILabel;
var LabelLevel : UILabel;
var LabelProfession : UILabel;
var labelGuild : UILabel;
var SpritePlayer : UISprite;
var labelZhanli : UILabel;
var labelTeli : UILabel;
var labelHP : UILabel;
var labelMP : UILabel;
var labelPVP : UILabel;
var SpriteDuelState : UISprite;
function ShowRemotePlayer(name : String , level : String , pro : String , playerid : String, guildName : String , force : String , Tile : String,Hp : String,Mp : String,Pvp : int , instanceid : int , DuelState : int){
	TagetDuelState = DuelState;
	TagetInstanceID = instanceid;
	TagetName = name;
	TagetID = playerid;
	TagetPro = pro;
	TargetGuild = guildName;
	labelGuild.text = guildName;
	labelZhanli.text = force;
	labelTeli.text = Tile;
	labelHP.text = Hp;
	labelMP.text = Mp;
	labelPVP.text = Pvp.ToString();
	
	uicl.TagetName = name;
	uicl.TagetID = playerid;
	uicl.TagetPro = pro;
	LabelName.text = name;
	LabelName2.text = name;	
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages062");
			AllManage.AllMge.Keys.Add(level + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelLevel);
//	LabelLevel.text = "等级 : " + level;
//	//print(pro + " == pro");
//	if(TagetDuelState != 0 || UIControl.mapType != MapType.yewai){
//		SpriteDuelState.spriteName = "UIB_Union_Player_A";
//	}else{
//		SpriteDuelState.spriteName = "UIH_Store_Button_A";
//	}
	switch(pro){
		case "1":
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages081");
			AllManage.AllMge.SetLabelLanguageAsID(LabelProfession);
//			LabelProfession.text = "职业 : 战士";
			SpritePlayer.spriteName = "head-zhanshi";
			break;
		case "2":
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages082");
			AllManage.AllMge.SetLabelLanguageAsID(LabelProfession);
//			LabelProfession.text = "职业 : 游侠";
			SpritePlayer.spriteName = "head-youxia";
			break;
		case "3":
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages083");
			AllManage.AllMge.SetLabelLanguageAsID(LabelProfession);
//			LabelProfession.text = "职业 : 法师";
			SpritePlayer.spriteName = "head-fashi";
			break;
	}
}

var stime : float;
function sendreq(){
//	Sendrequest("shs" , TagetName);
//	//print(TagetID);
	if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.TransactionSwitch) == "0"){
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
		return;
	}
	if(Time.time > stime){
		stime = Time.time + 5;
		if( parseInt(AllManage.psStatic.Level) > -1 ){
			if(parseInt(TargetLevel) > -1){			
				InRoom.GetInRoomInstantiate().TransactionRequest(TagetID);	
			}else{
				AllManage.tsStatic.Show("info551");
			}
		}else{
			AllManage.tsStatic.Show("tips036");
		}
	}else{
		AllManage.tsStatic.Show("tips037");
	}
//	photonView.RPC("Sendrequest",PhotonTargets.AllBuffered, InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText , TagetID , TagetName); 
}

var btns : GameObject;
function siliao(){ 
	var strs : String[];
	strs = new Array(2);
	strs[0] = TagetID;
	strs[1] = TagetName;
		AllManage.UICLStatic.OpenLiaoTianOne(strs);
	//PanelStatic.StaticBtnGameManagerBack.UICL.SendMessage ("OpenLiaoTianOne",strs,SendMessageOptions.DontRequireReceiver);
	 //btns.SendMessage("ShowOne",strs,SendMessageOptions.DontRequireReceiver);
}

var ttime : float = 0;
function TeamInviteAdd(){
	if(parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) >= 10){
		if(Time.time > ttime){
			ttime = Time.time + 5;
			InRoom.GetInRoomInstantiate().TeamInviteAdd(TagetID);
		}else{
			AllManage.tsStatic.Show("tips037");
		}	
	}else{
		AllManage.tsStatic.Show("info817");
	}
} 

function AddFirend(){
	InRoom.GetInRoomInstantiate().AddFirend(TagetName , "DarkSword2","PlayerInfo");
}

var atime : float = 0;
function AddJueDou(){
	if(UIControl.mapType == MapType.yewai){
		if(TagetDuelState == 0)
		ServerRequest.requestDuelInvite(parseInt(TagetInstanceID));
	}else
	if(UIControl.mapType == MapType.zhucheng){
		if(parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) >= 10){
			if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.InvitePVP1Switch) == "0"){
				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
				return;
			}
			if(Time.time > atime){
				atime = Time.time + 5;
				InRoom.GetInRoomInstantiate().PVP1Invite(TagetID);
			}else{
				AllManage.tsStatic.Show("tips037");
			}
		}else{
			AllManage.tsStatic.Show("info817");	
		}
	}
}

function AddChaKan(){
	if(tpw){
		tpw.chakanMe();
	}
}

function Update(){
	if(Input.GetButtonUp("Fire1")){	
		OtherBottons.SetActiveRecursively(false);
	}
}

private var tpw : ThirdPersonWeapon;
var OtherBottons : GameObject;
private var mi : float;
private var ma : float;
private var screenPos : Vector3 ;
private var uiPos : Vector3;
private var gensui : boolean = false;
private var GSpos : Vector3;
private var gaodu : float;
var par : Transform;
var xx : int;
private var thistransform : Transform;
private var sc = 0.0;
private var sz = 0.0;
var buttonDuel : GameObject;
function ClickMe(tw : ThirdPersonWeapon){
	tpw =tw;
	OtherBottons.SetActiveRecursively(true);

	sz =camera.main.transform.InverseTransformPoint(tw.gameObject.transform.position).z;
	if(sz>0){
		buttonDuel.active = true;
		screenPos = camera.main.WorldToScreenPoint (Vector3(tw.gameObject.transform.position.x,tw.gameObject.transform.position.y + 4,tw.gameObject.transform.position.z));
		uiPos = AllManage.uicamStatic.ScreenToWorldPoint(screenPos);
		uiPos.z = sz;
		OtherBottons.transform.position= uiPos;
		OtherBottons.transform.localPosition.z = 0;
		if(UIControl.mapType == MapType.yewai && GetPlayerDuelState(tw) != CommonDefine.DUEL_STATE_IDLE){
			buttonDuel.active = false;
		}
	}
}

function GetPlayerDuelState(tw : ThirdPersonWeapon){
	var pInfo : PlayerInfoInit;
	pInfo = tw.gameObject.GetComponent(PlayerInfoInit);
	if(pInfo != null){
		return pInfo.playerDuelState;
	}
	return 100;
}
  
//@RPC
//function Sendrequest(Me : String , To : String , name : String){
//	if(To == InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText){
//		TagetName = name;
//		JiaoYi(Me);
//	}
//} 

//var TP : TransactionParameters;
//function getPlayerParameters(parameters : TransactionParameters){
//	TP = parameters;
//	JiaoYi(TP.playerID);
//}
//
//var BFI : ButtonForwardInfo;
//function JiaoYi(from : String){
//	BFI.ShowInfoButton(gameObject , "isJiaoYi");
////	//print("jiao yi le" + gameObject.name);
//}
//
//var QR : QueRen;
//function isJiaoYi(){
//	QR.ShowQueRen(gameObject , "YesJiaoYi" , "NoJiaoYi" , TP.playerName +  " 对你发出交易申请");
//}
//
//var BtnGameManaB : BtnGameManagerBack;
//function YesJiaoYi(){
////	//print(TP.requstType + " === " + TP.playerID);
//	BtnGameManaB.SongSendTransactionRequest(yuan.YuanPhoton.ReturnCode.Yes , TP.requstType ,TP.playerID , "DarkSword2" , "PlayerInfo" );
//}
// 
//function NoJiaoYi(){
//	BtnGameManaB.SongSendTransactionRequest(yuan.YuanPhoton.ReturnCode.No , TP.requstType ,TP.playerID , "DarkSword2" , "PlayerInfo" );
//}
//
//var ts : TiShi;
//function beijujue(name : String){
//	ts.Show(name + " 拒绝于你交易");
//}
//
//var TransactionC : TransactionControl;
//function ShowJiaoYi(id : String){
//	tween.Play(false);
//	if(TP == null){
//		TP = new TransactionParameters();
//			TP.playerName = TagetName;
//			TP.playerID = TagetID;
////			//print("ssssss");
//	}
////	//print(TP.playerName);
//	TransactionC.ShowJiaoYi(TP.playerName , TP.playerID , id , TagetPro);
//}

var RcameraPic : rendercamerapic;
function ZHaoXiang(tagT : GameObject){
	RcameraPic.RenderC(tagT);
}

function show0(){
	tween.Play(false);
	yield WaitForSeconds(0.1);
	AllManage.UIALLPCStatic.show0();
}