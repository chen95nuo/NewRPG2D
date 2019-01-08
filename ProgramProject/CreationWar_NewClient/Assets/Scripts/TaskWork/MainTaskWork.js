
enum TaskValueType{
	info = 0,
	monster = 1,
	item = 2,
	done = 3,
	killer = 4,
	win = 5,
}

class TaskValue{
	var valueType : TaskValueType = 0;
	var num : int;
}
class GameValue{
	var task : MainTask;
	var mtw : MainTaskWork;
	var npcid : String;
}
 
class ProducedObject{
	var task : MainTask = null;
	var type : ProducedObjectType;
	var spawnObj : GameObject;
}
enum ProducedObjectType{
	npc = 0,
	monster = 1,
	item = 2
}

class	MainTaskWork	extends	XmlControl
{
	var	thisMapID		: String;
	static	var	MapID	: String; 
	
	var myTask : MainTask[]; 
	
	var sceneMaker : SceneTaskObjectMaker;
	var MainPS : MainPersonStatus;
	var UICL : UIControl; 
	var taskinfolist :   TaskInfoList;
	private var photonView : PhotonView;

	function	Awake()
	{
		AllManage.mtwStatic = this;
		thisMapID = Application.loadedLevelName.Substring(3,3);
		MapID = thisMapID;
		GameReonline.mapID = MapID;
	}
	
	class	DuplicateEva
	{
		var map : String;
		var xingxing : int = 0;
	}
	
	var DuplicateEvaNormal : DuplicateEva[];
	var DuplicateEvaElite : DuplicateEva[];
	var DuplicateEvaDungeon : DuplicateEva[];
	
	function Start(){   
		LevelJilu = new Array(3);
		var obj  = GameObject.Find("end");
		if(obj){
			endObj = 	obj.transform;
		}
		
//		npccl = AllManage.npcclStatic;
				
		var useStrs : String[];
		var useMapStrs : String[];
		var i : int = 0;
		useStrs = InventoryControl.yt.Rows[0]["DuplicateEvaNormal"].YuanColumnText.Split(Fstr.ToCharArray());
		DuplicateEvaNormal = new Array(useStrs.length);
		for(i=0; i<useStrs.length; i++){
			if(useStrs[i].Length > 1){
				useMapStrs = useStrs[i].Split(Dstr.ToCharArray());
				if(useMapStrs.Length > 1){
					DuplicateEvaNormal[i] = new DuplicateEva();
					DuplicateEvaNormal[i].map = useMapStrs[0];
					DuplicateEvaNormal[i].xingxing = parseInt(useMapStrs[1]);
				}
			}
		}
		useStrs = InventoryControl.yt.Rows[0]["DuplicateEvaElite"].YuanColumnText.Split(Fstr.ToCharArray());
		DuplicateEvaElite = new Array(useStrs.length);
		for(i=0; i<useStrs.length; i++){
			if(useStrs[i].Length > 1){
				useMapStrs = useStrs[i].Split(Dstr.ToCharArray());
				if(useMapStrs.Length > 1){
					DuplicateEvaElite[i] = new DuplicateEva();
					DuplicateEvaElite[i].map = useMapStrs[0];
					DuplicateEvaElite[i].xingxing = parseInt(useMapStrs[1]);
				}
			}
		}
		
				useStrs = InventoryControl.yt.Rows[0]["DuplicateEvaDungeon"].YuanColumnText.Split(Fstr.ToCharArray());
		DuplicateEvaDungeon = new Array(useStrs.length);
		for(i=0; i<useStrs.length; i++){
			if(useStrs[i].Length > 1){
				useMapStrs = useStrs[i].Split(Dstr.ToCharArray());
				if(useMapStrs.Length > 1){
					DuplicateEvaDungeon[i] = new DuplicateEva();
					DuplicateEvaDungeon[i].map = useMapStrs[0];
					DuplicateEvaDungeon[i].xingxing = parseInt(useMapStrs[1]);
				}
			}
		}
		
		var chuansongmen : TriggerLoadLevel;
		while(chuansongObj == null){
			chuansongmen = TriggerLoadLevel.chuansongmen;
			if(chuansongmen)
				chuansongObj = chuansongmen.transform;
		
			if(TriggerLoadLevel.chuansongmen1)
				chuansongObj1 = TriggerLoadLevel.chuansongmen1.transform;
			yield;
		}
//		print(chuansongObj + " == " + chuansongObj1);
	}
	
	function TaskCreateRobot(){
		for(var i=0; i<myTask.length; i++){
			if(myTask[i].taskType == MainTaskType.Robot){
//				print(myTask[i].jindu + " == myTask[i].jindu");
				if(myTask[i].doneType.Substring(4,3) == MapID && myTask[i].jindu == 1){
					AllManage.jiaochengCLStatic.CreateRobot();
				}
			}
		}	
	}
	
	var PlayerUCL : PlayerUIControl;
	function UICLShowTaskInfo(){
		if(MainPS.player != null){		
			 UICL.ShowTaskInfoList(MainPS.player,null);
		}
		PlayerUCL.playerS = MainPS.playerS;
	}
	
	function TriggerInfoValue(task : MainTask, type : TaskValueType){
		if(MainPS.playerC.enabled == false){
			TaskInfoValue(task,type);
		}
	}
	
	var FirstDaoHangObj : Transform;
	
	@HideInInspector
	var beginnersGuide : GameObject;
	
	var guidePanelPos0 : Transform;
	function FirstDaoHang(){
		
		if(null == beginnersGuide)
        {
		    CreateBeginnerGuid();
		    BeginnersGuide.beginnersGuide.SetSpriteAlpha(1);
		    
		    //if (Screen.height / Screen.width >= 0.75)
		    //{
		    //    guidePanelPos0.localPosition = Vector3(340, 260, 0);
		    //}
		    //else
		    //{
		    //    guidePanelPos0.localPosition = Vector3(250, 300, 0);
		    //}
	        BeginnersGuide.beginnersGuide.SwitchInstructionsBoxStyle(InstructionsBoxStyle.RightTop,guidePanelPos0, StaticLoc.Loc.Get("buttons066"), AllManage.buttonMessCL.GetTargetAsName("Button - SelectInfo"),"OnClick", yuan.YuanPhoton.GameScheduleType.FirstClickGO);
	        BeginnersGuide.beginnersGuide.SetTDString("第一次点GO");
        }
	}
	
	function ClickGoBtn(str : String)
	{
//		if(null == beginnersGuide)
//		{
//		    CreateBeginnerGuid();
//	    }
//		BeginnersGuide.beginnersGuide.SwitchInstructionsBoxStyle(InstructionsBoxStyle.RightTop,guidePanelPos0, StaticLoc.Loc.Get("buttons066"), AllManage.buttonMessCL.GetTargetAsName("Button - SelectInfo"),"OnClick", yuan.YuanPhoton.GameScheduleType.FirstClickGO);
//		BeginnersGuide.beginnersGuide.SetTDString(str);
	}
	
	function CreateBeginnerGuid()
	{
	    beginnersGuide = Instantiate(Resources.Load("Anchor - BeginnersGuide", GameObject));
	    beginnersGuide.transform.parent = FirstDaoHangObj.transform;
	    beginnersGuide.transform.localPosition = Vector3.zero;
	    beginnersGuide.transform.localRotation = Quaternion.identity;
	    beginnersGuide.transform.localScale = Vector3.one;
	}

	function TaskInfoValue(task : MainTask, type : TaskValueType){ 
		for(var i=0; i<myTask.length; i++){
			if(myTask[i].id == task.id){
				switch(type){
					case TaskValueType.info :
						Step = 0;
						isItemNPC = false;
						myTask[i].jindu = task.jindu;
						OneTask = myTask[i]; 
						TaskInfoStepStart();
						break;
					case TaskValueType.monster : 
						OneTask = myTask[i];
						MainPS.AddNewDoneObject(task); 	
						UICL.ShowTaskInfoList(MainPS.player,OneTask);
//						var p : PrivateTask;
//						p = MainPS.GetPrivateTask(task.id); 
//						if(p != null){
//							AllManage.tsStatic.Show1( "("+UIControl.taskLeixingStrs[p.task.leixing] + ")" + p.task.taskName + "  " +p.doneNum + "/" + p.task.doneNum);						
//						}
						break;
					case TaskValueType.item : 
						Step = 0; 
						isItemNPC = true;
						OneTask = myTask[i];
						TaskInfoStepStart();
						break;
					case TaskValueType.done :
						TaskDone(myTask[i]);
						Step = 0;
						isItemNPC = false;
						break; 
					case TaskValueType.killer :
						break;
					case TaskValueType.win :
						break;						
				}
			}
		}
	}
	
	function TaskInfoValue(tasks : MainTask[], type : TaskValueType){ 
		for(var task : MainTask in tasks){		
			for(var i=0; i<myTask.length; i++){
				if(myTask[i].id == task.id){
					switch(type){
						case TaskValueType.info :
							break;
						case TaskValueType.item : 
							break;
					}
				}
			}
		}
	}
	
	function DoneJiaoCheng(JCID : int){
		for(var i=0; i<myTask.length; i++){
			if(myTask[i].taskType == MainTaskType.jiaocheng){
				if(myTask[i].doneType == JCID.ToString()){
						OneTask = myTask[i];
						MainPS.AddNewDoneObject(OneTask); 	
						UICL.ShowTaskInfoList(MainPS.player,OneTask);					
				}
			}
		}	
	}
	
	function DoneDungeon(mapID : String , mapLevel : int){
		for(var i=0; i<myTask.length; i++){
			if(myTask[i].taskType == MainTaskType.fuben || myTask[i].taskType == MainTaskType.Robot){
				if(GetTaskLevel(myTask[i].doneType) == 5){
					if(myTask[i].doneType.Substring(4,3) == mapID && mapLevel == 5){
							OneTask = myTask[i];
							MainPS.AddNewDoneObject(OneTask); 	
							UICL.ShowTaskInfoList(MainPS.player,OneTask);					
					}					
				}else{
					if(myTask[i].doneType.Substring(4,3) == mapID){
							OneTask = myTask[i];
							MainPS.AddNewDoneObject(OneTask); 	
							UICL.ShowTaskInfoList(MainPS.player,OneTask);					
					}
				}
			}
		}
	}
	
	function DoneKiller(){
		for(var i=0; i<myTask.length; i++){
			if(myTask[i].taskType == MainTaskType.PVPKiller){
				if(myTask[i].doneType.Substring(4,3) == thisMapID){
						OneTask = myTask[i];
						MainPS.AddNewDoneObject(OneTask); 	
						UICL.ShowTaskInfoList(MainPS.player,OneTask);					
						var p : PrivateTask;
						p = MainPS.GetPrivateTask(OneTask.id); 
						if(p != null){
							AllManage.tsStatic.Show1( "("+UIControl.taskLeixingStrs[p.task.leixing] + ")" + p.task.ComplateInfo + "  " +p.doneNum + "/" + p.task.doneNum);						
						}
				}
			}
		}	
	} 
	
	function DoneWin(){
		for(var i=0; i<myTask.length; i++){
			if(myTask[i].taskType == MainTaskType.PVPWin){
				if(myTask[i].doneType.Substring(4,3) == thisMapID){
						OneTask = myTask[i];
						MainPS.AddNewDoneObject(OneTask); 	
						UICL.ShowTaskInfoList(MainPS.player,OneTask);					
						var p : PrivateTask;
						p = MainPS.GetPrivateTask(OneTask.id); 
						if(p != null){
							AllManage.tsStatic.Show1( "("+UIControl.taskLeixingStrs[p.task.leixing] + ")" + p.task.ComplateInfo + "  " +p.doneNum + "/" + p.task.doneNum);						
						}
				}
			}
		}	
	}
	
	function DoneWaKuang(KuangID : String){
		for(var i=0; i<myTask.length; i++){
			if(myTask[i].taskType == MainTaskType.WaKuang){
				if(myTask[i].doneType.Substring(0,4) == KuangID){
						OneTask = myTask[i];
						MainPS.AddNewDoneObject(OneTask); 	
						UICL.ShowTaskInfoList(MainPS.player,OneTask);			
						var p : PrivateTask;
						p = MainPS.GetPrivateTask(OneTask.id); 
						if(p != null){
							AllManage.tsStatic.Show1( "("+UIControl.taskLeixingStrs[p.task.leixing] + ")" + p.task.ComplateInfo + "  " +p.doneNum + "/" + p.task.doneNum);						
						}
				}
			}
		}
	}
	
	function TaskDone(task : MainTask){
		if(task.npc != null){
			task.jindu = 2;
			task.npc.SendMessage("SendJindu",task,SendMessageOptions.DontRequireReceiver);
		}
	}
	
	function TaskInfoStepStart(){
		switch(OneTask.jindu){
			case 0 : 
				stepIsServer = false;
				InTaskInfoStep();
				break; 
			case 1 : 
				if(!isItemNPC){
					UICL.TaskInfoKuangOn(OneTask.dialog[OneTask.dialog.length - 2] , OneTask , true,1); 
				}
				else{
					stepIsServer = false;
					InTaskInfoStep();
				}
				break;
			case 2 :
				if(Step == OneTask.dialog2.length - 2){
					UICL.TaskInfoKuangOn(OneTask.dialog2[Step],OneTask,false,2);				
				}else{
					UICL.TaskInfoKuangOn(OneTask.dialog2[Step],OneTask,false,0);								
				}
				break;
		}
	}
	
