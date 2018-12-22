using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardKuData : PropertyReader
{
	//==编号==//	
	public int id{get;set;}
	//==卡库种类==//
	public int kind{get;set;}
	//==卡牌==//
	public int card{get;set;}
	//==概率==//
	public string probability{get;set;}
	//==首次跳转==//
	public int firstjump{get;set;}
	//==二次跳转==//
	public string secondjump{get;set;}
	
	private static Dictionary<int,CardKuData> data=new Dictionary<int, CardKuData>();
	
	private static List<int> cardIdList = new List<int>();
	
	public void addData()
	{
		data.Add(id,this);
	}
	public void resetData(){}
	public void parse(string[] ss){}
	
	public static CardKuData getData(int id)
	{
		return (CardKuData)data[id];
	}
	
	public static List<int> getCardIdList(int type)
	{
		cardIdList.Clear();
		foreach(KeyValuePair<int,CardKuData> kc in data)
		{
			if(kc.Value.kind == type)
			{
				cardIdList.Add(kc.Value.card);
			}
		}
		return cardIdList;
	}
}
