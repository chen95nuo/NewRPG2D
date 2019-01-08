	#pragma strict
class InventoryControl extends Song{
	var bagInventorys : InventoryItem[];
	var equepmentIDs : int[];
	static var www : boolean = true;
	var PlayerId : String;
	var maxSolutionNum : int = 0;
	var maxXuePingNum : int = 0;// 最大血瓶数量，和vip对应
	var vipLevel : int = 0;// vip等级
	var mtw : MainTaskWork;
	//var LT : LootTable;
	var inv : InventoryItem[];
	static var PlayerProfession : ProfessionType;
	static var Plys : PlayerStatus;
	var ProfessionLabel : UILabel;
	var yuanPhoton : YuanUnityPhoton; 
	var UICL : UIControl;
	static var yt : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("PlayerInfo","PlayerID");
	static var VIpTable  : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("nnn","PlayerID");
	static var BattlefieldTable  : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("BattlefieldTableID","id");
	static var GameItem : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("GameItem1","id");
	static var TablePacks : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("Packs","id");
	static var Blueprint : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("Blueprint1","id");
	static var PlayerTitle : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("PlayerTitle1","id");
	static var TaskItem : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("TaskItem1","id");
	static var PlayerPet : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("PlayerPet1","id");

	static var ytLoad : boolean = false; 

	//var invMaker : Inventorymaker;
	private var Fstr : String = ";";
	var Inventory1 : String;
	var Inventory2 : String;
	var Inventory3 : String;
	var Inventory4 : String; 
	var EquipItemStr : String;
	var DaoJuItemStr : String;
	var RideItemStr : String;
	static var PlayerSelect : String;
	static var PlayerID : String;

	//var gm : GameObject;
	var btnsd : GameObject;
	var ServerAddress : String;
	var ServerApplication : String;
	static var xingdongzhi : int;
	var VipYaoPing : int;
	var VipYaoPingLevel : int;
	var VipInventoryUpdate : String;
	var VipMaxPower : int;
	var DaojuStr : String[];
	static var PlayerInventoryNum : int;
	static var PlayerBankInventoryNum : int;
	var SpeakComplete : String;
	var useSpeakComplete : String[];
	var TimeGOWCard : int;
	var TimeDoubleCard : int;

	var expFishing : int;
	var expMining : int;
	var expCooking : int;
	var expMake : int;

	var LabelVIP : UILabel;
	var LabelHP : UILabel;
	private var useYT : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("usePlayerInfo","PlayerID");
	private var useYT2 : yuan.YuanMemoryDB.YuanTable; 
	var sendACT_PlaySynAttr : boolean = true;
	
	function Awake(){
		if(isSkipTraining){
			isSkipTraining = false;
			//TD_info.skipSuccess();
		}
		Equeipitems = new Array(12);
		Loading.returnTimes = 0;
		AllManage.InvclStatic = this;
		try{
			PanelStatic.StaticBtnGameManager.InvMake = AllResources.InvmakerStatic.gameObject;
		}catch(e){
		
		}
		
	//	PanelStatic.StaticBtnGameManager.InvMake = AllResources.InvmakerStatic.gameObject;
		if(Application.loadedLevelName == "Map200"){
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.StartTraining).ToString());
			var useRow : yuan.YuanMemoryDB.YuanRow;
			useRow = yt.Rows[0].CopyTo();
			useYT.Add(useRow);
			useYT.Rows[0]["PlayerLevel"].YuanColumnText = "80";	
			if(yt.Rows[0]["ProID"].YuanColumnText == "1"){
				useYT.Rows[0]["EquipItem"].YuanColumnText = "1260822421222003000000000;1160831421222005000000000;4960831423252005000000000;4960831423252005000000000;4460831325222005000000000;4560831325222005000000000;4260831325222005000000000;4660822124282005000000000;4860831423252005000000000;4760831325222005000000000;4160831325222005000000000;4360831325222005000000000;";
				useYT.Rows[0]["SkillsBranch"].YuanColumnText = "2";
				useYT.Rows[0]["Skill"].YuanColumnText =  "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;00;00;00;";
				useYT.Rows[0]["SkillsPostion"].YuanColumnText = "15,11,ProID_111;30,12,ProID_112;6,03,ProID_103;12,02,ProID_102;";
			}else
			if(yt.Rows[0]["ProID"].YuanColumnText == "2"){
				useYT.Rows[0]["EquipItem"].YuanColumnText = "2160822421321002000000000;2260813431221002000000000;5960813421222005000000000;5960813421222005000000000;5460813824212005000000000;5560813824212005000000000;5260813824212005000000000;5660813124282005000000000;5860813824212005000000000;5760813824212005000000000;5160813824212005000000000;5360813824212005000000000;";
				useYT.Rows[0]["SkillsBranch"].YuanColumnText = "1";
				useYT.Rows[0]["Skill"].YuanColumnText =  "30;30;20;22;20;21;21;12;12;10;10;10;00;00;00;20;20;20;10;10;00;00;00;";
				useYT.Rows[0]["SkillsPostion"].YuanColumnText = "12,06,ProID_206;15,07,ProID_207;6,04,ProID_204;30,08,ProID_208;";		
				useYT.Rows[0]["Stamina"].YuanColumnText = "1300";
			}else
			if(yt.Rows[0]["ProID"].YuanColumnText == "3"){
				useYT.Rows[0]["EquipItem"].YuanColumnText = "3260813421222005000000000;3160831421222003000000000;6960813421371005000000000;6960813421371005000000000;6460813822212005000000000;6560813822212005000000000;6260813822212005000000000;6660813122282005000000000;6860813421361005000000000;6760813822212005000000000;6160813822212005000000000;6360813822212005000000000;";
				useYT.Rows[0]["SkillsBranch"].YuanColumnText = "2";
				useYT.Rows[0]["Skill"].YuanColumnText = "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;30;30;30;";
				useYT.Rows[0]["SkillsPostion"].YuanColumnText = "12,13,ProID_313;24,14,ProID_314;18,03,ProID_303;12,15,ProID_315;";
				useYT.Rows[0]["Stamina"].YuanColumnText = "1300";
			}
			useYT.Rows[0]["PlayerID"].YuanColumnText = "-1";
			useYT2 = yt;
			yt = useYT;
		}
    addint = new Array (14);	
	for (var i : int = 0; i < 14; i++) { 	
	    addint[i]=Random.Range(1,6);
	}	
	}
private var addint : int[];


	function RetrunYT(){
		yt = useYT2;
//		print(yt.Rows[0]["EquipItem"].YuanColumnText);
//		print(yt.Rows[0]["SkillsBranch"].YuanColumnText);
//		print(yt.Rows[0]["Skill"].YuanColumnText);
//		print(yt.Rows[0]["SkillsPostion"].YuanColumnText);
		
	//	PhotonNetwork.LeaveRoom();
		Loading.Level = "Map111";
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
		AllManage.UICLStatic.RemoveAllTeam();
		alljoy.DontJump = true;
			PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

	}
	
	static var isSkipTraining : boolean = false;
	function SkipTraining(){
		//TD_info.setSkipScreen();
		isSkipTraining = true;
		RetrunYT();
	}
	
		public function desP(){
			while(PhotonNetwork.isMasterClient){
				yield;
			}
			PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
		}

	private var FStr : String = ";";

	var EquipItemSoul : String;

	function Start () {
			var i : int = 0;
			for( i=0; i<EquipStatus.length; i++){
				EquipStatus[i] = 0;
			}
			for( i=0; i<PES.length; i++){
				PES[i].inv = null;
			}
			ClearDaoJuObj();
			var mm : boolean = false;
			InRoom.GetInRoomInstantiate().PlayerInMap(Application.loadedLevelName);
			Blueprint = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBlueprint;
			VIpTable = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytPlayerService;
			BattlefieldTable = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytBattlefield;
			GameItem = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytGameItem;
			TablePacks = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytTablePacks;
			PlayerTitle = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytPlayerTitle;
			TaskItem = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytTaskItem;
			PlayerPet = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytPlayerPet;
			BtnGameManager.yt=this.yt;
			EquipItemSoul = InventoryControl.yt.Rows[0]["EquipItemSoul"].YuanColumnText;
			if(EquipItemSoul == ""){
				EquipItemSoul = ";;;;;;;";
			}
			PlayerSelect = yt.Rows[0]["ProID"].YuanColumnText;
			Inventory1 = yt.Rows[0]["Inventory1"].YuanColumnText;
			Inventory2 = yt.Rows[0]["Inventory2"].YuanColumnText;
			Inventory3 = yt.Rows[0]["Inventory3"].YuanColumnText;
			Inventory4 = yt.Rows[0]["Inventory4"].YuanColumnText;
			EquipItemStr = yt.Rows[0]["EquipItem"].YuanColumnText;
			DaoJuItemStr = yt.Rows[0]["Item"].YuanColumnText;
			DaoJuNoBug();
			
			TimeGOWCard = GetBDInfoInt("GOWCard" , 0);
			TimeDoubleCard = GetBDInfoInt("DoubleCard" , 0);
			SpeakComplete = yt.Rows[0]["SpeakComplete"].YuanColumnText;
			
			expFishing = GetBDInfoInt("FishingExp" , 0);
			expMining = GetBDInfoInt("MiningExp" , 0);
			expCooking = GetBDInfoInt("CookingExp" , 0);
			expMake = GetBDInfoInt("MakeExp" , 0);
			if(Application.loadedLevelName != "Map200"){
				JianCeShan();
			}
			PlayerInventoryNum = GetBDInfoInt("InventoryNum" , 1);
			if(PlayerInventoryNum > 4){
				PlayerInventoryNum = 4;
			}
			RideItemStr = yt.Rows[0]["Mounts"].YuanColumnText;
			mm = true;
			ytLoad = true;
			var Pvip : String = GetBDInfoInt("Serving" , 0).ToString();
			xingdongzhi = parseInt(VIpTable.SelectRowEqual("VIPType" , Pvip)["HP"].YuanColumnText);
			VipYaoPing = GetBDInfoInt("SolutionNum",0); 
			var bool  : boolean = false;
			bool = LookTimesAs(yt.Rows[0]["SolutionTime"].YuanColumnText , InRoom.GetInRoomInstantiate().serverTime.ToString() , "day" , 7); 
			maxSolutionNum = parseInt(VIpTable.SelectRowEqual("VIPType" , Pvip)["SolutionUpdate"].YuanColumnText);
			maxXuePingNum = parseInt(VIpTable.SelectRowEqual("VIPType" , Pvip)["numSolution"].YuanColumnText);
			vipLevel = parseInt(Pvip);
			var useMyBottleLevel : int = GetBDInfoInt("SolutionLevel",1);
//			print(useMyBottleLevel + " == my == " + maxSolutionNum + " == vip == " + Pvip + " == serv");
			if(useMyBottleLevel > maxSolutionNum){
				VipYaoPingLevel = useMyBottleLevel; 			
			}else{
				VipYaoPingLevel = maxSolutionNum;
				if(VipYaoPingLevel > 9){
					VipYaoPingLevel = 9;
				}
			}
			if(GetVIPBagNum() > PlayerInventoryNum){
				PlayerInventoryNum = GetVIPBagNum();
			}
//			print(VipYaoPingLevel + " == VipYaoPingLevel");
			UICL.ShowYaoShuiIcon(VipYaoPingLevel);
			UICL.UseYaoPingAsNum1(0 , 10);
			SetInitValue();
//			print(AllResources.ar);
//			print(PhotonNetwork.connected);
			if(AllResources.ar&&PhotonNetwork.connected){
				AllResources.ar.CreatePlayer(parseInt(yt.Rows[0]["ProID"].YuanColumnText) , UICL.myTeamInfo , yt.Rows[0]["PlayerName"].YuanColumnText);
			yield;
			if(ps == null && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			} 
			if(ps != null && AllResources.ridemod){
				var canRideBool : boolean = false;
				if(UIControl.mapType == MapType.zhucheng){
					canRideBool = true;
				}else{
					if(InventoryControl.yt.Rows[0]["SelectMounts"].YuanColumnText != "" && parseInt(InventoryControl.yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(4,1)) > 3)
						canRideBool = true;
				}
							
				if(canRideBool)
					ps.rideOn(parseInt(InventoryControl.yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(2,2)) - 1 , parseInt(InventoryControl.yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(5,1)));
			}
			Plys = ps;
//			timeShangXian = System.DateTime.Now.ToString();
			if(Application.loadedLevelName != "Map200"){
				RewardShangXian(); 
			}
			UICL.showPlayer(parseInt(PlayerSelect));
			PlayerProfession = parseInt(yt.Rows[0]["ProID"].YuanColumnText);
//			ProfessionLabel.text = yt.Rows[0]["PlayerName"].YuanColumnText;
			yield; 
			yield; 
			ClearBagItem();
			SetEquepSoulItem(EquipItemSoul);
			SetSelectBagItem(Inventory1);
			SetEquipItem(EquipItemStr);
			GetPersonEquipment();
			ShowFuYeLevel();
		    ps.Health = ps.Maxhealth;	
		    ServerRequest.requestSetCurHP(parseInt(ps.Health));
	    if(ps.ProID==1)
		ps.Mana = "30";
		else if(ps.ProID==2)
		ps.Mana = "0";
			else
				ps.Mana = ps.Maxmana; 
		    if(Application.loadedLevelName == "Map200"){
		    	ps.Mana = ps.Maxmana;	
		    }	
			
//			ShowBagButton(BagID);
//			SetDaoJuItem();
//			SetRideItem();
//			GetTalkShow();
//			ReadVIPLevel();
			LabelHP.text = xingdongzhi.ToString();
		}
//		if(GetBDInfoInt("PlayerLevel" , 0) > 14){
//			AllManage.UIALLPCStatic.showPVP20();
//		}
//		StartGuiWei();
//		if(yt.Rows[0]["SelectMounts"].YuanColumnText.Length > 2){
//			RideButtonOn(parseInt(yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(2,2)).ToString());	
//		}
	if(yt.Rows[0]["SelectMounts"].YuanColumnText.Length > 2){
		RideButtonOn(parseInt(yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(2,2)).ToString());	
	}
//		yt.Rows[0]["Inventory1"].YuanColumnText = "8970,01,3;2101143421000000003000000;5210433315172000000000000,01;2203332425300000000000000,01;5910423422211000000000000,01;2107322112100000000000000,01;2103333421200000000000000,01;5402114131111000000000000,01;5402114131111000000000000,01;5402114131111000000000000,01;5402114131111000000000000,01;5402114131111000000000000,01;5402114131111000000000000,01;5402114131111000000000000,01;;";
//		yt.Rows[0]["Inventory2"].YuanColumnText = "";
//		yt.Rows[0]["Inventory2"].YuanColumnText = "";
//		yt.Rows[0]["Inventory2"].YuanColumnText = "";
		
						
//						print("Start ===== " + yt.Rows[0]["Inventory1"].YuanColumnText);
//						print(yt.Rows[0]["Inventory2"].YuanColumnText);
//						print(yt.Rows[0]["Inventory3"].YuanColumnText);
//						print(yt.Rows[0]["Inventory4"].YuanColumnText + " ===== End");
		InvokeRepeating("Updateyt", 2, 2); 
		InvokeRepeating("UpDateSt", 2, 1);  
//					yt.Rows[0]["Skill"].YuanColumnText = "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;30;30;30;";
			if(UIControl.mapType != MapType.zhucheng)
				AllManage.UIALLPCStatic.showSkill32();
		yield WaitForSeconds(4); 
		if(PlayerStatus.MainCharacter)
			PlayerStatus.MainCharacter.SendMessage("CanAddHealth" ,  SendMessageOptions.DontRequireReceiver);
		if(Application.loadedLevelName.Substring(3,1) == "1"){
			yt.Rows[0]["Place"].YuanColumnText = Application.loadedLevelName.Substring(3,3);
		} 
}

function GetVIPBagNum(){
	try{
		return parseInt(VIpTable.SelectRowEqual("VIPType" , GetBDInfoInt("Serving" , 0).ToString())["InventoryUpdate"].YuanColumnText.Split(Fstr.ToCharArray())[0]);
	}catch(e){
		return 1;
	}
}

function DaoJuNoBug(){
	UseDaojuIDs = DaoJuItemStr.Split(Fstr.ToCharArray());
	var i : int = 0;
	for(i=0; i<UseDaojuIDs.length; i++){
		if(UseDaojuIDs[i].Length < 7 && UseDaojuIDs[i].Length > 4){
			UseDaojuIDs[i] = UseDaojuIDs[i].Substring(0,5) + "0" + UseDaojuIDs[i].Substring(5,1);
		}
	}
	DaoJuItemStr = "";
	for(i=0; i<UseDaojuIDs.length; i++){
		DaoJuItemStr += UseDaojuIDs[i] + ";";
	}
}

	var btnBattleSelect : GameObject;
	function SetObj(AS : AwakeSet){
		AllManage.jiaochengCLStatic.allJiaoCheng[0].obj[1] = AS.allJiaoCheng[0].obj[1];
		AllManage.jiaochengCLStatic.allJiaoCheng[0].obj[2] = AS.allJiaoCheng[0].obj[2];
		AllManage.jiaochengCLStatic.allJiaoCheng[0].obj[3] = AS.allJiaoCheng[0].obj[3];
		AllManage.jiaochengCLStatic.allJiaoCheng[0].obj[4] = AS.allJiaoCheng[0].obj[4];
		AllManage.jiaochengCLStatic.allJiaoCheng[1].obj[1] = AS.allJiaoCheng[1].obj[1];
		AllManage.jiaochengCLStatic.allJiaoCheng[1].obj[5] = AS.allJiaoCheng[1].obj[5];
		AllManage.jiaochengCLStatic.allJiaoCheng[2].obj[1] = AS.allJiaoCheng[2].obj[1];
		AllManage.jiaochengCLStatic.allJiaoCheng[2].obj[2] = AS.allJiaoCheng[2].obj[2];
		AllManage.jiaochengCLStatic.allJiaoCheng[3].obj[3] = AS.allJiaoCheng[3].obj[3];
		AllManage.jiaochengCLStatic.allJiaoCheng[5].obj[0] = AS.allJiaoCheng[5].obj[0];
		AllManage.jiaochengCLStatic.allJiaoCheng[5].obj[1] = AS.allJiaoCheng[5].obj[1];
		AllManage.jiaochengCLStatic.allJiaoCheng[5].obj[2] = AS.allJiaoCheng[5].obj[2];
		AllManage.jiaochengCLStatic.allJiaoCheng[6].obj[3] = AS.allJiaoCheng[6].obj[3];
		AllManage.jiaochengCLStatic.allJiaoCheng[9].obj[1] = AS.allJiaoCheng[9].obj[1];
		AllManage.jiaochengCLStatic.allJiaoCheng[9].obj[5] = AS.allJiaoCheng[9].obj[5];
		
		AllManage.UICLStatic.ObjBackGroundPar = AS.ObjBackGroundPar ;		
		AllManage.UICLStatic.LabelCangZheng = AS.LabelCangZheng ;
		AllManage.UICLStatic.SpritePlayerTIao1 = AS.SpritePlayerTIao1 ;
		AllManage.UICLStatic.TransStores = AS.TransStores ;
		AllManage.UICLStatic.TransPlayerInfo = AS.TransPlayerInfo ;
		AllManage.UICLStatic.Objshulan = AS.Objshulan ;
		AllManage.UICLStatic.Objdiban = AS.Objdiban ;
		
		btnBattleSelect = AS.btnBattleSelect;
		randCam = AS.randCam;
		ProfessionLabel = AS.ProfessionLabel;
		ProfessionLabel.text = yt.Rows[0]["PlayerName"].YuanColumnText;
		ButtonsBag = AS.ButtonsBag ;
		DaojuGrid = AS.DaojuGrid ;
		EquepInfo = AS.EquepInfo ;
		EquipIt = AS.EquipIt ;
		SpriteQieHuanBag = AS.SpriteQieHuanBag ;
		TransQieHuanBag = AS.TransQieHuanBag ;
		PanelDaoJu = AS.PanelDaoJu ;
//		invCangku = AS.invCangku ;
		TransQieHuanParentEquep = AS.TransQieHuanParentEquep ;
		buttonsBagCangKu = AS.buttonsBagCangKu ;
		SpriteCangku = AS.SpriteCangku ;
		SpriteShangdian = AS.SpriteShangdian ;
		LabelCangku = AS.LabelCangku ;
		SpriteQieHuanEquep = AS.SpriteQieHuanEquep ;
		TransQieHuanEquep = AS.TransQieHuanEquep ;
		invinfoOBj = AS.invinfoOBj ;
		LabelEquepShuXing = AS.LabelEquepShuXing ;
		bagButtons = AS.bagButtons ;
		SelectBagGuangs = AS.SelectBagGuangs ;
		eqInfoObj = AS.eqInfoObj ;
		BagIt = AS.BagIt ;
		bagParent = AS.bagParent ;
		LabelExp = AS.LabelExp ;
		LabelHp = AS.LabelHp ;
		LabelMana = AS.LabelMana ;
		LabelAtk = AS.LabelAtk ;
		LabelAtkM = AS.LabelAtkM ;
		LabelBaoJi = AS.LabelBaoJi ;
		LabelJingZhun = AS.LabelJingZhun ;
		LabelHuJia = AS.LabelHuJia ;
		LabelFangYu = AS.LabelFangYu ;
		LabelShanBi = AS.LabelShanBi ;
		LabelFangYuMo = AS.LabelFangYuMo ;
		LabelPianXie = AS.LabelPianXie ;
		LabelNaiLi = AS.LabelNaiLi ;
		LabelLiLiang = AS.LabelLiLiang ;
		LabelMinJie = AS.LabelMinJie ;
		LabelZhiLi = AS.LabelZhiLi ;
		LabelZhuanZhu= AS.LabelZhuanZhu ;
		LabelCombat= AS.LabelCombat ;
		BarExp = AS.BarExp ;
		PlayerInfoRightOtherLabelHuJia = AS.PlayerInfoRightOtherLabelHuJia ;
		PlayerInfoRightOtherLabelFangYuMo = AS.PlayerInfoRightOtherLabelFangYuMo ;
		PlayerInfoRightOtherLabelAtk = AS.PlayerInfoRightOtherLabelAtk ;
		PlayerInfoRightOtherLabelAtkM= AS.PlayerInfoRightOtherLabelAtkM ;
		PlayerInfoRightOtherLabelLevel = AS.PlayerInfoRightOtherLabelLevel ;
		PlayerInfoRightOtherLabelhp = AS.PlayerInfoRightOtherLabelhp ;
		PlayerInfoRightOtherLabelnv = AS.PlayerInfoRightOtherLabelnv ;
		PlayerInfoRightOtherLabelName = AS.PlayerInfoRightOtherLabelName ;
		PlayerInfoRightOtherLabelVIP = AS.PlayerInfoRightOtherLabelVIP ;
		PlayerInfoRightOtherLabelHealthP = AS.PlayerInfoRightOtherLabelHealthP ;
		PlayerInfoRightOtherLabelPrestige = AS.PlayerInfoRightOtherLabelPrestige ;
		PlayerInfoRightOtherLabelPVPPoint= AS.PlayerInfoRightOtherLabelPVPPoint ;
		
		OtherLabelConquest = AS.OtherLabelConquest;
		OtherLabelHero = AS.OtherLabelHero;
		
		fsHP = AS.fsHP ;
		fsNU = AS.fsNU ;
		LabelExpNew1= AS.LabelExpNew1 ;
		LabelExpNew2 = AS.LabelExpNew2 ;
		fsEXP1 = AS.fsEXP1 ;
		fsEXP2= AS.fsEXP2 ;
		SeOtherLabelHp = AS.SeOtherLabelHp ;
		SeOtherLabelMana = AS.SeOtherLabelMana ;
		SeOtherLabelAtk = AS.SeOtherLabelAtk ;
		SeOtherLabelAtkM= AS.SeOtherLabelAtkM ;
		SeOtherLabelBaoJi = AS.SeOtherLabelBaoJi ;
		SeOtherLabelJingZhun = AS.SeOtherLabelJingZhun ;
		SeOtherLabelHuJia = AS.SeOtherLabelHuJia ;
		SeOtherLabelFangYu = AS.SeOtherLabelFangYu ;
		SeOtherLabelShanBi = AS.SeOtherLabelShanBi ;
		SeOtherLabelFangYuMo= AS.SeOtherLabelFangYuMo ;
		SeOtherLabelPianXie= AS.SeOtherLabelPianXie ;
		SeOtherLabelNaiLi = AS.SeOtherLabelNaiLi ;
		SeOtherLabelLiLiang = AS.SeOtherLabelLiLiang ;
		SeOtherLabelMinJie = AS.SeOtherLabelMinJie ;
		SeOtherLabelZhiLi = AS.SeOtherLabelZhiLi ;
		SeOtherLabelZhuanZhu = AS.SeOtherLabelZhuanZhu ;
		PlayerInfoOtherLabelHp = AS.PlayerInfoOtherLabelHp ;
		PlayerInfoOtherLabelMana = AS.PlayerInfoOtherLabelMana ;
		PlayerInfoOtherLabelAtk= AS.PlayerInfoOtherLabelAtk ;
		PlayerInfoOtherLabelAtkM = AS.PlayerInfoOtherLabelAtkM ;
		PlayerInfoOtherLabelBaoJi= AS.PlayerInfoOtherLabelBaoJi ;
		PlayerInfoOtherLabelJingZhun = AS.PlayerInfoOtherLabelJingZhun ;
		PlayerInfoOtherLabelHuJia = AS.PlayerInfoOtherLabelHuJia ;
		PlayerInfoOtherLabelFangYu= AS.PlayerInfoOtherLabelFangYu ;
		PlayerInfoOtherLabelShanBi = AS.PlayerInfoOtherLabelShanBi ;
		PlayerInfoOtherLabelFangYuMo= AS.PlayerInfoOtherLabelFangYuMo ;
		PlayerInfoOtherLabelPianXie = AS.PlayerInfoOtherLabelPianXie ;
		PlayerInfoOtherLabelNaiLi = AS.PlayerInfoOtherLabelNaiLi ;
		PlayerInfoOtherLabelLiLiang = AS.PlayerInfoOtherLabelLiLiang ;
		PlayerInfoOtherLabelMinJie = AS.PlayerInfoOtherLabelMinJie ;
		PlayerInfoOtherLabelZhiLi = AS.PlayerInfoOtherLabelZhiLi ;
		PlayerInfoOtherLabelZhuanZhu = AS.PlayerInfoOtherLabelZhuanZhu ;
		PlayerInfoOtherLabelCombat = AS.PlayerInfoOtherLabelCombat ;
		PlayerInfoOtherLabelPianXie = AS.PlayerInfoOtherLabelPianXie;
		OtherLabelHp = AS.OtherLabelHp ;
		OtherLabelMana = AS.OtherLabelMana ;
		OtherLabelAtk = AS.OtherLabelAtk ;
		OtherLabelAtkM = AS.OtherLabelAtkM ;
		OtherLabelBaoJi= AS.OtherLabelBaoJi ;
		OtherLabelJingZhun = AS.OtherLabelJingZhun ;
		OtherLabelHuJia = AS.OtherLabelHuJia ;
		OtherLabelFangYu= AS.OtherLabelFangYu ;
		OtherLabelShanBi= AS.OtherLabelShanBi ;
		OtherLabelFangYuMo= AS.OtherLabelFangYuMo ;
		OtherLabelPianXie= AS.OtherLabelPianXie ;
		OtherLabelNaiLi= AS.OtherLabelNaiLi ;
		OtherLabelLiLiang= AS.OtherLabelLiLiang ;
		OtherLabelMinJie = AS.OtherLabelMinJie ;
		OtherLabelZhiLi = AS.OtherLabelZhiLi ;
		OtherLabelZhuanZhu = AS.OtherLabelZhuanZhu ;
		AllManage.UICLStatic.bagAllParent  = AS.bagAllParent ;
		AllManage.UICLStatic.GroundLiaoTian  = AS.GroundLiaoTian ;
		AllManage.UICLStatic.LiaoTian = AS.LiaoTian ;
		AllManage.UICLStatic.TransBag = AS.TransBag ;
		AllManage.UICLStatic.TransInvInfo = AS.TransInvInfo ;
		AllManage.UICLStatic.TransXunLian = AS.TransXunLian ;
		AllManage.UICLStatic.xunPanel = AS.xunPanel ;
		AllManage.UICLStatic.ObjStoreButton1  = AS.ObjStoreButton1 ;
		AllManage.UICLStatic.ObjStoreButton2 = AS.ObjStoreButton2 ;
		AllManage.UICLStatic.ObjStoreButton3  = AS.ObjStoreButton3 ;
		AllManage.UICLStatic.ObjStoreButton4  = AS.ObjStoreButton4 ;
		AllManage.UICLStatic.transEquep  = AS.transEquep ;
		AllManage.UICLStatic.TweenDaoHang  = AS.TweenDaoHang ;
		AllManage.UICLStatic.ButtonDaoHang  = AS.ButtonDaoHang ;
		AllManage.UICLStatic.ParentYuan  = AS.ParentYuan ;
		AllManage.UICLStatic.AllParent = AS.AllParent ;
		AllManage.UICLStatic.TransformButtonPart  = AS.TransformButtonPart ;
		AllManage.UICLStatic.SpritePlayerInfo = AS.SpritePlayerInfo ;
		AllManage.UICLStatic.TransPlayerDetails  = AS.TransPlayerDetails ;
		AllManage.UICLStatic.SpritePlayerDetailsButton  = AS.SpritePlayerDetailsButton ;
		AllManage.UICLStatic.infoPlayerObj  = AS.infoPlayerObj ;
		AllManage.UICLStatic.TransPlayerRide  = AS.TransPlayerRide ;
		AllManage.UICLStatic.SpritePlayerRideButton = AS.SpritePlayerRideButton ;
		AllManage.UICLStatic.OtherShowDuelParent1 = AS.OtherShowDuelParent1 ;
		AllManage.UICLStatic.OtherShowDuelParent2  = AS.OtherShowDuelParent2 ;
		AllManage.UICLStatic.OtherShowDuelParent3  = AS.OtherShowDuelParent3 ;
		AllManage.UICLStatic.OtherObjPVP = AS.OtherObjPVP ;
//		AllManage.UIALLPCStatic.parentNowTask = AS.parentNowTask ;
		AllManage.UIALLPCStatic.parentActivity  = AS.parentActivity ;
		AllManage.UIALLPCStatic.parentPVP  = AS.parentPVP ;
		AllManage.UIALLPCStatic.parentXunLian  = AS.parentXunLian ;
		AllManage.UIALLPCStatic.parentAchievementPnael  = AS.parentAchievementPnael ;
		AllManage.UIALLPCStatic.parentOfflinePlayer  = AS.parentOfflinePlayer ;
		AllManage.UIALLPCStatic.parentSkill = AS.parentSkill ;
		RideGrid = AS.RideGrid ;
		ParentRideDown = AS.ParentRideDown ;
		NowRideItemIcon = AS.NowRideItemIcon ;
		NowRideItemName = AS.NowRideItemName ;
		NowRideItemInfo = AS.NowRideItemInfo ;
		NowRideItemBianKuang = AS.NowRideItemBianKuang ;
		SpriteRideButton1 = AS.SpriteRideButton1 ;
		NextStart();
	}

