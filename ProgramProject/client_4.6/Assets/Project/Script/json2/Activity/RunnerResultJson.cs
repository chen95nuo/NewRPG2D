using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunnerResultJson : ErrorJson {

	public int type;		//转动获得物品的类型//
	public string id;		//转动获得物品的id//
	public List<ShopElement> list;		//累计转动n次获得物品是否领取标识//
	public int state;		//1普通获得，2必得（客户端不用）//
	public int index;		//获得转动物品的索引//
	public List<ShopElement> view;	//显示剩余可转动次数//
}