	var stepValue : String = "";
	var stepIsServer : boolean = false;
	var Step : int;
	var OneTask : MainTask;
	var isItemNPC : boolean = false;
	var TaskJiaoChengC : TaskJiaoChengControl;
	var isGUanBi : boolean = false;
	var serverNowTaskID : String;
	function InTaskInfoStep(){
		stepValue = "StepZero";
		UICL.ShowFindWay(OneTask);
		if(Step >= OneTask.dialog.length-1 && OneTask.jindu == 0 && !isItemNPC){
			if(!MainPS.CanAddTask()){
				AllManage.tsStatic.Show("tips072.");
				return;
			}
			if(! stepIsServer){
				stepValue = "StepZero";				
				MainPS.AddNewTask(OneTask , 0);
				return;
			}else
			if(stepIsServer){
				MainPS.returnAddNewTask(serverNowTaskID);
				if(serverNowTaskID != OneTask.id){
					return;
				}
			}
			
			UICL.TaskKuangBack();
			OneTask.jindu = 1; 
			if(OneTask.npc == null){
				OneTask.npc = AllManage.npcclStatic.GetNPCAsID(OneTask.mainNPC.id).gameObject;
			}
			OneTask.npc.SendMessage("SendJindu",OneTask , SendMessageOptions.DontRequireReceiver);
			UICL.ShowTaskInfoList(MainPS.player,OneTask);
			if(OneTask.taskType == MainTaskType.jiaocheng){
				UICL.lookCanShowRealButtonAsID(OneTask.id);
				TaskJiaoChengC.SelectJiaoChengAsID(parseInt(OneTask.doneType));
			}else
			if(OneTask.taskType == MainTaskType.duihua){
				var gv = new GameValue();
				gv.task = OneTask;
				gv.mtw = this;
				gv.npcid = GetItemIDAsLevel(OneTask.doneType.Substring(0,4));
				OneTask.npc = AllManage.npcclStatic.SetNPCTask(gv);		
				AllManage.npcclStatic.NPCGuanBi(OneTask.mainNPC.id);
			}else
			if(OneTask.taskType == MainTaskType.PVPKiller || OneTask.taskType == MainTaskType.PVPWin){
				InRoom.GetInRoomInstantiate().ActivityPVPAdd(OneTask.doneType.Substring(4,3));
				PVPControl.PVPTaskNanDu = AllManage.mtwStatic.GetTaskLevel( OneTask.doneType );//1; //parseInt(OneTask.doneType.Substring(13,1))
				AllManage.tsStatic.ShowBlue("tips101");
			}
			UICL.FindNpcOtherTask();
			return;
		}else{
			if(OneTask.dialog2 != null){
				if(Step >= OneTask.dialog2.length-1 && OneTask.jindu == 1 && isItemNPC){
						if(UICL.NowNPCID == GetItemIDAsLevel(OneTask.doneType.Substring(0,4))){
							if(! stepIsServer){
								stepValue = "StepZero";				
								MainPS.TaskDone(OneTask); 
								return;
							}else
							if(stepIsServer){
								MainPS.returnTaskComplet(serverNowTaskID);
								if(serverNowTaskID != OneTask.id){
									return;
								}
							}
							
							OneTask.jindu += 2; 
							Step = 0; 
							isItemNPC = false;
							if(OneTask.npc == null){
								OneTask.npc = AllManage.npcclStatic.GetNPCAsID(OneTask.mainNPC.id).gameObject;
							}
							OneTask.npc.SendMessage("SendJindu",OneTask , SendMessageOptions.DontRequireReceiver);
							UICL.TaskKuangBack();
							UICL.ShowTaskInfoList(MainPS.player,OneTask);		 
							UICL.FindNpcOtherTask();
						}else{
							if(isGUanBi){
								UICL.TaskKuangBack();
								isGUanBi = false;
							}else{
								UICL.TaskInfoKuangOn(OneTask.dialog[OneTask.dialog.length - 2],OneTask,true,1); 									
							}
							UICL.FindNpcOtherTask();
						}
					return;
				}			
			}		
		}
		switch(OneTask.jindu){
			case 0 : 
					if(Step == OneTask.dialog.length - 2){
						UICL.TaskInfoKuangOn(OneTask.dialog[Step],OneTask,true,1); 					
					}else{
						UICL.TaskInfoKuangOn(OneTask.dialog[Step],OneTask,false,0); 										
					}
					Step += 1;
					if(Step >= OneTask.dialog.length-1 && !isItemNPC){
						UICL.ShowFindWay(OneTask);
					}
				break; 
			case 1 :
				if(!isItemNPC){
					UICL.TaskKuangBack();
					UICL.FindNpcOtherTask();
				}
				else{
					if(Step == OneTask.dialog2.length - 2){
						if(UICL.NowNPCID == GetItemIDAsLevel(OneTask.doneType.Substring(0,4))){
							UICL.TaskInfoKuangOn(OneTask.dialog2[Step],OneTask,false,2);					
						}else{
							UICL.TaskInfoKuangOn(OneTask.dialog[OneTask.dialog.length - 2],OneTask,true,1);											
						}
					}else{
						UICL.TaskInfoKuangOn(OneTask.dialog2[Step],OneTask,false,0);
					}
					Step += 1;				
				}
				break;
			case 2 :
				if(OneTask.taskType == MainTaskType.wupin){
					if(GetAndUseTaskItem(OneTask.doneItem.Substring(0,4) , OneTask.doneNum , OneTask.id)){ 
						if(! stepIsServer){
							stepValue = "StepZero";				
							MainPS.TaskDone(OneTask); 
							return;
						}else
						if(stepIsServer){
							MainPS.returnTaskComplet(serverNowTaskID);
							if(serverNowTaskID != OneTask.id){
								return;
							}
						}
						
						OneTask.jindu += 1; 
						DJshow(OneTask.taskName);
						Step = 0;
						isItemNPC = false;
						if(OneTask.npc == null){
							OneTask.npc = AllManage.npcclStatic.GetNPCAsID(OneTask.mainNPC.id).gameObject;
						}
						OneTask.npc.SendMessage("SendJindu",OneTask , SendMessageOptions.DontRequireReceiver);
						UICL.TaskKuangBack();							
						UICL.FindNpcOtherTask();
					}
				}else{
					if(! stepIsServer){
						stepValue = "StepZero";				
						MainPS.TaskDone(OneTask);
						return;
					}else
					if(stepIsServer){
						MainPS.returnTaskComplet(serverNowTaskID);
						if(serverNowTaskID != OneTask.id){
							return;
						}
					}
					
					OneTask.jindu += 1; 
					Step = 0;
					isItemNPC = false;
					if(OneTask.npc == null){
						OneTask.npc = AllManage.npcclStatic.GetNPCAsID(OneTask.mainNPC.id).gameObject;
					}
					OneTask.npc.SendMessage("SendJindu",OneTask , SendMessageOptions.DontRequireReceiver);
					UICL.TaskKuangBack();			
					UICL.FindNpcOtherTask();
			}
				break;
		}
		UICL.ShowTaskInfoList(MainPS.player,OneTask);
	}

		function itemTaskInfoStepStart(){
		switch(OneTask.jindu){
			case 0 : 
				stepIsServer = false;
				InTaskInfoStep();
				break; 
			case 1 :
				UICL.TaskInfoKuangOn(OneTask.dialog[OneTask.dialog.length - 2],OneTask,true,1);
				break;
			case 2 :
				UICL.TaskInfoKuangOn(OneTask.dialog[OneTask.dialog.length - 2],OneTask,false,2);
				break;
		}
	}
	
	function TaskObjectMakerDes(){
		var intis : SceneTaskObjectMaker[];
		intis = FindObjectsOfType(SceneTaskObjectMaker);
		for(var i=0; i<intis.length; i++){
			if(intis[i] != null){
				Destroy(intis[i].gameObject);
			}
		}
	}
	
	var producedTaskIDs : Array = new Array();
	public function TaskObjectMakerInit(player : MainPlayerStatus){
		TaskObjectMakerDes();
		var i : int = 0;
		var obj : GameObject;
		var j : int = 0;
		var useTaskShow : MainTask = null;
		SceneTaskInit(player);
		myTask = new Array(SceneTaskList.length);
		for(i=0; i<SceneTaskList.length; i++){   
			myTask[i] = GetTaskAsId(SceneTaskList[i]);
//			if(PlayerStatus.MainCharacter){
//				switch(PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus).ProID){
//					case 1: 
//						myTask[i].reward = myTask[i].RewardItemSoldier;
//						break;
//					case 2: 
//						myTask[i].reward = myTask[i].RewardItemRobber;
//						break;
//					case 3: 
//						myTask[i].reward = myTask[i].RewardItemMaster;
//						break;
//				}
//			}
			if(TaskIsNotDone(myTask[i] , player) && ((MapID == myTask[i].mainNPC.map && myTask[i].jindu == 0) || (MapID == myTask[i].mainNPC.map && myTask[i].jindu == 10))){  
				sceneMaker.produced = new ProducedObject(); 
				sceneMaker.produced.task = new MainTask();
				sceneMaker.produced.task = myTask[i]; 
				sceneMaker.produced.type = ProducedObjectType.npc;
				obj = Instantiate(sceneMaker.gameObject); 
				obj.transform.position = AllManage.npcclStatic.GetNPCPosition(myTask[i].mainNPC.id);
				obj.name = "npc_" + obj.name; 
				var zhiyin : boolean = false;
				for(var m=0; m<player.nowTask.length; m++){
					if(player.nowTask[m] != null){
						zhiyin = true;
					}
				}
			}
			if(useTaskShow == null && MainPS.LookTaskIsDone(myTask[i].id)){
//			print(useTaskShow.taskName);
				useTaskShow = myTask[i];		
			}else{
				if(myTask[i].leixing == 0 && MainPS.LookTaskIsDone(myTask[i].id) && myTask[i].mainNPC.map == thisMapID){
//			print(useTaskShow.taskName);
					useTaskShow = myTask[i];
				}
			}
		}
		if(!zhiyin && useTaskShow != null && useTaskShow.leixing != 3){
//			print(useTaskShow.taskName);
			UICL.ShowCanGetTaskInfo(useTaskShow);	
		}
		for(i=0; i<player.nowTask.length; i++){
			if(player.nowTask[i] != null){
				j = GetMyTaskTaskAsID(player.nowTask[i].id);
				switch(player.nowTask[i].jindu){
					case 1 : 
						if(player.nowTask[i].task.taskType != MainTaskType.fuben && player.nowTask[i].task.taskType != MainTaskType.wupin){
							var boolProduce : boolean = false;
							for(var s=0; s<producedTaskIDs.Count; s++){
								if(producedTaskIDs[s] == player.nowTask[i].id)
									boolProduce = true;
							}
							if(!boolProduce){
								TaskObjectProduce(player,player.nowTask[i].id);		
								producedTaskIDs.Add(player.nowTask[i].id);
							}
						}
						break;
					case 2 :   
						if(AllManage.npcclStatic != null){
							if(AllManage.npcclStatic.GetNPCAsID(player.nowTask[i].task.mainNPC.id) != null){
								sceneMaker.produced = new ProducedObject(); 
								sceneMaker.produced.task = new MainTask(); 
								myTask[j].jindu = player.nowTask[i].jindu;
								sceneMaker.produced.task = myTask[j]; 
								sceneMaker.produced.type = ProducedObjectType.npc;
								obj = Instantiate(sceneMaker.gameObject); 
								obj.transform.position = AllManage.npcclStatic.GetNPCPosition(player.nowTask[i].task.mainNPC.id);
								obj.name = "npc_" + obj.name;		
							}
						}
							break;
				}
			}
		}
		PopInit = true;
	}
	
	
	var PopInit : boolean = false;
	var os : ObjectSpawn[];
	function ProducePop(id : String , i : int){
		if(os.length > 1){
			os[i].Popid = id;
		}
	}
	
	function TaskIsNotDone(task : MainTask,player : MainPlayerStatus) : boolean{ 
		var i = 0; 
		var j = 0;
		if(player.doneTaskID != null){
			for(i=0; i<player.doneTaskID.length; i++){
				if(player.doneTaskID[i] == task.id && task.leixing != 5 && task.leixing != 4){
					return false;
				}
				if((task.leixing == 4 && MainPS.LookEveryDayActivityIsDone( task.id))){
					return false;
				}
			}
		}
		for(i=0; i<player.nowTask.length; i++){ 
			if(player.nowTask[i] != null){ 
				j = GetMyTaskTaskAsID(task.id);
				if(player.nowTask[i].id == task.id){
					myTask[j].jindu = player.nowTask[i].jindu;
				}			
			}
		}
		return true;
	}
	
	function GetMyTaskTaskAsID(id : String) : int{
		for(var i=0; i<myTask.length; i++){
			if(myTask[i] != null){
				if(myTask[i].id == id){
					return i;
				}
			}
		}
		return 0;
	}
	
	var mspn : MonsterSpawn[];
	public	function	TaskObjectProduce(	player : MainPlayerStatus,	thisID : String	)
	{ 
		mspn = new Array();
		if(	mspn.length == 0	)
		{
			mspn = FindObjectsOfType(MonsterSpawn);
		} 
		var	iscan : boolean = false;
		var n : int = 0;
		for(var i=0; i<player.nowTask.length; i++)
		{	//遍历任务//
			if(player.nowTask[i] != null){   
				if(player.nowTask[i].id == thisID){
					for(var m=0; m<myTask.length; m++){    
						if(myTask[m].doneType.Length > 3){
							if(myTask[m].id == thisID && MapID == myTask[m].doneType.Substring(4,3)){
								if(player.nowTask[i].task.taskType == MainTaskType.WaKuang){
									if(os.length < 1){
										os = FindObjectsOfType(ObjectSpawn);
									}
									for(n=0; n<myTask[m].doneNum; n++){
										ProducePop(myTask[m].doneType.Substring(0,4) , n);
									}
									return;
								}
								for(n=0; n<myTask[m].doneNum + 1; n++){
									sceneMaker.produced = new ProducedObject(); 
									sceneMaker.produced.task = new MainTask();
									sceneMaker.produced.task = myTask[m];  
									sceneMaker.produced.type = ProducedObjectType.npc;
									sceneMaker.produced.task.jindu = player.nowTask[i].jindu;
									
									if(sceneMaker.produced.task.taskType != MainTaskType.guaiwu){
										obj = Instantiate(sceneMaker.gameObject); 
										if(myTask[m].taskType == MainTaskType.duihua){
											obj.transform.position = AllManage.npcclStatic.GetNPCPosition(GetItemIDAsLevel(myTask[m].doneType.Substring(0,4)));
										}else{						
											obj.transform.position = Vector3(parseInt(myTask[m].doneType.Substring(7,3)),0,parseInt(myTask[m].doneType.Substring(10,3)));	 
										}
										obj.name = sceneMaker.produced.task.taskType +"_" + obj.name;			
										if(sceneMaker.produced.task.taskType == MainTaskType.daoda){
											var obj2 : GameObject;
											obj2 = Instantiate(dibiao);
											obj2.transform.parent = obj.transform;
											obj2.transform.localPosition = Vector3.zero;
										
										}
									}else{  
										iscan = true;
										var useRow : yuan.YuanMemoryDB.YuanRow;	
										
										for(var o=0; o<mspn.length; o++){
											useRow = dgcl.GetNMRowAsID(parseInt(myTask[m].doneType.Substring(0,4)).ToString());
											if(!mspn[o].isTask && iscan){  
												if(parseInt(useRow["MosterLevel"].YuanColumnText) < 3){
													if(mspn[o].spType == SpawnPointType.boss1 || mspn[o].spType == SpawnPointType.boss2){
														iscan = false;
														mspn[o].SetTaskObj(myTask[m] , sceneMaker.produced); 
														mspn[o].name += sceneMaker.produced.task.taskType +"_";	
													}
												}else
												if(mspn[o].spType == SpawnPointType.Enemy){
													iscan = false;
													try{
														mspn[o].SetTaskObj(myTask[m] , sceneMaker.produced); 
														mspn[o].name += sceneMaker.produced.task.taskType +"_";	
													}catch(e){
													
													}
												}
											}
										}
									}
								}
							}					
						}
					}
				}
			}
		}//For ==>> player.nowTask.length
	}
	