function NextStart(){
//	print(AllManage.UICLStatic.MainUIOn);
		yield;
//	print(AllManage.UICLStatic.MainUIOn);
//	AllManage.UICLStatic.ProfessionLabel.text = yt.Rows[0]["PlayerName"].YuanColumnText;
		AllManage.UICLStatic.PosStores[0] = AllManage.UICLStatic.TransStores[0].position.x;
//		AllManage.UICLStatic.PosStores[1] = AllManage.UICLStatic.TransStores[1].position.x;
		AllManage.UICLStatic.PosStores[1] = AllManage.UICLStatic.TransStores[1].position.x;
		AllManage.UICLStatic.PosStores[2] = AllManage.UICLStatic.TransStores[2].position.x;
		AllManage.UICLStatic.PosStores[3] = AllManage.UICLStatic.TransStores[3].position.x;
	ClearBagItem();
	BagID = 1;
	SetSelectBagItem(Inventory1);
	SetEquipItem(EquipItemStr);
	GetPersonEquipment();
	ShowBagButton(BagID);
	SetDaoJuItem();
	SetRideItem();
	GetTalkShow();
	ReadVIPLevel();
	StartGuiWei();
	SetPlayerInventoryNum(PlayerInventoryNum);
	if(yt.Rows[0]["SelectMounts"].YuanColumnText.Length > 2){
		RideButtonOn(parseInt(yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(2,2)).ToString());	
	}
}

	function ReadVIPLevel(){
		LabelVIP.text = "" + GetBDInfoInt("Serving" , 0);
	}

	var yanseLevel : String[];
	function GetNPCRankLevelAsName(name : String) : int{
	//	//print(id + " ===== id ");
		for(var rows : yuan.YuanMemoryDB.YuanRow in PlayerTitle.Rows){
			if(rows["PlayerTitle"].YuanColumnText == name){
				return GetRankLevelBDInfoInt(rows["PlayerRankLevel"].YuanColumnText , 0);
			}
		}
		return 0;
	}
	function GetRankLevelBDInfoInt(bd : String , it : int) : int{  
		var iii : int = 0;
		try{ 
			iii = parseInt(bd);
			return  iii;
		}catch(e){
			return it;	
		}
	}

	var soulEqueps : InventoryItem[];
	private var JStr : String = "#";
	private var QStr : String = "@";
	function SetEquepSoulItem(invID : String){
		var i : int = 0;
		var useInv : InventoryItem;
		var useInvID : String[];
		useInvID = invID.Split(QStr.ToCharArray());
	//	for(i=0; i<useInvID.length; i++){	 
	//		if(useInvID[i] != ""){ 
	//			useInv = invMaker.GetItemInfo(useInvID[i] , useInv);
	//			soulEqueps[i] = useInv;
	//			if(i == 6){
	//				peson.SendMessage("CallObjectSoul",useInv.itemProAbt);
	//			}
	//		}
	//	}
		
		if(useInvID[0] != "" && useInvID.length > 1){
			useInv = AllResources.InvmakerStatic.GetItemInfo(invID , useInv);
			soulEqueps[6] = useInv;
			peson.SendMessage("CallObjectSoul",useInv.itemProAbt);
			
			var useInvID1 : String[]; 
			useInvID1 = useInvID[1].Split(JStr.ToCharArray());
			for(i=0; i<useInvID1.length; i++){	 
				if(useInvID1[i] != "" && parseInt(useInvID1[i]) > 0){ 
					useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID1[i] , useInv);
					soulEqueps[i] = useInv;
				}
			}
		}

	}

	function BuildEquepPes(){
		var i : int = 0;
		for(i=0; i<soulEqueps.length; i++){
			if(soulEqueps[i] != null){
				if(i < 6){
					RealPes(soulEqueps[i]);
				}else
				if(i == 6){
					SetRealSoul(soulEqueps[i]);
				}
			}
		}
	}

	function RealPes(inv : InventoryItem){
		switch(inv.itemProAbt){
			case 1:
				EquipStatus[0] += inv.itemLevel * inv.itemQuality;
				break;
			case 2:
				EquipStatus[2] += inv.itemLevel * inv.itemQuality;
				break;
			case 3:
				EquipStatus[5] += inv.itemLevel * inv.itemQuality;
				break;
			case 4:
				EquipStatus[3] += inv.itemLevel * inv.itemQuality;
				break;
			case 5:
				EquipStatus[4] += inv.itemLevel * inv.itemQuality;
				break;
			case 6:
				EquipStatus[6] += inv.itemLevel * inv.itemQuality;
				break;
			case 7:
				EquipStatus[7] += inv.itemLevel * inv.itemQuality;
				break;
			case 8:
				EquipStatus[8] += inv.itemLevel * inv.itemQuality;
				break;
		}
	}

		var rs : RealSoul = new RealSoul();
		var peson : Transform;
		var soulPetInv : InventoryItem;
	function SetRealSoul(inv : InventoryItem){
		rs.name = inv.itemName;
		rs.level = inv.itemLevel;
		rs.quality = inv.itemQuality;
		rs.attr = inv.itemProAbt;
		rs.attrLevel = inv.SoulExp;
		rs.skillLevel = inv.SkillLevel;
		soulPetInv = inv;
		var useStrs : String[];
		if(soulPetInv.itemID.IndexOf("@")){
			useStrs = soulPetInv.itemID.Split("@"[0]);
			if(useStrs.length > 1 && useStrs[1] != null && useStrs[1] != ""){
				EquepPetStr = useStrs[1];
			}
		}
	}

	var UpdatePetbool : boolean = false;
	var EquepPetStr : String;
	function UpdateSoulPet(){
		if(AllManage.SoulCLStatic != null){
			return;
		}
		UpdatePetbool = false;
		if(soulPetInv != null && rs != null){
			if(rs.attrLevel > soulPetInv.SoulExp || rs.level > soulPetInv.itemLevel){
				var str1 : String;
				var str2 : String;
				soulPetInv.itemLevel = rs.level;
				str1 = soulPetInv.itemLevel.ToString();
				soulPetInv.SoulExp = rs.attrLevel;
				str2 = soulPetInv.SoulExp.ToString();
				if(str1.Length < 2){
					str1 = "0" + str1;
				}else
				if(str1.Length > 2){
					str1 = "99";
				}
				if(str2.Length < 2){
					str2 = "0" + str2;
				}else
				if(str2.Length > 2){
					str2 = "99";
				}
				soulPetInv.itemID = soulPetInv.itemID.Substring(0,2) + str1 + soulPetInv.itemID.Substring(4,2) + str2 + soulPetInv.itemID.Substring(8,2);
				if(EquepPetStr != null && EquepPetStr != ""){
					soulPetInv.itemID += ("@" + EquepPetStr);
				}
				UpdatePetbool = true;
			}
		}
		if(UpdatePetbool){
			InventoryControl.yt.Rows[0]["EquipItemSoul"].YuanColumnText = soulPetInv.itemID;
//			UpdateSouPetItem();
		}
	}
	
function UpdateSouPetItem(){
//	var EquipItemSoul = ""; 
//	var i : int = 0;
//	if(EquepSIT[6].inv != null){
//		EquipItemSoul += (EquepSIT[6].inv.itemID.Substring(0,10)) + "@";
//	}
//	for(i=0; i<EquepSIT.length-1; i++){
//		if(EquepSIT[i].inv != null && EquepSIT[6].inv != null){
//			EquipItemSoul += (EquepSIT[i].inv.itemID) + "#";
//		}else{
//			EquipItemSoul += "#";		
//		}
//	}
//	SetEquepAnoTherSoulItem(EquipItemSoul);
//	if(EquepSIT[6].inv != null){ 
//		EquepSIT[6].inv.itemID = EquipItemSoul;
//	}
//	InventoryControl.yt.Rows[0]["EquipItemSoul"].YuanColumnText = EquipItemSoul;
//	invcl.GetPersonEquipment();
} 

	function GetTalkShow(){
		SpeakComplete = yt.Rows[0]["SpeakComplete"].YuanColumnText;
		useSpeakComplete = SpeakComplete.Split(Fstr.ToCharArray());
	}

	function AddTalkShow(str : String){
		SpeakComplete = "";
		var useStr : String[];
		var i : int = 0;
		useStr = useSpeakComplete;
		useSpeakComplete = new Array(useStr.length + 1);
		for(i=0; i<useStr.length; i++){
			useSpeakComplete[i] = useStr[i];
		}
		useSpeakComplete[useStr.length - 1] = str;
		for(i=0; i<useSpeakComplete.length; i++){
			if(useSpeakComplete[i] != ""){
				SpeakComplete += useSpeakComplete[i] + ";";	
			}
		}
		yt.Rows[0]["SpeakComplete"].YuanColumnText = SpeakComplete;
	}

	function CanShowTalkAsMapID(Mid : String) : boolean {
		GetTalkShow();
		var i : int = 0;
		for(i=0; i<useSpeakComplete.length; i++){
			if(useSpeakComplete[i] == Mid){
				return true;
			}
		}
		return false;
	}

	var SpriteShan : UISprite;
	function JianCeShan(){
	//	AllManage.tsStatic.RefreshBaffleOn();
		//InRoom.GetInRoomInstantiate().GetYuanTable("select * from PlayerInfo where PlayerID = "  + yt.Rows[0]["PlayerID"].YuanColumnText ,"DarkSword2",yt);
	//	InRoom.GetInRoomInstantiate ().GetTableForID (BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,yuan.YuanPhoton.TableType.PlayerInfo,yt);
	//	while(yt.IsUpdate){
	//		yield WaitForSeconds(0.5);
	//	}
	//	AllManage.tsStatic.RefreshBaffleOff();
		if(yt.Rows[0]["CanDailyBenefits"].YuanColumnText == "1"){
			SpriteShan.enabled = true;
		}else{
			SpriteShan.enabled = false;	
		}
	}

	var StrPlayerProfession : String[];

	function GetBDInfoInt(bd : String , it : int) : int{  
		var iii : int = 0;
		try{ 
			iii = parseInt(yt.Rows[0][bd].YuanColumnText);
			return  iii;
		}catch(e){
			return it;	
		}
	}

	function LookTimesAs(oldTime : String , nowTime : String , timeType : String , timeValue : int) : boolean{ 
		var timeSpawn : System.TimeSpan; 
		try{
			timeSpawn = System.DateTime.Parse(nowTime) - System.DateTime.Parse(oldTime);
			switch(timeType){
				case "sec" : 
					if(timeSpawn.TotalSeconds < timeValue){
						return true;
					}
					break;
				case "min" : 
					if(timeSpawn.TotalMinutes < timeValue){
						return true;
					}
					break;
				case "hou" : 
					if(timeSpawn.TotalHours < timeValue){
						return true;
					}
					break;
				case "day" : 
					if(timeSpawn.TotalDays < timeValue){
						return true;
					}
					break;
			}
			return false;
		}catch(e){
			return false;
		}
	}

//	function AddExp(i : int){
//		if(PlayerStatus.MainCharacter != null){
//			if(ps == null && PlayerStatus.MainCharacter){
//				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
//			}
//			ps.AddExperience(i);	
//		}
//	}

	static var shangxian : boolean = false;
	static var timeShangXian : String = ""; 
	static var timePowerShangXian : String = ""; 
	private var ShangXianValue : int = 0;
	var hongU : HangUp;
	function RewardShangXian(){
	//	AllManage.tsStatic.RefreshBaffleOn();
	//	//InRoom.GetInRoomInstantiate().GetYuanTable("select * from PlayerInfo where PlayerID = "  + yt.Rows[0]["PlayerID"].YuanColumnText ,"DarkSword2",yt);
	//	InRoom.GetInRoomInstantiate ().GetTableForID (BtnGameManager.yt.Rows[0]["PlayerID"].YuanColumnText,yuan.YuanPhoton.TableType.PlayerInfo,yt);
	//	while(yt.IsUpdate){
	//		yield WaitForSeconds(0.5);
	//	}
	//	AllManage.tsStatic.RefreshBaffleOff();
		if(shangxian){ 
			try{
				if(InRoom.GetInRoomInstantiate().serverTime != null){
					timeShangXian = InRoom.GetInRoomInstantiate().serverTime.ToString();			
				}else{
					timeShangXian = System.DateTime.Now.ToString();
				}
				timePowerShangXian = timeShangXian;
				shangxian = false;
				var timeSpawn : System.TimeSpan;
				
				timeSpawn = System.DateTime.Parse(InventoryControl.yt.Rows[0]["LogonTime"].YuanColumnText) - System.DateTime.Parse(InventoryControl.yt.Rows[0]["HangUpTime"].YuanColumnText); 
//				Debug.Log("上线加体力----------------------"+InventoryControl.yt.Rows[0]["LogonTime"].YuanColumnText+","+InventoryControl.yt.Rows[0]["HangUpTime"].YuanColumnText+","+timeSpawn.TotalSeconds);
				ShangXianValue = timeSpawn.TotalMinutes / 30;
				hongU.GetHangUp(Mathf.Clamp(timeSpawn.TotalSeconds , 60 , 86400));
	//			ps.AddPower(-1 * ShangXianValue * 10);
				hongU.GetHangUpPower(Mathf.Clamp(timeSpawn.TotalSeconds , 60 , 86400));
			}catch(e){
			
			}
			yield WaitForSeconds(3);
	//		if(parseInt(ps.Level) < 10){
//				if(yt.Rows[0]["CanDailyBenefits"].YuanColumnText == "1"){
//					AllManage.UIALLPCStatic.show9();
//				}
	//		}
	//		//print("shang xian jia ti li == " +  timeSpawn.Minutes + " == " +  timeSpawn.Seconds + " == " + timeSpawn.TotalSeconds);
	//		ps.Power += timeSpawn.TotalMinutes / 30 * 10;
	//		if(parseInt(ps.Power) > xingdongzhi){
	//			ps.Power = xingdongzhi.ToString();
	//		}
	//		yt.Rows[0]["Power"].YuanColumnText = ps.Power;
		}
	}

	var ButtonsBag : UISprite[];
	function SetPlayerInventoryNum(bag : int){
		try{
			var i=0;
		//	//print(bag + " == bag");
			for(i=0; i<ButtonsBag.length; i++){
				if(i < bag){
					ButtonsBag[i].spriteName = "UIH_Backpack_A";
				}else{
					ButtonsBag[i].spriteName = "UIH_Backpack_O";
				}
			}
		}catch(e){
		
		}
	}

	var FuFishLabel : UILabel;
	var FuMiningLabel : UILabel;
	var FuCookingLabel : UILabel;
	var FuMakeLabel : UILabel;
	function GetOneFish(id : String , level : int){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		var uExp : int;
		uExp = parseInt(ps.Level) * 5;
		if(expFishing < 300){
			if(expFishing < uExp && level > expFishing / 5 / 6){
				expFishing += 1;
				yt.Rows[0]["FishingExp"].YuanColumnText = expFishing.ToString();
				yield WaitForSeconds(2);
				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages145")+expFishing);
			}
		}
		ShowFuYeLevel();
	}

	function ShowFuYeLevel(){
		if(AllManage.SkillCLStatic){
			AllManage.SkillCLStatic.ShowFuYeLevel();
		}
	}

	function GetOneStone(id : String , level : int){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		var uExp : int;
		uExp = parseInt(ps.Level )* 5;
		if(expMining < uExp && level > expMining / 5 / 6){
			expMining += 1;
			yt.Rows[0]["MiningExp"].YuanColumnText = expMining.ToString(); 
			yield WaitForSeconds(2);
			AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("tips019")  +expMining);
		}
		ShowFuYeLevel();
	}

	function Costjinbi(num : int){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.CostGold , num , 0 , "" , gameObject , "");
//		AllManage.AllMge.UseMoney(num , 0 , UseMoneyType.CostGold , gameObject , "");
	//	ps.UseMoney(num , 0);
	}

	private var Dstr : String = ",";
	function LookLiBaoItemAsID(id : String){
		var lastStr : String;
		var packStr : String;
		var packBlood : String;
		var packGold : String;
		var packSoul : String;
		var usePackInv : InventoryItem;
		var i : int = 0;
	//	//print(id);
		for(var rows : yuan.YuanMemoryDB.YuanRow in GameItem.Rows){
			if(rows["ItemID"].YuanColumnText.Substring(0,4) == id.Substring(0,4)){
				lastStr = rows["ItemValue"].YuanColumnText;
			}
		}
	//	//print(lastStr);
		for(var rows : yuan.YuanMemoryDB.YuanRow in TablePacks.Rows){
			if(rows["id"].YuanColumnText == lastStr){
				packStr = rows["Info"].YuanColumnText;
				packBlood = rows["BloodStrone"].YuanColumnText;
				packGold = rows["Cash"].YuanColumnText;
				packSoul =  rows["Soul"].YuanColumnText;
			}
		}
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		try{
			AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.OpenSpreeItemAsID , 0 , 0 , id , gameObject , "");
			ps.UseSoul(parseInt(packSoul) * (-1));
//			AllManage.AllMge.UseMoney(parseInt(packGold) * (-1) , parseInt(packBlood) * (-1) , UseMoneyType.OpenSpreeItemAsID , gameObject , "");
	//		ps.UseMoney(parseInt(packGold) * (-1) , parseInt(packBlood) * (-1));		
		}catch(e){
		}
		var USdaojuIDs : String[];
		var TowdaojuIDs : String[];
		var USjds : String[];
		var stNum : String;
		USdaojuIDs = packStr.Split(Fstr.ToCharArray());
