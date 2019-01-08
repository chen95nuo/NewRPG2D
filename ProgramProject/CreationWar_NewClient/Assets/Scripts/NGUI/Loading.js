//import ExitGames.Client.Photon;
//import System;
//import System.Collections;
//import System.Collections.Generic;
//import ExitGames.Client.Photon;
//import UnityEngine;

static var Level : String = "";
var isLevel1 : boolean = false;
var async : AsyncOperation ;
var progress : int = 0;
static var LoadingStr : String = "";
static var TeamID : String = "";
static var loadstr : String = ""; 
static var PlayerName : String = ""; 
static var nandu : String = "";
static var useNandu : String = "";
static var DaTingLoadingStr : String = "";
static var YaoQingStr : String = "";
private var isPVP : boolean = false; 
var LabelText : UILabel;
var times : int = 0;
static var GameHelp : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("GameHelp1","id");
var LabelHelp : UILabel;
static var AgainTimes : int = 0;
static var loadTimes : int = 0;
static var isLogin : boolean = false;
function Start () { 
	
//	print("Level.Substring(3,3) == " + Level.Substring(3,3));
	canOffLine = true;
	if(isLevel1){
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");
	
		return;
	}
//	print(Level + " == Level");
	if(Level == "Login-1"){
		DungeonControl.ReLevel = "";
	}
	InRoom.GetInRoomInstantiate().ClearLookPlayer();
	DungeonControl.alreadyRewards = false;
	ptime = Time.time + 10;
	if(isLogin){
		isLogin = false;
		loadScene();
		return;
	}
	ItemMove.itemMove = false;
	alljoy.DontJump = false;
	if(nandu == ""){
		nandu = "1";
	}
	useNandu = nandu;
	var ar = FindObjectOfType(AllResources);
	ar.ClearAll();
	DungeonControl.staticRoomSP = new ExitGames.Client.Photon.Hashtable();
	ArenaControl.staticRoomArean = new ExitGames.Client.Photon.Hashtable();
	PhotonNetwork.DoClearYuanList();
	ArenaControl.ArenaIng  = true;		
	ArenaControl.isArenaRed  = false;					
	ArenaControl.isArenaBlue  = false;						
	ArenaControl.RedTeam = null;
	ArenaControl.BlueTeam = null;
	Resources.UnloadUnusedAssets();
	GameHelp = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytHelp;
	if(GameHelp && GameHelp.Rows && GameHelp.Rows.Count > 0 && LabelHelp){ 
		LabelHelp.text = GameHelp.Rows[ Random.Range(0,GameHelp.Rows.Count) ]["HelpTip"].YuanColumnText;
	}
//PhotonNetwork.automaticallySyncScene = true;   PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated
	//print(BtnGameManager.roomPlayerNum + " == BtnGameManager.roomPlayerNum");
//if(!isLevel1){
//  	}else{
  		
  		DaTingLoadingStr = "";
		loadTimes = 0;
		if(PhotonNetwork.offlineMode == true){
			PhotonNetwork.offlineMode = false;
			PhotonNetwork.offlineMode_inRoom = false;
			//	  //print("changeofflinmode2");
			PhotonNetwork.Disconnect(); 
//			yield WaitForSeconds(1);
			try{
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add(LabelText.text + "\n");
				AllManage.AllMge.Keys.Add("messages137");
				AllManage.AllMge.SetLabelLanguageAsID(LabelText);
			}catch(e){
				Application.LoadLevel(Application.loadedLevel);
				//print("123123123");
			}
		//		  LabelText.text += "\n" + "退出省流量模式！";
		}else 
		if(PhotonNetwork.room){
//			if(PhotonNetwork.room.playerCount <= 1){
//				PhotonNetwork.DestroyAll();
//			}
			//print("jin dao zhe li le --------");
			PhotonNetwork.LeaveRoom();
//			yield;
			LabelText.text += "\n" + AllManage.AllMge.Loc.Get("messages137");
			//print(AllManage.AllMge.Loc.Get("messages137") + " == AllManage.AllMge.Loc.Get");
			while(PhotonNetwork.room){
				yield;
			}
//			while(PhotonNetwork.connected || PhotonNetwork.connectionState == ConnectionState.Connecting||PhotonNetwork.connectionState == ConnectionState.Disconnecting){
//				yield;
//			}
		}
		yield;
		//print("jin dao zhe li le --------  " + Level);
		if(Level == "Login-1"){
			Application.LoadLevel("Login-1");
		}else{
			returnTimes += 1;
			if(returnTimes > 2){
				returnTimes = 0;
//				if(DungeonControl.ReLevel && DungeonControl.ReLevel != ""){
//					Level = DungeonControl.ReLevel;				
//				}else{
////					Level = "Map111";
//				}
				canOffLine = true;
				newLoading1();
				return;
			}
			newLoading1();
//				AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");
	
		}
//	}
}

