#pragma strict

var ObjCreatTeam : GameObject;
var ObjFriendTeam : GameObject;

enum BtnEscTeam{
	Captain,
	Esc
}

var btnEsc : BtnEscTeam = BtnEscTeam.Esc;
var LabText : UILabel ;
var text : String ;
var IsClick : UIToggle;
var IsClickOne : UIToggle;
var ObjCreat : GameObject;
var ObjFriend : GameObject;
var ObjFind : GameObject;


function ShowTeamLayer(isTeam : boolean)
{
	if(isTeam)
	{
	
	ObjCreatTeam.transform.localPosition.y = -63;
	ObjFriendTeam.transform.localPosition.y = 3000;
	IsClickOne.value = true;
	ObjCreat.SetActiveRecursively(true);
	ObjFind.SetActiveRecursively(false);
	}else{
	
	ObjCreatTeam.transform.localPosition.y = 3000;
	ObjFriendTeam.transform.localPosition.y = -63;
	ObjFriend.SetActiveRecursively(true);
	ObjFind.SetActiveRecursively(false);
	IsClick.value = true;
	}
}
function Awake(){
	 AllManage.creatFriendTeam = this;
	InvokeRepeating("ShowText",0,1f);
}

function OnEnable(){
	}
function ShowText(){
		if(AllManage.UICLStatic.boolTeamHeadYes){
			ShowTeamExit(BtnEscTeam.Captain);
		}else{
			ShowTeamExit(BtnEscTeam.Esc);
		}
	}

function  ShowTeamExit(mBtnEsc : BtnEscTeam){
		switch(mBtnEsc){
		case BtnEscTeam.Captain:
			text = AllManage.AllMge.Loc.Get("buttons022");
			btnEsc = BtnEscTeam.Captain;
			break;
		case BtnEscTeam.Esc:
			text = AllManage.AllMge.Loc.Get("buttons019");
			btnEsc = BtnEscTeam.Esc;
			break;
		}
		LabText.text = text;
	}

	function TeamEscClick(){
		switch(btnEsc){
		case BtnEscTeam.Captain:
			//队长解散队伍的方法调用
			InRoom.GetInRoomInstantiate().RemoveTempTeam();
			ShowTeamLayer(true);
			break;
		case BtnEscTeam.Esc:
			//队员退出队伍的方法调用
			InRoom.GetInRoomInstantiate().TempTeamPlayerRemove();
			ShowTeamLayer(true);
			break;
		}

	}
	
	function AddTeamMapOK(){
	AllManage.UICLStatic.TeamHeadYesOK();
	}