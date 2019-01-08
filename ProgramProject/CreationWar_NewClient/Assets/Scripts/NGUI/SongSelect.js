#pragma strict
//
//var num : int = 0;
//var str : String;
//function Start(){
//}
//
//function Update(){
////	num = 23;
//	str = ToInt.IntToStr(num);
//	print(str);
//	print(ToInt.StrToInt(str));
//}

public var playerListGrid : Transform;
var UIckb : UIToggle[];
var can : boolean = false;
private var FStr : String = ";";
var war : Warnings;
var useYr : yuan.YuanMemoryDB.YuanRow;
function StartGame(){
	AllResources.ridemod = false;
	PlayerPrefs.SetInt("ConsumerTip" , 1);
		var playerID : String;
		var btnS : BtnPlayer;
		
		UIckb = playerListGrid.GetComponentsInChildren.<UIToggle>();
		
		for(var ckb : UIToggle in UIckb){
			btnS = ckb.GetComponent(BtnPlayer);
			if(ckb.value && btnS.btnType == BtnPlayer.BtnType.Read){
	            var yr=ckb.gameObject.GetComponent(BtnPlayer).yuanRow;
				useYr = yr;
				if(yr["deviceID"].YuanColumnText == SystemInfo.deviceUniqueIdentifier){			
					InventoryControl.shangxian = true;
					InventoryControl.yt.Clear();
					InventoryControl.yt.Add(ckb.gameObject.GetComponent(BtnPlayer).yuanRow);
					BtnGameManager.yt=InventoryControl.yt;
					playerID = yr["PlayerID"].YuanColumnText;
					BtnManager.my.RunBeginTimeOut(15,3,function() BtnManager.my.ConnectInRoom(3) ,function() InRoom.GetInRoomInstantiate().SendID(yr["PlayerID"].YuanColumnText.Trim(), yr["ProID"].YuanColumnText.Trim(), yr["PlayerName"].YuanColumnText.Trim(),false , PlayerPrefs.GetString ("Language","CH"),SystemInfo.deviceUniqueIdentifier,0,0,0,0),null);
	                //InRoom.GetInRoomInstantiate().SendID(yr["PlayerID"].YuanColumnText.Trim(), yr["ProID"].YuanColumnText.Trim(), yr["PlayerName"].YuanColumnText.Trim(),false , PlayerPrefs.GetString ("Language","CH"),SystemInfo.deviceUniqueIdentifier);
	                PhotonNetwork.playerName = "Guest" + playerID;
					InRoom.GetInRoomInstantiate().isUpdatePlayerLevel = false;
	                UIControl.oldLevel = parseInt(yr["PlayerLevel"].YuanColumnText);
			        var useStr : String[];
					useStr = yr["CompletTask"].YuanColumnText.Split(FStr.ToCharArray());
					UIControl.oldTsk = useStr;
					InventoryControl.PlayerID = playerID;
					InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.StartGame).ToString());
					
					UIControl.isLoadScene = false;
					//TD_info.setTDUserInit(yr["UserInfo_userId"].YuanColumnText + yr["PlayerID"].YuanColumnText + ";" + yr["PlayerName"].YuanColumnText + ";" + yr["PlayerLevel"].YuanColumnText + ";" +  PlayerPrefs.GetString("InAppServerName", "NON"));
//			        if(!PhotonNetwork.connected){
//			        	 var txtServerIP = PlayerPrefs.GetString("InAppServerIP","117.131.207.219:5055");
//			        	 var charIP=":";
//			        	 var IP =txtServerIP.Split(charIP.ToCharArray())[0];
//			        	 var post = 5055;
//			             PhotonNetwork.Connect(IP, post, "Master", "1.0"); 
//			             while(!PhotonNetwork.connected){
////			             	print("connecting ......");
//			             	yield;
//			             }
//			        }
//			        print(PhotonNetwork.connected + " == PhotonNetwork.connected");
					while(!MainMenuManage.isSetID)
					{
						yield;
					}
					MainMenuManage.isSetID=false;
					if(yr["Place"].YuanColumnText == ""){
						Loading.PlayerName = yr["PlayerName"].YuanColumnText;
						Loading.Level = "Map200";
						AllResources.ar.AllLoadLevel("Loading 1");
//						Application.LoadLevel("Loading 1");
		
					}else{			
						Loading.Level ="Map" + yr["Place"].YuanColumnText;
						AllResources.ar.AllLoadLevel("Loading 1");
//												AllResources.ar.AllLoadLevel("Loading 1");
//						Application.LoadLevel("Loading 1");

					}
				}else{
					war.warningAllEnterClose.btnEnter.target = this.gameObject;
					war.warningAllEnterClose.btnEnter.functionName = "Bangding";
					war.warningAllEnterClose.Show(AllManage.AllMge.Loc.Get("info358") , AllManage.AllMge.Loc.Get("info563"));
				}
				return;
			}
		}