	private var prefab : GameObject;
	private var obj : GameObject;
	function CreateTaskObjectManager(task : MainTask , prd : ProducedObject){
//		//print(task);
		for(var i=0; i<myTask.length; i++){
			if(myTask[i].id == task.id){ 
				if(MapID == myTask[i].mainNPC.map){		
					myTask[i].npc = CreateNPC(task);

				}
				if( task.doneType.Length < 3){
					return;
				}
				if(MapID == task.doneType.Substring(4,3)){
				switch(task.jindu){
					case 1 :    
						switch(task.taskType){
							case 1:	
							    CreateMonster(task , prd);
								break;
							case 2:
								myTask[i].npc = CreateNPCLook(task);
								break;
							case 3:																
								OneTask = myTask[i];
								MainPS.AddNewDoneObject(OneTask); 	
								UICL.ShowTaskInfoList(MainPS.player,OneTask);					
//								var p : PrivateTask;
//								p = MainPS.GetPrivateTask(task.id); 
//								if(p != null){
//									AllManage.tsStatic.Show1( "("+UIControl.taskLeixingStrs[p.task.leixing] + ")" + p.task.taskName + "  " +p.doneNum + "/" + p.task.doneNum);						
//								}
								break;
							case 4:
								break;
							case 5:

								break;
							case 6:
								
								break;
						}
						break; 
				}
				}
			}
		}
		return false;
	} 

	function LookNowItemTask(){
		var i : int = 0; 
		if(MainPS){
			for(i=0; i<MainPS.player.nowTask.length; i++){  
				if(MainPS.player.nowTask[i] != null){
					if(LookItemTask(MainPS.player.nowTask[i].task.id)){
						DoneLookItem(MainPS.player.nowTask[i].task.id);
					}	
				}
			}		
		}
	}
	
		
	var invcl : InventoryControl;
	private var FStr : String = ";";
	private var Dstr : String = ",";
	var LookUsetask : MainTask; 
	function LookItemTask(taskid : String) : boolean{
		var i : int = 0;
		var m : int = 0;
		var invStr : String;
		var useInvID : String[];
		LookUsetask = GetTaskAsId(taskid);
		invStr = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText + InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText + InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText + InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
		useInvID = invStr.Split(FStr.ToCharArray());
			for(m=0; m<useInvID.length; m++){ 
				if(LookUsetask){
					if(LookUsetask.doneItem != null){ 
						if(useInvID[m].Length > 4){
							if(LookUsetask.doneItem.Substring(0,4) == useInvID[m].Substring(0,4)){
								MainPS.ShowShouJiTaskNum(taskid , parseInt(useInvID[m].Substring(5,2)));
								UICL.ShowTaskInfoList(MainPS.player,LookUsetask);
								if(parseInt(useInvID[m].Substring(5,2)) >= LookUsetask.doneNum){
									return true;
								}
							}
						}
					}				
				}
			}
		return false;
	}
	
	function LookItemTask(taskid : String , ItemID : String) : boolean{
		var i : int = 0;
		var m : int = 0;
		var invStr : String;
		var useInvID : String[];
		LookUsetask = GetTaskAsId(taskid);
			if(LookUsetask != null){
						if(ItemID.Length > 4 && LookUsetask.doneItem != null){
							if(LookUsetask.doneItem.Substring(0,4) == ItemID.Substring(0,4)){
								var p : PrivateTask;
								p = MainPS.GetPrivateTask(taskid);  
								if(p != null && p.task.jindu < 2){
									AllManage.tsStatic.Show1( "("+UIControl.taskLeixingStrs[p.task.leixing] + ")" + p.task.ComplateInfo + "  " +parseInt(ItemID.Substring(5,2)) + "/" + p.task.doneNum);						
								}
							}
						}
			
			}
		return false;
	}
	function JianChaItem(ItemID : String){
		var i : int = 0; 
		if(MainPS){
			for(i=0; i<MainPS.player.nowTask.length; i++){  
				if(MainPS.player.nowTask[i] != null){
					if(LookItemTask(MainPS.player.nowTask[i].task.id , ItemID)){
					}	
				}
			}		
		}	
	}
//	var timeDJ : TimeDaoJi;
	function DoneLookItem(taskID : String){
		for(var i=0; i<myTask.length; i++){
			if(myTask[i].taskType == MainTaskType.wupin){
				if(myTask[i].id == taskID && myTask[i].jindu != 2){ 
						OneTask = myTask[i];
						MainPS.AddNewDoneObject(OneTask); 	
						UICL.ShowTaskInfoList(MainPS.player,OneTask);	
				}
			}
		}
	}
	
	var ps : PlayerStatus;
	var particDoneTask : ParticleEmitter;
	function DJshow(str : String){ 
		yield WaitForSeconds(1);
//		AllManage.tsStatic.Show1( AllManage.AllMge.Loc.Get("tips076") + " " + str + " " + AllManage.AllMge.Loc.Get("tips077"));		
		particDoneTask.Emit();
//		//print("shi zhe li le");
//		if(ps != null){
//			while(ps.battlemod){
//				yield;
//			}
//		}
		if(UIControl.mapType == MapType.fuben){
			AllManage.timeDJStatic.Show(gameObject , "LaterDaojiReturn" ,"NowDaojiReturn" ,  6, "messages022" , "messages023"  , "messages024" , false);			
		}
	}
	
	function NowDaojiReturn(){
		if(AllManage.UICLStatic.mapType == MapType.fuben && !AllManage.dungclStatic.isDungeonClear()){
			AllManage.qrStatic.ShowQueRen(gameObject , "ReturnTownNoBoss" , "" , "info769");
		}else{
			if( !AllManage.dungclStatic.canBackTown() && ! AllManage.dungclStatic.rewardIsDone){
				AllManage.tsStatic.Show("info765");
			}else{
//				InRoom.GetInRoomInstantiate().RemoveTempTeam();
				InRoom.GetInRoomInstantiate().ActivityPVPRemove();
				MainPersonStatus.FindWayID = OneTask.id;
				MainPersonStatus.FindWayGoOn = true; 
				Loading.Level = DungeonControl.ReLevel;
				InRoom.GetInRoomInstantiate().RemoveTempTeam();
				InRoom.GetInRoomInstantiate().SetPhotonMasterClient();
				AllManage.UICLStatic.RemoveAllTeam();
				alljoy.DontJump = true;
				yield;
			PhotonNetwork.LeaveRoom();
	InRoom.GetInRoomInstantiate().isMessageQueueRunning = false;
					AllResources.ar.AllLoadLevel("Loading 1");
//	PhotonNetwork.LoadLevel("Loading 1");
		
			}	
		}
	}
	
