	#pragma strict
class JiaoChengInfo{
	var obj : GameObject[];
	var str : String[];
}
class TaskJiaoChengControl extends Song{
	function Awake(){
		AllManage.jiaochengCLStatic = this;
	}
	private var useYT1 : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("usePlayerInfo1","PlayerID");
	private var useYT2 : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("usePlayerInfo2","PlayerID");
	private var useYT3 : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("usePlayerInfo3","PlayerID");

	function Start(){
		if(canInstantiatePlayer && Application.loadedLevelName != "Loading 1"){
			CreateRobot();
		}
	}
	var boolOnce : boolean = false;
	function CreateRobot(){
		if(canInstantiatePlayer){
			canInstantiatePlayer = false;		
		}
		if(!boolOnce){
			boolOnce = true;
			PlayerInfo.PlayerShowInfo();
		}
//		PanelStatic.StaticBtnGameManager.PlayerShowInfo();
//		var useRow : yuan.YuanMemoryDB.YuanRow;
//		if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText != "1"){
//		useRow =   InventoryControl.yt.Rows[0].CopyTo();
//			useYT1.Add(useRow);
//			useYT1.Rows[0]["PlayerLevel"].YuanColumnText = "80";	
//			useYT1.Rows[0]["PlayerID"].YuanColumnText = "-2";
//			useYT1.Rows[0]["ProID"].YuanColumnText = "1";
//			useYT1.Rows[0]["EquipItem"].YuanColumnText = "1290535120000009990000000;;4790535120000009990000000;4790535120000009990000000;4490535120000009990000000;4590535120000009990000000;4290535120000009990000000;4690535120000009990000000;4890535120000009990000000;4990535120000009990000000;4190535120000009990000000;4390535120000009990000000;";
//			useYT1.Rows[0]["SkillsBranch"].YuanColumnText = "2";
//			useYT1.Rows[0]["Skill"].YuanColumnText =  "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;00;00;00;";
//			useYT1.Rows[0]["SkillsPostion"].YuanColumnText = "15,11,ProID_111;30,12,ProID_112;6,03,ProID_103;12,02,ProID_102;";
//			CreateTeamPlayer(useYT1 , 1);
//			yield WaitForSeconds(1);
//		}
//		if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText != "2"){
//		useRow =   InventoryControl.yt.Rows[0].CopyTo();
//			useYT2.Add(useRow);
//			useYT2.Rows[0]["ProID"].YuanColumnText = "2";
//			useYT2.Rows[0]["PlayerID"].YuanColumnText = "-3";
//			useYT2.Rows[0]["PlayerLevel"].YuanColumnText = "80";	
//			useYT2.Rows[0]["EquipItem"].YuanColumnText = "2190535120000009990000000;;5790535120000009990000000;5790535120000009990000000;5490535120000009990000000;5590535120000009990000000;5290535120000009990000000;5690535120000009990000000;5890535120000009990000000;5990535120000009990000000;5190535120000009990000000;5390535120000009990000000;";
//			useYT2.Rows[0]["SkillsBranch"].YuanColumnText = "1";
//			useYT2.Rows[0]["Skill"].YuanColumnText =  "30;30;20;22;20;21;21;12;12;10;10;10;00;00;00;20;20;20;10;10;00;00;00;";
//			useYT2.Rows[0]["SkillsPostion"].YuanColumnText = "12,06,ProID_206;15,07,ProID_207;6,04,ProID_204;30,08,ProID_208;";		
//			CreateTeamPlayer(useYT2 , 2);
//			yield WaitForSeconds(1);
//		}
//		if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText != "3"){
//		useRow =   InventoryControl.yt.Rows[0].CopyTo();
//			useYT3.Add(useRow);
//			useYT3.Rows[0]["ProID"].YuanColumnText = "3";
//			useYT3.Rows[0]["PlayerID"].YuanColumnText = "-4";
//			useYT3.Rows[0]["PlayerLevel"].YuanColumnText = "80";	
//			useYT3.Rows[0]["EquipItem"].YuanColumnText = "3290535120000009990000000;;6790535120000009990000000;6790535120000009990000000;6490535120000009990000000;6590535120000009990000000;6290535120000009990000000;6690535120000009990000000;6890535120000009990000000;6990535120000009990000000;6190535120000009990000000;6390535120000009990000000;";
//			useYT3.Rows[0]["SkillsBranch"].YuanColumnText = "2";
//			useYT3.Rows[0]["Skill"].YuanColumnText = "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;30;30;30;";
//			useYT3.Rows[0]["SkillsPostion"].YuanColumnText = "12,13,ProID_313;24,14,ProID_314;18,03,ProID_303;12,15,ProID_315;";
//			CreateTeamPlayer(useYT3 , 3);
//			yield WaitForSeconds(1);
//		}
		if(canInstantiatePlayer){
			canInstantiatePlayer = false;		
		}
	}
	function CreateRobot2(){
		if(canInstantiatePlayer){
			canInstantiatePlayer = false;		
		}
		var boolCreate : boolean = false;
		PanelStatic.StaticBtnGameManager.PlayerShowInfo();
		var useRow : yuan.YuanMemoryDB.YuanRow;
		if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "1" && !boolCreate){
			boolCreate = true;
			useRow =   InventoryControl.yt.Rows[0].CopyTo();
			useYT3.Add(useRow);
			useYT3.Rows[0]["ProID"].YuanColumnText = "3";
			useYT3.Rows[0]["PlayerID"].YuanColumnText = "-4";
			useYT3.Rows[0]["PlayerLevel"].YuanColumnText = "80";	
			useYT3.Rows[0]["EquipItem"].YuanColumnText = "3290535120000009990000000;;6790535120000009990000000;6790535120000009990000000;6490535120000009990000000;6590535120000009990000000;6290535120000009990000000;6690535120000009990000000;6890535120000009990000000;6990535120000009990000000;6190535120000009990000000;6390535120000009990000000;";
			useYT3.Rows[0]["SkillsBranch"].YuanColumnText = "2";
			useYT3.Rows[0]["Skill"].YuanColumnText = "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;30;30;30;";
			useYT3.Rows[0]["SkillsPostion"].YuanColumnText = "12,13,ProID_313;24,14,ProID_314;18,03,ProID_303;12,15,ProID_315;";
			CreateTeamPlayer(useYT3 , 3);
			yield WaitForSeconds(1);			
		}
		if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "2" && !boolCreate){
			boolCreate = true;
			useRow =   InventoryControl.yt.Rows[0].CopyTo();
			useYT3.Add(useRow);
			useYT3.Rows[0]["ProID"].YuanColumnText = "3";
			useYT3.Rows[0]["PlayerID"].YuanColumnText = "-4";
			useYT3.Rows[0]["PlayerLevel"].YuanColumnText = "80";	
			useYT3.Rows[0]["EquipItem"].YuanColumnText = "3290535120000009990000000;;6790535120000009990000000;6790535120000009990000000;6490535120000009990000000;6590535120000009990000000;6290535120000009990000000;6690535120000009990000000;6890535120000009990000000;6990535120000009990000000;6190535120000009990000000;6390535120000009990000000;";
			useYT3.Rows[0]["SkillsBranch"].YuanColumnText = "2";
			useYT3.Rows[0]["Skill"].YuanColumnText = "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;30;30;30;";
			useYT3.Rows[0]["SkillsPostion"].YuanColumnText = "12,13,ProID_313;24,14,ProID_314;18,03,ProID_303;12,15,ProID_315;";
			CreateTeamPlayer(useYT3 , 3);
			yield WaitForSeconds(1);
		}
		if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3" && !boolCreate){
			boolCreate = true;
			useRow =   InventoryControl.yt.Rows[0].CopyTo();
			useYT1.Add(useRow);
			useYT1.Rows[0]["PlayerLevel"].YuanColumnText = "80";	
			useYT1.Rows[0]["PlayerID"].YuanColumnText = "-2";
			useYT1.Rows[0]["ProID"].YuanColumnText = "1";
			useYT1.Rows[0]["EquipItem"].YuanColumnText = "1290535120000009990000000;;4790535120000009990000000;4790535120000009990000000;4490535120000009990000000;4590535120000009990000000;4290535120000009990000000;4690535120000009990000000;4890535120000009990000000;4990535120000009990000000;4190535120000009990000000;4390535120000009990000000;";
			useYT1.Rows[0]["SkillsBranch"].YuanColumnText = "2";
			useYT1.Rows[0]["Skill"].YuanColumnText =  "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;00;00;00;";
			useYT1.Rows[0]["SkillsPostion"].YuanColumnText = "15,11,ProID_111;30,12,ProID_112;6,03,ProID_103;12,02,ProID_102;";
			CreateTeamPlayer(useYT1 , 1);
			yield WaitForSeconds(1);
		}
		if(canInstantiatePlayer){
			canInstantiatePlayer = false;		
		}
	}

	function CreateTeamPlayer(ytFuben : yuan.YuanMemoryDB.YuanTable , proid : int){
		var obj : GameObject;
		var npcName : String;
		ytFuben.Rows[0]["ProID"].YuanColumnText = proid.ToString();

		switch(proid){
			case 1:
				npcName = AllManage.AllMge.Loc.Get("info736");
				break;
			case 2:
				npcName = AllManage.AllMge.Loc.Get("info737");
				break;
			case 3:
				npcName = AllManage.AllMge.Loc.Get("info738");
				break;
		}
	//	print(ytFuben.Rows[0]["ProID"].YuanColumnText);
		obj = AllResources.ar.CreatePlayerFuBen(proid , UIControl.myTeamInfo , ytFuben.Rows[0]["PlayerName"].YuanColumnText , 	PlayerStatus.MainCharacter.position);
	    obj.SendMessage("SetMaster",PlayerStatus.MainCharacter, SendMessageOptions.DontRequireReceiver);
		obj.SendMessage("SetPlayerInfoAsYt" , ytFuben , SendMessageOptions.DontRequireReceiver);
	}

	var TransHeCheng : Transform[]; 
	var StrHeCheng : String[];
	var TransSkills : Transform[];
	var StrSkills : String[]; 
	var allJiaoCheng : JiaoChengInfo[];
	var NowJC : Transform[];  
	var NowStr : String[];
	var JiaoChengID : int;
	var brhcl : BranchControl;
	//var uiallpcl : UIAllPanelControl;
	private var isbool : boolean = false;
	function SelectJiaoChengAsID(id : int){ 
		JiaoChengID = id;
		step = 0; 
		if(JiaoChengID == 3){
			if(parseInt(InventoryControl.yt.Rows[0]["NonPoint"].YuanColumnText) < 8){
				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info767"));		
				return;
			}
		}
	//	//print(id + " == jiao cheng id");
		if(JiaoChengID == 4 && step == 0){
			yield;
			yield;
			yield;
			AllManage.UIALLPCStatic.show15();
			yield;
			yield;
			yield;
		 if(brhcl==null)
		   brhcl = FindObjectOfType(BranchControl);
			if(brhcl)
				brhcl.OpenBranch();
		}
		if(JiaoChengID == 5 && step == 0){   
	//		AllManage.UICLStatic.nowNPCType = NPCFunctions.WeaponShop; 
	//		AllManage.UICLStatic.NpcFunc();
			AllManage.UICLStatic.OpenStoreAsType(parseInt(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText));
	//		var npcs : npcAI[];
	//		npcs = FindObjectsOfType(npcAI); 
	//		
	//		for(var i=0; i<npcs.length; i++){
	//			if(npcs[i].npctype == NPCFunctions.WeaponShop){
	//				npcs[i].ShowNpcTieJiang();
	//			}
	//		}
			
	//		AllManage.UIALLPCStatic.show15();
	//		yield;
	//		yield;
	//		yield;
	//	 if(brhcl==null)
	//	   brhcl = FindObjectOfType(BranchControl);
	//		brhcl.OpenBranch();
		}else
		if(JiaoChengID == 6){
			isbool = DoHaveFish();
			if(!isbool){
				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info641"));
				return;
			}
		}else
		if(JiaoChengID == 7){
			TeamJiaoCheng1();
		}
	//	switch(id){
	//		case 0:
	//			NowJC = TransHeCheng; 
	//			NowStr = StrHeCheng;
	//			break;
	//		case 1:
	//			NowJC = TransSkills;
	//			NowStr = StrSkills;
	//			break;
	//	}
		ShowJiaoCheng();
	}

	function Update(){
		if(JiaoChengID == 3 && step == 0){
			if(! AllManage.UICLStatic.ButtonsShowZhu[1].active){
				AllManage.UICLStatic.ButtonsShowZhu[1].SetActiveRecursively(true);
			}
		}
	}	 

	var cb : GameObject;
	function charBarText(){
		AllManage.tsStatic.LiaoTian(AllManage.AllMge.Loc.Get("info731"), Color.red); 
	}

	function TeamJiaoCheng1(){
	//	NextStep();
		AllManage.qrStatic.ShowQueRen(gameObject , "YesTeam" , "" , "info730");
	}

	function YesTeam(){
		charBarText();
		yield WaitForSeconds(2);
		NextStep();
	}

	function TeamJiaoCheng2(){
		AllManage.qrStatic.ShowQueRen(gameObject , "YesFuben" , "" , "info732");
	}

	static var canInstantiatePlayer : boolean = false;
	function YesFuben(){
		NextStep();
	}

	function InstructorsGO(){
		canInstantiatePlayer = true;
		Loading.Level = "Map211";
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		AllManage.UICLStatic.RemoveAllTeam();
		alljoy.DontJump = true;
		if(UIControl.mapType == MapType.zhucheng)
			DungeonControl.ReLevel = Application.loadedLevelName;
				PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}

	var step : int;
	var ParentButton : GameObject;
	var ButtonLeft : GameObject;
	var ButtonRight : GameObject; 
	var ButtonLeftTP : GameObject;
	var ButtonRightTP : GameObject; 
	var ButtonCenter : GameObject;
	
