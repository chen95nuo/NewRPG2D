using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImaginationClearResultJson : ErrorJson {

	/**
	 * 服务器->客户端
	 * 一键清空返回数据
	 */
	
	//获得的金币数量//
	public int gold;
	//背包的空余格子数量//
	public int i;
	//点击兑换时返回的碎片信息： packElement//
	public List<PackElement> s;
	//玩家当前金币//
	public int g;
}
