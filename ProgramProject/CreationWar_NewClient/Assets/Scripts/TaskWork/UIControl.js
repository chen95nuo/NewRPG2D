	#pragma strict
class UIControl extends Song {
	static var mapType : MapType;
	var nowmapInstanceID : int = -1;
	var LookMapType : MapType;
	var zhucheng : boolean = false;
	var MainTW : MainTaskWork;
	var PlayerName : String = "";
	var PlayerTitle : String = "";
	var useColor : Color;
	var GameObjSwih : GameObjectSwich;
	var CKBoxLiaoTian : UIToggle;

	var showDeadYield : boolean = false;
	var PreJingji : GameObject;
	private var ObjJingji : GameObject;
	var ParentJingji : Transform;
	var PreDead : GameObject;
	private var ObjDead : GameObject;
	var ParentDead : Transform;
	var ButtonShowPVP : Transform;

	var PreWin : GameObject;
	private var ObjWin : GameObject;
	var ParentWin : Transform;
	var buttonAutoPlay : Transform;
	var buttonAutoPlay1 : Transform;
	var LabelCangZheng : UILabel;
	var ProfessionLabel : UILabel;
	var ObjDontControl : Collider;
	static var isLoadScene : boolean = false;
	var UICam : Camera;
	static var UICamStatic : Camera;
	var ActionCube : GameObject;
	var boolFullBag : boolean = false;
	var ObjState : GameObject ;
	var ObjMagic : GameObject ;
	var ObjSkillBox : GameObject;
	var ObjSkillBoxC : BoxCollider ;
	function Awake(){
		MonsterHandler.GetInstance();
		nowIsPVPGO = false;
		UICamStatic = UICam;
		if(!isLoadScene){
			InRoom.GetInRoomInstantiate().addtili();
			isLoadScene = true;
			//TD_info.setInGameScreen();
		}
		AllManage.UICLStatic = this;
		DungeonControl.DungeonJiSha = 0;
			var str : String = Application.loadedLevelName.Substring(3,1);
//			LabelCangZheng.text = AllManage.AllMge.Loc.Get("info638");
			lookShouZhuButtonAll.transform.localPosition.y = 0;
			ButtonYaoQing.transform.localPosition.x = -3000;
			if(ObjJingji){
				Destroy(ObjJingji);
			}
			if(ObjDead){
				Destroy(ObjDead);
			}
			if(ObjWin){
				Destroy(ObjWin);
			}
			if(str=="1"){
				isYtFuben = false;
				myTeamInfo = "";
				Loading.AgainTimes = 0;
				mapType = MapType.zhucheng;
//				LabelCangZheng.text = AllManage.AllMge.Loc.Get("messages073");
			}else
			if(str=="3" || str=="4"){
				Bottom.transform.localPosition.y = 5000;
				xuepingcdTimes = 60;
				CDtime = 60;
	//			AllManage.UIALLPCStatic.show32();
				lookShouZhuButtonAll.transform.localPosition.y = 3000;
				buttonAutoPlay.position.y = 3000;
				ButtonShowPVP.localPosition.y = -29.91943;	
				mapType = MapType.jingjichang;	
//				if(PreJingji == null)
//			    PreJingji = Resources.Load("Anchor - Arena", GameObject); 			
//				ObjJingji = GameObject.Instantiate( PreJingji );
//				ObjJingji.transform.parent = ParentJingji;
//				ObjJingji.transform.localPosition = Vector3.one;	
//				if(PreDead == null)
//			    PreDead = Resources.Load("Anchor - Dead", GameObject); 	
//				ObjDead = GameObject.Instantiate( PreDead );
//				ObjDead.transform.parent = ParentDead;
//				ObjDead.transform.localPosition = Vector3.one;	 
//				
//			    PreWin = Resources.Load("Anchor - Win", GameObject); 			
//				ObjWin = GameObject.Instantiate( PreWin );
//				ObjWin.transform.parent = ParentWin;
//				ObjWin.transform.localPosition = Vector3.one;	 
//				ObjWin.transform.localScale = Vector3.one;	 
//				ObjWin.SetActiveRecursively(false);
				if(PVP321){
					YesStartPVP();
				}
			}else
			if((str=="7" || str=="8") && Application.loadedLevelName != "Map721"){
				Bottom.transform.localPosition.y = 5000;
				lookShouZhuButtonAll.transform.localPosition.y = 3000;
				buttonAutoPlay.position.y = 3000;	
				myTeamInfo = "";
				mapType = MapType.yewai;
//				if(PreDead == null)
//			    PreDead = Resources.Load("Anchor - Dead", GameObject); 								
//				ObjDead = GameObject.Instantiate( PreDead );
//				ObjDead.transform.parent = ParentDead;
//				ObjDead.transform.localPosition = Vector3.one;	 
//				if(PreWin == null)
//			    PreWin = Resources.Load("Anchor - Win", GameObject); 			
//				ObjWin = GameObject.Instantiate( PreWin );
//				ObjWin.transform.parent = ParentWin;
//				ObjWin.transform.localPosition = Vector3.one;	 
//				ObjWin.transform.localScale = Vector3.one;	 
//				ObjWin.SetActiveRecursively(false);
			}else
			{
				Bottom.transform.localPosition.y = 5000;
				lookShouZhuButtonAll.transform.localPosition.y = 3000;
				buttonAutoPlay.position.y = 3000;	
				buttonAutoPlay1.localPosition.y = 0;	
				mapType = MapType.fuben;
//				if(Application.loadedLevelName != "Map200"){
//					if(PreDead == null)
//				    PreDead = Resources.Load("Anchor - Dead", GameObject); 								
//					ObjDead = GameObject.Instantiate( PreDead );
//					ObjDead.transform.parent = ParentDead;
//					ObjDead.transform.localPosition = Vector3.one;	 

//				}
//				print(PlayerInfo.canInviteGoPVE);
//				print(Application.loadedLevelName);
				if(PlayerInfo.canInviteGoPVE && Application.loadedLevelName != "Map200" && AllManage.AllMge.IsGetPlace("511")){
					ButtonYaoQing.transform.localPosition.x = 0;
				}else{
					ButtonYaoQing.transform.localPosition.x = -3000;
				}
			}
			
			if(Application.loadedLevelName == "Map721"){
				Bottom.transform.localPosition.y = 5000;
				lookShouZhuButtonAll.transform.localPosition.y = 3000;
				buttonAutoPlay.position.y = 3000;	
				buttonAutoPlay1.localPosition.y = 0;	
				mapType = MapType.fuben;
				if(PlayerInfo.canInviteGoPVE && Application.loadedLevelName != "Map200" && AllManage.AllMge.IsGetPlace("511")){
					ButtonYaoQing.transform.localPosition.x = 0;
				}else{
					ButtonYaoQing.transform.localPosition.x = -3000;
				}
			}
			
			if(mapType != MapType.zhucheng){
				Destroy(ActionCube.gameObject);
			}
			LookMapType = mapType;
			showWenHao(true);
			
			if(Application.loadedLevelName=="Map811" || Application.loadedLevelName=="Map812"){
				ObjSkillBox.SetActive(true);
				ObjSkillBoxC.enabled = true;
			}
			
	}
	var WinSource : AudioSource ;
	function MakePreWin(){
		WinSource.enabled = true;
		WinSource.Play();
		if(PreWin == null){
		    PreWin = Resources.Load("Anchor - Win", GameObject); 			
			ObjWin = GameObject.Instantiate( PreWin );
			ObjWin.transform.parent = ParentWin;
			ObjWin.transform.localPosition = Vector3.one;	 
			ObjWin.transform.localScale = Vector3.one;	 
//			ObjWin.SetActiveRecursively(false);	
		}
	}

	function MakePreDead(){
		if(PreDead == null){
		    PreDead = Resources.Load("Anchor - Dead", GameObject); 								
			ObjDead = GameObject.Instantiate( PreDead );
			ObjDead.transform.parent = ParentDead;
			ObjDead.transform.localPosition = Vector3.one;	 
		}
	}

	function MakePreArena(){
		if(PreJingji == null){
		    PreJingji = Resources.Load("Anchor - Arena", GameObject); 			
			ObjJingji = GameObject.Instantiate( PreJingji );
			ObjJingji.transform.parent = ParentJingji;
			ObjJingji.transform.localPosition = Vector3.one;	
		}
	}

	static var teamHeadMapName : String = "";
	function DuiZHangGoFB(){
		if(teamHeadMapName != ""){
			var str : String = teamHeadMapName.Substring(3,1);
			if(str=="1"){
			}else
			if(str=="3" || str=="4"){
			}else
			if(str=="7"){
			}else
			{
//				if(isInTeam){
					InRoom.GetInRoomInstantiate().TeamHeadIn(teamHeadMapName);
//				}
			}
			teamHeadMapName = "";
		}
	}

	private var strGoFB : String;
	private var strGoND : String;
	static var mTeamMapInsId : int = 0;
	private var usemTeamMapInsId : int = 0;
	private var activityID : String = "";
	function DuiYuanGoFB(objs : Object[]){
		var Map : String = "";
		Map = objs[0];
		
		if(Map.Length > 3){
			usemTeamMapInsId = objs[1];
			strGoFB = Map.Substring(0,6);
			strGoND = Map.Substring(6,1);
			if(Map.Length > 7)
			{
				activityID = Map.Substring(7);
			}
			
			var mapID : int = 0;
			mapID = parseInt( Map.Substring(3 , 2));
			if(mapID < 70){
				AllManage.qrStatic.ShowQueRen1(gameObject , "DuiYuanGenYes" , "DuiYuanGenNo" , AllManage.AllMge.Loc.Get("info607") + MainTW.taskMapCont.GetMapName( mapID ));
			}else{
				AllManage.qrStatic.ShowQueRen1(gameObject , "DuiYuanGenYes" , "DuiYuanGenNo" , AllManage.AllMge.Loc.Get("info607") + MainTW.taskMapCont.GetMapName1( parseInt( Map.Substring(4 , 2))));
			}
	//		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info607") + MainTW.taskMapCont.GetMapName(parseInt( Map.Substring(3 , 2) )));	
		}
	}

	function DuiYuanGenYes(){
		if(activityID != "")
		{
			InRoom.GetInRoomInstantiate().JoinActivity(activityID);
		}
		else
		{
			mTeamMapInsId = usemTeamMapInsId;
			Loading.Level = strGoFB;
			Loading.nandu = strGoND;
			DungeonControl.NowMapLevel = parseInt(strGoND);
			if(mapType == MapType.zhucheng)
			DungeonControl.ReLevel = Application.loadedLevelName;
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
			alljoy.DontJump = true;
			yield;
			alljoy.DontJump = true;
			yield;
				PhotonNetwork.LeaveRoom();
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
				AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

		}
	}

	function CZGoLinShiFuBen(objs : Object[]){
		var team : int;
		var map : String;
		var nandu : int;
		var instensid : int;
		team = objs[0];
		map = objs[1];
		nandu = objs[2];
		instensid = objs[3];
		mTeamMapInsId = instensid;
		//                ServerRequest.requestAddToMap()
	//	PhotonNetwork.LeaveRoom();
		Loading.TeamID = team + "fuben";
		Loading.Level = "Map" + map;
		Loading.nandu = nandu.ToString();
		DungeonControl.NowMapLevel = nandu;
		if(UIControl.mapType == MapType.zhucheng)
		DungeonControl.ReLevel = Application.loadedLevelName;
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			PhotonNetwork.LeaveRoom();
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}



	function DuiYuanGenNo(){
		if(mapType == MapType.fuben && DungeonControl.NowMapLevel == 5){
			Loading.Level = DungeonControl.ReLevel;
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
			InRoom.GetInRoomInstantiate().PVP1InviteRemove();			
			InRoom.GetInRoomInstantiate().PVPTeamDissolve();
			AllManage.UICLStatic.RemoveAllTeam();
			if(mapType == MapType.jingjichang){
				InRoom.GetInRoomInstantiate().LeaveRoom(PhotonNetwork.room.name);
				InRoom.GetInRoomInstantiate().ActivityPVPRemove();
			}
			InRoom.GetInRoomInstantiate().RemoveTempTeam();
			alljoy.DontJump = true;
			yield;
			PhotonNetwork.LeaveRoom();
			if(mapType == MapType.jingjichang){
				InRoom.GetInRoomInstantiate().BattlefieldExit();
			}
			InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
				AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");
	
		}
	}

	function ButtonsPVP(){
		if(mapType == MapType.jingjichang){
			if(ArenaControl.areType == 1){
				AllManage.UIALLPCStatic.show13();
				AllManage.areCLStatic.cardCLobj.SetActiveRecursively(true);
			}else
			if(ArenaControl.areType == 2){
				AllManage.UIALLPCStatic.show14();
				AllManage.areCLStatic.cardCLobj.SetActiveRecursively(true);
			}
		}else{
			AllManage.UIALLPCStatic.show20();
		}
	}

