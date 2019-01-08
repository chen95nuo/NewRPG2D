#pragma strict

var Player : Transform;
private var ps : PlayerStatus; 
//var ts : TiShi;
var parent : GameObject;
var parsentJingJi : GameObject;
var parsentZhanChang : GameObject;
var LabelHF1 : UILabel;
var LabelHF2 : UILabel;
var LabelChongSheng : UILabel; 
var SpriteChongSheng : UISprite;
private var dungcl : DungeonControl;
var SpriteButtons : UISprite[];
//private var SkillCForPlayer : SkillControl;
var fuhuo30:GameObject;

function Awake(){
	AllManage.pDeadStatic = this;
	parent.SetActiveRecursively(false);
	parsentJingJi.SetActiveRecursively(false);
	parsentZhanChang.SetActiveRecursively(false);
	AllManage.pDeadStatic = this;
	invcl = AllManage.InvclStatic;
	invcl.plyDead = this;
	dungcl = AllManage.dungclStatic;
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.GetComponent(PlayerStatus);
			Player = ps.transform;
		}
//	ts = AllManage.tsStatic;
//	SkillCForPlayer = AllManage.SkillCLStatic;
}

function Start(){
	if(Player == null){
		if(ps == null){
			if(PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.GetComponent(PlayerStatus);
				Player = ps.transform;
			}
		}else{
			Player = ps.transform;		
		}
	}
}


function Dead(level : int){
//try{
	
	if(Application.loadedLevelName == "Map721"){
		AllManage.UICLStatic.WowerFloorlose();
		return;
	}
	
	while(AllManage.UICLStatic.showDeadYield){
		yield;
	}
	
	if(UIControl.mapType == MapType.jingjichang){
		if(ArenaControl.areType == ArenaType.juedou){
			AllManage.areCLStatic.OverDuel(false);
		}
		if(! ArenaControl.ArenaIng){
			return;
		}
	}
	switch(Application.loadedLevelName){
		case  "Map200" : return; break;
		case  "Map311" : return; break;
		case  "Map321" : return; break;
		case  "Map331" : parsentJingJi.SetActiveRecursively(true); return;break;
//		case  "Map421" : parsentZhanChang.SetActiveRecursively(true); return;break;
//		case  "Map411" : parsentZhanChang.SetActiveRecursively(true); return;break;
	}
		if(ps == null && PlayerStatus.MainCharacter){
		ps = PlayerStatus.MainCharacter.GetComponent(PlayerStatus);
	}
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages091");
			//AllManage.AllMge.Keys.Add((parseInt(ps.Level) / 2 + 0.8)*2000 + "");
			AllManage.AllMge.Keys.Add((parseInt(ps.Level) / BtnGameManager.dicClientParms["FuHuobili"] + 0.8)*BtnGameManager.dicClientParms["FuHuoChangliang"] + "");
			AllManage.AllMge.Keys.Add("info335");
			AllManage.AllMge.SetLabelLanguageAsID(LabelHF1);
//	LabelHF1.text = "消耗" + (parseInt(ps.Level) / 5 + 2)+"血石";
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages091");
            //AllManage.AllMge.Keys.Add(100 + "");
			AllManage.AllMge.Keys.Add(BtnGameManager.dicClientParms["FuchouBlood"] + "");
			AllManage.AllMge.Keys.Add("messages053");
			AllManage.AllMge.SetLabelLanguageAsID(LabelHF2);
//	LabelHF2.text = "消耗" + (parseInt(ps.Level) / 5 + 2)*2+"血石";
//	Player.gameObject.SendMessage("Respawn" , SendMessageOptions.DontRequireReceiver);
	var num : int = 0;
	num =   invcl.NomDaojus("8861");
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages092");
			AllManage.AllMge.Keys.Add(num + "");
			AllManage.AllMge.Keys.Add("messages093");
			AllManage.AllMge.SetLabelLanguageAsID(LabelChongSheng);
//	LabelChongSheng.text = "剩余" + num + "次";
	if(num > 0){
		SpriteChongSheng.spriteName = "UIH_Main_Button_N";
	}else{
		SpriteChongSheng.spriteName = "UIH_Main_Button_O";
	}
//	//print("level == " + level);
	if(level < 10){
		SpriteButtons[0]. spriteName = "UIH_Main_Button_O";
		SpriteButtons[1]. spriteName = "UIH_Main_Button_O";
	}else
	if(level < 15){
		SpriteButtons[0]. spriteName = "UIH_Main_Button_N";
		SpriteButtons[1]. spriteName = "UIH_Main_Button_O";
	}else{
		SpriteButtons[0]. spriteName = "UIH_Main_Button_N";
		SpriteButtons[1]. spriteName = "UIH_Main_Button_N";
	}
	AllManage.UIALLPCStatic.show0();
	AllManage.UIALLPCStatic.allPanel[0].ap[0].gameObject.SetActiveRecursively(false);
	AllManage.UIALLPCStatic.SkillUI.gameObject.active = false;
	dungcl.OneDead();
			if(UIControl.mapType == MapType.jingjichang){
				if( AllManage.areCLStatic != null && !parent.active){
					AllManage.areCLStatic.AddBeKill();
				}
			}
	parent.SetActiveRecursively(true);
	fuhuo30.SetActiveRecursively(false);
		bool30 = true;
       Yield30FuHuo();
       AllManage.qrStatic.CloseAll();
//	}catch(e){
//	
//	}
}

