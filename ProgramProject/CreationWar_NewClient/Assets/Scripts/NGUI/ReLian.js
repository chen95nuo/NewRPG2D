#pragma strict

	var relian = true;
	function ButtonLianJie(){
		    AllManage.qrStatic.ShowQueRen(gameObject , "RealLianJie" , "NoChong" , "messages003");
	}

	function RealLianJie(){
		if(PhotonNetwork.offlineMode && relian){
	         relian = false;
			lianjie();
			
		}
	}	
//var LabelText : UILabel;
var offlinemodeobj:GameObject;	
	static var LoadingStr : String;
	var times : int = 0;
	function lianjie(){
		 PlayerAI.AutoAI =false;
		times = 0;
		PhotonNetwork.offlineMode = false;
		PhotonNetwork.offlineMode_inRoom = false;
		yield WaitForSeconds(1);
       while( times<3 &&(!PhotonNetwork.connected ||PhotonNetwork.connectionState == ConnectionState.Connecting) ){
             if(!PhotonNetwork.connected && PhotonNetwork.connectionState != ConnectionState.Connecting ){
//             LabelText.text += "\n" + "正在建立连接_";
             times +=1;
                     	 var txtServerIP = PlayerPrefs.GetString("InAppServerIP","117.131.207.219:5055");
        	 var charIP=":";
        	 var IP =txtServerIP.Split(charIP.ToCharArray())[0];
        	 var post = 5055;
             PhotonNetwork.Connect(IP, post, "Master", "1.0");  
       
              AllManage.tsStatic.ShowBlue("tips098");
             }

			yield WaitForSeconds(1+times);
		//	//print("waiting111111");
		   }
		   if(PhotonNetwork.connected){
		   
		   }
//		   LabelText.text += "\n" + "连接成功！";
		   else{
//		    LabelText.text += "\n" + "连接失败！重试 ";
		    PhotonNetwork.offlineMode = true;
		    PhotonNetwork.offlineMode_inRoom = true;
		    relian = true;
		     AllManage.tsStatic.ShowBlue("tips099");
//		    AllManage.qrStatic.ShowQueRen(gameObject , "RealLianJie" , "NoChong" , "转换模式失败，是否重试？");
		    return;
		   }
//		//print("PhotonNetwork.connectionState == " + PhotonNetwork.connectionState + " offlineMode == " + PhotonNetwork.offlineMode +"  ==name == " + LoadingStr);
//		yield WaitForSeconds(1); 
			PhotonNetwork.DoClearYuanList();
			
//		PhotonNetwork.JoinRoom(LoadingStr,true);
	   			var boolplus : boolean = false;
				for(var roomInfo : RoomInfo in PhotonNetwork.GetRoomList()){
					if(roomInfo.name == LoadingStr){
						boolplus = true;
						if(roomInfo.playerCount >= BtnGameManager.roomPlayerNum){
							LoadingStr += "1";
							boolplus = false;
							while( ! boolplus){
								var yesHaveNoom : boolean = false;
								for(var roomInfo1 : RoomInfo in PhotonNetwork.GetRoomList()){
									if(roomInfo.name == LoadingStr){
										yesHaveNoom = true;
										if(roomInfo.playerCount >= BtnGameManager.roomPlayerNum){
											LoadingStr += "1";
										}else{
											boolplus = true;
										}
									}
								}
								if( !yesHaveNoom){
									boolplus = true;
								}
								yield;
							}
							PhotonNetwork.JoinRoom(LoadingStr,true);					
						}else{
							PhotonNetwork.JoinRoom(LoadingStr,true);			
						}
					}
				}
				if( ! boolplus){
					PhotonNetwork.JoinRoom(LoadingStr,true);							
				}
	}
	
	function NoChong(){
	
	}
    function OnDisconnectedFromPhoton()
    {
		    AllManage.tsStatic.ShowBlue("tips099");
    	    relian = true;
	}
	
    function OnPhotonJoinRoomFailed(){
	    Debug.Log("JoinRoom Failed from Photon.");
  	PhotonNetwork.CreateRoom(LoadingStr, true, true, BtnGameManager.roomPlayerNum + 5);
	    yield WaitForSeconds(2.5);
	    if(relian)
	      return;
			PhotonNetwork.DoClearYuanList();
	   			var boolplus : boolean = false;
				for(var roomInfo : RoomInfo in PhotonNetwork.GetRoomList()){
					if(roomInfo.name == LoadingStr){
						boolplus = true;
						if(roomInfo.playerCount >= BtnGameManager.roomPlayerNum){
							LoadingStr += "1";
							boolplus = false;
							while( ! boolplus){
								var yesHaveNoom : boolean = false;
								for(var roomInfo1 : RoomInfo in PhotonNetwork.GetRoomList()){
									if(roomInfo.name == LoadingStr){
										yesHaveNoom = true;
										if(roomInfo.playerCount >= BtnGameManager.roomPlayerNum){
											LoadingStr += "1";
										}else{
											boolplus = true;
										}
									}
								}
								if( !yesHaveNoom){
									boolplus = true;
								}
								yield;
							}
							PhotonNetwork.JoinRoom(LoadingStr,true);					
						}else{
							PhotonNetwork.JoinRoom(LoadingStr,true);			
						}
					}
				}
				if( ! boolplus){
					PhotonNetwork.JoinRoom(LoadingStr,true);							
				}
		yield WaitForSeconds(2);
		times += 1;
		if(times >= 3){
//		LabelText.text += "\n" + "连接失败！重试 ";
		PhotonNetwork.offlineMode = true;
		PhotonNetwork.offlineMode_inRoom = true;
		    relian = true;
		    AllManage.tsStatic.ShowBlue("tips099");
//		    AllManage.qrStatic.ShowQueRen(gameObject , "RealLianJie" , "NoChong" , "转换模式失败，是否重试？");
//			PhotonNetwork.Disconnect(); 	
		}
    } 

        function OnJoinedRoom(){
//        //print("1111111");
        	if(isDisconnect){
        		isDisconnect = false;
		        if(UIControl.mapType == MapType.zhucheng){
					if(PlayerStatus.MainCharacter){
						GameManager.reLevel = Application.loadedLevelName;
						GameManager.rePlayerName = InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText;
						GameManager.rePosition = PlayerStatus.MainCharacter.transform.position;
					}
					relian = true;
					AllManage.tsStatic.ShowBlue("tips100");
//					offlinemodeobj.SetActiveRecursively(false);
					AllManage.dungclStatic.readedRoomSP = false;
					alljoy.DontJump = true;
//					if(PlayerStore.me){
//						Destroy(PlayerStore.me.gameObject);
//					}

					yield;
					//		Debug.LogError("ReLian Join -------------------------------------------------------------------------------------------");
					Loading.Level = Application.loadedLevelName;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
						AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

					if(AllResources.ar != null){
						AllResources.ar.LoadGUI(gameObject);
					}
					PhotonNetwork.LoadLevel(Application.loadedLevelName);
					//	     		//print("222222222");
		        }else{
		        	AllManage.UICLStatic.nowYesReturn();
		        }
     		}
		}
    
    function OnReceivedRoomListUpdate()
    { 
//        Debug.Log("OnReceivedRoomList1111111111111111111111111111111111111111111111111111111111111111");
//		for(var roomInfo : RoomInfo  in PhotonNetwork.GetRoomList()){
//		Debug.Log(roomInfo.name + " == roomInfo.name");
//		}
    }
   
 	function OnJoinedLobby(){ 
 	         PhotonNetwork.autoCleanUpPlayerObjects = true; 
//        Debug.Log("OnJoinedLobby222222222222222222222222222222222222222222222222222222222222222222222222222");
 	} 
 	
 	private var isDisconnect : boolean = false;
 	function SetOffLineActiveAsBool(bool : boolean){
 		isDisconnect = true;
 		if(UIControl.mapType != MapType.jingjichang && Application.loadedLevelName != "Map200"){
// 			offlinemodeobj.SetActiveRecursively(bool);
 			AllManage.qrStatic.ShowQueRen(gameObject , "LianYes" , "LianNo" , "messages003");
 		}else{
 			AllManage.UICLStatic.DisconnectZhanChang();
 		}
// 		print(bool);
 	}
 	
 	function LianYes(){
		if(PhotonNetwork.offlineMode && relian){
	        relian = false;
	        
			lianjie();
		
		} 	
 	}
 	 
 	function LianNo(){
 		Loading.Level = "Login-1";
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		AllManage.UICLStatic.RemoveAllTeam();
		alljoy.DontJump = true;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
		yield;
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

 	}
 	