	var useMoney : int;
	var useBlood : int;
	var useExp : int;
	private var ps : PlayerStatus;
	function Cost(){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.Cost , useMoney , useBlood , "" , gameObject , "");
//		AllManage.AllMge.UseMoney(useMoney , useBlood , UseMoneyType.Cost , gameObject , "");
	//	ps.UseMoney(useMoney , useBlood);
//		ps.AddExperience(useExp); 
		ps.AddServingMoney(-1*(useBlood/10));
		invcl.ReadVIPLevel();
	//	InRoom.GetInRoomInstantiate().peer.Disconnect();
	}

	static var taskLeixingStrs : String[];
	var teamyt : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("PlayerTeam","PlayerID");
	var teamyt1 : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("PlayerTeamssss","PlayerID");
	var ButtonTopRight : Transform;
	var ButtonYaoQing : GameObject;
	private var tmcl : TMonsterControl;
	var TransKuaiJie : Transform;
	var ButtonTiaoGuo : Transform;
	function Start(){
//		print(InRoom.GetInRoomInstantiate().serverTime);
		if(isPGetOutOf){
			AllManage.tsStatic.Show("info1014");
			isPGetOutOf = false;
		}
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = true;
			if(mapType == MapType.jingjichang)
				MakePreArena();
		if(buttonsZC.isOn){
			buttonsZC.buttonsOff();
		}
//		print("sd;kf;skf;dlkf;sdkf;sdkf uicl");
		if(Application.loadedLevelName == "Map271" || Application.loadedLevelName == "Map272"){
			tmcl = FindObjectOfType(TMonsterControl);
		}
		TransTeamParentSay.localPosition.y = 3000;
		if(mapType == MapType.zhucheng || Application.loadedLevelName == "Map200"){
			TransKuaiJie.localScale = Vector3(0,0,0);
		}else{
			TransKuaiJie.localScale = Vector3(1,1,1);	
		}
	//	//print("player == " + PhotonNetwork.player);
				Loading.loadstr = "";
				Loading.LoadingStr = "";
		if(Application.loadedLevelName == "Map200"){
			ButtonTopRight.localPosition.y = 3000;
		}else{
			ButtonTiaoGuo.gameObject.SetActive(false);
		}
	StartPoint = GameObject.FindWithTag("spawn").transform;
		if(taskLeixingStrs == null){
			taskLeixingStrs = new Array(6);
			taskLeixingStrs[0] = AllManage.AllMge.Loc.Get("info601");
			taskLeixingStrs[1] = AllManage.AllMge.Loc.Get("info602");
			taskLeixingStrs[2] = AllManage.AllMge.Loc.Get("info603");
			taskLeixingStrs[3] = AllManage.AllMge.Loc.Get("info604");
			taskLeixingStrs[4] = AllManage.AllMge.Loc.Get("info605");
			taskLeixingStrs[5] = AllManage.AllMge.Loc.Get("info606");
		}
	//	//print("chong xin du le chang jing 1111111111111111111111111111111111111111111111111111111111111111111111" + Application.loadedLevelName);
			if(mapType == MapType.zhucheng){
		    	ButtonZhuP.enabled=true;
				ButtonZhu.localPosition.y = 0;
				buttonJinengP.enabled=false;
				ButtonJineng.localPosition.y = 1000;
			}else{
			    ButtonZhuP.enabled=false;
				ButtonZhu.localPosition.y = -1000;
				buttonJinengP.enabled=true;
				ButtonJineng.localPosition.y = 0;		
			}


//		PosStores[0] = TransStores[0].position.x;
//		PosStores[1] = TransStores[1].position.x;
//		PosStores[2] = TransStores[2].position.x;
//		PosStores[3] = TransStores[3].position.x;
		st = new yuan.YuanTimeSpan(); 
		TuoLi = false;

	////////	
		var useSpStr : String;
			if(PhotonNetwork.room){
					useSpStr = PhotonNetwork.room.customProperties[OpenDoor];
			}else{
				useSpStr = null;
			}
			if(DungeonControl.staticRoomSP != null && useSpStr == null){
				useSpStr = DungeonControl.staticRoomSP[OpenDoor];
			}
			if(useSpStr != null && useSpStr == "1"){
				OpenDoorNow();
			}else{
				try{
					DungeonControl.staticRoomSP.Add(OpenDoor, "0");			
				}catch(e){			
				}
			}
		if(PhotonNetwork.connected && PhotonNetwork.isMasterClient)
		PhotonNetwork.room.SetCustomProperties(DungeonControl.staticRoomSP);
			yield WaitForSeconds(1);
	////////	
		yield WaitForSeconds(1);
		if(mapType == MapType.fuben){
	//		//print("sld;jflsdjfldjsflsdkjflsdkjflskdjflksdjflsdkjflsdkjflsdkjflsdjkflsdkjf");
			CKBoxLiaoTian.isChecked = false;
//			GameObjSwih.targe.SetActiveRecursively(false);
	//		GameObjSwih.gameObject.SendMessage("OnClick",SendMessageOptions.DontRequireReceiver);
		}
//		CloseLiaoTian();
		
	//		//print(PlayerStatus.MainCharacter);
			while( PlayerStatus.MainCharacter == null){
	//		//print(PlayerStatus.MainCharacter);
				yield;
			}
	//		//print(mapType);
			if(mapType == MapType.fuben){
	//		//print(isYtFuben);
				if(isYtFuben){
					var obj : GameObject;
					obj = AllResources.ar.CreatePlayerFuBen(parseInt(ytFuben.Rows[0]["ProID"].YuanColumnText) , myTeamInfo , ytFuben.Rows[0]["PlayerName"].YuanColumnText , 	PlayerStatus.MainCharacter.position);
				//	obj.SendMessage("SetIsMine" , false , SendMessageOptions.DontRequireReceiver);
				    obj.SendMessage("SetMaster",PlayerStatus.MainCharacter, SendMessageOptions.DontRequireReceiver);
					obj.SendMessage("SetPlayerInfoAsYt" , ytFuben , SendMessageOptions.DontRequireReceiver);
				}
			}
			
		var mm :  boolean = false;
	  while(!PlayerStatus.MainCharacter){
	       yield;
	     }
	     yield;
		if(mapType == MapType.fuben){
			buttonAutoPlay.position.y = 3000;	
			buttonAutoPlay1.localPosition.y = 0;
			if(Application.loadedLevelName != "Map200"){
				//TD_info.setInGameInstance(DungeonControl.MapName);			
			}else{
				//TD_info.trainingScreeen();
			}
		}
	     if(ps == null && PlayerStatus.MainCharacter)
		 ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			if(mapType == MapType.jingjichang){
				invcl.VipYaoPing = 0;
				SpriteXuePing.spriteName = "UIP_Bloodstone";
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages114");
				AllManage.AllMge.SetLabelLanguageAsID(LabelXuePing);
	//			LabelXuePing.text = "补满";
			}

		if ((Application.loadedLevelName.Substring(3, 1) == "3" || Application.loadedLevelName.Substring(3, 1) == "4") && Application.loadedLevelName != "Map321"){
//			print(BattlefieldOpenDoorTime + " ===== " + System.DateTime.Now);
			if(InRoom.GetInRoomInstantiate().serverTime >= System.DateTime.Parse(BattlefieldOpenDoorTime))
			{
				OpenDoorNow();
			}
	     }
		if (Opendoor &&(Application.loadedLevelName.Substring(3, 1) == "3" || Application.loadedLevelName.Substring(3, 1) == "4")){
	     OpenDoorNow();
	     Opendoor = false;
	     }
	     
	     
			for(var i=0; i<UITeam.length; i++){ 
				TeamGround.active = false;
				UITeam[i].SetActiveRecursively(false);
			}
		if(mapType == MapType.zhucheng){
			var times : int = 0;
			while(!mm && teamyt.Count == 0 && times < 2){ 
				yield WaitForSeconds(3);
				times += 1;
				if(! teamyt.IsUpdate){
					InRoom.GetInRoomInstantiate().GetMyTeams(teamyt , "DarkSword2" , "PlayerInfo");			
				}
				if(teamyt.Count>0){
					ShowTeam(teamyt);
					if(teamyt.Count >= 4){
						ButtonYaoQing.transform.localPosition.x = -3000;
	//					ButtonYaoQing.SetActiveRecursively(false);					
					}
				}
			}
	//		SetTeamList("");
		}else{
	//		SetTeamList("");
		}
//			ShowLiaoTian();
//			yield;
//			CloseLiaoTian();
//			yuanInputCtrl.SetActiveRecursively(false);
			ScroBvCtrl.SetActiveRecursively(false);
		if(Application.loadedLevelName == "Map212" && !AllManage.InvclStatic.CanMapManaged("2121")){
			AllManage.dungclStatic.AttackGuide();
		}
		if(Application.loadedLevelName == "Map721")
			TrappedtowerCallMonsterAsNum(towerNum);
	}

	function OpenYaoQing(){
	if(canOpenViewAsTaskID()){
		MainTweenMove();
		yield;
		yield;
		yield;
		yield;
		MoveDaoHang4();
	}
	}

	var buttonsActive : ButtonsActive[];
	var buttonsGroundActive : ButtonsActive[];
	static var oldLevel : int;  
	static var oldTsk : String[];
	var lookShouZhuButtonAll : GameObject ;
	var lookShouZhuGroundButtonAll : GameObject ;
	var ButtonsShowZhu : GameObject[];
	var ButtonsGroundShowZhu : GameObject[];
	var startButtonTime : int;
	var ButtonsTypeShowZhu : yuan.YuanPhoton.BenefitsType[];
	var ButtonsTypeAllGroundShowZhu : yuan.YuanPhoton.BenefitsType[];
	var BoolButtonWasOpen : boolean[];
	function StartlookShowZhuButtons(rtime : int){
		var times : int = 0;
		times = rtime;
		var i : int = 0;
		var bool : boolean = false;
		for(i=1; i<ButtonsShowZhu.length; i++){
			bool = GetRealButtonsActiveAsID(i , oldLevel , oldTsk , buttonsActive);
			if(!bool || InRoom.GetInRoomInstantiate().GetServerSwitchString(ButtonsTypeShowZhu[i]) == "0"){
				ButtonsShowZhu[i].SetActiveRecursively(false);
				BoolButtonWasOpen[i-1] = false;
			}else{
				BoolButtonWasOpen[i-1] = true;
			}
		} 
		
		for(i=1; i<ButtonsGroundShowZhu.length; i++){
			bool = GetRealButtonsActiveAsID(i , oldLevel , oldTsk , buttonsGroundActive);
			if(!bool || InRoom.GetInRoomInstantiate().GetServerSwitchString(ButtonsTypeAllGroundShowZhu[i]) == "0"){
				ButtonsGroundShowZhu[i].SetActiveRecursively(false);
				if(i == 1){
					BoolButtonWasOpen[6] = false;
				}
			}else{
				if(i == 1){
					BoolButtonWasOpen[6] = true;
				}		
			}
		}
//		print("StartlookShowZhuButtons ---- Close == " + rtime);
		yield WaitForSeconds(2);
		if(times == startButtonTime && mapType == MapType.zhucheng){
			if(lookShouZhuButtonAll.active){
			while(!PlayerStatus.MainCharacter){
	             yield;
	             }
				if(MainPlayerS == null){
					MainPlayerS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
				}
				for(i=1; i<ButtonsShowZhu.length; i++){
					bool = GetRealButtonsActiveAsID(i ,  parseInt(AllManage.psStatic.Level) , MainPlayerS.player.doneTaskID , buttonsActive);
					if(bool){
	//					//print(ButtonsShowZhu[i].active + " == " + i);
						if(! ButtonsShowZhu[i].active && InRoom.GetInRoomInstantiate().GetServerSwitchString(ButtonsTypeShowZhu[i]) == "1"){
							ButtonsShowZhu[i].SetActiveRecursively(true);
							AllManage.UIALLPCStatic.showExhibition37();
							yield;
							yield;
							AllManage.exhbtControl.OpenMe(BoolButtonWasOpen , i-1);

							if(ButtonsShowZhu[i].GetComponentInChildren(ParticleEmitter))
							ButtonsShowZhu[i].GetComponentInChildren(ParticleEmitter).emit=true;
							yield WaitForSeconds(0.1); 
							if(ButtonsShowZhu[i].GetComponentInChildren(ParticleEmitter))
							ButtonsShowZhu[i].GetComponentInChildren(ParticleEmitter).emit=false;
						}
					}
				}
				oldTsk = MainPlayerS.player.doneTaskID;
				oldLevel = parseInt(AllManage.psStatic.Level);
			}
			if(lookShouZhuGroundButtonAll.active){
			  while(!PlayerStatus.MainCharacter){
	                yield;
	               }
	               yield;
				if(MainPlayerS == null){
					MainPlayerS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
				}
				for(i=1; i<ButtonsGroundShowZhu.length; i++){
					bool = GetRealButtonsActiveAsID(i ,  parseInt(AllManage.psStatic.Level) , MainPlayerS.player.doneTaskID , buttonsGroundActive);
					if(bool){
	//					//print(ButtonsGroundShowZhu[i].active + " == " + i);
						if(! ButtonsGroundShowZhu[i].active && InRoom.GetInRoomInstantiate().GetServerSwitchString(ButtonsTypeAllGroundShowZhu[i]) == "1"){
							ButtonsGroundShowZhu[i].SetActiveRecursively(true);
							if(i == 1){
								AllManage.UIALLPCStatic.showExhibition37();
								yield;
								yield;
								AllManage.exhbtControl.OpenMe(BoolButtonWasOpen , 6);							
							}	
	//						ButtonsGroundShowZhu[i].GetComponentInChildren(ParticleEmitter).emit=true;
	//						yield WaitForSeconds(0.1); 
	//						ButtonsGroundShowZhu[i].GetComponentInChildren(ParticleEmitter).emit=false;
						}
					}
				}
				oldTsk = MainPlayerS.player.doneTaskID;
				oldLevel = parseInt(AllManage.psStatic.Level);
			}
		}
	}

	function UpDateLevelShowZhuButtons(){
		startButtonTime += 1;
		StartlookShowZhuButtons(startButtonTime);
	}

	var buttonsZC : ButtonsZhuControl;
	function lookCanShowRealButtonAsID(takid : String){
//		print("123123123123123123123");
		yield;
		yield;
		yield;
		var i : int = 0;
		for(i=0; i<buttonsActive.length; i++){ 
			if(buttonsActive[i].attr2 == takid && takid != "126" && takid != "62"){
	//			if(! buttonsZC.AllButtons.active){
				if(! buttonsZC.isOn){
					buttonsZC.buttonsOn();
				}
			}
		}
	}

	function lookCanShowRealButtonAsIDActive(takid : String){
		yield;
		yield;
		yield;
		var i : int = 0;
		for(i=0; i<buttonsActive.length; i++){ 
			if(buttonsActive[i].attr2 == takid && takid != "126" && takid != "62"){
				if(! buttonsZC.AllButtons.active && buttonsZC.active){
					buttonsZC.buttonsOn();
				}
			}
		}
	}

	function GetRealButtonsActiveAsID(id : int , lv : int , tsk : String[] , bat : ButtonsActive[]) : boolean{
		var bool : boolean = false;
		var i : int = 0;
		switch(bat[id].activeType){
			case ActiveType.level: 
				if(lv >= bat[id].attr1){
					bool = true;
					return bool;
				}
				break;
			case ActiveType.task: 
				if(tsk != null){
					for(i=0; i<tsk.length; i++){ 
						if(tsk[i] == bat[id].attr2){
							bool = true;
							return bool;
						}
					}		
				}
				break;
		}
		return bool;
	}


	function GetRealButtonsID(level : int) : int{
		if(level < 6){
			return 0;
		}else
		if(level < 10){
			return 1;
		}else
		if(level < 12){
			return 2;
		}else
		if(level < 20){
			return 3;
		}else
		if(level < 30){
			return 4;
		}else
		{
			return 5;
		}
	}

	static var Opendoor = false;
	var TeamGround : GameObject;

	function TeamTest(){
		var mm :  boolean = false;
	//				//print("123123123123123123123");
				if(! teamyt1.IsUpdate){
	//				//print("sdlfjlsdkjlskdjflsdjflsdjflsdjklsdkfj");
					InRoom.GetInRoomInstantiate().GetMyTeams(teamyt1 , "DarkSword2" , "PlayerInfo");	
				}
			while(!mm){ 
	//			//print(Time.time);
				if(teamyt1.Count>0){
					mm = true; 
					ShowTeam(teamyt1);
					if(teamyt1.Count >= 4){
						ButtonYaoQing.transform.localPosition.x = -3000;
	//					ButtonYaoQing.SetActiveRecursively(false);					
					}
				}
				yield;
			}
	}

	var TuoLi : boolean = false;
	var st : yuan.YuanTimeSpan;
	var StartPoint : Transform;
	var SpriteOutOfCD : UISprite;
	function UseTuoLi(){
		if(Application.loadedLevelName == "Map200")
			return;
			
		if(!TuoLi){
			st.TimeStart(InRoom.GetInRoomInstantiate().serverTime); 	
			PlayerStatus.MainCharacter.position = StartPoint.position;
			TuoLi = true;
		}else{
			var stime : int = st.TimeEndtoSeconds(InRoom.GetInRoomInstantiate().serverTime);
			if(stime / 60 > 60){
				PlayerStatus.MainCharacter.position = StartPoint.position;
				TuoLi = true;
				st.TimeStart(InRoom.GetInRoomInstantiate().serverTime); 	
			}else{
				AllManage.tsStatic.Show(AllManage.AllMge.Loc.Get("info1227")+StrTim);
			}
		}
	}
	
	function NowDivorced(){
		PlayerStatus.MainCharacter.position = StartPoint.position;
	}
	var StrTim : String ;
	var  hours : int ;
	var minutes : int ;
	var seconds : int  ;
	function UseOutOf (){
		if(TuoLi){
		SpriteOutOfCD.enabled = true;
		SpriteOutOfCD.fillAmount = 1-(parseFloat(st.TimeEndtoSeconds(InRoom.GetInRoomInstantiate().serverTime)))/3600;
		
		StrTim = (3600-parseFloat(st.TimeEndtoSeconds(InRoom.GetInRoomInstantiate().serverTime))).ToString() ;
		
		hours = parseInt((parseInt(StrTim) / (60 * 60)));
		minutes = parseInt(StrTim) % (60 * 60) / 60;
		seconds = parseInt(StrTim) % (60 * 60) % 60;
			if(parseInt(StrTim)>=0){
			StrTim = String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
			}
//		Debug.Log("JR.........................................."+(1-((parseFloat(st.TimeEndtoSeconds(InRoom.GetInRoomInstantiate().serverTime)))/3600)).ToString());
		}
	}

	var isZhao : boolean =false;
	var isInTeam : boolean = false;
	function ShowTeam(reTable : yuan.YuanMemoryDB.YuanTable){
		if(mapType == MapType.jingjichang || mapType == MapType.fuben){
			return;
		}
		if(PlayerStatus.MainCharacter != null){ 
			var PlayerName : String; 
			isZhao = true;
			PlayerName = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus).PlayerName;
	//		InRoom.GetInRoomInstantiate().GetMyTeams(yt , "DarkSword2" , "PlayerInfo");
	//		while(yt.IsUpdate){
	//			//print(yt.IsUpdate);
	//			yield;
	//		}
			isZhao = false;
			for(var i=0; i<UITeam.length; i++){ 
				TeamGround.active = false;
				UITeam[i].SetActiveRecursively(false);
			}
	//		//print(reTable.Count + " == yt.Count == " + PlayerName);
			isInTeam = false;
			if(reTable.Count > 0){
					TeamGround.active = true;
				for(var row : yuan.YuanMemoryDB.YuanRow in reTable.Rows){
	//				//print(row["ProID"].YuanColumnText + " == " + proid.ToString());
					if(row["PlayerName"].YuanColumnText != PlayerName.ToString()){
						AddNewTeamPlayer(row["PlayerName"].YuanColumnText , parseInt(row["ProID"].YuanColumnText) , parseInt(row["PlayerLevel"].YuanColumnText) , row["PlayerID"].YuanColumnText);
	//					//print(row["PlayerName"].YuanColumnText  + " == PlayerName "  );
	//					//print(row["ProID"].YuanColumnText  + " == ProID "  );
	//					//print(row["PlayerLevel"].YuanColumnText  + " == PlayerLevel "  );
					}else{
						
					}
				}
				if(reTable.Count >= 4){
					ButtonYaoQing.transform.localPosition.x = -3000;
	//				ButtonYaoQing.SetActiveRecursively(false);					
				}
			}else{
				SpriteMyTeamHeadID.enabled = false;
				DesTeamID();
			}
		}
	}

	function ShowTeamID(teamID : String){
			if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}
		try{
			ps.SetTeamID(parseInt(teamID));
			Loading.TeamID = teamID;
		}catch(e){
			ps.SetTeamID(-1);
			Loading.TeamID = "-1";
		}
	}

	function DesTeamID(){
		ps.SetTeamID(-1);
		Loading.TeamID = "";
	}

	var MyTeamHeadID : String;
	var SpriteMyTeamHeadID : UISprite;
	function ShowTeamHeadID(teamHeadID : String){
		MyTeamHeadID = teamHeadID;
		if(InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText == MyTeamHeadID){
			SpriteMyTeamHeadID.enabled = true;
		}
	}

	function SelectTeamMe(){
		if(PlayerInfo.canInviteGoPVE){
			if(ps == null && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}
			SelectTeamPlayerAsID(ps.PlayerID.ToString());
		}
	}

	var TeamAllObj : GameObject;
	var TeamHeadObj : GameObject[];
	var OnSelectTeamPlayerID : String;
	function SelectTeamPlayerAsID(id : String){
		yield;
		OnSelectTeamPlayerID = id;
		TeamAllObj.SetActiveRecursively(true);
		if(InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText != MyTeamHeadID){
			TeamHeadObj[0].SetActiveRecursively(false);
			TeamHeadObj[1].SetActiveRecursively(false);
			TeamHeadObj[2].SetActiveRecursively(false);
		}else{
			if(OnSelectTeamPlayerID == MyTeamHeadID){
				TeamHeadObj[0].SetActiveRecursively(false);
				TeamHeadObj[1].SetActiveRecursively(false);
			}
			TeamHeadObj[3].SetActiveRecursively(false);	
		}
	}

	function TeamZhuanYi(){
	//	//print("111");
		InRoom.GetInRoomInstantiate().TeamPlayerUp(OnSelectTeamPlayerID);
	}
	function TeamTiChu(){
	//	//print("222");
		InRoom.GetInRoomInstantiate().TeamRemove(OnSelectTeamPlayerID);
	}
	function TeamTuiChu(){
	//	//print("333");
		InRoom.GetInRoomInstantiate().TeamRemove(InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText);
	}

	function TeamJieSan(){
	//	//print("444");
		InRoom.GetInRoomInstantiate().TeamDissolve();
	}

	var UITeam : GameObject[];
	function AddNewTeamPlayer(name : String , Pessn : int , level : int , playerID : String){
		for(var i=0; i<UITeam.length; i++){
			if( !UITeam[i].active){
				UITeam[i].SetActiveRecursively(true);
				UITeam[i].GetComponent(TeamItem).AddNewTeamPlayer(name , Pessn , level ,FindPlayerAsName(name) , playerID);
				isInTeam = true;
				return;
			}
		}
	}

	function FindPlayerAsName(name : String) : PlayerStatus{
		var players : PlayerStatus[];
		players = FindObjectsOfType(PlayerStatus);
		for(var ps : PlayerStatus in players){
			if(ps.PlayerName == name){
				return ps;
			}
		}
		return null;
	}

	private var ptime : int;
	private var ServerConnected : boolean = false;
	//var qr : QueRen;
	var SpriteXunLianGuang : UISprite;
	var ttime : float = 0;
	var LabelTMHealth : UILabel;
	var tmTime : float = 0;
	var xuepingcdTimes : int = 3;
	private var DoorTime : int = 0;
	private var TGTime : float = 0;
	function Update () {
			UseOutOf();
	//	if(Time.time > tmTime){
	//		tmTime = Time.time + 1;
	//		SetTeamList("");	
	//	}
	//	//print(InRoom.GetInRoomInstantiate().serverTime.ToShortTimeString());
	//	if(Input.GetButtonUp ("Fire1")) {  
	//		TransWenhao[1].localPosition.y = 1000;	
	//		TransWenhao[0].localPosition.y = 1000;	 
	//		CloseWenHao();
	//		TransWenhao[1].gameObject.SetActiveRecursively(false);
	//		TransWenhao[0].gameObject.SetActiveRecursively(false);
	//	}
	//	if(!InRoom.GetInRoomInstantiate().ServerConnected && !ServerConnected){
	//		ServerConnected = true;
	//		qr.ShowQueRen(gameObject , "ReturnMainSecence" , "已断开连接");
	//	}
//		if(canOpenNPCTaskGO && Time.time > TGTime){
//			TGTime = Time.time + 0.5;
//			OneNPCTalkNoClick();
//		}
		if(UIControl.mapType == MapType.jingjichang && ! InRoom.GetInRoomInstantiate().ServerConnected){
			DisconnectZhanChang();
		}
	
		if(tmcl && Time.time > ttime){
			ttime = Time.time + 0.5;
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages115");
				AllManage.AllMge.Keys.Add(tmcl.Health + "");
				AllManage.AllMge.SetLabelLanguageAsID(LabelTMHealth);
	//		LabelTMHealth.text = "水晶血量:" + tmcl.Health;
		}
		if(Input.GetButtonUp("Fire1")){
			TeamAllObj.SetActiveRecursively(false);
			TaskGroundBack(); 
			invcl.CloseLiangKuang();
		}
		if(Time.time > ptime){
			ptime = Time.time + 1;
			if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			} 
			if(ps != null){
				if(ps.NonPoint >= 12){
					if(!SpriteXunLianGuang.enabled)
					SpriteXunLianGuang.enabled = true;
				}else
				if(SpriteXunLianGuang.enabled){
					SpriteXunLianGuang.enabled = false;
				}
			}
			if(! DoorWasOpen && mapType == MapType.jingjichang && Application.loadedLevelName != "Map321"){
//				print(BattlefieldOpenDoorTime + " ===== " + System.DateTime.Now);
				
				if(InRoom.GetInRoomInstantiate().serverTime >= System.DateTime.Parse(BattlefieldOpenDoorTime)){
					OpenDoor();
				}else{
					DoorTime = parseInt((System.DateTime.Parse(BattlefieldOpenDoorTime) - InRoom.GetInRoomInstantiate().serverTime).TotalSeconds);
					if(ArenaControl.areType != ArenaType.juedou){
						AllManage.tsStatic.Show1(DoorTime + AllManage.AllMge.Loc.Get("info852")); 					
					}
				}
			}
		}
		
//		if(Time.time > ctime){
//			if(AllManage.dungclStatic.readedRoomSP && PhotonNetwork.connected && !PhotonNetwork.isMasterClient && !PhotonNetwork.offlineMode){
//				CopySP();
//				ctime = Time.time + 3;
//	//			DungeonControl.staticRoomSP = PhotonNetwork.room.customProperties;	
//			}
//		}
		
			if(CDtime < xuepingcdTimes){
				CDtime += Time.deltaTime;
				SpriteYaoCD.fillAmount = (xuepingcdTimes - CDtime) / xuepingcdTimes;
				if(CDtime > xuepingcdTimes){
					CDtime = xuepingcdTimes;
				}
			}
			
			ShowXiePing();
			
			
			if(mapType == MapType.zhucheng){
			ObjState.transform.localPosition.y = 0;
			}else{
			ObjState.transform.localPosition.y = 3000;
			}
			
			if(mapType == MapType.zhucheng){
			ObjMagic.transform.localPosition.y = 270;
			}else{
			ObjMagic.transform.localPosition.y = 3000;
			}
			
			
//			ShowBoom();
	//	//print(InRoom.GetInRoomInstantiate().ServerConnected + " ====");
	}

	var PCDTime : float = 3;
	var CDtime : float = 3;
	var SpriteYaoCD : UIFilledSprite;
	var UpXuePing : UpdateXuePing;
	function UseYaoPing(){
		if(mapType == MapType.zhucheng){
	//		if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
	//			AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"UpLevelYaoShui" , "" , AllManage.AllMge.Loc.Get("info298")+ ""+ invcl.VipYaoPingLevel * 10 +AllManage.AllMge.Loc.Get("info307")+ "");
	//		else
	//			UpLevelYaoShui();
			//TD_info.panelStatistics("血瓶");
			UpXuePing.showView();
		}else{
			
			if(parseInt(AllManage.psStatic.Health) >= parseInt(AllManage.psStatic.Maxhealth)){
				AllManage.tsStatic.Show("info753");
			}else{	
				UseYaoPingAsNum(1 , Mathf.Clamp(invcl.VipYaoPingLevel , 1, 10));
				yield WaitForSeconds(3f);
				ISbool = false;
			}
		}
	}

	//var ts : TiShi;
	var SpriteYaoShui : UISprite;
	function UpLevelYaoShui(){
		if(invcl.VipYaoPingLevel < 9){
			if(ps == null && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			} 
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.UpLevelBottle , invcl.VipYaoPingLevel  , 0 , "" , gameObject , "realUpLevelYaoShui");
//			AllManage.AllMge.UseMoney(0 , invcl.VipYaoPingLevel * 10 , UseMoneyType.UpLevelBottle , gameObject , "realUpLevelYaoShui");
	//		if(ps.UseMoney(0 , )){
	//		}
		}else{
			AllManage.tsStatic.Show("tips082");
		}
	}

	function realUpLevelYaoShui(){
				invcl.VipYaoPingLevel += 1;
				invcl.yt.Rows[0]["SolutionLevel"].YuanColumnText = invcl.VipYaoPingLevel.ToString(); 
//				print(InRoom.GetInRoomInstantiate().serverTime.Day + " == yao ping level");
				invcl.yt.Rows[0]["SolutionTime"].YuanColumnText = InRoom.GetInRoomInstantiate().serverTime.ToString(); 
	//			ts.Show("当前药水等级为"+invcl.VipYaoPingLevel);	
				ShowYaoShuiIcon(invcl.VipYaoPingLevel);

	}
	
	function ShowXiePing(){
				if(mapType == MapType.zhucheng){
					if(invcl.VipYaoPing <= 0){
						ShowYaoShuiIcon(invcl.VipYaoPingLevel);
						if(LabelXuePing.text != AllManage.AllMge.Loc.Get("info1035")){
							LabelXuePing.text = AllManage.AllMge.Loc.Get("info1035");						
						}
					}
				}
	}

	function ShowYaoShuiIcon(lv : int){
		if(SpriteYaoShui.spriteName != "YaoShui0" + lv){
			SpriteYaoShui.spriteName = "YaoShui0" + lv;		
		}
	}


	var invcl : InventoryControl;
	var SpriteXuePing : UISprite;
	var LabelXuePing : UILabel;
	var dungcl : DungeonControl;
	var ISbool : boolean = false;
	var useBloodXue : int = 1;
	function UseYaoPingAsNum(i : int , level : int){
			if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			} 
		if(CDtime < xuepingcdTimes || ps.dead || parseInt(ps.Health) <= 0){
			AllManage.tsStatic.Show("tips083");
			return;
		}
		if(invcl.VipYaoPing > 0){
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.BloodSolution , "");
			invcl.VipYaoPing -= i;
			AllManage.InvclStatic.yt.Rows[0]["SolutionNum"].YuanColumnText = invcl.VipYaoPing.ToString();
			LabelXuePing.text = invcl.VipYaoPing.ToString()  + "/" + AllManage.InvclStatic.maxXuePingNum;
			if(PlayerStatus.MainCharacter != null){
				dungcl.OneXuePing();
				PlayerStatus.MainCharacter.gameObject.SendMessage("UseXuePing",level,SendMessageOptions.DontRequireReceiver);
			}
			if(invcl.VipYaoPing <= 0){
				SpriteXuePing.spriteName = "UIP_Bloodstone";
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages114");
				AllManage.AllMge.SetLabelLanguageAsID(LabelXuePing);
	//			LabelXuePing.text = "补满";
			}
			CDtime = 0;
		}else{
			var useInt : int = 0;
			if(PlayerPrefs.GetInt("ConsumerTip" , 1) == 1){
				if(parseInt(ps.Level.Substring(0,1)) < 10){
					useInt = 2;
				}else{
					useInt = parseInt(ps.Level.Substring(0,1))*2;
				}
				if(!ISbool){
//				AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"FullYaoShui" , "FullYaoShuiNo" , AllManage.AllMge.Loc.Get("info298")+""+ (useBloodXue * 10) +AllManage.AllMge.Loc.Get("info308")+"");			
				
				    //AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.FullBottle , useBloodXue , 0 , "" , gameObject , "FullYaoShuiTips");

				    AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"FullYaoShui" , "FullYaoShuiNo" , AllManage.AllMge.Loc.Get("info298")+""+ (Mathf.Clamp(useBloodXue, 1, 10) * BtnGameManager.dicClientParms["FubenCostStone"]) +AllManage.AllMge.Loc.Get("info308")+"");
				    ISbool = true;
				}
			}
			else{
				CDtime = 0;
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.BloodSolution , "");
				FullYaoShui();		
			}
		}
	}
	
	function FullYaoShuiTips(objs : Object[]){
		AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"FullYaoShui" , "FullYaoShuiNo" , AllManage.AllMge.Loc.Get("info298")+""+ objs[2] +AllManage.AllMge.Loc.Get("info308")+"");			
	}	
	function UseYaoPingAsNum1(i : int , level : int){
			if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			} 
	////	if(ps.dead){
	////		return;
	////	}
	//	if(invcl.VipYaoPing > 0){
	//		invcl.VipYaoPing -= i;
	//		AllManage.InvclStatic.yt.Rows[0]["SolutionNum"].YuanColumnText = invcl.VipYaoPing.ToString();
			LabelXuePing.text = invcl.VipYaoPing.ToString()  + "/" + AllManage.InvclStatic.maxXuePingNum;
			if(PlayerStatus.MainCharacter != null){
				PlayerStatus.MainCharacter.gameObject.SendMessage("HealthFull",SendMessageOptions.DontRequireReceiver);
			}
			if(invcl.VipYaoPing <= 0){
				SpriteXuePing.spriteName = "UIP_Bloodstone";
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages114");
				AllManage.AllMge.SetLabelLanguageAsID(LabelXuePing);
	//			LabelXuePing.text = "补满";
			}
	//	}else{
	//		PlayerStatus.MainCharacter.gameObject.SendMessage("HealthFull",SendMessageOptions.DontRequireReceiver);
	//	}
	}
	
	function FullYaoShuiNo(){
			yield WaitForSeconds(3f);
			ISbool = false;
	}

	function FullYaoShui(){
			if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			} 
			var useInt : int = 0;
				if(parseInt(ps.Level.Substring(0,1)) < 10){
					useInt = 2;
				}else{
					useInt = parseInt(ps.Level.Substring(0,1))*2;
				}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.FullBottle , useBloodXue , 0 , "" , gameObject , "realFullYaoShui");