//		print(packStr + " ============= packStr");
		AllManage.tsStatic.RefreshBaffleOn();
		for(i=0; i<USdaojuIDs.length; i++){
//			print(USdaojuIDs[i] + " == USdaojuIDs");
			if(USdaojuIDs[i] != null && USdaojuIDs[i].IndexOf("G") > 0){
				TowdaojuIDs = USdaojuIDs[i].Split("G"[0]);
				if(TowdaojuIDs.length > 1 && TowdaojuIDs[1] != ""){
//					print(TowdaojuIDs[0] + " ==== " + TowdaojuIDs[1]);
					if(Random.Range(0,100) < parseInt(TowdaojuIDs[1])){
						AddItemSwitch(TowdaojuIDs[0]);
					}
				}
			}else{
				if(USdaojuIDs[i] != ""){
					AddItemSwitch(USdaojuIDs[i]);
				}	
			}
	//		var st : String;
	//		USjds = USdaojuIDs[i].Split(Dstr.ToCharArray());
	//		if(USjds[0] != "" && GameItem.SelectRowEqual("id" ,USjds[0]) != null){
	//			st = GameItem.SelectRowEqual("id" ,USjds[0])["ItemID"].YuanColumnText;
	//			if(USjds[1].Length < 2){
	//				USjds[1] = "0" + USjds[1];
	//			}
	//			st += "," + USjds[1];
	//			if(st.Substring(0,2) == "88"){
	//				AddNewDaojuItemAsID(st);			
	//			}else{
	//				usePackInv = AllResources.InvmakerStatic.GetItemInfo(st , usePackInv);
	//				AddBagItem(usePackInv);
	//			}
	//		}
	//		yield WaitForSeconds(0.4);
		}
		useBagItem(id , 1);
	//	AllManage.tsStatic.RefreshBaffleOff();
	}

	function AddItemSwitch(itemID : String){
		if(itemID.Substring(0,2) == "88"){
			AddNewDaojuItemAsID(itemID);
		}else 
		if(itemID.Substring(0,2 )== "72"){
			AddNewRideItemAsID(itemID);
		}else 
		if(itemID.Substring(0,2) == "70"){
			AddBagDigestItemAsID(itemID);
		}else 
		if(itemID.Substring(0,2) == "71"){
			AddBagSoulItemAsID(itemID);
		}else{
			AddBagItemAsID(itemID);
		}
	}

	function AddNewDaojuItemAsID(id : String){
		AllManage.UICLStatic.CategoryTipsAsID(id);	
		var i : int = 0;
		var num : int = 0;
		var can : boolean = false;
		UseDaojuIDs = DaoJuItemStr.Split(Fstr.ToCharArray());
	//	//print("DaoJuItemStr == " + DaoJuItemStr);
		for(i=0; i<UseDaojuIDs.length; i++){	
			if(UseDaojuIDs[i].Length > 4){
				if(UseDaojuIDs[i].Substring(0,4) == id.Substring(0,4)){
	//				//print(UseDaojuIDs[i] + " == " + id);
					num = parseInt(UseDaojuIDs[i].Substring(5,2)) + parseInt(id.Substring(5,2));
					if(num > 9){
						UseDaojuIDs[i] = UseDaojuIDs[i].Substring(0,4) + "," + num.ToString();
					}else
					if(num > 0){
						UseDaojuIDs[i] = UseDaojuIDs[i].Substring(0,4)  + "," + "0" + num.ToString();
					}else{
						UseDaojuIDs[i] = "";
					}
					can = true;
				}
			}
		}
		if(!can){
			var useD : String[];
			useD = UseDaojuIDs;	
			UseDaojuIDs = new Array(useD.length + 1);
			for(i=0; i<useD.length; i++){
				UseDaojuIDs[i] = useD[i];
			}
			UseDaojuIDs[UseDaojuIDs.length - 1] = id+";";
		}
		DaoJuItemStr = "";
		for(i=0; i<UseDaojuIDs.length; i++){
			if(UseDaojuIDs[i].Length > 2){
				DaoJuItemStr += UseDaojuIDs[i] + ";";
			}
		}
		yt.Rows[0]["Item"].YuanColumnText = DaoJuItemStr;
		ClearDaoJuObj();
	//	SetRealDaoJuItemAsID(DaoJuItemStr);
		SetDaoJuItem();
	}

	var UseDaojuIDs : String[];
	function SetDaoJuItem(){
		var i : int = 0;
		UseDaojuIDs = DaoJuItemStr.Split(Fstr.ToCharArray());
	//	//print("DaoJuItemStr == " + DaoJuItemStr);
		for(i=0; i<UseDaojuIDs.length; i++){	 
			if(UseDaojuIDs[i] != ""){
				SetRealDaoJuItemAsID(UseDaojuIDs[i]);
			}
		}
	}

	var DaojuGrid : UIGrid;
	var DaojuPre : GameObject;
	var ObjArray : GameObject[];
	function SetRealDaoJuItemAsID(id : String){
		if(! DaojuGrid)
			return ;
		for(var rows : yuan.YuanMemoryDB.YuanRow in GameItem.Rows){
			if(id.Length > 4){
				if(rows["ItemID"].YuanColumnText == id.Substring(0,4)){
					var DaoJuPre : ItemDaoJu;
					var Obj : GameObject = Instantiate(DaojuPre); 
					Obj.transform.parent = DaojuGrid.transform;
					Obj.transform.localPosition.z = 0;
					DaoJuPre = Obj.GetComponent(ItemDaoJu);
					DaoJuPre.AddDaoJuObj(rows["ItemID"].YuanColumnText , rows["Name"].YuanColumnText , rows["ItemInfo"].YuanColumnText , id.Substring(5,2) , this);
					AddNewDaoJuInArray(Obj);
				}
			}
		}
		yield;
		DaojuGrid.repositionNow = true;
	}

	function ClearDaoJuObj(){
		for(var i=0; i<ObjArray.length; i++){
			Destroy(ObjArray[i]);
		}
		ObjArray = new Array();
	}

	function AddNewDaoJuInArray(obj : GameObject){
		var useArray : GameObject[];
		useArray = ObjArray;
		ObjArray = new Array(ObjArray.length + 1);
		for(var i=0; i<ObjArray.length-1; i++){
			ObjArray[i] = useArray[i];
		}
		ObjArray[ObjArray.length-1] = obj;
		obj.transform.localScale = Vector3(0.9 , 0.9 , 0.9);
	}

	var EquepInfo : EquepmentItemInfo;
	function SelectDaojuAsID(myid : String, myname : String, myinfo : String , icon : String){
		ItemInfoOn();
		EquepInfo.ShouDaoJuItem(myid , myname , myinfo , icon);
	}

	//var ts : TiShi;
	var plyDead : PlayerDead;
	private var useItemAttr : String = "";
	function UseDaojuAsID(id : String){
	//	//print(id);
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		var Attr : String;
		var str : String;
		var uTime : int;
		for(var rows : yuan.YuanMemoryDB.YuanRow in GameItem.Rows){
			if(rows["ItemID"].YuanColumnText == id.Substring(0,4)){
				Attr = rows["ItemValue"].YuanColumnText;
			}
		}
		if(id.Substring(0,3) ==  "886" && !ps.dead){
			AllManage.tsStatic.Show("tips027");
		}else
		if(id.Substring(0,3) ==  "884" && !ps.dead){
			useItemAttr = id.Substring(0,4);	
			if((xingdongzhi - parseInt(ps.Power)) > 0){
				if(parseInt(ps.Power) + parseInt(Attr) > xingdongzhi){
					AllManage.qrStatic.ShowQueRen1(gameObject , "YesPowerSolution" , "" , AllManage.AllMge.Loc.Get("tips106") + (xingdongzhi - parseInt(ps.Power)) + AllManage.AllMge.Loc.Get("tips107") +  AllManage.AllMge.Loc.Get("tips105") +  AllManage.AllMge.Loc.Get("meg0208"));
				}else{
					PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().UseItem(id.Substring(0,4)));
	//				InRoom.GetInRoomInstantiate().Coststrength(yuan.YuanPhoton.CostPowerType.PowerSolution , parseInt(Attr) , 0 , "");
				}
			}else{
				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("tips108"));
			}
		}else{
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().UseItem(id.Substring(0,4)));
		}
		return;
		switch(id.Substring(0,3)){
			case "881":
				if(JianDaojusID(id)){
					TimeGOWCard = 900;
					yt.Rows[0]["GOWCard"].YuanColumnText = "900";
					yt.Rows[0]["GOWCardValue"].YuanColumnText = Attr;
					AllManage.tsStatic.Show("tips020");
					if(ps != null){
						ps.PlayerAction(id);
	//					AllManage.UIALLPCStatic.show0();
					
					}
				}
				break;
			case "882":
//				AllManage.tsStatic.RefreshBaffleOn();
				PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().UseItem(id));
//				;
//				if(JianDaojusID(id)){
//					ps.AddExperience(parseInt(Attr));
//					AllManage.tsStatic.Show("tips021");
//					if(ps != null){
//						ps.PlayerAction(id);
//						AllManage.UIALLPCStatic.show0();
//					}
//				}
				break;
			case "883":
				if(JianDaojusID(id)){	
					uTime =GetBDInfoInt("DoubleCard" , 0);
					yt.Rows[0]["DoubleCard"].YuanColumnText = (uTime + parseInt(Attr)*60).ToString();
					TimeDoubleCard = uTime + parseInt(Attr)*60;
					AllManage.tsStatic.Show("tips022");
					if(ps != null){
						ps.PlayerAction(id);
//						AllManage.UIALLPCStatic.show0();
					}
				}
				break;
			case "884":
				if(JianDaojusID(id)){
					useItemAttr = Attr;	
//					ps.AddPower(parseInt(Attr) * (-1));
	//				ps.Power += parseInt(Attr);
					if(parseInt(ps.Power) + parseInt(Attr) > xingdongzhi){
						AllManage.qrStatic.ShowQueRen1(gameObject , "YesPowerSolution" , "" , AllManage.AllMge.Loc.Get("tips106") + Attr + AllManage.AllMge.Loc.Get("tips107") +  AllManage.AllMge.Loc.Get("tips105") +  AllManage.AllMge.Loc.Get("meg0208"));
					}else{
						InRoom.GetInRoomInstantiate().Coststrength(yuan.YuanPhoton.CostPowerType.PowerSolution , parseInt(Attr) , 0 , "");
					}
					
	//				yt.Rows[0]["Power"].YuanColumnText = ps.Power.ToString();
//					AllManage.tsStatic.Show("tips023");
					
					if(ps != null){
						ps.PlayerAction(id);
//						AllManage.UIALLPCStatic.show0();
					}
				}
				break;
			case "885":
				if(JianDaojusID(id)){
					ps.NonPoint += parseInt(Attr);
	//				PlayerPrefs.SetInt("NonPointPlus" , PlayerPrefs.GetInt("NonPointPlus") + parseInt(Attr));
					AllManage.tsStatic.Show("tips024");
					if(ps != null){
						ps.PlayerAction(id);
	//					AllManage.UIALLPCStatic.show0();
					}
				}
				break;
			case "886":
			if(ps == null && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}
			if(ps.dead){
				if(JianDaojusID(id)){
					plyDead.doFuHuo();
					AllManage.tsStatic.Show("tips025");
				}else{
					AllManage.tsStatic.Show("tips026");
				}
			}else{ 
				AllManage.tsStatic.Show("tips027");
			}
				break;
			case "887":
				if(JianDaojusID(id)){
					AllManage.tsStatic.Show("tips028");
					if(ps != null){
						ps.PlayerAction(id);
//						AllManage.UIALLPCStatic.show0();
					}
				}
				break;
		}
	}

	function YesPowerSolution(){
		if(useItemAttr != ""){
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().UseItem(useItemAttr));
//	 		InRoom.GetInRoomInstantiate().Coststrength(yuan.YuanPhoton.CostPowerType.PowerSolution , parseInt(useItemAttr) , 0 , "");
		}
		useItemAttr = "";
	}

	//这个提示因为太长了，还原成之前的提示。
	function returnPowerSolution(objs : Object[]){
		AllManage.tsStatic.Show("tips023");
//		var pID : String = "";
//		var pValue : int = 0;
//		pID = objs[0];
//		pValue = objs[1];
//		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("tips106") + pValue + AllManage.AllMge.Loc.Get("tips107") + AllManage.AllMge.Loc.Get("tips105"));
	}

	function returnUseItemAsID(id : String){
		if(id.Length > 2){
			if(ps == null && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}
			DaoJuItemStr = yt.Rows[0]["Item"].YuanColumnText;
			switch(id.Substring(0,3)){
				case "881":
					AllManage.tsStatic.Show("tips020");
					TimeGOWCard = GetBDInfoInt("GOWCard" , 0);
					if(ps != null){
						ps.PlayerAction(id);
	//					AllManage.UIALLPCStatic.show0();
					}
					break;
				case "882":
					AllManage.tsStatic.Show("tips021");
					if(ps != null){
						ps.PlayerAction(id);
	//					AllManage.UIALLPCStatic.show0();
					}
					break;
				case "883":
					TimeDoubleCard = GetBDInfoInt("DoubleCard" , 0);
					AllManage.tsStatic.Show("tips022");
					if(ps != null){
						ps.PlayerAction(id);
		//				AllManage.UIALLPCStatic.show0();
					}
					break;
				case "884":
					AllManage.tsStatic.Show("tips023");
					if(ps != null){
						ps.Power = GetBDInfoInt("Power" , 0).ToString();
						ps.PlayerAction(id);
		//				AllManage.UIALLPCStatic.show0();
					}
					break;
				case "885":
						AllManage.tsStatic.Show("tips024");
						if(ps != null){
							ps.NonPoint = GetBDInfoInt("NonPoint" , 0);
							ps.PlayerAction(id);
		//					AllManage.UIALLPCStatic.show0();
						}
					break;
				case "886":
					if(ps.dead){
						plyDead.doFuHuo();
						AllManage.tsStatic.Show("tips025");
					}else{ 
						AllManage.tsStatic.Show("tips027");
					}
					break;
				case "887":
					AllManage.tsStatic.Show("tips028");
					if(ps != null){
						ps.PlayerAction(id);
	//					AllManage.UIALLPCStatic.show0();
					}
					break;
			}
		}
	}

	function NomDaojus(id :  String) : int{
		var num : int = 0;
		var i: int = 0;
		UseDaojuIDs = DaoJuItemStr.Split(Fstr.ToCharArray());
		for(i=0; i<UseDaojuIDs.length; i++){
			if(UseDaojuIDs[i].Length > 3){
				if(UseDaojuIDs[i].Substring(0,4) == id.Substring(0,4)){
					num = parseInt(UseDaojuIDs[i].Substring(5,2));
				}
			}
		}
		return num;
	}

	function JianDaojusID(id :  String) : boolean{
		var i: int = 0;
		var num : int;
		var bool : boolean = false;
		for(i=0; i<UseDaojuIDs.length; i++){
			if(UseDaojuIDs[i].Length > 3){
			if(UseDaojuIDs[i].Substring(0,4) == id.Substring(0,4)){
				num = parseInt(UseDaojuIDs[i].Substring(5,2));
				num -= 1;
				if(num > 9){
						UseDaojuIDs[i] = UseDaojuIDs[i].Substring(0,4) + "," + num.ToString();
					}else
					if(num > 0){
						UseDaojuIDs[i] = UseDaojuIDs[i].Substring(0,4)  + "," + "0" + num.ToString();
					}else{
						UseDaojuIDs[i] = "";
					}
					bool = true;
					break;
				}	
			}
		}
		DaoJuItemStr = "";
		for(i=0; i<UseDaojuIDs.length; i++){
			DaoJuItemStr += UseDaojuIDs[i] + ";";
		} 
	//	//print(DaoJuItemStr);
		yt.Rows[0]["Item"].YuanColumnText = DaoJuItemStr;
		ClearDaoJuObj(); 
		SetDaoJuItem();
	//	SetRealDaoJuItemAsID(DaoJuItemStr);
//		print(EquepInfo + " == EquepInfo");
		if(EquepInfo)
		EquepInfo.InfoClose();
		return bool;
	}

	function StartGuiWei(){
		QieHuanBagBaoguo();
		QieHuanEquepZhuangBei();
	//	isCangku = true;
	//	QieHuanCangku();
	}

	private var str : String;
	var LabelBlood : UILabel;
	var LabelGold : UILabel;
	var LabelBlood1 : UILabel;
	var LabelGold1 : UILabel;
	var LabelBlood2 : UILabel;
	var LabelGold2 : UILabel;

	var LabelTime : UILabel;
	private var st1 : String;
	private var st2 : String;
	private var st3 : String;
	function UpDateSt(){  
		st1 = InRoom.GetInRoomInstantiate().serverTime.Hour + ""; 
		if(st1.Length<2){
			 st1 = "0" + st1;
		}
		st2 = InRoom.GetInRoomInstantiate().serverTime.Minute + "";
		if(st2.Length<2){
			 st2 = "0" + st2;
		}
		st3 = InRoom.GetInRoomInstantiate().serverTime.Second + "";
		if(st3.Length<2){
			 st3 = "0" + st3;
		}
		LabelTime.text = st1 + ":" + st2 +":"+ st3;
	} 

	var intMaster : int;
	var Ltime : int = 0;
	private var timeSpawn : System.TimeSpan;
	var buttonShowSkill : Transform;
	private var usePrivateInv : InventoryItem;
	var Atime : int = 0;
	function Updateyt(){
	if (PhotonNetwork.connected){
			showPlayerInfo();
			mtw.LookNowItemTask();
			if(InRoom.GetInRoomInstantiate().ServerConnected){	
//				print(PhotonNetwork.room.playerCount + " == playerCount == " + PhotonNetwork.room.name + " == " + PhotonNetwork.offlineMode + " == " + PhotonNetwork.offlineMode_inRoom); 
				if(!yt.IsUpdate)
				{    
					if( yt.Count > 0){
//					yt.Rows[0]["Skill"].YuanColumnText = "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;30;30;30;";
//						print("Start ===== " + yt.Rows[0]["Inventory1"].YuanColumnText);
//						print(yt.Rows[0]["Inventory2"].YuanColumnText);
//						print(yt.Rows[0]["Inventory3"].YuanColumnText);
//						print(yt.Rows[0]["Inventory4"].YuanColumnText + " ===== End");
//						print(yt.Rows[0]["CompletTask"].YuanColumnText);
//						yt.Rows[0]["SolutionTime"].YuanColumnText = "";
//					yt.Rows[0]["Task"].YuanColumnText = "";
//					yt.Rows[0]["CompletTask"].YuanColumnText = "";
//					yt.Rows[0]["GetPlace"].YuanColumnText;
//			print(yt.Rows[0]["Item"].YuanColumnText);
						if(ps == null && PlayerStatus.MainCharacter){
							ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
						}
						if(ps != null ){
							ps.GetCombat();
							if(PlayerStatus.MainCharacter.position.y < -50){
								AllManage.UICLStatic.NowDivorced();
							}
						}
//					print(Equeipitems[0].WeaponType + " ====================");
				if(Time.time > Atime && InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "0" && UIControl.mapType == MapType.zhucheng){
					var bool : boolean = false;
					Atime = Time.time + 12;
					if(! AllManage.mtwStatic.MainPS.LookTaskIsDone("530")){
						if(! AllManage.UICLStatic.MainUIOn){
							if(Equeipitems[0] != null){
								if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
									bool = (Equeipitems[0].WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
								}else{
									bool = (Equeipitems[0].WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
								}
								if(bool && parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText) != 3){
									LookHaveCanEquepmentItem111();
//									AllManage.qrStatic.ShowZhuangBei(AllManage.UICLStatic.gameObject , "BagMoveOn" , "" ,"dunpai", AllManage.AllMge.Loc.Get("info891") + "" + AllManage.AllMge.Loc.Get( "info889" ),1 );
								}
							}
						}else{
							if(EquipIt[0]){
								if(EquipIt[0].inv != null){
									if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
										bool = (EquipIt[0].inv.WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
									}else{
										bool = (EquipIt[0].inv.WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
									}	
									if(bool && parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText) != 3){
										LookCanEquepmentItem111();
//										AllManage.qrStatic.ShowZhuangBei(AllManage.UICLStatic.gameObject , "BagMoveOn" , "" , "dunpai", AllManage.AllMge.Loc.Get("info891") + "" + AllManage.AllMge.Loc.Get( "info889" ),1 );
									}
								}
							}
						}
					}
				}												
																		
						if(Time.time > Ltime && parseInt(ps.Level) > 3 && IsAlreadyDoneTaskID("11") && Input.touchCount < 1 && UIControl.mapType == MapType.zhucheng){
							Ltime = Time.time + 5;
							if(! AllManage.UICLStatic.MainUIOn){
								LookHaveCanEquepmentItem();
							}else{
								LookCanEquepmentItem();
							}
						}
						if(Application.loadedLevelName != "Map200"){
							timeSpawn = InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(timeShangXian);
							if(timeSpawn.TotalSeconds > 600 && VipYaoPing < 10){

							    if(VipYaoPing >= AllManage.InvclStatic.maxXuePingNum)
							    {
							        VipYaoPing = AllManage.InvclStatic.maxXuePingNum;

							        if(AllManage.InvclStatic.vipLevel < 9)
							        {
							            // 提示：开启更高等级的VIP可以增加血瓶上限
							            AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("meg0176"));
							        }
							        else
							        {
							            // 提示：血瓶已满
							            AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("meg0177"));
							        }
							    }
							    else{
							        AllManage.UICLStatic.PluseYaoPing(1);
							        AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info570"));
							    }
								
								timeShangXian = InRoom.GetInRoomInstantiate().serverTime.ToString();
							}
							timeSpawn =  InRoom.GetInRoomInstantiate().serverTime - System.DateTime.Parse(timePowerShangXian);
							if(timeSpawn.TotalMinutes > 60){
								timePowerShangXian = InRoom.GetInRoomInstantiate().serverTime.ToString();
								AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("meg0132"));
	 	 						InRoom.GetInRoomInstantiate().Coststrength(yuan.YuanPhoton.CostPowerType.PowerOnline , 0 , 0 , "");
//								YAddPower(15);
							}
						}
						if(PanelStatic.StaticBtnGameManager.lblStoreGold != null){
							PanelStatic.StaticBtnGameManager.lblStoreGold.text = yt.Rows[0]["Money"].YuanColumnText;
							PanelStatic.StaticBtnGameManager.lblStoreBlood.text = yt.Rows[0]["Bloodstone"].YuanColumnText;
						}
						if(PanelStatic.StaticBtnGameManager.lblStoreGold1 != null){
							PanelStatic.StaticBtnGameManager.lblStoreGold1.text = yt.Rows[0]["Money"].YuanColumnText;
							PanelStatic.StaticBtnGameManager.lblStoreBlood1.text = yt.Rows[0]["Bloodstone"].YuanColumnText;
						}
						if(PanelStatic.StaticBtnGameManager.lblStoreGold2 != null){
							PanelStatic.StaticBtnGameManager.lblStoreGold2.text = yt.Rows[0]["Money"].YuanColumnText;
							PanelStatic.StaticBtnGameManager.lblStoreBlood2.text = yt.Rows[0]["Bloodstone"].YuanColumnText;
						}
						if(PanelStatic.StaticBtnGameManager.lblStoreGold3 != null){
							PanelStatic.StaticBtnGameManager.lblStoreGold3.text = yt.Rows[0]["Money"].YuanColumnText;
							PanelStatic.StaticBtnGameManager.lblStoreBlood3.text = yt.Rows[0]["Bloodstone"].YuanColumnText;
						}					
	 					Profiler.BeginSample("SetInitValue");
						SetInitValue();
						Profiler.EndSample();
					} 
					if(Application.loadedLevelName != "Map200"){
						SetInitValue();
						InRoom.GetInRoomInstantiate().UpdateYuanTable("DarkSword2",yt,SystemInfo.deviceUniqueIdentifier);				
					}
					if(yt.Count > 0){
						if(ps && ps.myPes.length > 0){
							if(TimeGOWCard > 0){
								TimeGOWCard -= 2;
								ps.GOWCardValue = parseInt(yt.Rows[0]["GOWCardValue"].YuanColumnText);
								if( ! isGOWCard){
									isGOWCard = true;
									ps.SetEquepInfo(ps.myPes);
								}
								yt.Rows[0]["GOWCard"].YuanColumnText = TimeGOWCard.ToString();
							}else{
								ps.GOWCardValue = 1;						
								if(	isGOWCard){
									isGOWCard = false;
									ps.SetEquepInfo(ps.myPes);
								}
							}
							if(TimeDoubleCard > 0){
								TimeDoubleCard -= 2; 
								ps.DoubleCardValue = 2;
								yt.Rows[0]["DoubleCard"].YuanColumnText = TimeDoubleCard.ToString();					
							}else{
								ps.DoubleCardValue = 1;
							}
						}				
					}
				}
			}
		}
	}
	var isGOWCard : boolean = false;
	 
	function SetInitValue(){
		if(yt.Rows[0]["Inventory2"].YuanColumnText.Length < 15){ 
			Inventory2 = ";;;;;;;;;;;;;;;";
			yt.Rows[0]["Inventory2"].YuanColumnText = ";;;;;;;;;;;;;;;";
		}
		if(yt.Rows[0]["Inventory3"].YuanColumnText.Length < 15){ 
			Inventory3 = ";;;;;;;;;;;;;;;";
			yt.Rows[0]["Inventory3"].YuanColumnText = ";;;;;;;;;;;;;;;";
		}
		if(yt.Rows[0]["Inventory4"].YuanColumnText.Length < 15){ 
			Inventory4 = ";;;;;;;;;;;;;;;";
			yt.Rows[0]["Inventory4"].YuanColumnText = ";;;;;;;;;;;;;;;";
		}
		if(yt.Rows[0]["Inventory1"].YuanColumnText.Length < 15){ 
			Inventory1 = ";;;;;;;;;;;;;;;";
			yt.Rows[0]["Inventory1"].YuanColumnText = ";;;;;;;;;;;;;;;";
		}
		if(yt.Rows[0]["EquipItem"].YuanColumnText.Length < 12){ 
			EquipItemStr = ";;;;;;;;;;;;";
			yt.Rows[0]["EquipItem"].YuanColumnText = ";;;;;;;;;;;;";
		}
	}
	 
	var EquipIt : BagItem[];
	var useEquipType : SlotType[];
	var Equeipitems : InventoryItem[];
	function SetEquipItem(equStr : String){
		 ClearEqupItem();
	//	 //print(equStr + " == equStr");
		var i : int = 0;
		var useInv : InventoryItem;
		var useInvID : String[];
		useInvID = equStr.Split(Fstr.ToCharArray());
	//	//print(useInvID.length);
		if(useInvID.length < 2){
			return;
		}
		sendACT_PlaySynAttr = false;
		for(i=0; i<EquipIt.length; i++){	 
			if(useInvID[i] != ""){ 
				useInv = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , useInv);
				if(AllManage.UICLStatic.MainUIOn){
					EquipIt[i].SetInv(useInv);			
				}else{
					Equeipitems[i] = useInv;
					var myInv : BagItemType = new BagItemType(); 
					myInv.inv = useInv;
					myInv.myType = useEquipType[i];			
					UpdatePES(useInv , equepmentIDs[i]);
					GoShowWeapon(myInv,equepmentIDs[i]);		
				}
	//			//print(useInvID[i] + " == useInvID[i] ");
			}
		}
		sendACT_PlaySynAttr = true;
	//	UpdateEquipItem();
	}

	function ClearEqupItem(){
	//	//print("qing kong zhuang bei");
		for(var i=0; i<EquipIt.length; i++){	 
			if(EquipIt[i])
				EquipIt[i].OtherYiChu();
		}
	}

	function JiaoYiOK(){
	//	ts.Show("交易完成。");
		InRoom.GetInRoomInstantiate().UpdateYuanTable("DarkSword2",yt,SystemInfo.deviceUniqueIdentifier);
		while(yt.IsUpdate){
			yield WaitForSeconds(0.5);
		}
		isUpdatePhoton = false;
		ReInitItem1();
	}

	private var intShua : int = 0;
	function ReInitItem(){  
//		print(yt.Rows[0]["Inventory1"].YuanColumnText + " == return bloodstone");
	//	AllManage.tsStatic.RefreshBaffleOn();
	//	while(yt.IsUpdate){
	//		yield WaitForSeconds(0.1);
	//	}
		var useInt : int = 0;
		intShua += 1; 
		useInt = intShua;
	//	InRoom.GetInRoomInstantiate().GetYuanTable("select * from PlayerInfo where PlayerID = "  +  yt.Rows[0]["PlayerID"].YuanColumnText,"DarkSword2",yt);
	//	while(yt.IsUpdate){
	//		yield WaitForSeconds(0.1);
	//	}
		AllManage.tsStatic.RefreshBaffleOff();
		////print(useInt + " == " + intShua);
		if(useInt == intShua){
			AllManage.ItMoveStatic.nowReMove();
			BagID = 1;  
			ShowBagButton(BagID);
			ClearBagItem();
			Inventory1 = yt.Rows[0]["Inventory1"].YuanColumnText;
			Inventory2 = yt.Rows[0]["Inventory2"].YuanColumnText;
			Inventory3 = yt.Rows[0]["Inventory3"].YuanColumnText;
			Inventory4 = yt.Rows[0]["Inventory4"].YuanColumnText;
			EquipItemStr = yt.Rows[0]["EquipItem"].YuanColumnText;
		 	DaoJuItemStr = yt.Rows[0]["Item"].YuanColumnText;
		 	RideItemStr = yt.Rows[0]["Mounts"].YuanColumnText;
			ClearDaoJuObj();
			SetDaoJuItem();
			SetSelectBagItem(Inventory1);
			ps.Money = GetBDInfoInt("Money" , 0).ToString();
			ps.Bloodstone = GetBDInfoInt("Bloodstone" , 0).ToString();
			ps.Power = GetBDInfoInt("Power" , 0).ToString();
			ClearRideObj();
			SetRideItem();
			if(AllManage.UIALLPCStatic.isSoul17()){
				yield;
				AllManage.SoulCLStatic.BagSoul = InventoryControl.yt.Rows[0]["BagSoul"].YuanColumnText;
				AllManage.SoulCLStatic.BagDigest = InventoryControl.yt.Rows[0]["BagDigest"].YuanColumnText;
				AllManage.SoulCLStatic.ClearAnyItem(AllManage.SoulCLStatic.BagSIT);
				AllManage.SoulCLStatic.ClearAnyItem(AllManage.SoulCLStatic.BagDIT);
				AllManage.SoulCLStatic.SetBagSoulItem(AllManage.SoulCLStatic.BagSoul);
				AllManage.SoulCLStatic.SetBagDigestItem(AllManage.SoulCLStatic.BagDigest);
			}
			if(AllManage.newUseItemCLStatic && AllManage.newUseItemCLStatic.gameObject.active){
				AllManage.newUseItemCLStatic.resetList();
			}
		}
	}

	function ReInitItemItemNo(){  
	//		//print(yt.Rows[0]["Inventory1"].YuanColumnText + " == yt.Ro");
	//	var useInt : int = 0;
	//	intShua += 1; 
	//	useInt = intShua;
			AllManage.ItMoveStatic.nowReMove();
		BagID = 1;  
		ShowBagButton(BagID);
	//		//print(yt.Rows[0]["Inventory1"].YuanColumnText + " == yt.Ro");
	//		//print(yt.Rows[0]["Inventory1"].YuanColumnText + " == yt.Ro");
	//	if(useInt == intShua){
			Inventory1 = yt.Rows[0]["Inventory1"].YuanColumnText;
			Inventory2 = yt.Rows[0]["Inventory2"].YuanColumnText;
			Inventory3 = yt.Rows[0]["Inventory3"].YuanColumnText;
			Inventory4 = yt.Rows[0]["Inventory4"].YuanColumnText;
			EquipItemStr = yt.Rows[0]["EquipItem"].YuanColumnText;
		 	DaoJuItemStr = yt.Rows[0]["Item"].YuanColumnText;
		 	RideItemStr = yt.Rows[0]["Mounts"].YuanColumnText;
//			print(yt.Rows[0]["Inventory1"].YuanColumnText + " =1= yt.Ro");
			ClearDaoJuObj();
//			print(yt.Rows[0]["Inventory1"].YuanColumnText + " =2= yt.Ro" + " ==" + Inventory1);
	//		//print(Inventory1 + " == Inventory1");
		//	SetRealDaoJuItemAsID(DaoJuItemStr);
			SetDaoJuItem();
//			print(yt.Rows[0]["Inventory1"].YuanColumnText + " =3= yt.Ro" + " ==" + Inventory1);
		
		//		//print("chong xin zao ru bao guo");
			ClearBagItem();
			SetSelectBagItem(Inventory1);
//			print(yt.Rows[0]["Inventory1"].YuanColumnText + " == yt.Ro" + " ==" + Inventory1);
			ps.Money = GetBDInfoInt("Money" , 0).ToString();
//			print(yt.Rows[0]["Inventory1"].YuanColumnText + " =4= yt.Ro" + " ==" + Inventory1);
			ps.Bloodstone = GetBDInfoInt("Bloodstone" , 0).ToString();
//			print(yt.Rows[0]["Inventory1"].YuanColumnText + " =5= yt.Ro" + " ==" + Inventory1);
			ClearRideObj();
//			print(yt.Rows[0]["Inventory1"].YuanColumnText + " =6= yt.Ro" + " ==" + Inventory1);
			SetRideItem();
//			print(yt.Rows[0]["Inventory1"].YuanColumnText + " =7= yt.Ro" + " ==" + Inventory1);
			if(AllManage.UIALLPCStatic.isSoul17()){
				yield;
				AllManage.SoulCLStatic.BagSoul = InventoryControl.yt.Rows[0]["BagSoul"].YuanColumnText;
				AllManage.SoulCLStatic.BagDigest = InventoryControl.yt.Rows[0]["BagDigest"].YuanColumnText;
				AllManage.SoulCLStatic.ClearAnyItem(AllManage.SoulCLStatic.BagSIT);
				AllManage.SoulCLStatic.ClearAnyItem(AllManage.SoulCLStatic.BagDIT);
				AllManage.SoulCLStatic.SetBagSoulItem(AllManage.SoulCLStatic.BagSoul);
				AllManage.SoulCLStatic.SetBagDigestItem(AllManage.SoulCLStatic.BagDigest);
			}
		
//			print(yt.Rows[0]["Inventory1"].YuanColumnText + " =8= yt.Ro" + " ==" + Inventory1);
		//	SetEquipItem(EquipItemStr);
	//	}
	}

	var useGoldShua : int = 0;
	function UpDateGold1(){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		var useInt : int = 0;
		useGoldShua += 1; 
		useInt = useGoldShua;
		AllManage.tsStatic.RefreshBaffleOn();
		while(yt.IsUpdate){
			yield WaitForSeconds(0.5);
		}
		AllManage.tsStatic.RefreshBaffleOff();
		if(useInt == useGoldShua){
			ps.Money = GetBDInfoInt("Money" , 0).ToString();
			ps.Bloodstone = GetBDInfoInt("Bloodstone" , 0).ToString();
			ps.VIPLevel = GetBDInfoInt("Serving" , 0);
			ps.ServingMoney = GetBDInfoInt("ServingMoney" , 0);
			LabelVIP.text = "" + GetBDInfoInt("Serving" , 0);
		}
	}
	function UpDateGold2(){
		ps.Money = GetBDInfoInt("Money" , 0).ToString();
		ps.Bloodstone = GetBDInfoInt("Bloodstone" , 0).ToString();
	}
	function ReInitItem1(){
			AllManage.ItMoveStatic.nowReMove();
		BagID = 1;  
		ShowBagButton(BagID);
		ClearBagItem();
		Inventory1 = yt.Rows[0]["Inventory1"].YuanColumnText;
		Inventory2 = yt.Rows[0]["Inventory2"].YuanColumnText;
		Inventory3 = yt.Rows[0]["Inventory3"].YuanColumnText;
		Inventory4 = yt.Rows[0]["Inventory4"].YuanColumnText;
		EquipItemStr = yt.Rows[0]["EquipItem"].YuanColumnText;
	//	//print("chong xin zao ru bao guo" + EquipItemStr);
		
		SetSelectBagItem(Inventory1);
		SetEquipItem(EquipItemStr);
		isUpdatePhoton = true;
	}

	function UpdatePhotonEquep(){
		ps.selfProperties["EquipItem"] = EquipItemStr; 
	//		//print("3");
		PhotonNetwork.SetPlayerCustomProperties(ps.selfProperties);	
	}

	function ReInitItemDontUpDatePhoton(){
			AllManage.ItMoveStatic.nowReMove();
		BagID = 1;  
		ShowBagButton(BagID);
		ClearBagItem();
		Inventory1 = yt.Rows[0]["Inventory1"].YuanColumnText;
		Inventory2 = yt.Rows[0]["Inventory2"].YuanColumnText;
		Inventory3 = yt.Rows[0]["Inventory3"].YuanColumnText;
		Inventory4 = yt.Rows[0]["Inventory4"].YuanColumnText;
		EquipItemStr = yt.Rows[0]["EquipItem"].YuanColumnText;
	//	//print("chong xin zao ru bao guo" + EquipItemStr);
		
		SetSelectBagItem(Inventory1);
		SetEquipItem(EquipItemStr);
	}

	var SpriteQieHuanBag : UISlicedSprite[];
	var TransQieHuanBag : Transform[];
	function QieHuanBagBaoguo(){
		TransQieHuanBag[0].localPosition.y = 0;
		TransQieHuanBag[1].localPosition.y = 1000;

		SpriteQieHuanBag[0].spriteName = "UIB_Tab_A";
		SpriteQieHuanBag[1].spriteName = "UIB_Tab_N";
	}
	var PanelDaoJu : UIPanel;
	function QieHuanBagDaoju(){
		PanelDaoJu.transform.localPosition.y = 0;
		PanelDaoJu.clipOffset.y = 0;
		TransQieHuanBag[0].localPosition.y = 1000;
		TransQieHuanBag[1].localPosition.y = 0;

		ResetGridDaoJuPanel();

		SpriteQieHuanBag[0].spriteName = "UIB_Tab_N";
		SpriteQieHuanBag[1].spriteName = "UIB_Tab_A";
	}

	function ResetGridDaoJuPanel()
	{
	    var gird : UIGrid = PanelDaoJu.transform.GetComponentInChildren(UIGrid);
	    yield WaitForSeconds (0.5f);

	    PanelDaoJu.transform.localPosition.y = 0;
	    PanelDaoJu.clipOffset = Vector2.zero;

	    gird.Reposition();

	    yield WaitForSeconds (1.5f);

	    if(TransQieHuanBag[0].localPosition.y == 1000)
	    {
	        PanelDaoJu.transform.localPosition.y = 0;
	        PanelDaoJu.clipOffset.y = 0;
	        TransQieHuanBag[0].localPosition.y = 1000;
	        TransQieHuanBag[1].localPosition.y = 0;

	        gird.Reposition();
	    } 
	}