private var isYaoQing : boolean = false;
var canOffLine : boolean = false;
static var nowRoomName : String = "";
function newLoading1(){
	nowRoomName = LoadingStr;
 	var chongshi = 0;
	var isOffLine : boolean = false;
//	if(InventoryControl.yt && parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) <= 10){
////		print(Level);
//		var str : String = Level.Substring(3,1);
////		str = "";
//		if(str=="1"){
////				mapType = MapType.zhucheng;
//		}else
//		if(str=="3" || str=="4"){
////				mapType = MapType.jingjichang;	
//		}else
//		if(str=="7"){
////				mapType = MapType.yewai;
//		}else
//		{
			isOffLine = true;
////				mapType = MapType.fuben;
//		}
//	}
	if(Level == "Map200" || isOffLine || canOffLine){// &&!UIControl.nowIsPVPGO){
//		PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.OfflineMode;
		PhotonNetwork.offlineMode = true;
		PhotonNetwork.offlineMode_inRoom = true;
		MoveToGameScene1();		
	}else{
		PhotonNetwork.offlineMode = false;
		PhotonNetwork.offlineMode_inRoom = false;					

		      	while( chongshi<3 &&(!PhotonNetwork.connected ||PhotonNetwork.connectionState == ConnectionState.Connecting) ){
					try{
			             if(!PhotonNetwork.connected && PhotonNetwork.connectionState != ConnectionState.Connecting ){
			 			AllManage.AllMge.Keys.Clear();
						AllManage.AllMge.Keys.Add(LabelText.text + "\n");
						AllManage.AllMge.Keys.Add("messages131");
						AllManage.AllMge.SetLabelLanguageAsID(LabelText);
			            LabelText.text += "\n" + "正在建立连接_";
			             chongshi +=1;
			        	 var txtServerIP = PlayerPrefs.GetString("InAppServerIP","117.131.207.219:5055");
			        	 var charIP=":";
			        	 var IP =txtServerIP.Split(charIP.ToCharArray())[0];
			        	 var post = 5055;
			             PhotonNetwork.Connect(IP, post, "Master", "1.0");  
			             }
			        }catch(e){
			        	//print("123123123");
		    			Application.LoadLevel(Application.loadedLevel);
		    		}
					yield WaitForSeconds(2+chongshi);
				}
	}
			
				try{
				   if(PhotonNetwork.connected){
		 			AllManage.AllMge.Keys.Clear();
					AllManage.AllMge.Keys.Add(LabelText.text + "\n");
					AllManage.AllMge.Keys.Add("messages132");
					AllManage.AllMge.SetLabelLanguageAsID(LabelText);
		//		   LabelText.text += "\n" + "连接成功！";
					}
				   else{
		 			AllManage.AllMge.Keys.Clear();
					AllManage.AllMge.Keys.Add(LabelText.text + "\n");
					AllManage.AllMge.Keys.Add("messages133");
					AllManage.AllMge.SetLabelLanguageAsID(LabelText);
		//		    LabelText.text += "\n" + "连接失败！转化为省流量模式！";
		             OfflineMode();
		             return;
				   }  
				PlayerInfo.canInviteGoPVE = false;
				if(loadstr != "" || TeamID != "" ||( Level!="Login-1" && ( Level.Length > 3 && parseInt(Level.Substring(3,1)) > 1 )) || YaoQingStr != ""){
					if(Level!="Login-1" && parseInt(Level.Substring(3,1)) != 1 && parseInt(Level.Substring(3,1)) != 7){
						if(YaoQingStr != ""){
							LoadingStr = YaoQingStr.ToString();
							YaoQingStr = "";
							isYaoQing = true;
							PlayerInfo.canInviteGoPVE = true;
						}else
						if(loadstr != ""){
							isPVP = true;
							LoadingStr = Level + nandu + loadstr; // + UIControl.BattlefieldinstanceID;
						}else
						if(TeamID != ""){
							LoadingStr =  Level+ nandu + TeamID;
						}else{
							PlayerInfo.canInviteGoPVE = true;
							LoadingStr = Level+ nandu + PlayerName;
						}
					}else{
						LoadingStr = Level + "11";
					}
				}else{
					LoadingStr = Level +"11";
				}
//					LoadingStr += "1";
//				HideOtherPlayers
				if(parseInt(Level.Substring(3,1)) == 1){
//					print(PlayerPrefs.GetInt("HideOtherPlayers", 0) + " == PlayerPrefs.GetInt(");
					if( PlayerPrefs.GetInt("HideOtherPlayers", 0) == 1 ){
						LoadingStr += InventoryControl.yt.Rows[0]["PlayerName"].YuanColumnText;
					}
				}
				if(!isYaoQing)
					LoadingStr += ";0";
				PlayerInfo.mapName = LoadingStr.ToString();
				DaTingLoadingStr = LoadingStr.ToString();
	        }catch(e){
 				//print("123123123");
   				Application.LoadLevel(Application.loadedLevel);
    		}
				if(Level == "Login-1"){ 
			 		if(isLoading){
						return;
					}
					PhotonNetwork.Disconnect();
  

					while(PhotonNetwork.connected || PhotonNetwork.connectionState == ConnectionState.Disconnecting){
						try{
							AllManage.AllMge.Keys.Clear();
							AllManage.AllMge.Keys.Add(LabelText.text + "\n");
							AllManage.AllMge.Keys.Add("messages134");
							AllManage.AllMge.SetLabelLanguageAsID(LabelText);
				//				LabelText.text += "\n" + "正在退出区域_";
				        }catch(e){
			        	//print("123123123");
			    			Application.LoadLevel(Application.loadedLevel);
			    		}
						yield;	
					}
			 		if(isLoading){
						return;
					}
					try{
						AllManage.AllMge.Keys.Clear();
						AllManage.AllMge.Keys.Add(LabelText.text + "\n");
						AllManage.AllMge.Keys.Add("messages135");
						AllManage.AllMge.Keys.Add(Level + "");
						AllManage.AllMge.SetLabelLanguageAsID(LabelText);
						loadstr = "";
						nandu = "";
						LoadingStr = "";
						YaoQingStr = "";
						PhotonNetwork.LoadLevel(Level);
			        }catch(e){
		    			Application.LoadLevel(Application.loadedLevel);
					        	//print("123123123");
		    		}
				}else{
					if(isLoading){
						return;
					}
					if(isPVP){
						InRoom.GetInRoomInstantiate().InGameRoom(LoadingStr);
						ReLian.LoadingStr = LoadingStr;
					}else{
						try{
							AllManage.AllMge.Keys.Clear();
							AllManage.AllMge.Keys.Add(LabelText.text + "\n");
							AllManage.AllMge.Keys.Add("messages136");
							AllManage.AllMge.Keys.Add(LoadingStr + " == " + PhotonNetwork.connected);
							AllManage.AllMge.SetLabelLanguageAsID(LabelText);
							//				LabelText.text += "\n" + "正在加入区域_" + LoadingStr + " == " + PhotonNetwork.connected;

						}catch(e){
							Application.LoadLevel(Application.loadedLevel);
							//print("123123123");
						}
						ReLian.LoadingStr = LoadingStr;
						var boolplus : boolean = false;
//						print("RoomNum == " + PhotonNetwork.GetRoomList().length);
						for(var roomInfo : RoomInfo in PhotonNetwork.GetRoomList()){
//							print(roomInfo.playerCount + " == player" );
							if(roomInfo.name == LoadingStr){
								boolplus = true;
//								print(roomInfo.playerCount + " == " + BtnGameManager.roomPlayerNum + " == " + roomInfo.name);
								if(roomInfo.playerCount >= BtnGameManager.roomPlayerNum){
									LoadingStr = RoomNamePlus(LoadingStr , 1);
//									print(LoadingStr + "-----------");
									boolplus = false;
									while( ! boolplus){
//										print(" --------- ");
										try{
											var yesHaveNoom : boolean = false;
											for(var roomInfo1 : RoomInfo in PhotonNetwork.GetRoomList()){
												if(roomInfo1 && roomInfo1.name == LoadingStr){
													yesHaveNoom = true;
													if(roomInfo1.playerCount >= BtnGameManager.roomPlayerNum){
														LoadingStr = RoomNamePlus(LoadingStr , 1);
													}else{
														boolplus = true;
													}
												}
											}
											if( !yesHaveNoom){
												boolplus = true;
											}
										}catch(e){
											Application.LoadLevel(Application.loadedLevel);
//											print("123123123");
										}
										yield;
									}
//									print("1111111");
									PhotonNetwork.JoinRoom(LoadingStr,true);					
								}else{
//									print("2222222");
									PhotonNetwork.JoinRoom(LoadingStr,true);			
								}
							}
						}
						if( ! boolplus){
//							print("33333333");
							PhotonNetwork.JoinRoom(LoadingStr,true);							
						}
					}
			
				}
}

