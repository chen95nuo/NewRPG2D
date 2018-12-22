using UnityEngine;
using System.Collections;

public class PkBattleJson : BasicJson {
	
	//pk的对象在数据库中id//
	public int pkId;
	
	public PkBattleJson(int id){
		this.pkId = id;
	}
}
