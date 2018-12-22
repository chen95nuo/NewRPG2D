using UnityEngine;
using System.Collections;

public class MazeClearResultJson : ErrorJson {
	/**
	 * 迷宫重置请求，服务器返回的数据
	 */
	
	//迷宫id-位置-次数-付费次数//
	public string maze;
	//玩家当前的钻石数//
	public int crystal;
}
