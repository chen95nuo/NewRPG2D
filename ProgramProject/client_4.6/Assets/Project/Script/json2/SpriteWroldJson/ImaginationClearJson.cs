using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImaginationClearJson : BasicJson {

	/**
	 * 客户端->服务器
	 * 一键清理
	 */
	
	//需要清理的物品的列表: 类型-物品id-数量 //
	public List<string> s;
	
	//请求的类型：1 一键清理和返回， 2 兑换//
	public int t;
	
	public ImaginationClearJson(List<string> str, int type){
		this.s = str;
		this.t = type;
	}
}
