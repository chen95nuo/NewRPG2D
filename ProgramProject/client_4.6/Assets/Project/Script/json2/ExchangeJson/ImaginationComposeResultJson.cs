using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImaginationComposeResultJson : ErrorJson {
	
	/**
	 * 兑换物品
	 * 服务器->客户端
	 */

	//兑换的物品的id//
	public int id;
	//碎片//
	public List<PackElement> s;
}
