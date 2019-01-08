#pragma strict
import System;
//import System.Collections;
import System.Xml;
import System.Xml.Serialization; 
import System.Xml.XPath;
import System.IO;
import System.Text;
		
public enum MainTaskType {
	guaiwu = 1,
	duihua = 2,
	daoda = 3,
	wupin = 4,
	fuben = 5,
	jiaocheng = 6,
	PVPKiller = 7,
	PVPWin = 8,
	WaKuang = 9,
	Robot = 10
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
	var itemType : MainItemType = 0;
	var itemId : String = "000";
	var gold : int = 0;
	var exp : int = 0;
	var rank : int = 0;
	var taskDescription : String;
	var rewardExpType : int;
}

public class MainTaskNPC {
	var id : String = "001";
	var point : String = "001001";
	var map : String = "01";
}

public class TaskDialog {
	var npcLog : String;
	var playerLog : String;
	var npcId : String;
}

public class MainTask {
	var taskName : String;
	var taskType : MainTaskType; 
	var id : String;
	var needType : MainTaskNeedType;
	var needId : String; 
	var doneType :  String; 
	var doneNum : int;
	var mainNPC : MainTaskNPC;	
	var RewardItemMaster : MainTaskReward = null;
	var RewardItemSoldier : MainTaskReward = null;
	var RewardItemRobber : MainTaskReward = null;
	var reward : MainTaskReward = null;
	var jindu : int;	
	var dialog : TaskDialog[];
	var dialog2 : TaskDialog[];
	var npc : GameObject;
	var doneItem : String;
	var readyDone : boolean =  false;
	var objCollider : GameObject;
	var leixing : int;
	var ExcludeTask : String;
	var ComplateInfo : String;
}

public class TaskMap{
	var mapID : String;
	var nandu : int;
}

public class MainPlayerStatus{
	var level : int = 1;
	var name : String;
	var pro : int;
	var doneTaskID : String[];
	var nowTask : PrivateTask[];
	var doneTaskMap : TaskMap[];
}

public class PrivateTask{
	var id : String;
	var jindu : int;
	var doneNum : int;
	var task : MainTask;
	var taskInfo : TaskinfoParent;
}

class XmlControl extends Photon.MonoBehaviour{
	function Awake(){
	
	}
//	function OnLevelWasLoaded (level : int) {
//		if(level != 15 && level != 16){
//			Awake();
//			Start();
//		}
//	}
	class TaskData {
  		  // We have to define a default instance of the structure
  		 public var user : MainTask[];
  		  // Default constructor doesn't really do anything at the moment
  		 function TaskData() { }
	}
	class PlayerStatusData {
  		  // We have to define a default instance of the structure
  		 public var user : MainPlayerStatus;
  		  // Default constructor doesn't really do anything at the moment
  		 public function PlayerStatusData() { }
	}
	
	var st : yuan.YuanTimeSpan;

	static var mm : boolean = false;
	static var yt : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("Task1","id"); 
	static var NPCInfo : yuan.YuanMemoryDB.YuanTable = new yuan.YuanMemoryDB.YuanTable("NPCInfo11","id");
	function Start() : IEnumerator{
		yt = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytTask;
		NPCInfo = YuanUnityPhoton.GetYuanUnityPhotonInstantiate().ytNPCInfo;
	}
	function GetItemIDAsLevel(level : String) : String{
		var str : String;
		str = parseInt(level).ToString();
//		//print(" str ====== " + str + " == " + NPCInfo.Rows.Count);
		for(var rows : yuan.YuanMemoryDB.YuanRow in NPCInfo.Rows){
//			//print(rows["id"].YuanColumnText);
			if(rows["id"].YuanColumnText == str){
				return rows["NPCID"].YuanColumnText;
			}
		}
		return "";
	}
	
