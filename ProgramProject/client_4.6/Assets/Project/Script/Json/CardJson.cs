using UnityEngine;
using System.Collections.Generic;

public class CardJson
{
	public int se;//sequence//
	public int c;//cardid//
	public int lv;//level//
	public int s;//skillId//
	public int slv;//skillLevel//
	public List<string> ps; // passive skill list
	
	public int b;//==突破次数,等于5第二天赋有效==//
	public int t1;//天赋1//
	public int t2;//天赋2//
	public int t3;//天赋3//
	/**equipId-level**/
	public List<string> es;
	
	/**mission monster使用,固定的atk,def,maxhp**/
	public bool mm;//missionMonster//
	public int atk;
	public int def;
	public int hp;//maxHp//
	public int cri;
	public int avi;
	
	public int bm;//bossMark//
	
}
