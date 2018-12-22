using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class  RankResultJson : ErrorJson {
	
	//玩家信息： 排名-排名奖励领取表示 （0，未领取， 1，已领取）-pvp奖励-挑战次数-排名奖励//
	public string s;
	
	//pk对象列表//
	//格式：playerId-name-headName-rank(排名)-战力//
	public List<string> ss;
	
	//pvp总奖励//
	public int sAward;	
	//挑战总次数//
	public int sPknum;
	//pk剩余时间--秒//
	public int cdtime;

    public List<string> cardIds;  //顺序对应ss中的玩家顺序 cardId - id - id - id - id - id;
	
	
}