	function ReturnTownNoBoss(){
		yield;
		yield;
		yield;
	 	if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
			if(false){
				AllManage.qrStatic.ShowBuyQueRen(gameObject ,"HuaFeiFanHui11" , "nofanhui" , AllManage.AllMge.Loc.Get("info287") + (parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) * 200) + AllManage.AllMge.Loc.Get("info335") + AllManage.AllMge.Loc.Get("buttons485"));				
//				AllManage.qrStatic.ShowQueRen(gameObject , "YesReturn" , "NoReturn" , "messages009");
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
					if(AllManage.UICLStatic.mapType == MapType.jingjichang){
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
	}
	
	public function desP(){
		while(PhotonNetwork.isMasterClient){
			yield;
		}
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
	}
	
	function LaterDaojiReturn(){
	}
	
	function FangQiAs(tsk : MainTask){
		MainPS.FangQiTask(tsk);
		tsk.jindu = 0;
		AllManage.tsStatic.ShowRed(AllManage.AllMge.Loc.Get("info279") + tsk.taskName);
		FangQiHuoDong(tsk);
		var fromNPC : npcAI;
		var toNPC : npcAI;
		if(AllManage.npcclStatic != null){
			fromNPC = AllManage.npcclStatic.GetNPCAsID(tsk.mainNPC.id);
			if(fromNPC){
				fromNPC.SendJindu(tsk);
			}
			var npcid : String;
			if(tsk.taskType == MainTaskType.duihua){
				npcid = dgcl.GetNPCIDAsID(parseInt(tsk.doneType.Substring(0,4)).ToString());
				toNPC = AllManage.npcclStatic.GetNPCAsID(npcid);
				if(toNPC){
					toNPC.removeTask(tsk);			
				}
			}else{
			
			}
		}
	}
	
	function FangQiAsID(tskID : String){
		for(var tsk : MainTask in myTask){
			if(tsk.id == tskID){
				MainPS.FangQiTask(tsk);
				tsk.jindu = 0;
				var fromNPC : npcAI;
				var toNPC : npcAI;
				fromNPC = AllManage.npcclStatic.GetNPCAsID(tsk.mainNPC.id);
				if(fromNPC){
					fromNPC.SendJindu(tsk);
				}
				var npcid : String;
				if(tsk.taskType == MainTaskType.duihua){
					npcid = dgcl.GetNPCIDAsID(parseInt(tsk.doneType.Substring(0,4)).ToString());
					toNPC = AllManage.npcclStatic.GetNPCAsID(npcid);
					if(toNPC){
						toNPC.removeTask(tsk);			
					}
				}
			}
		}
	}
	
	function FangQiHuoDong(tsk : MainTask){
		var i : int = 0;
		var useActivityStr : String; 
		var usePlayerGet : String[];
		var usePlayerDH : String[];
				useActivityStr = InventoryControl.yt.Rows[0]["ActivtyTask"].YuanColumnText;
				usePlayerGet = useActivityStr.Split(FStr.ToCharArray());
				for(i=0; i<usePlayerGet.length; i++){
	//				//print(usePlayerGet[i] + " == usePlayerGet[i]");
					if(usePlayerGet[i].Length > 1){
						usePlayerDH = usePlayerGet[i].Split(DStr.ToCharArray());
	//					//print(usePlayerDH[0] + " == usePlayerDH[0] == " + tsk.id);
	//					//print(usePlayerDH[1] + " == usePlayerDH[1]");
						if(usePlayerDH[0] == tsk.id){
							usePlayerGet[i] = "";
						}
					}
				}
				useActivityStr = "";
				for(i=0; i<usePlayerGet.length; i++){
	//				//print(usePlayerGet[i] + " == usePlayerGet[i]");
					if(usePlayerGet[i] != ""){
						useActivityStr += usePlayerGet[i] + ";"; 
					}
				}
				InventoryControl.yt.Rows[0]["ActivtyTask"].YuanColumnText = useActivityStr;
				if(pa){
					pa.SetTaskFun(); 				
				}
	}
	
	var uItemNum : UseItemNum;
	function GetAndUseTaskItem(id : String , num : int, taskid : String) : boolean{ 
		var canDoneTask : boolean = false;
		var useInt : int = 0;
		var useStr : String = "";
		canDoneTask = LookItemTask(taskid);
		if(canDoneTask){
			var useInvID : String[]; 
			var invStr : String;
			invStr = InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText + InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText + InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText + InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText;
//			//print(InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText + " == useInvID.length");
//			//print(InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText + " == useInvID.length");
//			//print(InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText + " == useInvID.length");
//			//print(InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText + " == useInvID.length");
			useInvID = invStr.Split(FStr.ToCharArray());
//			//print(invStr + " == ");
//			//print(useInvID.length + " == useInvID.length");
			for(var i=0; i< useInvID.length; i++){ 
				if(useInvID[i].Length > 4){
					if(useInvID[i].Substring(0,4) == id.Substring(0,4) && parseInt(useInvID[i].Substring(5,2)) >= num){
							useInvID[i] = uItemNum.useItemNum(useInvID[i] , num);
//							//print(useInvID[i] + " ==");
							InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText = "";
							InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText = "";
							InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText = "";
							InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText = "";
							for(var o=0; o<useInvID.length; o++){	 
								if(o < 15){
									InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText += useInvID[o] + ";";
								}else
								if(o < 30){
									InventoryControl.yt.Rows[0]["Inventory2"].YuanColumnText += useInvID[o] + ";";		
								}else
								if(o < 45){
									InventoryControl.yt.Rows[0]["Inventory3"].YuanColumnText += useInvID[o] + ";";		
								}else
								if(o < 60){
									InventoryControl.yt.Rows[0]["Inventory4"].YuanColumnText += useInvID[o] + ";";		
								}
							}
//							//print(InventoryControl.yt.Rows[0]["Inventory1"].YuanColumnText + " ==");
							ShowBagItem();
						return true;
					}		
				}
			}
		}
		return false;
	}
	
	function ShowBagItem(){
		invcl.ReInitItemItemNo();
	}
	
	private var gv : GameValue;
//	private var npccl : NPCControl;
	function CreateNPC(task : MainTask){ 
			gv = new GameValue();
			gv.task = task;
			gv.mtw = this;
			gv.npcid = task.mainNPC.id;
			return AllManage.npcclStatic.SetNPCTask(gv);
	}

		function CreateNPCLook(task : MainTask){ 
			gv = new GameValue();
			gv.task = task;
			gv.mtw = this;
			gv.npcid = GetItemIDAsLevel(task.doneType.Substring(0,4));
			return AllManage.npcclStatic.SetNPCTask(gv);
	}
	
	var dgcl : DungeonControl;
	function CreateMonster(task : MainTask){ 
			prefab = Resources.Load("5" + dgcl.GetNPCIDAsID(parseInt(task.doneType.Substring(0,4)).ToString()).Substring(1,2) + "00", GameObject);
			gv = new GameValue();
			gv.task = task;
			gv.mtw = this;
			var ms : MonsterStatus = prefab.GetComponent(MonsterStatus);
			ms.task = gv.task;
			ms.mtw = gv.mtw;
			obj = PhotonNetwork.InstantiateSceneObject(this.prefab.name, Vector3(parseInt(task.doneType.Substring(7,3)),0,parseInt(task.doneType.Substring(10,3))) + Vector3(Random.Range(1,4),0,Random.Range(1,4)), Quaternion.identity, 0,null);
	}
	function CreateMonster(task : MainTask , prd : ProducedObject)
	{ 
		prefab	=	Resources.Load("5" + dgcl.GetNPCIDAsID(parseInt(task.doneType.Substring(0,4)).ToString()).Substring(1,2) + "00", GameObject);
		gv		=	new GameValue();
		gv.task	=	task;
		gv.mtw	=	this;
		var ms	:	MonsterStatus = prefab.GetComponent(MonsterStatus);
		ms.task	=	gv.task;
		ms.mtw	=	gv.mtw;
		obj		=	PhotonNetwork.InstantiateSceneObject(this.prefab.name, Vector3(parseInt(task.doneType.Substring(7,3)),0,parseInt(task.doneType.Substring(10,3))) + Vector3(Random.Range(1,4),0,Random.Range(1,4)), Quaternion.identity, 0,null);		
		if(	prd.spawnObj	)
		{
			obj.SendMessage("SetMonsterPrd" , prd.spawnObj, SendMessageOptions.DontRequireReceiver);		
			var rows : yuan.YuanMemoryDB.YuanRow;
			rows = dgcl.GetNMRowAsID(parseInt(task.doneType.Substring(0,4)).ToString());
			obj.SendMessage("SetMonsterInfo" ,rows , SendMessageOptions.DontRequireReceiver);		
		}
	} 
	
	///Used by Monster Spawn 产生任务怪//
	function	CreateMonster(task : MainTask , prd : ProducedObject , pos : Vector3,	MonsterID:int) : GameObject
	{ 
		prefab	=	Resources.Load("5" + dgcl.GetNPCIDAsID(parseInt(task.doneType.Substring(0,4)).ToString()).Substring(1,2) + "00", GameObject);
		gv		=	new GameValue();
		gv.task =	task;
		gv.mtw	=	this;
		var	ms	:	MonsterStatus = prefab.GetComponent(MonsterStatus);
		ms.task =	gv.task;
		ms.mtw	=	gv.mtw;
		obj		=	MonsterHandler.CreateMonster(	prefab, pos, Quaternion.identity,	MonsterID	);
		//obj		=	PhotonNetwork.InstantiateSceneObject(this.prefab.name, pos, Quaternion.identity, 0,null);		
		if(	prd.spawnObj	)	//产生这个怪的生成点//
		{
			obj.SendMessage("SetMonsterPrd" , prd.spawnObj, SendMessageOptions.DontRequireReceiver);		
			var rows : yuan.YuanMemoryDB.YuanRow;
			rows	=	dgcl.GetNMRowAsID(parseInt(task.doneType.Substring(0,4)).ToString());
			obj.SendMessage("SetMonsterInfo" ,rows , SendMessageOptions.DontRequireReceiver);	
			obj.SendMessage("isShowTask" , SendMessageOptions.DontRequireReceiver);	
			return obj;
		}
		return null;
	}
	
	private var Dobj : GameObject;
	var dibiao : GameObject;
	function CreateDaoda(task : MainTask){
//		print("sd;kfakdsf;klajsdflk;jaslkdjfalksjdflksjdflsjdflksdjlfkj");
		if(Dobj == null)
		Dobj = Resources.LoadAssetAtPath("Assets/Resources/Prefabs/Daoda/Daoda" + ".prefab", GameObject);
		obj = Instantiate(Dobj, transform.position, Quaternion.identity);
		obj.transform.position = Vector3(parseInt(task.doneType.Substring(7,3)),0,parseInt(task.doneType.Substring(10,3))) + Vector3(Random.Range(1,4),0,Random.Range(1,4));
		gv = new GameValue();
		gv.task = task;
		gv.mtw = this;
		obj.SendMessage("SetTask" , gv , SendMessageOptions.DontRequireReceiver);		
	}
	
	function CreateNPCItem(task : MainTask){
			prefab = Resources.LoadAssetAtPath("Assets/Resources/Prefabs/NPC/NPC" + task.doneType.Substring(0,5)+ ".prefab", GameObject);
			if(prefab != null){
				obj = Instantiate(prefab, transform.position, Quaternion.identity);
				obj.transform.position = Vector3(parseInt(task.doneType.Substring(8,3)),0,parseInt(task.doneType.Substring(11,3)));
				gv = new GameValue();
				gv.task = task;
				gv.mtw = this;
				obj.SendMessage("SetItemTask" , gv , SendMessageOptions.DontRequireReceiver);
				return obj;
			}
		return null;		
	} 
	
	var chuansongObj : Transform;
	var chuansongObj1 : Transform;
	var endObj : Transform;
	var useEnemyTrans : Transform;
	function FindWay(pt : PrivateTask){
		if(UIControl.mapType == MapType.fuben){
			FindToAttack();
		}
		var bool : boolean = true;
	//	print(pt.task.taskType + " == " + pt.jindu);
		if(pt.task.taskType == MainTaskType.wupin && pt.jindu == 1){
			try{
				if(Vector3(parseInt(pt.task.doneType.Substring(7,3)),0,parseInt(pt.task.doneType.Substring(10,3))) != Vector3(0,0,0)){
					bool = false;
				}
			}catch(e){
			}
		}
		if(UIControl.mapType != MapType.jingjichang){
			if(UIControl.mapType == MapType.fuben && bool){
				if( pt.jindu == 2){
					AllManage.timeDJStatic.Show(gameObject , "LaterDaojiReturn" ,"NowDaojiReturn" ,  6, "messages022" , "messages023"  , "messages024" , false);
					return;	
				}else
				if( pt.jindu == 1 && MainTaskWork.MapID == pt.task.doneType.Substring(4,3)){
					if(pt.task.taskType == MainTaskType.guaiwu || pt.task.taskType == MainTaskType.wupin){
						useEnemyTrans = FindClosestEnemysp();
						if(useEnemyTrans)
							GoFindAndTishi(useEnemyTrans.position , pt.task);
	//						MainPS.gameObject.SendMessage("UdateAgentTargets" , useEnemyTrans.position , SendMessageOptions.DontRequireReceiver);						
						else
							AllManage.tsStatic.ShowBlue("tips102");			
						return;
					}else
					if(pt.task.taskType == MainTaskType.fuben){
						GoFindAndTishi(endObj.position , pt.task);
					}
				}
//				if(UIControl.mapType == MapType.fuben){
//					AllManage.pAIStatic.AutoAttackSimple();
//				}
			}

			MainPS.setNowTask(pt.task);
				if(pt.task.taskType == MainTaskType.WaKuang && pt.jindu == 1 && MainTaskWork.MapID == pt.task.doneType.Substring(4,3)){
					var trans : Transform;
					trans = FindSceneObjectAsID(pt.task.doneType.Substring(0,4));
					if(trans){
							GoFindAndTishi(trans.position , pt.task);
	//					MainPS.gameObject.SendMessage("UdateAgentTargets" , trans.position , SendMessageOptions.DontRequireReceiver);															
					}
				}else
				if((pt.task.taskType == MainTaskType.PVPKiller || pt.task.taskType == MainTaskType.PVPWin) && pt.jindu == 1){
					InRoom.GetInRoomInstantiate().ActivityPVPAdd(pt.task.doneType.Substring(4,3));
					PVPControl.PVPTaskNanDu = AllManage.mtwStatic.GetTaskLevel( pt.task.doneType );//1; //parseInt(pt.task.doneType.Substring(13,1));
					AllManage.tsStatic.ShowBlue("tips101");
					return;
				}else
				if(pt.task.taskType == MainTaskType.wupin && pt.jindu == 1){
					if( thisMapID == pt.task.doneType.Substring(4,3)){
						NPCControl.NowGoID = "";
							GoFindAndTishi(Vector3(parseInt(pt.task.doneType.Substring(7,3)),0,parseInt(pt.task.doneType.Substring(10,3))) , pt.task);
	//					MainPS.gameObject.SendMessage("UdateAgentTargets" , Vector3(parseInt(pt.task.doneType.Substring(7,3)),0,parseInt(pt.task.doneType.Substring(10,3))) , SendMessageOptions.DontRequireReceiver);										
						return;
					}else{
						NPCControl.NowGoID = "";
						GoToChuanSongMenAsTask(pt.task);
	//					MainPS.gameObject.SendMessage("UdateAgentTargets" , chuansongObj.position , SendMessageOptions.DontRequireReceiver);				
						return;
					}
				}
			if(UIControl.mapType == MapType.fuben && MainTaskWork.MapID == pt.task.doneType.Substring(4,3)){
				if(pt.task.taskType != MainTaskType.daoda){
					return;
				}
			}
			if(pt.task.taskType == MainTaskType.jiaocheng && pt.jindu != 2){
				UICL.lookCanShowRealButtonAsID(pt.task.id);
			 	TaskJiaoChengC.SelectJiaoChengAsID(parseInt(pt.task.doneType));
				return;
			}else
			if(pt.jindu == 1 && thisMapID == pt.task.mainNPC.map && pt.task.taskType != MainTaskType.duihua){
					NPCControl.NowGoID = "";
						GoToChuanSongMenAsTask(pt.task);
	//				MainPS.gameObject.SendMessage("UdateAgentTargets" , chuansongObj.position , SendMessageOptions.DontRequireReceiver);
			}else
			if(pt.jindu == 1 && thisMapID == pt.task.doneType.Substring(4,3)){
				if(pt.task.taskType == MainTaskType.duihua){ 
					NPCControl.NowGoID = GetItemIDAsLevel(pt.task.doneType.Substring(0,4));
							GoFindAndTishi(AllManage.npcclStatic.GetNPCPosition(GetItemIDAsLevel(pt.task.doneType.Substring(0,4)))  , pt.task);
	//				MainPS.gameObject.SendMessage("UdateAgentTargets" , AllManage.npcclStatic.GetNPCPosition(GetItemIDAsLevel(pt.task.doneType.Substring(0,4))) , SendMessageOptions.DontRequireReceiver);			
				}else if(pt.task.taskType == MainTaskType.fuben){
				}else{
					NPCControl.NowGoID = "";
							GoFindAndTishi(Vector3(parseInt(pt.task.doneType.Substring(7,3)),0,parseInt(pt.task.doneType.Substring(10,3)))  , pt.task);
	//				MainPS.gameObject.SendMessage("UdateAgentTargets" , Vector3(parseInt(pt.task.doneType.Substring(7,3)),0,parseInt(pt.task.doneType.Substring(10,3))) , SendMessageOptions.DontRequireReceiver);						
				}
				
			}else
			if(pt.jindu == 2 && thisMapID == pt.task.mainNPC.map){
				NPCControl.NowGoID = pt.task.mainNPC.id;
							GoFindAndTishi(AllManage.npcclStatic.GetNPCPosition(pt.task.mainNPC.id) , pt.task);
	//			MainPS.gameObject.SendMessage("UdateAgentTargets" , AllManage.npcclStatic.GetNPCPosition(pt.task.mainNPC.id) , SendMessageOptions.DontRequireReceiver);
			}else
			if(pt.jindu == 1 && thisMapID == pt.task.mainNPC.map){
				NPCControl.NowGoID = "";
						GoToChuanSongMenAsTask(pt.task);
	//			MainPS.gameObject.SendMessage("UdateAgentTargets" , chuansongObj.position , SendMessageOptions.DontRequireReceiver);
			}else
			if(thisMapID != pt.task.mainNPC.map){
			//	//print(pt.task.doneType.Length);
				if(pt.task.doneType.Length > 2){
					if(thisMapID != pt.task.doneType.Substring(4,3)){
						if(chuansongObj != null){				
							NPCControl.NowGoID = "";
							GoToChuanSongMenAsTask(pt.task);
		//					MainPS.gameObject.SendMessage("UdateAgentTargets" , chuansongObj.position , SendMessageOptions.DontRequireReceiver);
						}else
						if(endObj != null){
		//					NPCControl.NowGoID = "";
		//					MainPS.gameObject.SendMessage("UdateAgentTargets" , endObj.position , SendMessageOptions.DontRequireReceiver);		
							if(notStart){
								 notStart = false;
								 return;
							}		
							ttsk = pt.task;
							AllManage.timeDJStatic.Show(gameObject , "LaterDaojiReturn" ,"NowDaojiReturn" ,  6, "messages022" , "messages023"  , "messages024" , false);			
						}
					}
				}else{
					NPCControl.NowGoID = "";
					GoToChuanSongMenAsTask(pt.task);
				}
			}
		}
	}
	
	private var findTimes : int = 0;
	private var agentPS : agentLocomotion;
	function FindToAttack(){
		findTimes += 1;
		var tms : int = 0 ;
		tms = findTimes;
		yield WaitForSeconds(0.5);
		if(tms == findTimes){
			if(agentPS == null && PlayerStatus.MainCharacter){
				agentPS = PlayerStatus.MainCharacter.gameObject.GetComponent(agentLocomotion);
			}
			if(agentPS != null && agentPS.enabled == false){
				AllManage.pAIStatic.AutoAttackSimple();
			}
		}
	}
	
	function GoFindAndTishi(vec : Vector3 , tsk : MainTask){
		MainPS.gameObject.SendMessage("UdateAgentTargets" , vec , SendMessageOptions.DontRequireReceiver);											
//		AllManage.tsStatic.Show1( AllManage.AllMge.Loc.Get("messages151") + "("+UIControl.taskLeixingStrs[tsk.leixing] + ")" + tsk.taskName);		
	}
	
	private function FindClosestEnemysp () : Transform{
		   var gos : MonsterSpawn[];
		  gos = FindObjectsOfType(MonsterSpawn);
		  var closest : MonsterSpawn;
		  var distance = Mathf.Infinity;
		for (var go : MonsterSpawn in gos) {
		if(go){
		    var diff = (go.transform.position - MainPS.gameObject.transform.position);
		    var curDistance = diff.sqrMagnitude;
		    if (go.IsAbleToSpawn() && !go.IsCleared() && curDistance < distance) {
		       closest = go;
		       distance = curDistance;
		       }
		       }
		    }
		 if(closest)
		return closest.transform;
		else
		return null;
	}
	var notStart : boolean = true;
	function HuaFeiFanHui(){
		if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
			AllManage.qrStatic.ShowBuyQueRen(gameObject ,"HuaFeiFanHui11" , "nofanhui" , AllManage.AllMge.Loc.Get("info287") + (parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) * 200) + AllManage.AllMge.Loc.Get("info335") + AllManage.AllMge.Loc.Get("buttons485"));				
		else
			HuaFeiFanHui11();
	}
	