//	var invCangku : InventoryCangku;
//	var TransQieHuanCangku : Transform; 
	var TransQieHuanParentEquep : Transform;
	var isCangku : boolean = false;
	var isShangdian : boolean = false;
	var buttonsBagCangKu : Transform;
	function QieHuanCangku(){
		if( Input.touchCount > 1 ){
			return;
		}
	//	if(UIControl.mapType == MapType.zhucheng){	
			isShangdian = false;
			if(!isCangku){
				AllManage.invCangKuStatic.cangkuBag();
				AllManage.invCangKuStatic.SelectBag1();
				isCangku = true;
				isShangDianTrue(false);
				if(AllManage.invCangKuStatic)
					AllManage.invCangKuStatic.transCangku.localPosition.y = 0; 
				buttonsBagCangKu.localPosition.y = 0; 
				TransQieHuanParentEquep.localPosition.y = 1000; 
			}else{
				isCangku = false;
				if(AllManage.invCangKuStatic)
					AllManage.invCangKuStatic.transCangku.localPosition.y = 1000; 
				TransQieHuanParentEquep.localPosition.y = 0; 
			}
			CangkuStoreButton();
			UICL.sdGuiWei();
	//	}else{
	//		InventoryArrange();
	////		AllManage.tsStatic.Show("tips029");
	//	}
	}

	function QieHuanShangdian(){
		if( Input.touchCount > 1 ){
			return;
		}
		isCangku = false;
		if(!isShangdian){
			AllManage.invCangKuStatic.SetShangDIanItem();
			isShangdian = true;
			isShangDianTrue(true);
			buttonsBagCangKu.localPosition.y = 3000; 
			if(AllManage.invCangKuStatic)
				AllManage.invCangKuStatic.transCangku.localPosition.y = 0; 
			TransQieHuanParentEquep.localPosition.y = 1000; 		
		}else{
			isShangdian = false;
			if(AllManage.invCangKuStatic)
				AllManage.invCangKuStatic.transCangku.localPosition.y = 1000; 
			TransQieHuanParentEquep.localPosition.y = 0; 
		}
		CangkuStoreButton();
		UICL.sdGuiWei();
	}

	function isShangDianTrue(bool : boolean){
		for(var i=0; i<AllManage.invCangKuStatic.BagIt.length ; i++){
			AllManage.invCangKuStatic.BagIt[i].isShangDian = bool;
		}
	}

	function QieHuanStore(tp : int){
		isCangku = false;
		if(!isShangdian){
			AllManage.invCangKuStatic.SetShangDIanItemAsType(tp);
			isShangdian = true;
			if(AllManage.invCangKuStatic)
				AllManage.invCangKuStatic.transCangku.localPosition.y = 0; 
			TransQieHuanParentEquep.localPosition.y = 1000; 		
		}else{
			isShangdian = false;
			if(AllManage.invCangKuStatic)
				AllManage.invCangKuStatic.transCangku.localPosition.y = 1000; 
			TransQieHuanParentEquep.localPosition.y = 0; 
		}
		CangkuStoreButton();
	}

	var SpriteCangku : UISprite;
	var SpriteShangdian : UISprite;
	var LabelCangku : UILabel;
	function CangkuStoreButton(){
		if(isCangku){
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages073");
				AllManage.AllMge.SetLabelLanguageAsID(LabelCangku);
	//		LabelCangku.text = "仓库";
			SpriteCangku.spriteName = "UIH_Minor_Button_A";
			SpriteShangdian.spriteName = "UIH_Minor_Button_N";
		}else
		if(isShangdian){
				AllManage.AllMge.Keys.Clear();
				AllManage.AllMge.Keys.Add("messages074");
				AllManage.AllMge.SetLabelLanguageAsID(LabelCangku);
	//		LabelCangku.text = "商店";
			SpriteCangku.spriteName = "UIH_Minor_Button_N";
			SpriteShangdian.spriteName = "UIH_Minor_Button_A";	
		}else{
			SpriteCangku.spriteName = "UIH_Minor_Button_N";
			SpriteShangdian.spriteName = "UIH_Minor_Button_N";		
		}
	}

	var SpriteQieHuanEquep : UISlicedSprite[];
	var TransQieHuanEquep : Transform[];
	var randCam : rendercamerapic;
	function QieHuanEquepZhuangBei(){
		TransQieHuanEquep[0].localPosition.y = 0;
		TransQieHuanEquep[1].localPosition.y = 1000;
		SpriteQieHuanEquep[0].spriteName = "UIB_Tab_A";
		SpriteQieHuanEquep[1].spriteName = "UIB_Tab_N";
		BagGuiWei();
		randCam.enabled = true;
		yield ;
		randCam.enabled = false;
		yield ;
		randCam.enabled = true;

		QieHuanBagBaoguo();
	}
	function QieHuanEquepShuXing(){
		TransQieHuanEquep[0].localPosition.y = 1000;
		TransQieHuanEquep[1].localPosition.y = 0;
		SpriteQieHuanEquep[0].spriteName = "UIB_Tab_N";
		SpriteQieHuanEquep[1].spriteName = "UIB_Tab_A";
		BagGuiWei();
	//	ShowEquepShuXing();
	}

	var invinfoOBj : GameObject;
	function BagGuiWei(){
		isCangku = false;
		isShangdian = false;
		if(AllManage.invCangKuStatic)
			AllManage.invCangKuStatic.transCangku.localPosition.y = 1000; 
		TransQieHuanParentEquep.localPosition.y = 0; 
		EquepInfo.InfoClose();
		UICL.CloseXunLian();
		invinfoOBj.transform.localPosition.y = 1000;
		invinfoOBj.SetActiveRecursively(false);
	}
	function BagGuiWeiNoInfo(){
		isCangku = false;
		isShangdian = false;
		if(AllManage.invCangKuStatic)
			AllManage.invCangKuStatic.transCangku.localPosition.y = 1000; 
		TransQieHuanParentEquep.localPosition.y = 0; 
	//	EquepInfo.InfoClose();
	//	UICL.CloseXunLian();
	}

	var LabelEquepShuXing : UILabel[];
	function ShowEquepShuXing(){
		var i : int = 0;
		for(i=0; i<LabelEquepShuXing.length; i++){
			if(LabelEquepShuXing[i] != null){	
				LabelEquepShuXing[i].text = EquipStatus[i].ToString();
			}
		}
	}

	var isUpdatePhoton : boolean = true;
	function UpdateEquipItem(){
			if(! EquipIt[0])
				return;
		EquipItemStr = ""; 
		var i : int = 0;
		for(i=0; i<EquipIt.length; i++){
			if(EquipIt[i].inv != null){
				EquipItemStr += EquipIt[i].inv.itemID;
			}
			EquipItemStr += ";";
		}
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(isUpdatePhoton){
			ps.selfProperties["EquipItem"] = EquipItemStr; 
	//		//print("1");
			PhotonNetwork.SetPlayerCustomProperties(ps.selfProperties);	
		}

		yt.Rows[0]["EquipItem"].YuanColumnText = EquipItemStr;
	//	//print(EquipItemStr + " == EquipItemStr");
	}

	var OpenBagMoney : int;
	function OpenBagNum(){
//		OpenBagMoney = PlayerInventoryNum*10;
		if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1){
			AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.YesOpenPackage , GetBDInfoInt("InventoryNum" , 1) , 0 , "" , gameObject , "YesOpenTips");
		}else{
			YesOpen();
		}
	}

	function YesOpenTips(objs : Object[]){
			AllManage.qrStatic.ShowBuyQueRen1(gameObject , "YesOpen" , "NoOpen" , AllManage.AllMge.Loc.Get("info298")+"" +objs[2] + AllManage.AllMge.Loc.Get("info303")+ "");		
	}

	function YesOpen(){
	 	if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.OpenInv).ToString());
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesOpenPackage , GetBDInfoInt("InventoryNum" , 1) , 0 , "" , gameObject , "realYesOpen");
//		PanelStatic.StaticBtnGameManager.RunOpenLoading(function()AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YesOpenPackage , GetBDInfoInt("InventoryNum" , 1) , 0 , "" , gameObject , "realYesOpen"));
//		AllManage.AllMge.UseMoney(0 , GetBDInfoInt("InventoryNum" , 1)*10 , UseMoneyType.YesOpenPackage , gameObject , "realYesOpen");
	//	if(ps.UseMoney(0 , OpenBagMoney)){
	//		PlayerInventoryNum += 1;
	//		yt.Rows[0]["InventoryNum"].YuanColumnText = PlayerInventoryNum.ToString();
	//	}
	//	SetPlayerInventoryNum(PlayerInventoryNum);
	}

	function realYesOpen(){
		PlayerInventoryNum += 1;
		if(PlayerInventoryNum > 4){
			PlayerInventoryNum = 4;
		}
		yt.Rows[0]["InventoryNum"].YuanColumnText = PlayerInventoryNum.ToString();
		if(GetVIPBagNum() > PlayerInventoryNum){
			PlayerInventoryNum = GetVIPBagNum();
		}

		SetPlayerInventoryNum(PlayerInventoryNum);
	}

	function NoOpen(){

	}

	var BagID : int = 1;
	function SelectBag1(){
		if( Input.touchCount > 1 ){
			return;
		}

		BagID = 1;  
		ShowBagButton(BagID);
		ClearBagItem();
		SetSelectBagItem(Inventory1);
	} 

	function SelectBag2(){
		if( Input.touchCount > 1 ){
			return;
		}
		if(PlayerInventoryNum >=2){
			BagID = 2;
			ShowBagButton(BagID);
			ClearBagItem();
			SetSelectBagItem(Inventory2); 
		}else{
			OpenBagNum();
		}
	//	//print(Inventory2 + " == ");
	} 
	function SelectBag3(){
		if( Input.touchCount > 1 ){
			return;
		}
		if(PlayerInventoryNum >=3){
			BagID = 3;
			ShowBagButton(BagID);
			ClearBagItem();
			SetSelectBagItem(Inventory3);
		}else{
			OpenBagNum();
		}
	} 
	function SelectBag4(){
		if( Input.touchCount > 1 ){
			return;
		}
		if(PlayerInventoryNum >=4){
			BagID = 4;
			ShowBagButton(BagID);
			ClearBagItem();
			SetSelectBagItem(Inventory4);
		}else{
			OpenBagNum();
		}
	}

	var bagButtons : UISprite[];
	var SelectBagGuangs : UISprite[];
	function ShowBagButton(id : int){
		for(var i=0; i<bagButtons.length; i++){
			if(SelectBagGuangs[i]){
				if(i == id){
					SelectBagGuangs[i].enabled = true;
				}else{
					SelectBagGuangs[i].enabled = false;
				}
			}
		}
	}

	function ClearOneBagItem(itemID : String){
		for(var i=0; i<BagIt.length; i++){
			if(BagIt[i].inv != null){
				if(BagIt[i].inv.itemID == itemID){
					BagIt[i].OtherYiChu();
					return;
				}	
			}
		}
	}

	function ClearBagItem(){
		for(var i=0; i<BagIt.length; i++){
//			if(bagInventorys[i])
				bagInventorys[i] = null;
			if(BagIt[i])
				BagIt[i].invClear();
		}
	}
	
	var useInvID : String[];
	function SetSelectBagItem(invID : String){
		var i : int = 0;
		useInvID = invID.Split(Fstr.ToCharArray());
		DontUpdate = true;
//		print(invID + " == " + BagID);	
//		print(yt.Rows[0]["Inventory1"].YuanColumnText + " =111= yt.Ro" + " ==" + Inventory1);
		
		for(i=0; i<useInvID.length; i++){	 
			if(useInvID[i] != null && useInvID[i] != "" && useInvID[i].Length > 3){ 
				bagInventorys[i] = AllResources.InvmakerStatic.GetItemInfo(useInvID[i] , bagInventorys[i]);	
			}
		}
//		for(i=0; i<15; i++){	 
//			if(bagInventorys[i] != null){ 
//				print(bagInventorys[i].itemID + " == " + BagID + " =111= useInvID[i]");	
//			}
//		}
//		print(yt.Rows[0]["Inventory1"].YuanColumnText + " =222= yt.Ro" + " ==" + Inventory1);
		ShowBagItemRealUI();
//		print(yt.Rows[0]["Inventory1"].YuanColumnText + " =333= yt.Ro" + " ==" + Inventory1);
		UpdateBagItem();
//		for(i=0; i<15; i++){	 
//			if(bagInventorys[i] != null){ 
//				print(bagInventorys[i].itemID + " == " + BagID + " =222= useInvID[i]");	
//			}
//		}
//		print(yt.Rows[0]["Inventory1"].YuanColumnText + " =444= yt.Ro" + " ==" + Inventory1);
	}

	function ShowBagItemRealUI(){
		if(! AllManage.UICLStatic.MainUIOn){
			return;
		}
		var i : int = 0;
		for(i=0; i<bagInventorys.length; i++){	 
//			print(bagInventorys[i]);
			BagIt[i].invClear();
			if(bagInventorys[i] != null){ 
//				print(bagInventorys[i].itemID + " == itemID");
				AddBagItemRealUI(bagInventorys[i]); 
			}
		}
	}

	function AddBagItemRealUI(inv : InventoryItem){
		var i : int = 0; 
		var j : int = 0;
		if(inv.slotType < 12){	
			for(i=0; i<BagIt.length; i++){ 
				if(BagIt[i].inv == null){
					BagIt[i].SetInv(inv);
//					UpdateBagItem();
					return;
				}
			}
		}else{
			for(i=0; i<BagIt.length; i++){
				if(BagIt[i].inv != null && BagIt[i].inv.itemID){
					if(BagIt[i].inv.itemID.Length > 5){
						if(BagIt[i].inv.itemID.Substring(0,5) == inv.itemID.Substring(0,5) && BagIt[i].inv.itemID.Substring(0,1) != "9"){
//							print(yt.Rows[0]["Inventory1"].YuanColumnText + " =99999= yt.Ro" + " == " + inv.itemID);
							if(BagIt[i].inv.consumablesNum + inv.consumablesNum <= 20){				
								BagIt[i].inv.consumablesNum += inv.consumablesNum; 
								if(BagIt[i].inv.consumablesNum < 10){
									BagIt[i].inv.itemID =  BagIt[i].inv.itemID.Substring(0,5) + "0" +  BagIt[i].inv.consumablesNum.ToString() + ","+BagIt[i].inv.itemID.Substring(8,1);				
								}else{
										BagIt[i].inv.itemID =  BagIt[i].inv.itemID.Substring(0,5) + BagIt[i].inv.consumablesNum.ToString() + ","+BagIt[i].inv.itemID.Substring(8,1); 											
								}
								BagIt[i].showConsumablesNum();
//								print(yt.Rows[0]["Inventory1"].YuanColumnText + " =100000= yt.Ro" + " ==" + Inventory1);
//								UpdateBagItem();
//								print(BagIt[i].inv.consumablesNum + " == itemID == " + yt.Rows[0]["Inventory1"].YuanColumnText);
								return;
							}else{
								var useNum : int = BagIt[i].inv.consumablesNum;
								BagIt[i].inv.consumablesNum =20; 
								BagIt[i].inv.itemID =  BagIt[i].inv.itemID.Substring(0,5) + BagIt[i].inv.consumablesNum.ToString() + ","+BagIt[i].inv.itemID.Substring(8,1);
								BagIt[i].showConsumablesNum(); 
//								UpdateBagItem(); 
								if(useNum + inv.consumablesNum == 20){
									return;
								}
								var useInv : InventoryItem;
								useInv = inv;
								useInv.consumablesNum = useNum + inv.consumablesNum - 20; 
								if(useInv.consumablesNum < 10){
									useInv.itemID =  useInv.itemID.Substring(0,5) + "0" +  useInv.consumablesNum.ToString() + ","+useInv.itemID.Substring(8,1);					
								}else{
									useInv.itemID =  useInv.itemID.Substring(0,5) + useInv.consumablesNum.ToString() + ","+useInv.itemID.Substring(8,1);											
								}
								for(j=0; j<BagIt.length; j++){
									if(BagIt[j].inv == null){ 
										BagIt[j].SetInv(useInv);
//										UpdateBagItem();
										return;					
									}
								}
							}
						}
					}
				}
			} 
			for(i=0; i<BagIt.length; i++){ 
				if(BagIt[i].inv == null){  
					 BagIt[i].SetInv(inv);
					BagIt[i].showConsumablesNum();
//					UpdateBagItem();
					return;					
				}
			}
		}
	}

	function GetBagStrAsLevel(level : int , sss : String) : String{
		switch(level){
			case 1 : sss = yt.Rows[0]["Inventory1"].YuanColumnText ; break;
			case 2 : sss = yt.Rows[0]["Inventory1"].YuanColumnText  + yt.Rows[0]["Inventory2"].YuanColumnText ; break;
			case 3 : sss = yt.Rows[0]["Inventory1"].YuanColumnText  + yt.Rows[0]["Inventory2"].YuanColumnText  + yt.Rows[0]["Inventory3"].YuanColumnText ; break;
			case 4 : sss = yt.Rows[0]["Inventory1"].YuanColumnText  + yt.Rows[0]["Inventory2"].YuanColumnText  + yt.Rows[0]["Inventory3"].YuanColumnText  + yt.Rows[0]["Inventory4"].YuanColumnText ; break;			
			default : sss = yt.Rows[0]["Inventory1"].YuanColumnText  + yt.Rows[0]["Inventory2"].YuanColumnText  + yt.Rows[0]["Inventory3"].YuanColumnText  + yt.Rows[0]["Inventory4"].YuanColumnText ;
		}
		return sss;
	}

	//var qr : QueRen;
	function isBagFull() : boolean{
		var str : String;
		str = GetBagStrAsLevel(PlayerInventoryNum , str);
		var i : int = 0;
		useInvID = str.Split(Fstr.ToCharArray());
		var useInt : int = 0;
		for(i=0; i<useInvID.length; i++){	
			if(useInvID[i] == ""){ 
				useInt += 1;
			}
		} 
		if(useInt <= 3){
			return true;
			AllManage.qrStatic.ShowQueRen(gameObject,"" , "messages002");
		}
		return false;
	}

	function AddBagItemAsID(id : String){
		var yinv : InventoryItem;
	//	yinv = new  InventoryItem();
		yinv = AllResources.InvmakerStatic.GetItemInfo(id , yinv ); 
		if(yinv != null){
			AddBagItem(yinv);	
		}
	}

	function YAddGold(i : int){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YAddGold , i , 0 , "" , gameObject , "");