private var Fstr : String = ";";
function RoomNamePlus(roomName : String , plus : int){
	var strs : String[];
	var useInt : int = 0;
	strs = roomName.Split(Fstr.ToCharArray());
	try{
		strs[1] = (parseInt(strs[1]) + plus).ToString() ;	
	}catch(e){
		strs[1] = "1";
	}
	roomName = strs[0] + ";" + strs[1];
	return roomName;
}

static var returnTimes : int = 0;
function PVPCreate(){
 		if(isLoading){
			return;
		}
	PhotonNetwork.CreateRoom(LoadingStr, true, true, BtnGameManager.roomPlayerNum + 5);
	ReLian.LoadingStr = LoadingStr;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelText.text + "\n");
			AllManage.AllMge.Keys.Add("messages139");
			AllManage.AllMge.Keys.Add(LoadingStr + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelText);
//	LabelText.text += "\n" + "创建副本_" + LoadingStr;
}

function GoMenu(){
//	//print("sdfsdfsdfsdfsdf");
 		if(isLoading){
			return;
		}
							AllResources.ar.AllLoadLevel("Loading 1");
//						Application.LoadLevel("Loading 1");

}

var canLoad : boolean = false;
function PVPJoin(){
	////print("level room +++++111111+++");
	times = 0;
	var bool : boolean = false;
 		if(isLoading){
			return;
		}
	do{
		times += 1;
		for(var roomInfo : RoomInfo in PhotonNetwork.GetRoomList()){
//			//print(LoadingStr + " === " + roomInfo.name);
	 		if(isLoading){
				return;
			}
			if(roomInfo.name == LoadingStr){
				bool = true;
				if(roomInfo.playerCount >= BtnGameManager.roomPlayerNum){
					LoadingStr = RoomNamePlus(LoadingStr , 1);			
							bool = false;
							while( ! bool){
								var yesHaveNoom : boolean = false;
								for(var roomInfo1 : RoomInfo in PhotonNetwork.GetRoomList()){
									if(roomInfo1.name == LoadingStr){
										yesHaveNoom = true;
										if(roomInfo1.playerCount >= BtnGameManager.roomPlayerNum){
											LoadingStr = RoomNamePlus(LoadingStr , 1);
										}else{
											bool = true;
										}
									}
								}
								if( !yesHaveNoom){
									bool = true;
								}
								yield;
							}
					PhotonNetwork.JoinRoom(LoadingStr,true);					
				}else{
					PhotonNetwork.JoinRoom(LoadingStr,true);			
				}
			}
		}
				if( ! bool){
					PhotonNetwork.JoinRoom(LoadingStr,true);							
				}

//		//print(times + " == " + LoadingStr); 
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelText.text + "\n");
			AllManage.AllMge.Keys.Add("messages140");
			AllManage.AllMge.Keys.Add(times + " == " + LoadingStr);
			AllManage.AllMge.SetLabelLanguageAsID(LabelText);
//		LabelText.text += "\n" + "正在等待其他玩家"+times + " == " + LoadingStr;
		yield WaitForSeconds(4);
	}while(times < 10); 
 		if(isLoading){
			return;
		}
	if(!bool){
	    ReLian.LoadingStr = LoadingStr;
		PhotonNetwork.CreateRoom(LoadingStr, true, true, BtnGameManager.roomPlayerNum + 5);
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelText.text + "\n");
			AllManage.AllMge.Keys.Add("messages139");
			AllManage.AllMge.Keys.Add(LoadingStr);
			AllManage.AllMge.SetLabelLanguageAsID(LabelText);
