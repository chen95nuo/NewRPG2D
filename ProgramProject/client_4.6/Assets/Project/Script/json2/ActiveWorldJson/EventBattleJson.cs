using UnityEngine;
using System.Collections;

public class EventBattleJson : BasicJson {

	//选择战斗的副本对应的关卡的id//
	public int id;
	//副本的id//
	public int eid;	
	
	public EventBattleJson(int eventId, int copyId){
		this.id = eventId;
		this.eid = copyId;
	}
}