//		AllManage.AllMge.UseMoney(i * (-1) , 0 , UseMoneyType.YAddGold , gameObject , "");
	//	ps.UseMoney(i * (-1) , 0);
	}
	function YAddBlood(i : int){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.YAddBlood , i  , 0 , "" , gameObject , "");
//		AllManage.AllMge.UseMoney(0 , i * (-1) , UseMoneyType.YAddBlood , gameObject , "");
	//	ps.UseMoney(0 , i * (-1));
	}
	function YAddPower(i : int){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		ps.AddPower(i * (-1));
	}

	function YesOpenshowCanEquep(){
	//	showCanEquepThisItembag.gameObject.SetActiveRecursively(true);
	//	yield;
	//	showCanEquepThisItembag.OtherYiChu();
	//	yield;
	//	showCanEquepThisItembag.gameObject.SetActiveRecursively(false);
		if(showCanEquepThisItemEqu.inv != null){
			AllManage.ItMoveStatic.SetJiaoHuan(showCanEquepThisItembag);
		}else{
			DesBagItem(showCanEquepThisItemInv);
			yield;
			UpdateBagItem();
			ReInitItem1();
		}
		
//		if(showCanEquepThisItemEqu.inv != null){
//			AddBagItem(showCanEquepThisItemEqu.inv);		
//		}
//		yield;
//		UpdateBagItem();
//		ReInitItem1();
		showCanEquepThisItemEqu.canJiaoHuan = true;
		showCanEquepThisItemEqu.RealSetZHuangbei(showCanEquepThisItemInv);
		showCanEquepThisItem = false;
	}

	function NoOpenshowCanEquep(){
		showCanEquepThisItem = false;
	}

	var showCanEquepThisItem : boolean = false;
	var showCanEquepThisItemEqu : BagItem;
	var showCanEquepThisItembag : BagItem;
	var showCanEquepThisItemInv : InventoryItem;
	var eqInfoObj : GameObject;

	function LookCanForceOver(Linv : InventoryItem) : int
	{
		var i : int = 0; 
		var m : int = 0; 
		
		if(! AllManage.UICLStatic.MainUIOn){
					for(m=0; m<Equeipitems.length; m++){
						if( Equeipitems[m] != null && Linv.slotType == Equeipitems[m].slotType  && m != 1){
							if(Linv.ATzongfen > Equeipitems[m].ATzongfen){
								return CommonDefine.Force_Higher;
							}else
							if(Linv.ATzongfen < Equeipitems[m].ATzongfen){
								return CommonDefine.Force_Lower;
							}else
							if(Linv.ATzongfen == Equeipitems[m].ATzongfen){
								return CommonDefine.Force_NON;
							}				
						}
					}
		}else{
			for(m=0; m<EquipIt.length; m++){
				if(EquipIt[m].inv != null && Linv.slotType == EquipIt[m].inv.slotType && m != 1){
					if(Linv.ATzongfen > EquipIt[m].inv.ATzongfen){
						return CommonDefine.Force_Higher;
					}else
					if(Linv.ATzongfen < EquipIt[m].inv.ATzongfen){
						return CommonDefine.Force_Lower;
					}else
					if(Linv.ATzongfen == EquipIt[m].inv.ATzongfen){
						return CommonDefine.Force_NON;
					}
				}
			}
		}
		
		return CommonDefine.Force_NON;
	}

	function LookCanEquepmentItem(){
		var i : int = 0; 
		var m : int = 0; 
		var bool : boolean = false;
		var isSet : boolean = false; 
		if(! showCanEquepThisItem){
			for(i=0; i<BagIt.length; i++){ 
				if(BagIt[i].inv != null && BagIt[i].inv.slotType < 12){
					if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
						bool = (BagIt[i].inv.WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
					}else{
						bool = (BagIt[i].inv.WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
					}
					if(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText == "" || InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText == "0"){
						bool = true;
					}
					for(m=0; m<EquipIt.length; m++){
						if( ! isSet && (EquipIt[m].inv == null  || (EquipIt[m].inv != null && BagIt[i].inv.ATzongfen > EquipIt[m].inv.ATzongfen && BagIt[i].inv.slotType == EquipIt[m].inv.slotType ))&& m != 1 && bool){
							isSet = EquipIt[m].OtherZhuangBeiAuto(BagIt[i].inv);
							if(isSet && isAlreadyShowThisEquip(BagIt[i].inv)){
								if(eqInfoObj)
									eqInfoObj.SendMessage("InfoClose" , SendMessageOptions.DontRequireReceiver);
								showCanEquepThisItemEqu = EquipIt[m];
								showCanEquepThisItembag = BagIt[i];
								showCanEquepThisItemInv = BagIt[i].inv;
								showCanEquepThisItem = true;
								AllManage.qrStatic.ShowZhuangBei(gameObject , "YesOpenshowCanEquep" , "NoOpenshowCanEquep" , BagIt[i].inv.atlasStr,AllManage.AllMge.Loc.Get("info1067"),BagIt[i].inv.itemQuality);
							}
						}
					}
				}
			}
		}
	}
//								if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
//									bool = (Equeipitems[0].WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
//								}else{
//									bool = (Equeipitems[0].WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
//								}
//
//									if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
//										bool = (EquipIt[0].inv.WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
//									}else{
//										bool = (EquipIt[0].inv.WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
//									}	

	function LookCanEquepmentItem111(){
		var i : int = 0; 
		var m : int = 0; 
		var isSet : boolean = false; 
		var bool : boolean = false;
		if(! showCanEquepThisItem){
			for(i=0; i<BagIt.length; i++){ 
				
				if(BagIt[i].inv != null && BagIt[i].inv.slotType < 12){
					if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
						bool = (BagIt[i].inv.WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
					}else{
						bool = (BagIt[i].inv.WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
					}
					for(m=0; m<EquipIt.length; m++){
//						if( ! isSet && (EquipIt[m].inv == null  || (EquipIt[m].inv != null && BagIt[i].inv.ATzongfen > EquipIt[m].inv.ATzongfen && BagIt[i].inv.slotType == EquipIt[m].inv.slotType && bool)) && m != 1){
						if( ! isSet && (EquipIt[m].inv == null  || (EquipIt[m].inv != null && BagIt[i].inv.slotType == EquipIt[m].inv.slotType && bool)) && m != 1){
							isSet = EquipIt[m].OtherZhuangBeiAuto111(BagIt[i].inv);
							if(isSet && isAlreadyShowThisEquip(BagIt[i].inv)){
								if(eqInfoObj)
									eqInfoObj.SendMessage("InfoClose" , SendMessageOptions.DontRequireReceiver);
								showCanEquepThisItemEqu = EquipIt[m];
								showCanEquepThisItembag = BagIt[i];
								showCanEquepThisItemInv = BagIt[i].inv;
								showCanEquepThisItem = true;
								AllManage.qrStatic.ShowZhuangBei(gameObject , "YesOpenshowCanEquep" , "NoOpenshowCanEquep" , BagIt[i].inv.atlasStr,AllManage.AllMge.Loc.Get("info1193"),BagIt[i].inv.itemQuality);
							}else{
								isSet = false;
							}
						}
					}
				}
			}
		}
	}
	
	function LookHaveCanEquepmentItem111(){
		var i : int = 0; 
		var m : int = 0; 
		var bool : boolean = false;
		var isSet : boolean = false; 
		if(! showCanEquepThisItem){
			for(i=0; i<bagInventorys.length; i++){ 

				if(bagInventorys[i] != null && bagInventorys[i].slotType < 12){
					if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
						bool = (bagInventorys[i].WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
					}else{
						bool = (bagInventorys[i].WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
					}
					for(m=0; m<Equeipitems.length; m++){
//						print(isSet + " == " + Equeipitems[m] + " == " + m);
//						if( ! isSet && (Equeipitems[m] == null  || (Equeipitems[m] != null && bagInventorys[i].ATzongfen > Equeipitems[m].ATzongfen && bagInventorys[i].slotType == Equeipitems[m].slotType && bool))  && m != 1){
						if( ! isSet && (Equeipitems[m] == null  || (Equeipitems[m] != null && bagInventorys[i].slotType == Equeipitems[m].slotType && bool))  && m != 1){
							isSet = OtherZhuangBeiAuto(bagInventorys[i] , m);
							if(isSet && isAlreadyShowThisEquip(bagInventorys[i])){
								AllManage.qrStatic.ShowZhuangBei(gameObject , "YesHaveEquep" , "" , bagInventorys[i].atlasStr,AllManage.AllMge.Loc.Get("info1193"),bagInventorys[i].itemQuality);
							}else{
								isSet = false;
							}
//								AllManage.qrStatic.ShowZhuangBei(gameObject , "YesHaveEquep" , "" , AllManage.AllMge.Loc.Get("info280") + bagInventorys[i].atlasStr + AllManage.AllMge.Loc.Get("info281"));
						}
					}
				}
			}
		}
	}
	
	function LookHaveCanEquepmentItem(){
		var i : int = 0; 
		var m : int = 0; 
		var bool : boolean = false;
		var isSet : boolean = false; 
		if(! showCanEquepThisItem){
			for(i=0; i<bagInventorys.length; i++){ 
				if(bagInventorys[i] != null && bagInventorys[i].slotType < 12){
					if(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText == "3"){
						bool = (bagInventorys[i].WeaponType != parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
					}else{
						bool = (bagInventorys[i].WeaponType == parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText));
					}
					if(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText == "" || InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText == "0"){
						bool = true;
					}
					for(m=0; m<Equeipitems.length; m++){
//						print(isSet + " == " + Equeipitems[m] + " == " + m);
						if( ! isSet && (Equeipitems[m] == null  || (Equeipitems[m] != null && bagInventorys[i].ATzongfen > Equeipitems[m].ATzongfen && bagInventorys[i].slotType == Equeipitems[m].slotType )) && m != 1 && bool){
							isSet = OtherZhuangBeiAuto(bagInventorys[i] , m);
							if(isSet && isAlreadyShowThisEquip(bagInventorys[i]))
								AllManage.qrStatic.ShowZhuangBei(gameObject , "YesHaveEquep" , "" , bagInventorys[i].atlasStr,AllManage.AllMge.Loc.Get("info1067"),bagInventorys[i].itemQuality);
//								AllManage.qrStatic.ShowZhuangBei(gameObject , "YesHaveEquep" , "" , AllManage.AllMge.Loc.Get("info280") + bagInventorys[i].atlasStr + AllManage.AllMge.Loc.Get("info281"));
						}
					}
				}
			}
		}
	}

	var ArrayCanEquep : Array = new Array();
	function isAlreadyShowThisEquip(Uinv : InventoryItem){
		for(var i=0; i<ArrayCanEquep.Count; i++){
			if(Uinv.itemID == ArrayCanEquep[i]){
				return false;
			}
		}
		if(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "0" && InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText != "3"){
			if(parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText) != parseInt(Uinv.WeaponType)){
				return false;
			}
		}
		ArrayCanEquep.Add(Uinv.itemID);
		return true;
	}


	function YesHaveEquep(){
		yield InsMainUI();
		var i : int = 0; 
		var m : int = 0; 
		var isSet : boolean = false; 
		if(! showCanEquepThisItem){
			for(i=0; i<BagIt.length; i++){ 
				if(BagIt[i].inv != null && BagIt[i].inv.slotType < 12){
					for(m=0; m<EquipIt.length; m++){
						if( ! isSet && (EquipIt[m].inv == null || (EquipIt[m] != null && BagIt[i].inv.ATzongfen > EquipIt[m].inv.ATzongfen && BagIt[i].inv.slotType == EquipIt[m].inv.slotType )) && m != 1){
							isSet = EquipIt[m].OtherZhuangBeiAuto(BagIt[i].inv);
							if(isSet){
								if(eqInfoObj)
									eqInfoObj.SendMessage("InfoClose" , SendMessageOptions.DontRequireReceiver);
								showCanEquepThisItemEqu = EquipIt[m];
								showCanEquepThisItembag = BagIt[i];
								showCanEquepThisItemInv = BagIt[i].inv;
								showCanEquepThisItem = true;
								YesOpenshowCanEquep();
							}
						}
					}
				}
			}
		}
	}

	function InsMainUI(){
		if(! AllManage.UICLStatic.objMainUI){
			AllManage.UICLStatic.MainUIOn = true;
			var preMainUIl = Resources.Load("Anchor - MainUI", GameObject);
			AllManage.UICLStatic.objMainUI = GameObject.Instantiate(preMainUIl);
			AllManage.UICLStatic.objMainUI.transform.parent = AllManage.UICLStatic.TransMainUI;
			AllManage.UICLStatic.objMainUI.transform.localPosition = Vector3.zero;
		}
		while(AllManage.UICLStatic.TransPlayerInfo[1] == null){
			yield;				
		}
		yield;	
		yield;	
	}

	var useZhuangBeiType : SlotType[];
	function OtherZhuangBeiAuto(otInv : InventoryItem , mID : int) : boolean{
		if(useZhuangBeiType[mID] == otInv.slotType && otInv.professionType == PlayerProfession && parseInt(Plys.Level) >= otInv.itemLevel  && parseInt(Plys.PVPPoint) >= otInv.needPVPPoint){  
//			print( parseInt(Plys.Level) );
//			print(otInv.itemLevel);
//			print(otInv.slotType);
//			print(otInv.professionType);
//			print();
//			print();
//			print();
			return true;
		}else{
		}
		return false;
	}

	private var mainPS : MainPersonStatus;
	function IsAlreadyDoneTaskID(id : String){
		if(mainPS == null && PlayerStatus.MainCharacter ){
			mainPS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
		}
		for(var i=0; i<mainPS.player.doneTaskID.length; i++){
			if(mainPS.player.doneTaskID[i] == id){
				return true;
			}
		}
		return false;
	}

	var BagIt : BagItem[];
	function AddBagItem(inv : InventoryItem){
		AllManage.UICLStatic.CategoryTipsAsID(inv.itemID);	
		var youKong : boolean = false;
		if(!youKong){
			SelectBag1();
			youKong = jianchabaoguoForXiaoHao(inv);
		} 
		if(!youKong && PlayerInventoryNum >=2){
			SelectBag2();
			youKong = jianchabaoguoForXiaoHao(inv);
		}
		if(!youKong && PlayerInventoryNum >=3){
			SelectBag3();
			youKong = jianchabaoguoForXiaoHao(inv);
		} 
		if(!youKong && PlayerInventoryNum >=4){
			SelectBag4();
			youKong = jianchabaoguoForXiaoHao(inv);
		} 
		if(!youKong){
			AllManage.tsStatic.Show("tips030");
			return;
		}
		var i : int = 0; 
		var j : int = 0;
		var isSet : boolean = false; 
		if(inv.slotType < 12){
			for(i=0; i<bagInventorys.length; i++){ 
				if(bagInventorys[i] == null){
					bagInventorys[i] = inv;
					UpdateBagItem();
					ShowBagItemRealUI();
					return;
				}
			}
		}else{
			for(i=0; i<bagInventorys.length; i++){
				if(bagInventorys[i] != null){
					if(bagInventorys[i].itemID.Length > 5 && bagInventorys[i].consumablesNum != 0){
						if(bagInventorys[i].itemID.Substring(0,5) == inv.itemID.Substring(0,5)&& bagInventorys[i].consumablesNum != 20 ){
							if(bagInventorys[i].consumablesNum + inv.consumablesNum <= 20){		
								bagInventorys[i].consumablesNum += inv.consumablesNum; 
								if(bagInventorys[i].consumablesNum < 10){
									bagInventorys[i].itemID = bagInventorys[i].itemID.Substring(0,5) + "0" +  bagInventorys[i].consumablesNum.ToString() + ","+ bagInventorys[i].itemID.Substring(8,1);					
								}else{
										bagInventorys[i].itemID = bagInventorys[i].itemID.Substring(0,5) + bagInventorys[i].consumablesNum.ToString() + ","+ bagInventorys[i].itemID.Substring(8,1);													
								}
//								BagIt[i].showConsumablesNum();
								UpdateBagItem();
								mtw.JianChaItem(bagInventorys[i].itemID);
								ShowBagItemRealUI();
								return;
							}else{  
								var useNum : int = bagInventorys[i].consumablesNum;
								bagInventorys[i].consumablesNum =20; 
								bagInventorys[i].itemID = bagInventorys[i].itemID.Substring(0,5) + bagInventorys[i].consumablesNum.ToString() + ","+bagInventorys[i].itemID.Substring(8,1);
//								BagIt[i].showConsumablesNum(); 
								UpdateBagItem(); 
								var useInv : InventoryItem;
								useInv = inv;
								useInv.consumablesNum = useNum + inv.consumablesNum - 20; 
								if(useInv.consumablesNum < 10){
									useInv.itemID =  useInv.itemID.Substring(0,5) + "0" +  useInv.consumablesNum.ToString() + ","+useInv.itemID.Substring(8,1);					
								}else{
									useInv.itemID =  useInv.itemID.Substring(0,5) + useInv.consumablesNum.ToString() + ","+useInv.itemID.Substring(8,1);	 											
								}
								if(jianchabaoguoForXiaoHao(useInv)){
									for(j=0; j<bagInventorys.length; j++){
										if(bagInventorys[j] == null){ 
											bagInventorys[j] = useInv;
											UpdateBagItem();
											mtw.JianChaItem(bagInventorys[j].itemID);
											ShowBagItemRealUI();
											return;					
										}
									}
								}else{ 
									AddBagItem2(useInv); 
									mtw.JianChaItem(useInv.itemID);
									ShowBagItemRealUI();
									return;
								}
							}
						}
					}
				}
			} 
			for(i=0; i<bagInventorys.length; i++){ 
				if(bagInventorys[i] == null){  
					bagInventorys[i] = inv;
//					BagIt[i].showConsumablesNum();
					UpdateBagItem();
					mtw.JianChaItem(bagInventorys[i].itemID);
					ShowBagItemRealUI();
					return;					
				}
			}
		}
		ShowBagItemRealUI();
	}

	function DesBagItemAsID(id : String){
		var useiv : InventoryItem;
		useiv = AllResources.InvmakerStatic.GetItemInfo(id , useiv);
		DesBagItem(useiv);
		UpdateBagItem();
		ReInitItem1();
	}

	function DesBagItem(inv : InventoryItem) : boolean{
		var youKong : boolean = false;
		if(!youKong){
			SelectNoOpenBag1();
			youKong = jianchabaoguo(inv.itemID);
		} 
		if(!youKong){
			SelectNoOpenBag2();
			youKong = jianchabaoguo(inv.itemID);
		} 
		if(!youKong){
			SelectNoOpenBag3();
			youKong = jianchabaoguo(inv.itemID);
		} 
		if(!youKong){
			SelectNoOpenBag4(); 
			youKong = jianchabaoguo(inv.itemID);
		}
		if(!youKong){
			 return false;
		} 
			var i : int = 0;
			for(i=0; i<bagInventorys.length; i++){
				if(bagInventorys[i] != null){
					if(inv.itemID.Length >= 25){
						if(bagInventorys[i].itemID.Length >= 25 && bagInventorys[i].itemID.Substring(0,25) == inv.itemID.Substring(0,25)){ 
							bagInventorys[i] = null;
							return true;
						}
					}else
					if(bagInventorys[i].itemID.Length > 5 && bagInventorys[i].itemID.Length < 12){
						if(bagInventorys[i].itemID.Substring(0,5) == inv.itemID.Substring(0,5)){ 
							bagInventorys[i] = null;
							return true;
						}
					}
				}
			}
			
	//	yield; 
		
	//	UpdateBagItem(); 
	//	SelectBag1();
		return false;
	}

	function fuzhi(){
		var iiiv : InventoryItem;
		iiiv = AllResources.InvmakerStatic.GetItemInfo("8401,12", iiiv); 
	//	//print(iiiv.consumablesNum + " == consumablesNum" );
		AddBagItem(iiiv);
	}


	function jianchabaoguo() : boolean{
		for(var i=0; i<BagIt.length; i++){ 
			if(BagIt[i].inv == null){
				return true;
			}
		}
		return false;
	}

	function jianchabaoguo(itemID : String) : boolean{
		for(var i=0; i<BagIt.length; i++){  
			if(BagIt[i].inv != null){	
				if((BagIt[i].inv.itemID.Substring(0,9) == itemID.Substring(0,9) && BagIt[i].inv.slotType >= 12) || (BagIt[i].inv.itemID.Split(Dstr.ToCharArray())[0] == itemID.Split(Dstr.ToCharArray())[0] && BagIt[i].inv.slotType < 12)){
					return true;
				}
			}
		}
		return false;
	}

	function jianchabaoguoForXiaoHao(inv : InventoryItem) : boolean{
		for(var i=0; i<bagInventorys.length; i++){  
			if(bagInventorys[i] != null){	
				if(bagInventorys[i].consumablesNum == 0){
					
				}else
				if(bagInventorys[i].itemID.Substring(0,5) == inv.itemID.Substring(0,5) && bagInventorys[i].consumablesNum != 20){
					return true;
				}
			}else{
				return true;
			}
		}
		return false;
	}

	function useBagItem(id : String , num : int) : boolean{
	//	//print("da kong qu" + invID + " == " + id);
	//	//print(useInvID[id] + " == " + m);
	//	useInvID[id] = useInvID[id].Substring(0,17) + useStr1 + useStr2 + useStr3; 
		var useInt : int;
		var useInvID : String[];
		var str : String;
		var bool : boolean = false;
		str = GetBagStrAsLevel(PlayerInventoryNum , str);
	//	//print(str);
		useInvID = str.Split(Fstr.ToCharArray());
		for(var j=0; j<useInvID.length; j++){
			if(useInvID[j].Length > 2 && !bool){
				if(useInvID[j].Substring(0,4) == id.Substring(0,4) || (id.Length > 4 && useInvID[j].Substring(0,4) == id.Substring(0,4) && useInvID[j].Substring(8,1) == id.Substring(8,1))){
					useInt = parseInt(useInvID[j].Substring(5,2));
					useInt -= 1;
					if(useInt < 0){
						useInt = 0;
					}
	//				//print(useInvID[j] + " == useInvID[j]");
					var intToString : String = useInt.ToString();
					if(intToString.Length < 2){
						intToString = "0" + intToString;
					}
					useInvID[j] = useInvID[j].Substring(0,5) + intToString;
					if(useInvID[j].Length > 7){
						useInvID[j] = useInvID[j].Substring(0,7) + ","+useInvID[j].Substring(8,1);
					}
					if(useInt == 0){
						useInvID[j] = "";
					}
					bool = true;
				}
			}
		}
		yt.Rows[0]["Inventory1"].YuanColumnText = "";
		yt.Rows[0]["Inventory2"].YuanColumnText = "";
		yt.Rows[0]["Inventory3"].YuanColumnText = "";
		yt.Rows[0]["Inventory4"].YuanColumnText = "";
		for(var i=0; i<useInvID.length; i++){	 
			if(i < 15){
				yt.Rows[0]["Inventory1"].YuanColumnText += useInvID[i] + ";";
			}else
			if(i < 30){
				yt.Rows[0]["Inventory2"].YuanColumnText += useInvID[i] + ";";		
			}else
			if(i < 45){
				yt.Rows[0]["Inventory3"].YuanColumnText += useInvID[i] + ";";		
			}else
			if(i < 60){
				yt.Rows[0]["Inventory4"].YuanColumnText += useInvID[i] + ";";		
			}
		}

	//	//print(yt.Rows[0]["Inventory1"].YuanColumnText);
	//	//print(yt.Rows[0]["Inventory2"].YuanColumnText);
	//	//print(yt.Rows[0]["Inventory3"].YuanColumnText);
	//	//print(yt.Rows[0]["Inventory4"].YuanColumnText);
	//	yield;
	//	yield;
	//	yield;
		
	//	ReInitItem();
		ReInitItemItemNo();
		return bool;
	//	
	}
	function CopyToBagIt(){
		if(BagIt[0] == null)
			return;
		for(var i=0; i<bagInventorys.length; i++){
			bagInventorys[i] = null;
			if(BagIt[i] != null && BagIt[i].inv != null){
				bagInventorys[i] = BagIt[i].inv;
			}
		}
	}

	private var yuseInventoryBagID : String;
	private var DontUpdate : boolean = false;
	function UpdateBagItem(){
//		CopyToBagIt()
		if(AllManage.invCangKuStatic)
		AllManage.invCangKuStatic.UpdateBagItem();
		yuseInventoryBagID = "";  
		if(DontUpdate){	
			yuseInventoryBagID = GetSelectBagID(BagID); 
		}		
		var i : int = 0;
		var str : String;
		if(!DontUpdate){
			for(i=0; i<bagInventorys.length; i++){
				if(bagInventorys[i] != null)
					str = bagInventorys[i].itemID;
				else
					str = "";
				yuseInventoryBagID += (str + ";"); 
			}
			yt.Rows[0]["Inventory" + BagID.ToString()].YuanColumnText = yuseInventoryBagID;
		}
		switch(BagID){
			case 1:  Inventory1 = yuseInventoryBagID; break;
			case 2:  Inventory2 = yuseInventoryBagID; break;
			case 3:  Inventory3 = yuseInventoryBagID; break;
			case 4:  Inventory4 = yuseInventoryBagID; break;
		}
		DontUpdate = false;
	}

	function GetSelectBagID(id : int) : String{	
		switch(id){
			case 1: return Inventory1;
			case 2: return Inventory2;
			case 3: return Inventory3;
			case 4: return Inventory4;
		}
	}


	var TPWeapon : ThirdPersonWeapon;
	var skillit : SkillItem[];
	function GoShowWeapon(myInv : BagItemType , equepmentID : int){
		if(TPWeapon == null){
			return;
		}
	//	//print(myInv.myType);

		if(myInv.myType == SlotType.Bag){
			if(myInv.inv.itemmodle1){
				myInv.inv.itemmodle1.transform.parent = null;
				myInv.inv.itemmodle1.SetActiveRecursively(false);
			}
			if(myInv.inv.itemmodle2){
				myInv.inv.itemmodle2.transform.parent = null;
				myInv.inv.itemmodle2.SetActiveRecursively(false);
			}
		}else
	//	if(myInv.inv)
		{
			TPWeapon.ShowWeapon(myInv.inv , equepmentID);
	//		SetPersonEquipment(myInv.inv , equepmentID);
		}
	//	//print(myInv.inv);
		for(var i=0; i<skillit.length; i++){
			skillit[i].StartSetSkill();
		}
		UpdateEquipItem();
		GetPersonEquipment();
	}

	function lookTou(){
		if(TPWeapon == null){
			return;
		}
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		ps.selfProperties["HideHelmet"] = GetBDInfoInt1("HideHelmet",0).ToString();   
		PhotonNetwork.SetPlayerCustomProperties(ps.selfProperties);	
		//	//print("2");
		TPWeapon.lookTou(GetBDInfoInt1("HideHelmet",0));
	}
	 function GetBDInfoInt1(bd : String , it : int) : int{  
		var iii : int = 0;
		try{ 
			iii = parseInt(InventoryControl.yt.Rows[0][bd].YuanColumnText);
			return  iii;
		}catch(e){
			return it;	
		}
	} 

	function reMoveWeapon(equepmentID : int){
		PES[equepmentID].inv = null;
	}

	function CloseInvItem(sType : SlotType , EquepmentID : int){
		TPWeapon.ObjCloseAsTypeFirst(sType , EquepmentID);
	}

	function GoShowMainTexture(){
		TPWeapon.ShowMainTexture();
	}

	function SetPersonEquipment(inv : InventoryItem , equepmentID : int){
	//	var i : int = 0;
	//	for(i=0; i<PES.length; i++){
	//		if(PES[i].invType == inv.slotType && i == equepmentID){
	//			PES[i].inv = inv;
	//		}
	//	}
	}

	function UpdatePES(inv : InventoryItem , equepmentID : int){
		var i : int = 0;
		for(i=0; i<PES.length; i++){
			if(i == equepmentID){
				PES[i].inv = inv;
			}
		}
	}

	var NewPes : PersonEquipment[];
	//var TransNewPes : Transform;
	function BiJiaoNewPes(inv : InventoryItem){
		var i : int = 0;
	//	PES.CopyTo(NewPes , 0);
		for(i=0; i<NewPes.length; i++){
			if(PES[i].inv && PES[i].inv.itemID.Length > 0){
				var invs : InventoryItem;
				invs = AllResources.InvmakerStatic.GetItemInfo(PES[i].inv.itemID , invs);
				NewPes[i].inv = invs;
			}
		}
	//	PES.CopyTo(NewPes , 0);
		for(i=0; i<NewPes.length; i++){
			if(NewPes[i].invType == inv.slotType){
				NewPes[i].inv = inv;
				BiJiaoEquipStatus();
				return;
			}
		}
	}

	function BiJiaoEquipStatus(){
		var i : int = 0;
		for(i=0; i<EquipStatus.length; i++){
			EquipStatus[i] = 0;
		}
		for(i=0; i<NewPes.length; i++){
			if(NewPes[i].inv != null){
				if(NewPes[i].inv.itemID != ""){
					GetEquipStatus(NewPes[i] , i);			
				}
			}
		}
		ps.OtherSetEquepInfo(EquipStatus);
		OthershowPlayerInfo();
	}

	var PES : PersonEquipment[];
	var ps : PlayerStatus;
	var SpriteWeapon : UISprite;  
	var SpriteWeapon1 : UISprite; 
	var soucl : SoulControl;
	function GetPersonEquipment(){
		var i : int = 0;
		for(i=0; i<EquipStatus.length; i++){
			EquipStatus[i] = 0;
		}
		
		for(i=0; i<PES.length; i++){
			if(PES[i].inv != null){
	//			//print(PES[i].inv.itemID);
				if(PES[i].inv.itemID != ""){
					GetEquipStatus(PES[i] , i);			
				}
			}
		}
		if(soucl){
			soucl.BuildEquepPes();	
		}else{
			BuildEquepPes();
		}
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		
		if(ps != null){
			var str : String;
			ps.SetEquepInfo(EquipStatus);
			if(PES[10].inv == null){
				ps.weaponType = PlayerWeaponType.empty;
				ps.ChangeWeapons(PlayerWeaponType.empty);
			}else{
				ps.weaponType = PES[10].inv.WeaponType;
	//			print(ps.ProID + " =2= " + ps.weaponType);
	//			if(ps.ProID == 3){
	//				if(ps.weaponType == 1){
	//					ps.weaponType = 2;
	//				}
	//				if(ps.weaponType == 2){
	//					ps.weaponType = 1;
	//				}
	//			}
	//			print(ps.ProID + " =1= " + ps.weaponType);
				ps.ChangeWeapons(PES[10].inv.WeaponType);			
			}
			switch(ps.weaponType){
				case PlayerWeaponType.empty : 
					str = "quantou" ;
					break;
				case PlayerWeaponType.weapon1 :
						switch(ps.ProID){
							case 1 : 
							str = "UIM_Anti-War_N";
							break;
							case 2 : 
							str = "UIM_Robber_O";
							break;
							case 3 : 
							str = "UIM_Necromancer_N ";
							break; 
						}
					break;
				case PlayerWeaponType.weapon2 :
						switch(ps.ProID){
							case 1 : 
							str = "UIM_Violent-War_N";
							break;
							case 2 : 
							str = "UIM_Ranger_N ";
							break;
							case 3 : 
							str = "UIM_Master_N";
							break; 
						}
					break;
			} 
	//		//print(str + " =================str");
			SpriteWeapon.spriteName = str;
	//		SpriteWeapon1.spriteName = str;
		}
	}

	//enum SlotType 	{Helmet=1,Breastplate=2,Spaulders=3,Gauntlets=4,Leggings=5,Rear=6,Ring=7,Collar=8,Belt=9,Weapon1=10,Weapon2=11,Hand, Chest, Wrist,Expendable,Empty,Bag}
	//pes shuxing		attr xiu zheng			item xiu zheng
	//0 : gongji    	0:nai li				0:toukui
	//1 : hujia			1:li liang				1:xiongjia
	//2 : fangyu		2:min jie				2:jianjia
	//3 : zhunque		3:zhi li				3:hushou
	//4 : baoji			4:bao ji				4:tuijia
	//5 : naili			5:zhun que				5:houbei
	//6 : liliang		6:fang yu zhi			6:jiezhi
	//7 : minjie		7:mo fa kang xing		7:bozi
	//8 : zhili			8:zui da mo fa			8:yaodai
	//9 : huifusudu		9:hui fu neng li		9:wuqi1
	//10 : Matk		10:e wai shanghai		
	//11 : Mdef 		
	//12 : Mana			

	var EquipStatus : int[];
	var attrXiuZheng : float[];
	var itemXiuZheng : float[];
	var usePes : PersonEquipment;
	private var EquipBei : float;
	function GetEquipStatus(pes : PersonEquipment , useI : int){
		usePes = pes;
		var itemType : SlotType = pes.invType;
		var weaponType : PlayerWeaponType = pes.inv.WeaponType;
		var pfType : ProfessionType = pes.inv.professionType;
		var level : float = pes.inv.itemLevel;
		var quality : float = pes.inv.itemQuality;
		var endurance : float = pes.inv.itemEndurance;
		var proAbt : float = pes.inv.itemProAbt;
		var abt1 : String = pes.inv.itemAbt1;
		var abt2 : String = pes.inv.itemAbt2;
		var abt3 : String = pes.inv.itemAbt3;
		var hAttr1 : HoleAttr = pes.inv.holeAttr1;
		var hAttr2 : HoleAttr = pes.inv.holeAttr2;
		var hAttr3 : HoleAttr = pes.inv.holeAttr3;
		
		if(useI == 11){
			EquipBei = 5.0;
		}else{
			EquipBei = 1.0;
		}
		var intTen : int = 0;
		intTen = AllResources.InvmakerStatic.GetInvFenMuAsQuality(pes.inv);
		
		var pinzhi : float = 0;
		pinzhi = getQuality(quality , level);	

		if(weaponType == PlayerWeaponType.weapon1 || weaponType == PlayerWeaponType.weapon2){
			if(weaponType == PlayerWeaponType.weapon1 && pfType == ProfessionType.Soldier ){
				EquipStatus[0] += (0.56 * pinzhi + 40) / EquipBei;
				EquipStatus[1] += ((0.6*pinzhi + 40)*1.2) / EquipBei;		
				EquipStatus[0] = Mathf.Clamp(EquipStatus[0] , 1 , 999999);
				EquipStatus[1] = Mathf.Clamp(EquipStatus[1] , 1 , 999999);
			}else{
				EquipStatus[0] += (0.68 * pinzhi + 56) / EquipBei;			
				EquipStatus[0] = Mathf.Clamp(EquipStatus[0] , 1 , 999999);
			}
		}else{
			if(pfType == ProfessionType.Master){
				EquipStatus[1] += ((0.4 * pinzhi + 12) *itemXiuZheng[itemType - 1]) / EquipBei;	
				EquipStatus[1] = Mathf.Clamp(EquipStatus[1] , 1 , 999999);
			}else
			if(pfType == ProfessionType.Robber){
				EquipStatus[1] += ((0.6 * pinzhi + 26) *itemXiuZheng[itemType - 1]) / EquipBei;	
				EquipStatus[1] = Mathf.Clamp(EquipStatus[1] , 1 , 999999);
			}else
			if(pfType == ProfessionType.Soldier){
				EquipStatus[1] += ((0.8 * pinzhi + 40) *itemXiuZheng[itemType - 1]) / EquipBei;	
				EquipStatus[1] = Mathf.Clamp(EquipStatus[1] , 1 , 999999);
			}
		}
		
		EquipStatus[5] += (pinzhi * endurance / intTen * 1.5 * itemXiuZheng[itemType - 1]) / EquipBei;
				EquipStatus[5] = Mathf.Clamp(EquipStatus[5] , 1 , 999999);
//		print(EquipStatus[5] + " =a= " + pinzhi + " =b= " + endurance + " =c= " + itemType + " =d= " + itemXiuZheng[itemType- 1]);
		switch(pfType){
			case ProfessionType.Soldier : 		
				EquipStatus[6] += (pinzhi * proAbt / intTen  * itemXiuZheng[itemType - 1]) / EquipBei;  
				EquipStatus[6] = Mathf.Clamp(EquipStatus[6] , 1 , 999999);
				break;
			case ProfessionType.Robber : 
				EquipStatus[7] += (pinzhi * proAbt / intTen * itemXiuZheng[itemType - 1]) / EquipBei; 
				EquipStatus[7] = Mathf.Clamp(EquipStatus[7] , 1 , 999999);
			 break;
			case ProfessionType.Master : 
				EquipStatus[8] += (pinzhi * proAbt / intTen * itemXiuZheng[itemType - 1]) / EquipBei;  
				EquipStatus[8] = Mathf.Clamp(EquipStatus[8] , 1 , 999999);
				break;
		}
	//	getAtb(abt1 , pinzhi + parseInt( pes.inv.itemBuild), itemType , pfType);
	//	getAtb(abt2 , pinzhi + parseInt( pes.inv.itemBuild), itemType , pfType);
	//	getAtb(abt3 , pinzhi + parseInt( pes.inv.itemBuild), itemType , pfType); 
		getAtb(abt1 , pes.inv.ATabt1.iValue, itemType , pfType);
		getAtb(abt2 , pes.inv.ATabt2.iValue, itemType , pfType);
		getAtb(abt3 , pes.inv.ATabt3.iValue, itemType , pfType); 
		getHole(hAttr1 , pes.inv.itemHole1);
		getHole(hAttr2 , pes.inv.itemHole2);
		getHole(hAttr3 , pes.inv.itemHole3);
	}

	function getHole(hAttr : HoleAttr , hole : String){ 
		if(hole == "00"){
			return;
		}
		var holeValue : int = 0;
		for(var rows : yuan.YuanMemoryDB.YuanRow in GameItem.Rows){
			var str : String;
			str = "81" + parseInt(hAttr.hType).ToString() + hAttr.hValue.ToString();
			if(rows["ItemID"].YuanColumnText == str){
				holeValue = parseInt(rows["ItemValue"].YuanColumnText);
			}
		}
	//	//print(holeValue + " == " + hAttr.hType + " == " + EquipBei);
		switch(hAttr.hType){
			case holeType.atk :
				 EquipStatus[0] += (holeValue) / EquipBei; break;
			case holeType.zhuanzhu :
				 EquipStatus[3] += (holeValue) / EquipBei;  break;
			case holeType.baoji :
				 EquipStatus[4] += (holeValue) / EquipBei;  break;
			case holeType.def :
				 EquipStatus[2] += (holeValue) / EquipBei; break;
			case holeType.mokang :
				 EquipStatus[11] += (holeValue) / EquipBei; break;
		}
	}

	function getAtb(str : String , pin : int , type : SlotType , pfType : ProfessionType){
		var a1 : int = parseInt(str.Substring(0,1)); 
		var a2 : int = parseInt(str.Substring(1,1));  
	//	//print(a1  + " == a1");
		pin /= EquipBei;
		switch(a1){
			case 1: EquipStatus[4] += pin; break;
			case 2: EquipStatus[3] += pin; break;
			case 3: EquipStatus[2] += pin; break;
			case 4: 
				if(pfType == ProfessionType.Soldier) 
					EquipStatus[0] += pin; else
				if(pfType == ProfessionType.Robber) 
					EquipStatus[0] += pin; else
				if(pfType == ProfessionType.Master) {
					EquipStatus[0] += pin ; 
				}
				break;
			case 5: EquipStatus[11] += pin ; break;
			case 6: EquipStatus[12] += pin ; break;
			case 7: EquipStatus[9] += pin; break;
			case 8: 
				if(pfType == ProfessionType.Soldier) 
					EquipStatus[6] += pin; else
				if(pfType == ProfessionType.Robber) 
					EquipStatus[7] += pin; else
				if(pfType == ProfessionType.Master) 
					EquipStatus[8] += pin;
				break;
			case 9: 
				if(pfType == ProfessionType.Soldier) 
					EquipStatus[7] += pin ; else
				if(pfType == ProfessionType.Robber) 
					EquipStatus[6] += pin; else
				if(pfType == ProfessionType.Master) 
					EquipStatus[7] += pin ;
				break;
		}
	}


	function getQuality(qua : int , lv : int):int{
		if(qua >= 6){
			qua -= 4;
		}
		switch(qua){
			case 1: return lv*2;
			case 2: return lv*3+6;
			case 3: return lv*7+12;
			case 4: return lv*12+24;
			case 5: return lv*15+56;
		}
	//	switch(qua){
	//		case 1: return lv*2;
	//		case 2: return lv*4+2;
	//		case 3: return lv*6+12;
	//		case 4: return lv*8+24;
	//		case 5: return lv*9+56;
	//	}
	}

	var bagParent : Transform;
	function ItemInfoOn(){
		bagParent.localPosition.y = 1000;
	}

	var LabelExp : UILabel;
	var LabelHp : UILabel;
	var LabelMana : UILabel;
	var LabelAtk : UILabel;
	var LabelAtkM : UILabel;
	var LabelBaoJi : UILabel;
	var LabelJingZhun : UILabel;
	var LabelHuJia : UILabel;
	var LabelFangYu : UILabel;
	var LabelShanBi : UILabel;
	var LabelFangYuMo : UILabel;
	var LabelPianXie : UILabel; 

	var LabelNaiLi : UILabel;
	var LabelLiLiang : UILabel;
	var LabelMinJie : UILabel;
	var LabelZhiLi : UILabel; 
	var LabelZhuanZhu : UILabel; 
	var LabelCombat : UILabel;
	var BarExp : UIFilledSprite;

	var PlayerInfoRightOtherLabelHuJia : UILabel;
	var PlayerInfoRightOtherLabelFangYuMo : UILabel;
	var PlayerInfoRightOtherLabelAtk : UILabel;
	var PlayerInfoRightOtherLabelAtkM : UILabel;
	var PlayerInfoRightOtherLabelLevel : UILabel;
	var PlayerInfoRightOtherLabelhp : UILabel;
	var PlayerInfoRightOtherLabelnv : UILabel;
	var PlayerInfoRightOtherLabelName : UILabel;
	var PlayerInfoRightOtherLabelVIP : UILabel;
	var PlayerInfoRightOtherLabelHealthP : UILabel;
	var PlayerInfoRightOtherLabelPrestige : UILabel;
	var PlayerInfoRightOtherLabelPVPPoint : UILabel;
	
	var OtherLabelConquest : UILabel;
	var OtherLabelHero : UILabel;
	
	public var ComBatLabel : int;

	private var Pfloat1 : float;
	private var Pfloat2 : float;
	private var Pfloat3 : float;
	private var Pfloat4 : float;
	var fsHP : UIFilledSprite;
	var fsNU : UIFilledSprite;
	var LabelExpNew1 : UILabel;
	var LabelExpNew2 : UILabel;
	var fsEXP1 : UIFilledSprite;
	var fsEXP2 : UIFilledSprite;

	function showPlayerInfo(){
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		ComBatLabel = ps.GetCombat();
		if(ps != null && LabelExp != null){
			var pCombat : int = ps.GetCombat();
			
			var f1 : float;
			var f2 : float; 
			f1 =  ps.getNowExp();  f2 = ps.getNextExp();
			BarExp.fillAmount = f1/f2;
			LabelZhuanZhu.text = (parseInt( ps.AllFocus)+ addint[0]).ToString();
			LabelExp.text = String.Format("{0}/{1}",f1+addint[1], f2);
			LabelHp.text = String.Format("{0}/{1}",ps.Health, ps.Maxhealth);
			LabelMana.text = String.Format("{0}/{1}",ps.Mana,  ps.Maxmana);
			LabelAtk.text =  (parseInt(ps.MaxATK)+ addint[2]).ToString();
			LabelAtkM.text =  (parseInt(ps.MaxATK)+ addint[3]).ToString();
			LabelBaoJi.text =  (parseInt(ps.Crit)+ addint[4]).ToString();
			LabelJingZhun.text =  (parseInt(ps.Retrieval)+ addint[5]).ToString();
			LabelHuJia.text =   (parseInt(ps.Armor)+ addint[6]).ToString();
			var ARM =parseInt(ps.Armor);
			LabelFangYu.text =  (ARM*10000/( ARM +400+80* parseInt(ps.Level))+  ps.extraDefense).ToString();
			LabelShanBi.text =  (parseInt(ps.Dodge)+ addint[7]).ToString();
			LabelFangYuMo.text = (ARM*4000/( ARM +400+60* parseInt(ps.Level))+  ps.extraResist).ToString() ;
			LabelPianXie.text = (parseInt(ps.Distortion)+ addint[8]).ToString();

			LabelNaiLi.text =  (parseInt(ps.AllStamina)+ addint[9]).ToString();
			LabelLiLiang.text =  (parseInt(ps.AllStrength)+ addint[10]).ToString();
			LabelMinJie.text =  (parseInt(ps.AllAgility)+ addint[11]).ToString();
			LabelZhiLi.text =  (parseInt(ps.AllIntellect)+ addint[12]).ToString();
			LabelCombat.text =  pCombat.ToString();
			
			SeOtherLabelZhuanZhu.text = LabelZhuanZhu.text;
			SeOtherLabelHp.text = String.Format("{0}/{1}",ps.Health, ps.Maxhealth); 
			SeOtherLabelMana.text = String.Format("{0}/{1}",ps.Mana,  ps.Maxmana); 
			SeOtherLabelAtk.text =  LabelAtk.text;
			SeOtherLabelAtkM.text =  LabelAtk.text;
			SeOtherLabelBaoJi.text =  LabelBaoJi.text;
			SeOtherLabelJingZhun.text =  LabelJingZhun.text;
			SeOtherLabelHuJia.text =  LabelHuJia.text;
			SeOtherLabelFangYu.text = ( ARM*10000/( ARM +400+80* parseInt(ps.Level))+  ps.extraDefense).ToString();
			SeOtherLabelShanBi.text =  LabelShanBi.text;
			SeOtherLabelFangYuMo.text =( ARM*4000/( ARM +400+60* parseInt(ps.Level))+  ps.extraResist).ToString();
			SeOtherLabelPianXie.text = LabelPianXie.text;

			SeOtherLabelNaiLi.text =  LabelNaiLi.text;
			SeOtherLabelLiLiang.text =  LabelLiLiang.text;
			SeOtherLabelMinJie.text =  LabelMinJie.text;
			SeOtherLabelZhiLi.text =  LabelZhiLi.text;

			PlayerInfoOtherLabelZhuanZhu.text =  LabelZhuanZhu.text;
			PlayerInfoOtherLabelHp.text = String.Format("{0}/{1}",ps.Health, ps.Maxhealth); 
			PlayerInfoOtherLabelMana.text = String.Format("{0}/{1}",ps.Mana,  ps.Maxmana); 
			PlayerInfoOtherLabelAtk.text =  LabelAtk.text;
			PlayerInfoOtherLabelAtkM.text =  LabelAtk.text;
			PlayerInfoOtherLabelBaoJi.text =  LabelBaoJi.text;
			PlayerInfoOtherLabelJingZhun.text =  LabelJingZhun.text;
			PlayerInfoOtherLabelHuJia.text =   LabelHuJia.text;
			PlayerInfoOtherLabelFangYu.text = ( ARM*10000/( ARM +400+80* parseInt(ps.Level))+  ps.extraDefense).ToString();
			PlayerInfoOtherLabelShanBi.text =  LabelShanBi.text;
			PlayerInfoOtherLabelFangYuMo.text =( ARM*4000/( ARM +400+60* parseInt(ps.Level))+  ps.extraResist).ToString();
			PlayerInfoOtherLabelPianXie.text = LabelPianXie.text;

			PlayerInfoOtherLabelNaiLi.text =  LabelNaiLi.text;
			PlayerInfoOtherLabelLiLiang.text =  LabelLiLiang.text;
			PlayerInfoOtherLabelMinJie.text =  LabelMinJie.text;
			PlayerInfoOtherLabelZhiLi.text =  LabelZhiLi.text;
			PlayerInfoOtherLabelCombat.text = pCombat.ToString();
			
			PlayerInfoRightOtherLabelHuJia.text =   LabelHuJia.text;
			PlayerInfoRightOtherLabelFangYuMo.text =( ARM*4000/( ARM +400+60* parseInt(ps.Level))+  ps.extraResist).ToString();
			PlayerInfoRightOtherLabelAtk.text =  LabelAtk.text;
			PlayerInfoRightOtherLabelAtkM.text =  LabelAtk.text;
			
			Pfloat1 = parseInt(ps.Health);
			Pfloat2 = parseInt(ps.Maxhealth);
			Pfloat3 = parseInt(ps.Mana);
			Pfloat4 = parseInt(ps.Maxmana);
			
			OtherLabelConquest.text = yt.Rows[0]["ConquerStone"].YuanColumnText;
			OtherLabelHero.text = yt.Rows[0]["HoreStone"].YuanColumnText;
			
					
			if(fsHP.fillAmount != Pfloat1 / Pfloat2){
				fsHP.fillAmount = Pfloat1 / Pfloat2;		
				PlayerInfoRightOtherLabelhp.text = String.Format("{0}/{1}",ps.Health, ps.Maxhealth); 
			}
			if(fsNU.fillAmount != Pfloat3 / Pfloat4){
				fsNU.fillAmount = Pfloat3 / Pfloat4;		
				PlayerInfoRightOtherLabelnv.text = String.Format("{0}/{1}",ps.Mana, ps.Maxmana); 
			}

			if(PlayerInfoRightOtherLabelLevel.text != ps.Level.ToString()){
				PlayerInfoRightOtherLabelLevel.text = ps.Level.ToString();		
			}
			if(PlayerInfoRightOtherLabelName.text != ps.PlayerName){
				PlayerInfoRightOtherLabelName.text = ps.PlayerName;
			}
			
			if(PlayerInfoRightOtherLabelVIP.text != ps.VIPLevel.ToString()){
				PlayerInfoRightOtherLabelVIP.text = ps.VIPLevel.ToString();
			}
			if(PlayerInfoRightOtherLabelHealthP.text != ps.Power){
				PlayerInfoRightOtherLabelHealthP.text = ps.Power;
			}
			
			if(PlayerInfoRightOtherLabelPrestige.text != ps.Prestige){
	//			//print(Time.time);
				PlayerInfoRightOtherLabelPrestige.text = ps.Prestige;		
			}
			if(PlayerInfoRightOtherLabelPVPPoint.text != ps.PVPPoint){
	//			//print(Time.time);
				PlayerInfoRightOtherLabelPVPPoint.text = ps.PVPPoint;		
			}
			
			if(LabelExpNew1.text.ToString() != LabelExp.text.ToString()){
				LabelExpNew1.text = LabelExp.text.ToString();
				fsEXP1.fillAmount = f1/f2;
				LabelExpNew2.text = LabelExp.text.ToString();
				fsEXP2.fillAmount = f1/f2;			
			}
			
		}
	}

	var SeOtherLabelHp : UILabel;
	var SeOtherLabelMana : UILabel;
	var SeOtherLabelAtk : UILabel;
	var SeOtherLabelAtkM : UILabel;
	var SeOtherLabelBaoJi : UILabel;
	var SeOtherLabelJingZhun : UILabel;
	var SeOtherLabelHuJia : UILabel;
	var SeOtherLabelFangYu : UILabel;
	var SeOtherLabelShanBi : UILabel;
	var SeOtherLabelFangYuMo : UILabel;
	var SeOtherLabelPianXie : UILabel; 
	var SeOtherLabelNaiLi : UILabel;
	var SeOtherLabelLiLiang : UILabel;
	var SeOtherLabelMinJie : UILabel;
	var SeOtherLabelZhiLi : UILabel; 
	var SeOtherLabelZhuanZhu : UILabel; 

	var PlayerInfoOtherLabelHp : UILabel;
	var PlayerInfoOtherLabelMana : UILabel;
	var PlayerInfoOtherLabelAtk : UILabel;
	var PlayerInfoOtherLabelAtkM : UILabel;
	var PlayerInfoOtherLabelBaoJi : UILabel;
	var PlayerInfoOtherLabelJingZhun : UILabel;
	var PlayerInfoOtherLabelHuJia : UILabel;
	var PlayerInfoOtherLabelFangYu : UILabel;
	var PlayerInfoOtherLabelShanBi : UILabel;
	var PlayerInfoOtherLabelFangYuMo : UILabel;
	var PlayerInfoOtherLabelPianXie : UILabel; 
	var PlayerInfoOtherLabelNaiLi : UILabel;
	var PlayerInfoOtherLabelLiLiang : UILabel;
	var PlayerInfoOtherLabelMinJie : UILabel;
	var PlayerInfoOtherLabelZhiLi : UILabel; 
	var PlayerInfoOtherLabelCombat : UILabel; 
	var PlayerInfoOtherLabelZhuanZhu : UILabel; 

	var OtherLabelHp : UILabel;
	var OtherLabelMana : UILabel;
	var OtherLabelAtk : UILabel;
	var OtherLabelAtkM : UILabel;
	var OtherLabelBaoJi : UILabel;
	var OtherLabelJingZhun : UILabel;
	var OtherLabelHuJia : UILabel;
	var OtherLabelFangYu : UILabel;
	var OtherLabelShanBi : UILabel;
	var OtherLabelFangYuMo : UILabel;
	var OtherLabelPianXie : UILabel; 
	var OtherLabelNaiLi : UILabel;
	var OtherLabelLiLiang : UILabel;
	var OtherLabelMinJie : UILabel;
	var OtherLabelZhiLi : UILabel; 
	var OtherLabelZhuanZhu : UILabel; 
	function OthershowPlayerInfo(){
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(ps != null){
			OtherLabelZhuanZhu.text =  GetiLabelYanSe(ps.OtherFocus , ps.Focus) + ps.OtherFocus + "";
			OtherLabelHp.text = ps.Health + "/" + GetiLabelYanSe(parseInt(ps.OtherMaxhealth),parseInt(ps.Maxhealth)) + ps.OtherMaxhealth;
			OtherLabelMana.text = ps.Mana + "/" + ps.Maxmana;
			OtherLabelAtk.text =  GetiLabelYanSe(parseInt(ps.OtherATK),parseInt(ps.MaxATK)) + ps.OtherATK + "" ;
			OtherLabelAtkM.text =  GetiLabelYanSe(parseInt(ps.OtherATK),parseInt(ps.MaxATK)) + ps.OtherATK + "" ;
			OtherLabelBaoJi.text =  GetiLabelYanSe(ps.OthersCrit,parseInt(ps.Crit)) + ps.OthersCrit + "" ;
			OtherLabelJingZhun.text =  GetiLabelYanSe(parseInt(ps.OthersRetrieval),parseInt(ps.Retrieval)) + ps.OthersRetrieval + "" ;
			OtherLabelHuJia.text =   GetiLabelYanSe(ps.OthersArmor,parseInt(ps.Armor)) + ps.OthersArmor + "" ;
			
			var ARM = parseInt(ps.OthersArmor);
			var fang1 : int = 0;
			var fangMo1 : int = 0;
			fang1 = ARM*10000/( ARM +400+80* parseInt(ps.Level))+  ps.extraDefense;
			fangMo1 = ARM*4000/( ARM +400+60* parseInt(ps.Level))+  ps.extraResist;
			
			ARM = parseInt(ps.Armor);
			var fang2 : int = 0;
			var fangMo2 : int = 0;
			fang2 = ARM*10000/( ARM +400+80* parseInt(ps.Level))+  ps.extraDefense;
			fangMo2 = ARM*4000/( ARM +400+60* parseInt(ps.Level))+  ps.extraResist;
			
			OtherLabelFangYuMo.text = GetiLabelYanSe(fangMo1,fangMo2) + fangMo1 + "" ;
			OtherLabelFangYu.text =  GetiLabelYanSe(fang1,fang2) + fang1 + "" ;
			
			OtherLabelShanBi.text =  GetiLabelYanSe(ps.OthersDodge,ps.Dodge) + ps.OthersDodge + "" ;
			OtherLabelPianXie.text = GetiLabelYanSe(ps.OthersDistortion,ps.Distortion) + ps.OthersDistortion + "" ;

			OtherLabelNaiLi.text =  GetiLabelYanSe(ps.OtherStamina,ps.Stamina) + ps.OtherStamina + "" ;
			OtherLabelLiLiang.text =  GetiLabelYanSe(ps.OtherStrength,ps.Strength) + ps.OtherStrength + ""  ;
			OtherLabelMinJie.text =  GetiLabelYanSe(ps.OtherAgility,ps.Agility) + ps.OtherAgility + ""  ;
			OtherLabelZhiLi.text =  GetiLabelYanSe(ps.OtherIntellect,ps.Intellect) + ps.OtherIntellect  + "" ;
		}
	}

	function GetiLabelYanSe(str1 : int  , str2 : int) : String{
		if(str1 > str2){
			return "[7fff00]";
		}else
		if(str1 < str2){
			return "[ff0000]";
		}else{
			return "";		
		}
	}

	function AddBagItem2(inv : InventoryItem){
		var youKong : boolean = false;
		if(!youKong){
			SelectNoOpenBag1();
			youKong = jianchabaoguoForXiaoHao(inv);
		} 
		if(!youKong){
			SelectNoOpenBag2();
			youKong = jianchabaoguoForXiaoHao(inv);
		} 
		if(!youKong){
			SelectNoOpenBag3();
			youKong = jianchabaoguoForXiaoHao(inv);
		} 
		if(!youKong){
			SelectNoOpenBag4();
			youKong = jianchabaoguoForXiaoHao(inv);
		} 
		if(!youKong){
			AllManage.tsStatic.Show("tips030");
			return;
		}	
		var i : int = 0; 
		var j : int = 0;
		if(inv.slotType < 12){	
			for(i=0; i<bagInventorys.length; i++){ 
				if(bagInventorys[i] == null){
					bagInventorys[i] = inv;
					UpdateBagItem();
					return;
				}
			}
		}else{
			for(i=0; i<bagInventorys.length; i++){
				if(bagInventorys[i] != null){
					if(bagInventorys[i].itemID.Length > 5 && bagInventorys[i].consumablesNum != 0){
						if(bagInventorys[i].itemID.Substring(0,5) == inv.itemID.Substring(0,5)&& bagInventorys[i].consumablesNum != 20 ){
							if(bagInventorys[i].consumablesNum + inv.consumablesNum <= 20){		
								bagInventorys[i].consumablesNum += inv.consumablesNum; 
								if(bagInventorys[i].consumablesNum < 10){
									bagInventorys[i].itemID =  bagInventorys[i].itemID.Substring(0,5) + "0" +  bagInventorys[i].consumablesNum.ToString() + ","+ bagInventorys[i].itemID.Substring(8,1); 						
								}else{
										bagInventorys[i].itemID = bagInventorys[i].itemID.Substring(0,5) + bagInventorys[i].consumablesNum.ToString() + ","+ bagInventorys[i].itemID.Substring(8,1);										
								}
//								BagIt[i].showConsumablesNum();
								UpdateBagItem();
								mtw.JianChaItem(bagInventorys[i].itemID);
								return;
							}else{
								var useNum : int = bagInventorys[i].consumablesNum;
								bagInventorys[i].consumablesNum =20; 
								bagInventorys[i].itemID = bagInventorys[i].itemID.Substring(0,5) + bagInventorys[i].consumablesNum.ToString() + ","+ bagInventorys[i].itemID.Substring(8,1);	
//								BagIt[i].showConsumablesNum(); 
								UpdateBagItem(); 
								var useInv : InventoryItem = inv;
								useInv.consumablesNum = useNum + inv.consumablesNum - 20;
								if(jianchabaoguoForXiaoHao(useInv)){
									for(j=0; j<bagInventorys.length; j++){
										if(bagInventorys[j] == null){ 
											inv.consumablesNum = useNum + inv.consumablesNum - 20;
											bagInventorys[j] = inv;
											UpdateBagItem();
											mtw.JianChaItem(bagInventorys[j].itemID);
											return;					
										}
									}		
								}else{
									return;
								}
							}
						}
					}
				}
			}
			for(i=0; i<bagInventorys.length; i++){ 
				if(bagInventorys[i] == null){  
					 bagInventorys[i] = inv;
//					BagIt[i].showConsumablesNum();
					UpdateBagItem();
					mtw.JianChaItem(bagInventorys[i].itemID);
					return;					
				}
			}
		}
	}

	function CanSkillAsID(type : int , EquepmentID : int) : boolean{
		if(EquipIt[0].inv != null && EquepmentID != EquipIt[0].EquepmentID){
			if(EquipIt[0].inv.WeaponType == type){
				return true;
			}
		}
		if(EquipIt[1].inv != null && EquepmentID != EquipIt[0].EquepmentID){
			if(EquipIt[1].inv.WeaponType == type){
				return true;
			}
		}
		return false;
	}
	function CanSkillAsID(type : int) : boolean{
		if(Application.loadedLevelName == "Map200")
			return true;
		var useInt : int = 0;
		useInt = parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText); 
		if(ps.ProID == 3){
			if(type == 1){
				type = 2;
			}else
			if(type == 2){
				type = 1;
			}
			if(useInt == 1){
				useInt = 2;
			}else
			if(useInt == 2){
				useInt = 1;
			}		
		}
		if(EquipIt[0]){
			if(EquipIt[0].inv != null){
				if(EquipIt[0].inv.WeaponType == type && (useInt == type || useInt == 3)){
					return true;
				}
			}
		}else{
			if(Equeipitems[0] != null){
				if(Equeipitems[0].WeaponType == type && (useInt == type || useInt == 3)){
					return true;
				}
			}
		}
		
		if(EquipIt[0]){
			if(EquipIt[1].inv != null){
				if(EquipIt[1].inv.WeaponType == type && (useInt == type || useInt == 3)){
					return true;
				}
			}
		}else{
			if(Equeipitems[1] != null){
				if(Equeipitems[1].WeaponType == type && (useInt == type || useInt == 3)){
					return true;
				}
			}		
		}
		return false;
	}

	function CanRemoveSkillAsID(type : int) : boolean{
		if(Application.loadedLevelName == "Map200")
			return false;
			
		var useInt : int = 0;
		useInt = parseInt(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText); 
		if(ps.ProID == 3){
			if(type == 1){
				type = 2;
			}else
			if(type == 2){
				type = 1;
			}
			if(useInt == 1){
				useInt = 2;
			}else
			if(useInt == 2){
				useInt = 1;
			}		
		}
		if(useInt != 3){
			if(useInt != type){
				return true;
			}
		}
		
		return false;
	}	

	var opBox : OpenBox;
	function openBox(strs : String[]){
	    var inv1 : InventoryItem = null;
	    var inv2 : InventoryItem = null;
	    var inv3 : InventoryItem = null;
	    var inv4 : InventoryItem = null; 
	    var Igold : int;
	    var iblood : int;
	    Igold = parseInt(strs[0]);
	    iblood = parseInt(strs[1]);
	    var newStr : String = "";
	    var useStr : String = "";
	    if(strs[2] != ""){
	        useStr = strs[2].Split(Dstr.ToCharArray())[0];
	        if(useStr.Length < 2 && ( useStr == "2" || useStr == "3" )){
	            newStr = strs[2];
	        }else{
	            inv1 = AllResources.InvmakerStatic.GetItemInfo(strs[2] , inv1);			
	        }
	    }
	    if(strs[3] != ""){
	        useStr = strs[3].Split(Dstr.ToCharArray())[0];
	        if(useStr.Length < 2 && ( useStr == "2" || useStr == "3" )){
	            newStr = strs[3];
	        }else{
	            inv2 = AllResources.InvmakerStatic.GetItemInfo(strs[3] , inv2);
	        }
	    }
	    if(strs[4] != ""){
	        useStr = strs[4].Split(Dstr.ToCharArray())[0];
	        if(useStr.Length < 2 && ( useStr == "2" || useStr == "3" )){
	            newStr = strs[4];
	        }else{
	            inv3 = AllResources.InvmakerStatic.GetItemInfo(strs[4] , inv3);
	        }
	    }
	    if(strs[5] != ""){
	        useStr = strs[5].Split(Dstr.ToCharArray())[0];
	        if(useStr.Length < 2 && ( useStr == "2" || useStr == "3" )){
	            newStr = strs[5];
	        }else{
	            inv4 = AllResources.InvmakerStatic.GetItemInfo(strs[5] , inv4);
	        }
	    }
	    opBox.open(2,Igold,iblood,inv1,inv2,inv3,inv4 , newStr);
	}


	function reTitle(){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(ps != null){
			ps.reTitle();
		}
	}

	function ShowLiangKuang(iv : InventoryItem){
		for(var i=0; i<EquipIt.length ; i++){
				if(EquipIt[i] && EquipIt[i].myType == iv.slotType){
					EquipIt[i].liangKuang.enabled = true;
				}
		}
	}

	function CloseLiangKuang(){
		for(var i=0; i<EquipIt.length ; i++){
			if(EquipIt[i] && EquipIt[i].liangKuang.enabled){
				EquipIt[i].liangKuang.enabled = false;
			}
		}
	}

	function SelectNoOpenBag1(){
		BagID = 1;  
		ShowBagButton(BagID);
		ClearBagItem();
		SetSelectBagItem(Inventory1);
	} 

	function SelectNoOpenBag2(){
		if(PlayerInventoryNum >=2){
			BagID = 2;
			ShowBagButton(BagID);
			ClearBagItem();
			SetSelectBagItem(Inventory2); 
		}
	//	//print(Inventory2 + " == ");
	} 
	function SelectNoOpenBag3(){
		if(PlayerInventoryNum >=3){
			BagID = 3;
			ShowBagButton(BagID);
			ClearBagItem();
			SetSelectBagItem(Inventory3);
		}
	} 
	function SelectNoOpenBag4(){
		if(PlayerInventoryNum >=4){
			BagID = 4;
			ShowBagButton(BagID);
			ClearBagItem();
			SetSelectBagItem(Inventory4);
		}
		

	}

	function Show16()
	{
		AllManage.UIALLPCStatic.show16();
	}

	function MakeInvItemRandom(objs : Object[]){
	    AllResources.staticLT.MakeItemRandom(objs);
	}

	function ShowInvItemRandom(objs : Object[]){
	    AllResources.staticLT.ShowItemRandom(objs);
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//RideItemStr
	function AddNewRideItemAsID(id : String){
		var i : int = 0;
		var num : int = 0;
		var can : boolean = false;
		UseRideIDs = RideItemStr.Split(Fstr.ToCharArray());
		for(i=0; i<UseRideIDs.length; i++){	
			if(UseRideIDs[i].Length > 4){
				if(UseRideIDs[i].Substring(0,4) == id.Substring(0,4)){
					num = parseInt(UseRideIDs[i].Substring(5,2)) + parseInt(id.Substring(5,2));
					if(num > 9){
						UseRideIDs[i] = UseRideIDs[i].Substring(0,4) + "," + num.ToString();
					}else
					if(num > 0){
						UseRideIDs[i] = UseRideIDs[i].Substring(0,4)  + "," + "0" + num.ToString();
					}else{
						UseRideIDs[i] = "";
					}
					can = true;
				}
			}
		}
		if(!can){
			var useD : String[];
			useD = UseRideIDs;	
			UseRideIDs = new Array(useD.length + 1);
			for(i=0; i<useD.length; i++){
				UseRideIDs[i] = useD[i];
			}
			UseRideIDs[UseRideIDs.length - 1] = id+";";
		}
		RideItemStr = "";
		for(i=0; i<UseRideIDs.length; i++){
			if(UseRideIDs[i].Length > 2){
				RideItemStr += UseRideIDs[i] + ";";
			}
		}
		yt.Rows[0]["Mounts"].YuanColumnText = RideItemStr;
		ClearRideObj();
		SetRideItem();
	}

	var UseRideIDs : String[];
	function SetRideItem(){
		var i : int = 0;
		UseRideIDs = RideItemStr.Split(Fstr.ToCharArray());
		for(i=0; i<UseRideIDs.length; i++){	 
			if(UseRideIDs[i] != ""){
				SetRealRideItemAsID(UseRideIDs[i]);
			}
		}
	}

	var RideGrid : UIGrid;
	var RidePre : GameObject;
	var ObjRideArray : GameObject[];
	function SetRealRideItemAsID(id : String){
		if( !RideGrid)
		    return;
		for(var rows : yuan.YuanMemoryDB.YuanRow in PlayerPet.Rows){
			if(id.Length > 4){
				if(rows["ItemID"].YuanColumnText == id){
					var RideCom : ItemRide;
					var Obj : GameObject = Instantiate(RidePre); 
					Obj.transform.parent = RideGrid.transform;
					Obj.transform.localPosition.z = 0;
					RideCom = Obj.GetComponent(ItemRide);
					RideCom.AddRideObj(rows["ItemID"].YuanColumnText , rows["Name"].YuanColumnText , rows["ItemInfo"].YuanColumnText , rows["MyLevel"].YuanColumnText , this);
					AddNewRideInArray(Obj);
				}
			}
		}
	    yield;
	    
		RideGrid.repositionNow = true;
	}

	function ResetGridPanel()
	{
	    RideGrid.transform.parent.localPosition = Vector3.zero;

	    var panel : UIPanel = RideGrid.transform.parent.GetComponent(UIPanel);
	    panel.clipOffset = Vector2.zero;

	    yield WaitForSeconds (1.0f);

	    RideGrid.transform.parent.localPosition = Vector3.zero;
	    panel.clipOffset = Vector2.zero;

	    RideGrid.repositionNow = true;
	}

	function ClearRideObj(){
		for(var i=0; i<ObjRideArray.length; i++){
			Destroy(ObjRideArray[i]);
		}
		ObjRideArray = new Array();
	}

	function AddNewRideInArray(obj : GameObject){
		var useArray : GameObject[];
		useArray = ObjRideArray;
		ObjRideArray = new Array(ObjRideArray.length + 1);
		for(var i=0; i<ObjRideArray.length-1; i++){
			ObjRideArray[i] = useArray[i];
		}
		ObjRideArray[ObjRideArray.length-1] = obj;
	}

	var NowRideItem : ItemRide;
	var ParentRideDown : Transform;
	var NowRideItemIcon : UISprite;
	var NowRideItemName : UILabel;
	var NowRideItemInfo : UILabel;
	var NowRideItemBianKuang : UISprite;
	function SelectRideAsID(myid : String  , myname : String  , myinfo : String  , spName : String  , biankuangName : String, SelectRide : ItemRide){
		NowRideItem = SelectRide;
		ParentRideDown.localPosition.y = 3000;
		for(var i=0; i<ObjRideArray.length; i++){
			if(ObjRideArray[i]){
				if(ObjRideArray[i] == SelectRide.gameObject){
					SelectRide.YesSelect();
					ParentRideDown.localPosition.y = 0;
					NowRideItemIcon.spriteName = spName;
					NowRideItemBianKuang.spriteName = biankuangName;
					NowRideItemName.text = myname;
					NowRideItemInfo.text = myinfo;
				}else{
					ObjRideArray[i].GetComponent(ItemRide).NoSelect();
				}
			}
		}
	}

	var SpriteRideButton : UISprite;
	var SpriteRideButton1 : UISprite;
	var SpriteRideButtonPar : GameObject;
	function RideButtonOn(ID : String){
		if(ID != "-1" && ID != ""){
			if(SpriteRideButton1){
				SpriteRideButton1.enabled = true;
				SpriteRideButton1.spriteName = "Ride_UI0" + (parseInt(ID)).ToString();
			}
			SpriteRideButton.spriteName = "Ride_UI0" + (parseInt(ID)).ToString();
//			if(UIControl.mapType == MapType.zhucheng){
				SpriteRideButtonPar.SetActiveRecursively(true);		
//			}
		}
	}

	function RideButtonOff(){
		SpriteRideButton1.enabled = false;
		SpriteRideButtonPar.SetActiveRecursively(false);
	}
	 
	function RideUse(){
		if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter ){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.Mounts).ToString());
		ps.rideQuality = parseInt(NowRideItem.myid.Substring(4,1));
		ps.rideLevel = NowRideItem.level;
	//	ps.selfRide = NowRideItem.level - 1;
		yt.Rows[0]["SelectMounts"].YuanColumnText = NowRideItem.myid;
		ps.rideMap = parseInt(yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(5,1));
		AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("messages146") + NowRideItem.myname);
		if(UIControl.mapType == MapType.zhucheng)
		RideButtonOn(parseInt(yt.Rows[0]["SelectMounts"].YuanColumnText.Substring(2,2)).ToString());
	         var rideMap = new ExitGames.Client.Photon.Hashtable();
	         rideMap.Add("rideMap",ps.rideMap);    
	     PhotonNetwork.SetPlayerCustomProperties(rideMap);  
	     ParentRideDown.localPosition.y = 3000;
	}

	function GetRideRowAsID(id : String) : yuan.YuanMemoryDB.YuanRow{
		for(var rows : yuan.YuanMemoryDB.YuanRow in PlayerPet.Rows){
			if(id.Length > 4){
				if(rows["ItemID"].YuanColumnText == id){
					return rows;
				}
			}
		}
		return null;
	}

	private var pInv : InventoryItem;
	function InventoryArrange(){
		if( Input.touchCount > 1 ){
			return;
		}
		var inv1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
		var inv2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
		var inv3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
		var inv4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
		
		var i : int = 0;
		var useStr : String;
		var useArrangeStr : String[];
		useStr = inv1 + inv2 + inv3 + inv4; 
		useArrangeStr = useStr.Split(Fstr.ToCharArray());
		var invs : InventoryItem[];
		invs = new Array(useArrangeStr.length);
		for(i=0; i<useArrangeStr.length; i++){
			if(useArrangeStr[i] != ""){
				invs[i] = AllResources.InvmakerStatic.GetItemInfo(useArrangeStr[i] , invs[i]);
			}
		}
		invStacking = new Array(60);
		for(i=0; i<invs.length; i++){
			if(invs[i] != null && invs[i].slotType < 12){
				pInv = AllResources.InvmakerStatic.GetItemInfo(invs[i].itemID , pInv);
				SetInventoryStacking(pInv);
				invs[i] = null;
			}
		}
		for(i=0; i<invs.length; i++){
			if(invs[i] != null && invs[i].slotType >= 12){
				pInv = AllResources.InvmakerStatic.GetItemInfo(invs[i].itemID , pInv);
				SetInventoryStacking(pInv);
				invs[i] = null;
			}
		}
		var useArray : Array = new Array();
		for(i=0; i<invStacking.length; i++){
			if(invStacking[i] != null){
				if(invStacking[i].slotType < 12){
					useArray.Add(invStacking[i]);
				}
			}
		}
		for(i=0; i<invStacking.length; i++){
			if(invStacking[i] != null){
				if(invStacking[i].slotType >= 12){
					useArray.Add(invStacking[i]);
				}
			}
		}
		for(i=0; i<invStacking.length; i++){
			if(invStacking[i] == null){
				useArray.Add(invStacking[i]);
			}
		}
		
		invStacking = new Array(60);
		for(i=0; i<invStacking.length; i++){
			invStacking[i] = useArray[i];
		}
		useArrangeStr = new Array(invStacking.length);
		for(i=0; i<invStacking.length; i++){
			if(invStacking[i] != null){
				useArrangeStr[i] = invStacking[i].itemID;
			}
		}
		InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText = "";
		InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText = "";
		InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText = "";
		InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText = "";
		for(i=0; i<useArrangeStr.length; i++){	
			if(i < 15){
				InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText += useArrangeStr[i] + ";";
			}else
			if(i < 30){
				InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText += useArrangeStr[i] + ";";		
			}else
			if(i < 45){
				InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText += useArrangeStr[i] + ";";		
			}else
			if(i < 60){
				InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText += useArrangeStr[i] + ";";		
			}
		}
		Inventory1 = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText;
		Inventory2 = InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText;
		Inventory3 = InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText;
		Inventory4 = InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
		SelectBag1();
	}
	private var invStacking : InventoryItem[];
	function SetInventoryStacking(inv : InventoryItem){
		var i : int = 0; 
		var j : int = 0;
		if(inv.slotType < 12){	
			for(i=0; i<invStacking.length; i++){ 
				if(invStacking[i] == null){
					invStacking[i] = (inv);
					return;
				}
			}
		}else{
			for(i=0; i<invStacking.length; i++){
				if(invStacking[i] != null){
					if(invStacking[i].itemID.Length > 5){
						if(invStacking[i].itemID.Substring(0,5) == inv.itemID.Substring(0,5) && invStacking[i].itemID.Substring(0,1) != "9"){
							if(invStacking[i].consumablesNum + inv.consumablesNum <= 20){				
								invStacking[i].consumablesNum += inv.consumablesNum; 
								if(invStacking[i].consumablesNum < 10){
									invStacking[i].itemID =  invStacking[i].itemID.Substring(0,5) + "0" +  invStacking[i].consumablesNum.ToString() + ","+invStacking[i].itemID.Substring(8,1);				
								}else{
										invStacking[i].itemID =  invStacking[i].itemID.Substring(0,5) + invStacking[i].consumablesNum.ToString() + ","+invStacking[i].itemID.Substring(8,1); 											
								}
								return;
							}else{
										var useNum : int = invStacking[i].consumablesNum;
										invStacking[i].consumablesNum =20; 
										invStacking[i].itemID =  invStacking[i].itemID.Substring(0,5) + invStacking[i].consumablesNum.ToString() + ","+invStacking[i].itemID.Substring(8,1);
										var useInv : InventoryItem;
										useInv = inv;
										useInv.consumablesNum = useNum + inv.consumablesNum - 20; 
										if(useInv.consumablesNum < 10){
											useInv.itemID =  useInv.itemID.Substring(0,5) + "0" +  useInv.consumablesNum.ToString() + ","+useInv.itemID.Substring(8,1);					
										}else{
											useInv.itemID =  useInv.itemID.Substring(0,5) + useInv.consumablesNum.ToString() + ","+useInv.itemID.Substring(8,1);											
										}
										
								for(j=0; j<invStacking.length; j++){
									if(invStacking[j] == null){ 
										invStacking[j] = (useInv);
										return;					
									}
								}
							}
						}
					}
				}
			}
			for(i=0; i<invStacking.length; i++){ 
				if(invStacking[i] == null){  
					 invStacking[i] = (inv);
					return;					
				}
			}
		}
	}


	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	private var useInvSoul : InventoryItem;
	function AddBagSoulItemAsID(ID : String){
		if( ! AllManage.UIALLPCStatic.isSoul17()){
			AllManage.UIALLPCStatic.showSoul17();
			yield;
			yield;
			yield;
			yield;
		}
		useInvSoul = new InventoryItem();
		useInvSoul = AllResources.InvmakerStatic.GetItemInfo(ID , useInvSoul);
		AllManage.SoulCLStatic.AddBagSoulItem(useInvSoul);
	}

	function AddBagDigestItemAsID(ID : String){
		if( ! AllManage.UIALLPCStatic.isSoul17()){
			AllManage.UIALLPCStatic.showSoul17();
			yield;
			yield;
			yield;
			yield;
		}
		useInvSoul = new InventoryItem();
		useInvSoul = AllResources.InvmakerStatic.GetItemInfo(ID , useInvSoul);
		AllManage.SoulCLStatic.AddBagDigestItem(useInvSoul);
	}

	function TutorialsDetectionAsID(ID : String){
		var strs : String[];
		var bool : boolean = false;
		strs = yt.Rows[0]["SpeakComplete"].YuanColumnText.Split(Fstr.ToCharArray());;
		for(var i=0; i<strs.length; i++){
			if(strs[i] == ID){
				return false;
			}
		}
		yt.Rows[0]["SpeakComplete"].YuanColumnText += (ID + ";");
		return true;
	}

	function TutorialsDetectionAsIDNoSave(ID : String){
		var strs : String[];
		var bool : boolean = false;
		strs = yt.Rows[0]["SpeakComplete"].YuanColumnText.Split(Fstr.ToCharArray());;
		for(var i=0; i<strs.length; i++){
			if(strs[i] == ID){
				return false;
			}
		}
		return true;
	}

	function SetYanSeAsID(obj : Object[]){
		var spObj : UISprite;
		var itemID : String;
		var inv : InventoryItem;

		itemID = obj[0];
		spObj = obj[1];
		var Str : String = "";
		inv = AllResources.InvmakerStatic.GetItemInfo(itemID,inv);
		if(inv.slotType < 12){
			if(inv.slotType  == 10 || inv.slotType == 11 ){
				if(inv.itemQuality < 6){
					Str = "yanse" + inv.itemQuality;				
				}else{
					Str = "yanse" + (inv.itemQuality-4);	
				}
			}else{
				if(inv.itemQuality < 6){
					Str = "yanse" + inv.itemQuality;				
				}else{
					Str = "yanse" + (inv.itemQuality-4);	
				}
			}
		}else{ 
			if(inv.slotType  != SlotType.Soul && inv.slotType == SlotType.Digest ){
				Str = "yanse" + 1;	
			}else{
				var intI : int = 0;
				if(inv.slotType != SlotType.Formula){
				    if(inv.slotType == SlotType.Ride)
				    {
				        intI = parseInt(inv.itemID.Substring(4,1));
				    }
				    else
				    {
				        intI = parseInt(inv.itemID.Substring(8,1));
				    }
				}else{
					intI = parseInt(inv.itemID.Substring(9,1));			
				}
				if(intI < 6){
					Str = "yanse" + intI;				
				}else{
					Str = "yanse" + (intI-4);	
				}
			}
		}
	//	print(Str + " ===1111111111111=== " + itemID);
		spObj.spriteName = Str;
	}
}

	private var useYT1 : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("usePlayerInfo11","PlayerID");
	private var useYTTwo : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("usePlayerInfo22","PlayerID");
	private var useYT3 : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("usePlayerInfo33","PlayerID");
