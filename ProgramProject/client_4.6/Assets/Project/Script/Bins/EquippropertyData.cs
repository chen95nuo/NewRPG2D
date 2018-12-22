using UnityEngine;
using System.Collections;

public class EquippropertyData :PropertyReader {
	
	
	public int type;
	public int level;
	public int[] starNumbers;
	
	private static ArrayList data=new ArrayList();
	
	public void addData()
	{
		data.Add(this);
	}
	
	public void resetData()
	{
		data.Clear();
	}
	
	public void parse(string[] ss)
	{
		type=StringUtil.getInt(ss[0]);
		level=StringUtil.getInt(ss[1]);
		int length=ss.Length-2;
		starNumbers=new int[length];
		for(int i=0;i<length;i++)
		{
			starNumbers[i]=StringUtil.getInt(ss[2+i]);
		}
		addData();
	}
	
	public static EquippropertyData getData(int type,int level)
	{
		foreach(EquippropertyData ed in data)
		{
			if(ed.type==type && ed.level==level)
			{
				return ed;
			}
		}
		return null;
	}
	
	public static int getValue(int type,int level,int star)
	{
		foreach(EquippropertyData ed in data)
		{
			if(ed.type==type && ed.level==level)
			{
				return ed.starNumbers[star-1];
			}
		}
		return 0;
	}
}
