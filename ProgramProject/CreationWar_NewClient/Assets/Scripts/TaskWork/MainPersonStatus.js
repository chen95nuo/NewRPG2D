#pragma strict


class MainPersonStatus extends XmlControl{

	var player : MainPlayerStatus;
	var MainTW : MainTaskWork;
	var invControl : InventoryControl;
	var playerC : ThirdPersonController;
	var playerS : PlayerStatus;
//	var taskList : TaskInfoList;
	var pppp : MainPlayerStatus;
	var mmmm :  MainTaskWork;
	private var ps : PlayerStatus; 
//	private var ts : TiShi;

	
	function Start () {
		if (PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID))
		{ 
//		invmaker =  FindObjectOfType(Inventorymaker);
//		AllResources.staticLT = FindObjectOfType(LootTable);
		invControl = AllManage.InvclStatic;
		MainTW = AllManage.mtwStatic; 
//		taskList = FindObjectOfType(TaskInfoList);
		MainTW.MainPS = this;
		mmmm = MainTW; 
		ps = GetComponent(PlayerStatus); 
		MainTW.ps = ps;
		
		yield PrivateStatusInit();
		MainTW.TaskCreateRobot();
//		 ts = AllManage.tsStatic;
		}
	}
	
	var DBTaskStr : String;
	var DBCompletTask : String;
	var DBGetPlace : String;
	var DBEveryDayActivity : String; 
	var StrEveryDayActivity : String[];
	private var FStr : String = ";";
	private var DStr : String = ",";
	
	
	function EveryDayActivityIsGet(id : String){ 
		StrEveryDayActivity = DBEveryDayActivity.Split(FStr.ToCharArray());
		for(var i=0; i<StrEveryDayActivity.length; i++){
			if(StrEveryDayActivity[i] == id){
				return false;
			}
		} 
		return true;
	}
	
	function PrivateStatusInit(){
		yield;
		yield;
		yield;
		var mm : boolean = false; 
		var OneTime : boolean = false;
			player = new MainPlayerStatus(); 
			player.nowTask = new Array(10);

					DBTaskStr = InventoryControl.yt.Rows[0]["Task"].YuanColumnText;
					DBCompletTask = InventoryControl.yt.Rows[0]["CompletTask"].YuanColumnText;
					DBGetPlace = InventoryControl.yt.Rows[0]["GetPlace"].YuanColumnText;
					DBEveryDayActivity = InventoryControl.yt.Rows[0]["EveryDayActivity"].YuanColumnText; 
					StrEveryDayActivity = DBEveryDayActivity.Split(FStr.ToCharArray());
					SetPlayer(DBTaskStr , DBCompletTask , DBGetPlace); 
					player.level = GetBDInfoInt("PlayerLevel" , 1);
					player.pro = GetBDInfoInt("ProID" , 0);
					OneTime = true;

		MainTW.TaskObjectMakerInit(player);
		MainTW.UICLShowTaskInfo();
		pppp = player;
		if(player.level == 1 && DBTaskStr == "" && DBCompletTask == "" && DBGetPlace == "" && Application.loadedLevelName == "Map111"){
			//MainTW.FirstDaoHang();
		}
		
//		//print(FindWayGoOn + " == " + FindWayID);
		yield;
		yield;
		yield;
		yield;
		yield;
		if(UIControl.mapType != MapType.zhucheng){
			//print(" Qu Xiao Zi Dong Xun Lu");
		}
		if(FindWayGoOn && UIControl.mapType == MapType.zhucheng){
			for(var i=0; i<player.nowTask.length; i++){
				if(player.nowTask[i] != null && goWayAddTask == "" ){				
					if(FindWayID == player.nowTask[i].id){
						FindWayGoOn = false;				
						MainTW.FindWay(player.nowTask[i]);
					}
				}
			}
		}
		if(goWayAddTask != ""){	
			FindWayGoOn = false;
			goWayAddTask = "";
		}
	}
	
	function GetPrivateTask(id : String) : PrivateTask{
		for(var i = 0; i<player.nowTask.length; i++){
			if(player.nowTask[i] != null){
				if(player.nowTask[i].task.id == id){
					return player.nowTask[i];
				}
			}
		}
		return null;
	}
	
	function GetBDInfoInt(bd : String , it : int) : int{  
		var iii : int = 0;
		try{ 
			iii = parseInt(InventoryControl.yt.Rows[0][bd].YuanColumnText);
			return  iii;
		}catch(e){
			return it;	
		}
	}
	
	function SetPlayer(taskID : String , CompletTask : String , GetPlace : String){
		var i : int = 0;
		var useStr : String[];
		
		useStr = CompletTask.Split(FStr.ToCharArray());
		player.doneTaskID = new Array(useStr.length);
		for(i=0; i<useStr.length; i++){
			if(useStr[i] != ""){
				player.doneTaskID[i] = useStr[i];
			}
		}
		
		useStr = GetPlace.Split(FStr.ToCharArray());
		player.doneTaskMap = new Array(useStr.length -1);
		for(i=0; i<useStr.length; i++){
			if(useStr[i] != "" && useStr[i].Length > 3){
				player.doneTaskMap[i] = new TaskMap();
				player.doneTaskMap[i].mapID = useStr[i].Substring(0,3);
				player.doneTaskMap[i].nandu = parseInt(useStr[i].Substring(3,1));
			}
		}
		
		var task : MainTask;
		useStr = taskID.Split(FStr.ToCharArray());
		for(i=0; i<useStr.length; i++){
			if(useStr[i] != ""){
				var str1 : String[] = useStr[i].Split(DStr.ToCharArray());
				task = GetTaskAsId(str1[0]);
				if(task != null){
					task.jindu = parseInt(str1[1]);
					SetAnyTask(task , parseInt(str1[2]));
					MainTW.UICL.ShowTaskInfoList(player,task);			
				}
			}
		}
	}
	
	function LookTaskIsGet(nowID : String){
		var i : int = 0;
		var useStr : String[];
		useStr = DBTaskStr.Split(FStr.ToCharArray());
		for(i=0; i<useStr.length; i++){
			if(useStr[i] != ""){
				var str1 : String[] = useStr[i].Split(DStr.ToCharArray());
				if(str1.length > 0){
					if(str1[0] == nowID){
						return false;
					}
				}
			}
		}
		return true;
	}
	
	function CreateTaskObject(prd : ProducedObject){
		if (photonView && PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID) && MainTW && prd)
		{
		
		MainTW.CreateTaskObjectManager(prd.task , prd);
		}
	}
	
	function AddNewTask(task : MainTask , doneNum : int){
		if(LookTaskIsDone(task.id)){
			if(parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) <= 10){
				//TD_info.setTask(task.taskName);
			}
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().TaskAcceptedAsID(task.id));		
			var use : String[]; 
			use = requestNewTask; 
			requestNewTask = new Array(requestNewTask.length + 1);
			for(var i=0; i<(requestNewTask.length - 1); i++){
				 requestNewTask[i] = use[i];
			} 
			requestNewTask[requestNewTask.length - 1] = task.id;
		}
		return;
		
		