//		LabelText.text += "\n" + "创建副本_" + LoadingStr;
	}else{
		PhotonNetwork.Disconnect(); 
		canLoad = true;
		while(PhotonNetwork.connected || PhotonNetwork.connectionState == ConnectionState.Disconnecting){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelText.text + "\n");
			AllManage.AllMge.Keys.Add("messages134");
			AllManage.AllMge.SetLabelLanguageAsID(LabelText);
//			LabelText.text += "\n" + "正在退出区域_";
			yield;	
		}
 		if(isLoading){
			return;
		}
		loadstr = "";
//		TeamID = ""; 
		nandu = "";
		LoadingStr = "";
		Level = DungeonControl.ReLevel;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages076");
			AllManage.AllMge.Keys.Add(DungeonControl.ReLevel + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelText);
//		LabelText.text = "正在返回区域_" + DungeonControl.ReLevel;
//		print("loading ======= " + this.name);
		newLoading1();
//			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}
}

function OfflineMode(){
	PhotonNetwork.offlineMode = true;
//		  PhotonNetwork.offlineMode_inRoom = true;
//			print("loading ===222==== ");
	AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

}

var isLoading : boolean = false;

function loadScene(){
//	print(AllResources.ar + " == " +AllResources.ar);
	if(AllResources.ar != null){
//		print(canOffLine + " == canOffLine");
		if(canOffLine || (Level != "Login-1" && Level != "Login-2")){
			AllResources.ar.LoadGUI(gameObject);
		}
	}
	oneLoad = true;
	loadTimes += 1;
	//print(loadTimes + " == loadTimes");
		isLoading = true;
		async = Application.LoadLevelAsync(Level);	
		yield async;
}