//	}
}

var RefreshBaffle : GameObject;
function Bangding(){
	//InRoom.GetInRoomInstantiate().BindDevice(useYr["UserInfo_userId"].YuanColumnText , SystemInfo.deviceUniqueIdentifier);
	BtnManager.my.BeginTimeOut(10,2,BtnManager.my.ConnectInRoom,function() InRoom.GetInRoomInstantiate().BindDevice(useYr["UserInfo_userId"].YuanColumnText , SystemInfo.deviceUniqueIdentifier),null);
	war.warningAllEnterClose.Close();
	//RefreshBaffle.SetActiveRecursively(true);
}

function BangDingOK(){
	UIControl.isLoadScene = false;
	RefreshBaffle.SetActiveRecursively(false);
	var playerID : String;
	useYr["deviceID"].YuanColumnText = SystemInfo.deviceUniqueIdentifier;
	InventoryControl.shangxian = true;
	InventoryControl.yt.Clear();
	InventoryControl.yt.Add(useYr);
	BtnGameManager.yt=InventoryControl.yt;
	playerID = useYr["PlayerID"].YuanColumnText;
	BtnManager.my.RunBeginTimeOut(15,3,function() BtnManager.my.ConnectInRoom(3),function() InRoom.GetInRoomInstantiate().SendID(useYr["PlayerID"].YuanColumnText.Trim(), useYr["ProID"].YuanColumnText.Trim(), useYr["PlayerName"].YuanColumnText.Trim(),false , PlayerPrefs.GetString ("Language","CH"),SystemInfo.deviceUniqueIdentifier,0,0,0,0),null);
	//InRoom.GetInRoomInstantiate().SendID(useYr["PlayerID"].YuanColumnText.Trim(), useYr["ProID"].YuanColumnText.Trim(), useYr["PlayerName"].YuanColumnText.Trim(),false , PlayerPrefs.GetString ("Language","CH"),SystemInfo.deviceUniqueIdentifier);
	PhotonNetwork.playerName = "Guest" + playerID;
	UIControl.oldLevel = parseInt(useYr["PlayerLevel"].YuanColumnText);
	var useStr : String[];
	useStr = useYr["CompletTask"].YuanColumnText.Split(FStr.ToCharArray());
	InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.StartGame).ToString());
	UIControl.oldTsk = useStr;
	InventoryControl.PlayerID = playerID;
	//TD_info.setTDUserInit(useYr["UserInfo_userId"].YuanColumnText + useYr["PlayerID"].YuanColumnText + ";" + useYr["PlayerName"].YuanColumnText + ";" + useYr["PlayerLevel"].YuanColumnText + ";" +  PlayerPrefs.GetString("InAppServerName", "NON"));
//			        if(!PhotonNetwork.connected){
//			        	 var txtServerIP = PlayerPrefs.GetString("InAppServerIP","117.131.207.219:5055");
//			        	 var charIP=":";
//			        	 var IP =txtServerIP.Split(charIP.ToCharArray())[0];
//			        	 var post = 5055;
//			             PhotonNetwork.Connect(IP, post, "Master", "1.0"); 
//			             while(!PhotonNetwork.connected){
////			             	print("connecting ......");
//			             	yield;
//			             }
//			        }
//			        print(PhotonNetwork.connected + " == PhotonNetwork.connected");
	while(!MainMenuManage.isSetID)
	{
		yield;
	}
	MainMenuManage.isSetID=false;
	if(useYr["Place"].YuanColumnText == ""){
		Loading.PlayerName = useYr["PlayerName"].YuanColumnText;
		Loading.Level = "Map200";
								AllResources.ar.AllLoadLevel("Loading 1");
//						Application.LoadLevel("Loading 1");
		
	}else{			
		Loading.Level ="Map" + useYr["Place"].YuanColumnText;
								AllResources.ar.AllLoadLevel("Loading 1");
//						Application.LoadLevel("Loading 1");

	}
}