//		if(MainTW == null){
//			return;
//		}
//		var dMap : TaskMap[]; 
//		var isNewMap : boolean = true;
//		for(var i=0; i<player.nowTask.length; i++){
//			if(player.nowTask[i] == null){
//				player.nowTask[i] = new PrivateTask(); 
//				player.nowTask[i].id = task.id; 
//				player.nowTask[i].jindu = task.jindu; 
//				player.nowTask[i].doneNum = doneNum; 
//				player.nowTask[i].task = task; 
//				MainTW.UICL.TinfoList.AlreadyGetTask(task.id);  
//				
//				if(player.doneTaskMap == null){
//					player.doneTaskMap = new Array(1); 
//					player.doneTaskMap[0] = new TaskMap();
//				}
//				for(var m=0; m<player.doneTaskMap.length; m++){
//					if(task.doneType.Length > 3){
//						if(player.doneTaskMap[m] != null){
//							if(task.doneType.Substring(4,3) == player.doneTaskMap[m].mapID){
//								if(parseInt(task.doneType.Substring(13,1)) > player.doneTaskMap[m].nandu){
//									player.doneTaskMap[m].nandu = parseInt(task.doneType.Substring(13,1));
//								}
//								isNewMap = false; 
//							}			
//						}
//					}
//				}
//				if(isNewMap){
//					dMap = player.doneTaskMap;
//					player.doneTaskMap = new Array(dMap.length + 1); 
//					for(var o=0; o<dMap.length; o++){
//						 player.doneTaskMap[o] = dMap[o]; 
//					}  
//					player.doneTaskMap[dMap.length] =  new TaskMap();
//					if( task.doneType.Length > 3){
//						player.doneTaskMap[dMap.length].mapID = task.doneType.Substring(4,3);
//						player.doneTaskMap[dMap.length].nandu = parseInt(task.doneType.Substring(13,1));
//						UpdateDBGetPlace();				
//					}
//				}
//				UpdateDBTaskStr();
////				print(LookTaskIsGet(task.id) + " == LookTaskIsGet(task.id)");
//				if(!LookTaskIsGet(task.id)){
//					MainTW.ShowGetTaskName(task.taskName);			
//					InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.AcceptFirstTask).ToString());
//				}
//				return true;
//			}
//		}
//		return false;
	}
	
	private var goWayAddTask : String = "";
	function returnAddNewTask(taskid : String){
		if(requestNewTask != null){
			for(var i=0; i<requestNewTask.length; i++){
				if(requestNewTask[i] == taskid){
					requestNewTask[i] = "";
				}
			}
		}
		AllManage.UICLStatic.isWayAddTask = false;
		yield PrivateStatusInit();	
		if(!AllManage.UICLStatic.isOpenNpcTalk){
			goWayAddTask = taskid;
			var findWayTask : PrivateTask;
			findWayTask = GetPrivateTask(goWayAddTask); 
			AllManage.mtwStatic.FindWay(findWayTask);
		}
		AllManage.UICLStatic.isOpenNpcTalk = false;
	}
	
	function SetAnyTask(task : MainTask , doneNum : int){
		if(MainTW == null){
			return;
		}
		var dMap : TaskMap[]; 
		var isNewMap : boolean = true;
		for(var i=0; i<player.nowTask.length; i++){
			if(player.nowTask[i] == null){
				player.nowTask[i] = new PrivateTask(); 
				player.nowTask[i].id = task.id; 
				player.nowTask[i].jindu = task.jindu; 
				player.nowTask[i].doneNum = doneNum; 
				player.nowTask[i].task = task; 
				MainTW.UICL.TinfoList.AlreadyGetTask(task.id);  
				
				if(player.doneTaskMap == null){
					player.doneTaskMap = new Array(1); 
					player.doneTaskMap[0] = new TaskMap();
				}
				for(var m=0; m<player.doneTaskMap.length; m++){
					if(task.doneType.Length > 3){
						if(player.doneTaskMap[m] != null){
							if(task.doneType.Substring(4,3) == player.doneTaskMap[m].mapID){
								if(parseInt(task.doneType.Substring(13,1)) > player.doneTaskMap[m].nandu){
									player.doneTaskMap[m].nandu = AllManage.mtwStatic.GetTaskLevel(task.doneType);//parseInt(task.doneType.Substring(13,1));
								}
								isNewMap = false; 
							}			
						}
					}
				}
				if(isNewMap){
					dMap = player.doneTaskMap;
					player.doneTaskMap = new Array(dMap.length + 1); 
					for(var o=0; o<dMap.length; o++){
						 player.doneTaskMap[o] = dMap[o]; 
					}  
					player.doneTaskMap[dMap.length] =  new TaskMap();
					if( task.doneType.Length > 3){
						player.doneTaskMap[dMap.length].mapID = task.doneType.Substring(4,3);
						player.doneTaskMap[dMap.length].nandu = AllManage.mtwStatic.GetTaskLevel(task.doneType);//1;//parseInt(task.doneType.Substring(13,1));
						UpdateDBGetPlace();				
					}
				}
				UpdateDBTaskStr();
//				print(LookTaskIsGet(task.id) + " == LookTaskIsGet(task.id)");
//				if(!LookTaskIsGet(task.id)){
//					MainTW.ShowGetTaskName(task.taskName);			
//					InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.AcceptFirstTask).ToString());
//				}
				return true;
			}
		}
		return false;	
	}
	
	function FangQiTask(task : MainTask){
		PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().TaskGiveUpAsID(task.id));	
		return;