	function nofanhui(){
	
	}
	
	var ttsk : MainTask;
	function HuaFeiFanHui11(){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
		if(ttsk != null){
			AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.SpendingReturns11 , parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) , 0 , "" , gameObject , "realHuaFeiFanHui11");
//			AllManage.AllMge.UseMoney(parseInt(InventoryControl.yt.Rows[0]["PlayerLevel"].YuanColumnText) * 200 , 0 , UseMoneyType.SpendingReturns11 , gameObject , "realHuaFeiFanHui11");
		}
	}

	function realHuaFeiFanHui11(){
//		InRoom.GetInRoomInstantiate().RemoveTempTeam();
	 	InRoom.GetInRoomInstantiate().ActivityPVPRemove();
		MainPersonStatus.FindWayID = ttsk.id;
		MainPersonStatus.FindWayGoOn = true; 
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

	private var TriggerSts : TriggerStone[];
	function FindSceneObjectAsID(id : String) : Transform{
		TriggerSts = FindObjectsOfType(TriggerStone);
		for(var i=0; i<TriggerSts.length; i++){
			if(TriggerSts[i].isTaskID == id){
				return TriggerSts[i].gameObject.transform;
			}
		}
		return null;
	}
	
	function GoToChuanSongMenAsTask(tsk : MainTask){
		if(tsk.taskType != MainTaskType.jiaocheng){
			if(tsk.doneType.Substring(13,1) == "5" && chuansongObj1 != null){
				if(chuansongObj1)
						GoFindAndTishi(chuansongObj1.position , tsk);
//				MainPS.gameObject.SendMessage("UdateAgentTargets" , chuansongObj1.position , SendMessageOptions.DontRequireReceiver);
			}else{
				if(chuansongObj)
						GoFindAndTishi(chuansongObj.position , tsk);
//				MainPS.gameObject.SendMessage("UdateAgentTargets" , chuansongObj.position , SendMessageOptions.DontRequireReceiver);		
			}
		}else{
			if(chuansongObj)
						GoFindAndTishi( chuansongObj.position  , tsk);
//			MainPS.gameObject.SendMessage("UdateAgentTargets" , chuansongObj.position , SendMessageOptions.DontRequireReceiver);					
		}
	}
	
	function FindWay(vec : Vector3){
	if(UIControl.mapType != MapType.jingjichang){
		NPCControl.NowGoID = "";
		MainPS.gameObject.SendMessage("UdateAgentTargets" , vec , SendMessageOptions.DontRequireReceiver);
	}
	}
	
	function Gochuansong(){
		if(UIControl.mapType != MapType.jingjichang && chuansongObj){
			NPCControl.NowGoID = "";
			MainPS.gameObject.SendMessage("UdateAgentTargets" , chuansongObj.position , SendMessageOptions.DontRequireReceiver);
		}
	}
	
	function Gochuansong1(){
		if(UIControl.mapType != MapType.jingjichang && chuansongObj1){
			NPCControl.NowGoID = "";
			MainPS.gameObject.SendMessage("UdateAgentTargets" , chuansongObj1.position , SendMessageOptions.DontRequireReceiver);
		}
	}
	
	function FindNPC(task : MainTask){
//	print(task);
//	print(task.mainNPC);	
	if(UIControl.mapType != MapType.jingjichang){
		MainPS.setNowTask(task);
//		//print();
		if(thisMapID == task.mainNPC.map){
			NPCControl.NowGoID = task.mainNPC.id;
						GoFindAndTishi(AllManage.npcclStatic.GetNPCPosition(task.mainNPC.id) , task);
//			MainPS.gameObject.SendMessage("UdateAgentTargets" , AllManage.npcclStatic.GetNPCPosition(task.mainNPC.id) , SendMessageOptions.DontRequireReceiver);
		}
	}
	}
	
	function SelectYouMapItem(mapid : String , nandu : int){
		var i : int = 0; 
		var usetsi : TaskSceneIcon;
		var Obj : GameObject;
		for(i=0; i<MapYouObj.length; i++){ 
			Obj = MapYouObj[i];
			usetsi = Obj.GetComponent(TaskSceneIcon);
			if(usetsi.mapID == mapid && usetsi.nandu == nandu){
				usetsi.OpenIsSelect(true);
			}else{
				usetsi.OpenIsSelect(false);				
			}
		}	
		transRightInfo.localPosition.y = 1000;
		transRightTeam.localPosition.y = 0;
		transRightInfo.gameObject.SetActiveRecursively(false);
		transRightTeam.gameObject.SetActiveRecursively(true);
	}
	
	var mc : MapContact;
	var taskMapCont : TaskMapContact;
	var RightTksIcon : TaskSceneIcon[];
	var MapYouObj = new Array();
	var UseMapYouObj : TaskSceneIcon;
	var MapYouParent : Transform;
	var UiGidYou : UIGrid;
	static var LevelJilu : String[];
	function SelectOneMap(mapID : String , nandu : int){ 
		MapYouItems[2].gameObject.SetActiveRecursively(false);
		MapYouObjInit(); 
		var i : int = 0; 
		var usetsi : TaskSceneIcon; 
		var useObj : GameObject;
		var bool : boolean = false;
		for(i=0; i<MapRightObj.length; i++){
			useObj = MapRightObj[i];
			usetsi = useObj.GetComponent(TaskSceneIcon);
			if(usetsi.mapID == mapID){
				usetsi.OpenIsSelect(true);
			}else{
				usetsi.OpenIsSelect(false);				
			}
		}
		for(i=0; i<chuansongPlayer.doneTaskMap.length; i++){ 
			if(chuansongPlayer.doneTaskMap[i] != null){
				if(chuansongPlayer.doneTaskMap[i].mapID == mapID){
					if(nandu == 0){
//						for(var m=1; m<=chuansongPlayer.doneTaskMap[i].nandu; m++){
//						var useInt : int = 0;
//						if(mapID.Substring(0,1) == "1"){
//							useInt = 2;
//						}else{
//							useInt = 5;
//						}
						var boolss : boolean = false;
						for(var m=1; m<3; m++){
	// 							var Obj : GameObject = Instantiate(UseMapYouObj.gameObject);
								var tsi : TaskSceneIcon;
						    //							Obj.transform.parent = MapYouParent;
                                
								if(mapID.Substring(0,1) == "1" && 1 == m){
								    //var panel : UIPanel = MapYouItems[0].gameObject.transform.parent.GetComponentInParent(UIPanel);
								    //panel.enabled = false;
								    //MapYouItems[0].gameObject.transform.localScale = Vector3.zero;
								    //panel.enabled = true;
								}
								

								if(mapID.Substring(0,1) == "1" && m > 1){
									MapYouItems[m-1].gameObject.SetActiveRecursively(false);
								}else{
								    MapYouItems[m-1].gameObject.SetActiveRecursively(true);	
								    if(mapID.Substring(0,1) != "1" && 1 == m)
								    {
								        MapYouItems[0].gameObject.transform.localScale = Vector3.one;
								    }
								}
									
								tsi = MapYouItems[m-1];
								tsi.OpenTanHao(false);
								tsi.SetRigthMap(chuansongPlayer.doneTaskMap[i].mapID , m);
								tsi.mtw = this;
								MapYouObj.Add(MapYouItems[m-1].gameObject); 
								if(taskinfolist.nowTaskInfo != null && taskinfolist.nowTaskInfo.task && taskinfolist.nowTaskInfo.task.doneType){ 
									if(taskinfolist.nowTaskInfo.task.doneType.Length > 2){
//										if(taskinfolist.nowTaskInfo.task.doneType.Substring(13,1) == m.ToString()){
										if(m == 1){
											tsi.OpenTanHao(true);
										}else{
											tsi.OpenTanHao(false);
										}
									}
								} 
							if(!boolss){					
								if(LevelJilu[2] == m.ToString() && LevelJilu[1] == TaskSceneIcon.LeftLevel && tsi.gameObject.active){  
									tsi.SelectOneRightMap(); 
									GoMapRightTeam();
									bool = true;
	//								tsi.GoLevel();
								}else{
									if(!bool && tsi.gameObject.active){
										tsi.SelectOneRightMap();
									}
								}
								if(MapYouItems[m-1].gameObject.active){
									if(mapID.Substring(0,1) == "1" && m >0){
										MapYouItems[m-1].gameObject.transform.localPosition.y = 2000;
									}else{
										MapYouItems[m-1].gameObject.transform.localPosition.y = 0;
									}
								}
								if(tsi.tanhao.enabled == true){
//									boolss = true;
									tsi.SelectOneRightMap();
								}
							}
						}
					}else{
						for(var n=0; n<taskMapCont.HeroMap.length; n++){
							if(mapID == taskMapCont.HeroMap[n]){
//								var Obj1 : GameObject = Instantiate(UseMapYouObj.gameObject);
								var tsi2 : TaskSceneIcon;
//								Obj1.transform.parent = MapYouParent;
//								tsi2 = Obj1.GetComponent(TaskSceneIcon);
								MapYouItems[0].gameObject.SetActiveRecursively(false);
								MapYouItems[1].gameObject.SetActiveRecursively(false);
								MapYouItems[2].gameObject.SetActiveRecursively(false);
//								MapYouItems[3].gameObject.SetActiveRecursively(false);
//								MapYouItems[4].gameObject.SetActiveRecursively(true);
								tsi2 = MapYouItems[2];
								tsi2.SetRigthMap(mapID , 5);
								tsi2.mtw = this;
								MapYouObj.Add(MapYouItems[2].gameObject); 
								tsi2.SelectOneRightMap(); 
							}
						}
					}
				}
			}
		}
//		yield;
		//UiGidYou.repositionNow = true;
	}
	
	function MapYouObjInit(){
//		for(var i=0; i<MapYouObj.length; i++){
//			Destroy(MapYouObj[i]);
//		}
		MapYouObj = new Array(0);		
	} 
	
	var btnGM : BtnGameManager; 
	private var Kstr : String = " ";
	private var useTeamID : String;
	function SetPaiDuiAsID(teamid : String){  
		btnGM.GetTempTeam();
		useTeamID = teamid;
	}
	
	function SetPaiDuiAsID2(){
	
		for(var btfc : BtnTeamForCreate in btnGM.listBtnTeamForTask){ 
			if(btfc.teamID == useTeamID){ 
				var strs : String[];
				strs = btfc.lblName.text.Split(Kstr.ToCharArray()); 
				if(strs[1].Length > 3){
					switch(strs[1]){
						case AllManage.AllMge.Loc.Get("info997"):
							LevelJilu[0] = "0";
							LevelJilu[2] = "1";
							break;
						case AllManage.AllMge.Loc.Get("info998"):
							LevelJilu[0] = "0";
							LevelJilu[2] = "2";
							break;
						case AllManage.AllMge.Loc.Get("info999"):
							LevelJilu[0] = "0";
							LevelJilu[2] = "3";
							break;
						case AllManage.AllMge.Loc.Get("info1000"):
							LevelJilu[0] = "1";
							LevelJilu[2] = "5";
							break;
					}
				}
//				if(strs[1].Length > 3){
//					LevelJilu[0] = "1";
//					LevelJilu[2] = "5";
//				}else{
//					LevelJilu[0] = "0"; 
//					LevelJilu[2] = strs[1].Substring(2,1);
//				}
				LevelJilu[1] = strs[0];
			}
		}
	}
	
	function setnowMen(nm : TriggerLoadLevel){
		nowMen = nm;
	}
	
	function setnowMenBool(bool : boolean){
		if(nowMen){
			nowMen.barOpen = bool;
		}
	}
	
	private var chuansongPlayer : MainPlayerStatus;
	var tksIcon : TaskSceneIcon[];
	var MapRightObj = new Array();
	var listStar=new System.Collections.Generic.List.<GameObject>();
	var UseMapRightObj : TaskSceneIcon;
	var MapRightParent : Transform;
	var UiGid : UIGrid;
//	var UIAllPC : UIAllPanelControl;
	var ChuansongRightBan : GameObject;
	var dungcl : DungeonControl;
	var wuneirong : GameObject;
	var SpriteFlag : UISprite;
	var LabelFlag : UILabel;
	var ArrayPlaceJiYe : String[];
	var star:RefreshMapItemStart ;
	private var nowMen : TriggerLoadLevel;
	function OpenChuanSongMen(player : MainPlayerStatus , nandu : int){ 
		if(nandu == 1){
			if(InRoom.GetInRoomInstantiate().GetServerSwitchString(yuan.YuanPhoton.BenefitsType.HeroPVESwitch) == "0"){
				AllManage.tsStatic.Show1(AllManage.AllMge.Loc.Get("info645"));
				return;
			}
		}
		if(null==star){
		star =GetComponent(RefreshMapItemStart);
		}
		if(nowMen)
			nowMen.barOpen = true;
		AllManage.UIALLPCStatic.show24();
		yield;
		MapYouItems[0].gameObject.SetActiveRecursively(false);
		MapYouItems[1].gameObject.SetActiveRecursively(false);
		MapYouItems[2].gameObject.SetActiveRecursively(false);
//		MapYouItems[3].gameObject.SetActiveRecursively(false);
//		MapYouItems[4].gameObject.SetActiveRecursively(false);
		transRightInfo.localPosition.y = 1000;
		transRightTeam.localPosition.y = 1000;
		if(nandu == 0){
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages112");
			AllManage.AllMge.SetLabelLanguageAsID(LabelFlag);
//			LabelFlag.text = "普通副本";
			SpriteFlag.spriteName = "Level1";
			star.Difficulty(0);
		}else{
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages113");
			AllManage.AllMge.SetLabelLanguageAsID(LabelFlag);
//			LabelFlag.text = "精英副本";
			SpriteFlag.spriteName = "Level2";	
			star.Difficulty(1);			
		}
		var show3 : boolean = false;
		ChuansongRightBan.SetActiveRecursively(false);
		listStar.Clear();
		MapRightObjInit();
		var i : int = 0;
		var m : int = 0;
		chuansongPlayer = player;
		mc = taskMapCont.GetMapContact(thisMapID.Substring(0,2),mc);
		if(nandu == 1 && Application.loadedLevelName.Substring(3,3) == "151"){
			ArrayPlaceJiYe = new Array(mc.contactMap.length + taskMapCont.HeroMap.length);
			for(i=0; i<ArrayPlaceJiYe.length; i++){
				if(i<mc.contactMap.length){
					ArrayPlaceJiYe[i] = mc.contactMap[i];
				}else{
					ArrayPlaceJiYe[i] = taskMapCont.HeroMap[i  - mc.contactMap.length];
				}
			}
			mc.contactMap = ArrayPlaceJiYe;
		}
		var useJingYingLastStr : String = "000";
		var posX : int = 0;
		for(i=0; i<mc.contactMap.length; i++){
			var open : boolean = false;
			for(m=0; m<player.doneTaskMap.length; m++){
				if(!open){
					if(player.doneTaskMap[m] != null){
						if(player.doneTaskMap[m].mapID == mc.contactMap[i]){
							open = true;
						}
					}
				}
			}
			var isHeroMap : boolean = true;
			if(nandu == 1){
				isHeroMap = false;
				if(mc.contactMap[i].Substring(0,1) == "1" || useJingYingLastStr.Substring(0,2) == mc.contactMap[i].Substring(0,2)){
					open = false;
				}
				
				for(var n=0; n<taskMapCont.HeroMap.length; n++){
					if(mc.contactMap[i] == taskMapCont.HeroMap[n]){
						isHeroMap = true;
					}
				}
				
			}
			
			try{
				if(! AllManage.dungclStatic.getMapNameIsNull(mc.contactMap[i].Substring(0,3))){
					open = false;
				}
			}catch(e){
				
			}
			
//			print(mc.contactMap[i] + " ===== " + open);
			if(open && isHeroMap){		 
			//	//print(mc.contactMap[i]);
				useJingYingLastStr = mc.contactMap[i];
				wuneirong.active = false;
				var Obj : GameObject = Instantiate(UseMapRightObj.gameObject);
				var tsi : TaskSceneIcon;
				Obj.transform.parent = MapRightParent;
				Obj.transform.localScale = Vector3(1.6 , 1.6 , 1.6);
				Obj.transform.localPosition.z = 0;
				Obj.transform.localPosition.y = 210;
				Obj.transform.localPosition.x = posX;
				posX += 500;
				tsi = Obj.GetComponent(TaskSceneIcon);
				var mapName : String = "";
				
				mapName = dungcl.getMapName(mc.contactMap[i].Substring(0,3));
				if(nandu == 1){
					mapName = mapName.Replace("1" , "");
				}
				tsi.SetMap(mc.contactMap[i] , mapName , nandu);
				tsi.mtw = this;
			
				Obj.SendMessage("SetMapID",tsi.mapID,SendMessageOptions.DontRequireReceiver);
				MapRightObj.Add(Obj);
				listStar.Add(Obj);
					if(taskinfolist.nowTaskInfo != null && taskinfolist.nowTaskInfo.task && taskinfolist.nowTaskInfo.task.doneType){
						if(taskinfolist.nowTaskInfo.task.doneType.Length > 2){
							if(taskinfolist.nowTaskInfo.task.doneType.Substring(4,3) == mc.contactMap[i].Substring(0,3)){
								tsi.OpenTanHao(true);
							}else{
								tsi.OpenTanHao(false);
							}			
						}
					}
				if(m == 0){
					 tsi.SelectThisMap();
				}
			}
			if(LevelJilu[0] == nandu.ToString()){
				if(LevelJilu[1] == tsi.LabelMapName.text){ 
					tsi.SelectThisMap(); 
					show3 = true;
				}
			}else{
				if(tsi){
					tsi.SelectThisMap(); 				
				}
			}
		}
		yield;
		UiGid.repositionNow = true;
		
		
		star.RefreshBtns(listStar);
		//this.gameObject.SendMessage("RefreshBtns",MapRightObj,SendMessageOptions.DontRequireReceiver);
	}
	
	var transRightInfo : Transform;
	var transRightTeam : Transform;	
	var nowTSI : TaskSceneIcon;
	var LabelInfo : UILabel;
	var nowRightMapID : String;
	var nowRightNanDu : int;
	var nowRightInfo : String;  
	var nowRightSprite : UISprite;
	var BUttonZuDui : GameObject;
	var ButtonSaoDang : Transform;
	var SaoDangSprite : UISprite;
	function SelectMapRigthAsID(str : String , nd : int , TSI : TaskSceneIcon){
		nowTSI = TSI;
		transRightInfo.localPosition.y = 0;
		transRightTeam.localPosition.y = 1000;
		transRightInfo.gameObject.SetActiveRecursively(true);
		transRightTeam.gameObject.SetActiveRecursively(false);
		nowRightMapID = str;
		nowRightNanDu = nd;
		nowRightInfo = dungcl.getMapInfo(nowRightMapID);
			AllManage.AllMge.Keys.Clear();
			AllManage.AllMge.Keys.Add("messages079");
			AllManage.AllMge.Keys.Add(nowRightNanDu + "\n");
			AllManage.AllMge.Keys.Add("messages080");
			AllManage.AllMge.Keys.Add(dungcl.getMapLevel(nowRightMapID,nowRightNanDu) + "\n");
			AllManage.AllMge.Keys.Add(nowRightInfo + "");
			AllManage.AllMge.SetLabelLanguageAsID(LabelInfo);
//		LabelInfo.text = "难度:"+ nowRightNanDu+"\n建议等级：" + dungcl.getMapLevel(nowRightMapID,nowRightNanDu) + "\n" + nowRightInfo; 
		nowRightSprite.spriteName = nowRightMapID.Substring(0,2);
		var i : int = 0; 
		var usetsi : TaskSceneIcon;
		var Obj : GameObject;
//		//print(nowRightMapID + " == " + nowRightNanDu);
		for(i=0; i<MapYouObj.length; i++){ 
			Obj = MapYouObj[i];
			usetsi = Obj.GetComponent(TaskSceneIcon);
			if(usetsi.mapID == nowRightMapID && usetsi.nandu == nowRightNanDu){
				usetsi.OpenIsSelect(true);
			}else{
				usetsi.OpenIsSelect(false);				
			}
		}
		if(nowRightMapID.Substring(0,1) == "1"){
			BUttonZuDui.SetActiveRecursively(false);
			ButtonSaoDang.gameObject.SetActiveRecursively(false);
		}else{
			BUttonZuDui.SetActiveRecursively(true);		
			ButtonSaoDang.gameObject.SetActiveRecursively(true);	
		}
		var mapND : String;
		mapND = nowRightMapID + "" + nowRightNanDu;
//		ButtonSaoDang.localPosition.x = 3000;
		SaoDangSprite.color.a = 0.5f;
		ButtonSaoDang.collider.enabled = false;
		if(nowRightMapID.Substring(0,1) != "1"){					
			if(nowRightNanDu == 5){
			MapYouItems[2].gameObject.SetActiveRecursively(true);
				for(i=0; i<DuplicateEvaElite.length; i++){
					if(DuplicateEvaElite[i] != null && DuplicateEvaElite[i].map == mapND){
						if(DuplicateEvaElite[i].xingxing >= 8){
//							ButtonSaoDang.localPosition.x = -160;
							ButtonSaoDang.collider.enabled = true;	
							SaoDangSprite.color.a = 1f;
						}else{
//							ButtonSaoDang.localPosition.x = 3000;	
							ButtonSaoDang.collider.enabled = false;		
							SaoDangSprite.color.a = 0.5f;			
						}
						return;
					}else{
//							ButtonSaoDang.localPosition.x = 3000;	
							ButtonSaoDang.collider.enabled = false;	
							SaoDangSprite.color.a = 0.5f;									
					}
				}
			}
				else if(nowRightNanDu == 2){
				for(i=0; i<DuplicateEvaDungeon.length; i++){
					if(DuplicateEvaDungeon[i] != null && DuplicateEvaDungeon[i].map == mapND){
						if(DuplicateEvaDungeon[i].xingxing >= 8){
//							ButtonSaoDang.localPosition.x = -160;
							ButtonSaoDang.collider.enabled = true;	
							SaoDangSprite.color.a = 1f;	
						}else{
//							ButtonSaoDang.localPosition.x = 3000;	
							ButtonSaoDang.collider.enabled = false;	
							SaoDangSprite.color.a = 0.5f;				
						}
						return;
					}else{
//							ButtonSaoDang.localPosition.x = 3000;
							ButtonSaoDang.collider.enabled = false;	
							SaoDangSprite.color.a = 0.5f;							
					}
				}		
			}		
			else{
				for(i=0; i<DuplicateEvaNormal.length; i++){
					if(DuplicateEvaNormal[i] != null && DuplicateEvaNormal[i].map == mapND){
						if(DuplicateEvaNormal[i].xingxing >= 8){
//							ButtonSaoDang.localPosition.x = -160;
							ButtonSaoDang.collider.enabled = true;	
							SaoDangSprite.color.a = 1f;	
						}else{
//							ButtonSaoDang.localPosition.x = 3000;	
							ButtonSaoDang.collider.enabled = false;	
							SaoDangSprite.color.a = 0.5f;				
						}
						return;
					}else{
//							ButtonSaoDang.localPosition.x = 3000;
							ButtonSaoDang.collider.enabled = false;	
							SaoDangSprite.color.a = 0.5f;							
					}
				}		
			}
		}
	}
	
	function GoMapRigthAsInt(){
		if(nowTSI != null){
			if(!MapYouItems[2].gameObject.activeSelf&&nowRightNanDu==5)
			{
				return;
			}
//			nowTSI.gameOject.SetActiveRecursively(true);
			nowTSI.GoLevelLeft();		
//			nowTSI.gameOject.SetActiveRecursively(false);				
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
	
	var SaoDangMapID : String;
	var SaoDangNanDu : int;
	var costSaoDangPower : int = 0;
	var StrID : String;
	function GoSaoDang(){
		if(AllManage.InvclStatic.isBagFull()){
			AllManage.tsStatic.Show("tips049");
			return;
		}
	
		if( ! AllManage.SaoDangDJstatic.parentTanKuang.active){
			if(nowTSI != null){
				SaoDangMapID = nowTSI.mapID;
				SaoDangNanDu = nowTSI.nandu;
				
				costSaoDangPower = AllResources.GetMapCostPower(SaoDangMapID , SaoDangNanDu);
				
				if(ps == null && PlayerStatus.MainCharacter){
					ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
				}
				
				if(parseInt(ps.Power) >= costSaoDangPower){
				NowSaoDang();
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
		}else{
			AllManage.tsStatic.Show("tips073");
		}
	}
	
	function ShowUseMoney()
	{
					if(SaoDangNanDu!=2){
					AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsHuntingUp , parseInt(AllManage.dungclStatic.getMapLevel(SaoDangMapID , SaoDangNanDu)),1, "" , gameObject , "ShowTipsNow");
					
					}else{
					AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsHuntingUp , parseInt(AllManage.dungclStatic.getMapLevel(SaoDangMapID , SaoDangNanDu)),2, "" , gameObject , "ShowTipsNowTwo");
					}	
	}
	
	function ShowTipsNow(objs : Object[])
	{
			var str : String ;
			str = String.Format("{0} {1} {2}",AllManage.AllMge.Loc.Get("info1224"),objs[2],AllManage.AllMge.Loc.Get("meg0081"));
			AllManage.qrStatic.ShowQueRen(gameObject,"returnMoneyNow","",str);
//			AllManage.qrStatic.ShowTimesQueRen(gameObject,"NowSaoDang" , "" , "messages037" , 1);	
	}
	
	
		function ShowTipsNowTwo(objs : Object[])
	{
			var str : String ;
			str = String.Format("{0} {1} {2}",AllManage.AllMge.Loc.Get("info1224"),objs[2],AllManage.AllMge.Loc.Get("meg0081"));
			AllManage.qrStatic.ShowQueRen(gameObject,"returnMoneyNowTwo","",str);
//			AllManage.qrStatic.ShowTimesQueRen(gameObject,"NowSaoDang" , "" , "messages037" , 1);	
	}
	
	function ClickMapNoSao()
	{
		
	}
	
	function YesSao(){
		//print(nowTSI);
		yield;
		if(nowTSI != null){
			SaoDangMapID = nowTSI.mapID;
			SaoDangNanDu = nowTSI.nandu;
			if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
				AllManage.qrStatic.ShowBuyQueRen(gameObject ,"SaoDangStart" , "" , "messages017");	
			else
				SaoDangStart();
		}
	}
	var SaoDangTimes : int = 0;
	function SaoDangStart(){
		//print(SaoDangTimes);
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.DuplicateRaids).ToString());
		InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.Raids , "");			
		AllManage.UIALLPCStatic.DesObj30();
		if(SaoDangTimes > 0){
			AllManage.dungclStatic.mtwMapID = SaoDangMapID;
			AllManage.dungclStatic.NowMapLevel = SaoDangNanDu;
			AllManage.dungclStatic.yt = new yuan.YuanMemoryDB.YuanTable("MapLevel","id");
			AllManage.dungclStatic.StartSetAttr();
			if(ps == null && PlayerStatus.MainCharacter){
				ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
			}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.RaidsStart , 0 , 0 , "" , gameObject , "realSaoDangStart");
//			AllManage.AllMge.UseMoney(0 , 10 , UseMoneyType.RaidsStart , gameObject , "realSaoDangStart");
//			if(ps.UseMoney(0 , 10)){
//				AllManage.SaoDangDJstatic.Show(gameObject , "OtherNowSao" , "GiveUpSaoDang" ,"NowSaoDang" ,  180 , "messages034" , "messages035"  , "messages036" , false);							
//			}
			SaoDangTimes -= 1;
		}else{
			AllManage.tsStatic.Show("tips074");
		}
	}
	
	function realSaoDangStart(){
		AllManage.SaoDangDJstatic.Show(gameObject , "OtherNowSao" , "GiveUpSaoDang" ,"NowSaoDang" ,  180 , "messages034" , "messages035"  , "messages036" , false);							
	}
	