//			AllManage.AllMge.UseMoney(0 , useInt , UseMoneyType.FullBottle , gameObject , "realFullYaoShui");
	//		if(ps.UseMoney(0 , useInt)){
	////			invcl.VipYaoPing = 10;
	////			LabelXuePing.text = invcl.VipYaoPing.ToString();
	//			
	//		}
			yield WaitForSeconds(3f);
			ISbool = false;
	}

	function realFullYaoShui(){
		if(PlayerStatus.MainCharacter != null){
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.BloodSolution , "");
			dungcl.OneXuePing();
			CDtime = 0;
			PlayerStatus.MainCharacter.gameObject.SendMessage("HealthFull",SendMessageOptions.DontRequireReceiver);
			useBloodXue += 1;
		}
	}

	private var costGold : int = 0;
	function PlusXuePing(){
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		} 
		costGold = parseInt( ps.Level ) * AllManage.InvclStatic.VipYaoPingLevel * 5 + 300;
		AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.PlusBottle1 , parseInt( ps.Level ) , AllManage.InvclStatic.VipYaoPingLevel  , "" , gameObject , "realPlusXuePing");
//		AllManage.AllMge.UseMoney(parseInt( ps.Level ) * AllManage.InvclStatic.VipYaoPingLevel * 5 + 300 , 0 , UseMoneyType.PlusBottle1 , gameObject , "realPlusXuePing");
	//	if(ps.UseMoney(costGold , 0)){
	//		
	//	} 
		ShowYaoShuiIcon(invcl.VipYaoPingLevel);
	}

	function realPlusXuePing(){
		PluseYaoPing(1);
		ShowYaoShuiIcon(invcl.VipYaoPingLevel);

	}

	function PluseYaoPing(num : int){
	    invcl.VipYaoPing += num;
	    if(invcl.VipYaoPing > AllManage.InvclStatic.maxXuePingNum){
	        invcl.VipYaoPing = AllManage.InvclStatic.maxXuePingNum;
		}
		AllManage.InvclStatic.yt.Rows[0]["SolutionNum"].YuanColumnText = invcl.VipYaoPing.ToString();
		LabelXuePing.text = invcl.VipYaoPing.ToString() + "/" + AllManage.InvclStatic.maxXuePingNum;
		ShowYaoShuiIcon(invcl.VipYaoPingLevel);
	}


	var ctime : float = 0;
	function CopySP(){
		if(PhotonNetwork.room && PhotonNetwork.room.customProperties && PhotonNetwork.room.customProperties.Count > 0){
	//		DungeonControl.staticRoomSP = new ExitGames.Client.Photon.Hashtable();
			DungeonControl.staticRoomSP.Clear();
			for(var key in PhotonNetwork.room.customProperties){
				DungeonControl.staticRoomSP.Add(key.Key , key.Value);
	//			//print(key.Key + " == " + key.Value);
			}
	//		//print(DungeonControl.staticRoomSP + " == " + DungeonControl.staticRoomSP.Count);
		}
	//	if(PhotonNetwork.room && !PhotonNetwork.offlineMode){
	//		PhotonNetwork.room.SetCustomProperties(DungeonControl.staticRoomSP);
	//		//print(DungeonControl.staticRoomSP + " == " + DungeonControl.staticRoomSP.Count);
	//	}
	}

	var isTaskGroundBack : boolean = false;
	var TweenTaskGround1 : TweenPosition;
	var TweenTaskGround2 : TweenPosition;
	function TaskGroundBack(){
		yield;
		yield;
		yield;
		if(!isTaskGroundBack){
			TweenTaskGround1.Play(false);
			TweenTaskGround2.Play(false);
		}
		isTaskGroundBack = false;
	}

	function UIDisconnect(){
		AllManage.qrStatic.ShowQueRen(gameObject , "ReturnMainSecence" , "messages004");
	}

	function DisconnectGround(){
	//	//print("yun xing duan kai lian jie");
		AllManage.qrStatic.ShowQueRen(gameObject , "ReturnMainSecence" , "messages005");
	}

	function Disconnectleave(){
	//	//print("yun xing duan kai lian jie");
		AllManage.qrStatic.ShowQueRen(gameObject , "ReturnMainSecence" , "messages006");
	}

	function DisconnectZhanChang(){
		AllManage.qrStatic.ShowQueRen(gameObject , "ReturnMainTown" , "messages006");
	}

	function ReturnMainTown(){
//			InRoom.GetInRoomInstantiate().RemoveTempTeam();
			if(DungeonControl.ReLevel != ""){
				Loading.Level = DungeonControl.ReLevel;		
			}else{
//				print("Go to Login-1");
				Loading.Level = "Login-1";
			}
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
			InRoom.GetInRoomInstantiate().PVP1InviteRemove();			
			InRoom.GetInRoomInstantiate().PVPTeamDissolve();
			AllManage.UICLStatic.RemoveAllTeam();
			if(mapType == MapType.jingjichang){
				InRoom.GetInRoomInstantiate().LeaveRoom(PhotonNetwork.room.name);
				InRoom.GetInRoomInstantiate().ActivityPVPRemove();
			}
		alljoy.DontJump = true;
		yield;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			PhotonNetwork.LeaveRoom();
				AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}

var GroundLiaoTian : GameObject;
var LiaoTian : UIPanel;
var yuanInputCtrl : GameObject;
var ScroBvCtrl : GameObject;
function ShowLiaoTian(){
	if(parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) < 5){
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info846"));
		return;
	}
			if(! objMainUI){
				MainUIOn = true;
				var preMainUIl = Resources.Load("Anchor - MainUI", GameObject);
				objMainUI = GameObject.Instantiate(preMainUIl);
				objMainUI.transform.parent = TransMainUI;
				objMainUI.transform.localPosition = Vector3.zero;
				while(LiaoTian == null){
					yield;				
				}
				yield;	
				yield;	
			}
	objMainUI.active = true;
	yuanInputCtrl.transform.localPosition.y = 268;
    LiaoTian.enabled=true;
    LiaoTian.gameObject.active = true;
	GroundLiaoTian.SetActiveRecursively(true);
	yuanInputCtrl.SetActiveRecursively(true);
	ScroBvCtrl.SetActiveRecursively(true);
}
function CloseLiaoTian(){
	if(GroundLiaoTian){
		GroundLiaoTian.SetActiveRecursively(false);
	    LiaoTian.enabled=false;
	    yuanInputCtrl.SetActiveRecursively(false);
	    ScroBvCtrl.SetActiveRecursively(false);
		show0();
	}
}

function OpenLiaoTianOne( mObj :Object )
{ 
   yield StartCoroutine(ShowLiaoTian());
	yuanInputCtrl.transform.localPosition.y = 268;
	yuanInputCtrl.SetActiveRecursively(true);
	LiaoTian.enabled=true;
	GroundLiaoTian.SetActiveRecursively(true);
	//PanelStatic.StaticBtnGameManager.btnSend.SendMessage("ShowOne", mObj, SendMessageOptions.DontRequireReceiver);
	 PanelStatic.StaticBtnGameManager.RunShowOne(mObj); 
}

	function CloseWenHao(){ 
	//	yield;
	//	TransWenhao[1].gameObject.SetActiveRecursively(false);
	//	TransWenhao[0].gameObject.SetActiveRecursively(false);
	}

	function ReturnMainSecence(){
	//	PhotonNetwork.LeaveRoom();
		Loading.Level = "Login-1";
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		AllManage.UICLStatic.RemoveAllTeam();
		alljoy.DontJump = true;
		yield;
			PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}
		public function desP(){
			while(PhotonNetwork.isMasterClient){
				yield;
			}
			PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
		}

	var TransBag : Transform;
	var TransInvInfo : Transform;
	var TransXunLian : Transform;
	var isXunLian : boolean =false;
	var xunPanel : UIPanel;
	function ShowXunLian(){
				if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.PetSwitch) == "0"){
					AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
					return;
				}
		
		if(!isXunLian){
			isXunLian = true;
			TransInvInfo.gameObject.SendMessage("InfoClose" , SendMessageOptions.DontRequireReceiver);
			TransBag.localPosition.y = 1000;
	//		TransXunLian.localPosition.y = 0;
			
			xunPanel.transform.localPosition.y = 0;
			xunPanel.clipOffset.y = 0;
		}else{
			CloseXunLian();
		}
	}

	function CloseXunLian(){
		isXunLian = false;
		TransBag.localPosition.y = 0;
	//	TransInvInfo.localPosition.y = 1000;
	//	TransXunLian.localPosition.y = 1000;
	}

	var wkcl : WaKControl;
	function FindOreStone(ts : TriggerStone){
		wkcl.FindStone(ts);
	}

	function ExitOreStone(){
		wkcl.ExitStone();
	}

	function ExitFish(){
		wkcl.ExitFish();
	}

	function ExitBox(){
		wkcl.ExitBox();
	}
	function ExitFood(){
		wkcl.ExitFood();
	}
	function ExitFlag(){
		wkcl.ExitFlag();
	}
	function ExitWaK(){
		wkcl.ExitWaK();
	}

	var ButtonZhuCL : ButtonsZhuControl;
	function BeiDaDuan(){
		wkcl.DaDuanStone();
		ButtonZhuCL.DaDuanRide();
	}

	var TransWenhao : Transform[];
	private var bb= true;
	function showWenHao(useBB : boolean){
	//	yield WaitForSeconds(0.2);
	//	bb = useBB;
	//	if(bb){
	//	bb=false;
	////	//print("tan chu wen hao");
		if(mapType == MapType.zhucheng){
			TransWenhao[1].gameObject.transform.localPosition.y = 0;
			TransWenhao[0].gameObject.transform.localPosition.y = 3000;
		}else{
			TransWenhao[1].gameObject.transform.localPosition.y = 3000;
			TransWenhao[0].gameObject.transform.localPosition.y = 0;
		}
	//	}
	//	else{
	//			CloseWenHao();
	//			bb= true;
	//	}
	}

	var SpritePlayer : UISprite;
	var SpritePlayerTIao : UISprite;
	var SpritePlayerTIao1 : UISprite;
	var SpritePlayerAttack : UISprite;
	var LabelJiNeng : UILabel[];
	var JiNengText1 : String[];
	var JiNengText2 : String[];
	var JiNengText3 : String[];
	function showPlayer(id : int){
		var i : int = 0;
		switch(id){
			case 1:
				SpritePlayer.spriteName = "head-zhanshi";
				SpritePlayerTIao.spriteName = "UIM_Anger Article";
				if(SpritePlayerTIao1)
					SpritePlayerTIao1.spriteName = "UIM_Anger Article";
	//			for(i=0; i<LabelJiNeng.length; i++){
	//				LabelJiNeng[i].text = AllManage.AllMge.Loc.Get(JiNengText1[i]) ;
	//			}
				break;
			case 2:
				SpritePlayer.spriteName = "head-youxia";
				SpritePlayerTIao.spriteName = "UIM_Charge_ Article";
				if(SpritePlayerTIao1)
					SpritePlayerTIao1.spriteName = "UIM_Charge_ Article";
	//			for(i=0; i<LabelJiNeng.length; i++){
	//				LabelJiNeng[i].text = AllManage.AllMge.Loc.Get(JiNengText2[i]);
	//			}
				break;
			case 3:
				SpritePlayer.spriteName = "head-fashi";
				SpritePlayerTIao.spriteName = "UIM_Magic Article";
				if(SpritePlayerTIao1)
					SpritePlayerTIao1.spriteName = "UIM_Magic Article";
	//			for(i=0; i<LabelJiNeng.length; i++){
	//				LabelJiNeng[i].text = AllManage.AllMge.Loc.Get(JiNengText3[i]);
	//			}
				break;
		}
	}

	var TweenTask : TweenPosition;
	var LabeltaskNPC : UILabel;
	var Labeltasktitle : UILabel;
	var LabeltaskPlayer : UILabel;
	var TaskNCL : TaskNowControl;
	var SpriteNPC : UISprite;
	var LabelButtonStep : UILabel;
	function TaskInfoKuangOn(log : TaskDialog , OneTask : MainTask , isGO : boolean , step : int){
	//	//print("22222" + LabelNPCTalk.text);
		if(ButtonJingJi)
		ButtonJingJi.parent.localPosition.y = 3000;
		AllManage.UIALLPCStatic.show23(); 
		yield;
		yield;
		parentTaskInfo.SetActiveRecursively(true);
		ShowFindWayButton.SetActiveRecursively(false);
		LabelNPCTalk.text = "";
		NPCGroup[0].localPosition.y = 0;
		NPCGroup[1].localPosition.y = 1000;

	//	TweenTask.Play(true);
		var mm : String ;
		if(PlayerName == ""){
			PlayerName = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus).PlayerName;
		}
		try{
			LabeltaskNPC.text = log.npcLog.Replace("XXXX" , PlayerName);
		}catch(e){
			LabeltaskNPC.text = log.npcLog;
		}
		if(NPCPanel1){
			NPCPanel1.clipOffset.y = 0;
			NPCPanel1.gameObject.transform.localPosition.y = 0;
		}
		
		try{
			LabeltaskPlayer.text = log.playerLog.Replace("XXXX" , PlayerName);
		}catch(e){
			LabeltaskPlayer.text = log.playerLog;
		}
		if(NPCPanel2){
			NPCPanel2.clipOffset.y = 0;
			NPCPanel2.gameObject.transform.localPosition.y = 0;
		}
		TaskNCL.ShowTaskInfooo(OneTask);
		SpriteNPC.spriteName = "NPC_" + NowNPCID;
		if(nowNPCType == NPCFunctions.Non){
			NPCFunctionsButton.SetActiveRecursively(false);
		}
		if(isGO){
			ButtonGo.SetActiveRecursively(true);
		}else{
			ButtonGo.SetActiveRecursively(false);	
		}
	//	//print(step + " == step");
		switch(step){
			case 0: 
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages117");
				AllManage.AllMge.SetLabelLanguageAsID(LabelButtonStep);
	//			LabelButtonStep.text = "继续"; 
				break;
			case 1: 
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages118");
				AllManage.AllMge.SetLabelLanguageAsID(LabelButtonStep);
	//			LabelButtonStep.text = "接受"; 
				break;
			case 2: 
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages119");
				AllManage.AllMge.SetLabelLanguageAsID(LabelButtonStep);
	//			LabelButtonStep.text = "完成"; 
				break;
		}