function StartGameYuan(yr:yuan.YuanMemoryDB.YuanRow)
{
	InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.StartGame).ToString());
            var playerID : String;
    		InventoryControl.shangxian = true;
				InventoryControl.yt.Clear();
				InventoryControl.yt.Add(yr);
				BtnGameManager.yt=InventoryControl.yt;
				playerID = yr["PlayerID"].YuanColumnText;
				BtnManager.my.RunBeginTimeOut(15,3,function() BtnManager.my.ConnectInRoom(3),function() InRoom.GetInRoomInstantiate().SendID(yr["PlayerID"].YuanColumnText.Trim(), yr["ProID"].YuanColumnText.Trim(), yr["PlayerName"].YuanColumnText.Trim(),false , PlayerPrefs.GetString ("Language","CH"),SystemInfo.deviceUniqueIdentifier,0,0,0,0),null);
                //InRoom.GetInRoomInstantiate().SendID(yr["PlayerID"].YuanColumnText.Trim(), yr["ProID"].YuanColumnText.Trim(), yr["PlayerName"].YuanColumnText.Trim(),false , PlayerPrefs.GetString ("Language","CH"),SystemInfo.deviceUniqueIdentifier);
				InventoryControl.PlayerID = playerID;

				while(!MainMenuManage.isSetID)
				{
					yield;
				}
				MainMenuManage.isSetID=false;

				if(yr["Place"].YuanColumnText == ""){
					Loading.PlayerName = yr["PlayerName"].YuanColumnText;
					Loading.Level = "Map200";
											AllResources.ar.AllLoadLevel("Loading 1");
//						Application.LoadLevel("Loading 1");
		
				}else{			
					Loading.Level ="Map" + yr["Place"].YuanColumnText;
											AllResources.ar.AllLoadLevel("Loading 1");
//						Application.LoadLevel("Loading 1");

				}
}

var conTimes : int = 0;
function OnFailedToConnectToPhoton( cause : DisconnectCause)
{
	InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.PhotonRoomConFail).ToString());
//	print(cause + " == cause");
//	if(conTimes < 3){
//		conTimes += 1;
//		print(conTimes);
//	 var txtServerIP = PlayerPrefs.GetString("InAppServerIP","117.131.207.219:5055");
//	 var charIP=":";
//	 var IP =txtServerIP.Split(charIP.ToCharArray())[0];
//	 var post = 5055;
//		PhotonNetwork.Connect(IP, post, "Master", "1.0"); 
//	}else{
		Application.LoadLevel("Login-1");
//	}
//	Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters);
}

function chuangjian(){
        while (PhotonNetwork.room == null)
        {
            return;
        }
        

        // Temporary disable processing of futher network messages
        PhotonNetwork.isMessageQueueRunning = false;
        Application.LoadLevel(2);

}

var yt : yuan.YuanMemoryDB.YuanTable;
function GetYT(y : yuan.YuanMemoryDB.YuanTable){
	yt = y;
}

function Select1(){
	Application.LoadLevel(2);
	InventoryControl.PlayerSelect = yt.Rows[0]["PlayerID"].YuanColumnText;
}

function Select2(){
	Application.LoadLevel(2);
	InventoryControl.PlayerSelect = yt.Rows[1]["PlayerID"].YuanColumnText;
}

function Select3(){
	Application.LoadLevel(2);
	InventoryControl.PlayerSelect = yt.Rows[2]["PlayerID"].YuanColumnText;
}

function Select4(){
	Application.LoadLevel(2);
	InventoryControl.PlayerSelect = yt.Rows[3]["PlayerID"].YuanColumnText;
}

function Select5(){
	Application.LoadLevel(2);
	InventoryControl.PlayerSelect = yt.Rows[4]["PlayerID"].YuanColumnText;
}

function Select6(){
	Application.LoadLevel(2);
	InventoryControl.PlayerSelect = yt.Rows[5]["PlayerID"].YuanColumnText;
}

function SongLoad(Level : String){
	Loading.isLogin = true;
	Loading.Level = Level;
							AllResources.ar.AllLoadLevel("Loading 1");
//						Application.LoadLevel("Loading 1");

}

