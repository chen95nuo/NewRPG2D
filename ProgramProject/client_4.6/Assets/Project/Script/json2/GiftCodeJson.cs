using UnityEngine;
using System.Collections;

public class GiftCodeJson : BasicJson {
	
	//激活码//
	public string code{get;set;}
	
	public GiftCodeJson(string _code)
	{
		code = _code;
	}
}
