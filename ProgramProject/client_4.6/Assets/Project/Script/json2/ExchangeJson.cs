using UnityEngine;
using System.Collections;

public class ExchangeJson : BasicJson {

	public int id{get;set;}
	
	public ExchangeJson(int id)
	{
		this.id = id;
	}
}