//		ButtonJingJi.parent.localPosition.y = 0;
	}

	var DoingTask : MainTask;
	var ShowFindWayButton : GameObject;
	function ShowFindWay(OneTask : MainTask){
		DoingTask = OneTask;
		ShowFindWayButton.SetActiveRecursively(true);
	}

	private var NextTaskKuangOn : boolean = false;
	function FindDoingWay(){
		try{
			MainTW.isGUanBi = true;
			NextTaskKuangOn = true;
			MainTW.stepIsServer = false;
			MainTW.InTaskInfoStep();
			var pt : PrivateTask;
			pt = new PrivateTask();
			pt.jindu = 1;
			pt.task = DoingTask;
			MainTW.FindWay(pt);
		}catch(e){
			ButtonGo.SetActiveRecursively(false);
		}
	}

	var NPCtaskItem : TaskNpcItem[];
	var NPCGroup : Transform[];
	private var MainPlayerS : MainPersonStatus;
	var canOpenNPCTaskGO : boolean = false;
	function OpenNPC(tasks : MainTask[] , str : String){
//		yield;
//		yield;
//		yield;
//		yield;
		if(canOpenNPCTaskGO){
			var switchGo : boolean = true;
			if(nowNPC != null){
				var bool = nowNPC.FindOtherCanShow();
				if(bool){
					switchGo = false;
//					nowNPC.TriggerOn();
				}
			}
			if(switchGo){
				var pt : PrivateTask;
				pt = new PrivateTask();
				pt.jindu = MainTW.OneTask.jindu;
				pt.task = MainTW.OneTask;
				AllManage.UIALLPCStatic.show0();
				canOpenNPCTaskGO = false;
				if(isWayAddTask)
					MainTW.FindWay(pt);	
				return;
			}
		}
		canOpenNPCTaskGO = false;

//		print("----------123123");
		if(ButtonJingJi)
		ButtonJingJi.parent.localPosition.y = 3000;
		AllManage.UIALLPCStatic.show23(); 
		yield;
		yield;
		LabelNPCTalk.text = str.ToString();
		NPCGroup[0].localPosition.y = 1000;
		NPCGroup[1].localPosition.y = 0;
	//	TweenTask.Play(true);
//		for(var m=0; m<NPCtaskItem.length; m++){
//			NPCtaskItem[m].gameObject.SetActiveRecursively(false);
//		}
		while(!PlayerStatus.MainCharacter){
	          yield;
	          }
	       yield;
		if(MainPlayerS == null){
			MainPlayerS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
		}
		var i : int = 0;
		var o : int = 0;
		for(i = 0; i<tasks.length; i++){
			if(tasks[i] != null && MainTW.LookTaskPaiChi(MainPlayerS.player , tasks[i].ExcludeTask) && tasks[i].leixing == 0){
				if(MainPlayerS.LookTaskIsDone(tasks[i].id) || tasks[i].leixing == 5  || (tasks[i].leixing == 4 && !MainPlayerS.LookEveryDayActivityIsDone( tasks[i].id))){
					for(o=0; o<NPCtaskItem.length; o++){
						if(!NPCtaskItem[o].active){
							NPCtaskItem[o].gameObject.SetActiveRecursively(true);
							NPCtaskItem[o].SetNPCTask(tasks[i] , NowNPCID);
							break;
						}
					}
				}
			}
		}
		for(i=0; i<tasks.length; i++){
			if(tasks[i] != null && MainTW.LookTaskPaiChi(MainPlayerS.player , tasks[i].ExcludeTask) && tasks[i].leixing != 0){
				if(MainPlayerS.LookTaskIsDone(tasks[i].id) || tasks[i].leixing == 5  || (tasks[i].leixing == 4 && !MainPlayerS.LookEveryDayActivityIsDone( tasks[i].id))){
					for(o=0; o<NPCtaskItem.length; o++){
						if(!NPCtaskItem[o].active){
							NPCtaskItem[o].gameObject.SetActiveRecursively(true);
							NPCtaskItem[o].SetNPCTask(tasks[i] , NowNPCID);
							break;
						}
					}
				}
			}
		}
		SpriteNPC.spriteName = "NPC_" + NowNPCID;
		
//		var bool : boolean = false;
//		for(i=0; i<NPCtaskItem.length; i++){
//			if(! bool && NPCtaskItem[i].active &&  NPCtaskItem[i].myTask != null && NPCtaskItem[i].myTask.jindu != 1){
//				bool = true;
//				NPCtaskItem[i].selectme();
//			}
//		}
//		ButtonJingJi.parent.localPosition.y = 0;
	}

	function IsMoreTaskCanGet(){
		for(var i=0; i<NPCtaskItem.length; i++){
			if(NPCtaskItem[i].active &&  NPCtaskItem[i].myTask != null && NPCtaskItem[i].myTask.jindu != 1){
				return true;
			}
		}
	}

	var NowNPCID : String;
	var nowNPC : npcAI;
	var npcl : NPCControl;
	function GetNowNpcID(id : String){
		NowNPCID = id;
		if(!npcl){
			npcl = FindObjectOfType(NPCControl);
		}
		nowNPC = npcl.GetNPCAsID(NowNPCID);
	//	//print("yun xing le zhe li");
	}

	function TaskInfoNext(){
		MainTW.stepIsServer = false;
		MainTW.InTaskInfoStep1();
	}

	function FindNpcOtherTask(){
		return;
		if(NextTaskKuangOn){
			NextTaskKuangOn = false;
			return;
		}
		yield;
		yield;
		yield;
		var bool = nowNPC.FindOtherCanShow();
		if(bool){
			nowNPC.TriggerOn();
		}
	}

	function TaskKuangBack(){
		AllManage.UIALLPCStatic.show0();
	//	TweenTask.Play(false);
		LabelNPCTalk.text = "";
	//	//print("33333" + LabelNPCTalk.text);
	}

	var LabelNPCTalk : UILabel;
	var parentTaskInfo : GameObject;
	var ButtonNextStep : GameObject;
	var rewardObj : GameObject;
	var ButtonGo : GameObject;
	function NPCTalk(str : String){
		if(ButtonJingJi)
		ButtonJingJi.parent.localPosition.y = 3000;
	//	//print("44444" + LabelNPCTalk.text);
		yield AllManage.UIALLPCStatic.show23(); 
//		yield;
//		yield;
//		yield;
		parentTaskInfo.SetActiveRecursively(false);
	//	NPCGroup[0].localPosition.y = 0;
	//	NPCGroup[1].localPosition.y = 1000;
		NPCGroup[0].localPosition.y = 1000;
		NPCGroup[1].localPosition.y = 0;
	//	TweenTask.Play(true);
		SpriteNPC.spriteName = "NPC_" + NowNPCID;
		ButtonNextStep.SetActiveRecursively(false);
		rewardObj.SetActiveRecursively(false);  
		NPCGroup[1].gameObject.SetActiveRecursively(false); 
		LabelNPCTalk.gameObject.active = true;
		if(str == ""){
			ButtonGo.SetActiveRecursively(false);
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages120");
				AllManage.AllMge.SetLabelLanguageAsID(LabelNPCTalk);
	//		LabelNPCTalk.text = "目前没有合适的任务";
		}else{
			LabelNPCTalk.text = str;	
		}
		NPCGroup[1].gameObject.active = true;
//		ButtonJingJi.parent.localPosition.y = 0;
	}

	var LabelNPCTalk1 : UILabel;
	var NpcTypeName : String[];
	var LableNPCName : UILabel;
	var LableNPCTitleName : UILabel;
	var LabelNPCTouXiangName : UILabel;
	var nowNPCType : NPCFunctions;
	var NPCFunctionsButton : GameObject;
	var refOnePvp : RefreshOnePVPList;
	var ButtonJingJi : Transform;
	var ButtonZhanChang : Transform; 
	var OnePvpList : GameObject;
	function SHowNPCTalk(str : String , npcType : NPCFunctions , NPCname : String){
//		return;
//		AllManage.UIALLPCStatic.show23(); 
//		yield;
//		yield;
//		yield;
//		yield;
		if(ButtonJingJi)
		ButtonJingJi.parent.localPosition.y = 3000;
		NPCFunctionsButton.SetActiveRecursively(true);
		LabelNPCTalk.text = str.ToString();
		LabelNPCTalk.parent.active = true;
		LabelNPCTalk1.text = str.ToString();
		yield;
		yield; 
		yield;
		yield;
		yield;
		yield;
		yield;
		if(NPCPanel1){
			NPCPanel1.clipOffset.y = 0;
			NPCPanel1.gameObject.transform.localPosition.y = 0;
		}
		LabelNPCTouXiangName.text = NPCname;
		nowNPCType = npcType; 
//		print(nowNPCType + "   =========================nowNPCType");
		LableNPCTitleName.text = AllManage.AllMge.Loc.Get(NpcTypeName[nowNPCType]);

		ButtonJingJi.localPosition.y = 3000;
		ButtonZhanChang.localPosition.y = 3000;
		Battlefield1.SetActiveRecursively(false);
	//	Battlefield2.SetActiveRecursively(false); 
		OnePvpList.SetActiveRecursively(false);
	//	yield;
		switch(nowNPCType){
			case NPCFunctions.Non :
				NPCFunctionsButton.SetActiveRecursively(false);
				LableNPCName.text = AllManage.AllMge.Loc.Get(NpcTypeName[nowNPCType]);
				break;
			case NPCFunctions.WeaponShop :
				LableNPCName.text = AllManage.AllMge.Loc.Get(NpcTypeName[nowNPCType]);
				break;
			case NPCFunctions.Grocerystore :
				LableNPCName.text = AllManage.AllMge.Loc.Get(NpcTypeName[nowNPCType]);
				break;
			case NPCFunctions.GuildShop :
				LableNPCName.text = AllManage.AllMge.Loc.Get(NpcTypeName[nowNPCType]);
				break;
			case NPCFunctions.HonorStore :
				LableNPCName.text = AllManage.AllMge.Loc.Get(NpcTypeName[nowNPCType]);
				break;
			case NPCFunctions.Arena :
				if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.PVPSwitch) == "1"){
					NPCFunctionsButton.SetActiveRecursively(false);
//					ButtonJingJi.localPosition.y = -65;
				}
				break;
			case NPCFunctions.Battlefield:  
				if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.PVPSwitch) == "1"){
					AllManage.AllMge.Keys.Clear();
					AllManage.AllMge.Keys.Add("buttons198");
					AllManage.AllMge.SetLabelLanguageAsID(LableNPCName);
		//			ShuaXinXueDi();
//					ButtonZhanChang.localPosition.y = 0;
//					NPCFunctionsButton.SetActiveRecursively(false);
		//			refOnePvp.GetLegionOneList();
				}
				break;
			case NPCFunctions.Duel: 
				if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.PVPSwitch) == "1"){
					AllManage.AllMge.Keys.Clear();
					AllManage.AllMge.Keys.Add("buttons095");
					AllManage.AllMge.SetLabelLanguageAsID(LableNPCName);
//					NPCFunctionsButton.SetActiveRecursively(false);
		//			OnePvpList.SetActiveRecursively(true);
		//			LableNPCName.text = "打开列表";
		//			PanelStatic.StaticBtnGameManager.GetPVP1List();
		//			refOnePvp.GetList();
				}
				break;
			case NPCFunctions.Skill :
				LableNPCName.text = AllManage.AllMge.Loc.Get(NpcTypeName[nowNPCType]);
				break;
			case NPCFunctions.RandomStore :
				LableNPCName.text = AllManage.AllMge.Loc.Get(NpcTypeName[nowNPCType]);
				break;
			case NPCFunctions.PVPStore :
				LableNPCName.text = AllManage.AllMge.Loc.Get(NpcTypeName[nowNPCType]);
				break;
		}
		if(ButtonJingJi)
		ButtonJingJi.parent.localPosition.y = 0;
	}

	function PaiDui2v2(){ 
		InRoom.GetInRoomInstantiate().PVPTeamCreate(InventoryControl.yt.Rows[0]["Corps2v2ID"].YuanColumnText,2);
	} 

	function PaiDui4v4(){
		InRoom.GetInRoomInstantiate().PVPTeamCreate(InventoryControl.yt.Rows[0]["Corps4v4ID"].YuanColumnText,4);
	}

	var Battlefield1 : GameObject;
	var Battlefield2 : GameObject;
	function ShuaXinCity(){
	//	Battlefield1.SetActiveRecursively(true);
	//	Battlefield2.SetActiveRecursively(false);
	}

	function ShuaXinXueDi(){
		refOnePvp.GetLegionOneList();
		Battlefield1.SetActiveRecursively(true);
		NPCGroup[0].gameObject.SetActiveRecursively(false);
		NPCGroup[1].gameObject.SetActiveRecursively(false);
	//	Battlefield2.SetActiveRecursively(true);
	}

	var BtnGM : BtnGameManager;
	function CreateChengQu(){
		BtnGM.BtnAddLegionOneMapCity();
	} 

	function CreateXueDi(){
		BtnGM.BtnAddLegionOneMapOut();
	}

	//private var bhcl : BranchControl;
	//private var arencl : ArenaControl;
	function NpcFunc(){
	 	if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(canOpenViewAsTaskID()){	
			switch(nowNPCType){
				case NPCFunctions.Non :
					break;
				case NPCFunctions.WeaponShop :
		//			OpenStoreAsType(ps.ProID + 3*Random.Range(0,2));
					OpenStoreAsType(1);
					break;
				case NPCFunctions.Grocerystore :
					OpenStoreAsType(87);
					break;
				case NPCFunctions.GuildShop :
					OpenStoreAsType(81);
					break;
				case NPCFunctions.HonorStore :
					OpenStoreAsType(9);
					break;
				case NPCFunctions.PVPStore :
					OpenStoreAsType(7);
					break;
				case NPCFunctions.Arena :
		//			//print("zhi xing le123123123123123");
					InRoom.GetInRoomInstantiate().PVPTeamCreate(InventoryControl.yt.Rows[0]["Corps2v2ID"].YuanColumnText,2);
					break;
				case NPCFunctions.Arena4v4 :
		//			//print("zhi xing le77777773");
					InRoom.GetInRoomInstantiate().PVPTeamCreate(InventoryControl.yt.Rows[0]["Corps4v4ID"].YuanColumnText,4);
					break;
				case NPCFunctions.Battlefield:
					BattleMoveOn();
//		//			//print("zhi xing le123123123123123");
//					InRoom.GetInRoomInstantiate().LegionAddQueue();
					break;
				case NPCFunctions.Duel:
					PVPMoveOn();
//		//			//print("zhi xing le");
//					NPCGroup[0].gameObject.SetActiveRecursively(false);
//					NPCGroup[1].gameObject.SetActiveRecursively(false);
//					OnePvpList.SetActiveRecursively(true);
//		//			PanelStatic.StaticBtnGameManager.GetPVP1List();
//					PanelStatic.StaticBtnGameManager.GetTempTeam();
//		//			InRoom.GetInRoomInstantiate().PVPTeamCreate("",1);
					break;
				case NPCFunctions.Skill :
		//			bhcl.OpenBranch();
					AllManage.UIALLPCStatic.show15();
					break;
				case NPCFunctions.RandomStore :
					OpenStoreAsType(0);
					break;
			}
		}
	}

	var Ylist1 : GameObject;
	var Ylist2 : GameObject;
	function UICLenable(){ 
		yield;
		yield;
		yield;
		Ylist1.SetActiveRecursively(false);
		Ylist2.SetActiveRecursively(false);
		switch(nowNPCType){
			case NPCFunctions.Arena :
				break;
			case NPCFunctions.Arena4v4 :
				break;
			case NPCFunctions.Battlefield:
				Ylist1.SetActiveRecursively(true);
				break;
			case NPCFunctions.Duel:
				Ylist2.SetActiveRecursively(true);
				break;
		}
	}

	function StartPeiDui(){
	//	//print("StartPeiDui");
		InRoom.GetInRoomInstantiate().LegionPVPAddQueue();
	}

	private var pvpstr : String;
	var pvpcl : PVPControl;
	function WanChengPeiDui(pvp : String){
		pvpstr = pvp;
		////print("WanChengPeiDui");
		switch(pvpstr){
			case "1":
				pvpcl.SetDuelType(3);
				break;
			case "2":
				AllManage.qrStatic.ShowQueRen(gameObject,"JiaRuYes","JiaRuNo","info1019");	
				break;
			case "4":
				AllManage.qrStatic.ShowQueRen(gameObject,"JiaRuYes","JiaRuNo","info1019");	
				break;
			case "8":
				pvpcl.SetBattlefieldType(3);
				break;
		}
	}

	function NewWanChengPeiDui(){
		AllManage.qrStatic.ShowQueRen(gameObject,"YesPeiDui","NoPeiDui","info1019");
	}

	function closeNewWanChengPeiDui(){
		AllManage.qrStatic.myObj.SetActiveRecursively(false);
	}

	function YesPeiDui(){
		InRoom.GetInRoomInstantiate().SendLegionPVPInfo(1);
	}

	function NoPeiDui(){
//		InRoom.GetInRoomInstantiate().PVPInviteIsNo();
	    InRoom.GetInRoomInstantiate().SendLegionPVPInfo(2); 
	    if(!PlayerUtil.battlefieldId.Equals(""))
	    {
	        InRoom.GetInRoomInstantiate().PVPCancel(PlayerUtil.battlefieldId);
	        PlayerUtil.battlefieldId = "";
	    }
//		InRoom.GetInRoomInstantiate().PVPTeamDissolve();
	}

	function BattlefieldTimeOut(){
		if(AllManage.qrStatic.returnFunctionY == "YesPeiDui" && AllManage.qrStatic.myObj.active){
			AllManage.qrStatic.myObj.SetActiveRecursively(false);		
		}
		if(null != AllManage.UICLStatic.pvpcl) AllManage.UICLStatic.pvpcl.SetDuelType(1);
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info987"));
	}

    // 可以退出排队
	function SetDuelType(type : int)
	{
	    if(null != AllManage.UICLStatic.pvpcl)
	    {
	        AllManage.UICLStatic.pvpcl.SetDuelType(type);
	    }
	}

	function OpenDoor(){
		DoorWasOpen = true;
	var GateSp = FindObjectsOfType(Gate);
//	AllManage.tsStatic.Show("tips084");
//	yield WaitForSeconds(2);
//	AllManage.tsStatic.Show("tips084");
//	yield WaitForSeconds(2);
//	AllManage.tsStatic.Show("tips084");
//	yield WaitForSeconds(2);
	//	for (var i : int = 0; i < 4; i++) {
		for (var i : int = 0; i < GateSp.length; i++) {
			GateSp[i].OpenGate();
		}
		var DotMC = FindObjectsOfType(DOTMonsterControl);
		if(DotMC.length > 0){
			for (i= 0; i < DotMC.length; i++) {
				DotMC[i].DotaStart();
			}
		}
		if(PhotonNetwork.connected && PhotonNetwork.isMasterClient && !PhotonNetwork.offlineMode){
	    	AllManage.dungclStatic.staticRoomSP["OpenDoor"] = "1";
	    	AllManage.dungclStatic.SetMonsterStaticRoom();				
		}

	AllManage.tsStatic.Show("tips085");	
	}

	function EnterOpen(str:String[]){
	   PVPGO(str);
	  Opendoor = true;
	}

	var DoorWasOpen : boolean = false;
	function OpenDoorNow(){
		DoorWasOpen = true;
		var i : int = 0;
		var GateSp = FindObjectsOfType(Gate);
		if(GateSp.length > 0){
			for (i=0; i < GateSp.length; i++) {
			GateSp[i].OpenGateNow();
			}
		}
		
		var DotMC = FindObjectsOfType(DOTMonsterControl);
		if(DotMC.length > 0){
			for (i= 0; i < DotMC.length; i++) {
				DotMC[i].DotaStart();
			}
		}
		
	}

	function JiaRuYes(){
		if(pvpstr == "8"){
			InRoom.GetInRoomInstantiate().SendLegionInfo(true);	
		}else{
			InRoom.GetInRoomInstantiate().SendLegionPVPInfo(1);	
		}
	}

	function JiaRuNo(){
		if(pvpstr == "8"){
			InRoom.GetInRoomInstantiate().SendLegionInfo(false);
		}else{
			InRoom.GetInRoomInstantiate().SendLegionPVPInfo(2);
		}
	}

	static var myTeam : String = "";
	static var otherTeam : String;
	static var myTeamInfo : String;
	static var myTeamHeadID : String;
	static var arenaType : String;
	static var ArenaID : String = "";
	static var BattlefieldID : String = "";
	static var Battlefieldrows : yuan.YuanMemoryDB.YuanRow;
	static var BattlefieldOpenDoorTime : String = "";
	static var BattlefieldEndTime : String = "";
	static var nowIsPVPGO : boolean = false;
	static var BattlefieldinstanceID : String = "";
	static var BattlefieldStarTime : String = "";
	function PVPGO(str : String[]){
		var  myTime : System.DateTime;
//		myTime = InRoom.GetInRoomInstantiate().serverTime;
		BattlefieldStarTime = str[11];
		myTime = System.DateTime.Parse(BattlefieldStarTime);
		BattlefieldOpenDoorTime = myTime.AddSeconds(parseFloat(CommonDefine.BattlefieldOpenDoorTime)).ToString();
		BattlefieldEndTime = myTime.AddSeconds(parseFloat(CommonDefine.PVP_END_TIME)).ToString();
		var i : int = 0;
		ArenaID = str[0];
		Loading.loadstr = str[0];
		myTeam = str[1];
		otherTeam = str[2];
		myTeamInfo = str[3];// Red || Blue
		arenaType = str[4];
		myTeamHeadID = str[5];
		ArenaControl.Battlefieldtype = "";
		BattlefieldID = str[7];
//		BattlefieldOpenDoorTime = str[8];
//		BattlefieldEndTime = str[9];
		BattlefieldinstanceID = str[10];
		
//		print(BattlefieldOpenDoorTime + " == BattlefieldOpenDoorTime -- " + BattlefieldEndTime + " == BattlefieldEndTime");
		for(var rows : yuan.YuanMemoryDB.YuanRow in InventoryControl.BattlefieldTable.Rows){
//			print(BattlefieldID + " == " + rows["id"].YuanColumnText);
			if(BattlefieldID == rows["id"].YuanColumnText){
				Battlefieldrows = rows;			
			}
		}
//		print(BattlefieldID + " == BattlefieldID");
		Loading.Level = "Map" + Battlefieldrows["mapid"].YuanColumnText;
		switch(arenaType){
			case "1":
//				Loading.Level = "Map311";
				ArenaControl.areType = 0;
				break;
			case "2":
//				Loading.Level = "Map331";
				ArenaControl.areType = 1;
				break;
			case "4":
//				Loading.Level = "Map331";
				ArenaControl.areType = 1;
				break;
			case "8":
				ArenaControl.Battlefieldtype = str[6];
				if(str[6] == "xuedi"){
//					Loading.Level = "Map421";				
				}else
				if(str[6] == "chengqu"){
//					Loading.Level = "Map411";
				}else{
					ArenaControl.Battlefieldtype = "chengqu";
//					Loading.Level = "Map"+str[6].Substring(0,3);
				}
				ArenaControl.areType = 2;
			break;
			case "16":
				ArenaControl.Battlefieldtype = str[6];
				if(str[6] == "xuedi"){
//					Loading.Level = "Map421";				
				}else
				if(str[6] == "chengqu"){
//					Loading.Level = "Map411";
				}else{
					ArenaControl.Battlefieldtype = "chengqu";
//					Loading.Level = "Map"+str[6].Substring(0,3);
				}
				ArenaControl.areType = 2;
			break;
		}

	//	//print(str[]);
//		if(arenaType != "8"){
			invcl.ps.PVPmyTeam = myTeam;
			if(mapType == MapType.zhucheng)
			DungeonControl.ReLevel = Application.loadedLevelName;
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		
//		print(ArenaControl.areType + " == ArenaControl.areType");
//		switch(ArenaControl.areType){
//			case 0 : 
//				AllManage.UIALLPCStatic.show12(); InRoom.GetInRoomInstantiate().PVPTeamDissolve(); break;
//			case 1 : 
//				AllManage.UIALLPCStatic.show13(); break;
//			case 2 : 
//				AllManage.UIALLPCStatic.show14(); InRoom.GetInRoomInstantiate().LegionOneRemove(); InRoom.GetInRoomInstantiate().ActivityPVPRemove();break;
//		}
//		InRoom.GetInRoomInstantiate().PVP1InviteRemove();			
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		InRoom.GetInRoomInstantiate().LeaveRoom(PhotonNetwork.room.name);
//		AllManage.UICLStatic.RemoveAllTeam();
		
		yield;
			nowIsPVPGO = true;
			PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
				AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

//		}else{
//			pvpcl.SetBattlefieldType(3);
//		}
	}

	static var InviteGoPVEID : int = 0;
	function YaoQingGO(objs : Object[]){
		var str : String;
		str = objs[0];
		InviteGoPVEID =  objs[1];
		Loading.Level = str.Substring(0,6);
		Loading.YaoQingStr = str;
		if(mapType == MapType.zhucheng)
		DungeonControl.ReLevel = Application.loadedLevelName;
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
		alljoy.DontJump = true;
		yield;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			PhotonNetwork.LeaveRoom();
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}

	function PVPBattlefieldGo(){
		invcl.ps.PVPmyTeam = myTeam;
		if(mapType == MapType.zhucheng)
		DungeonControl.ReLevel = Application.loadedLevelName;
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			PhotonNetwork.LeaveRoom();
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	} 

	function PVPTeamJieSan(){
		InRoom.GetInRoomInstantiate().PVPTeamDissolve();
	}

	function PVPTeamJieSanDone(){
	//	//print("cheng gong jie san dui wu");
		myTeam = "";
	}

	function ShowFuBenQR(){
		AllManage.qrStatic.ShowQueRen(gameObject,"YesFuBen","NoFuBen","messages008");
	}

	function YesFuBen(){
		InRoom.GetInRoomInstantiate().SendTeamTeamInfo(true);
	}

	function NoFuBen(){
		InRoom.GetInRoomInstantiate().SendTeamTeamInfo(false);
	}

	function GoLinShiFuBen(objs : Object[]){
		var team : int;
		var map : String;
		var nandu : int;
		team = objs[0];
		map = objs[1];
		nandu = objs[2];
	//	PhotonNetwork.LeaveRoom();
		Loading.TeamID = team + "fuben";
		Loading.Level = "Map" + map;
		Loading.nandu = nandu.ToString();
		DungeonControl.NowMapLevel = nandu;
		if(UIControl.mapType == MapType.zhucheng)
		DungeonControl.ReLevel = Application.loadedLevelName;
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			PhotonNetwork.LeaveRoom();
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}

	var ObjStoreButton1 : GameObject;
	var ObjStoreButton2 : GameObject;
	var ObjStoreButton3 : GameObject;
	var ObjStoreButton4 : GameObject;
//	var invCangKu : InventoryCangku;
	var TransStores : Transform[];
	var TransMiddle : Transform;
	var PosStores : float[];
	var isStore : boolean = false;
	var isStoreInt : int;
	function OpenStoreAsType(tp : int){
//		print(tp + "   ======================= tptptptptp");
		if(! objMainUI){
			MainUIOn = true;
			var preMainUIl = Resources.Load("Anchor - MainUI", GameObject);
			objMainUI = GameObject.Instantiate(preMainUIl);
			objMainUI.transform.parent = TransMainUI;
				objMainUI.transform.localPosition = Vector3.zero;
		}
		yield;	
		yield;	
		objMainUI.active = true;
		bTime = Time.time + 1;
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.GameShop).ToString());
		isStoreInt = tp;
		AllManage.UIALLPCStatic.show11();
		yield;
		MoveDaoHang1();
		invcl.QieHuanBagBaoguo();
		yield;
		yield;
		yield;
		yield;
		isStore = true;
		invcl.isCangku = true;
		invcl.isShangdian = false;
		ObjStoreButton1.SetActiveRecursively(false);
		ObjStoreButton2.SetActiveRecursively(false);
		ObjStoreButton3.SetActiveRecursively(false);
		ObjStoreButton4.SetActiveRecursively(false);
	//	//print(TransMiddle.position.x + "tp == " + tp);
		invcl.QieHuanStore(tp);
		invcl.isShangDianTrue(true);
