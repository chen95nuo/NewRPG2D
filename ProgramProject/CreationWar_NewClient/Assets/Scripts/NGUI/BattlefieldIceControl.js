#pragma strict

var Flags : TriggerStone[];

var shitou : BattlefieldCityItem[];

var shuijing : DOTMonsterControl[];
function Start(){
	yield;
	yield;
	var i : int = 0;
	var useInt : int = 0;
	var useSpStr : String = "";
	var strSP1 : String = "";
	var strSP2 : String = "";
	
	for(i=0; i<Flags.length; i++){		
		if(PhotonNetwork.room){
			useSpStr = PhotonNetwork.room.customProperties["flags" + Flags[i].viewID]; 
		}else{
			useSpStr = null;
		}
		if(DungeonControl.staticRoomSP != null && useSpStr == null)
			useSpStr = DungeonControl.staticRoomSP["flags" + Flags[i].viewID]; 
		
		////print(useSpStr + " == useSpStr == " + Flags[i].myFlagID + " == " + Flags[i].viewID);
		if(useSpStr != null){
			if((useSpStr == "0" || useSpStr == "1") && Flags[i].myFlagID != "0" && Flags[i].myFlagID != "1")
				Flags[i].SetFlagAsID(useSpStr);
		}else{
			DungeonControl.staticRoomSP.Add("flags" + Flags[i].viewID ,"No");
		}
	}
	if(PhotonNetwork.connected && PhotonNetwork.isMasterClient)
	PhotonNetwork.room.SetCustomProperties(DungeonControl.staticRoomSP);

while(!PlayerStatus.MainCharacter){
 yield;
}
yield WaitForSeconds(1);

	if(shitou.length < 1)
	shitou = FindObjectsOfType(BattlefieldCityItem);
	
	shuijing = FindObjectsOfType(DOTMonsterControl);
	
	var strTry : int;
	for(i=0; i<2; i++){		
		if(PhotonNetwork.room && PhotonNetwork.room.customProperties){
			try{
			if(shitou.length > 0){
				strTry = PhotonNetwork.room.customProperties["BattlefieldPoint" + shitou[i].viewID];
			}else
			if(shuijing.length > 0){
				strTry = PhotonNetwork.room.customProperties["BattlefieldPoint" + shuijing[i].viewID];
			}
				////print(strTry);
				if(strTry)
					useSpStr = strTry.ToString(); 
			}catch(e){
				useSpStr = null;			
			}
		}else{
			useSpStr = null;
		}
		////print(useSpStr);
		if(DungeonControl.staticRoomSP != null && useSpStr == null){
			try{
				if(shitou.length > 0){
					strTry = DungeonControl.staticRoomSP["BattlefieldPoint" + shitou[i].viewID];
				}else
				if(shuijing.length > 0){
					strTry = DungeonControl.staticRoomSP["BattlefieldPoint" + shuijing[i].viewID];
				}
			//	//print(strTry);
				if(strTry)
					useSpStr = strTry.ToString(); 
			}catch(e){
				useSpStr = null;			
			}
		}
		if(useSpStr != null && useSpStr != ""){
			if(shitou.length > 0){
				shitou[i].health = parseInt(useSpStr);	
				shitou[i].Status.Health = (shitou[i].health).ToString();		
			}else
			if(shuijing.length > 0){
				shuijing[i].Health = parseInt(useSpStr);			
			}
		}else{
		}
	}
	if(PhotonNetwork.connected && PhotonNetwork.isMasterClient)
	PhotonNetwork.room.SetCustomProperties(DungeonControl.staticRoomSP);
}