//	var meinv : GameObject;
	var LabelLeft : UILabel;
	var LabelRight : UILabel;
	var LabelLeftTP : UILabel;
	var LabelRightTP : UILabel;
	var LabelCenterTP : UILabel;
	function ShowJiaoCheng(){ 
		LabelLeft.text = AllManage.AllMge.Loc.Get( allJiaoCheng[JiaoChengID].str[step] );
		LabelRight.text = AllManage.AllMge.Loc.Get( allJiaoCheng[JiaoChengID].str[step] );
		LabelLeftTP.text = AllManage.AllMge.Loc.Get( allJiaoCheng[JiaoChengID].str[step] );
		LabelRightTP.text = AllManage.AllMge.Loc.Get( allJiaoCheng[JiaoChengID].str[step] );
		LabelCenterTP.text = AllManage.AllMge.Loc.Get( allJiaoCheng[JiaoChengID].str[step] );
		if(!isGen){
			ButtonGenSui();
		}
		ParentButton.SetActiveRecursively(false);
		yield;
		if((JiaoChengID == 3 && step == 1) || JiaoChengID == 2 && step == 1){
			yield;
			yield;
			yield;
			yield;
			yield;
		}
			yield;
			yield;
			yield;
			yield;
			yield;
		if(JiaoChengID == 8 && step == 2 && AllManage.SoulCLStatic){
			AllManage.SoulCLStatic.shanSoul.enabled = true;
		}else{
			if(AllManage.SoulCLStatic)
				AllManage.SoulCLStatic.shanSoul.enabled = false;		
		}
		if(JiaoChengID == 8 && step == 4 && AllManage.SoulCLStatic){
			AllManage.SoulCLStatic.shanDigest.enabled = true;
		}else{
			if(AllManage.SoulCLStatic)
				AllManage.SoulCLStatic.shanDigest.enabled = false;		
		}
		if(ParentButton.transform.localPosition.x <= 0){
			if(ParentButton.transform.localPosition.y > 0){
				ParentButton.active = true;
				ButtonLeft.SetActiveRecursively(true);		
			}else{
				ParentButton.active = true;
				ButtonLeftTP.SetActiveRecursively(true);					
			}
		}else{
			if(ParentButton.transform.localPosition.y > 0){
				ParentButton.active = true;
				ButtonRight.SetActiveRecursively(true);	
			}else{
				ParentButton.active = true;
				ButtonRightTP.SetActiveRecursively(true);				
			}
		}
		if(JiaoChengID == 4 && step == 0){
			ParentButton.SetActiveRecursively(false);
			ParentButton.active = true;
			ButtonCenter.SetActiveRecursively(true);		
		}else
		if(JiaoChengID == 0 && step == 1){
			ParentButton.SetActiveRecursively(false);
			ParentButton.active = true;
			ButtonLeft.SetActiveRecursively(true);
		}else
		if((JiaoChengID == 8 && step == 2) || (JiaoChengID == 8 && step == 4)){
			ParentButton.SetActiveRecursively(false);
			ParentButton.active = true;
			ButtonLeftTP.SetActiveRecursively(true);
		}
		if(JiaoChengID == 7 && step == 1){
			TeamJiaoCheng2();
		}
		if(JiaoChengID == 10 && step == 0){
			ParentButton.SetActiveRecursively(false);
			ParentButton.active = true;
			ButtonLeftTP.SetActiveRecursively(true);				
			AllManage.AllMge.btnYingMo.SetActiveRecursively(true);
		}
		if(JiaoChengID == 6 && step == 0){
			ParentButton.SetActiveRecursively(false);
			ParentButton.active = true;
			ButtonRight.SetActiveRecursively(true);			
			AllManage.AllMge.btnFish.SetActiveRecursively(true);
		}
		if(JiaoChengID == 8 && step == 0){
			ParentButton.SetActiveRecursively(false);
			ParentButton.active = true;
			ButtonLeftTP.SetActiveRecursively(true);			
			AllManage.AllMge.btnSoul.SetActiveRecursively(true);
		}
		if(JiaoChengID == 9 && step == 3){
			ParentButton.SetActiveRecursively(false);
			ParentButton.active = true;
			ButtonCenter.SetActiveRecursively(true);			
		}
//		if(JiaoChengID == 0 && step == 0){
//			ParentButton.SetActiveRecursively(false);
//			ParentButton.active = true;
//			ButtonLeft.SetActiveRecursively(true);
//		}
	}

	var isGen : boolean = false;
	function ButtonGenSui(){
		isGen = true;
		while(isGen){
			if(allJiaoCheng[JiaoChengID].obj[step]){
				if( !allJiaoCheng[JiaoChengID].obj[step].active){
					ParentButton.transform.position.x = 500000;
				}else{
					ParentButton.transform.position.x = allJiaoCheng[JiaoChengID].obj[step].transform.position.x;
					ParentButton.transform.position.y = allJiaoCheng[JiaoChengID].obj[step].transform.position.y;	 
				}
			}
			yield;
		}
	}

	function GiveUpJiaoCheng(taskID : String){
		yield;
		yield;
		ParentButton.SetActiveRecursively(false);
		mtw.FangQiAsID(taskID);
		yield;
		yield;
		if(taskID != "62"){
			AllManage.UIALLPCStatic.show0();	
		}
	}

	var mtw : MainTaskWork;
	function NextStep(){
		if((JiaoChengID == 4 && step == 0) || (JiaoChengID == 8 && step == 2) || (JiaoChengID == 8 && step == 4 ) || (JiaoChengID == 9 && step == 3 ) || (JiaoChengID == 13 && step == 1 ) || (JiaoChengID == 13 && step == 2 )){
			if((JiaoChengID == 13 && step == 1 )){
				if(AllManage.InvclStatic.TutorialsDetectionAsIDNoSave("132") ){
					PanelGamble.panelGamble.GetItemsTask();			
				}else{
					PanelGamble.panelGamble.GetItems();
				}
			}else
			if((JiaoChengID == 13 && step == 2 )){
				ptime = Time.time + 3;
				if(AllManage.InvclStatic.TutorialsDetectionAsID("132")){
					PanelGamble.panelGamble.BtnEnterTask();
				}else{
					PanelGamble.panelGamble.BtnEnter();
				}
			}
		}else{
			try{
				allJiaoCheng[JiaoChengID].obj[step].SendMessage("OnClick" , SendMessageOptions.DontRequireReceiver); 				
			}catch(e){
				ParentButton.SetActiveRecursively(false); 
//				step = 0;
//				ShowJiaoCheng();
//				print("JC BG");
			}
		}
		if(JiaoChengID == 10 && step == 0){
			while(PanelOfflinePlayer.panelOfflinePlayer == null){
				yield;
			}
			PanelStatic.StaticBtnGameManager.ShowRobotControl();
		}
	//	//print("send le " + NowJC[step]); 
	//	yield WaitForSeconds(5);
		if(step == allJiaoCheng[JiaoChengID].obj.Length - 1){
	//		//print("wan cheng"); 
			isGen = false;
			ParentButton.SetActiveRecursively(false); 
			mtw.DoneJiaoCheng(JiaoChengID);
			JiaoChengID= 0;
			step = 0;
		}else{	
			step += 1;
			yield;
			ShowJiaoCheng();
		}
	}

	var ptime : int = 0;
	function DoNext(){
		if(ptime > Time.time){
			return;
		}
		if((JiaoChengID == 4 && step == 0) || (JiaoChengID == 8 && step == 2) || (JiaoChengID == 8 && step == 4 ) || (JiaoChengID == 9 && step == 3 )){
		}else{
			NextStep();
		}
	}

	private var Sstr : String = ";";
	function DoHaveFish() : boolean{
		var inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
		var inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
		var inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
		var inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
		var useInvID : String[];
		
		var i : int = 0;	
		var useStr : String;
		var inv : InventoryItem;
		useStr = inv1 + inv2 + inv3 + inv4; 
	//	//print("useStr == " + useStr);
		useInvID = useStr.Split(Sstr.ToCharArray());
		for(i=0; i<useInvID.length; i++){
			if(useInvID[i] != "" && parseInt(useInvID[i].Substring(0,1)) == 8 && parseInt(useInvID[i].Substring(1,1)) == 2){
				return true;
			}
		}
		return false;
	}
}