//	function CostNewPower(type : yuan.YuanPhoton.CostPowerType , num1 : int , num2 : int , itemID : String , obj : GameObject , functionName : String){
	function NowSaoDang(){
		AllManage.AllMge.CostNewPower(yuan.YuanPhoton.CostPowerType.Raids ,  costSaoDangPower , 0 , "" , gameObject , "ShowUseMoney");
//		if(ps.AddPower(costSaoDangPower)){
//			
//		}
	}
	
	function returnMoneyNow()
	{
//	AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsHuntingUp , parseInt(AllManage.dungclStatic.getMapLevel(SaoDangMapID , SaoDangNanDu)),1, "" , gameObject , "returnNowSaoDang");
	PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().getHuntingMap(parseInt(dungcl.getMapLevel(SaoDangMapID,SaoDangNanDu)),SaoDangMapID,SaoDangNanDu,1));
	}
	
	function returnMoneyNowTwo()
	{
//	AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.TipsHuntingUp , parseInt(AllManage.dungclStatic.getMapLevel(SaoDangMapID , SaoDangNanDu)),2, "" , gameObject , "returnNowSaoDang");
	PanelStatic.StaticBtnGameManager.RunOpenLoading(function()InRoom.GetInRoomInstantiate().getHuntingMap(parseInt(dungcl.getMapLevel(SaoDangMapID,SaoDangNanDu)),SaoDangMapID,SaoDangNanDu,2));
	}
	
	function returnNowSaoDang(){
			AllManage.dungclStatic.mtwMapID = SaoDangMapID;
			AllManage.dungclStatic.NowMapLevel = SaoDangNanDu;
			AllManage.dungclStatic.yt = new yuan.YuanMemoryDB.YuanTable("MapLevel","id");
			AllManage.dungclStatic.YieldStartSetAttr();
	}
	
	function ClearDungeon(){
			AllManage.SaoDangDJstatic.parentTanKuang.SetActiveRecursively(false);
			AllManage.dungclStatic.DoneCard();
			AllManage.tsStatic.Show("tips074");
	}
	
	function GiveUpSaoDang(){
		AllManage.SaoDangDJstatic.parentTanKuang.SetActiveRecursively(false);
		AllManage.tsStatic.Show("tips075");
//		fang qi jiang li;
	}
	
	function OtherNowSao(){
		if(PlayerPrefs.GetInt("ConsumerTip" , 0) == 1)
			AllManage.AllMge.TipsMoney(yuan.YuanPhoton.UseMoneyType.CostRaidsNow , AllManage.dungclStatic.level  , 0 , "" , gameObject , "buyNowSaoTips");
//			AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"buyNowSao" , "" , AllManage.AllMge.Loc.Get("info305")+ "" + AllManage.dungclStatic.level + AllManage.AllMge.Loc.Get("info306")+  "");	
		else
			buyNowSao();
	}
	
	function buyNowSaoTips(objs : Object[]){
			AllManage.qrStatic.ShowBuyQueRen1(gameObject ,"buyNowSao" , "" , AllManage.AllMge.Loc.Get("info305")+ "" + objs[2] + AllManage.AllMge.Loc.Get("info306")+  "");	
	}
	
	
	function buyNowSao(){
		if(ps == null && PlayerStatus.MainCharacter){
			ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
		}
AllManage.AllMge.UseNewMoney(yuan.YuanPhoton.UseMoneyType.CostRaidsNow , AllManage.dungclStatic.level , 0 , "" , gameObject , "realbuyNowSao");
//		AllManage.AllMge.UseMoney(0 , AllManage.dungclStatic.level , UseMoneyType.CostRaidsNow , gameObject , "realbuyNowSao");
//		if(ps.UseMoney(0 , AllManage.dungclStatic.level)){
//		}
	} 
	
	function realbuyNowSao(){
			InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.DuplicateRaids).ToString());
			AllManage.SaoDangDJstatic.times += 1;
			NowSaoDang();	
	}
	
	function GoMapRightTeam(){
		if(nowTSI != null){ 
			try{
				nowTSI.GoLevel();			
			}catch(e){
				//print(e.ToString());
			}
		}
	}
	
	function GoMapTeamRetrun(){
		if(UIControl.TeamHeadYesOKmapid == "" && !UIControl.boolTeamHeadYes){
			transRightInfo.localPosition.y = 0;
			transRightTeam.localPosition.y = 1000;	
			transRightTeam.gameObject.SetActiveRecursively(false);
			transRightInfo.gameObject.SetActiveRecursively(true);
		}else{
			AllManage.UICLStatic.TeamHeadYesOK();
		}
	}
	
	function MapRightObjInit(){ 
		MapYouObjInit();
		for(var i=0; i<MapRightObj.length; i++){
			Destroy(MapRightObj[i]);
		}
		MapRightObj = new Array(0);	
	}
	