var LabelLoading : UILabel;
var XueTiao : UIFilledSprite;
var ff : float; 
var button : GameObject;
var ptime : int;
private var useF : float;
private var useF1 : int;
private var useChu : int = 0;
function Update () {
	if(async){
		progress =  async.progress *100;
			if(Level != "Login-2"){
				if(! AllResources.isLoadGUI){
					useF = 0.89;		
					useF1 = 89;								
				}else{
					useF = 0.89;		
					useF1 = 89;			
				}
			}
			else{
				useF1 = 100;
				useF = 1;			
			}
			useChu = progress / useF;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("info719");
			AllManage.AllMge.Keys.Add(useChu + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelLoading);
//		LabelLoading.text ="正在努力加载..." +progress.ToString();
		ff = progress;
		XueTiao.fillAmount = ff/useF1;
//		//print(progress);	
	}
	if(Time.time > ptime ){//&&  ! UIControl.nowIsPVPGO){
		canOffLine = true;
		newLoading1();
	}
}

    // We have two options here: we either joined(by title, list or random) or created a room.
    function OnJoinedRoom()
    {  
//    	print("123123123123");
        MoveToGameScene1();
    }

	function OnCreatedRoom(){
//		print("343434343434");
//    LabelText.text += "\n CreatedRoom+++";	
	
	}
	
//	function OnLeaveRoom(){
//		//print("li kai fang jian ");
// 		if(isLoading){
//			return;
//		}
//	 if(Level == "Login-1"){
//		Application.LoadLevel("Login-1");
//     }else{
////				print("loading ==444===== ");
//		AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