//		if(TransStores[0])
//			TransStores[0].position.x = -0.2323462;
//	//	TransStores[0].position.x -= 25;
//		TransStores[1].position.x = 1.3;
//		TransStores[2].position.x = -0.14801;
//		TransStores[3].position.x = 0.18;
//		//	
		if(TransStores[0])
			TransStores[0].localPosition.x = 0;
	//	TransStores[0].position.x -= 25;
		TransStores[1].localPosition.x = 0;
		TransStores[2].localPosition.x = 0;
		TransStores[3].localPosition.x = 0;
		//	

	//	TransStores[1].position.x += 440;
	}

	var transEquep : Transform;
	function returnStore(){
		transEquep.localPosition.y = 1000;
	//	yield;
	//	yield;
	//	yield;
	//	yield;
	//	TransStores[0].position.x = 0;
		if(TransStores[0])
			TransStores[0].localPosition.y = 0;
	//	TransStores[0].position.x -= 25;
	//	TransStores[1].position.x = 1.3;
	}

	function CloseStore(){
		if(TransStores[0])
			TransStores[0].localPosition.y = 1000;
		isStore = false;
		AllManage.UIALLPCStatic.show0();
	}

	//enum NPCFunctions{
	//	Non =0,
	//   WeaponShop = 1,
	//   Grocerystore = 2,
	//   GuildShop = 3,
	//   HonorStore =4,
	//   Duel = 5,
	//   Arena = 6,
	//   Battlefield = 7
	//	}

	//var LabeltaskInfo : UILabel;
	//private var useStr : String;
	//var taskInfoObj : TaskinfoParent;
	private var useTaskInfo : TaskinfoParent; 
	var infoParent : Transform;
	var TinfoList : TaskInfoList;
	function ShowTaskInfoList(Player : MainPlayerStatus,task : MainTask){
		TinfoList.SetNowTasklist(Player);
		if(nowNPCType == NPCFunctions.Non && NPCFunctionsButton){
			NPCFunctionsButton.SetActiveRecursively(false);
		}
	//	for(var i=0; i<Player.nowTask.length; i++){
	//		if(Player.nowTask[i] != null){
	//			if(Player.nowTask[i].taskInfo == null){
	//				var o = Instantiate(taskInfoObj.gameObject);
	//				Player.nowTask[i].taskInfo = o.GetComponent(TaskinfoParent);
	//			}
	//			Player.nowTask[i].taskInfo.SetTask(Player.nowTask[i]); 
	//			Player.nowTask[i].taskInfo.transform.parent = infoParent; 
	//			Player.nowTask[i].taskInfo.transform.localPosition.y = 0 - 0.3 * i;
	//		}
	//	}
	}

	function ShowCanGetTaskInfo(task : MainTask){
		TinfoList.SetCanGetTaskInfo(task);
	}

	function shuchu1(){
	//	//print("1111" + Time.time);
	}
	function shuchu2(){
	//	//print("2222" + Time.time);
	}


	function XunLianMoveOn(){
		AllManage.UIALLPCStatic.show22();
	//	MainTweenMove();
	//	yield;
	//	yield;
	//	yield;
	//	invcl.QieHuanEquepShuXing();
	//	yield;
	//	ShowXunLian();
	}
	var ObjBuyStore : GameObject;
	var ObjBuyStorePar : Transform;
	function StoreMoveOn(){

	    if(null != PanelStatic.StaticWarnings.warningAllEnterClose)
	    {
	        PanelStatic.StaticWarnings.warningAllEnterClose.Close();
	    }

	    if(canOpenViewAsTaskID()){
//		    show0();
		    if(isGo==false)
//		    yield MainTweenMove();
		    while(mTime > Time.time){
			    yield;
		    }
//		    MoveDaoHang7();
	   	if(ObjBuyStore==null){
		var preSection = Resources.Load("PanelBlood", GameObject);
		ObjBuyStore = GameObject.Instantiate(preSection);
		ObjBuyStore.transform.parent = ObjBuyStorePar;
		ObjBuyStore.transform.localPosition = Vector3.zero;
		}else{
		ObjBuyStore.SetActive(true);
			}
		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().GetBuyInfor(TableRead.strPageName));
		yield;
		if(dicGetResult1 != null && AllManage.buyStoreControl.LabelBloodNum1 && AllManage.buyStoreControl.LabelBloodNum1.text != dicGetResult1["propPrice"]){
			AllManage.buyStoreControl.LabelBloodNum1.text = dicGetResult1["propPrice"];
			AllManage.buyStoreControl.LabelBloodNum2.text = dicGetResult2["propPrice"];
			AllManage.buyStoreControl.LabelBloodNum3.text = dicGetResult3["propPrice"];
			AllManage.buyStoreControl.LabelBloodNum4.text = dicGetResult4["propPrice"];
			
			AllManage.buyStoreControl.LabelBloodName1.text = dicGetResult1["propName"];
			AllManage.buyStoreControl.LabelBloodName2.text = dicGetResult2["propName"];
			AllManage.buyStoreControl.LabelBloodName3.text = dicGetResult3["propName"];
			AllManage.buyStoreControl.LabelBloodName4.text = dicGetResult4["propName"];	
		}
	    }
	}
	
	function CloseStoreMoveOn(){
		if(ObjBuyStore){
			ObjBuyStore.SetActive(false);
	   		 }
	
	}

	function SkillMoveOn(){
	if(canOpenViewAsTaskID()){
		if(isGo==false)
		yield MainTweenMove();
		PlayerSelectSkill();
		}
	//	MoveDaoHang7();
	}

	function XunLianOverMoveOn(){
		if(canOpenViewAsTaskID()){
			if(isGo==false)
			yield MainTweenMove();
			PlayerSelectXunLian();
		}	
	}
	
	function XunLianOverMoveOnNew(){
		if(canOpenViewAsTaskID()){
			if(isGo==false)
			MainTweenMove();
			ShowxunLian();
	
		}	
	}

		var isShow : boolean  = true; 
		var isNum : int  =  0 ; 
	function ShowxunLian(){
			while(isShow){
			PlayerSelectXunLian();
			isNum++;
			if(isNum == 5){
			isShow = false;
			isNum = 0;
			}
			yield;
			yield;
			}
	}

	function yingmoOverMoveOn(){
	if(canOpenViewAsTaskID()){
		if(isGo==false)
		yield MainTweenMove();
		MoveDaoHang9();
		AllManage.UICLStatic.pvpcl.OffLinePlayerButton();
		}
	}

	var yieldTimes : int = 0;
	function StoreOpenMoveOn(){
		if(canOpenViewAsTaskID()){
			if(isGo==false)
				yield MainTweenMove();
			yieldTimes = 10;
			while(yieldTimes > 0){
				yieldTimes -= 1;
				yield;
			}
			MoveDaoHang7();
		}
	}

	function TaskMoveOn(){
	if(canOpenViewAsTaskID()){
		if(isGo==false)
		yield MainTweenMove();
		MoveDaoHang2();
		}
	}

	function BagMoveOn(){
		if(canOpenViewAsTaskID()){
			if(isGo==false){
				yield MainTweenMove();
				MoveDaoHang1();
			}
		}
	}
	
	function PVPMoveOn(){
		if(canOpenViewAsTaskID()){
			if(isGo==false){
				yield MainTweenMove();
				MoveDaoHang9();
			}
		}
	}
	
	function BattleMoveOn(){
		if(canOpenViewAsTaskID()){
			if(isGo==false){
				yield MainTweenMove();
				MoveDaoHang9();
				AllManage.InvclStatic.btnBattleSelect.SendMessage("OnClick" , SendMessageOptions.DontRequireReceiver);
		}
		}
	}
	function canOpenViewAsTaskID() : boolean{
		if(!MainTW.MainPS.LookTaskIsGet("11") || !MainTW.MainPS.LookTaskIsGet("111") || !MainTW.MainPS.LookTaskIsDone("11") || !MainTW.MainPS.LookTaskIsGet("13") || !MainTW.MainPS.LookTaskIsGet("24") || !MainTW.MainPS.LookTaskIsGet("27") || !MainTW.MainPS.LookTaskIsGet("30") || !MainTW.MainPS.LookTaskIsGet("61") || !MainTW.MainPS.LookTaskIsGet("62")  || !MainTW.MainPS.LookTaskIsGet("504") ){
			return true;
		}	
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info747"));
		return false;
	}

	function canOpenViewAsTaskID(id : String) : boolean{
		if(!MainTW.MainPS.LookTaskIsGet(id) || !MainTW.MainPS.LookTaskIsDone(id) ){
			return true;
		}
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info747"));
		return false;
	}
	//提示该功能暂未开启
	function TsNotO () {
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info778"));
}

	var mainTween : TweenPosition;
	var isGo : boolean = false;
	//var uiallPC : UIAllPanelControl;
	var Objshulan : GameObject;
	var Objdiban : GameObject;
	var ButtonZhu : Transform;
	var ButtonZhuP : UIPanel;
	var ButtonJineng : Transform;
	var buttonJinengParent : Transform;
	var buttonJinengP : UIPanel;
	var boolisBag : boolean = false;
	var mTime : float = 0;
	var TransMainUI : Transform;
	var objMainUI : GameObject;
	var MainUIOn : boolean = false;
	function SetMainUI(obj : GameObject){
		objMainUI = obj;
	}
	
	function MainTweenMove(){
		if(Application.loadedLevelName == "Map200" || mTime > Time.time){
			return;
		}
		if(mapType == MapType.fuben || mapType == MapType.jingjichang){
			AllManage.tsStatic.Show("info772");
			return;
		}
		isShow = true;
		if(canOpenViewAsTaskID()){
			if(! objMainUI){
				MainUIOn = true;
				var preMainUIl = Resources.Load("Anchor - MainUI", GameObject);
				objMainUI = GameObject.Instantiate(preMainUIl);
				objMainUI.transform.parent = TransMainUI;
				objMainUI.transform.localPosition = Vector3.zero;
			}
			while(TransPlayerInfo[1] == null){
				yield;				
			}
			yield;	
			yield;	
			AllManage.qrStatic.ZhuangBei.SetActiveRecursively(false);
			TransPlayerInfo[1].localPosition.y = 3000;
			if(!isGo){
				objMainUI.active = true;
				objMainUI.transform.localPosition = Vector3.zero;
				mTime = Time.time + 0.4;
				if(mapType == MapType.zhucheng){
				   ButtonZhuP.enabled=false;
					ButtonZhu.localPosition.y = -1000;
					buttonJinengP.enabled=false;
					ButtonJineng.localPosition.y = 1000;
				}else{
				    buttonJinengP.enabled=true;
					ButtonJineng.localPosition.y = 0;
				}
				AllManage.UIALLPCStatic.showThisPanel("Bag");
				Objshulan.SetActiveRecursively(true);
				Objdiban.SetActiveRecursively(true);
		//		mainTween.gameObject.SetActiveRecursively(true); 
				ButtonJineng.localPosition.z = 400;
				buttonJinengParent.localPosition.z = -500;
				yield;
				MoveDaoHang8();
		//		mainTween.Play(true);
				isGo = true;
				boolisBag = true;
				invcl.QieHuanBagBaoguo();
			}else{
				mTime = Time.time + 0.4;
				AllManage.UIALLPCStatic.show0();
				buttonJinengP.enabled=true;
				ButtonZhuP.enabled=false;
				ButtonJineng.localPosition.y = 0;
				Objshulan.SetActiveRecursively(false);
				Objdiban.SetActiveRecursively(false);
		//		mainTween.gameObject.SetActiveRecursively(false);
				buttonJinengParent.localPosition.z = 400;
				ButtonJineng.localPosition.z = 600;
				ButtonZhu.localPosition.y = -1000;
		//		mainTween.Play(false);
				isGo = false;
				if(mapType == MapType.zhucheng){
					ButtonZhuP.enabled=true;
					ButtonZhu.localPosition.y = 0;
					buttonJinengP.enabled=false;
					ButtonJineng.localPosition.y = 1000;
				}
				closeNewPlayerInfo();
				TeamObj.SetActiveRecursively(false);
			}
			if(TransStores[0])
				TransStores[0].position.x = PosStores[0];
//			TransStores[1].position.x = PosStores[1];
			TransStores[1].localPosition.x = 0;
			TransStores[2].localPosition.x = 0;
			TransStores[3].localPosition.x = 0;
			if(AllManage.invCangKuStatic)
			AllManage.invCangKuStatic.transCangku.localPosition.y = 1000;
		}
	}

	function closeNewPlayerInfo(){
		for(var i=0; i<ParentYuan.length ; i++){
			if(ParentYuan[i] != null && ParentYuan[i].active){
				ParentYuan[i].SetActiveRecursively(false);
			}
		}	
	}

	function reMoveGround(){
	//		uiallPC.showThisPanel("Ground");
		if(boolisBag){
			buttonJinengP.enabled=true;
			ButtonZhuP.enabled=false;
			ButtonJineng.localPosition.y = 0;
			Objshulan.SetActiveRecursively(false);
			Objdiban.SetActiveRecursively(false);
	//		mainTween.gameObject.SetActiveRecursively(false);
			buttonJinengParent.localPosition.z = 400;
			ButtonJineng.localPosition.z = 600;
			ButtonZhu.localPosition.y = -1000;
	//		mainTween.Play(false);
			isGo = false;
			if(mapType == MapType.zhucheng){
				ButtonZhuP.enabled=true;
				ButtonZhu.localPosition.y = 0;
				buttonJinengP.enabled=false;
				ButtonJineng.localPosition.y = 1000;
			}
			boolisBag = false;
		}
	}

	function sdGuiWei(){
		if(TransStores[0])
			TransStores[0].localPosition.x = 0;
//		TransStores[1].position.x = PosStores[1];
		TransStores[1].localPosition.x = 0;
		TransStores[2].localPosition.x = 0;
		TransStores[3].localPosition.x = 0;
	}
	function returnLevel(){
	//	PhotonNetwork.LeaveRoom();
		if(Application.loadedLevelName == "Map200")
			return;
	 	if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(Application.loadedLevelName == "Map311"){
		InRoom.GetInRoomInstantiate().BattlefieldExit();
		}
		
		if(Application.loadedLevelName == "Map911" || Application.loadedLevelName == "Map912"){
			nowYesReturn();
			return;
		}
		
		if(Application.loadedLevelName == "Map721"){
			AllManage.qrStatic.ShowQueRen(gameObject , "TowerReturn" , "NoReturn" , AllManage.AllMge.Loc.Get("meg0157"));		
			return;
		}
		if(mapType == MapType.fuben){
		AllManage.isOutMainMap = true;
		}
		if(nowmapState == 0){
		
			if(mapType == MapType.fuben && !AllManage.dungclStatic.isDungeonClear()){
				AllManage.qrStatic.ShowQueRen(gameObject , "ReturnTownNoBoss" , "" , "info769");
			}else{
				if(false){
					AllManage.qrStatic.ShowQueRen(gameObject , "YesReturn" , "NoReturn" , AllManage.AllMge.Loc.Get("info287") + (parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) * 200) + AllManage.AllMge.Loc.Get("info335") + AllManage.AllMge.Loc.Get("buttons485"));
	//				AllManage.qrStatic.ShowQueRen(gameObject , "YesReturn" , "NoReturn" , "messages009");
				}else{
					if( !AllManage.dungclStatic.canBackTown() && ! AllManage.dungclStatic.rewardIsDone){
						AllManage.tsStatic.Show("info765");
					}else{
						Loading.Level = DungeonControl.ReLevel;
						InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
						InRoom.GetInRoomInstantiate().PVP1InviteRemove();			
						InRoom.GetInRoomInstantiate().PVPTeamDissolve();
						AllManage.UICLStatic.RemoveAllTeam();
						if(mapType == MapType.jingjichang){
							InRoom.GetInRoomInstantiate().LeaveRoom(PhotonNetwork.room.name);
							InRoom.GetInRoomInstantiate().ActivityPVPRemove();
						}
						InRoom.GetInRoomInstantiate().RemoveTempTeam();
						alljoy.DontJump = true;
						yield;
						PhotonNetwork.LeaveRoom();
						
						if(mapType == MapType.jingjichang){
							InRoom.GetInRoomInstantiate().BattlefieldExit();
						}
						InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
							AllResources.ar.AllLoadLevel("Loading 1");
	//	PhotonNetwork.LoadLevel("Loading 1");

					}
				}
			}
		}else{
			nowYesReturn();
		}
	}
	
	function TowerReturn(){
		WowerFloorlose();
		nowYesReturn();
	}
	
	function ReturnTownNoBoss(){
		yield;
		yield;
		yield;
	 	if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
			if(false){
				AllManage.qrStatic.ShowQueRen(gameObject , "YesReturn" , "NoReturn" , AllManage.AllMge.Loc.Get("info287") + (parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) * 200) + AllManage.AllMge.Loc.Get("info335") + AllManage.AllMge.Loc.Get("buttons485"));
			}else{
				if( !AllManage.dungclStatic.canBackTown() && ! AllManage.dungclStatic.rewardIsDone){
					AllManage.tsStatic.Show("info765");
				}else{
//					InRoom.GetInRoomInstantiate().RemoveTempTeam();
					Loading.Level = DungeonControl.ReLevel;
					InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
					InRoom.GetInRoomInstantiate().PVP1InviteRemove();			
					InRoom.GetInRoomInstantiate().PVPTeamDissolve();
					AllManage.UICLStatic.RemoveAllTeam();
					if(mapType == MapType.jingjichang){
						InRoom.GetInRoomInstantiate().LeaveRoom(PhotonNetwork.room.name);
						InRoom.GetInRoomInstantiate().ActivityPVPRemove();
					}
					InRoom.GetInRoomInstantiate().RemoveTempTeam();
					alljoy.DontJump = true;
					yield;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
				PhotonNetwork.LeaveRoom();
					AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

				}
			}
	}
	
	function YesReturn(){ 
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesReturn , parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) , 0 , "" , gameObject , "realYesReturn");
//		AllManage.AllMge.UseMoney(parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) * 200 , 0 , UseMoneyType.YesReturn , gameObject , "realYesReturn");
	//	if(ps.UseMoney(0,2)){
	//	}
	}

	function realYesReturn(){
			if( !AllManage.dungclStatic.canBackTown()){
				AllManage.tsStatic.Show("info765");
			}else{
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.FightingOut).ToString());
				AllManage.UICLStatic.RemoveAllTeam();
				Loading.Level = DungeonControl.ReLevel;
//				InRoom.GetInRoomInstantiate().RemoveTempTeam();
				InRoom.GetInRoomInstantiate().PVP1InviteRemove();			
				InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
				InRoom.GetInRoomInstantiate().PVPTeamDissolve();
				if(mapType == MapType.jingjichang){
					InRoom.GetInRoomInstantiate().LeaveRoom(PhotonNetwork.room.name);
					InRoom.GetInRoomInstantiate().ActivityPVPRemove();
				}
				alljoy.DontJump = true;
				yield;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
				PhotonNetwork.LeaveRoom();
				AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

			}
	}
	
	function nowYesReturn(){
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.FightingOut).ToString());
		AllManage.UICLStatic.RemoveAllTeam();
		Loading.Level = DungeonControl.ReLevel;
//		InRoom.GetInRoomInstantiate().RemoveTempTeam();
		InRoom.GetInRoomInstantiate().PVP1InviteRemove();			
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		InRoom.GetInRoomInstantiate().PVPTeamDissolve();
		if(mapType == MapType.jingjichang){
			InRoom.GetInRoomInstantiate().LeaveRoom(PhotonNetwork.room.name);
			InRoom.GetInRoomInstantiate().ActivityPVPRemove();
		}
		alljoy.DontJump = true;
		yield;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			PhotonNetwork.LeaveRoom();
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");
		
	}
	
	function NoReturn(){

	}

	function show0(){
		AllManage.UIALLPCStatic.show0();
	}

	var NPCPanel1 : UIPanel;
	var NPCPanel2 : UIPanel;
	var ObjBackGroundPar : ParticleSystem;
	function SetObj(AS : AwakeSet){
		TweenTask = AS.TweenTask;
		LabeltaskNPC = AS.LabeltaskNPC ;
		Labeltasktitle = AS.Labeltasktitle ;
		LabeltaskPlayer = AS.LabeltaskPlayer ;
		TaskNCL = AS.TaskNCL ;
		SpriteNPC = AS.SpriteNPC ;
		LabelButtonStep = AS.LabelButtonStep ;
		ShowFindWayButton = AS.ShowFindWayButton ;
		NPCtaskItem = AS.NPCtaskItem ;
		NPCGroup = AS.NPCGroup ;
		parentTaskInfo = AS.parentTaskInfo ;
		ButtonNextStep = AS.ButtonNextStep ;
		rewardObj = AS.rewardObj ;
		ButtonGo = AS.ButtonGo ;
		LabelNPCTalk = AS.LabelNPCTalk ;
		LabelNPCTalk1 = AS.LabelNPCTalk1 ;
		LableNPCName = AS.LableNPCName ;
		LableNPCTitleName = AS.LableNPCTitleName ;
		LabelNPCTouXiangName = AS.LabelNPCTouXiangName ;
		NPCFunctionsButton = AS.NPCFunctionsButton ;
		refOnePvp = AS.refOnePvp ;
		ButtonJingJi = AS.ButtonJingJi ;
		ButtonZhanChang = AS.ButtonZhanChang ; 
		OnePvpList = AS.OnePvpList ;
		Battlefield1 = AS.Battlefield1 ;
		Ylist1 = AS.Ylist1 ;
		Ylist2 = AS.Ylist2 ;
		NPCPanel1 = AS.NPCPanel1 ;
		NPCPanel2 = AS.NPCPanel2 ;
		
	}

	var TP : TransactionParameters;
	function getPlayerParameters(parameters : TransactionParameters){
		TP = parameters;
		JiaoYi(TP.playerID);
	}

	var BFI : ButtonForwardInfo;
	function JiaoYi(from : String){
		BFI.ShowInfoButton(gameObject , "isJiaoYi");
	//	//print("jiao yi le" + gameObject.name);
	}

	//var QR : QueRen;
	function isJiaoYi(){
		AllManage.qrStatic.ShowQueRen1(gameObject , "YesJiaoYi" , "NoJiaoYi" , TP.playerName +  AllManage.AllMge.Loc.Get("info282"));
	}

	var BtnGameManaB : BtnGameManagerBack;
	function YesJiaoYi(){
	//	//print(TP.requstType + " === " + TP.playerID);
		BtnGameManaB.SongSendTransactionRequest(yuan.YuanPhoton.ReturnCode.Yes , TP.requstType ,TP.playerID , "DarkSword2" , "PlayerInfo" );
	}
	 
	function NoJiaoYi(){
		BtnGameManaB.SongSendTransactionRequest(yuan.YuanPhoton.ReturnCode.No , TP.requstType ,TP.playerID , "DarkSword2" , "PlayerInfo" );
	}

	function beijujue(name : String){
		AllManage.tsStatic.Show1(name + AllManage.AllMge.Loc.Get("messages155"));
	}

	var TagetName : String;
	var TagetID : String;
	var TagetPro : String;
	var TransactionC : TransactionControl;
	var tween : TweenPosition;
	function ShowJiaoYi(id : String){

		AllManage.UIALLPCStatic.show25();
		yield;
		yield;
		yield;
		tween.Play(false);
		if(TP == null){
			TP = new TransactionParameters();
				TP.playerName = TagetName;
				TP.playerID = TagetID;
	//			//print("ssssss");
		}
	//	//print(TP.playerName);
		TransactionC.ShowJiaoYi(TP.playerName , TP.playerID , id , TagetPro);
	}

	function produceReslist(){
		if(Application.loadedLevelName == "Map200" || mTime > Time.time){
			return;
		}
		if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.EqupmentBuildSwitch) == "0"){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
			return;
		}
//		if(InventoryControl.yt.Rows[0]["isOpenMake"].YuanColumnText == "1"){
			if(canOpenViewAsTaskID("13")){
				AllManage.UIALLPCStatic.show38();
				if(AllManage.inventoryProduceStatic){
					AllManage.inventoryProduceStatic.resetList();
				}
			}
//		}else{
//			if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
//				AllManage.qrStatic.ShowBuyQueRen(gameObject ,"yesZhiZao" , "" , "messages018");	
//			else
//				yesZhiZao();
//		}
	}
	
	
	function produceReslist1(){
		if(Application.loadedLevelName == "Map200" || mTime > Time.time){
			return;
		}
//		if(InventoryControl.yt.Rows[0]["isOpenMake"].YuanColumnText == "1"){
			if(canOpenViewAsTaskID("127")){
				AllManage.UIALLPCStatic.show3();
				if(AllManage.inventoryProduceStatic){
					AllManage.inventoryProduceStatic.resetList();
				}
			}
//		}else{
//			if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
//				AllManage.qrStatic.ShowBuyQueRen(gameObject ,"yesZhiZao" , "" , "messages018");	
//			else
//				yesZhiZao();
//		}
	}


	function yesZhiZao(){
			if(ps == null && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesManufacture , 0 , 0 , "" , gameObject , "realyesZhiZao");
//			AllManage.AllMge.UseMoney(0 , 20 , UseMoneyType.YesManufacture , gameObject , "realyesZhiZao");
	//		if(ps.UseMoney(0 , 20)){
	//		}
	}

	function realyesZhiZao(){
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.OpenMakeBtn).ToString());
				InventoryControl.yt.Rows[0]["isOpenMake"].YuanColumnText = "1"; 
				produceReslist();

	}


	function kaiQiSoul(){
//		if(InventoryControl.yt.Rows[0]["isOpenSoul"].YuanColumnText == "1"){
			AllManage.UIALLPCStatic.show17();
//		}else{
//			if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
//				AllManage.qrStatic.ShowBuyQueRen(gameObject ,"yesSoul" , "" , "messages019");	
//			else
//				yesSoul();
//		}
	}

	function yesSoul(){
			if(ps == null && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesSoul , 0 , 0 , "" , gameObject , "realyesSoul");
//			AllManage.AllMge.UseMoney(0 , 30 , UseMoneyType.YesSoul , gameObject , "realyesSoul");
	//		if(ps.UseMoney(0 , 30)){
	//		}
	}

	function realyesSoul(){
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.OpenMakeSoulBtn).ToString());
		InventoryControl.yt.Rows[0]["isOpenSoul"].YuanColumnText = "1"; 
		kaiQiSoul();
	}

	function kaiBaoShi(){
		if(Application.loadedLevelName == "Map200" || mTime > Time.time){
			return;
		}
//		if(InventoryControl.yt.Rows[0]["isOpenInlay"].YuanColumnText == "1"){
			if(canOpenViewAsTaskID("128")){
				AllManage.UIALLPCStatic.show2();
			}
//		}else{
//			if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
//				AllManage.qrStatic.ShowBuyQueRen(gameObject ,"yesBaoShi" , "" , "messages020");	
//			else
//				yesBaoShi();
//		}
	} 


	function kaiBaoShi1(){
		if(Application.loadedLevelName == "Map200" || mTime > Time.time){
			return;
		}
		if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.EqupmentHoleSwitch) == "0"){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
			return;
		}
//		if(InventoryControl.yt.Rows[0]["isOpenInlay"].YuanColumnText == "1"){
			if(canOpenViewAsTaskID("128")){
				AllManage.UIALLPCStatic.show18();
			}
//		}else{
//			if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
//				AllManage.qrStatic.ShowBuyQueRen(gameObject ,"yesBaoShi" , "" , "messages020");	
//			else
//				yesBaoShi();
//		}
	} 
	
	function yesBaoShi(){
			if(ps == null && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesGem , 0 , 0 , "" , gameObject , "realyesBaoShi");
//			AllManage.AllMge.UseMoney(0 , 50 , UseMoneyType.YesGem , gameObject , "realyesBaoShi");
	//		if(ps.UseMoney(0 , 50)){
	//		}
	}

	function realyesBaoShi(){
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.GemBtnOpen).ToString());
				InventoryControl.yt.Rows[0]["isOpenInlay"].YuanColumnText = "1"; 
				kaiBaoShi();	
	}

	function cookReslist(){ 
		if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.CookSwitch) == "0"){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
			return;
		}
//		if(InventoryControl.yt.Rows[0]["isOpenCooking"].YuanColumnText == "1"){
			AllManage.UIALLPCStatic.show5();
			if(AllManage.cookCLStatic){
				yield;
				yield;
				yield;
				AllManage.cookCLStatic.ReLoadFish();
			}
