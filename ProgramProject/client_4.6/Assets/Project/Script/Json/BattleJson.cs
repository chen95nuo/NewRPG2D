using UnityEngine;
using System.Collections;

public class BattleJson:BasicJson
{
	/**targetId:关卡Id,pvpId**/
	public int td;
	/**helpPlayerId:援护玩家**/
	public int h;
	
	public BattleJson(int targetId,int helperPlayerId)
	{
		this.td=targetId;
		this.h=helperPlayerId;
	}
}
