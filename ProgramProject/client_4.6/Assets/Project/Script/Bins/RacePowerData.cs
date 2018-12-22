using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RacePowerData : PropertyReader {

	public int id;
	/**人数**/
	public int number;
	/**属性类型-数值**/
	public List<string> attris;

	private static Hashtable data=new Hashtable();
	
	public void addData()
	{
		data.Add(number, this);
	}

	public void parse(string[] ss)
	{
		id=StringUtil.getInt(ss[0]);
		number=StringUtil.getInt(ss[1]);
		attris=new List<string>();
		int length=(ss.Length-2)/2;
		for(int k=0;k<length;k++)
		{
			int location=k*2+2;
			int type=StringUtil.getInt(ss[location]);
			int num=StringUtil.getInt(ss[location+1]);
			if(type>0 && num>0)
			{
				attris.Add(type+"-"+num);
			}
		}
		addData();
	}

	public void resetData()
	{
	}

	public static RacePowerData getData(int number)
	{
		return (RacePowerData)data[number];
	}
}
