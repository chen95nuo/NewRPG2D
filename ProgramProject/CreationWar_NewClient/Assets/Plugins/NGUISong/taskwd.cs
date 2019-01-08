//#region 完成任务相关
//using UnityEngine;
//
//public class taskwd : MonoBehaviour
//{
//	/// <summary>
//	/// 完成任务方法
//	/// </summary>
//	/// <param name="taskID">任务id</param>
//	void TaskDoneAsID(OperationRequest operationRequest)
//	{
//	    try
//	    {
//	        if (myTable != null)
//	        {
//	            //任务表,需要服务器取到 taskTable
//	            //yuan.YuanMemoryDB.YuanTable taskTable =
//	            YuanTable ytTask = null;
//	            string taskID = (string)operationRequest.Parameters[(byte)ParameterType.ItemID];
//	            if (YuanServerApplication.PubTask.TryGetValue(taskID, out ytTask))
//	            {
//	                //YuanRow rows = ytTask[0];
//	                
//
//	                YuanPhotonServer.TaskWasDone.MainTask task = new YuanPhotonServer.TaskWasDone.MainTask();
//	                task.taskName = ytTask[0]["TaskName"].YuanColumnText;
//	                task.taskType = (YuanPhotonServer.TaskWasDone.MainTaskType)int.Parse(ytTask[0]["TaskType"].YuanColumnText);
//	                task.id = ytTask[0]["id"].YuanColumnText;
//	                task.needType = (YuanPhotonServer.TaskWasDone.MainTaskNeedType)int.Parse(ytTask[0]["id"].YuanColumnText);
//	                task.needId = ytTask[0]["TaskNeedInfo"].YuanColumnText;
//	                task.leixing = YuanPhotonServer.TaskWasDone.GetIntDBAsName("BranchType", 0, ytTask[0]);
//	                task.jindu = 0;
//	                task.RewardItemMaster = YuanPhotonServer.TaskWasDone.GetTaskReward(ytTask[0], task.RewardItemMaster, "RewardItemMaster");
//	                task.RewardItemSoldier = YuanPhotonServer.TaskWasDone.GetTaskReward(ytTask[0], task.RewardItemSoldier, "RewardItemSoldier");
//	                task.RewardItemRobber = YuanPhotonServer.TaskWasDone.GetTaskReward(ytTask[0], task.RewardItemRobber, "RewardItemRobber");
//	                task.reward = YuanPhotonServer.TaskWasDone.GetTaskReward(ytTask[0], task.RewardItemRobber, "RewardItemMaster");
//	                task.doneType = ytTask[0]["ConditonInfo"].YuanColumnText;
//	                task.ExcludeTask = ytTask[0]["ExcludeTask"].YuanColumnText;
//						task.doneNum = ytTask[0]["ConditonNum"].YuanColumnText;
//	                if (task.taskType == YuanPhotonServer.TaskWasDone.MainTaskType.items)
//	                {
//	                    task.doneItem = task.doneType.Substring(15, 7);
//	                }
//						
//
//	                Dictionary<string, string> tempParms = new Dictionary<string, string>();//临时回传字典，用来储存键值对
//
//	                string itemid2;
//	                if (task.reward.itemId.Length > 4)
//	                {
//	                    itemid2 = task.reward.itemId.Substring(0, 2);
//	                    if (itemid2 == "88")
//	                    {
//	                        //道具id
//	                        //task.reward.itemId;
//	                        string itemID=task.reward.itemId.Split(',')[0];
//	                        int itemNum=int.Parse(task.reward.itemId.Split(',')[1]);
//	                        string[] strItem = AnalysePlayerInventory(myTable, itemID, operationRequest.OperationCode, itemNum);
//	                        if (strItem[0] != "0")
//	                        {
//	                            myTable[0][string.Format("Inventory{0}", strItem[0])].YuanColumnText = strItem[1];
//	                            tempParms[string.Format("Inventory{0}", strItem[0])] = myTable[0][string.Format("Inventory{0}", strItem[0])].YuanColumnText;
//	                        }
//	                        else
//	                        {
//	                            //包裹没有足够的空位
//	                            return;
//	                        }
//	                    }
//	                    else
//	                        if (itemid2 == "72")
//	                        {
//	                            //坐骑id
//	                            //task.reward.itemId;	
//	                            myTable[0]["Mounts"].YuanColumnText = string.Format("{0}{1};", myTable[0]["Mounts"].YuanColumnText, task.reward.itemId);
//	                            tempParms["Mounts"] = myTable[0]["Mounts"].YuanColumnText;
//	                        }
//	                        else
//	                        {
//	                            //装备id
//	                            //	task.reward.itemId; 
//	                            SetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GetItem, task.reward.itemId);
//	                            string[] strItem = AnalysePlayerInventory(myTable, task.reward.itemId, operationRequest.OperationCode, 1);
//	                            if (strItem[0] != "0")
//	                            {
//	                                myTable[0][string.Format("Inventory{0}", strItem[0])].YuanColumnText = strItem[1];
//	                                tempParms[string.Format("Inventory{0}", strItem[0])] = myTable[0][string.Format("Inventory{0}", strItem[0])].YuanColumnText;
//	                            }
//	                            else
//	                            {
//	                                //包裹没有足够的空位
//	                                return;
//	                            }
//	                        }
//	                }
//	                //经验值
//	                //task.reward.exp
//	                AddExp(task.reward.exp, 1, (byte)OperationCode.AddExperience);
//
//	                //增加的声望值，字段"Prestige"
//	                //20
//	                myTable[0]["Prestige"].YuanColumnText = (int.Parse(myTable[0]["Prestige"].YuanColumnText) + 20).ToString();
//	                tempParms["Prestige"] = myTable[0]["Prestige"].YuanColumnText;
//
//	                //金币
//	                //task.reward.gold
//
//
//	                //血石
//	                //task.reward.rank
//	                UseMoney(task.reward.gold, task.reward.rank);
//	                tempParms["Money"] = myTable[0]["Money"].YuanColumnText;
//	                tempParms["Bloodstone"] = myTable[0]["Bloodstone"].YuanColumnText;
//
//
//	                //角色表,需要服务器取到 myTable
//	                myTable.Rows[0]["CompletTask"].YuanColumnText += taskID + ";";
//	                tempParms["CompletTask"] = myTable.Rows[0]["CompletTask"].YuanColumnText;
//	                string[] strArray;
//	                string[] taskStrs;
//	                int i;
//	                strArray = myTable.Rows[0]["Task"].YuanColumnText.Split(';');
//	                for (i = 0; i < strArray.Length; i++)
//	                {
//	                    taskStrs = strArray[i].Split(',');
//	                    if (taskStrs.Length > 1 && taskStrs[0] == taskID)
//	                    {
//	                        strArray[i] = "";
//	                    }
//	                }
//	                myTable.Rows[0]["Task"].YuanColumnText = "";
//	                for (i = 0; i < strArray.Length; i++)
//	                {
//	                    if (strArray[i] != "")
//	                        myTable.Rows[0]["Task"].YuanColumnText += strArray[i] + ";";
//	                }
//	                tempParms["Task"] = myTable.Rows[0]["Task"].YuanColumnText;
//
//	                string[] strKey = null;
//	                string[] strValue = null;
//	                Dictionary<byte, object> dicSend = new Dictionary<byte, object>();
//	                SetTable(tempParms, out strKey, out strValue, dicSend);
//	                dicSend[(byte)ParameterType.ItemID] = taskID;
//	                OperationResponse response = new OperationResponse(operationRequest.OperationCode,dicSend) { ReturnCode = (short)ReturnCode.Yes, DebugMessage = "" };
//	                SendOperationResponse(response, new SendParameters());
//	            }
//	            else
//	            {
//	                OperationResponse response = new OperationResponse(operationRequest.OperationCode) { ReturnCode = (short)ReturnCode.Nothing, DebugMessage = "" };
//	                SendOperationResponse(response, new SendParameters());
//	            }
//	        }
//	        else
//	        {
//	            DataNoSave();
//	        }
//	    }
//	    catch (Exception ex)
//	    {
//	        OperationResponse response = new OperationResponse(operationRequest.OperationCode) { ReturnCode = (short)ReturnCode.Error, DebugMessage = ex.ToString() };
//	        SendOperationResponse(response, new SendParameters());
//	        YuanLog(ex.ToString());
//	    }
//	}
//	
//	/// <summary>
//	/// 接受任务方法
//	/// </summary>
//	/// <param name="taskID">任务id.</param>
//	void TaskAcceptedAsID(string taskID)
//	{
//		if(myTable != null)
//		{
//			string[] strArray;
//			string[] taskStrs;
//			string[] placeStrs;
//			int i;
//			strArray = myTable.Rows[0]["Task"].YuanColumnText.Split(';');
//			for (i = 0; i < strArray.Length; i++)
//			{
//				taskStrs = strArray[i].Split(',');
//				if (taskStrs.Length > 1 && taskStrs[0] == taskID)
//				{
////						提示：已经接受过此任务，不可重复接受.
//					return;
//				}
//			}
//			
//			YuanPhotonServer.TaskWasDone.MainTask task = GetTaskAsID(taskID);
//			if(task != null)
//			{
//				myTable.Rows[0]["Task"].YuanColumnText += string.Format("{0},{1},{2};", taskID , "1" , task.doneNum.ToString());
//				placeStrs = myTable.Rows[0]["GetPlace"].YuanColumnText.Split(';');
//				bool isNewMap = true;
//				if(task.doneType.Length > 3){
//					for(i=0; i<placeStrs.length; i++){
//						if(task.doneType.Substring(4,3) == placeStrs[i]){
//							isNewMap = false;
//						}
//					}
//					if(isNewMap){
//						myTable.Rows[0]["GetPlace"].YuanColumnText += string.Format("{0},{1};", task.doneType.Substring(4,3) , task.doneType.Substring(13,1));
//					}
//				}
////				修改玩家信息，需返回myTable。
//			}
//			else
//			{
////					提示：无此任务id.
//				return;
//			}
//		}
//		else
//		{
////				提示：没有获取到玩家信息.
//			return;
//		}
//	}
//
//	/// <summary>
//	/// 达成任务所需条目
//	/// </summary>
//	/// <param name="taskID">任务id</param>
//	void TaskAddNumsAsID(string taskID){
//		if(myTable != null)
//		{
//			string[] strArray;
//			string[] taskStrs;
//			int needNum;
//			int i;
//			YuanPhotonServer.TaskWasDone.MainTask task = GetTaskAsID(taskID);
//			needNum = task.doneNum;
//			strArray = myTable.Rows[0]["Task"].YuanColumnText.Split(';');
//			for (i = 0; i < strArray.Length; i++)
//			{
//				taskStrs = strArray[i].Split(',');
//				if (taskStrs.Length > 1 && taskStrs[0] == taskID)
//				{
//					task.jindu = int.Parse(taskStrs[1]);
//					task.doneNum = int.Parse(taskStrs[2]);
//					if(task.jindu < 2){
//						task.doneNum += 1;
//						if(task.doneNum >= needNum){
//							task.jindu += 1;
////							任务“task.taskName”已完成。
//						}
//					}
//				}
//			}
//			myTable.Rows[0]["Task"].YuanColumnText = "";
//			for (i = 0; i < strArray.Length; i++)
//			{
//				taskStrs = strArray[i].Split(',');
//				if (strArray[i] != "" && taskID != taskStrs[0]){
//					myTable.Rows[0]["Task"].YuanColumnText += strArray[i] + ";";
//				}else{
//					myTable.Rows[0]["Task"].YuanColumnText += string.Format("{0},{1},{2};", taskID , task.jindu , task.doneNum);
//				}
//			}
////			修改玩家信息，需返回myTable。
//		}
//		else
//		{
////			提示：没有获取到玩家信息.
//			return;
//		}
//	}
//
//	/// <summary>
//	/// 放弃任务方法s I.
//	/// </summary>
//	/// <param name="taskID">任务id</param>
//	void TaskGiveUpAsID(string taskID)
//	{
//		if(myTable != null)
//		{
//			string[] strArray;
//			string[] taskStrs;
//			int i;
//			strArray = myTable.Rows[0]["Task"].YuanColumnText.Split(';');
//			for (i = 0; i < strArray.Length; i++)
//			{
//				taskStrs = strArray[i].Split(',');
//				if (taskStrs.Length > 1 && taskStrs[0] == taskID)
//				{
//					strArray[i] = "";
//				}
//			}
//			myTable.Rows[0]["Task"].YuanColumnText = "";
//			for (i = 0; i < strArray.Length; i++)
//			{
//				if (strArray[i] != "")
//					myTable.Rows[0]["Task"].YuanColumnText += strArray[i] + ";";
//			}
////			修改玩家信息，需返回myTable。
//		}
//		else
//		{
////			提示：没有获取到玩家信息.
//			return;
//		}
//	}
//
//	/// <summary>
//	/// 获得一个任务
//	/// </summary>
//	/// <returns>任务id</returns>
//	/// <param name="id">任务对象</param>
//	YuanPhotonServer.TaskWasDone.MainTask GetTaskAsID(string id)
//	{
//		YuanTable ytTask = null;
//		if (YuanServerApplication.PubTask.TryGetValue(id, out ytTask))
//		{
//			YuanPhotonServer.TaskWasDone.MainTask task = new YuanPhotonServer.TaskWasDone.MainTask();
//			task.taskName = ytTask[0]["TaskName"].YuanColumnText;
//			task.taskType = (YuanPhotonServer.TaskWasDone.MainTaskType)int.Parse(ytTask[0]["TaskType"].YuanColumnText);
//			task.id = ytTask[0]["id"].YuanColumnText;
//			task.needType = (YuanPhotonServer.TaskWasDone.MainTaskNeedType)int.Parse(ytTask[0]["id"].YuanColumnText);
//			task.needId = ytTask[0]["TaskNeedInfo"].YuanColumnText;
//			task.leixing = YuanPhotonServer.TaskWasDone.GetIntDBAsName("BranchType", 0, ytTask[0]);
//			task.jindu = 0;
//			task.RewardItemMaster = YuanPhotonServer.TaskWasDone.GetTaskReward(ytTask[0], task.RewardItemMaster, "RewardItemMaster");
//			task.RewardItemSoldier = YuanPhotonServer.TaskWasDone.GetTaskReward(ytTask[0], task.RewardItemSoldier, "RewardItemSoldier");
//			task.RewardItemRobber = YuanPhotonServer.TaskWasDone.GetTaskReward(ytTask[0], task.RewardItemRobber, "RewardItemRobber");
//			task.reward = YuanPhotonServer.TaskWasDone.GetTaskReward(ytTask[0], task.RewardItemRobber, "RewardItemMaster");
//			task.doneType = ytTask[0]["ConditonInfo"].YuanColumnText;
//			task.ExcludeTask = ytTask[0]["ExcludeTask"].YuanColumnText;
//			task.doneNum = ytTask[0]["ConditonNum"].YuanColumnText;
//			if (task.taskType == YuanPhotonServer.TaskWasDone.MainTaskType.items)
//			{
//				task.doneItem = task.doneType.Substring(15, 7);
//			}
//			return task;
//		}
//		return null;
//	}
//}
//
//#endregion