//	var ts : TiShi;
	function ShowDoneTaskName(str : String){
		particDoneTask.Emit();
		AllManage.tsStatic.Show1("[ffff00] " + AllManage.AllMge.Loc.Get("messages152") + str + AllManage.AllMge.Loc.Get("messages153"));
		AllManage.tsStatic.LiaoTian(AllManage.AllMge.Loc.Get("messages152") + str + AllManage.AllMge.Loc.Get("messages153") , Color.yellow);
	}
	function ShowGetTaskName(str : String){
		AllManage.tsStatic.Show1("[ffff00] " + AllManage.AllMge.Loc.Get("messages152")  + str + AllManage.AllMge.Loc.Get("messages154"));
		AllManage.tsStatic.LiaoTian(AllManage.AllMge.Loc.Get("messages152")  + str + AllManage.AllMge.Loc.Get("messages154") , Color.yellow);
	}
	
	function ShowGetItemName(str : String){
		AllManage.tsStatic.LiaoTian(AllManage.AllMge.Loc.Get("info278") + str, Color.yellow);		
	}
	
	public var guidPanelPos : Transform;
	@HideInInspector
	public var guidPanelPos1 : Transform;
	function InTaskInfoStep1(){ 
//		print(isItemNPC + " isItemNPC ==" + OneTask.taskType);
		if(Step >= OneTask.dialog.length-1 && OneTask.jindu == 0 && !isItemNPC){
			if(!MainPS.CanAddTask()){
				AllManage.tsStatic.Show("tips072");
				return;
			}
			if(! stepIsServer){
				stepValue = "StepOne";				
				MainPS.AddNewTask(OneTask , 0);
				return;
			}else
			if(stepIsServer){
				MainPS.returnAddNewTask(serverNowTaskID);
				if(serverNowTaskID != OneTask.id){
					return;
				}
			}
			
			UICL.TaskKuangBack();
			OneTask.jindu = 1; 
		//	//print(OneTask);
			////print(OneTask.npc);
			if(OneTask.npc == null){
				OneTask.npc = AllManage.npcclStatic.GetNPCAsID(OneTask.mainNPC.id).gameObject;
			}
			OneTask.npc.SendMessage("SendJindu",OneTask , SendMessageOptions.DontRequireReceiver);
//			//print("jie xia ren wu");
			UICL.ShowTaskInfoList(MainPS.player,OneTask);
			if(OneTask.taskType == MainTaskType.jiaocheng){
				UICL.lookCanShowRealButtonAsID(OneTask.id);
				TaskJiaoChengC.SelectJiaoChengAsID(parseInt(OneTask.doneType));
			}else
			if(OneTask.taskType == MainTaskType.duihua){
				var gv = new GameValue();
				gv.task = OneTask;
				gv.mtw = this;
				gv.npcid = GetItemIDAsLevel(OneTask.doneType.Substring(0,4));
				OneTask.npc = AllManage.npcclStatic.SetNPCTask(gv);		
				AllManage.npcclStatic.NPCGuanBi(OneTask.mainNPC.id);
			}
			if(OneTask.taskType != MainTaskType.jiaocheng){
				UICL.FindNpcOtherTask();			
			}
//			yield;
//			if(UICL.IsMoreTaskCanGet()){
//				var pt : PrivateTask;
//				pt = new PrivateTask();
//				pt.jindu = OneTask.jindu;oDang;	SaoDan

//				pt.task = OneTask;
//				FindWay(pt);
//			}else{
				UICL.canOpenNPCTaskGO = true;
				UICL.OneNPCTalkNoClick();
//			}
			return;
		}else{ 
			if(OneTask.dialog2 != null){
//				//print("111");
//				if(UICL.NowNPCID == GetItemIDAsLevel(OneTask.doneType.Substring(0,4))){
					if(Step >= OneTask.dialog2.length-1 && OneTask.jindu == 1 && isItemNPC){ 
			//			UICL.TaskKuangBack();
			//			MainPS.AddNewDoneObject(OneTask); 	Sa uttonSa= AS.BoDang
//					//print("222");
							if(UICL.NowNPCID == GetItemIDAsLevel(OneTask.doneType.Substring(0,4))){
//					//print("133311");
								if(! stepIsServer){
									stepValue = "StepOne";				
									MainPS.TaskDone(OneTask); 
									return;
								}else
								if(stepIsServer){
									MainPS.returnTaskComplet(serverNowTaskID);
									if(serverNowTaskID != OneTask.id){
										return;
									}
								}
								OneTask.jindu += 2; 	
								Step = 0; 
								isItemNPC = false;
								if(OneTask.npc == null){
									OneTask.npc = AllManage.npcclStatic.GetNPCAsID(OneTask.mainNPC.id).gameObject;
								}
								OneTask.npc.SendMessage("SendJindu",OneTask , SendMessageOptions.DontRequireReceiver);
								UICL.TaskKuangBack();
								UICL.ShowTaskInfoList(MainPS.player,OneTask);		 
								UICL.FindNpcOtherTask();
								
								//start===================点击NPC对话按钮的新手引导=============================================================================================================================
								
//								if(OneTask.id == "4" || OneTask.id == "10" || OneTask.id == "499" || OneTask.id == "294"){
								if(OneTask.id == "4"){
									BeginnerGuideClickTalkBtn();
								}
								
								//end===================点击NPC对话按钮的新手引导=============================================================================================================================
							
							}else{
//					//print("444");
								 UICL.TaskKuangBack();
	//							UICL.TaskInfoKuangOn(OneTask.dialog[OneTask.dialog.length - 1],OneTask,false); 		
								UICL.FindNpcOtherTask();
							}
						return;
					}							
//				}else{
//					
//				}
			}			
		}
//		//print(OneTask.jindu);
//		//print(Step + " == " + OneTask.dialog.length);
		try{
			switch(OneTask.jindu){
				case 0 : 
						if(Step == OneTask.dialog.length - 2){
						    if(OneTask.id == "4"){
						        if(null == beginnersGuide)
						        {
						            CreateBeginnerGuid();
						        }
						        BeginnersGuide.beginnersGuide.SetTDString(String.Format("{0}{1}",OneTask.taskName,",点击接受按钮")); 
								BeginnersGuide.beginnersGuide.SwitchInstructionsBoxStyle(InstructionsBoxStyle.RightTop,guidPanelPos1, StaticLoc.Loc.Get("info832"), this.gameObject ,"InTaskInfoStep1", yuan.YuanPhoton.GameScheduleType.AcceptBtn);
							}
							
							UICL.TaskInfoKuangOn(OneTask.dialog[Step],OneTask,true,1); 					
						}else{
							UICL.TaskInfoKuangOn(OneTask.dialog[Step],OneTask,false,0); 										
						}
						Step += 1;
						if(Step >= OneTask.dialog.length-1 && !isItemNPC){
							UICL.ShowFindWay(OneTask);
						}
					break; 
				case 1 :
					if(!isItemNPC){
						UICL.TaskKuangBack();
						UICL.FindNpcOtherTask();
				}
					else{
						if(Step == OneTask.dialog2.length - 2){
							UICL.TaskInfoKuangOn(OneTask.dialog2[Step],OneTask,false,2);
						}else{
							UICL.TaskInfoKuangOn(OneTask.dialog2[Step],OneTask,false,0);
						}
						Step += 1;				
					}
					break;
				case 2 :
					if(OneTask.taskType == MainTaskType.wupin){
						if(GetAndUseTaskItem(OneTask.doneItem.Substring(0,4) , OneTask.doneNum , OneTask.id)){ 
							if(! stepIsServer){
								stepValue = "StepOne";				
								MainPS.TaskDone(OneTask); 
								return;
							}else
							if(stepIsServer){
								MainPS.returnTaskComplet(serverNowTaskID);
								if(serverNowTaskID != OneTask.id){
									return;
								}
							}
							OneTask.jindu += 1; 
							
							Step = 0;
							isItemNPC = false;
							if(OneTask.npc == null){
								OneTask.npc = AllManage.npcclStatic.GetNPCAsID(OneTask.mainNPC.id).gameObject;
							}
							OneTask.npc.SendMessage("SendJindu",OneTask , SendMessageOptions.DontRequireReceiver);
							UICL.TaskKuangBack();							
							UICL.FindNpcOtherTask();
						}
					}else{
						if(! stepIsServer){
							stepValue = "StepOne";				
							MainPS.TaskDone(OneTask);
							return;
						}else
						if(stepIsServer){
							MainPS.returnTaskComplet(serverNowTaskID);
							if(serverNowTaskID != OneTask.id){
								return;
							}
						}
						
						OneTask.jindu += 1; 
						Step = 0;
						isItemNPC = false;
						if(OneTask.npc == null){
							OneTask.npc = AllManage.npcclStatic.GetNPCAsID(OneTask.mainNPC.id).gameObject;
						}
						OneTask.npc.SendMessage("SendJindu",OneTask , SendMessageOptions.DontRequireReceiver);
						UICL.TaskKuangBack();			
						UICL.FindNpcOtherTask();
					}
					break;
			}
			UICL.ShowTaskInfoList(MainPS.player,OneTask);
		}catch(e){
			if(! stepIsServer){
				stepValue = "StepOne";				
				MainPS.AddNewTask(OneTask , 0);
				AllManage.UIALLPCStatic.show0();
				return;
			}
			AllManage.UIALLPCStatic.show0();
		}
	}
	
	function BeginnerGuideClickTalkBtn()
	{
	    //if(null == beginnersGuide)
	    //{
	    //    CreateBeginnerGuid();
	    //}

	    //BeginnersGuide.beginnersGuide.SetTDString(String.Format("{0}{1}",OneTask.taskName,",点击对话按钮"));
	    //BeginnersGuide.beginnersGuide.SwitchInstructionsBoxStyle(InstructionsBoxStyle.RightBottom,guidPanelPos, StaticLoc.Loc.Get("info826"), AllManage.UICLStatic.gameObject ,"OneNPCTalk", yuan.YuanPhoton.GameScheduleType.TalkBtn); 
	}
	
