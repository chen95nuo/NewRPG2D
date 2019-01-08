#pragma strict

var pTask : PrivateTask;
//var tLabel : UILabel;
private var useStr : String;
private var useTaskDescription : String;
function setTask(p : PrivateTask){
	pTask = p;
	useStr =  "("+UIControl.taskLeixingStrs[pTask.task.leixing] + ")" + pTask.task.ComplateInfo + " "+ p.doneNum + "/" + p.task.doneNum;
	useTaskDescription = p.task.reward.taskDescription + "\n" + p.doneNum + "/" + p.task.doneNum + "\nGold : " + p.task.reward.gold + "\nEXP : " + p.task.reward.exp + "\nRank : " + p.task.reward.rank;	
//	tLabel.text = useStr;
}

var taskDes : TaskDescription;
//var invMaker : Inventorymaker;
private var inv : InventoryItem;
var NowTaskID : int = 0;
function showInfo(){
	var str : String;
	str = useTaskDescription;
	if(pTask != null){
		if(pTask.task.reward.itemId != ""){
			str = "\n" + "\n" + useTaskDescription;
			taskDes.SetTaskinv(AllResources.InvmakerStatic.GetItemInfo(pTask.task.reward.itemId,inv));
		}else{
			str = useTaskDescription;
			taskDes.NonTaskinv();		
		}
	}
	UITooltipSong.ShowText(str);
}

var infoList : TaskInfoList;
function selectMe(){
//	//print("xuan ze le wo");
//	//print(pTask.task.taskType + " ====== pTask");
	if(pTask && pTask.task && pTask.task.taskType != 0){
		infoList.setNow(pTask);
//		infoList.TweenInfo.Play(false); 
		infoList.FindWay();
	}
}


function SetGetTask(task : MainTask){
	pTask.task = task;
	useStr = "("+UIControl.taskLeixingStrs[task.leixing] + ")" +  pTask.task.ComplateInfo + " 0" + "/" + task.doneNum;
	useTaskDescription = pTask.task.reward.taskDescription + "\n" + pTask.doneNum + "/" + pTask.task.doneNum + "\nGold : " + pTask.task.reward.gold + "\nEXP : " + pTask.task.reward.exp + "\nRank : " + pTask.task.reward.rank;	
//	tLabel.text = useStr;
}

var FirstObj : Transform;
//var PanelFirst : UIPanel;
private var ps : PlayerStatus;
private var npcTs : NPCTrigger[];
function GoTuGet(){ 
//	var i=0;
	if(Application.loadedLevelName == "Map200"){
		return;
	}
//		npcTs = FindObjectsOfType(NPCTrigger);
//		for( i=0; i<npcTs.length; i++){
//			npcTs[i].initMe();
//		}
	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
	}
	if(UIControl.mapType != MapType.jingjichang && ps != null && ! ps.dead){
		if(Application.loadedLevelName == "Map200" && mtw != null){
			go1();
		}else{
			if(FirstObj){
				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.ClickGOBtn).ToString());
				
				//InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.FirstClickGO).ToString());
//Debug.Log("+++++++++++++++++++++++++++++++++   FirstClickGO");
				//PanelFirst.enabled = false;
				//FirstObj.localPosition.y = 3000;
				
				//Destroy(FirstObj.gameObject);
			}

			infoList.FindNPC();	
		}
	}
//	yield;
//	yield;
//	yield;
//	yield;
//	yield;
//	if(Application.loadedLevelName == "Map200"){
//		return;
//	}
//		npcTs = FindObjectsOfType(NPCTrigger);
//		for( i=0; i<npcTs.length; i++){
//			npcTs[i].initMe();
//		}
//	if(ps == null && PlayerStatus.MainCharacter && PlayerStatus.MainCharacter != null){
//		ps = PlayerStatus.MainCharacter.gameObject.GetComponent(PlayerStatus);
//	}
//	if(UIControl.mapType != MapType.jingjichang && ps != null && ! ps.dead){
//		if(Application.loadedLevelName == "Map200" && mtw != null){
//			go1();
//		}else{
//			if(FirstObj){
//				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameSchedule , parseInt(yuan.YuanPhoton.GameScheduleType.ClickGOBtn).ToString());
//				PanelFirst.enabled = false;
//				FirstObj.localPosition.y = 1000;
//			}
//			infoList.FindNPC();	
//		}
//	}
	
}

var mtw : MainTaskWork;
var chuansongObj : Transform;
function go1(){
	////print("zou ");
	mtw.FindWay(chuansongObj.position);
}
