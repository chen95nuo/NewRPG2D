using UnityEngine;
using System.Collections;

public class PkBattleLogResultJson : ErrorJson {
	//战斗结果1，胜利，2失败//
	public int r;
	//战斗胜利对象的名字//
	public string name;
	//玩家当前的排名//
	public int rank;
    //玩家PK前的排名//
    public int rank0;
	//pk后获得的符文值//
	public int runeNum;

    public int sAward;
	
	public int power0;     	//升级前的体力值//
	public int power1;		//升级后的体力值//
	
	public int award;		//每次pk奖励符文值//
	
	public int honor;		//每次奖励的荣誉值//
}