var MapYouItems : TaskSceneIcon[];
var LabelTemTeamReturn : UILabel;
var LabelTemTeamGO : UILabel;
function SetObj(AS : AwakeSet){
	MapYouParent = AS.MapYouParent ;
	UiGidYou = AS.UiGidYou ;
	MapRightObj = AS.MapRightObj;
	MapRightParent = AS.MapRightParent ;
	UiGid = AS.UiGid ;
	ChuansongRightBan = AS.ChuansongRightBan ;
	wuneirong = AS.wuneirong ;
	SpriteFlag = AS.SpriteFlag ;
	LabelFlag = AS.LabelFlag ;
	transRightInfo = AS.transRightInfo ;
	transRightTeam = AS.transRightTeam ;	
	LabelInfo = AS.LabelInfo ;
	nowRightSprite = AS.nowRightSprite ;
	BUttonZuDui = AS.BUttonZuDui ;
	ButtonSaoDang = AS.ButtonSaoDang;
	SaoDangSprite = AS.SaoDangSprite;
	MapYouItems = AS.MapYouItems;
	LabelTemTeamReturn = AS.LabelTemTeamReturn;
	LabelTemTeamGO = AS.LabelTemTeamGO;
} 

function show0(){
	yield;
	yield;
	yield;
	if(nowMen.barOpen){
		nowMen.barOpen = false;
	}
	MapRightObjInit();
	AllManage.UIALLPCStatic.show0();	
}

 	var pa : PanelActivity;
	var HDtsk : MainTask;
	private var DStr : String = ",";
	private var Fstr : String = ";";
	function SetHuoDongTask(){
		var i : int = 0;
		var tsk : MainTask;
		var useActivityStr : String; 
		var usePlayerGet : String[];
		var usePlayerDH : String[];
		
		if(!pa){
			pa = FindObjectOfType(PanelActivity);
		}
		if( ! pa.btnEnter.Disable){
			if(pa.taskFunType == TaskFunType.CanAccept){
//				if(pa.yrSelect["Type"].YuanColumnText == "2"){
//					InventoryControl.yt.Rows[0]["ActivtyPVP"].YuanColumnText +=  pa.yrSelect["ActivityTaskID"].YuanColumnText+",1;"; 
//					InRoom.GetInRoomInstantiate().ActivityPVPAdd(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("id" , pa.yrSelect["ActivityTaskID"].YuanColumnText)["MapID"].YuanColumnText);
//					pa.SetTaskFun("ActivtyPVP");
//					return;
//				}
				if(!MainPS.CanAddTask()){
					AllManage.tsStatic.Show("tips072");
					return;
				}
				HDtsk = GetTaskAsId(pa.yrSelect["ActivityTaskID"].YuanColumnText);
				if(HDtsk){
					tsk = HDtsk;
					if(AllManage.npcclStatic.GetNPCAsID(tsk.mainNPC.id)){
						tsk.npc = AllManage.npcclStatic.GetNPCAsID(tsk.mainNPC.id).gameObject;
						UICL.nowNPC = AllManage.npcclStatic.GetNPCAsID(tsk.mainNPC.id);
					}
					tsk.jindu = 1; 
					for(i=0; i<myTask.length; i++){
						if(myTask[i].id == tsk.id){
							myTask[i].jindu = tsk.jindu;
						}
					}
					MainPS.AddNewTask(tsk , 0);
					if(tsk.npc == null){
						var objj = AllManage.npcclStatic.GetNPCAsID(tsk.mainNPC.id);
						if(objj)
						tsk.npc = objj.gameObject;
					}
					if(tsk.npc){
						tsk.npc.SendMessage("SendJindu",tsk , SendMessageOptions.DontRequireReceiver);
					}
					UICL.ShowTaskInfoList(MainPS.player,tsk);
					if(tsk.taskType == MainTaskType.jiaocheng){
						UICL.lookCanShowRealButtonAsID(tsk.id);
						TaskJiaoChengC.SelectJiaoChengAsID(parseInt(tsk.doneType));
					}else
					if(tsk.taskType == MainTaskType.duihua){
						var gv = new GameValue();
						gv.task = tsk;
						gv.mtw = this;
						gv.npcid = GetItemIDAsLevel(tsk.doneType.Substring(0,4));
						if(tsk.npc){
							tsk.npc = AllManage.npcclStatic.SetNPCTask(gv);		
						}
						AllManage.npcclStatic.NPCGuanBi(tsk.mainNPC.id);
					}else
					if(tsk.taskType == MainTaskType.PVPKiller || tsk.taskType == MainTaskType.PVPWin){
						InRoom.GetInRoomInstantiate().ActivityPVPAdd(tsk.doneType.Substring(4,3));
						PVPControl.PVPTaskNanDu = AllManage.mtwStatic.GetTaskLevel( tsk.doneType );//1; //parseInt(tsk.doneType.Substring(13,1));
						AllManage.tsStatic.ShowBlue("tips101");
					}
					if(pa.yrSelect["Type"].YuanColumnText != "2"){
						MainPS.setNowTask(tsk);
						MainPS.GoWayNow();
					}	
					AllManage.UIALLPCStatic.show0();						
				}
//				UICL.FindNpcOtherTask();
				pa.SetTaskFun();
				InventoryControl.yt.Rows[0]["ActivtyTask"].YuanColumnText +=  pa.yrSelect["ActivityTaskID"].YuanColumnText+",1;";
			}else
			if(pa.taskFunType == TaskFunType.CanConsign){
//				if(pa.yrSelect["Type"].YuanColumnText == "2"){
//					useActivityStr = InventoryControl.yt.Rows[0]["ActivtyPVP"].YuanColumnText;
//					usePlayerGet = useActivityStr.Split(FStr.ToCharArray());
//					for(i=0; i<usePlayerGet.length; i++){
//						//print(usePlayerGet[i] + " == usePlayerGet[i]");
//						if(usePlayerGet[i].Length > 1){
//							usePlayerDH = usePlayerGet[i].Split(DStr.ToCharArray());
//							//print(usePlayerDH[0] + " == usePlayerDH[0] == " + pa.yrSelect["ActivityTaskID"].YuanColumnText);
//							//print(usePlayerDH[1] + " == usePlayerDH[1]");
//							if(usePlayerDH[0] == pa.yrSelect["ActivityTaskID"].YuanColumnText){
//								//print(pa.yrSelect["PlayerGet"].YuanColumnText + " == pg");
//								if(pa.yrSelect["PlayerGet"].YuanColumnText == "1"){
//									usePlayerGet[i] = "";
//								}else
//								if(pa.yrSelect["PlayerGet"].YuanColumnText == "0"){
//									usePlayerGet[i] = usePlayerDH[0] + ",3";
//								}
//							}
//						}
//					}
//					useActivityStr = "";
//					for(i=0; i<usePlayerGet.length; i++){
//						//print(usePlayerGet[i] + " == usePlayerGet[i]");
//						if(usePlayerGet[i] != ""){
//							useActivityStr += usePlayerGet[i] + ";"; 
//						}
//					}
//					InventoryControl.yt.Rows[0]["ActivtyPVP"].YuanColumnText = useActivityStr;
//					pa.SetItem();
//					pa.SetTaskFun("ActivtyPVP"); 
//					return;
//				}
				HDtsk = GetTaskAsId(pa.yrSelect["ActivityTaskID"].YuanColumnText);
				tsk = HDtsk;
				MainPS.TaskDone(tsk);
				pa.SetItem();
				useActivityStr = InventoryControl.yt.Rows[0]["ActivtyTask"].YuanColumnText;
				usePlayerGet = useActivityStr.Split(FStr.ToCharArray());
				for(i=0; i<usePlayerGet.length; i++){
		//			//print(usePlayerGet[i] + " == usePlayerGet[i]");
					if(usePlayerGet[i].Length > 1){
						usePlayerDH = usePlayerGet[i].Split(DStr.ToCharArray());
		//				//print(usePlayerDH[0] + " == usePlayerDH[0] == " + pa.yrSelect["ActivityTaskID"].YuanColumnText);
		//				//print(usePlayerDH[1] + " == usePlayerDH[1]");
						if(usePlayerDH[0] == pa.yrSelect["ActivityTaskID"].YuanColumnText){
		//					//print(pa.yrSelect["PlayerGet"].YuanColumnText + " == pg");
							if(pa.yrSelect["PlayerGet"].YuanColumnText == "1"){
								usePlayerGet[i] = "";
							}else
							if(pa.yrSelect["PlayerGet"].YuanColumnText == "0"){
								usePlayerGet[i] = usePlayerDH[0] + ",3";
							}
						}
					}
				}
				useActivityStr = "";
				for(i=0; i<usePlayerGet.length; i++){
		//			//print(usePlayerGet[i] + " == usePlayerGet[i]");
					if(usePlayerGet[i] != ""){
						useActivityStr += usePlayerGet[i] + ";"; 
					}
				}
				InventoryControl.yt.Rows[0]["ActivtyTask"].YuanColumnText = useActivityStr;
				pa.SetTaskFun(); 
			}else
			if(pa.taskFunType == TaskFunType.Doding){
//				if(pa.yrSelect["Type"].YuanColumnText == "2"){
//					InRoom.GetInRoomInstantiate().ActivityPVPAdd(YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytMapLevel.SelectRowEqual("id" , pa.yrSelect["ActivityTaskID"].YuanColumnText)["MapID"].YuanColumnText);
//					return;
//				}
				HDtsk = GetTaskAsId(pa.yrSelect["ActivityTaskID"].YuanColumnText);
				try{		
					for(i=0; i<MainPS.player.nowTask.length; i++){ 
						if(MainPS.player.nowTask[i] != null && HDtsk.id == MainPS.player.nowTask[i].task.id){
							if(pa.yrSelect["Type"].YuanColumnText != "2"){
								MainPS.setNowTask(MainPS.player.nowTask[i].task);
								MainPS.GoWayNow();
							}else{
								InRoom.GetInRoomInstantiate().ActivityPVPAdd(HDtsk.doneType.Substring(4,3));
								PVPControl.PVPTaskNanDu = AllManage.mtwStatic.GetTaskLevel( HDtsk.doneType );//1; //parseInt(HDtsk.doneType.Substring(13,1));
								AllManage.tsStatic.ShowBlue("tips101");
							}
							AllManage.UIALLPCStatic.show0();						
						}
					}
				}catch(e){
				
				}

			}
		}
	}
	
	function GetTaskLevel(str : String) : int{
		try{
			var lv : int = 0;
			lv = parseInt(str.Substring(13,1));
			if(lv == 5){
				return 5;		
			}else{
				return 1;
			}
		}catch(e){
			return 1;
		}
	}
}

function BtnTaskTempTeamAdd(){
	if(UIControl.TeamHeadYesOKmapid == "" && !UIControl.boolTeamHeadYes && UIControl.tempTeamPlayerGoMapID == ""){
		PanelStatic.StaticBtnGameManager.BtnTaskTempTeamAdd();	
	}else{
		if(AllManage.UICLStatic.boolTeamHeadYes)
			InRoom.GetInRoomInstantiate().RemoveTempTeam();
		else
			InRoom.GetInRoomInstantiate().TempTeamPlayerRemove();
			
		AllManage.creatFriendTeam.ShowTeamLayer(true);
	}
}

function AddActivitiesTask(taskid : String){
//	var acTask : MainTask;
//	acTask = GetTaskAsId(taskid);
	MainPS.returnAddNewTask(serverNowTaskID);
}
