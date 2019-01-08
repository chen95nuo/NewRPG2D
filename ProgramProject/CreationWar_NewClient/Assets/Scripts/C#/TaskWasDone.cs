using UnityEngine;
using System.Collections;

public class TaskWasDone : MonoBehaviour {
	//任务类型
	public enum MainTaskType {
		monster = 1, 			//杀死怪物
		talk = 2,					//对话
		arrive = 3,				//到达某地
		items = 4,				//收集物品
		duplicate = 5,			//完成副本
		Tutorials = 6,			//教程任务
		PVPKiller = 7,			//pvp杀人
		PVPWin = 8,			//pvp胜利
		Mining = 9				//采矿
	}
	
	public enum MainItemType {
		equipment = 0,
		garbage = 1,
		taskItem = 2,
		consumable = 3
	}
	
	public enum MainTaskNeedType {
		level = 1,
		taskBefore = 2,
		time = 3
	}
	
	public class MainTaskReward{
		public MainItemType itemType = 0;
		public string itemId = "000";
		public int gold = 0;
		public int exp = 0;
		public int rank = 0;
		public string taskDescription;
	}
	
	public class MainTaskNPC {
		public string id = "001";
		public string point = "001001";
		public string map = "01";
	}
	
	public class TaskDialog {
		public string npcLog ;
		public string playerLog ;
		public string npcId ;
	}
	
	public class MainTask {
		public string taskName;
		public MainTaskType taskType; 
		public string id ;
		public MainTaskNeedType needType ;
		public string needId; 
		public string doneType; 
		public int doneNum;
		public MainTaskNPC mainNPC;	
		public MainTaskReward RewardItemMaster = null;
		public MainTaskReward RewardItemSoldier = null;
		public MainTaskReward RewardItemRobber = null;
		public MainTaskReward reward = null;
		public int jindu ;	
		public TaskDialog[] dialog;
		public TaskDialog[] dialog2;
		public GameObject npc;
		public string doneItem;
		public bool readyDone =  false;
		public GameObject objCollider;
		public int leixing;
		public string ExcludeTask;
	}

	/// <summary>
	/// 完成任务方法
	/// </summary>
	/// <param name="taskID">任务id</param>
	void TaskDoneAsID(string taskID){
		//任务表,需要服务器取到 taskTable
		yuan.YuanMemoryDB.YuanTable taskTable = new yuan.YuanMemoryDB.YuanTable("taskTable","id"); 
		foreach(yuan.YuanMemoryDB.YuanRow rows in taskTable.Rows){
			if(rows["id"].YuanColumnText == taskID){
				MainTask task = new MainTask();
				task.taskName = rows["TaskName"].YuanColumnText;
				task.taskType =   (MainTaskType)int.Parse(rows["TaskType"].YuanColumnText);
				task.id =  rows["id"].YuanColumnText;
				task.needType = (MainTaskNeedType)int.Parse(rows["id"].YuanColumnText);
				task.needId = rows["TaskNeedInfo"].YuanColumnText;
				task.leixing = GetIntDBAsName("BranchType" , 0 , rows);
				task.jindu = 0;
				task.RewardItemMaster = getTaskReward(rows,task.RewardItemMaster , "RewardItemMaster");
				task.RewardItemSoldier = getTaskReward(rows,task.RewardItemSoldier , "RewardItemSoldier");
				task.RewardItemRobber = getTaskReward(rows,task.RewardItemRobber , "RewardItemRobber");
				task.reward = getTaskReward(rows,task.RewardItemRobber , "RewardItemMaster");
				task.doneType = rows["ConditonInfo"].YuanColumnText; 
				task.ExcludeTask = rows["ExcludeTask"].YuanColumnText; 
				if(task.taskType == (MainTaskType)4){
					task.doneItem = task.doneType.Substring(15,7);
				}

				string itemid2;
				if(task.reward.itemId.Length > 4){
					itemid2 = task.reward.itemId.Substring(0,2);
					if(itemid2 == "88"){
						//道具id
						//	task.reward.itemId;
					}else
					if(itemid2 == "72"){
						//坐骑id
						//task.reward.itemId;			
					}else{
						//装备id
						//	task.reward.itemId; 
						InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem , task.reward.itemId);
					}
				}
				//经验值
				//task.reward.exp

				//增加的声望值，字段"Prestige"
				//20

				//金币
				//task.reward.gold

				//血石
				//task.reward.rank

				//角色表,需要服务器取到 playerTable
				yuan.YuanMemoryDB.YuanTable playerTable = new yuan.YuanMemoryDB.YuanTable("playerTable","id"); 
				playerTable.Rows[0]["CompletTask"].YuanColumnText += taskID + ";";
				string[] strArray ;
				string[] taskStrs;
				int i;
				strArray = playerTable.Rows[0]["Task"].YuanColumnText.Split(';');
				for(i=0; i<strArray.Length; i++ ){
					taskStrs = strArray[i].Split(',');
					if(taskStrs.Length > 1 && taskStrs[0] == taskID){
						strArray[i] = "";
					}
				}
				for(i=0; i<strArray.Length; i++){
					if(strArray[i] != "")
						playerTable.Rows[0]["Task"].YuanColumnText += strArray[i] + ";";
 				}
			}
		}
	}

	MainTaskReward getTaskReward( yuan.YuanMemoryDB.YuanRow rows , MainTaskReward reward , string str){
		reward = new MainTaskReward();
		reward.itemId = rows[str].YuanColumnText;
		reward.exp = GetIntDBAsName("RewardExp" , 0 , rows);
		reward.gold = GetIntDBAsName("RewardGold" , 0 , rows);
		reward.rank = GetIntDBAsName("RewardBloodStone" , 0 , rows);
		reward.taskDescription = rows["TaskInfo"].YuanColumnText;
		return reward;
	}

	int GetIntDBAsName(string name , int ret , yuan.YuanMemoryDB.YuanRow rows){
		int i = 0;
		try{ 
			i = int.Parse(rows[name].YuanColumnText);
			return i;
		}catch(System.Exception e){
			Debug.Log(e.ToString());
			return ret;
		}
	}
}