////		if(taskList == null){
////			taskList = FindObjectOfType(TaskInfoList);
////		}
//		try{
//			AllManage.taskILStatic.NowDone(player.nowTask[GetPlayerNowTaskAsID(task.id)]);
//			player.nowTask[GetPlayerNowTaskAsID(task.id)] = null;
//			UpdateDBTaskStr();
//			
//			MainTW.TaskObjectMakerInit(player);
//			MainTW.UICLShowTaskInfo();
//			pppp = player;
//	
//		}catch(e){
//		}
	}
	
	function returnTaskGiveUpAsID(taskid : String){
		if(parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) <= 10){
			var task : MainTask;
			task = MainTW.GetTaskAsId(taskid);
			//TD_info.taskFail(task.taskName);
		}
		yield PrivateStatusInit();
	}
	
	function UpdateEveryDayActivity(id : String){ 
		if(EveryDayActivityIsGet(id)){
			DBEveryDayActivity += id + ";"; 
			StrEveryDayActivity = DBEveryDayActivity.Split(FStr.ToCharArray());
		}
		if(InventoryControl.yt.Rows.Count > 0){
			InventoryControl.yt.Rows[0]["EveryDayActivity"].YuanColumnText = DBEveryDayActivity;
		}
	}
	
	function UpdateDBTaskStr(){ 
		DBTaskStr = "";
		for(var i=0; i<player.nowTask.length; i++){
			if(player.nowTask[i] != null){
				DBTaskStr += (player.nowTask[i].id + "," + player.nowTask[i].jindu.ToString() + "," + player.nowTask[i].doneNum.ToString() + ";");
			}
		}
		if(DBTaskStr != null){		
			if(InventoryControl.yt.Rows.Count > 0){
				InventoryControl.yt.Rows[0]["Task"].YuanColumnText = DBTaskStr;
			}
		} 
	}
	function UpdateDBGetPlace(){ 
		DBGetPlace = "";
		for(var i=0; i<player.doneTaskMap.length; i++){
			if(player.doneTaskMap[i] != null){		
				if(player.doneTaskMap[i].mapID != ""){				
					DBGetPlace += (player.doneTaskMap[i].mapID + player.doneTaskMap[i].nandu.ToString() + ";");
				}	
			}
		}
		if(InventoryControl.yt.Rows.Count > 0){
			InventoryControl.yt.Rows[0]["GetPlace"].YuanColumnText = DBGetPlace;
		}
	}
	

	function UpdateDBCompletTask(){ 
		DBCompletTask = "";
		for(var i=0; i<player.doneTaskID.length; i++){
			DBCompletTask += (player.doneTaskID[i] + ";");
		}
		if(InventoryControl.yt.Rows.Count > 0){
			InventoryControl.yt.Rows[0]["CompletTask"].YuanColumnText = DBCompletTask;
		} 
	}
	
	
	function AddNewDoneObject(task : MainTask){
		InRoom.GetInRoomInstantiate().TaskAddNumsAsID(task.id);	
		return;
	
//		for(var i=0; i<player.nowTask.length; i++){
//			if(player.nowTask[i] != null){		
//				if(player.nowTask[i].id == task.id && player.nowTask[i].jindu < 2){
//					player.nowTask[i].doneNum += 1;
//					if(player.nowTask[i].doneNum >= task.doneNum){ 
//						MainTW.TaskDone(task);
//						player.nowTask[i].jindu += 1;
//						if(player.nowTask[i].task.taskType == MainTaskType.guaiwu || player.nowTask[i].task.taskType == MainTaskType.daoda || player.nowTask[i].task.taskType == MainTaskType.wupin || player.nowTask[i].task.taskType == MainTaskType.PVPKiller || player.nowTask[i].task.taskType == MainTaskType.PVPWin || player.nowTask[i].task.taskType == MainTaskType.WaKuang){
//							MainTW.DJshow(task.taskName);
//						}
////						print("zhe li le");
//						PrivateStatusInit();
//					}
//				}
//			}
//		}
//		UpdateDBTaskStr();
	}
	
	private var useTask : MainTask;
	private var tipsComplete : Array = new Array();
	function returnTaskAddNumsAsID(taskid : String){
//		print(InventoryControl.yt.Rows[0]["Task"].YuanColumnText + " ===== task");
		var bool : boolean = false;
		var p : PrivateTask;
		var i : int = 0;
		yield PrivateStatusInit();
		useTask = MainTW.GetTaskAsId(taskid);
		p = GetPrivateTask(taskid); 
		if(p != null){
			if(p.task.jindu != 2){	
				AllManage.tsStatic.Show1( "("+UIControl.taskLeixingStrs[p.task.leixing] + ")" + p.task.ComplateInfo + "  " +p.doneNum + "/" + p.task.doneNum);						
			}else{
				if(!LookTaskIsTips(taskid)){
					tipsComplete.Add(taskid);
					AllManage.tsStatic.Show1( "("+UIControl.taskLeixingStrs[p.task.leixing] + ")" + p.task.ComplateInfo + "  " +p.doneNum + "/" + p.task.doneNum);											
				}
			}
		}
		for(i=0; i<player.nowTask.length; i++){
			if(player.nowTask[i] != null){		
				if(player.nowTask[i].id == useTask.id){// && player.nowTask[i].jindu != 2){
					if(player.nowTask[i].doneNum >= useTask.doneNum){ 
						if(player.nowTask[i].task.taskType == MainTaskType.guaiwu || player.nowTask[i].task.taskType == MainTaskType.daoda || player.nowTask[i].task.taskType == MainTaskType.wupin || player.nowTask[i].task.taskType == MainTaskType.PVPKiller || player.nowTask[i].task.taskType == MainTaskType.PVPWin || player.nowTask[i].task.taskType == MainTaskType.WaKuang){
//							MainTW.DJshow(useTask.taskName);
							if(! AllManage.dungclStatic.DungeonIsDone)
								AllManage.tsStatic.ShowFinger(useTask.taskName + " " + AllManage.AllMge.Loc.Get("meg0121") ,  Vector3.zero);
						}
					}
				}
			}
		}
		if(useTask.taskType == MainTaskType.jiaocheng){
			AllManage.mtwStatic.stepValue = "StepTutorials";
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().TaskComplete(useTask.id));
		}
	}
	
	function LookTaskIsTips(taskid : String) : boolean{
		for(var i=0; i<tipsComplete.Count ; i++){
			if(tipsComplete[i] == taskid){
				return true;
			}
		}
		return false;
	}
	
	function ShowShouJiTaskNum(taskid : String , num : int){
		for(var i=0; i<player.nowTask.length; i++){
			if(player.nowTask[i] != null){		
				if(player.nowTask[i].id == taskid && player.nowTask[i].jindu < 2){
					player.nowTask[i].doneNum = num;
				}
			}
		}
	}
	
	private var invReward : InventoryItem;
