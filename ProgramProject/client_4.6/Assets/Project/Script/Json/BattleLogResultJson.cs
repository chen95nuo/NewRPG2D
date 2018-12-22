using UnityEngine;
using System.Collections.Generic;

public class BattleLogResultJson : ErrorJson {
	
	/**1胜利,2失败**/
	public int r;//result//
	
	/**战前**/
	//player
	public int lv0;//playerLevel0//
	public int ce0;//playerCurExp0//
	public int me0;//playerMaxExp0//
	
	//角色卡,格式:cardId-level-curExp-maxExp-hp-atk-def
	public List<string> cs0;
	/**战后**/
	//player
	public int lv1;//playerLevel1//
	public int ce1;//playerCurExp1//
	public int me1;//playerMaxExp1//
	
	//角色卡,格式:cardId-level-curExp-maxExp-hp-atk-def
	public List<string> cs1;
	//奖励
	public int ag;//addGold//
	/**格式:type-info**/
	public List<string> ds;//dropsInfo//
	/**金币倍数**/
	public string gm;//goldMul//
	//模块解锁  id-是否解锁（0未解锁， 1解锁）//
	public string[] s;
	
	public int hi;//支援玩家playerId//
	public string hn;//支援玩家名字//
	
	public int power0;     	//升级前的体力值//
	public int power1;		//升级后的体力值//
}
