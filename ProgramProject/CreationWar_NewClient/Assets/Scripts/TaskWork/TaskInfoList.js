#pragma strict
class TaskInfoList extends Song{
	function Awake(){
		AllManage.taskILStatic = this;
	}

	function Start () {
	//	popup.items.Add("sssss");
//		NowLabel.text = AllManage.AllMge.Loc.Get("info013");
		nowTaskInfo = null;
		TweenTaskAll.Play(false);
	//	TaskPanel.enabled = false;
	}

	var popup : UIPopupList; 
	private var ptime : int = 0;
	function Update () {
		if (Input.GetButtonUp ("Fire1")) {  
			CloseMyTween();
			PanelInfo.enabled = false;
		} 
		if(Time.time > ptime){
			ptime = Time.time + 1; 
			LookGuang();
		}
	//	//print(popup.selection);
	}

	var mainPS : MainPersonStatus; 
	var item5 : Transform;
	function LookGuang() : boolean{
		if(mainPS == null){
			if(PlayerStatus.MainCharacter != null){
				mainPS = PlayerStatus.MainCharacter.gameObject.GetComponent(MainPersonStatus);
			}
		}
		CanShowQuan = false; 
		var i : int = 0;
		if(mainPS != null){ 
			if(mainPS.navMA.enabled){
				CanShowQuan = false;
			}else{
				if(nowTaskInfo != null && nowTaskInfo.task && nowTaskInfo.task.doneType){ 
					if(nowTaskInfo.task.doneType.Length > 3){
						if(UIControl.mapType == MapType.fuben && MainTaskWork.MapID == nowTaskInfo.task.doneType.Substring(4,3)){
							if(nowTaskInfo.task.taskType == MainTaskType.daoda){
								CanShowQuan = true;		
							}
						}else{
							CanShowQuan = true;
						}
					}else{
						CanShowQuan = true;		
					}
				}else
				if(item5.localPosition.y != 1000){
					CanShowQuan = true;					
				}
			}
		}
		if(CanShowQuan){ 
			GuangQuanObj.SetActiveRecursively(true);
			return true;
		}else{
			GuangQuanObj.SetActiveRecursively(false);
			return false;
		} 
		return true;
	}

	//function ListOpenTaskInfo(){ 
	//	popup.items.Clear();
	//	for(var i=0; i<Player.nowTask.length; i++){ 
	//		if(Player.nowTask[i] != null){
	//			popup.items.Add( "("+UIControl.taskLeixingStrs[Player.nowTask[i].task.leixing] + ")" + Player.nowTask[i].task.taskName + "  " +Player.nowTask[i].doneNum + "/" + Player.nowTask[i].task.doneNum);
	//		}
	//	}
	//}


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	var Player : MainPlayerStatus;
	function SetNowTasklist(p : MainPlayerStatus){
		Player = p;
		ShouNowTaskList(Player);
	//	//print("shua xin player");
	}

	var nowTaskInfo : PrivateTask = null;
	var CanShowQuan : boolean = false;
	var GuangQuanObj : GameObject;
	function ShouNowTaskList(p : MainPlayerStatus){
		var i : int = 0;
	//	CanShowQuan = false;
		for(i=0; i<Player.nowTask.length; i++){ 
			if(Player.nowTask[i] != null){ 
				if(nowTaskInfo == null && Player.nowTask[i].task != null){
	//				CanShowQuan = true;
					setNow(Player.nowTask[i]);
				}else
				if(nowTaskInfo && nowTaskInfo.id == Player.nowTask[i].id){
	//				CanShowQuan = true;
					setNow(Player.nowTask[i]);				
				}
			}
		}
		for(i=0; i<Player.nowTask.length; i++){ 
			if(Player.nowTask[i] != null){ 
				if(Player.nowTask[i].task != null && Player.nowTask[i].task.leixing == 0){
					setNow(Player.nowTask[i]);
				}
			}
		}
	//	if(CanShowQuan){
	//		GuangQuanObj.SetActiveRecursively(true);
	//	}else{
	//		GuangQuanObj.SetActiveRecursively(false);
	//	}
	}

	var NowObj : GameObject; 
	var NowLabel : UILabel; 
	private var useStr : String;
	private var useTaskDescription : String;
	function setNow(p : PrivateTask){ 
		uicl.isTaskGroundBack = true;
		nowTaskInfo = p;
		NowObj.SetActiveRecursively(true);  
		NowObj.transform.localPosition.y = 269.72;
		InfoItemGet.transform.localPosition.y = 1000;
		try{
			useStr = "("+UIControl.taskLeixingStrs[p.task.leixing] + ")" + p.task.ComplateInfo + "  " +p.doneNum + "/" + p.task.doneNum;
			useTaskDescription = p.task.reward.taskDescription + "\n" + p.doneNum + "/" + p.task.doneNum + "\nGold : " + p.task.reward.gold + "\nEXP : " + p.task.reward.exp + "\nRank : " + p.task.reward.rank;
			NowLabel.text = useStr;
			NowLabel.color = Color.green;
		}catch(e){
		
		}
//		FindWay();
	//	//print(useStr + "=======las;jdflasjdflasdjk");
	}