function CreateBotPlayer(Objs : Object[]){
	var botName : String;
	var botLevel : String;
	var botProID : String;
	botName = Objs[0];
	botLevel = Objs[1];
	botProID = Objs[2];
//	print(botProID);
	var useRow : yuan.YuanMemoryDB.YuanRow;
	if(botProID == "1"){
		useRow = InventoryControl.yt.Rows[0].CopyTo();
		useYT1.Add(useRow);
		useYT1.Rows[0]["PlayerLevel"].YuanColumnText = botLevel;	
		useYT1.Rows[0]["ProID"].YuanColumnText = "1";
		useYT1.Rows[0]["PlayerID"].YuanColumnText = "-2";
//		useYT1.Rows[0]["EquipItem"].YuanColumnText = "1290535120000009990000000;;4790535120000009990000000;4790535120000009990000000;4490535120000009990000000;4590535120000009990000000;4290535120000009990000000;4690535120000009990000000;4890535120000009990000000;4990535120000009990000000;4190535120000009990000000;4390535120000009990000000;";
		useYT1.Rows[0]["SkillsBranch"].YuanColumnText = Random.Range(1,3).ToString();
		useYT1.Rows[0]["EquipItem"].YuanColumnText = "";
		useYT1.Rows[0]["EquipItem"].YuanColumnText = GetRandomEqueps(useYT1.Rows[0]["EquipItem"].YuanColumnText , botProID , botLevel , useYT1.Rows[0]["SkillsBranch"].YuanColumnText);
		useYT1.Rows[0]["Skill"].YuanColumnText =  "00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;";
		useYT1.Rows[0]["SkillsPostion"].YuanColumnText = ";;;;";
//		useYT1.Rows[0]["Skill"].YuanColumnText =  "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;00;00;00;";
//		useYT1.Rows[0]["SkillsPostion"].YuanColumnText = "15,11,ProID_111;30,12,ProID_112;6,03,ProID_103;12,02,ProID_102;";
		CreateTeamPlayer(useYT1 , 1);
	}
	if(botProID == "2"){
		useRow = InventoryControl.yt.Rows[0].CopyTo();
		useYTTwo.Add(useRow);
		useYTTwo.Rows[0]["ProID"].YuanColumnText = "2";
		useYTTwo.Rows[0]["PlayerID"].YuanColumnText = "-3";
		useYTTwo.Rows[0]["PlayerLevel"].YuanColumnText = botLevel;	
//		useYT222.Rows[0]["EquipItem"].YuanColumnText = "2190535120000009990000000;;5790535120000009990000000;5790535120000009990000000;5490535120000009990000000;5590535120000009990000000;5290535120000009990000000;5690535120000009990000000;5890535120000009990000000;5990535120000009990000000;5190535120000009990000000;5390535120000009990000000;";
		useYTTwo.Rows[0]["SkillsBranch"].YuanColumnText = Random.Range(1,3).ToString();
		useYTTwo.Rows[0]["EquipItem"].YuanColumnText = "";
		useYTTwo.Rows[0]["EquipItem"].YuanColumnText = GetRandomEqueps(useYTTwo.Rows[0]["EquipItem"].YuanColumnText , botProID , botLevel , useYTTwo.Rows[0]["SkillsBranch"].YuanColumnText);
		useYTTwo.Rows[0]["Skill"].YuanColumnText =  "00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;";
		useYTTwo.Rows[0]["SkillsPostion"].YuanColumnText = ";;;;";
//		useYTTwo.Rows[0]["Skill"].YuanColumnText =  "30;30;20;22;20;21;21;12;12;10;10;10;00;00;00;20;20;20;10;10;00;00;00;";
//		useYTTwo.Rows[0]["SkillsPostion"].YuanColumnText = "12,06,ProID_206;15,07,ProID_207;6,04,ProID_204;30,08,ProID_208;";		
		CreateTeamPlayer(useYTTwo , 2);
	}
	if(botProID == "3"){
		useRow = InventoryControl.yt.Rows[0].CopyTo();
		useYT3.Add(useRow);
		useYT3.Rows[0]["PlayerID"].YuanColumnText = "-4";
		useYT3.Rows[0]["ProID"].YuanColumnText = "3";
		useYT3.Rows[0]["PlayerLevel"].YuanColumnText = botLevel;	
//		useYT3.Rows[0]["EquipItem"].YuanColumnText = "3290535120000009990000000;;6790535120000009990000000;6790535120000009990000000;6490535120000009990000000;6590535120000009990000000;6290535120000009990000000;6690535120000009990000000;6890535120000009990000000;6990535120000009990000000;6190535120000009990000000;6390535120000009990000000;";
		useYT3.Rows[0]["SkillsBranch"].YuanColumnText = Random.Range(1,3).ToString();
		useYT3.Rows[0]["EquipItem"].YuanColumnText = "";
		useYT3.Rows[0]["EquipItem"].YuanColumnText = GetRandomEqueps(useYT3.Rows[0]["EquipItem"].YuanColumnText , botProID , botLevel , useYT3.Rows[0]["SkillsBranch"].YuanColumnText);
		useYT3.Rows[0]["Skill"].YuanColumnText = "00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;00;";
		useYT3.Rows[0]["SkillsPostion"].YuanColumnText = ";;;;";
//		useYT3.Rows[0]["Skill"].YuanColumnText = "30;30;30;00;00;00;00;00;00;30;30;30;30;30;30;30;30;00;00;00;30;30;30;";
//		useYT3.Rows[0]["SkillsPostion"].YuanColumnText = "12,13,ProID_313;24,14,ProID_314;18,03,ProID_303;12,15,ProID_315;";
		CreateTeamPlayer(useYT3 , 3);
	}
}