//	private var LT : LootTable;
//	var invmaker : Inventorymaker;
	private var itemid2 : String;
	private var nowTaskMainNpc : npcAI;
	function TaskDone(task : MainTask){
//		;
		if(LookTaskIsDone(task.id)){		
			PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().TaskComplete(task.id));
			var use : String[]; 
			use = requestComplet; 
			requestComplet = new Array(requestComplet.length + 1);
			for(var i=0; i<(requestComplet.length - 1); i++){
				 requestComplet[i] = use[i];
			} 
			requestComplet[requestComplet.length - 1] = task.id;
		}
	//AllManage.tsStatic.RefreshBaffleOn();
	//InRoom.GetInRoomInstantiate().TaskComplete(task.id);
		return;
//		var str : String[];  
//		str = new Array(GetPlayerDoneTaskIDLength() + 1);
//		if(player.doneTaskID != null){		
//			for(var i=0; i<player.doneTaskID.length; i++){
//					str[i] = player.doneTaskID[i];
//			}
//		}
//		if(task.reward.itemId.length > 4){
//			itemid2 = task.reward.itemId.Substring(0,2);
//			if(itemid2 == "88"){
//				invControl.AddNewDaojuItemAsID(task.reward.itemId);
//			}else
//			if(itemid2 == "72"){
//				invControl.AddNewRideItemAsID(task.reward.itemId);			
//			}else{
//				invReward = AllResources.InvmakerStatic.GetItemInfo(task.reward.itemId , invReward);
//				invControl.AddBagItem(invReward); 
//				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , invReward.itemID);
//				MainTW.ShowGetItemName(invReward.itemName);
//			}
//		}
//		playerS.CompletTask();
//		playerS.AddExperience(task.reward.exp);
//		playerS.AddPrestige( -20);
//		AllManage.AllMge.UseMoney(task.reward.gold*(-1) , task.reward.rank*(-1) , UseMoneyType.TaskDone , gameObject , "");
//		if(task.leixing != 3){
//			str[str.length-1] = task.id; 		
//		}
//
//		player.doneTaskID = str;
//		UpdateDBCompletTask();
//
//		UpdateDBTaskStr();
		
	}
	
	var requestComplet : String[] = new String[0];
	var requestNewTask : String[] = new String[0];
	var Rtime : int = 0;
	private var it : int = 0;
	function Update(){
//		if(Time.time > Rtime){
//			Rtime =  Time.time + 3;
//			if(requestComplet.length > 0){
//				for(it =0; it<requestComplet.length; it++){
//					if(requestComplet[it] != "" && LookTaskIsDone(requestComplet[it])){		
//						PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().TaskComplete(requestComplet[it]));
//					}
//				}
//			}
//			if(requestNewTask.length > 0){
//				for(it =0; it<requestNewTask.length; it++){
//					if(requestNewTask[it] != "" && LookTaskIsDone(requestNewTask[it])){		
//						PanelStatic.StaticBtnGameManager.RunOpenLoading(function() InRoom.GetInRoomInstantiate().TaskAcceptedAsID(requestNewTask[it]));
//					}
//				}
//			}
//		}
	}
	
	var openNPCGo : boolean = false;
	var openNpcID : String = "";
	private var openNPCint : int = 0 ;
	
	function returnTaskComplet(taskid : String){
		var i : int = 0;
		if(requestComplet != null){
			for(i=0; i<requestComplet.length; i++){
				if(requestComplet[i] == taskid){
					requestComplet[i] = "";
				}
			}
		}
		openNPCint += 1;
		openNPCGo = true;
		if(taskid == "111")
		{
			AllManage.UICLStatic.MainTW.ClickGoBtn("女战士蕾娜塔,点击GO按钮");
		}
		else if(taskid == "11")
		{
			AllManage.UICLStatic.MainTW.ClickGoBtn("装备护甲,点击GO按钮");
		}
	
		if(! taskid == "23"){
			AllManage.tipclStatic.ShowTextButton(AllManage.AllMge.Loc.Get("info888") ,"" , "" , AllManage.AllMge.Loc.Get( "info889" ) , AllManage.UICLStatic.gameObject , "BagMoveOn");
		}
		playerS.CompletTask();
		var task : MainTask;
		task = MainTW.GetTaskAsId(taskid);
		if(parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) <= 10){
			//TD_info.taskSuccess(task.taskName);
		}
		if(task.leixing == 4){
			UpdateEveryDayActivity(task.id);
		}
		AllManage.taskILStatic.NowDone(player.nowTask[GetPlayerNowTaskAsID(task.id)]);
		player.nowTask[GetPlayerNowTaskAsID(task.id)] = null;
		yield;
		var mm : boolean = false; 
		var OneTime : boolean = false;
		player = new MainPlayerStatus(); 
		player.nowTask = new Array(10);

		DBTaskStr = InventoryControl.yt.Rows[0]["Task"].YuanColumnText;
		DBCompletTask = InventoryControl.yt.Rows[0]["CompletTask"].YuanColumnText;
		DBGetPlace = InventoryControl.yt.Rows[0]["GetPlace"].YuanColumnText;
		DBEveryDayActivity = InventoryControl.yt.Rows[0]["EveryDayActivity"].YuanColumnText; 
		StrEveryDayActivity = DBEveryDayActivity.Split(FStr.ToCharArray());
		SetPlayer(DBTaskStr , DBCompletTask , DBGetPlace); 
		player.level = GetBDInfoInt("PlayerLevel" , 1);
		player.pro = GetBDInfoInt("ProID" , 0);
		OneTime = true;

		MainTW.TaskObjectMakerInit(player);
		MainTW.UICLShowTaskInfo();
		pppp = player;
		if(player.level == 1 && DBTaskStr == "" && DBCompletTask == "" && DBGetPlace == "" && Application.loadedLevelName == "Map111"){
			//MainTW.FirstDaoHang();
		}
		yield;
		if(FindWayGoOn && UIControl.mapType == MapType.zhucheng){
			for(i=0; i<player.nowTask.length; i++){
				if(player.nowTask[i] != null){				
					if(FindWayID == player.nowTask[i].id){
						FindWayGoOn = false;				
						MainTW.FindWay(player.nowTask[i]);
					}
				}
			}
		}
		yield;
		MainTW.UICLShowTaskInfo();
		MainTW.ShowDoneTaskName(task.taskName);
		InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.ComTaskNum, 1);
		InRoom.GetInRoomInstantiate().SetHonor(yuan.YuanPhoton.HonorType.ComTask, parseInt(task.id));
		pppp = player;
		MainTW.UICL.UpDateLevelShowZhuButtonsNoWait();
		MainTW.UICL.lookCanShowRealButtonAsIDActive(task.id);
		InventoryControl.yt.Rows[0]["AimFinshTaskNum"].YuanColumnText = (parseInt(InventoryControl.yt.Rows[0]["AimFinshTaskNum"].YuanColumnText) + 1).ToString();
		
		if(InventoryControl.yt.Rows[0]["Place"].YuanColumnText == ""){
			InRoom.GetInRoomInstantiate().IsSaveData();
		}
		nowTaskMainNpc = AllManage.npcclStatic.GetNPCAsID(task.mainNPC.id);
		if(nowTaskMainNpc && nowTaskMainNpc.gameObject != task.npc){
			nowTaskMainNpc.gameObject.SendMessage("SendJindu",task,SendMessageOptions.DontRequireReceiver);			
		}
		if(taskid == "479"){
			AllManage.jiaochengCLStatic.InstructorsGO();
		}
		
		var useopenNPCint : int = 0;
		useopenNPCint = openNPCint;
		while(openNPCGo){
			yield;
		}