//		}else{
//			if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
//				AllManage.qrStatic.ShowBuyQueRen(gameObject ,"yesCook" , "" , "messages021");	
//			else
//				yesCook();
//		}
	}

	function yesCook(){
			if(ps == null && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.yesCook , 0 , 0 , "" , gameObject , "realyesCook");
//			AllManage.AllMge.UseMoney(0 , 2 , UseMoneyType.yesCook , gameObject , "realyesCook");
	//		if(ps.UseMoney(0 , 2)){
	//		}
	}

	function realyesCook(){
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.OpenCookBtn).ToString());
		InventoryControl.yt.Rows[0]["isOpenCooking"].YuanColumnText = "1"; 
		cookReslist();
	}

	function RemoveAllTeam(){
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		InRoom.GetInRoomInstantiate().ActivityPVPRemove();	
		InRoom.GetInRoomInstantiate().LegionOneRemove();	
		InRoom.GetInRoomInstantiate().PVP1InviteRemove();	
//		InRoom.GetInRoomInstantiate().RemoveTempTeam();	
		InRoom.GetInRoomInstantiate().PVPTeamDissolve();	
		InRoom.GetInRoomInstantiate().LegionRemove(ps.PlayerID.ToString());	
	}

	function CopyRoomSPTable(){
	}

	static var ytFuben :  yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("ytFuben","PlayerID");
	static var isYtFuben : boolean = false;
	function YaoQingFuben(useyt : yuan.YuanMemoryDB.YuanTable){
		if(mapType == MapType.jingjichang || mapType == MapType.yewai){
			AllManage.tsStatic.Show("tips086");
		}else{
			if(!isYtFuben){
				ytFuben = useyt;
			//					if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
			//						AllManage.qrStatic.ShowBuyQueRen(gameObject ,"YesMaiRu" , "NoMai" , "是否花费"+ItemMove.mInv.costGold+"金币," +ItemMove.mInv.costBlood+"血石购买"+ItemMove.mInv.itemName+"？");	
			//					else
			//						YesMaiRu();
				AllManage.qrStatic.ShowQueRen(gameObject , "YesFuben" , "" , "messages010");
			}else{
				AllManage.tsStatic.Show("tips087");
			}
		}
	}

	function YesFuben(){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(parseInt(ps.Power) >= 10 && ps.isBlood(50)){
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesDuplicate , 0 , 0 , "" , gameObject , "");
//			AllManage.AllMge.UseMoney(0 , 50 , UseMoneyType.YesDuplicate , gameObject , "");
	//		ps.UseMoney(0 , 50);
//			ps.AddPower(10);
			InRoom.GetInRoomInstantiate().Coststrength(yuan.YuanPhoton.CostPowerType.InviteShadowDemon , 0 , 0 , "");
			var obj : GameObject;
			obj = AllResources.ar.CreatePlayerFuBen(parseInt(ytFuben.Rows[0]["ProID"].YuanColumnText) , myTeamInfo , ytFuben.Rows[0]["PlayerName"].YuanColumnText , 	PlayerStatus.MainCharacter.position);
		//	obj.SendMessage("SetIsMine" , false , SendMessageOptions.DontRequireReceiver);
		    obj.SendMessage("SetMaster",PlayerStatus.MainCharacter, SendMessageOptions.DontRequireReceiver);
			obj.SendMessage("SetPlayerInfoAsYt" , ytFuben , SendMessageOptions.DontRequireReceiver);
			isYtFuben = true;
		}
	}

	static var ytPVP :  yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("ytFuben","PlayerID");
	private var FStr : String = ";";
	private var useFstrs : String[];
	function YaoQingPVP(useyt : yuan.YuanMemoryDB.YuanTable){
		ytPVP = useyt;
		useFstrs = InventoryControl.yt.Rows[0]["pvp1PlayerID"].YuanColumnText.Split(FStr.ToCharArray());
		for(var i=0; i<useFstrs.length; i++){
			if(useFstrs[i] == ytPVP.Rows[0]["PlayerID"].YuanColumnText){
				AllManage.tsStatic.Show("tips088");
				return;
			}
		}
		if(parseInt(ytPVP.Rows[0]["PlayerLevel"].YuanColumnText) >=10){
			if(parseInt(ytPVP.Rows[0]["pvp1BeNum"].YuanColumnText) < 10){
				if(ps == null && PlayerStatus.MainCharacter){
					ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
				}
				if(ps.pvpNum < 10){
					AllManage.qrStatic.ShowQueRen1(gameObject , "YesPVP" , "" , AllManage.AllMge.Loc.Get("info283") + (10-ps.pvpNum));	
				}else{
					if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1){
						AllManage.qrStatic.ShowQueRen1(gameObject , "YesChongPVP" , "" , AllManage.AllMge.Loc.Get("messages160") + BtnGameManager.dicClientParms["YesResetPVP"] + AllManage.AllMge.Loc.Get("messages161"));
					}else{
						YesChongPVP();
					}
				//	ts.Show("该功能未恢复。剩余可邀请次数:0");
				}
			}else{
				AllManage.tsStatic.Show("tips089");
			}
		}else{
				AllManage.tsStatic.Show("tips090");
		}
	}
	
	function YaoQingPVPRobotytShaDow(useyt : yuan.YuanMemoryDB.YuanTable){
		ytPVP = useyt;
		ytPVP.Rows[0]["GuildID"].YuanColumnText = "";
		AllManage.qrStatic.ShowQueRen1(gameObject , "YesPVP" , "" , AllManage.AllMge.Loc.Get("info283") + (10-ps.pvpNum));	
	}
	
	function YesChongPVP(){
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesResetPVP , 0 , 0 , "" , gameObject , "realYesChongPVP");
//		AllManage.AllMge.UseMoney(0 , 50 , UseMoneyType.YesResetPVP , gameObject , "realYesChongPVP");
	//	if(ps.UseMoney(0 , 50)){
	//	}
	}

	function realYesChongPVP(){
		if(InventoryControl.yt.Rows[0]["PvPYingmoTimes"].YuanColumnText != "0"){
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.RefreshOfflinePlayerNum).ToString());
			ps.pvpNum = 0;
			InventoryControl.yt.Rows[0]["pvp1Num"].YuanColumnText = "0";
			yield WaitForSeconds(1);
			AllManage.tsStatic.Show("tips091");
		}else{
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages162"));
		}

	}

	static var PVP321 : boolean = false;
	static var GuildLevel : int = -1;
	var ytGuild : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("ytGuildxxx","id");

	function YesPVP(){
		InventoryControl.yt.Rows[0]["pvp1PlayerID"].YuanColumnText = InventoryControl.yt.Rows[0]["pvp1PlayerID"].YuanColumnText + ytPVP.Rows[0]["PlayerID"].YuanColumnText + ";";
		var GuildID : String;
		GuildID = ytPVP.Rows[0]["GuildID"].YuanColumnText;
		if(GuildID != ""){
//			AllManage.tsStatic.RefreshBaffleOn();
			//InRoom.GetInRoomInstantiate().GetYuanTable("select GuildLevel from GuildInfo where id = " + GuildID , "DarkSword2", ytGuild);
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate ().GetTableForID (GuildID,yuan.YuanPhoton.TableType.GuildInfo,ytGuild));
//			;
			while(ytGuild.IsUpdate){
				yield;
			}
			AllManage.tsStatic.RefreshBaffleOff();
			GuildLevel = parseInt(ytGuild.Rows[0]["GuildLevel"].YuanColumnText);
		}else{
			GuildLevel = -1;
		}
	//	ArenaID = str[0];
	//	Loading.loadstr = str[0];
		InventoryControl.yt.Rows[0]["pvp1Num"].YuanColumnText = (ps.pvpNum + 1).ToString();
		while(InventoryControl.yt.IsUpdate){
			yield;
		}
		InRoom.GetInRoomInstantiate().UpdateYuanTable("DarkSword2",InventoryControl.yt,SystemInfo.deviceUniqueIdentifier);
		while(InventoryControl.yt.IsUpdate){
			yield;
		}
		//print(InventoryControl.yt.Rows[0]["BankInventory1"].YuanColumnText);
		//print(InventoryControl.yt.Rows[0]["BankInventory2"].YuanColumnText);
		//print(InventoryControl.yt.Rows[0]["BankInventory3"].YuanColumnText);
		//print(InventoryControl.yt.Rows[0]["BankInventory4"].YuanColumnText);
	//	//print(InventoryControl.yt.Rows[0]["pvp1Num"].YuanColumnText + " == pvp1num");
		myTeam = "0";
		otherTeam = "5";
		myTeamInfo = "0";
		ArenaControl.Battlefieldtype = "";
		Loading.Level = "Map321";
		ArenaControl.areType = 0;
		PVP321 = true;
		if(mapType == MapType.zhucheng)
			DungeonControl.ReLevel = Application.loadedLevelName;
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			PhotonNetwork.LeaveRoom();
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}

	function YesStartPVP(){
		var obj : GameObject;
		while(PlayerStatus.MainCharacter == null){
			yield;
		}
		var endposition = GameObject.FindWithTag("end");
		obj = AllResources.ar.CreatePlayerFuBen(parseInt(ytPVP.Rows[0]["ProID"].YuanColumnText) , myTeamInfo , ytPVP.Rows[0]["PlayerName"].YuanColumnText , endposition.transform.position);
	//	obj.SendMessage("SetIsMine" , false , SendMessageOptions.DontRequireReceiver);
		obj.SendMessage("SetPlayerInfoAsYt" , ytPVP , SendMessageOptions.DontRequireReceiver);
		obj.tag = "Enemy";
		PVP321 = false;
	}

	function OpenOfflinePlayer(){
		yingmoOverMoveOn();			
	}

	function SongLoadLevel(level : int){
//		print("uicontrol ======= " + this.name);
		alljoy.DontJump = true;
		AllResources.ar.myLoadLevel(level);
		AllResources.isLoadGUI = false;
//		Destroy(PlayerStore.me);
	}

	var boolNoWait : boolean = false;
	function StartlookShowZhuButtonsNoWait(rtime : int){
		var times : int = 0;
		times = rtime;
		var i : int = 0;
		var bool : boolean = false;
		for(i=1; i<ButtonsShowZhu.length; i++){
			bool = GetRealButtonsActiveAsID(i , oldLevel , oldTsk , buttonsActive);
			if(!bool || InRoom.GetInRoomInstantiate().GetServerSwitchString(ButtonsTypeShowZhu[i]) == "0"){
				ButtonsShowZhu[i].SetActiveRecursively(false);
				BoolButtonWasOpen[i-1] = false;
			}else{
				BoolButtonWasOpen[i-1] = true;
			}
		} 
		
		for(i=1; i<ButtonsGroundShowZhu.length; i++){
			bool = GetRealButtonsActiveAsID(i , oldLevel , oldTsk , buttonsGroundActive);
			if(!bool || InRoom.GetInRoomInstantiate().GetServerSwitchString(ButtonsTypeAllGroundShowZhu[i]) == "0"){
				ButtonsGroundShowZhu[i].SetActiveRecursively(false);
				if(i == 1){
					BoolButtonWasOpen[6] = false;
				}
			}else{
				if(i == 1){
					BoolButtonWasOpen[6] = true;
				}		
			}
		}
//		print("NoWait ---- Close == " + rtime);
		yield WaitForSeconds(0.1);
		if(times == startButtonTime && mapType == MapType.zhucheng){
			if(lookShouZhuButtonAll.active){
			while(!PlayerStatus.MainCharacter){
	             yield;
	             }
				if(MainPlayerS == null){
					MainPlayerS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
				}
				for(i=1; i<ButtonsShowZhu.length; i++){
					bool = GetRealButtonsActiveAsID(i ,  parseInt(AllManage.psStatic.Level) , MainPlayerS.player.doneTaskID , buttonsActive);
					if(bool){
	//					//print(ButtonsShowZhu[i].active + " == " + i);
						if(! ButtonsShowZhu[i].active && InRoom.GetInRoomInstantiate().GetServerSwitchString(ButtonsTypeShowZhu[i]) == "1"){
							AllManage.UIALLPCStatic.showExhibition37();
							yield;
							yield;
							if(!boolNoWait)
							AllManage.exhbtControl.OpenMe(BoolButtonWasOpen , i-1);
							boolNoWait = false;
							ButtonsShowZhu[i].SetActiveRecursively(true);
							if(ButtonsShowZhu[i].GetComponentInChildren(ParticleEmitter))
								ButtonsShowZhu[i].GetComponentInChildren(ParticleEmitter).emit=true;
							yield WaitForSeconds(0.1); 
							if(ButtonsShowZhu[i].GetComponentInChildren(ParticleEmitter))
								ButtonsShowZhu[i].GetComponentInChildren(ParticleEmitter).emit=false;
						}
					}
				}
				oldTsk = MainPlayerS.player.doneTaskID;
				oldLevel = parseInt(AllManage.psStatic.Level);
			}
			if(lookShouZhuGroundButtonAll.active){
			  while(!PlayerStatus.MainCharacter){
	                yield;
	               }
	               yield;
				if(MainPlayerS == null){
					MainPlayerS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
				}
				for(i=1; i<ButtonsGroundShowZhu.length; i++){
					bool = GetRealButtonsActiveAsID(i ,  parseInt(AllManage.psStatic.Level) , MainPlayerS.player.doneTaskID , buttonsGroundActive);
					if(bool){
	//					//print(ButtonsGroundShowZhu[i].active + " == " + i);
						if(! ButtonsGroundShowZhu[i].active && InRoom.GetInRoomInstantiate().GetServerSwitchString(ButtonsTypeAllGroundShowZhu[i]) == "1"){
							ButtonsGroundShowZhu[i].SetActiveRecursively(true);
							if(i == 1){
								AllManage.UIALLPCStatic.showExhibition37();
								yield;
								yield;
								if(!boolNoWait)
								AllManage.exhbtControl.OpenMe(BoolButtonWasOpen , 6);		
								boolNoWait = false;					
							}	
	//						ButtonsGroundShowZhu[i].GetComponentInChildren(ParticleEmitter).emit=true;
	//						yield WaitForSeconds(0.1); 
	//						ButtonsGroundShowZhu[i].GetComponentInChildren(ParticleEmitter).emit=false;
						}
					}
				}
				oldTsk = MainPlayerS.player.doneTaskID;
				oldLevel = parseInt(AllManage.psStatic.Level);
			}
		}
	}

	function UpDateLevelShowZhuButtonsNoWait(){
		startButtonTime += 1;
		StartlookShowZhuButtonsNoWait(startButtonTime);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	function ShowAllTeamPlayer(){
		if(mapType == MapType.jingjichang || mapType == MapType.fuben){
			SetTeamList("");
		}
	}

	var tmItem : TeamItem;
	var tmItemArray : TeamItem[];
	var tmParent : Transform; 
	var tmGID : UIGrid;
	var usetmID : String[];
	var TransTeamParentSay : Transform;
	var scenePS : PlayerStatus[]; 
	function SetTeamList(equStr : String){
//		print(equStr);
		if(mapType == MapType.zhucheng || mapType == MapType.yewai){
			return;
		}
		yield;
		tmClear();	
		scenePS = FindObjectsOfType(PlayerStatus);
		var i : int = 0;	
		var bool : boolean = true;
		for(i=0; i<scenePS.length; i++){
	//		if(useInvID[i] != "" && parseInt(useInvID[i].Substring(0,1)) == 8 && parseInt(useInvID[i].Substring(1,1)) == 2){
			if(mapType == MapType.jingjichang){
				if(scenePS[i].myteaminfo == myTeamInfo){
					bool = true;
				}else{
					bool = false;
				}
			}	
//			print( scenePS[i].PlayerID + " == " + ps.PlayerID);
			if(bool && scenePS[i] && ps && scenePS[i].PlayerID != ps.PlayerID && scenePS[i].instanceID > 0){
				var Obj : GameObject = Instantiate(tmItem.gameObject); 
				Obj.transform.parent = tmParent;
				var useEBI : TeamItem;
				useEBI = Obj.GetComponent(TeamItem);
				useEBI.SetTeamPlayerAsPS(scenePS[i]);
				useEBI.uicl = this; 
				addtmItem(useEBI);	
				if(mapType != MapType.zhucheng)
				TransTeamParentSay.localPosition.y = 0;
			}		
	//		}
		}
		yield;
	//	JianChaKaoyu();
		tmGID.repositionNow = true;
	}

	function addtmItem(tm : TeamItem){
		var use : TeamItem[]; 
		use = tmItemArray; 
		tmItemArray = new Array(tmItemArray.length + 1);
		for(var i=0; i<(tmItemArray.length - 1); i++){
			tmItemArray[i] = use[i];
		} 
		tmItemArray[tmItemArray.length - 1] = tm;
	}

	function tmClear(){
		for(var i=0; i<tmItemArray.length; i++){
			if(tmItemArray[i]){
				Destroy(tmItemArray[i].gameObject);
			}
		}
		tmItemArray = new Array(0);
	}

	function SelectPlayerSendMessage(from : String , str : String){
	//	//print(from);
		for(var i=0; i<(tmItemArray.length); i++){
	//		//print(from + " == " + tmItemArray[i].LabelName.text);
			if(tmItemArray[i].LabelName.text == from){
				tmItemArray[i].SetShowMessage(from , str);
			}
		} 
	}

	private var teamTimesUp : int = 0;
	var TransTeamMessageUp : Transform;
	var LabelTeamUp : UILabel;
	var SpriteTeamUp : UISprite;
	function TeamMessageUp(str : String){
		TransTeamMessageUp.localScale = Vector3(1,1,1);
		var myTimes : int = 0;
		
		LabelTeamUp.text = str;
		teamTimesUp += 1;
		myTimes = teamTimesUp;
			LabelTeamUp.enabled = false;
			SpriteTeamUp.enabled = false;
			LabelTeamUp.enabled = true;
			SpriteTeamUp.enabled = true;
		yield WaitForSeconds(2);
		if(myTimes == teamTimesUp){
			LabelTeamUp.enabled = false;
			SpriteTeamUp.enabled = false;
			LabelTeamUp.enabled = true;
			SpriteTeamUp.enabled = true;
			TransTeamMessageUp.localScale = Vector3(0,0,0);		
		}
	}

	private var teamTimesDown : int = 0;
	var TransTeamMessageDown : Transform;
	var LabelTeamDown : UILabel;
	var SpriteTeamDown : UISprite;
	function TeamMessageDown(str : String){
		TransTeamMessageDown.localScale = Vector3(1,1,1);
		var myTimes : int = 0;

		LabelTeamDown.text = str;
		teamTimesDown += 1;
		myTimes = teamTimesDown;
			LabelTeamDown.enabled = false;
			SpriteTeamDown.enabled = false;
			LabelTeamDown.enabled = true;
			SpriteTeamDown.enabled = true;
		yield WaitForSeconds(2);
		if(myTimes == teamTimesDown){
			LabelTeamDown.enabled = true;
			SpriteTeamDown.enabled = true;
			LabelTeamDown.enabled = false;
			SpriteTeamDown.enabled = false;
			TransTeamMessageDown.localScale = Vector3(0,0,0);		
		}
	}

	private var tipsInv : InventoryItem;
	var contShowCategoryTips : boolean = true;
	function CategoryTipsAsID(id : String){
		if(!contShowCategoryTips){
			contShowCategoryTips = true;
			return;
		}
		if(id.Length >= 4){
			if(id.Substring(0,2) == "88"){
				for(var rows : yuan.YuanMemoryDB.YuanRow in invcl.GameItem.Rows){
					if(rows["ItemID"].YuanColumnText == id.Substring(0,4)){
	//					rows["Name"].YuanColumnText					
						EquipEnhance.instance.ShowMyItem("",String.Format("{0} {1} {2} {3}",AllManage.AllMge.Loc.Get("tips079") , rows["Name"].YuanColumnText , AllManage.AllMge.Loc.Get("info650") , AllManage.AllMge.Loc.Get("info652")));
					}
				}
			}else
			{
				tipsInv = AllResources.InvmakerStatic.GetItemInfo(id , tipsInv);
				if(tipsInv.slotType == SlotType.Soul){
					EquipEnhance.instance.ShowMyItem("",String.Format("{0} {1} {2} {3}",AllManage.AllMge.Loc.Get("tips079"), tipsInv.itemName , AllManage.AllMge.Loc.Get("info650") , AllManage.AllMge.Loc.Get("info654")));			
				}else
				if(tipsInv.slotType == SlotType.Ride){
					EquipEnhance.instance.ShowMyItem("",String.Format("{0} {1} {2} {3}",AllManage.AllMge.Loc.Get("tips079") , tipsInv.itemName , AllManage.AllMge.Loc.Get("info650") , AllManage.AllMge.Loc.Get("info653")));						
				}else
				{
					EquipEnhance.instance.ShowMyItem("",String.Format("{0} {1} {2} {3}",AllManage.AllMge.Loc.Get("tips079") , tipsInv.itemName , AllManage.AllMge.Loc.Get("info650") , AllManage.AllMge.Loc.Get("info651")));					
				}
			}
		}
	}

	var TweenDaoHang : TweenPosition[];
	var ButtonDaoHang : UISprite[];
	var bagAllParent : GameObject;
	function MoveDaoHang1(){
		bagAllParent.active = true;
		TransBag.localPosition.x = 0;
		ButtonJineng.localPosition.y = 1000;
		ButtonJineng.localPosition.z = 600;
		TweenControl(TweenDaoHang[0]);
		invcl.isShangdian = true;
		invcl.QieHuanShangdian();
		if(AllManage.invCangKuStatic)
			AllManage.invCangKuStatic.gameObject.SetActiveRecursively(true);
		TransPlayerInfo[1].localPosition.y = 3000;
		AllManage.InvclStatic.QieHuanEquepZhuangBei();
	}

	function MoveDaoHang2(){
	    
		//AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info778"));
		//return;
	//    buttonJinengP.enabled=true;
	//	ButtonJineng.localPosition.y = 0; 
	    //	ButtonJineng.localPosition.z = -600;
	    if(parseInt(AllManage.psStatic.Level) >= 40){
	        TweenControl(TweenDaoHang[1]);
	        TransPlayerInfo[1].localPosition.y = 3000;
	    }else{
	        //ts.Show("tips054");
	        AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("meg0212"));// 提示：拍卖行30级开启，赶快升级吧！
	    }
		
	//	skillC.ShowSkillCanUpDate();
		
	}

	function MoveDaoHang3(){
		ButtonJineng.localPosition.y = 1000;
		ButtonJineng.localPosition.z = 600;
		TweenControl(TweenDaoHang[2]);
		TransPlayerInfo[1].localPosition.y = 3000;
	//	skillC.ShowSkillCanUpDate();
	}
	function MoveDaoHang4(){
		ButtonJineng.localPosition.y = 1000;
		ButtonJineng.localPosition.z = 600;
		TweenControl(TweenDaoHang[3]);
		TransPlayerInfo[1].localPosition.y = 3000;
	//	skillC.ShowSkillCanUpDate();
	}
	function MoveDaoHang5(){
		ButtonJineng.localPosition.y = 1000;
		ButtonJineng.localPosition.z = 600;
		TweenControl(TweenDaoHang[4]);
		TransPlayerInfo[1].localPosition.y = 3000;
	//	skillC.ShowSkillCanUpDate();
	}
	function MoveDaoHang6(){
		ButtonJineng.localPosition.y = 1000;
		ButtonJineng.localPosition.z = 600;
		TweenControl(TweenDaoHang[5]);
		TransPlayerInfo[1].localPosition.y = 3000;
	//	skillC.ShowSkillCanUpDate();
	}
	function MoveDaoHang7(){
		if(Application.loadedLevelName == "Map200" || mTime > Time.time){
			return;
		}
		//TD_info.panelStatistics("商城");
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.ServerShop).ToString());
		ButtonJineng.localPosition.y = 1000;
		ButtonJineng.localPosition.z = 600;
		TweenControl(TweenDaoHang[6]);
		TransPlayerInfo[1].localPosition.y = 3000;
