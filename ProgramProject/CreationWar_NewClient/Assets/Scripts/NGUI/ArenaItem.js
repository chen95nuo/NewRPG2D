#pragma strict

var areCL : ArenaControl;
var Spriteicon : UISprite;
var Labelname : UILabel;
var Labeljisha : UILabel;
var Labelsiwang : UILabel;
var Labeljiqiao : UILabel;
var Labelrongyu : UILabel;
var myAS : ArenaSettlement;
var SpriteTeams : UISprite[];
//							playerInfo.Add("id", player.myID);
//							playerInfo.Add("name", player.myNickName);
//							playerInfo.Add("kill", player.BattlefieldKillCount.ToString());
//							playerInfo.Add("die", player.BattlefieldDieCount.ToString());
//							playerInfo.Add("teamLabel", player.myPVPTeam.BattlefieldLabel);
//							playerInfo.Add("score",player.PVPScore.ToString());
//							playerInfo.Add("pro",player.myTable[0]["ProID"].YuanColumnText);				
function SetArenaItem(AS : System.Collections.Generic.Dictionary.<String, String> , TeamID : int){
	Labelname.text = AS["name"];
	Labeljisha.text = AS["kill"];
	Labelsiwang.text = AS["die"];
	
	Labeljiqiao.text = AS["score"];

	Labelrongyu.text = AS["pvpPoint"]; 
	
	
	SpriteTeams[0].enabled = false;
	SpriteTeams[1].enabled = false;
	SpriteTeams[TeamID].enabled = true;
	if(AS["teamLabel"] == "Red"){
		SpriteTeams[0].enabled = true;
	}else
	if(AS["teamLabel"] == "Blue"){
		SpriteTeams[1].enabled = true;
	}
	
	switch(AS["pro"]){
		case "1" :
			Spriteicon.spriteName = "head-zhanshi";		
			break;
		case "2":
			Spriteicon.spriteName = "head-youxia";		
			break;
		case "3":
			Spriteicon.spriteName = "head-zhanshi";		
			break;
	}
//	Spriteicon.spriteName = AS.icon;		
}

private var useInt : int = 0;
function SetOneKill(){
//	//print("ji sha le 4444" + myAS.ps.gameObject.name);
	try{
		useInt = parseInt(Labeljisha.text);
	}catch(e){
		useInt = 0;
	}	
	 Labeljisha.text = (useInt + 1).ToString();
} 
function SetBeKill(){ 
//	//print("ji sha le 5555" + myAS.ps.gameObject.name);
	try{
		useInt = parseInt(Labelsiwang.text);
	}catch(e){
		useInt = 0;
	}	
	 Labelsiwang.text = (useInt + 1).ToString();
}
function SetJiqiao(fen : int){
	try{
		useInt = parseInt(Labeljiqiao.text);
	}catch(e){
		useInt = 0;
	}	
	 Labeljiqiao.text = (useInt + fen).ToString();
}
function SetRongyu(fen : int){
	try{
		useInt = parseInt(Labelrongyu.text);
	}catch(e){
		useInt = 0;
	}	
	 Labelrongyu.text = (useInt + fen).ToString();
}