	public var SceneTaskList : Array;
	private var Tasknode : XmlNode;
	private var xmlelement : XmlElement;
	function SceneTaskInit(play : MainPlayerStatus){
//		if(nodeTaskList == null)
//			LoadAllTask();
//		//print(yt.Count);
		SceneTaskList = new Array();
//					//print(rows["TaskName"].YuanColumnText + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"); 
		for(var rows : yuan.YuanMemoryDB.YuanRow in yt.Rows){
//		for(Tasknode in nodeTaskList){
//			xmlelement = SetToElement(xmlelement,Tasknode);
			var cType : String = rows["TaskNeedType"].YuanColumnText;
			var cValue : String = rows["TaskNeedInfo"].YuanColumnText;
//			//print(cType + " ==ren wu tiao jian== " + cValue);
			var bool : boolean = false;
			bool = LookTaskPaiChi(play , rows["ExcludeTask"].YuanColumnText);
			if(bool){
				switch(cType){
					case "1" :
	//				//print(rows["id"].YuanColumnText + " == task ID" + cType + " == " + cValue + " == " + play.level);
					if(parseInt(play.level) >= parseInt(cValue)){
							if(rows["ProLimit"].YuanColumnText != "0"){
								if(play.pro.ToString() == rows["ProLimit"].YuanColumnText){
									SceneTaskList.Add(rows["id"].YuanColumnText);													
								}
							}else{
								SceneTaskList.Add(rows["id"].YuanColumnText);						
							}
						}
						break;
					case "2" :
						for(var ids : String in play.doneTaskID){
							if(ids == cValue){
	//							//print("ke yi jie " + rows["id"].YuanColumnText);
								if(rows["ProLimit"].YuanColumnText != "0"){
									if(play.pro.ToString() == rows["ProLimit"].YuanColumnText){
											SceneTaskList.Add(rows["id"].YuanColumnText);
									}
								}else{
									SceneTaskList.Add(rows["id"].YuanColumnText);						
								}
							}
						}
						break;
				}
			}
		}
		if(SceneTaskList.length == 0){
//			//print("mei you ke yi jie de ren wu");
		}
	}
	
	function LookTaskPaiChi( play : MainPlayerStatus , paichiID : String) : boolean{
		if(paichiID == "-1" || paichiID == ""){
			return true;
		}
		var bool : boolean = true;
		var i : int = 0;
		for(i=0; i<play.doneTaskID.length; i++){
			if(paichiID == play.doneTaskID[i]){
				bool =  false;
			}
		}
		for(i=0; i<play.nowTask.length; i++){
			if(play.nowTask[i] && paichiID == play.nowTask[i].task.id){
				bool =  false;
			}
		}
		return bool;
	}
	
	function GetTaskAsId(id : String) : MainTask{
		var task : MainTask = new MainTask();
		for(var rows : yuan.YuanMemoryDB.YuanRow in yt.Rows){
//			xmlelement = SetToElement(xmlelement,Tasknode);
			var cId : String = rows["id"].YuanColumnText;
//			//print(cId + " == " + id);
			if(cId == id){
				task = getAllAttribute(rows,task);
				return task;
			}
		}
		return null;
	}
	