	var taskDes : TaskDescription;
	//var invMaker : Inventorymaker;
	private var inv : InventoryItem;
	var TaskNowCL : TaskNowControl;
	//var allpcl : UIAllPanelControl;
	function showInfo(){
		AllManage.UIALLPCStatic.show6(Player);
	//	//print("123123123123123");
	//	TaskNowCL.SetPlayerTask(Player);
	//	var str : String;
	//	str = useTaskDescription;
	//	if(nowTaskInfo != null){
	//		if(nowTaskInfo.task.reward.itemId != "0"){
	//			str = "\n" + "\n" + useTaskDescription;
	//			taskDes.SetTaskinv(invMaker.GetItemInfo(nowTaskInfo.task.reward.itemId,inv));
	//		}else{
	//			str = useTaskDescription;
	//			taskDes.NonTaskinv();
	//		}
	//	}
	//	UITooltipSong.ShowText(str);
	}
	
	function showActivity(){
		AllManage.UIALLPCStatic.show8();
	}
	
	function showGHTask(){
		AllManage.UIALLPCStatic.show35();
	}
	

	function NowDone(p : PrivateTask){ 
	//	//print(nowTaskInfo.id + " == " + p.id);
		if(nowTaskInfo != null && p != null && nowTaskInfo.id == p.id){
			nowTaskInfo = null;
			NowObj.SetActiveRecursively(false);  
			NowObj.transform.localPosition.y = 1000;
		}
	}

//	var TweenInfo : TweenPosition;
	var InfoItem : TaskInfoItem[]; 
	var PanelInfo : UIPanel;
	function OpenTaskInfo(){ 
	//	//print("zhi xing ");
		uicl.isTaskGroundBack = true;
		var i : int = 0;
		var m : int = 0; 
		var isadd : boolean = false;
		for(i=0; i<InfoItem.length; i++){
			InfoItem[i].pTask = null;
			InfoItem[i].gameObject.SetActiveRecursively(false); 	
		}
		yield;
		for(i=0; i<Player.nowTask.length; i++){ 
			if(Player.nowTask[i] != null){
				isadd = true;
				for(m=0; m<InfoItem.length; m++){
					if(InfoItem[m].pTask == null && isadd){
						isadd = false;
						InfoItem[m].gameObject.SetActiveRecursively(true); 
						InfoItem[m].setTask(Player.nowTask[i]);
					}
				}
			}
		}
		PanelInfo.enabled = true;
//		TweenInfo.Play(true); 
	}

	var mtw : MainTaskWork;
	private var npcTs : NPCTrigger[];
	function FindWay(){
//		var i=0;
//		print("ssss == " + nowTaskInfo); 
		if(Application.loadedLevelName == "Map200"){
			return;
		}
//		npcTs = FindObjectsOfType(NPCTrigger);
//		for( i=0; i<npcTs.length; i++){
//			npcTs[i].initMe();
//		}
		if(UIControl.mapType != MapType.jingjichang){
//			print(nowTaskInfo);
			if(nowTaskInfo != null && nowTaskInfo.task != null){
				mtw.FindWay(nowTaskInfo);				
			}else
//			if(canGetTask != null){
			if(canGetTask && canGetTask.leixing == 0 && canGetTask.taskName != null && canGetTask.taskName.Length > 0 && UIControl.mapType == MapType.zhucheng){
					mtw.FindNPC(canGetTask);
//				}
			}else{
				mtw.Gochuansong();
				AllManage.tsStatic.Show("tips080");
			}
		}
//		yield;
//		yield;
//		yield;
//		yield;
//		yield;
//		yield;
////		print("ssss == " + nowTaskInfo); 
//		if(Application.loadedLevelName == "Map200"){
//			return;
//		}
//		npcTs = FindObjectsOfType(NPCTrigger);
//		for( i=0; i<npcTs.length; i++){
//			npcTs[i].initMe();
//		}
//		if(UIControl.mapType != MapType.jingjichang){
////			print(nowTaskInfo);
//			if(nowTaskInfo != null){	
//				mtw.FindWay(nowTaskInfo);
//			}else{
//				AllManage.tsStatic.Show("tips080");
//			}
//		}
	}
//function FindWay(){
////	print("ssss == " + nowTaskInfo); 
//	if(Application.loadedLevelName == "Map200"){
//		return;
//	}
//
//	if(UIControl.mapType != MapType.jingjichang){
//		if(nowTaskInfo != null){	
//			mtw.FindWay(nowTaskInfo);
//		}else{
//			AllManage.tsStatic.Show("tips080");
//		}
//	}
//}

	var canGetTask : MainTask;
	var canGetTaskLabel : UILabel;
	function SetCanGetTaskInfo(task : MainTask){
		canGetTask = task;
//		print(task.taskName + " == task.taskName");
		InfoItemGet.gameObject.SetActiveRecursively(true);
		InfoItemGet.SetGetTask(canGetTask);
		InfoItemGet.transform.localPosition.y = 267.9146;
		NowObj.transform.localPosition.y = 1000;
		useStr = "("+UIControl.taskLeixingStrs[task.leixing] + ")" + task.taskName;
//		print(useStr);
		canGetTaskLabel.text = useStr;
	}

	var InfoItemGet :  TaskInfoItem;
	function AlreadyGetTask(id : String){
		if(canGetTask){
			if(canGetTask.id == id){
				canGetTask = null;
				InfoItemGet.gameObject.SetActiveRecursively(false);	
			}	
		}
	}

	function FindNPC(){
		mtw.FindNPC(canGetTask);
	}

	function CloseMyTween(){
//		TweenInfo.Play(false);
	}

	var TweenTaskAll : TweenPosition;
	var TaskPanel : UIPanel;
	var uicl : UIControl;
	function Opendaohan(){
	//TaskPanel.widgetsAreStatic = false;
	TweenTaskAll.Play(true);
	uicl.isTaskGroundBack = true;
		OpenTaskInfo();
	}
}