//		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().GetBuyInfor(TableRead.strPageName));
//		if(dicGetResult1 == null)
//		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().GetBuyInfor());

	//	skillC.ShowSkillCanUpDate();
	}
	function MoveDaoHang8(){
		ButtonJineng.localPosition.y = 1000;
		ButtonJineng.localPosition.z = 600;
		TweenControl(TweenDaoHang[7]);
	//	yield;
		PlayerSelectInfo();
		if(AllManage.invCangKuStatic)
			AllManage.invCangKuStatic.transCangku.localPosition.y = 1000;
	//	skillC.ShowSkillCanUpDate();
	}

	function MoveDaoHang9(){
		if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.PVPSwitch) == "0"){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
			return;
		}

		TweenControl(TweenDaoHang[8]);
		return;
	}

	var ParentYuan : GameObject[];
	var PreYuan : GameObject[];
	var PreYuanStr : String[];
	var AllParent : Transform;
	var TeamObj : GameObject;
	var TransformButtonPart : Transform[];
	function TweenControl(tp : TweenPosition){
		TeamObj.SetActiveRecursively(false);
		invcl.BagGuiWei();
		for(var i=0; i<TweenDaoHang.length; i++){
			if(TweenDaoHang[i] == tp){
				TransformButtonPart[i].localPosition.y = 10;
				if(ParentYuan[i] == null){
					if(PreYuan[i] == null){
						PreYuan[i] = Resources.Load(PreYuanStr[i], GameObject);
					}
					ParentYuan[i] = GameObject.Instantiate(PreYuan[i]);
					ParentYuan[i].transform.parent = AllParent;
					ParentYuan[i].transform.localPosition = Vector3.zero;
					if(i == 5){
						PanelStatic.StaticBtnGameManager.ReOptionPanle();
					}				
				}
				if(ParentYuan[i]!= null && 	!ParentYuan[i].active){
					ParentYuan[i].SetActiveRecursively(true);
				}
				if(ButtonDaoHang[i]){
					if(i != 6){
						ButtonDaoHang[i].spriteName = "UIH_Main_Button_A";						
					}else{
						ButtonDaoHang[i].spriteName = "UIH_Store_Button_N";											
					}
				}
	//			//print("chu lai =" + i);
				TweenDaoHang[i].Play(true);
			}else{
				TransformButtonPart[i].localPosition.y = 3000;
				if(ButtonDaoHang[i]){
					if(i != 6){
						ButtonDaoHang[i].spriteName = "UIH_Main_Button_N";						
					}else{
						ButtonDaoHang[i].spriteName = "UIH_Store_Button_A";											
					}
				}
				if(ParentYuan[i]!= null){
					ParentYuan[i].SetActiveRecursively(false);
				}
	//			//print("jin qu =" + i);
				TweenDaoHang[i].Play(false);
			}
		}
//		yield;
//		if(dicGetResult1 != null && AllManage.buyStoreControl.LabelBloodNum1 && AllManage.buyStoreControl.LabelBloodNum1.text != dicGetResult1["propPrice"]){
//			AllManage.buyStoreControl.LabelBloodNum1.text = dicGetResult1["propPrice"];
//			AllManage.buyStoreControl.LabelBloodNum2.text = dicGetResult2["propPrice"];
//			AllManage.buyStoreControl.LabelBloodNum3.text = dicGetResult3["propPrice"];
//			AllManage.buyStoreControl.LabelBloodNum4.text = dicGetResult4["propPrice"];
//			
//			AllManage.buyStoreControl.LabelBloodName1.text = dicGetResult1["propName"];
//			AllManage.buyStoreControl.LabelBloodName2.text = dicGetResult2["propName"];
//			AllManage.buyStoreControl.LabelBloodName3.text = dicGetResult3["propName"];
//			AllManage.buyStoreControl.LabelBloodName4.text = dicGetResult4["propName"];	
//		}
	}

	var TransPlayerInfo : Transform[];
	var SpritePlayerInfo : UISprite[];
	function PlayerSelectInfo(){
		PlayerInfoDetailsOn();
		PlayerInfoSelectOne(0);
		
		if(! boolinfoPlayerObj){
			boolinfoPlayerObj = true;
			yield;
			infoPlayerObj.active = false;
			yield;
			infoPlayerObj.active = true;
		}
//		AllManage.UIALLPCStatic.parentSkill.gameObject.SetActiveRecursively(false);
	}

	function PlayerSelectSkill(){
	if(Application.loadedLevelName=="Map811" || Application.loadedLevelName=="Map812"){
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info772"));
		return;
	}
		if(canOpenViewAsTaskID("61") || canOpenViewAsTaskID("504")){
			TransPlayerDetails[0].localPosition.y = 3000;
			TransPlayerDetails[1].localPosition.y = 3000;
			PlayerInfoSelectOne(1);
			AllManage.UIALLPCStatic.show32();
			if(AllManage.SkillObjDet)
				AllManage.SkillObjDet.close();
		}
		
	if(AllManage.SkillCLStatic)
	{
	yield WaitForSeconds(0.1f);
	AllManage.SkillCLStatic.ShowSkillCanUpDate();
	}
	}
	
	function PlayerSelectSkillMe(){
	AllManage.UIALLPCStatic.parentSkill.gameObject.SetActiveRecursively(true);
	PlayerInfoSelectOne(1);
	}

	function PlayerSelectChengjiu(){
		TransPlayerDetails[0].localPosition.y = 3000;
		TransPlayerDetails[1].localPosition.y = 3000;
		PlayerInfoSelectOne(2);
		AllManage.UIALLPCStatic.show31();
	}

	function PlayerSelectXunLian(){
	
		if(canOpenViewAsTaskID("62") && SpritePlayerInfo[3] && SpritePlayerInfo[3].spriteName != "UIB_Tab_A"){
			TransPlayerDetails[0].localPosition.y = 3000;
			TransPlayerDetails[1].localPosition.y = 3000;
			PlayerInfoSelectOne(3);
			AllManage.UIALLPCStatic.show22();
		}
	}

	function PlayerInfoSelectOne(select : int){
		for(var i=0; i<TransPlayerInfo.length; i++){
			if(i == select){
				if(select == 1){
					TransPlayerInfo[i].gameObject.SetActiveRecursively(true);
				}
				SpritePlayerInfo[i].spriteName = "UIB_Tab_A";
				TransPlayerInfo[i].localPosition.y = 0;
			}else{
				SpritePlayerInfo[i].spriteName = "UIB_Tab_N";
				TransPlayerInfo[i].localPosition.y = 3000;
			}
		}
	}

	var TransPlayerDetails : Transform[];
	var SpritePlayerDetailsButton : UISprite;
//	var ParticleS : ParticleSystem;
	private var boolPlayerInfoDetails : boolean = false;
	function ChangePlayerInfoDetails(){
		if(	!boolPlayerInfoDetails){
			PlayerInfoDetailsOff();
		}else{
			PlayerInfoDetailsOn();
		}
	}

	var infoPlayerObj : GameObject;
	private var boolinfoPlayerObj : boolean = false;
	function PlayerInfoDetailsOn(){
		ObjBackGroundPar.Play();
		boolPlayerInfoDetails = false;
		TransPlayerDetails[0].localPosition.y = 0;
		TransPlayerDetails[1].localPosition.y = 3000;
		SpritePlayerDetailsButton.spriteName = "UIH_Minor_Button_N";
	}

	function PlayerInfoDetailsOff(){
		ObjBackGroundPar.Stop();
		boolPlayerInfoDetails = true;
		TransPlayerDetails[0].localPosition.y = 3000;
		TransPlayerDetails[1].localPosition.y = 0;
		SpritePlayerDetailsButton.spriteName = "UIH_Minor_Button_A";
	}

	var TransPlayerRide : Transform[];
	var SpritePlayerRideButton : UISprite;
	private var boolPlayerInfoRide : boolean = false;
	function ChangePlayerInfoRide(){
		if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.PetSwitch) == "0"){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
			return;
		}
		
		if(	!boolPlayerInfoRide){
			PlayerInfoRideOff();
		}else{
			PlayerInfoRideOn();
		}	
	}

	function PlayerInfoRideOn(){
		boolPlayerInfoRide = false;
	    TransPlayerRide[0].localPosition.y = 0;
		TransPlayerRide[1].localPosition.y = 3000;

		//PanelStatic.StaticBtnGameManager.invcl.SendMessage("ResetGridPanel", SendMessageOptions.DontRequireReceiver); 

		SpritePlayerRideButton.spriteName = "UIH_Minor_Button_N";
	}

	function PlayerInfoRideOff(){
		boolPlayerInfoRide = true;
		TransPlayerRide[0].localPosition.y = 3000;
		TransPlayerRide[1].localPosition.y = 0;
		
		PanelStatic.StaticBtnGameManager.invcl.SendMessage("ResetGridPanel", SendMessageOptions.DontRequireReceiver);

		SpritePlayerRideButton.spriteName = "UIH_Minor_Button_A";
	}

	private var bTime : float;
	function showBuild(){
		if(Application.loadedLevelName == "Map200" || mTime > Time.time){
			return;
		}
		if(Application.loadedLevelName == "Map200" || bTime > Time.time){
			return;
		}
		if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.EqupmentUpdateSwitch) == "0"){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
			return;
		}
		if(canOpenViewAsTaskID("13")){
			AllManage.UIALLPCStatic.show2();
		}
	}

	function showHole(){
		if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.EqupmentHoleSwitch) == "0"){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
			return;
		}
		AllManage.UIALLPCStatic.show18();
	}

	function showProduct(){
		if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.EqupmentBuildSwitch) == "0"){
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
			return;
		}

		AllManage.UIALLPCStatic.show3();
	}

	function OtherShowTask(){
		TinfoList.showInfo();
	//	AllManage.UIALLPCStatic.show18();
	}

	function OtherShowActivity(){
		AllManage.UIALLPCStatic.show8();
	}

	var OtherShowDuelParent1 : Transform;
	var OtherShowDuelParent2 : Transform;
	var OtherShowDuelParent3 : Transform;
	function OtherShowDuel(){
		AllManage.UIALLPCStatic.show20();
	//	yield;
		pvpcl.SetParent(OtherShowDuelParent1 , OtherShowDuelParent2 , OtherShowDuelParent3);
	}

	var OtherObjPVP : GameObject;
	function OtherShowOffLinePlayer(){
		//print("sdjflksdjflsdkjflsdkfl");
		OtherObjPVP.SendMessage("Select4" , SendMessageOptions.DontRequireReceiver);
		AllManage.UIALLPCStatic.show29();
	}

	var boolShowPVPInfo : boolean = false;
	function ButtonShowPVPInfo(){
		InRoom.GetInRoomInstantiate().BattlefieldInfo();
		boolShowPVPInfo = true;
	}
	
	var DicPvpInfo : System.Collections.Generic.Dictionary.<String, String>;
	function RetrunShowPVPInfo(objs : Object[]){
		if(AllManage.areCLStatic && boolShowPVPInfo){
			boolShowPVPInfo = false;
			AllManage.areCLStatic.RetrunShowPVPInfo(objs);
		}
	}
	
	function ReturnShowPVPWin(objs : Object[]){
		if(AllManage.areCLStatic){
			AllManage.areCLStatic.ReturnSHowPVPWin(objs);
		}
	}
	
	function ReturnBattlefieldScoreInfo(objs : Object[]){
		if(AllManage.areCLStatic){
			AllManage.areCLStatic.ReturnBattlefieldScoreInfo(objs);
		}	
	}
	
	function ReturnEquepmentBuild(bool : boolean){
		if(EquepmentBuildControl.me){
			EquepmentBuildControl.me.SendMessage("ReturnDoDaZao" , bool , SendMessageOptions.DontRequireReceiver);
			if(bool){			
				EquepmentBuildControl.me.SendMessage("RefreshList" , SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	function ReturnEquepmentBuildRefresh(){
		if(EquepmentBuildControl.me){		
			EquepmentBuildControl.me.SendMessage("RefreshList" , SendMessageOptions.DontRequireReceiver);
		}
	}

	function ReturnEquepmentHole(){
		if(EquepmentPunchControl.me){
			EquepmentPunchControl.me.SendMessage("RefreshList" , SendMessageOptions.DontRequireReceiver);
		}
	}

	function ReturnEquepmentProdece(){
		if(InventoryProduceControl.me){
			InventoryProduceControl.me.SendMessage("RefreshList" , SendMessageOptions.DontRequireReceiver);
		}
	}

	function ReturnTraining(its : int[]){
		if(XunLianControl.me){
			//print(its[0]);
			//print(its[1]);
			//print(its[2]);
			//print(its[3]);
			//print(its[4]);
			XunLianControl.me.SendMessage("returnSetTextAsColor" , its , SendMessageOptions.DontRequireReceiver);
		}
	}

	function ReturnTrainingSave(){
		//print("sdljfsdjlflsdjfljk  === zhe li le");
		if(XunLianControl.me){
			XunLianControl.me.SendMessage("returnSave" , SendMessageOptions.DontRequireReceiver);
		}
	}

	function ReturnPVPBossHP(hps : int[]){
		if(AllManage.areCLStatic){
			AllManage.areCLStatic.ReturnPVPBossHP(hps);
		}		
	}

	private var ActivityBoos : BattlefieldCityItem;
	function ReturnActivityBoosHP(hp : int){
		if(!ActivityBoos){
			ActivityBoos = FindObjectOfType(BattlefieldCityItem);
		}
		if(ActivityBoos){
			ActivityBoos.SetServerBossHP(hp);
		}
	}
	
	function ReturnActivityBossResult(){
//		returnLevel();
	}
	
	function retrunGetStoreList9(strs : String[]){
		if(AllManage.invCangKuStatic)
			AllManage.invCangKuStatic.returnGetStoreList9(strs);
	}

	function returnGetStoreList3(strs : String[]){
		if(AllManage.invCangKuStatic)
			AllManage.invCangKuStatic.returnGetStoreList3(strs);
	}

	function retrunReturnAddExperienceF(e : int){
		PlayerStatus.MainCharacter.gameObject.SendMessage("ReturnAddExperienceF" , e , SendMessageOptions.DontRequireReceiver);
	}

	function retrunReturnUpDateLevelF(strs : String[]){
//		print(strs[0]);
//		print(strs[1]);
		//TD_info.setUpUserLevel(InventoryControl.yt.Rows[0]["UserInfo_userId"].YuanColumnText + InventoryControl.yt.Rows[0]["PlayerID"].YuanColumnText + ";" + strs[0] + ";" + PlayerPrefs.GetString("InAppServerName", "NON") + ";" +  strs[1]);
		
		PlayerStatus.MainCharacter.gameObject.SendMessage("ReturnUpDateLevelF" , SendMessageOptions.DontRequireReceiver);
	}

	var PSkill : PassiveSkill;
	var ASkill : ActiveSkill;
	var STitem : SkillItem[];
	function SkillRetrun(){
		AllManage.SkillCLStatic.STitem = STitem;
	}

	var skim1 : SkillItem;
	var skim2 : SkillItem;
	var skim3 : SkillItem;
	var skim4 : SkillItem;
	function SkillObjREtrun(){
		AllManage.SkillObjDet.skim1 = skim1;
		AllManage.SkillObjDet.skim2 = skim2;
		AllManage.SkillObjDet.skim3 = skim3;
		AllManage.SkillObjDet.skim4 = skim4;
	}

	function retrunDoneCard(objs : Object[]){
		AllManage.dungclStatic.returnDoneCard(objs);
	}

	function retrunOpenCard(id : String){
		AllManage.CardCLStatic.RealOpenCard(id);
	}

	function retrunPVPCard(objs : Object[]){
		AllManage.areCLStatic.returnDuelCard(objs);
	}

	function ReturnPVPisFall(pvpStone : int){
		AllManage.areCLStatic.ReturnPVPisFall(pvpStone);
	}

//	var useTaskComplet : String[];
	var secObj : GameObject;
	function returnTaskComplet(taskid : String){
		
//		useTaskComplet = InventoryControl.yt.Rows[0]["CompletTask"].YuanColumnText.Split(FStr.ToCharArray());
//	
//		for(var myID:String in useTaskComplet ){
//			if(myID == taskid){
//				show0();
//				return;
//			}
//		}
//		if(MainPlayerS == null){
//			MainPlayerS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
//		}
//		MainPlayerS.InTaskInfoStep();
//		return;
		secObj.SendMessage("SectionTask" , taskid , SendMessageOptions.DontRequireReceiver);
		if(AllManage.mtwStatic.stepValue == "StepZero"){
			MainTW.serverNowTaskID = taskid;
			MainTW.stepIsServer = true;
			AllManage.mtwStatic.InTaskInfoStep();
		}else
		if(AllManage.mtwStatic.stepValue == "StepOne"){
			MainTW.serverNowTaskID = taskid;
			MainTW.stepIsServer = true;
			AllManage.mtwStatic.InTaskInfoStep1();
		}else
		if(AllManage.mtwStatic.stepValue == "StepTutorials"){
			PlayerStatus.MainCharacter.SendMessage("returnTaskComplet" , taskid , SendMessageOptions.DontRequireReceiver);		
		}
	}
	
//	var useAddNewTask : String[];
	var isWayAddTask : boolean = false;	
	function returnAddNewTask(taskid : String){
		
//		useAddNewTask = InventoryControl.yt.Rows[0]["Task"].YuanColumnText.Split(FStr.ToCharArray());
//	
//		for(var myID:String in useAddNewTask ){
//			if(myID == taskid){
//				show0();
//				return;
//			}
//		}
//		if(MainPlayerS == null){
//			MainPlayerS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
//		}
//		MainPlayerS.InTaskInfoStep();
//		return;
		isWayAddTask = true;
		AllManage.UICLStatic.isOpenNpcTalk = false;
		if(AllManage.mtwStatic.stepValue == "StepZero"){
			MainTW.serverNowTaskID = taskid;
			MainTW.stepIsServer = true;
			AllManage.mtwStatic.InTaskInfoStep();
		}else
		if(AllManage.mtwStatic.stepValue == "StepOne"){
			MainTW.serverNowTaskID = taskid;
			MainTW.stepIsServer = true;
			AllManage.mtwStatic.InTaskInfoStep1();
		}
		yield WaitForSeconds(0.5);
		if(! AllManage.jiaochengCLStatic.isGen && isWayAddTask){
			AllManage.taskILStatic.FindWay();
		}
//		PlayerStatus.MainCharacter.SendMessage("returnAddNewTask" , taskid , SendMessageOptions.DontRequireReceiver);		
	}
	
	function returnAddActivitiesTask(taskid : String){
		AllManage.mtwStatic.AddActivitiesTask(taskid);
		yield WaitForSeconds(0.5);
		AllManage.UIALLPCStatic.show0();
		AllManage.taskILStatic.FindWay();
	}
	
	function returnActivitiesTaskComplet(taskid : String){
		PlayerStatus.MainCharacter.SendMessage("returnTaskComplet" , taskid , SendMessageOptions.DontRequireReceiver);		
	}
	
	function returnTaskGiveUpAsID(taskid : String){
		PlayerStatus.MainCharacter.SendMessage("returnTaskGiveUpAsID" , taskid , SendMessageOptions.DontRequireReceiver);		
	}
	
	function returnTaskAddNumsAsID(taskid : String){
		PlayerStatus.MainCharacter.SendMessage("returnTaskAddNumsAsID" , taskid , SendMessageOptions.DontRequireReceiver);		
	}
	
	function ReturnUseMoney(type : yuan.YuanPhoton.UseMoneyType){
		AllManage.AllMge.ReturnUseMoney(type);
	}
	
	function ReturnUseTips(objs : Object[]){
		AllManage.AllMge.ReturnUseTips(objs);
	}
	
	function ReturnCostPower(type : yuan.YuanPhoton.CostPowerType){
		AllManage.AllMge.ReturnCostPower(type);
	}
	
	function ReturnTipsPower(objs : Object[]){
		AllManage.AllMge.ReturnTipsPower(objs);
	}
	
	function ReturnTipsNoPower(){
			var Istrue : boolean = false;
			var StrItem = InventoryControl.yt.Rows[0]["Item"].YuanColumnText;
			var str : String[] = StrItem.Split(";"[0]); 
			for(var j= 0 ; j<str.Length ; j++)
			{
				if(str[j]!="")

				if(str[j].Substring(0,3).Equals("884")){
				StrID = str[j];
				Istrue = true;
				AllManage.qrStatic.ShowQueRen(gameObject,"UseDaoju" , "","info1119");
				}

				if(!Istrue){
				AllManage.qrStatic.ShowQueRen(gameObject,"UseShangdian" , "","info1120");
				}

			}
	}
	
	var StrID : String;
	function UseDaoju()
{
		AllManage.InvclStatic.UseDaojuAsID(StrID);
}
	function UseShangdian()
	{
		AllManage.UICLStatic.StoreOpenMoveOn();
	}
	
	function ReturnActivityJoinSuccess(mapID : String){
		PhotonNetwork.isMessageQueueRunning = false;	
		Loading.Level = "Map" + mapID;
		Loading.nandu = "1";
		if(mapType == MapType.zhucheng)
			DungeonControl.ReLevel = Application.loadedLevelName;
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
		alljoy.DontJump = true;
		yield;
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
		PhotonNetwork.LeaveRoom();
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");
		
	}

	static var mapInstanceID : int = 0;
	function ReturnActivityJoinSuccess(objs : Object[]){
		var mapID : String;
		mapID = objs[0];
		mapInstanceID = objs[1];
		PhotonNetwork.isMessageQueueRunning = false;	
		Loading.Level = "Map" + mapID;
		Loading.nandu = "1";
		if(mapType == MapType.zhucheng)
			DungeonControl.ReLevel = Application.loadedLevelName;
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
		alljoy.DontJump = true;
		yield;
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
		PhotonNetwork.LeaveRoom();
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");
		
	}
			
	function ReturnEquipmentResolve(bool : boolean){
		if(AllManage.equipBreakCL)
			AllManage.equipBreakCL.ReturnDoDaZao(bool);
	}
	function ReturnSellAll(){
		if(AllManage.equipBreakCL)
			AllManage.equipBreakCL.RefreshList();
	}
	
//	static var nowNPCObj : GameObject;
	private var nowNPCs : npcAI[];
	var buttonNpcTalk : GameObject;
	var npcTalkShan : UISprite;
//	@HideInInspector
//	var taskNpcItem : TaskNpcItem; 
	function OneNPCTalk(){
//		print("1231231234");
		if(nowNPCs == null || nowNPCs.length == 0){
			nowNPCs = FindObjectsOfType(npcAI);
		}
		var distance : float = 99999;
		var num : int = 0;
		for(var i=0; i<nowNPCs.length; i++){
			if((nowNPCs[i].transform.position - PlayerStatus.MainCharacter.transform.position).magnitude < distance){
				distance = (nowNPCs[i].transform.position - PlayerStatus.MainCharacter.transform.position).magnitude;
				num = i;
			}
		}
//		print(nowNPCs);
//		print(nowNPCs.length);
//		print(num);
//		print(nowNPCs + " == " + nowNPCs.length + " == " + num);
		if(nowNPCs != null && nowNPCs.length != 0 && nowNPCs.length > num)
		{
			nowNPCs[num].OnClick();
//			Debug.Log("===================================taskNpcItem.myTask:" + taskNpcItem.myTask.id);
//			taskNpcItem.CheckGuideTask(taskNpcItem.myTask);
		}
	}
	var isOpenNpcTalk : boolean = false;
	function OneNPCTalkNoClick(){
//		print("1231231234");
		if(nowNPCs == null || nowNPCs.length == 0){
			nowNPCs = FindObjectsOfType(npcAI);
		}
		var distance : float = 99999;
		var num : int = 0;
		for(var i=0; i<nowNPCs.length; i++){
			if((nowNPCs[i].transform.position - PlayerStatus.MainCharacter.transform.position).magnitude < distance){
				distance = (nowNPCs[i].transform.position - PlayerStatus.MainCharacter.transform.position).magnitude;
				num = i;
			}
		}
//		print(nowNPCs);
//		print(nowNPCs.length);
//		print(num);
//		print(nowNPCs + " == " + nowNPCs.length + " == " + num);
		if(nowNPCs != null && nowNPCs.length != 0 && nowNPCs.length > num && nowNPCs[num].FindOtherCanShow())
		{
			nowNPCs[num].NpcMoveOn();
			isOpenNpcTalk = true;
//			Debug.Log("===================================taskNpcItem.myTask:" + taskNpcItem.myTask.id);
//			taskNpcItem.CheckGuideTask(taskNpcItem.myTask);
		}
	}
	
	function ShowTaskInfoList(){
		TinfoList.showInfo();
	}
	
	var joystick : Transform;
	
	var charB : GameObject;
	function CharBarTextMoney(ints : int[]){
		var mGold : int = 0;
		var mBlood : int = 0;
		var strGold : String = "";
		var strBlood : String = "";
		
		mGold = ints[0];
		mBlood = ints[1];
		
		if(mGold > 0){
			strGold = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info554") , mGold ,  AllManage.AllMge.Loc.Get("info335"));
		}else
		if(mGold < 0){
			strGold = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info552") , Mathf.Abs(mGold) ,  AllManage.AllMge.Loc.Get("info335"));
		}
		
		if(strGold != ""){
			charB.SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);
			if(PanelStatic.StaticSendManager.listBar[0]){
				PanelStatic.StaticSendManager.listBar[0].SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);
				PanelStatic.StaticSendManager.listBar[4].SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);	
			}
		}
		
		if(mBlood > 0){
			strBlood = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info554") , mBlood ,  AllManage.AllMge.Loc.Get("messages053"));
		}else
		if(mBlood < 0){
			strBlood = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info552") , Mathf.Abs(mBlood) ,  AllManage.AllMge.Loc.Get("messages053"));		
		}
		
		if(strBlood != ""){
			if(PanelStatic.StaticSendManager.listBar[0]){
				charB.SendMessage("AddText" , strBlood , SendMessageOptions.DontRequireReceiver);
				PanelStatic.StaticSendManager.listBar[0].SendMessage("AddText" , strBlood , SendMessageOptions.DontRequireReceiver);
				PanelStatic.StaticSendManager.listBar[4].SendMessage("AddText" , strBlood , SendMessageOptions.DontRequireReceiver);		
			}
		}
	}
	
	function CharBarTextStone(ints : int[]){
		var mGold : int = 0;
		var mBlood : int = 0;
		var strGold : String = "";
		var strBlood : String = "";
		
		mGold = ints[0];
		mBlood = ints[1];
		
		if(mGold > 0){
			strGold = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info554") , mGold ,  AllManage.AllMge.Loc.Get("info1064"));
		}else
		if(mGold < 0){
			strGold = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info552") , Mathf.Abs(mGold) ,  AllManage.AllMge.Loc.Get("info1064"));
		}
		
		if(strGold != ""){
			charB.SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);
			if(PanelStatic.StaticSendManager.listBar[0]){
				PanelStatic.StaticSendManager.listBar[0].SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);
				PanelStatic.StaticSendManager.listBar[4].SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);	
			}
		}
		
		if(mBlood > 0){
			strBlood = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info554") , mBlood ,  AllManage.AllMge.Loc.Get("info1065"));
		}else
		if(mBlood < 0){
			strBlood = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info552") , Mathf.Abs(mBlood) ,  AllManage.AllMge.Loc.Get("info1065"));		
		}
		
		if(strBlood != ""){
			if(PanelStatic.StaticSendManager.listBar[0]){
				charB.SendMessage("AddText" , strBlood , SendMessageOptions.DontRequireReceiver);
				PanelStatic.StaticSendManager.listBar[0].SendMessage("AddText" , strBlood , SendMessageOptions.DontRequireReceiver);
				PanelStatic.StaticSendManager.listBar[4].SendMessage("AddText" , strBlood , SendMessageOptions.DontRequireReceiver);		
			}
		}
	}
	
	function CharBarTextPower(ints : int){
		var mGold : int = 0;
		var strGold : String = "";
		var strBlood : String = "";
		
		mGold = ints;
		
		if(mGold > 0){
			strGold = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info552") , mGold ,  AllManage.AllMge.Loc.Get("meg0119"));
		}else
		if(mGold < 0){
			strGold = String.Format("{0} {1} {2}" , AllManage.AllMge.Loc.Get("info554") , Mathf.Abs(mGold) ,  AllManage.AllMge.Loc.Get("meg0119"));
		}
		
		if(strGold != ""){
			AllManage.tsStatic.ShowRed1(strGold);
//			charB.SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);
//			if(PanelStatic.StaticSendManager.listBar[0]){
//				PanelStatic.StaticSendManager.listBar[0].SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);
//				PanelStatic.StaticSendManager.listBar[4].SendMessage("AddText" , strGold , SendMessageOptions.DontRequireReceiver);	
//			}
		}
	}
	
	static var dicGetResult1 : System.Collections.Generic.Dictionary.<String, String> = null;
	static var dicGetResult2 : System.Collections.Generic.Dictionary.<String, String> = null;
	static var dicGetResult3 : System.Collections.Generic.Dictionary.<String, String> = null;
	static var dicGetResult4 : System.Collections.Generic.Dictionary.<String, String> = null;;
	function ReturnPayGameRole(objs : Object[]){
		dicGetResult1 = objs[0];
		dicGetResult2 = objs[1];
		dicGetResult3 = objs[2];
		dicGetResult4 = objs[3];		
		
		AllManage.buyStoreControl.LabelBloodNum1.text = dicGetResult1["propPrice"];
		AllManage.buyStoreControl.LabelBloodNum2.text = dicGetResult2["propPrice"];
		AllManage.buyStoreControl.LabelBloodNum3.text = dicGetResult3["propPrice"];
		AllManage.buyStoreControl.LabelBloodNum4.text = dicGetResult4["propPrice"];
		
		AllManage.buyStoreControl.LabelBloodName1.text = dicGetResult1["propName"];
		AllManage.buyStoreControl.LabelBloodName2.text = dicGetResult2["propName"];
		AllManage.buyStoreControl.LabelBloodName3.text = dicGetResult3["propName"];
		AllManage.buyStoreControl.LabelBloodName4.text = dicGetResult4["propName"];
	}

	var TDPurchaseName : String[];
	function setTDPurchase(objs : Object[]){
		var intBlood : int = 0;
		var type : yuan.YuanPhoton.UseMoneyType;
		intBlood = objs[0];
		type = objs[1];
		if(intBlood < 0){
			//TD_info.setUserPurchase(String.Format("{0};{1};{2}", TDPurchaseName[parseInt(type)] , "1" , Mathf.Abs(intBlood)));		
		}
	}
	
	function BattlefieldNotifyExit(battlefieldLabel : String){
		if(AllManage.areCLStatic != null){
			if(myTeamInfo != battlefieldLabel && ArenaControl.areType == ArenaType.juedou){
				AllManage.areCLStatic.JueDouWin(true);
			}
		}
	}
	
	//function retrunOpenPVPCard(id : String){
	//	
	//}

	//function TaskSelectTask(){
	//	TaskSelectOne(0);
	//}
	//
	//function TaskSelectActivity(){
	//	TaskSelectOne(1);
	//}
	//
	//function TaskSelectUnion(){
	//	TaskSelectOne(2);
	//}
	//
	//var SpriteTaskInfo : UISprite[];
	//var 
	//function TaskSelectOne(select : int){
	//	for(var i=0; i<TransTaskInfo.length; i++){
	//		if(i == select){
	//			SpriteTaskInfo[i].spriteName = "UIB_Tab_A";
	//			TransTaskInfo[i].localPosition.y = 0;
	//		}else{
	//			SpriteTaskInfo[i].spriteName = "UIB_Tab_N";
	//			TransTaskInfo[i].localPosition.y = 3000;
	//		}
	//	}
	//}
}