function CreateTeamPlayer(ytFuben : yuan.YuanMemoryDB.YuanTable , proid : int){
	var obj : GameObject;
	var npcName : String;
	ytFuben.Rows[0]["ProID"].YuanColumnText = proid.ToString();
	obj = AllResources.ar.CreatePlayerFuBen(proid , UIControl.myTeamInfo , npcName , 	PlayerStatus.MainCharacter.position);
    obj.SendMessage("SetMaster",PlayerStatus.MainCharacter, SendMessageOptions.DontRequireReceiver);
//    print(ytFuben.Rows[0]["EquipItem"].YuanColumnText );
	obj.SendMessage("SetPlayerInfoAsYt" , ytFuben , SendMessageOptions.DontRequireReceiver);
}

var Equeps : String[];
function GetRandomEqueps(str : String , ProID : String , Level : String , type : String) : String{
//	print(ProID);
	Equeps = new String[12];
	Equeps[0] = AllResources.staticLT.MakeItemIDAsBot(Equeps[0] , parseInt(Level) , parseInt(ProID) , parseInt(type));
//	print(Equeps[0]);
	Equeps[2] = AllResources.staticLT.MakeItemIDAsBot(Equeps[2] , parseInt(Level) , parseInt(ProID) + 3 , 7);
//	print(Equeps[2]);
	Equeps[3] = AllResources.staticLT.MakeItemIDAsBot(Equeps[3] , parseInt(Level) , parseInt(ProID) + 3  , 7);
//	print(Equeps[3]);
	Equeps[4] = AllResources.staticLT.MakeItemIDAsBot(Equeps[4] , parseInt(Level) , parseInt(ProID) + 3  , 4);
	Equeps[5] = AllResources.staticLT.MakeItemIDAsBot(Equeps[5] , parseInt(Level) , parseInt(ProID) + 3  , 5);
	Equeps[6] = AllResources.staticLT.MakeItemIDAsBot(Equeps[6] , parseInt(Level) , parseInt(ProID) + 3  , 2);
	Equeps[7] = AllResources.staticLT.MakeItemIDAsBot(Equeps[7] , parseInt(Level) , parseInt(ProID) + 3  , 6);
	Equeps[8] = AllResources.staticLT.MakeItemIDAsBot(Equeps[8] , parseInt(Level) , parseInt(ProID) + 3  , 8);
	Equeps[9] = AllResources.staticLT.MakeItemIDAsBot(Equeps[9] , parseInt(Level) , parseInt(ProID) + 3  , 9);
	Equeps[10] = AllResources.staticLT.MakeItemIDAsBot(Equeps[10] , parseInt(Level) , parseInt(ProID) + 3  , 1);
	Equeps[11] = AllResources.staticLT.MakeItemIDAsBot(Equeps[11] , parseInt(Level) , parseInt(ProID) + 3  , 3);
	for(var i=0; i<Equeps.length; i++){
		str += Equeps[i] + ";";
	}
	return str;
}

