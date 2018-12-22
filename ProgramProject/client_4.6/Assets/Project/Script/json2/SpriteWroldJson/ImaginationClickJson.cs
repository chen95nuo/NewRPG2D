using UnityEngine;
using System.Collections;

public class ImaginationClickJson : BasicJson {

	/**
	 * 客户端->服务器
	 * 点击冥想按钮
	 */
	//当前npc的id //
	public int id;
	
	public ImaginationClickJson(int npcId){
		this.id = npcId;
	}
}