function ShowInvite(){
			AllManage.UIALLPCStatic.show34();
}

function ShowSectionTip(){
			AllManage.UIALLPCStatic.show41();
}
private var objSection : GameObject;
var parentSection : Transform;
function show41(){
		if(objSection==null){
		var preSection = Resources.Load("Anchor - Section-tips", GameObject);
		objSection = GameObject.Instantiate(preSection);
		objSection.transform.parent = parentSection;
		objSection.transform.localPosition = Vector3.zero;
		}else{
		objSection.SetActive(true);
	}
}

var BtnJumpNext : GameObject;
var talkControl : TalkControl;
function SectionClose(){
    objSection.SetActive(false);
    if(BtnJumpNext){
    BtnJumpNext.SetActiveRecursively(true);
    }
	
    if(Application.loadedLevelName == "Map111"){
        talkControl.SetActivePanel();
    }
    if(Application.loadedLevelName == "Map200"){
        AllManage.camStoryStatic.StartStory();    
    }
}

static var isPGetOutOf : boolean = false;
function PGetOutOf(){
	if(mapType != MapType.zhucheng){
		nowYesReturn();
		isPGetOutOf = true;
		InRoom.GetInRoomInstantiate().CZPGetOutOf();
	}
}

function TeamHeadNextLoad(){
	if(mapType == MapType.fuben){
		AllManage.dungclStatic.TeamHeadNextLoad();
	}
}

function SDKSongLoadOut(){
	AllResources.ar.SongLoadOut();
}

var Bottom : GameObject; 
function ShowBoom(){
      if(mapType == MapType.zhucheng){
      Bottom.SetActive(true);
      }else{
      Bottom.SetActive(false);
      }
}

function SetCourseStepObject(trans : Transform){
	AllManage.jiaochengCLStatic.allJiaoCheng[10].obj[1] = trans.gameObject;
}

function CloseNPCtaskItem(){
	for(var m=0; m<NPCtaskItem.length; m++){
		if(NPCtaskItem[m] != null){
			NPCtaskItem[m].gameObject.SetActiveRecursively(false);	
		}
	}
	if(NPCGroup[0] != null){
		NPCGroup[0].localPosition.y = 1000;
	}
}

function CloseBoom(){
//	Bottom.SetActiveRecursively(false);
}

function CloseDoneCardButton(){
	AllManage.dungclStatic.CloseDoneCardButton();
}

function DuelInvite(duel : Object[]){
	if(mapType == MapType.yewai){
		var playerPro : int = duel[0];
		var playerName : String = duel[1];
		var playerLevel : int = duel[2];
		var playerForceValue : int = duel[3];
		
		AllManage.qrStatic.ShowDuelQueRen(gameObject , "YesDuel" , "NoDuel" , AllManage.AllMge.Loc.Get("info1133") ,AllManage.AllMge.Loc.Get("info1130") + playerName  , AllManage.AllMge.Loc.Get("info1131") + playerLevel , AllManage.AllMge.Loc.Get("info1132") + playerForceValue , playerPro);
	}
}

function YesDuel(){
	ServerRequest.requestAcceptDuel(1);
}

function NoDuel(){
	ServerRequest.requestAcceptDuel(2);
}

function duelInviteError(msg : String){
	AllManage.tsStatic.Show1(msg);	
}

var TextThree : GameObject ;
var TextTwo : GameObject ;
var TextOne : GameObject ;
var TextZero : GameObject ;

var objStone : GameObject;
function StartDuel(instanceID : int){
	TextThree.SetActive(true);
	yield WaitForSeconds(1);
	TextThree.SetActive(false);
	TextTwo.SetActive(true);
	yield WaitForSeconds(1);
	TextThree.SetActive(false);
	TextTwo.SetActive(false);
	TextOne.SetActive(true);
	yield WaitForSeconds(1);
	TextThree.SetActive(false);
	TextTwo.SetActive(false);
	TextOne.SetActive(false);
	TextZero.SetActive(true);
	yield WaitForSeconds(1);
	TextThree.SetActive(false);
	TextTwo.SetActive(false);
	TextOne.SetActive(false);
	TextZero.SetActive(false);
	
	
	var go : GameObject = ObjectAccessor.getAOIObject(instanceID);
	go.tag = "Enemy";
	go.SendMessage("SetEnemytag" , "Player" , SendMessageOptions.DontRequireReceiver);
	
	var vecfrom : Vector3 ;
	var vecto :  Vector3 ;
	vecfrom = PlayerStatus.MainCharacter.position ; 
	vecto = go.transform.position;
	var posfrom : Vector3 ;
	var posto :  Vector3 ;
	 posto = Vector3((vecto.x+vecfrom.x)/2,2,(vecto.z+vecfrom.z)/2);
	 posfrom = posto+Vector3(0,10,0);
	objStone = Instantiate(Resources.Load("Stone - Flags1", GameObject));
	objStone.SetActive(true);
	FlagDown(posfrom,posto);
	//objStone.transform.position = Vector3.Lerp(posfrom,posto,Time.time*0.8f);
//	objStone.transform.parent = ps.gameObject.transform;
//	objStone.transform.localPosition = PlayerStatus.MainCharacter.localPosition;
	
//	print("StartDuel");
}

function FlagDown(from : Vector3, to : Vector3)
{
	var isDone : boolean = true;
	objStone.transform.position = from;
	
	while(isDone)
	{
		
		objStone.transform.position = Vector3.Lerp(objStone.transform.position, to, Time.deltaTime*20f);
		if(objStone.transform.position.y <=2)
		{
			isDone = false;
		}
		
		yield WaitForSeconds(0.1f);
	}
}

function RefuseDuel(){
	AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1134"));	
//	print("RefuseDuel");
}

function DuelWin(instanceID : int){
	var go : GameObject = ObjectAccessor.getAOIObject(instanceID);
	go.tag = "Player";
	go.SendMessage("SetEnemytag" , "Enemy" , SendMessageOptions.DontRequireReceiver);
	
	ServerRequest.requestResetHp();
	qiuai.objs = null;
	AllManage.UIALLPCStatic.show45();
	go.SendMessage("ClearBuff" , SendMessageOptions.DontRequireReceiver);
	PlayerStatus.MainCharacter.SendMessage("ClearBuff" , SendMessageOptions.DontRequireReceiver);
	
	yield WaitForSeconds(3);
	AllManage.UIALLPCStatic.show0();
	objStone.transform.position = Vector3(999,999,999);
//	print("You Win !!!!!!!!!!!!!!!!!!!!!!!");
}

function DuelLose(instanceID : int){
	var go : GameObject = ObjectAccessor.getAOIObject(instanceID);
	go.tag = "Player";
	go.SendMessage("SetEnemytag" , "Enemy" , SendMessageOptions.DontRequireReceiver);
	ServerRequest.requestResetHp();
	qiuai.objs = null;
	AllManage.UIALLPCStatic.show46();
	go.SendMessage("ClearBuff" , SendMessageOptions.DontRequireReceiver);
	PlayerStatus.MainCharacter.SendMessage("ClearBuff" , SendMessageOptions.DontRequireReceiver);
	yield WaitForSeconds(3);
	AllManage.UIALLPCStatic.show0();
	objStone.transform.position = Vector3(999,999,999);
//	print("You Lose !!!!!!!!!!!!!!!!!!!!!!!");
}

function RefreshTempTeamButton(){
	if(AllManage.mtwStatic.LabelTemTeamReturn != null){
		if(boolTeamHeadYes && TeamHeadYesOKmapid != ""){
			AllManage.mtwStatic.LabelTemTeamReturn.text = AllManage.AllMge.Loc.Get("buttons827");
			if(isTempHead)
				AllManage.mtwStatic.LabelTemTeamGO.text = AllManage.AllMge.Loc.Get("buttons828");
			else
				AllManage.mtwStatic.LabelTemTeamGO.text = AllManage.AllMge.Loc.Get("buttons829");
			//TODU gaibian zhuangtai
		}else{
			AllManage.mtwStatic.LabelTemTeamReturn.text = AllManage.AllMge.Loc.Get("buttons125");
			AllManage.mtwStatic.LabelTemTeamGO.text = AllManage.AllMge.Loc.Get("buttons826");
			//TODU return zhuangtai
		}
	}
}

function saveMapInstanceID(id : int){
	if(boolTeamHeadYes){
		InRoom.GetInRoomInstantiate().TempTeamHeadGoMap(id);	
		boolTeamHeadYes = false;
		TeamHeadYesOKmapid = "";
	}
	nowmapInstanceID = id;
}

static var TeamHeadYesOKmapid : String = "";
static var boolTeamHeadYes : boolean = false;
var isTempHead : boolean = false;
function DungeonTeamHeadYes(mapid : String){
	isTempHead = true;
	boolTeamHeadYes = true;
	TeamHeadYesOKmapid = mapid;
	AllManage.qrStatic.ShowQueRen1(gameObject , "TeamHeadYesOK" , "TeamHeadYesNO" , AllManage.AllMge.Loc.Get("info1139"));
}

function TempTeamNewPlayerAdd(mapid : String){
	isTempHead = true;
	TeamHeadYesOKmapid = mapid;
	boolTeamHeadYes = true;
	if(mapType == MapType.zhucheng)
		AllManage.qrStatic.ShowQueRen1(gameObject , "TeamHeadYesOK" , "TeamHeadYesNO" , AllManage.AllMge.Loc.Get("info1140"));
}

function TeamHeadYesOK(){
	if(TeamHeadYesOKmapid){
		Loading.Level = "Map" + TeamHeadYesOKmapid.Substring(0,3);
		Loading.nandu = TeamHeadYesOKmapid.Substring(4,1);
		DungeonControl.NowMapLevel = parseInt(TeamHeadYesOKmapid.Substring(4,1));
		if(mapType == MapType.zhucheng)
		DungeonControl.ReLevel = Application.loadedLevelName;
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
		alljoy.DontJump = true;
		yield;
		PhotonNetwork.LeaveRoom();
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
		AllResources.ar.AllLoadLevel("Loading 1");
	}
}

function TeamHeadYesNO(){
	
}

function AddTempTeamTeamPlayerYes(){
	AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1141"));
}

static var tempTeammapInstensID : int = 0;
static var tempTeamPlayerGoMapID : String = "";
function AddTempTeamYesDo(objects : Object[]){
	isTempHead = false;
	boolTeamHeadYes = true;
	tempTeammapInstensID = objects[0];
	tempTeamPlayerGoMapID = objects[1];
	if(mapType == MapType.zhucheng)
		AllManage.qrStatic.ShowQueRen1(gameObject , "TeamPlayerYesOK" , "TeamPlayerYesNO" , AllManage.AllMge.Loc.Get("info1155"));
}

function TeamPlayerYesOK(){
	if(tempTeamPlayerGoMapID != ""){
		Loading.Level = "Map" + tempTeamPlayerGoMapID.Substring(0,3);
		Loading.nandu = tempTeamPlayerGoMapID.Substring(4,1);
		DungeonControl.NowMapLevel = parseInt(tempTeamPlayerGoMapID.Substring(4,1));
		if(mapType == MapType.zhucheng)
		DungeonControl.ReLevel = Application.loadedLevelName;
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
		alljoy.DontJump = true;
		yield;
		PhotonNetwork.LeaveRoom();
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
		AllResources.ar.AllLoadLevel("Loading 1");
	}

}

function TeamPlayerYesNO(){
	InRoom.GetInRoomInstantiate().TempTeamPlayerRemove();
}

function TempTeamHeadRemove(){
	TempTeamInit();
	RefreshTempTeamButton();
	AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1144"));	
}

function TempTeamPlayerRemove(){
	TempTeamInit();
	RefreshTempTeamButton();
	AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info1143"));	
}

function TempTeamInit(){
	TeamHeadYesOKmapid = "";
	tempTeamPlayerGoMapID = "";
	tempTeammapInstensID = 0;
	isTempHead = false;
	boolTeamHeadYes = false;
}

static var nowSelectChannel : int = 0;
function SelectChannel(channel : int){
//	nowSelectChannel = channel;
	if(nowSelectChannel != nowmapInstanceID){
		AllManage.qrStatic.ShowQueRen1(gameObject , "YesSelectChannel" , "NoSelectChannel" , AllManage.AllMge.Loc.Get("info1156"));
	}else{
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages159"));	
	}
}

function YesSelectChannel(){
	ServerRequest.requestCanAddToInsMap(nowSelectChannel);
}

function NowSelectChannel(bool : boolean){
	AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages158"));	
}

function NowGoSelectChannel(channel : int){
	nowSelectChannel = channel;
		Loading.Level = Application.loadedLevelName;
		Loading.nandu = "1";
		DungeonControl.NowMapLevel = 1;
		if(mapType == MapType.zhucheng)
		DungeonControl.ReLevel = Application.loadedLevelName;
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
		alljoy.DontJump = true;
		yield;
		PhotonNetwork.LeaveRoom();
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
		AllResources.ar.AllLoadLevel("Loading 1");	
}

function NoSelectChannel(){
	nowSelectChannel = 0;
}

static var towerNum : int = 0;
function TrappedtowerLoadLevel(towerInfo : int[]){
	var difficulty : int = 0;
	towerNum = towerInfo[0];
	difficulty = towerInfo[1];
		Loading.Level = "Map721";
		Loading.nandu = difficulty.ToString();
		DungeonControl.NowMapLevel = difficulty;
		if(UIControl.mapType == MapType.zhucheng)
			DungeonControl.ReLevel = Application.loadedLevelName;
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		alljoy.DontJump = true;
		yield;
		InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
		PhotonNetwork.LeaveRoom();
		AllResources.ar.AllLoadLevel("Loading 1");
}

var MonsterSp : MonsterSpawn[];
function TrappedtowerCallMonsterAsNum(num : int){
	PlayerStatus.MainCharacter.position = StartPoint.position;
	PlayerStatus.MainCharacter.SendMessage("PlayEffect" , 166 , SendMessageOptions.DontRequireReceiver);
	towerNum = num;
	AllManage.dungclStatic.SetTowerMonsters(towerNum);
//	Debug.Log("TrappedtowerCallMonsterAsNum--------------------" + towerNum);
	yield;

	if(MonsterSp == null || MonsterSp.length == 0)
		MonsterSp = FindObjectsOfType(MonsterSpawn);
	for( var Mp : MonsterSpawn in MonsterSp )
	{	
		Mp.SetClear(true);
		if(Mp.GetRow1())
			Mp.CallMonster(towerNum);
	}
}

function TowerFloorFinish(){
	//AllManage.UIALLPCStatic.show47();
	//PanelStatic.StaticBtnGameManager.RunOpenLoading(function() ServerRequest.requestTowerOpen());

	if(TrapTower.instance)
	{
	    TrapTower.instance.OpenTowerPanel(true);
	    //TrapTower.instance.EnableStartChallengeBtn();
	}
	else
	{
	    ShowTowerPanel();
		while(!TrapTower.instance){
			yield;
		}
		TrapTower.instance.OpenTowerPanel(true);
	}
}

function WowerFloorlose(){
	if(TrapTower.instance)
	{
	    TrapTower.instance.OpenTowerPanel(false);
	    //TrapTower.instance.EnableStartChallengeBtn();
	}
	else
	{
	    ShowTowerPanel();
		while(!TrapTower.instance){
			yield;
		}
		TrapTower.instance.OpenTowerPanel(false);
	}
	ServerRequest.requestTowerFailed();
}

function ShowTowerPanel()
{
    AllManage.UIALLPCStatic.show47();
}

function Show9()
{
    AllManage.UIALLPCStatic.show9();
}

static var ActivityBossHPMax : int = 0;
function SetActivityBossHPMax(hp : int){
	ActivityBossHPMax = hp;
}

var nowmapState : int = 0;
function SetmapState(state : int){
	nowmapState = state;
}

function RetrunsmeltGetNum(num : int){
	if(AllManage.SoulCLStatic){
		AllManage.SoulCLStatic.RerequestSmeltGetNum(num);
	}
}

function Retrunsmelt(num : int){
	if(AllManage.SoulCLStatic){
		AllManage.SoulCLStatic.ReturnrequestSmelt(num);
	}
}

function AnlaysUseSoul(num : int){
	if(AllManage.SoulCLStatic){
		AllManage.SoulCLStatic.SetSoulBuildLabel();
	}
	PlayerStatus.MainCharacter.SendMessage("RefreshSoul",SendMessageOptions.DontRequireReceiver);
	
	
	AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info552") + num + AllManage.AllMge.Loc.Get("info553"));	
}

function ClickRightMapGo(objs : Object[])
{
		if(objs[2]==3){
		AllManage.stictaskSceneIcon.RealGoLevelLeft1(objs[0],objs[1]);
		}else{
			MainTW.returnNowSaoDang();
		}
}

function EverydayAnimAction(id : int){
	AllManage.everyDayAction.GoAsID(id);
}

function DuelFaild(){
	if(AllManage.areCLStatic){
		AllManage.areCLStatic.JueDouWin(false);		
	}
}

function BuildSuccessful(){
		if(EquepmentBuildControl.me){
			EquepmentBuildControl.me.SendMessage("SuccessfulNow", SendMessageOptions.DontRequireReceiver);
			}
}

var StorageSpr : UIPanel;

