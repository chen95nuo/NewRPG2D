using UnityEngine;
using System.Collections;

public class RankJson : BasicJson {
	
	//获取的界面信息的类型，0 排位赛，1 天位赛， 2夺宝奇兵//
	public int type;
	
	public RankJson(int t){
		this.type = t;
	}
}
