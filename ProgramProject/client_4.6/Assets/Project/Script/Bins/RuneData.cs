using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneData : PropertyReader {

	/**编号**/
	public int id{get;set;}
	/**属性**/
	public int proprety{get;set;}
	/**数值**/
	public int value{get;set;}
	/**消耗符文**/
	public int cost{get;set;}
	/**成功率**/
	public int successrate{get;set;}
	/**增长成功率**/
	public int upgrade{get;set;}

	private static Hashtable data=new Hashtable();
	//==分组存放,key为id高3位,value为List<RuneData>==//
	private static Hashtable groupData=new Hashtable();
	
	public void addData()
	{
		data.Add(id,this);
		
		int key=id/100;
		List<RuneData> list=(List<RuneData>)(groupData[key]);
		if(list==null)
		{
			list=new List<RuneData>();
			groupData.Add(key,list);
		}
		list.Add(this);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{}
	
	public static RuneData getData(int runeId)
	{
		return (RuneData)(data[runeId]);
	}
	
	public static RuneData getNextData(int times,int page,int num)
	{
		List<RuneData> list=(List<RuneData>)groupData[times*100+page];
		if(list.Count>num)
		{
			return list[num];
		}
		return null;
	}
	
	public static int getValues(int key,int num)
	{
		int result=0;
		List<RuneData> list=(List<RuneData>)groupData[key];
		for(int k=0;k<num;k++)
		{
			result+=list[k].value;
		}
		return result;
	}
	
	public static int getValues(int key)
	{
		int result=0;
		List<RuneData> list=(List<RuneData>)groupData[key];
		for (int i = 0; i < list.Count; i++)
		{
			result+=list[i].value;
		}
		return result;
	}
	
	public static int getGroupDataNum(int key)
	{
		return ((List<RuneData>)groupData[key]).Count;
	}
	
}