//		print(useopenNPCint + "  ==111==  " + openNPCint);
//		print(openNpcID + "  ==222==  " + AllManage.UICLStatic.nowNPC.npcid);
		if(useopenNPCint == openNPCint ){		
			if( AllManage.UICLStatic.nowNPC != null && openNpcID == AllManage.UICLStatic.nowNPC.npcid){
				AllManage.UICLStatic.nowNPC.InitTaskNPCOn();			
			}else{
				AllManage.taskILStatic.FindWay();
			}
		}
	}
	
	function FirstClickGOBtn()
	{
		if(player.level == 1 && DBTaskStr == "" && DBCompletTask == "" && DBGetPlace == "" && Application.loadedLevelName == "Map111"){
        	MainTW.FirstDaoHang();
    	}
	}
	
	function CanAddTask() : boolean{
		var mm : boolean = false;
		for(var i =0; i<player.nowTask.length; i++){
			if(player.nowTask[i] == null){
				mm = true;
			}
		}
		return mm;
	}
	
	function GetPlayerNowTaskAsID(id : String) : int{
		for(var i=0; i<player.nowTask.length; i++){
			if(player.nowTask[i] != null){
				if(player.nowTask[i].id == id){
					return i;
				}
			}
		}
		return 0;
	}
	
	function GetPlayerDoneTaskIDLength(){
		if(player.doneTaskID == null)
			return 0;
			else
			return player.doneTaskID.length;
	}
	
	var nowTaskGo : MainTask;
	function setNowTask(task : MainTask){
		if(task){
			nowTaskGo = task;	
			FindWayID = nowTaskGo.id;
		}
	}
	
	function LookTaskIsDone(id : String) : boolean{
		for(var myID:String in player.doneTaskID ){
			if(myID == id){
				return false;
			}
		}
		return true;
	}
	
	function LookEveryDayActivityIsDone(id : String) : boolean{
		for(var myID:String in StrEveryDayActivity ){
			if(myID == id){
				return true;
			}
		}
		return false;
	}
	
	function LookisGoOn(){
		if(navMA.enabled){
			FindWayGoOn = true;
		}
	}
	
	function GoWayNow(){
		if(nowTaskGo != null && PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
			FindWayGoOn = true;
			AllManage.timeDJStatic = AllManage.timeDJStatic;
			for(var i=0; i<player.nowTask.length; i++){
				if(player.nowTask[i] != null){				
					if(FindWayID == player.nowTask[i].id){
						if(player.nowTask[i].jindu == 1){
							if(player.nowTask[i].task.doneType.Length > 2){
								if(AllManage.mtwStatic.GetTaskLevel( player.nowTask[i].task.doneType ) == 5){
									UseDaoHangMap = player.nowTask[i].task.doneType.Substring(4,2) + "1";
								}else{
									UseDaoHangMap = player.nowTask[i].task.doneType.Substring(4,3);
								}
								
								if(UseDaoHangMap != Application.loadedLevelName.Substring(3,3)){
									AllManage.timeDJStatic.Show(gameObject , "YesDaoHang" , "NoDaoHang" , 3 , "messages031" , "messages032" , "messages033" , false);
								}
							}else{
								UseDaoHangMap = player.nowTask[i].task.mainNPC.map;
								if(UseDaoHangMap != Application.loadedLevelName.Substring(3,3)){
									AllManage.timeDJStatic.Show(gameObject , "YesDaoHang" , "NoDaoHang" , 3 , "messages031" , "messages032" , "messages033" , false);
								}						
							}
						}else
						if(player.nowTask[i].jindu == 2){
							UseDaoHangMap = player.nowTask[i].task.mainNPC.map;
							if(UseDaoHangMap != Application.loadedLevelName.Substring(3,3)){
								AllManage.timeDJStatic.Show(gameObject , "YesDaoHang" , "NoDaoHang" , 3 , "messages031" , "messages032" , "messages033" , false);
							}
						}
					}
				}
			}
		}	
	}
	
	var navMA : NavMeshAgent;
	static var FindWayGoOn : boolean = false;
	static var FindWayID : String;
