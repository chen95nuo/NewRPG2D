using UnityEngine;
using System.Collections;

public class ExchangeResultJson : ErrorJson {
	
	public int id{get;set;}
	public int curNeedNum{get;set;}
	public int sell{get;set;}		//是否可以兑换:0不能兑换，1可以兑换//
}
