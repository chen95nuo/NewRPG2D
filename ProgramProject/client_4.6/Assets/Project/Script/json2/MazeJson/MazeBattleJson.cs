using UnityEngine;
using System.Collections;

public class MazeBattleJson : BasicJson {
	/**战斗类型pkModel:1 普通掉落,2 boss战**/
	public int t;
	/**迷宫编号**/
	public int td;
	/**迷宫中的位置编号**/
	public int state;
	/**pkModel:1 普通掉落,2 boss战,targetId:迷宫编号**/
	public MazeBattleJson(int pkModel,int targetId, int state)
	{
		this.t=pkModel;
		this.td=targetId;
		this.state = state;
	}
	
}
