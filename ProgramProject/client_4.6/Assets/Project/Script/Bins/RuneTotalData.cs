using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneTotalData : PropertyReader {
	
	/**编号**/
	public int id{get;set;}
	/**名称**/
	public string name{get;set;}
	/**属性**/
	public int proprety{get;set;}
	/**数值**/
	public int value{get;set;}
	
	public int level{get;set;}
	
	private static Hashtable data=new Hashtable();
	private static List<int> ids=new List<int>();
	
	public void addData()
	{
		data.Add(id,this);
		ids.Add(id);
	}
	public void resetData()
	{
		data.Clear();
	}
	public void parse(string[] ss)
	{}
	
	public static RuneTotalData getData(int id)
	{
		return (RuneTotalData)(data[id]);
	}
	
	/**
	 * 获取本轮的同proprety的value总和
	 * lt@2014-2-21 上午11:01:20
	 * @param id
	 * @param proprety
	 * @return
	 */
	public static int getValues(int times,int property)
	{
		int result=0;
		for(int i=0;i<ids.Count;i++)
		{
			int idTemp=ids[i];
			if(idTemp/100<=times)
			{
				RuneTotalData rtd=RuneTotalData.getData(idTemp);
				if(rtd.proprety==property)
				{
					result+=rtd.value;
					result+=RuneData.getValues(idTemp);
				}
			}
		}
		return result;
	}
}
