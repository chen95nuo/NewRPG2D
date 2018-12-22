using UnityEngine;
using System.Collections;

public class PayOrderJson : BasicJson {
	public string consumValue;
	public string extra;
	
	public PayOrderJson(string consumValue,string extra)
	{
		this.consumValue=consumValue;
		this.extra=extra;
	}
}
