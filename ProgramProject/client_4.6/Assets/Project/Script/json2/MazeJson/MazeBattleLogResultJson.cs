using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeBattleLogResultJson : ErrorJson {

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
	/**迷宫编号**/
	public int map;
	/**迷宫位置**/
	public int state;
	/**战斗类型**/
	public int type;
	
	public int power0;     	//升级前的体力值//
	public int power1;		//升级后的体力值//
}
