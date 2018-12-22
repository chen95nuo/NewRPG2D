using UnityEngine;
using System.Collections.Generic;

public class CardGetData : PropertyReader {

	/**编号**/
	public int id;
	/**名称**/
	public string name;
	/**1友情值,2水晶,3屌丝券**/
	public int type;
	/**活动标志:1活动**/
	public int activity;
	/**金额**/
	public int cost;
	/**抽取次数**/
	public int gettime;
	/**增加幸运值**/
	public int luckyadd;
	/**库消耗的幸运值,库**/
	public List<int> luckys;
	public List<int> cardHouses;
	
	private static List<CardGetData> data=new List<CardGetData>();
	
	public void addData()
	{
		data.Add(this);
	}

	public void parse(string[] ss)
	{
		int location=0;
		id=StringUtil.getInt(ss[location]);
		name=StringUtil.getString(ss[location+1]);
		type=StringUtil.getInt(ss[location+2]);
		activity=StringUtil.getInt(ss[location+3]);
		cost=StringUtil.getInt(ss[location+4]);
		gettime=StringUtil.getInt(ss[location+5]);
		luckyadd=StringUtil.getInt(ss[location+6]);
		int length=(ss.Length-7)/2;
		luckys=new List<int>();
		cardHouses=new List<int>();
		for(int i=0;i<length;i++)
		{
			location=7+i*2;
			luckys.Add(StringUtil.getInt(ss[location]));
			cardHouses.Add(StringUtil.getInt(ss[location+1]));
		}
		addData();
	}

	public void resetData()
	{
		data.Clear();
	}

	public static List<CardGetData> getDatas()
	{
		return data;
	}
	
	public static CardGetData getData(int id)
	{
		foreach(CardGetData c in data)
		{
			if(c.id==id)
			{
				return c;
			}
		}
		return null;
	}
	
	public string getCostName()
	{
		string result="";
		switch(type)
		{
		case 1:
			result=TextsData.getData(48).chinese;
			break;
		case 2:
			result=TextsData.getData(49).chinese;
			break;
		case 3:
			result=TextsData.getData(50).chinese;
			break;
		}
		return result;
	}
	
}