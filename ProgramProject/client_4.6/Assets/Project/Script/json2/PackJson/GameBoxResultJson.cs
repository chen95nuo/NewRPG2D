using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBoxResultJson : ErrorJson {
	//获得的物品，如果有固定物品则list中有两条数据，如果没有则list中就一条数据//
	public List<GameBoxElement> ges;
}