	private var useTask : MainTask;
	function inquireXmlTask(play : MainPlayerStatus) : MainTask{
		//useTask = new MainTask();
//		var Tasknode : XmlNode;
//		var xmlelement : XmlElement;
		for(var rows : yuan.YuanMemoryDB.YuanRow in yt.Rows){
//		for(Tasknode in nodeTaskList){
//			xmlelement = SetToElement(xmlelement,Tasknode);
			var cType : String = xmlelement.GetAttribute("conditionType");
			var cValue : String = xmlelement.GetAttribute("conditionId");
//			//print(cType);
			switch(cType){
				case "1" :
					if(parseInt(play.level) >= parseInt(cValue)){
						useTask = getAllAttribute(rows,useTask); 
						return useTask;
//						can = true;
					}
					break;
				case "002" :
					break;
			}
		}
		return null;
	}
	
//	function getAllAttribute(xn : XmlNode , task : MainTask){
	function getAllAttribute(rows : yuan.YuanMemoryDB.YuanRow , task : MainTask){
//		var xmlelement : XmlElement;
//		xmlelement = SetToElement(xmlelement,xn); 
		task = new MainTask();
		
//		var xmlelementStatus : XmlElement;
//		xmlelementStatus = SetToElement(xmlelementStatus,xn.ChildNodes[0]);
//		//print(task + " == " + xmlelement);
		task.taskName = rows["TaskName"].YuanColumnText;
		task.taskType =  parseInt(rows["TaskType"].YuanColumnText);
		task.id =  rows["id"].YuanColumnText;
		task.needType = parseInt(rows["id"].YuanColumnText);
		task.needId = rows["TaskNeedInfo"].YuanColumnText;
		task.leixing = GetIntDBAsName("BranchType" , 0 , rows);
		task.jindu = 0;
		task.mainNPC = LoadXmlNPC(rows, rows["NPC"].YuanColumnText , task.mainNPC);
		task.RewardItemMaster = getTaskReward(rows,task.RewardItemMaster , "RewardItemMaster");
		task.RewardItemSoldier = getTaskReward(rows,task.RewardItemSoldier , "RewardItemSoldier");
		task.RewardItemRobber = getTaskReward(rows,task.RewardItemRobber , "RewardItemRobber");
		task.reward = getTaskReward(rows,task.reward , "RewardItemMaster");
		task.dialog = getTaskDialog(rows,0);
		task.dialog2 = getTaskDialog(rows,1);
		task.doneType = rows["ConditonInfo"].YuanColumnText; 
		task.ExcludeTask = rows["ExcludeTask"].YuanColumnText; 
		task.ComplateInfo = rows["ComplateInfo"].YuanColumnText; 
		if(task.taskType == 4){
			task.doneItem = task.doneType.Substring(15,7);
		}
		task.doneNum = GetIntDBAsName("ConditonNum" , 0 , rows);
		return task;
	}

	
	var DuiHua : String;
	private var textFen : String = ";";
	private var textQuan : String = "@";
	private var textJing : String = "#";
	function getTaskDialog(rows : yuan.YuanMemoryDB.YuanRow,ss : int){
		DuiHua = rows["TaskTalk"].YuanColumnText;
//		//print(rows["id"].YuanColumnText + " == " + DuiHua + " ==");
		var MainDuiHua = DuiHua.Split(textFen.ToCharArray());
		var ZiDuiHua = MainDuiHua[ss];
		var RenDuihua = ZiDuiHua.Split(textJing.ToCharArray());
		var log : TaskDialog[];
		log = new Array(RenDuihua.length);
		for(var i=0; i<RenDuihua.length; i++){
			var dhs : String = RenDuihua[i];
			if(dhs.Length > 5){
				var dh = dhs.Split(textQuan.ToCharArray());
				log[i] = new TaskDialog();
				log[i].npcLog = dh[0];
				log[i].playerLog = dh[1];
			
			}
		}
		return log;
	}
	
	function getTaskReward(rows : yuan.YuanMemoryDB.YuanRow , reward : MainTaskReward , str){
		reward = new MainTaskReward();
		var xmlelement : XmlElement;
//		var logNodes = xn.ChildNodes[1];
//		xmlelement = SetToElement(xmlelement,logNodes);
//		reward.itemType = parseInt(xmlelement.GetAttribute("itemType"));
		reward.itemId = rows[str].YuanColumnText;
		reward.exp = GetIntDBAsName("RewardExp" , 0 , rows);
		reward.gold = GetIntDBAsName("RewardGold" , 0 , rows);
		reward.rank = GetIntDBAsName("RewardBloodStone" , 0 , rows);
//		reward.rank = parseInt(xmlelement.GetAttribute("rank"));
		reward.taskDescription = rows["TaskInfo"].YuanColumnText;
		reward.rewardExpType = GetIntDBAsName("RewardExpType" , 0 , rows);
		return reward;
	}
	function GetIntDBAsName(name : String , ret  : int , rows : yuan.YuanMemoryDB.YuanRow) : int{
		var i : int = 0;
		try{ 
			i = parseInt(rows[name].YuanColumnText);
			return i;
		}catch(e){
			return ret;
		}
	}

	function LoadXmlNPC(rows : yuan.YuanMemoryDB.YuanRow ,NPCid : String , npc : MainTaskNPC) : MainTaskNPC{
			npc =  new MainTaskNPC();
					npc.id = rows["NPC"].YuanColumnText ;
					npc.point = "000000";
//					print(rows["NPCatMap"].YuanColumnText + " ========================== ");
					if( rows["NPCatMap"].YuanColumnText.Length > 2){
						npc.map =  rows["NPCatMap"].YuanColumnText.Substring(0,3);
					}else{
						npc.map = "999";
					}
		return npc;
	}
	
	
	function GetString(i : int, length : int){
		var str : String;
		str = i.ToString();
		while(str.length < length){
			str = "0" + str;
		}
		return str;
	}
}
