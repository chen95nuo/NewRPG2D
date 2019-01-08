#pragma strict

function Start () {

}
//var MonsterS : MonsterStatus;
private var playerS : PlayerStatus;
var fsHP : UIFilledSprite;
var fsNU : UIFilledSprite;
private var float1 : float;
private var float2 : float;
private var float3 : float;
private var float4 : float;
var pTime : int;
function Update () {
	if(Time.time > pTime){
		if(playerS != null){
			float1 = parseInt(playerS.Health);
			float2 = parseInt(playerS.Maxhealth);
			float3 = parseInt(playerS.Mana);
			float4 = parseInt(playerS.Maxmana);
			fsHP.fillAmount = float1 / float2;
			fsNU.fillAmount = float3 / float4;
		}
		if(UIControl.mapType == MapType.zhucheng){
			if(fsHP.fillAmount != 1){
				fsHP.fillAmount = 1;
			}
			if(fsNU.fillAmount != 1){
				fsNU.fillAmount = 1;
			}
		}
		pTime = Time.time + 1;
	}
}

private var myTmPS : PlayerStatus;
function SetTeamPlayerAsPS(tmPS : PlayerStatus){
	MyPlayerID = tmPS.PlayerID.ToString();
//	if(uicl.MyTeamHeadID == MyPlayerID){
//		SpriteTeamHead.enabled = true;
//	}else{
//		SpriteTeamHead.enabled = false;		
//	}
	playerS = tmPS;
	LabelName.text = tmPS.PlayerName;

	if(tmPS.ProID == 1){
		SpriteMP.spriteName = "UIM_Anger Article";		
//		SpriteTeam.spriteName = "head-zhanshi";	
	}else
	if(tmPS.ProID == 2){
		SpriteMP.spriteName = "UIM_Charge_ Article";
//		SpriteTeam.spriteName = "head-youxia";	
	}else
	if(tmPS.ProID == 3){
		SpriteMP.spriteName = "UIM_Magic Article";
//		SpriteTeam.spriteName = "head-fashi";	
	}
			if(tmPS.ProID == 1){
				switch(tmPS.BranchID){
					case "0" :
						SpriteTeam.spriteName = "head-zhanshi";
						break; 
					case "1" :
						SpriteTeam.spriteName = "UIM_Anti-War_N";
						break; 
					case "2" :
						SpriteTeam.spriteName = "UIM_Violent-War_N";
						break; 
					case "3" :
						SpriteTeam.spriteName = "UIM_Violent-War_N";
						break; 
				}
			}else
			if(tmPS.ProID == 2){
				switch(tmPS.BranchID){
					case "0" :
						SpriteTeam.spriteName = "head-youxia";
						break; 
					case "1" :
						SpriteTeam.spriteName = "UIM_Robber_O";
						break; 
					case "2" :
						SpriteTeam.spriteName = "UIM_Ranger_N ";
						break; 
					case "3" :
						SpriteTeam.spriteName = "UIM_Ranger_N ";
						break; 
				}
			}else
			if(tmPS.ProID == 3){
				switch(tmPS.BranchID){
					case "0" :
						SpriteTeam.spriteName = "head-fashi";
						break; 
					case "1" :
						SpriteTeam.spriteName = "UIM_Necromancer_N ";
						break; 
					case "2" :
						SpriteTeam.spriteName = "UIM_Master_N";
						break; 
					case "3" :
						SpriteTeam.spriteName = "UIM_Master_N";
						break; 
				}
			}
	while(tmPS.Level == "0"){
		yield;
	}
	LabelLevel.text = tmPS.Level.ToString();
}

function TriggerCube(message : String){
	//print(LabelName.text + " == " + message);
	switch(message){
		case "up" :
			positionID = 0;
			if(showing){
				showing = false;
				AllManage.UICLStatic.TeamMessageUp(myFrom + ": " + myStr);
				myMessageTrans.localScale = Vector3(0,0,0);
			}
			break;
		case "center" :
			positionID = 1;
			if(showing){
				SetShowMessage(myFrom , myStr);
			}
			break;
		case "down" :
			positionID = 2;
			if(showing){
				showing = false;
				AllManage.UICLStatic.TeamMessageDown(myFrom + ": " + myStr);
				myMessageTrans.localScale = Vector3(0,0,0);
			}
			break; 
	}
}

private var stimes : int = 0;
var positionID : int = 0;
var myMessageTrans : Transform;
var LabelMessage : UILabel;
var SpriteMessage : UISprite;
var showing : boolean = false;
var myFrom : String;
var myStr : String;
function SetShowMessage(from : String , str : String){
	stimes += 1;
	var myTimes : int = 0;
	myTimes = stimes;
	myFrom = from;
	myStr = str;
	if(positionID == 1){
		LabelMessage.text = from + ": " + str;
		myMessageTrans.localScale = Vector3(1,1,1);
	}else
	if(positionID == 0){
		AllManage.UICLStatic.TeamMessageUp(from + ": " + str);
	}else
	if(positionID == 2){
		AllManage.UICLStatic.TeamMessageDown(from + ": " + str);
	}
	showing = true;
	LabelMessage.enabled = false;
	SpriteMessage.enabled = false;
	LabelMessage.enabled = true;
	SpriteMessage.enabled = true;
	yield WaitForSeconds(2);
	if(myTimes == stimes && showing){
		LabelMessage.enabled = true;
		SpriteMessage.enabled = true;
		LabelMessage.enabled = false;
		SpriteMessage.enabled = false;
		showing = false;
		myMessageTrans.localScale = Vector3(0,0,0);
	}
}

var LabelName : UILabel;
var LabelLevel : UILabel;
var SpriteTeam : UISprite;
var SpriteHP : UISprite;
var SpriteMP : UISprite;
var MyPlayerID : String;
var SpriteTeamHead : UISprite;
function AddNewTeamPlayer(name : String , Pessn : int , level : int , ps : PlayerStatus , playerID : String){ 
	MyPlayerID = playerID;
	if(uicl.MyTeamHeadID == MyPlayerID){
		SpriteTeamHead.enabled = true;
	}else{
		SpriteTeamHead.enabled = false;		
	}
	playerS = ps;
	LabelName.text = name;
	LabelLevel.text = level.ToString();
	if(Pessn == 1){
//		SpriteHP.spriteName = "UIM_Article _Blood_A";
		SpriteMP.spriteName = "UIM_Anger Article";		
		SpriteTeam.spriteName = "head-zhanshi";	
	}else
	if(Pessn == 2){
//		SpriteHP.spriteName = "UIM_Article _Blood_A";
		SpriteMP.spriteName = "UIM_Charge_ Article";
		SpriteTeam.spriteName = "head-youxia";	
	}else
	if(Pessn == 3){
//		SpriteHP.spriteName = "UIM_Article _Blood_A";
		SpriteMP.spriteName = "UIM_Magic Article";
		SpriteTeam.spriteName = "head-fashi";	
	}
}

var uicl : UIControl;
function selectMe(){
	uicl.SelectTeamPlayerAsID(MyPlayerID);
}
