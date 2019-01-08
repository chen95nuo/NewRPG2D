using UnityEngine;
using System.Collections;

public class EquepmentBuild11 : MonoBehaviour {

	int RandomBuild;
	/// <summary>
	/// 判断是否可以强化该装备
	/// </summary>
	/// <param name="oldID">要强化的装备号</param>
	/// <param name="index">装备所在包裹编号</param>
	/// <param name="useBlood">是否使用血石头</param>
	/// <param name="yt">人物表</param>
	void EquepmentBuild(string oldID , int index , int useBlood , yuan.YuanMemoryDB.YuanTable yt){
		bool canBuild = false;
		bool useBloodBuild = false;

		if(useBlood == 0){
			canBuild = true;
		}else{
			if(int.Parse(yt.Rows[0]["Bloodstone"].YuanColumnText) >= useBlood){
				canBuild = true;
				useBloodBuild = true;
			}else{
				canBuild = false;
			}
		}
		if(canBuild){
			if(useBloodBuild){
//				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.UpdateEquipBlood).ToString());
				realBuild(oldID , index , yt);				
			}else{
				int useRandom = 0;
				useRandom = Random.Range(0,100); 
				RandomBuild = 90 - Mathf.Abs(30 - InRoom.GetInRoomInstantiate().serverTime.Minute); 
				if(useRandom <= RandomBuild){
					realBuild(oldID , index , yt);
				}else{
//					强化失败
				}
			}
		}else{
//			不可强化
		}
	}

	/// <summary>
	/// 强化装备
	/// </summary>
	/// <param name="oldID">要强化的装备号</param>
	/// <param name="index">装备所在包裹编号</param>
	/// <param name="yt">人物表</param>
	void realBuild(string oldID , int index , yuan.YuanMemoryDB.YuanTable yt){
		string ids = "";
		string[] useInvID;
		ids = yt.Rows[0]["EquipItem"].YuanColumnText + yt.Rows[0]["Inventory1"].YuanColumnText + yt.Rows[0]["Inventory2"].YuanColumnText + yt.Rows[0]["Inventory3"].YuanColumnText + yt.Rows[0]["Inventory4"].YuanColumnText;
		useInvID = ids.Split(';');
		for(int i=0; i<useInvID.Length; i++){
			if(useInvID[i] == oldID){
//				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.GameFunction , parseInt(yuan.YuanPhoton.GameFunction.UpdateEquip).ToString());
//				InRoom.GetInRoomInstantiate().SetSetPlayerBehavior(yuan.YuanPhoton.PlayerBehaviorType.Strengthen , invID);			
				int m = 6;
				int useInt;
				string useStr = "";
				int invitemQuality;
				invitemQuality =  int.Parse(oldID.Substring(4,1));
				if(invitemQuality >= 6){
					invitemQuality -= 4;
				}
				switch(invitemQuality){
					case 1: m = 0; break;
					case 2: m = 4; break;
					case 3: m = 8; break;
					case 4: m = 12; break;
					case 5: m = 12; break;
				}
				yt.Rows[0]["AimUpdateEquip"].YuanColumnText = (int.Parse(yt.Rows[0]["AimUpdateEquip"].YuanColumnText) + 1).ToString();		
				useInt = int.Parse(oldID.Substring(15,3)) + m;
				useStr = useInt.ToString();
				if(useStr.Length == 1){
					useStr = "00" + useStr;
				}else
				if(useStr.Length == 2){
					useStr = "0" + useStr;		
				}
				useInvID[index] = useInvID[index].Substring(0,15) + useStr + useInvID[index].Substring(18,7); 
				yt.Rows[0]["Inventory1"].YuanColumnText = "";
				yt.Rows[0]["Inventory2"].YuanColumnText = "";
				yt.Rows[0]["Inventory3"].YuanColumnText = "";
				yt.Rows[0]["Inventory4"].YuanColumnText = "";
				yt.Rows[0]["EquipItem"].YuanColumnText = "";
				break;
			}
		}
	}
}