var bool30 : boolean = false;;
var Label30 : UILabel;
var real30Time : int = 1;
var times = 0;
private var aa = true;
function Yield30FuHuo(){
    real30Time =10 + times*20;
	while(real30Time >0 && bool30){
		if(aa){	
		    aa = false;	
		    real30Time -= 1;
			Label30.text = real30Time.ToString();
			yield WaitForSeconds(1);
            aa = true;
		   }
          yield;
	}
	if(bool30){		
          fuhuo30.SetActiveRecursively(true);
	}
}

function LookOtherPlayer(){
	var players : PlayerStatus[];
	players = FindObjectsOfType(PlayerStatus);
	if(ps == null && PlayerStatus.MainCharacter){
		ps = Player.gameObject.GetComponent(PlayerStatus);
	}
	for(var p : PlayerStatus in players){
		if(p.PlayerID != ps.PlayerID){
			Camera.main.SendMessage("SetTarget" , p.gameObject.transform , SendMessageOptions.DontRequireReceiver);
		}
	}
	parsentJingJi.SetActiveRecursively(false);
}

function JingJiLiKai(){
	AllManage.areCLStatic.likai();
}

private var invcl : InventoryControl;
function fuhuoChongSheng(){
	if(invcl.NomDaojus("8861") > 0){
		AllManage.isDie = true;
		invcl.UseDaojuAsID("8861,01");	
	}
}

function doFuHuo(){
	bool30 = false;
	Player.gameObject.SendMessage("Respawn",2 , SendMessageOptions.DontRequireReceiver);
	parent.SetActiveRecursively(false);
		AllManage.UIALLPCStatic.show0();
	AllManage.UIALLPCStatic.SkillUI.gameObject.active = true;

}

var Fpoint : Transform;
function fuhuo1(){
	if(ps == null && PlayerStatus.MainCharacter){
		ps = Player.gameObject.GetComponent(PlayerStatus);
	}
	AllManage.isDie = true;
	        times +=1;
		bool30 = false;
		Player.gameObject.SendMessage("Respawn" ,1, SendMessageOptions.DontRequireReceiver);
		parent.SetActiveRecursively(false);
		AllManage.UIALLPCStatic.show0();
	AllManage.UIALLPCStatic.SkillUI.gameObject.active = true;

}

function fuhuo2(){ 
	if(SpriteButtons[0]. spriteName == "UIH_Main_Button_N"){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = Player.gameObject.GetComponent(PlayerStatus);
		}
		AllManage.isDie = true;
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.Resurrection2 , parseInt(ps.Level) , 0 , "" , gameObject , "realfuhuo2");
//		AllManage.AllMge.UseMoney((parseInt(ps.Level) / 5 + 2) * 800 , 0 , UseMoneyType.Resurrection2 , gameObject , "realfuhuo2");
//		if(ps.UseMoney((parseInt(ps.Level) / 5 + 2) * 800 , 0 )){
//		}else{
//			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages148")+ps.Level);
//		}
	}else{
		AllManage.tsStatic.Show("tips040");
	}
}

function realfuhuo2(){
			Player.gameObject.SendMessage("Respawn",2 , SendMessageOptions.DontRequireReceiver);
			parent.SetActiveRecursively(false);
			bool30 = false;
		AllManage.UIALLPCStatic.show0();
	AllManage.UIALLPCStatic.SkillUI.gameObject.active = true;
}

function fuhuo3(){

//	Player.position = Fpoint.position; 
	if(SpriteButtons[1]. spriteName == "UIH_Main_Button_N"){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = Player.gameObject.GetComponent(PlayerStatus);
		}
		AllManage.isDie = true;
		AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.Resurrection3 , parseInt(ps.Level) , 0 , "" , gameObject , "realfuhuo3");
//		AllManage.AllMge.UseMoney(0 , (parseInt(ps.Level) / 5 + 2)*2 , UseMoneyType.Resurrection3 , gameObject , "realfuhuo3");
		
//		if(ps.UseMoney(0 , (parseInt(ps.Level) / 5 + 2)*2 )){
//		}else{
//			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages148")+ps.Level*2);
//		}
	}else{
		AllManage.tsStatic.Show("tips041");
	}
}

function realfuhuo3(){
			Player.gameObject.SendMessage("Respawn" ,3, SendMessageOptions.DontRequireReceiver);
			parent.SetActiveRecursively(false);
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.BloodRevival , "");		
			bool30 = false;
		AllManage.UIALLPCStatic.show0();
	AllManage.UIALLPCStatic.SkillUI.gameObject.active = true;
}


function ButtonReturnTown(){
	if(UIControl.mapType == MapType.fuben){
		if(Application.loadedLevelName != "Map200"){
			//TD_info.setLeaveInstance(DungeonControl.MapName);
		}
	}

	AllManage.isDie = true;
			if(UIControl.mapType == MapType.jingjichang){
				InRoom.GetInRoomInstantiate().BattlefieldExit();
			}
		InRoom.GetInRoomInstantiate().RemoveTempTeam();
		Loading.Level = DungeonControl.ReLevel;
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		InRoom.GetInRoomInstantiate().PVP1InviteRemove();			
		InRoom.GetInRoomInstantiate().PVPTeamDissolve();
	AllManage.UICLStatic.RemoveAllTeam();
		if(UIControl.mapType == MapType.jingjichang){
			InRoom.GetInRoomInstantiate().LeaveRoom(PhotonNetwork.room.name);
			InRoom.GetInRoomInstantiate().ActivityPVPRemove();
		}
	alljoy.DontJump = true;
	yield;
			PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");


}