//	var timeDJ : TimeDaoJi;
	function GoWay(){
		if(nowTaskGo != null && PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
			if(navMA.enabled){
				var boolNV : boolean = false;
				FindWayGoOn = true;
				AllManage.timeDJStatic = AllManage.timeDJStatic;
				for(var i=0; i<player.nowTask.length; i++){
					if(player.nowTask[i] != null){				
						if(FindWayID == player.nowTask[i].id){
							if(player.nowTask[i].jindu == 1){
								if(player.nowTask[i].task.doneType.Length > 2){
//									UseDaoHangMap = player.nowTask[i].task.doneType.Substring(4,3);
								if(AllManage.mtwStatic.GetTaskLevel( player.nowTask[i].task.doneType ) == 5){
									UseDaoHangMap = player.nowTask[i].task.doneType.Substring(4,2) + "1";
								}else{
									UseDaoHangMap = player.nowTask[i].task.doneType.Substring(4,3);
								}
									if(UseDaoHangMap != Application.loadedLevelName.Substring(3,3)){
										if(!AllManage.InvclStatic.isBagFull()){
											boolNV = true;
											AllManage.timeDJStatic.Show(gameObject , "YesDaoHang" , "NoDaoHang" , 3 , "messages031" , "messages032" , "messages033" , false);
											MainTW.setnowMenBool(true);
										}else{
//											AllManage.qrStatic.ShowQueRen(gameObject,"","","info885");
											AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info885"));
											return;
										}
									}
								}
							}else
							if(player.nowTask[i].jindu == 2){
								UseDaoHangMap = player.nowTask[i].task.mainNPC.map;
								if(UseDaoHangMap != Application.loadedLevelName.Substring(3,3)){
									if(!AllManage.InvclStatic.isBagFull()){
										boolNV = true;
										AllManage.timeDJStatic.Show(gameObject , "YesDaoHang" , "NoDaoHang" , 3 , "messages031" , "messages032" , "messages033" , false);
										MainTW.setnowMenBool(true);
									}else{
//										AllManage.qrStatic.ShowQueRen(gameObject,"","","info885");
										AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info885"));
										return;
									}
								}
							}
						}
					}
				}
				if( !boolNV){
					try{					
						MainTW.OpenChuanSongMen(player , 0);				
					}catch(e){
					
					}
				}
			}else{
				yield;
				yield;
				yield;
				MainTW.OpenChuanSongMen(player , 0);
			}
		}
	}
	
	private var UseDaoHangMap : String;
	var StrID : String;
	function YesDaoHang(){
	var costPower : int = 0;
		for(var i=0; i<player.nowTask.length; i++){
			if(player.nowTask[i] != null){				
				if(FindWayID == player.nowTask[i].id){
					if(player.nowTask[i].jindu == 1){
						if(player.nowTask[i].task.doneType.Length > 2){
//							UseDaoHangMap = player.nowTask[i].task.doneType.Substring(4,3);
								if(AllManage.mtwStatic.GetTaskLevel( player.nowTask[i].task.doneType ) == 5){
									UseDaoHangMap = player.nowTask[i].task.doneType.Substring(4,2) + "1";
								}else{
									UseDaoHangMap = player.nowTask[i].task.doneType.Substring(4,3);
								}
								if(UseDaoHangMap != Application.loadedLevelName.Substring(3,3)){
								
//								costPower = AllResources.GetMapCostPower(UseDaoHangMap , 1);
								
//								if(parseInt(ps.Power) >= costPower || UseDaoHangMap.Substring(0,1) == "1"){
									Loading.Level = "Map" + UseDaoHangMap;
									if(UIControl.mapType == MapType.zhucheng)
									DungeonControl.ReLevel = Application.loadedLevelName;
									if(player.nowTask[i].task.doneType.Length > 2){
										DungeonControl.NowMapLevel = AllManage.mtwStatic.GetTaskLevel( player.nowTask[i].task.doneType );//1;//parseInt(player.nowTask[i].task.doneType.Substring(13,1));
										Loading.nandu = AllManage.mtwStatic.GetTaskLevel( player.nowTask[i].task.doneType ).ToString();//"1"; //player.nowTask[i].task.doneType.Substring(13,1);
									}
									
									if(Loading.nandu == ""){
										Loading.nandu = "1";
									}
									if(parseInt(Loading.nandu) == 5){
										AllManage.AllMge.TipsNewPower(yuan.YuanPhoton.CostPowerType.EliteDungeon , parseInt(Loading.nandu) , 0 , "" , gameObject , "YesDaoHangTipsPower");		
									}else{
										AllManage.AllMge.TipsNewPower(yuan.YuanPhoton.CostPowerType.NormalDungeon , parseInt(Loading.nandu) , 0 , "" , gameObject , "YesDaoHangTipsPower");					
									}
									
//									InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
//									alljoy.DontJump = true;
//									yield;
//									PhotonNetwork.LeaveRoom();
//									InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
//			                        AllResources.ar.AllLoadLevel("Loading 1");
//			                  	}else{
//			                  	
//									var Istrue : boolean = false;
//									var StrItem = InventoryControl.yt.Rows[0]["Item"].YuanColumnText;
//									var str : String[] = StrItem.Split(";"[0]); 
//									for(var j= 0 ; j<str.Length ; j++)
//									{
//										if(str[j]!="")
//
//										if(str[j].Substring(0,3).Equals("884")){
//										StrID = str[j];
//										Istrue = true;
//										AllManage.qrStatic.ShowQueRen(gameObject,"UseDaoju" , "","info1119");
//										}
//
//										if(!Istrue){
//										AllManage.qrStatic.ShowQueRen(gameObject,"UseShangdian" , "","info1120");
//										}
//
//									}
//			                  	}
							}
						}
					}else
					if(player.nowTask[i].jindu == 2){
						UseDaoHangMap = player.nowTask[i].task.mainNPC.map;
						if(UseDaoHangMap != Application.loadedLevelName.Substring(3,3)){
//							if(parseInt(ps.Power) >= costPower || UseDaoHangMap.Substring(0,1) == "1"){
								Loading.Level = "Map" + UseDaoHangMap; 
								if(UIControl.mapType == MapType.zhucheng)
								DungeonControl.ReLevel = Application.loadedLevelName;
								if(player.nowTask[i].task.doneType.Length > 2){
									DungeonControl.NowMapLevel = AllManage.mtwStatic.GetTaskLevel( player.nowTask[i].task.doneType );//1;//parseInt(player.nowTask[i].task.doneType.Substring(13,1));
									Loading.nandu = AllManage.mtwStatic.GetTaskLevel( player.nowTask[i].task.doneType ).ToString();//"1"; //player.nowTask[i].task.doneType.Substring(13,1);
								}

								if(Loading.nandu == ""){
									Loading.nandu = "1";
								}
								if(parseInt(Loading.nandu) == 5){
									AllManage.AllMge.TipsNewPower(yuan.YuanPhoton.CostPowerType.EliteDungeon , parseInt(Loading.nandu) , 0 , "" , gameObject , "YesDaoHangTipsPower");		
								}else{
									AllManage.AllMge.TipsNewPower(yuan.YuanPhoton.CostPowerType.NormalDungeon , parseInt(Loading.nandu) , 0 , "" , gameObject , "YesDaoHangTipsPower");					
								}
								
//								InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
//								alljoy.DontJump = true;
//								yield;
//								InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
//								PhotonNetwork.LeaveRoom();
//								AllResources.ar.AllLoadLevel("Loading 1");
			 
//			              	}else{
//						var StrItem1 = InventoryControl.yt.Rows[0]["Item"].YuanColumnText;
//						var str1 : String[] = StrItem1.Split(";"[0]); 
//						for(var k= 0 ; k<str.Length ; k++)
//						{
//						if(str1[k]!="")
//							
//							if(str1[k].Substring(0,3).Equals("884")){
//							StrID = str[k];
//							Istrue = true;
//							AllManage.qrStatic.ShowQueRen(gameObject,"UseDaoju" , "","info1119");
//							}
//					if(!Istrue){
//							AllManage.qrStatic.ShowQueRen(gameObject,"UseShangdian" , "","info1120");
//					}
//							
//						}
//			                }
						}
					}
				}
			}
		}
	}
	
	function YesDaoHangTipsPower(isBool : boolean){
		if(isBool){
			InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
			alljoy.DontJump = true;
			yield;
			PhotonNetwork.LeaveRoom();
			InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			AllResources.ar.AllLoadLevel("Loading 1");			
		}else{
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
	}
	
	function UseDaoju()
{
		AllManage.InvclStatic.UseDaojuAsID(StrID);
}
	function UseShangdian()
	{
		AllManage.UICLStatic.StoreOpenMoveOn();
	}
		public function desP(){
		while(PhotonNetwork.isMasterClient){
			yield;
		}
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
	}

	private var nowTriggerLL : TriggerLoadLevel;
	function SetNowPortal(nowPortal : TriggerLoadLevel){
		nowTriggerLL = nowPortal;
	}

	function NoDaoHang(){
		if(nowTriggerLL && nowTriggerLL.bool){
			MainTW.OpenChuanSongMen(player , 0);			
		}
	}
	
	function GoWay1(){
		if(navMA.enabled){
			GoWay();
			return;
		}
		if(nowTaskGo != null && PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
			if(navMA.enabled){
				FindWayGoOn = true;
			}
//			//print(nowTaskGo.doneType.Substring(3,2) + " dao le di tu");
			yield;
			yield;
			yield;
			MainTW.OpenChuanSongMen(player , 1);
//			Application.LoadLevel(parseInt(nowTaskGo.doneType.Substring(3,2)) -1);
		}
	}
	
	function returnLevel(){
		if(PlayerUtil.isMine(GetComponent(PlayerStatus).instanceID)){
			if(navMA.enabled){
				FindWayGoOn = true;
			}
//		    PhotonNetwork.LeaveRoom();
			Loading.Level = DungeonControl.ReLevel;
	AllManage.UICLStatic.RemoveAllTeam();
		InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
	alljoy.DontJump = true;
	yield;
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
			PhotonNetwork.LeaveRoom();
	        	AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");

		}
	}
}