//		}
// 	}
 	
 	var duankai : boolean = false;
 	function OnJoinedLobby(){
 		if(DaTingLoadingStr == ""){
// 			print("loading ====555=== ");
 			yield WaitForSeconds(2);
 				newLoading1();
 				return;
//				AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

 		}
// 		print("shi zhe li a ~????");
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages078");
			AllManage.AllMge.SetLabelLanguageAsID(LabelText);
//		LabelText.text = "已回到大厅_";
         PhotonNetwork.autoCleanUpPlayerObjects = true; 
		times = 0;
		var boolplus : boolean = false;
					do{
						times += 1;
						for(var roomInfo : RoomInfo in PhotonNetwork.GetRoomList()){
							if(roomInfo.name == DaTingLoadingStr){
								//print(DaTingLoadingStr + " == DaTingLoadingStr");
								boolplus = true;
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelText.text + "\n");
			AllManage.AllMge.Keys.Add("messages142");
			AllManage.AllMge.Keys.Add(DaTingLoadingStr + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelText);
//								LabelText.text += "\n 尝试加入区域_" + DaTingLoadingStr;
	   							ReLian.LoadingStr = DaTingLoadingStr;
								if(roomInfo.playerCount >= BtnGameManager.roomPlayerNum){
									DaTingLoadingStr = RoomNamePlus(DaTingLoadingStr , 1);
							boolplus = false;
							while( ! boolplus){
								var yesHaveNoom : boolean = false;
								for(var roomInfo1 : RoomInfo in PhotonNetwork.GetRoomList()){
									if(roomInfo1.name == DaTingLoadingStr){
										yesHaveNoom = true;
										if(roomInfo1.playerCount >= BtnGameManager.roomPlayerNum){
											DaTingLoadingStr = RoomNamePlus(DaTingLoadingStr , 1);
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
									PhotonNetwork.JoinRoom(DaTingLoadingStr,true);					
								}else{
									PhotonNetwork.JoinRoom(DaTingLoadingStr,true);		
								}						
							}
						}
 
						yield WaitForSeconds(3);
					}while(times < 10); 
				if( ! boolplus){
					PhotonNetwork.JoinRoom(LoadingStr,true);							
				}

//                    duankai = true;
//                    PhotonNetwork.Disconnect(); 	
 	} 
	
    function OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from Photon.");
        yield; 
 		if(isLoading){
			return;
		}
        if( !canLoad){
        	if(Level == "Login-1"){
				Application.LoadLevel("Login-1");
     		}else{
				returnTimes += 1;
				if(returnTimes > 2){
					returnTimes = 0;
//				if(DungeonControl.ReLevel && DungeonControl.ReLevel != ""){
//					Level = DungeonControl.ReLevel;				
//				}else{
////					Level = "Map111";
//				}
		canOffLine = true;
		newLoading1();
//					Application.LoadLevel("Login-1");
					return;
				}
//				print("loading ==666===== ");
				AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");
  
			}    
        }

    }

    function OnFailedToConnectToPhoton( parameters : Object)
    {
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters);
    }
    
    function OnPhotonJoinRoomFailed(){
 		if(isLoading){
			return;
		}
		for(var roomInfo : RoomInfo in PhotonNetwork.GetRoomList()){
			if(roomInfo.name == LoadingStr){
				if(roomInfo.playerCount >= BtnGameManager.roomPlayerNum){
					LoadingStr = RoomNamePlus(LoadingStr , 1);
					PhotonNetwork.JoinRoom(LoadingStr,true);
					return;												
				}
			}
		}
	    
	    Debug.Log("JoinRoom Failed from Photon.");
	    LabelText.text += "\n JoinRoom Failed from Photon.";
	    ReLian.LoadingStr = LoadingStr;
		PhotonNetwork.CreateRoom(LoadingStr, true, true, BtnGameManager.roomPlayerNum + 5);
	    yield WaitForSeconds(2 );
 		if(isLoading){
			return;
		}
	    ReLian.LoadingStr = LoadingStr;
//	    PhotonNetwork.JoinRoom(LoadingStr,true);
		times += 1;
		if(times >= 6){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add(LabelText.text + "\n");
			AllManage.AllMge.Keys.Add("messages143");
			AllManage.AllMge.SetLabelLanguageAsID(LabelText);
//			LabelText.text += "\n 连接失败重新尝试";
			duankai = false;
			PhotonNetwork.Disconnect(); 	
		}
    }
	
	function OnPhotonCreateRoomFailed(){
	}

	var oneLoad : boolean = false;
    function MoveToGameScene1()
    {
		
		if(!oneLoad){
	    	do{
	    		if(PhotonNetwork.room != null){
	    		   	PhotonNetwork.isMessageQueueRunning = false;
					loadstr = "";
		//			TeamID = ""; 
					nandu = "";
					LoadingStr = "";
					StartCoroutine(loadScene());
	    		}
	    		yield;
	    	}
	        while(PhotonNetwork.room == null);
			}
    }