private var UitemID : String;		
private var UitemIcon : UISprite;
private var UitemBox : UISprite;
private var UlblNum : UILabel;
function SetItemIconAsID(objs : Object[]){
	UitemID =  objs[0];
	UitemIcon = objs[1];
	UitemBox = objs[2];
	if(objs[3]!=""){
	UlblNum = objs[3];
	}
	var UIV : InventoryItem;
	UIV = AllResources.InvmakerStatic.GetItemInfo(UitemID , UIV);


	var Str : String = "";
//	print(UitemID + " == UitemID");
	UIV = AllResources.InvmakerStatic.GetItemInfo(UitemID,UIV);
	if(UIV.slotType < 12){
		if(UIV.slotType  == 10 || UIV.slotType == 11 ){
			if(UIV.itemQuality < 6){
				Str = "yanse" + UIV.itemQuality;				
			}else{
				Str = "yanse" + (UIV.itemQuality-4);	
			}
		}else{
			if(UIV.itemQuality < 6){
				Str = "yanse" + UIV.itemQuality;				
			}else{
				Str = "yanse" + (UIV.itemQuality-4);	
			}
		}
	}else{ 
		if(UIV.slotType  != SlotType.Soul && UIV.slotType == SlotType.Digest ){
			Str = "yanse" + 1;	
		}else{
			var intI : int = 0;
			if(UIV.slotType != SlotType.Formula){
				intI = parseInt(UIV.itemID.Substring(8,1));
			}else{
				intI = parseInt(UIV.itemID.Substring(9,1));			
			}
			if(intI < 6){
				Str = "yanse" + intI;			
			}else{
				Str = "yanse" + (intI-4);
			}
		}
	}
	if(UitemBox)
		UitemBox.spriteName = Str;
	if(UitemIcon)
		UitemIcon.spriteName = UIV.atlasStr;
	if(UIV.slotType < 12){
		if(UlblNum)
			UlblNum.text = "";
	}else{
	    if(UlblNum)
	    {
	        UlblNum.text = UIV.consumablesNum.ToString() == "0" ? "1" : UIV.consumablesNum.ToString();

	    }
	}
	
	
}

function GetProSpriteName() : String{
	var str : String = "";
	switch(InventoryControl.yt.Rows[0]["SkillsBranch"].YuanColumnText){
		case "0" : 
			str = "dunpai" ;
			break;
		case "1":
				switch(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText){
					case "1" : 
					str = "UIM_Anti-War_N";
					break;
					case "2" : 
					str = "UIM_Robber_O";
					break;
					case "3" : 
					str = "UIM_Master_N";
					break; 
				}
			break;
		case "2":
				switch(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText){
					case "1" : 
					str = "UIM_Violent-War_N";
					break;
					case "2" : 
					str = "UIM_Ranger_N ";
					break;
					case "3" : 
					str = "UIM_Necromancer_N ";
					break; 
				}
			break;
		case "3":
				switch(InventoryControl.yt.Rows[0]["ProID"].YuanColumnText){
					case "1" : 
					str = "UIM_Violent-War_N";
					break;
					case "2" : 
					str = "UIM_Ranger_N ";
					break;
					case "3" : 
					str = "UIM_Necromancer_N ";
					break; 
				}
			break;
	}
	return str;
}

function CanMapManaged(str : String){
	if(yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText.IndexOf(str) > 0){
		return true;
	}
	if(yt.Rows[0]["DuplicateEvaElite"].YuanColumnText.IndexOf(str) > 0){
		return true;
	}
		if(yt.Rows[0]["DuplicateEvaDungeon"].YuanColumnText.IndexOf(str) > 0){
		return true;
	}
	return false;
}
